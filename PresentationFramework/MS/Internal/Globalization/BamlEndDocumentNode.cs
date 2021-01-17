using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A2 RID: 1698
	internal sealed class BamlEndDocumentNode : BamlTreeNode
	{
		// Token: 0x06006E57 RID: 28247 RVA: 0x001FBE7F File Offset: 0x001FA07F
		internal BamlEndDocumentNode() : base(BamlNodeType.EndDocument)
		{
		}

		// Token: 0x06006E58 RID: 28248 RVA: 0x001FBE88 File Offset: 0x001FA088
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndDocument();
		}

		// Token: 0x06006E59 RID: 28249 RVA: 0x001FBE90 File Offset: 0x001FA090
		internal override BamlTreeNode Copy()
		{
			return new BamlEndDocumentNode();
		}
	}
}
