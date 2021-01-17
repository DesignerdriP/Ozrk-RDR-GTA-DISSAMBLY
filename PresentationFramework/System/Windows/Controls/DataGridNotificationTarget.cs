using System;

namespace System.Windows.Controls
{
	// Token: 0x020004B2 RID: 1202
	[Flags]
	internal enum DataGridNotificationTarget
	{
		// Token: 0x040029D3 RID: 10707
		None = 0,
		// Token: 0x040029D4 RID: 10708
		Cells = 1,
		// Token: 0x040029D5 RID: 10709
		CellsPresenter = 2,
		// Token: 0x040029D6 RID: 10710
		Columns = 4,
		// Token: 0x040029D7 RID: 10711
		ColumnCollection = 8,
		// Token: 0x040029D8 RID: 10712
		ColumnHeaders = 16,
		// Token: 0x040029D9 RID: 10713
		ColumnHeadersPresenter = 32,
		// Token: 0x040029DA RID: 10714
		DataGrid = 64,
		// Token: 0x040029DB RID: 10715
		DetailsPresenter = 128,
		// Token: 0x040029DC RID: 10716
		RefreshCellContent = 256,
		// Token: 0x040029DD RID: 10717
		RowHeaders = 512,
		// Token: 0x040029DE RID: 10718
		Rows = 1024,
		// Token: 0x040029DF RID: 10719
		All = 4095
	}
}
