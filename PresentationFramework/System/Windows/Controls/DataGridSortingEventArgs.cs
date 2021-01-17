using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.Sorting" /> event. </summary>
	// Token: 0x020004BC RID: 1212
	public class DataGridSortingEventArgs : DataGridColumnEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGridSortingEventArgs" /> class.</summary>
		/// <param name="column">The column that is being sorted.</param>
		// Token: 0x06004998 RID: 18840 RVA: 0x00149DCE File Offset: 0x00147FCE
		public DataGridSortingEventArgs(DataGridColumn column) : base(column)
		{
		}

		/// <summary>Gets or sets a value that specifies whether the routed event is handled.</summary>
		/// <returns>
		///     <see langword="true" /> if the event has been handled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x170011FB RID: 4603
		// (get) Token: 0x06004999 RID: 18841 RVA: 0x0014D095 File Offset: 0x0014B295
		// (set) Token: 0x0600499A RID: 18842 RVA: 0x0014D09D File Offset: 0x0014B29D
		public bool Handled
		{
			get
			{
				return this._handled;
			}
			set
			{
				this._handled = value;
			}
		}

		// Token: 0x04002A22 RID: 10786
		private bool _handled;
	}
}
