using System;

namespace MS.Internal.Data
{
	// Token: 0x0200070D RID: 1805
	internal class AbandonedGroupItem
	{
		// Token: 0x060073DE RID: 29662 RVA: 0x00212A22 File Offset: 0x00210C22
		public AbandonedGroupItem(LiveShapingItem lsi, CollectionViewGroupInternal group)
		{
			this._lsi = lsi;
			this._group = group;
		}

		// Token: 0x17001B87 RID: 7047
		// (get) Token: 0x060073DF RID: 29663 RVA: 0x00212A38 File Offset: 0x00210C38
		public LiveShapingItem Item
		{
			get
			{
				return this._lsi;
			}
		}

		// Token: 0x17001B88 RID: 7048
		// (get) Token: 0x060073E0 RID: 29664 RVA: 0x00212A40 File Offset: 0x00210C40
		public CollectionViewGroupInternal Group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x040037BF RID: 14271
		private LiveShapingItem _lsi;

		// Token: 0x040037C0 RID: 14272
		private CollectionViewGroupInternal _group;
	}
}
