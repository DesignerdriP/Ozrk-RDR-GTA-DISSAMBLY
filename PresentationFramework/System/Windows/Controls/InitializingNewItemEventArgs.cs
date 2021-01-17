using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.InitializingNewItem" /> event.</summary>
	// Token: 0x020004E8 RID: 1256
	public class InitializingNewItemEventArgs : EventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.InitializingNewItemEventArgs" /> class. </summary>
		/// <param name="newItem">The new item added to the <see cref="T:System.Windows.Controls.DataGrid" />.</param>
		// Token: 0x06004E83 RID: 20099 RVA: 0x00161211 File Offset: 0x0015F411
		public InitializingNewItemEventArgs(object newItem)
		{
			this._newItem = newItem;
		}

		/// <summary>Gets the new item added to the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The new item added to the grid.</returns>
		// Token: 0x17001320 RID: 4896
		// (get) Token: 0x06004E84 RID: 20100 RVA: 0x00161220 File Offset: 0x0015F420
		public object NewItem
		{
			get
			{
				return this._newItem;
			}
		}

		// Token: 0x04002BCA RID: 11210
		private object _newItem;
	}
}
