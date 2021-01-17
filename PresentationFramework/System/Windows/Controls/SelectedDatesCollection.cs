using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace System.Windows.Controls
{
	/// <summary>Represents a set of selected dates in a <see cref="T:System.Windows.Controls.Calendar" />.</summary>
	// Token: 0x0200052C RID: 1324
	public sealed class SelectedDatesCollection : ObservableCollection<DateTime>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.SelectedDatesCollection" /> class. </summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Calendar" /> associated with this collection.</param>
		// Token: 0x060055C5 RID: 21957 RVA: 0x0017C1FB File Offset: 0x0017A3FB
		public SelectedDatesCollection(Calendar owner)
		{
			this._dispatcherThread = Thread.CurrentThread;
			this._owner = owner;
			this._addedItems = new Collection<DateTime>();
			this._removedItems = new Collection<DateTime>();
		}

		// Token: 0x170014DC RID: 5340
		// (get) Token: 0x060055C6 RID: 21958 RVA: 0x0017C22C File Offset: 0x0017A42C
		internal DateTime? MinimumDate
		{
			get
			{
				if (base.Count < 1)
				{
					return null;
				}
				if (this._minimumDate == null)
				{
					DateTime dateTime = base[0];
					foreach (DateTime dateTime2 in this)
					{
						if (DateTime.Compare(dateTime2, dateTime) < 0)
						{
							dateTime = dateTime2;
						}
					}
					this._maximumDate = new DateTime?(dateTime);
				}
				return this._minimumDate;
			}
		}

		// Token: 0x170014DD RID: 5341
		// (get) Token: 0x060055C7 RID: 21959 RVA: 0x0017C2B4 File Offset: 0x0017A4B4
		internal DateTime? MaximumDate
		{
			get
			{
				if (base.Count < 1)
				{
					return null;
				}
				if (this._maximumDate == null)
				{
					DateTime dateTime = base[0];
					foreach (DateTime dateTime2 in this)
					{
						if (DateTime.Compare(dateTime2, dateTime) > 0)
						{
							dateTime = dateTime2;
						}
					}
					this._maximumDate = new DateTime?(dateTime);
				}
				return this._maximumDate;
			}
		}

		/// <summary>Adds all the dates in the specified range, which includes the first and last dates, to the collection.</summary>
		/// <param name="start">The first date to add to the collection.</param>
		/// <param name="end">The last date to add to the collection.</param>
		// Token: 0x060055C8 RID: 21960 RVA: 0x0017C33C File Offset: 0x0017A53C
		public void AddRange(DateTime start, DateTime end)
		{
			this.BeginAddRange();
			if (this._owner.SelectionMode == CalendarSelectionMode.SingleRange && base.Count > 0)
			{
				this.ClearInternal();
			}
			foreach (DateTime item in SelectedDatesCollection.GetDaysInRange(start, end))
			{
				base.Add(item);
			}
			this.EndAddRange();
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x0017C3B4 File Offset: 0x0017A5B4
		protected override void ClearItems()
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			this._owner.HoverStart = null;
			this.ClearInternal(true);
		}

		// Token: 0x060055CA RID: 21962 RVA: 0x0017C3F4 File Offset: 0x0017A5F4
		protected override void InsertItem(int index, DateTime item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (!base.Contains(item))
			{
				Collection<DateTime> collection = new Collection<DateTime>();
				bool flag = this.CheckSelectionMode();
				if (!Calendar.IsValidDateSelection(this._owner, item))
				{
					throw new ArgumentOutOfRangeException(SR.Get("Calendar_OnSelectedDateChanged_InvalidValue"));
				}
				if (flag)
				{
					index = 0;
				}
				base.InsertItem(index, item);
				this.UpdateMinMax(item);
				if (index == 0 && (this._owner.SelectedDate == null || DateTime.Compare(this._owner.SelectedDate.Value, item) != 0))
				{
					this._owner.SelectedDate = new DateTime?(item);
				}
				if (this._isAddingRange)
				{
					this._addedItems.Add(item);
					return;
				}
				collection.Add(item);
				this.RaiseSelectionChanged(this._removedItems, collection);
				this._removedItems.Clear();
				int num = DateTimeHelper.CompareYearMonth(item, this._owner.DisplayDateInternal);
				if (num < 2 && num > -2)
				{
					this._owner.UpdateCellItems();
					return;
				}
			}
		}

		// Token: 0x060055CB RID: 21963 RVA: 0x0017C510 File Offset: 0x0017A710
		protected override void RemoveItem(int index)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (index >= base.Count)
			{
				base.RemoveItem(index);
				this.ClearMinMax();
				return;
			}
			Collection<DateTime> addedItems = new Collection<DateTime>();
			Collection<DateTime> collection = new Collection<DateTime>();
			int num = DateTimeHelper.CompareYearMonth(base[index], this._owner.DisplayDateInternal);
			collection.Add(base[index]);
			base.RemoveItem(index);
			this.ClearMinMax();
			if (index == 0)
			{
				if (base.Count > 0)
				{
					this._owner.SelectedDate = new DateTime?(base[0]);
				}
				else
				{
					this._owner.SelectedDate = null;
				}
			}
			this.RaiseSelectionChanged(collection, addedItems);
			if (num < 2 && num > -2)
			{
				this._owner.UpdateCellItems();
			}
		}

		// Token: 0x060055CC RID: 21964 RVA: 0x0017C5E0 File Offset: 0x0017A7E0
		protected override void SetItem(int index, DateTime item)
		{
			if (!this.IsValidThread())
			{
				throw new NotSupportedException(SR.Get("CalendarCollection_MultiThreadedCollectionChangeNotSupported"));
			}
			if (!base.Contains(item))
			{
				Collection<DateTime> collection = new Collection<DateTime>();
				Collection<DateTime> collection2 = new Collection<DateTime>();
				if (index >= base.Count)
				{
					base.SetItem(index, item);
					this.UpdateMinMax(item);
					return;
				}
				if (DateTime.Compare(base[index], item) != 0 && Calendar.IsValidDateSelection(this._owner, item))
				{
					collection2.Add(base[index]);
					base.SetItem(index, item);
					this.UpdateMinMax(item);
					collection.Add(item);
					if (index == 0 && (this._owner.SelectedDate == null || DateTime.Compare(this._owner.SelectedDate.Value, item) != 0))
					{
						this._owner.SelectedDate = new DateTime?(item);
					}
					this.RaiseSelectionChanged(collection2, collection);
					int num = DateTimeHelper.CompareYearMonth(item, this._owner.DisplayDateInternal);
					if (num < 2 && num > -2)
					{
						this._owner.UpdateCellItems();
					}
				}
			}
		}

		// Token: 0x060055CD RID: 21965 RVA: 0x0017C6F4 File Offset: 0x0017A8F4
		internal void AddRangeInternal(DateTime start, DateTime end)
		{
			this.BeginAddRange();
			DateTime currentDate = start;
			foreach (DateTime dateTime in SelectedDatesCollection.GetDaysInRange(start, end))
			{
				if (Calendar.IsValidDateSelection(this._owner, dateTime))
				{
					base.Add(dateTime);
					currentDate = dateTime;
				}
				else if (this._owner.SelectionMode == CalendarSelectionMode.SingleRange)
				{
					this._owner.CurrentDate = currentDate;
					break;
				}
			}
			this.EndAddRange();
		}

		// Token: 0x060055CE RID: 21966 RVA: 0x0017C784 File Offset: 0x0017A984
		internal void ClearInternal()
		{
			this.ClearInternal(false);
		}

		// Token: 0x060055CF RID: 21967 RVA: 0x0017C790 File Offset: 0x0017A990
		internal void ClearInternal(bool fireChangeNotification)
		{
			if (base.Count > 0)
			{
				foreach (DateTime item in this)
				{
					this._removedItems.Add(item);
				}
				base.ClearItems();
				this.ClearMinMax();
				if (fireChangeNotification)
				{
					if (this._owner.SelectedDate != null)
					{
						this._owner.SelectedDate = null;
					}
					if (this._removedItems.Count > 0)
					{
						Collection<DateTime> addedItems = new Collection<DateTime>();
						this.RaiseSelectionChanged(this._removedItems, addedItems);
						this._removedItems.Clear();
					}
					this._owner.UpdateCellItems();
				}
			}
		}

		// Token: 0x060055D0 RID: 21968 RVA: 0x0017C858 File Offset: 0x0017AA58
		internal void Toggle(DateTime date)
		{
			if (Calendar.IsValidDateSelection(this._owner, date))
			{
				CalendarSelectionMode selectionMode = this._owner.SelectionMode;
				if (selectionMode != CalendarSelectionMode.SingleDate)
				{
					if (selectionMode != CalendarSelectionMode.MultipleRange)
					{
						return;
					}
					if (!base.Remove(date))
					{
						base.Add(date);
					}
				}
				else
				{
					if (this._owner.SelectedDate == null || DateTimeHelper.CompareDays(this._owner.SelectedDate.Value, date) != 0)
					{
						this._owner.SelectedDate = new DateTime?(date);
						return;
					}
					this._owner.SelectedDate = null;
					return;
				}
			}
		}

		// Token: 0x060055D1 RID: 21969 RVA: 0x0017C8F3 File Offset: 0x0017AAF3
		private void RaiseSelectionChanged(IList removedItems, IList addedItems)
		{
			this._owner.OnSelectedDatesCollectionChanged(new CalendarSelectionChangedEventArgs(Calendar.SelectedDatesChangedEvent, removedItems, addedItems));
		}

		// Token: 0x060055D2 RID: 21970 RVA: 0x0017C90C File Offset: 0x0017AB0C
		private void BeginAddRange()
		{
			this._isAddingRange = true;
		}

		// Token: 0x060055D3 RID: 21971 RVA: 0x0017C915 File Offset: 0x0017AB15
		private void EndAddRange()
		{
			this._isAddingRange = false;
			this.RaiseSelectionChanged(this._removedItems, this._addedItems);
			this._removedItems.Clear();
			this._addedItems.Clear();
			this._owner.UpdateCellItems();
		}

		// Token: 0x060055D4 RID: 21972 RVA: 0x0017C954 File Offset: 0x0017AB54
		private bool CheckSelectionMode()
		{
			if (this._owner.SelectionMode == CalendarSelectionMode.None)
			{
				throw new InvalidOperationException(SR.Get("Calendar_OnSelectedDateChanged_InvalidOperation"));
			}
			if (this._owner.SelectionMode == CalendarSelectionMode.SingleDate && base.Count > 0)
			{
				throw new InvalidOperationException(SR.Get("Calendar_CheckSelectionMode_InvalidOperation"));
			}
			if (this._owner.SelectionMode == CalendarSelectionMode.SingleRange && !this._isAddingRange && base.Count > 0)
			{
				this.ClearInternal();
				return true;
			}
			return false;
		}

		// Token: 0x060055D5 RID: 21973 RVA: 0x0017C9CD File Offset: 0x0017ABCD
		private bool IsValidThread()
		{
			return Thread.CurrentThread == this._dispatcherThread;
		}

		// Token: 0x060055D6 RID: 21974 RVA: 0x0017C9DC File Offset: 0x0017ABDC
		private void UpdateMinMax(DateTime date)
		{
			if (this._maximumDate == null || date > this._maximumDate.Value)
			{
				this._maximumDate = new DateTime?(date);
			}
			if (this._minimumDate == null || date < this._minimumDate.Value)
			{
				this._minimumDate = new DateTime?(date);
			}
		}

		// Token: 0x060055D7 RID: 21975 RVA: 0x0017CA41 File Offset: 0x0017AC41
		private void ClearMinMax()
		{
			this._maximumDate = null;
			this._minimumDate = null;
		}

		// Token: 0x060055D8 RID: 21976 RVA: 0x0017CA5B File Offset: 0x0017AC5B
		private static IEnumerable<DateTime> GetDaysInRange(DateTime start, DateTime end)
		{
			int increment = SelectedDatesCollection.GetDirection(start, end);
			DateTime? rangeStart = new DateTime?(start);
			do
			{
				yield return rangeStart.Value;
				rangeStart = DateTimeHelper.AddDays(rangeStart.Value, increment);
			}
			while (rangeStart != null && DateTime.Compare(end, rangeStart.Value) != -increment);
			yield break;
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x0017CA72 File Offset: 0x0017AC72
		private static int GetDirection(DateTime start, DateTime end)
		{
			if (DateTime.Compare(end, start) < 0)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x04002E0C RID: 11788
		private Collection<DateTime> _addedItems;

		// Token: 0x04002E0D RID: 11789
		private Collection<DateTime> _removedItems;

		// Token: 0x04002E0E RID: 11790
		private Thread _dispatcherThread;

		// Token: 0x04002E0F RID: 11791
		private bool _isAddingRange;

		// Token: 0x04002E10 RID: 11792
		private Calendar _owner;

		// Token: 0x04002E11 RID: 11793
		private DateTime? _maximumDate;

		// Token: 0x04002E12 RID: 11794
		private DateTime? _minimumDate;
	}
}
