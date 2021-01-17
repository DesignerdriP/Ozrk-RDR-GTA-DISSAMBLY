using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.Printing
{
	// Token: 0x02000657 RID: 1623
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06006BC9 RID: 27593
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("comdlg32.dll", CharSet = CharSet.Auto)]
		internal static extern int PrintDlgEx(IntPtr pdex);

		// Token: 0x06006BCA RID: 27594
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GlobalFree(IntPtr hMem);

		// Token: 0x06006BCB RID: 27595
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern IntPtr GlobalLock(IntPtr hMem);

		// Token: 0x06006BCC RID: 27596
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		internal static extern bool GlobalUnlock(IntPtr hMem);
	}
}
