using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a list of items in a document.</summary>
	// Token: 0x02000451 RID: 1105
	public class ListStructure : SemanticBasicElement, IAddChild, IEnumerable<ListItemStructure>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.ListStructure" /> class. </summary>
		// Token: 0x0600400C RID: 16396 RVA: 0x00125C03 File Offset: 0x00123E03
		public ListStructure()
		{
			this._elementType = FixedElement.ElementType.List;
		}

		/// <summary>Adds a list item to the list.</summary>
		/// <param name="listItem">The list item to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="listItem" /> is <see langword="null" />.</exception>
		// Token: 0x0600400D RID: 16397 RVA: 0x00125C13 File Offset: 0x00123E13
		public void Add(ListItemStructure listItem)
		{
			if (listItem == null)
			{
				throw new ArgumentNullException("listItem");
			}
			((IAddChild)this).AddChild(listItem);
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> that is added.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks.</exception>
		// Token: 0x0600400E RID: 16398 RVA: 0x00125C2C File Offset: 0x00123E2C
		void IAddChild.AddChild(object value)
		{
			if (value is ListItemStructure)
			{
				this._elementList.Add((ListItemStructure)value);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(ListItemStructure)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.  </param>
		// Token: 0x0600400F RID: 16399 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x00041C10 File Offset: 0x0003FE10
		IEnumerator<ListItemStructure> IEnumerable<ListItemStructure>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x06004011 RID: 16401 RVA: 0x00125C83 File Offset: 0x00123E83
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ListItemStructure>)this).GetEnumerator();
		}
	}
}
