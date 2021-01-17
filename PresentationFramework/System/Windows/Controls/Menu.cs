using System;
using System.Security;
using System.Security.Permissions;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary> Represents a Windows menu control that enables you to hierarchically organize elements associated with commands and event handlers. </summary>
	// Token: 0x02000501 RID: 1281
	public class Menu : MenuBase
	{
		// Token: 0x060051BF RID: 20927 RVA: 0x0016DDAC File Offset: 0x0016BFAC
		static Menu()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(typeof(Menu)));
			Menu._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Menu));
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(Menu.GetDefaultPanel()));
			Control.IsTabStopProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(false));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(Menu), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			EventManager.RegisterClassHandler(typeof(Menu), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(Menu.OnAccessKeyPressed));
			ControlsTraceLogger.AddControl(TelemetryControls.Menu);
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x0016DED0 File Offset: 0x0016C0D0
		private static ItemsPanelTemplate GetDefaultPanel()
		{
			FrameworkElementFactory root = new FrameworkElementFactory(typeof(WrapPanel));
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(root);
			itemsPanelTemplate.Seal();
			return itemsPanelTemplate;
		}

		/// <summary> Gets or sets a value that indicates whether this <see cref="T:System.Windows.Controls.Menu" /> receives a main menu activation notification.  </summary>
		/// <returns>
		///     <see langword="true" /> if the menu receives a main menu activation notification; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x060051C1 RID: 20929 RVA: 0x0016DEFB File Offset: 0x0016C0FB
		// (set) Token: 0x060051C2 RID: 20930 RVA: 0x0016DF0D File Offset: 0x0016C10D
		public bool IsMainMenu
		{
			get
			{
				return (bool)base.GetValue(Menu.IsMainMenuProperty);
			}
			set
			{
				base.SetValue(Menu.IsMainMenuProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x0016DF20 File Offset: 0x0016C120
		private static void OnIsMainMenuChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Menu menu = d as Menu;
			if ((bool)e.NewValue)
			{
				menu.SetupMainMenu();
				return;
			}
			menu.CleanupMainMenu();
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.MenuAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060051C4 RID: 20932 RVA: 0x0016DF4F File Offset: 0x0016C14F
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new MenuAutomationPeer(this);
		}

		/// <summary> Called when the <see cref="P:System.Windows.FrameworkElement.IsInitialized" /> property is set to <see langword="true" />. </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x060051C5 RID: 20933 RVA: 0x0016DF57 File Offset: 0x0016C157
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			if (this.IsMainMenu)
			{
				this.SetupMainMenu();
			}
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x0016DF70 File Offset: 0x0016C170
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void SetupMainMenu()
		{
			if (this._enterMenuModeHandler == null)
			{
				this._enterMenuModeHandler = new KeyboardNavigation.EnterMenuModeEventHandler(this.OnEnterMenuMode);
				new UIPermission(UIPermissionWindow.AllWindows).Assert();
				try
				{
					KeyboardNavigation.Current.EnterMenuMode += this._enterMenuModeHandler;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x0016DFCC File Offset: 0x0016C1CC
		private void CleanupMainMenu()
		{
			if (this._enterMenuModeHandler != null)
			{
				KeyboardNavigation.Current.EnterMenuMode -= this._enterMenuModeHandler;
			}
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x0016DFE6 File Offset: 0x0016C1E6
		private static object OnGetIsMainMenu(DependencyObject d)
		{
			return BooleanBoxes.Box(((Menu)d).IsMainMenu);
		}

		/// <summary> Prepares the specified element to display the specified item. </summary>
		/// <param name="element">The element used to display the specified item.</param>
		/// <param name="item">The item to display.</param>
		// Token: 0x060051C9 RID: 20937 RVA: 0x00136F0A File Offset: 0x0013510A
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			MenuItem.PrepareMenuItem(element, item);
		}

		/// <summary>Responds to the <see cref="E:System.Windows.ContentElement.KeyDown" /> event. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data. </param>
		// Token: 0x060051CA RID: 20938 RVA: 0x0016DFF8 File Offset: 0x0016C1F8
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Handled)
			{
				return;
			}
			switch (e.Key)
			{
			case Key.Left:
			case Key.Right:
				if (base.CurrentSelection != null)
				{
					Panel itemsHost = base.ItemsHost;
					bool flag = itemsHost != null && itemsHost.HasLogicalOrientation && itemsHost.LogicalOrientation == Orientation.Vertical;
					if (flag)
					{
						base.CurrentSelection.OpenSubmenuWithKeyboard();
						e.Handled = true;
					}
				}
				break;
			case Key.Up:
			case Key.Down:
				if (base.CurrentSelection != null)
				{
					Panel itemsHost2 = base.ItemsHost;
					if (itemsHost2 == null || !itemsHost2.HasLogicalOrientation || itemsHost2.LogicalOrientation != Orientation.Vertical)
					{
						base.CurrentSelection.OpenSubmenuWithKeyboard();
						e.Handled = true;
						return;
					}
				}
				break;
			default:
				return;
			}
		}

		/// <summary>Handles the <see cref="E:System.Windows.UIElement.TextInput" /> routed event that occurs when the menu receives text input from any device.</summary>
		/// <param name="e">The event data. </param>
		// Token: 0x060051CB RID: 20939 RVA: 0x0016E0B4 File Offset: 0x0016C2B4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (e.Handled)
			{
				return;
			}
			if (e.UserInitiated && e.Text == " " && this.IsMainMenu && (base.CurrentSelection == null || !base.CurrentSelection.IsSubmenuOpen))
			{
				base.IsMenuMode = false;
				HwndSource hwndSource = PresentationSource.CriticalFromVisual(this) as HwndSource;
				if (hwndSource != null)
				{
					hwndSource.ShowSystemMenu();
					e.Handled = true;
				}
			}
		}

		/// <summary> Called when any mouse button is pressed or released. </summary>
		/// <param name="e">A <see cref="T:System.Windows.Input.MouseButtonEventArgs" /> that contains the event data. </param>
		// Token: 0x060051CC RID: 20940 RVA: 0x0016E12C File Offset: 0x0016C32C
		protected override void HandleMouseButton(MouseButtonEventArgs e)
		{
			base.HandleMouseButton(e);
			if (e.Handled)
			{
				return;
			}
			if (e.ChangedButton != MouseButton.Left && e.ChangedButton != MouseButton.Right)
			{
				return;
			}
			if (base.IsMenuMode)
			{
				FrameworkElement frameworkElement = e.OriginalSource as FrameworkElement;
				if (frameworkElement != null && (frameworkElement == this || frameworkElement.TemplatedParent == this))
				{
					base.IsMenuMode = false;
					e.Handled = true;
				}
			}
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x0016E190 File Offset: 0x0016C390
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			bool result = base.FocusItem(info, itemNavigateArgs);
			if (itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				MenuItem menuItem = info.Container as MenuItem;
				if (menuItem != null && menuItem.Role == MenuItemRole.TopLevelHeader && menuItem.IsSubmenuOpen)
				{
					menuItem.NavigateToStart(itemNavigateArgs);
				}
			}
			return result;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x0016E1DB File Offset: 0x0016C3DB
		private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
		{
			if (!Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
			{
				e.Scope = sender;
				e.Handled = true;
			}
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x0016E200 File Offset: 0x0016C400
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool OnEnterMenuMode(object sender, EventArgs e)
		{
			if (Mouse.Captured != null)
			{
				return false;
			}
			PresentationSource presentationSource = sender as PresentationSource;
			PresentationSource presentationSource2 = PresentationSource.CriticalFromVisual(this);
			if (presentationSource == presentationSource2)
			{
				for (int i = 0; i < base.Items.Count; i++)
				{
					MenuItem menuItem = base.ItemContainerGenerator.ContainerFromIndex(i) as MenuItem;
					if (menuItem != null && !(base.Items[i] is Separator) && menuItem.Focus())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x060051D0 RID: 20944 RVA: 0x000961BF File Offset: 0x000943BF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x060051D1 RID: 20945 RVA: 0x0016E273 File Offset: 0x0016C473
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Menu._dType;
			}
		}

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.Menu.IsMainMenu" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Menu.IsMainMenu" /> dependency property.</returns>
		// Token: 0x04002C7D RID: 11389
		public static readonly DependencyProperty IsMainMenuProperty = DependencyProperty.Register("IsMainMenu", typeof(bool), typeof(Menu), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox, new PropertyChangedCallback(Menu.OnIsMainMenuChanged)));

		// Token: 0x04002C7E RID: 11390
		private KeyboardNavigation.EnterMenuModeEventHandler _enterMenuModeHandler;

		// Token: 0x04002C7F RID: 11391
		private static DependencyObjectType _dType;
	}
}
