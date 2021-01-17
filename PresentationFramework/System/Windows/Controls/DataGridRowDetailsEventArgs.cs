using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.LoadingRowDetails" />, <see cref="E:System.Windows.Controls.DataGrid.UnloadingRowDetails" />, and <see cref="E:System.Windows.Controls.DataGrid.RowDetailsVisibilityChanged" /> events.</summary>
	// Token: 0x020004B6 RID: 1206
	public class DataGridRowDetailsEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowDetailsEventArgs" /> class. </summary>
		/// <param name="row">The row for which the event occurred.</param>
		/// <param name="detailsElement">The row details section as a framework element. </param>
		// Token: 0x0600498B RID: 18827 RVA: 0x0014D006 File Offset: 0x0014B206
		public DataGridRowDetailsEventArgs(DataGridRow row, FrameworkElement detailsElement)
		{
			this.Row = row;
			this.DetailsElement = detailsElement;
		}

		/// <summary>Gets the row details section as a framework element. </summary>
		/// <returns>The row details section as a framework element. </returns>
		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x0600498C RID: 18828 RVA: 0x0014D01C File Offset: 0x0014B21C
		// (set) Token: 0x0600498D RID: 18829 RVA: 0x0014D024 File Offset: 0x0014B224
		public FrameworkElement DetailsElement { get; private set; }

		/// <summary>Gets the row for which the event occurred.</summary>
		/// <returns>The row for which the event occurred.</returns>
		// Token: 0x170011F6 RID: 4598
		// (get) Token: 0x0600498E RID: 18830 RVA: 0x0014D02D File Offset: 0x0014B22D
		// (set) Token: 0x0600498F RID: 18831 RVA: 0x0014D035 File Offset: 0x0014B235
		public DataGridRow Row { get; private set; }
	}
}
