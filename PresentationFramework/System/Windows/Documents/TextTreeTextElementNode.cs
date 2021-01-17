using System;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000427 RID: 1063
	internal class TextTreeTextElementNode : TextTreeNode
	{
		// Token: 0x06003DE3 RID: 15843 RVA: 0x0011CF15 File Offset: 0x0011B115
		internal TextTreeTextElementNode()
		{
			this._symbolOffsetCache = -1;
		}

		// Token: 0x06003DE4 RID: 15844 RVA: 0x0011CF24 File Offset: 0x0011B124
		internal override TextTreeNode Clone()
		{
			return new TextTreeTextElementNode
			{
				_symbolCount = this._symbolCount,
				_imeCharCount = this._imeCharCount,
				_textElement = this._textElement
			};
		}

		// Token: 0x06003DE5 RID: 15845 RVA: 0x0011CF5C File Offset: 0x0011B15C
		internal override TextPointerContext GetPointerContext(LogicalDirection direction)
		{
			if (direction != LogicalDirection.Forward)
			{
				return TextPointerContext.ElementEnd;
			}
			return TextPointerContext.ElementStart;
		}

		// Token: 0x17000F66 RID: 3942
		// (get) Token: 0x06003DE6 RID: 15846 RVA: 0x0011CF65 File Offset: 0x0011B165
		// (set) Token: 0x06003DE7 RID: 15847 RVA: 0x0011CF6D File Offset: 0x0011B16D
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

		// Token: 0x17000F67 RID: 3943
		// (get) Token: 0x06003DE8 RID: 15848 RVA: 0x0011CF7B File Offset: 0x0011B17B
		// (set) Token: 0x06003DE9 RID: 15849 RVA: 0x0011CF83 File Offset: 0x0011B183
		internal override SplayTreeNode ContainedNode
		{
			get
			{
				return this._containedNode;
			}
			set
			{
				this._containedNode = (TextTreeNode)value;
			}
		}

		// Token: 0x17000F68 RID: 3944
		// (get) Token: 0x06003DEA RID: 15850 RVA: 0x0011CF91 File Offset: 0x0011B191
		// (set) Token: 0x06003DEB RID: 15851 RVA: 0x0011CF99 File Offset: 0x0011B199
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

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x06003DEC RID: 15852 RVA: 0x0011CFA2 File Offset: 0x0011B1A2
		// (set) Token: 0x06003DED RID: 15853 RVA: 0x0011CFAA File Offset: 0x0011B1AA
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

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06003DEE RID: 15854 RVA: 0x0011CFB3 File Offset: 0x0011B1B3
		// (set) Token: 0x06003DEF RID: 15855 RVA: 0x0011CFBB File Offset: 0x0011B1BB
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

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x0011CFC9 File Offset: 0x0011B1C9
		// (set) Token: 0x06003DF1 RID: 15857 RVA: 0x0011CFD1 File Offset: 0x0011B1D1
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

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x0011CFDF File Offset: 0x0011B1DF
		// (set) Token: 0x06003DF3 RID: 15859 RVA: 0x0011CFE7 File Offset: 0x0011B1E7
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

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x0011CFF0 File Offset: 0x0011B1F0
		// (set) Token: 0x06003DF5 RID: 15861 RVA: 0x0011CFF8 File Offset: 0x0011B1F8
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

		// Token: 0x17000F6E RID: 3950
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x0011D001 File Offset: 0x0011B201
		// (set) Token: 0x06003DF7 RID: 15863 RVA: 0x0011D009 File Offset: 0x0011B209
		internal override int SymbolCount
		{
			get
			{
				return this._symbolCount;
			}
			set
			{
				this._symbolCount = value;
			}
		}

		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x06003DF8 RID: 15864 RVA: 0x0011D012 File Offset: 0x0011B212
		// (set) Token: 0x06003DF9 RID: 15865 RVA: 0x0011D01A File Offset: 0x0011B21A
		internal override int IMECharCount
		{
			get
			{
				return this._imeCharCount;
			}
			set
			{
				this._imeCharCount = value;
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x0011D023 File Offset: 0x0011B223
		// (set) Token: 0x06003DFB RID: 15867 RVA: 0x0011D030 File Offset: 0x0011B230
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

		// Token: 0x17000F71 RID: 3953
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x0011D046 File Offset: 0x0011B246
		// (set) Token: 0x06003DFD RID: 15869 RVA: 0x0011D053 File Offset: 0x0011B253
		internal override bool AfterStartReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.AfterStart) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.AfterStart;
			}
		}

		// Token: 0x17000F72 RID: 3954
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x0011D069 File Offset: 0x0011B269
		// (set) Token: 0x06003DFF RID: 15871 RVA: 0x0011D076 File Offset: 0x0011B276
		internal override bool BeforeEndReferenceCount
		{
			get
			{
				return (this._edgeReferenceCounts & ElementEdge.BeforeEnd) > (ElementEdge)0;
			}
			set
			{
				Invariant.Assert(value);
				this._edgeReferenceCounts |= ElementEdge.BeforeEnd;
			}
		}

		// Token: 0x17000F73 RID: 3955
		// (get) Token: 0x06003E00 RID: 15872 RVA: 0x0011D08C File Offset: 0x0011B28C
		// (set) Token: 0x06003E01 RID: 15873 RVA: 0x0011D099 File Offset: 0x0011B299
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

		// Token: 0x17000F74 RID: 3956
		// (get) Token: 0x06003E02 RID: 15874 RVA: 0x0011D0AF File Offset: 0x0011B2AF
		// (set) Token: 0x06003E03 RID: 15875 RVA: 0x0011D0B7 File Offset: 0x0011B2B7
		internal TextElement TextElement
		{
			get
			{
				return this._textElement;
			}
			set
			{
				this._textElement = value;
			}
		}

		// Token: 0x17000F75 RID: 3957
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x0011D0C0 File Offset: 0x0011B2C0
		internal int IMELeftEdgeCharCount
		{
			get
			{
				if (this._textElement != null)
				{
					return this._textElement.IMELeftEdgeCharCount;
				}
				return -1;
			}
		}

		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x0011D0D7 File Offset: 0x0011B2D7
		internal bool IsFirstSibling
		{
			get
			{
				base.Splay();
				return this._leftChildNode == null;
			}
		}

		// Token: 0x0400267E RID: 9854
		private int _leftSymbolCount;

		// Token: 0x0400267F RID: 9855
		private int _leftCharCount;

		// Token: 0x04002680 RID: 9856
		private TextTreeNode _parentNode;

		// Token: 0x04002681 RID: 9857
		private TextTreeNode _leftChildNode;

		// Token: 0x04002682 RID: 9858
		private TextTreeNode _rightChildNode;

		// Token: 0x04002683 RID: 9859
		private TextTreeNode _containedNode;

		// Token: 0x04002684 RID: 9860
		private uint _generation;

		// Token: 0x04002685 RID: 9861
		private int _symbolOffsetCache;

		// Token: 0x04002686 RID: 9862
		private int _symbolCount;

		// Token: 0x04002687 RID: 9863
		private int _imeCharCount;

		// Token: 0x04002688 RID: 9864
		private TextElement _textElement;

		// Token: 0x04002689 RID: 9865
		private ElementEdge _edgeReferenceCounts;
	}
}
