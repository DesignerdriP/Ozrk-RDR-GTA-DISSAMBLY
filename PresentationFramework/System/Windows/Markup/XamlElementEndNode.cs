using System;

namespace System.Windows.Markup
{
	// Token: 0x0200024C RID: 588
	internal class XamlElementEndNode : XamlNode
	{
		// Token: 0x060022E3 RID: 8931 RVA: 0x000AC43C File Offset: 0x000AA63C
		internal XamlElementEndNode(int lineNumber, int linePosition, int depth) : this(XamlNodeType.ElementEnd, lineNumber, linePosition, depth)
		{
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000AC0C0 File Offset: 0x000AA2C0
		internal XamlElementEndNode(XamlNodeType tokenType, int lineNumber, int linePosition, int depth) : base(tokenType, lineNumber, linePosition, depth)
		{
		}
	}
}
