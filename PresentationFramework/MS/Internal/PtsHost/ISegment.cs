using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000646 RID: 1606
	internal interface ISegment
	{
		// Token: 0x06006A52 RID: 27218
		void GetFirstPara(out int successful, out IntPtr firstParaName);

		// Token: 0x06006A53 RID: 27219
		void GetNextPara(BaseParagraph currentParagraph, out int found, out IntPtr nextParaName);

		// Token: 0x06006A54 RID: 27220
		void UpdGetFirstChangeInSegment(out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange);
	}
}
