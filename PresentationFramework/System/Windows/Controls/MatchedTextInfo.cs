using System;

namespace System.Windows.Controls
{
	// Token: 0x02000468 RID: 1128
	internal class MatchedTextInfo
	{
		// Token: 0x060041CF RID: 16847 RVA: 0x0012DBD4 File Offset: 0x0012BDD4
		internal MatchedTextInfo(int matchedItemIndex, string matchedText, int matchedPrefixLength, int textExcludingPrefixLength)
		{
			this._matchedItemIndex = matchedItemIndex;
			this._matchedText = matchedText;
			this._matchedPrefixLength = matchedPrefixLength;
			this._textExcludingPrefixLength = textExcludingPrefixLength;
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x060041D0 RID: 16848 RVA: 0x0012DBF9 File Offset: 0x0012BDF9
		internal static MatchedTextInfo NoMatch
		{
			get
			{
				return MatchedTextInfo.s_NoMatch;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x060041D1 RID: 16849 RVA: 0x0012DC00 File Offset: 0x0012BE00
		internal string MatchedText
		{
			get
			{
				return this._matchedText;
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x0012DC08 File Offset: 0x0012BE08
		internal int MatchedItemIndex
		{
			get
			{
				return this._matchedItemIndex;
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x060041D3 RID: 16851 RVA: 0x0012DC10 File Offset: 0x0012BE10
		internal int MatchedPrefixLength
		{
			get
			{
				return this._matchedPrefixLength;
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x060041D4 RID: 16852 RVA: 0x0012DC18 File Offset: 0x0012BE18
		internal int TextExcludingPrefixLength
		{
			get
			{
				return this._textExcludingPrefixLength;
			}
		}

		// Token: 0x040027B3 RID: 10163
		private readonly string _matchedText;

		// Token: 0x040027B4 RID: 10164
		private readonly int _matchedItemIndex;

		// Token: 0x040027B5 RID: 10165
		private readonly int _matchedPrefixLength;

		// Token: 0x040027B6 RID: 10166
		private readonly int _textExcludingPrefixLength;

		// Token: 0x040027B7 RID: 10167
		private static MatchedTextInfo s_NoMatch = new MatchedTextInfo(-1, null, 0, 0);
	}
}
