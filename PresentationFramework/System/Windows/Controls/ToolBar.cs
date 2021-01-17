using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Provides a container for a group of commands or controls.  </summary>
	// Token: 0x02000544 RID: 1348
	[TemplatePart(Name = "PART_ToolBarPanel", Type = typeof(ToolBarPanel))]
	[TemplatePart(Name = "PART_ToolBarOverflowPanel", Type = typeof(ToolBarOverflowPanel))]
	public class ToolBar : HeaderedItemsControl
	{
		// Token: 0x0600580F RID: 22543 RVA: 0x0018636C File Offset: 0x0018456C
		static ToolBar()
		{
			ToolTipService.IsEnabledProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(null, new CoerceValueCallback(ToolBar.CoerceToolTipIsEnabled)));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(typeof(ToolBar)));
			ToolBar._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToolBar));
			Control.IsTabStopProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			UIElement.FocusableProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Cycle));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			FocusManager.IsFocusScopeProperty.OverrideMetadata(typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			EventManager.RegisterClassHandler(typeof(ToolBar), Mouse.MouseDownEvent, new MouseButtonEventHandler(ToolBar.OnMouseButtonDown), true);
			EventManager.RegisterClassHandler(typeof(ToolBar), ButtonBase.ClickEvent, new RoutedEventHandler(ToolBar._OnClick));
			ControlsTraceLogger.AddControl(TelemetryControls.ToolBar);
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x0018668C File Offset: 0x0018488C
		private static object CoerceOrientation(DependencyObject d, object value)
		{
			ToolBarTray toolBarTray = ((ToolBar)d).ToolBarTray;
			if (toolBarTray == null)
			{
				return value;
			}
			return toolBarTray.Orientation;
		}

		/// <summary> Gets the orientation of the <see cref="T:System.Windows.Controls.ToolBar" />.  </summary>
		/// <returns>The toolbar orientation. The default is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x17001571 RID: 5489
		// (get) Token: 0x06005812 RID: 22546 RVA: 0x001866B5 File Offset: 0x001848B5
		public Orientation Orientation
		{
			get
			{
				return (Orientation)base.GetValue(ToolBar.OrientationProperty);
			}
		}

		/// <summary>Gets or sets a value that indicates where the toolbar should be located in the <see cref="T:System.Windows.Controls.ToolBarTray" />.  </summary>
		/// <returns>The band of the <see cref="T:System.Windows.Controls.ToolBarTray" /> in which the toolbar is positioned. The default is 0.</returns>
		// Token: 0x17001572 RID: 5490
		// (get) Token: 0x06005813 RID: 22547 RVA: 0x001866C7 File Offset: 0x001848C7
		// (set) Token: 0x06005814 RID: 22548 RVA: 0x001866D9 File Offset: 0x001848D9
		public int Band
		{
			get
			{
				return (int)base.GetValue(ToolBar.BandProperty);
			}
			set
			{
				base.SetValue(ToolBar.BandProperty, value);
			}
		}

		/// <summary>Gets or sets the band index number that indicates the position of the toolbar on the band.  </summary>
		/// <returns>The position of a toolbar on the band of a <see cref="T:System.Windows.Controls.ToolBarTray" />.</returns>
		// Token: 0x17001573 RID: 5491
		// (get) Token: 0x06005815 RID: 22549 RVA: 0x001866EC File Offset: 0x001848EC
		// (set) Token: 0x06005816 RID: 22550 RVA: 0x001866FE File Offset: 0x001848FE
		public int BandIndex
		{
			get
			{
				return (int)base.GetValue(ToolBar.BandIndexProperty);
			}
			set
			{
				base.SetValue(ToolBar.BandIndexProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="T:System.Windows.Controls.ToolBar" /> overflow area is currently visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if the overflow area is visible; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001574 RID: 5492
		// (get) Token: 0x06005817 RID: 22551 RVA: 0x00186711 File Offset: 0x00184911
		// (set) Token: 0x06005818 RID: 22552 RVA: 0x00186723 File Offset: 0x00184923
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsOverflowOpen
		{
			get
			{
				return (bool)base.GetValue(ToolBar.IsOverflowOpenProperty);
			}
			set
			{
				base.SetValue(ToolBar.IsOverflowOpenProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x00186738 File Offset: 0x00184938
		private static object CoerceIsOverflowOpen(DependencyObject d, object value)
		{
			if ((bool)value)
			{
				ToolBar toolBar = (ToolBar)d;
				if (!toolBar.IsLoaded)
				{
					toolBar.RegisterToOpenOnLoad();
					return BooleanBoxes.FalseBox;
				}
			}
			return value;
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x0018676C File Offset: 0x0018496C
		private static object CoerceToolTipIsEnabled(DependencyObject d, object value)
		{
			ToolBar toolBar = (ToolBar)d;
			if (!toolBar.IsOverflowOpen)
			{
				return value;
			}
			return BooleanBoxes.FalseBox;
		}

		// Token: 0x0600581B RID: 22555 RVA: 0x0018678F File Offset: 0x0018498F
		private void RegisterToOpenOnLoad()
		{
			base.Loaded += this.OpenOnLoad;
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x001867A3 File Offset: 0x001849A3
		private void OpenOnLoad(object sender, RoutedEventArgs e)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				base.CoerceValue(ToolBar.IsOverflowOpenProperty);
				return null;
			}), null);
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x001867C0 File Offset: 0x001849C0
		private static void OnOverflowOpenChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			ToolBar toolBar = (ToolBar)element;
			if ((bool)e.NewValue)
			{
				Mouse.Capture(toolBar, CaptureMode.SubTree);
				toolBar.SetFocusOnToolBarOverflowPanel();
			}
			else
			{
				ToolBarOverflowPanel toolBarOverflowPanel = toolBar.ToolBarOverflowPanel;
				if (toolBarOverflowPanel != null && toolBarOverflowPanel.IsKeyboardFocusWithin)
				{
					Keyboard.Focus(null);
				}
				if (Mouse.Captured == toolBar)
				{
					Mouse.Capture(null);
				}
			}
			toolBar.CoerceValue(ToolTipService.IsEnabledProperty);
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x00186825 File Offset: 0x00184A25
		private void SetFocusOnToolBarOverflowPanel()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				if (this.ToolBarOverflowPanel != null)
				{
					if (KeyboardNavigation.IsKeyboardMostRecentInputDevice())
					{
						this.ToolBarOverflowPanel.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
					}
					else
					{
						this.ToolBarOverflowPanel.Focus();
					}
				}
				return null;
			}), null);
		}

		/// <summary>Gets a value that indicates whether the toolbar has items that are not visible.  </summary>
		/// <returns>
		///     <see langword="true" /> if there are items on the toolbar that are not visible; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001575 RID: 5493
		// (get) Token: 0x0600581F RID: 22559 RVA: 0x00186841 File Offset: 0x00184A41
		public bool HasOverflowItems
		{
			get
			{
				return (bool)base.GetValue(ToolBar.HasOverflowItemsProperty);
			}
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x00186853 File Offset: 0x00184A53
		internal static void SetIsOverflowItem(DependencyObject element, object value)
		{
			element.SetValue(ToolBar.IsOverflowItemPropertyKey, value);
		}

		/// <summary> Reads the value of the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowItem" /> property from the specified element. </summary>
		/// <param name="element">The element from which to read the property.</param>
		/// <returns>The value of the property.</returns>
		// Token: 0x06005821 RID: 22561 RVA: 0x00186861 File Offset: 0x00184A61
		public static bool GetIsOverflowItem(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(ToolBar.IsOverflowItemProperty);
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x00186884 File Offset: 0x00184A84
		private static void OnOverflowModeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
		{
			ToolBar toolBar = ItemsControl.ItemsControlFromItemContainer(element) as ToolBar;
			if (toolBar != null)
			{
				toolBar.InvalidateLayout();
			}
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x001868A8 File Offset: 0x00184AA8
		private void InvalidateLayout()
		{
			this._minLength = 0.0;
			this._maxLength = 0.0;
			base.InvalidateMeasure();
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				toolBarPanel.InvalidateMeasure();
			}
		}

		// Token: 0x06005824 RID: 22564 RVA: 0x001868EC File Offset: 0x00184AEC
		private static bool IsValidOverflowMode(object o)
		{
			OverflowMode overflowMode = (OverflowMode)o;
			return overflowMode == OverflowMode.AsNeeded || overflowMode == OverflowMode.Always || overflowMode == OverflowMode.Never;
		}

		/// <summary>Writes the value of the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> property to the specified element. </summary>
		/// <param name="element">The element to write the property to.</param>
		/// <param name="mode">The property value to set.</param>
		// Token: 0x06005825 RID: 22565 RVA: 0x0018690D File Offset: 0x00184B0D
		public static void SetOverflowMode(DependencyObject element, OverflowMode mode)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(ToolBar.OverflowModeProperty, mode);
		}

		/// <summary>Reads the value of the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> property from the specified element. </summary>
		/// <param name="element">The element from which to read the property.</param>
		/// <returns>The value of the property.</returns>
		// Token: 0x06005826 RID: 22566 RVA: 0x0018692E File Offset: 0x00184B2E
		[AttachedPropertyBrowsableForChildren(IncludeDescendants = true)]
		public static OverflowMode GetOverflowMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (OverflowMode)element.GetValue(ToolBar.OverflowModeProperty);
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ToolBarAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x06005827 RID: 22567 RVA: 0x0018694E File Offset: 0x00184B4E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToolBarAutomationPeer(this);
		}

		/// <summary>Prepares the specified element to display the specified item. </summary>
		/// <param name="element">The element that will display the item.</param>
		/// <param name="item">The item to display.</param>
		// Token: 0x06005828 RID: 22568 RVA: 0x00186958 File Offset: 0x00184B58
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			FrameworkElement frameworkElement = element as FrameworkElement;
			if (frameworkElement != null)
			{
				Type type = frameworkElement.GetType();
				ResourceKey resourceKey = null;
				if (type == typeof(Button))
				{
					resourceKey = ToolBar.ButtonStyleKey;
				}
				else if (type == typeof(ToggleButton))
				{
					resourceKey = ToolBar.ToggleButtonStyleKey;
				}
				else if (type == typeof(Separator))
				{
					resourceKey = ToolBar.SeparatorStyleKey;
				}
				else if (type == typeof(CheckBox))
				{
					resourceKey = ToolBar.CheckBoxStyleKey;
				}
				else if (type == typeof(RadioButton))
				{
					resourceKey = ToolBar.RadioButtonStyleKey;
				}
				else if (type == typeof(ComboBox))
				{
					resourceKey = ToolBar.ComboBoxStyleKey;
				}
				else if (type == typeof(TextBox))
				{
					resourceKey = ToolBar.TextBoxStyleKey;
				}
				else if (type == typeof(Menu))
				{
					resourceKey = ToolBar.MenuStyleKey;
				}
				if (resourceKey != null)
				{
					bool flag;
					BaseValueSourceInternal valueSource = frameworkElement.GetValueSource(FrameworkElement.StyleProperty, null, out flag);
					if (valueSource <= BaseValueSourceInternal.ImplicitReference)
					{
						frameworkElement.SetResourceReference(FrameworkElement.StyleProperty, resourceKey);
					}
					frameworkElement.DefaultStyleKey = resourceKey;
				}
			}
		}

		// Token: 0x06005829 RID: 22569 RVA: 0x00186A85 File Offset: 0x00184C85
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this._toolBarPanel = null;
			this._toolBarOverflowPanel = null;
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
		}

		/// <summary> Called when the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> property changes. </summary>
		/// <param name="e">
		///
		///       The arguments for the <see cref="E:System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged" /> event.</param>
		// Token: 0x0600582A RID: 22570 RVA: 0x00186A9D File Offset: 0x00184C9D
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			this.InvalidateLayout();
			base.OnItemsChanged(e);
		}

		/// <summary> Remeasures a <see cref="T:System.Windows.Controls.ToolBar" />. </summary>
		/// <param name="constraint">The measurement constraints. A <see cref="T:System.Windows.Controls.ToolBar" /> cannot return a size larger than the constraint.</param>
		/// <returns>The size of the <see cref="T:System.Windows.Controls.ToolBar" />.</returns>
		// Token: 0x0600582B RID: 22571 RVA: 0x00186AAC File Offset: 0x00184CAC
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = base.MeasureOverride(constraint);
			ToolBarPanel toolBarPanel = this.ToolBarPanel;
			if (toolBarPanel != null)
			{
				Thickness margin = toolBarPanel.Margin;
				double num;
				if (toolBarPanel.Orientation == Orientation.Horizontal)
				{
					num = Math.Max(0.0, result.Width - toolBarPanel.DesiredSize.Width + margin.Left + margin.Right);
				}
				else
				{
					num = Math.Max(0.0, result.Height - toolBarPanel.DesiredSize.Height + margin.Top + margin.Bottom);
				}
				this._minLength = toolBarPanel.MinLength + num;
				this._maxLength = toolBarPanel.MaxLength + num;
			}
			return result;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> routed event that occurs when the <see cref="T:System.Windows.Controls.ToolBar" /> loses mouse capture. </summary>
		/// <param name="e">The arguments for the <see cref="E:System.Windows.UIElement.LostMouseCapture" /> event.</param>
		// Token: 0x0600582C RID: 22572 RVA: 0x00186B68 File Offset: 0x00184D68
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			if (Mouse.Captured == null)
			{
				this.Close();
			}
		}

		// Token: 0x17001576 RID: 5494
		// (get) Token: 0x0600582D RID: 22573 RVA: 0x00186B7E File Offset: 0x00184D7E
		internal ToolBarPanel ToolBarPanel
		{
			get
			{
				if (this._toolBarPanel == null)
				{
					this._toolBarPanel = this.FindToolBarPanel();
				}
				return this._toolBarPanel;
			}
		}

		// Token: 0x0600582E RID: 22574 RVA: 0x00186B9C File Offset: 0x00184D9C
		private ToolBarPanel FindToolBarPanel()
		{
			DependencyObject templateChild = base.GetTemplateChild("PART_ToolBarPanel");
			ToolBarPanel toolBarPanel = templateChild as ToolBarPanel;
			if (templateChild != null && toolBarPanel == null)
			{
				throw new NotSupportedException(SR.Get("ToolBar_InvalidStyle_ToolBarPanel", new object[]
				{
					templateChild.GetType()
				}));
			}
			return toolBarPanel;
		}

		// Token: 0x17001577 RID: 5495
		// (get) Token: 0x0600582F RID: 22575 RVA: 0x00186BE2 File Offset: 0x00184DE2
		internal ToolBarOverflowPanel ToolBarOverflowPanel
		{
			get
			{
				if (this._toolBarOverflowPanel == null)
				{
					this._toolBarOverflowPanel = this.FindToolBarOverflowPanel();
				}
				return this._toolBarOverflowPanel;
			}
		}

		// Token: 0x06005830 RID: 22576 RVA: 0x00186C00 File Offset: 0x00184E00
		private ToolBarOverflowPanel FindToolBarOverflowPanel()
		{
			DependencyObject templateChild = base.GetTemplateChild("PART_ToolBarOverflowPanel");
			ToolBarOverflowPanel toolBarOverflowPanel = templateChild as ToolBarOverflowPanel;
			if (templateChild != null && toolBarOverflowPanel == null)
			{
				throw new NotSupportedException(SR.Get("ToolBar_InvalidStyle_ToolBarOverflowPanel", new object[]
				{
					templateChild.GetType()
				}));
			}
			return toolBarOverflowPanel;
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.KeyDown" /> routed event that occurs when a key is pressed on an item in the <see cref="T:System.Windows.Controls.ToolBar" />. </summary>
		/// <param name="e">The arguments for the <see cref="E:System.Windows.UIElement.KeyDown" /> event.</param>
		// Token: 0x06005831 RID: 22577 RVA: 0x00186C48 File Offset: 0x00184E48
		protected override void OnKeyDown(KeyEventArgs e)
		{
			UIElement uielement = null;
			UIElement uielement2 = e.Source as UIElement;
			if (uielement2 != null && ItemsControl.ItemsControlFromItemContainer(uielement2) == this)
			{
				Panel panel = VisualTreeHelper.GetParent(uielement2) as Panel;
				if (panel != null)
				{
					Key key = e.Key;
					if (key != Key.Escape)
					{
						if (key != Key.End)
						{
							if (key == Key.Home)
							{
								uielement = (VisualTreeHelper.GetChild(panel, 0) as UIElement);
							}
						}
						else
						{
							uielement = (VisualTreeHelper.GetChild(panel, VisualTreeHelper.GetChildrenCount(panel) - 1) as UIElement);
						}
					}
					else
					{
						ToolBarOverflowPanel toolBarOverflowPanel = this.ToolBarOverflowPanel;
						if (toolBarOverflowPanel != null && toolBarOverflowPanel.IsKeyboardFocusWithin)
						{
							this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Last));
						}
						else
						{
							Keyboard.Focus(null);
						}
						this.Close();
					}
					if (uielement != null && uielement.Focus())
					{
						e.Handled = true;
					}
				}
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x06005832 RID: 22578 RVA: 0x00186D14 File Offset: 0x00184F14
		private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
		{
			ToolBar toolBar = (ToolBar)sender;
			if (!e.Handled)
			{
				toolBar.Close();
				e.Handled = true;
			}
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x00186D40 File Offset: 0x00184F40
		private static void _OnClick(object e, RoutedEventArgs args)
		{
			ToolBar toolBar = (ToolBar)e;
			ButtonBase buttonBase = args.OriginalSource as ButtonBase;
			if (toolBar.IsOverflowOpen && buttonBase != null && buttonBase.Parent == toolBar)
			{
				toolBar.Close();
			}
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x00186D7A File Offset: 0x00184F7A
		internal override void OnAncestorChanged()
		{
			base.CoerceValue(ToolBar.OrientationProperty);
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x00186D87 File Offset: 0x00184F87
		private void Close()
		{
			base.SetCurrentValueInternal(ToolBar.IsOverflowOpenProperty, BooleanBoxes.FalseBox);
		}

		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x06005836 RID: 22582 RVA: 0x00186D99 File Offset: 0x00184F99
		private ToolBarTray ToolBarTray
		{
			get
			{
				return base.Parent as ToolBarTray;
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x06005837 RID: 22583 RVA: 0x00186DA6 File Offset: 0x00184FA6
		internal double MinLength
		{
			get
			{
				return this._minLength;
			}
		}

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x06005838 RID: 22584 RVA: 0x00186DAE File Offset: 0x00184FAE
		internal double MaxLength
		{
			get
			{
				return this._maxLength;
			}
		}

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x06005839 RID: 22585 RVA: 0x00186DB6 File Offset: 0x00184FB6
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ToolBar._dType;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to buttons on a toolbar.</summary>
		/// <returns>A resource key that represents the default style for buttons on the toolbar.</returns>
		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x0600583A RID: 22586 RVA: 0x00186DBD File Offset: 0x00184FBD
		public static ResourceKey ButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarButtonStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> controls on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for toggle buttons on the toolbar.</returns>
		// Token: 0x1700157D RID: 5501
		// (get) Token: 0x0600583B RID: 22587 RVA: 0x00186DC4 File Offset: 0x00184FC4
		public static ResourceKey ToggleButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarToggleButtonStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to separators on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for separators on the toolbar.</returns>
		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x0600583C RID: 22588 RVA: 0x00186DCB File Offset: 0x00184FCB
		public static ResourceKey SeparatorStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarSeparatorStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to check boxes on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for check boxes on the <see cref="T:System.Windows.Controls.ToolBar" />.</returns>
		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x0600583D RID: 22589 RVA: 0x00186DD2 File Offset: 0x00184FD2
		public static ResourceKey CheckBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarCheckBoxStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to radio buttons on a toolbar.</summary>
		/// <returns>A resource key that represents the default style for radio buttons on the toolbar.</returns>
		// Token: 0x17001580 RID: 5504
		// (get) Token: 0x0600583E RID: 22590 RVA: 0x00186DD9 File Offset: 0x00184FD9
		public static ResourceKey RadioButtonStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarRadioButtonStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to combo boxes on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for combo boxes on the toolbar.</returns>
		// Token: 0x17001581 RID: 5505
		// (get) Token: 0x0600583F RID: 22591 RVA: 0x00186DE0 File Offset: 0x00184FE0
		public static ResourceKey ComboBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarComboBoxStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to text boxes on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for text boxes on the toolbar</returns>
		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x06005840 RID: 22592 RVA: 0x00186DE7 File Offset: 0x00184FE7
		public static ResourceKey TextBoxStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarTextBoxStyleKey;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Style" /> applied to menus on a <see cref="T:System.Windows.Controls.ToolBar" />.</summary>
		/// <returns>A resource key that represents the default style for menus on the toolbar.</returns>
		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06005841 RID: 22593 RVA: 0x00186DEE File Offset: 0x00184FEE
		public static ResourceKey MenuStyleKey
		{
			get
			{
				return SystemResourceKey.ToolBarMenuStyleKey;
			}
		}

		// Token: 0x04002EAB RID: 11947
		private static readonly DependencyPropertyKey OrientationPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Orientation", typeof(Orientation), typeof(ToolBar), new FrameworkPropertyMetadata(Orientation.Horizontal, null, new CoerceValueCallback(ToolBar.CoerceOrientation)));

		/// <summary>Identifies the <see cref="T:System.Windows.Controls.Orientation" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="T:System.Windows.Controls.Orientation" /> dependency property.</returns>
		// Token: 0x04002EAC RID: 11948
		public static readonly DependencyProperty OrientationProperty = ToolBar.OrientationPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.Band" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.Band" /> dependency property.</returns>
		// Token: 0x04002EAD RID: 11949
		public static readonly DependencyProperty BandProperty = DependencyProperty.Register("Band", typeof(int), typeof(ToolBar), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.BandIndex" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.BandIndex" /> dependency property.</returns>
		// Token: 0x04002EAE RID: 11950
		public static readonly DependencyProperty BandIndexProperty = DependencyProperty.Register("BandIndex", typeof(int), typeof(ToolBar), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowOpen" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowOpen" /> dependency property.</returns>
		// Token: 0x04002EAF RID: 11951
		public static readonly DependencyProperty IsOverflowOpenProperty = DependencyProperty.Register("IsOverflowOpen", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ToolBar.OnOverflowOpenChanged), new CoerceValueCallback(ToolBar.CoerceIsOverflowOpen)));

		// Token: 0x04002EB0 RID: 11952
		internal static readonly DependencyPropertyKey HasOverflowItemsPropertyKey = DependencyProperty.RegisterReadOnly("HasOverflowItems", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.HasOverflowItems" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.HasOverflowItems" /> dependency property.</returns>
		// Token: 0x04002EB1 RID: 11953
		public static readonly DependencyProperty HasOverflowItemsProperty = ToolBar.HasOverflowItemsPropertyKey.DependencyProperty;

		// Token: 0x04002EB2 RID: 11954
		internal static readonly DependencyPropertyKey IsOverflowItemPropertyKey = DependencyProperty.RegisterAttachedReadOnly("IsOverflowItem", typeof(bool), typeof(ToolBar), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowItem" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.IsOverflowItem" /> attached property.</returns>
		// Token: 0x04002EB3 RID: 11955
		public static readonly DependencyProperty IsOverflowItemProperty = ToolBar.IsOverflowItemPropertyKey.DependencyProperty;

		/// <summary> Identifies the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> attached property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ToolBar.OverflowMode" /> attached property.</returns>
		// Token: 0x04002EB4 RID: 11956
		public static readonly DependencyProperty OverflowModeProperty = DependencyProperty.RegisterAttached("OverflowMode", typeof(OverflowMode), typeof(ToolBar), new FrameworkPropertyMetadata(OverflowMode.AsNeeded, new PropertyChangedCallback(ToolBar.OnOverflowModeChanged)), new ValidateValueCallback(ToolBar.IsValidOverflowMode));

		// Token: 0x04002EB5 RID: 11957
		private ToolBarPanel _toolBarPanel;

		// Token: 0x04002EB6 RID: 11958
		private ToolBarOverflowPanel _toolBarOverflowPanel;

		// Token: 0x04002EB7 RID: 11959
		private const string ToolBarPanelTemplateName = "PART_ToolBarPanel";

		// Token: 0x04002EB8 RID: 11960
		private const string ToolBarOverflowPanelTemplateName = "PART_ToolBarOverflowPanel";

		// Token: 0x04002EB9 RID: 11961
		private double _minLength;

		// Token: 0x04002EBA RID: 11962
		private double _maxLength;

		// Token: 0x04002EBB RID: 11963
		private static DependencyObjectType _dType;
	}
}
