using System;
using System.Windows.Media;

namespace System.Windows.Shapes
{
	/// <summary>Draws an ellipse. </summary>
	// Token: 0x02000153 RID: 339
	public sealed class Ellipse : Shape
	{
		// Token: 0x06000F1A RID: 3866 RVA: 0x0003B04D File Offset: 0x0003924D
		static Ellipse()
		{
			Shape.StretchProperty.OverrideMetadata(typeof(Ellipse), new FrameworkPropertyMetadata(Stretch.Fill));
		}

		/// <summary>Gets the final rendered <see cref="T:System.Windows.Media.Geometry" /> of an <see cref="T:System.Windows.Shapes.Ellipse" />.</summary>
		/// <returns>The final rendered <see cref="T:System.Windows.Media.Geometry" /> of an <see cref="T:System.Windows.Shapes.Ellipse" />.</returns>
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0003B06E File Offset: 0x0003926E
		public override Geometry RenderedGeometry
		{
			get
			{
				return this.DefiningGeometry;
			}
		}

		/// <summary>Gets the value of any <see cref="P:System.Windows.Media.Transform.Identity" /> transforms that are applied to the <see cref="T:System.Windows.Media.Geometry" /> of an <see cref="T:System.Windows.Shapes.Ellipse" /> before it is rendered.</summary>
		/// <returns>The value of any <see cref="P:System.Windows.Media.Transform.Identity" /> transforms that are applied to the <see cref="T:System.Windows.Media.Geometry" /> of an <see cref="T:System.Windows.Shapes.Ellipse" /> before it is rendered.</returns>
		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x0003B076 File Offset: 0x00039276
		public override Transform GeometryTransform
		{
			get
			{
				return Transform.Identity;
			}
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0003B080 File Offset: 0x00039280
		protected override Size MeasureOverride(Size constraint)
		{
			if (base.Stretch != Stretch.UniformToFill)
			{
				return this.GetNaturalSize();
			}
			double num = constraint.Width;
			double height = constraint.Height;
			if (double.IsInfinity(num) && double.IsInfinity(height))
			{
				return this.GetNaturalSize();
			}
			if (double.IsInfinity(num) || double.IsInfinity(height))
			{
				num = Math.Min(num, height);
			}
			else
			{
				num = Math.Max(num, height);
			}
			return new Size(num, num);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0003B0F0 File Offset: 0x000392F0
		protected override Size ArrangeOverride(Size finalSize)
		{
			double strokeThickness = base.GetStrokeThickness();
			double num = strokeThickness / 2.0;
			this._rect = new Rect(num, num, Math.Max(0.0, finalSize.Width - strokeThickness), Math.Max(0.0, finalSize.Height - strokeThickness));
			switch (base.Stretch)
			{
			case Stretch.None:
				this._rect.Width = (this._rect.Height = 0.0);
				break;
			case Stretch.Uniform:
				if (this._rect.Width > this._rect.Height)
				{
					this._rect.Width = this._rect.Height;
				}
				else
				{
					this._rect.Height = this._rect.Width;
				}
				break;
			case Stretch.UniformToFill:
				if (this._rect.Width < this._rect.Height)
				{
					this._rect.Width = this._rect.Height;
				}
				else
				{
					this._rect.Height = this._rect.Width;
				}
				break;
			}
			base.ResetRenderedGeometry();
			return finalSize;
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0003B229 File Offset: 0x00039429
		protected override Geometry DefiningGeometry
		{
			get
			{
				if (this._rect.IsEmpty)
				{
					return Geometry.Empty;
				}
				return new EllipseGeometry(this._rect);
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0003B24C File Offset: 0x0003944C
		protected override void OnRender(DrawingContext drawingContext)
		{
			if (!this._rect.IsEmpty)
			{
				Pen pen = base.GetPen();
				drawingContext.DrawGeometry(base.Fill, pen, new EllipseGeometry(this._rect));
			}
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0003B288 File Offset: 0x00039488
		internal override void CacheDefiningGeometry()
		{
			double num = base.GetStrokeThickness() / 2.0;
			this._rect = new Rect(num, num, 0.0, 0.0);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x0003B2C8 File Offset: 0x000394C8
		internal override Size GetNaturalSize()
		{
			double strokeThickness = base.GetStrokeThickness();
			return new Size(strokeThickness, strokeThickness);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x0003B2E3 File Offset: 0x000394E3
		internal override Rect GetDefiningGeometryBounds()
		{
			return this._rect;
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000F24 RID: 3876 RVA: 0x0003B2EB File Offset: 0x000394EB
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 13;
			}
		}

		// Token: 0x0400117D RID: 4477
		private Rect _rect = Rect.Empty;
	}
}
