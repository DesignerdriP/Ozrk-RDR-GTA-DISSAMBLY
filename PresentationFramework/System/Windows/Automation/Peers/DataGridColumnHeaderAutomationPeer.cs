using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.DataGridColumnHeader" /> types to UI Automation.</summary>
	// Token: 0x020002A3 RID: 675
	public sealed class DataGridColumnHeaderAutomationPeer : ButtonBaseAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridColumnHeaderAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x060025D5 RID: 9685 RVA: 0x000B2F7E File Offset: 0x000B117E
		public DataGridColumnHeaderAutomationPeer(DataGridColumnHeader owner) : base(owner)
		{
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00094E7F File Offset: 0x0009307F
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.HeaderItem;
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x000B3324 File Offset: 0x000B1524
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool IsContentElementCore()
		{
			return false;
		}
	}
}
