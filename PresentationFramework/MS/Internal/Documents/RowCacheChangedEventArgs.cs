using System;
using System.Collections.Generic;

namespace MS.Internal.Documents
{
	// Token: 0x020006EE RID: 1774
	internal class RowCacheChangedEventArgs : EventArgs
	{
		// Token: 0x060071F6 RID: 29174 RVA: 0x0020933C File Offset: 0x0020753C
		public RowCacheChangedEventArgs(List<RowCacheChange> changes)
		{
			this._changes = changes;
		}

		// Token: 0x17001B1D RID: 6941
		// (get) Token: 0x060071F7 RID: 29175 RVA: 0x0020934B File Offset: 0x0020754B
		public List<RowCacheChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x0400374E RID: 14158
		private readonly List<RowCacheChange> _changes;
	}
}
