﻿using System;
using System.Runtime.InteropServices;
using MS.Internal.Interop;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A6 RID: 1958
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 8)]
	internal struct THUMBBUTTON
	{
		// Token: 0x04003A0C RID: 14860
		public const int THBN_CLICKED = 6144;

		// Token: 0x04003A0D RID: 14861
		public THB dwMask;

		// Token: 0x04003A0E RID: 14862
		public uint iId;

		// Token: 0x04003A0F RID: 14863
		public uint iBitmap;

		// Token: 0x04003A10 RID: 14864
		public IntPtr hIcon;

		// Token: 0x04003A11 RID: 14865
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szTip;

		// Token: 0x04003A12 RID: 14866
		public THBF dwFlags;
	}
}
