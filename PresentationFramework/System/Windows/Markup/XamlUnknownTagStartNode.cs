using System;

namespace System.Windows.Markup
{
	// Token: 0x0200024F RID: 591
	internal class XamlUnknownTagStartNode : XamlAttributeNode
	{
		// Token: 0x060022E9 RID: 8937 RVA: 0x000AC482 File Offset: 0x000AA682
		internal XamlUnknownTagStartNode(int lineNumber, int linePosition, int depth, string xmlNamespace, string value) : base(XamlNodeType.UnknownTagStart, lineNumber, linePosition, depth, value)
		{
			this._xmlNamespace = xmlNamespace;
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x060022EA RID: 8938 RVA: 0x000AC499 File Offset: 0x000AA699
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x04001A52 RID: 6738
		private string _xmlNamespace;
	}
}
