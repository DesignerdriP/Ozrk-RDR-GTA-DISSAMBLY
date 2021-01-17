using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006DF RID: 1759
	internal sealed class ContainerParagraphResult : ParagraphResult
	{
		// Token: 0x0600716F RID: 29039 RVA: 0x0020724B File Offset: 0x0020544B
		internal ContainerParagraphResult(ContainerParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x06007170 RID: 29040 RVA: 0x00207254 File Offset: 0x00205454
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			return ((ContainerParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
		}

		// Token: 0x17001AEF RID: 6895
		// (get) Token: 0x06007171 RID: 29041 RVA: 0x00207269 File Offset: 0x00205469
		internal ReadOnlyCollection<ParagraphResult> Paragraphs
		{
			get
			{
				if (this._paragraphs == null)
				{
					this._paragraphs = ((ContainerParaClient)this._paraClient).GetChildrenParagraphResults(out this._hasTextContent);
				}
				Invariant.Assert(this._paragraphs != null, "Paragraph collection is empty");
				return this._paragraphs;
			}
		}

		// Token: 0x17001AF0 RID: 6896
		// (get) Token: 0x06007172 RID: 29042 RVA: 0x002072A8 File Offset: 0x002054A8
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

		// Token: 0x04003724 RID: 14116
		private ReadOnlyCollection<ParagraphResult> _paragraphs;
	}
}
