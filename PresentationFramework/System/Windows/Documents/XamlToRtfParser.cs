using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x02000437 RID: 1079
	internal class XamlToRtfParser
	{
		// Token: 0x06003F3C RID: 16188 RVA: 0x0012110E File Offset: 0x0011F30E
		internal XamlToRtfParser(string xaml)
		{
			this._xaml = xaml;
			this._xamlLexer = new XamlToRtfParser.XamlLexer(this._xaml);
			this._xamlTagStack = new XamlToRtfParser.XamlTagStack();
			this._xamlAttributes = new XamlToRtfParser.XamlAttributes(this._xaml);
		}

		// Token: 0x06003F3D RID: 16189 RVA: 0x0012114C File Offset: 0x0011F34C
		internal XamlToRtfError Parse()
		{
			if (this._xamlContent == null || this._xamlError == null)
			{
				return XamlToRtfError.Unknown;
			}
			XamlToRtfParser.XamlToken xamlToken = new XamlToRtfParser.XamlToken();
			string empty = string.Empty;
			XamlToRtfError xamlToRtfError;
			for (xamlToRtfError = this._xamlContent.StartDocument(); xamlToRtfError == XamlToRtfError.None; xamlToRtfError = XamlToRtfError.Unknown)
			{
				xamlToRtfError = this._xamlLexer.Next(xamlToken);
				if (xamlToRtfError != XamlToRtfError.None || xamlToken.TokenType == XamlTokenType.XTokEOF)
				{
					break;
				}
				switch (xamlToken.TokenType)
				{
				case XamlTokenType.XTokInvalid:
					xamlToRtfError = XamlToRtfError.Unknown;
					continue;
				case XamlTokenType.XTokCharacters:
					xamlToRtfError = this._xamlContent.Characters(xamlToken.Text);
					continue;
				case XamlTokenType.XTokEntity:
					xamlToRtfError = this._xamlContent.SkippedEntity(xamlToken.Text);
					continue;
				case XamlTokenType.XTokStartElement:
					xamlToRtfError = this.ParseXTokStartElement(xamlToken, ref empty);
					continue;
				case XamlTokenType.XTokEndElement:
					xamlToRtfError = this.ParseXTokEndElement(xamlToken, ref empty);
					continue;
				case XamlTokenType.XTokCData:
				case XamlTokenType.XTokPI:
				case XamlTokenType.XTokComment:
					continue;
				case XamlTokenType.XTokWS:
					xamlToRtfError = this._xamlContent.IgnorableWhitespace(xamlToken.Text);
					continue;
				}
			}
			if (xamlToRtfError == XamlToRtfError.None && this._xamlTagStack.Count != 0)
			{
				xamlToRtfError = XamlToRtfError.Unknown;
			}
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlContent.EndDocument();
			}
			return xamlToRtfError;
		}

		// Token: 0x06003F3E RID: 16190 RVA: 0x00121262 File Offset: 0x0011F462
		internal void SetCallbacks(IXamlContentHandler xamlContent, IXamlErrorHandler xamlError)
		{
			this._xamlContent = xamlContent;
			this._xamlError = xamlError;
		}

		// Token: 0x06003F3F RID: 16191 RVA: 0x00121274 File Offset: 0x0011F474
		private XamlToRtfError ParseXTokStartElement(XamlToRtfParser.XamlToken xamlToken, ref string name)
		{
			XamlToRtfError xamlToRtfError = this._xamlAttributes.Init(xamlToken.Text);
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlAttributes.GetTag(ref name);
				if (xamlToRtfError == XamlToRtfError.None)
				{
					xamlToRtfError = this._xamlContent.StartElement(string.Empty, name, name, this._xamlAttributes);
					if (xamlToRtfError == XamlToRtfError.None)
					{
						if (this._xamlAttributes.IsEmpty)
						{
							xamlToRtfError = this._xamlContent.EndElement(string.Empty, name, name);
						}
						else
						{
							xamlToRtfError = (XamlToRtfError)this._xamlTagStack.Push(name);
						}
					}
				}
			}
			return xamlToRtfError;
		}

		// Token: 0x06003F40 RID: 16192 RVA: 0x001212F8 File Offset: 0x0011F4F8
		private XamlToRtfError ParseXTokEndElement(XamlToRtfParser.XamlToken xamlToken, ref string name)
		{
			XamlToRtfError xamlToRtfError = this._xamlAttributes.Init(xamlToken.Text);
			if (xamlToRtfError == XamlToRtfError.None)
			{
				xamlToRtfError = this._xamlAttributes.GetTag(ref name);
				if (xamlToRtfError == XamlToRtfError.None && this._xamlTagStack.IsMatchTop(name))
				{
					this._xamlTagStack.Pop();
					xamlToRtfError = this._xamlContent.EndElement(string.Empty, name, name);
				}
			}
			return xamlToRtfError;
		}

		// Token: 0x04002725 RID: 10021
		private string _xaml;

		// Token: 0x04002726 RID: 10022
		private XamlToRtfParser.XamlLexer _xamlLexer;

		// Token: 0x04002727 RID: 10023
		private XamlToRtfParser.XamlTagStack _xamlTagStack;

		// Token: 0x04002728 RID: 10024
		private XamlToRtfParser.XamlAttributes _xamlAttributes;

		// Token: 0x04002729 RID: 10025
		private IXamlContentHandler _xamlContent;

		// Token: 0x0400272A RID: 10026
		private IXamlErrorHandler _xamlError;

		// Token: 0x0200091F RID: 2335
		internal class XamlLexer
		{
			// Token: 0x06008623 RID: 34339 RVA: 0x0024BCBA File Offset: 0x00249EBA
			internal XamlLexer(string xaml)
			{
				this._xaml = xaml;
			}

			// Token: 0x06008624 RID: 34340 RVA: 0x0024BCCC File Offset: 0x00249ECC
			internal XamlToRtfError Next(XamlToRtfParser.XamlToken token)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int xamlIndex = this._xamlIndex;
				if (this._xamlIndex < this._xaml.Length)
				{
					char c = this._xaml[this._xamlIndex];
					if (c <= ' ')
					{
						switch (c)
						{
						case '\t':
						case '\n':
						case '\r':
							break;
						case '\v':
						case '\f':
							goto IL_124;
						default:
							if (c != ' ')
							{
								goto IL_124;
							}
							break;
						}
						token.TokenType = XamlTokenType.XTokWS;
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (!this.IsSpace(this._xaml[this._xamlIndex]))
							{
								break;
							}
							this._xamlIndex++;
						}
						goto IL_17C;
					}
					if (c == '&')
					{
						token.TokenType = XamlTokenType.XTokInvalid;
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (this._xaml[this._xamlIndex] == ';')
							{
								this._xamlIndex++;
								token.TokenType = XamlTokenType.XTokEntity;
								break;
							}
							this._xamlIndex++;
						}
						goto IL_17C;
					}
					if (c == '<')
					{
						this.NextLessThanToken(token);
						goto IL_17C;
					}
					IL_124:
					token.TokenType = XamlTokenType.XTokCharacters;
					this._xamlIndex++;
					while (this.IsCharsAvailable(1) && this._xaml[this._xamlIndex] != '&' && this._xaml[this._xamlIndex] != '<')
					{
						this._xamlIndex++;
					}
				}
				IL_17C:
				token.Text = this._xaml.Substring(xamlIndex, this._xamlIndex - xamlIndex);
				if (token.Text.Length == 0)
				{
					token.TokenType = XamlTokenType.XTokEOF;
				}
				return result;
			}

			// Token: 0x06008625 RID: 34341 RVA: 0x0024BE84 File Offset: 0x0024A084
			private bool IsSpace(char character)
			{
				return character == ' ' || character == '\t' || character == '\n' || character == '\r';
			}

			// Token: 0x06008626 RID: 34342 RVA: 0x0024BE9C File Offset: 0x0024A09C
			private bool IsCharsAvailable(int index)
			{
				return this._xamlIndex + index <= this._xaml.Length;
			}

			// Token: 0x06008627 RID: 34343 RVA: 0x0024BEB8 File Offset: 0x0024A0B8
			private void NextLessThanToken(XamlToRtfParser.XamlToken token)
			{
				this._xamlIndex++;
				if (!this.IsCharsAvailable(1))
				{
					token.TokenType = XamlTokenType.XTokInvalid;
					return;
				}
				token.TokenType = XamlTokenType.XTokInvalid;
				char c = this._xaml[this._xamlIndex];
				if (c <= '/')
				{
					if (c == '!')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(3))
						{
							if (this._xaml[this._xamlIndex] == '-' && this._xaml[this._xamlIndex + 1] == '-' && this._xaml[this._xamlIndex + 2] == '>')
							{
								this._xamlIndex += 3;
								token.TokenType = XamlTokenType.XTokComment;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
					if (c == '/')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(1))
						{
							if (this._xaml[this._xamlIndex] == '>')
							{
								this._xamlIndex++;
								token.TokenType = XamlTokenType.XTokEndElement;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
				}
				else
				{
					if (c == '>')
					{
						this._xamlIndex++;
						token.TokenType = XamlTokenType.XTokInvalid;
						return;
					}
					if (c == '?')
					{
						this._xamlIndex++;
						while (this.IsCharsAvailable(2))
						{
							if (this._xaml[this._xamlIndex] == '?' && this._xaml[this._xamlIndex + 1] == '>')
							{
								this._xamlIndex += 2;
								token.TokenType = XamlTokenType.XTokPI;
								return;
							}
							this._xamlIndex++;
						}
						return;
					}
				}
				char c2 = '\0';
				while (this.IsCharsAvailable(1))
				{
					if (c2 != '\0')
					{
						if (this._xaml[this._xamlIndex] == c2)
						{
							c2 = '\0';
						}
					}
					else if (this._xaml[this._xamlIndex] == '"' || this._xaml[this._xamlIndex] == '\'')
					{
						c2 = this._xaml[this._xamlIndex];
					}
					else if (this._xaml[this._xamlIndex] == '>')
					{
						this._xamlIndex++;
						token.TokenType = XamlTokenType.XTokStartElement;
						return;
					}
					this._xamlIndex++;
				}
			}

			// Token: 0x04004359 RID: 17241
			private string _xaml;

			// Token: 0x0400435A RID: 17242
			private int _xamlIndex;
		}

		// Token: 0x02000920 RID: 2336
		internal class XamlTagStack : ArrayList
		{
			// Token: 0x06008628 RID: 34344 RVA: 0x0024C120 File Offset: 0x0024A320
			internal XamlTagStack() : base(10)
			{
			}

			// Token: 0x06008629 RID: 34345 RVA: 0x0024C12A File Offset: 0x0024A32A
			internal RtfToXamlError Push(string xamlTag)
			{
				this.Add(xamlTag);
				return RtfToXamlError.None;
			}

			// Token: 0x0600862A RID: 34346 RVA: 0x0024C135 File Offset: 0x0024A335
			internal void Pop()
			{
				if (this.Count > 0)
				{
					this.RemoveAt(this.Count - 1);
				}
			}

			// Token: 0x0600862B RID: 34347 RVA: 0x0024C150 File Offset: 0x0024A350
			internal bool IsMatchTop(string xamlTag)
			{
				if (this.Count == 0)
				{
					return false;
				}
				string text = (string)this[this.Count - 1];
				return text.Length != 0 && string.Compare(xamlTag, xamlTag.Length, text, text.Length, text.Length, StringComparison.OrdinalIgnoreCase) == 0;
			}
		}

		// Token: 0x02000921 RID: 2337
		internal class XamlAttributes : IXamlAttributes
		{
			// Token: 0x0600862C RID: 34348 RVA: 0x0024C1A4 File Offset: 0x0024A3A4
			internal XamlAttributes(string xaml)
			{
				this._xamlParsePoints = new XamlToRtfParser.XamlParsePoints();
			}

			// Token: 0x0600862D RID: 34349 RVA: 0x0024C1B7 File Offset: 0x0024A3B7
			internal XamlToRtfError Init(string xaml)
			{
				return this._xamlParsePoints.Init(xaml);
			}

			// Token: 0x0600862E RID: 34350 RVA: 0x0024C1C8 File Offset: 0x0024A3C8
			internal XamlToRtfError GetTag(ref string xamlTag)
			{
				XamlToRtfError result = XamlToRtfError.None;
				if (!this._xamlParsePoints.IsValid)
				{
					return XamlToRtfError.Unknown;
				}
				xamlTag = (string)this._xamlParsePoints[0];
				return result;
			}

			// Token: 0x0600862F RID: 34351 RVA: 0x0024C1FC File Offset: 0x0024A3FC
			XamlToRtfError IXamlAttributes.GetLength(ref int length)
			{
				XamlToRtfError result = XamlToRtfError.None;
				if (this._xamlParsePoints.IsValid)
				{
					length = (this._xamlParsePoints.Count - 1) / 2;
					return result;
				}
				return XamlToRtfError.Unknown;
			}

			// Token: 0x06008630 RID: 34352 RVA: 0x0024C22C File Offset: 0x0024A42C
			XamlToRtfError IXamlAttributes.GetUri(int index, ref string uri)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008631 RID: 34353 RVA: 0x0024C23C File Offset: 0x0024A43C
			XamlToRtfError IXamlAttributes.GetLocalName(int index, ref string localName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008632 RID: 34354 RVA: 0x0024C24C File Offset: 0x0024A44C
			XamlToRtfError IXamlAttributes.GetQName(int index, ref string qName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008633 RID: 34355 RVA: 0x0024C25C File Offset: 0x0024A45C
			XamlToRtfError IXamlAttributes.GetName(int index, ref string uri, ref string localName, ref string qName)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int num = (this._xamlParsePoints.Count - 1) / 2;
				if (index < 0 || index > num - 1)
				{
					return XamlToRtfError.Unknown;
				}
				localName = (string)this._xamlParsePoints[index * 2 + 1];
				qName = (string)this._xamlParsePoints[index * 2 + 2];
				return result;
			}

			// Token: 0x06008634 RID: 34356 RVA: 0x0024C2B8 File Offset: 0x0024A4B8
			XamlToRtfError IXamlAttributes.GetIndexFromName(string uri, string localName, ref int index)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008635 RID: 34357 RVA: 0x0024C2C8 File Offset: 0x0024A4C8
			XamlToRtfError IXamlAttributes.GetIndexFromQName(string qName, ref int index)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008636 RID: 34358 RVA: 0x0024C2D8 File Offset: 0x0024A4D8
			XamlToRtfError IXamlAttributes.GetType(int index, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008637 RID: 34359 RVA: 0x0024C2E8 File Offset: 0x0024A4E8
			XamlToRtfError IXamlAttributes.GetTypeFromName(string uri, string localName, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x06008638 RID: 34360 RVA: 0x0024C2F8 File Offset: 0x0024A4F8
			XamlToRtfError IXamlAttributes.GetValue(int index, ref string valueName)
			{
				XamlToRtfError result = XamlToRtfError.None;
				int num = (this._xamlParsePoints.Count - 1) / 2;
				if (index < 0 || index > num - 1)
				{
					return XamlToRtfError.OutOfRange;
				}
				valueName = (string)this._xamlParsePoints[index * 2 + 2];
				return result;
			}

			// Token: 0x06008639 RID: 34361 RVA: 0x0024C33C File Offset: 0x0024A53C
			XamlToRtfError IXamlAttributes.GetValueFromName(string uri, string localName, ref string valueName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x0600863A RID: 34362 RVA: 0x0024C34C File Offset: 0x0024A54C
			XamlToRtfError IXamlAttributes.GetValueFromQName(string qName, ref string valueName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x0600863B RID: 34363 RVA: 0x0024C35C File Offset: 0x0024A55C
			XamlToRtfError IXamlAttributes.GetTypeFromQName(string qName, ref string typeName)
			{
				return XamlToRtfError.None;
			}

			// Token: 0x17001E5A RID: 7770
			// (get) Token: 0x0600863C RID: 34364 RVA: 0x0024C36C File Offset: 0x0024A56C
			internal bool IsEmpty
			{
				get
				{
					return this._xamlParsePoints.IsEmpty;
				}
			}

			// Token: 0x0400435B RID: 17243
			private XamlToRtfParser.XamlParsePoints _xamlParsePoints;
		}

		// Token: 0x02000922 RID: 2338
		internal class XamlParsePoints : ArrayList
		{
			// Token: 0x0600863D RID: 34365 RVA: 0x0024C120 File Offset: 0x0024A320
			internal XamlParsePoints() : base(10)
			{
			}

			// Token: 0x0600863E RID: 34366 RVA: 0x0024C37C File Offset: 0x0024A57C
			internal XamlToRtfError Init(string xaml)
			{
				XamlToRtfError result = XamlToRtfError.None;
				this._empty = false;
				this._valid = false;
				this.Clear();
				int i = 0;
				if (xaml.Length < 2 || xaml[0] != '<' || xaml[xaml.Length - 1] != '>')
				{
					return XamlToRtfError.Unknown;
				}
				i++;
				if (this.IsSpace(xaml[i]))
				{
					return XamlToRtfError.Unknown;
				}
				if (xaml[i] == '/')
				{
					return this.HandleEndTag(xaml, i);
				}
				int num = i;
				i++;
				while (this.IsNameChar(xaml[i]))
				{
					i++;
				}
				this.AddParseData(xaml.Substring(num, i - num));
				while (i < xaml.Length)
				{
					while (this.IsSpace(xaml[i]))
					{
						i++;
					}
					if (i == xaml.Length - 1)
					{
						break;
					}
					if (xaml[i] == '/')
					{
						if (i == xaml.Length - 2)
						{
							this._empty = true;
							break;
						}
						return XamlToRtfError.Unknown;
					}
					else
					{
						num = i;
						i++;
						while (this.IsNameChar(xaml[i]))
						{
							i++;
						}
						this.AddParseData(xaml.Substring(num, i - num));
						if (i < xaml.Length)
						{
							while (this.IsSpace(xaml[i]))
							{
								i++;
							}
						}
						if (i == xaml.Length || xaml[i] != '=')
						{
							return XamlToRtfError.Unknown;
						}
						i++;
						while (this.IsSpace(xaml[i]))
						{
							i++;
						}
						if (xaml[i] != '\'' && xaml[i] != '"')
						{
							return XamlToRtfError.Unknown;
						}
						char c = xaml[i++];
						num = i;
						while (i < xaml.Length && xaml[i] != c)
						{
							i++;
						}
						if (i == xaml.Length)
						{
							return XamlToRtfError.Unknown;
						}
						this.AddParseData(xaml.Substring(num, i - num));
						i++;
					}
				}
				this._valid = true;
				return result;
			}

			// Token: 0x0600863F RID: 34367 RVA: 0x000AA011 File Offset: 0x000A8211
			internal void AddParseData(string parseData)
			{
				this.Add(parseData);
			}

			// Token: 0x17001E5B RID: 7771
			// (get) Token: 0x06008640 RID: 34368 RVA: 0x0024C551 File Offset: 0x0024A751
			internal bool IsEmpty
			{
				get
				{
					return this._empty;
				}
			}

			// Token: 0x17001E5C RID: 7772
			// (get) Token: 0x06008641 RID: 34369 RVA: 0x0024C559 File Offset: 0x0024A759
			internal bool IsValid
			{
				get
				{
					return this._valid;
				}
			}

			// Token: 0x06008642 RID: 34370 RVA: 0x0024BE84 File Offset: 0x0024A084
			private bool IsSpace(char character)
			{
				return character == ' ' || character == '\t' || character == '\n' || character == '\r';
			}

			// Token: 0x06008643 RID: 34371 RVA: 0x0024C561 File Offset: 0x0024A761
			private bool IsNameChar(char character)
			{
				return !this.IsSpace(character) && character != '=' && character != '>' && character != '/';
			}

			// Token: 0x06008644 RID: 34372 RVA: 0x0024C580 File Offset: 0x0024A780
			private XamlToRtfError HandleEndTag(string xaml, int xamlIndex)
			{
				xamlIndex++;
				while (this.IsSpace(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				int num = xamlIndex;
				xamlIndex++;
				while (this.IsNameChar(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				this.AddParseData(xaml.Substring(num, xamlIndex - num));
				while (this.IsSpace(xaml[xamlIndex]))
				{
					xamlIndex++;
				}
				if (xamlIndex == xaml.Length - 1)
				{
					this._valid = true;
					return XamlToRtfError.None;
				}
				return XamlToRtfError.Unknown;
			}

			// Token: 0x0400435C RID: 17244
			private bool _empty;

			// Token: 0x0400435D RID: 17245
			private bool _valid;
		}

		// Token: 0x02000923 RID: 2339
		internal class XamlToken
		{
			// Token: 0x06008645 RID: 34373 RVA: 0x0000326D File Offset: 0x0000146D
			internal XamlToken()
			{
			}

			// Token: 0x17001E5D RID: 7773
			// (get) Token: 0x06008646 RID: 34374 RVA: 0x0024C600 File Offset: 0x0024A800
			// (set) Token: 0x06008647 RID: 34375 RVA: 0x0024C608 File Offset: 0x0024A808
			internal XamlTokenType TokenType
			{
				get
				{
					return this._tokenType;
				}
				set
				{
					this._tokenType = value;
				}
			}

			// Token: 0x17001E5E RID: 7774
			// (get) Token: 0x06008648 RID: 34376 RVA: 0x0024C611 File Offset: 0x0024A811
			// (set) Token: 0x06008649 RID: 34377 RVA: 0x0024C619 File Offset: 0x0024A819
			internal string Text
			{
				get
				{
					return this._text;
				}
				set
				{
					this._text = value;
				}
			}

			// Token: 0x0400435E RID: 17246
			private XamlTokenType _tokenType;

			// Token: 0x0400435F RID: 17247
			private string _text;
		}
	}
}
