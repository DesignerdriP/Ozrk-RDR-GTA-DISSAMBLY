using System;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Represents the base class for user interface (UI) elements that use a <see cref="T:System.Windows.Controls.ControlTemplate" /> to define their appearance. </summary>
	// Token: 0x0200048E RID: 1166
	public class Control : FrameworkElement
	{
		// Token: 0x06004481 RID: 17537 RVA: 0x001378BC File Offset: 0x00135ABC
		static Control()
		{
			Control.PreviewMouseDoubleClickEvent = EventManager.RegisterRoutedEvent("PreviewMouseDoubleClick", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(Control));
			Control.MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Direct, typeof(MouseButtonEventHandler), typeof(Control));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Control), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
			EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			EventManager.RegisterClassHandler(typeof(Control), UIElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			EventManager.RegisterClassHandler(typeof(Control), UIElement.PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			EventManager.RegisterClassHandler(typeof(Control), UIElement.MouseRightButtonDownEvent, new MouseButtonEventHandler(Control.HandleDoubleClick), true);
			UIElement.IsKeyboardFocusedPropertyKey.OverrideMetadata(typeof(Control), new PropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Control" /> class. </summary>
		// Token: 0x06004482 RID: 17538 RVA: 0x00137C6C File Offset: 0x00135E6C
		public Control()
		{
			PropertyMetadata metadata = Control.TemplateProperty.GetMetadata(base.DependencyObjectType);
			ControlTemplate controlTemplate = (ControlTemplate)metadata.DefaultValue;
			if (controlTemplate != null)
			{
				Control.OnTemplateChanged(this, new DependencyPropertyChangedEventArgs(Control.TemplateProperty, metadata, null, controlTemplate));
			}
		}

		/// <summary>Gets or sets a brush that describes the border background of a control. </summary>
		/// <returns>The brush that is used to fill the control's border; the default is <see cref="P:System.Windows.Media.Brushes.Transparent" />.</returns>
		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x06004483 RID: 17539 RVA: 0x00137CB2 File Offset: 0x00135EB2
		// (set) Token: 0x06004484 RID: 17540 RVA: 0x00137CC4 File Offset: 0x00135EC4
		[Bindable(true)]
		[Category("Appearance")]
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(Control.BorderBrushProperty);
			}
			set
			{
				base.SetValue(Control.BorderBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the border thickness of a control.  </summary>
		/// <returns>A thickness value; the default is a thickness of 0 on all four sides.</returns>
		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x06004485 RID: 17541 RVA: 0x00137CD2 File Offset: 0x00135ED2
		// (set) Token: 0x06004486 RID: 17542 RVA: 0x00137CE4 File Offset: 0x00135EE4
		[Bindable(true)]
		[Category("Appearance")]
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(Control.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(Control.BorderThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a brush that describes the background of a control. </summary>
		/// <returns>The brush that is used to fill the background of the control. The default is <see cref="P:System.Windows.Media.Brushes.Transparent" />. </returns>
		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x06004487 RID: 17543 RVA: 0x00137CF7 File Offset: 0x00135EF7
		// (set) Token: 0x06004488 RID: 17544 RVA: 0x00137D09 File Offset: 0x00135F09
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(Control.BackgroundProperty);
			}
			set
			{
				base.SetValue(Control.BackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets a brush that describes the foreground color.    </summary>
		/// <returns>The brush that paints the foreground of the control. The default value is the system dialog font color.</returns>
		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x06004489 RID: 17545 RVA: 0x00137D17 File Offset: 0x00135F17
		// (set) Token: 0x0600448A RID: 17546 RVA: 0x00137D29 File Offset: 0x00135F29
		[Bindable(true)]
		[Category("Appearance")]
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(Control.ForegroundProperty);
			}
			set
			{
				base.SetValue(Control.ForegroundProperty, value);
			}
		}

		/// <summary>Gets or sets the font family of the control.   </summary>
		/// <returns>A font family. The default is the system dialog font.</returns>
		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x0600448B RID: 17547 RVA: 0x00137D37 File Offset: 0x00135F37
		// (set) Token: 0x0600448C RID: 17548 RVA: 0x00137D49 File Offset: 0x00135F49
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.Font)]
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(Control.FontFamilyProperty);
			}
			set
			{
				base.SetValue(Control.FontFamilyProperty, value);
			}
		}

		/// <summary>Gets or sets the font size.   </summary>
		/// <returns>The size of the text in the <see cref="T:System.Windows.Controls.Control" />. The default is <see cref="P:System.Windows.SystemFonts.MessageFontSize" />. The font size must be a positive number.</returns>
		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x0600448D RID: 17549 RVA: 0x00137D57 File Offset: 0x00135F57
		// (set) Token: 0x0600448E RID: 17550 RVA: 0x00137D69 File Offset: 0x00135F69
		[TypeConverter(typeof(FontSizeConverter))]
		[Bindable(true)]
		[Category("Appearance")]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(Control.FontSizeProperty);
			}
			set
			{
				base.SetValue(Control.FontSizeProperty, value);
			}
		}

		/// <summary>Gets or sets the degree to which a font is condensed or expanded on the screen.   </summary>
		/// <returns>A <see cref="T:System.Windows.FontStretch" /> value. The default is <see cref="P:System.Windows.FontStretches.Normal" />.</returns>
		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x0600448F RID: 17551 RVA: 0x00137D7C File Offset: 0x00135F7C
		// (set) Token: 0x06004490 RID: 17552 RVA: 0x00137D8E File Offset: 0x00135F8E
		[Bindable(true)]
		[Category("Appearance")]
		public FontStretch FontStretch
		{
			get
			{
				return (FontStretch)base.GetValue(Control.FontStretchProperty);
			}
			set
			{
				base.SetValue(Control.FontStretchProperty, value);
			}
		}

		/// <summary>Gets or sets the font style.   </summary>
		/// <returns>A <see cref="T:System.Windows.FontStyle" /> value. The default is <see cref="P:System.Windows.FontStyles.Normal" />.</returns>
		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x06004491 RID: 17553 RVA: 0x00137DA1 File Offset: 0x00135FA1
		// (set) Token: 0x06004492 RID: 17554 RVA: 0x00137DB3 File Offset: 0x00135FB3
		[Bindable(true)]
		[Category("Appearance")]
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(Control.FontStyleProperty);
			}
			set
			{
				base.SetValue(Control.FontStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the weight or thickness of the specified font.   </summary>
		/// <returns>A <see cref="T:System.Windows.FontWeight" /> value. The default is <see cref="P:System.Windows.FontWeights.Normal" />.</returns>
		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x00137DC6 File Offset: 0x00135FC6
		// (set) Token: 0x06004494 RID: 17556 RVA: 0x00137DD8 File Offset: 0x00135FD8
		[Bindable(true)]
		[Category("Appearance")]
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(Control.FontWeightProperty);
			}
			set
			{
				base.SetValue(Control.FontWeightProperty, value);
			}
		}

		/// <summary>Gets or sets the horizontal alignment of the control's content.    </summary>
		/// <returns>One of the <see cref="T:System.Windows.HorizontalAlignment" /> values. The default is <see cref="F:System.Windows.HorizontalAlignment.Left" />.</returns>
		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x06004495 RID: 17557 RVA: 0x00137DEB File Offset: 0x00135FEB
		// (set) Token: 0x06004496 RID: 17558 RVA: 0x00137DFD File Offset: 0x00135FFD
		[Bindable(true)]
		[Category("Layout")]
		public HorizontalAlignment HorizontalContentAlignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(Control.HorizontalContentAlignmentProperty);
			}
			set
			{
				base.SetValue(Control.HorizontalContentAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets the vertical alignment of the control's content.    </summary>
		/// <returns>One of the <see cref="T:System.Windows.VerticalAlignment" /> values. The default is <see cref="F:System.Windows.VerticalAlignment.Top" />.</returns>
		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x00137E10 File Offset: 0x00136010
		// (set) Token: 0x06004498 RID: 17560 RVA: 0x00137E22 File Offset: 0x00136022
		[Bindable(true)]
		[Category("Layout")]
		public VerticalAlignment VerticalContentAlignment
		{
			get
			{
				return (VerticalAlignment)base.GetValue(Control.VerticalContentAlignmentProperty);
			}
			set
			{
				base.SetValue(Control.VerticalContentAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets a value that determines the order in which elements receive focus when the user navigates through controls by using the TAB key.   </summary>
		/// <returns>A value that determines the order of logical navigation for a device. The default value is <see cref="F:System.Int32.MaxValue" />.</returns>
		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06004499 RID: 17561 RVA: 0x00137E35 File Offset: 0x00136035
		// (set) Token: 0x0600449A RID: 17562 RVA: 0x00137E47 File Offset: 0x00136047
		[Bindable(true)]
		[Category("Behavior")]
		public int TabIndex
		{
			get
			{
				return (int)base.GetValue(Control.TabIndexProperty);
			}
			set
			{
				base.SetValue(Control.TabIndexProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether a control is included in tab navigation.   </summary>
		/// <returns>
		///     <see langword="true" /> if the control is included in tab navigation; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x0600449B RID: 17563 RVA: 0x00137E5A File Offset: 0x0013605A
		// (set) Token: 0x0600449C RID: 17564 RVA: 0x00137E6C File Offset: 0x0013606C
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsTabStop
		{
			get
			{
				return (bool)base.GetValue(Control.IsTabStopProperty);
			}
			set
			{
				base.SetValue(Control.IsTabStopProperty, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x00137E80 File Offset: 0x00136080
		private static bool IsMarginValid(object value)
		{
			Thickness thickness = (Thickness)value;
			return thickness.Left >= 0.0 && thickness.Right >= 0.0 && thickness.Top >= 0.0 && thickness.Bottom >= 0.0;
		}

		/// <summary>Gets or sets the padding inside a control.   </summary>
		/// <returns>The amount of space between the content of a <see cref="T:System.Windows.Controls.Control" /> and its <see cref="P:System.Windows.FrameworkElement.Margin" /> or <see cref="T:System.Windows.Controls.Border" />.  The default is a thickness of 0 on all four sides.</returns>
		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x0600449E RID: 17566 RVA: 0x00137EE1 File Offset: 0x001360E1
		// (set) Token: 0x0600449F RID: 17567 RVA: 0x00137EF3 File Offset: 0x001360F3
		[Bindable(true)]
		[Category("Layout")]
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(Control.PaddingProperty);
			}
			set
			{
				base.SetValue(Control.PaddingProperty, value);
			}
		}

		/// <summary>Gets or sets a control template.   </summary>
		/// <returns>The template that defines the appearance of the <see cref="T:System.Windows.Controls.Control" />.</returns>
		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x060044A0 RID: 17568 RVA: 0x00137F06 File Offset: 0x00136106
		// (set) Token: 0x060044A1 RID: 17569 RVA: 0x00137F0E File Offset: 0x0013610E
		public ControlTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(Control.TemplateProperty, value);
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x060044A2 RID: 17570 RVA: 0x00137F1C File Offset: 0x0013611C
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x060044A3 RID: 17571 RVA: 0x00137F06 File Offset: 0x00136106
		// (set) Token: 0x060044A4 RID: 17572 RVA: 0x00137F24 File Offset: 0x00136124
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (ControlTemplate)value;
			}
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x00137F32 File Offset: 0x00136132
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((ControlTemplate)oldTemplate, (ControlTemplate)newTemplate);
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x00137F48 File Offset: 0x00136148
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control fe = (Control)d;
			StyleHelper.UpdateTemplateCache(fe, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, Control.TemplateProperty);
		}

		/// <summary>Called whenever the control's template changes. </summary>
		/// <param name="oldTemplate">The old template.</param>
		/// <param name="newTemplate">The new template.</param>
		// Token: 0x060044A7 RID: 17575 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
		}

		/// <summary>Gets a value that indicates whether a control supports scrolling.</summary>
		/// <returns>
		///     <see langword="true" /> if the control has a <see cref="T:System.Windows.Controls.ScrollViewer" /> in its style and has a custom keyboard scrolling behavior; otherwise, <see langword="false" />.</returns>
		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x0000B02A File Offset: 0x0000922A
		protected internal virtual bool HandlesScrolling
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x060044A9 RID: 17577 RVA: 0x00137F7F File Offset: 0x0013617F
		// (set) Token: 0x060044AA RID: 17578 RVA: 0x00137F8C File Offset: 0x0013618C
		internal bool VisualStateChangeSuspended
		{
			get
			{
				return this.ReadControlFlag(Control.ControlBoolFlags.VisualStateChangeSuspended);
			}
			set
			{
				this.WriteControlFlag(Control.ControlBoolFlags.VisualStateChangeSuspended, value);
			}
		}

		/// <summary>Returns the string representation of a <see cref="T:System.Windows.Controls.Control" /> object. </summary>
		/// <returns>A string that represents the control.</returns>
		// Token: 0x060044AB RID: 17579 RVA: 0x00137F9C File Offset: 0x0013619C
		public override string ToString()
		{
			string text;
			if (base.CheckAccess())
			{
				text = this.GetPlainText();
			}
			else
			{
				text = (string)base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback((object o) => this.GetPlainText()), null);
			}
			if (!string.IsNullOrEmpty(text))
			{
				return SR.Get("ToStringFormatString_Control", new object[]
				{
					base.ToString(),
					text
				});
			}
			return base.ToString();
		}

		/// <summary>Occurs when a user clicks the mouse button two or more times. </summary>
		// Token: 0x140000AB RID: 171
		// (add) Token: 0x060044AC RID: 17580 RVA: 0x00138013 File Offset: 0x00136213
		// (remove) Token: 0x060044AD RID: 17581 RVA: 0x00138021 File Offset: 0x00136221
		public event MouseButtonEventHandler PreviewMouseDoubleClick
		{
			add
			{
				base.AddHandler(Control.PreviewMouseDoubleClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(Control.PreviewMouseDoubleClickEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Control.PreviewMouseDoubleClick" /> routed event. </summary>
		/// <param name="e">The event data. </param>
		// Token: 0x060044AE RID: 17582 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnPreviewMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Occurs when a mouse button is clicked two or more times. </summary>
		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060044AF RID: 17583 RVA: 0x0013802F File Offset: 0x0013622F
		// (remove) Token: 0x060044B0 RID: 17584 RVA: 0x0013803D File Offset: 0x0013623D
		public event MouseButtonEventHandler MouseDoubleClick
		{
			add
			{
				base.AddHandler(Control.MouseDoubleClickEvent, value);
			}
			remove
			{
				base.RemoveHandler(Control.MouseDoubleClickEvent, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Control.MouseDoubleClick" /> routed event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060044B1 RID: 17585 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnMouseDoubleClick(MouseButtonEventArgs e)
		{
			base.RaiseEvent(e);
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0013804C File Offset: 0x0013624C
		private static void HandleDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount == 2)
			{
				Control control = (Control)sender;
				MouseButtonEventArgs mouseButtonEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton, e.StylusDevice);
				if (e.RoutedEvent == UIElement.PreviewMouseLeftButtonDownEvent || e.RoutedEvent == UIElement.PreviewMouseRightButtonDownEvent)
				{
					mouseButtonEventArgs.RoutedEvent = Control.PreviewMouseDoubleClickEvent;
					mouseButtonEventArgs.Source = e.OriginalSource;
					mouseButtonEventArgs.OverrideSource(e.Source);
					control.OnPreviewMouseDoubleClick(mouseButtonEventArgs);
				}
				else
				{
					mouseButtonEventArgs.RoutedEvent = Control.MouseDoubleClickEvent;
					mouseButtonEventArgs.Source = e.OriginalSource;
					mouseButtonEventArgs.OverrideSource(e.Source);
					control.OnMouseDoubleClick(mouseButtonEventArgs);
				}
				if (mouseButtonEventArgs.Handled)
				{
					e.Handled = true;
				}
			}
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x00138109 File Offset: 0x00136309
		internal override void OnPreApplyTemplate()
		{
			this.VisualStateChangeSuspended = true;
			base.OnPreApplyTemplate();
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x00138118 File Offset: 0x00136318
		internal override void OnPostApplyTemplate()
		{
			base.OnPostApplyTemplate();
			this.VisualStateChangeSuspended = false;
			this.UpdateVisualState(false);
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0013812E File Offset: 0x0013632E
		internal void UpdateVisualState()
		{
			this.UpdateVisualState(true);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x00138137 File Offset: 0x00136337
		internal void UpdateVisualState(bool useTransitions)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Info, EventTrace.Event.UpdateVisualStateStart);
			if (!this.VisualStateChangeSuspended)
			{
				this.ChangeVisualState(useTransitions);
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordGeneral | EventTrace.Keyword.KeywordPerf, EventTrace.Level.Info, EventTrace.Event.UpdateVisualStateEnd);
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x00138159 File Offset: 0x00136359
		internal virtual void ChangeVisualState(bool useTransitions)
		{
			this.ChangeValidationVisualState(useTransitions);
		}

		// Token: 0x060044B8 RID: 17592 RVA: 0x00138162 File Offset: 0x00136362
		internal void ChangeValidationVisualState(bool useTransitions)
		{
			if (!Validation.GetHasError(this))
			{
				VisualStateManager.GoToState(this, "Valid", useTransitions);
				return;
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "InvalidFocused", useTransitions);
				return;
			}
			VisualStateManager.GoToState(this, "InvalidUnfocused", useTransitions);
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x001381A0 File Offset: 0x001363A0
		internal static void OnVisualStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Control control = d as Control;
			if (control != null)
			{
				control.UpdateVisualState();
			}
		}

		/// <summary>Called to remeasure a control. </summary>
		/// <param name="constraint">The maximum size that the method can return.</param>
		/// <returns>The size of the control, up to the maximum specified by <paramref name="constraint" />.</returns>
		// Token: 0x060044BA RID: 17594 RVA: 0x001381C0 File Offset: 0x001363C0
		protected override Size MeasureOverride(Size constraint)
		{
			int visualChildrenCount = this.VisualChildrenCount;
			if (visualChildrenCount > 0)
			{
				UIElement uielement = (UIElement)this.GetVisualChild(0);
				if (uielement != null)
				{
					uielement.Measure(constraint);
					return uielement.DesiredSize;
				}
			}
			return new Size(0.0, 0.0);
		}

		/// <summary>Called to arrange and size the content of a <see cref="T:System.Windows.Controls.Control" /> object. </summary>
		/// <param name="arrangeBounds">The computed size that is used to arrange the content.</param>
		/// <returns>The size of the control.</returns>
		// Token: 0x060044BB RID: 17595 RVA: 0x00138210 File Offset: 0x00136410
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			int visualChildrenCount = this.VisualChildrenCount;
			if (visualChildrenCount > 0)
			{
				UIElement uielement = (UIElement)this.GetVisualChild(0);
				if (uielement != null)
				{
					uielement.Arrange(new Rect(arrangeBounds));
				}
			}
			return arrangeBounds;
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x00138245 File Offset: 0x00136445
		internal bool ReadControlFlag(Control.ControlBoolFlags reqFlag)
		{
			return (this._controlBoolField & reqFlag) > (Control.ControlBoolFlags)0;
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x00138252 File Offset: 0x00136452
		internal void WriteControlFlag(Control.ControlBoolFlags reqFlag, bool set)
		{
			if (set)
			{
				this._controlBoolField |= reqFlag;
				return;
			}
			this._controlBoolField &= ~reqFlag;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.BorderBrush" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.BorderBrush" /> dependency property.</returns>
		// Token: 0x0400289C RID: 10396
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderBrushProperty = Border.BorderBrushProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.BorderThickness" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.BorderThickness" /> dependency property.</returns>
		// Token: 0x0400289D RID: 10397
		[CommonDependencyProperty]
		public static readonly DependencyProperty BorderThicknessProperty = Border.BorderThicknessProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(Border.BorderThicknessProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.Background" /> dependency property.</returns>
		// Token: 0x0400289E RID: 10398
		[CommonDependencyProperty]
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(Panel.BackgroundProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.None));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.Foreground" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.Foreground" /> dependency property.</returns>
		// Token: 0x0400289F RID: 10399
		[CommonDependencyProperty]
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.FontFamily" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.FontFamily" /> dependency property.</returns>
		// Token: 0x040028A0 RID: 10400
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.FontSize" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.FontSize" /> dependency property.</returns>
		// Token: 0x040028A1 RID: 10401
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.FontStretch" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.FontStretch" /> dependency property.</returns>
		// Token: 0x040028A2 RID: 10402
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(TextElement.FontStretchProperty.DefaultMetadata.DefaultValue, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.FontStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.FontStyle" /> dependency property.</returns>
		// Token: 0x040028A3 RID: 10403
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.FontWeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.FontWeight" /> dependency property.</returns>
		// Token: 0x040028A4 RID: 10404
		[CommonDependencyProperty]
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(Control), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.HorizontalContentAlignment" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.HorizontalContentAlignment" /> dependency property.</returns>
		// Token: 0x040028A5 RID: 10405
		[CommonDependencyProperty]
		public static readonly DependencyProperty HorizontalContentAlignmentProperty = DependencyProperty.Register("HorizontalContentAlignment", typeof(HorizontalAlignment), typeof(Control), new FrameworkPropertyMetadata(HorizontalAlignment.Left), new ValidateValueCallback(FrameworkElement.ValidateHorizontalAlignmentValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.VerticalContentAlignment" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.VerticalContentAlignment" /> dependency property.</returns>
		// Token: 0x040028A6 RID: 10406
		[CommonDependencyProperty]
		public static readonly DependencyProperty VerticalContentAlignmentProperty = DependencyProperty.Register("VerticalContentAlignment", typeof(VerticalAlignment), typeof(Control), new FrameworkPropertyMetadata(VerticalAlignment.Top), new ValidateValueCallback(FrameworkElement.ValidateVerticalAlignmentValue));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.TabIndex" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.TabIndex" /> dependency property.</returns>
		// Token: 0x040028A7 RID: 10407
		[CommonDependencyProperty]
		public static readonly DependencyProperty TabIndexProperty = KeyboardNavigation.TabIndexProperty.AddOwner(typeof(Control));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.IsTabStop" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.IsTabStop" /> dependency property.</returns>
		// Token: 0x040028A8 RID: 10408
		[CommonDependencyProperty]
		public static readonly DependencyProperty IsTabStopProperty = KeyboardNavigation.IsTabStopProperty.AddOwner(typeof(Control));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.Padding" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.Padding" /> dependency property.</returns>
		// Token: 0x040028A9 RID: 10409
		[CommonDependencyProperty]
		public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Control), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsParentMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Control.Template" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Control.Template" /> dependency property.</returns>
		// Token: 0x040028AA RID: 10410
		[CommonDependencyProperty]
		public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ControlTemplate), typeof(Control), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(Control.OnTemplateChanged)));

		// Token: 0x040028AD RID: 10413
		private ControlTemplate _templateCache;

		// Token: 0x040028AE RID: 10414
		internal Control.ControlBoolFlags _controlBoolField;

		// Token: 0x02000962 RID: 2402
		internal enum ControlBoolFlags : ushort
		{
			// Token: 0x0400440F RID: 17423
			ContentIsNotLogical = 1,
			// Token: 0x04004410 RID: 17424
			IsSpaceKeyDown,
			// Token: 0x04004411 RID: 17425
			HeaderIsNotLogical = 4,
			// Token: 0x04004412 RID: 17426
			CommandDisabled = 8,
			// Token: 0x04004413 RID: 17427
			ContentIsItem = 16,
			// Token: 0x04004414 RID: 17428
			HeaderIsItem = 32,
			// Token: 0x04004415 RID: 17429
			ScrollHostValid = 64,
			// Token: 0x04004416 RID: 17430
			ContainsSelection = 128,
			// Token: 0x04004417 RID: 17431
			VisualStateChangeSuspended = 256
		}
	}
}
