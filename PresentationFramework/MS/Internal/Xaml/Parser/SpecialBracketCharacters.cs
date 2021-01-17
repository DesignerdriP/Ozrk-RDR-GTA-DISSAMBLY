using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MS.Internal.Xaml.Parser
{
	// Token: 0x02000806 RID: 2054
	internal class SpecialBracketCharacters : ISupportInitialize
	{
		// Token: 0x06007E01 RID: 32257 RVA: 0x0023507F File Offset: 0x0023327F
		internal SpecialBracketCharacters()
		{
			this.BeginInit();
		}

		// Token: 0x06007E02 RID: 32258 RVA: 0x0023508D File Offset: 0x0023328D
		internal SpecialBracketCharacters(IReadOnlyDictionary<char, char> attributeList)
		{
			this.BeginInit();
			if (attributeList != null && attributeList.Count > 0)
			{
				this.Tokenize(attributeList);
			}
		}

		// Token: 0x06007E03 RID: 32259 RVA: 0x002350AE File Offset: 0x002332AE
		internal void AddBracketCharacters(char openingBracket, char closingBracket)
		{
			if (this._initializing)
			{
				this._startCharactersStringBuilder.Append(openingBracket);
				this._endCharactersStringBuilder.Append(closingBracket);
				return;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x06007E04 RID: 32260 RVA: 0x002350D8 File Offset: 0x002332D8
		private void Tokenize(IReadOnlyDictionary<char, char> attributeList)
		{
			if (this._initializing)
			{
				foreach (char c in attributeList.Keys)
				{
					char c2 = attributeList[c];
					string empty = string.Empty;
					if (this.IsValidBracketCharacter(c, c2))
					{
						this._startCharactersStringBuilder.Append(c);
						this._endCharactersStringBuilder.Append(c2);
					}
				}
			}
		}

		// Token: 0x06007E05 RID: 32261 RVA: 0x00235158 File Offset: 0x00233358
		private bool IsValidBracketCharacter(char openingBracket, char closingBracket)
		{
			if (openingBracket == closingBracket)
			{
				throw new InvalidOperationException("Opening bracket character cannot be the same as closing bracket character.");
			}
			if (char.IsLetterOrDigit(openingBracket) || char.IsLetterOrDigit(closingBracket) || char.IsWhiteSpace(openingBracket) || char.IsWhiteSpace(closingBracket))
			{
				throw new InvalidOperationException("Bracket characters cannot be alpha-numeric or whitespace.");
			}
			if (SpecialBracketCharacters._restrictedCharSet.Contains(openingBracket) || SpecialBracketCharacters._restrictedCharSet.Contains(closingBracket))
			{
				throw new InvalidOperationException("Bracket characters cannot be one of the following: '=' , ',', ''', '\"', '{ ', ' }', '\\'");
			}
			return true;
		}

		// Token: 0x06007E06 RID: 32262 RVA: 0x002351C5 File Offset: 0x002333C5
		internal bool IsSpecialCharacter(char ch)
		{
			return this._startChars.Contains(ch.ToString()) || this._endChars.Contains(ch.ToString());
		}

		// Token: 0x06007E07 RID: 32263 RVA: 0x002351EF File Offset: 0x002333EF
		internal bool StartsEscapeSequence(char ch)
		{
			return this._startChars.Contains(ch.ToString());
		}

		// Token: 0x06007E08 RID: 32264 RVA: 0x00235203 File Offset: 0x00233403
		internal bool EndsEscapeSequence(char ch)
		{
			return this._endChars.Contains(ch.ToString());
		}

		// Token: 0x06007E09 RID: 32265 RVA: 0x00235217 File Offset: 0x00233417
		internal bool Match(char start, char end)
		{
			return this._endChars.IndexOf(end.ToString()) == this._startChars.IndexOf(start.ToString());
		}

		// Token: 0x17001D49 RID: 7497
		// (get) Token: 0x06007E0A RID: 32266 RVA: 0x0023523F File Offset: 0x0023343F
		internal string StartBracketCharacters
		{
			get
			{
				return this._startChars;
			}
		}

		// Token: 0x17001D4A RID: 7498
		// (get) Token: 0x06007E0B RID: 32267 RVA: 0x00235247 File Offset: 0x00233447
		internal string EndBracketCharacters
		{
			get
			{
				return this._endChars;
			}
		}

		// Token: 0x06007E0C RID: 32268 RVA: 0x0023524F File Offset: 0x0023344F
		public void BeginInit()
		{
			this._initializing = true;
			this._startCharactersStringBuilder = new StringBuilder();
			this._endCharactersStringBuilder = new StringBuilder();
		}

		// Token: 0x06007E0D RID: 32269 RVA: 0x0023526E File Offset: 0x0023346E
		public void EndInit()
		{
			this._startChars = this._startCharactersStringBuilder.ToString();
			this._endChars = this._endCharactersStringBuilder.ToString();
			this._initializing = false;
		}

		// Token: 0x04003B7A RID: 15226
		private string _startChars;

		// Token: 0x04003B7B RID: 15227
		private string _endChars;

		// Token: 0x04003B7C RID: 15228
		private static readonly ISet<char> _restrictedCharSet = new SortedSet<char>(new char[]
		{
			'=',
			',',
			'\'',
			'"',
			'{',
			'}',
			'\\'
		});

		// Token: 0x04003B7D RID: 15229
		private bool _initializing;

		// Token: 0x04003B7E RID: 15230
		private StringBuilder _startCharactersStringBuilder;

		// Token: 0x04003B7F RID: 15231
		private StringBuilder _endCharactersStringBuilder;
	}
}
