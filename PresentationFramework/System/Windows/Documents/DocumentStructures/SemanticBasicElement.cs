using System;
using System.Collections.Generic;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>An XML element in the markup for XML Paper Specification (XPS) documents. </summary>
	// Token: 0x0200044D RID: 1101
	public class SemanticBasicElement : BlockElement
	{
		// Token: 0x06003FF8 RID: 16376 RVA: 0x00125A47 File Offset: 0x00123C47
		internal SemanticBasicElement()
		{
			this._elementList = new List<BlockElement>();
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06003FF9 RID: 16377 RVA: 0x00125A5A File Offset: 0x00123C5A
		internal List<BlockElement> BlockElementList
		{
			get
			{
				return this._elementList;
			}
		}

		// Token: 0x0400275A RID: 10074
		internal List<BlockElement> _elementList;
	}
}
