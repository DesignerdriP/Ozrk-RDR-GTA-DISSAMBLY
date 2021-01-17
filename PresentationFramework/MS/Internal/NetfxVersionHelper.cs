using System;
using System.IO;
using System.Runtime.Versioning;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace MS.Internal
{
	// Token: 0x020005DC RID: 1500
	internal static class NetfxVersionHelper
	{
		// Token: 0x060063A5 RID: 25509 RVA: 0x001C0830 File Offset: 0x001BEA30
		[SecuritySafeCritical]
		internal static int GetNetFXReleaseVersion()
		{
			int result = 0;
			RegistryPermission registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, NetfxVersionHelper._frameworkRegKeyFullPath);
			try
			{
				registryPermission.Assert();
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(NetfxVersionHelper._frameworkRegKey);
				if (registryKey != null)
				{
					object value = registryKey.GetValue("Release");
					if (value != null)
					{
						result = Convert.ToInt32(value);
					}
				}
			}
			catch (Exception ex) when (ex is SecurityException || ex is ObjectDisposedException || ex is IOException || ex is UnauthorizedAccessException || ex is FormatException || ex is OverflowException)
			{
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x060063A6 RID: 25510 RVA: 0x001C08F0 File Offset: 0x001BEAF0
		internal static string GetTargetFrameworkVersion()
		{
			string result = string.Empty;
			string targetFrameworkName = AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
			if (!string.IsNullOrEmpty(targetFrameworkName))
			{
				try
				{
					FrameworkName frameworkName = new FrameworkName(targetFrameworkName);
					result = frameworkName.Version.ToString();
				}
				catch (Exception ex) when (ex is ArgumentException)
				{
				}
			}
			return result;
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x060063A7 RID: 25511 RVA: 0x001C0960 File Offset: 0x001BEB60
		internal static string FrameworkRegKey
		{
			get
			{
				return NetfxVersionHelper._frameworkRegKey;
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x060063A8 RID: 25512 RVA: 0x001C0967 File Offset: 0x001BEB67
		internal static string FrameworkRegKeyFullPath
		{
			get
			{
				return NetfxVersionHelper._frameworkRegKeyFullPath;
			}
		}

		// Token: 0x040031EB RID: 12779
		private static readonly string _frameworkRegKey = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full";

		// Token: 0x040031EC RID: 12780
		private static readonly string _frameworkRegKeyFullPath = "HKEY_LOCAL_MACHINE\\" + NetfxVersionHelper._frameworkRegKey;
	}
}
