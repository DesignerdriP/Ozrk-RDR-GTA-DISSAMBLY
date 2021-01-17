using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002D5 RID: 725
	internal class PopupRootAutomationPeer : FrameworkElementAutomationPeer
	{
		// Token: 0x060027A0 RID: 10144 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public PopupRootAutomationPeer(PopupRoot owner) : base(owner)
		{
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000BA694 File Offset: 0x000B8894
		protected override string GetClassNameCore()
		{
			return "Popup";
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000969C4 File Offset: 0x00094BC4
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Window;
		}
	}
}
