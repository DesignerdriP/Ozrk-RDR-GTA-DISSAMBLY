using System;
using System.ComponentModel;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.SelectionMoving" /> and <see cref="E:System.Windows.Controls.InkCanvas.SelectionResizing" /> events. </summary>
	// Token: 0x0200056B RID: 1387
	public class InkCanvasSelectionEditingEventArgs : CancelEventArgs
	{
		// Token: 0x06005B79 RID: 23417 RVA: 0x0019C34C File Offset: 0x0019A54C
		internal InkCanvasSelectionEditingEventArgs(Rect oldRectangle, Rect newRectangle)
		{
			this._oldRectangle = oldRectangle;
			this._newRectangle = newRectangle;
		}

		/// <summary>Gets the bounds of the selection before the user moved or resized it.</summary>
		/// <returns>The bounds of the selection before the user moved or resized it.</returns>
		// Token: 0x17001628 RID: 5672
		// (get) Token: 0x06005B7A RID: 23418 RVA: 0x0019C362 File Offset: 0x0019A562
		public Rect OldRectangle
		{
			get
			{
				return this._oldRectangle;
			}
		}

		/// <summary>Gets or sets the bounds of the selection after it is moved or resized.</summary>
		/// <returns>The bounds of the selection after it is moved or resized.</returns>
		// Token: 0x17001629 RID: 5673
		// (get) Token: 0x06005B7B RID: 23419 RVA: 0x0019C36A File Offset: 0x0019A56A
		// (set) Token: 0x06005B7C RID: 23420 RVA: 0x0019C372 File Offset: 0x0019A572
		public Rect NewRectangle
		{
			get
			{
				return this._newRectangle;
			}
			set
			{
				this._newRectangle = value;
			}
		}

		// Token: 0x04002F85 RID: 12165
		private Rect _oldRectangle;

		// Token: 0x04002F86 RID: 12166
		private Rect _newRectangle;
	}
}
