using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Ink;

namespace System.Windows.Controls
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.InkCanvas.SelectionChanging" />.</summary>
	// Token: 0x02000569 RID: 1385
	public class InkCanvasSelectionChangingEventArgs : CancelEventArgs
	{
		// Token: 0x06005B6E RID: 23406 RVA: 0x0019C26C File Offset: 0x0019A46C
		internal InkCanvasSelectionChangingEventArgs(StrokeCollection selectedStrokes, IEnumerable<UIElement> selectedElements)
		{
			if (selectedStrokes == null)
			{
				throw new ArgumentNullException("selectedStrokes");
			}
			if (selectedElements == null)
			{
				throw new ArgumentNullException("selectedElements");
			}
			this._strokes = selectedStrokes;
			List<UIElement> elements = new List<UIElement>(selectedElements);
			this._elements = elements;
			this._strokesChanged = false;
			this._elementsChanged = false;
		}

		// Token: 0x17001626 RID: 5670
		// (get) Token: 0x06005B6F RID: 23407 RVA: 0x0019C2BE File Offset: 0x0019A4BE
		internal bool StrokesChanged
		{
			get
			{
				return this._strokesChanged;
			}
		}

		// Token: 0x17001627 RID: 5671
		// (get) Token: 0x06005B70 RID: 23408 RVA: 0x0019C2C6 File Offset: 0x0019A4C6
		internal bool ElementsChanged
		{
			get
			{
				return this._elementsChanged;
			}
		}

		/// <summary>Sets the selected elements.</summary>
		/// <param name="selectedElements">The elements to select.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="selectedElements" /> is <see langword="null" />.</exception>
		// Token: 0x06005B71 RID: 23409 RVA: 0x0019C2D0 File Offset: 0x0019A4D0
		public void SetSelectedElements(IEnumerable<UIElement> selectedElements)
		{
			if (selectedElements == null)
			{
				throw new ArgumentNullException("selectedElements");
			}
			List<UIElement> elements = new List<UIElement>(selectedElements);
			this._elements = elements;
			this._elementsChanged = true;
		}

		/// <summary>Returns the selected elements.</summary>
		/// <returns>The selected elements.</returns>
		// Token: 0x06005B72 RID: 23410 RVA: 0x0019C300 File Offset: 0x0019A500
		public ReadOnlyCollection<UIElement> GetSelectedElements()
		{
			return new ReadOnlyCollection<UIElement>(this._elements);
		}

		/// <summary>Sets the selected strokes.</summary>
		/// <param name="selectedStrokes">The strokes to select.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="selectedStrokes" /> is <see langword="null" />.</exception>
		// Token: 0x06005B73 RID: 23411 RVA: 0x0019C30D File Offset: 0x0019A50D
		public void SetSelectedStrokes(StrokeCollection selectedStrokes)
		{
			if (selectedStrokes == null)
			{
				throw new ArgumentNullException("selectedStrokes");
			}
			this._strokes = selectedStrokes;
			this._strokesChanged = true;
		}

		/// <summary>Returns the selected strokes.</summary>
		/// <returns>The selected strokes.</returns>
		// Token: 0x06005B74 RID: 23412 RVA: 0x0019C32C File Offset: 0x0019A52C
		public StrokeCollection GetSelectedStrokes()
		{
			return new StrokeCollection
			{
				this._strokes
			};
		}

		// Token: 0x04002F81 RID: 12161
		private StrokeCollection _strokes;

		// Token: 0x04002F82 RID: 12162
		private List<UIElement> _elements;

		// Token: 0x04002F83 RID: 12163
		private bool _strokesChanged;

		// Token: 0x04002F84 RID: 12164
		private bool _elementsChanged;
	}
}
