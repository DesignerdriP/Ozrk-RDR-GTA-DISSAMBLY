using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MS.Internal.Ink
{
	// Token: 0x02000684 RID: 1668
	internal abstract class EditingBehavior
	{
		// Token: 0x06006D18 RID: 27928 RVA: 0x001F5848 File Offset: 0x001F3A48
		internal EditingBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			if (editingCoordinator == null)
			{
				throw new ArgumentNullException("editingCoordinator");
			}
			this._inkCanvas = inkCanvas;
			this._editingCoordinator = editingCoordinator;
		}

		// Token: 0x06006D19 RID: 27929 RVA: 0x001F587A File Offset: 0x001F3A7A
		public void Activate()
		{
			this.OnActivate();
		}

		// Token: 0x06006D1A RID: 27930 RVA: 0x001F5882 File Offset: 0x001F3A82
		public void Deactivate()
		{
			this.OnDeactivate();
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x001F588A File Offset: 0x001F3A8A
		public void Commit(bool commit)
		{
			this.OnCommit(commit);
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x001F5893 File Offset: 0x001F3A93
		public void UpdateTransform()
		{
			if (!this.EditingCoordinator.IsTransformValid(this))
			{
				this.OnTransformChanged();
			}
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x06006D1D RID: 27933 RVA: 0x001F58A9 File Offset: 0x001F3AA9
		public Cursor Cursor
		{
			get
			{
				if (this._cachedCursor == null || !this.EditingCoordinator.IsCursorValid(this))
				{
					this._cachedCursor = this.GetCurrentCursor();
				}
				return this._cachedCursor;
			}
		}

		// Token: 0x06006D1E RID: 27934
		protected abstract void OnActivate();

		// Token: 0x06006D1F RID: 27935
		protected abstract void OnDeactivate();

		// Token: 0x06006D20 RID: 27936
		protected abstract void OnCommit(bool commit);

		// Token: 0x06006D21 RID: 27937
		protected abstract Cursor GetCurrentCursor();

		// Token: 0x06006D22 RID: 27938 RVA: 0x001F58D3 File Offset: 0x001F3AD3
		protected void SelfDeactivate()
		{
			this.EditingCoordinator.DeactivateDynamicBehavior();
		}

		// Token: 0x06006D23 RID: 27939 RVA: 0x001F58E0 File Offset: 0x001F3AE0
		protected Matrix GetElementTransformMatrix()
		{
			Transform layoutTransform = this.InkCanvas.LayoutTransform;
			Transform renderTransform = this.InkCanvas.RenderTransform;
			Matrix value = layoutTransform.Value;
			return value * renderTransform.Value;
		}

		// Token: 0x06006D24 RID: 27940 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnTransformChanged()
		{
		}

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x06006D25 RID: 27941 RVA: 0x001F591A File Offset: 0x001F3B1A
		protected InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x06006D26 RID: 27942 RVA: 0x001F5922 File Offset: 0x001F3B22
		protected EditingCoordinator EditingCoordinator
		{
			get
			{
				return this._editingCoordinator;
			}
		}

		// Token: 0x040035D1 RID: 13777
		private InkCanvas _inkCanvas;

		// Token: 0x040035D2 RID: 13778
		private EditingCoordinator _editingCoordinator;

		// Token: 0x040035D3 RID: 13779
		private Cursor _cachedCursor;
	}
}
