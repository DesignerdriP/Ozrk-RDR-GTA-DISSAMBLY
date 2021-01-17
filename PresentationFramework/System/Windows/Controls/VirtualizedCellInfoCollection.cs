using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x0200055C RID: 1372
	internal class VirtualizedCellInfoCollection : IList<DataGridCellInfo>, ICollection<DataGridCellInfo>, IEnumerable<DataGridCellInfo>, IEnumerable
	{
		// Token: 0x060059D2 RID: 22994 RVA: 0x0018C012 File Offset: 0x0018A212
		internal VirtualizedCellInfoCollection(DataGrid owner)
		{
			this._owner = owner;
			this._regions = new List<VirtualizedCellInfoCollection.CellRegion>();
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x0018C02C File Offset: 0x0018A22C
		private VirtualizedCellInfoCollection(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions)
		{
			this._owner = owner;
			this._regions = ((regions != null) ? new List<VirtualizedCellInfoCollection.CellRegion>(regions) : new List<VirtualizedCellInfoCollection.CellRegion>());
			this.IsReadOnly = true;
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x0018C058 File Offset: 0x0018A258
		internal static VirtualizedCellInfoCollection MakeEmptyCollection(DataGrid owner)
		{
			return new VirtualizedCellInfoCollection(owner, null);
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x0018C064 File Offset: 0x0018A264
		public void Add(DataGridCellInfo cell)
		{
			this._owner.Dispatcher.VerifyAccess();
			this.ValidateIsReadOnly();
			if (!this.IsValidPublicCell(cell))
			{
				throw new ArgumentException(SR.Get("SelectedCellsCollection_InvalidItem"), "cell");
			}
			if (this.Contains(cell))
			{
				throw new ArgumentException(SR.Get("SelectedCellsCollection_DuplicateItem"), "cell");
			}
			this.AddValidatedCell(cell);
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x0018C0CC File Offset: 0x0018A2CC
		internal void AddValidatedCell(DataGridCellInfo cell)
		{
			int rowIndex;
			int columnIndex;
			this.ConvertCellInfoToIndexes(cell, out rowIndex, out columnIndex);
			this.AddRegion(rowIndex, columnIndex, 1, 1);
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x0018C0F0 File Offset: 0x0018A2F0
		public void Clear()
		{
			this._owner.Dispatcher.VerifyAccess();
			this.ValidateIsReadOnly();
			if (!this.IsEmpty)
			{
				VirtualizedCellInfoCollection oldItems = new VirtualizedCellInfoCollection(this._owner, this._regions);
				this._regions.Clear();
				this.OnRemove(oldItems);
			}
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x0018C140 File Offset: 0x0018A340
		public bool Contains(DataGridCellInfo cell)
		{
			if (!this.IsValidPublicCell(cell))
			{
				throw new ArgumentException(SR.Get("SelectedCellsCollection_InvalidItem"), "cell");
			}
			if (this.IsEmpty)
			{
				return false;
			}
			int rowIndex;
			int columnIndex;
			this.ConvertCellInfoToIndexes(cell, out rowIndex, out columnIndex);
			return this.Contains(rowIndex, columnIndex);
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x0018C188 File Offset: 0x0018A388
		internal bool Contains(DataGridCell cell)
		{
			if (!this.IsEmpty)
			{
				object rowDataItem = cell.RowDataItem;
				int displayIndex = cell.Column.DisplayIndex;
				ItemCollection items = this._owner.Items;
				int count = items.Count;
				int count2 = this._regions.Count;
				for (int i = 0; i < count2; i++)
				{
					VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
					if (cellRegion.Left <= displayIndex && displayIndex <= cellRegion.Right)
					{
						int bottom = cellRegion.Bottom;
						for (int j = cellRegion.Top; j <= bottom; j++)
						{
							if (j < count && items[j] == rowDataItem)
							{
								return true;
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x0018C23C File Offset: 0x0018A43C
		internal bool Contains(int rowIndex, int columnIndex)
		{
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._regions[i].Contains(columnIndex, rowIndex))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x0018C27C File Offset: 0x0018A47C
		public void CopyTo(DataGridCellInfo[] array, int arrayIndex)
		{
			List<DataGridCellInfo> list = new List<DataGridCellInfo>();
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				this.AddRegionToList(this._regions[i], list);
			}
			list.CopyTo(array, arrayIndex);
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x0018C2C2 File Offset: 0x0018A4C2
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new VirtualizedCellInfoCollection.VirtualizedCellInfoCollectionEnumerator(this._owner, this._regions, this);
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x0018C2C2 File Offset: 0x0018A4C2
		public IEnumerator<DataGridCellInfo> GetEnumerator()
		{
			return new VirtualizedCellInfoCollection.VirtualizedCellInfoCollectionEnumerator(this._owner, this._regions, this);
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x0018C2D8 File Offset: 0x0018A4D8
		public int IndexOf(DataGridCellInfo cell)
		{
			int num;
			int num2;
			this.ConvertCellInfoToIndexes(cell, out num, out num2);
			int count = this._regions.Count;
			int num3 = 0;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
				if (cellRegion.Contains(num2, num))
				{
					return num3 + ((num - cellRegion.Top) * cellRegion.Width + num2 - cellRegion.Left);
				}
				num3 += cellRegion.Size;
			}
			return -1;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x0018C351 File Offset: 0x0018A551
		public void Insert(int index, DataGridCellInfo cell)
		{
			throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_DoesNotSupportIndexChanges"));
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x0018C364 File Offset: 0x0018A564
		public bool Remove(DataGridCellInfo cell)
		{
			this._owner.Dispatcher.VerifyAccess();
			this.ValidateIsReadOnly();
			if (!this.IsEmpty)
			{
				int rowIndex;
				int columnIndex;
				this.ConvertCellInfoToIndexes(cell, out rowIndex, out columnIndex);
				if (this.Contains(rowIndex, columnIndex))
				{
					this.RemoveRegion(rowIndex, columnIndex, 1, 1);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x0018C351 File Offset: 0x0018A551
		public void RemoveAt(int index)
		{
			throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_DoesNotSupportIndexChanges"));
		}

		// Token: 0x170015DD RID: 5597
		public DataGridCellInfo this[int index]
		{
			get
			{
				if (index >= 0 && index < this.Count)
				{
					return this.GetCellInfoFromIndex(this._owner, this._regions, index);
				}
				throw new ArgumentOutOfRangeException("index");
			}
			set
			{
				throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_DoesNotSupportIndexChanges"));
			}
		}

		// Token: 0x170015DE RID: 5598
		// (get) Token: 0x060059E4 RID: 23012 RVA: 0x0018C3E0 File Offset: 0x0018A5E0
		public int Count
		{
			get
			{
				int num = 0;
				int count = this._regions.Count;
				for (int i = 0; i < count; i++)
				{
					num += this._regions[i].Size;
				}
				return num;
			}
		}

		// Token: 0x170015DF RID: 5599
		// (get) Token: 0x060059E5 RID: 23013 RVA: 0x0018C41F File Offset: 0x0018A61F
		// (set) Token: 0x060059E6 RID: 23014 RVA: 0x0018C427 File Offset: 0x0018A627
		public bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
			private set
			{
				this._isReadOnly = value;
			}
		}

		// Token: 0x060059E7 RID: 23015 RVA: 0x0018C430 File Offset: 0x0018A630
		private void OnAdd(VirtualizedCellInfoCollection newItems)
		{
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, null, newItems);
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x0018C43B File Offset: 0x0018A63B
		private void OnRemove(VirtualizedCellInfoCollection oldItems)
		{
			this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, oldItems, null);
		}

		// Token: 0x060059E9 RID: 23017 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, VirtualizedCellInfoCollection oldItems, VirtualizedCellInfoCollection newItems)
		{
		}

		// Token: 0x060059EA RID: 23018 RVA: 0x0018C446 File Offset: 0x0018A646
		private bool IsValidCell(DataGridCellInfo cell)
		{
			return cell.IsValidForDataGrid(this._owner);
		}

		// Token: 0x060059EB RID: 23019 RVA: 0x0018C455 File Offset: 0x0018A655
		private bool IsValidPublicCell(DataGridCellInfo cell)
		{
			return cell.IsValidForDataGrid(this._owner) && cell.IsValid;
		}

		// Token: 0x170015E0 RID: 5600
		// (get) Token: 0x060059EC RID: 23020 RVA: 0x0018C470 File Offset: 0x0018A670
		protected bool IsEmpty
		{
			get
			{
				int count = this._regions.Count;
				for (int i = 0; i < count; i++)
				{
					if (!this._regions[i].IsEmpty)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060059ED RID: 23021 RVA: 0x0018C4B0 File Offset: 0x0018A6B0
		protected void GetBoundingRegion(out int left, out int top, out int right, out int bottom)
		{
			left = int.MaxValue;
			top = int.MaxValue;
			right = 0;
			bottom = 0;
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
				if (cellRegion.Left < left)
				{
					left = cellRegion.Left;
				}
				if (cellRegion.Top < top)
				{
					top = cellRegion.Top;
				}
				if (cellRegion.Right > right)
				{
					right = cellRegion.Right;
				}
				if (cellRegion.Bottom > bottom)
				{
					bottom = cellRegion.Bottom;
				}
			}
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x0018C549 File Offset: 0x0018A749
		internal void AddRegion(int rowIndex, int columnIndex, int rowCount, int columnCount)
		{
			this.AddRegion(rowIndex, columnIndex, rowCount, columnCount, true);
		}

		// Token: 0x060059EF RID: 23023 RVA: 0x0018C558 File Offset: 0x0018A758
		private void AddRegion(int rowIndex, int columnIndex, int rowCount, int columnCount, bool notify)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = new List<VirtualizedCellInfoCollection.CellRegion>();
			list.Add(new VirtualizedCellInfoCollection.CellRegion(columnIndex, rowIndex, columnCount, rowCount));
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion region = this._regions[i];
				for (int j = 0; j < list.Count; j++)
				{
					List<VirtualizedCellInfoCollection.CellRegion> list2;
					if (list[j].Remainder(region, out list2))
					{
						list.RemoveAt(j);
						j--;
						if (list2 != null)
						{
							list.AddRange(list2);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				VirtualizedCellInfoCollection newItems = new VirtualizedCellInfoCollection(this._owner, list);
				for (int k = 0; k < count; k++)
				{
					for (int l = 0; l < list.Count; l++)
					{
						VirtualizedCellInfoCollection.CellRegion value = this._regions[k];
						if (value.Union(list[l]))
						{
							this._regions[k] = value;
							list.RemoveAt(l);
							l--;
						}
					}
				}
				int count2 = list.Count;
				for (int m = 0; m < count2; m++)
				{
					this._regions.Add(list[m]);
				}
				if (notify)
				{
					this.OnAdd(newItems);
				}
			}
		}

		// Token: 0x060059F0 RID: 23024 RVA: 0x0018C69C File Offset: 0x0018A89C
		internal void RemoveRegion(int rowIndex, int columnIndex, int rowCount, int columnCount)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(rowIndex, columnIndex, rowCount, columnCount, ref list);
			if (list != null && list.Count > 0)
			{
				this.OnRemove(new VirtualizedCellInfoCollection(this._owner, list));
			}
		}

		// Token: 0x060059F1 RID: 23025 RVA: 0x0018C6D8 File Offset: 0x0018A8D8
		private void RemoveRegion(int rowIndex, int columnIndex, int rowCount, int columnCount, ref List<VirtualizedCellInfoCollection.CellRegion> removeList)
		{
			if (!this.IsEmpty)
			{
				VirtualizedCellInfoCollection.CellRegion region = new VirtualizedCellInfoCollection.CellRegion(columnIndex, rowIndex, columnCount, rowCount);
				for (int i = 0; i < this._regions.Count; i++)
				{
					VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
					VirtualizedCellInfoCollection.CellRegion cellRegion2 = cellRegion.Intersection(region);
					if (!cellRegion2.IsEmpty)
					{
						if (removeList == null)
						{
							removeList = new List<VirtualizedCellInfoCollection.CellRegion>();
						}
						removeList.Add(cellRegion2);
						this._regions.RemoveAt(i);
						List<VirtualizedCellInfoCollection.CellRegion> list;
						cellRegion.Remainder(cellRegion2, out list);
						if (list != null)
						{
							this._regions.InsertRange(i, list);
							i += list.Count;
						}
						i--;
					}
				}
			}
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x0018C780 File Offset: 0x0018A980
		internal void OnItemsCollectionChanged(NotifyCollectionChangedEventArgs e, List<Tuple<int, int>> ranges)
		{
			if (!this.IsEmpty)
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					this.OnAddRow(e.NewStartingIndex);
					return;
				case NotifyCollectionChangedAction.Remove:
					this.OnRemoveRow(e.OldStartingIndex, e.OldItems[0]);
					return;
				case NotifyCollectionChangedAction.Replace:
					this.OnReplaceRow(e.OldStartingIndex, e.OldItems[0]);
					return;
				case NotifyCollectionChangedAction.Move:
					this.OnMoveRow(e.OldStartingIndex, e.NewStartingIndex);
					return;
				case NotifyCollectionChangedAction.Reset:
					this.RestoreOnlyFullRows(ranges);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060059F3 RID: 23027 RVA: 0x0018C810 File Offset: 0x0018AA10
		private void OnAddRow(int rowIndex)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count2 > 0)
			{
				this.RemoveRegion(rowIndex, 0, count - 1 - rowIndex, count2, ref list);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top + 1, cellRegion.Left, cellRegion.Height, cellRegion.Width, false);
					}
				}
			}
		}

		// Token: 0x060059F4 RID: 23028 RVA: 0x0018C8A0 File Offset: 0x0018AAA0
		private void OnRemoveRow(int rowIndex, object item)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count2 > 0)
			{
				this.RemoveRegion(rowIndex + 1, 0, count - rowIndex, count2, ref list);
				this.RemoveRegion(rowIndex, 0, 1, count2, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top - 1, cellRegion.Left, cellRegion.Height, cellRegion.Width, false);
					}
				}
				if (list2 != null)
				{
					VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list2, item);
					this.OnRemove(oldItems);
				}
			}
		}

		// Token: 0x060059F5 RID: 23029 RVA: 0x0018C95C File Offset: 0x0018AB5C
		private void OnReplaceRow(int rowIndex, object item)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(rowIndex, 0, 1, this._owner.Columns.Count, ref list);
			if (list != null)
			{
				VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list, item);
				this.OnRemove(oldItems);
			}
		}

		// Token: 0x060059F6 RID: 23030 RVA: 0x0018C9A0 File Offset: 0x0018ABA0
		private void OnMoveRow(int oldIndex, int newIndex)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count2 > 0)
			{
				this.RemoveRegion(oldIndex + 1, 0, count - oldIndex - 1, count2, ref list);
				this.RemoveRegion(oldIndex, 0, 1, count2, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top - 1, cellRegion.Left, cellRegion.Height, cellRegion.Width, false);
					}
				}
				list = null;
				this.RemoveRegion(newIndex, 0, count - newIndex, count2, ref list);
				if (list2 != null)
				{
					int count4 = list2.Count;
					for (int j = 0; j < count4; j++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion2 = list2[j];
						this.AddRegion(newIndex, cellRegion2.Left, cellRegion2.Height, cellRegion2.Width, false);
					}
				}
				if (list != null)
				{
					int count5 = list.Count;
					for (int k = 0; k < count5; k++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion3 = list[k];
						this.AddRegion(cellRegion3.Top + 1, cellRegion3.Left, cellRegion3.Height, cellRegion3.Width, false);
					}
				}
			}
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x0018CAE4 File Offset: 0x0018ACE4
		internal void OnColumnsChanged(NotifyCollectionChangedAction action, int oldDisplayIndex, DataGridColumn oldColumn, int newDisplayIndex, IList selectedRows)
		{
			if (!this.IsEmpty)
			{
				switch (action)
				{
				case NotifyCollectionChangedAction.Add:
					this.OnAddColumn(newDisplayIndex, selectedRows);
					return;
				case NotifyCollectionChangedAction.Remove:
					this.OnRemoveColumn(oldDisplayIndex, oldColumn);
					return;
				case NotifyCollectionChangedAction.Replace:
					this.OnReplaceColumn(oldDisplayIndex, oldColumn, selectedRows);
					return;
				case NotifyCollectionChangedAction.Move:
					this.OnMoveColumn(oldDisplayIndex, newDisplayIndex);
					return;
				case NotifyCollectionChangedAction.Reset:
					this._regions.Clear();
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x0018CB48 File Offset: 0x0018AD48
		private void OnAddColumn(int columnIndex, IList selectedRows)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count > 0)
			{
				this.RemoveRegion(0, columnIndex, count, count2 - 1 - columnIndex, ref list);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top, cellRegion.Left + 1, cellRegion.Height, cellRegion.Width, false);
					}
				}
				this.FillInFullRowRegions(selectedRows, columnIndex, true);
			}
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x0018CBE0 File Offset: 0x0018ADE0
		private void FillInFullRowRegions(IList rows, int columnIndex, bool notify)
		{
			int count = rows.Count;
			for (int i = 0; i < count; i++)
			{
				int num = this._owner.Items.IndexOf(rows[i]);
				if (num >= 0)
				{
					this.AddRegion(num, columnIndex, 1, 1, notify);
				}
			}
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x0018CC28 File Offset: 0x0018AE28
		private void OnRemoveColumn(int columnIndex, DataGridColumn oldColumn)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count > 0)
			{
				this.RemoveRegion(0, columnIndex + 1, count, count2 - columnIndex, ref list);
				this.RemoveRegion(0, columnIndex, count, 1, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top, cellRegion.Left - 1, cellRegion.Height, cellRegion.Width, false);
					}
				}
				if (list2 != null)
				{
					VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list2, oldColumn);
					this.OnRemove(oldItems);
				}
			}
		}

		// Token: 0x060059FB RID: 23035 RVA: 0x0018CCE4 File Offset: 0x0018AEE4
		private void OnReplaceColumn(int columnIndex, DataGridColumn oldColumn, IList selectedRows)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(0, columnIndex, this._owner.Items.Count, 1, ref list);
			if (list != null)
			{
				VirtualizedCellInfoCollection.RemovedCellInfoCollection oldItems = new VirtualizedCellInfoCollection.RemovedCellInfoCollection(this._owner, list, oldColumn);
				this.OnRemove(oldItems);
			}
			this.FillInFullRowRegions(selectedRows, columnIndex, true);
		}

		// Token: 0x060059FC RID: 23036 RVA: 0x0018CD30 File Offset: 0x0018AF30
		private void OnMoveColumn(int oldIndex, int newIndex)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			List<VirtualizedCellInfoCollection.CellRegion> list2 = null;
			int count = this._owner.Items.Count;
			int count2 = this._owner.Columns.Count;
			if (count > 0)
			{
				this.RemoveRegion(0, oldIndex + 1, count, count2 - oldIndex - 1, ref list);
				this.RemoveRegion(0, oldIndex, count, 1, ref list2);
				if (list != null)
				{
					int count3 = list.Count;
					for (int i = 0; i < count3; i++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion = list[i];
						this.AddRegion(cellRegion.Top, cellRegion.Left - 1, cellRegion.Height, cellRegion.Width, false);
					}
				}
				list = null;
				this.RemoveRegion(0, newIndex, count, count2 - newIndex, ref list);
				if (list2 != null)
				{
					int count4 = list2.Count;
					for (int j = 0; j < count4; j++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion2 = list2[j];
						this.AddRegion(cellRegion2.Top, newIndex, cellRegion2.Height, cellRegion2.Width, false);
					}
				}
				if (list != null)
				{
					int count5 = list.Count;
					for (int k = 0; k < count5; k++)
					{
						VirtualizedCellInfoCollection.CellRegion cellRegion3 = list[k];
						this.AddRegion(cellRegion3.Top, cellRegion3.Left + 1, cellRegion3.Height, cellRegion3.Width, false);
					}
				}
			}
		}

		// Token: 0x060059FD RID: 23037 RVA: 0x0018CE74 File Offset: 0x0018B074
		internal void Union(VirtualizedCellInfoCollection collection)
		{
			int count = collection._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = collection._regions[i];
				this.AddRegion(cellRegion.Top, cellRegion.Left, cellRegion.Height, cellRegion.Width);
			}
		}

		// Token: 0x060059FE RID: 23038 RVA: 0x0018CEC8 File Offset: 0x0018B0C8
		internal static void Xor(VirtualizedCellInfoCollection c1, VirtualizedCellInfoCollection c2)
		{
			VirtualizedCellInfoCollection virtualizedCellInfoCollection = new VirtualizedCellInfoCollection(c2._owner, c2._regions);
			int count = c1._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = c1._regions[i];
				c2.RemoveRegion(cellRegion.Top, cellRegion.Left, cellRegion.Height, cellRegion.Width);
			}
			count = virtualizedCellInfoCollection._regions.Count;
			for (int j = 0; j < count; j++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion2 = virtualizedCellInfoCollection._regions[j];
				c1.RemoveRegion(cellRegion2.Top, cellRegion2.Left, cellRegion2.Height, cellRegion2.Width);
			}
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x0018CF7C File Offset: 0x0018B17C
		internal void ClearFullRows(IList rows)
		{
			if (!this.IsEmpty)
			{
				int count = this._owner.Columns.Count;
				if (this._regions.Count == 1)
				{
					VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[0];
					if (cellRegion.Width == count && cellRegion.Height == rows.Count)
					{
						this.Clear();
						return;
					}
				}
				List<VirtualizedCellInfoCollection.CellRegion> list = new List<VirtualizedCellInfoCollection.CellRegion>();
				int count2 = rows.Count;
				for (int i = 0; i < count2; i++)
				{
					int num = this._owner.Items.IndexOf(rows[i]);
					if (num >= 0)
					{
						this.RemoveRegion(num, 0, 1, count, ref list);
					}
				}
				if (list.Count > 0)
				{
					this.OnRemove(new VirtualizedCellInfoCollection(this._owner, list));
				}
			}
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x0018D044 File Offset: 0x0018B244
		internal void RestoreOnlyFullRows(List<Tuple<int, int>> ranges)
		{
			this.Clear();
			int count = this._owner.Columns.Count;
			if (count > 0)
			{
				foreach (Tuple<int, int> tuple in ranges)
				{
					this.AddRegion(tuple.Item1, 0, tuple.Item2, count);
				}
			}
		}

		// Token: 0x06005A01 RID: 23041 RVA: 0x0018D0BC File Offset: 0x0018B2BC
		internal void RemoveAllButOne(DataGridCellInfo cellInfo)
		{
			if (!this.IsEmpty)
			{
				int rowIndex;
				int columnIndex;
				this.ConvertCellInfoToIndexes(cellInfo, out rowIndex, out columnIndex);
				this.RemoveAllButRegion(rowIndex, columnIndex, 1, 1);
			}
		}

		// Token: 0x06005A02 RID: 23042 RVA: 0x0018D0E8 File Offset: 0x0018B2E8
		internal void RemoveAllButOne()
		{
			if (!this.IsEmpty)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[0];
				this.RemoveAllButRegion(cellRegion.Top, cellRegion.Left, 1, 1);
			}
		}

		// Token: 0x06005A03 RID: 23043 RVA: 0x0018D120 File Offset: 0x0018B320
		internal void RemoveAllButOneRow(int rowIndex)
		{
			if (!this.IsEmpty && rowIndex >= 0)
			{
				this.RemoveAllButRegion(rowIndex, 0, 1, this._owner.Columns.Count);
			}
		}

		// Token: 0x06005A04 RID: 23044 RVA: 0x0018D148 File Offset: 0x0018B348
		private void RemoveAllButRegion(int rowIndex, int columnIndex, int rowCount, int columnCount)
		{
			List<VirtualizedCellInfoCollection.CellRegion> list = null;
			this.RemoveRegion(rowIndex, columnIndex, rowCount, columnCount, ref list);
			VirtualizedCellInfoCollection oldItems = new VirtualizedCellInfoCollection(this._owner, this._regions);
			this._regions.Clear();
			this._regions.Add(new VirtualizedCellInfoCollection.CellRegion(columnIndex, rowIndex, columnCount, rowCount));
			this.OnRemove(oldItems);
		}

		// Token: 0x06005A05 RID: 23045 RVA: 0x0018D1A0 File Offset: 0x0018B3A0
		internal bool Intersects(int rowIndex)
		{
			VirtualizedCellInfoCollection.CellRegion region = new VirtualizedCellInfoCollection.CellRegion(0, rowIndex, this._owner.Columns.Count, 1);
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				if (this._regions[i].Intersects(region))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005A06 RID: 23046 RVA: 0x0018D1FC File Offset: 0x0018B3FC
		internal bool Intersects(int rowIndex, out List<int> columnIndexRanges)
		{
			VirtualizedCellInfoCollection.CellRegion region = new VirtualizedCellInfoCollection.CellRegion(0, rowIndex, this._owner.Columns.Count, 1);
			columnIndexRanges = null;
			int count = this._regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = this._regions[i];
				if (cellRegion.Intersects(region))
				{
					if (columnIndexRanges == null)
					{
						columnIndexRanges = new List<int>();
					}
					columnIndexRanges.Add(cellRegion.Left);
					columnIndexRanges.Add(cellRegion.Width);
				}
			}
			return columnIndexRanges != null;
		}

		// Token: 0x170015E1 RID: 5601
		// (get) Token: 0x06005A07 RID: 23047 RVA: 0x0018D281 File Offset: 0x0018B481
		protected DataGrid Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06005A08 RID: 23048 RVA: 0x0018D289 File Offset: 0x0018B489
		private void ConvertCellInfoToIndexes(DataGridCellInfo cell, out int rowIndex, out int columnIndex)
		{
			columnIndex = cell.Column.DisplayIndex;
			rowIndex = cell.ItemInfo.Index;
			if (rowIndex < 0)
			{
				rowIndex = this._owner.Items.IndexOf(cell.Item);
			}
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x0018D2C8 File Offset: 0x0018B4C8
		private static void ConvertIndexToIndexes(List<VirtualizedCellInfoCollection.CellRegion> regions, int index, out int rowIndex, out int columnIndex)
		{
			columnIndex = -1;
			rowIndex = -1;
			int count = regions.Count;
			for (int i = 0; i < count; i++)
			{
				VirtualizedCellInfoCollection.CellRegion cellRegion = regions[i];
				int size = cellRegion.Size;
				if (index < size)
				{
					columnIndex = cellRegion.Left + index % cellRegion.Width;
					rowIndex = cellRegion.Top + index / cellRegion.Width;
					return;
				}
				index -= size;
			}
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x0018D330 File Offset: 0x0018B530
		private DataGridCellInfo GetCellInfoFromIndex(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, int index)
		{
			int num;
			int num2;
			VirtualizedCellInfoCollection.ConvertIndexToIndexes(regions, index, out num, out num2);
			if (num >= 0 && num2 >= 0 && num < owner.Items.Count && num2 < owner.Columns.Count)
			{
				DataGridColumn column = owner.ColumnFromDisplayIndex(num2);
				ItemsControl.ItemInfo rowInfo = owner.ItemInfoFromIndex(num);
				return this.CreateCellInfo(rowInfo, column, owner);
			}
			return DataGridCellInfo.Unset;
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x0018D38B File Offset: 0x0018B58B
		private void ValidateIsReadOnly()
		{
			if (this.IsReadOnly)
			{
				throw new NotSupportedException(SR.Get("VirtualizedCellInfoCollection_IsReadOnly"));
			}
		}

		// Token: 0x06005A0C RID: 23052 RVA: 0x0018D3A8 File Offset: 0x0018B5A8
		private void AddRegionToList(VirtualizedCellInfoCollection.CellRegion region, List<DataGridCellInfo> list)
		{
			for (int i = region.Top; i <= region.Bottom; i++)
			{
				ItemsControl.ItemInfo rowInfo = this._owner.ItemInfoFromIndex(i);
				for (int j = region.Left; j <= region.Right; j++)
				{
					DataGridColumn column = this._owner.ColumnFromDisplayIndex(j);
					DataGridCellInfo item = this.CreateCellInfo(rowInfo, column, this._owner);
					list.Add(item);
				}
			}
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x0018D417 File Offset: 0x0018B617
		protected virtual DataGridCellInfo CreateCellInfo(ItemsControl.ItemInfo rowInfo, DataGridColumn column, DataGrid owner)
		{
			return new DataGridCellInfo(rowInfo, column, owner);
		}

		// Token: 0x04002F2B RID: 12075
		private bool _isReadOnly;

		// Token: 0x04002F2C RID: 12076
		private DataGrid _owner;

		// Token: 0x04002F2D RID: 12077
		private List<VirtualizedCellInfoCollection.CellRegion> _regions;

		// Token: 0x020009C6 RID: 2502
		private class VirtualizedCellInfoCollectionEnumerator : IEnumerator<DataGridCellInfo>, IDisposable, IEnumerator
		{
			// Token: 0x06008892 RID: 34962 RVA: 0x002525A4 File Offset: 0x002507A4
			public VirtualizedCellInfoCollectionEnumerator(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, VirtualizedCellInfoCollection collection)
			{
				this._owner = owner;
				this._regions = new List<VirtualizedCellInfoCollection.CellRegion>(regions);
				this._current = -1;
				this._collection = collection;
				if (this._regions != null)
				{
					int count = this._regions.Count;
					for (int i = 0; i < count; i++)
					{
						this._count += this._regions[i].Size;
					}
				}
			}

			// Token: 0x06008893 RID: 34963 RVA: 0x000B1359 File Offset: 0x000AF559
			public void Dispose()
			{
				GC.SuppressFinalize(this);
			}

			// Token: 0x06008894 RID: 34964 RVA: 0x00252619 File Offset: 0x00250819
			public bool MoveNext()
			{
				if (this._current < this._count)
				{
					this._current++;
				}
				return this.CurrentWithinBounds;
			}

			// Token: 0x06008895 RID: 34965 RVA: 0x0025263D File Offset: 0x0025083D
			public void Reset()
			{
				this._current = -1;
			}

			// Token: 0x17001ED5 RID: 7893
			// (get) Token: 0x06008896 RID: 34966 RVA: 0x00252646 File Offset: 0x00250846
			public DataGridCellInfo Current
			{
				get
				{
					if (this.CurrentWithinBounds)
					{
						return this._collection.GetCellInfoFromIndex(this._owner, this._regions, this._current);
					}
					return DataGridCellInfo.Unset;
				}
			}

			// Token: 0x17001ED6 RID: 7894
			// (get) Token: 0x06008897 RID: 34967 RVA: 0x00252673 File Offset: 0x00250873
			private bool CurrentWithinBounds
			{
				get
				{
					return this._current >= 0 && this._current < this._count;
				}
			}

			// Token: 0x17001ED7 RID: 7895
			// (get) Token: 0x06008898 RID: 34968 RVA: 0x0025268E File Offset: 0x0025088E
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400458B RID: 17803
			private DataGrid _owner;

			// Token: 0x0400458C RID: 17804
			private List<VirtualizedCellInfoCollection.CellRegion> _regions;

			// Token: 0x0400458D RID: 17805
			private int _current;

			// Token: 0x0400458E RID: 17806
			private int _count;

			// Token: 0x0400458F RID: 17807
			private VirtualizedCellInfoCollection _collection;
		}

		// Token: 0x020009C7 RID: 2503
		private struct CellRegion
		{
			// Token: 0x06008899 RID: 34969 RVA: 0x0025269B File Offset: 0x0025089B
			public CellRegion(int left, int top, int width, int height)
			{
				this._left = left;
				this._top = top;
				this._width = width;
				this._height = height;
			}

			// Token: 0x17001ED8 RID: 7896
			// (get) Token: 0x0600889A RID: 34970 RVA: 0x002526BA File Offset: 0x002508BA
			// (set) Token: 0x0600889B RID: 34971 RVA: 0x002526C2 File Offset: 0x002508C2
			public int Left
			{
				get
				{
					return this._left;
				}
				set
				{
					this._left = value;
				}
			}

			// Token: 0x17001ED9 RID: 7897
			// (get) Token: 0x0600889C RID: 34972 RVA: 0x002526CB File Offset: 0x002508CB
			// (set) Token: 0x0600889D RID: 34973 RVA: 0x002526D3 File Offset: 0x002508D3
			public int Top
			{
				get
				{
					return this._top;
				}
				set
				{
					this._top = value;
				}
			}

			// Token: 0x17001EDA RID: 7898
			// (get) Token: 0x0600889E RID: 34974 RVA: 0x002526DC File Offset: 0x002508DC
			// (set) Token: 0x0600889F RID: 34975 RVA: 0x002526ED File Offset: 0x002508ED
			public int Right
			{
				get
				{
					return this._left + this._width - 1;
				}
				set
				{
					this._width = value - this._left + 1;
				}
			}

			// Token: 0x17001EDB RID: 7899
			// (get) Token: 0x060088A0 RID: 34976 RVA: 0x002526FF File Offset: 0x002508FF
			// (set) Token: 0x060088A1 RID: 34977 RVA: 0x00252710 File Offset: 0x00250910
			public int Bottom
			{
				get
				{
					return this._top + this._height - 1;
				}
				set
				{
					this._height = value - this._top + 1;
				}
			}

			// Token: 0x17001EDC RID: 7900
			// (get) Token: 0x060088A2 RID: 34978 RVA: 0x00252722 File Offset: 0x00250922
			// (set) Token: 0x060088A3 RID: 34979 RVA: 0x0025272A File Offset: 0x0025092A
			public int Width
			{
				get
				{
					return this._width;
				}
				set
				{
					this._width = value;
				}
			}

			// Token: 0x17001EDD RID: 7901
			// (get) Token: 0x060088A4 RID: 34980 RVA: 0x00252733 File Offset: 0x00250933
			// (set) Token: 0x060088A5 RID: 34981 RVA: 0x0025273B File Offset: 0x0025093B
			public int Height
			{
				get
				{
					return this._height;
				}
				set
				{
					this._height = value;
				}
			}

			// Token: 0x17001EDE RID: 7902
			// (get) Token: 0x060088A6 RID: 34982 RVA: 0x00252744 File Offset: 0x00250944
			public bool IsEmpty
			{
				get
				{
					return this._width == 0 || this._height == 0;
				}
			}

			// Token: 0x17001EDF RID: 7903
			// (get) Token: 0x060088A7 RID: 34983 RVA: 0x00252759 File Offset: 0x00250959
			public int Size
			{
				get
				{
					return this._width * this._height;
				}
			}

			// Token: 0x060088A8 RID: 34984 RVA: 0x00252768 File Offset: 0x00250968
			public bool Contains(int x, int y)
			{
				return !this.IsEmpty && (x >= this.Left && y >= this.Top && x <= this.Right) && y <= this.Bottom;
			}

			// Token: 0x060088A9 RID: 34985 RVA: 0x002527A0 File Offset: 0x002509A0
			public bool Contains(VirtualizedCellInfoCollection.CellRegion region)
			{
				return this.Left <= region.Left && this.Top <= region.Top && this.Right >= region.Right && this.Bottom >= region.Bottom;
			}

			// Token: 0x060088AA RID: 34986 RVA: 0x002527F0 File Offset: 0x002509F0
			public bool Intersects(VirtualizedCellInfoCollection.CellRegion region)
			{
				return VirtualizedCellInfoCollection.CellRegion.Intersects(this.Left, this.Right, region.Left, region.Right) && VirtualizedCellInfoCollection.CellRegion.Intersects(this.Top, this.Bottom, region.Top, region.Bottom);
			}

			// Token: 0x060088AB RID: 34987 RVA: 0x0025283F File Offset: 0x00250A3F
			private static bool Intersects(int start1, int end1, int start2, int end2)
			{
				return start1 <= end2 && end1 >= start2;
			}

			// Token: 0x060088AC RID: 34988 RVA: 0x00252850 File Offset: 0x00250A50
			public VirtualizedCellInfoCollection.CellRegion Intersection(VirtualizedCellInfoCollection.CellRegion region)
			{
				if (this.Intersects(region))
				{
					int num = Math.Max(this.Left, region.Left);
					int num2 = Math.Max(this.Top, region.Top);
					int num3 = Math.Min(this.Right, region.Right);
					int num4 = Math.Min(this.Bottom, region.Bottom);
					return new VirtualizedCellInfoCollection.CellRegion(num, num2, num3 - num + 1, num4 - num2 + 1);
				}
				return VirtualizedCellInfoCollection.CellRegion.Empty;
			}

			// Token: 0x060088AD RID: 34989 RVA: 0x002528CC File Offset: 0x00250ACC
			public bool Union(VirtualizedCellInfoCollection.CellRegion region)
			{
				if (this.Contains(region))
				{
					return true;
				}
				if (region.Contains(this))
				{
					this.Left = region.Left;
					this.Top = region.Top;
					this.Width = region.Width;
					this.Height = region.Height;
					return true;
				}
				bool flag = region.Left == this.Left && region.Width == this.Width;
				bool flag2 = region.Top == this.Top && region.Height == this.Height;
				if (flag || flag2)
				{
					int num = flag ? this.Top : this.Left;
					int num2 = flag ? this.Bottom : this.Right;
					int num3 = flag ? region.Top : region.Left;
					int num4 = flag ? region.Bottom : region.Right;
					bool flag3 = false;
					if (num4 <= num2)
					{
						flag3 = (num - num4 <= 1);
					}
					else if (num <= num3)
					{
						flag3 = (num3 - num2 <= 1);
					}
					if (flag3)
					{
						int right = this.Right;
						int bottom = this.Bottom;
						this.Left = Math.Min(this.Left, region.Left);
						this.Top = Math.Min(this.Top, region.Top);
						this.Right = Math.Max(right, region.Right);
						this.Bottom = Math.Max(bottom, region.Bottom);
						return true;
					}
				}
				return false;
			}

			// Token: 0x060088AE RID: 34990 RVA: 0x00252A5C File Offset: 0x00250C5C
			public bool Remainder(VirtualizedCellInfoCollection.CellRegion region, out List<VirtualizedCellInfoCollection.CellRegion> remainder)
			{
				if (this.Intersects(region))
				{
					if (region.Contains(this))
					{
						remainder = null;
					}
					else
					{
						remainder = new List<VirtualizedCellInfoCollection.CellRegion>();
						if (this.Top < region.Top)
						{
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(this.Left, this.Top, this.Width, region.Top - this.Top));
						}
						if (this.Left < region.Left)
						{
							int num = Math.Max(this.Top, region.Top);
							int num2 = Math.Min(this.Bottom, region.Bottom);
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(this.Left, num, region.Left - this.Left, num2 - num + 1));
						}
						if (this.Right > region.Right)
						{
							int num3 = Math.Max(this.Top, region.Top);
							int num4 = Math.Min(this.Bottom, region.Bottom);
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(region.Right + 1, num3, this.Right - region.Right, num4 - num3 + 1));
						}
						if (this.Bottom > region.Bottom)
						{
							remainder.Add(new VirtualizedCellInfoCollection.CellRegion(this.Left, region.Bottom + 1, this.Width, this.Bottom - region.Bottom));
						}
					}
					return true;
				}
				remainder = null;
				return false;
			}

			// Token: 0x17001EE0 RID: 7904
			// (get) Token: 0x060088AF RID: 34991 RVA: 0x00252BCB File Offset: 0x00250DCB
			public static VirtualizedCellInfoCollection.CellRegion Empty
			{
				get
				{
					return new VirtualizedCellInfoCollection.CellRegion(0, 0, 0, 0);
				}
			}

			// Token: 0x04004590 RID: 17808
			private int _left;

			// Token: 0x04004591 RID: 17809
			private int _top;

			// Token: 0x04004592 RID: 17810
			private int _width;

			// Token: 0x04004593 RID: 17811
			private int _height;
		}

		// Token: 0x020009C8 RID: 2504
		private class RemovedCellInfoCollection : VirtualizedCellInfoCollection
		{
			// Token: 0x060088B0 RID: 34992 RVA: 0x00252BD6 File Offset: 0x00250DD6
			internal RemovedCellInfoCollection(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, DataGridColumn column) : base(owner, regions)
			{
				this._removedColumn = column;
			}

			// Token: 0x060088B1 RID: 34993 RVA: 0x00252BE7 File Offset: 0x00250DE7
			internal RemovedCellInfoCollection(DataGrid owner, List<VirtualizedCellInfoCollection.CellRegion> regions, object item) : base(owner, regions)
			{
				this._removedItem = item;
			}

			// Token: 0x060088B2 RID: 34994 RVA: 0x00252BF8 File Offset: 0x00250DF8
			protected override DataGridCellInfo CreateCellInfo(ItemsControl.ItemInfo rowInfo, DataGridColumn column, DataGrid owner)
			{
				if (this._removedColumn != null)
				{
					return new DataGridCellInfo(rowInfo, this._removedColumn, owner);
				}
				return new DataGridCellInfo(this._removedItem, column, owner);
			}

			// Token: 0x04004594 RID: 17812
			private DataGridColumn _removedColumn;

			// Token: 0x04004595 RID: 17813
			private object _removedItem;
		}
	}
}
