using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B0 RID: 1712
	internal sealed class BamlStartConstructorNode : BamlTreeNode
	{
		// Token: 0x06006E9E RID: 28318 RVA: 0x001FC3F6 File Offset: 0x001FA5F6
		internal BamlStartConstructorNode() : base(BamlNodeType.StartConstructor)
		{
		}

		// Token: 0x06006E9F RID: 28319 RVA: 0x001FC400 File Offset: 0x001FA600
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteStartConstructor();
		}

		// Token: 0x06006EA0 RID: 28320 RVA: 0x001FC408 File Offset: 0x001FA608
		internal override BamlTreeNode Copy()
		{
			return new BamlStartConstructorNode();
		}
	}
}
