using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C7 RID: 1991
	[Flags]
	internal enum AttachmentLevel
	{
		// Token: 0x04003A1F RID: 14879
		Full = 7,
		// Token: 0x04003A20 RID: 14880
		StartPortion = 4,
		// Token: 0x04003A21 RID: 14881
		MiddlePortion = 2,
		// Token: 0x04003A22 RID: 14882
		EndPortion = 1,
		// Token: 0x04003A23 RID: 14883
		Incomplete = 256,
		// Token: 0x04003A24 RID: 14884
		Unresolved = 0
	}
}
