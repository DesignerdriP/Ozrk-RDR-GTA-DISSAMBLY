using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x020004A4 RID: 1188
	internal class DataGridColumnHeaderCollection : IEnumerable, INotifyCollectionChanged, IDisposable
	{
		// Token: 0x06004882 RID: 18562 RVA: 0x00149B74 File Offset: 0x00147D74
		public DataGridColumnHeaderCollection(ObservableCollection<DataGridColumn> columns)
		{
			this._columns = columns;
			if (this._columns != null)
			{
				this._columns.CollectionChanged += this.OnColumnsChanged;
			}
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x00149BA2 File Offset: 0x00147DA2
		public DataGridColumn ColumnFromIndex(int index)
		{
			if (index >= 0 && index < this._columns.Count)
			{
				return this._columns[index];
			}
			return null;
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x00149BC4 File Offset: 0x00147DC4
		internal void NotifyHeaderPropertyChanged(DataGridColumn column, DependencyPropertyChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewValue, e.OldValue, this._columns.IndexOf(column));
			this.FireCollectionChanged(args);
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x00149BF9 File Offset: 0x00147DF9
		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (this._columns != null)
			{
				this._columns.CollectionChanged -= this.OnColumnsChanged;
			}
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x00149C20 File Offset: 0x00147E20
		public IEnumerator GetEnumerator()
		{
			return new DataGridColumnHeaderCollection.ColumnHeaderCollectionEnumerator(this._columns);
		}

		// Token: 0x140000C8 RID: 200
		// (add) Token: 0x06004887 RID: 18567 RVA: 0x00149C30 File Offset: 0x00147E30
		// (remove) Token: 0x06004888 RID: 18568 RVA: 0x00149C68 File Offset: 0x00147E68
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x06004889 RID: 18569 RVA: 0x00149CA0 File Offset: 0x00147EA0
		private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventArgs args;
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.NewItems), e.NewStartingIndex);
				break;
			case NotifyCollectionChangedAction.Remove:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Replace:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.NewItems), DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.OldStartingIndex);
				break;
			case NotifyCollectionChangedAction.Move:
				args = new NotifyCollectionChangedEventArgs(e.Action, DataGridColumnHeaderCollection.HeadersFromColumns(e.OldItems), e.NewStartingIndex, e.OldStartingIndex);
				break;
			default:
				args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
				break;
			}
			this.FireCollectionChanged(args);
		}

		// Token: 0x0600488A RID: 18570 RVA: 0x00149D6A File Offset: 0x00147F6A
		private void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x0600488B RID: 18571 RVA: 0x00149D84 File Offset: 0x00147F84
		private static object[] HeadersFromColumns(IList columns)
		{
			object[] array = new object[columns.Count];
			for (int i = 0; i < columns.Count; i++)
			{
				DataGridColumn dataGridColumn = columns[i] as DataGridColumn;
				if (dataGridColumn != null)
				{
					array[i] = dataGridColumn.Header;
				}
				else
				{
					array[i] = null;
				}
			}
			return array;
		}

		// Token: 0x0400299D RID: 10653
		private ObservableCollection<DataGridColumn> _columns;

		// Token: 0x02000967 RID: 2407
		private class ColumnHeaderCollectionEnumerator : IEnumerator, IDisposable
		{
			// Token: 0x06008752 RID: 34642 RVA: 0x0024F7EF File Offset: 0x0024D9EF
			public ColumnHeaderCollectionEnumerator(ObservableCollection<DataGridColumn> columns)
			{
				if (columns != null)
				{
					this._columns = columns;
					this._columns.CollectionChanged += this.OnColumnsChanged;
				}
				this._current = -1;
			}

			// Token: 0x17001E95 RID: 7829
			// (get) Token: 0x06008753 RID: 34643 RVA: 0x0024F820 File Offset: 0x0024DA20
			public object Current
			{
				get
				{
					if (!this.IsValid)
					{
						throw new InvalidOperationException();
					}
					DataGridColumn dataGridColumn = this._columns[this._current];
					if (dataGridColumn != null)
					{
						return dataGridColumn.Header;
					}
					return null;
				}
			}

			// Token: 0x06008754 RID: 34644 RVA: 0x0024F858 File Offset: 0x0024DA58
			public bool MoveNext()
			{
				if (this.HasChanged)
				{
					throw new InvalidOperationException();
				}
				if (this._columns != null && this._current < this._columns.Count - 1)
				{
					this._current++;
					return true;
				}
				return false;
			}

			// Token: 0x06008755 RID: 34645 RVA: 0x0024F896 File Offset: 0x0024DA96
			public void Reset()
			{
				if (this.HasChanged)
				{
					throw new InvalidOperationException();
				}
				this._current = -1;
			}

			// Token: 0x06008756 RID: 34646 RVA: 0x0024F8AD File Offset: 0x0024DAAD
			public void Dispose()
			{
				GC.SuppressFinalize(this);
				if (this._columns != null)
				{
					this._columns.CollectionChanged -= this.OnColumnsChanged;
				}
			}

			// Token: 0x17001E96 RID: 7830
			// (get) Token: 0x06008757 RID: 34647 RVA: 0x0024F8D4 File Offset: 0x0024DAD4
			private bool HasChanged
			{
				get
				{
					return this._columnsChanged;
				}
			}

			// Token: 0x17001E97 RID: 7831
			// (get) Token: 0x06008758 RID: 34648 RVA: 0x0024F8DC File Offset: 0x0024DADC
			private bool IsValid
			{
				get
				{
					return this._columns != null && this._current >= 0 && this._current < this._columns.Count && !this.HasChanged;
				}
			}

			// Token: 0x06008759 RID: 34649 RVA: 0x0024F90D File Offset: 0x0024DB0D
			private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
			{
				this._columnsChanged = true;
			}

			// Token: 0x0400442F RID: 17455
			private int _current;

			// Token: 0x04004430 RID: 17456
			private bool _columnsChanged;

			// Token: 0x04004431 RID: 17457
			private ObservableCollection<DataGridColumn> _columns;
		}
	}
}
