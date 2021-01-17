using System;

namespace System.Windows.Markup
{
	// Token: 0x02000243 RID: 579
	internal class XamlPropertyComplexStartNode : XamlPropertyBaseNode
	{
		// Token: 0x060022AD RID: 8877 RVA: 0x000AC074 File Offset: 0x000AA274
		internal XamlPropertyComplexStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyComplexStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000AC094 File Offset: 0x000AA294
		internal XamlPropertyComplexStartNode(XamlNodeType token, int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(token, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
