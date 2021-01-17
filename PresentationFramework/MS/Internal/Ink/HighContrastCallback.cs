using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace MS.Internal.Ink
{
	// Token: 0x02000688 RID: 1672
	internal abstract class HighContrastCallback
	{
		// Token: 0x06006D6D RID: 28013
		internal abstract void TurnHighContrastOn(Color highContrastColor);

		// Token: 0x06006D6E RID: 28014
		internal abstract void TurnHighContrastOff();

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x06006D6F RID: 28015
		internal abstract Dispatcher Dispatcher { get; }
	}
}
