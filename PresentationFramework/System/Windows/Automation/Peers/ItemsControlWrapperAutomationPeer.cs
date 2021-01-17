using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002C9 RID: 713
	internal class ItemsControlWrapperAutomationPeer : ItemsControlAutomationPeer
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x000B53BC File Offset: 0x000B35BC
		public ItemsControlWrapperAutomationPeer(ItemsControl owner) : base(owner)
		{
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000B9F5A File Offset: 0x000B815A
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new ItemsControlItemAutomationPeer(item, this);
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000B9F63 File Offset: 0x000B8163
		protected override string GetClassNameCore()
		{
			return "ItemsControl";
		}

		// Token: 0x0600275C RID: 10076 RVA: 0x000956EC File Offset: 0x000938EC
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.List;
		}
	}
}
