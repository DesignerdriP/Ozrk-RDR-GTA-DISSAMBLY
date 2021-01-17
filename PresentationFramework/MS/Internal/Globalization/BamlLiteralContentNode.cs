using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AA RID: 1706
	internal sealed class BamlLiteralContentNode : BamlTreeNode
	{
		// Token: 0x06006E88 RID: 28296 RVA: 0x001FC21E File Offset: 0x001FA41E
		internal BamlLiteralContentNode(string literalContent) : base(BamlNodeType.LiteralContent)
		{
			this._literalContent = literalContent;
		}

		// Token: 0x06006E89 RID: 28297 RVA: 0x001FC22F File Offset: 0x001FA42F
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteLiteralContent(this._literalContent);
		}

		// Token: 0x06006E8A RID: 28298 RVA: 0x001FC23D File Offset: 0x001FA43D
		internal override BamlTreeNode Copy()
		{
			return new BamlLiteralContentNode(this._literalContent);
		}

		// Token: 0x17001A45 RID: 6725
		// (get) Token: 0x06006E8B RID: 28299 RVA: 0x001FC24A File Offset: 0x001FA44A
		// (set) Token: 0x06006E8C RID: 28300 RVA: 0x001FC252 File Offset: 0x001FA452
		internal string Content
		{
			get
			{
				return this._literalContent;
			}
			set
			{
				this._literalContent = value;
			}
		}

		// Token: 0x0400365C RID: 13916
		private string _literalContent;
	}
}
