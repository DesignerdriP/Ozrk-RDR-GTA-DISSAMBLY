using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.ColumnDisplayIndexChanged" /> and <see cref="E:System.Windows.Controls.DataGrid.ColumnReordered" /> events.</summary>
	// Token: 0x020004A2 RID: 1186
	public class DataGridColumnEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridColumnEventArgs" /> class.</summary>
		/// <param name="column">The column related to the event.</param>
		// Token: 0x06004873 RID: 18547 RVA: 0x001497CE File Offset: 0x001479CE
		public DataGridColumnEventArgs(DataGridColumn column)
		{
			this._column = column;
		}

		/// <summary>Gets the column related to the event.</summary>
		/// <returns>An object that represents the column related to the event.</returns>
		// Token: 0x170011B2 RID: 4530
		// (get) Token: 0x06004874 RID: 18548 RVA: 0x001497DD File Offset: 0x001479DD
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x04002998 RID: 10648
		private DataGridColumn _column;
	}
}
