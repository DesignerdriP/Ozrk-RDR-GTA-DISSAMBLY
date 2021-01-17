using System;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace MS.Internal.Data
{
	// Token: 0x02000731 RID: 1841
	internal class LiveShapingTree : RBTree<LiveShapingItem>
	{
		// Token: 0x060075E1 RID: 30177 RVA: 0x0021A1A8 File Offset: 0x002183A8
		internal LiveShapingTree(LiveShapingList list)
		{
			this._list = list;
		}

		// Token: 0x17001C14 RID: 7188
		// (get) Token: 0x060075E2 RID: 30178 RVA: 0x0021A1B7 File Offset: 0x002183B7
		internal LiveShapingList List
		{
			get
			{
				return this._list;
			}
		}

		// Token: 0x17001C15 RID: 7189
		// (get) Token: 0x060075E3 RID: 30179 RVA: 0x0021A1BF File Offset: 0x002183BF
		internal LiveShapingBlock PlaceholderBlock
		{
			get
			{
				if (this._placeholderBlock == null)
				{
					this._placeholderBlock = new LiveShapingBlock(false);
					this._placeholderBlock.Parent = this;
				}
				return this._placeholderBlock;
			}
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x0021A1E7 File Offset: 0x002183E7
		internal override RBNode<LiveShapingItem> NewNode()
		{
			return new LiveShapingBlock();
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x0021A1F0 File Offset: 0x002183F0
		internal void Move(int oldIndex, int newIndex)
		{
			LiveShapingItem item = base[oldIndex];
			base.RemoveAt(oldIndex);
			base.Insert(newIndex, item);
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x0021A214 File Offset: 0x00218414
		internal void RestoreLiveSortingByInsertionSort(Action<NotifyCollectionChangedEventArgs, int, int> RaiseMoveEvent)
		{
			RBFinger<LiveShapingItem> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				LiveShapingItem item = finger.Item;
				item.IsSortDirty = false;
				item.IsSortPendingClean = false;
				RBFinger<LiveShapingItem> newFinger = base.LocateItem(finger, base.Comparison);
				int index = finger.Index;
				int index2 = newFinger.Index;
				if (index != index2)
				{
					base.ReInsert(ref finger, newFinger);
					RaiseMoveEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item.Item, index, index2), index, index2);
				}
				finger = ++finger;
			}
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x0021A298 File Offset: 0x00218498
		internal void FindPosition(LiveShapingItem lsi, out int oldIndex, out int newIndex)
		{
			RBFinger<LiveShapingItem> rbfinger;
			RBFinger<LiveShapingItem> rbfinger2;
			lsi.FindPosition(out rbfinger, out rbfinger2, base.Comparison);
			oldIndex = rbfinger.Index;
			newIndex = rbfinger2.Index;
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x0021A2C8 File Offset: 0x002184C8
		internal void ReplaceAt(int index, object item)
		{
			RBFinger<LiveShapingItem> rbfinger = base.FindIndex(index, true);
			LiveShapingItem item2 = rbfinger.Item;
			item2.Clear();
			rbfinger.Node.SetItemAt(rbfinger.Offset, new LiveShapingItem(item, this.List, false, null, false));
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x0021A310 File Offset: 0x00218510
		internal LiveShapingItem FindItem(object item)
		{
			RBFinger<LiveShapingItem> finger = base.FindIndex(0, true);
			while (finger.Node != this)
			{
				if (ItemsControl.EqualsEx(finger.Item.Item, item))
				{
					return finger.Item;
				}
				finger = ++finger;
			}
			return null;
		}

		// Token: 0x060075EA RID: 30186 RVA: 0x0021A358 File Offset: 0x00218558
		public override int IndexOf(LiveShapingItem lsi)
		{
			RBFinger<LiveShapingItem> finger = lsi.GetFinger();
			if (!finger.Found)
			{
				return -1;
			}
			return finger.Index;
		}

		// Token: 0x0400384C RID: 14412
		private LiveShapingList _list;

		// Token: 0x0400384D RID: 14413
		private LiveShapingBlock _placeholderBlock;
	}
}
