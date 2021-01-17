using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000665 RID: 1637
	internal class ManagedChunk
	{
		// Token: 0x06006C67 RID: 27751 RVA: 0x001F38FC File Offset: 0x001F1AFC
		internal ManagedChunk(uint index, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid, CHUNKSTATE flags)
		{
			Invariant.Assert(breakType >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && breakType <= CHUNK_BREAKTYPE.CHUNK_EOC);
			Invariant.Assert(attribute != null);
			this._index = index;
			this._breakType = breakType;
			this._lcid = lcid;
			this._attribute = attribute;
			this._flags = flags;
			this._idChunkSource = this._index;
		}

		// Token: 0x170019E7 RID: 6631
		// (get) Token: 0x06006C68 RID: 27752 RVA: 0x001F395C File Offset: 0x001F1B5C
		// (set) Token: 0x06006C69 RID: 27753 RVA: 0x001F3964 File Offset: 0x001F1B64
		internal uint ID
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x06006C6A RID: 27754 RVA: 0x001F396D File Offset: 0x001F1B6D
		internal CHUNKSTATE Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x170019E9 RID: 6633
		// (get) Token: 0x06006C6B RID: 27755 RVA: 0x001F3975 File Offset: 0x001F1B75
		// (set) Token: 0x06006C6C RID: 27756 RVA: 0x001F397D File Offset: 0x001F1B7D
		internal CHUNK_BREAKTYPE BreakType
		{
			get
			{
				return this._breakType;
			}
			set
			{
				Invariant.Assert(value >= CHUNK_BREAKTYPE.CHUNK_NO_BREAK && value <= CHUNK_BREAKTYPE.CHUNK_EOC);
				this._breakType = value;
			}
		}

		// Token: 0x170019EA RID: 6634
		// (get) Token: 0x06006C6D RID: 27757 RVA: 0x001F3999 File Offset: 0x001F1B99
		// (set) Token: 0x06006C6E RID: 27758 RVA: 0x001F39A1 File Offset: 0x001F1BA1
		internal uint Locale
		{
			get
			{
				return this._lcid;
			}
			set
			{
				this._lcid = value;
			}
		}

		// Token: 0x170019EB RID: 6635
		// (get) Token: 0x06006C6F RID: 27759 RVA: 0x001F39AA File Offset: 0x001F1BAA
		internal uint ChunkSource
		{
			get
			{
				return this._idChunkSource;
			}
		}

		// Token: 0x170019EC RID: 6636
		// (get) Token: 0x06006C70 RID: 27760 RVA: 0x001F39B2 File Offset: 0x001F1BB2
		internal uint StartSource
		{
			get
			{
				return this._startSource;
			}
		}

		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x06006C71 RID: 27761 RVA: 0x001F39BA File Offset: 0x001F1BBA
		internal uint LenSource
		{
			get
			{
				return this._lenSource;
			}
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x06006C72 RID: 27762 RVA: 0x001F39C2 File Offset: 0x001F1BC2
		// (set) Token: 0x06006C73 RID: 27763 RVA: 0x001F39CA File Offset: 0x001F1BCA
		internal ManagedFullPropSpec Attribute
		{
			get
			{
				return this._attribute;
			}
			set
			{
				this._attribute = value;
			}
		}

		// Token: 0x0400353D RID: 13629
		private uint _index;

		// Token: 0x0400353E RID: 13630
		private CHUNK_BREAKTYPE _breakType;

		// Token: 0x0400353F RID: 13631
		private CHUNKSTATE _flags;

		// Token: 0x04003540 RID: 13632
		private uint _lcid;

		// Token: 0x04003541 RID: 13633
		private ManagedFullPropSpec _attribute;

		// Token: 0x04003542 RID: 13634
		private uint _idChunkSource;

		// Token: 0x04003543 RID: 13635
		private uint _startSource;

		// Token: 0x04003544 RID: 13636
		private uint _lenSource;
	}
}
