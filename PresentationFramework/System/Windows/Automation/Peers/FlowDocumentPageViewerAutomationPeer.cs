using System;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal.Documents;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> types to UI Automation.</summary>
	// Token: 0x020002B1 RID: 689
	public class FlowDocumentPageViewerAutomationPeer : DocumentViewerBaseAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.FlowDocumentPageViewerAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentPageViewerAutomationPeer" />.</param>
		// Token: 0x0600267D RID: 9853 RVA: 0x000B700E File Offset: 0x000B520E
		public FlowDocumentPageViewerAutomationPeer(FlowDocumentPageViewer owner) : base(owner)
		{
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentPageViewerAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x0600267E RID: 9854 RVA: 0x000B72D8 File Offset: 0x000B54D8
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			if (base.Owner is IFlowDocumentViewer && list != null && list.Count > 0 && list[list.Count - 1] is DocumentAutomationPeer)
			{
				list.RemoveAt(list.Count - 1);
				if (list.Count == 0)
				{
					list = null;
				}
			}
			return list;
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.FlowDocumentPageViewer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.FlowDocumentPageViewerAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains " FlowDocumentPageViewer".</returns>
		// Token: 0x0600267F RID: 9855 RVA: 0x000B7333 File Offset: 0x000B5533
		protected override string GetClassNameCore()
		{
			return "FlowDocumentPageViewer";
		}
	}
}
