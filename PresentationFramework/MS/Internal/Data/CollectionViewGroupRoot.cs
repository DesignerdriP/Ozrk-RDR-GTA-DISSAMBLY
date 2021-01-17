using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200070C RID: 1804
	internal class CollectionViewGroupRoot : CollectionViewGroupInternal, INotifyCollectionChanged
	{
		// Token: 0x060073B4 RID: 29620 RVA: 0x00211D20 File Offset: 0x0020FF20
		internal CollectionViewGroupRoot(CollectionView view) : base("Root", null, false)
		{
			this._view = view;
		}

		// Token: 0x14000155 RID: 341
		// (add) Token: 0x060073B5 RID: 29621 RVA: 0x00211D44 File Offset: 0x0020FF44
		// (remove) Token: 0x060073B6 RID: 29622 RVA: 0x00211D7C File Offset: 0x0020FF7C
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060073B7 RID: 29623 RVA: 0x00211DB1 File Offset: 0x0020FFB1
		public void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (args == null)
			{
				throw new ArgumentNullException("args");
			}
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, args);
			}
		}

		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x060073B8 RID: 29624 RVA: 0x00211DD6 File Offset: 0x0020FFD6
		public virtual ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._groupBy;
			}
		}

		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x060073B9 RID: 29625 RVA: 0x00211DDE File Offset: 0x0020FFDE
		// (set) Token: 0x060073BA RID: 29626 RVA: 0x00211DE6 File Offset: 0x0020FFE6
		public virtual GroupDescriptionSelectorCallback GroupBySelector
		{
			get
			{
				return this._groupBySelector;
			}
			set
			{
				this._groupBySelector = value;
			}
		}

		// Token: 0x060073BB RID: 29627 RVA: 0x00211DEF File Offset: 0x0020FFEF
		protected override void OnGroupByChanged()
		{
			if (this.GroupDescriptionChanged != null)
			{
				this.GroupDescriptionChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x14000156 RID: 342
		// (add) Token: 0x060073BC RID: 29628 RVA: 0x00211E0C File Offset: 0x0021000C
		// (remove) Token: 0x060073BD RID: 29629 RVA: 0x00211E44 File Offset: 0x00210044
		internal event EventHandler GroupDescriptionChanged;

		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x060073BE RID: 29630 RVA: 0x00211E79 File Offset: 0x00210079
		// (set) Token: 0x060073BF RID: 29631 RVA: 0x00211E81 File Offset: 0x00210081
		internal IComparer ActiveComparer
		{
			get
			{
				return this._comparer;
			}
			set
			{
				this._comparer = value;
			}
		}

		// Token: 0x17001B84 RID: 7044
		// (get) Token: 0x060073C0 RID: 29632 RVA: 0x00211E8A File Offset: 0x0021008A
		internal CultureInfo Culture
		{
			get
			{
				return this._view.Culture;
			}
		}

		// Token: 0x17001B85 RID: 7045
		// (get) Token: 0x060073C1 RID: 29633 RVA: 0x00211E97 File Offset: 0x00210097
		// (set) Token: 0x060073C2 RID: 29634 RVA: 0x00211E9F File Offset: 0x0021009F
		internal bool IsDataInGroupOrder
		{
			get
			{
				return this._isDataInGroupOrder;
			}
			set
			{
				this._isDataInGroupOrder = value;
			}
		}

		// Token: 0x17001B86 RID: 7046
		// (get) Token: 0x060073C3 RID: 29635 RVA: 0x00211EA8 File Offset: 0x002100A8
		internal CollectionView View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x060073C4 RID: 29636 RVA: 0x00211EB0 File Offset: 0x002100B0
		internal void Initialize()
		{
			if (CollectionViewGroupRoot._topLevelGroupDescription == null)
			{
				CollectionViewGroupRoot._topLevelGroupDescription = new CollectionViewGroupRoot.TopLevelGroupDescription();
			}
			this.InitializeGroup(this, CollectionViewGroupRoot._topLevelGroupDescription, 0);
		}

		// Token: 0x060073C5 RID: 29637 RVA: 0x00211ED0 File Offset: 0x002100D0
		internal void AddToSubgroups(object item, LiveShapingItem lsi, bool loading)
		{
			this.AddToSubgroups(item, lsi, this, 0, loading);
		}

		// Token: 0x060073C6 RID: 29638 RVA: 0x00211EDD File Offset: 0x002100DD
		internal bool RemoveFromSubgroups(object item)
		{
			return this.RemoveFromSubgroups(item, this, 0);
		}

		// Token: 0x060073C7 RID: 29639 RVA: 0x00211EE8 File Offset: 0x002100E8
		internal void RemoveItemFromSubgroupsByExhaustiveSearch(object item)
		{
			this.RemoveItemFromSubgroupsByExhaustiveSearch(this, item);
		}

		// Token: 0x060073C8 RID: 29640 RVA: 0x00211EF4 File Offset: 0x002100F4
		internal void InsertSpecialItem(int index, object item, bool loading)
		{
			base.ChangeCounts(item, 1);
			base.ProtectedItems.Insert(index, item);
			if (!loading)
			{
				int index2 = base.LeafIndexFromItem(item, index);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index2));
			}
		}

		// Token: 0x060073C9 RID: 29641 RVA: 0x00211F30 File Offset: 0x00210130
		internal void RemoveSpecialItem(int index, object item, bool loading)
		{
			int index2 = -1;
			if (!loading)
			{
				index2 = base.LeafIndexFromItem(item, index);
			}
			base.ChangeCounts(item, -1);
			base.ProtectedItems.RemoveAt(index);
			if (!loading)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index2));
			}
		}

		// Token: 0x060073CA RID: 29642 RVA: 0x00211F70 File Offset: 0x00210170
		internal void MoveWithinSubgroups(object item, LiveShapingItem lsi, IList list, int oldIndex, int newIndex)
		{
			if (lsi == null)
			{
				this.MoveWithinSubgroups(item, this, 0, list, oldIndex, newIndex);
				return;
			}
			CollectionViewGroupInternal parentGroup = lsi.ParentGroup;
			if (parentGroup != null)
			{
				this.MoveWithinSubgroup(item, parentGroup, list, oldIndex, newIndex);
				return;
			}
			foreach (CollectionViewGroupInternal group in lsi.ParentGroups)
			{
				this.MoveWithinSubgroup(item, group, list, oldIndex, newIndex);
			}
		}

		// Token: 0x060073CB RID: 29643 RVA: 0x00211FF4 File Offset: 0x002101F4
		protected override int FindIndex(object item, object seed, IComparer comparer, int low, int high)
		{
			IEditableCollectionView editableCollectionView = this._view as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				if (editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning)
				{
					low++;
					if (editableCollectionView.IsAddingNew)
					{
						low++;
					}
				}
				else
				{
					if (editableCollectionView.IsAddingNew)
					{
						high--;
					}
					if (editableCollectionView.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
					{
						high--;
					}
				}
			}
			return base.FindIndex(item, seed, comparer, low, high);
		}

		// Token: 0x060073CC RID: 29644 RVA: 0x0021205C File Offset: 0x0021025C
		internal void RestoreGrouping(LiveShapingItem lsi, List<AbandonedGroupItem> deleteList)
		{
			CollectionViewGroupRoot.GroupTreeNode groupTreeNode = this.BuildGroupTree(lsi);
			groupTreeNode.ContainsItem = true;
			this.RestoreGrouping(lsi, groupTreeNode, 0, deleteList);
		}

		// Token: 0x060073CD RID: 29645 RVA: 0x00212084 File Offset: 0x00210284
		private void RestoreGrouping(LiveShapingItem lsi, CollectionViewGroupRoot.GroupTreeNode node, int level, List<AbandonedGroupItem> deleteList)
		{
			if (node.ContainsItem)
			{
				object obj = this.GetGroupName(lsi.Item, node.Group.GroupBy, level);
				if (obj == CollectionViewGroupRoot.UseAsItemDirectly)
				{
					goto IL_12E;
				}
				ICollection collection = obj as ICollection;
				ArrayList arrayList = (collection == null) ? null : new ArrayList(collection);
				for (CollectionViewGroupRoot.GroupTreeNode groupTreeNode = node.FirstChild; groupTreeNode != null; groupTreeNode = groupTreeNode.Sibling)
				{
					if (arrayList == null)
					{
						if (object.Equals(obj, groupTreeNode.Group.Name))
						{
							groupTreeNode.ContainsItem = true;
							obj = DependencyProperty.UnsetValue;
							break;
						}
					}
					else if (arrayList.Contains(groupTreeNode.Group.Name))
					{
						groupTreeNode.ContainsItem = true;
						arrayList.Remove(groupTreeNode.Group.Name);
					}
				}
				if (arrayList == null)
				{
					if (obj != DependencyProperty.UnsetValue)
					{
						this.AddToSubgroup(lsi.Item, lsi, node.Group, level, obj, false);
						goto IL_12E;
					}
					goto IL_12E;
				}
				else
				{
					using (IEnumerator enumerator = arrayList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object name = enumerator.Current;
							this.AddToSubgroup(lsi.Item, lsi, node.Group, level, name, false);
						}
						goto IL_12E;
					}
				}
			}
			if (node.ContainsItemDirectly)
			{
				deleteList.Add(new AbandonedGroupItem(lsi, node.Group));
			}
			IL_12E:
			for (CollectionViewGroupRoot.GroupTreeNode groupTreeNode2 = node.FirstChild; groupTreeNode2 != null; groupTreeNode2 = groupTreeNode2.Sibling)
			{
				this.RestoreGrouping(lsi, groupTreeNode2, level + 1, deleteList);
			}
		}

		// Token: 0x060073CE RID: 29646 RVA: 0x002121F4 File Offset: 0x002103F4
		private CollectionViewGroupRoot.GroupTreeNode BuildGroupTree(LiveShapingItem lsi)
		{
			CollectionViewGroupInternal collectionViewGroupInternal = lsi.ParentGroup;
			if (collectionViewGroupInternal != null)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = new CollectionViewGroupRoot.GroupTreeNode
				{
					Group = collectionViewGroupInternal,
					ContainsItemDirectly = true
				};
				for (;;)
				{
					CollectionViewGroupInternal collectionViewGroupInternal2 = collectionViewGroupInternal;
					collectionViewGroupInternal = collectionViewGroupInternal2.Parent;
					if (collectionViewGroupInternal == null)
					{
						break;
					}
					CollectionViewGroupRoot.GroupTreeNode groupTreeNode2 = new CollectionViewGroupRoot.GroupTreeNode
					{
						Group = collectionViewGroupInternal,
						FirstChild = groupTreeNode
					};
					groupTreeNode = groupTreeNode2;
				}
				return groupTreeNode;
			}
			List<CollectionViewGroupInternal> parentGroups = lsi.ParentGroups;
			List<CollectionViewGroupRoot.GroupTreeNode> list = new List<CollectionViewGroupRoot.GroupTreeNode>(parentGroups.Count + 1);
			CollectionViewGroupRoot.GroupTreeNode result = null;
			foreach (CollectionViewGroupInternal group in parentGroups)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = new CollectionViewGroupRoot.GroupTreeNode
				{
					Group = group,
					ContainsItemDirectly = true
				};
				list.Add(groupTreeNode);
			}
			for (int i = 0; i < list.Count; i++)
			{
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode = list[i];
				collectionViewGroupInternal = groupTreeNode.Group.Parent;
				CollectionViewGroupRoot.GroupTreeNode groupTreeNode3 = null;
				if (collectionViewGroupInternal == null)
				{
					result = groupTreeNode;
				}
				else
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						if (list[j].Group == collectionViewGroupInternal)
						{
							groupTreeNode3 = list[j];
							break;
						}
					}
					if (groupTreeNode3 == null)
					{
						groupTreeNode3 = new CollectionViewGroupRoot.GroupTreeNode
						{
							Group = collectionViewGroupInternal,
							FirstChild = groupTreeNode
						};
						list.Add(groupTreeNode3);
					}
					else
					{
						groupTreeNode.Sibling = groupTreeNode3.FirstChild;
						groupTreeNode3.FirstChild = groupTreeNode;
					}
				}
			}
			return result;
		}

		// Token: 0x060073CF RID: 29647 RVA: 0x00212368 File Offset: 0x00210568
		internal void DeleteAbandonedGroupItems(List<AbandonedGroupItem> deleteList)
		{
			foreach (AbandonedGroupItem abandonedGroupItem in deleteList)
			{
				this.RemoveFromGroupDirectly(abandonedGroupItem.Group, abandonedGroupItem.Item.Item);
				abandonedGroupItem.Item.RemoveParentGroup(abandonedGroupItem.Group);
			}
		}

		// Token: 0x060073D0 RID: 29648 RVA: 0x002123D8 File Offset: 0x002105D8
		private void InitializeGroup(CollectionViewGroupInternal group, GroupDescription parentDescription, int level)
		{
			GroupDescription groupDescription = this.GetGroupDescription(group, parentDescription, level);
			group.GroupBy = groupDescription;
			ObservableCollection<object> observableCollection = (groupDescription != null) ? groupDescription.GroupNames : null;
			if (observableCollection != null)
			{
				int i = 0;
				int count = observableCollection.Count;
				while (i < count)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = new CollectionViewGroupInternal(observableCollection[i], group, true);
					this.InitializeGroup(collectionViewGroupInternal, groupDescription, level + 1);
					group.Add(collectionViewGroupInternal);
					i++;
				}
			}
			group.LastIndex = 0;
		}

		// Token: 0x060073D1 RID: 29649 RVA: 0x00212444 File Offset: 0x00210644
		private GroupDescription GetGroupDescription(CollectionViewGroup group, GroupDescription parentDescription, int level)
		{
			GroupDescription groupDescription = null;
			if (group == this)
			{
				group = null;
			}
			if (groupDescription == null && this.GroupBySelector != null)
			{
				groupDescription = this.GroupBySelector(group, level);
			}
			if (groupDescription == null && level < this.GroupDescriptions.Count)
			{
				groupDescription = this.GroupDescriptions[level];
			}
			return groupDescription;
		}

		// Token: 0x060073D2 RID: 29650 RVA: 0x00212494 File Offset: 0x00210694
		private void AddToSubgroups(object item, LiveShapingItem lsi, CollectionViewGroupInternal group, int level, bool loading)
		{
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				if (lsi != null)
				{
					lsi.AddParentGroup(group);
				}
				if (loading)
				{
					group.Add(item);
					return;
				}
				int index = group.Insert(item, item, this.ActiveComparer);
				int index2 = group.LeafIndexFromItem(item, index);
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index2));
				return;
			}
			else
			{
				ICollection collection;
				if ((collection = (groupName as ICollection)) == null)
				{
					this.AddToSubgroup(item, lsi, group, level, groupName, loading);
					return;
				}
				foreach (object name in collection)
				{
					this.AddToSubgroup(item, lsi, group, level, name, loading);
				}
				return;
			}
		}

		// Token: 0x060073D3 RID: 29651 RVA: 0x00212564 File Offset: 0x00210764
		private void AddToSubgroup(object item, LiveShapingItem lsi, CollectionViewGroupInternal group, int level, object name, bool loading)
		{
			int i = (loading && this.IsDataInGroupOrder) ? group.LastIndex : 0;
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				group.LastIndex = ((group.Items[i] == collectionViewGroupInternal) ? i : 0);
				this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
				return;
			}
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					group.LastIndex = i;
					group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
					this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
					return;
				}
				i++;
			}
			collectionViewGroupInternal = new CollectionViewGroupInternal(name, group, false);
			this.InitializeGroup(collectionViewGroupInternal, group.GroupBy, level + 1);
			if (loading)
			{
				group.Add(collectionViewGroupInternal);
				group.LastIndex = i;
			}
			else
			{
				group.Insert(collectionViewGroupInternal, item, this.ActiveComparer);
			}
			group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
			this.AddToSubgroups(item, lsi, collectionViewGroupInternal, level + 1, loading);
		}

		// Token: 0x060073D4 RID: 29652 RVA: 0x00212690 File Offset: 0x00210890
		private void MoveWithinSubgroups(object item, CollectionViewGroupInternal group, int level, IList list, int oldIndex, int newIndex)
		{
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				this.MoveWithinSubgroup(item, group, list, oldIndex, newIndex);
				return;
			}
			ICollection collection;
			if ((collection = (groupName as ICollection)) == null)
			{
				this.MoveWithinSubgroup(item, group, level, groupName, list, oldIndex, newIndex);
				return;
			}
			foreach (object name in collection)
			{
				this.MoveWithinSubgroup(item, group, level, name, list, oldIndex, newIndex);
			}
		}

		// Token: 0x060073D5 RID: 29653 RVA: 0x0021272C File Offset: 0x0021092C
		private void MoveWithinSubgroup(object item, CollectionViewGroupInternal group, int level, object name, IList list, int oldIndex, int newIndex)
		{
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				this.MoveWithinSubgroups(item, collectionViewGroupInternal, level + 1, list, oldIndex, newIndex);
				return;
			}
			int i = 0;
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					group.AddSubgroupToMap(groupNameKey, collectionViewGroupInternal);
					this.MoveWithinSubgroups(item, collectionViewGroupInternal, level + 1, list, oldIndex, newIndex);
					return;
				}
				i++;
			}
		}

		// Token: 0x060073D6 RID: 29654 RVA: 0x002127D1 File Offset: 0x002109D1
		private void MoveWithinSubgroup(object item, CollectionViewGroupInternal group, IList list, int oldIndex, int newIndex)
		{
			if (group.Move(item, list, ref oldIndex, ref newIndex))
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, item, newIndex, oldIndex));
			}
		}

		// Token: 0x060073D7 RID: 29655 RVA: 0x002127F4 File Offset: 0x002109F4
		private object GetGroupNameKey(object name, CollectionViewGroupInternal group)
		{
			object result = name;
			PropertyGroupDescription propertyGroupDescription = group.GroupBy as PropertyGroupDescription;
			if (propertyGroupDescription != null)
			{
				string text = name as string;
				if (text != null)
				{
					if (propertyGroupDescription.StringComparison == StringComparison.OrdinalIgnoreCase || propertyGroupDescription.StringComparison == StringComparison.InvariantCultureIgnoreCase)
					{
						text = text.ToUpperInvariant();
					}
					else if (propertyGroupDescription.StringComparison == StringComparison.CurrentCultureIgnoreCase)
					{
						text = text.ToUpper(CultureInfo.CurrentCulture);
					}
					result = text;
				}
			}
			return result;
		}

		// Token: 0x060073D8 RID: 29656 RVA: 0x00212850 File Offset: 0x00210A50
		private bool RemoveFromSubgroups(object item, CollectionViewGroupInternal group, int level)
		{
			bool result = false;
			object groupName = this.GetGroupName(item, group.GroupBy, level);
			ICollection collection;
			if (groupName == CollectionViewGroupRoot.UseAsItemDirectly)
			{
				result = this.RemoveFromGroupDirectly(group, item);
			}
			else if ((collection = (groupName as ICollection)) == null)
			{
				if (this.RemoveFromSubgroup(item, group, level, groupName))
				{
					result = true;
				}
			}
			else
			{
				foreach (object name in collection)
				{
					if (this.RemoveFromSubgroup(item, group, level, name))
					{
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060073D9 RID: 29657 RVA: 0x002128EC File Offset: 0x00210AEC
		private bool RemoveFromSubgroup(object item, CollectionViewGroupInternal group, int level, object name)
		{
			object groupNameKey = this.GetGroupNameKey(name, group);
			CollectionViewGroupInternal collectionViewGroupInternal;
			if ((collectionViewGroupInternal = group.GetSubgroupFromMap(groupNameKey)) != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
			{
				return this.RemoveFromSubgroups(item, collectionViewGroupInternal, level + 1);
			}
			int i = 0;
			int count = group.Items.Count;
			while (i < count)
			{
				collectionViewGroupInternal = (group.Items[i] as CollectionViewGroupInternal);
				if (collectionViewGroupInternal != null && group.GroupBy.NamesMatch(collectionViewGroupInternal.Name, name))
				{
					return this.RemoveFromSubgroups(item, collectionViewGroupInternal, level + 1);
				}
				i++;
			}
			return true;
		}

		// Token: 0x060073DA RID: 29658 RVA: 0x00212980 File Offset: 0x00210B80
		private bool RemoveFromGroupDirectly(CollectionViewGroupInternal group, object item)
		{
			int num = group.Remove(item, true);
			if (num >= 0)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, num));
				return false;
			}
			return true;
		}

		// Token: 0x060073DB RID: 29659 RVA: 0x002129AC File Offset: 0x00210BAC
		private void RemoveItemFromSubgroupsByExhaustiveSearch(CollectionViewGroupInternal group, object item)
		{
			if (this.RemoveFromGroupDirectly(group, item))
			{
				for (int i = group.Items.Count - 1; i >= 0; i--)
				{
					CollectionViewGroupInternal collectionViewGroupInternal = group.Items[i] as CollectionViewGroupInternal;
					if (collectionViewGroupInternal != null)
					{
						this.RemoveItemFromSubgroupsByExhaustiveSearch(collectionViewGroupInternal, item);
					}
				}
			}
		}

		// Token: 0x060073DC RID: 29660 RVA: 0x002129F8 File Offset: 0x00210BF8
		private object GetGroupName(object item, GroupDescription groupDescription, int level)
		{
			if (groupDescription != null)
			{
				return groupDescription.GroupNameFromItem(item, level, this.Culture);
			}
			return CollectionViewGroupRoot.UseAsItemDirectly;
		}

		// Token: 0x040037B8 RID: 14264
		private CollectionView _view;

		// Token: 0x040037B9 RID: 14265
		private IComparer _comparer;

		// Token: 0x040037BA RID: 14266
		private bool _isDataInGroupOrder;

		// Token: 0x040037BB RID: 14267
		private ObservableCollection<GroupDescription> _groupBy = new ObservableCollection<GroupDescription>();

		// Token: 0x040037BC RID: 14268
		private GroupDescriptionSelectorCallback _groupBySelector;

		// Token: 0x040037BD RID: 14269
		private static GroupDescription _topLevelGroupDescription;

		// Token: 0x040037BE RID: 14270
		private static readonly object UseAsItemDirectly = new NamedObject("UseAsItemDirectly");

		// Token: 0x02000B45 RID: 2885
		private class GroupTreeNode
		{
			// Token: 0x17001F79 RID: 8057
			// (get) Token: 0x06008D8B RID: 36235 RVA: 0x00259A48 File Offset: 0x00257C48
			// (set) Token: 0x06008D8C RID: 36236 RVA: 0x00259A50 File Offset: 0x00257C50
			public CollectionViewGroupRoot.GroupTreeNode FirstChild { get; set; }

			// Token: 0x17001F7A RID: 8058
			// (get) Token: 0x06008D8D RID: 36237 RVA: 0x00259A59 File Offset: 0x00257C59
			// (set) Token: 0x06008D8E RID: 36238 RVA: 0x00259A61 File Offset: 0x00257C61
			public CollectionViewGroupRoot.GroupTreeNode Sibling { get; set; }

			// Token: 0x17001F7B RID: 8059
			// (get) Token: 0x06008D8F RID: 36239 RVA: 0x00259A6A File Offset: 0x00257C6A
			// (set) Token: 0x06008D90 RID: 36240 RVA: 0x00259A72 File Offset: 0x00257C72
			public CollectionViewGroupInternal Group { get; set; }

			// Token: 0x17001F7C RID: 8060
			// (get) Token: 0x06008D91 RID: 36241 RVA: 0x00259A7B File Offset: 0x00257C7B
			// (set) Token: 0x06008D92 RID: 36242 RVA: 0x00259A83 File Offset: 0x00257C83
			public bool ContainsItem { get; set; }

			// Token: 0x17001F7D RID: 8061
			// (get) Token: 0x06008D93 RID: 36243 RVA: 0x00259A8C File Offset: 0x00257C8C
			// (set) Token: 0x06008D94 RID: 36244 RVA: 0x00259A94 File Offset: 0x00257C94
			public bool ContainsItemDirectly { get; set; }
		}

		// Token: 0x02000B46 RID: 2886
		private class TopLevelGroupDescription : GroupDescription
		{
			// Token: 0x06008D97 RID: 36247 RVA: 0x00041C10 File Offset: 0x0003FE10
			public override object GroupNameFromItem(object item, int level, CultureInfo culture)
			{
				throw new NotSupportedException();
			}
		}
	}
}
