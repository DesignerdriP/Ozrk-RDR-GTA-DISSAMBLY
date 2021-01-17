﻿using System;
using System.Security;
using System.Text;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007BE RID: 1982
	internal static class ShellUtil
	{
		// Token: 0x06007B4D RID: 31565 RVA: 0x0022AF0F File Offset: 0x0022910F
		[SecurityCritical]
		public static string GetPathFromShellItem(IShellItem item)
		{
			return item.GetDisplayName((SIGDN)2147647488U);
		}

		// Token: 0x06007B4E RID: 31566 RVA: 0x0022AF1C File Offset: 0x0022911C
		[SecurityCritical]
		public static string GetPathForKnownFolder(Guid knownFolder)
		{
			if (knownFolder == default(Guid))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(260);
			if (!NativeMethods2.SHGetFolderPathEx(ref knownFolder, KF_FLAG.DEFAULT, IntPtr.Zero, stringBuilder, (uint)stringBuilder.Capacity).Succeeded)
			{
				return null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007B4F RID: 31567 RVA: 0x0022AF70 File Offset: 0x00229170
		[SecurityCritical]
		public static IShellItem2 GetShellItemForPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			Guid guid = new Guid("7e9fb0d3-919f-4307-ab2e-9b1860310c93");
			object obj;
			HRESULT hrLeft = NativeMethods2.SHCreateItemFromParsingName(path, null, ref guid, out obj);
			if (hrLeft == (HRESULT)Win32Error.ERROR_FILE_NOT_FOUND || hrLeft == (HRESULT)Win32Error.ERROR_PATH_NOT_FOUND)
			{
				hrLeft = HRESULT.S_OK;
				obj = null;
			}
			hrLeft.ThrowIfFailed();
			return (IShellItem2)obj;
		}
	}
}
