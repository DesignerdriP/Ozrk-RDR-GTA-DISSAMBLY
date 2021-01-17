using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000613 RID: 1555
	internal sealed class ColumnPropertiesGroup
	{
		// Token: 0x0600677E RID: 26494 RVA: 0x001CF030 File Offset: 0x001CD230
		internal ColumnPropertiesGroup(DependencyObject o)
		{
			this._columnWidth = (double)o.GetValue(FlowDocument.ColumnWidthProperty);
			this._columnGap = (double)o.GetValue(FlowDocument.ColumnGapProperty);
			this._columnRuleWidth = (double)o.GetValue(FlowDocument.ColumnRuleWidthProperty);
			this._columnRuleBrush = (Brush)o.GetValue(FlowDocument.ColumnRuleBrushProperty);
			this._isColumnWidthFlexible = (bool)o.GetValue(FlowDocument.IsColumnWidthFlexibleProperty);
		}

		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x0600677F RID: 26495 RVA: 0x001CF0B1 File Offset: 0x001CD2B1
		internal double ColumnWidth
		{
			get
			{
				return this._columnWidth;
			}
		}

		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x06006780 RID: 26496 RVA: 0x001CF0B9 File Offset: 0x001CD2B9
		internal bool IsColumnWidthFlexible
		{
			get
			{
				return this._isColumnWidthFlexible;
			}
		}

		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x06006781 RID: 26497 RVA: 0x00094B24 File Offset: 0x00092D24
		internal ColumnSpaceDistribution ColumnSpaceDistribution
		{
			get
			{
				return ColumnSpaceDistribution.Between;
			}
		}

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x06006782 RID: 26498 RVA: 0x001CF0C1 File Offset: 0x001CD2C1
		internal double ColumnGap
		{
			get
			{
				Invariant.Assert(!double.IsNaN(this._columnGap));
				return this._columnGap;
			}
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x06006783 RID: 26499 RVA: 0x001CF0DC File Offset: 0x001CD2DC
		internal Brush ColumnRuleBrush
		{
			get
			{
				return this._columnRuleBrush;
			}
		}

		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x06006784 RID: 26500 RVA: 0x001CF0E4 File Offset: 0x001CD2E4
		internal double ColumnRuleWidth
		{
			get
			{
				return this._columnRuleWidth;
			}
		}

		// Token: 0x1700190E RID: 6414
		// (get) Token: 0x06006785 RID: 26501 RVA: 0x001CF0EC File Offset: 0x001CD2EC
		internal bool ColumnWidthAuto
		{
			get
			{
				return DoubleUtil.IsNaN(this._columnWidth);
			}
		}

		// Token: 0x1700190F RID: 6415
		// (get) Token: 0x06006786 RID: 26502 RVA: 0x001CF0F9 File Offset: 0x001CD2F9
		internal bool ColumnGapAuto
		{
			get
			{
				return DoubleUtil.IsNaN(this._columnGap);
			}
		}

		// Token: 0x04003373 RID: 13171
		private double _columnWidth;

		// Token: 0x04003374 RID: 13172
		private bool _isColumnWidthFlexible;

		// Token: 0x04003375 RID: 13173
		private double _columnGap;

		// Token: 0x04003376 RID: 13174
		private Brush _columnRuleBrush;

		// Token: 0x04003377 RID: 13175
		private double _columnRuleWidth;
	}
}
