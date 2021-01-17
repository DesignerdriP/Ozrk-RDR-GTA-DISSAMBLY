using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a cell in a table.</summary>
	// Token: 0x02000456 RID: 1110
	public class TableCellStructure : SemanticBasicElement, IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.TableCellStructure" /> class. </summary>
		// Token: 0x0600402C RID: 16428 RVA: 0x00125EDB File Offset: 0x001240DB
		public TableCellStructure()
		{
			this._elementType = FixedElement.ElementType.TableCell;
			this._rowSpan = 1;
			this._columnSpan = 1;
		}

		/// <summary>Adds a block element to the table cell.</summary>
		/// <param name="element">The element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The element is <see langword="null" />.</exception>
		// Token: 0x0600402D RID: 16429 RVA: 0x00125A72 File Offset: 0x00123C72
		public void Add(BlockElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		/// <summary>Adds a child object to the <see cref="T:System.Windows.Documents.DocumentStructures.TableCellStructure" />. </summary>
		/// <param name="value">The child object to add.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks. </exception>
		// Token: 0x0600402E RID: 16430 RVA: 0x00125EFC File Offset: 0x001240FC
		void IAddChild.AddChild(object value)
		{
			if (value is ParagraphStructure || value is TableStructure || value is ListStructure || value is FigureStructure)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType4", new object[]
			{
				value.GetType(),
				typeof(ParagraphStructure),
				typeof(TableStructure),
				typeof(ListStructure),
				typeof(FigureStructure)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x0600402F RID: 16431 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x00041C10 File Offset: 0x0003FE10
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This API is not implemented.</summary>
		/// <returns>This API is not implemented.</returns>
		/// <exception cref="T:System.NotSupportedException">In all cases.</exception>
		// Token: 0x06004031 RID: 16433 RVA: 0x00125B22 File Offset: 0x00123D22
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}

		/// <summary>Gets or sets the number of rows spanned by the cell.</summary>
		/// <returns>The number of rows that the cell spans. The default is 1.</returns>
		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x00125F92 File Offset: 0x00124192
		// (set) Token: 0x06004033 RID: 16435 RVA: 0x00125F9A File Offset: 0x0012419A
		public int RowSpan
		{
			get
			{
				return this._rowSpan;
			}
			set
			{
				this._rowSpan = value;
			}
		}

		/// <summary>Gets or sets the number of columns spanned by the cell.</summary>
		/// <returns>The number of columns that the cell spans. The default is 1.</returns>
		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x00125FA3 File Offset: 0x001241A3
		// (set) Token: 0x06004035 RID: 16437 RVA: 0x00125FAB File Offset: 0x001241AB
		public int ColumnSpan
		{
			get
			{
				return this._columnSpan;
			}
			set
			{
				this._columnSpan = value;
			}
		}

		// Token: 0x0400275C RID: 10076
		private int _rowSpan;

		// Token: 0x0400275D RID: 10077
		private int _columnSpan;
	}
}
