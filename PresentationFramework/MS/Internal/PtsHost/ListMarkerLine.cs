using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200062B RID: 1579
	internal class ListMarkerLine : LineBase
	{
		// Token: 0x0600689C RID: 26780 RVA: 0x001D8204 File Offset: 0x001D6404
		internal ListMarkerLine(TextFormatterHost host, ListParaClient paraClient) : base(paraClient)
		{
			this._host = host;
		}

		// Token: 0x0600689D RID: 26781 RVA: 0x001D8214 File Offset: 0x001D6414
		internal override TextRun GetTextRun(int dcp)
		{
			return new ParagraphBreakRun(1, PTS.FSFLRES.fsflrEndOfParagraph);
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x001D821D File Offset: 0x001D641D
		internal override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			return new TextSpan<CultureSpecificCharacterBufferRange>(0, new CultureSpecificCharacterBufferRange(null, CharacterBufferRange.Empty));
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x00012630 File Offset: 0x00010830
		internal override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp)
		{
			return dcp;
		}

		// Token: 0x060068A0 RID: 26784 RVA: 0x001D8230 File Offset: 0x001D6430
		internal void FormatAndDrawVisual(DrawingContext ctx, LineProperties lineProps, int ur, int vrBaseline)
		{
			bool flag = lineProps.FlowDirection == FlowDirection.RightToLeft;
			this._host.Context = this;
			try
			{
				TextLine textLine = this._host.TextFormatter.FormatLine(this._host, 0, 0.0, lineProps.FirstLineProps, null, new TextRunCache());
				Point origin = new Point(TextDpi.FromTextDpi(ur), TextDpi.FromTextDpi(vrBaseline) - textLine.Baseline);
				textLine.Draw(ctx, origin, flag ? InvertAxes.Horizontal : InvertAxes.None);
				textLine.Dispose();
			}
			finally
			{
				this._host.Context = null;
			}
		}

		// Token: 0x040033E2 RID: 13282
		private readonly TextFormatterHost _host;
	}
}
