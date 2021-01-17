using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Represents a range of dates in a <see cref="T:System.Windows.Controls.Calendar" />.</summary>
	// Token: 0x02000478 RID: 1144
	public sealed class CalendarDateRange : INotifyPropertyChanged
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarDateRange" /> class. </summary>
		// Token: 0x0600430F RID: 17167 RVA: 0x00133062 File Offset: 0x00131262
		public CalendarDateRange() : this(DateTime.MinValue, DateTime.MaxValue)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarDateRange" /> class with a single date.</summary>
		/// <param name="day">The date to add.</param>
		// Token: 0x06004310 RID: 17168 RVA: 0x00133074 File Offset: 0x00131274
		public CalendarDateRange(DateTime day) : this(day, day)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.CalendarDateRange" /> class with a range of dates.</summary>
		/// <param name="start">The start of the range to be represented.</param>
		/// <param name="end">The end of the range to be represented.</param>
		// Token: 0x06004311 RID: 17169 RVA: 0x0013307E File Offset: 0x0013127E
		public CalendarDateRange(DateTime start, DateTime end)
		{
			this._start = start;
			this._end = end;
		}

		/// <summary>Occurs when a property value changes.</summary>
		// Token: 0x140000A5 RID: 165
		// (add) Token: 0x06004312 RID: 17170 RVA: 0x00133094 File Offset: 0x00131294
		// (remove) Token: 0x06004313 RID: 17171 RVA: 0x001330CC File Offset: 0x001312CC
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Gets the last date in the represented range.</summary>
		/// <returns>The last date in the represented range.</returns>
		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06004314 RID: 17172 RVA: 0x00133101 File Offset: 0x00131301
		// (set) Token: 0x06004315 RID: 17173 RVA: 0x00133114 File Offset: 0x00131314
		public DateTime End
		{
			get
			{
				return CalendarDateRange.CoerceEnd(this._start, this._end);
			}
			set
			{
				DateTime dateTime = CalendarDateRange.CoerceEnd(this._start, value);
				if (dateTime != this.End)
				{
					this.OnChanging(new CalendarDateRangeChangingEventArgs(this._start, dateTime));
					this._end = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("End"));
				}
			}
		}

		/// <summary>Gets the first date in the represented range.</summary>
		/// <returns>The first date in the represented range.</returns>
		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06004316 RID: 17174 RVA: 0x00133165 File Offset: 0x00131365
		// (set) Token: 0x06004317 RID: 17175 RVA: 0x00133170 File Offset: 0x00131370
		public DateTime Start
		{
			get
			{
				return this._start;
			}
			set
			{
				if (this._start != value)
				{
					DateTime end = this.End;
					DateTime dateTime = CalendarDateRange.CoerceEnd(value, this._end);
					this.OnChanging(new CalendarDateRangeChangingEventArgs(value, dateTime));
					this._start = value;
					this.OnPropertyChanged(new PropertyChangedEventArgs("Start"));
					if (dateTime != end)
					{
						this.OnPropertyChanged(new PropertyChangedEventArgs("End"));
					}
				}
			}
		}

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06004318 RID: 17176 RVA: 0x001331DC File Offset: 0x001313DC
		// (remove) Token: 0x06004319 RID: 17177 RVA: 0x00133214 File Offset: 0x00131414
		internal event EventHandler<CalendarDateRangeChangingEventArgs> Changing;

		// Token: 0x0600431A RID: 17178 RVA: 0x00133249 File Offset: 0x00131449
		internal bool ContainsAny(CalendarDateRange range)
		{
			return range.End >= this.Start && this.End >= range.Start;
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00133274 File Offset: 0x00131474
		private void OnChanging(CalendarDateRangeChangingEventArgs e)
		{
			EventHandler<CalendarDateRangeChangingEventArgs> changing = this.Changing;
			if (changing != null)
			{
				changing(this, e);
			}
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x00133294 File Offset: 0x00131494
		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, e);
			}
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x001332B3 File Offset: 0x001314B3
		private static DateTime CoerceEnd(DateTime start, DateTime end)
		{
			if (DateTime.Compare(start, end) > 0)
			{
				return start;
			}
			return end;
		}

		// Token: 0x04002821 RID: 10273
		private DateTime _end;

		// Token: 0x04002822 RID: 10274
		private DateTime _start;
	}
}
