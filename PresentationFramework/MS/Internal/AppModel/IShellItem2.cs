using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AD RID: 1965
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93")]
	[ComImport]
	internal interface IShellItem2 : IShellItem
	{
		// Token: 0x06007A89 RID: 31369
		[return: MarshalAs(UnmanagedType.Interface)]
		object BindToHandler(IBindCtx pbc, [In] ref Guid bhid, [In] ref Guid riid);

		// Token: 0x06007A8A RID: 31370
		IShellItem GetParent();

		// Token: 0x06007A8B RID: 31371
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetDisplayName(SIGDN sigdnName);

		// Token: 0x06007A8C RID: 31372
		SFGAO GetAttributes(SFGAO sfgaoMask);

		// Token: 0x06007A8D RID: 31373
		int Compare(IShellItem psi, SICHINT hint);

		// Token: 0x06007A8E RID: 31374
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStore(GPS flags, [In] ref Guid riid);

		// Token: 0x06007A8F RID: 31375
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreWithCreateObject(GPS flags, [MarshalAs(UnmanagedType.IUnknown)] object punkCreateObject, [In] ref Guid riid);

		// Token: 0x06007A90 RID: 31376
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyStoreForKeys(IntPtr rgKeys, uint cKeys, GPS flags, [In] ref Guid riid);

		// Token: 0x06007A91 RID: 31377
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetPropertyDescriptionList(IntPtr keyType, [In] ref Guid riid);

		// Token: 0x06007A92 RID: 31378
		void Update(IBindCtx pbc);

		// Token: 0x06007A93 RID: 31379
		[SecurityCritical]
		void GetProperty(IntPtr key, [In] [Out] PROPVARIANT pv);

		// Token: 0x06007A94 RID: 31380
		Guid GetCLSID(IntPtr key);

		// Token: 0x06007A95 RID: 31381
		System.Runtime.InteropServices.ComTypes.FILETIME GetFileTime(IntPtr key);

		// Token: 0x06007A96 RID: 31382
		int GetInt32(IntPtr key);

		// Token: 0x06007A97 RID: 31383
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetString(IntPtr key);

		// Token: 0x06007A98 RID: 31384
		uint GetUInt32(IntPtr key);

		// Token: 0x06007A99 RID: 31385
		ulong GetUInt64(IntPtr key);

		// Token: 0x06007A9A RID: 31386
		[return: MarshalAs(UnmanagedType.Bool)]
		bool GetBool(IntPtr key);
	}
}
