using System;
using System.Security;
using MS.Internal;

namespace System.Windows.Interop
{
	/// <summary>Assists interoperation between Windows Presentation Foundation (WPF) and Win32 code. </summary>
	// Token: 0x020005C1 RID: 1473
	public sealed class WindowInteropHelper
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Interop.WindowInteropHelper" /> class for a specified Windows Presentation Foundation (WPF) window. </summary>
		/// <param name="window">A WPF window object.</param>
		// Token: 0x06006242 RID: 25154 RVA: 0x001B8992 File Offset: 0x001B6B92
		public WindowInteropHelper(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			this._window = window;
		}

		/// <summary>Gets the window handle for a Windows Presentation Foundation (WPF) window that is used to create this <see cref="T:System.Windows.Interop.WindowInteropHelper" />. </summary>
		/// <returns>The Windows Presentation Foundation (WPF) window handle (HWND).</returns>
		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x06006243 RID: 25155 RVA: 0x001B89AF File Offset: 0x001B6BAF
		public IntPtr Handle
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUIWindowPermission();
				return this.CriticalHandle;
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x06006244 RID: 25156 RVA: 0x001B89BC File Offset: 0x001B6BBC
		internal IntPtr CriticalHandle
		{
			[SecurityCritical]
			get
			{
				Invariant.Assert(this._window != null, "Cannot be null since we verify in the constructor");
				return this._window.CriticalHandle;
			}
		}

		/// <summary>Gets or sets the handle of the Windows Presentation Foundation (WPF) owner window. </summary>
		/// <returns>The owner window handle (HWND).</returns>
		// Token: 0x1700179F RID: 6047
		// (get) Token: 0x06006245 RID: 25157 RVA: 0x001B89DC File Offset: 0x001B6BDC
		// (set) Token: 0x06006246 RID: 25158 RVA: 0x001B89EE File Offset: 0x001B6BEE
		public IntPtr Owner
		{
			[SecurityCritical]
			get
			{
				SecurityHelper.DemandUIWindowPermission();
				return this._window.OwnerHandle;
			}
			[SecurityCritical]
			set
			{
				SecurityHelper.DemandUIWindowPermission();
				this._window.OwnerHandle = value;
			}
		}

		/// <summary>Creates the HWND of the window if the HWND has not been created yet.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that represents the HWND.</returns>
		// Token: 0x06006247 RID: 25159 RVA: 0x001B8A01 File Offset: 0x001B6C01
		[SecurityCritical]
		public IntPtr EnsureHandle()
		{
			SecurityHelper.DemandUIWindowPermission();
			if (this.CriticalHandle == IntPtr.Zero)
			{
				this._window.CreateSourceWindow(false);
			}
			return this.CriticalHandle;
		}

		// Token: 0x0400318A RID: 12682
		private Window _window;
	}
}
