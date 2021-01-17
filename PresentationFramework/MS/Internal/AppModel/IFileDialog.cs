using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B2 RID: 1970
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("42f85136-db7e-439c-85f1-e4075d135fc8")]
	[ComImport]
	internal interface IFileDialog : IModalWindow
	{
		// Token: 0x06007ABC RID: 31420
		[PreserveSig]
		HRESULT Show(IntPtr parent);

		// Token: 0x06007ABD RID: 31421
		void SetFileTypes(uint cFileTypes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] [In] COMDLG_FILTERSPEC[] rgFilterSpec);

		// Token: 0x06007ABE RID: 31422
		void SetFileTypeIndex(uint iFileType);

		// Token: 0x06007ABF RID: 31423
		uint GetFileTypeIndex();

		// Token: 0x06007AC0 RID: 31424
		uint Advise(IFileDialogEvents pfde);

		// Token: 0x06007AC1 RID: 31425
		void Unadvise(uint dwCookie);

		// Token: 0x06007AC2 RID: 31426
		void SetOptions(FOS fos);

		// Token: 0x06007AC3 RID: 31427
		FOS GetOptions();

		// Token: 0x06007AC4 RID: 31428
		void SetDefaultFolder(IShellItem psi);

		// Token: 0x06007AC5 RID: 31429
		void SetFolder(IShellItem psi);

		// Token: 0x06007AC6 RID: 31430
		IShellItem GetFolder();

		// Token: 0x06007AC7 RID: 31431
		IShellItem GetCurrentSelection();

		// Token: 0x06007AC8 RID: 31432
		void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x06007AC9 RID: 31433
		[return: MarshalAs(UnmanagedType.LPWStr)]
		string GetFileName();

		// Token: 0x06007ACA RID: 31434
		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		// Token: 0x06007ACB RID: 31435
		void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		// Token: 0x06007ACC RID: 31436
		void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		// Token: 0x06007ACD RID: 31437
		IShellItem GetResult();

		// Token: 0x06007ACE RID: 31438
		void AddPlace(IShellItem psi, FDAP alignment);

		// Token: 0x06007ACF RID: 31439
		void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		// Token: 0x06007AD0 RID: 31440
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		// Token: 0x06007AD1 RID: 31441
		void SetClientGuid([In] ref Guid guid);

		// Token: 0x06007AD2 RID: 31442
		void ClearClientData();

		// Token: 0x06007AD3 RID: 31443
		void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);
	}
}
