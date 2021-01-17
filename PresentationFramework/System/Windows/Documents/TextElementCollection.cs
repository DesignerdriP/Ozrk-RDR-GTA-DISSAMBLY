using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using MS.Internal;

namespace System.Windows.Documents
{
	/// <summary>Provides standard facilities for creating and managing a type-safe, ordered collection of <see cref="T:System.Windows.Documents.TextElement" /> objects.  This is a generic collection for working with objects of a specified type that derives from <see cref="T:System.Windows.Documents.TextElement" />.</summary>
	/// <typeparam name="TextElementType">Type specifier for the generic collection.  Acceptable types are constrained to a type of <see cref="T:System.Windows.Documents.TextElement" /> or a descendant of <see cref="T:System.Windows.Documents.TextElement" />.</typeparam>
	// Token: 0x02000401 RID: 1025
	public class TextElementCollection<TextElementType> : IList, ICollection, IEnumerable, ICollection<TextElementType>, IEnumerable<TextElementType> where TextElementType : TextElement
	{
		// Token: 0x06003977 RID: 14711 RVA: 0x00104828 File Offset: 0x00102A28
		internal TextElementCollection(DependencyObject owner, bool isOwnerParent)
		{
			if (isOwnerParent)
			{
				Invariant.Assert(owner is TextElement || owner is FlowDocument || owner is TextBlock);
			}
			else
			{
				Invariant.Assert(owner is TextElement);
			}
			this._owner = owner;
			this._isOwnerParent = isOwnerParent;
			this._indexCache = new TextElementCollection<TextElementType>.ElementIndexCache(-1, default(TextElementType));
		}

