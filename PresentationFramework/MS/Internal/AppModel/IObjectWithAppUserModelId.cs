﻿using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B8 RID: 1976
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
	[ComImport]
	internal interface IObjectWithAppUserModelId
	{
		// Token: 0x06007B19 RID: 31513
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06007B1A RID: 31514
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetAppID();
	}
}
