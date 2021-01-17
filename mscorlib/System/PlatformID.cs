using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Identifies the operating system, or platform, supported by an assembly.</summary>
	// Token: 0x02000122 RID: 290
	[ComVisible(true)]
	[Serializable]
	public enum PlatformID
	{
		/// <summary>The operating system is Win32s. Win32s is a layer that runs on 16-bit versions of Windows to provide access to 32-bit applications.</summary>
		// Token: 0x040005E4 RID: 1508
		Win32S,
		/// <summary>The operating system is Windows 95 or Windows 98.</summary>
		// Token: 0x040005E5 RID: 1509
		Win32Windows,
		/// <summary>The operating system is Windows NT or later.</summary>
		// Token: 0x040005E6 RID: 1510
		Win32NT,
		/// <summary>The operating system is Windows CE.</summary>
		// Token: 0x040005E7 RID: 1511
		WinCE,
		/// <summary>The operating system is Unix.</summary>
		// Token: 0x040005E8 RID: 1512
		Unix,
		/// <summary>The development platform is Xbox 360.</summary>
		// Token: 0x040005E9 RID: 1513
		Xbox,
		/// <summary>The operating system is Macintosh.</summary>
		// Token: 0x040005EA RID: 1514
		MacOSX
	}
}
