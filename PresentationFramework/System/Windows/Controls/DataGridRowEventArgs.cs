using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.LoadingRow" /> and <see cref="E:System.Windows.Controls.DataGrid.UnloadingRow" /> events. </summary>
	// Token: 0x020004B9 RID: 1209
	public class DataGridRowEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowEventArgs" /> class. </summary>
		/// <param name="row">The row for which the event occurred. </param>
		// Token: 0x06004995 RID: 18837 RVA: 0x0014D075 File Offset: 0x0014B275
		public DataGridRowEventArgs(DataGridRow row)
		{
			this.Row = row;
		}

		/// <summary>Gets the row for which the event occurred. </summary>
		/// <returns>The row for which the event occurred. </returns>
		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06004996 RID: 18838 RVA: 0x0014D084 File Offset: 0x0014B284
		// (set) Token: 0x06004997 RID: 18839 RVA: 0x0014D08C File Offset: 0x0014B28C
		public DataGridRow Row { get; private set; }
	}
}
