using System;
using System.Windows;

namespace MS.Internal.Globalization
{
	// Token: 0x0200069F RID: 1695
	internal interface ILocalizabilityInheritable
	{
		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x06006E3A RID: 28218
		ILocalizabilityInheritable LocalizabilityAncestor { get; }

		// Token: 0x17001A2B RID: 6699
		// (get) Token: 0x06006E3B RID: 28219
		// (set) Token: 0x06006E3C RID: 28220
		LocalizabilityAttribute InheritableAttribute { get; set; }

		// Token: 0x17001A2C RID: 6700
		// (get) Token: 0x06006E3D RID: 28221
		// (set) Token: 0x06006E3E RID: 28222
		bool IsIgnored { get; set; }
	}
}
