using System;
using System.Security;
using System.Windows.Interop;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000751 RID: 1873
	internal class ActiveXContainer : UnsafeNativeMethods.IOleContainer, UnsafeNativeMethods.IOleInPlaceFrame
	{
		// Token: 0x06007751 RID: 30545 RVA: 0x00221841 File Offset: 0x0021FA41
		[SecurityCritical]
		internal ActiveXContainer(ActiveXHost host)
		{
			this._host = host;
			Invariant.Assert(this._host != null);
		}

		// Token: 0x06007752 RID: 30546 RVA: 0x0022185E File Offset: 0x0021FA5E
		int UnsafeNativeMethods.IOleContainer.ParseDisplayName(object pbc, string pszDisplayName, int[] pchEaten, object[] ppmkOut)
		{
			if (ppmkOut != null)
			{
				ppmkOut[0] = null;
			}
			return -2147467263;
		}

		// Token: 0x06007753 RID: 30547 RVA: 0x00221870 File Offset: 0x0021FA70
		[SecurityCritical]
		int UnsafeNativeMethods.IOleContainer.EnumObjects(int grfFlags, out UnsafeNativeMethods.IEnumUnknown ppenum)
		{
			ppenum = null;
			object activeXInstance = this._host.ActiveXInstance;
			if (activeXInstance != null && ((grfFlags & 1) != 0 || ((grfFlags & 16) != 0 && this._host.ActiveXState == ActiveXHelper.ActiveXState.Running)))
			{
				ppenum = new EnumUnknown(new object[]
				{
					activeXInstance
				});
				return 0;
			}
			ppenum = new EnumUnknown(null);
			return 0;
		}

		// Token: 0x06007754 RID: 30548 RVA: 0x002218C6 File Offset: 0x0021FAC6
		int UnsafeNativeMethods.IOleContainer.LockContainer(bool fLock)
		{
			return -2147467263;
		}

		// Token: 0x06007755 RID: 30549 RVA: 0x002218D0 File Offset: 0x0021FAD0
		[SecurityCritical]
		IntPtr UnsafeNativeMethods.IOleInPlaceFrame.GetWindow()
		{
			return this._host.ParentHandle.Handle;
		}

		// Token: 0x06007756 RID: 30550 RVA: 0x0000B02A File Offset: 0x0000922A
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.ContextSensitiveHelp(int fEnterMode)
		{
			return 0;
		}

		// Token: 0x06007757 RID: 30551 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.GetBorder(NativeMethods.COMRECT lprectBorder)
		{
			return -2147467263;
		}

		// Token: 0x06007758 RID: 30552 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.RequestBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x06007759 RID: 30553 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.SetBorderSpace(NativeMethods.COMRECT pborderwidths)
		{
			return -2147467263;
		}

		// Token: 0x0600775A RID: 30554 RVA: 0x0000B02A File Offset: 0x0000922A
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.SetActiveObject(UnsafeNativeMethods.IOleInPlaceActiveObject pActiveObject, string pszObjName)
		{
			return 0;
		}

		// Token: 0x0600775B RID: 30555 RVA: 0x0000B02A File Offset: 0x0000922A
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.InsertMenus(IntPtr hmenuShared, NativeMethods.tagOleMenuGroupWidths lpMenuWidths)
		{
			return 0;
		}

		// Token: 0x0600775C RID: 30556 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.SetMenu(IntPtr hmenuShared, IntPtr holemenu, IntPtr hwndActiveObject)
		{
			return -2147467263;
		}

		// Token: 0x0600775D RID: 30557 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.RemoveMenus(IntPtr hmenuShared)
		{
			return -2147467263;
		}

		// Token: 0x0600775E RID: 30558 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.SetStatusText(string pszStatusText)
		{
			return -2147467263;
		}

		// Token: 0x0600775F RID: 30559 RVA: 0x002218C6 File Offset: 0x0021FAC6
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.EnableModeless(bool fEnable)
		{
			return -2147467263;
		}

		// Token: 0x06007760 RID: 30560 RVA: 0x00016748 File Offset: 0x00014948
		[SecurityCritical]
		int UnsafeNativeMethods.IOleInPlaceFrame.TranslateAccelerator(ref MSG lpmsg, short wID)
		{
			return 1;
		}

		// Token: 0x06007761 RID: 30561 RVA: 0x002218F0 File Offset: 0x0021FAF0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnUIActivate(ActiveXHost site)
		{
			if (this._siteUIActive == site)
			{
				return;
			}
			if (this._siteUIActive != null)
			{
				ActiveXHost siteUIActive = this._siteUIActive;
				siteUIActive.ActiveXInPlaceObject.UIDeactivate();
			}
			this._siteUIActive = site;
		}

		// Token: 0x06007762 RID: 30562 RVA: 0x00221929 File Offset: 0x0021FB29
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnUIDeactivate(ActiveXHost site)
		{
			this._siteUIActive = null;
		}

		// Token: 0x06007763 RID: 30563 RVA: 0x00221932 File Offset: 0x0021FB32
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void OnInPlaceDeactivate(ActiveXHost site)
		{
			ActiveXHost activeXHost = this.ActiveXHost;
		}

		// Token: 0x17001C59 RID: 7257
		// (get) Token: 0x06007764 RID: 30564 RVA: 0x0022193D File Offset: 0x0021FB3D
		internal ActiveXHost ActiveXHost
		{
			[SecurityCritical]
			get
			{
				return this._host;
			}
		}

		// Token: 0x040038BA RID: 14522
		[SecurityCritical]
		private ActiveXHost _host;

		// Token: 0x040038BB RID: 14523
		[SecurityCritical]
		private ActiveXHost _siteUIActive;
	}
}
