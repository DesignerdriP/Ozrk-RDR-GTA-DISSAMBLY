using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A8 RID: 1704
	internal sealed class BamlEndComplexPropertyNode : BamlTreeNode
	{
		// Token: 0x06006E7E RID: 28286 RVA: 0x001FC143 File Offset: 0x001FA343
		internal BamlEndComplexPropertyNode() : base(BamlNodeType.EndComplexProperty)
		{
		}

		// Token: 0x06006E7F RID: 28287 RVA: 0x001FC14D File Offset: 0x001FA34D
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteEndComplexProperty();
		}

		// Token: 0x06006E80 RID: 28288 RVA: 0x001FC155 File Offset: 0x001FA355
		internal override BamlTreeNode Copy()
		{
			return new BamlEndComplexPropertyNode();
		}
	}
}
