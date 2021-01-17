using System;
using System.Collections.Generic;

namespace MS.Internal.Documents
{
	// Token: 0x020006DB RID: 1755
	internal class PageCacheChangedEventArgs : EventArgs
	{
		// Token: 0x06007160 RID: 29024 RVA: 0x0020712E File Offset: 0x0020532E
		public PageCacheChangedEventArgs(List<PageCacheChange> changes)
		{
			this._changes = changes;
		}

		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x06007161 RID: 29025 RVA: 0x0020713D File Offset: 0x0020533D
		public List<PageCacheChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x04003717 RID: 14103
		private readonly List<PageCacheChange> _changes;
	}
}
