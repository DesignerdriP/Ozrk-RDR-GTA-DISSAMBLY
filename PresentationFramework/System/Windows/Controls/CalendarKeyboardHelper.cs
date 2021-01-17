using System;
using System.Windows.Input;

namespace System.Windows.Controls
{
	// Token: 0x0200047A RID: 1146
	internal static class CalendarKeyboardHelper
	{
		// Token: 0x06004321 RID: 17185 RVA: 0x001332E8 File Offset: 0x001314E8
		public static void GetMetaKeyState(out bool ctrl, out bool shift)
		{
			ctrl = ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control);
			shift = ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
		}
	}
}
