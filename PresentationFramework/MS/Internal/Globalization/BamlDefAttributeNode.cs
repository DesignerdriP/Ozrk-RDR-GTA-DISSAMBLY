using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AE RID: 1710
	internal sealed class BamlDefAttributeNode : BamlTreeNode
	{
		// Token: 0x06006E98 RID: 28312 RVA: 0x001FC365 File Offset: 0x001FA565
		internal BamlDefAttributeNode(string name, string value) : base(BamlNodeType.DefAttribute)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x06006E99 RID: 28313 RVA: 0x001FC37D File Offset: 0x001FA57D
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteDefAttribute(this._name, this._value);
		}

		// Token: 0x06006E9A RID: 28314 RVA: 0x001FC391 File Offset: 0x001FA591
		internal override BamlTreeNode Copy()
		{
			return new BamlDefAttributeNode(this._name, this._value);
		}

		// Token: 0x04003666 RID: 13926
		private string _name;

		// Token: 0x04003667 RID: 13927
		private string _value;
	}
}
