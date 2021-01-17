using System;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000629 RID: 1577
	internal sealed class LineVisual : DrawingVisual
	{
		// Token: 0x06006899 RID: 26777 RVA: 0x001D81F2 File Offset: 0x001D63F2
		internal DrawingContext Open()
		{
			return base.RenderOpen();
		}

		// Token: 0x040033E1 RID: 13281
		internal double WidthIncludingTrailingWhitespace;
	}
}
