using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace System.Windows.Documents.DocumentStructures
{
	/// <summary>Represents all or part of a story within an XPS document.</summary>
	// Token: 0x02000458 RID: 1112
	[ContentProperty("BlockElementList")]
	public class StoryFragment : IAddChild, IEnumerable<BlockElement>, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.DocumentStructures.StoryFragment" /> class. </summary>
		// Token: 0x0600403D RID: 16445 RVA: 0x00126047 File Offset: 0x00124247
		public StoryFragment()
		{
			this._elementList = new List<BlockElement>();
		}

		/// <summary>Add a block to the story fragment.</summary>
		/// <param name="element">The block to add.</param>
		/// <exception cref="T:System.ArgumentNullException">The block passed is <see langword="null" />.</exception>
		// Token: 0x0600403E RID: 16446 RVA: 0x00125A72 File Offset: 0x00123C72
		public void Add(BlockElement element)
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
		// Token: 0x0600403F RID: 16447 RVA: 0x0012605C File Offset: 0x0012425C
		void IAddChild.AddChild(object value)
		{
			if (value is SectionStructure || value is ParagraphStructure || value is FigureStructure || value is ListStructure || value is TableStructure || value is StoryBreak)
			{
				this._elementList.Add((BlockElement)value);
				return;
			}
			throw new ArgumentException(SR.Get("DocumentStructureUnexpectedParameterType6", new object[]
			{
				value.GetType(),
				typeof(SectionStructure),
				typeof(ParagraphStructure),
				typeof(FigureStructure),
				typeof(ListStructure),
				typeof(TableStructure),
				typeof(StoryBreak)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06004040 RID: 16448 RVA: 0x00002137 File Offset: 0x00000337
		void IAddChild.AddText(string text)
		{
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x00041C10 File Offset: 0x0003FE10
		IEnumerator<BlockElement> IEnumerable<BlockElement>.GetEnumerator()
		{
			throw new NotSupportedException();
		}

		/// <summary>This method has not been implemented.</summary>
		/// <returns>Always raises <see cref="T:System.NotSupportedException" />.</returns>
		// Token: 0x06004042 RID: 16450 RVA: 0x00125B22 File Offset: 0x00123D22
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BlockElement>)this).GetEnumerator();
		}

		/// <summary>Gets or sets the name of the story. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the name of the story.</returns>
		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06004043 RID: 16451 RVA: 0x0012611C File Offset: 0x0012431C
		// (set) Token: 0x06004044 RID: 16452 RVA: 0x00126124 File Offset: 0x00124324
		public string StoryName
		{
			get
			{
				return this._storyName;
			}
			set
			{
				this._storyName = value;
			}
		}

		/// <summary>Gets or sets the name of the story fragment. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the name of this fragment. </returns>
		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0012612D File Offset: 0x0012432D
		// (set) Token: 0x06004046 RID: 16454 RVA: 0x00126135 File Offset: 0x00124335
		public string FragmentName
		{
			get
			{
				return this._fragmentName;
			}
			set
			{
				this._fragmentName = value;
			}
		}

		/// <summary>Gets or sets the type of fragment. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the type of fragment.</returns>
		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06004047 RID: 16455 RVA: 0x0012613E File Offset: 0x0012433E
		// (set) Token: 0x06004048 RID: 16456 RVA: 0x00126146 File Offset: 0x00124346
		public string FragmentType
		{
			get
			{
				return this._fragmentType;
			}
			set
			{
				this._fragmentType = value;
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06004049 RID: 16457 RVA: 0x0012614F File Offset: 0x0012434F
		internal List<BlockElement> BlockElementList
		{
			get
			{
				return this._elementList;
			}
		}

		// Token: 0x0400275F RID: 10079
		private List<BlockElement> _elementList;

		// Token: 0x04002760 RID: 10080
		private string _storyName;

		// Token: 0x04002761 RID: 10081
		private string _fragmentName;

		// Token: 0x04002762 RID: 10082
		private string _fragmentType;
	}
}
