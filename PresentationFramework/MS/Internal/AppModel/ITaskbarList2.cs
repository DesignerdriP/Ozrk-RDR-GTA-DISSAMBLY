using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BB RID: 1979
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("602D4995-B13A-429b-A66E-1935E44F4317")]
	[ComImport]
	internal interface ITaskbarList2 : ITaskbarList
	{
		// Token: 0x06007B22 RID: 31522
		void HrInit();

		// Token: 0x06007B23 RID: 31523
		void AddTab(IntPtr hwnd);

		// Token: 0x06007B24 RID: 31524
		void DeleteTab(IntPtr hwnd);

		// Token: 0x06007B25 RID: 31525
		void ActivateTab(IntPtr hwnd);

		// Token: 0x06007B26 RID: 31526
		void SetActiveAlt(IntPtr hwnd);

		// Token: 0x06007B27 RID: 31527
		void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
	}
}
