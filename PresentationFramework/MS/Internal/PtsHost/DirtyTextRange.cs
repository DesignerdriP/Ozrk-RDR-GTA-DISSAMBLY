using System;
using System.Windows.Documents;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000616 RID: 1558
	internal struct DirtyTextRange
	{
		// Token: 0x060067A7 RID: 26535 RVA: 0x001D0DDB File Offset: 0x001CEFDB
		internal DirtyTextRange(int startIndex, int positionsAdded, int positionsRemoved, bool fromHighlightLayer = false)
		{
			this.StartIndex = startIndex;
			this.PositionsAdded = positionsAdded;
			this.PositionsRemoved = positionsRemoved;
			this.FromHighlightLayer = fromHighlightLayer;
		}

		// Token: 0x060067A8 RID: 26536 RVA: 0x001D0DFC File Offset: 0x001CEFFC
		internal DirtyTextRange(TextContainerChangeEventArgs change)
		{
			this.StartIndex = change.ITextPosition.Offset;
			this.PositionsAdded = 0;
			this.PositionsRemoved = 0;
			this.FromHighlightLayer = false;
			switch (change.TextChange)
			{
			case TextChangeType.ContentAdded:
				this.PositionsAdded = change.Count;
				return;
			case TextChangeType.ContentRemoved:
				this.PositionsRemoved = change.Count;
				return;
			case TextChangeType.PropertyModified:
				this.PositionsAdded = change.Count;
				this.PositionsRemoved = change.Count;
				return;
			default:
				return;
			}
		}

		// Token: 0x17001912 RID: 6418
		// (get) Token: 0x060067A9 RID: 26537 RVA: 0x001D0E7B File Offset: 0x001CF07B
		// (set) Token: 0x060067AA RID: 26538 RVA: 0x001D0E83 File Offset: 0x001CF083
		internal int StartIndex { get; set; }

		// Token: 0x17001913 RID: 6419
		// (get) Token: 0x060067AB RID: 26539 RVA: 0x001D0E8C File Offset: 0x001CF08C
		// (set) Token: 0x060067AC RID: 26540 RVA: 0x001D0E94 File Offset: 0x001CF094
		internal int PositionsAdded { get; set; }

		// Token: 0x17001914 RID: 6420
		// (get) Token: 0x060067AD RID: 26541 RVA: 0x001D0E9D File Offset: 0x001CF09D
		// (set) Token: 0x060067AE RID: 26542 RVA: 0x001D0EA5 File Offset: 0x001CF0A5
		internal int PositionsRemoved { get; set; }

		// Token: 0x17001915 RID: 6421
		// (get) Token: 0x060067AF RID: 26543 RVA: 0x001D0EAE File Offset: 0x001CF0AE
		// (set) Token: 0x060067B0 RID: 26544 RVA: 0x001D0EB6 File Offset: 0x001CF0B6
		internal bool FromHighlightLayer { get; set; }
	}
}
