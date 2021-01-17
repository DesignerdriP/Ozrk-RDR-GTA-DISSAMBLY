using System;
using System.Collections.Generic;
using System.Windows.Documents.MsSpellCheckLib;
using MS.Internal.WindowsRuntime.Windows.Data.Text;

namespace System.Windows.Documents
{
	// Token: 0x02000431 RID: 1073
	internal static class WinRTSpellerInteropExtensions
	{
		// Token: 0x06003F1A RID: 16154 RVA: 0x00120230 File Offset: 0x0011E430
		public static IReadOnlyList<WinRTSpellerInterop.SpellerSegment> ComprehensiveGetTokens(this WordsSegmenter segmenter, string text, SpellChecker spellChecker, WinRTSpellerInterop owner)
		{
			IReadOnlyList<WordSegment> readOnlyList = ((segmenter != null) ? segmenter.GetTokens(text) : null) ?? new List<WordSegment>().AsReadOnly();
			List<WinRTSpellerInterop.SpellerSegment> list = new List<WinRTSpellerInterop.SpellerSegment>();
			if (readOnlyList.Count == 0)
			{
				return list.AsReadOnly();
			}
			int num = 0;
			for (int i = 0; i < readOnlyList.Count; i++)
			{
				int startPosition = (int)readOnlyList[i].SourceTextSegment.StartPosition;
				int length = (int)readOnlyList[i].SourceTextSegment.Length;
				if (spellChecker != null && startPosition > num)
				{
					WinRTSpellerInterop.SpellerSegment missingFragment = new WinRTSpellerInterop.SpellerSegment(text, new WinRTSpellerInterop.TextRange(num, startPosition - num), spellChecker, owner);
					if (list.Count > 0)
					{
						WinRTSpellerInterop.TextRange? spellCheckCleanSubstitutionToken = WinRTSpellerInteropExtensions.GetSpellCheckCleanSubstitutionToken(spellChecker, text, list[list.Count - 1], missingFragment);
						if (spellCheckCleanSubstitutionToken != null)
						{
							list[list.Count - 1] = new WinRTSpellerInterop.SpellerSegment(text, spellCheckCleanSubstitutionToken.Value, spellChecker, owner);
						}
					}
				}
				list.Add(new WinRTSpellerInterop.SpellerSegment(text, new WinRTSpellerInterop.TextRange(startPosition, length), spellChecker, owner));
				num = startPosition + length;
			}
			if (readOnlyList.Count > 0)
			{
				bool flag;
				if (spellChecker == null)
				{
					flag = true;
				}
				else
				{
					List<SpellChecker.SpellingError> list2 = spellChecker.ComprehensiveCheck(readOnlyList[readOnlyList.Count - 1].Text, true);
					flag = (((list2 != null) ? new int?(list2.Count) : null) != 0);
				}
				if (flag && num < text.Length)
				{
					WinRTSpellerInterop.SpellerSegment missingFragment2 = new WinRTSpellerInterop.SpellerSegment(text, new WinRTSpellerInterop.TextRange(num, text.Length - num), spellChecker, owner);
					if (list.Count > 0)
					{
						WinRTSpellerInterop.TextRange? spellCheckCleanSubstitutionToken2 = WinRTSpellerInteropExtensions.GetSpellCheckCleanSubstitutionToken(spellChecker, text, list[list.Count - 1], missingFragment2);
						if (spellCheckCleanSubstitutionToken2 != null)
						{
							list[list.Count - 1] = new WinRTSpellerInterop.SpellerSegment(text, spellCheckCleanSubstitutionToken2.Value, spellChecker, owner);
						}
					}
				}
			}
			return list.AsReadOnly();
		}

		// Token: 0x06003F1B RID: 16155 RVA: 0x0012041C File Offset: 0x0011E61C
		private static WinRTSpellerInterop.TextRange? GetSpellCheckCleanSubstitutionToken(SpellChecker spellChecker, string documentText, WinRTSpellerInterop.SpellerSegment lastToken, WinRTSpellerInterop.SpellerSegment missingFragment)
		{
			if (string.IsNullOrWhiteSpace((missingFragment != null) ? missingFragment.Text : null) || string.IsNullOrWhiteSpace((lastToken != null) ? lastToken.Text : null) || string.IsNullOrWhiteSpace(documentText))
			{
				return null;
			}
			int num = Math.Min(missingFragment.TextRange.Length, 4);
			List<SpellChecker.SpellingError> list = (spellChecker != null) ? spellChecker.ComprehensiveCheck(lastToken.Text, true) : null;
			if (list == null || list.Count != 0)
			{
				for (int i = 1; i <= num; i++)
				{
					string text = documentText.Substring(lastToken.TextRange.Start, lastToken.TextRange.Length + i).TrimEnd(new char[0]);
					if (spellChecker != null)
					{
						List<SpellChecker.SpellingError> list2 = spellChecker.ComprehensiveCheck(text, true);
						if (((list2 != null) ? new int?(list2.Count) : null) == 0)
						{
							return new WinRTSpellerInterop.TextRange?(new WinRTSpellerInterop.TextRange(lastToken.TextRange.Start, text.Length));
						}
					}
				}
			}
			return null;
		}
	}
}
