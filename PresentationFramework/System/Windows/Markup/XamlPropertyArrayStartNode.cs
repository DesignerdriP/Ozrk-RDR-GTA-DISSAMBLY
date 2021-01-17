using System;

namespace System.Windows.Markup
{
	// Token: 0x02000255 RID: 597
	internal class XamlPropertyArrayStartNode : XamlPropertyComplexStartNode
	{
		// Token: 0x060022F9 RID: 8953 RVA: 0x000AC588 File Offset: 0x000AA788
		internal XamlPropertyArrayStartNode(int lineNumber, int linePosition, int depth, object propertyMember, string assemblyName, string typeFullName, string propertyName) : base(XamlNodeType.PropertyArrayStart, lineNumber, linePosition, depth, propertyMember, assemblyName, typeFullName, propertyName)
		{
		}
	}
}
