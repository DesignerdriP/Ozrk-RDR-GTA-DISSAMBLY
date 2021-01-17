using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000421 RID: 1057
	internal class TextTreeObjectNode : TextTreeNode
	{
		// Token: 0x06003D58 RID: 15704 RVA: 0x0011C115 File Offset: 0x0011A315
		internal TextTreeObjectNode(DependencyObject embeddedElement)
		{
			this._embeddedElement = embeddedElement;
			this._symbolOffsetCache = -1;
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x0011C12C File Offset: 0x0011A32C
		internal override TextTreeNode Clone()
		{
			return new TextTreeObjectNode(this._embeddedElement);
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x00094B24 File Offset: 0x00092D24
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			return TextPointerContext.EmbeddedElement;
		}

		// Token: 0x17000F2B RID: 3883
		// (get) Token: 0x06003D5B RID: 15707 RVA: 0x0011C146 File Offset: 0x0011A346
		// (set) Token: 0x06003D5C RID: 15708 RVA: 0x0011C14E File Offset: 0x0011A34E
		internal override SplayTreeNode ParentNode
		{
			get
			{
				return this._parentNode;
			}
			set
			{
				this._parentNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F2C RID: 3884
		// (get) Token: 0x06003D5D RID: 15709 RVA: 0x0000C238 File Offset: 0x0000A438
		// (set) Token: 0x06003D5E RID: 15710 RVA: 0x0011C15C File Offset: 0x0011A35C
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return null;
			}
			set
			{
				Invariant.Assert(false, "Can't set contained node on a TextTreeObjectNode!");
			}
		}

		// Token: 0x17000F2D RID: 3885
		// (get) Token: 0x06003D5F RID: 15711 RVA: 0x0011C169 File Offset: 0x0011A369
		// (set) Token: 0x06003D60 RID: 15712 RVA: 0x0011C171 File Offset: 0x0011A371
		internal override int LeftSymbolCount
		{
			get
			{
				return this._leftSymbolCount;
			}
			set
			{
				this._leftSymbolCount = value;
			}
		}

		// Token: 0x17000F2E RID: 3886
		// (get) Token: 0x06003D61 RID: 15713 RVA: 0x0011C17A File Offset: 0x0011A37A
		// (set) Token: 0x06003D62 RID: 15714 RVA: 0x0011C182 File Offset: 0x0011A382
		internal override int LeftCharCount
		{
			get
			{
				return this._leftCharCount;
			}
			set
			{
				this._leftCharCount = value;
			}
		}

		// Token: 0x17000F2F RID: 3887
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x0011C18B File Offset: 0x0011A38B
		// (set) Token: 0x06003D64 RID: 15716 RVA: 0x0011C193 File Offset: 0x0011A393
		internal override SplayTreeNode LeftChildNode
		{
			get
			{
				return this._leftChildNode;
			}
			set
			{
				this._leftChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F30 RID: 3888
		// (get) Token: 0x06003D65 RID: 15717 RVA: 0x0011C1A1 File Offset: 0x0011A3A1
		// (set) Token: 0x06003D66 RID: 15718 RVA: 0x0011C1A9 File Offset: 0x0011A3A9
		internal override SplayTreeNode RightChildNode
		{
			get
			{
				return this._rightChildNode;
			}
			set
			{
				this._rightChildNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x06003D67 RID: 15719 RVA: 0x0011C1B7 File Offset: 0x0011A3B7
		// (set) Token: 0x06003D68 RID: 15720 RVA: 0x0011C1BF File Offset: 0x0011A3BF
		internal override uint Generation
		{
			get
			{
				return this._generation;
			}
			set
			{
				this._generation = value;
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x06003D69 RID: 15721 RVA: 0x0011C1C8 File Offset: 0x0011A3C8
		// (set) Token: 0x06003D6A RID: 15722 RVA: 0x0011C1D0 File Offset: 0x0011A3D0
		internal override int SymbolOffsetCache
		{
			get
			{
				return this._symbolOffsetCache;
			}
			set
			{
				this._symbolOffsetCache = value;
			}
		}

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x00016748 File Offset: 0x00014948
		// (set) Token: 0x06003D6C RID: 15724 RVA: 0x0011C1D9 File Offset: 0x0011A3D9
		internal override int SymbolCount
		{
			get
			{
				return 1;
			}
			set
			{
				Invariant.Assert(false, "Can't set SymbolCount on TextTreeObjectNode!");
			}
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x00016748 File Offset: 0x00014948
		// (set) Token: 0x06003D6E RID: 15726 RVA: 0x0011C1E6 File Offset: 0x0011A3E6
		internal override int IMECharCount
		{
			get
			{
				return 1;
			}
			set
			{
				Invariant.Assert(false, "Can't set CharCount on TextTreeObjectNode!");
			}
		}

		// Token: 0x17000F35 RID: 3893
		// (get) Token: 0x06003D6F RID: 15727 RVA: 0x0011C1F3 File Offset: 0x0011A3F3
		// (set) Token: 0x06003D70 RID: 15728 RVA: 0x0011C200 File Offset: 0x0011A400
		internal override bool BeforeStartReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.BeforeStart) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.BeforeStart;
			}
		}

		// Token: 0x17000F36 RID: 3894
		// (get) Token: 0x06003D71 RID: 15729 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D72 RID: 15730 RVA: 0x0011C216 File Offset: 0x0011A416
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Object nodes don't have an AfterStart edge!");
			}
		}

		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06003D73 RID: 15731 RVA: 0x0000B02A File Offset: 0x0000922A
		// (set) Token: 0x06003D74 RID: 15732 RVA: 0x0011C223 File Offset: 0x0011A423
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return false;
			}
			set
			{
				Invariant.Assert(false, "Object nodes don't have a BeforeEnd edge!");
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06003D75 RID: 15733 RVA: 0x0011C230 File Offset: 0x0011A430
		// (set) Token: 0x06003D76 RID: 15734 RVA: 0x0011C23D File Offset: 0x0011A43D
		internal override bool AfterEndReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.AfterEnd) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.AfterEnd;
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06003D77 RID: 15735 RVA: 0x0011C253 File Offset: 0x0011A453
		internal DependencyObject EmbeddedElement
		{
			get
			{
				return this._embeddedElement;
			}
		}

		// Token: 0x04002660 RID: 9824
		private int _leftSymbolCount;

		// Token: 0x04002661 RID: 9825
		private int _leftCharCount;

		// Token: 0x04002662 RID: 9826
		private TextTreeNode _parentNode;

		// Token: 0x04002663 RID: 9827
		private TextTreeNode _leftChildNode;

		// Token: 0x04002664 RID: 9828
		private TextTreeNode _rightChildNode;

		// Token: 0x04002665 RID: 9829
		private uint _generation;

		// Token: 0x04002666 RID: 9830
		private int _symbolOffsetCache;

		// Token: 0x04002667 RID: 9831
		private ElementEdge _edgeReferenceCounts;

		// Token: 0x04002668 RID: 9832
		private readonly DependencyObject _embeddedElement;
	}
}
