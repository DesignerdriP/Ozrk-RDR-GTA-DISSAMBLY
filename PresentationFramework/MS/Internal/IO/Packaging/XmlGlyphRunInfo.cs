using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066A RID: 1642
	internal class XmlGlyphRunInfo : GlyphRunInfo
	{
		// Token: 0x06006C97 RID: 27799 RVA: 0x001F3E8B File Offset: 0x001F208B
		internal XmlGlyphRunInfo(XmlNode glyphsNode)
		{
			this._glyphsNode = (glyphsNode as XmlElement);
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x06006C98 RID: 27800 RVA: 0x001F3E9F File Offset: 0x001F209F
		internal override Point StartPosition
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x06006C99 RID: 27801 RVA: 0x001F3E9F File Offset: 0x001F209F
		internal override Point EndPosition
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019F9 RID: 6649
		// (get) Token: 0x06006C9A RID: 27802 RVA: 0x001F3E9F File Offset: 0x001F209F
		internal override double WidthEmFontSize
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x06006C9B RID: 27803 RVA: 0x001F3E9F File Offset: 0x001F209F
		internal override double HeightEmFontSize
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x06006C9C RID: 27804 RVA: 0x001F3E9F File Offset: 0x001F209F
		internal override bool GlyphsHaveSidewaysOrientation
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FC RID: 6652
		// (get) Token: 0x06006C9D RID: 27805 RVA: 0x001F3E9F File Offset: 0x001F209F
		internal override int BidiLevel
		{
			get
			{
				throw new NotSupportedException(SR.Get("XmlGlyphRunInfoIsNonGraphic"));
			}
		}

		// Token: 0x170019FD RID: 6653
		// (get) Token: 0x06006C9E RID: 27806 RVA: 0x001F3EB0 File Offset: 0x001F20B0
		internal override uint LanguageID
		{
			get
			{
				checked
				{
					if (this._languageID == null)
					{
						XmlElement xmlElement = this._glyphsNode;
						while (xmlElement != null && this._languageID == null)
						{
							string attribute = xmlElement.GetAttribute("xml:lang");
							if (attribute != null && attribute.Length > 0)
							{
								if (string.CompareOrdinal(attribute.ToUpperInvariant(), "UND") == 0)
								{
									this._languageID = new uint?(0U);
								}
								else
								{
									XmlLanguage language = XmlLanguage.GetLanguage(attribute);
									CultureInfo compatibleCulture = language.GetCompatibleCulture();
									this._languageID = new uint?((uint)compatibleCulture.LCID);
								}
							}
							xmlElement = (xmlElement.ParentNode as XmlElement);
						}
						if (this._languageID == null)
						{
							this._languageID = new uint?((uint)CultureInfo.InvariantCulture.LCID);
						}
					}
					return this._languageID.Value;
				}
			}
		}

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x06006C9F RID: 27807 RVA: 0x001F3F78 File Offset: 0x001F2178
		internal override string UnicodeString
		{
			get
			{
				if (this._unicodeString == null)
				{
					this._unicodeString = this._glyphsNode.GetAttribute("UnicodeString");
				}
				return this._unicodeString;
			}
		}

		// Token: 0x0400354D RID: 13645
		private const string _glyphRunName = "Glyphs";

		// Token: 0x0400354E RID: 13646
		private const string _xmlLangAttribute = "xml:lang";

		// Token: 0x0400354F RID: 13647
		private const string _unicodeStringAttribute = "UnicodeString";

		// Token: 0x04003550 RID: 13648
		private const string _undeterminedLanguageStringUpper = "UND";

		// Token: 0x04003551 RID: 13649
		private XmlElement _glyphsNode;

		// Token: 0x04003552 RID: 13650
		private string _unicodeString;

		// Token: 0x04003553 RID: 13651
		private uint? _languageID;
	}
}
