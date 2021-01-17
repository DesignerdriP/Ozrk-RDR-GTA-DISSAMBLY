using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" /> types to UI Automation.</summary>
	// Token: 0x020002E2 RID: 738
	public class StatusBarItemAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.StatusBarItemAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.StatusBarItemAutomationPeer" />.</param>
		// Token: 0x06002808 RID: 10248 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public StatusBarItemAutomationPeer(StatusBarItem owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.StatusBarItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "StatusBarItem".</returns>
		// Token: 0x06002809 RID: 10249 RVA: 0x000BB746 File Offset: 0x000B9946
		protected override string GetClassNameCore()
		{
			return "StatusBarItem";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.StatusBarItemAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Text" /> enumeration value.</returns>
		// Token: 0x0600280A RID: 10250 RVA: 0x0009432F File Offset: 0x0009252F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Text;
		}
	}
}
