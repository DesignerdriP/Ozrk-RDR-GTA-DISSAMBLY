using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x02000299 RID: 665
	internal struct DateTimeCalendarModePair
	{
		// Token: 0x06002544 RID: 9540 RVA: 0x000B3C85 File Offset: 0x000B1E85
		internal DateTimeCalendarModePair(DateTime date, CalendarMode mode)
		{
			this.ButtonMode = mode;
			this.Date = date;
		}

		// Token: 0x04001B66 RID: 7014
		private CalendarMode ButtonMode;

		// Token: 0x04001B67 RID: 7015
		private DateTime Date;
	}
}
