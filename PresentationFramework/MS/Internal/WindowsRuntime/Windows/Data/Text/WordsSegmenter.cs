using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using MS.Internal.WindowsRuntime.Windows.Globalization;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x020007F5 RID: 2037
	internal class WordsSegmenter
	{
		// Token: 0x06007D60 RID: 32096 RVA: 0x00233974 File Offset: 0x00231B74
		static WordsSegmenter()
		{
			ConstructorInfo left = null;
			try
			{
				WordsSegmenter.s_WinRTType = Type.GetType(WordsSegmenter.s_TypeName);
				if (WordsSegmenter.s_WinRTType != null)
				{
					left = WordsSegmenter.s_WinRTType.GetConstructor(new Type[]
					{
						typeof(string)
					});
				}
			}
			catch
			{
				WordsSegmenter.s_WinRTType = null;
			}
			WordsSegmenter.s_PlatformSupported = (WordsSegmenter.s_WinRTType != null && left != null);
		}

		// Token: 0x06007D61 RID: 32097 RVA: 0x00233AD8 File Offset: 0x00231CD8
		private WordsSegmenter(string language)
		{
			if (!WordsSegmenter.s_PlatformSupported)
			{
				throw new PlatformNotSupportedException();
			}
			try
			{
				this._wordsSegmenter = WordsSegmenter.s_WinRTType.ReflectionNew(language);
			}
			catch (TargetInvocationException ex) when (ex.InnerException is ArgumentException)
			{
				throw new ArgumentException(string.Empty, "language", ex);
			}
			if (this._wordsSegmenter == null)
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06007D62 RID: 32098 RVA: 0x00233B5C File Offset: 0x00231D5C
		public static WordsSegmenter Create(string language, bool shouldPreferNeutralSegmenter = false)
		{
			if (shouldPreferNeutralSegmenter && !WordsSegmenter.ShouldUseDedicatedSegmenter(language))
			{
				language = WordsSegmenter.Undetermined;
			}
			return new WordsSegmenter(language);
		}

		// Token: 0x06007D63 RID: 32099 RVA: 0x00233B78 File Offset: 0x00231D78
		public WordSegment GetTokenAt(string text, uint startIndex)
		{
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			WordSegment result;
			TargetInvocationException ex2;
			object obj2;
			TargetInvocationException ex;
			bool flag;
			Exception innerException;
			try
			{
				object obj = this._wordsSegmenter.ReflectionCall("GetTokenAt", text, startIndex);
				if (obj != null)
				{
					result = new WordSegment(obj);
				}
				else
				{
					result = null;
				}
			}
			catch when (delegate
			{
				// Failed to create a 'catch-when' expression
				ex = (obj2 as TargetInvocationException);
				if (ex == null)
				{
					flag = false;
				}
				else
				{
					ex2 = ex;
					innerException = ex2.InnerException;
					flag = ((innerException != null && innerException.HResult == -2147483637) > false);
				}
				endfilter(flag);
			})
			{
				throw new IndexOutOfRangeException(string.Empty, ex2);
			}
			return result;
		}

		// Token: 0x06007D64 RID: 32100 RVA: 0x00233C00 File Offset: 0x00231E00
		public IReadOnlyList<WordSegment> GetTokens(string text)
		{
			object obj = this._wordsSegmenter.ReflectionCall("GetTokens", text);
			List<WordSegment> list = new List<WordSegment>();
			foreach (object obj2 in ((IEnumerable)obj))
			{
				if (obj2 != null)
				{
					list.Add(new WordSegment(obj2));
				}
			}
			return list.AsReadOnly();
		}

		// Token: 0x06007D65 RID: 32101 RVA: 0x0003E264 File Offset: 0x0003C464
		public void Tokenize(string text, uint startIndex, WordSegmentsTokenizingHandler handler)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001D24 RID: 7460
		// (get) Token: 0x06007D66 RID: 32102 RVA: 0x00233C80 File Offset: 0x00231E80
		public string ResolvedLanguage
		{
			get
			{
				if (this._resolvedLanguage == null)
				{
					this._resolvedLanguage = this._wordsSegmenter.ReflectionGetProperty("ResolvedLanguage");
				}
				return this._resolvedLanguage;
			}
		}

		// Token: 0x17001D25 RID: 7461
		// (get) Token: 0x06007D67 RID: 32103 RVA: 0x00233CA6 File Offset: 0x00231EA6
		public static Type WinRTType
		{
			get
			{
				return WordsSegmenter.s_WinRTType;
			}
		}

		// Token: 0x06007D68 RID: 32104 RVA: 0x00233CB0 File Offset: 0x00231EB0
		private static bool ShouldUseDedicatedSegmenter(string languageTag)
		{
			bool result = true;
			try
			{
				Language language = new Language(languageTag);
				string script = language.Script;
				if (WordsSegmenter.ScriptCodesRequiringDedicatedSegmenter.FindIndex((string s) => s.Equals(script, StringComparison.InvariantCultureIgnoreCase)) == -1)
				{
					result = false;
				}
			}
			catch (Exception ex) when (ex is NotSupportedException || ex is ArgumentException || ex is TargetInvocationException || ex is MissingMemberException)
			{
			}
			return result;
		}

		// Token: 0x17001D26 RID: 7462
		// (get) Token: 0x06007D69 RID: 32105 RVA: 0x00233D40 File Offset: 0x00231F40
		private static List<string> ScriptCodesRequiringDedicatedSegmenter { get; } = new List<string>
		{
			"Bopo",
			"Brah",
			"Egyp",
			"Goth",
			"Hang",
			"Hani",
			"Ital",
			"Java",
			"Kana",
			"Khar",
			"Laoo",
			"Lisu",
			"Mymr",
			"Talu",
			"Thai",
			"Tibt",
			"Xsux",
			"Yiii"
		};

		// Token: 0x04003B06 RID: 15110
		public static readonly string Undetermined = "und";

		// Token: 0x04003B07 RID: 15111
		private static string s_TypeName = "Windows.Data.Text.WordsSegmenter, Windows, ContentType=WindowsRuntime";

		// Token: 0x04003B08 RID: 15112
		private static Type s_WinRTType;

		// Token: 0x04003B09 RID: 15113
		private static bool s_PlatformSupported;

		// Token: 0x04003B0A RID: 15114
		private object _wordsSegmenter;

		// Token: 0x04003B0B RID: 15115
		private string _resolvedLanguage;
	}
}
