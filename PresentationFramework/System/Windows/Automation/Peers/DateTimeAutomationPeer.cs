using System;
using System.Collections.Generic;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.CalendarDayButton" /> and <see cref="T:System.Windows.Controls.Primitives.CalendarButton" /> types to UI Automation. </summary>
	// Token: 0x020002AA RID: 682
	public sealed class DateTimeAutomationPeer : AutomationPeer, IGridItemProvider, ISelectionItemProvider, ITableItemProvider, IInvokeProvider, IVirtualizedItemProvider
	{
		// Token: 0x0600261B RID: 9755 RVA: 0x000B6138 File Offset: 0x000B4338
		internal DateTimeAutomationPeer(DateTime date, Calendar owningCalendar, CalendarMode buttonMode)
		{
			if (owningCalendar == null)
			{
				throw new ArgumentNullException("owningCalendar");
			}
			this.Date = date;
			this.ButtonMode = buttonMode;
			this.OwningCalendar = owningCalendar;
		}

		// Token: 0x17000971 RID: 2417
		// (get) Token: 0x0600261C RID: 9756 RVA: 0x000B51A9 File Offset: 0x000B33A9
		// (set) Token: 0x0600261D RID: 9757 RVA: 0x000B6164 File Offset: 0x000B4364
		internal override bool AncestorsInvalid
		{
			get
			{
				return base.AncestorsInvalid;
			}
			set
			{
				base.AncestorsInvalid = value;
				if (value)
				{
					return;
				}
				AutomationPeer wrapperPeer = this.WrapperPeer;
				if (wrapperPeer != null)
				{
					wrapperPeer.AncestorsInvalid = false;
				}
			}
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x000B618D File Offset: 0x000B438D
		// (set) Token: 0x0600261F RID: 9759 RVA: 0x000B6195 File Offset: 0x000B4395
		private Calendar OwningCalendar { get; set; }

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000B619E File Offset: 0x000B439E
		// (set) Token: 0x06002621 RID: 9761 RVA: 0x000B61A6 File Offset: 0x000B43A6
		internal DateTime Date { get; private set; }

		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x000B61AF File Offset: 0x000B43AF
		// (set) Token: 0x06002623 RID: 9763 RVA: 0x000B61B7 File Offset: 0x000B43B7
		internal CalendarMode ButtonMode { get; private set; }

		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x000B61C0 File Offset: 0x000B43C0
		internal bool IsDayButton
		{
			get
			{
				return this.ButtonMode == CalendarMode.Month;
			}
		}

		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002625 RID: 9765 RVA: 0x000B61CC File Offset: 0x000B43CC
		private IRawElementProviderSimple OwningCalendarProvider
		{
			get
			{
				if (this.OwningCalendar != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar);
					if (automationPeer != null)
					{
						return base.ProviderFromPeer(automationPeer);
					}
				}
				return null;
			}
		}

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x000B61FC File Offset: 0x000B43FC
		internal Button OwningButton
		{
			get
			{
				if (this.OwningCalendar.DisplayMode != this.ButtonMode)
				{
					return null;
				}
				if (this.IsDayButton)
				{
					CalendarItem monthControl = this.OwningCalendar.MonthControl;
					if (monthControl == null)
					{
						return null;
					}
					return monthControl.GetCalendarDayButton(this.Date);
				}
				else
				{
					CalendarItem monthControl2 = this.OwningCalendar.MonthControl;
					if (monthControl2 == null)
					{
						return null;
					}
					return monthControl2.GetCalendarButton(this.Date, this.ButtonMode);
				}
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x000B6268 File Offset: 0x000B4468
		internal FrameworkElementAutomationPeer WrapperPeer
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton != null)
				{
					return UIElementAutomationPeer.CreatePeerForElement(owningButton) as FrameworkElementAutomationPeer;
				}
				return null;
			}
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000B628C File Offset: 0x000B448C
		protected override string GetAcceleratorKeyCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAcceleratorKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000B62B8 File Offset: 0x000B44B8
		protected override string GetAccessKeyCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAccessKey();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000B62E4 File Offset: 0x000B44E4
		protected override string GetAutomationIdCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetAutomationId();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000B6310 File Offset: 0x000B4510
		protected override Rect GetBoundingRectangleCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetBoundingRectangle();
			}
			this.ThrowElementNotAvailableException();
			return default(Rect);
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000B6340 File Offset: 0x000B4540
		protected override List<AutomationPeer> GetChildrenCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetChildren();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000B6368 File Offset: 0x000B4568
		protected override string GetClassNameCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClassName();
			}
			if (!this.IsDayButton)
			{
				return "CalendarButton";
			}
			return "CalendarDayButton";
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000B639C File Offset: 0x000B459C
		protected override Point GetClickablePointCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetClickablePoint();
			}
			this.ThrowElementNotAvailableException();
			return new Point(double.NaN, double.NaN);
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x000B63D8 File Offset: 0x000B45D8
		protected override string GetHelpTextCore()
		{
			string text = DateTimeHelper.ToLongDateString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
			if (this.IsDayButton && this.OwningCalendar.BlackoutDates.Contains(this.Date))
			{
				return string.Format(DateTimeHelper.GetCurrentDateFormat(), SR.Get("CalendarAutomationPeer_BlackoutDayHelpText"), new object[]
				{
					text
				});
			}
			return text;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x000B6444 File Offset: 0x000B4644
		protected override string GetItemStatusCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetItemStatus();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x000B6470 File Offset: 0x000B4670
		protected override string GetItemTypeCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetItemType();
			}
			this.ThrowElementNotAvailableException();
			return string.Empty;
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x000B649C File Offset: 0x000B469C
		protected override AutomationPeer GetLabeledByCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLabeledBy();
			}
			this.ThrowElementNotAvailableException();
			return null;
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000B64C4 File Offset: 0x000B46C4
		protected override AutomationLiveSetting GetLiveSettingCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetLiveSetting();
			}
			this.ThrowElementNotAvailableException();
			return AutomationLiveSetting.Off;
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000B64E9 File Offset: 0x000B46E9
		protected override string GetLocalizedControlTypeCore()
		{
			if (!this.IsDayButton)
			{
				return SR.Get("CalendarAutomationPeer_CalendarButtonLocalizedControlType");
			}
			return SR.Get("CalendarAutomationPeer_DayButtonLocalizedControlType");
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x000B6508 File Offset: 0x000B4708
		protected override string GetNameCore()
		{
			string result = "";
			switch (this.ButtonMode)
			{
			case CalendarMode.Month:
				result = DateTimeHelper.ToLongDateString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
				break;
			case CalendarMode.Year:
				result = DateTimeHelper.ToYearMonthPatternString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
				break;
			case CalendarMode.Decade:
				result = DateTimeHelper.ToYearString(new DateTime?(this.Date), DateTimeHelper.GetCulture(this.OwningCalendar));
				break;
			}
			return result;
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000B6590 File Offset: 0x000B4790
		protected override AutomationOrientation GetOrientationCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetOrientation();
			}
			this.ThrowElementNotAvailableException();
			return AutomationOrientation.None;
		}

		/// <summary>Gets the control pattern implementation for this <see cref="T:System.Windows.Automation.Peers.DateTimeAutomationPeer" />.</summary>
		/// <param name="patternInterface">The type of pattern implemented by the element to retrieve.</param>
		/// <returns>The object that implements the pattern interface, or <see langword="null" /> if the specified pattern interface is not implemented by this peer.</returns>
		// Token: 0x06002638 RID: 9784 RVA: 0x000B65B8 File Offset: 0x000B47B8
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			Button owningButton = this.OwningButton;
			if (patternInterface <= PatternInterface.GridItem)
			{
				if (patternInterface == PatternInterface.Invoke || patternInterface == PatternInterface.GridItem)
				{
					if (owningButton != null)
					{
						result = this;
					}
				}
			}
			else if (patternInterface != PatternInterface.SelectionItem)
			{
				if (patternInterface != PatternInterface.TableItem)
				{
					if (patternInterface == PatternInterface.VirtualizedItem)
					{
						if (VirtualizedItemPatternIdentifiers.Pattern != null)
						{
							if (owningButton == null)
							{
								result = this;
							}
							else if (!this.IsItemInAutomationTree())
							{
								return this;
							}
						}
					}
				}
				else if (this.IsDayButton && owningButton != null)
				{
					result = this;
				}
			}
			else
			{
				result = this;
			}
			return result;
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x000B6620 File Offset: 0x000B4820
		protected override int GetPositionInSetCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetPositionInSet();
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x000B6648 File Offset: 0x000B4848
		protected override int GetSizeOfSetCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.GetSizeOfSet();
			}
			this.ThrowElementNotAvailableException();
			return -1;
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000B6670 File Offset: 0x000B4870
		internal override Rect GetVisibleBoundingRectCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer == null)
			{
				return base.GetBoundingRectangle();
			}
			return wrapperPeer.GetVisibleBoundingRect();
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000B6694 File Offset: 0x000B4894
		protected override bool HasKeyboardFocusCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.HasKeyboardFocus();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000B66BC File Offset: 0x000B48BC
		protected override bool IsContentElementCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsContentElement();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000B66E4 File Offset: 0x000B48E4
		protected override bool IsControlElementCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsControlElement();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000B670C File Offset: 0x000B490C
		protected override bool IsEnabledCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsEnabled();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000B6734 File Offset: 0x000B4934
		protected override bool IsKeyboardFocusableCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsKeyboardFocusable();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000B675C File Offset: 0x000B495C
		protected override bool IsOffscreenCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsOffscreen();
			}
			this.ThrowElementNotAvailableException();
			return true;
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000B6784 File Offset: 0x000B4984
		protected override bool IsPasswordCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsPassword();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000B67AC File Offset: 0x000B49AC
		protected override bool IsRequiredForFormCore()
		{
			AutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				return wrapperPeer.IsRequiredForForm();
			}
			this.ThrowElementNotAvailableException();
			return false;
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000B67D4 File Offset: 0x000B49D4
		protected override void SetFocusCore()
		{
			UIElementAutomationPeer wrapperPeer = this.WrapperPeer;
			if (wrapperPeer != null)
			{
				wrapperPeer.SetFocus();
				return;
			}
			this.ThrowElementNotAvailableException();
		}

		/// <summary>Gets the ordinal number of the column that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the column containing the cell or item.</returns>
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06002645 RID: 9797 RVA: 0x000B67F8 File Offset: 0x000B49F8
		int IGridItemProvider.Column
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton != null)
				{
					return (int)owningButton.GetValue(Grid.ColumnProperty);
				}
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		/// <summary>Gets the number of columns spanned by a cell or item.</summary>
		/// <returns>The number of columns spanned.</returns>
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x000B6830 File Offset: 0x000B4A30
		int IGridItemProvider.ColumnSpan
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton != null)
				{
					return (int)owningButton.GetValue(Grid.ColumnSpanProperty);
				}
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}

		/// <summary>Gets a UI Automation provider that implements <see cref="T:System.Windows.Automation.Provider.IGridProvider" /> and represents the container of the cell or item.</summary>
		/// <returns>A UI Automation provider that represents the cell or item container.</returns>
		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002647 RID: 9799 RVA: 0x000B6867 File Offset: 0x000B4A67
		IRawElementProviderSimple IGridItemProvider.ContainingGrid
		{
			get
			{
				return this.OwningCalendarProvider;
			}
		}

		/// <summary>Gets the ordinal number of the row that contains the cell or item.</summary>
		/// <returns>A zero-based ordinal number that identifies the row containing the cell or item.</returns>
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002648 RID: 9800 RVA: 0x000B6870 File Offset: 0x000B4A70
		int IGridItemProvider.Row
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton == null)
				{
					throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
				}
				if (this.IsDayButton)
				{
					return (int)owningButton.GetValue(Grid.RowProperty) - 1;
				}
				return (int)owningButton.GetValue(Grid.RowProperty);
			}
		}

		/// <summary>Gets the number of rows spanned by a cell or item.</summary>
		/// <returns>The number of rows spanned.</returns>
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002649 RID: 9801 RVA: 0x000B68C4 File Offset: 0x000B4AC4
		int IGridItemProvider.RowSpan
		{
			get
			{
				Button owningButton = this.OwningButton;
				if (owningButton == null)
				{
					throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
				}
				if (this.IsDayButton)
				{
					return (int)owningButton.GetValue(Grid.RowSpanProperty);
				}
				return 1;
			}
		}

		/// <summary>Gets a value that indicates whether an item is selected.</summary>
		/// <returns>
		///     <see langword="true" /> if the element is selected; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600264A RID: 9802 RVA: 0x000B6905 File Offset: 0x000B4B05
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return this.IsDayButton && this.OwningCalendar.SelectedDates.Contains(this.Date);
			}
		}

		/// <summary>Gets the UI Automation provider that implements <see cref="T:System.Windows.Automation.Provider.ISelectionProvider" /> and acts as the container for the calling object.</summary>
		/// <returns>The provider that acts as the container for the calling object.</returns>
		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600264B RID: 9803 RVA: 0x000B6867 File Offset: 0x000B4A67
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return this.OwningCalendarProvider;
			}
		}

		/// <summary>Adds the current element to the collection of selected items.</summary>
		// Token: 0x0600264C RID: 9804 RVA: 0x000B6928 File Offset: 0x000B4B28
		void ISelectionItemProvider.AddToSelection()
		{
			if (((ISelectionItemProvider)this).IsSelected)
			{
				return;
			}
			if (this.IsDayButton && this.EnsureSelection())
			{
				if (this.OwningCalendar.SelectionMode == CalendarSelectionMode.SingleDate)
				{
					this.OwningCalendar.SelectedDate = new DateTime?(this.Date);
					return;
				}
				this.OwningCalendar.SelectedDates.Add(this.Date);
			}
		}

		/// <summary>Removes the current element from the collection of selected items.</summary>
		// Token: 0x0600264D RID: 9805 RVA: 0x000B6988 File Offset: 0x000B4B88
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!((ISelectionItemProvider)this).IsSelected)
			{
				return;
			}
			if (this.IsDayButton)
			{
				this.OwningCalendar.SelectedDates.Remove(this.Date);
			}
		}

		/// <summary>Clears any selected items and then selects the current element.</summary>
		// Token: 0x0600264E RID: 9806 RVA: 0x000B69B4 File Offset: 0x000B4BB4
		void ISelectionItemProvider.Select()
		{
			Button owningButton = this.OwningButton;
			if (this.IsDayButton)
			{
				if (this.EnsureSelection() && this.OwningCalendar.SelectionMode == CalendarSelectionMode.SingleDate)
				{
					this.OwningCalendar.SelectedDate = new DateTime?(this.Date);
					return;
				}
			}
			else if (owningButton != null && owningButton.IsEnabled)
			{
				owningButton.Focus();
			}
		}

		/// <summary>Retrieves a collection of UI Automation providers that represents all the column headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x0600264F RID: 9807 RVA: 0x000B6A10 File Offset: 0x000B4C10
		IRawElementProviderSimple[] ITableItemProvider.GetColumnHeaderItems()
		{
			if (this.IsDayButton && this.OwningButton != null && this.OwningCalendar != null && this.OwningCalendarProvider != null)
			{
				IRawElementProviderSimple[] columnHeaders = ((ITableProvider)UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar)).GetColumnHeaders();
				if (columnHeaders != null)
				{
					int column = ((IGridItemProvider)this).Column;
					return new IRawElementProviderSimple[]
					{
						columnHeaders[column]
					};
				}
			}
			return null;
		}

		/// <summary>Retrieves a collection of UI Automation providers that represents all the row headers associated with a table item or cell.</summary>
		/// <returns>A collection of UI Automation providers.</returns>
		// Token: 0x06002650 RID: 9808 RVA: 0x0000C238 File Offset: 0x0000A438
		IRawElementProviderSimple[] ITableItemProvider.GetRowHeaderItems()
		{
			return null;
		}

		/// <summary>Sends a request to activate a control and initiate its single, unambiguous action.</summary>
		// Token: 0x06002651 RID: 9809 RVA: 0x000B6A6C File Offset: 0x000B4C6C
		void IInvokeProvider.Invoke()
		{
			Button owningButton = this.OwningButton;
			if (owningButton == null || !base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate(object param)
			{
				owningButton.AutomationButtonBaseClick();
				return null;
			}), null);
		}

		/// <summary>Makes the virtual item fully accessible as a UI Automation element.</summary>
		// Token: 0x06002652 RID: 9810 RVA: 0x000B6ABB File Offset: 0x000B4CBB
		void IVirtualizedItemProvider.Realize()
		{
			if (this.OwningCalendar.DisplayMode != this.ButtonMode)
			{
				this.OwningCalendar.DisplayMode = this.ButtonMode;
			}
			this.OwningCalendar.DisplayDate = this.Date;
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsDataItemAutomationPeer()
		{
			return true;
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000B6AF4 File Offset: 0x000B4CF4
		internal override void AddToParentProxyWeakRefCache()
		{
			CalendarAutomationPeer calendarAutomationPeer = UIElementAutomationPeer.CreatePeerForElement(this.OwningCalendar) as CalendarAutomationPeer;
			if (calendarAutomationPeer != null)
			{
				calendarAutomationPeer.AddProxyToWeakRefStorage(base.ElementProxyWeakReference, this);
			}
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000B6B22 File Offset: 0x000B4D22
		private bool EnsureSelection()
		{
			return !this.OwningCalendar.BlackoutDates.Contains(this.Date) && this.OwningCalendar.SelectionMode != CalendarSelectionMode.None;
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x000B6B50 File Offset: 0x000B4D50
		private bool IsItemInAutomationTree()
		{
			AutomationPeer parent = base.GetParent();
			return base.Index != -1 && parent != null && parent.Children != null && base.Index < parent.Children.Count && parent.Children[base.Index] == this;
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x000B6BA2 File Offset: 0x000B4DA2
		private void ThrowElementNotAvailableException()
		{
			if (VirtualizedItemPatternIdentifiers.Pattern != null)
			{
				throw new ElementNotAvailableException(SR.Get("VirtualizedElement"));
			}
		}
	}
}
