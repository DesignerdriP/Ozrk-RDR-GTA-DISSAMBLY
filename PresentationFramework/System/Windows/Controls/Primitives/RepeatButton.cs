using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Input;
using System.Windows.Threading;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents a control that raises its <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event repeatedly from the time it is pressed until it is released. </summary>
	// Token: 0x020005A1 RID: 1441
	public class RepeatButton : ButtonBase
	{
		// Token: 0x06005F54 RID: 24404 RVA: 0x001ABAD8 File Offset: 0x001A9CD8
		static RepeatButton()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(RepeatButton), new FrameworkPropertyMetadata(typeof(RepeatButton)));
			RepeatButton._dType = DependencyObjectType.FromSystemTypeInternal(typeof(RepeatButton));
			ButtonBase.ClickModeProperty.OverrideMetadata(typeof(RepeatButton), new FrameworkPropertyMetadata(ClickMode.Press));
		}

		/// <summary>Gets or sets the amount of time, in milliseconds, the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> waits while it is pressed before it starts repeating. The value must be non-negative.  </summary>
		/// <returns>The amount of time, in milliseconds, the <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> waits while it is pressed before it starts repeating. The default is the value of <see cref="P:System.Windows.SystemParameters.KeyboardDelay" />.</returns>
		// Token: 0x170016FC RID: 5884
		// (get) Token: 0x06005F56 RID: 24406 RVA: 0x001ABBB7 File Offset: 0x001A9DB7
		// (set) Token: 0x06005F57 RID: 24407 RVA: 0x001ABBC9 File Offset: 0x001A9DC9
		[Bindable(true)]
		[Category("Behavior")]
		public int Delay
		{
			get
			{
				return (int)base.GetValue(RepeatButton.DelayProperty);
			}
			set
			{
				base.SetValue(RepeatButton.DelayProperty, value);
			}
		}

		/// <summary>Gets or sets the amount of time, in milliseconds, between repeats once repeating starts. The value must be non-negative.  </summary>
		/// <returns>The amount of time, in milliseconds, between repeats after repeating starts. The default is the value of <see cref="P:System.Windows.SystemParameters.KeyboardSpeed" />.</returns>
		// Token: 0x170016FD RID: 5885
		// (get) Token: 0x06005F58 RID: 24408 RVA: 0x001ABBDC File Offset: 0x001A9DDC
		// (set) Token: 0x06005F59 RID: 24409 RVA: 0x001ABBEE File Offset: 0x001A9DEE
		[Bindable(true)]
		[Category("Behavior")]
		public int Interval
		{
			get
			{
				return (int)base.GetValue(RepeatButton.IntervalProperty);
			}
			set
			{
				base.SetValue(RepeatButton.IntervalProperty, value);
			}
		}

		// Token: 0x06005F5A RID: 24410 RVA: 0x0015A2F7 File Offset: 0x001584F7
		private static bool IsDelayValid(object value)
		{
			return (int)value >= 0;
		}

		// Token: 0x06005F5B RID: 24411 RVA: 0x0015A305 File Offset: 0x00158505
		private static bool IsIntervalValid(object value)
		{
			return (int)value > 0;
		}

		// Token: 0x06005F5C RID: 24412 RVA: 0x001ABC04 File Offset: 0x001A9E04
		private void StartTimer()
		{
			if (this._timer == null)
			{
				this._timer = new DispatcherTimer();
				this._timer.Tick += this.OnTimeout;
			}
			else if (this._timer.IsEnabled)
			{
				return;
			}
			this._timer.Interval = TimeSpan.FromMilliseconds((double)this.Delay);
			this._timer.Start();
		}

		// Token: 0x06005F5D RID: 24413 RVA: 0x001ABC6D File Offset: 0x001A9E6D
		private void StopTimer()
		{
			if (this._timer != null)
			{
				this._timer.Stop();
			}
		}

		// Token: 0x06005F5E RID: 24414 RVA: 0x001ABC84 File Offset: 0x001A9E84
		private void OnTimeout(object sender, EventArgs e)
		{
			TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)this.Interval);
			if (this._timer.Interval != timeSpan)
			{
				this._timer.Interval = timeSpan;
			}
			if (base.IsPressed)
			{
				this.OnClick();
			}
		}

		// Token: 0x06005F5F RID: 24415 RVA: 0x001ABCCC File Offset: 0x001A9ECC
		internal static int GetKeyboardDelay()
		{
			int num = SystemParameters.KeyboardDelay;
			if (num < 0 || num > 3)
			{
				num = 0;
			}
			return (num + 1) * 250;
		}

		// Token: 0x06005F60 RID: 24416 RVA: 0x001ABCF4 File Offset: 0x001A9EF4
		internal static int GetKeyboardSpeed()
		{
			int num = SystemParameters.KeyboardSpeed;
			if (num < 0 || num > 31)
			{
				num = 31;
			}
			return (31 - num) * 367 / 31 + 33;
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.RepeatButtonAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x06005F61 RID: 24417 RVA: 0x001ABD23 File Offset: 0x001A9F23
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new RepeatButtonAutomationPeer(this);
		}

		/// <summary>Raises an automation event and calls the base method to raise the <see cref="E:System.Windows.Controls.Primitives.ButtonBase.Click" /> event. </summary>
		// Token: 0x06005F62 RID: 24418 RVA: 0x001ABD2C File Offset: 0x001A9F2C
		protected override void OnClick()
		{
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this);
				if (automationPeer != null)
				{
					automationPeer.RaiseAutomationEvent(AutomationEvents.InvokePatternOnInvoked);
				}
			}
			base.OnClick();
		}

		/// <summary>Responds to the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event. </summary>
		/// <param name="e">The event data for a <see cref="E:System.Windows.UIElement.MouseLeftButtonDown" /> event.</param>
		// Token: 0x06005F63 RID: 24419 RVA: 0x001ABD58 File Offset: 0x001A9F58
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			if (base.IsPressed && base.ClickMode != ClickMode.Hover)
			{
				this.StartTimer();
			}
		}

		/// <summary>Responds to the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp" /> event. </summary>
		/// <param name="e">The event data for a <see cref="E:System.Windows.ContentElement.MouseLeftButtonUp" /> event.</param>
		// Token: 0x06005F64 RID: 24420 RVA: 0x001ABD78 File Offset: 0x001A9F78
		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonUp(e);
			if (base.ClickMode != ClickMode.Hover)
			{
				this.StopTimer();
			}
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.Primitives.RepeatButton" /> loses mouse capture. </summary>
		/// <param name="e">The event data for a <see cref="E:System.Windows.UIElement.LostMouseCapture" /> event.</param>
		// Token: 0x06005F65 RID: 24421 RVA: 0x001ABD90 File Offset: 0x001A9F90
		protected override void OnLostMouseCapture(MouseEventArgs e)
		{
			base.OnLostMouseCapture(e);
			this.StopTimer();
		}

		/// <summary>Reports when the mouse enters an element. </summary>
		/// <param name="e">The event data for a <see cref="E:System.Windows.UIElement.MouseEnter" /> event.</param>
		// Token: 0x06005F66 RID: 24422 RVA: 0x001ABD9F File Offset: 0x001A9F9F
		protected override void OnMouseEnter(MouseEventArgs e)
		{
			base.OnMouseEnter(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		/// <summary>Reports when the mouse leaves an element. </summary>
		/// <param name="e">The event data for a <see cref="E:System.Windows.UIElement.MouseLeave" /> event.</param>
		// Token: 0x06005F67 RID: 24423 RVA: 0x001ABDB7 File Offset: 0x001A9FB7
		protected override void OnMouseLeave(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			if (this.HandleIsMouseOverChanged())
			{
				e.Handled = true;
			}
		}

		// Token: 0x06005F68 RID: 24424 RVA: 0x001ABDCF File Offset: 0x001A9FCF
		private bool HandleIsMouseOverChanged()
		{
			if (base.ClickMode == ClickMode.Hover)
			{
				if (base.IsMouseOver)
				{
					this.StartTimer();
				}
				else
				{
					this.StopTimer();
				}
				return true;
			}
			return false;
		}

		/// <summary>Responds to the <see cref="E:System.Windows.UIElement.KeyDown" /> event. </summary>
		/// <param name="e">The event data for a <see cref="E:System.Windows.UIElement.KeyDown" /> event.</param>
		// Token: 0x06005F69 RID: 24425 RVA: 0x001ABDF3 File Offset: 0x001A9FF3
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (e.Key == Key.Space && base.ClickMode != ClickMode.Hover)
			{
				this.StartTimer();
			}
		}

		/// <summary>Responds to the <see cref="E:System.Windows.UIElement.KeyUp" /> event. </summary>
		/// <param name="e">The event data for a <see cref="E:System.Windows.UIElement.KeyUp" /> event.</param>
		// Token: 0x06005F6A RID: 24426 RVA: 0x001ABE15 File Offset: 0x001AA015
		protected override void OnKeyUp(KeyEventArgs e)
		{
			if (e.Key == Key.Space && base.ClickMode != ClickMode.Hover)
			{
				this.StopTimer();
			}
			base.OnKeyUp(e);
		}

		// Token: 0x170016FE RID: 5886
		// (get) Token: 0x06005F6B RID: 24427 RVA: 0x000961BF File Offset: 0x000943BF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x06005F6C RID: 24428 RVA: 0x001ABE37 File Offset: 0x001AA037
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return RepeatButton._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RepeatButton.Delay" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RepeatButton.Delay" /> dependency property.</returns>
		// Token: 0x0400309F RID: 12447
		public static readonly DependencyProperty DelayProperty = DependencyProperty.Register("Delay", typeof(int), typeof(RepeatButton), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardDelay()), new ValidateValueCallback(RepeatButton.IsDelayValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RepeatButton.Interval" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RepeatButton.Interval" /> dependency property.</returns>
		// Token: 0x040030A0 RID: 12448
		public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(RepeatButton), new FrameworkPropertyMetadata(RepeatButton.GetKeyboardSpeed()), new ValidateValueCallback(RepeatButton.IsIntervalValid));

		// Token: 0x040030A1 RID: 12449
		private DispatcherTimer _timer;

		// Token: 0x040030A2 RID: 12450
		private static DependencyObjectType _dType;
	}
}
