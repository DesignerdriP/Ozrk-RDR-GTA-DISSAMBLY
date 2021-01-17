﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Interop
{
	/// <summary>A helper class that provides information about the browser environment in which a XAML browser application (XBAP) application is hosted.</summary>
	// Token: 0x020005BA RID: 1466
	public static class BrowserInteropHelper
	{
		// Token: 0x060061A0 RID: 24992 RVA: 0x001B5FDE File Offset: 0x001B41DE
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static BrowserInteropHelper()
		{
			BrowserInteropHelper.SetBrowserHosted(false);
			BrowserInteropHelper.IsInitialViewerNavigation = true;
		}

		/// <summary>Returns a reference to an object that can be used to access the host browser via its OLE container interfaces (for example, IOleClientSite::GetContainer();).</summary>
		/// <returns>An object that can be cast to <see cref="T:Microsoft.VisualStudio.OLE.Interop.IOleClientSite" />.</returns>
		// Token: 0x1700177F RID: 6015
		// (get) Token: 0x060061A1 RID: 24993 RVA: 0x001B5FEC File Offset: 0x001B41EC
		public static object ClientSite
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUnmanagedCode();
				object result = null;
				if (BrowserInteropHelper.IsBrowserHosted)
				{
					Application.Current.BrowserCallbackServices.GetOleClientSite(out result);
				}
				return result;
			}
		}

		/// <summary>Gets a script object that provides access to the HTML window object, custom script functions, and global variables for the HTML page, if the XAML browser application (XBAP) is hosted in a frame.</summary>
		/// <returns>A script object that provides access to the HTML window object, custom script functions, and global variables for the HTML page, if the XAML browser application (XBAP) is hosted in a frame; otherwise, <see langword="null" />.</returns>
		// Token: 0x17001780 RID: 6016
		// (get) Token: 0x060061A2 RID: 24994 RVA: 0x001B601C File Offset: 0x001B421C
		[Dynamic]
		public static dynamic HostScript
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			[return: Dynamic]
			get
			{
				BrowserInteropHelper.EnsureScriptInteropAllowed();
				Verify.IsApartmentState(ApartmentState.STA);
				IHostBrowser2 hostBrowser = BrowserInteropHelper.HostBrowser as IHostBrowser2;
				if (hostBrowser == null)
				{
					return null;
				}
				object result;
				try
				{
					UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)hostBrowser.HostScriptObject;
					if (dispatch == null)
					{
						result = null;
					}
					else
					{
						DynamicScriptObject dynamicScriptObject = new DynamicScriptObject(dispatch);
						BrowserInteropHelper.InitializeHostHtmlDocumentServiceProvider(dynamicScriptObject);
						result = dynamicScriptObject;
					}
				}
				catch (UnauthorizedAccessException)
				{
					result = null;
				}
				return result;
			}
		}

		/// <summary>Gets a value that specifies whether the current Windows Presentation Foundation (WPF) application is browser hosted.</summary>
		/// <returns>
		///     <see langword="true" /> if the application is browser hosted; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001781 RID: 6017
		// (get) Token: 0x060061A3 RID: 24995 RVA: 0x001B6080 File Offset: 0x001B4280
		public static bool IsBrowserHosted
		{
			get
			{
				return BrowserInteropHelper._isBrowserHosted.Value;
			}
		}

		// Token: 0x060061A4 RID: 24996 RVA: 0x001B608C File Offset: 0x001B428C
		[SecurityCritical]
		internal static void SetBrowserHosted(bool value)
		{
			BrowserInteropHelper._isBrowserHosted.Value = value;
		}

		// Token: 0x17001782 RID: 6018
		// (get) Token: 0x060061A5 RID: 24997 RVA: 0x001B6099 File Offset: 0x001B4299
		// (set) Token: 0x060061A6 RID: 24998 RVA: 0x001B60A5 File Offset: 0x001B42A5
		internal static HostingFlags HostingFlags
		{
			get
			{
				return BrowserInteropHelper._hostingFlags.Value;
			}
			[SecurityCritical]
			set
			{
				BrowserInteropHelper._hostingFlags.Value = value;
			}
		}

		/// <summary>Gets the uniform resource identifier (URI) for the location from which a XAML browser application (XBAP) application was launched.</summary>
		/// <returns>The <see cref="T:System.Uri" /> for the location from which a XAML browser application (XBAP) application was launched; otherwise, <see langword="null" />.</returns>
		// Token: 0x17001783 RID: 6019
		// (get) Token: 0x060061A7 RID: 24999 RVA: 0x001B60B2 File Offset: 0x001B42B2
		public static Uri Source
		{
			get
			{
				return SiteOfOriginContainer.BrowserSource;
			}
		}

		// Token: 0x17001784 RID: 6020
		// (get) Token: 0x060061A8 RID: 25000 RVA: 0x001B60BC File Offset: 0x001B42BC
		internal static bool IsViewer
		{
			get
			{
				Application application = Application.Current;
				return application != null && application.MimeType == MimeType.Markup;
			}
		}

		// Token: 0x17001785 RID: 6021
		// (get) Token: 0x060061A9 RID: 25001 RVA: 0x001B60DD File Offset: 0x001B42DD
		internal static bool IsAvalonTopLevel
		{
			get
			{
				return !BrowserInteropHelper.IsBrowserHosted || (BrowserInteropHelper.HostingFlags & HostingFlags.hfHostedInFrame) == (HostingFlags)0;
			}
		}

		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x060061AA RID: 25002 RVA: 0x001B60F2 File Offset: 0x001B42F2
		internal static bool IsHostedInIEorWebOC
		{
			get
			{
				return (BrowserInteropHelper.HostingFlags & HostingFlags.hfHostedInIEorWebOC) > (HostingFlags)0;
			}
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x060061AB RID: 25003 RVA: 0x001B60FE File Offset: 0x001B42FE
		// (set) Token: 0x060061AC RID: 25004 RVA: 0x001B6113 File Offset: 0x001B4313
		internal static bool IsInitialViewerNavigation
		{
			get
			{
				return BrowserInteropHelper.IsViewer && BrowserInteropHelper._isInitialViewerNavigation.Value;
			}
			[SecurityCritical]
			set
			{
				BrowserInteropHelper._isInitialViewerNavigation.Value = value;
			}
		}

		// Token: 0x060061AD RID: 25005 RVA: 0x001B6120 File Offset: 0x001B4320
		[SecurityCritical]
		internal static void ReleaseBrowserInterfaces()
		{
			if (BrowserInteropHelper.HostBrowser != null)
			{
				Marshal.ReleaseComObject(BrowserInteropHelper.HostBrowser);
				BrowserInteropHelper.HostBrowser = null;
			}
			if (BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value != null)
			{
				Marshal.ReleaseComObject(BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value);
				BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value = null;
			}
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x060061AE RID: 25006 RVA: 0x001B616C File Offset: 0x001B436C
		internal static UnsafeNativeMethods.IServiceProvider HostHtmlDocumentServiceProvider
		{
			[SecurityCritical]
			get
			{
				Invariant.Assert(!BrowserInteropHelper._initializedHostScript.Value || BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value != null);
				return BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value;
			}
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x001B619C File Offset: 0x001B439C
		[SecurityCritical]
		private static void InitializeHostHtmlDocumentServiceProvider(DynamicScriptObject scriptObject)
		{
			if (BrowserInteropHelper.IsHostedInIEorWebOC && scriptObject.ScriptObject is UnsafeNativeMethods.IHTMLWindow4 && BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value == null)
			{
				object obj;
				bool condition = scriptObject.TryFindMemberAndInvokeNonWrapped("document", 2, true, null, out obj);
				Invariant.Assert(condition);
				BrowserInteropHelper._hostHtmlDocumentServiceProvider.Value = (UnsafeNativeMethods.IServiceProvider)obj;
				BrowserInteropHelper._initializedHostScript.Value = true;
			}
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x001B61FC File Offset: 0x001B43FC
		[SecurityCritical]
		private static void HostFilterInput(ref MSG msg, ref bool handled)
		{
			WindowMessage message = (WindowMessage)msg.message;
			if ((message == WindowMessage.WM_INPUT || (message >= WindowMessage.WM_KEYFIRST && message <= WindowMessage.WM_IME_COMPOSITION) || (message >= WindowMessage.WM_MOUSEMOVE && message <= WindowMessage.WM_MOUSEHWHEEL)) && BrowserInteropHelper.ForwardTranslateAccelerator(ref msg, false) == 0)
			{
				handled = true;
			}
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x001B6244 File Offset: 0x001B4444
		[SecurityCritical]
		internal static IntPtr PostFilterInput(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (!handled && msg >= 256 && msg <= 271)
			{
				MSG msg2 = new MSG(hwnd, msg, wParam, lParam, SafeNativeMethods.GetMessageTime(), 0, 0);
				if (BrowserInteropHelper.ForwardTranslateAccelerator(ref msg2, true) == 0)
				{
					handled = true;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x001B628C File Offset: 0x001B448C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void InitializeHostFilterInput()
		{
			new UIPermission(PermissionState.Unrestricted).Assert();
			try
			{
				ComponentDispatcher.ThreadFilterMessage += BrowserInteropHelper.HostFilterInput;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x001B62D0 File Offset: 0x001B44D0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private static void EnsureScriptInteropAllowed()
		{
			if (BrowserInteropHelper._isScriptInteropDisabled.Value == null)
			{
				BrowserInteropHelper._isScriptInteropDisabled.Value = new bool?(SafeSecurityHelper.IsFeatureDisabled(SafeSecurityHelper.KeyToRead.ScriptInteropDisable));
			}
			if (BrowserInteropHelper._isScriptInteropDisabled.Value.Value)
			{
				SecurityHelper.DemandWebBrowserPermission();
				return;
			}
			new WebBrowserPermission(WebBrowserPermissionLevel.Safe).Demand();
		}

		// Token: 0x060061B4 RID: 25012
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationHost_v0400.dll")]
		private static extern int ForwardTranslateAccelerator(ref MSG pMsg, bool appUnhandled);

		// Token: 0x04003164 RID: 12644
		[SecurityCritical]
		internal static IHostBrowser HostBrowser;

		// Token: 0x04003165 RID: 12645
		private static SecurityCriticalDataForSet<bool> _isBrowserHosted;

		// Token: 0x04003166 RID: 12646
		private static SecurityCriticalDataForSet<HostingFlags> _hostingFlags;

		// Token: 0x04003167 RID: 12647
		private static SecurityCriticalDataForSet<bool> _isInitialViewerNavigation;

		// Token: 0x04003168 RID: 12648
		private static SecurityCriticalDataForSet<bool?> _isScriptInteropDisabled;

		// Token: 0x04003169 RID: 12649
		[SecurityCritical]
		private static SecurityCriticalDataForSet<UnsafeNativeMethods.IServiceProvider> _hostHtmlDocumentServiceProvider;

		// Token: 0x0400316A RID: 12650
		private static SecurityCriticalDataForSet<bool> _initializedHostScript;
	}
}
