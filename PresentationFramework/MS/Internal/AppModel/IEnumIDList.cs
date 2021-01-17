using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A7 RID: 1959
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F2-0000-0000-C000-000000000046")]
	[ComImport]
	internal interface IEnumIDList
	{
		// Token: 0x06007A69 RID: 31337
		[PreserveSig]
		HRESULT Next(uint celt, out IntPtr rgelt, out int pceltFetched);

		// Token: 0x06007A6A RID: 31338
		[PreserveSig]
		HRESULT Skip(uint celt);

		// Token: 0x06007A6B RID: 31339
		void Reset();

		// Token: 0x06007A6C RID: 31340
		[return: MarshalAs(UnmanagedType.Interface)]
		IEnumIDList Clone();
	}
}
