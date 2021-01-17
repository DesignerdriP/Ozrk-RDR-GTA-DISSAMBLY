using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AB RID: 1707
	internal sealed class BamlTextNode : BamlTreeNode
	{
		// Token: 0x06006E8D RID: 28301 RVA: 0x001FC25B File Offset: 0x001FA45B
		internal BamlTextNode(string text) : this(text, null, null)
		{
		}

		// Token: 0x06006E8E RID: 28302 RVA: 0x001FC266 File Offset: 0x001FA466
		internal BamlTextNode(string text, string typeConverterAssemblyName, string typeConverterName) : base(BamlNodeType.Text)
		{
			this._content = text;
			this._typeConverterAssemblyName = typeConverterAssemblyName;
			this._typeConverterName = typeConverterName;
		}

		// Token: 0x06006E8F RID: 28303 RVA: 0x001FC285 File Offset: 0x001FA485
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteText(this._content, this._typeConverterAssemblyName, this._typeConverterName);
		}

		// Token: 0x06006E90 RID: 28304 RVA: 0x001FC29F File Offset: 0x001FA49F
		internal override BamlTreeNode Copy()
		{
			return new BamlTextNode(this._content, this._typeConverterAssemblyName, this._typeConverterName);
		}

		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x06006E91 RID: 28305 RVA: 0x001FC2B8 File Offset: 0x001FA4B8
		internal string Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x0400365D RID: 13917
		private string _content;

		// Token: 0x0400365E RID: 13918
		private string _typeConverterAssemblyName;

		// Token: 0x0400365F RID: 13919
		private string _typeConverterName;
	}
}
