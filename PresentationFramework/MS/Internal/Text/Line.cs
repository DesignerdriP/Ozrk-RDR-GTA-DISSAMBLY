using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.Text
{
	// Token: 0x02000600 RID: 1536
	internal abstract class Line : TextSource, IDisposable
	{
		// Token: 0x0600662B RID: 26155 RVA: 0x001CB7EF File Offset: 0x001C99EF
		public void Dispose()
		{
			if (this._line != null)
			{
				this._line.Dispose();
				this._line = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600662C RID: 26156 RVA: 0x001CB814 File Offset: 0x001C9A14
		internal Line(TextBlock owner)
		{
			this._owner = owner;
			this._textAlignment = owner.TextAlignment;
			this._showParagraphEllipsis = false;
			this._wrappingWidth = this._owner.RenderSize.Width;
			base.PixelsPerDip = this._owner.GetDpi().PixelsPerDip;
		}

		// Token: 0x0600662D RID: 26157 RVA: 0x001CB874 File Offset: 0x001C9A74
		internal void Format(int dcp, double width, TextParagraphProperties lineProperties, TextLineBreak textLineBreak, TextRunCache textRunCache, bool showParagraphEllipsis)
		{
			this._mirror = (lineProperties.FlowDirection == FlowDirection.RightToLeft);
			this._dcp = dcp;
			this._showParagraphEllipsis = showParagraphEllipsis;
			this._wrappingWidth = width;
			this._line = this._owner.TextFormatter.FormatLine(this, dcp, width, lineProperties, textLineBreak, textRunCache);
		}

		// Token: 0x0600662E RID: 26158 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void Arrange(VisualCollection vc, Vector lineOffset)
		{
		}

		// Token: 0x0600662F RID: 26159 RVA: 0x001CB8C4 File Offset: 0x001C9AC4
		internal void Render(DrawingContext ctx, Point lineOffset)
		{
			TextLine textLine = this._line;
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
			}
			double num = this.CalculateXOffsetShift();
			textLine.Draw(ctx, new Point(lineOffset.X + num, lineOffset.Y), this._mirror ? InvertAxes.Horizontal : InvertAxes.None);
		}

		// Token: 0x06006630 RID: 26160 RVA: 0x001CB952 File Offset: 0x001C9B52
		internal Rect GetBoundsFromTextPosition(int characterIndex, out FlowDirection flowDirection)
		{
			return this.GetBoundsFromPosition(characterIndex, 1, out flowDirection);
		}

		// Token: 0x06006631 RID: 26161 RVA: 0x001CB960 File Offset: 0x001C9B60
		internal List<Rect> GetRangeBounds(int cp, int cch, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>();
			double num = this.CalculateXOffsetShift();
			double num2 = xOffset + num;
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				textBounds = textLine.GetTextBounds(cp, cch);
			}
			else
			{
				textBounds = this._line.GetTextBounds(cp, cch);
			}
			Invariant.Assert(textBounds.Count > 0);
			for (int i = 0; i < textBounds.Count; i++)
			{
				Rect rectangle = textBounds[i].Rectangle;
				rectangle.X += num2;
				rectangle.Y += yOffset;
				list.Add(rectangle);
			}
			return list;
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x001CBA64 File Offset: 0x001C9C64
		internal CharacterHit GetTextPositionFromDistance(double distance)
		{
			double num = this.CalculateXOffsetShift();
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				return textLine.GetCharacterHitFromDistance(distance);
			}
			return this._line.GetCharacterHitFromDistance(distance - num);
		}

		// Token: 0x06006633 RID: 26163 RVA: 0x001CBAFD File Offset: 0x001C9CFD
		internal CharacterHit GetNextCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetNextCaretCharacterHit(index);
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x001CBB0B File Offset: 0x001C9D0B
		internal CharacterHit GetPreviousCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetPreviousCaretCharacterHit(index);
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x001CBB19 File Offset: 0x001C9D19
		internal CharacterHit GetBackspaceCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetBackspaceCaretCharacterHit(index);
		}

		// Token: 0x06006636 RID: 26166 RVA: 0x001CBB27 File Offset: 0x001C9D27
		internal bool IsAtCaretCharacterHit(CharacterHit charHit)
		{
			return this._line.IsAtCaretCharacterHit(charHit, this._dcp);
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool HasInlineObjects()
		{
			return false;
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual IInputElement InputHitTest(double offset)
		{
			return null;
		}

		// Token: 0x06006639 RID: 26169 RVA: 0x001CBB3B File Offset: 0x001C9D3B
		internal TextLineBreak GetTextLineBreak()
		{
			if (this._line == null)
			{
				return null;
			}
			return this._line.GetTextLineBreak();
		}

		// Token: 0x0600663A RID: 26170 RVA: 0x001CBB54 File Offset: 0x001C9D54
		internal int GetEllipsesLength()
		{
			if (!this._line.HasOverflowed)
			{
				return 0;
			}
			if (this._owner.ParagraphProperties.TextTrimming == TextTrimming.None)
			{
				return 0;
			}
			TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
			{
				this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
			});
			IList<TextCollapsedRange> textCollapsedRanges = textLine.GetTextCollapsedRanges();
			if (textCollapsedRanges != null)
			{
				TextCollapsedRange textCollapsedRange = textCollapsedRanges[0];
				return textCollapsedRange.Length;
			}
			return 0;
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x001CBBCC File Offset: 0x001C9DCC
		internal double GetCollapsedWidth()
		{
			if (!this._line.HasOverflowed)
			{
				return this.Width;
			}
			if (this._owner.ParagraphProperties.TextTrimming == TextTrimming.None)
			{
				return this.Width;
			}
			TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
			{
				this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
			});
			return textLine.Width;
		}

		// Token: 0x1700187C RID: 6268
		// (get) Token: 0x0600663C RID: 26172 RVA: 0x001CBC38 File Offset: 0x001C9E38
		internal double Width
		{
			get
			{
				if (this.IsWidthAdjusted)
				{
					return this._line.WidthIncludingTrailingWhitespace;
				}
				return this._line.Width;
			}
		}

		// Token: 0x1700187D RID: 6269
		// (get) Token: 0x0600663D RID: 26173 RVA: 0x001CBC59 File Offset: 0x001C9E59
		internal double Start
		{
			get
			{
				if (this.IsXOffsetAdjusted)
				{
					return this._line.Start + this.CalculateXOffsetShift();
				}
				return this._line.Start;
			}
		}

		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x0600663E RID: 26174 RVA: 0x001CBC81 File Offset: 0x001C9E81
		internal double Height
		{
			get
			{
				return this._line.Height;
			}
		}

		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x0600663F RID: 26175 RVA: 0x001CBC8E File Offset: 0x001C9E8E
		internal double BaselineOffset
		{
			get
			{
				return this._line.Baseline;
			}
		}

		// Token: 0x17001880 RID: 6272
		// (get) Token: 0x06006640 RID: 26176 RVA: 0x001CBC9C File Offset: 0x001C9E9C
		internal bool EndOfParagraph
		{
			get
			{
				if (this._line.NewlineLength == 0)
				{
					return false;
				}
				IList<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
				return textRunSpans[textRunSpans.Count - 1].Value is TextEndOfParagraph;
			}
		}

		// Token: 0x17001881 RID: 6273
		// (get) Token: 0x06006641 RID: 26177 RVA: 0x001CBCDF File Offset: 0x001C9EDF
		internal int Length
		{
			get
			{
				return this._line.Length - (this.EndOfParagraph ? Line._syntheticCharacterLength : 0);
			}
		}

		// Token: 0x17001882 RID: 6274
		// (get) Token: 0x06006642 RID: 26178 RVA: 0x001CBCFD File Offset: 0x001C9EFD
		internal int ContentLength
		{
			get
			{
				return this._line.Length - this._line.NewlineLength;
			}
		}

		// Token: 0x06006643 RID: 26179 RVA: 0x001CBD18 File Offset: 0x001C9F18
		protected Rect GetBoundsFromPosition(int cp, int cch, out FlowDirection flowDirection)
		{
			double num = this.CalculateXOffsetShift();
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
				});
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				textBounds = textLine.GetTextBounds(cp, cch);
			}
			else
			{
				textBounds = this._line.GetTextBounds(cp, cch);
			}
			Invariant.Assert(textBounds != null && textBounds.Count == 1, "Expecting exactly one TextBounds for a single text position.");
			IList<TextRunBounds> textRunBounds = textBounds[0].TextRunBounds;
			Rect rectangle;
			if (textRunBounds != null)
			{
				rectangle = textRunBounds[0].Rectangle;
			}
			else
			{
				rectangle = textBounds[0].Rectangle;
			}
			rectangle.X += num;
			flowDirection = textBounds[0].FlowDirection;
			return rectangle;
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x001CBE1C File Offset: 0x001CA01C
		protected TextCollapsingProperties GetCollapsingProps(double wrappingWidth, LineProperties paraProperties)
		{
			TextCollapsingProperties result;
			if (paraProperties.TextTrimming == TextTrimming.CharacterEllipsis)
			{
				result = new TextTrailingCharacterEllipsis(wrappingWidth, paraProperties.DefaultTextRunProperties);
			}
			else
			{
				result = new TextTrailingWordEllipsis(wrappingWidth, paraProperties.DefaultTextRunProperties);
			}
			return result;
		}

		// Token: 0x06006645 RID: 26181 RVA: 0x001CBE50 File Offset: 0x001CA050
		protected double CalculateXOffsetShift()
		{
			if (!this.IsXOffsetAdjusted)
			{
				return 0.0;
			}
			if (this._textAlignment == TextAlignment.Center)
			{
				return (this._line.Width - this._line.WidthIncludingTrailingWhitespace) / 2.0;
			}
			return this._line.Width - this._line.WidthIncludingTrailingWhitespace;
		}

		// Token: 0x17001883 RID: 6275
		// (get) Token: 0x06006646 RID: 26182 RVA: 0x001CBEB1 File Offset: 0x001CA0B1
		protected bool ShowEllipsis
		{
			get
			{
				return this._owner.ParagraphProperties.TextTrimming != TextTrimming.None && (this._line.HasOverflowed || this._showParagraphEllipsis);
			}
		}

		// Token: 0x17001884 RID: 6276
		// (get) Token: 0x06006647 RID: 26183 RVA: 0x001CBEDF File Offset: 0x001CA0DF
		protected bool HasLineBreak
		{
			get
			{
				return this._line.NewlineLength > 0;
			}
		}

		// Token: 0x17001885 RID: 6277
		// (get) Token: 0x06006648 RID: 26184 RVA: 0x001CBEEF File Offset: 0x001CA0EF
		protected bool IsXOffsetAdjusted
		{
			get
			{
				return (this._textAlignment == TextAlignment.Right || this._textAlignment == TextAlignment.Center) && this.IsWidthAdjusted;
			}
		}

		// Token: 0x17001886 RID: 6278
		// (get) Token: 0x06006649 RID: 26185 RVA: 0x001CBF0C File Offset: 0x001CA10C
		protected bool IsWidthAdjusted
		{
			get
			{
				bool result = false;
				if ((this.HasLineBreak || this.EndOfParagraph) && !this.ShowEllipsis)
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x040032F7 RID: 13047
		protected TextBlock _owner;

		// Token: 0x040032F8 RID: 13048
		protected TextLine _line;

		// Token: 0x040032F9 RID: 13049
		protected int _dcp;

		// Token: 0x040032FA RID: 13050
		protected static int _syntheticCharacterLength = 1;

		// Token: 0x040032FB RID: 13051
		protected bool _mirror;

		// Token: 0x040032FC RID: 13052
		protected TextAlignment _textAlignment;

		// Token: 0x040032FD RID: 13053
		protected bool _showParagraphEllipsis;

		// Token: 0x040032FE RID: 13054
		protected double _wrappingWidth;
	}
}
