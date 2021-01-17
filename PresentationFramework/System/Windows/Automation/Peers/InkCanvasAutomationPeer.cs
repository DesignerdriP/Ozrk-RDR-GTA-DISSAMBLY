using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.InkCanvas" /> types to UI Automation.</summary>
	// Token: 0x020002C2 RID: 706
	public class InkCanvasAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.InkCanvas" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" />.</param>
		// Token: 0x060026FC RID: 9980 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public InkCanvasAutomationPeer(InkCanvas owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.InkCanvas" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "InkCanvas".</returns>
		// Token: 0x060026FD RID: 9981 RVA: 0x000B8A20 File Offset: 0x000B6C20
		protected override string GetClassNameCore()
		{
			return "InkCanvas";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.InkCanvas" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.InkCanvasAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.Custom" /> enumeration value.</returns>
		// Token: 0x060026FE RID: 9982 RVA: 0x00094967 File Offset: 0x00092B67
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}
	}
}
