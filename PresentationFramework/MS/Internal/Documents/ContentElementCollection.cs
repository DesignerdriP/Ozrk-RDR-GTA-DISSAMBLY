using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006BA RID: 1722
	internal abstract class ContentElementCollection<TParent, TItem> : IList<TItem>, ICollection<TItem>, IEnumerable<TItem>, IEnumerable, IList, ICollection where TParent : TextElement, IAcceptInsertion where TItem : FrameworkContentElement, IIndexedChild<TParent>
	{
		// Token: 0x06006ED5 RID: 28373 RVA: 0x001FDEB2 File Offset: 0x001FC0B2
		internal ContentElementCollection(TParent owner)
		{
			this._owner = owner;
			this.Items = new TItem[this.DefaultCapacity];
		}

		// Token: 0x06006ED6 RID: 28374 RVA: 0x001FDED4 File Offset: 0x001FC0D4
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.Get("TableCollectionRankMultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("TableCollectionOutOfRangeNeedNonNegNum"));
			}
			if (array.Length - index < this.Size)
			{
				throw new ArgumentException(SR.Get("TableCollectionInvalidOffLen"));
			}
			Array.Copy(this.Items, 0, array, index, this.Size);
		}

		// Token: 0x06006ED7 RID: 28375 RVA: 0x001FDF58 File Offset: 0x001FC158
		public void CopyTo(TItem[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.Get("TableCollectionOutOfRangeNeedNonNegNum"));
			}
			if (array.Length - index < this.Size)
			{
				throw new ArgumentException(SR.Get("TableCollectionInvalidOffLen"));
			}
			Array.Copy(this.Items, 0, array, index, this.Size);
		}

		// Token: 0x06006ED8 RID: 28376 RVA: 0x001FDFBD File Offset: 0x001FC1BD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06006ED9 RID: 28377 RVA: 0x001FDFC5 File Offset: 0x001FC1C5
		internal IEnumerator GetEnumerator()
		{
			return new ContentElementCollection<TParent, TItem>.ContentElementCollectionEnumeratorSimple(this);
		}

		// Token: 0x06006EDA RID: 28378 RVA: 0x001FDFC5 File Offset: 0x001FC1C5
		IEnumerator<TItem> IEnumerable<!1>.GetEnumerator()
		{
			return new ContentElementCollection<TParent, TItem>.ContentElementCollectionEnumeratorSimple(this);
		}

		// Token: 0x06006EDB RID: 28379
		public abstract void Add(TItem item);

		// Token: 0x06006EDC RID: 28380
		public abstract void Clear();

		// Token: 0x06006EDD RID: 28381 RVA: 0x001FDFCD File Offset: 0x001FC1CD
		public bool Contains(TItem item)
		{
			return this.BelongsToOwner(item);
		}

		// Token: 0x06006EDE RID: 28382 RVA: 0x001FDFDB File Offset: 0x001FC1DB
		public int IndexOf(TItem item)
		{
			if (this.BelongsToOwner(item))
			{
				return item.Index;
			}
			return -1;
		}

		// Token: 0x06006EDF RID: 28383
		public abstract void Insert(int index, TItem item);

		// Token: 0x06006EE0 RID: 28384
		public abstract bool Remove(TItem item);

		// Token: 0x06006EE1 RID: 28385
		public abstract void RemoveAt(int index);

		// Token: 0x06006EE2 RID: 28386
		public abstract void RemoveRange(int index, int count);

		// Token: 0x06006EE3 RID: 28387 RVA: 0x001FDFF3 File Offset: 0x001FC1F3
		public void TrimToSize()
		{
			this.PrivateCapacity = this.Size;
		}

		// Token: 0x06006EE4 RID: 28388 RVA: 0x001FE004 File Offset: 0x001FC204
		int IList.Add(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TItem titem = value as TItem;
			this.Add(titem);
			return ((IList)this).IndexOf(titem);
		}

		// Token: 0x06006EE5 RID: 28389 RVA: 0x001FE03E File Offset: 0x001FC23E
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06006EE6 RID: 28390 RVA: 0x001FE048 File Offset: 0x001FC248
		bool IList.Contains(object value)
		{
			TItem titem = value as TItem;
			return titem != null && this.Contains(titem);
		}

		// Token: 0x06006EE7 RID: 28391 RVA: 0x001FE074 File Offset: 0x001FC274
		int IList.IndexOf(object value)
		{
			TItem titem = value as TItem;
			if (titem == null)
			{
				return -1;
			}
			return this.IndexOf(titem);
		}

		// Token: 0x06006EE8 RID: 28392 RVA: 0x001FE0A0 File Offset: 0x001FC2A0
		void IList.Insert(int index, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TItem titem = value as TItem;
			if (titem == null)
			{
				throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
				{
					typeof(TItem).Name
				}), "value");
			}
			this.Insert(index, titem);
		}

		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x06006EE9 RID: 28393 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x06006EEA RID: 28394 RVA: 0x001FE104 File Offset: 0x001FC304
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsReadOnly;
			}
		}

		// Token: 0x06006EEB RID: 28395 RVA: 0x001FE10C File Offset: 0x001FC30C
		void IList.Remove(object value)
		{
			TItem titem = value as TItem;
			if (titem == null)
			{
				return;
			}
			this.Remove(titem);
		}

		// Token: 0x06006EEC RID: 28396 RVA: 0x001FE136 File Offset: 0x001FC336
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17001A51 RID: 6737
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				TItem titem = value as TItem;
				if (titem == null)
				{
					throw new ArgumentException(SR.Get("TableCollectionElementTypeExpected", new object[]
					{
						typeof(TItem).Name
					}), "value");
				}
				this[index] = titem;
			}
		}

		// Token: 0x17001A52 RID: 6738
		public abstract TItem this[int index]
		{
			get;
			set;
		}

		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x06006EF1 RID: 28401 RVA: 0x001FE1B4 File Offset: 0x001FC3B4
		public int Count
		{
			get
			{
				return this.Size;
			}
		}

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x06006EF2 RID: 28402 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x06006EF3 RID: 28403 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A56 RID: 6742
		// (get) Token: 0x06006EF4 RID: 28404 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001A57 RID: 6743
		// (get) Token: 0x06006EF5 RID: 28405 RVA: 0x001FE1BC File Offset: 0x001FC3BC
		// (set) Token: 0x06006EF6 RID: 28406 RVA: 0x001FE1C4 File Offset: 0x001FC3C4
		public int Capacity
		{
			get
			{
				return this.PrivateCapacity;
			}
			set
			{
				this.PrivateCapacity = value;
			}
		}

		// Token: 0x17001A58 RID: 6744
		// (get) Token: 0x06006EF7 RID: 28407 RVA: 0x001FE1CD File Offset: 0x001FC3CD
		public TParent Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x17001A59 RID: 6745
		// (get) Token: 0x06006EF8 RID: 28408 RVA: 0x001FE1D5 File Offset: 0x001FC3D5
		// (set) Token: 0x06006EF9 RID: 28409 RVA: 0x001FE1DD File Offset: 0x001FC3DD
		private protected TItem[] Items
		{
			protected get
			{
				return this._items;
			}
			private set
			{
				this._items = value;
			}
		}

		// Token: 0x17001A5A RID: 6746
		// (get) Token: 0x06006EFA RID: 28410 RVA: 0x001FE1E6 File Offset: 0x001FC3E6
		// (set) Token: 0x06006EFB RID: 28411 RVA: 0x001FE1EE File Offset: 0x001FC3EE
		protected int Size
		{
			get
			{
				return this._size;
			}
			set
			{
				this._size = value;
			}
		}

		// Token: 0x17001A5B RID: 6747
		// (get) Token: 0x06006EFC RID: 28412 RVA: 0x001FE1F7 File Offset: 0x001FC3F7
		// (set) Token: 0x06006EFD RID: 28413 RVA: 0x001FE1FF File Offset: 0x001FC3FF
		protected int Version
		{
			get
			{
				return this._version;
			}
			set
			{
				this._version = value;
			}
		}

		// Token: 0x17001A5C RID: 6748
		// (get) Token: 0x06006EFE RID: 28414 RVA: 0x000956EC File Offset: 0x000938EC
		protected int DefaultCapacity
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06006EFF RID: 28415 RVA: 0x001FE208 File Offset: 0x001FC408
		internal void EnsureCapacity(int min)
		{
			if (this.PrivateCapacity < min)
			{
				this.PrivateCapacity = Math.Max(min, this.PrivateCapacity * 2);
			}
		}

		// Token: 0x06006F00 RID: 28416
		internal abstract void PrivateConnectChild(int index, TItem item);

		// Token: 0x06006F01 RID: 28417
		internal abstract void PrivateDisconnectChild(TItem item);

		// Token: 0x06006F02 RID: 28418 RVA: 0x001FE228 File Offset: 0x001FC428
		internal void PrivateRemove(TItem item)
		{
			int index = item.Index;
			this.PrivateDisconnectChild(item);
			int size = this.Size - 1;
			this.Size = size;
			for (int i = index; i < this.Size; i++)
			{
				this.Items[i] = this.Items[i + 1];
				this.Items[i].Index = i;
			}
			this.Items[this.Size] = default(TItem);
		}

		// Token: 0x06006F03 RID: 28419 RVA: 0x001FE2B4 File Offset: 0x001FC4B4
		internal bool BelongsToOwner(TItem item)
		{
			if (item == null)
			{
				return false;
			}
			DependencyObject parent = item.Parent;
			if (parent is ContentElementCollection<TParent, TItem>.DummyProxy)
			{
				parent = LogicalTreeHelper.GetParent(parent);
			}
			return parent == this.Owner;
		}

		// Token: 0x17001A5D RID: 6749
		// (get) Token: 0x06006F04 RID: 28420 RVA: 0x001FE2F4 File Offset: 0x001FC4F4
		// (set) Token: 0x06006F05 RID: 28421 RVA: 0x001FE300 File Offset: 0x001FC500
		internal int PrivateCapacity
		{
			get
			{
				return this.Items.Length;
			}
			set
			{
				if (value != this.Items.Length)
				{
					if (value < this.Size)
					{
						throw new ArgumentOutOfRangeException(SR.Get("TableCollectionNotEnoughCapacity"));
					}
					if (value > 0)
					{
						TItem[] array = new TItem[value];
						if (this.Size > 0)
						{
							Array.Copy(this.Items, 0, array, 0, this.Size);
						}
						this.Items = array;
						return;
					}
					this.Items = new TItem[this.DefaultCapacity];
				}
			}
		}

		// Token: 0x0400368D RID: 13965
		private readonly TParent _owner;

		// Token: 0x0400368E RID: 13966
		private TItem[] _items;

		// Token: 0x0400368F RID: 13967
		private int _size;

		// Token: 0x04003690 RID: 13968
		private int _version;

		// Token: 0x04003691 RID: 13969
		protected const int c_defaultCapacity = 8;

		// Token: 0x02000B2B RID: 2859
		protected class ContentElementCollectionEnumeratorSimple : IEnumerator<!1>, IDisposable, IEnumerator
		{
			// Token: 0x06008D49 RID: 36169 RVA: 0x0025922C File Offset: 0x0025742C
			internal ContentElementCollectionEnumeratorSimple(ContentElementCollection<TParent, TItem> collection)
			{
				this._collection = collection;
				this._index = -1;
				this.Version = this._collection.Version;
				this._currentElement = collection;
			}

			// Token: 0x06008D4A RID: 36170 RVA: 0x0025925C File Offset: 0x0025745C
			public bool MoveNext()
			{
				if (this.Version != this._collection.Version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				if (this._index < this._collection.Size - 1)
				{
					this._index++;
					this._currentElement = this._collection[this._index];
					return true;
				}
				this._currentElement = this._collection;
				this._index = this._collection.Size;
				return false;
			}

			// Token: 0x17001F70 RID: 8048
			// (get) Token: 0x06008D4B RID: 36171 RVA: 0x002592EC File Offset: 0x002574EC
			public TItem Current
			{
				get
				{
					if (this._currentElement != this._collection)
					{
						return (TItem)((object)this._currentElement);
					}
					if (this._index == -1)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
				}
			}

			// Token: 0x17001F71 RID: 8049
			// (get) Token: 0x06008D4C RID: 36172 RVA: 0x0025933B File Offset: 0x0025753B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06008D4D RID: 36173 RVA: 0x00259348 File Offset: 0x00257548
			public void Reset()
			{
				if (this.Version != this._collection.Version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				this._currentElement = this._collection;
				this._index = -1;
			}

			// Token: 0x06008D4E RID: 36174 RVA: 0x000B1359 File Offset: 0x000AF559
			public void Dispose()
			{
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004A80 RID: 19072
			private ContentElementCollection<TParent, TItem> _collection;

			// Token: 0x04004A81 RID: 19073
			private int _index;

			// Token: 0x04004A82 RID: 19074
			protected int Version;

			// Token: 0x04004A83 RID: 19075
			private object _currentElement;
		}

		// Token: 0x02000B2C RID: 2860
		protected class DummyProxy : DependencyObject
		{
		}
	}
}
