using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B1 RID: 1713
	internal sealed class BamlEndConstructorNode : BamlTreeNode
	{
		// Token: 0x06006EA1 RID: 28321 RVA: 0x001FC40F File Offset: 0x001FA60F
		internal BamlEndConstructorNode() : base(BamlNodeType.EndConstructor)
		{
		}

		// Token: 0x06006EA2 RID: 28322 RVA: 0x001FC419 File Offset: 0x001FA619
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndConstructor();
		}

		// Token: 0x06006EA3 RID: 28323 RVA: 0x001FC421 File Offset: 0x001FA621
		internal override BamlTreeNode Copy()
		{
			return new BamlEndConstructorNode();
		}
	}
}
