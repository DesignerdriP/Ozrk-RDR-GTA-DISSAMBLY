using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200062F RID: 1583
	internal sealed class MarginCollapsingState : UnmanagedHandle
	{
		// Token: 0x060068B1 RID: 26801 RVA: 0x001D8CE8 File Offset: 0x001D6EE8
		internal static void CollapseTopMargin(PtsContext ptsContext, MbpInfo mbp, MarginCollapsingState mcsCurrent, out MarginCollapsingState mcsNew, out int margin)
		{
			margin = 0;
			mcsNew = null;
			mcsNew = new MarginCollapsingState(ptsContext, mbp.MarginTop);
			if (mcsCurrent != null)
			{
				mcsNew.Collapse(mcsCurrent);
			}
			if (mbp.BPTop != 0)
			{
				margin = mcsNew.Margin;
				mcsNew.Dispose();
				mcsNew = null;
				return;
			}
			if (mcsCurrent == null && DoubleUtil.IsZero(mbp.Margin.Top))
			{
				mcsNew.Dispose();
				mcsNew = null;
			}
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x001D8D54 File Offset: 0x001D6F54
		internal static void CollapseBottomMargin(PtsContext ptsContext, MbpInfo mbp, MarginCollapsingState mcsCurrent, out MarginCollapsingState mcsNew, out int margin)
		{
			margin = 0;
			mcsNew = null;
			if (!DoubleUtil.IsZero(mbp.Margin.Bottom))
			{
				mcsNew = new MarginCollapsingState(ptsContext, mbp.MarginBottom);
			}
			if (mcsCurrent != null)
			{
				if (mbp.BPBottom != 0)
				{
					margin = mcsCurrent.Margin;
					return;
				}
				if (mcsNew == null)
				{
					mcsNew = new MarginCollapsingState(ptsContext, 0);
				}
				mcsNew.Collapse(mcsCurrent);
			}
		}

		// Token: 0x060068B3 RID: 26803 RVA: 0x001D8DB5 File Offset: 0x001D6FB5
		internal MarginCollapsingState(PtsContext ptsContext, int margin) : base(ptsContext)
		{
			this._maxPositive = ((margin >= 0) ? margin : 0);
			this._minNegative = ((margin < 0) ? margin : 0);
		}

		// Token: 0x060068B4 RID: 26804 RVA: 0x001D8DDA File Offset: 0x001D6FDA
		private MarginCollapsingState(MarginCollapsingState mcs) : base(mcs.PtsContext)
		{
			this._maxPositive = mcs._maxPositive;
			this._minNegative = mcs._minNegative;
		}

		// Token: 0x060068B5 RID: 26805 RVA: 0x001D8E00 File Offset: 0x001D7000
		internal MarginCollapsingState Clone()
		{
			return new MarginCollapsingState(this);
		}

		// Token: 0x060068B6 RID: 26806 RVA: 0x001D8E08 File Offset: 0x001D7008
		internal bool IsEqual(MarginCollapsingState mcs)
		{
			return this._maxPositive == mcs._maxPositive && this._minNegative == mcs._minNegative;
		}

		// Token: 0x060068B7 RID: 26807 RVA: 0x001D8E28 File Offset: 0x001D7028
		internal void Collapse(MarginCollapsingState mcs)
		{
			this._maxPositive = Math.Max(this._maxPositive, mcs._maxPositive);
			this._minNegative = Math.Min(this._minNegative, mcs._minNegative);
		}

		// Token: 0x17001948 RID: 6472
		// (get) Token: 0x060068B8 RID: 26808 RVA: 0x001D8E58 File Offset: 0x001D7058
		internal int Margin
		{
			get
			{
				return this._maxPositive + this._minNegative;
			}
		}

		// Token: 0x040033EB RID: 13291
		private int _maxPositive;

		// Token: 0x040033EC RID: 13292
		private int _minNegative;
	}
}
