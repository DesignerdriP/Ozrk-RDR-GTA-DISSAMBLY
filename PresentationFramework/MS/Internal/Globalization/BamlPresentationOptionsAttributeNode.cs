using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B3 RID: 1715
	internal sealed class BamlPresentationOptionsAttributeNode : BamlTreeNode
	{
		// Token: 0x06006EA7 RID: 28327 RVA: 0x001FC479 File Offset: 0x001FA679
		internal BamlPresentationOptionsAttributeNode(string name, string value) : base(BamlNodeType.PresentationOptionsAttribute)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x06006EA8 RID: 28328 RVA: 0x001FC491 File Offset: 0x001FA691
		internal override void Serialize(BamlWriter writer)
		{
			writer.WritePresentationOptionsAttribute(this._name, this._value);
		}

		// Token: 0x06006EA9 RID: 28329 RVA: 0x001FC4A5 File Offset: 0x001FA6A5
		internal override BamlTreeNode Copy()
		{
			return new BamlPresentationOptionsAttributeNode(this._name, this._value);
		}

		// Token: 0x0400366E RID: 13934
		private string _name;

		// Token: 0x0400366F RID: 13935
		private string _value;
	}
}
