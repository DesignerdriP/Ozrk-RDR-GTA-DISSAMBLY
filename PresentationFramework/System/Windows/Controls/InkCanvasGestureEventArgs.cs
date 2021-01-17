using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.Gesture" /> event. </summary>
	// Token: 0x0200056F RID: 1391
	public class InkCanvasGestureEventArgs : RoutedEventArgs
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.InkCanvasGestureEventArgs" /> class. </summary>
		/// <param name="strokes">The strokes that represent the possible gesture.</param>
		/// <param name="gestureRecognitionResults">The results from the gesture recognizer.</param>
		// Token: 0x06005B87 RID: 23431 RVA: 0x0019C3A0 File Offset: 0x0019A5A0
		public InkCanvasGestureEventArgs(StrokeCollection strokes, IEnumerable<GestureRecognitionResult> gestureRecognitionResults) : base(InkCanvas.GestureEvent)
		{
			if (strokes == null)
			{
				throw new ArgumentNullException("strokes");
			}
			if (strokes.Count < 1)
			{
				throw new ArgumentException(SR.Get("InvalidEmptyStrokeCollection"), "strokes");
			}
			if (gestureRecognitionResults == null)
			{
				throw new ArgumentNullException("strokes");
			}
			List<GestureRecognitionResult> list = new List<GestureRecognitionResult>(gestureRecognitionResults);
			if (list.Count == 0)
			{
				throw new ArgumentException(SR.Get("InvalidEmptyArray"), "gestureRecognitionResults");
			}
			this._strokes = strokes;
			this._gestureRecognitionResults = list;
		}

		/// <summary>Gets the strokes that represent the possible gesture.</summary>
		/// <returns>The strokes that represent the possible gesture.</returns>
		// Token: 0x1700162B RID: 5675
		// (get) Token: 0x06005B88 RID: 23432 RVA: 0x0019C424 File Offset: 0x0019A624
		public StrokeCollection Strokes
		{
			get
			{
				return this._strokes;
			}
		}

		/// <summary>Returns results from the gesture recognizer.</summary>
		/// <returns>A collection of possible application gestures that the <see cref="P:System.Windows.Controls.InkCanvasGestureEventArgs.Strokes" /> might be.</returns>
		// Token: 0x06005B89 RID: 23433 RVA: 0x0019C42C File Offset: 0x0019A62C
		public ReadOnlyCollection<GestureRecognitionResult> GetGestureRecognitionResults()
		{
			return new ReadOnlyCollection<GestureRecognitionResult>(this._gestureRecognitionResults);
		}

		/// <summary>Gets or sets a Boolean value that indicates whether strokes should be considered a gesture.</summary>
		/// <returns>
		///     <see langword="true" /> if the strokes are ink; <see langword="false" /> if the strokes are a gesture.</returns>
		// Token: 0x1700162C RID: 5676
		// (get) Token: 0x06005B8A RID: 23434 RVA: 0x0019C439 File Offset: 0x0019A639
		// (set) Token: 0x06005B8B RID: 23435 RVA: 0x0019C441 File Offset: 0x0019A641
		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		/// <summary>When overridden in a derived class, provides a way to invoke event handlers in a type-specific way, which can increase efficiency over the base implementation.</summary>
		/// <param name="genericHandler">The generic handler / delegate implementation to be invoked.</param>
		/// <param name="genericTarget">The target on which the provided handler should be invoked.</param>
		// Token: 0x06005B8C RID: 23436 RVA: 0x0019C44C File Offset: 0x0019A64C
		protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
		{
			InkCanvasGestureEventHandler inkCanvasGestureEventHandler = (InkCanvasGestureEventHandler)genericHandler;
			inkCanvasGestureEventHandler(genericTarget, this);
		}

		// Token: 0x04002F88 RID: 12168
		private StrokeCollection _strokes;

		// Token: 0x04002F89 RID: 12169
		private List<GestureRecognitionResult> _gestureRecognitionResults;

		// Token: 0x04002F8A RID: 12170
		private bool _cancel;
	}
}
