using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006EF RID: 1775
	internal class RowCacheChange
	{
		// Token: 0x060071F8 RID: 29176 RVA: 0x00209353 File Offset: 0x00207553
		public RowCacheChange(int start, int count)
		{
			this._start = start;
			this._count = count;
		}

		// Token: 0x17001B1E RID: 6942
		// (get) Token: 0x060071F9 RID: 29177 RVA: 0x00209369 File Offset: 0x00207569
		public int Start
		{
			get
			{
				return this._start;
			}
		}

		// Token: 0x17001B1F RID: 6943
		// (get) Token: 0x060071FA RID: 29178 RVA: 0x00209371 File Offset: 0x00207571
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x0400374F RID: 14159
		private readonly int _start;

		// Token: 0x04003750 RID: 14160
		private readonly int _count;
	}
}
