using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.CellEditEnding" /> event. </summary>
	// Token: 0x02000498 RID: 1176
	public class DataGridCellEditEndingEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridCellEditEndingEventArgs" /> class. </summary>
		/// <param name="column">The column that contains the cell for which the event occurred. </param>
		/// <param name="row">The row that contains the cell for which the event occurred. </param>
		/// <param name="editingElement">The element that the cell displays in editing mode.</param>
		/// <param name="editAction">A value that indicates whether the edit was canceled or committed. </param>
		// Token: 0x0600471E RID: 18206 RVA: 0x0014251C File Offset: 0x0014071C
		public DataGridCellEditEndingEventArgs(DataGridColumn column, DataGridRow row, FrameworkElement editingElement, DataGridEditAction editAction)
		{
			this._dataGridColumn = column;
			this._dataGridRow = row;
			this._editingElement = editingElement;
			this._editAction = editAction;
		}

		/// <summary>Gets or sets a value that indicates whether the event should be canceled. </summary>
		/// <returns>
		///     <see langword="true" /> if the event should be canceled; otherwise, <see langword="false" />. The default is <see langword="false" />. </returns>
		// Token: 0x1700116B RID: 4459
		// (get) Token: 0x0600471F RID: 18207 RVA: 0x00142541 File Offset: 0x00140741
		// (set) Token: 0x06004720 RID: 18208 RVA: 0x00142549 File Offset: 0x00140749
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

		/// <summary>Gets the column that contains the cell for which the event occurred. </summary>
		/// <returns>The column that contains the cell for which the event occurred. </returns>
		// Token: 0x1700116C RID: 4460
		// (get) Token: 0x06004721 RID: 18209 RVA: 0x00142552 File Offset: 0x00140752
		public DataGridColumn Column
		{
			get
			{
				return this._dataGridColumn;
			}
		}

		/// <summary>Gets the row that contains the cell for which the event occurred. </summary>
		/// <returns>The row that contains the cell for which the event occurred. </returns>
		// Token: 0x1700116D RID: 4461
		// (get) Token: 0x06004722 RID: 18210 RVA: 0x0014255A File Offset: 0x0014075A
		public DataGridRow Row
		{
			get
			{
				return this._dataGridRow;
			}
		}

		/// <summary>Gets the element that the cell displays in editing mode.</summary>
		/// <returns>The element that the cell displays in editing mode.</returns>
		// Token: 0x1700116E RID: 4462
		// (get) Token: 0x06004723 RID: 18211 RVA: 0x00142562 File Offset: 0x00140762
		public FrameworkElement EditingElement
		{
			get
			{
				return this._editingElement;
			}
		}

		/// <summary>Gets a value that indicates whether the edit was canceled or committed. </summary>
		/// <returns>A value that indicates whether the edit was canceled or committed. </returns>
		// Token: 0x1700116F RID: 4463
		// (get) Token: 0x06004724 RID: 18212 RVA: 0x0014256A File Offset: 0x0014076A
		public DataGridEditAction EditAction
		{
			get
			{
				return this._editAction;
			}
		}

		// Token: 0x0400294C RID: 10572
		private bool _cancel;

		// Token: 0x0400294D RID: 10573
		private DataGridColumn _dataGridColumn;

		// Token: 0x0400294E RID: 10574
		private DataGridRow _dataGridRow;

		// Token: 0x0400294F RID: 10575
		private FrameworkElement _editingElement;

		// Token: 0x04002950 RID: 10576
		private DataGridEditAction _editAction;
	}
}
