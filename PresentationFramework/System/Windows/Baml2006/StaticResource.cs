using System;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015C RID: 348
	internal class StaticResource
	{
		// Token: 0x06000F98 RID: 3992 RVA: 0x0003C89E File Offset: 0x0003AA9E
		public StaticResource(XamlType type, XamlSchemaContext schemaContext)
		{
			this.ResourceNodeList = new XamlNodeList(schemaContext, 8);
			this.ResourceNodeList.Writer.WriteStartObject(type);
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000F99 RID: 3993 RVA: 0x0003C8C4 File Offset: 0x0003AAC4
		// (set) Token: 0x06000F9A RID: 3994 RVA: 0x0003C8CC File Offset: 0x0003AACC
		public XamlNodeList ResourceNodeList { get; private set; }
	}
}
