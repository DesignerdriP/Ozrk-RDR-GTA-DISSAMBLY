using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Input;

namespace MS.Internal.Ink
{
	// Token: 0x02000694 RID: 1684
	internal abstract class StylusEditingBehavior : EditingBehavior, IStylusEditing
	{
		// Token: 0x06006DF1 RID: 28145 RVA: 0x001F990B File Offset: 0x001F7B0B
		internal StylusEditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06006DF2 RID: 28146 RVA: 0x001FA2CC File Offset: 0x001F84CC
		internal void SwitchToMode(InkCanvasEditingMode mode)
		{
			this._disableInput = true;
			try
			{
				this.OnSwitchToMode(mode);
			}
			finally
			{
				this._disableInput = false;
			}
		}

		// Token: 0x06006DF3 RID: 28147 RVA: 0x001FA304 File Offset: 0x001F8504
		[SecurityCritical]
		void IStylusEditing.AddStylusPoints(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (this._disableInput)
			{
				return;
			}
			if (!base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = true;
				this.StylusInputBegin(stylusPoints, userInitiated);
				return;
			}
			this.StylusInputContinue(stylusPoints, userInitiated);
		}

		// Token: 0x06006DF4 RID: 28148
		protected abstract void OnSwitchToMode(InkCanvasEditingMode mode);

		// Token: 0x06006DF5 RID: 28149 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnActivate()
		{
		}

		// Token: 0x06006DF6 RID: 28150 RVA: 0x00002137 File Offset: 0x00000337
		protected override void OnDeactivate()
		{
		}

		// Token: 0x06006DF7 RID: 28151 RVA: 0x001FA339 File Offset: 0x001F8539
		protected sealed override void OnCommit(bool commit)
		{
			if (base.EditingCoordinator.UserIsEditing)
			{
				base.EditingCoordinator.UserIsEditing = false;
				this.StylusInputEnd(commit);
				return;
			}
			this.OnCommitWithoutStylusInput(commit);
		}

		// Token: 0x06006DF8 RID: 28152 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		protected virtual void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
		}

		// Token: 0x06006DF9 RID: 28153 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		protected virtual void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
		}

		// Token: 0x06006DFA RID: 28154 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void StylusInputEnd(bool commit)
		{
		}

		// Token: 0x06006DFB RID: 28155 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnCommitWithoutStylusInput(bool commit)
		{
		}

		// Token: 0x0400361D RID: 13853
		private bool _disableInput;
	}
}
