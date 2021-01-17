using System;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	// Token: 0x02000429 RID: 1065
	internal static class TextTreeUndo
	{
		// Token: 0x06003E29 RID: 15913 RVA: 0x0011D570 File Offset: 0x0011B770
		internal static void CreateInsertUndoUnit(TextContainer tree, int symbolOffset, int symbolCount)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return;
			}
			orClearUndoManager.Add(new TextTreeInsertUndoUnit(tree, symbolOffset, symbolCount));
		}

		// Token: 0x06003E2A RID: 15914 RVA: 0x0011D598 File Offset: 0x0011B798
		internal static void CreateInsertElementUndoUnit(TextContainer tree, int symbolOffset, bool deep)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return;
			}
			orClearUndoManager.Add(new TextTreeInsertElementUndoUnit(tree, symbolOffset, deep));
		}

		// Token: 0x06003E2B RID: 15915 RVA: 0x0011D5C0 File Offset: 0x0011B7C0
		internal static void CreatePropertyUndoUnit(TextElement element, DependencyPropertyChangedEventArgs e)
		{
			TextContainer textContainer = element.TextContainer;
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(textContainer);
			if (orClearUndoManager == null)
			{
				return;
			}
			PropertyRecord propertyRecord = default(PropertyRecord);
			propertyRecord.Property = e.Property;
			propertyRecord.Value = ((e.OldValueSource == BaseValueSourceInternal.Local) ? e.OldValue : DependencyProperty.UnsetValue);
			orClearUndoManager.Add(new TextTreePropertyUndoUnit(textContainer, element.TextElementNode.GetSymbolOffset(textContainer.Generation) + 1, propertyRecord));
		}

		// Token: 0x06003E2C RID: 15916 RVA: 0x0011D638 File Offset: 0x0011B838
		internal static TextTreeDeleteContentUndoUnit CreateDeleteContentUndoUnit(TextContainer tree, TextPointer start, TextPointer end)
		{
			if (start.CompareTo(end) == 0)
			{
				return null;
			}
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return null;
			}
			TextTreeDeleteContentUndoUnit textTreeDeleteContentUndoUnit = new TextTreeDeleteContentUndoUnit(tree, start, end);
			orClearUndoManager.Add(textTreeDeleteContentUndoUnit);
			return textTreeDeleteContentUndoUnit;
		}

		// Token: 0x06003E2D RID: 15917 RVA: 0x0011D670 File Offset: 0x0011B870
		internal static TextTreeExtractElementUndoUnit CreateExtractElementUndoUnit(TextContainer tree, TextTreeTextElementNode elementNode)
		{
			UndoManager orClearUndoManager = TextTreeUndo.GetOrClearUndoManager(tree);
			if (orClearUndoManager == null)
			{
				return null;
			}
			TextTreeExtractElementUndoUnit textTreeExtractElementUndoUnit = new TextTreeExtractElementUndoUnit(tree, elementNode);
			orClearUndoManager.Add(textTreeExtractElementUndoUnit);
			return textTreeExtractElementUndoUnit;
		}

		// Token: 0x06003E2E RID: 15918 RVA: 0x0011D69C File Offset: 0x0011B89C
		internal static UndoManager GetOrClearUndoManager(ITextContainer textContainer)
		{
			UndoManager undoManager = textContainer.UndoManager;
			if (undoManager == null)
			{
				return null;
			}
			if (!undoManager.IsEnabled)
			{
				return null;
			}
			if (undoManager.OpenedUnit == null)
			{
				undoManager.Clear();
				return null;
			}
			return undoManager;
		}
	}
}