		/// <summary>Appends a specified item to the collection.</summary>
		/// <param name="item">An item to append to the collection.</param>
		/// <exception cref="T:System.ArgumentException">Raised when item already belongs to a collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when item is null.</exception>
		// Token: 0x06003978 RID: 14712 RVA: 0x00104894 File Offset: 0x00102A94
		public void Add(TextElementType item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateChild(item);
			this.TextContainer.BeginChange();
			try
			{
				item.RepositionWithContent(this.ContentEnd);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		/// <summary>Clears all items from the collection.</summary>
		// Token: 0x06003979 RID: 14713 RVA: 0x001048F8 File Offset: 0x00102AF8
		public void Clear()
		{
			TextContainer textContainer = this.TextContainer;
			textContainer.BeginChange();
			try
			{
				textContainer.DeleteContentInternal(this.ContentStart, this.ContentEnd);
			}
			finally
			{
				textContainer.EndChange();
			}
		}

		/// <summary>Queries for the presence of a specified item in the collection.</summary>
		/// <param name="item">An item to query for the presence of in the collection.</param>
		/// <returns>
		///     true if the specified item is present in the collection; otherwise, false.</returns>
		// Token: 0x0600397A RID: 14714 RVA: 0x00104940 File Offset: 0x00102B40
		public bool Contains(TextElementType item)
		{
			if (item == null)
			{
				return false;
			}
			TextElementType textElementType = this.FirstChild;
			while (textElementType != null && textElementType != item)
			{
				textElementType = (TextElementType)((object)textElementType.NextElement);
			}
			return textElementType == item;
		}

		/// <summary>Copies the contents of the collection and inserts them into a specified array starting at a specified index position in the array.</summary>
		/// <param name="array">A one-dimensional array to which the collection contents will be copied.  This array must use zero-based indexing.</param>
		/// <param name="arrayIndex">A zero-based index in <paramref name="array" /> specifying the position at which to begin inserting the copied collection objects.</param>
		/// <exception cref="T:System.ArgumentException">Raised when array includes items that are not compatible with the type <see cref="T:System.Windows.Documents.TextElement" />, or if arrayIndex specifies a position that falls outside of the bounds of array.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when array is null.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">Raised when arrayIndex is less than 0.</exception>
		// Token: 0x0600397B RID: 14715 RVA: 0x000DB9B4 File Offset: 0x000D9BB4
		public void CopyTo(TextElementType[] array, int arrayIndex)
		{
			((ICollection)this).CopyTo(array, arrayIndex);
		}

		/// <summary>Gets the number of items currently in the collection.</summary>
		/// <returns>The number of items currently in the collection.</returns>
		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x0600397C RID: 14716 RVA: 0x00104998 File Offset: 0x00102B98
		public int Count
		{
			get
			{
				int num = 0;
				TextElement textElement;
				if (this._indexCache.IsValid(this))
				{
					textElement = this._indexCache.Element;
					num += this._indexCache.Index;
				}
				else
				{
					textElement = this.FirstChild;
				}
				while (textElement != null)
				{
					num++;
					textElement = textElement.NextElement;
				}
				return num;
			}
		}

		/// <summary>Gets a value that indicates whether or not the collection is read-only.</summary>
		/// <returns>
		///     true if the collection is read-only; otherwise, false.</returns>
		// Token: 0x17000E95 RID: 3733
		// (get) Token: 0x0600397D RID: 14717 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		/// <summary>Removes a specified item from the collection.</summary>
		/// <param name="item">An item to be removed fro the collection.</param>
		/// <returns>
		///     true if the specified item was found and removed; otherwise, false.</returns>
		// Token: 0x0600397E RID: 14718 RVA: 0x001049F4 File Offset: 0x00102BF4
		public bool Remove(TextElementType item)
		{
			if (item == null)
			{
				return false;
			}
			if (item.Parent != this.Parent)
			{
				return false;
			}
			TextContainer textContainer = this.TextContainer;
			textContainer.BeginChange();
			try
			{
				item.RepositionWithContent(null);
			}
			finally
			{
				textContainer.EndChange();
			}
			return true;
		}

		/// <summary>Inserts a specified item in the collection after a specified collection item.</summary>
		/// <param name="previousSibling">An item in the collection after which the new item will be inserted.</param>
		/// <param name="newItem">An item to insert into the collection.</param>
		/// <exception cref="T:System.ArgumentException">Raised when newItem already belongs to a collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when previousSibling or newItem is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">Raised when previousSibling does not belong to this collection.</exception>
		// Token: 0x0600397F RID: 14719 RVA: 0x00104A54 File Offset: 0x00102C54
		public void InsertAfter(TextElementType previousSibling, TextElementType newItem)
		{
			if (previousSibling == null)
			{
				throw new ArgumentNullException("previousSibling");
			}
			if (newItem == null)
			{
				throw new ArgumentNullException("newItem");
			}
			if (previousSibling.Parent != this.Parent)
			{
				throw new InvalidOperationException(SR.Get("TextElementCollection_PreviousSiblingDoesNotBelongToThisCollection", new object[]
				{
					previousSibling.GetType().Name
				}));
			}
			if (newItem.Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					base.GetType().Name
				}));
			}
			this.ValidateChild(newItem);
			this.TextContainer.BeginChange();
			try
			{
				newItem.RepositionWithContent(previousSibling.ElementEnd);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		/// <summary>Inserts a specified item in the collection before a specified collection item.</summary>
		/// <param name="nextSibling">An item in the collection before which the new item will be inserted.</param>
		/// <param name="newItem">An item to insert into the collection.</param>
		/// <exception cref="T:System.ArgumentException">Raised when newItem already belongs to a collection.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when nextSibling or newItem is null.</exception>
		/// <exception cref="T:System.InvalidOperationException">Raised when nextSibling does not belong to this collection.</exception>
		// Token: 0x06003980 RID: 14720 RVA: 0x00104B3C File Offset: 0x00102D3C
		public void InsertBefore(TextElementType nextSibling, TextElementType newItem)
		{
			if (nextSibling == null)
			{
				throw new ArgumentNullException("nextSibling");
			}
			if (newItem == null)
			{
				throw new ArgumentNullException("newItem");
			}
			if (nextSibling.Parent != this.Parent)
			{
				throw new InvalidOperationException(SR.Get("TextElementCollection_NextSiblingDoesNotBelongToThisCollection", new object[]
				{
					nextSibling.GetType().Name
				}));
			}
			if (newItem.Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					base.GetType().Name
				}));
			}
			this.ValidateChild(newItem);
			this.TextContainer.BeginChange();
			try
			{
				newItem.RepositionWithContent(nextSibling.ElementStart);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		/// <summary>Appends a specified range of items to the collection.</summary>
		/// <param name="range">An object that implements the <see cref="T:System.Collections.IEnumerable" /> interface, and that specifies a range of items to add to the collection.</param>
		/// <exception cref="T:System.ArgumentException">Raised when range includes any null items.</exception>
		/// <exception cref="T:System.ArgumentNullException">Raised when range is null.</exception>
		// Token: 0x06003981 RID: 14721 RVA: 0x00104C24 File Offset: 0x00102E24
		public void AddRange(IEnumerable range)
		{
			if (range == null)
			{
				throw new ArgumentNullException("range");
			}
			IEnumerator enumerator = range.GetEnumerator();
			if (enumerator == null)
			{
				throw new ArgumentException(SR.Get("TextElementCollection_NoEnumerator"), "range");
			}
			this.TextContainer.BeginChange();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					TextElementType textElementType = obj as TextElementType;
					if (textElementType == null)
					{
						throw new ArgumentException(SR.Get("TextElementCollection_ItemHasUnexpectedType", new object[]
						{
							"range",
							typeof(TextElementType).Name,
							typeof(TextElementType).Name
						}), "value");
					}
					this.Add(textElementType);
				}
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		/// <summary>Returns an enumerator for the contents of the collection.</summary>
		/// <returns>An enumerator for the contents of the collection.</returns>
		// Token: 0x06003982 RID: 14722 RVA: 0x00104CF8 File Offset: 0x00102EF8
		public IEnumerator<TextElementType> GetEnumerator()
		{
			return new TextElementEnumerator<TextElementType>(this.ContentStart, this.ContentEnd);
		}

		/// <summary>Returns an enumerator that iterates through a collection.  Use the type-safe <see cref="M:System.Windows.Documents.TextElementCollection`1.GetEnumerator" /> method instead.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06003983 RID: 14723 RVA: 0x00104D0B File Offset: 0x00102F0B
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new RangeContentEnumerator(this.ContentStart, this.ContentEnd);
		}

