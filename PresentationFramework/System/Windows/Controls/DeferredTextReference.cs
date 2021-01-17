using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x020004C8 RID: 1224
	internal class DeferredTextReference : DeferredReference
	{
		// Token: 0x06004A4E RID: 19022 RVA: 0x0014F8FE File Offset: 0x0014DAFE
		internal DeferredTextReference(ITextContainer textContainer)
		{
			this._textContainer = textContainer;
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x0014F910 File Offset: 0x0014DB10
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			string textInternal = TextRangeBase.GetTextInternal(this._textContainer.Start, this._textContainer.End);
			TextBox textBox = this._textContainer.Parent as TextBox;
			if (textBox != null)
			{
				textBox.OnDeferredTextReferenceResolved(this, textInternal);
			}
			return textInternal;
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x000B075A File Offset: 0x000AE95A
		internal override Type GetValueType()
		{
			return typeof(string);
		}

		// Token: 0x04002A52 RID: 10834
		private readonly ITextContainer _textContainer;
	}
}
