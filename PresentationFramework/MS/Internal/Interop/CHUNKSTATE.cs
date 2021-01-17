using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000678 RID: 1656
	[Flags]
	internal enum CHUNKSTATE
	{
		// Token: 0x040035BC RID: 13756
		CHUNK_TEXT = 1,
		// Token: 0x040035BD RID: 13757
		CHUNK_VALUE = 2,
		// Token: 0x040035BE RID: 13758
		CHUNK_FILTER_OWNED_VALUE = 4
	}
}
