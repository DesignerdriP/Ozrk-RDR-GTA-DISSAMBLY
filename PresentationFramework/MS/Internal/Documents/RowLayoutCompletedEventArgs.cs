using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006ED RID: 1773
	internal class RowLayoutCompletedEventArgs : EventArgs
	{
		// Token: 0x060071F4 RID: 29172 RVA: 0x00209325 File Offset: 0x00207525
		public RowLayoutCompletedEventArgs(int pivotRowIndex)
		{
			this._pivotRowIndex = pivotRowIndex;
		}

		// Token: 0x17001B1C RID: 6940
		// (get) Token: 0x060071F5 RID: 29173 RVA: 0x00209334 File Offset: 0x00207534
		public int PivotRowIndex
		{
			get
			{
				return this._pivotRowIndex;
			}
		}

		// Token: 0x0400374D RID: 14157
		private readonly int _pivotRowIndex;
	}
}
