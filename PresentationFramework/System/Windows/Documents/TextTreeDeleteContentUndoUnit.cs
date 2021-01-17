using System;
using System.IO;
using System.Security;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x0200041B RID: 1051
	internal class TextTreeDeleteContentUndoUnit : TextTreeUndoUnit
	{
		// Token: 0x06003D0D RID: 15629 RVA: 0x0011B8BC File Offset: 0x00119ABC
		internal TextTreeDeleteContentUndoUnit(TextContainer tree, TextPointer start, TextPointer end) : base(tree, start.GetSymbolOffset())
		{
			start.DebugAssertGeneration();
			end.DebugAssertGeneration();
			Invariant.Assert(start.GetScopingNode() == end.GetScopingNode(), "start/end have different scope!");
			TextTreeNode adjacentNode = start.GetAdjacentNode(LogicalDirection.Forward);
			TextTreeNode adjacentNode2 = end.GetAdjacentNode(LogicalDirection.Forward);
			this._content = this.CopyContent(adjacentNode, adjacentNode2);
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x0011B918 File Offset: 0x00119B18
		public override void DoCore()
		{
			base.VerifyTreeContentHashCode();
			TextPointer navigator = new TextPointer(base.TextContainer, base.SymbolOffset, LogicalDirection.Forward);
			for (TextTreeDeleteContentUndoUnit.ContentContainer contentContainer = this._content; contentContainer != null; contentContainer = contentContainer.NextContainer)
			{
				contentContainer.Do(navigator);
			}
		}

		// Token: 0x06003D0F RID: 15631 RVA: 0x0011B958 File Offset: 0x00119B58
		internal static TableColumn[] SaveColumns(Table table)
		{
			TableColumn[] array;
			if (table.Columns.Count > 0)
			{
				array = new TableColumn[table.Columns.Count];
				for (int i = 0; i < table.Columns.Count; i++)
				{
					array[i] = TextTreeDeleteContentUndoUnit.CopyColumn(table.Columns[i]);
				}
			}
			else
			{
				array = null;
			}
			return array;
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x0011B9B4 File Offset: 0x00119BB4
		internal static void RestoreColumns(Table table, TableColumn[] savedColumns)
		{
			if (savedColumns != null)
			{
				for (int i = 0; i < savedColumns.Length; i++)
				{
					if (table.Columns.Count <= i)
					{
						table.Columns.Add(TextTreeDeleteContentUndoUnit.CopyColumn(savedColumns[i]));
					}
				}
			}
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0011B9F4 File Offset: 0x00119BF4
		private static TableColumn CopyColumn(TableColumn sourceTableColumn)
		{
			TableColumn tableColumn = new TableColumn();
			LocalValueEnumerator localValueEnumerator = sourceTableColumn.GetLocalValueEnumerator();
			while (localValueEnumerator.MoveNext())
			{
				LocalValueEntry localValueEntry = localValueEnumerator.Current;
				if (!localValueEntry.Property.ReadOnly)
				{
					tableColumn.SetValue(localValueEntry.Property, localValueEntry.Value);
				}
			}
			return tableColumn;
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x0011BA44 File Offset: 0x00119C44
		private TextTreeDeleteContentUndoUnit.ContentContainer CopyContent(TextTreeNode node, TextTreeNode haltNode)
		{
			TextTreeDeleteContentUndoUnit.ContentContainer result = null;
			TextTreeDeleteContentUndoUnit.ContentContainer contentContainer = null;
			while (node != haltNode && node != null)
			{
				TextTreeTextNode textTreeTextNode = node as TextTreeTextNode;
				TextTreeDeleteContentUndoUnit.ContentContainer contentContainer2;
				if (textTreeTextNode != null)
				{
					node = this.CopyTextNode(textTreeTextNode, haltNode, out contentContainer2);
				}
				else
				{
					TextTreeObjectNode textTreeObjectNode = node as TextTreeObjectNode;
					if (textTreeObjectNode != null)
					{
						node = this.CopyObjectNode(textTreeObjectNode, out contentContainer2);
					}
					else
					{
						Invariant.Assert(node is TextTreeTextElementNode, "Unexpected TextTreeNode type!");
						TextTreeTextElementNode elementNode = (TextTreeTextElementNode)node;
						node = this.CopyElementNode(elementNode, out contentContainer2);
					}
				}
				if (contentContainer == null)
				{
					result = contentContainer2;
				}
				else
				{
					contentContainer.NextContainer = contentContainer2;
				}
				contentContainer = contentContainer2;
			}
			return result;
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x0011BAC8 File Offset: 0x00119CC8
		private TextTreeNode CopyTextNode(TextTreeTextNode textNode, TextTreeNode haltNode, out TextTreeDeleteContentUndoUnit.ContentContainer container)
		{
			Invariant.Assert(textNode != haltNode, "Expect at least one node to copy!");
			int symbolOffset = textNode.GetSymbolOffset(base.TextContainer.Generation);
			int num = 0;
			SplayTreeNode nextNode;
			do
			{
				num += textNode.SymbolCount;
				nextNode = textNode.GetNextNode();
				textNode = (nextNode as TextTreeTextNode);
			}
			while (textNode != null && textNode != haltNode);
			char[] array = new char[num];
			TextTreeText.ReadText(base.TextContainer.RootTextBlock, symbolOffset, num, array, 0);
			container = new TextTreeDeleteContentUndoUnit.TextContentContainer(array);
			return (TextTreeNode)nextNode;
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x0011BB44 File Offset: 0x00119D44
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private TextTreeNode CopyObjectNode(TextTreeObjectNode objectNode, out TextTreeDeleteContentUndoUnit.ContentContainer container)
		{
			if (SecurityHelper.CheckUnmanagedCodePermission())
			{
				string xml = XamlWriter.Save(objectNode.EmbeddedElement);
				container = new TextTreeDeleteContentUndoUnit.ObjectContentContainer(xml, objectNode.EmbeddedElement);
			}
			else
			{
				container = new TextTreeDeleteContentUndoUnit.ObjectContentContainer(null, null);
			}
			return (TextTreeNode)objectNode.GetNextNode();
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x0011BB88 File Offset: 0x00119D88
		private TextTreeNode CopyElementNode(TextTreeTextElementNode elementNode, out TextTreeDeleteContentUndoUnit.ContentContainer container)
		{
			if (elementNode.TextElement is Table)
			{
				container = new TextTreeDeleteContentUndoUnit.TableElementContentContainer(elementNode.TextElement as Table, TextTreeUndoUnit.GetPropertyRecordArray(elementNode.TextElement), this.CopyContent((TextTreeNode)elementNode.GetFirstContainedNode(), null));
			}
			else
			{
				container = new TextTreeDeleteContentUndoUnit.ElementContentContainer(elementNode.TextElement.GetType(), TextTreeUndoUnit.GetPropertyRecordArray(elementNode.TextElement), elementNode.TextElement.Resources, this.CopyContent((TextTreeNode)elementNode.GetFirstContainedNode(), null));
			}
			return (TextTreeNode)elementNode.GetNextNode();
		}

		// Token: 0x04002652 RID: 9810
		private readonly TextTreeDeleteContentUndoUnit.ContentContainer _content;

		// Token: 0x02000916 RID: 2326
		private abstract class ContentContainer
		{
			// Token: 0x06008600 RID: 34304
			internal abstract void Do(TextPointer navigator);

			// Token: 0x17001E4E RID: 7758
			// (get) Token: 0x06008601 RID: 34305 RVA: 0x0024B72A File Offset: 0x0024992A
			// (set) Token: 0x06008602 RID: 34306 RVA: 0x0024B732 File Offset: 0x00249932
			internal TextTreeDeleteContentUndoUnit.ContentContainer NextContainer
			{
				get
				{
					return this._nextContainer;
				}
				set
				{
					this._nextContainer = value;
				}
			}

			// Token: 0x04004341 RID: 17217
			private TextTreeDeleteContentUndoUnit.ContentContainer _nextContainer;
		}

		// Token: 0x02000917 RID: 2327
		private class TextContentContainer : TextTreeDeleteContentUndoUnit.ContentContainer
		{
			// Token: 0x06008604 RID: 34308 RVA: 0x0024B73B File Offset: 0x0024993B
			internal TextContentContainer(char[] text)
			{
				this._text = text;
			}

			// Token: 0x06008605 RID: 34309 RVA: 0x0024B74A File Offset: 0x0024994A
			internal override void Do(TextPointer navigator)
			{
				navigator.TextContainer.InsertTextInternal(navigator, this._text);
			}

			// Token: 0x04004342 RID: 17218
			private readonly char[] _text;
		}

		// Token: 0x02000918 RID: 2328
		private class ObjectContentContainer : TextTreeDeleteContentUndoUnit.ContentContainer
		{
			// Token: 0x06008606 RID: 34310 RVA: 0x0024B75E File Offset: 0x0024995E
			internal ObjectContentContainer(string xml, object element)
			{
				this._xml = xml;
				this._element = element;
			}

			// Token: 0x06008607 RID: 34311 RVA: 0x0024B774 File Offset: 0x00249974
			internal override void Do(TextPointer navigator)
			{
				DependencyObject dependencyObject = null;
				if (this._xml != null)
				{
					try
					{
						dependencyObject = (DependencyObject)XamlReader.Load(new XmlTextReader(new StringReader(this._xml)));
					}
					catch (XamlParseException ex)
					{
						Invariant.Assert(ex != null);
					}
				}
				if (dependencyObject == null)
				{
					dependencyObject = new Grid();
				}
				navigator.TextContainer.InsertEmbeddedObjectInternal(navigator, dependencyObject);
			}

			// Token: 0x04004343 RID: 17219
			private readonly string _xml;

			// Token: 0x04004344 RID: 17220
			private readonly object _element;
		}

		// Token: 0x02000919 RID: 2329
		private class ElementContentContainer : TextTreeDeleteContentUndoUnit.ContentContainer
		{
			// Token: 0x06008608 RID: 34312 RVA: 0x0024B7DC File Offset: 0x002499DC
			internal ElementContentContainer(Type elementType, PropertyRecord[] localValues, ResourceDictionary resources, TextTreeDeleteContentUndoUnit.ContentContainer childContainer)
			{
				this._elementType = elementType;
				this._localValues = localValues;
				this._childContainer = childContainer;
				this._resources = resources;
			}

			// Token: 0x06008609 RID: 34313 RVA: 0x0024B804 File Offset: 0x00249A04
			internal override void Do(TextPointer navigator)
			{
				TextElement textElement = (TextElement)Activator.CreateInstance(this._elementType);
				textElement.Reposition(navigator, navigator);
				navigator.MoveToNextContextPosition(LogicalDirection.Backward);
				navigator.TextContainer.SetValues(navigator, TextTreeUndoUnit.ArrayToLocalValueEnumerator(this._localValues));
				textElement.Resources = this._resources;
				for (TextTreeDeleteContentUndoUnit.ContentContainer contentContainer = this._childContainer; contentContainer != null; contentContainer = contentContainer.NextContainer)
				{
					contentContainer.Do(navigator);
				}
				navigator.MoveToNextContextPosition(LogicalDirection.Forward);
			}

			// Token: 0x04004345 RID: 17221
			private readonly Type _elementType;

			// Token: 0x04004346 RID: 17222
			private readonly PropertyRecord[] _localValues;

			// Token: 0x04004347 RID: 17223
			private readonly ResourceDictionary _resources;

			// Token: 0x04004348 RID: 17224
			private readonly TextTreeDeleteContentUndoUnit.ContentContainer _childContainer;
		}

		// Token: 0x0200091A RID: 2330
		private class TableElementContentContainer : TextTreeDeleteContentUndoUnit.ElementContentContainer
		{
			// Token: 0x0600860A RID: 34314 RVA: 0x0024B877 File Offset: 0x00249A77
			internal TableElementContentContainer(Table table, PropertyRecord[] localValues, TextTreeDeleteContentUndoUnit.ContentContainer childContainer) : base(table.GetType(), localValues, table.Resources, childContainer)
			{
				this._cpTable = table.TextContainer.Start.GetOffsetToPosition(table.ContentStart);
				this._columns = TextTreeDeleteContentUndoUnit.SaveColumns(table);
			}

			// Token: 0x0600860B RID: 34315 RVA: 0x0024B8B8 File Offset: 0x00249AB8
			internal override void Do(TextPointer navigator)
			{
				base.Do(navigator);
				if (this._columns != null)
				{
					TextPointer textPointer = new TextPointer(navigator.TextContainer.Start, this._cpTable, LogicalDirection.Forward);
					Table table = (Table)textPointer.Parent;
					TextTreeDeleteContentUndoUnit.RestoreColumns(table, this._columns);
				}
			}

			// Token: 0x04004349 RID: 17225
			private TableColumn[] _columns;

			// Token: 0x0400434A RID: 17226
			private int _cpTable;
		}
	}
}
