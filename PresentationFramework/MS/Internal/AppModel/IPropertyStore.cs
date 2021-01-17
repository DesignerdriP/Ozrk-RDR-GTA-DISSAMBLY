using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007AA RID: 1962
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
	[ComImport]
	internal interface IPropertyStore
	{
		// Token: 0x06007A75 RID: 31349
		uint GetCount();

		// Token: 0x06007A76 RID: 31350
		PKEY GetAt(uint iProp);

		// Token: 0x06007A77 RID: 31351
		[SecurityCritical]
		void GetValue([In] ref PKEY pkey, [In] [Out] PROPVARIANT pv);

		// Token: 0x06007A78 RID: 31352
		[SecurityCritical]
		void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

		// Token: 0x06007A79 RID: 31353
		void Commit();
	}
}
