using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using MS.Win32;

namespace MS.Internal
{
	// Token: 0x020005E5 RID: 1509
	internal class AssemblyFilter
	{
		// Token: 0x06006477 RID: 25719 RVA: 0x001C308A File Offset: 0x001C128A
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static AssemblyFilter()
		{
			AssemblyFilter._disallowedListExtracted = new SecurityCriticalDataForSet<bool>(false);
			AssemblyFilter._assemblyList = new SecurityCriticalDataForSet<List<string>>(new List<string>());
		}

		// Token: 0x06006478 RID: 25720 RVA: 0x001C30B0 File Offset: 0x001C12B0
		[SecurityCritical]
		internal void FilterCallback(object sender, AssemblyLoadEventArgs args)
		{
			object @lock = AssemblyFilter._lock;
			lock (@lock)
			{
				Assembly loadedAssembly = args.LoadedAssembly;
				if (!loadedAssembly.ReflectionOnly && loadedAssembly.GlobalAssemblyCache)
				{
					object[] customAttributes = loadedAssembly.GetCustomAttributes(typeof(AllowPartiallyTrustedCallersAttribute), false);
					if (customAttributes.Length != 0 && customAttributes[0] is AllowPartiallyTrustedCallersAttribute)
					{
						string text = this.AssemblyNameWithFileVersion(loadedAssembly);
						if (this.AssemblyOnDisallowedList(text))
						{
							UnsafeNativeMethods.ProcessUnhandledException_DLL(SR.Get("KillBitEnforcedShutdown") + text);
							new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
							try
							{
								Environment.Exit(-1);
							}
							finally
							{
								CodeAccessPermission.RevertAssert();
							}
						}
					}
				}
			}
		}

		// Token: 0x06006479 RID: 25721 RVA: 0x001C3170 File Offset: 0x001C1370
		[SecurityCritical]
		private string AssemblyNameWithFileVersion(Assembly a)
		{
			StringBuilder stringBuilder = new StringBuilder(a.FullName);
			new FileIOPermission(PermissionState.Unrestricted).Assert();
			FileVersionInfo versionInfo;
			try
			{
				versionInfo = FileVersionInfo.GetVersionInfo(a.Location);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (versionInfo != null && versionInfo.ProductVersion != null)
			{
				stringBuilder.Append(", FileVersion=" + versionInfo.ProductVersion);
			}
			return stringBuilder.ToString().ToLower(CultureInfo.InvariantCulture).Trim();
		}

		// Token: 0x0600647A RID: 25722 RVA: 0x001C31F0 File Offset: 0x001C13F0
		[SecurityCritical]
		private bool AssemblyOnDisallowedList(string assemblyToCheck)
		{
			bool result = false;
			if (!AssemblyFilter._disallowedListExtracted.Value)
			{
				this.ExtractDisallowedRegistryList();
				AssemblyFilter._disallowedListExtracted.Value = true;
			}
			if (AssemblyFilter._assemblyList.Value.Contains(assemblyToCheck))
			{
				result = true;
			}
			return result;
		}

		// Token: 0x0600647B RID: 25723 RVA: 0x001C3234 File Offset: 0x001C1434
		[SecurityCritical]
		private void ExtractDisallowedRegistryList()
		{
			new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\Software\\Microsoft\\.NetFramework\\policy\\APTCA").Assert();
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NetFramework\\policy\\APTCA");
				if (registryKey != null)
				{
					string[] subKeyNames = registryKey.GetSubKeyNames();
					foreach (string text in subKeyNames)
					{
						registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\.NetFramework\\policy\\APTCA\\" + text);
						object value = registryKey.GetValue("APTCA_FLAG");
						if (value != null && (int)value == 1 && !AssemblyFilter._assemblyList.Value.Contains(text))
						{
							AssemblyFilter._assemblyList.Value.Add(text.ToLower(CultureInfo.InvariantCulture).Trim());
						}
					}
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x040032AA RID: 12970
		private static SecurityCriticalDataForSet<List<string>> _assemblyList;

		// Token: 0x040032AB RID: 12971
		private static SecurityCriticalDataForSet<bool> _disallowedListExtracted;

		// Token: 0x040032AC RID: 12972
		private static object _lock = new object();

		// Token: 0x040032AD RID: 12973
		private const string FILEVERSION_STRING = ", FileVersion=";

		// Token: 0x040032AE RID: 12974
		private const string KILL_BIT_REGISTRY_HIVE = "HKEY_LOCAL_MACHINE\\";

		// Token: 0x040032AF RID: 12975
		private const string KILL_BIT_REGISTRY_LOCATION = "Software\\Microsoft\\.NetFramework\\policy\\APTCA";

		// Token: 0x040032B0 RID: 12976
		private const string SUBKEY_VALUE = "APTCA_FLAG";
	}
}
