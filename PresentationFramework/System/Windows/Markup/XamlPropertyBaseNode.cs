using System;
using System.Diagnostics;

namespace System.Windows.Markup
{
	// Token: 0x02000242 RID: 578
	[DebuggerDisplay("Prop:{_typeFullName}.{_propName}")]
	internal class XamlPropertyBaseNode : XamlNode
	{
		// Token: 0x060022A6 RID: 8870 RVA: 0x000ABFA8 File Offset: 0x000AA1A8
		internal XamlPropertyBaseNode(XamlNodeType token, int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(token, lineNumber, linePosition, depth)
		{
			if (typeFullName == null)
			{
				throw new ArgumentNullException("typeFullName");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			this._propertyMember = propertyMember;
			this._assemblyName = assemblyName;
			this._typeFullName = typeFullName;
			this._propName = propertyName;
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x000ABFFE File Offset: 0x000AA1FE
		internal string AssemblyName
		{
			get
			{
				return this._assemblyName;
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x000AC006 File Offset: 0x000AA206
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x000AC00E File Offset: 0x000AA20E
		internal string PropName
		{
			get
			{
				return this._propName;
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x060022AA RID: 8874 RVA: 0x000AC016 File Offset: 0x000AA216
		internal Type PropDeclaringType
		{
			get
			{
				if (this._declaringType == null && this._propertyMember != null)
				{
					this._declaringType = XamlTypeMapper.GetDeclaringType(this._propertyMember);
				}
				return this._declaringType;
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x000AC045 File Offset: 0x000AA245
		internal Type PropValidType
		{
			get
			{
				if (this._validType == null)
				{
					this._validType = XamlTypeMapper.GetPropertyType(this._propertyMember);
				}
				return this._validType;
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x000AC06C File Offset: 0x000AA26C
		internal object PropertyMember
		{
			get
			{
				return this._propertyMember;
			}
		}

		// Token: 0x04001A29 RID: 6697
		private object _propertyMember;

		// Token: 0x04001A2A RID: 6698
		private string _assemblyName;

		// Token: 0x04001A2B RID: 6699
		private string _typeFullName;

		// Token: 0x04001A2C RID: 6700
		private string _propName;

		// Token: 0x04001A2D RID: 6701
		private Type _validType;

		// Token: 0x04001A2E RID: 6702
		private Type _declaringType;
	}
}
