using System;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200064C RID: 1612
	internal sealed class TextFormatterHost : TextSource
	{
		// Token: 0x06006ADD RID: 27357 RVA: 0x001E958E File Offset: 0x001E778E
		internal TextFormatterHost(TextFormatter textFormatter, TextFormattingMode textFormattingMode, double pixelsPerDip)
		{
			if (textFormatter == null)
			{
				this.TextFormatter = TextFormatter.FromCurrentDispatcher(textFormattingMode);
			}
			else
			{
				this.TextFormatter = textFormatter;
			}
			base.PixelsPerDip = pixelsPerDip;
		}

		// Token: 0x06006ADE RID: 27358 RVA: 0x001E95B8 File Offset: 0x001E77B8
		public override TextRun GetTextRun(int textSourceCharacterIndex)
		{
			TextRun textRun = this.Context.GetTextRun(textSourceCharacterIndex);
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06006ADF RID: 27359 RVA: 0x001E95EC File Offset: 0x001E77EC
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
		{
			return this.Context.GetPrecedingText(textSourceCharacterIndexLimit);
		}

		// Token: 0x06006AE0 RID: 27360 RVA: 0x001E95FA File Offset: 0x001E77FA
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return this.Context.GetTextEffectCharacterIndexFromTextSourceCharacterIndex(textSourceCharacterIndex);
		}

		// Token: 0x04003456 RID: 13398
		internal LineBase Context;

		// Token: 0x04003457 RID: 13399
		internal TextFormatter TextFormatter;
	}
}
