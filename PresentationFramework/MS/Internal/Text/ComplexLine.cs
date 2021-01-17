using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost;

namespace MS.Internal.Text
{
	// Token: 0x020005FD RID: 1533
	internal sealed class ComplexLine : Line
	{
		// Token: 0x060065FF RID: 26111 RVA: 0x001CAA74 File Offset: 0x001C8C74
		public override TextRun GetTextRun(int dcp)
		{
			TextRun textRun = null;
			StaticTextPointer position = this._owner.TextContainer.CreateStaticPointerAtOffset(dcp);
			switch (position.GetPointerContext(LogicalDirection.Forward))
			{
			case TextPointerContext.None:
				textRun = new TextEndOfParagraph(Line._syntheticCharacterLength);
				break;
			case TextPointerContext.Text:
				textRun = this.HandleText(position);
				break;
			case TextPointerContext.EmbeddedElement:
				textRun = this.HandleInlineObject(position, dcp);
				break;
			case TextPointerContext.ElementStart:
				textRun = this.HandleElementStartEdge(position);
				break;
			case TextPointerContext.ElementEnd:
				textRun = this.HandleElementEndEdge(position);
				break;
			}
			if (textRun.Properties != null)
			{
				textRun.Properties.PixelsPerDip = base.PixelsPerDip;
			}
			return textRun;
		}

		// Token: 0x06006600 RID: 26112 RVA: 0x001CAB08 File Offset: 0x001C8D08
		public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp)
		{
			int num = 0;
			CharacterBufferRange empty = CharacterBufferRange.Empty;
			CultureInfo culture = null;
			if (dcp > 0)
			{
				ITextPointer textPointer = this._owner.TextContainer.CreatePointerAtOffset(dcp, LogicalDirection.Backward);
				while (textPointer.GetPointerContext(LogicalDirection.Backward) != TextPointerContext.Text && textPointer.CompareTo(this._owner.TextContainer.Start) != 0)
				{
					textPointer.MoveByOffset(-1);
					num++;
				}
				string textInRun = textPointer.GetTextInRun(LogicalDirection.Backward);
				empty = new CharacterBufferRange(textInRun, 0, textInRun.Length);
				StaticTextPointer staticTextPointer = textPointer.CreateStaticPointer();
				DependencyObject element = (staticTextPointer.Parent != null) ? staticTextPointer.Parent : this._owner;
				culture = DynamicPropertyReader.GetCultureInfo(element);
			}
			return new TextSpan<CultureSpecificCharacterBufferRange>(num + empty.Length, new CultureSpecificCharacterBufferRange(culture, empty));
		}

		// Token: 0x06006601 RID: 26113 RVA: 0x00012630 File Offset: 0x00010830
		public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
		{
			return textSourceCharacterIndex;
		}

		// Token: 0x06006602 RID: 26114 RVA: 0x001CABC2 File Offset: 0x001C8DC2
		internal ComplexLine(TextBlock owner) : base(owner)
		{
		}

