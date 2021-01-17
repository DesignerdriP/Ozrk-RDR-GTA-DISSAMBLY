using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B0 RID: 1968
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("973510DB-7D7F-452B-8975-74A85828D354")]
	[ComImport]
	internal interface IFileDialogEvents
	{
		// Token: 0x06007AB4 RID: 31412
		[PreserveSig]
		HRESULT OnFileOk(IFileDialog pfd);

		// Token: 0x06007AB5 RID: 31413
		[PreserveSig]
		HRESULT OnFolderChanging(IFileDialog pfd, IShellItem psiFolder);

		// Token: 0x06007AB6 RID: 31414
		[PreserveSig]
		HRESULT OnFolderChange(IFileDialog pfd);

		// Token: 0x06007AB7 RID: 31415
		[PreserveSig]
		HRESULT OnSelectionChange(IFileDialog pfd);

		// Token: 0x06007AB8 RID: 31416
		[PreserveSig]
		HRESULT OnShareViolation(IFileDialog pfd, IShellItem psi, out FDESVR pResponse);

		// Token: 0x06007AB9 RID: 31417
		[PreserveSig]
		HRESULT OnTypeChange(IFileDialog pfd);

		// Token: 0x06007ABA RID: 31418
		[PreserveSig]
		HRESULT OnOverwrite(IFileDialog pfd, IShellItem psi, out FDEOR pResponse);
	}
}
