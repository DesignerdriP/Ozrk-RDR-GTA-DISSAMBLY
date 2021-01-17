using System;

namespace System.Windows.Documents
{
	// Token: 0x020003B9 RID: 953
	internal static class Validators
	{
		// Token: 0x060032D6 RID: 13014 RVA: 0x000E5112 File Offset: 0x000E3312
		internal static bool IsValidFontSize(long fs)
		{
			return fs >= 0L && fs <= 32767L;
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x000E5127 File Offset: 0x000E3327
		internal static bool IsValidWidthType(long wt)
		{
			return wt >= 0L && wt <= 3L;
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x000E5138 File Offset: 0x000E3338
		internal static long MakeValidShading(long s)
		{
			if (s > 10000L)
			{
				s = 10000L;
			}
			return s;
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x000E514C File Offset: 0x000E334C
		internal static long MakeValidBorderWidth(long w)
		{
			if (w < 0L)
			{
				w = 0L;
			}
			if (w > 1440L)
			{
				w = 1440L;
			}
			return w;
		}
	}
}
