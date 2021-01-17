using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using MS.Internal.Controls;

namespace MS.Internal.Ink
{
	// Token: 0x0200068E RID: 1678
	internal sealed class LassoSelectionBehavior : StylusEditingBehavior
	{
		// Token: 0x06006DB4 RID: 28084 RVA: 0x001F66C8 File Offset: 0x001F48C8
		internal LassoSelectionBehavior(EditingCoordinator editingCoordinator, InkCanvas inkCanvas) : base(editingCoordinator, inkCanvas)
		{
		}

		// Token: 0x06006DB5 RID: 28085 RVA: 0x001F84DC File Offset: 0x001F66DC
		protected override void OnSwitchToMode(InkCanvasEditingMode mode)
		{
			switch (mode)
			{
			case InkCanvasEditingMode.None:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				break;
			case InkCanvasEditingMode.Ink:
			case InkCanvasEditingMode.GestureOnly:
			case InkCanvasEditingMode.InkAndGesture:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			case InkCanvasEditingMode.Select:
				break;
			case InkCanvasEditingMode.EraseByPoint:
			case InkCanvasEditingMode.EraseByStroke:
				base.Commit(false);
				base.EditingCoordinator.ChangeStylusEditingMode(this, mode);
				return;
			default:
				return;
			}
		}

		// Token: 0x06006DB6 RID: 28086 RVA: 0x001F8550 File Offset: 0x001F6750
		protected override void StylusInputBegin(StylusPointCollection stylusPoints, bool userInitiated)
		{
			this._disableLasso = false;
			bool flag = false;
			List<Point> list = new List<Point>();
			for (int i = 0; i < stylusPoints.Count; i++)
			{
				Point point = (Point)stylusPoints[i];
				if (i == 0)
				{
					this._startPoint = point;
					list.Add(point);
				}
				else if (!flag)
				{
					double lengthSquared = (point - this._startPoint).LengthSquared;
					if (DoubleUtil.GreaterThan(lengthSquared, 49.0))
					{
						list.Add(point);
						flag = true;
					}
				}
				else
				{
					list.Add(point);
				}
			}
			if (flag)
			{
				this.StartLasso(list);
			}
		}

		// Token: 0x06006DB7 RID: 28087 RVA: 0x001F85E8 File Offset: 0x001F67E8
		protected override void StylusInputContinue(StylusPointCollection stylusPoints, bool userInitiated)
		{
			if (this._lassoHelper != null)
			{
				List<Point> list = new List<Point>();
				for (int i = 0; i < stylusPoints.Count; i++)
				{
					list.Add((Point)stylusPoints[i]);
				}
				Point[] array = this._lassoHelper.AddPoints(list);
				if (array.Length != 0)
				{
					this._incrementalLassoHitTester.AddPoints(array);
					return;
				}
			}
			else if (!this._disableLasso)
			{
				bool flag = false;
				List<Point> list2 = new List<Point>();
				for (int j = 0; j < stylusPoints.Count; j++)
				{
					Point point = (Point)stylusPoints[j];
					if (!flag)
					{
						double lengthSquared = (point - this._startPoint).LengthSquared;
						if (DoubleUtil.GreaterThan(lengthSquared, 49.0))
						{
							list2.Add(point);
							flag = true;
						}
					}
					else
					{
						list2.Add(point);
					}
				}
				if (flag)
				{
					this.StartLasso(list2);
				}
			}
		}

