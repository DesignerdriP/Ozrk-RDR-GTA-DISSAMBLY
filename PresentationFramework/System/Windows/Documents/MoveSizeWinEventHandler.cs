﻿using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200042E RID: 1070
	internal class MoveSizeWinEventHandler : WinEventHandler
	{
		// Token: 0x06003EF2 RID: 16114 RVA: 0x0011F474 File Offset: 0x0011D674
		internal MoveSizeWinEventHandler() : base(11, 11)
		{
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x0011F480 File Offset: 0x0011D680
		internal void RegisterTextStore(TextStore textstore)
		{
			if (this._arTextStore == null)
			{
				this._arTextStore = new ArrayList(1);
			}
			this._arTextStore.Add(textstore);
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x0011F4A3 File Offset: 0x0011D6A3
		internal void UnregisterTextStore(TextStore textstore)
		{
			this._arTextStore.Remove(textstore);
		}

		// Token: 0x06003EF5 RID: 16117 RVA: 0x0011F4B4 File Offset: 0x0011D6B4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void WinEventProc(int eventId, IntPtr hwnd)
		{
			Invariant.Assert(eventId == 11);
			if (this._arTextStore != null)
			{
				for (int i = 0; i < this._arTextStore.Count; i++)
				{
					bool flag = false;
					TextStore textStore = (TextStore)this._arTextStore[i];
					IntPtr intPtr = textStore.CriticalSourceWnd;
					while (intPtr != IntPtr.Zero)
					{
						if (hwnd == intPtr)
						{
							textStore.OnLayoutUpdated();
							flag = true;
							break;
						}
						intPtr = UnsafeNativeMethods.GetParent(new HandleRef(this, intPtr));
					}
					if (!flag)
					{
						textStore.MakeLayoutChangeOnGotFocus();
					}
				}
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06003EF6 RID: 16118 RVA: 0x0011F53C File Offset: 0x0011D73C
		internal int TextStoreCount
		{
			get
			{
				return this._arTextStore.Count;
			}
		}

		// Token: 0x040026C7 RID: 9927
		private ArrayList _arTextStore;
	}
}
