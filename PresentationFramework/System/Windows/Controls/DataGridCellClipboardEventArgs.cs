using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGridColumn.CopyingCellClipboardContent" /> and <see cref="E:System.Windows.Controls.DataGridColumn.PastingCellClipboardContent" /> events.</summary>
	// Token: 0x02000497 RID: 1175
	public class DataGridCellClipboardEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCellClipboardEventArgs" /> class. </summary>
		/// <param name="item">The data item for the row that contains the cell for which the event occurred.</param>
		/// <param name="column">The column that contains the cell for which the event occurred. </param>
		/// <param name="content">The text value of the cell for which the event occurred. </param>
		// Token: 0x06004719 RID: 18201 RVA: 0x001424DE File Offset: 0x001406DE
		public DataGridCellClipboardEventArgs(object item, DataGridColumn column, object content)
		{
			this._item = item;
			this._column = column;
			this._content = content;
		}

		/// <summary>Gets or sets the text value of the cell for which the event occurred.</summary>
		/// <returns>The text value of the cell for which the event occurred.</returns>
		// Token: 0x17001168 RID: 4456
		// (get) Token: 0x0600471A RID: 18202 RVA: 0x001424FB File Offset: 0x001406FB
		// (set) Token: 0x0600471B RID: 18203 RVA: 0x00142503 File Offset: 0x00140703
		public object Content
		{
			get
			{
				return this._content;
			}
			set
			{
				this._content = value;
			}
		}

		/// <summary>Gets the data item for the row that contains the cell for which the event occurred.</summary>
		/// <returns>The data item for the row that contains the cell for which the event occurred.</returns>
		// Token: 0x17001169 RID: 4457
		// (get) Token: 0x0600471C RID: 18204 RVA: 0x0014250C File Offset: 0x0014070C
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		/// <summary>Gets the column that contains the cell for which the event occurred. </summary>
		/// <returns>The column that contains the cell for which the event occurred. </returns>
		// Token: 0x1700116A RID: 4458
		// (get) Token: 0x0600471D RID: 18205 RVA: 0x00142514 File Offset: 0x00140714
		public DataGridColumn Column
		{
			get
			{
				return this._column;
			}
		}

		// Token: 0x04002949 RID: 10569
		private object _content;

		// Token: 0x0400294A RID: 10570
		private object _item;

		// Token: 0x0400294B RID: 10571
		private DataGridColumn _column;
	}
}
