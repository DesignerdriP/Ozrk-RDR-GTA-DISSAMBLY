using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x020004C6 RID: 1222
	internal class DeferredRunTextReference : DeferredReference
	{
		// Token: 0x06004A48 RID: 19016 RVA: 0x0014F8A5 File Offset: 0x0014DAA5
		internal DeferredRunTextReference(Run run)
		{
			this._run = run;
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x0014F8B4 File Offset: 0x0014DAB4
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return TextRangeBase.GetTextInternal(this._run.ContentStart, this._run.ContentEnd);
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x000B075A File Offset: 0x000AE95A
		internal override Type GetValueType()
		{
			return typeof(string);
		}

		// Token: 0x04002A50 RID: 10832
		private readonly Run _run;
	}
}
