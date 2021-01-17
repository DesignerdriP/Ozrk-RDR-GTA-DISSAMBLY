using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DatePicker" /> types to UI Automation.</summary>
	// Token: 0x020002A9 RID: 681
	public sealed class DatePickerAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IValueProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DatePickerAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x0600260C RID: 9740 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public DatePickerAutomationPeer(DatePicker owner) : base(owner)
		{
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x0600260D RID: 9741 RVA: 0x000B6019 File Offset: 0x000B4219
		private DatePicker OwningDatePicker
		{
			get
			{
				return base.Owner as DatePicker;
			}
		}

		/// <summary>Returns the object that supports the specified control pattern of the element that is associated with this automation peer.</summary>
		/// <param name="patternInterface">An enumeration value that specifies the control pattern.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.ExpandCollapse" /> or <see cref="F:System.Windows.Automation.Peers.PatternInterface.Value" />, this method returns a <see langword="this" /> pointer; otherwise, this method returns <see langword="null" />.</returns>
		// Token: 0x0600260E RID: 9742 RVA: 0x000B6026 File Offset: 0x000B4226
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.ExpandCollapse || patternInterface == PatternInterface.Value)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000B603C File Offset: 0x000B423C
		protected override void SetFocusCore()
		{
			DatePicker owningDatePicker = this.OwningDatePicker;
			if (owningDatePicker.Focusable)
			{
				if (!owningDatePicker.Focus())
				{
					TextBox textBox = owningDatePicker.TextBox;
					if (textBox == null || !textBox.IsKeyboardFocused)
					{
						throw new InvalidOperationException(SR.Get("SetFocusFailed"));
					}
				}
				return;
			}
			throw new InvalidOperationException(SR.Get("SetFocusFailed"));
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x00094967 File Offset: 0x00092B67
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000B6094 File Offset: 0x000B4294
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> childrenCore = base.GetChildrenCore();
			if (this.OwningDatePicker.IsDropDownOpen && this.OwningDatePicker.Calendar != null)
			{
				CalendarAutomationPeer calendarAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningDatePicker.Calendar) as CalendarAutomationPeer;
				if (calendarAutomationPeer != null)
				{
					childrenCore.Add(calendarAutomationPeer);
				}
			}
			return childrenCore;
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000B3324 File Offset: 0x000B1524
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000B60E3 File Offset: 0x000B42E3
		protected override string GetLocalizedControlTypeCore()
		{
			return SR.Get("DatePickerAutomationPeer_LocalizedControlType");
		}

		/// <summary>Gets the state (expanded or collapsed) of the control.</summary>
		/// <returns>The state (expanded or collapsed) of the control.</returns>
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x000B60EF File Offset: 0x000B42EF
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				if (this.OwningDatePicker.IsDropDownOpen)
				{
					return ExpandCollapseState.Expanded;
				}
				return ExpandCollapseState.Collapsed;
			}
		}

		/// <summary>Hides all nodes, controls, or content that are descendants of the control.</summary>
		// Token: 0x06002615 RID: 9749 RVA: 0x000B6101 File Offset: 0x000B4301
		void IExpandCollapseProvider.Collapse()
		{
			this.OwningDatePicker.IsDropDownOpen = false;
		}

		/// <summary>Displays all child nodes, controls, or content of the control.</summary>
		// Token: 0x06002616 RID: 9750 RVA: 0x000B610F File Offset: 0x000B430F
		void IExpandCollapseProvider.Expand()
		{
			this.OwningDatePicker.IsDropDownOpen = true;
		}

		/// <summary>Gets a value that specifies whether the value of a control is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> if the value is read-only; <see langword="false" /> if it can be modified. </returns>
		// Token: 0x1700096F RID: 2415
		// (get) Token: 0x06002617 RID: 9751 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IValueProvider.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets the value of the control.</summary>
		/// <returns>A string that represents the value of the control. </returns>
		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x000B611D File Offset: 0x000B431D
		string IValueProvider.Value
		{
			get
			{
				return this.OwningDatePicker.ToString();
			}
		}

		/// <summary>Sets the value of a control.</summary>
		/// <param name="value">The value to set. The provider is responsible for converting the value to the appropriate data type.</param>
		// Token: 0x06002619 RID: 9753 RVA: 0x000B612A File Offset: 0x000B432A
		void IValueProvider.SetValue(string value)
		{
			this.OwningDatePicker.Text = value;
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000B3EB9 File Offset: 0x000B20B9
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseValuePropertyChangedEvent(string oldValue, string newValue)
		{
			if (oldValue != newValue)
			{
				base.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, oldValue, newValue);
			}
		}
	}
}
