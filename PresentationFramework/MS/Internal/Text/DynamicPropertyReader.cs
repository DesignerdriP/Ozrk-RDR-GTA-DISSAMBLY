using System;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;

namespace MS.Internal.Text
{
	// Token: 0x020005FE RID: 1534
	internal static class DynamicPropertyReader
	{
		// Token: 0x0600660C RID: 26124 RVA: 0x001CB27C File Offset: 0x001C947C
		internal static Typeface GetTypeface(DependencyObject element)
		{
			FontFamily fontFamily = (FontFamily)element.GetValue(TextElement.FontFamilyProperty);
			FontStyle style = (FontStyle)element.GetValue(TextElement.FontStyleProperty);
			FontWeight weight = (FontWeight)element.GetValue(TextElement.FontWeightProperty);
			FontStretch stretch = (FontStretch)element.GetValue(TextElement.FontStretchProperty);
			return new Typeface(fontFamily, style, weight, stretch);
		}

		// Token: 0x0600660D RID: 26125 RVA: 0x001CB2D8 File Offset: 0x001C94D8
		internal static Typeface GetModifiedTypeface(DependencyObject element, FontFamily fontFamily)
		{
			FontStyle style = (FontStyle)element.GetValue(TextElement.FontStyleProperty);
			FontWeight weight = (FontWeight)element.GetValue(TextElement.FontWeightProperty);
			FontStretch stretch = (FontStretch)element.GetValue(TextElement.FontStretchProperty);
			return new Typeface(fontFamily, style, weight, stretch);
		}

		// Token: 0x0600660E RID: 26126 RVA: 0x001CB324 File Offset: 0x001C9524
		internal static TextDecorationCollection GetTextDecorationsForInlineObject(DependencyObject element, TextDecorationCollection textDecorations)
		{
			DependencyObject parent = LogicalTreeHelper.GetParent(element);
			TextDecorationCollection textDecorationCollection = null;
			if (parent != null)
			{
				textDecorationCollection = DynamicPropertyReader.GetTextDecorations(parent);
			}
			if (!((textDecorations == null) ? (textDecorationCollection == null) : textDecorations.ValueEquals(textDecorationCollection)))
			{
				if (textDecorationCollection == null)
				{
					textDecorations = null;
				}
				else
				{
					textDecorations = new TextDecorationCollection();
					int count = textDecorationCollection.Count;
					for (int i = 0; i < count; i++)
					{
						textDecorations.Add(textDecorationCollection[i]);
					}
				}
			}
			return textDecorations;
		}

		// Token: 0x0600660F RID: 26127 RVA: 0x001CB38D File Offset: 0x001C958D
		internal static TextDecorationCollection GetTextDecorations(DependencyObject element)
		{
			return DynamicPropertyReader.GetCollectionValue(element, Inline.TextDecorationsProperty) as TextDecorationCollection;
		}

		// Token: 0x06006610 RID: 26128 RVA: 0x001CB39F File Offset: 0x001C959F
		internal static TextEffectCollection GetTextEffects(DependencyObject element)
		{
			return DynamicPropertyReader.GetCollectionValue(element, TextElement.TextEffectsProperty) as TextEffectCollection;
		}

		// Token: 0x06006611 RID: 26129 RVA: 0x001CB3B4 File Offset: 0x001C95B4
		private static object GetCollectionValue(DependencyObject element, DependencyProperty property)
		{
			bool flag;
			if (element.GetValueSource(property, null, out flag) != BaseValueSourceInternal.Default || flag)
			{
				return element.GetValue(property);
			}
			return null;
		}

		// Token: 0x06006612 RID: 26130 RVA: 0x001CB3E0 File Offset: 0x001C95E0
		internal static bool GetKeepTogether(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			return paragraph != null && paragraph.KeepTogether;
		}

		// Token: 0x06006613 RID: 26131 RVA: 0x001CB400 File Offset: 0x001C9600
		internal static bool GetKeepWithNext(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			return paragraph != null && paragraph.KeepWithNext;
		}

		// Token: 0x06006614 RID: 26132 RVA: 0x001CB420 File Offset: 0x001C9620
		internal static int GetMinWidowLines(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			if (paragraph == null)
			{
				return 0;
			}
			return paragraph.MinWidowLines;
		}

		// Token: 0x06006615 RID: 26133 RVA: 0x001CB440 File Offset: 0x001C9640
		internal static int GetMinOrphanLines(DependencyObject element)
		{
			Paragraph paragraph = element as Paragraph;
			if (paragraph == null)
			{
				return 0;
			}
			return paragraph.MinOrphanLines;
		}

