using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000604 RID: 1540
	internal sealed class SimpleLine : Line
	{
		// Token: 0x0600666D RID: 26221 RVA: 0x001CC3BC File Offset: 0x001CA5BC
		public override TextRun GetTextRun(int dcp)
		{
			TextRun textRun;
			if (dcp < this._content.Length)
			{
				textRun = new TextCharacters(this._content, dcp, this._content.Length - dcp, this._textProps);
			}
			else
			{
				textRun = new TextEndOfParagraph(Line._syntheticCharacterLength);
			}
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x0600666E RID: 26222 RVA: 0x001CC420 File Offset: 0x001CA620
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				empty = new CharacterBufferRange(this._content, 0, Math.Min(dcp, this._content.Length));
				culture = this._textProps.CultureInfo;
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(dcp, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x00012630 File Offset: 0x00010830
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return textSourceCharacterIndex;
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x001CC471 File Offset: 0x001CA671
		internal SimpleLine(TextBlock owner, string content, TextRunProperties textProps) : base(owner)
		{
			this._content = content;
			this._textProps = textProps;
		}

		// Token: 0x04003316 RID: 13078
		private readonly string _content;

		// Token: 0x04003317 RID: 13079
		private readonly TextRunProperties _textProps;
	}
}
