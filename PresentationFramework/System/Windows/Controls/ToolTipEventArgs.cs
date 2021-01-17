using System;

namespace System.Windows.Controls
{
	/// <summary>Provides event information for events that occur when a tooltip opens or closes.</summary>
	// Token: 0x02000549 RID: 1353
	public sealed class ToolTipEventArgs : RoutedEventArgs
	{
		// Token: 0x060058BC RID: 22716 RVA: 0x00188E45 File Offset: 0x00187045
		internal ToolTipEventArgs(bool opening)
		{
			if (opening)
			{
				base.RoutedEvent = ToolTipService.ToolTipOpeningEvent;
				return;
			}
			base.RoutedEvent = ToolTipService.ToolTipClosingEvent;
		}

		// Token: 0x060058BD RID: 22717 RVA: 0x00188E68 File Offset: 0x00187068
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			ToolTipEventHandler toolTipEventHandler = (ToolTipEventHandler)genericHandler;
			toolTipEventHandler(genericTarget, this);
		}
	}
}
