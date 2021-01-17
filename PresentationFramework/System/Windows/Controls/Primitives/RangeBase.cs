using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an element that has a value within a specific range. </summary>
	// Token: 0x0200059F RID: 1439
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	public abstract class RangeBase : Control
	{
		// Token: 0x06005F3A RID: 24378 RVA: 0x001AB424 File Offset: 0x001A9624
		static RangeBase()
		{
			RangeBase.ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<double>), typeof(RangeBase));
			RangeBase.MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(RangeBase.OnMinimumChanged)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(1.0, new PropertyChangedCallback(RangeBase.OnMaximumChanged), new CoerceValueCallback(RangeBase.CoerceMaximum)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(RangeBase.OnValueChanged), new CoerceValueCallback(RangeBase.ConstrainToRange)), new ValidateValueCallback(RangeBase.IsValidDoubleValue));
			RangeBase.LargeChangeProperty = DependencyProperty.Register("LargeChange", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(1.0), new ValidateValueCallback(RangeBase.IsValidChange));
			RangeBase.SmallChangeProperty = DependencyProperty.Register("SmallChange", typeof(double), typeof(RangeBase), new FrameworkPropertyMetadata(0.1), new ValidateValueCallback(RangeBase.IsValidChange));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(RangeBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
			UIElement.IsMouseOverPropertyKey.OverrideMetadata(typeof(RangeBase), new UIPropertyMetadata(new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));
		}

		/// <summary>Occurs when the range value changes. </summary>
		// Token: 0x1400011A RID: 282
		// (add) Token: 0x06005F3C RID: 24380 RVA: 0x001AB62A File Offset: 0x001A982A
		// (remove) Token: 0x06005F3D RID: 24381 RVA: 0x001AB638 File Offset: 0x001A9838
		[Category("Behavior")]
		public event RoutedPropertyChangedEventHandler<double> ValueChanged
		{
			add
			{
				base.AddHandler(RangeBase.ValueChangedEvent, value);
			}
			remove
			{
				base.RemoveHandler(RangeBase.ValueChangedEvent, value);
			}
		}

		/// <summary>Gets or sets the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element.  </summary>
		/// <returns>
		///     <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element. The default is 0.</returns>
		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x06005F3E RID: 24382 RVA: 0x001AB646 File Offset: 0x001A9846
		// (set) Token: 0x06005F3F RID: 24383 RVA: 0x001AB658 File Offset: 0x001A9858
		[Bindable(true)]
		[Category("Behavior")]
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(RangeBase.MinimumProperty);
			}
			set
			{
				base.SetValue(RangeBase.MinimumProperty, value);
			}
		}

		// Token: 0x06005F40 RID: 24384 RVA: 0x001AB66C File Offset: 0x001A986C
		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseMinimumPropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.CoerceValue(RangeBase.MaximumProperty);
			rangeBase.CoerceValue(RangeBase.ValueProperty);
			rangeBase.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property changes. </summary>
		/// <param name="oldMinimum">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		/// <param name="newMinimum">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> property.</param>
		// Token: 0x06005F41 RID: 24385 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
		{
		}

		// Token: 0x06005F42 RID: 24386 RVA: 0x001AB6E4 File Offset: 0x001A98E4
		private static object CoerceMaximum(DependencyObject d, object value)
		{
			RangeBase rangeBase = (RangeBase)d;
			double minimum = rangeBase.Minimum;
			if ((double)value < minimum)
			{
				return minimum;
			}
			return value;
		}

		/// <summary>Gets or sets the highest possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element.  </summary>
		/// <returns>The highest possible <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the range element. The default is 1.</returns>
		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x06005F43 RID: 24387 RVA: 0x001AB710 File Offset: 0x001A9910
		// (set) Token: 0x06005F44 RID: 24388 RVA: 0x001AB722 File Offset: 0x001A9922
		[Bindable(true)]
		[Category("Behavior")]
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(RangeBase.MaximumProperty);
			}
			set
			{
				base.SetValue(RangeBase.MaximumProperty, value);
			}
		}

		// Token: 0x06005F45 RID: 24389 RVA: 0x001AB738 File Offset: 0x001A9938
		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseMaximumPropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.CoerceValue(RangeBase.ValueProperty);
			rangeBase.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property changes. </summary>
		/// <param name="oldMaximum">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		/// <param name="newMaximum">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> property.</param>
		// Token: 0x06005F46 RID: 24390 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
		{
		}

		// Token: 0x06005F47 RID: 24391 RVA: 0x001AB7A4 File Offset: 0x001A99A4
		internal static object ConstrainToRange(DependencyObject d, object value)
		{
			RangeBase rangeBase = (RangeBase)d;
			double minimum = rangeBase.Minimum;
			double num = (double)value;
			if (num < minimum)
			{
				return minimum;
			}
			double maximum = rangeBase.Maximum;
			if (num > maximum)
			{
				return maximum;
			}
			return value;
		}

		/// <summary>Gets or sets the current magnitude of the range control.  </summary>
		/// <returns>The current magnitude of the range control. The default is 0.</returns>
		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x06005F48 RID: 24392 RVA: 0x001AB7E4 File Offset: 0x001A99E4
		// (set) Token: 0x06005F49 RID: 24393 RVA: 0x001AB7F6 File Offset: 0x001A99F6
		[Bindable(true)]
		[Category("Behavior")]
		public double Value
		{
			get
			{
				return (double)base.GetValue(RangeBase.ValueProperty);
			}
			set
			{
				base.SetValue(RangeBase.ValueProperty, value);
			}
		}

		// Token: 0x06005F4A RID: 24394 RVA: 0x001AB80C File Offset: 0x001A9A0C
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RangeBase rangeBase = (RangeBase)d;
			RangeBaseAutomationPeer rangeBaseAutomationPeer = UIElementAutomationPeer.FromElement(rangeBase) as RangeBaseAutomationPeer;
			if (rangeBaseAutomationPeer != null)
			{
				rangeBaseAutomationPeer.RaiseValuePropertyChangedEvent((double)e.OldValue, (double)e.NewValue);
			}
			rangeBase.OnValueChanged((double)e.OldValue, (double)e.NewValue);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.Primitives.RangeBase.ValueChanged" /> routed event. </summary>
		/// <param name="oldValue">Old value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property</param>
		/// <param name="newValue">New value of the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> property</param>
		// Token: 0x06005F4B RID: 24395 RVA: 0x001AB86C File Offset: 0x001A9A6C
		protected virtual void OnValueChanged(double oldValue, double newValue)
		{
			base.RaiseEvent(new RoutedPropertyChangedEventArgs<double>(oldValue, newValue)
			{
				RoutedEvent = RangeBase.ValueChangedEvent
			});
		}

		// Token: 0x06005F4C RID: 24396 RVA: 0x001AB894 File Offset: 0x001A9A94
		private static bool IsValidDoubleValue(object value)
		{
			double num = (double)value;
			return !DoubleUtil.IsNaN(num) && !double.IsInfinity(num);
		}

		// Token: 0x06005F4D RID: 24397 RVA: 0x001AB8BC File Offset: 0x001A9ABC
		private static bool IsValidChange(object value)
		{
			double num = (double)value;
			return RangeBase.IsValidDoubleValue(value) && num >= 0.0;
		}

		/// <summary>Gets or sets a value to be added to or subtracted from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> control.  </summary>
		/// <returns>
		///     <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to add to or subtract from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> element. The default is 1.</returns>
		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x06005F4E RID: 24398 RVA: 0x001AB8E9 File Offset: 0x001A9AE9
		// (set) Token: 0x06005F4F RID: 24399 RVA: 0x001AB8FB File Offset: 0x001A9AFB
		[Bindable(true)]
		[Category("Behavior")]
		public double LargeChange
		{
			get
			{
				return (double)base.GetValue(RangeBase.LargeChangeProperty);
			}
			set
			{
				base.SetValue(RangeBase.LargeChangeProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to be added to or subtracted from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> control.  </summary>
		/// <returns>
		///     <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> to add to or subtract from the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> of the <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> element. The default is 0.1.</returns>
		// Token: 0x170016FB RID: 5883
		// (get) Token: 0x06005F50 RID: 24400 RVA: 0x001AB90E File Offset: 0x001A9B0E
		// (set) Token: 0x06005F51 RID: 24401 RVA: 0x001AB920 File Offset: 0x001A9B20
		[Bindable(true)]
		[Category("Behavior")]
		public double SmallChange
		{
			get
			{
				return (double)base.GetValue(RangeBase.SmallChangeProperty);
			}
			set
			{
				base.SetValue(RangeBase.SmallChangeProperty, value);
			}
		}

		// Token: 0x06005F52 RID: 24402 RVA: 0x001AB934 File Offset: 0x001A9B34
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else if (base.IsMouseOver)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"MouseOver",
					"Normal"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", useTransitions);
			}
			if (base.IsKeyboardFocused)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Focused",
					"Unfocused"
				});
			}
			else
			{
				VisualStateManager.GoToState(this, "Unfocused", useTransitions);
			}
			base.ChangeVisualState(useTransitions);
		}

		/// <summary>Provides a string representation of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> object. </summary>
		/// <returns>Returns the string representation of a <see cref="T:System.Windows.Controls.Primitives.RangeBase" /> object.</returns>
		// Token: 0x06005F53 RID: 24403 RVA: 0x001AB9D8 File Offset: 0x001A9BD8
		public override string ToString()
		{
			string text = base.GetType().ToString();
			double min = double.NaN;
			double max = double.NaN;
			double val = double.NaN;
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				min = this.Minimum;
				max = this.Maximum;
				val = this.Value;
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					min = this.Minimum;
					max = this.Maximum;
					val = this.Value;
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_RangeBase", new object[]
				{
					text,
					min,
					max,
					val
				});
			}
			return text;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Minimum" /> dependency property.</returns>
		// Token: 0x04003095 RID: 12437
		public static readonly DependencyProperty MinimumProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Maximum" /> dependency property.</returns>
		// Token: 0x04003096 RID: 12438
		public static readonly DependencyProperty MaximumProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.Value" /> dependency property.</returns>
		// Token: 0x04003097 RID: 12439
		public static readonly DependencyProperty ValueProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.LargeChange" /> dependency property.</returns>
		// Token: 0x04003098 RID: 12440
		public static readonly DependencyProperty LargeChangeProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.RangeBase.SmallChange" /> dependency property.</returns>
		// Token: 0x04003099 RID: 12441
		public static readonly DependencyProperty SmallChangeProperty;
	}
}
