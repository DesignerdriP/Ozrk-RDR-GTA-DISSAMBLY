using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A6 RID: 1702
	internal sealed class BamlXmlnsPropertyNode : BamlTreeNode
	{
		// Token: 0x06006E6F RID: 28271 RVA: 0x001FC068 File Offset: 0x001FA268
		internal BamlXmlnsPropertyNode(string prefix, string xmlns) : base(BamlNodeType.XmlnsProperty)
		{
			this._prefix = prefix;
			this._xmlns = xmlns;
		}

		// Token: 0x06006E70 RID: 28272 RVA: 0x001FC07F File Offset: 0x001FA27F
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteXmlnsProperty(this._prefix, this._xmlns);
		}

		// Token: 0x06006E71 RID: 28273 RVA: 0x001FC093 File Offset: 0x001FA293
		internal override BamlTreeNode Copy()
		{
			return new BamlXmlnsPropertyNode(this._prefix, this._xmlns);
		}

		// Token: 0x04003651 RID: 13905
		private string _prefix;

		// Token: 0x04003652 RID: 13906
		private string _xmlns;
	}
}
