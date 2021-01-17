using System;
using System.Runtime.InteropServices;
using System.Security;
using MS.Internal;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200042F RID: 1071
	internal class WinEventHandler : IDisposable
	{
		// Token: 0x06003EF7 RID: 16119 RVA: 0x0011F54C File Offset: 0x0011D74C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal WinEventHandler(int eventMin, int eventMax)
		{
			this._eventMin = eventMin;
			this._eventMax = eventMax;
			this._winEventProc.Value = new NativeMethods.WinEventProcDef(this.WinEventDefaultProc);
			this._gchThis = GCHandle.Alloc(this._winEventProc.Value);
			this._shutdownListener = new WinEventHandler.WinEventHandlerShutDownListener(this);
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x0011F5A8 File Offset: 0x0011D7A8
		~WinEventHandler()
		{
			this.Clear();
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x0011F5D4 File Offset: 0x0011D7D4
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			this.Clear();
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void WinEventProc(int eventId, IntPtr hwnd)
		{
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x0011F5E2 File Offset: 0x0011D7E2
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Clear()
		{
			this.Stop();
			if (this._gchThis.IsAllocated)
			{
				this._gchThis.Free();
			}
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x0011F604 File Offset: 0x0011D804
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Start()
		{
			if (this._gchThis.IsAllocated)
			{
				this._hHook.Value = UnsafeNativeMethods.SetWinEventHook(this._eventMin, this._eventMax, IntPtr.Zero, this._winEventProc.Value, 0U, 0U, 0);
				if (this._hHook.Value == IntPtr.Zero)
				{
					this.Stop();
				}
			}
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x0011F66C File Offset: 0x0011D86C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Stop()
		{
			if (this._hHook.Value != IntPtr.Zero)
			{
				UnsafeNativeMethods.UnhookWinEvent(this._hHook.Value);
				this._hHook.Value = IntPtr.Zero;
			}
			if (this._shutdownListener != null)
			{
				this._shutdownListener.StopListening();
				this._shutdownListener = null;
			}
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x0011F6CB File Offset: 0x0011D8CB
		private void WinEventDefaultProc(int winEventHook, int eventId, IntPtr hwnd, int idObject, int idChild, int eventThread, int eventTime)
		{
			this.WinEventProc(eventId, hwnd);
		}

		// Token: 0x040026C8 RID: 9928
		private int _eventMin;

		// Token: 0x040026C9 RID: 9929
		private int _eventMax;

		// Token: 0x040026CA RID: 9930
		private SecurityCriticalDataForSet<IntPtr> _hHook;

		// Token: 0x040026CB RID: 9931
		private SecurityCriticalDataForSet<NativeMethods.WinEventProcDef> _winEventProc;

		// Token: 0x040026CC RID: 9932
		private GCHandle _gchThis;

		// Token: 0x040026CD RID: 9933
		private WinEventHandler.WinEventHandlerShutDownListener _shutdownListener;

		// Token: 0x0200091B RID: 2331
		private sealed class WinEventHandlerShutDownListener : ShutDownListener
		{
			// Token: 0x0600860C RID: 34316 RVA: 0x0024B904 File Offset: 0x00249B04
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public WinEventHandlerShutDownListener(WinEventHandler target) : base(target, ShutDownEvents.DispatcherShutdown)
			{
			}

			// Token: 0x0600860D RID: 34317 RVA: 0x0024B910 File Offset: 0x00249B10
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				WinEventHandler winEventHandler = (WinEventHandler)target;
				winEventHandler.Stop();
			}
		}
	}
}
