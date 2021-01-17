﻿using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Interop;
using MS.Internal.Documents.Application;
using MS.Internal.PresentationFramework;
using MS.Internal.Utility;
using MS.Win32;

namespace MS.Internal.AppModel
{
	// Token: 0x0200076F RID: 1903
	internal static class AppSecurityManager
	{
		// Token: 0x060078AC RID: 30892 RVA: 0x00226044 File Offset: 0x00224244
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void SafeLaunchBrowserDemandWhenUnsafe(Uri originatingUri, Uri destinationUri, bool fIsTopLevel)
		{
			LaunchResult launchResult = AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(originatingUri, destinationUri, fIsTopLevel);
			if (launchResult == LaunchResult.NotLaunched)
			{
				SecurityHelper.DemandUnmanagedCode();
				AppSecurityManager.UnsafeLaunchBrowser(destinationUri, null);
			}
		}

		// Token: 0x060078AD RID: 30893 RVA: 0x0022606C File Offset: 0x0022426C
		internal static LaunchResult SafeLaunchBrowserOnlyIfPossible(Uri originatingUri, Uri destinationUri, bool fIsTopLevel)
		{
			return AppSecurityManager.SafeLaunchBrowserOnlyIfPossible(originatingUri, destinationUri, null, fIsTopLevel);
		}

		// Token: 0x060078AE RID: 30894 RVA: 0x00226078 File Offset: 0x00224278
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static LaunchResult SafeLaunchBrowserOnlyIfPossible(Uri originatingUri, Uri destinationUri, string targetName, bool fIsTopLevel)
		{
			LaunchResult launchResult = LaunchResult.NotLaunched;
			bool flag = destinationUri.Scheme == Uri.UriSchemeHttp || destinationUri.Scheme == Uri.UriSchemeHttps || destinationUri.IsFile;
			bool flag2 = string.Compare(destinationUri.Scheme, Uri.UriSchemeMailto, StringComparison.OrdinalIgnoreCase) == 0;
			if (!BrowserInteropHelper.IsInitialViewerNavigation && SecurityHelper.CallerHasUserInitiatedNavigationPermission() && ((fIsTopLevel && flag) || flag2))
			{
				if (flag)
				{
					IBrowserCallbackServices browserCallbackServices = (Application.Current != null) ? Application.Current.BrowserCallbackServices : null;
					if (browserCallbackServices != null)
					{
						launchResult = AppSecurityManager.CanNavigateToUrlWithZoneCheck(originatingUri, destinationUri);
						if (launchResult == LaunchResult.Launched)
						{
							browserCallbackServices.DelegateNavigation(BindUriHelper.UriToString(destinationUri), targetName, AppSecurityManager.GetHeaders(destinationUri));
							launchResult = LaunchResult.Launched;
						}
					}
				}
				else if (flag2)
				{
					UnsafeNativeMethods.ShellExecute(new HandleRef(null, IntPtr.Zero), null, BindUriHelper.UriToString(destinationUri), null, null, 0);
					launchResult = LaunchResult.Launched;
				}
			}
			return launchResult;
		}

		// Token: 0x060078AF RID: 30895 RVA: 0x00226138 File Offset: 0x00224338
		[SecurityCritical]
		internal static void UnsafeLaunchBrowser(Uri uri, string targetFrame = null)
		{
			if (Application.Current != null && Application.Current.CheckAccess())
			{
				IBrowserCallbackServices browserCallbackServices = Application.Current.BrowserCallbackServices;
				if (browserCallbackServices != null)
				{
					browserCallbackServices.DelegateNavigation(BindUriHelper.UriToString(uri), targetFrame, AppSecurityManager.GetHeaders(uri));
					return;
				}
			}
			AppSecurityManager.ShellExecuteDefaultBrowser(uri);
		}

		// Token: 0x060078B0 RID: 30896 RVA: 0x00226180 File Offset: 0x00224380
		[SecurityCritical]
		internal static void ShellExecuteDefaultBrowser(Uri uri)
		{
			UnsafeNativeMethods.ShellExecuteInfo shellExecuteInfo = new UnsafeNativeMethods.ShellExecuteInfo();
			shellExecuteInfo.cbSize = Marshal.SizeOf(shellExecuteInfo);
			shellExecuteInfo.fMask = UnsafeNativeMethods.ShellExecuteFlags.SEE_MASK_FLAG_DDEWAIT;
			if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
			{
				shellExecuteInfo.fMask |= UnsafeNativeMethods.ShellExecuteFlags.SEE_MASK_CLASSNAME;
				shellExecuteInfo.lpClass = ".htm";
			}
			shellExecuteInfo.lpFile = uri.ToString();
			if (!UnsafeNativeMethods.ShellExecuteEx(shellExecuteInfo))
			{
				throw new InvalidOperationException(SR.Get("FailToLaunchDefaultBrowser"), new Win32Exception());
			}
		}

		// Token: 0x060078B1 RID: 30897 RVA: 0x00226210 File Offset: 0x00224410
		private static string GetHeaders(Uri destinationUri)
		{
			string text = BindUriHelper.GetReferer(destinationUri);
			if (!string.IsNullOrEmpty(text))
			{
				text = "Referer: " + text + "\r\n";
			}
			return text;
		}