		// Token: 0x06006603 RID: 26115 RVA: 0x001CABCC File Offset: 0x001C8DCC
		internal override void Arrange(VisualCollection vc, Vector lineOffset)
		{
			int num = this._dcp;
			IList<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
			double num2 = lineOffset.X + base.CalculateXOffsetShift();
			foreach (TextSpan<TextRun> textSpan in textRunSpans)
			{
				TextRun value = textSpan.Value;
				if (value is InlineObject)
				{
					InlineObject inlineObject = value as InlineObject;
					Visual visual = VisualTreeHelper.GetParent(inlineObject.Element) as Visual;
					if (visual != null)
					{
						ContainerVisual containerVisual = visual as ContainerVisual;
						Invariant.Assert(containerVisual != null, "parent should always derives from ContainerVisual");
						containerVisual.Children.Remove(inlineObject.Element);
					}
					FlowDirection flowDirection;
					Rect boundsFromPosition = base.GetBoundsFromPosition(num, inlineObject.Length, out flowDirection);
					ContainerVisual containerVisual2 = new ContainerVisual();
					if (inlineObject.Element is FrameworkElement)
					{
						FlowDirection childFD = this._owner.FlowDirection;
						DependencyObject parent = ((FrameworkElement)inlineObject.Element).Parent;
						if (parent != null)
						{
							childFD = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
						}
						PtsHelper.UpdateMirroringTransform(this._owner.FlowDirection, childFD, containerVisual2, boundsFromPosition.Width);
					}
					vc.Add(containerVisual2);
					if (this._owner.UseLayoutRounding)
					{
						DpiScale dpi = this._owner.GetDpi();
						containerVisual2.Offset = new Vector(UIElement.RoundLayoutValue(lineOffset.X + boundsFromPosition.Left, dpi.DpiScaleX), UIElement.RoundLayoutValue(lineOffset.Y + boundsFromPosition.Top, dpi.DpiScaleY));
					}
					else
					{
						containerVisual2.Offset = new Vector(lineOffset.X + boundsFromPosition.Left, lineOffset.Y + boundsFromPosition.Top);
					}
					containerVisual2.Children.Add(inlineObject.Element);
					inlineObject.Element.Arrange(new Rect(inlineObject.Element.DesiredSize));
				}
				num += textSpan.Length;
			}
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x001CADE8 File Offset: 0x001C8FE8
		internal override bool HasInlineObjects()
		{
			bool result = false;
			IList<TextSpan<TextRun>> textRunSpans = this._line.GetTextRunSpans();
			foreach (TextSpan<TextRun> textSpan in textRunSpans)
			{
				if (textSpan.Value is InlineObject)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06006605 RID: 26117 RVA: 0x001CAE4C File Offset: 0x001C904C
		internal override IInputElement InputHitTest(double offset)
		{
			DependencyObject dependencyObject = null;
			TextContainer textContainer = this._owner.TextContainer as TextContainer;
			double num = base.CalculateXOffsetShift();
			if (textContainer != null)
			{
				CharacterHit characterHitFromDistance;
				if (this._line.HasOverflowed && this._owner.ParagraphProperties.TextTrimming != TextTrimming.None)
				{
					Invariant.Assert(DoubleUtil.AreClose(num, 0.0));
					TextLine textLine = this._line.Collapse(new TextCollapsingProperties[]
					{
						base.GetCollapsingProps(this._wrappingWidth, this._owner.ParagraphProperties)
					});
					Invariant.Assert(textLine.HasCollapsed, "Line has not been collapsed");
					characterHitFromDistance = textLine.GetCharacterHitFromDistance(offset);
				}
				else
				{
					characterHitFromDistance = this._line.GetCharacterHitFromDistance(offset - num);
				}
				TextPointer textPointer = new TextPointer(this._owner.ContentStart, this.CalcPositionOffset(characterHitFromDistance), LogicalDirection.Forward);
				if (textPointer != null)
				{
					TextPointerContext pointerContext;
					if (characterHitFromDistance.TrailingLength == 0)
					{
						pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
					}
					else
					{
						pointerContext = textPointer.GetPointerContext(LogicalDirection.Backward);
					}
					if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.ElementEnd)
					{
						dependencyObject = (textPointer.Parent as TextElement);
					}
					else if (pointerContext == TextPointerContext.ElementStart)
					{
						dependencyObject = textPointer.GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					}
				}
			}
			return dependencyObject as IInputElement;
		}

		// Token: 0x06006606 RID: 26118 RVA: 0x001CAF70 File Offset: 0x001C9170
		private TextRun HandleText(StaticTextPointer position)
		{
			DependencyObject target;
			if (position.Parent != null)
			{
				target = position.Parent;
			}
			else
			{
				target = this._owner;
			}
			TextRunProperties textRunProperties = new TextProperties(target, position, false, true, base.PixelsPerDip);
			StaticTextPointer position2 = this._owner.Highlights.GetNextPropertyChangePosition(position, LogicalDirection.Forward);
			if (position.GetOffsetToPosition(position2) > 4096)
			{
				position2 = position.CreatePointer(4096);
			}
			char[] array = new char[position.GetOffsetToPosition(position2)];
			int textInRun = position.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
			return new TextCharacters(array, 0, textInRun, textRunProperties);
		}

		// Token: 0x06006607 RID: 26119 RVA: 0x001CB000 File Offset: 0x001C9200
		private TextRun HandleElementStartEdge(StaticTextPointer position)
		{
			TextElement textElement = (TextElement)position.GetAdjacentElement(LogicalDirection.Forward);
			TextRun result;
			if (textElement is LineBreak)
			{
				result = new TextEndOfLine(ComplexLine._elementEdgeCharacterLength * 2);
			}
			else if (textElement.IsEmpty)
			{
				TextRunProperties textRunProperties = new TextProperties(textElement, position, false, true, base.PixelsPerDip);
				char[] array = new char[ComplexLine._elementEdgeCharacterLength * 2];
				array[0] = '​';
				array[1] = '​';
				result = new TextCharacters(array, 0, array.Length, textRunProperties);
			}
			else
			{
				Inline inline = textElement as Inline;
				if (inline == null)
				{
					result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
				}
				else
				{
					DependencyObject parent = inline.Parent;
					FlowDirection flowDirection = inline.FlowDirection;
					FlowDirection flowDirection2 = flowDirection;
					if (parent != null)
					{
						flowDirection2 = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
					}
					TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
					if (flowDirection != flowDirection2)
					{
						if (textDecorations == null || textDecorations.Count == 0)
						{
							result = new TextSpanModifier(ComplexLine._elementEdgeCharacterLength, null, null, flowDirection);
						}
						else
						{
							result = new TextSpanModifier(ComplexLine._elementEdgeCharacterLength, textDecorations, inline.Foreground, flowDirection);
						}
					}
					else if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
					}
					else
					{
						result = new TextSpanModifier(ComplexLine._elementEdgeCharacterLength, textDecorations, inline.Foreground);
					}
				}
			}
			return result;
		}

		// Token: 0x06006608 RID: 26120 RVA: 0x001CB13C File Offset: 0x001C933C
		private TextRun HandleElementEndEdge(StaticTextPointer position)
		{
			TextElement textElement = (TextElement)position.GetAdjacentElement(LogicalDirection.Forward);
			Inline inline = textElement as Inline;
			TextRun result;
			if (inline == null)
			{
				result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
			}
			else
			{
				DependencyObject parent = inline.Parent;
				FlowDirection flowDirection = inline.FlowDirection;
				if (parent != null)
				{
					flowDirection = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				if (inline.FlowDirection != flowDirection)
				{
					result = new TextEndOfSegment(ComplexLine._elementEdgeCharacterLength);
				}
				else
				{
					TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
					if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextHidden(ComplexLine._elementEdgeCharacterLength);
					}
					else
					{
						result = new TextEndOfSegment(ComplexLine._elementEdgeCharacterLength);
					}
				}
			}
			return result;
		}

		// Token: 0x06006609 RID: 26121 RVA: 0x001CB1DC File Offset: 0x001C93DC
		private TextRun HandleInlineObject(StaticTextPointer position, int dcp)
		{
			DependencyObject dependencyObject = position.GetAdjacentElement(LogicalDirection.Forward) as DependencyObject;
			TextRun result;
			if (dependencyObject is UIElement)
			{
				TextRunProperties textProps = new TextProperties(dependencyObject, position, true, true, base.PixelsPerDip);
				result = new InlineObject(dcp, TextContainerHelper.EmbeddedObjectLength, (UIElement)dependencyObject, textProps, this._owner);
			}
			else
			{
				result = this.HandleElementEndEdge(position);
			}
			return result;
		}

		// Token: 0x0600660A RID: 26122 RVA: 0x001CB238 File Offset: 0x001C9438
		private int CalcPositionOffset(CharacterHit charHit)
		{
			int num = charHit.FirstCharacterIndex + charHit.TrailingLength;
			if (base.EndOfParagraph)
			{
				num = Math.Min(this._dcp + base.Length, num);
			}
			return num;
		}

		// Token: 0x040032F1 RID: 13041
		private static int _elementEdgeCharacterLength = 1;
	}
}
