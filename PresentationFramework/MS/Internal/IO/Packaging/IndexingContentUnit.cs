using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000660 RID: 1632
	internal class IndexingContentUnit : ManagedChunk
	{
		// Token: 0x06006C2A RID: 27690 RVA: 0x001F1C82 File Offset: 0x001EFE82
		internal IndexingContentUnit(string contents, uint chunkID, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid) : base(chunkID, breakType, attribute, lcid, CHUNKSTATE.CHUNK_TEXT)
		{
			this._contents = contents;
		}

		// Token: 0x06006C2B RID: 27691 RVA: 0x001F1C98 File Offset: 0x001EFE98
		internal void InitIndexingContentUnit(string contents, uint chunkID, CHUNK_BREAKTYPE breakType, ManagedFullPropSpec attribute, uint lcid)
		{
			this._contents = contents;
			base.ID = chunkID;
			base.BreakType = breakType;
			base.Attribute = attribute;
			base.Locale = lcid;
		}

		// Token: 0x170019E0 RID: 6624
		// (get) Token: 0x06006C2C RID: 27692 RVA: 0x001F1CBF File Offset: 0x001EFEBF
		internal string Text
		{
			get
			{
				return this._contents;
			}
		}

		// Token: 0x04003517 RID: 13591
		private string _contents;
	}
}
