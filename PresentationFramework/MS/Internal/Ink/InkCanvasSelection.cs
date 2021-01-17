using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using MS.Internal.Controls;

namespace MS.Internal.Ink
{
	// Token: 0x0200068A RID: 1674
	internal sealed class InkCanvasSelection
	{
		// Token: 0x06006D76 RID: 28022 RVA: 0x001F6D3E File Offset: 0x001F4F3E
		internal InkCanvasSelection(InkCanvas inkCanvas)
		{
			if (inkCanvas == null)
			{
				throw new ArgumentNullException("inkCanvas");
			}
			this._inkCanvas = inkCanvas;
			this._inkCanvas.FeedbackAdorner.UpdateBounds(Rect.Empty);
		}

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x06006D77 RID: 28023 RVA: 0x001F6D70 File Offset: 0x001F4F70
		internal StrokeCollection SelectedStrokes
		{
			get
			{
				if (this._selectedStrokes == null)
				{
					this._selectedStrokes = new StrokeCollection();
					this._areStrokesChanged = true;
				}
				return this._selectedStrokes;
			}
		}

		// Token: 0x17001A1C RID: 6684
		// (get) Token: 0x06006D78 RID: 28024 RVA: 0x001F6D92 File Offset: 0x001F4F92
		internal ReadOnlyCollection<UIElement> SelectedElements
		{
			get
			{
				if (this._selectedElements == null)
				{
					this._selectedElements = new List<UIElement>();
				}
				return new ReadOnlyCollection<UIElement>(this._selectedElements);
			}
		}

		// Token: 0x17001A1D RID: 6685
		// (get) Token: 0x06006D79 RID: 28025 RVA: 0x001F6DB2 File Offset: 0x001F4FB2
		internal bool HasSelection
		{
			get
			{
				return this.SelectedStrokes.Count != 0 || this.SelectedElements.Count != 0;
			}
		}

		// Token: 0x17001A1E RID: 6686
		// (get) Token: 0x06006D7A RID: 28026 RVA: 0x001F6DD1 File Offset: 0x001F4FD1
		internal Rect SelectionBounds
		{
			get
			{
				return Rect.Union(this.GetStrokesBounds(), this.GetElementsUnionBounds());
			}
		}

		// Token: 0x06006D7B RID: 28027 RVA: 0x001F6DE4 File Offset: 0x001F4FE4
		internal void StartFeedbackAdorner(Rect feedbackRect, InkCanvasSelectionHitResult activeSelectionHitResult)
		{
			this._activeSelectionHitResult = new InkCanvasSelectionHitResult?(activeSelectionHitResult);
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._inkCanvas.InnerCanvas);
			InkCanvasFeedbackAdorner feedbackAdorner = this._inkCanvas.FeedbackAdorner;
			adornerLayer.Add(feedbackAdorner);
			feedbackAdorner.UpdateBounds(feedbackRect);
		}

		// Token: 0x06006D7C RID: 28028 RVA: 0x001F6E28 File Offset: 0x001F5028
		internal void UpdateFeedbackAdorner(Rect feedbackRect)
		{
			this._inkCanvas.FeedbackAdorner.UpdateBounds(feedbackRect);
		}

