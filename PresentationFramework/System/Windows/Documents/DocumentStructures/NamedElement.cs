﻿using System;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Identifies an element within the hierarchy of elements under a <see cref="T:System.Windows.Documents.FixedPage" />.</summary>
	// Token: 0x0200044C RID: 1100
	public class NamedElement : BlockElement
	{
		/// <summary>Gets or sets the name of the element in the <see cref="T:System.Windows.Documents.FixedPage" /> markup hierarchy that provides the content for the parent of the <see cref="T:System.Windows.Documents.DocumentStructures.NamedElement" />. </summary>
		/// <returns>The name of the element.</returns>
		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06003FF6 RID: 16374 RVA: 0x00125A36 File Offset: 0x00123C36
		// (set) Token: 0x06003FF7 RID: 16375 RVA: 0x00125A3E File Offset: 0x00123C3E
		public string NameReference
		{
			get
			{
				return this._reference;
			}
			set
			{
				this._reference = value;
			}
		}

		// Token: 0x04002759 RID: 10073
		private string _reference;
	}
}
