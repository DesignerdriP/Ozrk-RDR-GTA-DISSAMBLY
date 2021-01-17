using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000775 RID: 1909
	[Flags]
	internal enum HostingFlags
	{
		// Token: 0x04003937 RID: 14647
		hfHostedInIE = 1,
		// Token: 0x04003938 RID: 14648
		hfHostedInWebOC = 2,
		// Token: 0x04003939 RID: 14649
		hfHostedInIEorWebOC = 3,
		// Token: 0x0400393A RID: 14650
		hfHostedInMozilla = 4,
		// Token: 0x0400393B RID: 14651
		hfHostedInFrame = 8,
		// Token: 0x0400393C RID: 14652
		hfIsBrowserLowIntegrityProcess = 16,
		// Token: 0x0400393D RID: 14653
		hfInDebugMode = 32
	}
}
