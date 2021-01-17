using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A9 RID: 1705
	internal sealed class BamlPropertyNode : BamlStartComplexPropertyNode
	{
		// Token: 0x06006E81 RID: 28289 RVA: 0x001FC15C File Offset: 0x001FA35C
		internal BamlPropertyNode(string assemblyName, string ownerTypeFullName, string propertyName, string value, BamlAttributeUsage usage) : base(assemblyName, ownerTypeFullName, propertyName)
		{
			this._value = value;
			this._attributeUsage = usage;
			this._nodeType = BamlNodeType.Property;
		}

		// Token: 0x06006E82 RID: 28290 RVA: 0x001FC180 File Offset: 0x001FA380
		internal override void Serialize(BamlWriter writer)
		{
			if (!LocComments.IsLocCommentsProperty(this._ownerTypeFullName, this._propertyName) && !LocComments.IsLocLocalizabilityProperty(this._ownerTypeFullName, this._propertyName))
			{
				writer.WriteProperty(this._assemblyName, this._ownerTypeFullName, this._propertyName, this._value, this._attributeUsage);
			}
		}

		// Token: 0x06006E83 RID: 28291 RVA: 0x001FC1D7 File Offset: 0x001FA3D7
		internal override BamlTreeNode Copy()
		{
			return new BamlPropertyNode(this._assemblyName, this._ownerTypeFullName, this._propertyName, this._value, this._attributeUsage);
		}

		// Token: 0x17001A43 RID: 6723
		// (get) Token: 0x06006E84 RID: 28292 RVA: 0x001FC1FC File Offset: 0x001FA3FC
		// (set) Token: 0x06006E85 RID: 28293 RVA: 0x001FC204 File Offset: 0x001FA404
		internal string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17001A44 RID: 6724
		// (get) Token: 0x06006E86 RID: 28294 RVA: 0x001FC20D File Offset: 0x001FA40D
		// (set) Token: 0x06006E87 RID: 28295 RVA: 0x001FC215 File Offset: 0x001FA415
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x04003659 RID: 13913
		private string _value;

		// Token: 0x0400365A RID: 13914
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x0400365B RID: 13915
		private int _index;
	}
}
