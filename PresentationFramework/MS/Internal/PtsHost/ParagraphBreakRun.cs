using System;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000642 RID: 1602
	internal sealed class ParagraphBreakRun : TextEndOfParagraph
	{
		// Token: 0x06006A39 RID: 27193 RVA: 0x001E3CE2 File Offset: 0x001E1EE2
		internal ParagraphBreakRun(int length, PTS.FSFLRES breakReason) : base(length)
		{
			this.BreakReason = breakReason;
		}

		// Token: 0x04003430 RID: 13360
		internal readonly PTS.FSFLRES BreakReason;
	}
}
