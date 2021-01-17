using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents a paragraph in a document. </summary>
	// Token: 0x0200044F RID: 1103
	public class ParagraphStructure : SemanticBasicElement, IAddChild, IEnumerable<NamedElement>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.ParagraphStructure" /> class. </summary>
		// Token: 0x06004000 RID: 16384 RVA: 0x00125B2A File Offset: 0x00123D2A
		public ParagraphStructure()
		{
			this._elementType = FixedElement.ElementType.Paragraph;
		}

		/// <summary>Adds a named element to the paragraph.</summary>
		/// <param name="element">The element to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="element" /> is <see langword="null" />.</exception>
		// Token: 0x06004001 RID: 16385 RVA: 0x00125A72 File Offset: 0x00123C72
		public void Add(NamedElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			((IAddChild)this).AddChild(element);
		}

		/// <summary>This member supports the Microsoft .NET Framework infrastructure and is not intended to be used directly from your code. </summary>
		/// <param name="value">The child <see cref="T:System.Object" /> that is added.</param>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="value" /> is not one of the types that can be a child of this class. See Remarks.</exception>
		// Token: 0x06004002 RID: 16386 RVA: 0x00125B3C File Offset: 0x00123D3C
		void IAddChild.AddChild(object value)
		{
			if (value is NamedElement)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType1", new object[]
			{
				value.GetType(),
				typeof(NamedElement)
			}), "value");
		}

		/// <summary>Not implemented.</summary>
		/// <param name="text">Not used.</param>
		// Token: 0x06004003 RID: 16387 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x00041C10 File Offset: 0x0003FE10
		IEnumerator<NamedElement> IEnumerable<NamedElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x06004005 RID: 16389 RVA: 0x00125B93 File Offset: 0x00123D93
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<NamedElement>)this).GetEnumerator();
		}
	}
}
