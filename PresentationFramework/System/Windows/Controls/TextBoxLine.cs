using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal;
using MS.Internal.Text;

namespace System.Windows.Controls
{
	// Token: 0x02000465 RID: 1125
	internal class TextBoxLine : TextSource, IDisposable
	{
		// Token: 0x0600412F RID: 16687 RVA: 0x0012A088 File Offset: 0x00128288
		internal TextBoxLine(TextBoxView owner)
		{
			this._owner = owner;
			base.PixelsPerDip = this._owner.GetDpi().PixelsPerDip;
		}

		// Token: 0x06004130 RID: 16688 RVA: 0x0012A0BB File Offset: 0x001282BB
		public void Dispose()
		{
			if (this._line != null)
			{
				this._line.Dispose();
				this._line = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06004131 RID: 16689 RVA: 0x0012A0E0 File Offset: 0x001282E0
		public override TextRun GetTextRun(int dcp)
		{
			TextRun textRun = null;
			StaticTextPointer position = this._owner.Host.TextContainer.CreateStaticPointerAtOffset(dcp);
			switch (position.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				textRun = new TextEndOfParagraph(1);
				goto IL_5C;
			case TextPointerContext.Text:
				textRun = this.HandleText(position);
				goto IL_5C;
			}
			Invariant.Assert(false, "Unsupported position type.");
			IL_5C:
			Invariant.Assert(textRun != null, "TextRun has not been created.");
			Invariant.Assert(textRun.Length > 0, "TextRun has to have positive length.");
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06004132 RID: 16690 RVA: 0x0012A184 File Offset: 0x00128384
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				ITextPointer textPointer = this._owner.Host.TextContainer.CreatePointerAtOffset(dcp, LogicalDirection.Backward);
				int num = Math.Min(128, textPointer.GetTextRunLength(LogicalDirection.Backward));
				char[] array = new char[num];
				textPointer.GetTextInRun(LogicalDirection.Backward, array, 0, num);
				empty = new CharacterBufferRange(array, 0, num);
				culture = DynamicPropertyReader.GetCultureInfo((Control)this._owner.Host);
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(empty.Length, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x06004133 RID: 16691 RVA: 0x00012630 File Offset: 0x00010830
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return textSourceCharacterIndex;
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x0012A210 File Offset: 0x00128410
		internal void Format(int dcp, double formatWidth, double paragraphWidth, LineProperties lineProperties, TextRunCache textRunCache, TextFormatter formatter)
		{
			this._lineProperties = lineProperties;
			this._dcp = dcp;
			this._paragraphWidth = paragraphWidth;
			lineProperties.IgnoreTextAlignment = (lineProperties.TextAlignment != TextAlignment.Justify);
			try
			{
				this._line = formatter.FormatLine(this, dcp, formatWidth, lineProperties, null, textRunCache);
			}
			finally
			{
				lineProperties.IgnoreTextAlignment = false;
			}
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x0012A278 File Offset: 0x00128478
		internal TextBoxLineDrawingVisual CreateVisual(Geometry selectionGeometry)
		{
			TextBoxLineDrawingVisual textBoxLineDrawingVisual = new TextBoxLineDrawingVisual();
			double x = this.CalculateXOffsetShift();
			DrawingContext drawingContext = textBoxLineDrawingVisual.RenderOpen();
			if (selectionGeometry != null)
			{
				TextBoxView owner = this._owner;
				FrameworkElement frameworkElement;
				if (owner == null)
				{
					frameworkElement = null;
				}
				else
				{
					ITextBoxViewHost host = owner.Host;
					if (host == null)
					{
						frameworkElement = null;
					}
					else
					{
						ITextContainer textContainer = host.TextContainer;
						if (textContainer == null)
						{
							frameworkElement = null;
						}
						else
						{
							ITextSelection textSelection = textContainer.TextSelection;
							if (textSelection == null)
							{
								frameworkElement = null;
							}
							else
							{
								TextEditor textEditor = textSelection.TextEditor;
								frameworkElement = ((textEditor != null) ? textEditor.UiScope : null);
							}
						}
					}
				}
				FrameworkElement frameworkElement2 = frameworkElement;
				if (frameworkElement2 != null)
				{
					Brush brush = frameworkElement2.GetValue(TextBoxBase.SelectionBrushProperty) as Brush;
					if (brush != null)
					{
						double opacity = (double)frameworkElement2.GetValue(TextBoxBase.SelectionOpacityProperty);
						drawingContext.PushOpacity(opacity);
						drawingContext.DrawGeometry(brush, new Pen
						{
							Brush = brush
						}, selectionGeometry);
						drawingContext.Pop();
					}
				}
			}
			this._line.Draw(drawingContext, new Point(x, 0.0), (this._lineProperties.FlowDirection == FlowDirection.RightToLeft) ? InvertAxes.Horizontal : InvertAxes.None);
			drawingContext.Close();
			return textBoxLineDrawingVisual;
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x0012A365 File Offset: 0x00128565
		internal Rect GetBoundsFromTextPosition(int characterIndex, out FlowDirection flowDirection)
		{
			return this.GetBoundsFromPosition(characterIndex, 1, out flowDirection);
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x0012A370 File Offset: 0x00128570
		internal List<Rect> GetRangeBounds(int cp, int cch, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>();
			double num = this.CalculateXOffsetShift();
			double num2 = xOffset + num;
			IList<TextBounds> textBounds = this._line.GetTextBounds(cp, cch);
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

		// Token: 0x06004138 RID: 16696 RVA: 0x0012A3F8 File Offset: 0x001285F8
		internal CharacterHit GetTextPositionFromDistance(double distance)
		{
			double num = this.CalculateXOffsetShift();
			return this._line.GetCharacterHitFromDistance(distance - num);
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x0012A41A File Offset: 0x0012861A
		internal CharacterHit GetNextCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetNextCaretCharacterHit(index);
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x0012A428 File Offset: 0x00128628
		internal CharacterHit GetPreviousCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetPreviousCaretCharacterHit(index);
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x0012A436 File Offset: 0x00128636
		internal CharacterHit GetBackspaceCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetBackspaceCaretCharacterHit(index);
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x0012A444 File Offset: 0x00128644
		internal bool IsAtCaretCharacterHit(CharacterHit charHit)
		{
			return this._line.IsAtCaretCharacterHit(charHit, this._dcp);
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x0600413D RID: 16701 RVA: 0x0012A458 File Offset: 0x00128658
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

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x0600413E RID: 16702 RVA: 0x0012A479 File Offset: 0x00128679
		internal double Height
		{
			get
			{
				return this._line.Height;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x0600413F RID: 16703 RVA: 0x0012A488 File Offset: 0x00128688
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

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06004140 RID: 16704 RVA: 0x0012A4CB File Offset: 0x001286CB
		internal int Length
		{
			get
			{
				return this._line.Length - (this.EndOfParagraph ? 1 : 0);
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06004141 RID: 16705 RVA: 0x0012A4E5 File Offset: 0x001286E5
		internal int ContentLength
		{
			get
			{
				return this._line.Length - this._line.NewlineLength;
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06004142 RID: 16706 RVA: 0x0012A4FE File Offset: 0x001286FE
		internal bool HasLineBreak
		{
			get
			{
				return this._line.NewlineLength > 0;
			}
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x0012A510 File Offset: 0x00128710
		private TextRun HandleText(StaticTextPointer position)
		{
			StaticTextPointer position2 = this._owner.Host.TextContainer.Highlights.GetNextPropertyChangePosition(position, LogicalDirection.Forward);
			if (position.GetOffsetToPosition(position2) > 4096)
			{
				position2 = position.CreatePointer(4096);
			}
			Highlights highlights = position.TextContainer.Highlights;
			TextDecorationCollection textDecorationCollection = highlights.GetHighlightValue(position, LogicalDirection.Forward, typeof(SpellerHighlightLayer)) as TextDecorationCollection;
			TextRunProperties textRunProperties = this._lineProperties.DefaultTextRunProperties;
			if (textDecorationCollection != null)
			{
				if (this._spellerErrorProperties == null)
				{
					this._spellerErrorProperties = new TextProperties((TextProperties)textRunProperties, textDecorationCollection);
				}
				textRunProperties = this._spellerErrorProperties;
			}
			ITextSelection textSelection = position.TextContainer.TextSelection;
			TextEditor textEditor = (textSelection != null) ? textSelection.TextEditor : null;
			if (textEditor != null)
			{
				ITextView textView = textEditor.TextView;
				if (((textView != null) ? new bool?(textView.RendersOwnSelection) : null) == true && highlights.GetHighlightValue(position, LogicalDirection.Forward, typeof(TextSelection)) != DependencyProperty.UnsetValue)
				{
					TextProperties textProperties = new TextProperties((TextProperties)textRunProperties, textDecorationCollection);
					FrameworkElement frameworkElement = (textEditor != null) ? textEditor.UiScope : null;
					if (frameworkElement != null)
					{
						textProperties.SetBackgroundBrush(null);
						Brush brush = frameworkElement.GetValue(TextBoxBase.SelectionTextBrushProperty) as Brush;
						if (brush != null)
						{
							textProperties.SetForegroundBrush(brush);
						}
					}
					textRunProperties = textProperties;
				}
			}
			char[] array = new char[position.GetOffsetToPosition(position2)];
			int textInRun = position.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
			Invariant.Assert(textInRun == array.Length);
			return new TextCharacters(array, 0, textInRun, textRunProperties);
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x0012A6A8 File Offset: 0x001288A8
		private Rect GetBoundsFromPosition(int cp, int cch, out FlowDirection flowDirection)
		{
			double num = this.CalculateXOffsetShift();
			IList<TextBounds> textBounds = this._line.GetTextBounds(cp, cch);
			Invariant.Assert(textBounds != null && textBounds.Count == 1, "Expecting exactly one TextBounds for a single text position.");
			IList<TextRunBounds> textRunBounds = textBounds[0].TextRunBounds;
			Rect rectangle;
			if (textRunBounds != null)
			{
				Invariant.Assert(textRunBounds.Count == 1, "Expecting exactly one TextRunBounds for a single text position.");
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

		// Token: 0x06004145 RID: 16709 RVA: 0x0012A740 File Offset: 0x00128940
		private double CalculateXOffsetShift()
		{
			double num = 0.0;
			if (this._lineProperties.TextAlignmentInternal == TextAlignment.Right)
			{
				num = this._paragraphWidth - this._line.Width;
			}
			else if (this._lineProperties.TextAlignmentInternal == TextAlignment.Center)
			{
				num = (this._paragraphWidth - this._line.Width) / 2.0;
			}
			if (this.IsXOffsetAdjusted)
			{
				if (this._lineProperties.TextAlignmentInternal == TextAlignment.Center)
				{
					num += (this._line.Width - this._line.WidthIncludingTrailingWhitespace) / 2.0;
				}
				else
				{
					num += this._line.Width - this._line.WidthIncludingTrailingWhitespace;
				}
			}
			return num;
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06004146 RID: 16710 RVA: 0x0012A7FC File Offset: 0x001289FC
		private bool IsXOffsetAdjusted
		{
			get
			{
				return (this._lineProperties.TextAlignmentInternal == TextAlignment.Right || this._lineProperties.TextAlignmentInternal == TextAlignment.Center) && this.IsWidthAdjusted;
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06004147 RID: 16711 RVA: 0x0012A822 File Offset: 0x00128A22
		private bool IsWidthAdjusted
		{
			get
			{
				return this.HasLineBreak || this.EndOfParagraph;
			}
		}

		// Token: 0x0400279B RID: 10139
		private readonly TextBoxView _owner;

		// Token: 0x0400279C RID: 10140
		private TextLine _line;

		// Token: 0x0400279D RID: 10141
		private int _dcp;

		// Token: 0x0400279E RID: 10142
		private LineProperties _lineProperties;

		// Token: 0x0400279F RID: 10143
		private TextProperties _spellerErrorProperties;

		// Token: 0x040027A0 RID: 10144
		private double _paragraphWidth;

		// Token: 0x040027A1 RID: 10145
		private const int _syntheticCharacterLength = 1;
	}
}
