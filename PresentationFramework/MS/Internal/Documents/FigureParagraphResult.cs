using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006E4 RID: 1764
	internal sealed class FigureParagraphResult : ParagraphResult
	{
		// Token: 0x06007193 RID: 29075 RVA: 0x0020724B File Offset: 0x0020544B
		internal FigureParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001AFE RID: 6910
		// (get) Token: 0x06007194 RID: 29076 RVA: 0x00207722 File Offset: 0x00205922
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((FigureParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17001AFF RID: 6911
		// (get) Token: 0x06007195 RID: 29077 RVA: 0x00207764 File Offset: 0x00205964
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

		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x06007196 RID: 29078 RVA: 0x00207786 File Offset: 0x00205986
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((FigureParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17001B01 RID: 6913
		// (get) Token: 0x06007197 RID: 29079 RVA: 0x002077C0 File Offset: 0x002059C0
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x06007198 RID: 29080 RVA: 0x00207838 File Offset: 0x00205A38
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect, out bool success)
		{
			success = false;
			if (this.Contains(startPosition, true))
			{
				success = true;
				ITextPointer endPosition2 = (endPosition.CompareTo(base.EndPosition) < 0) ? endPosition : base.EndPosition;
				return ((FigureParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(this.Columns, this.FloatingElements, startPosition, endPosition2, visibleRect);
			}
			return null;
		}

		// Token: 0x0400372D RID: 14125
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x0400372E RID: 14126
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
