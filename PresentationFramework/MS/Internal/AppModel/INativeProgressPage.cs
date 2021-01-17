using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x02000794 RID: 1940
	[Guid("1f681651-1024-4798-af36-119bbe5e5665")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[ComImport]
	internal interface INativeProgressPage
	{
		// Token: 0x060079C0 RID: 31168
		[PreserveSig]
		HRESULT Show();

		// Token: 0x060079C1 RID: 31169
		[PreserveSig]
		HRESULT Hide();

		// Token: 0x060079C2 RID: 31170
		[PreserveSig]
		HRESULT ShowProgressMessage(string message);

		// Token: 0x060079C3 RID: 31171
		[PreserveSig]
		HRESULT SetApplicationName(string appName);

		// Token: 0x060079C4 RID: 31172
		[PreserveSig]
		HRESULT SetPublisherName(string publisherName);

		// Token: 0x060079C5 RID: 31173
		[PreserveSig]
		HRESULT OnDownloadProgress(ulong bytesDownloaded, ulong bytesTotal);
	}
}
