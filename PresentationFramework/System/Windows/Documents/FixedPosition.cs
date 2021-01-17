using System;

namespace System.Windows.Documents
{
	// Token: 0x02000357 RID: 855
	internal struct FixedPosition
	{
		// Token: 0x06002DA5 RID: 11685 RVA: 0x000CD864 File Offset: 0x000CBA64
		internal FixedPosition(FixedNode fixedNode, int offset)
		{
			this._fixedNode = fixedNode;
			this._offset = offset;
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06002DA6 RID: 11686 RVA: 0x000CD874 File Offset: 0x000CBA74
		internal int Page
		{
			get
			{
				return this._fixedNode.Page;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06002DA7 RID: 11687 RVA: 0x000CD88F File Offset: 0x000CBA8F
		internal FixedNode Node
		{
			get
			{
				return this._fixedNode;
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06002DA8 RID: 11688 RVA: 0x000CD897 File Offset: 0x000CBA97
		internal int Offset
		{
			get
			{
				return this._offset;
			}
		}

		// Token: 0x04001DBA RID: 7610
		private readonly FixedNode _fixedNode;

		// Token: 0x04001DBB RID: 7611
		private readonly int _offset;
	}
}
