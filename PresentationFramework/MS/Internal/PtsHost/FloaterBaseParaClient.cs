using System;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000622 RID: 1570
	internal abstract class FloaterBaseParaClient : BaseParaClient
	{
		// Token: 0x06006804 RID: 26628 RVA: 0x001CF106 File Offset: 0x001CD306
		protected FloaterBaseParaClient(FloaterBaseParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x06006805 RID: 26629 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void ArrangeFloater(PTS.FSRECT rcFloater, PTS.FSRECT rcHostPara, uint fswdirParent, PageContext pageContext)
		{
		}

		// Token: 0x06006806 RID: 26630
		internal abstract override TextContentRange GetTextContentRange();
	}
}
