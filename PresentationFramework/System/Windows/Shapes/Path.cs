using System;
using System.Windows.Media;
using MS.Internal.PresentationFramework;

namespace System.Windows.Shapes
{
	/// <summary>Draws a series of connected lines and curves. </summary>
	// Token: 0x02000155 RID: 341
	public sealed class Path : Shape
	{
		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Geometry" /> that specifies the shape to be drawn.  </summary>
		/// <returns>A description of the shape to be drawn. </returns>
		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06000F32 RID: 3890 RVA: 0x0003B4F1 File Offset: 0x000396F1
		// (set) Token: 0x06000F33 RID: 3891 RVA: 0x0003B503 File Offset: 0x00039703
		public Geometry Data
		{
			get
			{
				return (Geometry)base.GetValue(Path.DataProperty);
			}
			set
			{
				base.SetValue(Path.DataProperty, value);
			}
		}

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06000F34 RID: 3892 RVA: 0x0003B514 File Offset: 0x00039714
		protected override Geometry DefiningGeometry
		{
			get
			{
				Geometry geometry = this.Data;
				if (geometry == null)
				{
					geometry = Geometry.Empty;
				}
				return geometry;
			}
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06000F35 RID: 3893 RVA: 0x0003B2EB File Offset: 0x000394EB
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 13;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Shapes.Path.Data" /> dependency property.  </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Shapes.Path.Data" /> dependency property.</returns>
		// Token: 0x04001183 RID: 4483
		[CommonDependencyProperty]
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(Geometry), typeof(Path), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender), null);
	}
}
