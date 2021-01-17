﻿using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ListBox" /> types to UI Automation.</summary>
	// Token: 0x020002CC RID: 716
	public class ListBoxAutomationPeer : SelectorAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ListBoxAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ListBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxAutomationPeer" />.</param>
		// Token: 0x06002767 RID: 10087 RVA: 0x000B3CD4 File Offset: 0x000B1ED4
		public ListBoxAutomationPeer(ListBox owner) : base(owner)
		{
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> class.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListBoxItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxAutomationPeer" />.</param>
		/// <returns>A new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> class.</returns>
		// Token: 0x06002768 RID: 10088 RVA: 0x000B3CDD File Offset: 0x000B1EDD
		protected override ItemAutomationPeer CreateItemAutomationPeer(object item)
		{
			return new ListBoxItemAutomationPeer(item, this);
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ListBox" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ListBoxAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ListBox".</returns>
		// Token: 0x06002769 RID: 10089 RVA: 0x000B9FB1 File Offset: 0x000B81B1
		protected override string GetClassNameCore()
		{
			return "ListBox";
		}
	}
}
