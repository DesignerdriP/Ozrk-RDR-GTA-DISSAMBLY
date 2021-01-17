﻿using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ToolTip" /> types to UI Automation.</summary>
	// Token: 0x020002EF RID: 751
	public class ToolTipAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ToolTip" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />.</param>
		// Token: 0x0600285C RID: 10332 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public ToolTipAutomationPeer(ToolTip owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ToolTip" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "ToolTip".</returns>
		// Token: 0x0600285D RID: 10333 RVA: 0x000BC39B File Offset: 0x000BA59B
		protected override string GetClassNameCore()
		{
			return "ToolTip";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ToolTip" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolTipAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ToolTip" /> enumeration value.</returns>
		// Token: 0x0600285E RID: 10334 RVA: 0x000BC3A2 File Offset: 0x000BA5A2
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ToolTip;
		}
	}
}
