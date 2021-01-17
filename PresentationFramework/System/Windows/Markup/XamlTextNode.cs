using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x02000241 RID: 577
	[DebuggerDisplay("Text:{_text}")]
	internal class XamlTextNode : XamlNode
	{
		// Token: 0x060022A2 RID: 8866 RVA: 0x000ABF70 File Offset: 0x000AA170
		internal XamlTextNode(int lineNumber, int linePosition, int depth, string textContent, Type converterType) : base(XamlNodeType.Text, lineNumber, linePosition, depth)
		{
			this._text = textContent;
			this._converterType = converterType;
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x000ABF8D File Offset: 0x000AA18D
		internal string Text
		{
			get
			{
				return this._text;
			}
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000ABF95 File Offset: 0x000AA195
		internal Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x000ABF9D File Offset: 0x000AA19D
		internal void UpdateText(string text)
		{
			this._text = text;
		}

		// Token: 0x04001A27 RID: 6695
		private string _text;

		// Token: 0x04001A28 RID: 6696
		private Type _converterType;
	}
}