		// Token: 0x06006DB8 RID: 28088 RVA: 0x001F86CC File Offset: 0x001F68CC
		protected override void StylusInputEnd(bool commit)
		{
			StrokeCollection strokeCollection = new StrokeCollection();
			List<UIElement> list = new List<UIElement>();
			if (this._lassoHelper != null)
			{
				strokeCollection = base.InkCanvas.EndDynamicSelection(this._lassoHelper.Visual);
				list = this.HitTestForElements();
				this._incrementalLassoHitTester.SelectionChanged -= this.OnSelectionChanged;
				this._incrementalLassoHitTester.EndHitTesting();
				this._incrementalLassoHitTester = null;
				this._lassoHelper = null;
			}
			else
			{
				Stroke stroke;
				UIElement uielement;
				this.TapSelectObject(this._startPoint, out stroke, out uielement);
				if (stroke != null)
				{
					strokeCollection = new StrokeCollection();
					strokeCollection.Add(stroke);
				}
				else if (uielement != null)
				{
					list.Add(uielement);
				}
			}
			base.SelfDeactivate();
			if (commit)
			{
				base.InkCanvas.ChangeInkCanvasSelection(strokeCollection, list.ToArray());
			}
		}

		// Token: 0x06006DB9 RID: 28089 RVA: 0x001F8784 File Offset: 0x001F6984
		protected override void OnCommitWithoutStylusInput(bool commit)
		{
			base.SelfDeactivate();
		}

		// Token: 0x06006DBA RID: 28090 RVA: 0x001F878C File Offset: 0x001F698C
		protected override Cursor GetCurrentCursor()
		{
			return Cursors.Cross;
		}

		// Token: 0x06006DBB RID: 28091 RVA: 0x001F8793 File Offset: 0x001F6993
		private void OnSelectionChanged(object sender, LassoSelectionChangedEventArgs e)
		{
			base.InkCanvas.UpdateDynamicSelection(e.SelectedStrokes, e.DeselectedStrokes);
		}

		// Token: 0x06006DBC RID: 28092 RVA: 0x001F87AC File Offset: 0x001F69AC
		private List<UIElement> HitTestForElements()
		{
			List<UIElement> list = new List<UIElement>();
			if (base.InkCanvas.Children.Count == 0)
			{
				return list;
			}
			for (int i = 0; i < base.InkCanvas.Children.Count; i++)
			{
				UIElement uiElement = base.InkCanvas.Children[i];
				this.HitTestElement(base.InkCanvas.InnerCanvas, uiElement, list);
			}
			return list;
		}

		// Token: 0x06006DBD RID: 28093 RVA: 0x001F8814 File Offset: 0x001F6A14
		private void HitTestElement(InkCanvasInnerCanvas parent, UIElement uiElement, List<UIElement> elementsToSelect)
		{
			LassoSelectionBehavior.ElementCornerPoints transformedElementCornerPoints = LassoSelectionBehavior.GetTransformedElementCornerPoints(parent, uiElement);
			if (transformedElementCornerPoints.Set)
			{
				Point[] points = this.GeneratePointGrid(transformedElementCornerPoints);
				if (this._lassoHelper.ArePointsInLasso(points, 60))
				{
					elementsToSelect.Add(uiElement);
				}
			}
		}

		// Token: 0x06006DBE RID: 28094 RVA: 0x001F8850 File Offset: 0x001F6A50
		private static LassoSelectionBehavior.ElementCornerPoints GetTransformedElementCornerPoints(InkCanvasInnerCanvas canvas, UIElement childElement)
		{
			LassoSelectionBehavior.ElementCornerPoints result = default(LassoSelectionBehavior.ElementCornerPoints);
			result.Set = false;
			if (childElement.Visibility != Visibility.Visible)
			{
				return result;
			}
			GeneralTransform generalTransform = childElement.TransformToAncestor(canvas);
			generalTransform.TryTransform(new Point(0.0, 0.0), out result.UpperLeft);
			generalTransform.TryTransform(new Point(childElement.RenderSize.Width, 0.0), out result.UpperRight);
			generalTransform.TryTransform(new Point(0.0, childElement.RenderSize.Height), out result.LowerLeft);
			generalTransform.TryTransform(new Point(childElement.RenderSize.Width, childElement.RenderSize.Height), out result.LowerRight);
			result.Set = true;
			return result;
		}

