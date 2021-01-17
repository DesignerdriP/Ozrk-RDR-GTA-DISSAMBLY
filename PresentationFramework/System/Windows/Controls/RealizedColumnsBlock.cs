using System;

namespace System.Windows.Controls
{
	// Token: 0x0200051F RID: 1311
	internal struct RealizedColumnsBlock
	{
		// Token: 0x060054AD RID: 21677 RVA: 0x00177156 File Offset: 0x00175356
		public RealizedColumnsBlock(int startIndex, int endIndex, int startIndexOffset)
		{
			this = default(RealizedColumnsBlock);
			this.StartIndex = startIndex;
			this.EndIndex = endIndex;
			this.StartIndexOffset = startIndexOffset;
		}

		// Token: 0x17001495 RID: 5269
		// (get) Token: 0x060054AE RID: 21678 RVA: 0x00177174 File Offset: 0x00175374
		// (set) Token: 0x060054AF RID: 21679 RVA: 0x0017717C File Offset: 0x0017537C
		public int StartIndex { get; private set; }

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x060054B0 RID: 21680 RVA: 0x00177185 File Offset: 0x00175385
		// (set) Token: 0x060054B1 RID: 21681 RVA: 0x0017718D File Offset: 0x0017538D
		public int EndIndex { get; private set; }

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x060054B2 RID: 21682 RVA: 0x00177196 File Offset: 0x00175396
		// (set) Token: 0x060054B3 RID: 21683 RVA: 0x0017719E File Offset: 0x0017539E
		public int StartIndexOffset { get; private set; }
	}
}
