using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000630 RID: 1584
	internal sealed class MbpInfo
	{
		// Token: 0x060068B9 RID: 26809 RVA: 0x001D8E68 File Offset: 0x001D7068
		internal static MbpInfo FromElement(DependencyObject o, double pixelsPerDip)
		{
			if (o is Block || o is AnchoredBlock || o is TableCell || o is ListItem)
			{
				MbpInfo mbpInfo = new MbpInfo((TextElement)o);
				double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(o);
				if (mbpInfo.IsMarginAuto)
				{
					MbpInfo.ResolveAutoMargin(mbpInfo, o, lineHeightValue);
				}
				if (mbpInfo.IsPaddingAuto)
				{
					MbpInfo.ResolveAutoPadding(mbpInfo, o, lineHeightValue, pixelsPerDip);
				}
				return mbpInfo;
			}
			return MbpInfo._empty;
		}

		// Token: 0x060068BA RID: 26810 RVA: 0x001D8ED0 File Offset: 0x001D70D0
		internal void MirrorMargin()
		{
			MbpInfo.ReverseFlowDirection(ref this._margin);
		}

		// Token: 0x060068BB RID: 26811 RVA: 0x001D8EDD File Offset: 0x001D70DD
		internal void MirrorBP()
		{
			MbpInfo.ReverseFlowDirection(ref this._border);
			MbpInfo.ReverseFlowDirection(ref this._padding);
		}

		// Token: 0x060068BC RID: 26812 RVA: 0x001D8EF8 File Offset: 0x001D70F8
		private static void ReverseFlowDirection(ref Thickness thickness)
		{
			double left = thickness.Left;
			thickness.Left = thickness.Right;
			thickness.Right = left;
		}

		// Token: 0x060068BE RID: 26814 RVA: 0x001D8F2B File Offset: 0x001D712B
		private MbpInfo()
		{
			this._margin = default(Thickness);
			this._border = default(Thickness);
			this._padding = default(Thickness);
			this._borderBrush = new SolidColorBrush();
		}

		// Token: 0x060068BF RID: 26815 RVA: 0x001D8F64 File Offset: 0x001D7164
		private MbpInfo(TextElement block)
		{
			this._margin = (Thickness)block.GetValue(Block.MarginProperty);
			this._border = (Thickness)block.GetValue(Block.BorderThicknessProperty);
			this._padding = (Thickness)block.GetValue(Block.PaddingProperty);
			this._borderBrush = (Brush)block.GetValue(Block.BorderBrushProperty);
		}

		// Token: 0x060068C0 RID: 26816 RVA: 0x001D8FD0 File Offset: 0x001D71D0
		private static void ResolveAutoMargin(MbpInfo mbp, DependencyObject o, double lineHeight)
		{
			Thickness thickness;
			if (o is Paragraph)
			{
				DependencyObject parent = ((Paragraph)o).Parent;
				if (parent is ListItem || parent is TableCell || parent is AnchoredBlock)
				{
					thickness = new Thickness(0.0);
				}
				else
				{
					thickness = new Thickness(0.0, lineHeight, 0.0, lineHeight);
				}
			}
			else if (o is Table || o is List)
			{
				thickness = new Thickness(0.0, lineHeight, 0.0, lineHeight);
			}
			else if (o is Figure || o is Floater)
			{
				thickness = new Thickness(0.5 * lineHeight);
			}
			else
			{
				thickness = new Thickness(0.0);
			}
			mbp.Margin = new Thickness(double.IsNaN(mbp.Margin.Left) ? thickness.Left : mbp.Margin.Left, double.IsNaN(mbp.Margin.Top) ? thickness.Top : mbp.Margin.Top, double.IsNaN(mbp.Margin.Right) ? thickness.Right : mbp.Margin.Right, double.IsNaN(mbp.Margin.Bottom) ? thickness.Bottom : mbp.Margin.Bottom);
		}

		// Token: 0x060068C1 RID: 26817 RVA: 0x001D9154 File Offset: 0x001D7354
		private static void ResolveAutoPadding(MbpInfo mbp, DependencyObject o, double lineHeight, double pixelsPerDip)
		{
			Thickness thickness;
			if (o is Figure || o is Floater)
			{
				thickness = new Thickness(0.5 * lineHeight);
			}
			else if (o is List)
			{
				thickness = ListMarkerSourceInfo.CalculatePadding((List)o, lineHeight, pixelsPerDip);
			}
			else
			{
				thickness = new Thickness(0.0);
			}
			mbp.Padding = new Thickness(double.IsNaN(mbp.Padding.Left) ? thickness.Left : mbp.Padding.Left, double.IsNaN(mbp.Padding.Top) ? thickness.Top : mbp.Padding.Top, double.IsNaN(mbp.Padding.Right) ? thickness.Right : mbp.Padding.Right, double.IsNaN(mbp.Padding.Bottom) ? thickness.Bottom : mbp.Padding.Bottom);
		}

		// Token: 0x17001949 RID: 6473
		// (get) Token: 0x060068C2 RID: 26818 RVA: 0x001D9268 File Offset: 0x001D7468
		internal int MBPLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Left) + TextDpi.ToTextDpi(this._border.Left) + TextDpi.ToTextDpi(this._padding.Left);
			}
		}

		// Token: 0x1700194A RID: 6474
		// (get) Token: 0x060068C3 RID: 26819 RVA: 0x001D929C File Offset: 0x001D749C
		internal int MBPRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Right) + TextDpi.ToTextDpi(this._border.Right) + TextDpi.ToTextDpi(this._padding.Right);
			}
		}

		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x060068C4 RID: 26820 RVA: 0x001D92D0 File Offset: 0x001D74D0
		internal int MBPTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Top) + TextDpi.ToTextDpi(this._border.Top) + TextDpi.ToTextDpi(this._padding.Top);
			}
		}

		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x060068C5 RID: 26821 RVA: 0x001D9304 File Offset: 0x001D7504
		internal int MBPBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Bottom) + TextDpi.ToTextDpi(this._border.Bottom) + TextDpi.ToTextDpi(this._padding.Bottom);
			}
		}

		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x060068C6 RID: 26822 RVA: 0x001D9338 File Offset: 0x001D7538
		internal int BPLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Left) + TextDpi.ToTextDpi(this._padding.Left);
			}
		}

		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x060068C7 RID: 26823 RVA: 0x001D935B File Offset: 0x001D755B
		internal int BPRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Right) + TextDpi.ToTextDpi(this._padding.Right);
			}
		}

		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x060068C8 RID: 26824 RVA: 0x001D937E File Offset: 0x001D757E
		internal int BPTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Top) + TextDpi.ToTextDpi(this._padding.Top);
			}
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x060068C9 RID: 26825 RVA: 0x001D93A1 File Offset: 0x001D75A1
		internal int BPBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Bottom) + TextDpi.ToTextDpi(this._padding.Bottom);
			}
		}

		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x060068CA RID: 26826 RVA: 0x001D93C4 File Offset: 0x001D75C4
		internal int BorderLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Left);
			}
		}

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x060068CB RID: 26827 RVA: 0x001D93D6 File Offset: 0x001D75D6
		internal int BorderRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Right);
			}
		}

		// Token: 0x17001953 RID: 6483
		// (get) Token: 0x060068CC RID: 26828 RVA: 0x001D93E8 File Offset: 0x001D75E8
		internal int BorderTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Top);
			}
		}

		// Token: 0x17001954 RID: 6484
		// (get) Token: 0x060068CD RID: 26829 RVA: 0x001D93FA File Offset: 0x001D75FA
		internal int BorderBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._border.Bottom);
			}
		}

		// Token: 0x17001955 RID: 6485
		// (get) Token: 0x060068CE RID: 26830 RVA: 0x001D940C File Offset: 0x001D760C
		internal int MarginLeft
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Left);
			}
		}

		// Token: 0x17001956 RID: 6486
		// (get) Token: 0x060068CF RID: 26831 RVA: 0x001D941E File Offset: 0x001D761E
		internal int MarginRight
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Right);
			}
		}

		// Token: 0x17001957 RID: 6487
		// (get) Token: 0x060068D0 RID: 26832 RVA: 0x001D9430 File Offset: 0x001D7630
		internal int MarginTop
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Top);
			}
		}

		// Token: 0x17001958 RID: 6488
		// (get) Token: 0x060068D1 RID: 26833 RVA: 0x001D9442 File Offset: 0x001D7642
		internal int MarginBottom
		{
			get
			{
				return TextDpi.ToTextDpi(this._margin.Bottom);
			}
		}

		// Token: 0x17001959 RID: 6489
		// (get) Token: 0x060068D2 RID: 26834 RVA: 0x001D9454 File Offset: 0x001D7654
		// (set) Token: 0x060068D3 RID: 26835 RVA: 0x001D945C File Offset: 0x001D765C
		internal Thickness Margin
		{
			get
			{
				return this._margin;
			}
			set
			{
				this._margin = value;
			}
		}

		// Token: 0x1700195A RID: 6490
		// (get) Token: 0x060068D4 RID: 26836 RVA: 0x001D9465 File Offset: 0x001D7665
		// (set) Token: 0x060068D5 RID: 26837 RVA: 0x001D946D File Offset: 0x001D766D
		internal Thickness Border
		{
			get
			{
				return this._border;
			}
			set
			{
				this._border = value;
			}
		}

		// Token: 0x1700195B RID: 6491
		// (get) Token: 0x060068D6 RID: 26838 RVA: 0x001D9476 File Offset: 0x001D7676
		// (set) Token: 0x060068D7 RID: 26839 RVA: 0x001D947E File Offset: 0x001D767E
		internal Thickness Padding
		{
			get
			{
				return this._padding;
			}
			set
			{
				this._padding = value;
			}
		}

		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x060068D8 RID: 26840 RVA: 0x001D9487 File Offset: 0x001D7687
		internal Brush BorderBrush
		{
			get
			{
				return this._borderBrush;
			}
		}

		// Token: 0x1700195D RID: 6493
		// (get) Token: 0x060068D9 RID: 26841 RVA: 0x001D9490 File Offset: 0x001D7690
		private bool IsPaddingAuto
		{
			get
			{
				return double.IsNaN(this._padding.Left) || double.IsNaN(this._padding.Right) || double.IsNaN(this._padding.Top) || double.IsNaN(this._padding.Bottom);
			}
		}

		// Token: 0x1700195E RID: 6494
		// (get) Token: 0x060068DA RID: 26842 RVA: 0x001D94E8 File Offset: 0x001D76E8
		private bool IsMarginAuto
		{
			get
			{
				return double.IsNaN(this._margin.Left) || double.IsNaN(this._margin.Right) || double.IsNaN(this._margin.Top) || double.IsNaN(this._margin.Bottom);
			}
		}

		// Token: 0x040033ED RID: 13293
		private Thickness _margin;

		// Token: 0x040033EE RID: 13294
		private Thickness _border;

		// Token: 0x040033EF RID: 13295
		private Thickness _padding;

		// Token: 0x040033F0 RID: 13296
		private Brush _borderBrush;

		// Token: 0x040033F1 RID: 13297
		private static MbpInfo _empty = new MbpInfo();
	}
}
