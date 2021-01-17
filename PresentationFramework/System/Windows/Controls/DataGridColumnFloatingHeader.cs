using System;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020004A3 RID: 1187
	[TemplatePart(Name = "PART_VisualBrushCanvas", Type = typeof(Canvas))]
	internal class DataGridColumnFloatingHeader : Control
	{
		// Token: 0x06004875 RID: 18549 RVA: 0x001497E8 File Offset: 0x001479E8
		static DataGridColumnFloatingHeader()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(DataGridColumnHeader.ColumnFloatingHeaderStyleKey));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnFloatingHeader.OnWidthChanged), new CoerceValueCallback(DataGridColumnFloatingHeader.OnCoerceWidth)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnFloatingHeader), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridColumnFloatingHeader.OnHeightChanged), new CoerceValueCallback(DataGridColumnFloatingHeader.OnCoerceHeight)));
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x00149878 File Offset: 0x00147A78
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			double num = (double)e.NewValue;
			if (dataGridColumnFloatingHeader._visualBrushCanvas != null && !DoubleUtil.IsNaN(num))
			{
				VisualBrush visualBrush = dataGridColumnFloatingHeader._visualBrushCanvas.Background as VisualBrush;
				if (visualBrush != null)
				{
					Rect viewbox = visualBrush.Viewbox;
					visualBrush.Viewbox = new Rect(viewbox.X, viewbox.Y, num - dataGridColumnFloatingHeader.GetVisualCanvasMarginX(), viewbox.Height);
				}
			}
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x001498EC File Offset: 0x00147AEC
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			if (dataGridColumnFloatingHeader._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnFloatingHeader._referenceHeader.ActualWidth + dataGridColumnFloatingHeader.GetVisualCanvasMarginX();
			}
			return baseValue;
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x00149930 File Offset: 0x00147B30
		private static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			double num = (double)e.NewValue;
			if (dataGridColumnFloatingHeader._visualBrushCanvas != null && !DoubleUtil.IsNaN(num))
			{
				VisualBrush visualBrush = dataGridColumnFloatingHeader._visualBrushCanvas.Background as VisualBrush;
				if (visualBrush != null)
				{
					Rect viewbox = visualBrush.Viewbox;
					visualBrush.Viewbox = new Rect(viewbox.X, viewbox.Y, viewbox.Width, num - dataGridColumnFloatingHeader.GetVisualCanvasMarginY());
				}
			}
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x001499A4 File Offset: 0x00147BA4
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnFloatingHeader dataGridColumnFloatingHeader = (DataGridColumnFloatingHeader)d;
			if (dataGridColumnFloatingHeader._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnFloatingHeader._referenceHeader.ActualHeight + dataGridColumnFloatingHeader.GetVisualCanvasMarginY();
			}
			return baseValue;
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x001499E8 File Offset: 0x00147BE8
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._visualBrushCanvas = (base.GetTemplateChild("PART_VisualBrushCanvas") as Canvas);
			this.UpdateVisualBrush();
		}

		// Token: 0x170011B3 RID: 4531
		// (get) Token: 0x0600487B RID: 18555 RVA: 0x00149A0C File Offset: 0x00147C0C
		// (set) Token: 0x0600487C RID: 18556 RVA: 0x00149A14 File Offset: 0x00147C14
		internal DataGridColumnHeader ReferenceHeader
		{
			get
			{
				return this._referenceHeader;
			}
			set
			{
				this._referenceHeader = value;
			}
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x00149A20 File Offset: 0x00147C20
		private void UpdateVisualBrush()
		{
			if (this._referenceHeader != null && this._visualBrushCanvas != null)
			{
				VisualBrush visualBrush = new VisualBrush(this._referenceHeader);
				visualBrush.ViewboxUnits = BrushMappingMode.Absolute;
				double num = base.Width;
				if (DoubleUtil.IsNaN(num))
				{
					num = this._referenceHeader.ActualWidth;
				}
				else
				{
					num -= this.GetVisualCanvasMarginX();
				}
				double num2 = base.Height;
				if (DoubleUtil.IsNaN(num2))
				{
					num2 = this._referenceHeader.ActualHeight;
				}
				else
				{
					num2 -= this.GetVisualCanvasMarginY();
				}
				Vector offset = VisualTreeHelper.GetOffset(this._referenceHeader);
				visualBrush.Viewbox = new Rect(offset.X, offset.Y, num, num2);
				this._visualBrushCanvas.Background = visualBrush;
			}
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x00149AD5 File Offset: 0x00147CD5
		internal void ClearHeader()
		{
			this._referenceHeader = null;
			if (this._visualBrushCanvas != null)
			{
				this._visualBrushCanvas.Background = null;
			}
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x00149AF4 File Offset: 0x00147CF4
		private double GetVisualCanvasMarginX()
		{
			double num = 0.0;
			if (this._visualBrushCanvas != null)
			{
				Thickness margin = this._visualBrushCanvas.Margin;
				num += margin.Left;
				num += margin.Right;
			}
			return num;
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x00149B34 File Offset: 0x00147D34
		private double GetVisualCanvasMarginY()
		{
			double num = 0.0;
			if (this._visualBrushCanvas != null)
			{
				Thickness margin = this._visualBrushCanvas.Margin;
				num += margin.Top;
				num += margin.Bottom;
			}
			return num;
		}

		// Token: 0x04002999 RID: 10649
		private DataGridColumnHeader _referenceHeader;

		// Token: 0x0400299A RID: 10650
		private const string VisualBrushCanvasTemplateName = "PART_VisualBrushCanvas";

		// Token: 0x0400299B RID: 10651
		private Canvas _visualBrushCanvas;
	}
}
