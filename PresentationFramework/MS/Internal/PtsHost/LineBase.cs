using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000627 RID: 1575
	internal abstract class LineBase : UnmanagedHandle
	{
		// Token: 0x06006889 RID: 26761 RVA: 0x001D7D5E File Offset: 0x001D5F5E
		internal LineBase(BaseParaClient paraClient) : base(paraClient.PtsContext)
		{
			this._paraClient = paraClient;
		}

		// Token: 0x0600688A RID: 26762
		internal abstract TextRun GetTextRun(int dcp);

		// Token: 0x0600688B RID: 26763
		internal abstract TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int dcp);

		// Token: 0x0600688C RID: 26764
		internal abstract int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int dcp);

		// Token: 0x0600688D RID: 26765 RVA: 0x001D7D74 File Offset: 0x001D5F74
		protected TextRun HandleText(StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text, "TextPointer does not point to characters.");
			DependencyObject target;
			if (position.Parent != null)
			{
				target = position.Parent;
			}
			else
			{
				target = this._paraClient.Paragraph.Element;
			}
			TextProperties textRunProperties = new TextProperties(target, position, false, true, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			StaticTextPointer position2 = position.TextContainer.Highlights.GetNextPropertyChangePosition(position, LogicalDirection.Forward);
			if (position.GetOffsetToPosition(position2) > 4096)
			{
				position2 = position.CreatePointer(4096);
			}
			char[] array = new char[position.GetOffsetToPosition(position2)];
			int textInRun = position.GetTextInRun(LogicalDirection.Forward, array, 0, array.Length);
			return new TextCharacters(array, 0, textInRun, textRunProperties);
		}

		// Token: 0x0600688E RID: 26766 RVA: 0x001D7E38 File Offset: 0x001D6038
		protected TextRun HandleElementStartEdge(StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart, "TextPointer does not point to element start edge.");
			TextElement textElement = (TextElement)position.GetAdjacentElement(LogicalDirection.Forward);
			Invariant.Assert(!(textElement is Block), "We do not expect any Blocks inside Paragraphs");
			TextRun result;
			if (textElement is Figure || textElement is Floater)
			{
				int elementLength = TextContainerHelper.GetElementLength(this._paraClient.Paragraph.StructuralCache.TextContainer, textElement);
				result = new FloatingRun(elementLength, textElement is Figure);
				if (textElement is Figure)
				{
					this._hasFigures = true;
				}
				else
				{
					this._hasFloaters = true;
				}
			}
			else if (textElement is LineBreak)
			{
				int elementLength2 = TextContainerHelper.GetElementLength(this._paraClient.Paragraph.StructuralCache.TextContainer, textElement);
				result = new LineBreakRun(elementLength2, PTS.FSFLRES.fsflrSoftBreak);
			}
			else if (textElement.IsEmpty)
			{
				TextProperties textRunProperties = new TextProperties(textElement, position, false, true, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				char[] array = new char[LineBase._elementEdgeCharacterLength * 2];
				Invariant.Assert(LineBase._elementEdgeCharacterLength == 1, "Expected value of _elementEdgeCharacterLength is 1");
				array[0] = '​';
				array[1] = '​';
				result = new TextCharacters(array, 0, array.Length, textRunProperties);
			}
			else
			{
				Inline inline = (Inline)textElement;
				DependencyObject parent = inline.Parent;
				FlowDirection flowDirection = inline.FlowDirection;
				FlowDirection flowDirection2 = flowDirection;
				TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
				if (parent != null)
				{
					flowDirection2 = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				if (flowDirection != flowDirection2)
				{
					if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextSpanModifier(LineBase._elementEdgeCharacterLength, null, null, flowDirection);
					}
					else
					{
						result = new TextSpanModifier(LineBase._elementEdgeCharacterLength, textDecorations, inline.Foreground, flowDirection);
					}
				}
				else if (textDecorations == null || textDecorations.Count == 0)
				{
					result = new TextHidden(LineBase._elementEdgeCharacterLength);
				}
				else
				{
					result = new TextSpanModifier(LineBase._elementEdgeCharacterLength, textDecorations, inline.Foreground);
				}
			}
			return result;
		}

		// Token: 0x0600688F RID: 26767 RVA: 0x001D802C File Offset: 0x001D622C
		protected TextRun HandleElementEndEdge(StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementEnd, "TextPointer does not point to element end edge.");
			TextRun result;
			if (position.Parent == this._paraClient.Paragraph.Element)
			{
				result = new ParagraphBreakRun(LineBase._syntheticCharacterLength, PTS.FSFLRES.fsflrEndOfParagraph);
			}
			else
			{
				TextElement textElement = (TextElement)position.GetAdjacentElement(LogicalDirection.Forward);
				Inline inline = (Inline)textElement;
				DependencyObject parent = inline.Parent;
				FlowDirection flowDirection = inline.FlowDirection;
				if (parent != null)
				{
					flowDirection = (FlowDirection)parent.GetValue(FrameworkElement.FlowDirectionProperty);
				}
				if (inline.FlowDirection != flowDirection)
				{
					result = new TextEndOfSegment(LineBase._elementEdgeCharacterLength);
				}
				else
				{
					TextDecorationCollection textDecorations = DynamicPropertyReader.GetTextDecorations(inline);
					if (textDecorations == null || textDecorations.Count == 0)
					{
						result = new TextHidden(LineBase._elementEdgeCharacterLength);
					}
					else
					{
						result = new TextEndOfSegment(LineBase._elementEdgeCharacterLength);
					}
				}
			}
			return result;
		}

		// Token: 0x06006890 RID: 26768 RVA: 0x001D80F4 File Offset: 0x001D62F4
		protected TextRun HandleEmbeddedObject(int dcp, StaticTextPointer position)
		{
			Invariant.Assert(position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.EmbeddedElement, "TextPointer does not point to embedded object.");
			DependencyObject dependencyObject = position.GetAdjacentElement(LogicalDirection.Forward) as DependencyObject;
			TextRun result;
			if (dependencyObject is UIElement)
			{
				TextRunProperties textProps = new TextProperties(dependencyObject, position, true, true, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				result = new InlineObjectRun(TextContainerHelper.EmbeddedObjectLength, (UIElement)dependencyObject, textProps, this._paraClient.Paragraph as TextParagraph);
			}
			else
			{
				result = new TextHidden(TextContainerHelper.EmbeddedObjectLength);
			}
			return result;
		}

		// Token: 0x17001944 RID: 6468
		// (get) Token: 0x06006891 RID: 26769 RVA: 0x001D8182 File Offset: 0x001D6382
		internal static int SyntheticCharacterLength
		{
			get
			{
				return LineBase._syntheticCharacterLength;
			}
		}

		// Token: 0x17001945 RID: 6469
		// (get) Token: 0x06006892 RID: 26770 RVA: 0x001D8189 File Offset: 0x001D6389
		internal bool HasFigures
		{
			get
			{
				return this._hasFigures;
			}
		}

		// Token: 0x17001946 RID: 6470
		// (get) Token: 0x06006893 RID: 26771 RVA: 0x001D8191 File Offset: 0x001D6391
		internal bool HasFloaters
		{
			get
			{
				return this._hasFloaters;
			}
		}

		// Token: 0x040033DB RID: 13275
		protected readonly BaseParaClient _paraClient;

		// Token: 0x040033DC RID: 13276
		protected bool _hasFigures;

		// Token: 0x040033DD RID: 13277
		protected bool _hasFloaters;

		// Token: 0x040033DE RID: 13278
		protected static int _syntheticCharacterLength = 1;

		// Token: 0x040033DF RID: 13279
		protected static int _elementEdgeCharacterLength = 1;
	}
}
