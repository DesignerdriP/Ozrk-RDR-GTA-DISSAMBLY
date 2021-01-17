using System;
using MS.Internal.AppModel;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002F4 RID: 756
	internal class RootBrowserWindowAutomationPeer : WindowAutomationPeer
	{
		// Token: 0x0600289C RID: 10396 RVA: 0x000BA519 File Offset: 0x000B8719
		public RootBrowserWindowAutomationPeer(RootBrowserWindow owner) : base(owner)
		{
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000BCE3F File Offset: 0x000BB03F
		protected override string GetClassNameCore()
		{
			return "RootBrowserWindow";
		}
	}
}
