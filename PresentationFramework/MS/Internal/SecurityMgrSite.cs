using System;
using System.Security;
using System.Windows;
using MS.Internal.AppModel;
using MS.Win32;

namespace MS.Internal
{
	// Token: 0x020005E4 RID: 1508
	internal class SecurityMgrSite : NativeMethods.IInternetSecurityMgrSite
	{
		// Token: 0x06006474 RID: 25716 RVA: 0x0000326D File Offset: 0x0000146D
		internal SecurityMgrSite()
		{
		}

		// Token: 0x06006475 RID: 25717 RVA: 0x001C303C File Offset: 0x001C123C
		[SecurityCritical]
		public void GetWindow(ref IntPtr phwnd)
		{
			phwnd = IntPtr.Zero;
			if (Application.Current != null)
			{
				Window mainWindow = Application.Current.MainWindow;
				Invariant.Assert(Application.Current.BrowserCallbackServices == null || mainWindow is RootBrowserWindow);
				if (mainWindow != null)
				{
					phwnd = mainWindow.CriticalHandle;
				}
			}
		}

		// Token: 0x06006476 RID: 25718 RVA: 0x00002137 File Offset: 0x00000337
		public void EnableModeless(bool fEnable)
		{
		}
	}
}
