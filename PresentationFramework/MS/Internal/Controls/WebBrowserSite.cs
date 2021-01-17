using System;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x0200075B RID: 1883
	internal class WebBrowserSite : ActiveXSite, UnsafeNativeMethods.IDocHostUIHandler, UnsafeNativeMethods.IOleControlSite
	{
		// Token: 0x060077E3 RID: 30691 RVA: 0x00223483 File Offset: 0x00221683
		[SecurityCritical]
		internal WebBrowserSite(WebBrowser host) : base(host)
		{
		}

		// Token: 0x060077E4 RID: 30692 RVA: 0x00016748 File Offset: 0x00014948
		int UnsafeNativeMethods.IDocHostUIHandler.ShowContextMenu(int dwID, NativeMethods.POINT pt, object pcmdtReserved, object pdispReserved)
		{
			return 1;
		}

		// Token: 0x060077E5 RID: 30693 RVA: 0x0022348C File Offset: 0x0022168C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		int UnsafeNativeMethods.IDocHostUIHandler.GetHostInfo(NativeMethods.DOCHOSTUIINFO info)
		{
			WebBrowser webBrowser = (WebBrowser)base.Host;
			info.dwDoubleClick = 0;
			info.dwFlags = 94846994;
			return 0;
		}

		// Token: 0x060077E6 RID: 30694 RVA: 0x002218C6 File Offset: 0x0021FAC6
		int UnsafeNativeMethods.IDocHostUIHandler.EnableModeless(bool fEnable)
		{
			return -2147467263;
		}

		// Token: 0x060077E7 RID: 30695 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecuritySafeCritical]
		int UnsafeNativeMethods.IDocHostUIHandler.ShowUI(int dwID, UnsafeNativeMethods.IOleInPlaceActiveObject activeObject, NativeMethods.IOleCommandTarget commandTarget, UnsafeNativeMethods.IOleInPlaceFrame frame, UnsafeNativeMethods.IOleInPlaceUIWindow doc)
		{
			return -2147467263;
		}

		// Token: 0x060077E8 RID: 30696 RVA: 0x002218C6 File Offset: 0x0021FAC6
		int UnsafeNativeMethods.IDocHostUIHandler.HideUI()
		{
			return -2147467263;
		}

		// Token: 0x060077E9 RID: 30697 RVA: 0x002218C6 File Offset: 0x0021FAC6
		int UnsafeNativeMethods.IDocHostUIHandler.UpdateUI()
		{
			return -2147467263;
		}

		// Token: 0x060077EA RID: 30698 RVA: 0x002218C6 File Offset: 0x0021FAC6
		int UnsafeNativeMethods.IDocHostUIHandler.OnDocWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		// Token: 0x060077EB RID: 30699 RVA: 0x002218C6 File Offset: 0x0021FAC6
		int UnsafeNativeMethods.IDocHostUIHandler.OnFrameWindowActivate(bool fActivate)
		{
			return -2147467263;
		}

		// Token: 0x060077EC RID: 30700 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecuritySafeCritical]
		int UnsafeNativeMethods.IDocHostUIHandler.ResizeBorder(NativeMethods.COMRECT rect, UnsafeNativeMethods.IOleInPlaceUIWindow doc, bool fFrameWindow)
		{
			return -2147467263;
		}

		// Token: 0x060077ED RID: 30701 RVA: 0x002218C6 File Offset: 0x0021FAC6
		int UnsafeNativeMethods.IDocHostUIHandler.GetOptionKeyPath(string[] pbstrKey, int dw)
		{
			return -2147467263;
		}

		// Token: 0x060077EE RID: 30702 RVA: 0x002234B8 File Offset: 0x002216B8
		[SecurityCritical]
		int UnsafeNativeMethods.IDocHostUIHandler.GetDropTarget(UnsafeNativeMethods.IOleDropTarget pDropTarget, out UnsafeNativeMethods.IOleDropTarget ppDropTarget)
		{
			ppDropTarget = null;
			return -2147467263;
		}

		// Token: 0x060077EF RID: 30703 RVA: 0x002234C4 File Offset: 0x002216C4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		int UnsafeNativeMethods.IDocHostUIHandler.GetExternal(out object ppDispatch)
		{
			WebBrowser webBrowser = (WebBrowser)base.Host;
			ppDispatch = webBrowser.HostingAdaptor.ObjectForScripting;
			return 0;
		}

		// Token: 0x060077F0 RID: 30704 RVA: 0x00016748 File Offset: 0x00014948
		int UnsafeNativeMethods.IDocHostUIHandler.TranslateAccelerator(ref MSG msg, ref Guid group, int nCmdID)
		{
			return 1;
		}

		// Token: 0x060077F1 RID: 30705 RVA: 0x00221B88 File Offset: 0x0021FD88
		int UnsafeNativeMethods.IDocHostUIHandler.TranslateUrl(int dwTranslate, string strUrlIn, out string pstrUrlOut)
		{
			pstrUrlOut = null;
			return -2147467263;
		}

		// Token: 0x060077F2 RID: 30706 RVA: 0x002234B8 File Offset: 0x002216B8
		int UnsafeNativeMethods.IDocHostUIHandler.FilterDataObject(IDataObject pDO, out IDataObject ppDORet)
		{
			ppDORet = null;
			return -2147467263;
		}

		// Token: 0x060077F3 RID: 30707 RVA: 0x002234EC File Offset: 0x002216EC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		int UnsafeNativeMethods.IOleControlSite.TranslateAccelerator(ref MSG msg, int grfModifiers)
		{
			if (msg.message == 256 && (int)msg.wParam == 9)
			{
				FocusNavigationDirection focusNavigationDirection = ((grfModifiers & 1) != 0) ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next;
				base.Host.Dispatcher.Invoke(DispatcherPriority.Send, new SendOrPostCallback(this.MoveFocusCallback), focusNavigationDirection);
				return 0;
			}
			return 1;
		}

		// Token: 0x060077F4 RID: 30708 RVA: 0x00223547 File Offset: 0x00221747
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void MoveFocusCallback(object direction)
		{
			base.Host.MoveFocus(new TraversalRequest((FocusNavigationDirection)direction));
		}
	}
}
