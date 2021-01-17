using System;

namespace System.Windows.Controls
{
	// Token: 0x02000479 RID: 1145
	internal class CalendarDateRangeChangingEventArgs : EventArgs
	{
		// Token: 0x0600431E RID: 17182 RVA: 0x001332C2 File Offset: 0x001314C2
		public CalendarDateRangeChangingEventArgs(DateTime start, DateTime end)
		{
			this._start = start;
			this._end = end;
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x0600431F RID: 17183 RVA: 0x001332D8 File Offset: 0x001314D8
		public DateTime Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06004320 RID: 17184 RVA: 0x001332E0 File Offset: 0x001314E0
		public DateTime End
		{
			get
			{
				return this._end;
			}
		}

		// Token: 0x04002825 RID: 10277
		private DateTime _start;

		// Token: 0x04002826 RID: 10278
		private DateTime _end;
	}
}
