using System;

namespace System.Windows.Markup
{
	// Token: 0x02000260 RID: 608
	internal class XamlKeyElementStartNode : XamlElementStartNode
	{
		// Token: 0x0600230B RID: 8971 RVA: 0x000AC6D8 File Offset: 0x000AA8D8
		internal XamlKeyElementStartNode(int lineNumber, int linePosition, int depth, string assemblyName, string typeFullName, Type elementType, Type serializerType) : base(XamlNodeType.KeyElementStart, lineNumber, linePosition, depth, assemblyName, typeFullName, elementType, serializerType, false, false, false)
		{
		}
	}
}
