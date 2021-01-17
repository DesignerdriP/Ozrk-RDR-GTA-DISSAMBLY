using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006CD RID: 1741
	internal interface IFlowDocumentFormatter
	{
		// Token: 0x0600707C RID: 28796
		void OnContentInvalidated(bool affectsLayout);

		// Token: 0x0600707D RID: 28797
		void OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end);

		// Token: 0x0600707E RID: 28798
		void Suspend();

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x0600707F RID: 28799
		bool IsLayoutDataValid { get; }
	}
}
