using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x02000746 RID: 1862
	internal class ViewRecord
	{
		// Token: 0x060076E7 RID: 30439 RVA: 0x0021FA48 File Offset: 0x0021DC48
		internal ViewRecord(ICollectionView view)
		{
			this._view = view;
			this._version = -1;
		}

		// Token: 0x17001C43 RID: 7235
		// (get) Token: 0x060076E8 RID: 30440 RVA: 0x0021FA5E File Offset: 0x0021DC5E
		internal ICollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x17001C44 RID: 7236
		// (get) Token: 0x060076E9 RID: 30441 RVA: 0x0021FA66 File Offset: 0x0021DC66
		// (set) Token: 0x060076EA RID: 30442 RVA: 0x0021FA6E File Offset: 0x0021DC6E
		internal int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x17001C45 RID: 7237
		// (get) Token: 0x060076EB RID: 30443 RVA: 0x0021FA77 File Offset: 0x0021DC77
		internal bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
		}

		// Token: 0x060076EC RID: 30444 RVA: 0x0021FA7F File Offset: 0x0021DC7F
		internal void InitializeView()
		{
			this._view.MoveCurrentToFirst();
			this._isInitialized = true;
		}

		// Token: 0x0400389A RID: 14490
		private ICollectionView _view;

		// Token: 0x0400389B RID: 14491
		private int _version;

		// Token: 0x0400389C RID: 14492
		private bool _isInitialized;
	}
}
