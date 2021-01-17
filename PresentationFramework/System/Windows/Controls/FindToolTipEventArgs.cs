using System;

namespace System.Windows.Controls
{
	// Token: 0x0200054B RID: 1355
	internal sealed class FindToolTipEventArgs : RoutedEventArgs
	{
		// Token: 0x060058C2 RID: 22722 RVA: 0x00188E84 File Offset: 0x00187084
		internal FindToolTipEventArgs(ToolTip.ToolTipTrigger triggerAction)
		{
			base.RoutedEvent = ToolTipService.FindToolTipEvent;
			this._triggerAction = triggerAction;
		}

		// Token: 0x17001597 RID: 5527
		// (get) Token: 0x060058C3 RID: 22723 RVA: 0x00188E9E File Offset: 0x0018709E
		// (set) Token: 0x060058C4 RID: 22724 RVA: 0x00188EA6 File Offset: 0x001870A6
		internal DependencyObject TargetElement
		{
			get
			{
				return this._targetElement;
			}
			set
			{
				this._targetElement = value;
			}
		}

		// Token: 0x17001598 RID: 5528
		// (get) Token: 0x060058C5 RID: 22725 RVA: 0x00188EAF File Offset: 0x001870AF
		// (set) Token: 0x060058C6 RID: 22726 RVA: 0x00188EB7 File Offset: 0x001870B7
		internal bool KeepCurrentActive
		{
			get
			{
				return this._keepCurrentActive;
			}
			set
			{
				this._keepCurrentActive = value;
			}
		}

		// Token: 0x17001599 RID: 5529
		// (get) Token: 0x060058C7 RID: 22727 RVA: 0x00188EC0 File Offset: 0x001870C0
		internal ToolTip.ToolTipTrigger TriggerAction
		{
			get
			{
				return this._triggerAction;
			}
		}

		// Token: 0x060058C8 RID: 22728 RVA: 0x00188EC8 File Offset: 0x001870C8
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			FindToolTipEventHandler findToolTipEventHandler = (FindToolTipEventHandler)genericHandler;
			findToolTipEventHandler(genericTarget, this);
		}

		// Token: 0x04002EE2 RID: 12002
		private DependencyObject _targetElement;

		// Token: 0x04002EE3 RID: 12003
		private bool _keepCurrentActive;

		// Token: 0x04002EE4 RID: 12004
		private ToolTip.ToolTipTrigger _triggerAction;
	}
}
