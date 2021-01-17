using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that can be dragged by the user. </summary>
	// Token: 0x020005AD RID: 1453
	[DefaultEvent("DragDelta")]
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class Thumb : Control
	{
		// Token: 0x060060B1 RID: 24753 RVA: 0x001B18D4 File Offset: 0x001AFAD4
		static Thumb()
		{
			Thumb.DragStartedEvent = EventManager.RegisterRoutedEvent("DragStarted", RoutingStrategy.Bubble, typeof(DragStartedEventHandler), typeof(Thumb));
			Thumb.DragDeltaEvent = EventManager.RegisterRoutedEvent("DragDelta", RoutingStrategy.Bubble, typeof(DragDeltaEventHandler), typeof(Thumb));
			Thumb.DragCompletedEvent = EventManager.RegisterRoutedEvent("DragCompleted", RoutingStrategy.Bubble, typeof(DragCompletedEventHandler), typeof(Thumb));
			Thumb.IsDraggingPropertyKey = DependencyProperty.RegisterReadOnly("IsDragging", typeof(bool), typeof(Thumb), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Thumb.OnIsDraggingPropertyChanged)));
			Thumb.IsDraggingProperty = Thumb.IsDraggingPropertyKey.DependencyProperty;
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(typeof(Thumb)));
			Thumb._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Thumb));
			UIElement.FocusableProperty.OverrideMetadata(typeof(Thumb), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			EventManager.RegisterClassHandler(typeof(Thumb), Mouse.LostMouseCaptureEvent, new MouseEventHandler(Thumb.OnLostMouseCapture));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(Thumb), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control receives logical focus and mouse capture.</summary>
		// Token: 0x1400011F RID: 287
		// (add) Token: 0x060060B2 RID: 24754 RVA: 0x001B1A54 File Offset: 0x001AFC54
		// (remove) Token: 0x060060B3 RID: 24755 RVA: 0x001B1A62 File Offset: 0x001AFC62
		[Category("Behavior")]
		public event DragStartedEventHandler DragStarted
		{
			add
			{
				base.AddHandler(Thumb.DragStartedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragStartedEvent, value);
			}
		}

		/// <summary>Occurs one or more times as the mouse changes position when a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control has logical focus and mouse capture. </summary>
		// Token: 0x14000120 RID: 288
		// (add) Token: 0x060060B4 RID: 24756 RVA: 0x001B1A70 File Offset: 0x001AFC70
		// (remove) Token: 0x060060B5 RID: 24757 RVA: 0x001B1A7E File Offset: 0x001AFC7E
		[Category("Behavior")]
		public event DragDeltaEventHandler DragDelta
		{
			add
			{
				base.AddHandler(Thumb.DragDeltaEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragDeltaEvent, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control loses mouse capture.</summary>
		// Token: 0x14000121 RID: 289
		// (add) Token: 0x060060B6 RID: 24758 RVA: 0x001B1A8C File Offset: 0x001AFC8C
		// (remove) Token: 0x060060B7 RID: 24759 RVA: 0x001B1A9A File Offset: 0x001AFC9A
		[Category("Behavior")]
		public event DragCompletedEventHandler DragCompleted
		{
			add
			{
				base.AddHandler(Thumb.DragCompletedEvent, value);
			}
			remove
			{
				base.RemoveHandler(Thumb.DragCompletedEvent, value);
			}
		}

		/// <summary>Gets whether the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control has logical focus and mouse capture and the left mouse button is pressed.   </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control has focus and mouse capture; otherwise <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17001743 RID: 5955
		// (get) Token: 0x060060B8 RID: 24760 RVA: 0x001B1AA8 File Offset: 0x001AFCA8
		// (set) Token: 0x060060B9 RID: 24761 RVA: 0x001B1ABA File Offset: 0x001AFCBA
		[Bindable(true)]
		[Browsable(false)]
		[Category("Appearance")]
		public bool IsDragging
		{
			get
			{
				return (bool)base.GetValue(Thumb.IsDraggingProperty);
			}
			protected set
			{
				base.SetValue(Thumb.IsDraggingPropertyKey, BooleanBoxes.Box(value));
			}
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x001B1AD0 File Offset: 0x001AFCD0
		private static void OnIsDraggingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Thumb thumb = (Thumb)d;
			thumb.OnDraggingChanged(e);
			thumb.UpdateVisualState();
		}

		/// <summary>Cancels a drag operation for the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		// Token: 0x060060BB RID: 24763 RVA: 0x001B1AF4 File Offset: 0x001AFCF4
		public void CancelDrag()
		{
			if (this.IsDragging)
			{
				if (base.IsMouseCaptured)
				{
					base.ReleaseMouseCapture();
				}
				base.ClearValue(Thumb.IsDraggingPropertyKey);
				base.RaiseEvent(new DragCompletedEventArgs(this._previousScreenCoordPosition.X - this._originScreenCoordPosition.X, this._previousScreenCoordPosition.Y - this._originScreenCoordPosition.Y, true));
			}
		}

		/// <summary>Responds to a change in the value of the <see cref="P:System.Windows.Controls.Primitives.Thumb.IsDragging" /> property. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060BC RID: 24764 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnDraggingChanged(DependencyPropertyChangedEventArgs e)
		{
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x001B1B5C File Offset: 0x001AFD5C
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStateManager.GoToState(this, "Disabled", useTransitions);
			}
			else if (this.IsDragging)
			{
				VisualStateManager.GoToState(this, "Pressed", useTransitions);
			}
			else if (base.IsMouseOver)
			{
				VisualStateManager.GoToState(this, "MouseOver", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStateManager.GoToState(this, "Focused", useTransitions);
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Creates an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.ThumbAutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control.</returns>
		// Token: 0x060060BE RID: 24766 RVA: 0x001B1BE6 File Offset: 0x001AFDE6
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ThumbAutomationPeer(this);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.ContentElement.MouseLeftButtonDown" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060BF RID: 24767 RVA: 0x001B1BF0 File Offset: 0x001AFDF0
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			if (!this.IsDragging)
			{
				e.Handled = true;
				base.Focus();
				base.CaptureMouse();
				base.SetValue(Thumb.IsDraggingPropertyKey, true);
				this._originThumbPoint = e.GetPosition(this);
				this._previousScreenCoordPosition = (this._originScreenCoordPosition = SafeSecurityHelper.ClientToScreen(this, this._originThumbPoint));
				bool flag = true;
				try
				{
					base.RaiseEvent(new DragStartedEventArgs(this._originThumbPoint.X, this._originThumbPoint.Y));
					flag = false;
				}
				finally
				{
					if (flag)
					{
						this.CancelDrag();
					}
				}
			}
			base.OnMouseLeftButtonDown(e);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.ContentElement.MouseLeftButtonUp" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060C0 RID: 24768 RVA: 0x001B1C98 File Offset: 0x001AFE98
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if (base.IsMouseCaptured && this.IsDragging)
			{
				e.Handled = true;
				base.ClearValue(Thumb.IsDraggingPropertyKey);
				base.ReleaseMouseCapture();
				Point point = SafeSecurityHelper.ClientToScreen(this, e.MouseDevice.GetPosition(this));
				base.RaiseEvent(new DragCompletedEventArgs(point.X - this._originScreenCoordPosition.X, point.Y - this._originScreenCoordPosition.Y, false));
			}
			base.OnMouseLeftButtonUp(e);
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x001B1D1C File Offset: 0x001AFF1C
		private static void OnLostMouseCapture(object sender, MouseEventArgs e)
		{
			Thumb thumb = (Thumb)sender;
			if (Mouse.Captured != thumb)
			{
				thumb.CancelDrag();
			}
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.MouseMove" /> event. </summary>
		/// <param name="e">The event data.</param>
		// Token: 0x060060C2 RID: 24770 RVA: 0x001B1D40 File Offset: 0x001AFF40
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (this.IsDragging)
			{
				if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
				{
					Point position = e.GetPosition(this);
					Point point = SafeSecurityHelper.ClientToScreen(this, position);
					if (point != this._previousScreenCoordPosition)
					{
						this._previousScreenCoordPosition = point;
						e.Handled = true;
						base.RaiseEvent(new DragDeltaEventArgs(position.X - this._originThumbPoint.X, position.Y - this._originThumbPoint.Y));
						return;
					}
				}
				else
				{
					if (e.MouseDevice.Captured == this)
					{
						base.ReleaseMouseCapture();
					}
					base.ClearValue(Thumb.IsDraggingPropertyKey);
					this._originThumbPoint.X = 0.0;
					this._originThumbPoint.Y = 0.0;
				}
			}
		}

		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x060060C3 RID: 24771 RVA: 0x0003BBDF File Offset: 0x00039DDF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x060060C4 RID: 24772 RVA: 0x001B1E15 File Offset: 0x001B0015
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Thumb._dType;
			}
		}

		// Token: 0x04003112 RID: 12562
		private static readonly DependencyPropertyKey IsDraggingPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.Thumb.IsDragging" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.Thumb.IsDragging" /> dependency property.</returns>
		// Token: 0x04003113 RID: 12563
		public static readonly DependencyProperty IsDraggingProperty;

		// Token: 0x04003114 RID: 12564
		private Point _originThumbPoint;

		// Token: 0x04003115 RID: 12565
		private Point _originScreenCoordPosition;

		// Token: 0x04003116 RID: 12566
		private Point _previousScreenCoordPosition;

		// Token: 0x04003117 RID: 12567
		private static DependencyObjectType _dType;
	}
}