		// Token: 0x06006DBF RID: 28095 RVA: 0x001F8930 File Offset: 0x001F6B30
		private Point[] GeneratePointGrid(LassoSelectionBehavior.ElementCornerPoints elementPoints)
		{
			if (!elementPoints.Set)
			{
				return new Point[0];
			}
			ArrayList arrayList = new ArrayList();
			this.UpdatePointDistances(elementPoints);
			arrayList.Add(elementPoints.UpperLeft);
			arrayList.Add(elementPoints.UpperRight);
			this.FillInPoints(arrayList, elementPoints.UpperLeft, elementPoints.UpperRight);
			arrayList.Add(elementPoints.LowerLeft);
			arrayList.Add(elementPoints.LowerRight);
			this.FillInPoints(arrayList, elementPoints.LowerLeft, elementPoints.LowerRight);
			this.FillInGrid(arrayList, elementPoints.UpperLeft, elementPoints.UpperRight, elementPoints.LowerRight, elementPoints.LowerLeft);
			Point[] array = new Point[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x06006DC0 RID: 28096 RVA: 0x001F89FC File Offset: 0x001F6BFC
		private void FillInPoints(ArrayList pointArray, Point point1, Point point2)
		{
			if (!this.PointsAreCloseEnough(point1, point2))
			{
				Point point3 = LassoSelectionBehavior.GeneratePointBetweenPoints(point1, point2);
				pointArray.Add(point3);
				if (!this.PointsAreCloseEnough(point1, point3))
				{
					this.FillInPoints(pointArray, point1, point3);
				}
				if (!this.PointsAreCloseEnough(point3, point2))
				{
					this.FillInPoints(pointArray, point3, point2);
				}
			}
		}

		// Token: 0x06006DC1 RID: 28097 RVA: 0x001F8A50 File Offset: 0x001F6C50
		private void FillInGrid(ArrayList pointArray, Point upperLeft, Point upperRight, Point lowerRight, Point lowerLeft)
		{
			if (!this.PointsAreCloseEnough(upperLeft, lowerLeft))
			{
				Point point = LassoSelectionBehavior.GeneratePointBetweenPoints(upperLeft, lowerLeft);
				Point point2 = LassoSelectionBehavior.GeneratePointBetweenPoints(upperRight, lowerRight);
				pointArray.Add(point);
				pointArray.Add(point2);
				this.FillInPoints(pointArray, point, point2);
				if (!this.PointsAreCloseEnough(upperLeft, point))
				{
					this.FillInGrid(pointArray, upperLeft, upperRight, point2, point);
				}
				if (!this.PointsAreCloseEnough(point, lowerLeft))
				{
					this.FillInGrid(pointArray, point, point2, lowerRight, lowerLeft);
				}
			}
		}

		// Token: 0x06006DC2 RID: 28098 RVA: 0x001F8ACC File Offset: 0x001F6CCC
		private static Point GeneratePointBetweenPoints(Point point1, Point point2)
		{
			double num = (point1.X > point2.X) ? point1.X : point2.X;
			double num2 = (point1.X < point2.X) ? point1.X : point2.X;
			double num3 = (point1.Y > point2.Y) ? point1.Y : point2.Y;
			double num4 = (point1.Y < point2.Y) ? point1.Y : point2.Y;
			return new Point(num2 + (num - num2) * 0.5, num4 + (num3 - num4) * 0.5);
		}

		// Token: 0x06006DC3 RID: 28099 RVA: 0x001F8B80 File Offset: 0x001F6D80
		private bool PointsAreCloseEnough(Point point1, Point point2)
		{
			double num = point1.X - point2.X;
			double num2 = point1.Y - point2.Y;
			return num < this._xDiff && num > -this._xDiff && num2 < this._yDiff && num2 > -this._yDiff;
		}

		// Token: 0x06006DC4 RID: 28100 RVA: 0x001F8BD8 File Offset: 0x001F6DD8
		private void UpdatePointDistances(LassoSelectionBehavior.ElementCornerPoints elementPoints)
		{
			double num = elementPoints.UpperLeft.X - elementPoints.UpperRight.X;
			if (num < 0.0)
			{
				num = -num;
			}
			double num2 = elementPoints.UpperLeft.Y - elementPoints.LowerLeft.Y;
			if (num2 < 0.0)
			{
				num2 = -num2;
			}
			this._xDiff = num * 0.25;
			if (this._xDiff > 50.0)
			{
				this._xDiff = 50.0;
			}
			else if (this._xDiff < 15.0)
			{
				this._xDiff = 15.0;
			}
			this._yDiff = num2 * 0.25;
			if (this._yDiff > 50.0)
			{
				this._yDiff = 50.0;
				return;
			}
			if (this._yDiff < 15.0)
			{
				this._yDiff = 15.0;
			}
		}

		// Token: 0x06006DC5 RID: 28101 RVA: 0x001F8CDC File Offset: 0x001F6EDC
		private void StartLasso(List<Point> points)
		{
			if (base.InkCanvas.ClearSelectionRaiseSelectionChanging() && base.EditingCoordinator.ActiveEditingMode == InkCanvasEditingMode.Select)
			{
				this._incrementalLassoHitTester = base.InkCanvas.Strokes.GetIncrementalLassoHitTester(80);
				this._incrementalLassoHitTester.SelectionChanged += this.OnSelectionChanged;
				this._lassoHelper = new LassoHelper();
				base.InkCanvas.BeginDynamicSelection(this._lassoHelper.Visual);
				Point[] array = this._lassoHelper.AddPoints(points);
				if (array.Length != 0)
				{
					this._incrementalLassoHitTester.AddPoints(array);
					return;
				}
			}
			else
			{
				this._disableLasso = true;
			}
		}

		// Token: 0x06006DC6 RID: 28102 RVA: 0x001F8D7C File Offset: 0x001F6F7C
		private void TapSelectObject(Point point, out Stroke tappedStroke, out UIElement tappedElement)
		{
			tappedStroke = null;
			tappedElement = null;
			StrokeCollection strokeCollection = base.InkCanvas.Strokes.HitTest(point, 5.0);
			if (strokeCollection.Count > 0)
			{
				tappedStroke = strokeCollection[strokeCollection.Count - 1];
				return;
			}
			GeneralTransform generalTransform = base.InkCanvas.TransformToVisual(base.InkCanvas.InnerCanvas);
			Point point2 = generalTransform.Transform(point);
			tappedElement = base.InkCanvas.InnerCanvas.HitTestOnElements(point2);
		}

		// Token: 0x0400360B RID: 13835
		private Point _startPoint;

		// Token: 0x0400360C RID: 13836
		private bool _disableLasso;

		// Token: 0x0400360D RID: 13837
		private LassoHelper _lassoHelper;

		// Token: 0x0400360E RID: 13838
		private IncrementalLassoHitTester _incrementalLassoHitTester;

		// Token: 0x0400360F RID: 13839
		private double _xDiff;

		// Token: 0x04003610 RID: 13840
		private double _yDiff;

		// Token: 0x04003611 RID: 13841
		private const double _maxThreshold = 50.0;

		// Token: 0x04003612 RID: 13842
		private const double _minThreshold = 15.0;

		// Token: 0x04003613 RID: 13843
		private const int _percentIntersectForInk = 80;

		// Token: 0x04003614 RID: 13844
		private const int _percentIntersectForElements = 60;

		// Token: 0x02000B24 RID: 2852
		private struct ElementCornerPoints
		{
			// Token: 0x04004A67 RID: 19047
			internal Point UpperLeft;

			// Token: 0x04004A68 RID: 19048
			internal Point UpperRight;

			// Token: 0x04004A69 RID: 19049
			internal Point LowerRight;

			// Token: 0x04004A6A RID: 19050
			internal Point LowerLeft;

			// Token: 0x04004A6B RID: 19051
			internal bool Set;
		}
	}
}
