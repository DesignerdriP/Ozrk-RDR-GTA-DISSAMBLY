using System;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.StrokesReplaced" /> event. </summary>
	// Token: 0x02000567 RID: 1383
	public class InkCanvasStrokesReplacedEventArgs : EventArgs
	{
		// Token: 0x06005B67 RID: 23399 RVA: 0x0019C228 File Offset: 0x0019A428
		internal InkCanvasStrokesReplacedEventArgs(StrokeCollection newStrokes, StrokeCollection previousStrokes)
		{
			if (newStrokes == null)
			{
				throw new ArgumentNullException("newStrokes");
			}
			if (previousStrokes == null)
			{
				throw new ArgumentNullException("previousStrokes");
			}
			this._newStrokes = newStrokes;
			this._previousStrokes = previousStrokes;
		}

		/// <summary>Gets the new strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The new strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001624 RID: 5668
		// (get) Token: 0x06005B68 RID: 23400 RVA: 0x0019C25A File Offset: 0x0019A45A
		public StrokeCollection NewStrokes
		{
			get
			{
				return this._newStrokes;
			}
		}

		/// <summary>Gets the previous strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</summary>
		/// <returns>The previous strokes of the <see cref="T:System.Windows.Controls.InkCanvas" />.</returns>
		// Token: 0x17001625 RID: 5669
		// (get) Token: 0x06005B69 RID: 23401 RVA: 0x0019C262 File Offset: 0x0019A462
		public StrokeCollection PreviousStrokes
		{
			get
			{
				return this._previousStrokes;
			}
		}

		// Token: 0x04002F7F RID: 12159
		private StrokeCollection _newStrokes;

		// Token: 0x04002F80 RID: 12160
		private StrokeCollection _previousStrokes;
	}
}
