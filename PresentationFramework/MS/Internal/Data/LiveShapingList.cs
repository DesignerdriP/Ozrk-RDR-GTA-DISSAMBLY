using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace MS.Internal.Data
{
	// Token: 0x02000730 RID: 1840
	internal class LiveShapingList : IList, ICollection, IEnumerable
	{
		// Token: 0x060075AC RID: 30124 RVA: 0x002196E0 File Offset: 0x002178E0
		internal LiveShapingList(ICollectionViewLiveShaping view, LiveShapingFlags flags, IComparer comparer)
		{
			this._view = view;
			this._comparer = comparer;
			this._isCustomSorting = !(comparer is SortFieldComparer);
			this._dpFromPath = new LiveShapingList.DPFromPath();
			this._root = new LiveShapingTree(this);
			if (comparer != null)
			{
				this._root.Comparison = new Comparison<LiveShapingItem>(this.CompareLiveShapingItems);
			}
			this._sortDirtyItems = new List<LiveShapingItem>();
			this._filterDirtyItems = new List<LiveShapingItem>();
			this._groupDirtyItems = new List<LiveShapingItem>();
			this.SetLiveShapingProperties(flags);
		}

		// Token: 0x17001C08 RID: 7176
		// (get) Token: 0x060075AD RID: 30125 RVA: 0x0021976C File Offset: 0x0021796C
		internal ICollectionViewLiveShaping View
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x17001C09 RID: 7177
		// (get) Token: 0x060075AE RID: 30126 RVA: 0x00219774 File Offset: 0x00217974
		internal Dictionary<string, DependencyProperty> ObservedProperties
		{
			get
			{
				return this._dpFromPath;
			}
		}

		// Token: 0x060075AF RID: 30127 RVA: 0x0021977C File Offset: 0x0021797C
		internal void SetLiveShapingProperties(LiveShapingFlags flags)
		{
			this._dpFromPath.BeginReset();
			SortDescriptionCollection sortDescriptions = ((ICollectionView)this.View).SortDescriptions;
			int count = sortDescriptions.Count;
			this._compInfos = new LivePropertyInfo[count];
			for (int i = 0; i < count; i++)
			{
				string path = this.NormalizePath(sortDescriptions[i].PropertyName);
				this._compInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
			}
			if (this.TestLiveShapingFlag(flags, LiveShapingFlags.Sorting))
			{
				Collection<string> liveSortingProperties = this.View.LiveSortingProperties;
				if (liveSortingProperties.Count == 0)
				{
					this._sortInfos = this._compInfos;
				}
				else
				{
					count = liveSortingProperties.Count;
					this._sortInfos = new LivePropertyInfo[count];
					for (int i = 0; i < count; i++)
					{
						string path = this.NormalizePath(liveSortingProperties[i]);
						this._sortInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
					}
				}
			}
			else
			{
				this._sortInfos = new LivePropertyInfo[0];
			}
			if (this.TestLiveShapingFlag(flags, LiveShapingFlags.Filtering))
			{
				Collection<string> liveFilteringProperties = this.View.LiveFilteringProperties;
				count = liveFilteringProperties.Count;
				this._filterInfos = new LivePropertyInfo[count];
				for (int i = 0; i < count; i++)
				{
					string path = this.NormalizePath(liveFilteringProperties[i]);
					this._filterInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
				}
				this._filterRoot = new LiveShapingTree(this);
			}
			else
			{
				this._filterInfos = new LivePropertyInfo[0];
				this._filterRoot = null;
			}
			if (this.TestLiveShapingFlag(flags, LiveShapingFlags.Grouping))
			{
				Collection<string> collection = this.View.LiveGroupingProperties;
				if (collection.Count == 0)
				{
					collection = new Collection<string>();
					ICollectionView collectionView = this.View as ICollectionView;
					ObservableCollection<GroupDescription> observableCollection = (collectionView != null) ? collectionView.GroupDescriptions : null;
					if (observableCollection != null)
					{
						foreach (GroupDescription groupDescription in observableCollection)
						{
							PropertyGroupDescription propertyGroupDescription = groupDescription as PropertyGroupDescription;
							if (propertyGroupDescription != null)
							{
								collection.Add(propertyGroupDescription.PropertyName);
							}
						}
					}
				}
				count = collection.Count;
				this._groupInfos = new LivePropertyInfo[count];
				for (int i = 0; i < count; i++)
				{
					string path = this.NormalizePath(collection[i]);
					this._groupInfos[i] = new LivePropertyInfo(path, this._dpFromPath.GetDP(path));
				}
			}
			else
			{
				this._groupInfos = new LivePropertyInfo[0];
			}
			this._dpFromPath.EndReset();
		}

		// Token: 0x060075B0 RID: 30128 RVA: 0x00219A10 File Offset: 0x00217C10
		private bool TestLiveShapingFlag(LiveShapingFlags flags, LiveShapingFlags flag)
		{
			return (flags & flag) > (LiveShapingFlags)0;
		}

		// Token: 0x060075B1 RID: 30129 RVA: 0x00219A18 File Offset: 0x00217C18
		internal int Search(int index, int count, object value)
		{
			LiveShapingItem liveShapingItem = new LiveShapingItem(value, this, true, null, true);
			RBFinger<LiveShapingItem> rbfinger = this._root.BoundedSearch(liveShapingItem, index, index + count);
			this.ClearItem(liveShapingItem);
			if (!rbfinger.Found)
			{
				return ~rbfinger.Index;
			}
			return rbfinger.Index;
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x00219A61 File Offset: 0x00217C61
		internal void Sort()
		{
			this._root.Sort();
		}

		// Token: 0x060075B3 RID: 30131 RVA: 0x00219A70 File Offset: 0x00217C70
		internal int CompareLiveShapingItems(LiveShapingItem x, LiveShapingItem y)
		{
			if (x == y || ItemsControl.EqualsEx(x.Item, y.Item))
			{
				return 0;
			}
			int num = 0;
			if (!this._isCustomSorting)
			{
				SortFieldComparer sortFieldComparer = this._comparer as SortFieldComparer;
				SortDescriptionCollection sortDescriptions = ((ICollectionView)this.View).SortDescriptions;
				int num2 = this._compInfos.Length;
				for (int i = 0; i < num2; i++)
				{
					object value = x.GetValue(this._compInfos[i].Path, this._compInfos[i].Property);
					object value2 = y.GetValue(this._compInfos[i].Path, this._compInfos[i].Property);
					num = sortFieldComparer.BaseComparer.Compare(value, value2);
					if (sortDescriptions[i].Direction == ListSortDirection.Descending)
					{
						num = -num;
					}
					if (num != 0)
					{
						break;
					}
				}
			}
			else
			{
				num = this._comparer.Compare(x.Item, y.Item);
			}
			return num;
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x00219B7C File Offset: 0x00217D7C
		internal void Move(int oldIndex, int newIndex)
		{
			this._root.Move(oldIndex, newIndex);
		}

		// Token: 0x060075B5 RID: 30133 RVA: 0x00219B8B File Offset: 0x00217D8B
		internal void RestoreLiveSortingByInsertionSort(Action<NotifyCollectionChangedEventArgs, int, int> RaiseMoveEvent)
		{
			this._isRestoringLiveSorting = true;
			this._root.RestoreLiveSortingByInsertionSort(RaiseMoveEvent);
			this._isRestoringLiveSorting = false;
		}

		// Token: 0x060075B6 RID: 30134 RVA: 0x00219BA8 File Offset: 0x00217DA8
		internal void AddFilteredItem(object item)
		{
			LiveShapingItem item2 = new LiveShapingItem(item, this, true, null, false)
			{
				FailsFilter = true
			};
			this._filterRoot.Insert(this._filterRoot.Count, item2);
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x00219BDE File Offset: 0x00217DDE
		internal void AddFilteredItem(LiveShapingItem lsi)
		{
			this.InitializeItem(lsi, lsi.Item, true, false);
			lsi.FailsFilter = true;
			this._filterRoot.Insert(this._filterRoot.Count, lsi);
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x00219C10 File Offset: 0x00217E10
		internal void SetStartingIndexForFilteredItem(object item, int value)
		{
			foreach (LiveShapingItem liveShapingItem in this._filterDirtyItems)
			{
				if (ItemsControl.EqualsEx(item, liveShapingItem.Item))
				{
					liveShapingItem.StartingIndex = value;
					break;
				}
			}
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x00219C74 File Offset: 0x00217E74
		internal void RemoveFilteredItem(LiveShapingItem lsi)
		{
			this._filterRoot.RemoveAt(this._filterRoot.IndexOf(lsi));
			this.ClearItem(lsi);
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x00219C94 File Offset: 0x00217E94
		internal void RemoveFilteredItem(object item)
		{
			LiveShapingItem liveShapingItem = this._filterRoot.FindItem(item);
			if (liveShapingItem != null)
			{
				this.RemoveFilteredItem(liveShapingItem);
			}
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x00219CB8 File Offset: 0x00217EB8
		internal void ReplaceFilteredItem(object oldItem, object newItem)
		{
			LiveShapingItem liveShapingItem = this._filterRoot.FindItem(oldItem);
			if (liveShapingItem != null)
			{
				this.ClearItem(liveShapingItem);
				this.InitializeItem(liveShapingItem, newItem, true, false);
			}
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x00219CE6 File Offset: 0x00217EE6
		internal int IndexOf(LiveShapingItem lsi)
		{
			return this._root.IndexOf(lsi);
		}

		// Token: 0x060075BD RID: 30141 RVA: 0x00219CF4 File Offset: 0x00217EF4
		internal void InitializeItem(LiveShapingItem lsi, object item, bool filtered, bool oneTime)
		{
			lsi.Item = item;
			if (!filtered)
			{
				foreach (LivePropertyInfo livePropertyInfo in this._sortInfos)
				{
					lsi.Block = this._root.PlaceholderBlock;
					lsi.SetBinding(livePropertyInfo.Path, livePropertyInfo.Property, oneTime, true);
				}
				foreach (LivePropertyInfo livePropertyInfo2 in this._groupInfos)
				{
					lsi.SetBinding(livePropertyInfo2.Path, livePropertyInfo2.Property, oneTime, false);
				}
			}
			foreach (LivePropertyInfo livePropertyInfo3 in this._filterInfos)
			{
				lsi.SetBinding(livePropertyInfo3.Path, livePropertyInfo3.Property, oneTime, false);
			}
			lsi.ForwardChanges = !oneTime;
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x00219DD4 File Offset: 0x00217FD4
		internal void ClearItem(LiveShapingItem lsi)
		{
			lsi.ForwardChanges = false;
			foreach (DependencyProperty dp in this.ObservedProperties.Values)
			{
				BindingOperations.ClearBinding(lsi, dp);
			}
		}

		// Token: 0x060075BF RID: 30143 RVA: 0x00219E34 File Offset: 0x00218034
		private string NormalizePath(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				return path;
			}
			return string.Empty;
		}

		// Token: 0x060075C0 RID: 30144 RVA: 0x00219E48 File Offset: 0x00218048
		internal void OnItemPropertyChanged(LiveShapingItem lsi, DependencyProperty dp)
		{
			if (this.ContainsDP(this._sortInfos, dp) && !lsi.FailsFilter && !lsi.IsSortPendingClean)
			{
				lsi.IsSortDirty = true;
				lsi.IsSortPendingClean = true;
				this._sortDirtyItems.Add(lsi);
				this.OnLiveShapingDirty();
			}
			if (this.ContainsDP(this._filterInfos, dp) && !lsi.IsFilterDirty)
			{
				lsi.IsFilterDirty = true;
				this._filterDirtyItems.Add(lsi);
				this.OnLiveShapingDirty();
			}
			if (this.ContainsDP(this._groupInfos, dp) && !lsi.FailsFilter && !lsi.IsGroupDirty)
			{
				lsi.IsGroupDirty = true;
				this._groupDirtyItems.Add(lsi);
				this.OnLiveShapingDirty();
			}
		}

		// Token: 0x060075C1 RID: 30145 RVA: 0x00219EFC File Offset: 0x002180FC
		internal void OnItemPropertyChangedCrossThread(LiveShapingItem lsi, DependencyProperty dp)
		{
			if (this._isCustomSorting && this.ContainsDP(this._sortInfos, dp) && !lsi.FailsFilter)
			{
				lsi.IsSortDirty = true;
			}
		}

		// Token: 0x1400015B RID: 347
		// (add) Token: 0x060075C2 RID: 30146 RVA: 0x00219F24 File Offset: 0x00218124
		// (remove) Token: 0x060075C3 RID: 30147 RVA: 0x00219F5C File Offset: 0x0021815C
		internal event EventHandler LiveShapingDirty;

		// Token: 0x060075C4 RID: 30148 RVA: 0x00219F91 File Offset: 0x00218191
		private void OnLiveShapingDirty()
		{
			if (this.LiveShapingDirty != null)
			{
				this.LiveShapingDirty(this, EventArgs.Empty);
			}
		}

		// Token: 0x060075C5 RID: 30149 RVA: 0x00219FAC File Offset: 0x002181AC
		private bool ContainsDP(LivePropertyInfo[] infos, DependencyProperty dp)
		{
			for (int i = 0; i < infos.Length; i++)
			{
				if (infos[i].Property == dp || (dp == null && string.IsNullOrEmpty(infos[i].Path)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060075C6 RID: 30150 RVA: 0x00219FEF File Offset: 0x002181EF
		internal void FindPosition(LiveShapingItem lsi, out int oldIndex, out int newIndex)
		{
			this._root.FindPosition(lsi, out oldIndex, out newIndex);
		}

		// Token: 0x17001C0A RID: 7178
		// (get) Token: 0x060075C7 RID: 30151 RVA: 0x00219FFF File Offset: 0x002181FF
		internal List<LiveShapingItem> SortDirtyItems
		{
			get
			{
				return this._sortDirtyItems;
			}
		}

		// Token: 0x17001C0B RID: 7179
		// (get) Token: 0x060075C8 RID: 30152 RVA: 0x0021A007 File Offset: 0x00218207
		internal List<LiveShapingItem> FilterDirtyItems
		{
			get
			{
				return this._filterDirtyItems;
			}
		}

		// Token: 0x17001C0C RID: 7180
		// (get) Token: 0x060075C9 RID: 30153 RVA: 0x0021A00F File Offset: 0x0021820F
		internal List<LiveShapingItem> GroupDirtyItems
		{
			get
			{
				return this._groupDirtyItems;
			}
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x0021A017 File Offset: 0x00218217
		internal LiveShapingItem ItemAt(int index)
		{
			return this._root[index];
		}

		// Token: 0x17001C0D RID: 7181
		// (get) Token: 0x060075CB RID: 30155 RVA: 0x0021A025 File Offset: 0x00218225
		internal bool IsRestoringLiveSorting
		{
			get
			{
				return this._isRestoringLiveSorting;
			}
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x0021A02D File Offset: 0x0021822D
		public int Add(object value)
		{
			this.Insert(this.Count, value);
			return this.Count;
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x0021A042 File Offset: 0x00218242
		public void Clear()
		{
			this.ForEach(delegate(LiveShapingItem x)
			{
				this.ClearItem(x);
			});
			this._root = new LiveShapingTree(this);
		}

		// Token: 0x060075CE RID: 30158 RVA: 0x0021A062 File Offset: 0x00218262
		public bool Contains(object value)
		{
			return this.IndexOf(value) >= 0;
		}

		// Token: 0x060075CF RID: 30159 RVA: 0x0021A074 File Offset: 0x00218274
		public int IndexOf(object value)
		{
			int result = 0;
			this.ForEachUntil(delegate(LiveShapingItem x)
			{
				if (ItemsControl.EqualsEx(value, x.Item))
				{
					return true;
				}
				int result;
				result++;
				result = result;
				return false;
			});
			if (result >= this.Count)
			{
				return -1;
			}
			return result;
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x0021A0BD File Offset: 0x002182BD
		public void Insert(int index, object value)
		{
			this._root.Insert(index, new LiveShapingItem(value, this, false, null, false));
		}

		// Token: 0x17001C0E RID: 7182
		// (get) Token: 0x060075D1 RID: 30161 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C0F RID: 7183
		// (get) Token: 0x060075D2 RID: 30162 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060075D3 RID: 30163 RVA: 0x0021A0D8 File Offset: 0x002182D8
		public void Remove(object value)
		{
			int num = this.IndexOf(value);
			if (num >= 0)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x060075D4 RID: 30164 RVA: 0x0021A0F8 File Offset: 0x002182F8
		public void RemoveAt(int index)
		{
			LiveShapingItem liveShapingItem = this._root[index];
			this._root.RemoveAt(index);
			this.ClearItem(liveShapingItem);
			liveShapingItem.IsDeleted = true;
		}

		// Token: 0x17001C10 RID: 7184
		public object this[int index]
		{
			get
			{
				return this._root[index].Item;
			}
			set
			{
				this._root.ReplaceAt(index, value);
			}
		}

		// Token: 0x060075D7 RID: 30167 RVA: 0x0003E264 File Offset: 0x0003C464
		public void CopyTo(Array array, int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x060075D8 RID: 30168 RVA: 0x0021A14E File Offset: 0x0021834E
		public int Count
		{
			get
			{
				return this._root.Count;
			}
		}

		// Token: 0x17001C12 RID: 7186
		// (get) Token: 0x060075D9 RID: 30169 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001C13 RID: 7187
		// (get) Token: 0x060075DA RID: 30170 RVA: 0x0000C238 File Offset: 0x0000A438
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060075DB RID: 30171 RVA: 0x0021A15B File Offset: 0x0021835B
		public IEnumerator GetEnumerator()
		{
			return new LiveShapingList.ItemEnumerator(this._root.GetEnumerator());
		}

		// Token: 0x060075DC RID: 30172 RVA: 0x0021A16D File Offset: 0x0021836D
		private void ForEach(Action<LiveShapingItem> action)
		{
			this._root.ForEach(action);
		}

		// Token: 0x060075DD RID: 30173 RVA: 0x0021A17B File Offset: 0x0021837B
		private void ForEachUntil(Func<LiveShapingItem, bool> action)
		{
			this._root.ForEachUntil(action);
		}

		// Token: 0x060075DE RID: 30174 RVA: 0x00016748 File Offset: 0x00014948
		internal bool VerifyLiveSorting(LiveShapingItem lsi)
		{
			return true;
		}

		// Token: 0x0400383C RID: 14396
		private ICollectionViewLiveShaping _view;

		// Token: 0x0400383D RID: 14397
		private LiveShapingList.DPFromPath _dpFromPath;

		// Token: 0x0400383E RID: 14398
		private LivePropertyInfo[] _compInfos;

		// Token: 0x0400383F RID: 14399
		private LivePropertyInfo[] _sortInfos;

		// Token: 0x04003840 RID: 14400
		private LivePropertyInfo[] _filterInfos;

		// Token: 0x04003841 RID: 14401
		private LivePropertyInfo[] _groupInfos;

		// Token: 0x04003842 RID: 14402
		private IComparer _comparer;

		// Token: 0x04003843 RID: 14403
		private LiveShapingTree _root;

		// Token: 0x04003844 RID: 14404
		private LiveShapingTree _filterRoot;

		// Token: 0x04003845 RID: 14405
		private List<LiveShapingItem> _sortDirtyItems;

		// Token: 0x04003846 RID: 14406
		private List<LiveShapingItem> _filterDirtyItems;

		// Token: 0x04003847 RID: 14407
		private List<LiveShapingItem> _groupDirtyItems;

		// Token: 0x04003848 RID: 14408
		private bool _isRestoringLiveSorting;

		// Token: 0x04003849 RID: 14409
		private bool _isCustomSorting;

		// Token: 0x0400384A RID: 14410
		private static List<DependencyProperty> s_dpList = new List<DependencyProperty>();

		// Token: 0x0400384B RID: 14411
		private static object s_Sync = new object();

		// Token: 0x02000B50 RID: 2896
		private class DPFromPath : Dictionary<string, DependencyProperty>
		{
			// Token: 0x06008DB3 RID: 36275 RVA: 0x00259F11 File Offset: 0x00258111
			public void BeginReset()
			{
				this._unusedKeys = new List<string>(base.Keys);
				this._dpIndex = 0;
			}

			// Token: 0x06008DB4 RID: 36276 RVA: 0x00259F2C File Offset: 0x0025812C
			public void EndReset()
			{
				foreach (string key in this._unusedKeys)
				{
					base.Remove(key);
				}
				this._unusedKeys = null;
			}

			// Token: 0x06008DB5 RID: 36277 RVA: 0x00259F88 File Offset: 0x00258188
			public DependencyProperty GetDP(string path)
			{
				DependencyProperty dependencyProperty;
				if (base.TryGetValue(path, out dependencyProperty))
				{
					this._unusedKeys.Remove(path);
					return dependencyProperty;
				}
				ICollection<DependencyProperty> values = base.Values;
				while (this._dpIndex < LiveShapingList.s_dpList.Count)
				{
					dependencyProperty = LiveShapingList.s_dpList[this._dpIndex];
					if (!values.Contains(dependencyProperty))
					{
						base[path] = dependencyProperty;
						return dependencyProperty;
					}
					this._dpIndex++;
				}
				object s_Sync = LiveShapingList.s_Sync;
				lock (s_Sync)
				{
					dependencyProperty = DependencyProperty.RegisterAttached(string.Format(TypeConverterHelper.InvariantEnglishUS, "LiveSortingTargetProperty{0}", new object[]
					{
						LiveShapingList.s_dpList.Count
					}), typeof(object), typeof(LiveShapingList));
					LiveShapingList.s_dpList.Add(dependencyProperty);
				}
				base[path] = dependencyProperty;
				return dependencyProperty;
			}

			// Token: 0x04004AF1 RID: 19185
			private List<string> _unusedKeys;

			// Token: 0x04004AF2 RID: 19186
			private int _dpIndex;
		}

		// Token: 0x02000B51 RID: 2897
		private class ItemEnumerator : IEnumerator
		{
			// Token: 0x06008DB7 RID: 36279 RVA: 0x0025A088 File Offset: 0x00258288
			public ItemEnumerator(IEnumerator<LiveShapingItem> ie)
			{
				this._ie = ie;
			}

			// Token: 0x06008DB8 RID: 36280 RVA: 0x0025A097 File Offset: 0x00258297
			void IEnumerator.Reset()
			{
				this._ie.Reset();
			}

			// Token: 0x06008DB9 RID: 36281 RVA: 0x0025A0A4 File Offset: 0x002582A4
			bool IEnumerator.MoveNext()
			{
				return this._ie.MoveNext();
			}

			// Token: 0x17001F81 RID: 8065
			// (get) Token: 0x06008DBA RID: 36282 RVA: 0x0025A0B1 File Offset: 0x002582B1
			object IEnumerator.Current
			{
				get
				{
					return this._ie.Current.Item;
				}
			}

			// Token: 0x04004AF3 RID: 19187
			private IEnumerator<LiveShapingItem> _ie;
		}
	}
}
