using System;

namespace System.Windows.Markup
{
	// Token: 0x02000244 RID: 580
	internal class XamlPropertyComplexEndNode : XamlNode
	{
		// Token: 0x060022AF RID: 8879 RVA: 0x000AC0B4 File Offset: 0x000AA2B4
		internal XamlPropertyComplexEndNode(int lineNumber, int linePosition, int depth) : base(XamlNodeType.PropertyComplexEnd, lineNumber, linePosition, depth)
		{
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000AC0C0 File Offset: 0x000AA2C0
		internal XamlPropertyComplexEndNode(XamlNodeType token, int lineNumber, int linePosition, int depth) : base(token, lineNumber, linePosition, depth)
		{
		}
	}
}