		// Token: 0x060078B2 RID: 30898 RVA: 0x00226240 File Offset: 0x00224440
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static LaunchResult CanNavigateToUrlWithZoneCheck(Uri originatingUri, Uri destinationUri)
		{
			AppSecurityManager.EnsureSecurityManager();
			bool flag = UnsafeNativeMethods.CoInternetIsFeatureEnabled(1, 2) != 1;
			int num = AppSecurityManager.MapUrlToZone(destinationUri);
			Uri uri = null;
			if (Application.Current.MimeType != MimeType.Document)
			{
				uri = BrowserInteropHelper.Source;
			}
			else if (destinationUri.IsFile && Path.GetExtension(destinationUri.LocalPath).Equals(DocumentStream.XpsFileExtension, StringComparison.OrdinalIgnoreCase))
			{
				num = 3;
			}
			int num2;
			if (uri != null)
			{
				num2 = AppSecurityManager.MapUrlToZone(uri);
			}
			else
			{
				bool flag2 = SecurityHelper.CheckUnmanagedCodePermission();
				if (flag2)
				{
					return LaunchResult.Launched;
				}
				num2 = 3;
				uri = originatingUri;
			}
			if ((!flag && ((num2 != 3 && num2 != 4) || num != 0)) || (flag && (num2 == num || (num2 <= 4 && num <= 4 && (num2 < num || ((num2 == 2 || num2 == 1) && (num == 2 || num == 1)))))))
			{
				return LaunchResult.Launched;
			}
			return AppSecurityManager.CheckBlockNavigation(uri, destinationUri, flag);
		}

		// Token: 0x060078B3 RID: 30899 RVA: 0x0022630E File Offset: 0x0022450E
		[SecurityCritical]
		private static LaunchResult CheckBlockNavigation(Uri originatingUri, Uri destinationUri, bool fEnabled)
		{
			if (!fEnabled)
			{
				return LaunchResult.Launched;
			}
			if (UnsafeNativeMethods.CoInternetIsFeatureZoneElevationEnabled(BindUriHelper.UriToString(originatingUri), BindUriHelper.UriToString(destinationUri), AppSecurityManager._secMgr, 2) == 1)
			{
				return LaunchResult.Launched;
			}
			if (AppSecurityManager.IsZoneElevationSettingPrompt(destinationUri))
			{
				return LaunchResult.NotLaunchedDueToPrompt;
			}
			return LaunchResult.NotLaunched;
		}

		// Token: 0x060078B4 RID: 30900 RVA: 0x0022633C File Offset: 0x0022453C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private unsafe static bool IsZoneElevationSettingPrompt(Uri target)
		{
			Invariant.Assert(AppSecurityManager._secMgr != null);
			int num = 3;
			string pwszUrl = BindUriHelper.UriToString(target);
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				AppSecurityManager._secMgr.ProcessUrlAction(pwszUrl, 8449, (byte*)(&num), Marshal.SizeOf(typeof(int)), null, 0, 1, 0);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return num == 1;
		}

		// Token: 0x060078B5 RID: 30901 RVA: 0x002263B0 File Offset: 0x002245B0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void EnsureSecurityManager()
		{
			if (AppSecurityManager._secMgr == null)
			{
				object lockObj = AppSecurityManager._lockObj;
				lock (lockObj)
				{
					if (AppSecurityManager._secMgr == null)
					{
						AppSecurityManager._secMgr = (UnsafeNativeMethods.IInternetSecurityManager)new AppSecurityManager.InternetSecurityManager();
						AppSecurityManager._secMgrSite = new SecurityMgrSite();
						new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
						try
						{
							AppSecurityManager._secMgr.SetSecuritySite(AppSecurityManager._secMgrSite);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
				}
			}
		}

		// Token: 0x060078B6 RID: 30902 RVA: 0x00226440 File Offset: 0x00224640
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void ClearSecurityManager()
		{
			if (AppSecurityManager._secMgr != null)
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					object lockObj = AppSecurityManager._lockObj;
					lock (lockObj)
					{
						if (AppSecurityManager._secMgr != null)
						{
							AppSecurityManager._secMgr.SetSecuritySite(null);
							AppSecurityManager._secMgrSite = null;
							AppSecurityManager._secMgr = null;
						}
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x060078B7 RID: 30903 RVA: 0x002264BC File Offset: 0x002246BC
		[SecurityCritical]
		internal static int MapUrlToZone(Uri url)
		{
			AppSecurityManager.EnsureSecurityManager();
			int result;
			AppSecurityManager._secMgr.MapUrlToZone(BindUriHelper.UriToString(url), out result, 0);
			return result;
		}

		// Token: 0x0400392E RID: 14638
		private const string RefererHeader = "Referer: ";

		// Token: 0x0400392F RID: 14639
		private const string BrowserOpenCommandLookupKey = "htmlfile\\shell\\open\\command";

		// Token: 0x04003930 RID: 14640
		private static object _lockObj = new object();

		// Token: 0x04003931 RID: 14641
		[SecurityCritical]
		private static UnsafeNativeMethods.IInternetSecurityManager _secMgr;

		// Token: 0x04003932 RID: 14642
		private static SecurityMgrSite _secMgrSite;

		// Token: 0x02000B73 RID: 2931
		[ComVisible(false)]
		[Guid("7b8a2d94-0ac9-11d1-896c-00c04Fb6bfc4")]
		[ComImport]
		internal class InternetSecurityManager
		{
			// Token: 0x06008E28 RID: 36392
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern InternetSecurityManager();
		}
	}
}
