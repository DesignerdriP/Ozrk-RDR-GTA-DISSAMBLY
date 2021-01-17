using System;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A3 RID: 1699
	internal sealed class BamlConnectionIdNode : BamlTreeNode
	{
		// Token: 0x06006E5A RID: 28250 RVA: 0x001FBE97 File Offset: 0x001FA097
		internal BamlConnectionIdNode(int connectionId) : base(BamlNodeType.ConnectionId)
		{
			this._connectionId = connectionId;
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x001FBEA7 File Offset: 0x001FA0A7
		internal override void Serialize(BamlWriter writer)
		{
			writer.WriteConnectionId(this._connectionId);
		}

		// Token: 0x06006E5C RID: 28252 RVA: 0x001FBEB5 File Offset: 0x001FA0B5
		internal override BamlTreeNode Copy()
		{
			return new BamlConnectionIdNode(this._connectionId);
		}

		// Token: 0x04003647 RID: 13895
		private int _connectionId;
	}
}
