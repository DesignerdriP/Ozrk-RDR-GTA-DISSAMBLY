using System;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.DataGrid.AddingNewItem" /> event.</summary>
	// Token: 0x0200046B RID: 1131
	public class AddingNewItemEventArgs : EventArgs
	{
		/// <summary>Gets or sets the item that will be added.</summary>
		/// <returns>The item that will be added.</returns>
		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x0600421C RID: 16924 RVA: 0x0012E866 File Offset: 0x0012CA66
		// (set) Token: 0x0600421D RID: 16925 RVA: 0x0012E86E File Offset: 0x0012CA6E
		public object NewItem
		{
			get
			{
				return this._newItem;
			}
			set
			{
				this._newItem = value;
			}
		}

		// Token: 0x040027D3 RID: 10195
		private object _newItem;
	}
}
