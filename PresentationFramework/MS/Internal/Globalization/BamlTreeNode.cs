using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace MS.Internal.Globalization
{
	// Token: 0x020006A0 RID: 1696
	internal abstract class BamlTreeNode
	{
		// Token: 0x06006E3F RID: 28223 RVA: 0x001FBD3E File Offset: 0x001F9F3E
		internal BamlTreeNode(BamlNodeType type)
		{
			this.NodeType = type;
		}

		// Token: 0x06006E40 RID: 28224 RVA: 0x001FBD4D File Offset: 0x001F9F4D
		internal void AddChild(BamlTreeNode child)
		{
			if (this._children == null)
			{
				this._children = new List<BamlTreeNode>();
			}
			this._children.Add(child);
			child.Parent = this;
		}

		// Token: 0x06006E41 RID: 28225
		internal abstract BamlTreeNode Copy();

		// Token: 0x06006E42 RID: 28226
		internal abstract void Serialize(BamlWriter writer);

		// Token: 0x17001A2D RID: 6701
		// (get) Token: 0x06006E43 RID: 28227 RVA: 0x001FBD75 File Offset: 0x001F9F75
		// (set) Token: 0x06006E44 RID: 28228 RVA: 0x001FBD7D File Offset: 0x001F9F7D
		internal BamlNodeType NodeType
		{
			get
			{
				return this._nodeType;
			}
			set
			{
				this._nodeType = value;
			}
		}

		// Token: 0x17001A2E RID: 6702
		// (get) Token: 0x06006E45 RID: 28229 RVA: 0x001FBD86 File Offset: 0x001F9F86
		// (set) Token: 0x06006E46 RID: 28230 RVA: 0x001FBD8E File Offset: 0x001F9F8E
		internal List<BamlTreeNode> Children
		{
			get
			{
				return this._children;
			}
			set
			{
				this._children = value;
			}
		}

		// Token: 0x17001A2F RID: 6703
		// (get) Token: 0x06006E47 RID: 28231 RVA: 0x001FBD97 File Offset: 0x001F9F97
		// (set) Token: 0x06006E48 RID: 28232 RVA: 0x001FBD9F File Offset: 0x001F9F9F
		internal BamlTreeNode Parent
		{
			get
			{
				return this._parent;
			}
			set
			{
				this._parent = value;
			}
		}

		// Token: 0x17001A30 RID: 6704
		// (get) Token: 0x06006E49 RID: 28233 RVA: 0x001FBDA8 File Offset: 0x001F9FA8
		// (set) Token: 0x06006E4A RID: 28234 RVA: 0x001FBDB5 File Offset: 0x001F9FB5
		internal bool Formatted
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.ContentFormatted) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.ContentFormatted;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.ContentFormatted;
			}
		}

		// Token: 0x17001A31 RID: 6705
		// (get) Token: 0x06006E4B RID: 28235 RVA: 0x001FBDDB File Offset: 0x001F9FDB
		// (set) Token: 0x06006E4C RID: 28236 RVA: 0x001FBDE8 File Offset: 0x001F9FE8
		internal bool Visited
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.NodeVisited) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.NodeVisited;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.NodeVisited;
			}
		}

		// Token: 0x17001A32 RID: 6706
		// (get) Token: 0x06006E4D RID: 28237 RVA: 0x001FBE0E File Offset: 0x001FA00E
		// (set) Token: 0x06006E4E RID: 28238 RVA: 0x001FBE1B File Offset: 0x001FA01B
		internal bool Unidentifiable
		{
			get
			{
				return (this._state & BamlTreeNode.BamlTreeNodeState.Unidentifiable) > BamlTreeNode.BamlTreeNodeState.None;
			}
			set
			{
				if (value)
				{
					this._state |= BamlTreeNode.BamlTreeNodeState.Unidentifiable;
					return;
				}
				this._state &= ~BamlTreeNode.BamlTreeNodeState.Unidentifiable;
			}
		}

		// Token: 0x04003643 RID: 13891
		protected BamlNodeType _nodeType;

		// Token: 0x04003644 RID: 13892
		protected List<BamlTreeNode> _children;

		// Token: 0x04003645 RID: 13893
		protected BamlTreeNode _parent;

		// Token: 0x04003646 RID: 13894
		private BamlTreeNode.BamlTreeNodeState _state;

		// Token: 0x02000B28 RID: 2856
		[Flags]
		private enum BamlTreeNodeState : byte
		{
			// Token: 0x04004A75 RID: 19061
			None = 0,
			// Token: 0x04004A76 RID: 19062
			ContentFormatted = 1,
			// Token: 0x04004A77 RID: 19063
			NodeVisited = 2,
			// Token: 0x04004A78 RID: 19064
			Unidentifiable = 4
		}
	}
}
