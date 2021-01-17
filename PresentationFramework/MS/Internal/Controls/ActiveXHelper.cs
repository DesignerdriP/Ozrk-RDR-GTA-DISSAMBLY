using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000752 RID: 1874
	internal class ActiveXHelper
	{
		// Token: 0x06007765 RID: 30565 RVA: 0x0000326D File Offset: 0x0000146D
		private ActiveXHelper()
		{
		}

		// Token: 0x06007766 RID: 30566 RVA: 0x00221945 File Offset: 0x0021FB45
		public static int Pix2HM(int pix, int logP)
		{
			return (2540 * pix + (logP >> 1)) / logP;
		}

		// Token: 0x06007767 RID: 30567 RVA: 0x00221954 File Offset: 0x0021FB54
		public static int HM2Pix(int hm, int logP)
		{
			return (logP * hm + 1270) / 2540;
		}

		// Token: 0x17001C5A RID: 7258
		// (get) Token: 0x06007768 RID: 30568 RVA: 0x00221968 File Offset: 0x0021FB68
		public static int LogPixelsX
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (ActiveXHelper.logPixelsX == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						ActiveXHelper.logPixelsX = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 88);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return ActiveXHelper.logPixelsX;
			}
		}

		// Token: 0x06007769 RID: 30569 RVA: 0x002219BF File Offset: 0x0021FBBF
		public static void ResetLogPixelsX()
		{
			ActiveXHelper.logPixelsX = -1;
		}

		// Token: 0x17001C5B RID: 7259
		// (get) Token: 0x0600776A RID: 30570 RVA: 0x002219C8 File Offset: 0x0021FBC8
		public static int LogPixelsY
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				if (ActiveXHelper.logPixelsY == -1)
				{
					IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
					if (dc != IntPtr.Zero)
					{
						ActiveXHelper.logPixelsY = UnsafeNativeMethods.GetDeviceCaps(new HandleRef(null, dc), 90);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
					}
				}
				return ActiveXHelper.logPixelsY;
			}
		}

		// Token: 0x0600776B RID: 30571 RVA: 0x00221A1F File Offset: 0x0021FC1F
		public static void ResetLogPixelsY()
		{
			ActiveXHelper.logPixelsY = -1;
		}

		// Token: 0x0600776C RID: 30572
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		[DllImport("PresentationHost_v0400.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		internal static extern object CreateIDispatchSTAForwarder([MarshalAs(UnmanagedType.IDispatch)] object pDispatchDelegate);

		// Token: 0x040038BC RID: 14524
		public static readonly int sinkAttached = BitVector32.CreateMask();

		// Token: 0x040038BD RID: 14525
		public static readonly int inTransition = BitVector32.CreateMask(ActiveXHelper.sinkAttached);

		// Token: 0x040038BE RID: 14526
		public static readonly int processingKeyUp = BitVector32.CreateMask(ActiveXHelper.inTransition);

		// Token: 0x040038BF RID: 14527
		private static int logPixelsX = -1;

		// Token: 0x040038C0 RID: 14528
		private static int logPixelsY = -1;

		// Token: 0x040038C1 RID: 14529
		private const int HMperInch = 2540;

		// Token: 0x02000B68 RID: 2920
		public enum ActiveXState
		{
			// Token: 0x04004B3C RID: 19260
			Passive,
			// Token: 0x04004B3D RID: 19261
			Loaded,
			// Token: 0x04004B3E RID: 19262
			Running,
			// Token: 0x04004B3F RID: 19263
			InPlaceActive = 4,
			// Token: 0x04004B40 RID: 19264
			UIActive = 8,
			// Token: 0x04004B41 RID: 19265
			Open = 16
		}
	}
}
