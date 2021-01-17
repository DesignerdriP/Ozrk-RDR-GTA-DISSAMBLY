using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.RowEditEnding" /> event. </summary>
	// Token: 0x020004B8 RID: 1208
	public class DataGridRowEditEndingEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridRowEditEndingEventArgs" /> class. </summary>
		/// <param name="row">The row for which the event occurred. </param>
		/// <param name="editAction">A value that indicates whether the edit was canceled or committed. </param>
		// Token: 0x06004990 RID: 18832 RVA: 0x0014D03E File Offset: 0x0014B23E
		public DataGridRowEditEndingEventArgs(DataGridRow row, DataGridEditAction editAction)
		{
			this._dataGridRow = row;
			this._editAction = editAction;
		}

		/// <summary>Gets or sets a value that indicates whether the event should be canceled. </summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
		// Token: 0x170011F7 RID: 4599
		// (get) Token: 0x06004991 RID: 18833 RVA: 0x0014D054 File Offset: 0x0014B254
		// (set) Token: 0x06004992 RID: 18834 RVA: 0x0014D05C File Offset: 0x0014B25C
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		/// <summary>Gets the row for which the event occurred. </summary>
		/// <returns>The row for which the event occurred. </returns>
		// Token: 0x170011F8 RID: 4600
		// (get) Token: 0x06004993 RID: 18835 RVA: 0x0014D065 File Offset: 0x0014B265
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		/// <summary>Gets a value that indicates whether the edit was canceled or committed. </summary>
		/// <returns>A value that indicates whether the edit was canceled or committed. </returns>
		// Token: 0x170011F9 RID: 4601
		// (get) Token: 0x06004994 RID: 18836 RVA: 0x0014D06D File Offset: 0x0014B26D
		public DataGridEditAction EditAction
		{
			get
			{
				return this._editAction;
			}
		}

		// Token: 0x04002A17 RID: 10775
		private bool _cancel;

		// Token: 0x04002A18 RID: 10776
		private DataGridRow _dataGridRow;

		// Token: 0x04002A19 RID: 10777
		private DataGridEditAction _editAction;
	}
}
