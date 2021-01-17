using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.Primitives.DataGridDetailsPresenter" /> types to UI Automation.</summary>
	// Token: 0x020002A5 RID: 677
	public sealed class DataGridDetailsPresenterAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.DataGridDetailsPresenterAutomationPeer" /> class. </summary>
		/// <param name="owner">The element associated with this automation peer.</param>
		// Token: 0x060025E1 RID: 9697 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public DataGridDetailsPresenterAutomationPeer(DataGridDetailsPresenter owner) : base(owner)
		{
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000B3324 File Offset: 0x000B1524
		protected override string GetClassNameCore()
		{
			return base.Owner.GetType().Name;
		}
	}
}
