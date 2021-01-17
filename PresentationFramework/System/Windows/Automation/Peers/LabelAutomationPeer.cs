using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Label" /> types to UI Automation.</summary>
	// Token: 0x020002CB RID: 715
	public class LabelAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.Label" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" />.</param>
		// Token: 0x06002763 RID: 10083 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public LabelAutomationPeer(Label owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.Label" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "Label".</returns>
		// Token: 0x06002764 RID: 10084 RVA: 0x000B9F6A File Offset: 0x000B816A
		protected override string GetClassNameCore()
		{
			return "Text";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.Label" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Text" /> enumeration value.</returns>
		// Token: 0x06002765 RID: 10085 RVA: 0x0009432F File Offset: 0x0009252F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Text;
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.Controls.Label" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.LabelAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>The string that contains the label.</returns>
		// Token: 0x06002766 RID: 10086 RVA: 0x000B9F74 File Offset: 0x000B8174
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore))
			{
				Label label = (Label)base.Owner;
				if (label.Content is string)
				{
					return AccessText.RemoveAccessKeyMarker(nameCore);
				}
			}
			return nameCore;
		}
	}
}
