using System;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x02000254 RID: 596
	internal class XamlClrEventNode : XamlAttributeNode
	{
		// Token: 0x060022F8 RID: 8952 RVA: 0x000AC577 File Offset: 0x000AA777
		internal XamlClrEventNode(int lineNumber, int linePosition, int depth, string eventName, MemberInfo eventMember, string value) : base(XamlNodeType.ClrEvent, lineNumber, linePosition, depth, value)
		{
		}
	}
}
