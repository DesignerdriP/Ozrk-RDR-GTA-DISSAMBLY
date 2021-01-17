﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Enables multiple collections and items to be displayed as a single list.</summary>
	// Token: 0x020001AB RID: 427
	[Localizability(LocalizationCategory.Ignore)]
	public class CompositeCollection : IList, ICollection, IEnumerable, INotifyCollectionChanged, ICollectionViewFactory, IWeakEventListener
	{
		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Data.CompositeCollection" /> class that is empty and has default initial capacity. </summary>
		// Token: 0x06001B2A RID: 6954 RVA: 0x000802E5 File Offset: 0x0007E4E5
		public CompositeCollection()
		{
			this.Initialize(new ArrayList());
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Windows.Data.CompositeCollection" /> class that is empty and has a specified initial capacity. </summary>
		/// <param name="capacity">The number of items that the new list is initially capable of storing.</param>
		// Token: 0x06001B2B RID: 6955 RVA: 0x000802F8 File Offset: 0x0007E4F8
		public CompositeCollection(int capacity)
		{
			this.Initialize(new ArrayList(capacity));
		}

		/// <summary>Returns an enumerator.</summary>
		/// <returns>An IEnumerator object.</returns>
		// Token: 0x06001B2C RID: 6956 RVA: 0x0008030C File Offset: 0x0007E50C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.InternalList.GetEnumerator();
		}

		/// <summary>Makes a shallow copy of object references from this collection to the given array.</summary>
		/// <param name="array">The array that is the destination of the copy operation.</param>
		/// <param name="index">Zero-based index in the target array at which the copying starts.</param>
		// Token: 0x06001B2D RID: 6957 RVA: 0x00080319 File Offset: 0x0007E519
		public void CopyTo(Array array, int index)
		{
			this.InternalList.CopyTo(array, index);
		}

		/// <summary>Adds the specified item to this collection.</summary>
		/// <param name="newItem">New item to add to the collection.</param>
		/// <returns>Zero-based index where the new item is added.</returns>
		// Token: 0x06001B2E RID: 6958 RVA: 0x00080328 File Offset: 0x0007E528
		public int Add(object newItem)
		{
			CollectionContainer collectionContainer = newItem as CollectionContainer;
			if (collectionContainer != null)
			{
				this.AddCollectionContainer(collectionContainer);
			}
			int num = this.InternalList.Add(newItem);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, newItem, num);
			return num;
		}

		/// <summary>Clears the collection.</summary>
		// Token: 0x06001B2F RID: 6959 RVA: 0x00080360 File Offset: 0x0007E560
		public void Clear()
		{
			int i = 0;
			int count = this.InternalList.Count;
			while (i < count)
			{
				CollectionContainer collectionContainer = this[i] as CollectionContainer;
				if (collectionContainer != null)
				{
					this.RemoveCollectionContainer(collectionContainer);
				}
				i++;
			}
			this.InternalList.Clear();
			this.OnCollectionChanged(NotifyCollectionChangedAction.Reset);
		}

		/// <summary>Checks to see if a given item is in this collection.</summary>
		/// <param name="containItem">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the collection contains the given item; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B30 RID: 6960 RVA: 0x000803AE File Offset: 0x0007E5AE
		public bool Contains(object containItem)
		{
			return this.InternalList.Contains(containItem);
		}

		/// <summary>Returns the index in this collection where the given item is found. </summary>
		/// <param name="indexItem">The item to retrieve the index for.</param>
		/// <returns>If the item appears in the collection, then the zero-based index in the collection where the given item is found; otherwise, -1.</returns>
		// Token: 0x06001B31 RID: 6961 RVA: 0x000803BC File Offset: 0x0007E5BC
		public int IndexOf(object indexItem)
		{
			return this.InternalList.IndexOf(indexItem);
		}

		/// <summary>Inserts an item in the collection at a given index. All items after the given position are moved down by one. </summary>
		/// <param name="insertIndex">The index to insert the item at.</param>
		/// <param name="insertItem">The item reference to add to the collection.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If index is out of range.</exception>
		// Token: 0x06001B32 RID: 6962 RVA: 0x000803CC File Offset: 0x0007E5CC
		public void Insert(int insertIndex, object insertItem)
		{
			CollectionContainer collectionContainer = insertItem as CollectionContainer;
			if (collectionContainer != null)
			{
				this.AddCollectionContainer(collectionContainer);
			}
			this.InternalList.Insert(insertIndex, insertItem);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, insertItem, insertIndex);
		}

		/// <summary>Removes the given item reference from the collection. All remaining items move up by one. </summary>
		/// <param name="removeItem">The item to remove.</param>
		// Token: 0x06001B33 RID: 6963 RVA: 0x00080400 File Offset: 0x0007E600
		public void Remove(object removeItem)
		{
			int num = this.InternalList.IndexOf(removeItem);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		/// <summary>Removes an item from the collection at the given index. All remaining items move up by one. </summary>
		/// <param name="removeIndex">The index at which to remove an item.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If index is out of range.</exception>
		// Token: 0x06001B34 RID: 6964 RVA: 0x00080428 File Offset: 0x0007E628
		public void RemoveAt(int removeIndex)
		{
			if (0 <= removeIndex && removeIndex < this.Count)
			{
				object obj = this[removeIndex];
				CollectionContainer collectionContainer = obj as CollectionContainer;
				if (collectionContainer != null)
				{
					this.RemoveCollectionContainer(collectionContainer);
				}
				this.InternalList.RemoveAt(removeIndex);
				this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, obj, removeIndex);
				return;
			}
			throw new ArgumentOutOfRangeException("removeIndex", SR.Get("ItemCollectionRemoveArgumentOutOfRange"));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The view created.</returns>
		// Token: 0x06001B35 RID: 6965 RVA: 0x00080485 File Offset: 0x0007E685
		ICollectionView ICollectionViewFactory.CreateView()
		{
			return new CompositeCollectionView(this);
		}

		/// <summary>Gets the number of items stored in this collection.</summary>
		/// <returns>The number of items stored in this collection.</returns>
		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06001B36 RID: 6966 RVA: 0x0008048D File Offset: 0x0007E68D
		public int Count
		{
			get
			{
				return this.InternalList.Count;
			}
		}

		/// <summary>Indexer property that retrieves or replaces the item at the given zero-based offset in the collection. </summary>
		/// <param name="itemIndex">The zero-based offset of the item to retrieve or replace.</param>
		/// <returns>The item at the specified zero-based offset.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">If the index is out of range.</exception>
		// Token: 0x17000653 RID: 1619
		public object this[int itemIndex]
		{
			get
			{
				return this.InternalList[itemIndex];
			}
			set
			{
				object obj = this.InternalList[itemIndex];
				CollectionContainer cc;
				if ((cc = (obj as CollectionContainer)) != null)
				{
					this.RemoveCollectionContainer(cc);
				}
				if ((cc = (value as CollectionContainer)) != null)
				{
					this.AddCollectionContainer(cc);
				}
				this.InternalList[itemIndex] = value;
				this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, obj, value, itemIndex);
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, false.</returns>
		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x000804FB File Offset: 0x0007E6FB
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.InternalList.IsSynchronized;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x00080508 File Offset: 0x0007E708
		object ICollection.SyncRoot
		{
			get
			{
				return this.InternalList.SyncRoot;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001B3B RID: 6971 RVA: 0x00080515 File Offset: 0x0007E715
		bool IList.IsFixedSize
		{
			get
			{
				return this.InternalList.IsFixedSize;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001B3C RID: 6972 RVA: 0x00080522 File Offset: 0x0007E722
		bool IList.IsReadOnly
		{
			get
			{
				return this.InternalList.IsReadOnly;
			}
		}

		/// <summary>Occurs when the collection has changed.</summary>
		// Token: 0x14000053 RID: 83
		// (add) Token: 0x06001B3D RID: 6973 RVA: 0x0008052F File Offset: 0x0007E72F
		// (remove) Token: 0x06001B3E RID: 6974 RVA: 0x00080538 File Offset: 0x0007E738
		event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
		{
			add
			{
				this.CollectionChanged += value;
			}
			remove
			{
				this.CollectionChanged -= value;
			}
		}

		/// <summary>Occurs when the collection changes, either by adding or removing an item. </summary>
		// Token: 0x14000054 RID: 84
		// (add) Token: 0x06001B3F RID: 6975 RVA: 0x00080544 File Offset: 0x0007E744
		// (remove) Token: 0x06001B40 RID: 6976 RVA: 0x0008057C File Offset: 0x0007E77C
		protected event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Collections.Specialized.CollectionChangedEventManager" />.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B41 RID: 6977 RVA: 0x000805B1 File Offset: 0x0007E7B1
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		/// <summary>Handles events from the centralized event table.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Collections.Specialized.CollectionChangedEventManager" />.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B42 RID: 6978 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x06001B43 RID: 6979 RVA: 0x000805BC File Offset: 0x0007E7BC
		// (remove) Token: 0x06001B44 RID: 6980 RVA: 0x000805F4 File Offset: 0x0007E7F4
		internal event NotifyCollectionChangedEventHandler ContainedCollectionChanged;

		// Token: 0x06001B45 RID: 6981 RVA: 0x00080629 File Offset: 0x0007E829
		private void OnContainedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.ContainedCollectionChanged != null)
			{
				this.ContainedCollectionChanged(sender, e);
			}
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x00080640 File Offset: 0x0007E840
		private void Initialize(ArrayList internalList)
		{
			this._internalList = internalList;
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001B47 RID: 6983 RVA: 0x00080649 File Offset: 0x0007E849
		private ArrayList InternalList
		{
			get
			{
				return this._internalList;
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x00080651 File Offset: 0x0007E851
		private void AddCollectionContainer(CollectionContainer cc)
		{
			if (this.InternalList.Contains(cc))
			{
				throw new ArgumentException(SR.Get("CollectionContainerMustBeUniqueForComposite"), "cc");
			}
			CollectionChangedEventManager.AddHandler(cc, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnContainedCollectionChanged));
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00080688 File Offset: 0x0007E888
		private void RemoveCollectionContainer(CollectionContainer cc)
		{
			CollectionChangedEventManager.RemoveHandler(cc, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnContainedCollectionChanged));
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0008069C File Offset: 0x0007E89C
		private void OnCollectionChanged(NotifyCollectionChangedAction action)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
			}
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x000806B8 File Offset: 0x0007E8B8
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, item, index));
			}
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x000806D6 File Offset: 0x0007E8D6
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object oldItem, object newItem, int index)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
			}
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x000806F8 File Offset: 0x0007E8F8
		internal void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			foreach (object obj in this.InternalList)
			{
				CollectionContainer collectionContainer = obj as CollectionContainer;
				if (collectionContainer != null)
				{
					collectionContainer.GetCollectionChangedSources(level + 1, format, sources);
				}
			}
		}

		// Token: 0x040013A0 RID: 5024
		private ArrayList _internalList;
	}
}
