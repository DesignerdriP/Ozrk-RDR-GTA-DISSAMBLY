using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ToolBar" /> types to UI Automation.</summary>
	// Token: 0x020002EE RID: 750
	public class ToolBarAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.ToolBarAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ToolBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolBarAutomationPeer" />.</param>
		// Token: 0x06002859 RID: 10329 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public ToolBarAutomationPeer(ToolBar owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.ToolBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolBarAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains the word "ToolBar".</returns>
		// Token: 0x0600285A RID: 10330 RVA: 0x000BC390 File Offset: 0x000BA590
		protected override string GetClassNameCore()
		{
			return "ToolBar";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.ToolBar" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ToolBarAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.ToolBar" /> enumeration value.</returns>
		// Token: 0x0600285B RID: 10331 RVA: 0x000BC397 File Offset: 0x000BA597
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.ToolBar;
		}
	}
}
