using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a row of one or more cells in a table.</summary>
	// Token: 0x02000455 RID: 1109
	public class TableRowStructure : SemanticBasicElement, IAddChild, IEnumerable<TableCellStructure>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.TableRowStructure" /> class. </summary>
		// Token: 0x06004026 RID: 16422 RVA: 0x00125E53 File Offset: 0x00124053
		public TableRowStructure()
		{
			this._elementType = FixedElement.ElementType.TableRow;
		}

		/// <summary>Adds a cell to a table row.</summary>
		/// <param name="tableCell">The cell to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The cell is <see langword="null" />.</exception>
		// Token: 0x06004027 RID: 16423 RVA: 0x00125E63 File Offset: 0x00124063
		public void Add(TableCellStructure tableCell)
		{
			if (tableCell == null)
			{
				throw new ArgumentNullException("tableCell");
			}
			((IAddChild)this).AddChild(tableCell);
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> that is added.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks.</exception>
		// Token: 0x06004028 RID: 16424 RVA: 0x00125E7C File Offset: 0x0012407C
		void IAddChild.AddChild(object value)
		{
			if (value is TableCellStructure)
			{
				this._elementList.Add((TableCellStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableCellStructure)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06004029 RID: 16425 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x0600402A RID: 16426 RVA: 0x00041C10 File Offset: 0x0003FE10
		IEnumerator<TableCellStructure> IEnumerable<TableCellStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x0600402B RID: 16427 RVA: 0x00125ED3 File Offset: 0x001240D3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TableCellStructure>)this).GetEnumerator();
		}
	}
}
