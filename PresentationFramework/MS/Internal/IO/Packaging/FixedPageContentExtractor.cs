using System;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200065F RID: 1631
	internal class FixedPageContentExtractor
	{
		// Token: 0x06006C27 RID: 27687 RVA: 0x001F1BF7 File Offset: 0x001EFDF7
		internal FixedPageContentExtractor(XmlNode fixedPage)
		{
			this._fixedPageInfo = new XmlFixedPageInfo(fixedPage);
			this._nextGlyphRun = 0;
		}

		// Token: 0x06006C28 RID: 27688 RVA: 0x001F1C14 File Offset: 0x001EFE14
		internal string NextGlyphContent(out bool inline, out uint lcid)
		{
			inline = false;
			lcid = 0U;
			if (this._nextGlyphRun >= this._fixedPageInfo.GlyphRunCount)
			{
				return null;
			}
			GlyphRunInfo glyphRunInfo = this._fixedPageInfo.GlyphRunAtPosition(this._nextGlyphRun);
			lcid = glyphRunInfo.LanguageID;
			this._nextGlyphRun++;
			return glyphRunInfo.UnicodeString;
		}

		// Token: 0x170019DF RID: 6623
		// (get) Token: 0x06006C29 RID: 27689 RVA: 0x001F1C6A File Offset: 0x001EFE6A
		internal bool AtEndOfPage
		{
			get
			{
				return this._nextGlyphRun >= this._fixedPageInfo.GlyphRunCount;
			}
		}

		// Token: 0x04003515 RID: 13589
		private XmlFixedPageInfo _fixedPageInfo;

		// Token: 0x04003516 RID: 13590
		private int _nextGlyphRun;
	}
}
