using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000679 RID: 1657
	internal struct STAT_CHUNK
	{
		// Token: 0x040035BF RID: 13759
		internal uint idChunk;

		// Token: 0x040035C0 RID: 13760
		internal CHUNK_BREAKTYPE breakType;

		// Token: 0x040035C1 RID: 13761
		internal CHUNKSTATE flags;

		// Token: 0x040035C2 RID: 13762
		internal uint locale;

		// Token: 0x040035C3 RID: 13763
		internal FULLPROPSPEC attribute;

		// Token: 0x040035C4 RID: 13764
		internal uint idChunkSource;

		// Token: 0x040035C5 RID: 13765
		internal uint cwcStartSource;

		// Token: 0x040035C6 RID: 13766
		internal uint cwcLenSource;
	}
}
