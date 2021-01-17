using System;
using System.Runtime.InteropServices;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A4 RID: 1956
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct COMDLG_FILTERSPEC
	{
		// Token: 0x04003A02 RID: 14850
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszName;

		// Token: 0x04003A03 RID: 14851
		[MarshalAs(UnmanagedType.LPWStr)]
		public string pszSpec;
	}
}
