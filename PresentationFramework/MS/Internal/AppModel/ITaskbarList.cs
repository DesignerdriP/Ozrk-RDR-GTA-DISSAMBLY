using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BA RID: 1978
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("56FDF342-FD6D-11d0-958A-006097C9A090")]
	[ComImport]
	internal interface ITaskbarList
	{
		// Token: 0x06007B1D RID: 31517
		void HrInit();

		// Token: 0x06007B1E RID: 31518
		void AddTab(IntPtr hwnd);

		// Token: 0x06007B1F RID: 31519
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06007B20 RID: 31520
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06007B21 RID: 31521
		void SetActiveAlt(IntPtr hwnd);
	}
}