		// Token: 0x06006616 RID: 26134 RVA: 0x001CB460 File Offset: 0x001C9660
		internal static double GetLineHeightValue(DependencyObject d)
		{
			double num = (double)d.GetValue(Block.LineHeightProperty);
			if (DoubleUtil.IsNaN(num))
			{
				FontFamily fontFamily = (FontFamily)d.GetValue(TextElement.FontFamilyProperty);
				double num2 = (double)d.GetValue(TextElement.FontSizeProperty);
				num = fontFamily.LineSpacing * num2;
			}
			return Math.Max(TextDpi.MinWidth, Math.Min(TextDpi.MaxWidth, num));
		}

		// Token: 0x06006617 RID: 26135 RVA: 0x001CB4C8 File Offset: 0x001C96C8
		internal static Brush GetBackgroundBrush(DependencyObject element)
		{
			Brush brush = null;
			while (brush == null && DynamicPropertyReader.CanApplyBackgroundBrush(element))
			{
				brush = (Brush)element.GetValue(TextElement.BackgroundProperty);
				Invariant.Assert(element is FrameworkContentElement);
				element = ((FrameworkContentElement)element).Parent;
			}
			return brush;
		}

		// Token: 0x06006618 RID: 26136 RVA: 0x001CB514 File Offset: 0x001C9714
		internal static Brush GetBackgroundBrushForInlineObject(StaticTextPointer position)
		{
			object highlightValue = position.TextContainer.Highlights.GetHighlightValue(position, LogicalDirection.Forward, typeof(TextSelection));
			Brush result;
			if (highlightValue == DependencyProperty.UnsetValue)
			{
				result = (Brush)position.GetValue(TextElement.BackgroundProperty);
			}
			else
			{
				result = SelectionHighlightInfo.BackgroundBrush;
			}
			return result;
		}

		// Token: 0x06006619 RID: 26137 RVA: 0x001CB564 File Offset: 0x001C9764
		internal static BaselineAlignment GetBaselineAlignment(DependencyObject element)
		{
			Inline inline = element as Inline;
			BaselineAlignment result = (inline != null) ? inline.BaselineAlignment : BaselineAlignment.Baseline;
			while (inline != null && DynamicPropertyReader.BaselineAlignmentIsDefault(inline))
			{
				inline = (inline.Parent as Inline);
			}
			if (inline != null)
			{
				result = inline.BaselineAlignment;
			}
			return result;
		}

		// Token: 0x0600661A RID: 26138 RVA: 0x001CB5A9 File Offset: 0x001C97A9
		internal static BaselineAlignment GetBaselineAlignmentForInlineObject(DependencyObject element)
		{
			return DynamicPropertyReader.GetBaselineAlignment(LogicalTreeHelper.GetParent(element));
		}

		// Token: 0x0600661B RID: 26139 RVA: 0x001CB5B8 File Offset: 0x001C97B8
		internal static CultureInfo GetCultureInfo(DependencyObject element)
		{
			XmlLanguage xmlLanguage = (XmlLanguage)element.GetValue(FrameworkElement.LanguageProperty);
			CultureInfo result;
			try
			{
				result = xmlLanguage.GetSpecificCulture();
			}
			catch (InvalidOperationException)
			{
				result = TypeConverterHelper.InvariantEnglishUS;
			}
			return result;
		}

		// Token: 0x0600661C RID: 26140 RVA: 0x001CB5FC File Offset: 0x001C97FC
		internal static NumberSubstitution GetNumberSubstitution(DependencyObject element)
		{
			return new NumberSubstitution
			{
				CultureSource = (NumberCultureSource)element.GetValue(NumberSubstitution.CultureSourceProperty),
				CultureOverride = (CultureInfo)element.GetValue(NumberSubstitution.CultureOverrideProperty),
				Substitution = (NumberSubstitutionMethod)element.GetValue(NumberSubstitution.SubstitutionProperty)
			};
		}

		// Token: 0x0600661D RID: 26141 RVA: 0x001CB652 File Offset: 0x001C9852
		private static bool CanApplyBackgroundBrush(DependencyObject element)
		{
			return element is Inline && !(element is AnchoredBlock);
		}

		// Token: 0x0600661E RID: 26142 RVA: 0x001CB668 File Offset: 0x001C9868
		private static bool BaselineAlignmentIsDefault(DependencyObject element)
		{
			Invariant.Assert(element != null);
			bool flag;
			return element.GetValueSource(Inline.BaselineAlignmentProperty, null, out flag) == BaseValueSourceInternal.Default && !flag;
		}
	}
}