		// Token: 0x06006D7D RID: 28029 RVA: 0x001F6E3C File Offset: 0x001F503C
		internal void EndFeedbackAdorner(Rect finalRectangle)
		{
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this._inkCanvas.InnerCanvas);
			InkCanvasFeedbackAdorner feedbackAdorner = this._inkCanvas.FeedbackAdorner;
			feedbackAdorner.UpdateBounds(Rect.Empty);
			adornerLayer.Remove(feedbackAdorner);
			this.CommitChanges(finalRectangle, true);
			this._activeSelectionHitResult = null;
		}

		// Token: 0x06006D7E RID: 28030 RVA: 0x001F6E8C File Offset: 0x001F508C
		internal void Select(StrokeCollection strokes, IList<UIElement> elements, bool raiseSelectionChanged)
		{
			bool flag;
			bool flag2;
			this.SelectionIsDifferentThanCurrent(strokes, out flag, elements, out flag2);
			if (flag || flag2)
			{
				if (flag && this.SelectedStrokes.Count != 0)
				{
					this.QuitListeningToStrokeChanges();
					int count = this.SelectedStrokes.Count;
					for (int i = 0; i < count; i++)
					{
						this.SelectedStrokes[i].IsSelected = false;
					}
				}
				this._selectedStrokes = strokes;
				this._areStrokesChanged = true;
				this._selectedElements = new List<UIElement>(elements);
				if (this._inkCanvas.ActiveEditingMode == InkCanvasEditingMode.Select)
				{
					int count = strokes.Count;
					for (int j = 0; j < count; j++)
					{
						strokes[j].IsSelected = true;
					}
				}
				this.UpdateCanvasLayoutUpdatedHandler();
				this.UpdateSelectionAdorner();
				this.ListenToStrokeChanges();
				if (raiseSelectionChanged)
				{
					this._inkCanvas.RaiseSelectionChanged(EventArgs.Empty);
				}
			}
		}

		// Token: 0x06006D7F RID: 28031 RVA: 0x001F6F64 File Offset: 0x001F5164
		internal void CommitChanges(Rect finalRectangle, bool raiseEvent)
		{
			Rect selectionBounds = this.SelectionBounds;
			if (selectionBounds.IsEmpty)
			{
				return;
			}
			try
			{
				this.QuitListeningToStrokeChanges();
				if (raiseEvent)
				{
					if (!DoubleUtil.AreClose(finalRectangle.Height, selectionBounds.Height) || !DoubleUtil.AreClose(finalRectangle.Width, selectionBounds.Width))
					{
						this.CommitResizeChange(finalRectangle);
					}
					else if (!DoubleUtil.AreClose(finalRectangle.Top, selectionBounds.Top) || !DoubleUtil.AreClose(finalRectangle.Left, selectionBounds.Left))
					{
						this.CommitMoveChange(finalRectangle);
					}
				}
				else
				{
					this.MoveSelection(selectionBounds, finalRectangle);
				}
			}
			finally
			{
				this.ListenToStrokeChanges();
			}
		}

		// Token: 0x06006D80 RID: 28032 RVA: 0x001F7014 File Offset: 0x001F5214
		internal void RemoveElement(UIElement removedElement)
		{
			if (this._selectedElements == null || this._selectedElements.Count == 0)
			{
				return;
			}
			if (this._selectedElements.Remove(removedElement) && this._selectedElements.Count == 0)
			{
				this.UpdateCanvasLayoutUpdatedHandler();
				this.UpdateSelectionAdorner();
			}
		}

		// Token: 0x06006D81 RID: 28033 RVA: 0x001F7053 File Offset: 0x001F5253
		internal void UpdateElementBounds(UIElement element, Matrix transform)
		{
			this.UpdateElementBounds(element, element, transform);
		}

		// Token: 0x06006D82 RID: 28034 RVA: 0x001F7060 File Offset: 0x001F5260
		internal void UpdateElementBounds(UIElement originalElement, UIElement updatedElement, Matrix transform)
		{
			if (originalElement.DependencyObjectType.Id == updatedElement.DependencyObjectType.Id)
			{
				GeneralTransform generalTransform = originalElement.TransformToAncestor(this._inkCanvas.InnerCanvas);
				FrameworkElement frameworkElement = originalElement as FrameworkElement;
				Thickness thickness = default(Thickness);
				Size renderSize;
				if (frameworkElement == null)
				{
					renderSize = originalElement.RenderSize;
				}
				else
				{
					renderSize = new Size(frameworkElement.ActualWidth, frameworkElement.ActualHeight);
					thickness = frameworkElement.Margin;
				}
				Rect rect = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
				rect = generalTransform.TransformBounds(rect);
				Rect rect2 = Rect.Transform(rect, transform);
				if (!DoubleUtil.AreClose(rect.Width, rect2.Width))
				{
					if (frameworkElement == null)
					{
						Size renderSize2 = originalElement.RenderSize;
						renderSize2.Width = rect2.Width;
						updatedElement.RenderSize = renderSize2;
					}
					else
					{
						((FrameworkElement)updatedElement).Width = rect2.Width;
					}
				}
				if (!DoubleUtil.AreClose(rect.Height, rect2.Height))
				{
					if (frameworkElement == null)
					{
						Size renderSize3 = originalElement.RenderSize;
						renderSize3.Height = rect2.Height;
						updatedElement.RenderSize = renderSize3;
					}
					else
					{
						((FrameworkElement)updatedElement).Height = rect2.Height;
					}
				}
				double left = InkCanvas.GetLeft(originalElement);
				double top = InkCanvas.GetTop(originalElement);
				double right = InkCanvas.GetRight(originalElement);
				double bottom = InkCanvas.GetBottom(originalElement);
				Point point = default(Point);
				if (!double.IsNaN(left))
				{
					point.X = left;
				}
				else if (!double.IsNaN(right))
				{
					point.X = right;
				}
				if (!double.IsNaN(top))
				{
					point.Y = top;
				}
				else if (!double.IsNaN(bottom))
				{
					point.Y = bottom;
				}
				Point point2 = point * transform;
				if (!double.IsNaN(left))
				{
					InkCanvas.SetLeft(updatedElement, point2.X - thickness.Left);
				}
				else if (!double.IsNaN(right))
				{
					InkCanvas.SetRight(updatedElement, right - (point2.X - point.X));
				}
				else
				{
					InkCanvas.SetLeft(updatedElement, point2.X - thickness.Left);
				}
				if (!double.IsNaN(top))
				{
					InkCanvas.SetTop(updatedElement, point2.Y - thickness.Top);
					return;
				}
				if (!double.IsNaN(bottom))
				{
					InkCanvas.SetBottom(updatedElement, bottom - (point2.Y - point.Y));
					return;
				}
				InkCanvas.SetTop(updatedElement, point2.Y - thickness.Top);
			}
		}

		// Token: 0x06006D83 RID: 28035 RVA: 0x001F72CA File Offset: 0x001F54CA
		internal void TransformStrokes(StrokeCollection strokes, Matrix matrix)
		{
			strokes.Transform(matrix, false);
		}

		// Token: 0x06006D84 RID: 28036 RVA: 0x001F72D4 File Offset: 0x001F54D4
		internal InkCanvasSelectionHitResult HitTestSelection(Point pointOnInkCanvas)
		{
			if (this._activeSelectionHitResult != null)
			{
				return this._activeSelectionHitResult.Value;
			}
			if (!this.HasSelection)
			{
				return InkCanvasSelectionHitResult.None;
			}
			GeneralTransform generalTransform = this._inkCanvas.TransformToDescendant(this._inkCanvas.SelectionAdorner);
			Point point = generalTransform.Transform(pointOnInkCanvas);
			InkCanvasSelectionHitResult inkCanvasSelectionHitResult = this._inkCanvas.SelectionAdorner.SelectionHandleHitTest(point);
			if (inkCanvasSelectionHitResult == InkCanvasSelectionHitResult.Selection && this.SelectedElements.Count == 1 && this.SelectedStrokes.Count == 0)
			{
				GeneralTransform generalTransform2 = this._inkCanvas.TransformToDescendant(this._inkCanvas.InnerCanvas);
				Point pointOnInnerCanvas = generalTransform2.Transform(pointOnInkCanvas);
				if (this.HasHitSingleSelectedElement(pointOnInnerCanvas))
				{
					inkCanvasSelectionHitResult = InkCanvasSelectionHitResult.None;
				}
			}
			return inkCanvasSelectionHitResult;
		}

		// Token: 0x06006D85 RID: 28037 RVA: 0x001F7384 File Offset: 0x001F5584
		internal void SelectionIsDifferentThanCurrent(StrokeCollection strokes, out bool strokesAreDifferent, IList<UIElement> elements, out bool elementsAreDifferent)
		{
			strokesAreDifferent = false;
			elementsAreDifferent = false;
			if (this.SelectedStrokes.Count == 0)
			{
				if (strokes.Count > 0)
				{
					strokesAreDifferent = true;
				}
			}
			else if (!InkCanvasSelection.StrokesAreEqual(this.SelectedStrokes, strokes))
			{
				strokesAreDifferent = true;
			}
			if (this.SelectedElements.Count == 0)
			{
				if (elements.Count > 0)
				{
					elementsAreDifferent = true;
					return;
				}
			}
			else if (!InkCanvasSelection.FrameworkElementArraysAreEqual(elements, this.SelectedElements))
			{
				elementsAreDifferent = true;
			}
		}

		// Token: 0x06006D86 RID: 28038 RVA: 0x001F73F4 File Offset: 0x001F55F4
		private bool HasHitSingleSelectedElement(Point pointOnInnerCanvas)
		{
			bool result = false;
			if (this.SelectedElements.Count == 1)
			{
				IEnumerator<Rect> enumerator = this.SelectedElementsBoundsEnumerator.GetEnumerator();
				if (enumerator.MoveNext())
				{
					Rect rect = enumerator.Current;
					result = rect.Contains(pointOnInnerCanvas);
				}
			}
			return result;
		}

		// Token: 0x06006D87 RID: 28039 RVA: 0x001F7438 File Offset: 0x001F5638
		private void QuitListeningToStrokeChanges()
		{
			if (this._inkCanvas.Strokes != null)
			{
				this._inkCanvas.Strokes.StrokesChanged -= this.OnStrokeCollectionChanged;
			}
			foreach (Stroke stroke in this.SelectedStrokes)
			{
				stroke.Invalidated -= this.OnStrokeInvalidated;
			}
		}

		// Token: 0x06006D88 RID: 28040 RVA: 0x001F74BC File Offset: 0x001F56BC
		private void ListenToStrokeChanges()
		{
			if (this._inkCanvas.Strokes != null)
			{
				this._inkCanvas.Strokes.StrokesChanged += this.OnStrokeCollectionChanged;
			}
			foreach (Stroke stroke in this.SelectedStrokes)
			{
				stroke.Invalidated += this.OnStrokeInvalidated;
			}
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x001F7540 File Offset: 0x001F5740
		private void CommitMoveChange(Rect finalRectangle)
		{
			Rect selectionBounds = this.SelectionBounds;
			InkCanvasSelectionEditingEventArgs inkCanvasSelectionEditingEventArgs = new InkCanvasSelectionEditingEventArgs(selectionBounds, finalRectangle);
			this._inkCanvas.RaiseSelectionMoving(inkCanvasSelectionEditingEventArgs);
			if (!inkCanvasSelectionEditingEventArgs.Cancel)
			{
				if (finalRectangle != inkCanvasSelectionEditingEventArgs.NewRectangle)
				{
					finalRectangle = inkCanvasSelectionEditingEventArgs.NewRectangle;
				}
				this.MoveSelection(selectionBounds, finalRectangle);
				this._inkCanvas.RaiseSelectionMoved(EventArgs.Empty);
			}
		}

		// Token: 0x06006D8A RID: 28042 RVA: 0x001F75A0 File Offset: 0x001F57A0
		private void CommitResizeChange(Rect finalRectangle)
		{
			Rect selectionBounds = this.SelectionBounds;
			InkCanvasSelectionEditingEventArgs inkCanvasSelectionEditingEventArgs = new InkCanvasSelectionEditingEventArgs(selectionBounds, finalRectangle);
			this._inkCanvas.RaiseSelectionResizing(inkCanvasSelectionEditingEventArgs);
			if (!inkCanvasSelectionEditingEventArgs.Cancel)
			{
				if (finalRectangle != inkCanvasSelectionEditingEventArgs.NewRectangle)
				{
					finalRectangle = inkCanvasSelectionEditingEventArgs.NewRectangle;
				}
				this.MoveSelection(selectionBounds, finalRectangle);
				this._inkCanvas.RaiseSelectionResized(EventArgs.Empty);
			}
		}

		// Token: 0x06006D8B RID: 28043 RVA: 0x001F7600 File Offset: 0x001F5800
		private void MoveSelection(Rect previousRect, Rect newRect)
		{
			Matrix matrix = InkCanvasSelection.MapRectToRect(newRect, previousRect);
			int count = this.SelectedElements.Count;
			IList<UIElement> selectedElements = this.SelectedElements;
			for (int i = 0; i < count; i++)
			{
				this.UpdateElementBounds(selectedElements[i], matrix);
			}
			if (this.SelectedStrokes.Count > 0)
			{
				this.TransformStrokes(this.SelectedStrokes, matrix);
				this._areStrokesChanged = true;
			}
			if (this.SelectedElements.Count == 0)
			{
				this.UpdateSelectionAdorner();
			}
			this._inkCanvas.BringIntoView(newRect);
		}

		// Token: 0x06006D8C RID: 28044 RVA: 0x001F7683 File Offset: 0x001F5883
		private void OnCanvasLayoutUpdated(object sender, EventArgs e)
		{
			this.UpdateSelectionAdorner();
		}

		// Token: 0x06006D8D RID: 28045 RVA: 0x001F768B File Offset: 0x001F588B
		private void OnStrokeInvalidated(object sender, EventArgs e)
		{
			this.OnStrokeCollectionChanged(sender, new StrokeCollectionChangedEventArgs(new StrokeCollection(), new StrokeCollection()));
		}

		// Token: 0x06006D8E RID: 28046 RVA: 0x001F76A4 File Offset: 0x001F58A4
		private void OnStrokeCollectionChanged(object target, StrokeCollectionChangedEventArgs e)
		{
			if (e.Added.Count != 0 && e.Removed.Count == 0)
			{
				return;
			}
			foreach (Stroke stroke in e.Removed)
			{
				if (this.SelectedStrokes.Contains(stroke))
				{
					stroke.Invalidated -= this.OnStrokeInvalidated;
					stroke.IsSelected = false;
					this.SelectedStrokes.Remove(stroke);
				}
			}
			this._areStrokesChanged = true;
			this.UpdateSelectionAdorner();
		}

		// Token: 0x06006D8F RID: 28047 RVA: 0x001F7748 File Offset: 0x001F5948
		private Rect GetStrokesBounds()
		{
			if (this._areStrokesChanged)
			{
				this._cachedStrokesBounds = ((this.SelectedStrokes.Count != 0) ? this.SelectedStrokes.GetBounds() : Rect.Empty);
				this._areStrokesChanged = false;
			}
			return this._cachedStrokesBounds;
		}

		// Token: 0x06006D90 RID: 28048 RVA: 0x001F7784 File Offset: 0x001F5984
		private List<Rect> GetElementsBounds()
		{
			List<Rect> list = new List<Rect>();
			if (this.SelectedElements.Count != 0)
			{
				foreach (Rect item in this.SelectedElementsBoundsEnumerator)
				{
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x001F77E8 File Offset: 0x001F59E8
		private Rect GetElementsUnionBounds()
		{
			if (this.SelectedElements.Count == 0)
			{
				return Rect.Empty;
			}
			Rect empty = Rect.Empty;
			foreach (Rect rect in this.SelectedElementsBoundsEnumerator)
			{
				empty.Union(rect);
			}
			return empty;
		}

		// Token: 0x06006D92 RID: 28050 RVA: 0x001F7850 File Offset: 0x001F5A50
		private void UpdateSelectionAdorner()
		{
			if (this._inkCanvas.ActiveEditingMode != InkCanvasEditingMode.None)
			{
				this._inkCanvas.SelectionAdorner.UpdateSelectionWireFrame(this.GetStrokesBounds(), this.GetElementsBounds());
			}
		}

		// Token: 0x06006D93 RID: 28051 RVA: 0x001F787C File Offset: 0x001F5A7C
		private void EnusreElementsBounds()
		{
			InkCanvasInnerCanvas innerCanvas = this._inkCanvas.InnerCanvas;
			if (!innerCanvas.IsMeasureValid || !innerCanvas.IsArrangeValid)
			{
				innerCanvas.UpdateLayout();
			}
		}

		// Token: 0x06006D94 RID: 28052 RVA: 0x001F78AC File Offset: 0x001F5AAC
		private static Matrix MapRectToRect(Rect target, Rect source)
		{
			if (source.IsEmpty)
			{
				throw new ArgumentOutOfRangeException("source", SR.Get("InvalidDiameter"));
			}
			double num = target.Width / source.Width;
			double offsetX = target.Left - num * source.Left;
			double num2 = target.Height / source.Height;
			double offsetY = target.Top - num2 * source.Top;
			return new Matrix(num, 0.0, 0.0, num2, offsetX, offsetY);
		}

		// Token: 0x06006D95 RID: 28053 RVA: 0x001F7938 File Offset: 0x001F5B38
		private void UpdateCanvasLayoutUpdatedHandler()
		{
			if (this.SelectedElements.Count != 0)
			{
				if (this._layoutUpdatedHandler == null)
				{
					this._layoutUpdatedHandler = new EventHandler(this.OnCanvasLayoutUpdated);
					this._inkCanvas.InnerCanvas.LayoutUpdated += this._layoutUpdatedHandler;
					return;
				}
			}
			else if (this._layoutUpdatedHandler != null)
			{
				this._inkCanvas.InnerCanvas.LayoutUpdated -= this._layoutUpdatedHandler;
				this._layoutUpdatedHandler = null;
			}
		}

		// Token: 0x06006D96 RID: 28054 RVA: 0x001F79A8 File Offset: 0x001F5BA8
		private static bool StrokesAreEqual(StrokeCollection strokes1, StrokeCollection strokes2)
		{
			if (strokes1 == null && strokes2 == null)
			{
				return true;
			}
			if (strokes1 == null || strokes2 == null)
			{
				return false;
			}
			if (strokes1.Count != strokes2.Count)
			{
				return false;
			}
			foreach (Stroke item in strokes1)
			{
				if (!strokes2.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006D97 RID: 28055 RVA: 0x001F7A1C File Offset: 0x001F5C1C
		private static bool FrameworkElementArraysAreEqual(IList<UIElement> elements1, IList<UIElement> elements2)
		{
			if (elements1 == null && elements2 == null)
			{
				return true;
			}
			if (elements1 == null || elements2 == null)
			{
				return false;
			}
			if (elements1.Count != elements2.Count)
			{
				return false;
			}
			foreach (UIElement item in elements1)
			{
				if (!elements2.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17001A1F RID: 6687
		// (get) Token: 0x06006D98 RID: 28056 RVA: 0x001F7A90 File Offset: 0x001F5C90
		private IEnumerable<Rect> SelectedElementsBoundsEnumerator
		{
			get
			{
				this.EnusreElementsBounds();
				InkCanvasInnerCanvas innerCanvas = this._inkCanvas.InnerCanvas;
				foreach (UIElement uielement in this.SelectedElements)
				{
					GeneralTransform generalTransform = uielement.TransformToAncestor(innerCanvas);
					Size renderSize = uielement.RenderSize;
					Rect rect = new Rect(0.0, 0.0, renderSize.Width, renderSize.Height);
					rect = generalTransform.TransformBounds(rect);
					yield return rect;
				}
				IEnumerator<UIElement> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x040035ED RID: 13805
		private InkCanvas _inkCanvas;

		// Token: 0x040035EE RID: 13806
		private StrokeCollection _selectedStrokes;

		// Token: 0x040035EF RID: 13807
		private Rect _cachedStrokesBounds;

		// Token: 0x040035F0 RID: 13808
		private bool _areStrokesChanged;

		// Token: 0x040035F1 RID: 13809
		private List<UIElement> _selectedElements;

		// Token: 0x040035F2 RID: 13810
		private EventHandler _layoutUpdatedHandler;

		// Token: 0x040035F3 RID: 13811
		private InkCanvasSelectionHitResult? _activeSelectionHitResult;
	}
}
