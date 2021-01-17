using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.ColumnReordering" /> event.</summary>
	// Token: 0x020004A5 RID: 1189
	public class DataGridColumnReorderingEventArgs : DataGridColumnEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridColumnReorderingEventArgs" /> class.</summary>
		/// <param name="dataGridColumn">The column that is being moved.</param>
		// Token: 0x0600488C RID: 18572 RVA: 0x00149DCE File Offset: 0x00147FCE
		public DataGridColumnReorderingEventArgs(DataGridColumn dataGridColumn) : base(dataGridColumn)
		{
		}

		/// <summary>Gets or sets a value that indicates whether the reordering operation is stopped before completion.</summary>
		/// <returns>
		///     <see langword="true" /> if the reordering operation is stopped before completion; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011B4 RID: 4532
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x00149DD7 File Offset: 0x00147FD7
		// (set) Token: 0x0600488E RID: 18574 RVA: 0x00149DDF File Offset: 0x00147FDF
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

		/// <summary>Gets or sets the control that is used to display the visual indicator of the current drop location during a column drag operation.</summary>
		/// <returns>The control that is used to display the drop location indicator during a column drag operation.</returns>
		// Token: 0x170011B5 RID: 4533
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x00149DE8 File Offset: 0x00147FE8
		// (set) Token: 0x06004890 RID: 18576 RVA: 0x00149DF0 File Offset: 0x00147FF0
		public Control DropLocationIndicator
		{
			get
			{
				return this._dropLocationIndicator;
			}
			set
			{
				this._dropLocationIndicator = value;
			}
		}

		/// <summary>Gets or sets the control that is used to display the visual indicator of the header for the column that is being dragged.</summary>
		/// <returns>The control that is used to display a dragged column header.</returns>
		// Token: 0x170011B6 RID: 4534
		// (get) Token: 0x06004891 RID: 18577 RVA: 0x00149DF9 File Offset: 0x00147FF9
		// (set) Token: 0x06004892 RID: 18578 RVA: 0x00149E01 File Offset: 0x00148001
		public Control DragIndicator
		{
			get
			{
				return this._dragIndicator;
			}
			set
			{
				this._dragIndicator = value;
			}
		}

		// Token: 0x0400299E RID: 10654
		private bool _cancel;

		// Token: 0x0400299F RID: 10655
		private Control _dropLocationIndicator;

		// Token: 0x040029A0 RID: 10656
		private Control _dragIndicator;
	}
}
