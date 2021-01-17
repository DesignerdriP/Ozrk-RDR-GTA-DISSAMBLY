using System;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002C8 RID: 712
	internal class ItemsControlItemAutomationPeer : ItemAutomationPeer
	{
		// Token: 0x06002756 RID: 10070 RVA: 0x000B9F49 File Offset: 0x000B8149
		public ItemsControlItemAutomationPeer(object item, ItemsControlWrapperAutomationPeer parent) : base(item, parent)
		{
		}

		// Token: 0x06002757 RID: 10071 RVA: 0x0009639A File Offset: 0x0009459A
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.DataItem;
		}

		// Token: 0x06002758 RID: 10072 RVA: 0x000B9F53 File Offset: 0x000B8153
		protected override string GetClassNameCore()
		{
			return "ItemsControlItem";
		}
	}
}
