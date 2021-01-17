using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.CopyingRowClipboardContent" /> event. </summary>
	// Token: 0x020004B5 RID: 1205
	public class DataGridRowClipboardEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowClipboardEventArgs" /> class. </summary>
		/// <param name="item">The data item for the row for which the event occurred.</param>
		/// <param name="startColumnDisplayIndex">The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the first selected cell in the row.</param>
		/// <param name="endColumnDisplayIndex">The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the last selected cell in the row. </param>
		/// <param name="isColumnHeadersRow">A value that indicates whether the row for which the event occurred represents the column headers. </param>
		// Token: 0x06004982 RID: 18818 RVA: 0x0014CF2A File Offset: 0x0014B12A
		public DataGridRowClipboardEventArgs(object item, int startColumnDisplayIndex, int endColumnDisplayIndex, bool isColumnHeadersRow)
		{
			this._item = item;
			this._startColumnDisplayIndex = startColumnDisplayIndex;
			this._endColumnDisplayIndex = endColumnDisplayIndex;
			this._isColumnHeadersRow = isColumnHeadersRow;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x0014CF56 File Offset: 0x0014B156
		internal DataGridRowClipboardEventArgs(object item, int startColumnDisplayIndex, int endColumnDisplayIndex, bool isColumnHeadersRow, int rowIndexHint) : this(item, startColumnDisplayIndex, endColumnDisplayIndex, isColumnHeadersRow)
		{
			this._rowIndexHint = rowIndexHint;
		}

		/// <summary>Gets the data item for the row for which the event occurred.</summary>
		/// <returns>The data item for the row for which the event occurred.</returns>
		// Token: 0x170011EF RID: 4591
		// (get) Token: 0x06004984 RID: 18820 RVA: 0x0014CF6B File Offset: 0x0014B16B
		public object Item
		{
			get
			{
				return this._item;
			}
		}

		/// <summary>Gets a list of <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> values that represent the text values of the cells being copied. </summary>
		/// <returns>A list of <see cref="T:System.Windows.Controls.DataGridClipboardCellContent" /> values that represent the text values of the cells being copied. </returns>
		// Token: 0x170011F0 RID: 4592
		// (get) Token: 0x06004985 RID: 18821 RVA: 0x0014CF73 File Offset: 0x0014B173
		public List<DataGridClipboardCellContent> ClipboardRowContent
		{
			get
			{
				if (this._clipboardRowContent == null)
				{
					this._clipboardRowContent = new List<DataGridClipboardCellContent>();
				}
				return this._clipboardRowContent;
			}
		}

		/// <summary>Returns the <see cref="P:System.Windows.Controls.DataGridRowClipboardEventArgs.ClipboardRowContent" /> values as a string in the specified format. </summary>
		/// <param name="format">The data format in which to serialize the cell values. </param>
		/// <returns>The formatted string.</returns>
		// Token: 0x06004986 RID: 18822 RVA: 0x0014CF90 File Offset: 0x0014B190
		public string FormatClipboardCellValues(string format)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = this.ClipboardRowContent.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridClipboardHelper.FormatCell(this.ClipboardRowContent[i].Content, i == 0, i == count - 1, stringBuilder, format);
			}
			return stringBuilder.ToString();
		}

		/// <summary>Gets the <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the first selected cell in the row.</summary>
		/// <returns>The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the first selected cell in the row.</returns>
		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06004987 RID: 18823 RVA: 0x0014CFE6 File Offset: 0x0014B1E6
		public int StartColumnDisplayIndex
		{
			get
			{
				return this._startColumnDisplayIndex;
			}
		}

		/// <summary>Gets the <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the last selected cell in the row. </summary>
		/// <returns>The <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> value of the column that contains the last selected cell in the row. </returns>
		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06004988 RID: 18824 RVA: 0x0014CFEE File Offset: 0x0014B1EE
		public int EndColumnDisplayIndex
		{
			get
			{
				return this._endColumnDisplayIndex;
			}
		}

		/// <summary>Gets a value that indicates whether the row for which the event occurred represents the column headers. </summary>
		/// <returns>
		///     <see langword="true" /> if the row represents the column headers; otherwise, <see langword="false" />.</returns>
		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x0014CFF6 File Offset: 0x0014B1F6
		public bool IsColumnHeadersRow
		{
			get
			{
				return this._isColumnHeadersRow;
			}
		}

		// Token: 0x170011F4 RID: 4596
		// (get) Token: 0x0600498A RID: 18826 RVA: 0x0014CFFE File Offset: 0x0014B1FE
		internal int RowIndexHint
		{
			get
			{
				return this._rowIndexHint;
			}
		}

		// Token: 0x04002A0B RID: 10763
		private int _startColumnDisplayIndex;

		// Token: 0x04002A0C RID: 10764
		private int _endColumnDisplayIndex;

		// Token: 0x04002A0D RID: 10765
		private object _item;

		// Token: 0x04002A0E RID: 10766
		private bool _isColumnHeadersRow;

		// Token: 0x04002A0F RID: 10767
		private List<DataGridClipboardCellContent> _clipboardRowContent;

		// Token: 0x04002A10 RID: 10768
		private int _rowIndexHint = -1;
	}
}
