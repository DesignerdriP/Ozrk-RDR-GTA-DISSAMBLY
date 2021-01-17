using System;
using System.Windows.Media;

namespace System.Windows.Documents.Internal
{
	// Token: 0x02000462 RID: 1122
	internal class ColumnResizeAdorner : Adorner
	{
		// Token: 0x060040B3 RID: 16563 RVA: 0x00127794 File Offset: 0x00125994
		internal ColumnResizeAdorner(UIElement scope) : base(scope)
		{
			this._pen = new Pen(new SolidColorBrush(Colors.LightSlateGray), 2.0);
			this._x = double.NaN;
			this._top = double.NaN;
			this._height = double.NaN;
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x001277F4 File Offset: 0x001259F4
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			TranslateTransform value = new TranslateTransform(this._x, this._top);
			generalTransformGroup.Children.Add(value);
			if (transform != null)
			{
				generalTransformGroup.Children.Add(transform);
			}
			return generalTransformGroup;
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x00127835 File Offset: 0x00125A35
		protected override void OnRender(DrawingContext drawingContext)
		{
			drawingContext.DrawLine(this._pen, new Point(0.0, 0.0), new Point(0.0, this._height));
		}

		// Token: 0x060040B6 RID: 16566 RVA: 0x00127870 File Offset: 0x00125A70
		internal void Update(double newX)
		{
			if (this._x != newX)
			{
				this._x = newX;
				AdornerLayer adornerLayer = VisualTreeHelper.GetParent(this) as AdornerLayer;
				if (adornerLayer != null)
				{
					adornerLayer.Update(base.AdornedElement);
					adornerLayer.InvalidateVisual();
				}
			}
		}

		// Token: 0x060040B7 RID: 16567 RVA: 0x001278AE File Offset: 0x00125AAE
		internal void Initialize(UIElement renderScope, double xPos, double yPos, double height)
		{
			this._adornerLayer = AdornerLayer.GetAdornerLayer(renderScope);
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Add(this);
			}
			this._x = xPos;
			this._top = yPos;
			this._height = height;
		}

		// Token: 0x060040B8 RID: 16568 RVA: 0x001278E6 File Offset: 0x00125AE6
		internal void Uninitialize()
		{
			if (this._adornerLayer != null)
			{
				this._adornerLayer.Remove(this);
				this._adornerLayer = null;
			}
		}

		// Token: 0x04002771 RID: 10097
		private double _x;

		// Token: 0x04002772 RID: 10098
		private double _top;

		// Token: 0x04002773 RID: 10099
		private double _height;

		// Token: 0x04002774 RID: 10100
		private Pen _pen;

		// Token: 0x04002775 RID: 10101
		private AdornerLayer _adornerLayer;
	}
}
