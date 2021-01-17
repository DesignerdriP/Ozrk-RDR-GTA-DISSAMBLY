using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E1 RID: 1761
	internal sealed class TableParagraphResult : ParagraphResult
	{
		// Token: 0x06007180 RID: 29056 RVA: 0x0020724B File Offset: 0x0020544B
		internal TableParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x06007181 RID: 29057 RVA: 0x0020747F File Offset: 0x0020567F
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPoint(Point point, bool snapToText)
		{
			return ((TableParaClient)this._paraClient).GetParagraphsFromPoint(point, snapToText);
		}

		// Token: 0x06007182 RID: 29058 RVA: 0x00207493 File Offset: 0x00205693
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetParagraphsFromPosition(position);
		}

		// Token: 0x06007183 RID: 29059 RVA: 0x002074A6 File Offset: 0x002056A6
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			return ((TableParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
		}

		// Token: 0x06007184 RID: 29060 RVA: 0x002074BB File Offset: 0x002056BB
		internal CellParaClient GetCellParaClientFromPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetCellParaClientFromPosition(position);
		}

		// Token: 0x06007185 RID: 29061 RVA: 0x002074CE File Offset: 0x002056CE
		internal CellParaClient GetCellAbove(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			return ((TableParaClient)this._paraClient).GetCellAbove(suggestedX, rowGroupIndex, rowIndex);
		}

		// Token: 0x06007186 RID: 29062 RVA: 0x002074E3 File Offset: 0x002056E3
		internal CellParaClient GetCellBelow(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			return ((TableParaClient)this._paraClient).GetCellBelow(suggestedX, rowGroupIndex, rowIndex);
		}

		// Token: 0x06007187 RID: 29063 RVA: 0x002074F8 File Offset: 0x002056F8
		internal CellInfo GetCellInfoFromPoint(Point point)
		{
			return ((TableParaClient)this._paraClient).GetCellInfoFromPoint(point);
		}

		// Token: 0x06007188 RID: 29064 RVA: 0x0020750B File Offset: 0x0020570B
		internal Rect GetRectangleFromRowEndPosition(ITextPointer position)
		{
			return ((TableParaClient)this._paraClient).GetRectangleFromRowEndPosition(position);
		}

		// Token: 0x17001AF6 RID: 6902
		// (get) Token: 0x06007189 RID: 29065 RVA: 0x0020751E File Offset: 0x0020571E
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			get
			{
				if (this._paragraphs == null)
				{
					this._paragraphs = ((TableParaClient)this._paraClient).GetChildrenParagraphResults(out this._hasTextContent);
				}
				Invariant.Assert(this._paragraphs != null, "Paragraph collection is empty");
				return this._paragraphs;
			}
		}

		// Token: 0x17001AF7 RID: 6903
		// (get) Token: 0x0600718A RID: 29066 RVA: 0x00207560 File Offset: 0x00205760
		internal override bool HasTextContent
		{
			get
			{
				if (this._paragraphs == null)
				{
					ReadOnlyCollection<ParagraphResult> paragraphs = this.Paragraphs;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x04003728 RID: 14120
		private ReadOnlyCollection<ParagraphResult> _paragraphs;
	}
}
