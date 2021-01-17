using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006E6 RID: 1766
	internal sealed class FloaterParagraphResult : FloaterBaseParagraphResult
	{
		// Token: 0x0600719A RID: 29082 RVA: 0x00207891 File Offset: 0x00205A91
		internal FloaterParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x0600719B RID: 29083 RVA: 0x0020789A File Offset: 0x00205A9A
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((FloaterParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17001B03 RID: 6915
		// (get) Token: 0x0600719C RID: 29084 RVA: 0x002078DC File Offset: 0x00205ADC
		internal override bool HasTextContent
		{
			get
			{
				if (this._columns == null)
				{
					ReadOnlyCollection<ColumnResult> columns = this.Columns;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x0600719D RID: 29085 RVA: 0x002078FE File Offset: 0x00205AFE
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((FloaterParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17001B05 RID: 6917
		// (get) Token: 0x0600719E RID: 29086 RVA: 0x00207938 File Offset: 0x00205B38
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x0600719F RID: 29087 RVA: 0x002079B0 File Offset: 0x00205BB0
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect, out bool success)
		{
			success = false;
			if (this.Contains(startPosition, true))
			{
				success = true;
				ITextPointer endPosition2 = (endPosition.CompareTo(base.EndPosition) < 0) ? endPosition : base.EndPosition;
				return ((FloaterParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(this.Columns, this.FloatingElements, startPosition, endPosition2, visibleRect);
			}
			return null;
		}

		// Token: 0x0400372F RID: 14127
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x04003730 RID: 14128
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
