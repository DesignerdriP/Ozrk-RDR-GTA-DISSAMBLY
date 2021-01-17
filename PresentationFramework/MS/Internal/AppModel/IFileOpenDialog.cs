﻿using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007B3 RID: 1971
	[SecurityCritical(SecurityCriticalScope.Everything)]
	[SuppressUnmanagedCodeSecurity]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d57c7288-d4ad-4768-be02-9d969532d960")]
	[ComImport]
	internal interface IFileOpenDialog : IFileDialog, IModalWindow
	{
		// Token: 0x06007AD4 RID: 31444
		[PreserveSig]
		HRESULT Show(IntPtr parent);

		// Token: 0x06007AD5 RID: 31445
		void SetFileTypes(uint cFileTypes, [In] COMDLG_FILTERSPEC[] rgFilterSpec);

		// Token: 0x06007AD6 RID: 31446
		void SetFileTypeIndex(uint iFileType);

		// Token: 0x06007AD7 RID: 31447
		uint GetFileTypeIndex();

		// Token: 0x06007AD8 RID: 31448
		uint Advise(IFileDialogEvents pfde);

		// Token: 0x06007AD9 RID: 31449
		void Unadvise(uint dwCookie);

		// Token: 0x06007ADA RID: 31450
		void SetOptions(FOS fos);

		// Token: 0x06007ADB RID: 31451
		FOS GetOptions();

		// Token: 0x06007ADC RID: 31452
		void SetDefaultFolder(IShellItem psi);

		// Token: 0x06007ADD RID: 31453
		void SetFolder(IShellItem psi);

		// Token: 0x06007ADE RID: 31454
		IShellItem GetFolder();

		// Token: 0x06007ADF RID: 31455
		IShellItem GetCurrentSelection();

		// Token: 0x06007AE0 RID: 31456
		void SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);

		// Token: 0x06007AE1 RID: 31457
		[return: MarshalAs(UnmanagedType.LPWStr)]
		void GetFileName();

		// Token: 0x06007AE2 RID: 31458
		void SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		// Token: 0x06007AE3 RID: 31459
		void SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);

		// Token: 0x06007AE4 RID: 31460
		void SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		// Token: 0x06007AE5 RID: 31461
		IShellItem GetResult();

		// Token: 0x06007AE6 RID: 31462
		void AddPlace(IShellItem psi, FDAP fdcp);

		// Token: 0x06007AE7 RID: 31463
		void SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		// Token: 0x06007AE8 RID: 31464
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		// Token: 0x06007AE9 RID: 31465
		void SetClientGuid([In] ref Guid guid);

		// Token: 0x06007AEA RID: 31466
		void ClearClientData();

		// Token: 0x06007AEB RID: 31467
		void SetFilter([MarshalAs(UnmanagedType.Interface)] object pFilter);

		// Token: 0x06007AEC RID: 31468
		IShellItemArray GetResults();

		// Token: 0x06007AED RID: 31469
		IShellItemArray GetSelectedItems();
	}
}
