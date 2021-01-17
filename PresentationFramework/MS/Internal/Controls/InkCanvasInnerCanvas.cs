using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MS.Internal.Controls
{
	// Token: 0x02000758 RID: 1880
	internal class InkCanvasInnerCanvas : Panel
	{
		// Token: 0x060077AB RID: 30635 RVA: 0x00222584 File Offset: 0x00220784
		internal InkCanvasInnerCanvas(InkCanvas inkCanvas)
		{
			this._inkCanvas = inkCanvas;
		}

		// Token: 0x060077AC RID: 30636 RVA: 0x001334BC File Offset: 0x001316BC
		private InkCanvasInnerCanvas()
		{
		}

		// Token: 0x060077AD RID: 30637 RVA: 0x00222594 File Offset: 0x00220794
		protected internal override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
		{
			base.OnVisualChildrenChanged(visualAdded, visualRemoved);
			UIElement uielement = visualRemoved as UIElement;
			if (uielement != null)
			{
				this.InkCanvas.InkCanvasSelection.RemoveElement(uielement);
			}
			this.InkCanvas.RaiseOnVisualChildrenChanged(visualAdded, visualRemoved);
		}

		// Token: 0x060077AE RID: 30638 RVA: 0x002225D4 File Offset: 0x002207D4
		protected override Size MeasureOverride(Size constraint)
		{
			Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Size result = default(Size);
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					double num = InkCanvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(num))
					{
						result.Width = Math.Max(result.Width, num + uielement.DesiredSize.Width);
					}
					else
					{
						result.Width = Math.Max(result.Width, uielement.DesiredSize.Width);
					}
					double num2 = InkCanvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(num2))
					{
						result.Height = Math.Max(result.Height, num2 + uielement.DesiredSize.Height);
					}
					else
					{
						result.Height = Math.Max(result.Height, uielement.DesiredSize.Height);
					}
				}
			}
			return result;
		}

		// Token: 0x060077AF RID: 30639 RVA: 0x00222714 File Offset: 0x00220914
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			foreach (object obj in base.InternalChildren)
			{
				UIElement uielement = (UIElement)obj;
				if (uielement != null)
				{
					double x = 0.0;
					double y = 0.0;
					double num = InkCanvas.GetLeft(uielement);
					if (!DoubleUtil.IsNaN(num))
					{
						x = num;
					}
					else
					{
						double num2 = InkCanvas.GetRight(uielement);
						if (!DoubleUtil.IsNaN(num2))
						{
							x = arrangeSize.Width - uielement.DesiredSize.Width - num2;
						}
					}
					double num3 = InkCanvas.GetTop(uielement);
					if (!DoubleUtil.IsNaN(num3))
					{
						y = num3;
					}
					else
					{
						double num4 = InkCanvas.GetBottom(uielement);
						if (!DoubleUtil.IsNaN(num4))
						{
							y = arrangeSize.Height - uielement.DesiredSize.Height - num4;
						}
					}
					uielement.Arrange(new Rect(new Point(x, y), uielement.DesiredSize));
				}
			}
			return arrangeSize;
		}

		// Token: 0x060077B0 RID: 30640 RVA: 0x0022282C File Offset: 0x00220A2C
		protected override void OnChildDesiredSizeChanged(UIElement child)
		{
			base.OnChildDesiredSizeChanged(child);
			base.InvalidateMeasure();
		}

		// Token: 0x060077B1 RID: 30641 RVA: 0x0022283B File Offset: 0x00220A3B
		protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
		{
			return base.CreateUIElementCollection(this._inkCanvas);
		}

		// Token: 0x17001C61 RID: 7265
		// (get) Token: 0x060077B2 RID: 30642 RVA: 0x00222849 File Offset: 0x00220A49
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return EmptyEnumerator.Instance;
			}
		}

		// Token: 0x060077B3 RID: 30643 RVA: 0x00163A98 File Offset: 0x00161C98
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			if (base.ClipToBounds)
			{
				return base.GetLayoutClip(layoutSlotSize);
			}
			return null;
		}

		// Token: 0x060077B4 RID: 30644 RVA: 0x00222850 File Offset: 0x00220A50
		internal UIElement HitTestOnElements(Point point)
		{
			UIElement result = null;
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);
			if (hitTestResult != null)
			{
				Visual visual = hitTestResult.VisualHit as Visual;
				Visual3D visual3D = hitTestResult.VisualHit as Visual3D;
				DependencyObject dependencyObject = null;
				if (visual != null)
				{
					dependencyObject = visual;
				}
				else if (visual3D != null)
				{
					dependencyObject = visual3D;
				}
				while (dependencyObject != null)
				{
					DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject);
					if (parent == this.InkCanvas.InnerCanvas)
					{
						result = (dependencyObject as UIElement);
						break;
					}
					dependencyObject = parent;
				}
			}
			return result;
		}

		// Token: 0x17001C62 RID: 7266
		// (get) Token: 0x060077B5 RID: 30645 RVA: 0x002228C0 File Offset: 0x00220AC0
		internal IEnumerator PrivateLogicalChildren
		{
			get
			{
				return base.LogicalChildren;
			}
		}

		// Token: 0x17001C63 RID: 7267
		// (get) Token: 0x060077B6 RID: 30646 RVA: 0x002228C8 File Offset: 0x00220AC8
		internal InkCanvas InkCanvas
		{
			get
			{
				return this._inkCanvas;
			}
		}

		// Token: 0x040038D1 RID: 14545
		private InkCanvas _inkCanvas;
	}
}
