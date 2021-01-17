using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A5 RID: 1701
	internal sealed class BamlEndElementNode : BamlTreeNode
	{
		// Token: 0x06006E6C RID: 28268 RVA: 0x001FC050 File Offset: 0x001FA250
		internal BamlEndElementNode() : base(BamlNodeType.EndElement)
		{
		}

		// Token: 0x06006E6D RID: 28269 RVA: 0x001FC059 File Offset: 0x001FA259
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndElement();
		}

		// Token: 0x06006E6E RID: 28270 RVA: 0x001FC061 File Offset: 0x001FA261
		internal override BamlTreeNode Copy()
		{
			return new BamlEndElementNode();
		}
	}
}
