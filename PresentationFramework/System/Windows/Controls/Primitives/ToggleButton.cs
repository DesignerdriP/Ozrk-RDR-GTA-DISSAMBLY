using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Threading;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Base class for controls that can switch states, such as <see cref="T:System.Windows.Controls.CheckBox" />. </summary>
	// Token: 0x020005B1 RID: 1457
	[DefaultEvent("Checked")]
	public class ToggleButton : ButtonBase
	{
		// Token: 0x060060E1 RID: 24801 RVA: 0x001B2D88 File Offset: 0x001B0F88
		static ToggleButton()
		{
			ToggleButton.CheckedEvent = EventManager.RegisterRoutedEvent("Checked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButton));
			ToggleButton.UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButton));
			ToggleButton.IndeterminateEvent = EventManager.RegisterRoutedEvent("Indeterminate", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ToggleButton));
			ToggleButton.IsCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool?), typeof(ToggleButton), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(ToggleButton.OnIsCheckedChanged)));
			ToggleButton.IsThreeStateProperty = DependencyProperty.Register("IsThreeState", typeof(bool), typeof(ToggleButton), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ToggleButton), new FrameworkPropertyMetadata(typeof(ToggleButton)));
			ToggleButton._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ToggleButton));
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> is checked.</summary>
		// Token: 0x14000122 RID: 290
		// (add) Token: 0x060060E3 RID: 24803 RVA: 0x001B2EA3 File Offset: 0x001B10A3
		// (remove) Token: 0x060060E4 RID: 24804 RVA: 0x001B2EB1 File Offset: 0x001B10B1
		[Category("Behavior")]
		public event RoutedEventHandler Checked
		{
			add
			{
				base.AddHandler(ToggleButton.CheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ToggleButton.CheckedEvent, value);
			}
		}

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> is unchecked.</summary>
		// Token: 0x14000123 RID: 291
		// (add) Token: 0x060060E5 RID: 24805 RVA: 0x001B2EBF File Offset: 0x001B10BF
		// (remove) Token: 0x060060E6 RID: 24806 RVA: 0x001B2ECD File Offset: 0x001B10CD
		[Category("Behavior")]
		public event RoutedEventHandler Unchecked
		{
			add
			{
				base.AddHandler(ToggleButton.UncheckedEvent, value);
			}
			remove
			{
				base.RemoveHandler(ToggleButton.UncheckedEvent, value);
			}
		}

		/// <summary>Occurs when the state of a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> is neither on nor off.</summary>
		// Token: 0x14000124 RID: 292
		// (add) Token: 0x060060E7 RID: 24807 RVA: 0x001B2EDB File Offset: 0x001B10DB
		// (remove) Token: 0x060060E8 RID: 24808 RVA: 0x001B2EE9 File Offset: 0x001B10E9
		[Category("Behavior")]
		public event RoutedEventHandler Indeterminate
		{
			add
			{
				base.AddHandler(ToggleButton.IndeterminateEvent, value);
			}
			remove
			{
				base.RemoveHandler(ToggleButton.IndeterminateEvent, value);
			}
		}

		/// <summary>Gets or sets whether the <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> is checked.   </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> is checked; false if the <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> is unchecked; otherwise <see langword="null" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001751 RID: 5969
		// (get) Token: 0x060060E9 RID: 24809 RVA: 0x001B2EF8 File Offset: 0x001B10F8
		// (set) Token: 0x060060EA RID: 24810 RVA: 0x001B2F29 File Offset: 0x001B1129
		[Category("Appearance")]
		[TypeConverter(typeof(NullableBoolConverter))]
		[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
		public bool? IsChecked
		{
			get
			{
				object value = base.GetValue(ToggleButton.IsCheckedProperty);
				if (value == null)
				{
					return null;
				}
				return new bool?((bool)value);
			}
			set
			{
				base.SetValue(ToggleButton.IsCheckedProperty, (value != null) ? BooleanBoxes.Box(value.Value) : null);
			}
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x001B2F4E File Offset: 0x001B114E
		private static object OnGetIsChecked(DependencyObject d)
		{
			return ((ToggleButton)d).IsChecked;
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x001B2F60 File Offset: 0x001B1160
		private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ToggleButton toggleButton = (ToggleButton)d;
			bool? oldValue = (bool?)e.OldValue;
			bool? flag = (bool?)e.NewValue;
			ToggleButtonAutomationPeer toggleButtonAutomationPeer = UIElementAutomationPeer.FromElement(toggleButton) as ToggleButtonAutomationPeer;
			if (toggleButtonAutomationPeer != null)
			{
				toggleButtonAutomationPeer.RaiseToggleStatePropertyChangedEvent(oldValue, flag);
			}
			if (flag == true)
			{
				toggleButton.OnChecked(new RoutedEventArgs(ToggleButton.CheckedEvent));
			}
			else if (flag == false)
			{
				toggleButton.OnUnchecked(new RoutedEventArgs(ToggleButton.UncheckedEvent));
			}
			else
			{
				toggleButton.OnIndeterminate(new RoutedEventArgs(ToggleButton.IndeterminateEvent));
			}
			toggleButton.UpdateVisualState();
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> raises a <see cref="E:System.Windows.Controls.Primitives.ToggleButton.Checked" /> event.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.Primitives.ToggleButton.Checked" /> event.</param>
		// Token: 0x060060ED RID: 24813 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnChecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> raises an <see cref="E:System.Windows.Controls.Primitives.ToggleButton.Unchecked" /> event.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.Primitives.ToggleButton.Unchecked" /> event.</param>
		// Token: 0x060060EE RID: 24814 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnUnchecked(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Called when a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> raises an <see cref="E:System.Windows.Controls.Primitives.ToggleButton.Indeterminate" /> event.</summary>
		/// <param name="e">The event data for the <see cref="E:System.Windows.Controls.Primitives.ToggleButton.Indeterminate" /> event.</param>
		// Token: 0x060060EF RID: 24815 RVA: 0x00012CF1 File Offset: 0x00010EF1
		protected virtual void OnIndeterminate(RoutedEventArgs e)
		{
			base.RaiseEvent(e);
		}

		/// <summary>Determines whether the control supports two or three states.   </summary>
		/// <returns>
		///     <see langword="true" /> if the control supports three states; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001752 RID: 5970
		// (get) Token: 0x060060F0 RID: 24816 RVA: 0x001B3019 File Offset: 0x001B1219
		// (set) Token: 0x060060F1 RID: 24817 RVA: 0x001B302B File Offset: 0x001B122B
		[Bindable(true)]
		[Category("Behavior")]
		public bool IsThreeState
		{
			get
			{
				return (bool)base.GetValue(ToggleButton.IsThreeStateProperty);
			}
			set
			{
				base.SetValue(ToggleButton.IsThreeStateProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.ToggleButtonAutomationPeer" /> implementation for this control, as part of the WPF infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060060F2 RID: 24818 RVA: 0x001B303E File Offset: 0x001B123E
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new ToggleButtonAutomationPeer(this);
		}

		/// <summary>Called when a control is clicked by the mouse or the keyboard. </summary>
		// Token: 0x060060F3 RID: 24819 RVA: 0x001B3046 File Offset: 0x001B1246
		protected override void OnClick()
		{
			this.OnToggle();
			base.OnClick();
		}

		// Token: 0x060060F4 RID: 24820 RVA: 0x001B3054 File Offset: 0x001B1254
		internal override void ChangeVisualState(bool useTransitions)
		{
			base.ChangeVisualState(useTransitions);
			bool? isChecked = this.IsChecked;
			if (isChecked == true)
			{
				VisualStateManager.GoToState(this, "Checked", useTransitions);
				return;
			}
			if (isChecked == false)
			{
				VisualStateManager.GoToState(this, "Unchecked", useTransitions);
				return;
			}
			VisualStates.GoToState(this, useTransitions, new string[]
			{
				"Indeterminate",
				"Unchecked"
			});
		}

		/// <summary>Returns the string representation of a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> object. </summary>
		/// <returns>String representation of a <see cref="T:System.Windows.Controls.Primitives.ToggleButton" /> object.</returns>
		// Token: 0x060060F5 RID: 24821 RVA: 0x001B30DC File Offset: 0x001B12DC
		public override string ToString()
		{
			string text = base.GetType().ToString();
			string contentText = string.Empty;
			bool? isChecked = new bool?(false);
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				contentText = this.GetPlainText();
				isChecked = this.IsChecked;
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					contentText = this.GetPlainText();
					isChecked = this.IsChecked;
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_ToggleButton", new object[]
				{
					text,
					contentText,
					(isChecked != null) ? isChecked.Value.ToString() : "null"
				});
			}
			return text;
		}

		/// <summary>Called by the <see cref="M:System.Windows.Controls.Primitives.ToggleButton.OnClick" /> method to implement toggle behavior. </summary>
		// Token: 0x060060F6 RID: 24822 RVA: 0x001B31C0 File Offset: 0x001B13C0
		protected internal virtual void OnToggle()
		{
			bool? flag;
			if (this.IsChecked == true)
			{
				flag = (this.IsThreeState ? null : new bool?(false));
			}
			else
			{
				flag = new bool?(this.IsChecked != null);
			}
			base.SetCurrentValueInternal(ToggleButton.IsCheckedProperty, flag);
		}

		// Token: 0x17001753 RID: 5971
		// (get) Token: 0x060060F7 RID: 24823 RVA: 0x001B322E File Offset: 0x001B142E
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ToggleButton._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ToggleButton.IsChecked" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ToggleButton.IsChecked" /> dependency property.</returns>
		// Token: 0x04003130 RID: 12592
		public static readonly DependencyProperty IsCheckedProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.Primitives.ToggleButton.IsThreeState" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.Primitives.ToggleButton.IsThreeState" /> dependency property.</returns>
		// Token: 0x04003131 RID: 12593
		public static readonly DependencyProperty IsThreeStateProperty;

		// Token: 0x04003132 RID: 12594
		private static DependencyObjectType _dType;
	}
}
