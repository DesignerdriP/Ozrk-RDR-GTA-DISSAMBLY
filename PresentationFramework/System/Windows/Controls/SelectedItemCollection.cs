using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	// Token: 0x0200052D RID: 1325
	internal class SelectedItemCollection : ObservableCollection<object>
	{
		// Token: 0x060055DA RID: 21978 RVA: 0x0017CA81 File Offset: 0x0017AC81
		public SelectedItemCollection(Selector selector)
		{
			this._selector = selector;
			this._changer = new SelectedItemCollection.Changer(this);
		}

		// Token: 0x060055DB RID: 21979 RVA: 0x0017CA9C File Offset: 0x0017AC9C
		protected override void ClearItems()
		{
			if (this._updatingSelectedItems)
			{
				using (IEnumerator<ItemsControl.ItemInfo> enumerator = ((IEnumerable<ItemsControl.ItemInfo>)this._selector._selectedItems).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemsControl.ItemInfo info = enumerator.Current;
						this._selector.SelectionChange.Unselect(info);
					}
					return;
				}
			}
			using (this.ChangeSelectedItems())
			{
				base.ClearItems();
			}
		}

		// Token: 0x060055DC RID: 21980 RVA: 0x0017CB28 File Offset: 0x0017AD28
		protected override void RemoveItem(int index)
		{
			if (this._updatingSelectedItems)
			{
				this._selector.SelectionChange.Unselect(this._selector.NewItemInfo(base[index], null, -1));
				return;
			}
			using (this.ChangeSelectedItems())
			{
				base.RemoveItem(index);
			}
		}

		// Token: 0x060055DD RID: 21981 RVA: 0x0017CB90 File Offset: 0x0017AD90
		protected override void InsertItem(int index, object item)
		{
			if (!this._updatingSelectedItems)
			{
				using (this.ChangeSelectedItems())
				{
					base.InsertItem(index, item);
				}
				return;
			}
			if (index == base.Count)
			{
				this._selector.SelectionChange.Select(this._selector.NewItemInfo(item, null, -1), true);
				return;
			}
			throw new InvalidOperationException(SR.Get("InsertInDeferSelectionActive"));
		}

		// Token: 0x060055DE RID: 21982 RVA: 0x0017CC0C File Offset: 0x0017AE0C
		protected override void SetItem(int index, object item)
		{
			if (this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("SetInDeferSelectionActive"));
			}
			using (this.ChangeSelectedItems())
			{
				base.SetItem(index, item);
			}
		}

		// Token: 0x060055DF RID: 21983 RVA: 0x0017CC5C File Offset: 0x0017AE5C
		protected override void MoveItem(int oldIndex, int newIndex)
		{
			if (oldIndex != newIndex)
			{
				if (this._updatingSelectedItems)
				{
					throw new InvalidOperationException(SR.Get("MoveInDeferSelectionActive"));
				}
				using (this.ChangeSelectedItems())
				{
					base.MoveItem(oldIndex, newIndex);
				}
			}
		}

		// Token: 0x170014DE RID: 5342
		// (get) Token: 0x060055E0 RID: 21984 RVA: 0x0017CCB0 File Offset: 0x0017AEB0
		internal bool IsChanging
		{
			get
			{
				return this._changeCount > 0;
			}
		}

		// Token: 0x060055E1 RID: 21985 RVA: 0x0017CCBB File Offset: 0x0017AEBB
		private IDisposable ChangeSelectedItems()
		{
			this._changeCount++;
			return this._changer;
		}

		// Token: 0x060055E2 RID: 21986 RVA: 0x0017CCD4 File Offset: 0x0017AED4
		private void FinishChange()
		{
			int num = this._changeCount - 1;
			this._changeCount = num;
			if (num == 0)
			{
				this._selector.FinishSelectedItemsChange();
			}
		}

		// Token: 0x060055E3 RID: 21987 RVA: 0x0017CD00 File Offset: 0x0017AF00
		internal void BeginUpdateSelectedItems()
		{
			if (this._selector.SelectionChange.IsActive || this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionActive"));
			}
			this._updatingSelectedItems = true;
			this._selector.SelectionChange.Begin();
		}

		// Token: 0x060055E4 RID: 21988 RVA: 0x0017CD50 File Offset: 0x0017AF50
		internal void EndUpdateSelectedItems()
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._updatingSelectedItems = false;
			this._selector.SelectionChange.End();
		}

		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x060055E5 RID: 21989 RVA: 0x0017CD9E File Offset: 0x0017AF9E
		internal bool IsUpdatingSelectedItems
		{
			get
			{
				return this._selector.SelectionChange.IsActive || this._updatingSelectedItems;
			}
		}

		// Token: 0x060055E6 RID: 21990 RVA: 0x0017CDBA File Offset: 0x0017AFBA
		internal void Add(ItemsControl.ItemInfo info)
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._selector.SelectionChange.Select(info, true);
		}

		// Token: 0x060055E7 RID: 21991 RVA: 0x0017CDF9 File Offset: 0x0017AFF9
		internal void Remove(ItemsControl.ItemInfo info)
		{
			if (!this._selector.SelectionChange.IsActive || !this._updatingSelectedItems)
			{
				throw new InvalidOperationException(SR.Get("DeferSelectionNotActive"));
			}
			this._selector.SelectionChange.Unselect(info);
		}

		// Token: 0x04002E13 RID: 11795
		private int _changeCount;

		// Token: 0x04002E14 RID: 11796
		private SelectedItemCollection.Changer _changer;

		// Token: 0x04002E15 RID: 11797
		private Selector _selector;

		// Token: 0x04002E16 RID: 11798
		private bool _updatingSelectedItems;

		// Token: 0x020009B8 RID: 2488
		private class Changer : IDisposable
		{
			// Token: 0x06008869 RID: 34921 RVA: 0x0025217F File Offset: 0x0025037F
			public Changer(SelectedItemCollection owner)
			{
				this._owner = owner;
			}

			// Token: 0x0600886A RID: 34922 RVA: 0x0025218E File Offset: 0x0025038E
			public void Dispose()
			{
				this._owner.FinishChange();
			}

			// Token: 0x0400455B RID: 17755
			private SelectedItemCollection _owner;
		}
	}
}
