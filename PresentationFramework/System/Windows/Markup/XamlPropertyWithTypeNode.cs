using System;

namespace System.Windows.Markup
{
	// Token: 0x02000247 RID: 583
	internal class XamlPropertyWithTypeNode : XamlPropertyBaseNode
	{
		// Token: 0x060022CD RID: 8909 RVA: 0x000AC29C File Offset: 0x000AA49C
		internal XamlPropertyWithTypeNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName, string valueTypeFullName, string valueAssemblyName, Type valueElementType, string valueSerializerTypeFullName, string valueSerializerTypeAssemblyName) : base(XamlNodeType.PropertyWithType, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
			this._valueTypeFullname = valueTypeFullName;
			this._valueTypeAssemblyName = valueAssemblyName;
			this._valueElementType = valueElementType;
			this._valueSerializerTypeFullName = valueSerializerTypeFullName;
			this._valueSerializerTypeAssemblyName = valueSerializerTypeAssemblyName;
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060022CE RID: 8910 RVA: 0x000AC2E4 File Offset: 0x000AA4E4
		internal string ValueTypeFullName
		{
			get
			{
				return this._valueTypeFullname;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x060022CF RID: 8911 RVA: 0x000AC2EC File Offset: 0x000AA4EC
		internal string ValueTypeAssemblyName
		{
			get
			{
				return this._valueTypeAssemblyName;
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x060022D0 RID: 8912 RVA: 0x000AC2F4 File Offset: 0x000AA4F4
		internal Type ValueElementType
		{
			get
			{
				return this._valueElementType;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x060022D1 RID: 8913 RVA: 0x000AC2FC File Offset: 0x000AA4FC
		internal string ValueSerializerTypeFullName
		{
			get
			{
				return this._valueSerializerTypeFullName;
			}
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060022D2 RID: 8914 RVA: 0x000AC304 File Offset: 0x000AA504
		internal string ValueSerializerTypeAssemblyName
		{
			get
			{
				return this._valueSerializerTypeAssemblyName;
			}
		}

		// Token: 0x04001A40 RID: 6720
		private string _valueTypeFullname;

		// Token: 0x04001A41 RID: 6721
		private string _valueTypeAssemblyName;

		// Token: 0x04001A42 RID: 6722
		private Type _valueElementType;

		// Token: 0x04001A43 RID: 6723
		private string _valueSerializerTypeFullName;

		// Token: 0x04001A44 RID: 6724
		private string _valueSerializerTypeAssemblyName;
	}
}
