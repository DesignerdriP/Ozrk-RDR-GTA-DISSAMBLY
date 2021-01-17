using System;
using System.Collections.ObjectModel;
using System.Windows;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E2 RID: 1762
	internal sealed class RowParagraphResult : ParagraphResult
	{
		// Token: 0x0600718B RID: 29067 RVA: 0x00207582 File Offset: 0x00205782
		internal RowParagraphResult(BaseParaClient paraClient, int index, Rect rowRect, RowParagraph rowParagraph) : base(paraClient, rowRect, rowParagraph.Element)
		{
			this._index = index;
		}

		// Token: 0x17001AF8 RID: 6904
		// (get) Token: 0x0600718C RID: 29068 RVA: 0x0020759C File Offset: 0x0020579C
		internal ReadOnlyCollection<ParagraphResult> CellParagraphs
		{
			get
			{
				if (this._cells == null)
				{
					this._cells = ((TableParaClient)this._paraClient).GetChildrenParagraphResultsForRow(this._index, out this._hasTextContent);
				}
				Invariant.Assert(this._cells != null, "Paragraph collection is empty");
				return this._cells;
			}
		}

		// Token: 0x17001AF9 RID: 6905
		// (get) Token: 0x0600718D RID: 29069 RVA: 0x002075EC File Offset: 0x002057EC
		internal override bool HasTextContent
		{
			get
			{
				if (this._cells == null)
				{
					ReadOnlyCollection<ParagraphResult> cellParagraphs = this.CellParagraphs;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x04003729 RID: 14121
		private ReadOnlyCollection<ParagraphResult> _cells;

		// Token: 0x0400372A RID: 14122
		private int _index;
	}
}
