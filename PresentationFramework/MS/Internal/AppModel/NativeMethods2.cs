using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BF RID: 1983
	internal static class NativeMethods2
	{
		// Token: 0x06007B50 RID: 31568
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void SHAddToRecentDocsString(SHARD uFlags, [MarshalAs(UnmanagedType.LPWStr)] string pv);

		// Token: 0x06007B51 RID: 31569
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll", EntryPoint = "SHAddToRecentDocs")]
		private static extern void SHAddToRecentDocs_ShellLink(SHARD uFlags, IShellLinkW pv);

		// Token: 0x06007B52 RID: 31570 RVA: 0x0022AFD8 File Offset: 0x002291D8
		[SecurityCritical]
		internal static void SHAddToRecentDocs(string path)
		{
			NativeMethods2.SHAddToRecentDocsString(SHARD.PATHW, path);
		}

		// Token: 0x06007B53 RID: 31571 RVA: 0x0022AFE1 File Offset: 0x002291E1
		[SecurityCritical]
		internal static void SHAddToRecentDocs(IShellLinkW shellLink)
		{
			NativeMethods2.SHAddToRecentDocs_ShellLink(SHARD.LINK, shellLink);
		}

		// Token: 0x06007B54 RID: 31572
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll")]
		internal static extern HRESULT SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IBindCtx pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x06007B55 RID: 31573
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("shell32.dll")]
		internal static extern HRESULT SHGetFolderPathEx([In] ref Guid rfid, KF_FLAG dwFlags, [In] [Optional] IntPtr hToken, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszPath, uint cchPath);

		// Token: 0x06007B56 RID: 31574
		[DllImport("shell32.dll", PreserveSig = false)]
		internal static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		// Token: 0x06007B57 RID: 31575
		[DllImport("shell32.dll")]
		internal static extern HRESULT GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);
	}
}
