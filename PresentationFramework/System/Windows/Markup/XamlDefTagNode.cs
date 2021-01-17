﻿using System;
using System.Xml;

namespace System.Windows.Markup
{
	// Token: 0x0200025C RID: 604
	internal class XamlDefTagNode : XamlAttributeNode
	{
		// Token: 0x06002301 RID: 8961 RVA: 0x000AC62C File Offset: 0x000AA82C
		internal XamlDefTagNode(int lineNumber, int linePosition, int depth, bool isEmptyElement, XmlReader xmlReader, string defTagName) : base(XamlNodeType.DefTag, lineNumber, linePosition, depth, defTagName)
		{
		}
	}
}
