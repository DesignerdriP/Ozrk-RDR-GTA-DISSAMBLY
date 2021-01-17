using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B2 RID: 1714
	internal sealed class BamlContentPropertyNode : BamlTreeNode
	{
		// Token: 0x06006EA4 RID: 28324 RVA: 0x001FC428 File Offset: 0x001FA628
		internal BamlContentPropertyNode(string assemblyName, string typeFullName, string propertyName) : base(BamlNodeType.ContentProperty)
		{
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._propertyName = propertyName;
		}

		// Token: 0x06006EA5 RID: 28325 RVA: 0x001FC446 File Offset: 0x001FA646
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteContentProperty(this._assemblyName, this._typeFullName, this._propertyName);
		}

		// Token: 0x06006EA6 RID: 28326 RVA: 0x001FC460 File Offset: 0x001FA660
		internal override BamlTreeNode Copy()
		{
			return new BamlContentPropertyNode(this._assemblyName, this._typeFullName, this._propertyName);
		}

		// Token: 0x0400366B RID: 13931
		private string _assemblyName;

		// Token: 0x0400366C RID: 13932
		private string _typeFullName;

		// Token: 0x0400366D RID: 13933
		private string _propertyName;
	}
}
