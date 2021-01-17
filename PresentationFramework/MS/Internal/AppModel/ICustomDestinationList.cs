using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B7 RID: 1975
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6332debf-87b5-4670-90c0-5e57b408a49e")]
	[ComImport]
	internal interface ICustomDestinationList
	{
		// Token: 0x06007B10 RID: 31504
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06007B11 RID: 31505
		[return: MarshalAs(UnmanagedType.Interface)]
		object BeginList(out uint pcMaxSlots, [In] ref Guid riid);

		// Token: 0x06007B12 RID: 31506
		[PreserveSig]
		HRESULT AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, IObjectArray poa);

		// Token: 0x06007B13 RID: 31507
		void AppendKnownCategory(KDC category);

		// Token: 0x06007B14 RID: 31508
		[PreserveSig]
		HRESULT AddUserTasks(IObjectArray poa);

		// Token: 0x06007B15 RID: 31509
		void CommitList();

		// Token: 0x06007B16 RID: 31510
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetRemovedDestinations([In] ref Guid riid);

		// Token: 0x06007B17 RID: 31511
		void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		// Token: 0x06007B18 RID: 31512
		void AbortList();
	}
}
