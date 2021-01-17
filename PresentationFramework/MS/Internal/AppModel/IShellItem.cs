﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AC RID: 1964
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
	[ComImport]
	internal interface IShellItem
	{
		// Token: 0x06007A84 RID: 31364
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x06007A85 RID: 31365
		IShellItem GetParent();

		// Token: 0x06007A86 RID: 31366
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x06007A87 RID: 31367
		uint GetAttributes(SFGAO sfgaoMask);

		// Token: 0x06007A88 RID: 31368
		int Compare(IShellItem psi, SICHINT hint);
	}
}