		// Token: 0x06003984 RID: 14724 RVA: 0x00104D20 File Offset: 0x00102F20
		internal virtual int OnAdd(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is TextElementType))
			{
				throw new ArgumentException(SR.Get("TextElementCollection_TextElementTypeExpected", new object[]
				{
					typeof(TextElementType).Name
				}), "value");
			}
			this.ValidateChild((TextElementType)((object)value));
			this.TextContainer.BeginChange();
			int result;
			try
			{
				bool isCacheSafePreviousIndex = this._indexCache.IsValid(this);
				this.Add((TextElementType)((object)value));
				result = this.IndexOfInternal(value, isCacheSafePreviousIndex);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
			return result;
		}

		/// <summary>Adds an item to the <see cref="T:System.Collections.IList" />.  Use the type-safe <see cref="M:System.Windows.Documents.TextElementCollection`1.Add(`0)" /> method instead.</summary>
		/// <param name="value">The object to add to the <see cref="T:System.Collections.IList" />. </param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06003985 RID: 14725 RVA: 0x00104DC8 File Offset: 0x00102FC8
		int IList.Add(object value)
		{
			return this.OnAdd(value);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.  Use the type-safe <see cref="M:System.Windows.Documents.TextElementCollection`1.Clear" /> method instead.</summary>
		// Token: 0x06003986 RID: 14726 RVA: 0x00104DD1 File Offset: 0x00102FD1
		void IList.Clear()
		{
			this.Clear();
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value..  Use the type-safe <see cref="M:System.Windows.Documents.TextElementCollection`1.Contains(`0)" /> method instead.</summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />. </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003987 RID: 14727 RVA: 0x00104DDC File Offset: 0x00102FDC
		bool IList.Contains(object value)
		{
			TextElementType textElementType = value as TextElementType;
			return textElementType != null && this.Contains(textElementType);
		}

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="value">The object to locate in the <see cref="T:System.Collections.IList" />. </param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06003988 RID: 14728 RVA: 0x00104E06 File Offset: 0x00103006
		int IList.IndexOf(object value)
		{
			return this.IndexOfInternal(value, false);
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.  Use the type-safe <see cref="M:System.Windows.Documents.TextElementCollection`1.InsertAfter(`0,`0)" /> or <see cref="M:System.Windows.Documents.TextElementCollection`1.InsertBefore(`0,`0)" /> methods instead.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
		/// <param name="value">The object to insert into the <see cref="T:System.Collections.IList" />. </param>
		// Token: 0x06003989 RID: 14729 RVA: 0x00104E10 File Offset: 0x00103010
		void IList.Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TextElementType textElementType = value as TextElementType;
			if (textElementType == null)
			{
				throw new ArgumentException(SR.Get("TextElementCollection_TextElementTypeExpected", new object[]
				{
					typeof(TextElementType).Name
				}), "value");
			}
			if (index < 0)
			{
				throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
			}
			if (textElementType.Parent != null)
			{
				throw new ArgumentException(SR.Get("TextSchema_TheChildElementBelongsToAnotherTreeAlready", new object[]
				{
					base.GetType().Name
				}));
			}
			this.ValidateChild(textElementType);
			this.TextContainer.BeginChange();
			try
			{
				TextPointer textPointer;
				if (this.FirstChild == null)
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
					}
					textPointer = this.ContentStart;
				}
				else
				{
					bool flag;
					TextElementType elementAtIndex = this.GetElementAtIndex(index, out flag);
					if (!flag && elementAtIndex == null)
					{
						throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
					}
					textPointer = (flag ? this.ContentEnd : elementAtIndex.ElementStart);
				}
				textPointer.InsertTextElement(textElementType);
				this.SetCache(index, textElementType);
			}
			finally
			{
				this.TextContainer.EndChange();
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x0600398A RID: 14730 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x0600398B RID: 14731 RVA: 0x00104F5C File Offset: 0x0010315C
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.  Use the type-safe <see cref="M:System.Windows.Documents.TextElementCollection`1.Remove(`0)" /> method instead.</summary>
		/// <param name="value">The object to remove from the <see cref="T:System.Collections.IList" />. </param>
		// Token: 0x0600398C RID: 14732 RVA: 0x00104F64 File Offset: 0x00103164
		void IList.Remove(object value)
		{
			TextElementType textElementType = value as TextElementType;
			if (textElementType == null)
			{
				return;
			}
			this.Remove(textElementType);
		}

		/// <summary>Removes the <see cref="T:System.Collections.IList" /> item at the specified index.</summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		// Token: 0x0600398D RID: 14733 RVA: 0x00104F8E File Offset: 0x0010318E
		void IList.RemoveAt(int index)
		{
			this.RemoveAtInternal(index);
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set. </param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x17000E98 RID: 3736
		object IList.this[int index]
		{
			get
			{
				if (index < 0)
				{
					throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
				}
				TextElementType elementAtIndex = this.GetElementAtIndex(index);
				if (elementAtIndex == null)
				{
					throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
				}
				this.SetCache(index, elementAtIndex);
				return elementAtIndex;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!(value is TextElementType))
				{
					throw new ArgumentException(SR.Get("TextElementCollection_TextElementTypeExpected", new object[]
					{
						typeof(TextElementType).Name
					}), "value");
				}
				this.ValidateChild((TextElementType)((object)value));
				this.TextContainer.BeginChange();
				try
				{
					TextElementType textElementType = this.RemoveAtInternal(index);
					TextPointer textPointer = (textElementType == null) ? this.ContentEnd : textElementType.ElementStart;
					textPointer.InsertTextElement((TextElementType)((object)value));
					this.SetCache(index, (TextElementType)((object)value));
				}
				finally
				{
					this.TextContainer.EndChange();
				}
			}
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index..  Use the type-safe <see cref="M:System.Windows.Documents.TextElementCollection`1.CopyTo(`0[],System.Int32)" /> method instead.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		// Token: 0x06003990 RID: 14736 RVA: 0x001050B0 File Offset: 0x001032B0
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			int count = this.Count;
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			Type elementType = array.GetType().GetElementType();
			if (elementType == null || !elementType.IsAssignableFrom(typeof(TextElementType)))
			{
				throw new ArgumentException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (arrayIndex > array.Length)
			{
				throw new ArgumentException("arrayIndex");
			}
			if (array.Length < arrayIndex + count)
			{
				throw new ArgumentException(SR.Get("TextElementCollection_CannotCopyToArrayNotSufficientMemory", new object[]
				{
					count,
					arrayIndex,
					array.Length
				}));
			}
			for (TextElementType textElementType = this.FirstChild; textElementType != null; textElementType = (TextElementType)((object)textElementType.NextElement))
			{
				array.SetValue(textElementType, arrayIndex++);
			}
		}

		/// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.  Use the type-safe <see cref="P:System.Windows.Documents.TextElementCollection`1.Count" /> property instead.</summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06003991 RID: 14737 RVA: 0x0010519C File Offset: 0x0010339C
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		/// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
		/// <returns>
		///     <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06003992 RID: 14738 RVA: 0x00016748 File Offset: 0x00014948
		bool ICollection.IsSynchronized
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06003993 RID: 14739 RVA: 0x001051A4 File Offset: 0x001033A4
		object ICollection.SyncRoot
		{
			get
			{
				return this.TextContainer;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06003994 RID: 14740 RVA: 0x001051AC File Offset: 0x001033AC
		internal DependencyObject Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06003995 RID: 14741 RVA: 0x001051B4 File Offset: 0x001033B4
		internal DependencyObject Parent
		{
			get
			{
				if (!this._isOwnerParent)
				{
					return ((TextElement)this._owner).Parent;
				}
				return this._owner;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06003996 RID: 14742 RVA: 0x001051D8 File Offset: 0x001033D8
		internal TextContainer TextContainer
		{
			get
			{
				TextContainer result;
				if (this._owner is TextBlock)
				{
					result = (TextContainer)((TextBlock)this._owner).TextContainer;
				}
				else if (this._owner is FlowDocument)
				{
					result = ((FlowDocument)this._owner).TextContainer;
				}
				else
				{
					result = ((TextElement)this._owner).TextContainer;
				}
				return result;
			}
		}

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06003997 RID: 14743 RVA: 0x0010523C File Offset: 0x0010343C
		internal TextElementType FirstChild
		{
			get
			{
				TextElementType result;
				if (this.Parent is TextElement)
				{
					result = (TextElementType)((object)((TextElement)this.Parent).FirstChildElement);
				}
				else
				{
					TextTreeTextElementNode textTreeTextElementNode = this.TextContainer.FirstContainedNode as TextTreeTextElementNode;
					result = (TextElementType)((object)((textTreeTextElementNode == null) ? null : textTreeTextElementNode.TextElement));
				}
				return result;
			}
		}

		// Token: 0x17000EA0 RID: 3744
		// (get) Token: 0x06003998 RID: 14744 RVA: 0x00105294 File Offset: 0x00103494
		internal TextElementType LastChild
		{
			get
			{
				TextElementType result;
				if (this.Parent is TextElement)
				{
					result = (TextElementType)((object)((TextElement)this.Parent).LastChildElement);
				}
				else
				{
					TextTreeTextElementNode textTreeTextElementNode = this.TextContainer.LastContainedNode as TextTreeTextElementNode;
					result = (TextElementType)((object)((textTreeTextElementNode == null) ? null : textTreeTextElementNode.TextElement));
				}
				return result;
			}
		}

		// Token: 0x06003999 RID: 14745 RVA: 0x001052EC File Offset: 0x001034EC
		private TextElementType RemoveAtInternal(int index)
		{
			if (index < 0)
			{
				throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
			}
			TextElementType elementAtIndex = this.GetElementAtIndex(index);
			if (elementAtIndex == null)
			{
				throw new IndexOutOfRangeException(SR.Get("TextElementCollection_IndexOutOfRange"));
			}
			TextElementType textElementType = (TextElementType)((object)elementAtIndex.NextElement);
			TextContainer textContainer = this.TextContainer;
			textContainer.BeginChange();
			try
			{
				TextElementType textElementType2 = textElementType;
				if (textElementType2 == null)
				{
					textElementType2 = (TextElementType)((object)elementAtIndex.PreviousElement);
					index--;
				}
				elementAtIndex.RepositionWithContent(null);
				if (textElementType2 != null)
				{
					this.SetCache(index, textElementType2);
				}
			}
			finally
			{
				textContainer.EndChange();
			}
			return textElementType;
		}

		// Token: 0x0600399A RID: 14746 RVA: 0x001053A4 File Offset: 0x001035A4
		private TextElementType GetElementAtIndex(int index)
		{
			bool flag;
			return this.GetElementAtIndex(index, out flag);
		}

		// Token: 0x0600399B RID: 14747 RVA: 0x001053BC File Offset: 0x001035BC
		private TextElementType GetElementAtIndex(int index, out bool atCollectionEnd)
		{
			bool flag = true;
			TextElementType textElementType;
			if (this._indexCache.IsValid(this))
			{
				if (this._indexCache.Index == index)
				{
					textElementType = this._indexCache.Element;
					index = 0;
				}
				else if (this._indexCache.Index < index)
				{
					textElementType = this._indexCache.Element;
					index -= this._indexCache.Index;
				}
				else
				{
					textElementType = this._indexCache.Element;
					index = this._indexCache.Index - index;
					flag = false;
				}
			}
			else
			{
				textElementType = this.FirstChild;
			}
			while (index > 0 && textElementType != null)
			{
				textElementType = (TextElementType)((object)(flag ? textElementType.NextElement : textElementType.PreviousElement));
				index--;
			}
			atCollectionEnd = (index == 0 && textElementType == null);
			return textElementType;
		}

		// Token: 0x0600399C RID: 14748 RVA: 0x0010548F File Offset: 0x0010368F
		private void SetCache(int index, TextElementType item)
		{
			this._indexCache = new TextElementCollection<TextElementType>.ElementIndexCache(index, item);
			TextElementCollectionHelper.MarkClean(this.Parent, this);
		}

		// Token: 0x0600399D RID: 14749 RVA: 0x001054AC File Offset: 0x001036AC
		private int IndexOfInternal(object value, bool isCacheSafePreviousIndex)
		{
			TextElementType textElementType = value as TextElementType;
			if (value == null)
			{
				return -1;
			}
			if (this._indexCache.IsValid(this) && textElementType == this._indexCache.Element)
			{
				return this._indexCache.Index;
			}
			int num;
			TextElementType textElementType2;
			if (isCacheSafePreviousIndex)
			{
				num = this._indexCache.Index;
				textElementType2 = this._indexCache.Element;
			}
			else
			{
				num = 0;
				textElementType2 = this.FirstChild;
			}
			while (textElementType2 != null)
			{
				if (textElementType2 == textElementType)
				{
					this.SetCache(num, textElementType);
					return num;
				}
				textElementType2 = (TextElementType)((object)textElementType2.NextElement);
				num++;
			}
			return -1;
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void ValidateChild(TextElementType child)
		{
		}

		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x0600399F RID: 14751 RVA: 0x0010555A File Offset: 0x0010375A
		private TextPointer ContentStart
		{
			get
			{
				if (!(this.Parent is TextElement))
				{
					return this.TextContainer.Start;
				}
				return ((TextElement)this.Parent).ContentStart;
			}
		}

		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x060039A0 RID: 14752 RVA: 0x00105585 File Offset: 0x00103785
		private TextPointer ContentEnd
		{
			get
			{
				if (!(this.Parent is TextElement))
				{
					return this.TextContainer.End;
				}
				return ((TextElement)this.Parent).ContentEnd;
			}
		}

		// Token: 0x040025AE RID: 9646
		private DependencyObject _owner;

		// Token: 0x040025AF RID: 9647
		private bool _isOwnerParent;

		// Token: 0x040025B0 RID: 9648
		private TextElementCollection<TextElementType>.ElementIndexCache _indexCache;

		// Token: 0x02000904 RID: 2308
		private struct ElementIndexCache
		{
			// Token: 0x060085CB RID: 34251 RVA: 0x0024AF86 File Offset: 0x00249186
			internal ElementIndexCache(int index, TextElementType element)
			{
				Invariant.Assert(index == -1 || element != null);
				this._index = index;
				this._element = element;
			}

			// Token: 0x060085CC RID: 34252 RVA: 0x0024AFAB File Offset: 0x002491AB
			internal bool IsValid(TextElementCollection<TextElementType> collection)
			{
				return this._index >= 0 && TextElementCollectionHelper.IsCleanParent(this._element.Parent, collection);
			}

			// Token: 0x17001E34 RID: 7732
			// (get) Token: 0x060085CD RID: 34253 RVA: 0x0024AFCE File Offset: 0x002491CE
			internal int Index
			{
				get
				{
					return this._index;
				}
			}

			// Token: 0x17001E35 RID: 7733
			// (get) Token: 0x060085CE RID: 34254 RVA: 0x0024AFD6 File Offset: 0x002491D6
			internal TextElementType Element
			{
				get
				{
					return this._element;
				}
			}

			// Token: 0x04004300 RID: 17152
			private readonly int _index;

			// Token: 0x04004301 RID: 17153
			private readonly TextElementType _element;
		}
	}
}
