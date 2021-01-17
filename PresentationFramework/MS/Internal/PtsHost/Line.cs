using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000626 RID: 1574
	internal sealed class Line : LineBase
	{
		// Token: 0x0600685F RID: 26719 RVA: 0x001D6A34 File Offset: 0x001D4C34
		internal Line(TextFormatterHost host, TextParaClient paraClient, int cpPara) : base(paraClient)
		{
			this._host = host;
			this._cpPara = cpPara;
			this._textAlignment = (TextAlignment)this.TextParagraph.Element.GetValue(Block.TextAlignmentProperty);
			this._indent = 0.0;
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x001D6A94 File Offset: 0x001D4C94
		public override void Dispose()
		{
			try
			{
				if (this._line != null)
				{
					this._line.Dispose();
				}
			}
			finally
			{
				this._line = null;
				this._runs = null;
				this._hasFigures = false;
				this._hasFloaters = false;
				base.Dispose();
			}
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x001D6AEC File Offset: 0x001D4CEC
		internal void GetDvrSuppressibleBottomSpace(out int dvrSuppressible)
		{
			dvrSuppressible = Math.Max(0, TextDpi.ToTextDpi(this._line.OverhangAfter));
		}

		// Token: 0x06006862 RID: 26722 RVA: 0x001D6B08 File Offset: 0x001D4D08
		internal void GetDurFigureAnchor(FigureParagraph paraFigure, uint fswdir, out int dur)
		{
			int cpfromElement = TextContainerHelper.GetCPFromElement(this._paraClient.Paragraph.StructuralCache.TextContainer, paraFigure.Element, ElementEdge.BeforeStart);
			int firstCharacterIndex = cpfromElement - this._cpPara;
			double distanceFromCharacterHit = this._line.GetDistanceFromCharacterHit(new CharacterHit(firstCharacterIndex, 0));
			dur = TextDpi.ToTextDpi(distanceFromCharacterHit);
		}

		// Token: 0x06006863 RID: 26723 RVA: 0x001D6B5C File Offset: 0x001D4D5C
		internal override TextRun GetTextRun(int dcp)
		{
			TextRun textRun = null;
			ITextContainer textContainer = this._paraClient.Paragraph.StructuralCache.TextContainer;
			StaticTextPointer position = textContainer.CreateStaticPointerAtOffset(this._cpPara + dcp);
			switch (position.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				textRun = new ParagraphBreakRun(LineBase._syntheticCharacterLength, PTS.FSFLRES.fsflrEndOfParagraph);
				break;
			case TextPointerContext.Text:
				textRun = base.HandleText(position);
				break;
			case TextPointerContext.EmbeddedElement:
				textRun = base.HandleEmbeddedObject(dcp, position);
				break;
			case TextPointerContext.ElementStart:
				textRun = base.HandleElementStartEdge(position);
				break;
			case TextPointerContext.ElementEnd:
				textRun = base.HandleElementEndEdge(position);
				break;
			}
			Invariant.Assert(textRun != null, "TextRun has not been created.");
			Invariant.Assert(textRun.Length > 0, "TextRun has to have positive length.");
			return textRun;
		}

		// Token: 0x06006864 RID: 26724 RVA: 0x001D6C0C File Offset: 0x001D4E0C
		internal override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			Invariant.Assert(dcp >= 0);
			int num = 0;
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara, LogicalDirection.Forward);
				ITextPointer textPointerFromCP2 = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, this._cpPara + dcp, LogicalDirection.Forward);
				while (textPointerFromCP2.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text && textPointerFromCP2.CompareTo(textPointerFromCP) != 0)
				{
					textPointerFromCP2.MoveByOffset(-1);
					num++;
				}
				string textInRun = textPointerFromCP2.GetTextInRun(LogicalDirection.Backward);
				empty = new CharacterBufferRange(textInRun, 0, textInRun.Length);
				StaticTextPointer staticTextPointer = textPointerFromCP2.CreateStaticPointer();
				DependencyObject element = (staticTextPointer.Parent != null) ? staticTextPointer.Parent : this._paraClient.Paragraph.Element;
				culture = DynamicPropertyReader.GetCultureInfo(element);
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(num + empty.Length, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x06006865 RID: 26725 RVA: 0x001D6D06 File Offset: 0x001D4F06
		internal override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp)
		{
			return this._cpPara + dcp;
		}

		// Token: 0x06006866 RID: 26726 RVA: 0x001D6D10 File Offset: 0x001D4F10
		internal void Format(Line.FormattingContext ctx, int dcp, int width, int trackWidth, TextParagraphProperties lineProps, TextLineBreak textLineBreak)
		{
			this._formattingContext = ctx;
			this._dcp = dcp;
			this._host.Context = this;
			this._wrappingWidth = TextDpi.FromTextDpi(width);
			this._trackWidth = TextDpi.FromTextDpi(trackWidth);
			this._mirror = (lineProps.FlowDirection == FlowDirection.RightToLeft);
			this._indent = lineProps.Indent;
			try
			{
				if (ctx.LineFormatLengthTarget == -1)
				{
					this._line = this._host.TextFormatter.FormatLine(this._host, dcp, this._wrappingWidth, lineProps, textLineBreak, ctx.TextRunCache);
				}
				else
				{
					this._line = this._host.TextFormatter.RecreateLine(this._host, dcp, ctx.LineFormatLengthTarget, this._wrappingWidth, lineProps, textLineBreak, ctx.TextRunCache);
				}
				this._runs = this._line.GetTextRunSpans();
				Invariant.Assert(this._runs != null, "Cannot retrieve runs collection.");
				if (this._formattingContext.MeasureMode)
				{
					List<InlineObject> list = new List<InlineObject>(1);
					int num = this._dcp;
					foreach (TextSpan<TextRun> textSpan in this._runs)
					{
						TextRun value = textSpan.Value;
						if (value is InlineObjectRun)
						{
							list.Add(new InlineObject(num, ((InlineObjectRun)value).UIElementIsland, (TextParagraph)this._paraClient.Paragraph));
						}
						else if (value is FloatingRun)
						{
							if (((FloatingRun)value).Figure)
							{
								this._hasFigures = true;
							}
							else
							{
								this._hasFloaters = true;
							}
						}
						num += textSpan.Length;
					}
					if (list.Count == 0)
					{
						list = null;
					}
					this.TextParagraph.SubmitInlineObjects(dcp, dcp + this.ActualLength, list);
				}
			}
			finally
			{
				this._host.Context = null;
			}
		}

		// Token: 0x06006867 RID: 26727 RVA: 0x001D6F10 File Offset: 0x001D5110
		internal Size MeasureChild(InlineObjectRun inlineObject)
		{
			Size result;
			if (this._formattingContext.MeasureMode)
			{
				double height = this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.DocumentPageSize.Height;
				if (!this._paraClient.Paragraph.StructuralCache.CurrentFormatContext.FinitePage)
				{
					height = double.PositiveInfinity;
				}
				result = inlineObject.UIElementIsland.DoLayout(new Size(this._trackWidth, height), true, true);
			}
			else
			{
				result = inlineObject.UIElementIsland.Root.DesiredSize;
			}
			return result;
		}

		// Token: 0x06006868 RID: 26728 RVA: 0x001D6FA4 File Offset: 0x001D51A4
		internal ContainerVisual CreateVisual()
		{
			LineVisual lineVisual = new LineVisual();
			this._host.Context = this;
			try
			{
				IList<TextSpan<TextRun>> list = this._runs;
				TextLine textLine = this._line;
				if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
				{
					textLine = this._line.Collapse(new TextCollapsingProperties[]
					{
						this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
					});
					Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
					list = textLine.GetTextRunSpans();
				}
				if (this.HasInlineObjects())
				{
					VisualCollection children = lineVisual.Children;
					DependencyObject element = this._paraClient.Paragraph.Element;
					FlowDirection parentFD = (FlowDirection)element.GetValue(FrameworkElement.FlowDirectionProperty);
					int num = this._dcp;
					foreach (TextSpan<TextRun> textSpan in list)
					{
						TextRun value = textSpan.Value;
						if (value is InlineObjectRun)
						{
							InlineObjectRun inlineObjectRun = (InlineObjectRun)value;
							FlowDirection flowDirection;
							Rect boundsFromPosition = this.GetBoundsFromPosition(num, value.Length, out flowDirection);
							Visual visual = VisualTreeHelper.GetParent(inlineObjectRun.UIElementIsland) as Visual;
							if (visual != null)
							{
								ContainerVisual containerVisual = visual as ContainerVisual;
								Invariant.Assert(containerVisual != null, "Parent should always derives from ContainerVisual.");
								containerVisual.Children.Remove(inlineObjectRun.UIElementIsland);
							}
							if (!textLine.HasCollapsed || boundsFromPosition.Left + inlineObjectRun.UIElementIsland.Root.DesiredSize.Width < textLine.Width)
							{
								if (inlineObjectRun.UIElementIsland.Root is FrameworkElement)
								{
									DependencyObject parent = ((FrameworkElement)inlineObjectRun.UIElementIsland.Root).Parent;
									FlowDirection childFD = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
									PtsHelper.UpdateMirroringTransform(parentFD, childFD, inlineObjectRun.UIElementIsland, boundsFromPosition.Width);
								}
								children.Add(inlineObjectRun.UIElementIsland);
								inlineObjectRun.UIElementIsland.Offset = new Vector(boundsFromPosition.Left, boundsFromPosition.Top);
							}
						}
						num += textSpan.Length;
					}
				}
				double x = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
				DrawingContext drawingContext = lineVisual.Open();
				textLine.Draw(drawingContext, new Point(x, 0.0), this._mirror ? InvertAxes.Horizontal : InvertAxes.None);
				drawingContext.Close();
				lineVisual.WidthIncludingTrailingWhitespace = textLine.WidthIncludingTrailingWhitespace - this._indent;
			}
			finally
			{
				this._host.Context = null;
			}
			return lineVisual;
		}

		// Token: 0x06006869 RID: 26729 RVA: 0x001D7270 File Offset: 0x001D5470
		internal Rect GetBoundsFromTextPosition(int textPosition, out FlowDirection flowDirection)
		{
			return this.GetBoundsFromPosition(textPosition, 1, out flowDirection);
		}

		// Token: 0x0600686A RID: 26730 RVA: 0x001D727C File Offset: 0x001D547C
		internal List<Rect> GetRangeBounds(int cp, int cch, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>();
			double num = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
			double num2 = xOffset + num;
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
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

		// Token: 0x0600686B RID: 26731 RVA: 0x001D7384 File Offset: 0x001D5584
		internal TextLineBreak GetTextLineBreak()
		{
			if (this._line == null)
			{
				return null;
			}
			return this._line.GetTextLineBreak();
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x001D739C File Offset: 0x001D559C
		internal CharacterHit GetTextPositionFromDistance(int urDistance)
		{
			int num = this.CalculateUOffsetShift();
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
				});
				Invariant.Assert(num == 0);
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				return textLine.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urDistance));
			}
			return this._line.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urDistance - num));
		}

		// Token: 0x0600686D RID: 26733 RVA: 0x001D7434 File Offset: 0x001D5634
		internal IInputElement InputHitTest(int urOffset)
		{
			DependencyObject dependencyObject = null;
			int num = this.CalculateUOffsetShift();
			CharacterHit characterHitFromDistance;
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(num == 0);
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
				});
				Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
				characterHitFromDistance = textLine.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urOffset));
			}
			else
			{
				characterHitFromDistance = this._line.GetCharacterHitFromDistance(TextDpi.FromTextDpi(urOffset - num));
			}
			int cp = this._paraClient.Paragraph.ParagraphStartCharacterPosition + characterHitFromDistance.FirstCharacterIndex + characterHitFromDistance.TrailingLength;
			TextPointer textPointer = TextContainerHelper.GetTextPointerFromCP(this._paraClient.Paragraph.StructuralCache.TextContainer, cp, LogicalDirection.Forward) as TextPointer;
			if (textPointer != null)
			{
				TextPointerContext pointerContext = textPointer.GetPointerContext((characterHitFromDistance.TrailingLength == 0) ? LogicalDirection.Forward : LogicalDirection.Backward);
				if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.ElementEnd)
				{
					dependencyObject = textPointer.Parent;
				}
				else if (pointerContext == TextPointerContext.ElementStart)
				{
					dependencyObject = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
				}
			}
			return dependencyObject as IInputElement;
		}

		// Token: 0x0600686E RID: 26734 RVA: 0x001D7558 File Offset: 0x001D5758
		internal int GetEllipsesLength()
		{
			if (!this._line.HasOverflowed)
			{
				return 0;
			}
			if (this.TextParagraph.Properties.TextTrimming == TextTrimming.None)
			{
				return 0;
			}
			TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
			{
				this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
			});
			Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
			IList<TextCollapsedRange> textCollapsedRanges = textLine.GetTextCollapsedRanges();
			if (textCollapsedRanges != null)
			{
				Invariant.Assert(textCollapsedRanges.Count == 1, "Multiple collapsed ranges are not supported.");
				TextCollapsedRange textCollapsedRange = textCollapsedRanges[0];
				return textCollapsedRange.Length;
			}
			return 0;
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x001D75F4 File Offset: 0x001D57F4
		internal void GetGlyphRuns(List<GlyphRun> glyphRuns, int dcpStart, int dcpEnd)
		{
			int num = dcpStart - this._dcp;
			int num2 = dcpEnd - dcpStart;
			IList<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
			DrawingGroup drawingGroup = new DrawingGroup();
			DrawingContext drawingContext = drawingGroup.Open();
			double x = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
			this._line.Draw(drawingContext, new Point(x, 0.0), InvertAxes.None);
			drawingContext.Close();
			int num3 = 0;
			ArrayList arrayList = new ArrayList(4);
			this.AddGlyphRunRecursive(drawingGroup, arrayList, ref num3);
			int num4 = 0;
			using (IEnumerator<TextSpan<TextRun>> enumerator = textRunSpans.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TextSpan<TextRun> textSpan = enumerator.Current;
					if (textSpan.Value is TextCharacters)
					{
						num4 += textSpan.Length;
					}
				}
				goto IL_EA;
			}
			IL_B6:
			GlyphRun glyphRun = (GlyphRun)arrayList[0];
			num3 -= ((glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count);
			arrayList.RemoveAt(0);
			IL_EA:
			if (num3 <= num4)
			{
				int num5 = 0;
				int num6 = 0;
				foreach (TextSpan<TextRun> textSpan2 in textRunSpans)
				{
					if (textSpan2.Value is TextCharacters)
					{
						int i = 0;
						while (i < textSpan2.Length)
						{
							Invariant.Assert(num6 < arrayList.Count);
							GlyphRun glyphRun2 = (GlyphRun)arrayList[num6];
							int num7 = (glyphRun2.Characters == null) ? 0 : glyphRun2.Characters.Count;
							if (num < num5 + num7 && num + num2 > num5)
							{
								glyphRuns.Add(glyphRun2);
							}
							i += num7;
							num6++;
						}
						Invariant.Assert(i == textSpan2.Length);
						if (num + num2 <= num5 + textSpan2.Length)
						{
							break;
						}
					}
					num5 += textSpan2.Length;
				}
				return;
			}
			goto IL_B6;
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x001D77F0 File Offset: 0x001D59F0
		internal CharacterHit GetNextCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetNextCaretCharacterHit(index);
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x001D77FE File Offset: 0x001D59FE
		internal CharacterHit GetPreviousCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetPreviousCaretCharacterHit(index);
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x001D780C File Offset: 0x001D5A0C
		internal CharacterHit GetBackspaceCaretCharacterHit(CharacterHit index)
		{
			return this._line.GetBackspaceCaretCharacterHit(index);
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x001D781A File Offset: 0x001D5A1A
		internal bool IsAtCaretCharacterHit(CharacterHit charHit)
		{
			return this._line.IsAtCaretCharacterHit(charHit, this._dcp);
		}

		// Token: 0x17001934 RID: 6452
		// (get) Token: 0x06006874 RID: 26740 RVA: 0x001D782E File Offset: 0x001D5A2E
		internal int Start
		{
			get
			{
				return TextDpi.ToTextDpi(this._line.Start) + TextDpi.ToTextDpi(this._indent) + this.CalculateUOffsetShift();
			}
		}

		// Token: 0x17001935 RID: 6453
		// (get) Token: 0x06006875 RID: 26741 RVA: 0x001D7854 File Offset: 0x001D5A54
		internal int Width
		{
			get
			{
				int num;
				if (this.IsWidthAdjusted)
				{
					num = TextDpi.ToTextDpi(this._line.WidthIncludingTrailingWhitespace) - TextDpi.ToTextDpi(this._indent);
				}
				else
				{
					num = TextDpi.ToTextDpi(this._line.Width) - TextDpi.ToTextDpi(this._indent);
				}
				Invariant.Assert(num >= 0, "Line width cannot be negative");
				return num;
			}
		}

		// Token: 0x17001936 RID: 6454
		// (get) Token: 0x06006876 RID: 26742 RVA: 0x001D78B7 File Offset: 0x001D5AB7
		internal int Height
		{
			get
			{
				return TextDpi.ToTextDpi(this._line.Height);
			}
		}

		// Token: 0x17001937 RID: 6455
		// (get) Token: 0x06006877 RID: 26743 RVA: 0x001D78C9 File Offset: 0x001D5AC9
		internal int Baseline
		{
			get
			{
				return TextDpi.ToTextDpi(this._line.Baseline);
			}
		}

		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x06006878 RID: 26744 RVA: 0x001D78DB File Offset: 0x001D5ADB
		internal bool EndOfParagraph
		{
			get
			{
				return this._line.NewlineLength != 0 && this._runs[this._runs.Count - 1].Value is ParagraphBreakRun;
			}
		}

		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x06006879 RID: 26745 RVA: 0x001D7911 File Offset: 0x001D5B11
		internal int SafeLength
		{
			get
			{
				return this._line.Length;
			}
		}

		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x0600687A RID: 26746 RVA: 0x001D791E File Offset: 0x001D5B1E
		internal int ActualLength
		{
			get
			{
				return this._line.Length - (this.EndOfParagraph ? LineBase._syntheticCharacterLength : 0);
			}
		}

		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x0600687B RID: 26747 RVA: 0x001D793C File Offset: 0x001D5B3C
		internal int ContentLength
		{
			get
			{
				return this._line.Length - this._line.NewlineLength;
			}
		}

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x0600687C RID: 26748 RVA: 0x001D7955 File Offset: 0x001D5B55
		internal int DependantLength
		{
			get
			{
				return this._line.DependentLength;
			}
		}

		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x0600687D RID: 26749 RVA: 0x001D7962 File Offset: 0x001D5B62
		internal bool IsTruncated
		{
			get
			{
				return this._line.IsTruncated;
			}
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x0600687E RID: 26750 RVA: 0x001D7970 File Offset: 0x001D5B70
		internal PTS.FSFLRES FormattingResult
		{
			get
			{
				PTS.FSFLRES result = PTS.FSFLRES.fsflrOutOfSpace;
				if (this._line.NewlineLength == 0)
				{
					return result;
				}
				TextRun value = this._runs[this._runs.Count - 1].Value;
				if (value is ParagraphBreakRun)
				{
					result = ((ParagraphBreakRun)value).BreakReason;
				}
				else if (value is LineBreakRun)
				{
					result = ((LineBreakRun)value).BreakReason;
				}
				return result;
			}
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x001D79D8 File Offset: 0x001D5BD8
		private bool HasInlineObjects()
		{
			bool result = false;
			foreach (TextSpan<TextRun> textSpan in this._runs)
			{
				if (textSpan.Value is InlineObjectRun)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x001D7A34 File Offset: 0x001D5C34
		private Rect GetBoundsFromPosition(int cp, int cch, out FlowDirection flowDirection)
		{
			double num = TextDpi.FromTextDpi(this.CalculateUOffsetShift());
			IList<TextBounds> textBounds;
			if (this._line.HasOverflowed && this.TextParagraph.Properties.TextTrimming != TextTrimming.None)
			{
				Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
				TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
				{
					this.GetCollapsingProps(this._wrappingWidth, this.TextParagraph.Properties)
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
			flowDirection = textBounds[0].FlowDirection;
			rectangle.X += num;
			return rectangle;
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x001D7B3C File Offset: 0x001D5D3C
		private TextCollapsingProperties GetCollapsingProps(double wrappingWidth, LineProperties paraProperties)
		{
			Invariant.Assert(paraProperties.TextTrimming > TextTrimming.None, "Text trimming must be enabled.");
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

		// Token: 0x06006882 RID: 26754 RVA: 0x001D7B84 File Offset: 0x001D5D84
		private void AddGlyphRunRecursive(Drawing drawing, IList glyphRunsCollection, ref int cchGlyphRuns)
		{
			DrawingGroup drawingGroup = drawing as DrawingGroup;
			if (drawingGroup != null)
			{
				using (DrawingCollection.Enumerator enumerator = drawingGroup.Children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Drawing drawing2 = enumerator.Current;
						this.AddGlyphRunRecursive(drawing2, glyphRunsCollection, ref cchGlyphRuns);
					}
					return;
				}
			}
			GlyphRunDrawing glyphRunDrawing = drawing as GlyphRunDrawing;
			if (glyphRunDrawing != null)
			{
				GlyphRun glyphRun = glyphRunDrawing.GlyphRun;
				if (glyphRun != null)
				{
					cchGlyphRuns += ((glyphRun.Characters == null) ? 0 : glyphRun.Characters.Count);
					glyphRunsCollection.Add(glyphRun);
				}
			}
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x001D7C20 File Offset: 0x001D5E20
		internal int CalculateUOffsetShift()
		{
			int num;
			int num2;
			if (this.IsUOffsetAdjusted)
			{
				num = TextDpi.ToTextDpi(this._line.WidthIncludingTrailingWhitespace);
				num2 = TextDpi.ToTextDpi(this._line.Width) - num;
				Invariant.Assert(num2 <= 0);
			}
			else
			{
				num = TextDpi.ToTextDpi(this._line.Width);
				num2 = 0;
			}
			int num3 = 0;
			if ((this._textAlignment == TextAlignment.Center || this._textAlignment == TextAlignment.Right) && !this.ShowEllipses)
			{
				if (num > TextDpi.ToTextDpi(this._wrappingWidth))
				{
					num3 = num - TextDpi.ToTextDpi(this._wrappingWidth);
				}
				else
				{
					num3 = 0;
				}
			}
			int result;
			if (this._textAlignment == TextAlignment.Center)
			{
				result = (num3 + num2) / 2;
			}
			else
			{
				result = num3 + num2;
			}
			return result;
		}

		// Token: 0x1700193F RID: 6463
		// (get) Token: 0x06006884 RID: 26756 RVA: 0x001D7CCE File Offset: 0x001D5ECE
		private bool HasLineBreak
		{
			get
			{
				return this._line.NewlineLength > 0;
			}
		}

		// Token: 0x17001940 RID: 6464
		// (get) Token: 0x06006885 RID: 26757 RVA: 0x001D7CDE File Offset: 0x001D5EDE
		private bool IsUOffsetAdjusted
		{
			get
			{
				return (this._textAlignment == TextAlignment.Right || this._textAlignment == TextAlignment.Center) && this.IsWidthAdjusted;
			}
		}

		// Token: 0x17001941 RID: 6465
		// (get) Token: 0x06006886 RID: 26758 RVA: 0x001D7CFC File Offset: 0x001D5EFC
		private bool IsWidthAdjusted
		{
			get
			{
				bool result = false;
				if ((this.HasLineBreak || this.EndOfParagraph) && !this.ShowEllipses)
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x17001942 RID: 6466
		// (get) Token: 0x06006887 RID: 26759 RVA: 0x001D7D26 File Offset: 0x001D5F26
		private bool ShowEllipses
		{
			get
			{
				return this.TextParagraph.Properties.TextTrimming != TextTrimming.None && this._line.HasOverflowed;
			}
		}

		// Token: 0x17001943 RID: 6467
		// (get) Token: 0x06006888 RID: 26760 RVA: 0x001D7D4C File Offset: 0x001D5F4C
		private TextParagraph TextParagraph
		{
			get
			{
				return this._paraClient.Paragraph as TextParagraph;
			}
		}

		// Token: 0x040033D0 RID: 13264
		private readonly TextFormatterHost _host;

		// Token: 0x040033D1 RID: 13265
		private readonly int _cpPara;

		// Token: 0x040033D2 RID: 13266
		private Line.FormattingContext _formattingContext;

		// Token: 0x040033D3 RID: 13267
		private TextLine _line;

		// Token: 0x040033D4 RID: 13268
		private IList<TextSpan<TextRun>> _runs;

		// Token: 0x040033D5 RID: 13269
		private int _dcp;

		// Token: 0x040033D6 RID: 13270
		private double _wrappingWidth;

		// Token: 0x040033D7 RID: 13271
		private double _trackWidth = double.NaN;

		// Token: 0x040033D8 RID: 13272
		private bool _mirror;

		// Token: 0x040033D9 RID: 13273
		private double _indent;

		// Token: 0x040033DA RID: 13274
		private TextAlignment _textAlignment;

		// Token: 0x02000A1D RID: 2589
		internal class FormattingContext
		{
			// Token: 0x06008AAB RID: 35499 RVA: 0x00257686 File Offset: 0x00255886
			internal FormattingContext(bool measureMode, bool clearOnLeft, bool clearOnRight, TextRunCache textRunCache)
			{
				this.MeasureMode = measureMode;
				this.ClearOnLeft = clearOnLeft;
				this.ClearOnRight = clearOnRight;
				this.TextRunCache = textRunCache;
				this.LineFormatLengthTarget = -1;
			}

			// Token: 0x040046FA RID: 18170
			internal TextRunCache TextRunCache;

			// Token: 0x040046FB RID: 18171
			internal bool MeasureMode;

			// Token: 0x040046FC RID: 18172
			internal bool ClearOnLeft;

			// Token: 0x040046FD RID: 18173
			internal bool ClearOnRight;

			// Token: 0x040046FE RID: 18174
			internal int LineFormatLengthTarget;
		}
	}
}
