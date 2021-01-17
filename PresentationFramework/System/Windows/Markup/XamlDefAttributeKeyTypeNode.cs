using System;

namespace System.Windows.Markup
{
	// Token: 0x0200025E RID: 606
	internal class XamlDefAttributeKeyTypeNode : XamlAttributeNode
	{
		// Token: 0x06002306 RID: 8966 RVA: 0x000AC688 File Offset: 0x000AA888
		internal XamlDefAttributeKeyTypeNode(int lineNumber, int linePosition, int depth, string value, string assemblyName, Type valueType) : base(XamlNodeType.DefKeyTypeAttribute, lineNumber, linePosition, depth, value)
		{
			this._assemblyName = assemblyName;
			this._valueType = valueType;
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002307 RID: 8967 RVA: 0x000AC6A7 File Offset: 0x000AA8A7
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002308 RID: 8968 RVA: 0x000AC6AF File Offset: 0x000AA8AF
		internal Type ValueType
		{
			get
			{
				return this._valueType;
			}
		}

		// Token: 0x04001A61 RID: 6753
		private string _assemblyName;

		// Token: 0x04001A62 RID: 6754
		private Type _valueType;
	}
}
