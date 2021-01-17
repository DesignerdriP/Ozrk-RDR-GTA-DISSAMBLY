using System;

namespace System.Windows.Controls
{
	// Token: 0x02000536 RID: 1334
	internal interface IStackMeasure
	{
		// Token: 0x17001500 RID: 5376
		// (get) Token: 0x06005667 RID: 22119
		bool IsScrolling { get; }

		// Token: 0x17001501 RID: 5377
		// (get) Token: 0x06005668 RID: 22120
		UIElementCollection InternalChildren { get; }

		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x06005669 RID: 22121
		Orientation Orientation { get; }

		// Token: 0x17001503 RID: 5379
		// (get) Token: 0x0600566A RID: 22122
		bool CanVerticallyScroll { get; }

		// Token: 0x17001504 RID: 5380
		// (get) Token: 0x0600566B RID: 22123
		bool CanHorizontallyScroll { get; }

		// Token: 0x0600566C RID: 22124
		void OnScrollChange();
	}
}
