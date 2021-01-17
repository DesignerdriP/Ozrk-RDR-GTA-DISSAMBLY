using System;

namespace System.Windows.Markup
{
	// Token: 0x02000225 RID: 549
	internal abstract class ParserHooks
	{
		// Token: 0x06002202 RID: 8706 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual ParserAction LoadNode(XamlNode tokenNode)
		{
			return ParserAction.Normal;
		}
	}
}
