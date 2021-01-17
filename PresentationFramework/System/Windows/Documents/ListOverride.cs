using System;

namespace System.Windows.Documents
{
	// Token: 0x020003CC RID: 972
	internal class ListOverride
	{
		// Token: 0x0600341F RID: 13343 RVA: 0x000E7C87 File Offset: 0x000E5E87
		internal ListOverride()
		{
			this._id = 0L;
			this._index = 0L;
			this._levels = null;
			this._nStartIndex = -1L;
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x000E7CAE File Offset: 0x000E5EAE
		// (set) Token: 0x06003421 RID: 13345 RVA: 0x000E7CB6 File Offset: 0x000E5EB6
		internal long ID
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x000E7CBF File Offset: 0x000E5EBF
		// (set) Token: 0x06003423 RID: 13347 RVA: 0x000E7CC7 File Offset: 0x000E5EC7
		internal long Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x000E7CD0 File Offset: 0x000E5ED0
		// (set) Token: 0x06003425 RID: 13349 RVA: 0x000E7CD8 File Offset: 0x000E5ED8
		internal ListLevelTable Levels
		{
			get
			{
				return this._levels;
			}
			set
			{
				this._levels = value;
			}
		}

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x000E7CE1 File Offset: 0x000E5EE1
		// (set) Token: 0x06003427 RID: 13351 RVA: 0x000E7CE9 File Offset: 0x000E5EE9
		internal long StartIndex
		{
			get
			{
				return this._nStartIndex;
			}
			set
			{
				this._nStartIndex = value;
			}
		}

		// Token: 0x040024C2 RID: 9410
		private long _id;

		// Token: 0x040024C3 RID: 9411
		private long _index;

		// Token: 0x040024C4 RID: 9412
		private long _nStartIndex;

		// Token: 0x040024C5 RID: 9413
		private ListLevelTable _levels;
	}
}
