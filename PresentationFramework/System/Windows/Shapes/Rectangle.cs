using System;
using System.ComponentModel;
using System.Windows.Media;

namespace System.Windows.Shapes
{
	/// <summary>Draws a rectangle.</summary>
	// Token: 0x02000158 RID: 344
	public sealed class Rectangle : Shape
	{
		// Token: 0x06000F48 RID: 3912 RVA: 0x0003B89C File Offset: 0x00039A9C
		static Rectangle()
		{
			Shape.StretchProperty.OverrideMetadata(typeof(Rectangle), new FrameworkPropertyMetadata(Stretch.Fill));
		}

		/// <summary>Gets or sets the x-axis radius of the ellipse that is used to round the corners of the rectangle.  </summary>
		/// <returns>The x-axis radius of the ellipse that is used to round the corners of the rectangle.</returns>
		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0003B938 File Offset: 0x00039B38
		// (set) Token: 0x06000F4A RID: 3914 RVA: 0x0003B94A File Offset: 0x00039B4A
		[TypeConverter(typeof(LengthConverter))]
		public double RadiusX
		{
			get
			{
				return (double)base.GetValue(Rectangle.RadiusXProperty);
			}
			set
			{
				base.SetValue(Rectangle.RadiusXProperty, value);
			}
		}

		/// <summary>Gets or sets the y-axis radius of the ellipse that is used to round the corners of the rectangle.  </summary>
		/// <returns>The y-axis radius of the ellipse that is used to round the corners of the rectangle. The default is 0.</returns>
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0003B95D File Offset: 0x00039B5D
		// (set) Token: 0x06000F4C RID: 3916 RVA: 0x0003B96F File Offset: 0x00039B6F
		[TypeConverter(typeof(LengthConverter))]
		public double RadiusY
		{
			get
			{
				return (double)base.GetValue(Rectangle.RadiusYProperty);
			}
			set
			{
				base.SetValue(Rectangle.RadiusYProperty, value);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Geometry" /> object that represents the final rendered shape. </summary>
		/// <returns>The final rendered shape.</returns>
		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06000F4D RID: 3917 RVA: 0x0003B982 File Offset: 0x00039B82
		public override Geometry RenderedGeometry
		{
			get
			{
				return new RectangleGeometry(this._rect, this.RadiusX, this.RadiusY);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Media.Transform" /> that is applied to this <see cref="T:System.Windows.Shapes.Rectangle" />. </summary>
		/// <returns>The transform that is applied to this <see cref="T:System.Windows.Shapes.Rectangle" />.</returns>
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0003B076 File Offset: 0x00039276
		public override Transform GeometryTransform
		{
			get
			{
				return Transform.Identity;
			}
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0003B99C File Offset: 0x00039B9C
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

		// Token: 0x06000F50 RID: 3920 RVA: 0x0003BA0C File Offset: 0x00039C0C
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

		// Token: 0x1700049C RID: 1180
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0003B982 File Offset: 0x00039B82
		protected override Geometry DefiningGeometry
		{
			get
			{
				return new RectangleGeometry(this._rect, this.RadiusX, this.RadiusY);
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0003BB48 File Offset: 0x00039D48
		protected override void OnRender(DrawingContext drawingContext)
		{
			Pen pen = base.GetPen();
			drawingContext.DrawRoundedRectangle(base.Fill, pen, this._rect, this.RadiusX, this.RadiusY);
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0003BB7C File Offset: 0x00039D7C
		internal override void CacheDefiningGeometry()
		{
			double num = base.GetStrokeThickness() / 2.0;
			this._rect = new Rect(num, num, 0.0, 0.0);
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0003BBBC File Offset: 0x00039DBC
		internal override Size GetNaturalSize()
		{
			double strokeThickness = base.GetStrokeThickness();
			return new Size(strokeThickness, strokeThickness);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0003BBD7 File Offset: 0x00039DD7
		internal override Rect GetDefiningGeometryBounds()
		{
			return this._rect;
		}

		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x06000F56 RID: 3926 RVA: 0x0003BBDF File Offset: 0x00039DDF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 19;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Rectangle.RadiusX" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Rectangle.RadiusX" /> dependency property.</returns>
		// Token: 0x0400118A RID: 4490
		public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register("RadiusX", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Rectangle.RadiusY" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Rectangle.RadiusY" /> dependency property.</returns>
		// Token: 0x0400118B RID: 4491
		public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register("RadiusY", typeof(double), typeof(Rectangle), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

		// Token: 0x0400118C RID: 4492
		private Rect _rect = Rect.Empty;
	}
}
