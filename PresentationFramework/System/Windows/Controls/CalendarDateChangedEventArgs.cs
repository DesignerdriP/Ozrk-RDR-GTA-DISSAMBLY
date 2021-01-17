using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.Calendar.DisplayDateChanged" /> event.</summary>
	// Token: 0x02000477 RID: 1143
	public class CalendarDateChangedEventArgs : RoutedEventArgs
	{
		// Token: 0x0600430A RID: 17162 RVA: 0x0013302A File Offset: 0x0013122A
		internal CalendarDateChangedEventArgs(DateTime? removedDate, DateTime? addedDate)
		{
			this.RemovedDate = removedDate;
			this.AddedDate = addedDate;
		}

		/// <summary>Gets or sets the date to be newly displayed.</summary>
		/// <returns>The date to be newly displayed.</returns>
		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x0600430B RID: 17163 RVA: 0x00133040 File Offset: 0x00131240
		// (set) Token: 0x0600430C RID: 17164 RVA: 0x00133048 File Offset: 0x00131248
		public DateTime? AddedDate { get; private set; }

		/// <summary>Gets or sets the date that was previously displayed.</summary>
		/// <returns>The date that was previously displayed.</returns>
		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x0600430D RID: 17165 RVA: 0x00133051 File Offset: 0x00131251
		// (set) Token: 0x0600430E RID: 17166 RVA: 0x00133059 File Offset: 0x00131259
		public DateTime? RemovedDate { get; private set; }
	}
}
