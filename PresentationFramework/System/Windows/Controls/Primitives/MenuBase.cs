using System;
using System.Collections.Specialized;
using System.Security;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Win32;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that defines choices for users to select. </summary>
	// Token: 0x02000598 RID: 1432
	[Localizability(LocalizationCategory.Menu)]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(MenuItem))]
	public abstract class MenuBase : ItemsControl
	{
		// Token: 0x06005E69 RID: 24169 RVA: 0x001A74E0 File Offset: 0x001A56E0
		static MenuBase()
		{
			EventManager.RegisterClassHandler(typeof(MenuBase), MenuItem.PreviewClickEvent, new RoutedEventHandler(MenuBase.OnMenuItemPreviewClick));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseDownEvent, new MouseButtonEventHandler(MenuBase.OnMouseButtonDown));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseUpEvent, new MouseButtonEventHandler(MenuBase.OnMouseButtonUp));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.LostMouseCaptureEvent, new MouseEventHandler(MenuBase.OnLostMouseCapture));
			EventManager.RegisterClassHandler(typeof(MenuBase), MenuBase.IsSelectedChangedEvent, new RoutedPropertyChangedEventHandler<bool>(MenuBase.OnIsSelectedChanged));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseDownEvent, new MouseButtonEventHandler(MenuBase.OnPromotedMouseButton));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.MouseUpEvent, new MouseButtonEventHandler(MenuBase.OnPromotedMouseButton));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.PreviewMouseDownOutsideCapturedElementEvent, new MouseButtonEventHandler(MenuBase.OnClickThroughThunk));
			EventManager.RegisterClassHandler(typeof(MenuBase), Mouse.PreviewMouseUpOutsideCapturedElementEvent, new MouseButtonEventHandler(MenuBase.OnClickThroughThunk));
			EventManager.RegisterClassHandler(typeof(MenuBase), Keyboard.PreviewKeyboardInputProviderAcquireFocusEvent, new KeyboardInputProviderAcquireFocusEventHandler(MenuBase.OnPreviewKeyboardInputProviderAcquireFocus), true);
			EventManager.RegisterClassHandler(typeof(MenuBase), Keyboard.KeyboardInputProviderAcquireFocusEvent, new KeyboardInputProviderAcquireFocusEventHandler(MenuBase.OnKeyboardInputProviderAcquireFocus), true);
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(MenuBase), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			InputMethod.IsInputMethodSuspendedProperty.OverrideMetadata(typeof(MenuBase), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, FrameworkPropertyMetadataOptions.Inherits));
		}

		/// <summary>Gets or sets the custom logic for choosing a template used to display each item. </summary>
		/// <returns>A custom object that provides logic and returns an item container. </returns>
		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x06005E6A RID: 24170 RVA: 0x001A770B File Offset: 0x001A590B
		// (set) Token: 0x06005E6B RID: 24171 RVA: 0x001A771D File Offset: 0x001A591D
		public ItemContainerTemplateSelector ItemContainerTemplateSelector
		{
			get
			{
				return (ItemContainerTemplateSelector)base.GetValue(MenuBase.ItemContainerTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(MenuBase.ItemContainerTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the menu selects different item containers, depending on the type of the item in the underlying collection or some other heuristic.</summary>
		/// <returns>
		///     <see langword="true" /> the menu selects different item containers; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x06005E6C RID: 24172 RVA: 0x001A772B File Offset: 0x001A592B
		// (set) Token: 0x06005E6D RID: 24173 RVA: 0x001A773D File Offset: 0x001A593D
		public bool UsesItemContainerTemplate
		{
			get
			{
				return (bool)base.GetValue(MenuBase.UsesItemContainerTemplateProperty);
			}
			set
			{
				base.SetValue(MenuBase.UsesItemContainerTemplateProperty, value);
			}
		}

		// Token: 0x06005E6E RID: 24174 RVA: 0x001A774B File Offset: 0x001A594B
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			((MenuBase)sender).HandleMouseButton(e);
		}

		// Token: 0x06005E6F RID: 24175 RVA: 0x001A774B File Offset: 0x001A594B
		private static void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
		{
			((MenuBase)sender).HandleMouseButton(e);
		}

		/// <summary>Called when a mouse button is pressed or released. </summary>
		/// <param name="e">The event data for a mouse event.</param>
		// Token: 0x06005E70 RID: 24176 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void HandleMouseButton(MouseButtonEventArgs e)
		{
		}

		// Token: 0x06005E71 RID: 24177 RVA: 0x001A7759 File Offset: 0x001A5959
		private static void OnClickThroughThunk(object sender, MouseButtonEventArgs e)
		{
			((MenuBase)sender).OnClickThrough(e);
		}

		// Token: 0x06005E72 RID: 24178 RVA: 0x001A7768 File Offset: 0x001A5968
		private void OnClickThrough(MouseButtonEventArgs e)
		{
			if ((e.ChangedButton == MouseButton.Left || e.ChangedButton == MouseButton.Right) && this.HasCapture)
			{
				bool flag = true;
				if (e.ButtonState == MouseButtonState.Released)
				{
					if (e.ChangedButton == MouseButton.Left && this.IgnoreNextLeftRelease)
					{
						this.IgnoreNextLeftRelease = false;
						flag = false;
					}
					else if (e.ChangedButton == MouseButton.Right && this.IgnoreNextRightRelease)
					{
						this.IgnoreNextRightRelease = false;
						flag = false;
					}
				}
				if (flag)
				{
					this.IsMenuMode = false;
				}
			}
		}

		// Token: 0x06005E73 RID: 24179 RVA: 0x001A77D7 File Offset: 0x001A59D7
		private static void OnPromotedMouseButton(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				e.Handled = true;
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseLeave" /> routed event that occurs when the mouse leaves the control.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.MouseLeave" /> event.</param>
		// Token: 0x06005E74 RID: 24180 RVA: 0x001A77E8 File Offset: 0x001A59E8
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (!this.HasCapture && !base.IsMouseOver && this.CurrentSelection != null && !this.CurrentSelection.IsKeyboardFocused && !this.CurrentSelection.IsSubmenuOpen)
			{
				this.CurrentSelection = null;
			}
		}

		// Token: 0x06005E75 RID: 24181 RVA: 0x001A7838 File Offset: 0x001A5A38
		private static void OnPreviewKeyboardInputProviderAcquireFocus(object sender, KeyboardInputProviderAcquireFocusEventArgs e)
		{
			MenuBase menuBase = (MenuBase)sender;
			if (!menuBase.IsKeyboardFocusWithin && !menuBase.HasPushedMenuMode)
			{
				menuBase.PushMenuMode(true);
			}
		}

		// Token: 0x06005E76 RID: 24182 RVA: 0x001A7864 File Offset: 0x001A5A64
		private static void OnKeyboardInputProviderAcquireFocus(object sender, KeyboardInputProviderAcquireFocusEventArgs e)
		{
			MenuBase menuBase = (MenuBase)sender;
			if (!menuBase.IsKeyboardFocusWithin && !e.FocusAcquired && menuBase.IsAcquireFocusMenuMode)
			{
				menuBase.PopMenuMode();
			}
		}

		/// <summary>Responds to a change to the <see cref="P:System.Windows.UIElement.IsKeyboardFocusWithin" /> property. </summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.IsKeyboardFocusWithinChanged" /> event.</param>
		// Token: 0x06005E77 RID: 24183 RVA: 0x001A7898 File Offset: 0x001A5A98
		protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsKeyboardFocusWithinChanged(e);
			if (base.IsKeyboardFocusWithin)
			{
				if (!this.IsMenuMode)
				{
					this.IsMenuMode = true;
					this.OpenOnMouseEnter = false;
				}
				if (KeyboardNavigation.IsKeyboardMostRecentInputDevice())
				{
					KeyboardNavigation.EnableKeyboardCues(this, true);
				}
			}
			else
			{
				KeyboardNavigation.EnableKeyboardCues(this, false);
				if (this.IsMenuMode)
				{
					if (this.HasCapture)
					{
						this.IsMenuMode = false;
					}
				}
				else if (this.CurrentSelection != null)
				{
					this.CurrentSelection = null;
				}
			}
			this.InvokeMenuOpenedClosedAutomationEvent(base.IsKeyboardFocusWithin);
		}

		// Token: 0x06005E78 RID: 24184 RVA: 0x001A7918 File Offset: 0x001A5B18
		private void InvokeMenuOpenedClosedAutomationEvent(bool open)
		{
			AutomationEvents automationEvent = open ? AutomationEvents.MenuOpened : AutomationEvents.MenuClosed;
			if (AutomationPeer.ListenerExists(automationEvent))
			{
				AutomationPeer peer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (peer != null)
				{
					if (open)
					{
						base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
						{
							peer.RaiseAutomationEvent(automationEvent);
							return null;
						}), null);
						return;
					}
					peer.RaiseAutomationEvent(automationEvent);
				}
			}
		}

		// Token: 0x06005E79 RID: 24185 RVA: 0x001A799C File Offset: 0x001A5B9C
		private static void OnIsSelectedChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
		{
			MenuItem menuItem = e.OriginalSource as MenuItem;
			if (menuItem != null)
			{
				MenuBase menuBase = (MenuBase)sender;
				if (e.NewValue)
				{
					if (menuBase.CurrentSelection != menuItem && menuItem.LogicalParent == menuBase)
					{
						bool flag = false;
						if (menuBase.CurrentSelection != null)
						{
							flag = menuBase.CurrentSelection.IsSubmenuOpen;
							menuBase.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
						}
						menuBase.CurrentSelection = menuItem;
						if (menuBase.CurrentSelection != null && flag)
						{
							MenuItemRole role = menuBase.CurrentSelection.Role;
							if ((role == MenuItemRole.SubmenuHeader || role == MenuItemRole.TopLevelHeader) && menuBase.CurrentSelection.IsSubmenuOpen != flag)
							{
								menuBase.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.Box(flag));
							}
						}
					}
				}
				else if (menuBase.CurrentSelection == menuItem)
				{
					menuBase.CurrentSelection = null;
				}
				e.Handled = true;
			}
		}

		// Token: 0x06005E7A RID: 24186 RVA: 0x001A7A75 File Offset: 0x001A5C75
		private bool IsDescendant(DependencyObject node)
		{
			return MenuBase.IsDescendant(this, node);
		}

		// Token: 0x06005E7B RID: 24187 RVA: 0x001A7A80 File Offset: 0x001A5C80
		internal static bool IsDescendant(DependencyObject reference, DependencyObject node)
		{
			bool result = false;
			DependencyObject dependencyObject = node;
			while (dependencyObject != null)
			{
				if (dependencyObject == reference)
				{
					result = true;
					break;
				}
				PopupRoot popupRoot = dependencyObject as PopupRoot;
				if (popupRoot != null)
				{
					Popup popup = popupRoot.Parent as Popup;
					dependencyObject = popup;
					if (popup != null)
					{
						dependencyObject = popup.Parent;
						if (dependencyObject == null)
						{
							dependencyObject = popup.PlacementTarget;
						}
					}
				}
				else
				{
					dependencyObject = PopupControlService.FindParent(dependencyObject);
				}
			}
			return result;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event that occurs when the user presses a key.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.UIElement.KeyDown" /> event.</param>
		// Token: 0x06005E7C RID: 24188 RVA: 0x001A7AD4 File Offset: 0x001A5CD4
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			Key key = e.Key;
			if (key != Key.Escape)
			{
				if (key != Key.System)
				{
					return;
				}
				if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt || e.SystemKey == Key.F10)
				{
					this.KeyboardLeaveMenuMode();
					e.Handled = true;
				}
				return;
			}
			else
			{
				if (this.CurrentSelection != null && this.CurrentSelection.IsSubmenuOpen)
				{
					this.CurrentSelection.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
					this.OpenOnMouseEnter = false;
					e.Handled = true;
					return;
				}
				this.KeyboardLeaveMenuMode();
				e.Handled = true;
				return;
			}
		}

		/// <summary>Determines whether the specified item is, or is eligible to be, its own item container. </summary>
		/// <param name="item">The item to check whether it is an item container.</param>
		/// <returns>
		///     <see langword="true" /> if the item is a <see cref="T:System.Windows.Controls.MenuItem" /> or a <see cref="T:System.Windows.Controls.Separator" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005E7D RID: 24189 RVA: 0x001A7B70 File Offset: 0x001A5D70
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			bool flag = item is MenuItem || item is Separator;
			if (!flag)
			{
				this._currentItem = item;
			}
			return flag;
		}

		/// <summary>Creates or identifies the element used to display the specified item.</summary>
		/// <returns>The element used to display the specified item.</returns>
		// Token: 0x06005E7E RID: 24190 RVA: 0x001A7BA0 File Offset: 0x001A5DA0
		protected override DependencyObject GetContainerForItemOverride()
		{
			object currentItem = this._currentItem;
			this._currentItem = null;
			if (this.UsesItemContainerTemplate)
			{
				DataTemplate dataTemplate = this.ItemContainerTemplateSelector.SelectTemplate(currentItem, this);
				if (dataTemplate != null)
				{
					object obj = dataTemplate.LoadContent();
					if (obj is MenuItem || obj is Separator)
					{
						return obj as DependencyObject;
					}
					throw new InvalidOperationException(SR.Get("InvalidItemContainer", new object[]
					{
						base.GetType().Name,
						typeof(MenuItem).Name,
						typeof(Separator).Name,
						obj
					}));
				}
			}
			return new MenuItem();
		}

		// Token: 0x06005E7F RID: 24191 RVA: 0x001A7C44 File Offset: 0x001A5E44
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			MenuBase menuBase = sender as MenuBase;
			if (Mouse.Captured != menuBase)
			{
				if (e.OriginalSource == menuBase)
				{
					if (Mouse.Captured == null || !MenuBase.IsDescendant(menuBase, Mouse.Captured as DependencyObject))
					{
						menuBase.IsMenuMode = false;
						return;
					}
				}
				else if (MenuBase.IsDescendant(menuBase, e.OriginalSource as DependencyObject))
				{
					if (menuBase.IsMenuMode && Mouse.Captured == null && SafeNativeMethods.GetCapture() == IntPtr.Zero)
					{
						Mouse.Capture(menuBase, CaptureMode.SubTree);
						e.Handled = true;
						return;
					}
				}
				else
				{
					menuBase.IsMenuMode = false;
				}
			}
		}

		// Token: 0x06005E80 RID: 24192 RVA: 0x001A7CD4 File Offset: 0x001A5ED4
		private static void OnMenuItemPreviewClick(object sender, RoutedEventArgs e)
		{
			MenuBase menuBase = (MenuBase)sender;
			MenuItem menuItem = e.OriginalSource as MenuItem;
			if (menuItem != null && !menuItem.StaysOpenOnClick)
			{
				MenuItemRole role = menuItem.Role;
				if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem)
				{
					menuBase.IsMenuMode = false;
					e.Handled = true;
				}
			}
		}

		// Token: 0x14000116 RID: 278
		// (add) Token: 0x06005E81 RID: 24193 RVA: 0x001A7D1B File Offset: 0x001A5F1B
		// (remove) Token: 0x06005E82 RID: 24194 RVA: 0x001A7D29 File Offset: 0x001A5F29
		internal event EventHandler InternalMenuModeChanged
		{
			add
			{
				base.EventHandlersStoreAdd(MenuBase.InternalMenuModeChangedKey, value);
			}
			remove
			{
				base.EventHandlersStoreRemove(MenuBase.InternalMenuModeChangedKey, value);
			}
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x001A7D38 File Offset: 0x001A5F38
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void RestorePreviousFocus()
		{
			if (base.IsKeyboardFocusWithin)
			{
				IntPtr focus = UnsafeNativeMethods.GetFocus();
				HwndSource hwndSource = (focus != IntPtr.Zero) ? HwndSource.CriticalFromHwnd(focus) : null;
				if (hwndSource != null)
				{
					Keyboard.Focus(null);
					return;
				}
				Keyboard.ClearFocus();
			}
		}

		// Token: 0x06005E84 RID: 24196 RVA: 0x001A7D7C File Offset: 0x001A5F7C
		internal static void SetSuspendingPopupAnimation(ItemsControl menu, MenuItem ignore, bool suspend)
		{
			if (menu != null)
			{
				int count = menu.Items.Count;
				for (int i = 0; i < count; i++)
				{
					MenuItem menuItem = menu.ItemContainerGenerator.ContainerFromIndex(i) as MenuItem;
					if (menuItem != null && menuItem != ignore && menuItem.IsSuspendingPopupAnimation != suspend)
					{
						menuItem.IsSuspendingPopupAnimation = suspend;
						if (!suspend)
						{
							MenuBase.SetSuspendingPopupAnimation(menuItem, null, suspend);
						}
					}
				}
			}
		}

		// Token: 0x06005E85 RID: 24197 RVA: 0x001A7DD8 File Offset: 0x001A5FD8
		internal void KeyboardLeaveMenuMode()
		{
			if (this.IsMenuMode)
			{
				this.IsMenuMode = false;
				return;
			}
			this.CurrentSelection = null;
			this.RestorePreviousFocus();
		}

		// Token: 0x170016D1 RID: 5841
		// (get) Token: 0x06005E86 RID: 24198 RVA: 0x001A7DF7 File Offset: 0x001A5FF7
		// (set) Token: 0x06005E87 RID: 24199 RVA: 0x001A7E00 File Offset: 0x001A6000
		internal MenuItem CurrentSelection
		{
			get
			{
				return this._currentSelection;
			}
			set
			{
				bool flag = false;
				if (this._currentSelection != null)
				{
					flag = this._currentSelection.IsKeyboardFocused;
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.FalseBox);
				}
				this._currentSelection = value;
				if (this._currentSelection != null)
				{
					this._currentSelection.SetCurrentValueInternal(MenuItem.IsSelectedProperty, BooleanBoxes.TrueBox);
					if (flag)
					{
						this._currentSelection.Focus();
					}
				}
			}
		}

		// Token: 0x170016D2 RID: 5842
		// (get) Token: 0x06005E88 RID: 24200 RVA: 0x0013568C File Offset: 0x0013388C
		internal bool HasCapture
		{
			get
			{
				return Mouse.Captured == this;
			}
		}

		// Token: 0x170016D3 RID: 5843
		// (get) Token: 0x06005E89 RID: 24201 RVA: 0x001A7E6B File Offset: 0x001A606B
		// (set) Token: 0x06005E8A RID: 24202 RVA: 0x001A7E79 File Offset: 0x001A6079
		internal bool IgnoreNextLeftRelease
		{
			get
			{
				return this._bitFlags[1];
			}
			set
			{
				this._bitFlags[1] = value;
			}
		}

		// Token: 0x170016D4 RID: 5844
		// (get) Token: 0x06005E8B RID: 24203 RVA: 0x001A7E88 File Offset: 0x001A6088
		// (set) Token: 0x06005E8C RID: 24204 RVA: 0x001A7E96 File Offset: 0x001A6096
		internal bool IgnoreNextRightRelease
		{
			get
			{
				return this._bitFlags[2];
			}
			set
			{
				this._bitFlags[2] = value;
			}
		}

		// Token: 0x170016D5 RID: 5845
		// (get) Token: 0x06005E8D RID: 24205 RVA: 0x001A7EA5 File Offset: 0x001A60A5
		// (set) Token: 0x06005E8E RID: 24206 RVA: 0x001A7EB4 File Offset: 0x001A60B4
		internal bool IsMenuMode
		{
			get
			{
				return this._bitFlags[4];
			}
			set
			{
				bool flag = this._bitFlags[4];
				if (flag != value)
				{
					this._bitFlags[4] = value;
					flag = value;
					if (flag)
					{
						if (!MenuBase.IsDescendant(this, Mouse.Captured as Visual) && !Mouse.Capture(this, CaptureMode.SubTree))
						{
							flag = (this._bitFlags[4] = false);
						}
						else
						{
							if (!this.HasPushedMenuMode)
							{
								this.PushMenuMode(false);
							}
							base.RaiseClrEvent(MenuBase.InternalMenuModeChangedKey, EventArgs.Empty);
						}
					}
					if (!flag)
					{
						if (this.CurrentSelection != null)
						{
							bool isSubmenuOpen = this.CurrentSelection.IsSubmenuOpen;
							this.CurrentSelection.IsSubmenuOpen = false;
							this.CurrentSelection = null;
						}
						if (this.HasPushedMenuMode)
						{
							this.PopMenuMode();
						}
						if (!value)
						{
							base.RaiseClrEvent(MenuBase.InternalMenuModeChangedKey, EventArgs.Empty);
						}
						MenuBase.SetSuspendingPopupAnimation(this, null, false);
						if (this.HasCapture)
						{
							Mouse.Capture(null);
						}
						this.RestorePreviousFocus();
					}
					this.OpenOnMouseEnter = flag;
				}
			}
		}

		// Token: 0x170016D6 RID: 5846
		// (get) Token: 0x06005E8F RID: 24207 RVA: 0x001A7FA7 File Offset: 0x001A61A7
		// (set) Token: 0x06005E90 RID: 24208 RVA: 0x001A7FB5 File Offset: 0x001A61B5
		internal bool OpenOnMouseEnter
		{
			get
			{
				return this._bitFlags[8];
			}
			set
			{
				this._bitFlags[8] = value;
			}
		}

		// Token: 0x06005E91 RID: 24209 RVA: 0x001A7FC4 File Offset: 0x001A61C4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void PushMenuMode(bool isAcquireFocusMenuMode)
		{
			this._pushedMenuMode = PresentationSource.CriticalFromVisual(this);
			this.IsAcquireFocusMenuMode = isAcquireFocusMenuMode;
			InputManager.UnsecureCurrent.PushMenuMode(this._pushedMenuMode);
		}

		// Token: 0x06005E92 RID: 24210 RVA: 0x001A7FEC File Offset: 0x001A61EC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void PopMenuMode()
		{
			PresentationSource pushedMenuMode = this._pushedMenuMode;
			this._pushedMenuMode = null;
			this.IsAcquireFocusMenuMode = false;
			InputManager.UnsecureCurrent.PopMenuMode(pushedMenuMode);
		}

		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x06005E93 RID: 24211 RVA: 0x001A8019 File Offset: 0x001A6219
		private bool HasPushedMenuMode
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return this._pushedMenuMode != null;
			}
		}

		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x06005E94 RID: 24212 RVA: 0x001A8024 File Offset: 0x001A6224
		// (set) Token: 0x06005E95 RID: 24213 RVA: 0x001A8033 File Offset: 0x001A6233
		private bool IsAcquireFocusMenuMode
		{
			get
			{
				return this._bitFlags[16];
			}
			set
			{
				this._bitFlags[16] = value;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.MenuBase.ItemContainerTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.MenuBase.ItemContainerTemplateSelector" /> dependency property.</returns>
		// Token: 0x04003055 RID: 12373
		public static readonly DependencyProperty ItemContainerTemplateSelectorProperty = DependencyProperty.Register("ItemContainerTemplateSelector", typeof(ItemContainerTemplateSelector), typeof(MenuBase), new FrameworkPropertyMetadata(new DefaultItemContainerTemplateSelector()));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.MenuBase.UsesItemContainerTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.MenuBase.UsesItemContainerTemplate" /> dependency property.</returns>
		// Token: 0x04003056 RID: 12374
		public static readonly DependencyProperty UsesItemContainerTemplateProperty = DependencyProperty.Register("UsesItemContainerTemplate", typeof(bool), typeof(MenuBase));

		// Token: 0x04003057 RID: 12375
		internal static readonly RoutedEvent IsSelectedChangedEvent = EventManager.RegisterRoutedEvent("IsSelectedChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<bool>), typeof(MenuBase));

		// Token: 0x04003058 RID: 12376
		private object _currentItem;

		// Token: 0x04003059 RID: 12377
		private static readonly EventPrivateKey InternalMenuModeChangedKey = new EventPrivateKey();

		// Token: 0x0400305A RID: 12378
		[SecurityCritical]
		private PresentationSource _pushedMenuMode;

		// Token: 0x0400305B RID: 12379
		private MenuItem _currentSelection;

		// Token: 0x0400305C RID: 12380
		private BitVector32 _bitFlags = new BitVector32(0);

		// Token: 0x020009E6 RID: 2534
		private enum MenuBaseFlags
		{
			// Token: 0x04004662 RID: 18018
			IgnoreNextLeftRelease = 1,
			// Token: 0x04004663 RID: 18019
			IgnoreNextRightRelease,
			// Token: 0x04004664 RID: 18020
			IsMenuMode = 4,
			// Token: 0x04004665 RID: 18021
			OpenOnMouseEnter = 8,
			// Token: 0x04004666 RID: 18022
			IsAcquireFocusMenuMode = 16
		}
	}
}
