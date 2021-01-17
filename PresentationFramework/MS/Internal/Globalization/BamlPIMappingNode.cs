using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006AF RID: 1711
	internal sealed class BamlPIMappingNode : BamlTreeNode
	{
		// Token: 0x06006E9B RID: 28315 RVA: 0x001FC3A4 File Offset: 0x001FA5A4
		internal BamlPIMappingNode(string xmlNamespace, string clrNamespace, string assemblyName) : base(BamlNodeType.PIMapping)
		{
			this._xmlNamespace = xmlNamespace;
			this._clrNamespace = clrNamespace;
			this._assemblyName = assemblyName;
		}

		// Token: 0x06006E9C RID: 28316 RVA: 0x001FC3C3 File Offset: 0x001FA5C3
		internal override void Serialize(BamlWriter writer)
		{
			writer.WritePIMapping(this._xmlNamespace, this._clrNamespace, this._assemblyName);
		}

		// Token: 0x06006E9D RID: 28317 RVA: 0x001FC3DD File Offset: 0x001FA5DD
		internal override BamlTreeNode Copy()
		{
			return new BamlPIMappingNode(this._xmlNamespace, this._clrNamespace, this._assemblyName);
		}

		// Token: 0x04003668 RID: 13928
		private string _xmlNamespace;

		// Token: 0x04003669 RID: 13929
		private string _clrNamespace;

		// Token: 0x0400366A RID: 13930
		private string _assemblyName;
	}
}
