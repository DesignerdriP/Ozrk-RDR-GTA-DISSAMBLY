using System;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.StrokeCollected" /> event. </summary>
	// Token: 0x02000565 RID: 1381
	public class InkCanvasStrokeCollectedEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.InkCanvasStrokeCollectedEventArgs" /> class.</summary>
		/// <param name="stroke">The collected <see cref="T:System.Windows.Ink.Stroke" /> object.</param>
		// Token: 0x06005B60 RID: 23392 RVA: 0x0019C1E0 File Offset: 0x0019A3E0
		public InkCanvasStrokeCollectedEventArgs(Stroke stroke) : base(InkCanvas.StrokeCollectedEvent)
		{
			if (stroke == null)
			{
				throw new ArgumentNullException("stroke");
			}
			this._stroke = stroke;
		}

		/// <summary>Gets the stroke that was added to the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The stroke that was added to the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001623 RID: 5667
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x0019C202 File Offset: 0x0019A402
		public Stroke Stroke
		{
			get
			{
				return this._stroke;
			}
		}

		/// <summary>Provides a way to invoke event handlers in a type-specific way.</summary>
		/// <param name="genericHandler">The event handler.</param>
		/// <param name="genericTarget">The event target.</param>
		// Token: 0x06005B62 RID: 23394 RVA: 0x0019C20C File Offset: 0x0019A40C
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			InkCanvasStrokeCollectedEventHandler inkCanvasStrokeCollectedEventHandler = (InkCanvasStrokeCollectedEventHandler)genericHandler;
			inkCanvasStrokeCollectedEventHandler(genericTarget, this);
		}

		// Token: 0x04002F7E RID: 12158
		private Stroke _stroke;
	}
}
