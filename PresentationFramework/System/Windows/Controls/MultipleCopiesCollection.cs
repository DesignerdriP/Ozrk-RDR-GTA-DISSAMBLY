using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x02000505 RID: 1285
	internal class MultipleCopiesCollection : IList, ICollection, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		// Token: 0x06005262 RID: 21090 RVA: 0x00170512 File Offset: 0x0016E712
		internal MultipleCopiesCollection(object item, int count)
		{
			this.CopiedItem = item;
			this._count = count;
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x00170528 File Offset: 0x0016E728
		internal void MirrorCollectionChange(NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.Insert(e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Remove:
				this.RemoveAt(e.OldStartingIndex);
				return;
			case NotifyCollectionChangedAction.Replace:
				this.OnReplace(this.CopiedItem, this.CopiedItem, e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Move:
				this.Move(e.OldStartingIndex, e.NewStartingIndex);
				return;
			case NotifyCollectionChangedAction.Reset:
				this.Reset();
				return;
			default:
				return;
			}
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x001705A4 File Offset: 0x0016E7A4
		internal void SyncToCount(int newCount)
		{
			int repeatCount = this.RepeatCount;
			if (newCount != repeatCount)
			{
				if (newCount > repeatCount)
				{
					this.InsertRange(repeatCount, newCount - repeatCount);
					return;
				}
				int num = repeatCount - newCount;
				this.RemoveRange(repeatCount - num, num);
			}
		}

		// Token: 0x170013F9 RID: 5113
		// (get) Token: 0x06005265 RID: 21093 RVA: 0x001705D9 File Offset: 0x0016E7D9
		// (set) Token: 0x06005266 RID: 21094 RVA: 0x001705E4 File Offset: 0x0016E7E4
		internal object CopiedItem
		{
			get
			{
				return this._item;
			}
			set
			{
				if (value == CollectionView.NewItemPlaceholder)
				{
					value = DataGrid.NewItemPlaceholder;
				}
				if (this._item != value)
				{
					object item = this._item;
					this._item = value;
					this.OnPropertyChanged("Item[]");
					for (int i = 0; i < this._count; i++)
					{
						this.OnReplace(item, this._item, i);
					}
				}
			}
		}

		// Token: 0x170013FA RID: 5114
		// (get) Token: 0x06005267 RID: 21095 RVA: 0x00170641 File Offset: 0x0016E841
		// (set) Token: 0x06005268 RID: 21096 RVA: 0x00170649 File Offset: 0x0016E849
		private int RepeatCount
		{
			get
			{
				return this._count;
			}
			set
			{
				if (this._count != value)
				{
					this._count = value;
					this.OnPropertyChanged("Count");
					this.OnPropertyChanged("Item[]");
				}
			}
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x00170674 File Offset: 0x0016E874
		private void Insert(int index)
		{
			int repeatCount = this.RepeatCount;
			this.RepeatCount = repeatCount + 1;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, this.CopiedItem, index);
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x001706A0 File Offset: 0x0016E8A0
		private void InsertRange(int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Insert(index);
				index++;
			}
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x001706C5 File Offset: 0x0016E8C5
		private void Move(int oldIndex, int newIndex)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, this.CopiedItem, newIndex, oldIndex));
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x001706DC File Offset: 0x0016E8DC
		private void RemoveAt(int index)
		{
			int repeatCount = this.RepeatCount;
			this.RepeatCount = repeatCount - 1;
			this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, this.CopiedItem, index);
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x00170708 File Offset: 0x0016E908
		private void RemoveRange(int index, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.RemoveAt(index);
			}
		}

		// Token: 0x0600526E RID: 21102 RVA: 0x00170728 File Offset: 0x0016E928
		private void OnReplace(object oldItem, object newItem, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index));
		}

		// Token: 0x0600526F RID: 21103 RVA: 0x00170739 File Offset: 0x0016E939
		private void Reset()
		{
			this.RepeatCount = 0;
			this.OnCollectionReset();
		}

		// Token: 0x06005270 RID: 21104 RVA: 0x00170748 File Offset: 0x0016E948
		public int Add(object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06005271 RID: 21105 RVA: 0x00170748 File Offset: 0x0016E948
		public void Clear()
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06005272 RID: 21106 RVA: 0x00170759 File Offset: 0x0016E959
		public bool Contains(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			return this._item == value;
		}

		// Token: 0x06005273 RID: 21107 RVA: 0x00170772 File Offset: 0x0016E972
		public int IndexOf(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (this._item != value)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x06005274 RID: 21108 RVA: 0x00170748 File Offset: 0x0016E948
		public void Insert(int index, object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x170013FB RID: 5115
		// (get) Token: 0x06005275 RID: 21109 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170013FC RID: 5116
		// (get) Token: 0x06005276 RID: 21110 RVA: 0x00016748 File Offset: 0x00014948
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005277 RID: 21111 RVA: 0x00170748 File Offset: 0x0016E948
		public void Remove(object value)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x06005278 RID: 21112 RVA: 0x00170748 File Offset: 0x0016E948
		void IList.RemoveAt(int index)
		{
			throw new NotSupportedException(SR.Get("DataGrid_ReadonlyCellsItemsSource"));
		}

		// Token: 0x170013FD RID: 5117
		public object this[int index]
		{
			get
			{
				if (index >= 0 && index < this.RepeatCount)
				{
					return this._item;
				}
				throw new ArgumentOutOfRangeException("index");
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600527B RID: 21115 RVA: 0x00041C10 File Offset: 0x0003FE10
		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170013FE RID: 5118
		// (get) Token: 0x0600527C RID: 21116 RVA: 0x001707B5 File Offset: 0x0016E9B5
		public int Count
		{
			get
			{
				return this.RepeatCount;
			}
		}

		// Token: 0x170013FF RID: 5119
		// (get) Token: 0x0600527D RID: 21117 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001400 RID: 5120
		// (get) Token: 0x0600527E RID: 21118 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x001707BD File Offset: 0x0016E9BD
		public IEnumerator GetEnumerator()
		{
			return new MultipleCopiesCollection.MultipleCopiesCollectionEnumerator(this);
		}

		// Token: 0x14000101 RID: 257
		// (add) Token: 0x06005280 RID: 21120 RVA: 0x001707C8 File Offset: 0x0016E9C8
		// (remove) Token: 0x06005281 RID: 21121 RVA: 0x00170800 File Offset: 0x0016EA00
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x06005282 RID: 21122 RVA: 0x00170835 File Offset: 0x0016EA35
		private void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x00170845 File Offset: 0x0016EA45
		private void OnCollectionReset()
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x00170853 File Offset: 0x0016EA53
		private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, e);
			}
		}

		// Token: 0x14000102 RID: 258
		// (add) Token: 0x06005285 RID: 21125 RVA: 0x0017086C File Offset: 0x0016EA6C
		// (remove) Token: 0x06005286 RID: 21126 RVA: 0x001708A4 File Offset: 0x0016EAA4
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06005287 RID: 21127 RVA: 0x001708D9 File Offset: 0x0016EAD9
		private void OnPropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x001708E7 File Offset: 0x0016EAE7
		private void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, e);
			}
		}

		// Token: 0x04002CAF RID: 11439
		private object _item;

		// Token: 0x04002CB0 RID: 11440
		private int _count;

		// Token: 0x04002CB1 RID: 11441
		private const string CountName = "Count";

		// Token: 0x04002CB2 RID: 11442
		private const string IndexerName = "Item[]";

		// Token: 0x020009AB RID: 2475
		private class MultipleCopiesCollectionEnumerator : IEnumerator
		{
			// Token: 0x0600883B RID: 34875 RVA: 0x00251C64 File Offset: 0x0024FE64
			public MultipleCopiesCollectionEnumerator(MultipleCopiesCollection collection)
			{
				this._collection = collection;
				this._item = this._collection.CopiedItem;
				this._count = this._collection.RepeatCount;
				this._current = -1;
			}

			// Token: 0x17001EBE RID: 7870
			// (get) Token: 0x0600883C RID: 34876 RVA: 0x00251C9C File Offset: 0x0024FE9C
			object IEnumerator.Current
			{
				get
				{
					if (this._current < 0)
					{
						return null;
					}
					if (this._current < this._count)
					{
						return this._item;
					}
					throw new InvalidOperationException();
				}
			}

			// Token: 0x0600883D RID: 34877 RVA: 0x00251CC4 File Offset: 0x0024FEC4
			bool IEnumerator.MoveNext()
			{
				if (!this.IsCollectionUnchanged)
				{
					throw new InvalidOperationException();
				}
				int num = this._current + 1;
				if (num < this._count)
				{
					this._current = num;
					return true;
				}
				return false;
			}

			// Token: 0x0600883E RID: 34878 RVA: 0x00251CFB File Offset: 0x0024FEFB
			void IEnumerator.Reset()
			{
				if (this.IsCollectionUnchanged)
				{
					this._current = -1;
					return;
				}
				throw new InvalidOperationException();
			}

			// Token: 0x17001EBF RID: 7871
			// (get) Token: 0x0600883F RID: 34879 RVA: 0x00251D12 File Offset: 0x0024FF12
			private bool IsCollectionUnchanged
			{
				get
				{
					return this._collection.RepeatCount == this._count && this._collection.CopiedItem == this._item;
				}
			}

			// Token: 0x04004508 RID: 17672
			private object _item;

			// Token: 0x04004509 RID: 17673
			private int _count;

			// Token: 0x0400450A RID: 17674
			private int _current;

			// Token: 0x0400450B RID: 17675
			private MultipleCopiesCollection _collection;
		}
	}
}
