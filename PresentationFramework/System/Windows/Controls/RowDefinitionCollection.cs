using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	/// <summary>Provides access to an ordered, strongly typed collection of <see cref="T:System.Windows.Controls.RowDefinition" /> objects.</summary>
	// Token: 0x02000572 RID: 1394
	public sealed class RowDefinitionCollection : IList<RowDefinition>, ICollection<RowDefinition>, IEnumerable<RowDefinition>, IEnumerable, IList, ICollection
	{
		// Token: 0x06005BBE RID: 23486 RVA: 0x0019CE21 File Offset: 0x0019B021
		internal RowDefinitionCollection(Grid owner)
		{
			this._owner = owner;
			this.PrivateOnModified();
		}

		/// <summary>Copies the elements of the collection to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">A zero-based <see cref="T:System.Array" /> that receives the copied items from the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <param name="index">The first position in the specified <see cref="T:System.Array" /> to receive the copied contents.</param>
		// Token: 0x06005BBF RID: 23487 RVA: 0x0019CE38 File Offset: 0x0019B038
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidRank"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("GridCollection_DestArrayInvalidLowerBound", new object[]
				{
					"index"
				}));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidLength", new object[]
				{
					"array"
				}));
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		/// <summary>Copies an array of <see cref="T:System.Windows.Controls.RowDefinition" /> objects to a given index position within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="array">An array of <see cref="T:System.Windows.Controls.RowDefinition" /> objects.</param>
		/// <param name="index">Identifies the index position within <paramref name="array" /> to which the <see cref="T:System.Windows.Controls.RowDefinition" /> objects are copied.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="array" /> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from index to the end of the destination array. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero. </exception>
		// Token: 0x06005BC0 RID: 23488 RVA: 0x0019CEDC File Offset: 0x0019B0DC
		public void CopyTo(RowDefinition[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("GridCollection_DestArrayInvalidLowerBound", new object[]
				{
					"index"
				}));
			}
			if (array.Length - index < this._size)
			{
				throw new ArgumentException(SR.Get("GridCollection_DestArrayInvalidLength", new object[]
				{
					"array"
				}));
			}
			if (this._size > 0)
			{
				Array.Copy(this._items, 0, array, index, this._size);
			}
		}

		/// <summary>Adds an item to the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		// Token: 0x06005BC1 RID: 23489 RVA: 0x0019CF61 File Offset: 0x0019B161
		int IList.Add(object value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value as RowDefinition);
			return this._size - 1;
		}

		/// <summary>Adds a <see cref="T:System.Windows.Controls.RowDefinition" /> element to a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="T:System.Windows.Controls.RowDefinition" /> to add to the collection.</param>
		// Token: 0x06005BC2 RID: 23490 RVA: 0x0019CF8A File Offset: 0x0019B18A
		public void Add(RowDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(this._size, value);
		}

		/// <summary>Clears the content of the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		// Token: 0x06005BC3 RID: 23491 RVA: 0x0019CFA8 File Offset: 0x0019B1A8
		public void Clear()
		{
			this.PrivateVerifyWriteAccess();
			this.PrivateOnModified();
			for (int i = 0; i < this._size; i++)
			{
				this.PrivateDisconnectChild(this._items[i]);
				this._items[i] = null;
			}
			this._size = 0;
		}

		/// <summary>Determines whether the collection contains a specific value.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005BC4 RID: 23492 RVA: 0x0019CFF0 File Offset: 0x0019B1F0
		bool IList.Contains(object value)
		{
			RowDefinition rowDefinition = value as RowDefinition;
			return rowDefinition != null && rowDefinition.Parent == this._owner;
		}

		/// <summary>Determines whether a given <see cref="T:System.Windows.Controls.RowDefinition" /> exists within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">Identifies the <see cref="T:System.Windows.Controls.RowDefinition" /> that is being tested.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.RowDefinition" /> exists within the collection; otherwise <see langword="false" />.</returns>
		// Token: 0x06005BC5 RID: 23493 RVA: 0x0019D018 File Offset: 0x0019B218
		public bool Contains(RowDefinition value)
		{
			return value != null && value.Parent == this._owner;
		}

		/// <summary>Determines the index of a specific item in the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x06005BC6 RID: 23494 RVA: 0x0019D02E File Offset: 0x0019B22E
		int IList.IndexOf(object value)
		{
			return this.IndexOf(value as RowDefinition);
		}

		/// <summary>Returns the index position of a given <see cref="T:System.Windows.Controls.RowDefinition" /> within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Controls.RowDefinition" /> whose index position is desired.</param>
		/// <returns>The index of <paramref name="value" /> if found in the collection; otherwise, -1.</returns>
		// Token: 0x06005BC7 RID: 23495 RVA: 0x0019D03C File Offset: 0x0019B23C
		public int IndexOf(RowDefinition value)
		{
			if (value == null || value.Parent != this._owner)
			{
				return -1;
			}
			return value.Index;
		}

		/// <summary>Inserts an item to the collection at the specified index.</summary>
		/// <param name="index">The zero-based index at which to insert the <see cref="T:System.Object" />.</param>
		/// <param name="value">The <see cref="T:System.Object" /> to insert into the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		// Token: 0x06005BC8 RID: 23496 RVA: 0x0019D057 File Offset: 0x0019B257
		void IList.Insert(int index, object value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value as RowDefinition);
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Controls.RowDefinition" /> at the specified index position within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />. </summary>
		/// <param name="index">The position within the collection where the item is inserted.</param>
		/// <param name="value">The <see cref="T:System.Windows.Controls.RowDefinition" /> to insert.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />. </exception>
		// Token: 0x06005BC9 RID: 23497 RVA: 0x0019D090 File Offset: 0x0019B290
		public void Insert(int index, RowDefinition value)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index > this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateValidateValueForAddition(value);
			this.PrivateInsert(index, value);
		}

		/// <summary>Removes the first occurrence of a specific object from the collection.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to remove from the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</param>
		// Token: 0x06005BCA RID: 23498 RVA: 0x0019D0C4 File Offset: 0x0019B2C4
		void IList.Remove(object value)
		{
			this.PrivateVerifyWriteAccess();
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value as RowDefinition);
			}
		}

		/// <summary>Removes a <see cref="T:System.Windows.Controls.RowDefinition" /> from a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <param name="value">The <see cref="T:System.Windows.Controls.RowDefinition" /> to remove from the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.RowDefinition" /> was found in the collection and removed; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005BCB RID: 23499 RVA: 0x0019D0F0 File Offset: 0x0019B2F0
		public bool Remove(RowDefinition value)
		{
			bool flag = this.PrivateValidateValueForRemoval(value);
			if (flag)
			{
				this.PrivateRemove(value);
			}
			return flag;
		}

		/// <summary>Removes a <see cref="T:System.Windows.Controls.RowDefinition" /> from a <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> at the specified index position.</summary>
		/// <param name="index">The position within the collection at which the <see cref="T:System.Windows.Controls.RowDefinition" /> is removed.</param>
		// Token: 0x06005BCC RID: 23500 RVA: 0x0019D110 File Offset: 0x0019B310
		public void RemoveAt(int index)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			this.PrivateRemove(this._items[index]);
		}

		/// <summary>Removes a range of <see cref="T:System.Windows.Controls.RowDefinition" /> objects from a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />. </summary>
		/// <param name="index">The position within the collection at which the first <see cref="T:System.Windows.Controls.RowDefinition" /> is removed.</param>
		/// <param name="count">The total number of <see cref="T:System.Windows.Controls.RowDefinition" /> objects to remove from the collection.</param>
		// Token: 0x06005BCD RID: 23501 RVA: 0x0019D144 File Offset: 0x0019B344
		public void RemoveRange(int index, int count)
		{
			this.PrivateVerifyWriteAccess();
			if (index < 0 || index >= this._size)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(SR.Get("TableCollectionCountNeedNonNegNum"));
			}
			if (this._size - index < count)
			{
				throw new ArgumentException(SR.Get("TableCollectionRangeOutOfRange"));
			}
			this.PrivateOnModified();
			if (count > 0)
			{
				for (int i = index + count - 1; i >= index; i--)
				{
					this.PrivateDisconnectChild(this._items[i]);
				}
				this._size -= count;
				for (int j = index; j < this._size; j++)
				{
					this._items[j] = this._items[j + count];
					this._items[j].Index = j;
					this._items[j + count] = null;
				}
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
		// Token: 0x06005BCE RID: 23502 RVA: 0x0019D215 File Offset: 0x0019B415
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new RowDefinitionCollection.Enumerator(this);
		}

		// Token: 0x06005BCF RID: 23503 RVA: 0x0019D215 File Offset: 0x0019B415
		IEnumerator<RowDefinition> IEnumerable<RowDefinition>.GetEnumerator()
		{
			return new RowDefinitionCollection.Enumerator(this);
		}

		/// <summary>Gets the total number of items within this instance of <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <returns>The total number of items in the collection. This property has no default value.</returns>
		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x06005BD0 RID: 23504 RVA: 0x0019D222 File Offset: 0x0019B422
		public int Count
		{
			get
			{
				return this._size;
			}
		}

		/// <summary>Gets a value indicating whether the collection has a fixed size.</summary>
		/// <returns>
		///     <see langword="true" /> if the the <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x06005BD1 RID: 23505 RVA: 0x0019D22A File Offset: 0x0019B42A
		bool IList.IsFixedSize
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether a <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> is read-only. </summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise <see langword="false" />. This property has no default value.</returns>
		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x06005BD2 RID: 23506 RVA: 0x0019D22A File Offset: 0x0019B42A
		public bool IsReadOnly
		{
			get
			{
				return this._owner.MeasureOverrideInProgress || this._owner.ArrangeOverrideInProgress;
			}
		}

		/// <summary>Gets a value that indicates whether access to this <see cref="T:System.Windows.Controls.RowDefinitionCollection" /> is synchronized (thread-safe).</summary>
		/// <returns>
		///     <see langword="true" /> if access to this collection is synchronized; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x06005BD3 RID: 23507 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Windows.Controls.RowDefinitionCollection" />.</returns>
		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x06005BD4 RID: 23508 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the list.</exception>
		// Token: 0x17001640 RID: 5696
		object IList.this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value as RowDefinition);
			}
		}

		/// <summary>Gets a value that indicates the current item within a <see cref="T:System.Windows.Controls.RowDefinitionCollection" />. </summary>
		/// <param name="index">The current item in the collection.</param>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is not a valid index position in the collection.</exception>
		// Token: 0x17001641 RID: 5697
		public RowDefinition this[int index]
		{
			get
			{
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				return (RowDefinition)this._items[index];
			}
			set
			{
				this.PrivateVerifyWriteAccess();
				this.PrivateValidateValueForAddition(value);
				if (index < 0 || index >= this._size)
				{
					throw new ArgumentOutOfRangeException(SR.Get("TableCollectionOutOfRange"));
				}
				this.PrivateDisconnectChild(this._items[index]);
				this.PrivateConnectChild(index, value);
			}
		}

		// Token: 0x06005BD9 RID: 23513 RVA: 0x0019D33D File Offset: 0x0019B53D
		internal void InternalTrimToSize()
		{
			this.PrivateSetCapacity(this._size);
		}

		// Token: 0x17001642 RID: 5698
		// (get) Token: 0x06005BDA RID: 23514 RVA: 0x0019D222 File Offset: 0x0019B422
		internal int InternalCount
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x06005BDB RID: 23515 RVA: 0x0019D34B File Offset: 0x0019B54B
		internal DefinitionBase[] InternalItems
		{
			get
			{
				return this._items;
			}
		}

		// Token: 0x06005BDC RID: 23516 RVA: 0x0019D353 File Offset: 0x0019B553
		private void PrivateVerifyWriteAccess()
		{
			if (this.IsReadOnly)
			{
				throw new InvalidOperationException(SR.Get("GridCollection_CannotModifyReadOnly", new object[]
				{
					"RowDefinitionCollection"
				}));
			}
		}

		// Token: 0x06005BDD RID: 23517 RVA: 0x0019D37C File Offset: 0x0019B57C
		private void PrivateValidateValueForAddition(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			RowDefinition rowDefinition = value as RowDefinition;
			if (rowDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"RowDefinitionCollection",
					"RowDefinition"
				}));
			}
			if (rowDefinition.Parent != null)
			{
				throw new ArgumentException(SR.Get("GridCollection_InOtherCollection", new object[]
				{
					"value",
					"RowDefinitionCollection"
				}));
			}
		}

		// Token: 0x06005BDE RID: 23518 RVA: 0x0019D3F8 File Offset: 0x0019B5F8
		private bool PrivateValidateValueForRemoval(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			RowDefinition rowDefinition = value as RowDefinition;
			if (rowDefinition == null)
			{
				throw new ArgumentException(SR.Get("GridCollection_MustBeCertainType", new object[]
				{
					"RowDefinitionCollection",
					"RowDefinition"
				}));
			}
			return rowDefinition.Parent == this._owner;
		}

		// Token: 0x06005BDF RID: 23519 RVA: 0x0019D451 File Offset: 0x0019B651
		private void PrivateConnectChild(int index, DefinitionBase value)
		{
			this._items[index] = value;
			value.Index = index;
			this._owner.AddLogicalChild(value);
			value.OnEnterParentTree();
		}

		// Token: 0x06005BE0 RID: 23520 RVA: 0x0019D475 File Offset: 0x0019B675
		private void PrivateDisconnectChild(DefinitionBase value)
		{
			value.OnExitParentTree();
			this._items[value.Index] = null;
			value.Index = -1;
			this._owner.RemoveLogicalChild(value);
		}

		// Token: 0x06005BE1 RID: 23521 RVA: 0x0019D4A0 File Offset: 0x0019B6A0
		private void PrivateInsert(int index, DefinitionBase value)
		{
			this.PrivateOnModified();
			if (this._items == null)
			{
				this.PrivateSetCapacity(4);
			}
			else if (this._size == this._items.Length)
			{
				this.PrivateSetCapacity(Math.Max(this._items.Length * 2, 4));
			}
			for (int i = this._size - 1; i >= index; i--)
			{
				this._items[i + 1] = this._items[i];
				this._items[i].Index = i + 1;
			}
			this._items[index] = null;
			this._size++;
			this.PrivateConnectChild(index, value);
		}

		// Token: 0x06005BE2 RID: 23522 RVA: 0x0019D540 File Offset: 0x0019B740
		private void PrivateRemove(DefinitionBase value)
		{
			this.PrivateOnModified();
			int index = value.Index;
			this.PrivateDisconnectChild(value);
			this._size--;
			for (int i = index; i < this._size; i++)
			{
				this._items[i] = this._items[i + 1];
				this._items[i].Index = i;
			}
			this._items[this._size] = null;
		}

		// Token: 0x06005BE3 RID: 23523 RVA: 0x0019D5AE File Offset: 0x0019B7AE
		private void PrivateOnModified()
		{
			this._version++;
			this._owner.RowDefinitionCollectionDirty = true;
			this._owner.Invalidate();
		}

		// Token: 0x06005BE4 RID: 23524 RVA: 0x0019D5D8 File Offset: 0x0019B7D8
		private void PrivateSetCapacity(int value)
		{
			if (value <= 0)
			{
				this._items = null;
				return;
			}
			if (this._items == null || value != this._items.Length)
			{
				RowDefinition[] array = new RowDefinition[value];
				if (this._size > 0)
				{
					Array.Copy(this._items, 0, array, 0, this._size);
				}
				this._items = array;
			}
		}

		// Token: 0x04002F93 RID: 12179
		private readonly Grid _owner;

		// Token: 0x04002F94 RID: 12180
		private DefinitionBase[] _items;

		// Token: 0x04002F95 RID: 12181
		private int _size;

		// Token: 0x04002F96 RID: 12182
		private int _version;

		// Token: 0x04002F97 RID: 12183
		private const int c_defaultCapacity = 4;

		// Token: 0x020009E0 RID: 2528
		internal struct Enumerator : IEnumerator<RowDefinition>, IDisposable, IEnumerator
		{
			// Token: 0x0600894D RID: 35149 RVA: 0x00254955 File Offset: 0x00252B55
			internal Enumerator(RowDefinitionCollection collection)
			{
				this._collection = collection;
				this._index = -1;
				this._version = ((this._collection != null) ? this._collection._version : -1);
				this._currentElement = collection;
			}

			// Token: 0x0600894E RID: 35150 RVA: 0x00254988 File Offset: 0x00252B88
			public bool MoveNext()
			{
				if (this._collection == null)
				{
					return false;
				}
				this.PrivateValidate();
				if (this._index < this._collection._size - 1)
				{
					this._index++;
					this._currentElement = this._collection[this._index];
					return true;
				}
				this._currentElement = this._collection;
				this._index = this._collection._size;
				return false;
			}

			// Token: 0x17001F0C RID: 7948
			// (get) Token: 0x0600894F RID: 35151 RVA: 0x002549FF File Offset: 0x00252BFF
			object IEnumerator.Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x17001F0D RID: 7949
			// (get) Token: 0x06008950 RID: 35152 RVA: 0x00254A40 File Offset: 0x00252C40
			public RowDefinition Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (RowDefinition)this._currentElement;
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x06008951 RID: 35153 RVA: 0x00254A8F File Offset: 0x00252C8F
			public void Reset()
			{
				if (this._collection == null)
				{
					return;
				}
				this.PrivateValidate();
				this._currentElement = this._collection;
				this._index = -1;
			}

			// Token: 0x06008952 RID: 35154 RVA: 0x00254AB3 File Offset: 0x00252CB3
			public void Dispose()
			{
				this._currentElement = null;
			}

			// Token: 0x06008953 RID: 35155 RVA: 0x00254ABC File Offset: 0x00252CBC
			private void PrivateValidate()
			{
				if (this._currentElement == null)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorCollectionDisposed"));
				}
				if (this._version != this._collection._version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
			}

			// Token: 0x04004647 RID: 17991
			private RowDefinitionCollection _collection;

			// Token: 0x04004648 RID: 17992
			private int _index;

			// Token: 0x04004649 RID: 17993
			private int _version;

			// Token: 0x0400464A RID: 17994
			private object _currentElement;
		}
	}
}
