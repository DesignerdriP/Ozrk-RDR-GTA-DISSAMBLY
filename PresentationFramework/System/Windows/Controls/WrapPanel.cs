using System;
using System.ComponentModel;
using MS.Internal;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Positions child elements in sequential position from left to right, breaking content to the next line at the edge of the containing box. Subsequent ordering happens sequentially from top to bottom or from right to left, depending on the value of the <see cref="P:System.Windows.Controls.WrapPanel.Orientation" /> property.</summary>
	// Token: 0x02000560 RID: 1376
	public class WrapPanel : Panel
	{
		// Token: 0x06005B48 RID: 23368 RVA: 0x0019BACC File Offset: 0x00199CCC
		static WrapPanel()
		{
			ControlsTraceLogger.AddControl(TelemetryControls.WrapPanel);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.WrapPanel" /> class.</summary>
		// Token: 0x06005B49 RID: 23369 RVA: 0x0019BB9E File Offset: 0x00199D9E
		public WrapPanel()
		{
			this._orientation = (Orientation)WrapPanel.OrientationProperty.GetDefaultValue(base.DependencyObjectType);
		}

		// Token: 0x06005B4A RID: 23370 RVA: 0x0019BBC4 File Offset: 0x00199DC4
		private static bool IsWidthHeightValid(object value)
		{
			double num = (double)value;
			return DoubleUtil.IsNaN(num) || (num >= 0.0 && !double.IsPositiveInfinity(num));
		}

		/// <summary>Gets or sets a value that specifies the width of all items that are contained within a <see cref="T:System.Windows.Controls.WrapPanel" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the uniform width of all items that are contained within the <see cref="T:System.Windows.Controls.WrapPanel" />. The default value is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x06005B4B RID: 23371 RVA: 0x0019BBF9 File Offset: 0x00199DF9
		// (set) Token: 0x06005B4C RID: 23372 RVA: 0x0019BC0B File Offset: 0x00199E0B
		[TypeConverter(typeof(LengthConverter))]
		public double ItemWidth
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemWidthProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemWidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the height of all items that are contained within a <see cref="T:System.Windows.Controls.WrapPanel" />. </summary>
		/// <returns>The <see cref="T:System.Double" /> that represents the uniform height of all items that are contained within the <see cref="T:System.Windows.Controls.WrapPanel" />. The default value is <see cref="F:System.Double.NaN" />.</returns>
		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06005B4D RID: 23373 RVA: 0x0019BC1E File Offset: 0x00199E1E
		// (set) Token: 0x06005B4E RID: 23374 RVA: 0x0019BC30 File Offset: 0x00199E30
		[TypeConverter(typeof(LengthConverter))]
		public double ItemHeight
		{
			get
			{
				return (double)base.GetValue(WrapPanel.ItemHeightProperty);
			}
			set
			{
				base.SetValue(WrapPanel.ItemHeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the dimension in which child content is arranged. </summary>
		/// <returns>An <see cref="T:System.Windows.Controls.Orientation" /> value that represents the physical orientation of content within the <see cref="T:System.Windows.Controls.WrapPanel" /> as horizontal or vertical. The default value is <see cref="F:System.Windows.Controls.Orientation.Horizontal" />.</returns>
		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x06005B4F RID: 23375 RVA: 0x0019BC43 File Offset: 0x00199E43
		// (set) Token: 0x06005B50 RID: 23376 RVA: 0x0019BC4B File Offset: 0x00199E4B
		public Orientation Orientation
		{
			get
			{
				return this._orientation;
			}
			set
			{
				base.SetValue(WrapPanel.OrientationProperty, value);
			}
		}

		// Token: 0x06005B51 RID: 23377 RVA: 0x0019BC60 File Offset: 0x00199E60
		private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			WrapPanel wrapPanel = (WrapPanel)d;
			wrapPanel._orientation = (Orientation)e.NewValue;
		}

		/// <summary>Measures the child elements of a <see cref="T:System.Windows.Controls.WrapPanel" /> in anticipation of arranging them during the <see cref="M:System.Windows.Controls.WrapPanel.ArrangeOverride(System.Windows.Size)" /> pass.</summary>
		/// <param name="constraint">An upper limit <see cref="T:System.Windows.Size" /> that should not be exceeded.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the desired size of the element.</returns>
		// Token: 0x06005B52 RID: 23378 RVA: 0x0019BC88 File Offset: 0x00199E88
		protected override Size MeasureOverride(Size constraint)
		{
			WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize2 = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize3 = new WrapPanel.UVSize(this.Orientation, constraint.Width, constraint.Height);
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			bool flag = !DoubleUtil.IsNaN(itemWidth);
			bool flag2 = !DoubleUtil.IsNaN(itemHeight);
			Size availableSize = new Size(flag ? itemWidth : constraint.Width, flag2 ? itemHeight : constraint.Height);
			UIElementCollection internalChildren = base.InternalChildren;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					uielement.Measure(availableSize);
					WrapPanel.UVSize uvsize4 = new WrapPanel.UVSize(this.Orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
					if (DoubleUtil.GreaterThan(uvsize.U + uvsize4.U, uvsize3.U))
					{
						uvsize2.U = Math.Max(uvsize.U, uvsize2.U);
						uvsize2.V += uvsize.V;
						uvsize = uvsize4;
						if (DoubleUtil.GreaterThan(uvsize4.U, uvsize3.U))
						{
							uvsize2.U = Math.Max(uvsize4.U, uvsize2.U);
							uvsize2.V += uvsize4.V;
							uvsize = new WrapPanel.UVSize(this.Orientation);
						}
					}
					else
					{
						uvsize.U += uvsize4.U;
						uvsize.V = Math.Max(uvsize4.V, uvsize.V);
					}
				}
				i++;
			}
			uvsize2.U = Math.Max(uvsize.U, uvsize2.U);
			uvsize2.V += uvsize.V;
			return new Size(uvsize2.Width, uvsize2.Height);
		}

		/// <summary>Arranges the content of a <see cref="T:System.Windows.Controls.WrapPanel" /> element.</summary>
		/// <param name="finalSize">The <see cref="T:System.Windows.Size" /> that this element should use to arrange its child elements.</param>
		/// <returns>The <see cref="T:System.Windows.Size" /> that represents the arranged size of this <see cref="T:System.Windows.Controls.WrapPanel" /> element and its children.</returns>
		// Token: 0x06005B53 RID: 23379 RVA: 0x0019BE90 File Offset: 0x0019A090
		protected override Size ArrangeOverride(Size finalSize)
		{
			int num = 0;
			double itemWidth = this.ItemWidth;
			double itemHeight = this.ItemHeight;
			double num2 = 0.0;
			double itemU = (this.Orientation == Orientation.Horizontal) ? itemWidth : itemHeight;
			WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation);
			WrapPanel.UVSize uvsize2 = new WrapPanel.UVSize(this.Orientation, finalSize.Width, finalSize.Height);
			bool flag = !DoubleUtil.IsNaN(itemWidth);
			bool flag2 = !DoubleUtil.IsNaN(itemHeight);
			bool useItemU = (this.Orientation == Orientation.Horizontal) ? flag : flag2;
			UIElementCollection internalChildren = base.InternalChildren;
			int i = 0;
			int count = internalChildren.Count;
			while (i < count)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					WrapPanel.UVSize uvsize3 = new WrapPanel.UVSize(this.Orientation, flag ? itemWidth : uielement.DesiredSize.Width, flag2 ? itemHeight : uielement.DesiredSize.Height);
					if (DoubleUtil.GreaterThan(uvsize.U + uvsize3.U, uvsize2.U))
					{
						this.arrangeLine(num2, uvsize.V, num, i, useItemU, itemU);
						num2 += uvsize.V;
						uvsize = uvsize3;
						if (DoubleUtil.GreaterThan(uvsize3.U, uvsize2.U))
						{
							this.arrangeLine(num2, uvsize3.V, i, ++i, useItemU, itemU);
							num2 += uvsize3.V;
							uvsize = new WrapPanel.UVSize(this.Orientation);
						}
						num = i;
					}
					else
					{
						uvsize.U += uvsize3.U;
						uvsize.V = Math.Max(uvsize3.V, uvsize.V);
					}
				}
				i++;
			}
			if (num < internalChildren.Count)
			{
				this.arrangeLine(num2, uvsize.V, num, internalChildren.Count, useItemU, itemU);
			}
			return finalSize;
		}

		// Token: 0x06005B54 RID: 23380 RVA: 0x0019C068 File Offset: 0x0019A268
		private void arrangeLine(double v, double lineV, int start, int end, bool useItemU, double itemU)
		{
			double num = 0.0;
			bool flag = this.Orientation == Orientation.Horizontal;
			UIElementCollection internalChildren = base.InternalChildren;
			for (int i = start; i < end; i++)
			{
				UIElement uielement = internalChildren[i];
				if (uielement != null)
				{
					WrapPanel.UVSize uvsize = new WrapPanel.UVSize(this.Orientation, uielement.DesiredSize.Width, uielement.DesiredSize.Height);
					double num2 = useItemU ? itemU : uvsize.U;
					uielement.Arrange(new Rect(flag ? num : v, flag ? v : num, flag ? num2 : lineV, flag ? lineV : num2));
					num += num2;
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.ItemWidth" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.ItemWidth" />  dependency property.</returns>
		// Token: 0x04002F6F RID: 12143
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(WrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(WrapPanel.IsWidthHeightValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.ItemHeight" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.ItemHeight" />  dependency property.</returns>
		// Token: 0x04002F70 RID: 12144
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(WrapPanel), new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(WrapPanel.IsWidthHeightValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.WrapPanel.Orientation" />  dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.WrapPanel.Orientation" />  dependency property.</returns>
		// Token: 0x04002F71 RID: 12145
		public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof(WrapPanel), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(WrapPanel.OnOrientationChanged)));

		// Token: 0x04002F72 RID: 12146
		private Orientation _orientation;

		// Token: 0x020009DD RID: 2525
		private struct UVSize
		{
			// Token: 0x06008938 RID: 35128 RVA: 0x002545E0 File Offset: 0x002527E0
			internal UVSize(Orientation orientation, double width, double height)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
				this.Width = width;
				this.Height = height;
			}

			// Token: 0x06008939 RID: 35129 RVA: 0x0025461C File Offset: 0x0025281C
			internal UVSize(Orientation orientation)
			{
				this.U = (this.V = 0.0);
				this._orientation = orientation;
			}

			// Token: 0x17001F06 RID: 7942
			// (get) Token: 0x0600893A RID: 35130 RVA: 0x00254648 File Offset: 0x00252848
			// (set) Token: 0x0600893B RID: 35131 RVA: 0x0025465F File Offset: 0x0025285F
			internal double Width
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.V;
					}
					return this.U;
				}
				set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.U = value;
						return;
					}
					this.V = value;
				}
			}

			// Token: 0x17001F07 RID: 7943
			// (get) Token: 0x0600893C RID: 35132 RVA: 0x00254678 File Offset: 0x00252878
			// (set) Token: 0x0600893D RID: 35133 RVA: 0x0025468F File Offset: 0x0025288F
			internal double Height
			{
				get
				{
					if (this._orientation != Orientation.Horizontal)
					{
						return this.U;
					}
					return this.V;
				}
				set
				{
					if (this._orientation == Orientation.Horizontal)
					{
						this.V = value;
						return;
					}
					this.U = value;
				}
			}

			// Token: 0x0400463A RID: 17978
			internal double U;

			// Token: 0x0400463B RID: 17979
			internal double V;

			// Token: 0x0400463C RID: 17980
			private Orientation _orientation;
		}
	}
}
