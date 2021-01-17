using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A9 RID: 1961
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9")]
	[ComImport]
	internal interface IObjectCollection : IObjectArray
	{
		// Token: 0x06007A6F RID: 31343
		uint GetCount();

		// Token: 0x06007A70 RID: 31344
		[return: MarshalAs(UnmanagedType.IUnknown)]
		object GetAt([In] uint uiIndex, [In] ref Guid riid);

		// Token: 0x06007A71 RID: 31345
		void AddObject([MarshalAs(UnmanagedType.IUnknown)] object punk);

		// Token: 0x06007A72 RID: 31346
		void AddFromArray(IObjectArray poaSource);

		// Token: 0x06007A73 RID: 31347
		void RemoveObjectAt(uint uiIndex);

		// Token: 0x06007A74 RID: 31348
		void Clear();
	}
}
