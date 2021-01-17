using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AF RID: 1967
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F9-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IShellLinkW
	{
		// Token: 0x06007AA2 RID: 31394
		void GetPath([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxPath, [In] [Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

		// Token: 0x06007AA3 RID: 31395
		IntPtr GetIDList();

		// Token: 0x06007AA4 RID: 31396
		void SetIDList(IntPtr pidl);

		// Token: 0x06007AA5 RID: 31397
		void GetDescription([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszFile, int cchMaxName);

		// Token: 0x06007AA6 RID: 31398
		void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x06007AA7 RID: 31399
		void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszDir, int cchMaxPath);

		// Token: 0x06007AA8 RID: 31400
		void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

		// Token: 0x06007AA9 RID: 31401
		void GetArguments([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszArgs, int cchMaxPath);

		// Token: 0x06007AAA RID: 31402
		void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

		// Token: 0x06007AAB RID: 31403
		short GetHotKey();

		// Token: 0x06007AAC RID: 31404
		void SetHotKey(short wHotKey);

		// Token: 0x06007AAD RID: 31405
		uint GetShowCmd();

		// Token: 0x06007AAE RID: 31406
		void SetShowCmd(uint iShowCmd);

		// Token: 0x06007AAF RID: 31407
		void GetIconLocation([MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

		// Token: 0x06007AB0 RID: 31408
		void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

		// Token: 0x06007AB1 RID: 31409
		void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

		// Token: 0x06007AB2 RID: 31410
		void Resolve(IntPtr hwnd, uint fFlags);

		// Token: 0x06007AB3 RID: 31411
		void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
	}
}
