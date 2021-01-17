using System;

namespace System.Windows.Controls
{
	// Token: 0x02000563 RID: 1379
	internal static class EditingModeHelper
	{
		// Token: 0x06005B5B RID: 23387 RVA: 0x0019C1D1 File Offset: 0x0019A3D1
		internal static bool IsDefined(InkCanvasEditingMode InkCanvasEditingMode)
		{
			return InkCanvasEditingMode >= InkCanvasEditingMode.None && InkCanvasEditingMode <= InkCanvasEditingMode.EraseByStroke;
		}
	}
}
