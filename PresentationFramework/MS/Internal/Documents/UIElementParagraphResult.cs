using System;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006E7 RID: 1767
	internal sealed class UIElementParagraphResult : FloaterBaseParagraphResult
	{
		// Token: 0x060071A0 RID: 29088 RVA: 0x00207891 File Offset: 0x00205A91
		internal UIElementParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001B06 RID: 6918
		// (get) Token: 0x060071A1 RID: 29089 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool HasTextContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060071A2 RID: 29090 RVA: 0x00207A09 File Offset: 0x00205C09
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition)
		{
			return ((UIElementParaClient)this._paraClient).GetTightBoundingGeometryFromTextPositions(startPosition, endPosition);
		}
	}
}
