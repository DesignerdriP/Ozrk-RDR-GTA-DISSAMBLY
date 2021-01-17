using System;

namespace System.Windows.Markup
{
	// Token: 0x0200025D RID: 605
	internal class XamlDefAttributeNode : XamlAttributeNode
	{
		// Token: 0x06002302 RID: 8962 RVA: 0x000AC63B File Offset: 0x000AA83B
		internal XamlDefAttributeNode(int lineNumber, int linePosition, int depth, string name, string value) : base(XamlNodeType.DefAttribute, lineNumber, linePosition, depth, value)
		{
			this._attributeUsage = BamlAttributeUsage.Default;
			this._name = name;
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x000AC659 File Offset: 0x000AA859
		internal XamlDefAttributeNode(int lineNumber, int linePosition, int depth, string name, string value, BamlAttributeUsage bamlAttributeUsage) : base(XamlNodeType.DefAttribute, lineNumber, linePosition, depth, value)
		{
			this._attributeUsage = bamlAttributeUsage;
			this._name = name;
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002304 RID: 8964 RVA: 0x000AC678 File Offset: 0x000AA878
		internal string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002305 RID: 8965 RVA: 0x000AC680 File Offset: 0x000AA880
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return this._attributeUsage;
			}
		}

		// Token: 0x04001A5F RID: 6751
		private BamlAttributeUsage _attributeUsage;

		// Token: 0x04001A60 RID: 6752
		private string _name;
	}
}
