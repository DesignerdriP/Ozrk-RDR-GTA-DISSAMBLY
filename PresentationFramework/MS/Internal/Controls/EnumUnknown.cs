using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Win32;

namespace MS.Internal.Controls
{
	// Token: 0x02000755 RID: 1877
	internal class EnumUnknown : UnsafeNativeMethods.IEnumUnknown
	{
		// Token: 0x06007795 RID: 30613 RVA: 0x0022215C File Offset: 0x0022035C
		internal EnumUnknown(object[] arr)
		{
			this.arr = arr;
			this.loc = 0;
			this.size = ((arr == null) ? 0 : arr.Length);
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x00222181 File Offset: 0x00220381
		private EnumUnknown(object[] arr, int loc) : this(arr)
		{
			this.loc = loc;
		}

		// Token: 0x06007797 RID: 30615 RVA: 0x00222194 File Offset: 0x00220394
		[SecurityCritical]
		int UnsafeNativeMethods.IEnumUnknown.Next(int celt, IntPtr rgelt, IntPtr pceltFetched)
		{
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, 0, 0);
			}
			if (celt < 0)
			{
				return -2147024809;
			}
			int num = 0;
			if (this.loc >= this.size)
			{
				num = 0;
			}
			else
			{
				while (this.loc < this.size && num < celt)
				{
					if (this.arr[this.loc] != null)
					{
						Marshal.WriteIntPtr(rgelt, Marshal.GetIUnknownForObject(this.arr[this.loc]));
						rgelt = (IntPtr)((long)rgelt + (long)sizeof(IntPtr));
						num++;
					}
					this.loc++;
				}
			}
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, 0, num);
			}
			if (num != celt)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06007798 RID: 30616 RVA: 0x00222250 File Offset: 0x00220450
		[SecurityCritical]
		int UnsafeNativeMethods.IEnumUnknown.Skip(int celt)
		{
			this.loc += celt;
			if (this.loc >= this.size)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06007799 RID: 30617 RVA: 0x00222271 File Offset: 0x00220471
		[SecurityCritical]
		void UnsafeNativeMethods.IEnumUnknown.Reset()
		{
			this.loc = 0;
		}

		// Token: 0x0600779A RID: 30618 RVA: 0x0022227A File Offset: 0x0022047A
		[SecurityCritical]
		void UnsafeNativeMethods.IEnumUnknown.Clone(out UnsafeNativeMethods.IEnumUnknown ppenum)
		{
			ppenum = new EnumUnknown(this.arr, this.loc);
		}

		// Token: 0x040038C6 RID: 14534
		private object[] arr;

		// Token: 0x040038C7 RID: 14535
		private int loc;

		// Token: 0x040038C8 RID: 14536
		private int size;
	}
}
