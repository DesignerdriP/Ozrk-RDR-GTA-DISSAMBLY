using System;

namespace System.Windows.Data
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Data.BindingOperations.CollectionViewRegistering" /> event.</summary>
	// Token: 0x020001A9 RID: 425
	public class CollectionViewRegisteringEventArgs : EventArgs
	{
		// Token: 0x06001ADE RID: 6878 RVA: 0x0007F3E1 File Offset: 0x0007D5E1
		internal CollectionViewRegisteringEventArgs(CollectionView view)
		{
			this._view = view;
		}

		/// <summary>Gets the collection view to be registered for cross-thread access.</summary>
		/// <returns>The collection view to be registered for cross-thread access.</returns>
		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x0007F3F0 File Offset: 0x0007D5F0
		public CollectionView CollectionView
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x04001379 RID: 4985
		private CollectionView _view;
	}
}
