using System;

namespace System.Windows.Controls
{
	// Token: 0x02000537 RID: 1335
	internal interface IStackMeasureScrollData
	{
		// Token: 0x17001505 RID: 5381
		// (get) Token: 0x0600566D RID: 22125
		// (set) Token: 0x0600566E RID: 22126
		Vector Offset { get; set; }

		// Token: 0x17001506 RID: 5382
		// (get) Token: 0x0600566F RID: 22127
		// (set) Token: 0x06005670 RID: 22128
		Size Viewport { get; set; }

		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x06005671 RID: 22129
		// (set) Token: 0x06005672 RID: 22130
		Size Extent { get; set; }

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x06005673 RID: 22131
		// (set) Token: 0x06005674 RID: 22132
		Vector ComputedOffset { get; set; }

		// Token: 0x06005675 RID: 22133
		void SetPhysicalViewport(double value);
	}
}
