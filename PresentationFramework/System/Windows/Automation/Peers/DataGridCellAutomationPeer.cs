using System;
using System.Windows.Controls;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.DataGridCell" /> types to UI Automation.</summary>
	// Token: 0x020002A0 RID: 672
	public sealed class DataGridCellAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridCellAutomationPeer" /> class. </summary>
		/// <param name="owner">An enumeration value that specifies the control pattern.</param>
		// Token: 0x0600257E RID: 9598 RVA: 0x000B469C File Offset: 0x000B289C
		public DataGridCellAutomationPeer(DataGridCell owner) : base(owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00094967 File Offset: 0x00092B67
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000B3324 File Offset: 0x000B1524
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}
	}
}
