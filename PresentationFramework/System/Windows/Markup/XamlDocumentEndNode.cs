using System;

namespace System.Windows.Markup
{
	// Token: 0x02000240 RID: 576
	internal class XamlDocumentEndNode : XamlNode
	{
		// Token: 0x060022A1 RID: 8865 RVA: 0x000ABF64 File Offset: 0x000AA164
		internal XamlDocumentEndNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.DocumentEnd, lineNumber, linePosition, depth)
		{
		}
	}
}
