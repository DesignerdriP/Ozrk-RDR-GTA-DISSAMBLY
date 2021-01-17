using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006DC RID: 1756
	internal class PageCacheChange
	{
		// Token: 0x06007162 RID: 29026 RVA: 0x00207145 File Offset: 0x00205345
		public PageCacheChange(int start, int count, PageCacheChangeType type)
		{
			this._start = start;
			this._count = count;
			this._type = type;
		}

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x06007163 RID: 29027 RVA: 0x00207162 File Offset: 0x00205362
		public int Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x06007164 RID: 29028 RVA: 0x0020716A File Offset: 0x0020536A
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x06007165 RID: 29029 RVA: 0x00207172 File Offset: 0x00205372
		public PageCacheChangeType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x04003718 RID: 14104
		private readonly int _start;

		// Token: 0x04003719 RID: 14105
		private readonly int _count;

		// Token: 0x0400371A RID: 14106
		private readonly PageCacheChangeType _type;
	}
}
