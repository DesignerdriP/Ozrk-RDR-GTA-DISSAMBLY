using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;

namespace MS.Internal.Text
{
	// Token: 0x02000606 RID: 1542
	internal sealed class TextLineResult : LineResult
	{
		// Token: 0x06006681 RID: 26241 RVA: 0x001CC8F8 File Offset: 0x001CAAF8
		internal override ITextPointer GetTextPositionFromDistance(double distance)
		{
			return this._owner.GetTextPositionFromDistance(this._dcp, distance, this._layoutBox.Top, this._index);
		}

		// Token: 0x06006682 RID: 26242 RVA: 0x0000B02A File Offset: 0x0000922A
		internal override bool IsAtCaretUnitBoundary(ITextPointer position)
		{
			return false;
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override ITextPointer GetNextCaretUnitPosition(ITextPointer position, LogicalDirection direction)
		{
			return null;
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override ITextPointer GetBackspaceCaretUnitPosition(ITextPointer position)
		{
			return null;
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x0000C238 File Offset: 0x0000A438
		internal override ReadOnlyCollection<GlyphRun> GetGlyphRuns(ITextPointer start, ITextPointer end)
		{
			return null;
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x001CC92B File Offset: 0x001CAB2B
		internal override ITextPointer GetContentEndPosition()
		{
			this.EnsureComplexData();
			return this._owner.TextContainer.CreatePointerAtOffset(this._dcp + this._cchContent, LogicalDirection.Backward);
		}

		// Token: 0x06006687 RID: 26247 RVA: 0x001CC951 File Offset: 0x001CAB51
		internal override ITextPointer GetEllipsesPosition()
		{
			this.EnsureComplexData();
			if (this._cchEllipses != 0)
			{
				return this._owner.TextContainer.CreatePointerAtOffset(this._dcp + this._cch - this._cchEllipses, LogicalDirection.Forward);
			}
			return null;
		}

		// Token: 0x06006688 RID: 26248 RVA: 0x001CC988 File Offset: 0x001CAB88
		internal override int GetContentEndPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cchContent;
		}

		// Token: 0x06006689 RID: 26249 RVA: 0x001CC99D File Offset: 0x001CAB9D
		internal override int GetEllipsesPositionCP()
		{
			this.EnsureComplexData();
			return this._dcp + this._cch - this._cchEllipses;
		}

		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x0600668A RID: 26250 RVA: 0x001CC9B9 File Offset: 0x001CABB9
		internal override ITextPointer StartPosition
		{
			get
			{
				if (this._startPosition == null)
				{
					this._startPosition = this._owner.TextContainer.CreatePointerAtOffset(this._dcp, LogicalDirection.Forward);
				}
				return this._startPosition;
			}
		}

		// Token: 0x170018A0 RID: 6304
		// (get) Token: 0x0600668B RID: 26251 RVA: 0x001CC9E6 File Offset: 0x001CABE6
		internal override ITextPointer EndPosition
		{
			get
			{
				if (this._endPosition == null)
				{
					this._endPosition = this._owner.TextContainer.CreatePointerAtOffset(this._dcp + this._cch, LogicalDirection.Backward);
				}
				return this._endPosition;
			}
		}

		// Token: 0x170018A1 RID: 6305
		// (get) Token: 0x0600668C RID: 26252 RVA: 0x001CCA1A File Offset: 0x001CAC1A
		internal override int StartPositionCP
		{
			get
			{
				return this._dcp;
			}
		}

		// Token: 0x170018A2 RID: 6306
		// (get) Token: 0x0600668D RID: 26253 RVA: 0x001CCA22 File Offset: 0x001CAC22
		internal override int EndPositionCP
		{
			get
			{
				return this._dcp + this._cch;
			}
		}

		// Token: 0x170018A3 RID: 6307
		// (get) Token: 0x0600668E RID: 26254 RVA: 0x001CCA31 File Offset: 0x001CAC31
		internal override Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x170018A4 RID: 6308
		// (get) Token: 0x0600668F RID: 26255 RVA: 0x001CCA39 File Offset: 0x001CAC39
		internal override double Baseline
		{
			get
			{
				return this._baseline;
			}
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x001CCA44 File Offset: 0x001CAC44
		internal TextLineResult(TextBlock owner, int dcp, int cch, Rect layoutBox, double baseline, int index)
		{
			this._owner = owner;
			this._dcp = dcp;
			this._cch = cch;
			this._layoutBox = layoutBox;
			this._baseline = baseline;
			this._index = index;
			this._cchContent = (this._cchEllipses = -1);
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x001CCA94 File Offset: 0x001CAC94
		private void EnsureComplexData()
		{
			if (this._cchContent == -1)
			{
				this._owner.GetLineDetails(this._dcp, this._index, this._layoutBox.Top, out this._cchContent, out this._cchEllipses);
			}
		}

		// Token: 0x0400331E RID: 13086
		private readonly TextBlock _owner;

		// Token: 0x0400331F RID: 13087
		private readonly int _dcp;

		// Token: 0x04003320 RID: 13088
		private readonly int _cch;

		// Token: 0x04003321 RID: 13089
		private readonly Rect _layoutBox;

		// Token: 0x04003322 RID: 13090
		private int _index;

		// Token: 0x04003323 RID: 13091
		private readonly double _baseline;

		// Token: 0x04003324 RID: 13092
		private ITextPointer _startPosition;

		// Token: 0x04003325 RID: 13093
		private ITextPointer _endPosition;

		// Token: 0x04003326 RID: 13094
		private int _cchContent;

		// Token: 0x04003327 RID: 13095
		private int _cchEllipses;
	}
}
