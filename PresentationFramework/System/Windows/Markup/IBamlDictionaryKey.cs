﻿using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001DD RID: 477
	internal interface IBamlDictionaryKey
	{
		// Token: 0x06001F0C RID: 7948
		void UpdateValuePosition(int newPosition, BinaryWriter bamlBinaryWriter);

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x06001F0D RID: 7949
		// (set) Token: 0x06001F0E RID: 7950
		int ValuePosition { get; set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001F0F RID: 7951
		// (set) Token: 0x06001F10 RID: 7952
		object KeyObject { get; set; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001F11 RID: 7953
		// (set) Token: 0x06001F12 RID: 7954
		long ValuePositionPosition { get; set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001F13 RID: 7955
		// (set) Token: 0x06001F14 RID: 7956
		bool Shared { get; set; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001F15 RID: 7957
		// (set) Token: 0x06001F16 RID: 7958
		bool SharedSet { get; set; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001F17 RID: 7959
		// (set) Token: 0x06001F18 RID: 7960
		object[] StaticResourceValues { get; set; }
	}
}
