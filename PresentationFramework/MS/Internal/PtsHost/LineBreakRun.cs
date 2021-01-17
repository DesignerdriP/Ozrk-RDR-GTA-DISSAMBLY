using System;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000643 RID: 1603
	internal sealed class LineBreakRun : TextEndOfLine
	{
		// Token: 0x06006A3A RID: 27194 RVA: 0x001E3CF2 File Offset: 0x001E1EF2
		internal LineBreakRun(int length, PTS.FSFLRES breakReason) : base(length)
		{
			this.BreakReason = breakReason;
		}

		// Token: 0x04003431 RID: 13361
		internal readonly PTS.FSFLRES BreakReason;
	}
}
