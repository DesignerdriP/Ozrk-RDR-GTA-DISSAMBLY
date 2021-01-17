using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;
using MS.Internal.Utility;

namespace System.Windows.Controls
{
	/// <summary>Holds the list of items that constitute the content of an <see cref="T:System.Windows.Controls.ItemsControl" />.</summary>
	// Token: 0x020004EF RID: 1263
	[Localizability(LocalizationCategory.Ignore)]
	public sealed class ItemCollection : CollectionView, IList, ICollection, IEnumerable, IEditableCollectionViewAddNewItem, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties, IWeakEventListener
	{
		// Token: 0x06004F48 RID: 20296 RVA: 0x00163D3E File Offset: 0x00161F3E
		internal ItemCollection(DependencyObject modelParent) : base(EmptyEnumerable.Instance, false)
		{
			this._modelParent = new WeakReference(modelParent);
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x00163D60 File Offset: 0x00161F60
		internal ItemCollection(FrameworkElement modelParent, int capacity) : base(EmptyEnumerable.Instance, false)
		{
			this._defaultCapacity = capacity;
			this._modelParent = new WeakReference(modelParent);
		}

		/// <summary>Sets the first item in the view as the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate that the resulting <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F4A RID: 20298 RVA: 0x00163D89 File Offset: 0x00161F89
		public override bool MoveCurrentToFirst()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToFirst();
		}

		/// <summary>Sets the item after the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> in the view as the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate that the resulting <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F4B RID: 20299 RVA: 0x00163DA6 File Offset: 0x00161FA6
		public override bool MoveCurrentToNext()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToNext();
		}

		/// <summary>Sets the item before the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> in the view as the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" />  to indicate that the resulting <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F4C RID: 20300 RVA: 0x00163DC3 File Offset: 0x00161FC3
		public override bool MoveCurrentToPrevious()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToPrevious();
		}

		/// <summary>Sets the last item in the view as the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" />.</summary>
		/// <returns>
		///     <see langword="true" /> to indicate that the resulting <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F4D RID: 20301 RVA: 0x00163DE0 File Offset: 0x00161FE0
		public override bool MoveCurrentToLast()
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToLast();
		}

		/// <summary>Sets the specified item in the collection as the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" />.</summary>
		/// <param name="item">The item to set as the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" />.</param>
		/// <returns>
		///     <see langword="true" /> to indicate that the resulting <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F4E RID: 20302 RVA: 0x00163DFD File Offset: 0x00161FFD
		public override bool MoveCurrentTo(object item)
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentTo(item);
		}

		/// <summary>Sets the item at the specified index to be the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> in the view.</summary>
		/// <param name="position">The zero-based index of the item to set as the <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" />.</param>
		/// <returns>
		///     <see langword="true" /> to indicate that the resulting <see cref="P:System.Windows.Controls.ItemCollection.CurrentItem" /> is an item within the view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F4F RID: 20303 RVA: 0x00163E1B File Offset: 0x0016201B
		public override bool MoveCurrentToPosition(int position)
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.MoveCurrentToPosition(position);
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00163E39 File Offset: 0x00162039
		protected override IEnumerator GetEnumerator()
		{
			if (!this.EnsureCollectionView())
			{
				return EmptyEnumerator.Instance;
			}
			return ((IEnumerable)this._collectionView).GetEnumerator();
		}

		/// <summary>Adds an item to the <see cref="T:System.Windows.Controls.ItemCollection" />.</summary>
		/// <param name="newItem">The item to add to the collection.</param>
		/// <returns>The zero-based index at which the object is added or -1 if the item cannot be added.</returns>
		/// <exception cref="T:System.InvalidOperationException">The item to add already has a different logical parent. </exception>
		/// <exception cref="T:System.InvalidOperationException">The collection is in ItemsSource mode.</exception>
		// Token: 0x06004F51 RID: 20305 RVA: 0x00163E54 File Offset: 0x00162054
		public int Add(object newItem)
		{
			this.CheckIsUsingInnerView();
			int result = this._internalView.Add(newItem);
			this.ModelParent.SetValue(ItemsControl.HasItemsPropertyKey, BooleanBoxes.TrueBox);
			return result;
		}

		/// <summary>Clears the collection and releases the references on all items currently in the collection.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.ItemCollection" /> is in <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> mode. (When the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> property is set, the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection will be made read-only and fixed-size.)</exception>
		// Token: 0x06004F52 RID: 20306 RVA: 0x00163E8C File Offset: 0x0016208C
		public void Clear()
		{
			this.VerifyRefreshNotDeferred();
			if (this.IsUsingItemsSource)
			{
				throw new InvalidOperationException(SR.Get("ItemsSourceInUse"));
			}
			if (this._internalView != null)
			{
				this._internalView.Clear();
			}
			this.ModelParent.ClearValue(ItemsControl.HasItemsPropertyKey);
		}

		/// <summary>Returns a value that indicates whether the specified item is in this view.</summary>
		/// <param name="containItem">The object to check.</param>
		/// <returns>
		///     <see langword="true" /> to indicate that the item belongs to this collection and passes the active filter; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F53 RID: 20307 RVA: 0x00163EDA File Offset: 0x001620DA
		public override bool Contains(object containItem)
		{
			if (!this.EnsureCollectionView())
			{
				return false;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.Contains(containItem);
		}

		/// <summary>Copies the elements of the collection to an array, starting at a particular array index. </summary>
		/// <param name="array">The destination array to copy to.</param>
		/// <param name="index">The zero-based index in the destination array.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="array" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The destination <paramref name="array" /> is multidimensional.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <paramref name="index" /> parameter is less than 0.</exception>
		// Token: 0x06004F54 RID: 20308 RVA: 0x00163EF8 File Offset: 0x001620F8
		public void CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException(SR.Get("BadTargetArray"), "array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (!this.EnsureCollectionView())
			{
				return;
			}
			this.VerifyRefreshNotDeferred();
			IndexedEnumerable.CopyTo(this._collectionView, array, index);
		}

		/// <summary>Returns the index in this collection where the specified item is located. </summary>
		/// <param name="item">The object to look for in the collection.</param>
		/// <returns>The index of the item in the collection, or -1 if the item does not exist in the collection.</returns>
		// Token: 0x06004F55 RID: 20309 RVA: 0x00163F5C File Offset: 0x0016215C
		public override int IndexOf(object item)
		{
			if (!this.EnsureCollectionView())
			{
				return -1;
			}
			this.VerifyRefreshNotDeferred();
			return this._collectionView.IndexOf(item);
		}

		/// <summary>Returns the item at the specified zero-based index in this view.</summary>
		/// <param name="index">The zero-based index at which the item is located.</param>
		/// <returns>The item at the specified zero-based index in this view.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is out of range.</exception>
		/// <exception cref="T:System.InvalidOperationException">The collection is uninitialized or the binding on <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> supplied a <see langword="null" /> value.</exception>
		// Token: 0x06004F56 RID: 20310 RVA: 0x00163F7C File Offset: 0x0016217C
		public override object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.VerifyRefreshNotDeferred();
			if (!this.EnsureCollectionView())
			{
				throw new InvalidOperationException(SR.Get("ItemCollectionHasNoCollection"));
			}
			if (this._collectionView == this._internalView && index >= this._internalView.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this._collectionView.GetItemAt(index);
		}

		/// <summary> Inserts an element into the collection at the specified index. </summary>
		/// <param name="insertIndex">The zero-based index at which to insert the item.</param>
		/// <param name="insertItem">The item to insert.</param>
		/// <exception cref="T:System.InvalidOperationException">The item to insert already has a different logical parent. </exception>
		/// <exception cref="T:System.InvalidOperationException">The collection is in ItemsSource mode.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is out of range. </exception>
		// Token: 0x06004F57 RID: 20311 RVA: 0x00163FE9 File Offset: 0x001621E9
		public void Insert(int insertIndex, object insertItem)
		{
			this.CheckIsUsingInnerView();
			this._internalView.Insert(insertIndex, insertItem);
			this.ModelParent.SetValue(ItemsControl.HasItemsPropertyKey, BooleanBoxes.TrueBox);
		}

		/// <summary>Removes the specified item reference from the collection or view.</summary>
		/// <param name="removeItem">The object to remove.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.ItemCollection" /> is read-only because it is in <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> mode or if DeferRefresh is in effect.</exception>
		// Token: 0x06004F58 RID: 20312 RVA: 0x00164013 File Offset: 0x00162213
		public void Remove(object removeItem)
		{
			this.CheckIsUsingInnerView();
			this._internalView.Remove(removeItem);
			if (this.IsEmpty)
			{
				this.ModelParent.ClearValue(ItemsControl.HasItemsPropertyKey);
			}
		}

		/// <summary>Removes the item at the specified index of the collection or view.</summary>
		/// <param name="removeIndex">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Controls.ItemCollection" /> is read-only because it is in <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> mode or if DeferRefresh is in effect.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is out of range.</exception>
		// Token: 0x06004F59 RID: 20313 RVA: 0x0016403F File Offset: 0x0016223F
		public void RemoveAt(int removeIndex)
		{
			this.CheckIsUsingInnerView();
			this._internalView.RemoveAt(removeIndex);
			if (this.IsEmpty)
			{
				this.ModelParent.ClearValue(ItemsControl.HasItemsPropertyKey);
			}
		}

		/// <summary>Returns a value that indicates whether the specified item belongs to this view.</summary>
		/// <param name="item">The object to test.</param>
		/// <returns>
		///     <see langword="true" /> to indicate that the specified item belongs to this view or there is no filter set on this collection view; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004F5A RID: 20314 RVA: 0x0016406B File Offset: 0x0016226B
		public override bool PassesFilter(object item)
		{
			return !this.EnsureCollectionView() || this._collectionView.PassesFilter(item);
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00164083 File Offset: 0x00162283
		protected override void RefreshOverride()
		{
			if (this._collectionView != null)
			{
				if (this._collectionView.NeedsRefresh)
				{
					this._collectionView.Refresh();
					return;
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		/// <summary>Gets the number of records in the collection. </summary>
		/// <returns>The number of items in the collection or 0 if the collection is uninitialized or if there is no collection in the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> mode.</returns>
		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x06004F5C RID: 20316 RVA: 0x001640B2 File Offset: 0x001622B2
		public override int Count
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return 0;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.Count;
			}
		}

		/// <summary>Gets a value that indicates whether the resulting (filtered) view is empty.</summary>
		/// <returns>
		///     <see langword="true" /> if the resulting view is empty; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06004F5D RID: 20317 RVA: 0x001640CF File Offset: 0x001622CF
		public override bool IsEmpty
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return true;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.IsEmpty;
			}
		}

		/// <summary>Gets or sets the item at the given zero-based index.</summary>
		/// <param name="index">The zero-based index of the item.</param>
		/// <returns>The object retrieved or the object that is being set to the specified index.</returns>
		/// <exception cref="T:System.InvalidOperationException">The collection is uninitialized, or the item to set already has a different logical parent, or the collection is in <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> mode.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The index is out of range.</exception>
		// Token: 0x17001341 RID: 4929
		public object this[int index]
		{
			get
			{
				return this.GetItemAt(index);
			}
			set
			{
				this.CheckIsUsingInnerView();
				if (index < 0 || index >= this._internalView.Count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				this._internalView[index] = value;
			}
		}

		/// <summary>Gets the unsorted and unfiltered collection that underlies this collection view. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerable" /> object that is the underlying collection or the user-provided <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> collection.</returns>
		// Token: 0x17001342 RID: 4930
		// (get) Token: 0x06004F60 RID: 20320 RVA: 0x00164127 File Offset: 0x00162327
		public override IEnumerable SourceCollection
		{
			get
			{
				if (this.IsUsingItemsSource)
				{
					return this.ItemsSource;
				}
				this.EnsureInternalView();
				return this;
			}
		}

		/// <summary>Gets a value that indicates whether the collection needs to be refreshed.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection needs to be refreshed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001343 RID: 4931
		// (get) Token: 0x06004F61 RID: 20321 RVA: 0x0016413F File Offset: 0x0016233F
		public override bool NeedsRefresh
		{
			get
			{
				return this.EnsureCollectionView() && this._collectionView.NeedsRefresh;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describe how the items in the collection are sorted in the view.</summary>
		/// <returns>A collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describe how the items in the collection are sorted in the view.</returns>
		// Token: 0x17001344 RID: 4932
		// (get) Token: 0x06004F62 RID: 20322 RVA: 0x00164158 File Offset: 0x00162358
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				if (this.MySortDescriptions == null)
				{
					this.MySortDescriptions = new SortDescriptionCollection();
					if (this._collectionView != null)
					{
						this.CloneList(this.MySortDescriptions, this._collectionView.SortDescriptions);
					}
					((INotifyCollectionChanged)this.MySortDescriptions).CollectionChanged += this.SortDescriptionsChanged;
				}
				return this.MySortDescriptions;
			}
		}

		/// <summary>Gets a value that indicates whether this collection view supports sorting.</summary>
		/// <returns>
		///     <see langword="true" /> if this view support sorting; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x06004F63 RID: 20323 RVA: 0x001641B4 File Offset: 0x001623B4
		public override bool CanSort
		{
			get
			{
				return !this.EnsureCollectionView() || this._collectionView.CanSort;
			}
		}

		/// <summary>Gets or sets a callback used to determine if an item is suitable for inclusion in the view.</summary>
		/// <returns>A method used to determine if an item is suitable for inclusion in the view.</returns>
		/// <exception cref="T:System.NotSupportedException">Filtering is not supported.</exception>
		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x06004F64 RID: 20324 RVA: 0x001641CB File Offset: 0x001623CB
		// (set) Token: 0x06004F65 RID: 20325 RVA: 0x001641E7 File Offset: 0x001623E7
		public override Predicate<object> Filter
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return this.MyFilter;
				}
				return this._collectionView.Filter;
			}
			set
			{
				this.MyFilter = value;
				if (this._collectionView != null)
				{
					this._collectionView.Filter = value;
				}
			}
		}

		/// <summary>Gets a value that indicates whether this collection view supports filtering.</summary>
		/// <returns>
		///     <see langword="true" /> if this view supports filtering; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x06004F66 RID: 20326 RVA: 0x00164204 File Offset: 0x00162404
		public override bool CanFilter
		{
			get
			{
				return !this.EnsureCollectionView() || this._collectionView.CanFilter;
			}
		}

		/// <summary>Gets a value that indicates whether this collection view supports grouping.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection supports grouping; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x06004F67 RID: 20327 RVA: 0x0016421B File Offset: 0x0016241B
		public override bool CanGroup
		{
			get
			{
				return this.EnsureCollectionView() && this._collectionView.CanGroup;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.ComponentModel.GroupDescription" /> objects that defines how to group the items.</summary>
		/// <returns>An <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> of <see cref="T:System.ComponentModel.GroupDescription" /> objects. The collection is indexed by the group levels.</returns>
		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x00164234 File Offset: 0x00162434
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				if (this.MyGroupDescriptions == null)
				{
					this.MyGroupDescriptions = new ObservableCollection<GroupDescription>();
					if (this._collectionView != null)
					{
						this.CloneList(this.MyGroupDescriptions, this._collectionView.GroupDescriptions);
					}
					((INotifyCollectionChanged)this.MyGroupDescriptions).CollectionChanged += this.GroupDescriptionsChanged;
				}
				return this.MyGroupDescriptions;
			}
		}

		/// <summary>Gets the top-level groups that are constructed according to the <see cref="P:System.Windows.Controls.ItemCollection.GroupDescriptions" />.</summary>
		/// <returns>The top-level groups that are constructed according to the <see cref="P:System.Windows.Controls.ItemCollection.GroupDescriptions" />. The default value is <see langword="null" />.</returns>
		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x06004F69 RID: 20329 RVA: 0x00164290 File Offset: 0x00162490
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return null;
				}
				return this._collectionView.Groups;
			}
		}

		/// <summary>Enters a defer cycle that you can use to merge changes to the view and delay automatic refresh.</summary>
		/// <returns>An <see cref="T:System.IDisposable" /> object that you can use to dispose of the calling object.</returns>
		// Token: 0x06004F6A RID: 20330 RVA: 0x001642A7 File Offset: 0x001624A7
		public override IDisposable DeferRefresh()
		{
			if (this._deferLevel == 0 && this._collectionView != null)
			{
				this._deferInnerRefresh = this._collectionView.DeferRefresh();
			}
			this._deferLevel++;
			return new ItemCollection.DeferHelper(this);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x06004F6B RID: 20331 RVA: 0x0000B02A File Offset: 0x0000922A
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</returns>
		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x06004F6C RID: 20332 RVA: 0x001642DE File Offset: 0x001624DE
		object ICollection.SyncRoot
		{
			get
			{
				if (this.IsUsingItemsSource)
				{
					throw new NotSupportedException(SR.Get("ItemCollectionShouldUseInnerSyncRoot"));
				}
				return this._internalView.SyncRoot;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x06004F6D RID: 20333 RVA: 0x00164303 File Offset: 0x00162503
		bool IList.IsFixedSize
		{
			get
			{
				return this.IsUsingItemsSource;
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read only; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x06004F6E RID: 20334 RVA: 0x00164303 File Offset: 0x00162503
		bool IList.IsReadOnly
		{
			get
			{
				return this.IsUsingItemsSource;
			}
		}

		/// <summary>Gets the ordinal position of the current item within the view.</summary>
		/// <returns>The ordinal position of the current item within the view or -1 if the collection is uninitialized or if there is no collection in the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> mode.</returns>
		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06004F6F RID: 20335 RVA: 0x0016430B File Offset: 0x0016250B
		public override int CurrentPosition
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return -1;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.CurrentPosition;
			}
		}

		/// <summary>Gets the current item in the view.</summary>
		/// <returns>The current object in the view or <see langword="null" /> if the collection is uninitialized or if there is no collection in the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> mode.By default, the first item of the collection starts as the current item.</returns>
		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06004F70 RID: 20336 RVA: 0x00164328 File Offset: 0x00162528
		public override object CurrentItem
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return null;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.CurrentItem;
			}
		}

		/// <summary>Gets a value that indicates whether the current item of the view is beyond the end of the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if the current item of the view is beyond the end of the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06004F71 RID: 20337 RVA: 0x00164345 File Offset: 0x00162545
		public override bool IsCurrentAfterLast
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return false;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.IsCurrentAfterLast;
			}
		}

		/// <summary>Gets a value that indicates whether the current item of the view is beyond the beginning of the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if the current item of the view is beyond the beginning of the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06004F72 RID: 20338 RVA: 0x00164362 File Offset: 0x00162562
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				if (!this.EnsureCollectionView())
				{
					return false;
				}
				this.VerifyRefreshNotDeferred();
				return this._collectionView.IsCurrentBeforeFirst;
			}
		}

		/// <summary>Gets or sets the position of the new item placeholder in the collection view.</summary>
		/// <returns>One of the enumeration values that specifies the position of the new item placeholder in the collection view.</returns>
		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06004F73 RID: 20339 RVA: 0x00164380 File Offset: 0x00162580
		// (set) Token: 0x06004F74 RID: 20340 RVA: 0x001643A4 File Offset: 0x001625A4
		NewItemPlaceholderPosition IEditableCollectionView.NewItemPlaceholderPosition
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.NewItemPlaceholderPosition;
				}
				return NewItemPlaceholderPosition.None;
			}
			set
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					editableCollectionView.NewItemPlaceholderPosition = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
				{
					"NewItemPlaceholderPosition"
				}));
			}
		}

		/// <summary>Gets a value that indicates whether a new item can be added to the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if a new item can be added to the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06004F75 RID: 20341 RVA: 0x001643E8 File Offset: 0x001625E8
		bool IEditableCollectionView.CanAddNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanAddNew;
			}
		}

		/// <summary>Adds a new item to the collection.</summary>
		/// <returns>The new item that is added to the collection.</returns>
		// Token: 0x06004F76 RID: 20342 RVA: 0x0016440C File Offset: 0x0016260C
		object IEditableCollectionView.AddNew()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				return editableCollectionView.AddNew();
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNew"
			}));
		}

		/// <summary>Ends the add transaction and saves the pending new item.</summary>
		// Token: 0x06004F77 RID: 20343 RVA: 0x0016444C File Offset: 0x0016264C
		void IEditableCollectionView.CommitNew()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CommitNew();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CommitNew"
			}));
		}

		/// <summary>Ends the add transaction and discards the pending new item.</summary>
		// Token: 0x06004F78 RID: 20344 RVA: 0x0016448C File Offset: 0x0016268C
		void IEditableCollectionView.CancelNew()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CancelNew();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CancelNew"
			}));
		}

		/// <summary>Gets a value that indicates whether an add transaction is in progress.</summary>
		/// <returns>
		///     <see langword="true" /> if an add transaction is in progress; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06004F79 RID: 20345 RVA: 0x001644CC File Offset: 0x001626CC
		bool IEditableCollectionView.IsAddingNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsAddingNew;
			}
		}

		/// <summary>Gets the item that is being added during the current add transaction.</summary>
		/// <returns>The item that is being added if <see cref="P:System.ComponentModel.IEditableCollectionView.IsAddingNew" /> is <see langword="true" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06004F7A RID: 20346 RVA: 0x001644F0 File Offset: 0x001626F0
		object IEditableCollectionView.CurrentAddItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentAddItem;
				}
				return null;
			}
		}

		/// <summary>Gets a value that indicates whether an item can be removed from the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if an item can be removed from the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06004F7B RID: 20347 RVA: 0x00164514 File Offset: 0x00162714
		bool IEditableCollectionView.CanRemove
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanRemove;
			}
		}

		/// <summary>Removes the item at the specified position from the collection.</summary>
		/// <param name="index">The position of the item to remove.</param>
		// Token: 0x06004F7C RID: 20348 RVA: 0x00164538 File Offset: 0x00162738
		void IEditableCollectionView.RemoveAt(int index)
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.RemoveAt(index);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"RemoveAt"
			}));
		}

		/// <summary>Removes the specified item from the collection.</summary>
		/// <param name="item">The item to remove.</param>
		// Token: 0x06004F7D RID: 20349 RVA: 0x0016457C File Offset: 0x0016277C
		void IEditableCollectionView.Remove(object item)
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.Remove(item);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"Remove"
			}));
		}

		/// <summary>Begins an edit transaction of the specified item.</summary>
		/// <param name="item">The item to edit.</param>
		// Token: 0x06004F7E RID: 20350 RVA: 0x001645C0 File Offset: 0x001627C0
		void IEditableCollectionView.EditItem(object item)
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.EditItem(item);
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"EditItem"
			}));
		}

		/// <summary>Ends the edit transaction and saves the pending changes.</summary>
		// Token: 0x06004F7F RID: 20351 RVA: 0x00164604 File Offset: 0x00162804
		void IEditableCollectionView.CommitEdit()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CommitEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CommitEdit"
			}));
		}

		/// <summary>Ends the edit transaction and, if possible, restores the original value to the item.</summary>
		// Token: 0x06004F80 RID: 20352 RVA: 0x00164644 File Offset: 0x00162844
		void IEditableCollectionView.CancelEdit()
		{
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				editableCollectionView.CancelEdit();
				return;
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"CancelEdit"
			}));
		}

		/// <summary>Gets a value that indicates whether the collection view can discard pending changes and restore the original values of an edited object.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view can discard pending changes and restore the original values of an edited object; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06004F81 RID: 20353 RVA: 0x00164684 File Offset: 0x00162884
		bool IEditableCollectionView.CanCancelEdit
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanCancelEdit;
			}
		}

		/// <summary>Gets a value that indicates whether an edit transaction is in progress.</summary>
		/// <returns>
		///     <see langword="true" /> if an edit transaction is in progress; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x06004F82 RID: 20354 RVA: 0x001646A8 File Offset: 0x001628A8
		bool IEditableCollectionView.IsEditingItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsEditingItem;
			}
		}

		/// <summary>Gets the item in the collection that is being edited.</summary>
		/// <returns>The item in the collection that is being edited if <see cref="P:System.ComponentModel.IEditableCollectionView.IsEditingItem" /> is <see langword="true" />; otherwise, <see langword="null" />.</returns>
		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x06004F83 RID: 20355 RVA: 0x001646CC File Offset: 0x001628CC
		object IEditableCollectionView.CurrentEditItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentEditItem;
				}
				return null;
			}
		}

		/// <summary>Gets a value that indicates whether a specified object can be added to the collection.</summary>
		/// <returns>
		///     <see langword="true" /> if a specified object can be added to the collection; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06004F84 RID: 20356 RVA: 0x001646F0 File Offset: 0x001628F0
		bool IEditableCollectionViewAddNewItem.CanAddNewItem
		{
			get
			{
				IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this._collectionView as IEditableCollectionViewAddNewItem;
				return editableCollectionViewAddNewItem != null && editableCollectionViewAddNewItem.CanAddNewItem;
			}
		}

		/// <summary>Adds the specified object to the collection.</summary>
		/// <param name="newItem">The object to add to the collection.</param>
		/// <returns>The object that was added to the collection.</returns>
		// Token: 0x06004F85 RID: 20357 RVA: 0x00164714 File Offset: 0x00162914
		object IEditableCollectionViewAddNewItem.AddNewItem(object newItem)
		{
			IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this._collectionView as IEditableCollectionViewAddNewItem;
			if (editableCollectionViewAddNewItem != null)
			{
				return editableCollectionViewAddNewItem.AddNewItem(newItem);
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNewItem"
			}));
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning sorting data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view supports turning live sorting on or off; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06004F86 RID: 20358 RVA: 0x00164758 File Offset: 0x00162958
		public bool CanChangeLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning filtering data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view supports turning live filtering on or off; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06004F87 RID: 20359 RVA: 0x0016477C File Offset: 0x0016297C
		public bool CanChangeLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning grouping data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view supports turning live grouping on or off; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06004F88 RID: 20360 RVA: 0x001647A0 File Offset: 0x001629A0
		public bool CanChangeLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping;
			}
		}

		/// <summary>Gets or sets a value that indicates whether sorting in real time is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if sorting data in real time is enabled; <see langword="false" /> if live sorting is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live sorting.</returns>
		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06004F89 RID: 20361 RVA: 0x001647C4 File Offset: 0x001629C4
		// (set) Token: 0x06004F8A RID: 20362 RVA: 0x001647F0 File Offset: 0x001629F0
		public bool? IsLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveSorting;
			}
			set
			{
				this.MyIsLiveSorting = value;
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting)
				{
					collectionViewLiveShaping.IsLiveSorting = value;
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether filtering data in real time is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if filtering data in real time is enabled; <see langword="false" /> if live filtering is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live filtering.</returns>
		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06004F8B RID: 20363 RVA: 0x00164824 File Offset: 0x00162A24
		// (set) Token: 0x06004F8C RID: 20364 RVA: 0x00164850 File Offset: 0x00162A50
		public bool? IsLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveFiltering;
			}
			set
			{
				this.MyIsLiveFiltering = value;
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering)
				{
					collectionViewLiveShaping.IsLiveFiltering = value;
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether grouping data in real time is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if grouping data in real time is enabled; <see langword="false" /> if live grouping is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live grouping.</returns>
		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x06004F8D RID: 20365 RVA: 0x00164884 File Offset: 0x00162A84
		// (set) Token: 0x06004F8E RID: 20366 RVA: 0x001648B0 File Offset: 0x00162AB0
		public bool? IsLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveGrouping;
			}
			set
			{
				this.MyIsLiveGrouping = value;
				ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping)
				{
					collectionViewLiveShaping.IsLiveGrouping = value;
				}
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in sorting data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in sorting data in real time.</returns>
		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x06004F8F RID: 20367 RVA: 0x001648E4 File Offset: 0x00162AE4
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this.MyLiveSortingProperties == null)
				{
					this.MyLiveSortingProperties = new ObservableCollection<string>();
					ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
					if (collectionViewLiveShaping != null)
					{
						this.CloneList(this.MyLiveSortingProperties, collectionViewLiveShaping.LiveSortingProperties);
					}
					((INotifyCollectionChanged)this.MyLiveSortingProperties).CollectionChanged += this.LiveSortingChanged;
				}
				return this.MyLiveSortingProperties;
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in filtering data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in filtering data in real time.</returns>
		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06004F90 RID: 20368 RVA: 0x00164944 File Offset: 0x00162B44
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this.MyLiveFilteringProperties == null)
				{
					this.MyLiveFilteringProperties = new ObservableCollection<string>();
					ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
					if (collectionViewLiveShaping != null)
					{
						this.CloneList(this.MyLiveFilteringProperties, collectionViewLiveShaping.LiveFilteringProperties);
					}
					((INotifyCollectionChanged)this.MyLiveFilteringProperties).CollectionChanged += this.LiveFilteringChanged;
				}
				return this.MyLiveFilteringProperties;
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in grouping data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in grouping data in real time.</returns>
		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06004F91 RID: 20369 RVA: 0x001649A4 File Offset: 0x00162BA4
		public ObservableCollection<string> LiveGroupingProperties
		{
			get
			{
				if (this.MyLiveGroupingProperties == null)
				{
					this.MyLiveGroupingProperties = new ObservableCollection<string>();
					ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
					if (collectionViewLiveShaping != null)
					{
						this.CloneList(this.MyLiveGroupingProperties, collectionViewLiveShaping.LiveGroupingProperties);
					}
					((INotifyCollectionChanged)this.MyLiveGroupingProperties).CollectionChanged += this.LiveGroupingChanged;
				}
				return this.MyLiveGroupingProperties;
			}
		}

		/// <summary>Gets a collection that contains information about the properties that are available on the items in a collection.</summary>
		/// <returns>A collection that contains information about the properties that are available on the items in a collection.</returns>
		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06004F92 RID: 20370 RVA: 0x00164A04 File Offset: 0x00162C04
		ReadOnlyCollection<ItemPropertyInfo> IItemProperties.ItemProperties
		{
			get
			{
				IItemProperties itemProperties = this._collectionView as IItemProperties;
				if (itemProperties != null)
				{
					return itemProperties.ItemProperties;
				}
				return null;
			}
		}

		// Token: 0x17001366 RID: 4966
		// (get) Token: 0x06004F93 RID: 20371 RVA: 0x00164A28 File Offset: 0x00162C28
		internal DependencyObject ModelParent
		{
			get
			{
				return (DependencyObject)this._modelParent.Target;
			}
		}

		// Token: 0x17001367 RID: 4967
		// (get) Token: 0x06004F94 RID: 20372 RVA: 0x00164A3A File Offset: 0x00162C3A
		internal FrameworkElement ModelParentFE
		{
			get
			{
				return this.ModelParent as FrameworkElement;
			}
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x00164A48 File Offset: 0x00162C48
		internal void SetItemsSource(IEnumerable value, Func<object, object> GetSourceItem = null)
		{
			if (!this.IsUsingItemsSource && this._internalView != null && this._internalView.RawCount > 0)
			{
				throw new InvalidOperationException(SR.Get("CannotUseItemsSource"));
			}
			this._itemsSource = value;
			this._isUsingItemsSource = true;
			this.SetCollectionView(CollectionViewSource.GetDefaultCollectionView(this._itemsSource, this.ModelParent, GetSourceItem));
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x00164AA9 File Offset: 0x00162CA9
		internal void ClearItemsSource()
		{
			if (this.IsUsingItemsSource)
			{
				this._itemsSource = null;
				this._isUsingItemsSource = false;
				this.SetCollectionView(this._internalView);
			}
		}

		// Token: 0x17001368 RID: 4968
		// (get) Token: 0x06004F97 RID: 20375 RVA: 0x00164ACD File Offset: 0x00162CCD
		internal IEnumerable ItemsSource
		{
			get
			{
				return this._itemsSource;
			}
		}

		// Token: 0x17001369 RID: 4969
		// (get) Token: 0x06004F98 RID: 20376 RVA: 0x00164AD5 File Offset: 0x00162CD5
		internal bool IsUsingItemsSource
		{
			get
			{
				return this._isUsingItemsSource;
			}
		}

		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06004F99 RID: 20377 RVA: 0x00164ADD File Offset: 0x00162CDD
		internal CollectionView CollectionView
		{
			get
			{
				return this._collectionView;
			}
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x00164AE5 File Offset: 0x00162CE5
		internal void BeginInit()
		{
			this._isInitializing = true;
			if (this._collectionView != null)
			{
				this.UnhookCollectionView(this._collectionView);
			}
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x00164B02 File Offset: 0x00162D02
		internal void EndInit()
		{
			this.EnsureCollectionView();
			this._isInitializing = false;
			if (this._collectionView != null)
			{
				this.HookCollectionView(this._collectionView);
				this.Refresh();
			}
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x06004F9C RID: 20380 RVA: 0x00164B2C File Offset: 0x00162D2C
		internal IEnumerator LogicalChildren
		{
			get
			{
				this.EnsureInternalView();
				return this._internalView.LogicalChildren;
			}
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x00164B3F File Offset: 0x00162D3F
		internal override void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._collectionView != null)
			{
				this._collectionView.GetCollectionChangedSources(level + 1, format, sources);
			}
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x06004F9E RID: 20382 RVA: 0x00164B68 File Offset: 0x00162D68
		private new bool IsRefreshDeferred
		{
			get
			{
				return this._deferLevel > 0;
			}
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x00164B74 File Offset: 0x00162D74
		private bool EnsureCollectionView()
		{
			if (this._collectionView == null && !this.IsUsingItemsSource && this._internalView != null)
			{
				if (this._internalView.IsEmpty)
				{
					bool isInitializing = this._isInitializing;
					this._isInitializing = true;
					this.SetCollectionView(this._internalView);
					this._isInitializing = isInitializing;
				}
				else
				{
					this.SetCollectionView(this._internalView);
				}
				if (!this._isInitializing)
				{
					this.HookCollectionView(this._collectionView);
				}
			}
			return this._collectionView != null;
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x00164BF2 File Offset: 0x00162DF2
		private void EnsureInternalView()
		{
			if (this._internalView == null)
			{
				this._internalView = new InnerItemCollectionView(this._defaultCapacity, this);
			}
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x00164C10 File Offset: 0x00162E10
		private void SetCollectionView(CollectionView view)
		{
			if (this._collectionView == view)
			{
				return;
			}
			if (this._collectionView != null)
			{
				if (!this._isInitializing)
				{
					this.UnhookCollectionView(this._collectionView);
				}
				if (this.IsRefreshDeferred)
				{
					this._deferInnerRefresh.Dispose();
					this._deferInnerRefresh = null;
				}
			}
			bool flag = false;
			this._collectionView = view;
			base.InvalidateEnumerableWrapper();
			if (this._collectionView != null)
			{
				this._deferInnerRefresh = this._collectionView.DeferRefresh();
				this.ApplySortFilterAndGroup();
				if (!this._isInitializing)
				{
					this.HookCollectionView(this._collectionView);
				}
				if (!this.IsRefreshDeferred)
				{
					flag = !this._collectionView.NeedsRefresh;
					this._deferInnerRefresh.Dispose();
					this._deferInnerRefresh = null;
				}
			}
			else if (!this.IsRefreshDeferred)
			{
				flag = true;
			}
			if (flag)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
			this.OnPropertyChanged(new PropertyChangedEventArgs("IsLiveSorting"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("IsLiveFiltering"));
			this.OnPropertyChanged(new PropertyChangedEventArgs("IsLiveGrouping"));
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x00164D14 File Offset: 0x00162F14
		private void ApplySortFilterAndGroup()
		{
			if (!this.IsShapingActive)
			{
				return;
			}
			if (this._collectionView.CanSort)
			{
				SortDescriptionCollection master = this.IsSortingSet ? this.MySortDescriptions : this._collectionView.SortDescriptions;
				SortDescriptionCollection clone = this.IsSortingSet ? this._collectionView.SortDescriptions : this.MySortDescriptions;
				using (this.SortDescriptionsMonitor.Enter())
				{
					this.CloneList(clone, master);
				}
			}
			if (this._collectionView.CanFilter && this.MyFilter != null)
			{
				this._collectionView.Filter = this.MyFilter;
			}
			if (this._collectionView.CanGroup)
			{
				ObservableCollection<GroupDescription> master2 = this.IsGroupingSet ? this.MyGroupDescriptions : this._collectionView.GroupDescriptions;
				ObservableCollection<GroupDescription> clone2 = this.IsGroupingSet ? this._collectionView.GroupDescriptions : this.MyGroupDescriptions;
				using (this.GroupDescriptionsMonitor.Enter())
				{
					this.CloneList(clone2, master2);
				}
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				if (this.MyIsLiveSorting != null && collectionViewLiveShaping.CanChangeLiveSorting)
				{
					collectionViewLiveShaping.IsLiveSorting = this.MyIsLiveSorting;
				}
				if (this.MyIsLiveFiltering != null && collectionViewLiveShaping.CanChangeLiveFiltering)
				{
					collectionViewLiveShaping.IsLiveFiltering = this.MyIsLiveFiltering;
				}
				if (this.MyIsLiveGrouping != null && collectionViewLiveShaping.CanChangeLiveGrouping)
				{
					collectionViewLiveShaping.IsLiveGrouping = this.MyIsLiveGrouping;
				}
			}
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x00164EBC File Offset: 0x001630BC
		private void HookCollectionView(CollectionView view)
		{
			CollectionChangedEventManager.AddHandler(view, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnViewCollectionChanged));
			CurrentChangingEventManager.AddHandler(view, new EventHandler<CurrentChangingEventArgs>(this.OnCurrentChanging));
			CurrentChangedEventManager.AddHandler(view, new EventHandler<EventArgs>(this.OnCurrentChanged));
			PropertyChangedEventManager.AddHandler(view, new EventHandler<PropertyChangedEventArgs>(this.OnViewPropertyChanged), string.Empty);
			SortDescriptionCollection sortDescriptions = view.SortDescriptions;
			if (sortDescriptions != null && sortDescriptions != SortDescriptionCollection.Empty)
			{
				CollectionChangedEventManager.AddHandler(sortDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerSortDescriptionsChanged));
			}
			ObservableCollection<GroupDescription> groupDescriptions = view.GroupDescriptions;
			if (groupDescriptions != null)
			{
				CollectionChangedEventManager.AddHandler(groupDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerGroupDescriptionsChanged));
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = view as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				ObservableCollection<string> liveSortingProperties = collectionViewLiveShaping.LiveSortingProperties;
				if (liveSortingProperties != null)
				{
					CollectionChangedEventManager.AddHandler(liveSortingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveSortingChanged));
				}
				ObservableCollection<string> liveFilteringProperties = collectionViewLiveShaping.LiveFilteringProperties;
				if (liveFilteringProperties != null)
				{
					CollectionChangedEventManager.AddHandler(liveFilteringProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveFilteringChanged));
				}
				ObservableCollection<string> liveGroupingProperties = collectionViewLiveShaping.LiveGroupingProperties;
				if (liveGroupingProperties != null)
				{
					CollectionChangedEventManager.AddHandler(liveGroupingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveGroupingChanged));
				}
			}
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x00164FBC File Offset: 0x001631BC
		private void UnhookCollectionView(CollectionView view)
		{
			CollectionChangedEventManager.RemoveHandler(view, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnViewCollectionChanged));
			CurrentChangingEventManager.RemoveHandler(view, new EventHandler<CurrentChangingEventArgs>(this.OnCurrentChanging));
			CurrentChangedEventManager.RemoveHandler(view, new EventHandler<EventArgs>(this.OnCurrentChanged));
			PropertyChangedEventManager.RemoveHandler(view, new EventHandler<PropertyChangedEventArgs>(this.OnViewPropertyChanged), string.Empty);
			SortDescriptionCollection sortDescriptions = view.SortDescriptions;
			if (sortDescriptions != null && sortDescriptions != SortDescriptionCollection.Empty)
			{
				CollectionChangedEventManager.RemoveHandler(sortDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerSortDescriptionsChanged));
			}
			ObservableCollection<GroupDescription> groupDescriptions = view.GroupDescriptions;
			if (groupDescriptions != null)
			{
				CollectionChangedEventManager.RemoveHandler(groupDescriptions, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerGroupDescriptionsChanged));
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = view as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				ObservableCollection<string> liveSortingProperties = collectionViewLiveShaping.LiveSortingProperties;
				if (liveSortingProperties != null)
				{
					CollectionChangedEventManager.RemoveHandler(liveSortingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveSortingChanged));
				}
				ObservableCollection<string> liveFilteringProperties = collectionViewLiveShaping.LiveFilteringProperties;
				if (liveFilteringProperties != null)
				{
					CollectionChangedEventManager.RemoveHandler(liveFilteringProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveFilteringChanged));
				}
				ObservableCollection<string> liveGroupingProperties = collectionViewLiveShaping.LiveGroupingProperties;
				if (liveGroupingProperties != null)
				{
					CollectionChangedEventManager.RemoveHandler(liveGroupingProperties, new EventHandler<NotifyCollectionChangedEventArgs>(this.OnInnerLiveGroupingChanged));
				}
			}
			IEditableCollectionView editableCollectionView = this._collectionView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				if (editableCollectionView.IsAddingNew)
				{
					editableCollectionView.CancelNew();
				}
				if (editableCollectionView.IsEditingItem)
				{
					if (editableCollectionView.CanCancelEdit)
					{
						editableCollectionView.CancelEdit();
						return;
					}
					editableCollectionView.CommitEdit();
				}
			}
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x001650F7 File Offset: 0x001632F7
		private void OnViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.InvalidateEnumerableWrapper();
			this.OnCollectionChanged(e);
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x00165106 File Offset: 0x00163306
		private void OnCurrentChanged(object sender, EventArgs e)
		{
			this.OnCurrentChanged();
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x0016510E File Offset: 0x0016330E
		private void OnCurrentChanging(object sender, CurrentChangingEventArgs e)
		{
			this.OnCurrentChanging(e);
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x00165117 File Offset: 0x00163317
		private void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnPropertyChanged(e);
		}

		/// <summary>Receives events from the centralized event manager.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004FA9 RID: 20393 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x00165120 File Offset: 0x00163320
		private void CheckIsUsingInnerView()
		{
			if (this.IsUsingItemsSource)
			{
				throw new InvalidOperationException(SR.Get("ItemsSourceInUse"));
			}
			this.EnsureInternalView();
			this.EnsureCollectionView();
			this.VerifyRefreshNotDeferred();
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x00165150 File Offset: 0x00163350
		private void EndDefer()
		{
			this._deferLevel--;
			if (this._deferLevel == 0)
			{
				if (this._deferInnerRefresh != null)
				{
					IDisposable deferInnerRefresh = this._deferInnerRefresh;
					this._deferInnerRefresh = null;
					deferInnerRefresh.Dispose();
					return;
				}
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x0016519C File Offset: 0x0016339C
		private new void VerifyRefreshNotDeferred()
		{
			if (this.IsRefreshDeferred)
			{
				throw new InvalidOperationException(SR.Get("NoCheckOrChangeWhenDeferred"));
			}
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x001651B8 File Offset: 0x001633B8
		private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.SortDescriptionsMonitor.Busy)
			{
				return;
			}
			if (this._collectionView != null && this._collectionView.CanSort)
			{
				using (this.SortDescriptionsMonitor.Enter())
				{
					this.SynchronizeCollections<SortDescription>(e, this.MySortDescriptions, this._collectionView.SortDescriptions);
				}
			}
			this.IsSortingSet = true;
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x00165230 File Offset: 0x00163430
		private void OnInnerSortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.SortDescriptionsMonitor.Busy)
			{
				return;
			}
			using (this.SortDescriptionsMonitor.Enter())
			{
				this.SynchronizeCollections<SortDescription>(e, this._collectionView.SortDescriptions, this.MySortDescriptions);
			}
			this.IsSortingSet = false;
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x0016529C File Offset: 0x0016349C
		private void GroupDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.GroupDescriptionsMonitor.Busy)
			{
				return;
			}
			if (this._collectionView != null && this._collectionView.CanGroup)
			{
				using (this.GroupDescriptionsMonitor.Enter())
				{
					this.SynchronizeCollections<GroupDescription>(e, this.MyGroupDescriptions, this._collectionView.GroupDescriptions);
				}
			}
			this.IsGroupingSet = true;
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x00165314 File Offset: 0x00163514
		private void OnInnerGroupDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.GroupDescriptionsMonitor.Busy)
			{
				return;
			}
			using (this.GroupDescriptionsMonitor.Enter())
			{
				this.SynchronizeCollections<GroupDescription>(e, this._collectionView.GroupDescriptions, this.MyGroupDescriptions);
			}
			this.IsGroupingSet = false;
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x00165380 File Offset: 0x00163580
		private void LiveSortingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.LiveSortingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveSortingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, this.MyLiveSortingProperties, collectionViewLiveShaping.LiveSortingProperties);
				}
			}
			this.IsLiveSortingSet = true;
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x001653EC File Offset: 0x001635EC
		private void OnInnerLiveSortingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.LiveSortingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveSortingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, collectionViewLiveShaping.LiveSortingProperties, this.MyLiveSortingProperties);
				}
			}
			this.IsLiveSortingSet = false;
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x00165460 File Offset: 0x00163660
		private void LiveFilteringChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.LiveFilteringMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveFilteringMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, this.MyLiveFilteringProperties, collectionViewLiveShaping.LiveFilteringProperties);
				}
			}
			this.IsLiveFilteringSet = true;
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x001654CC File Offset: 0x001636CC
		private void OnInnerLiveFilteringChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.LiveFilteringMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveFilteringMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, collectionViewLiveShaping.LiveFilteringProperties, this.MyLiveFilteringProperties);
				}
			}
			this.IsLiveFilteringSet = false;
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x00165540 File Offset: 0x00163740
		private void LiveGroupingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.LiveGroupingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveGroupingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, this.MyLiveGroupingProperties, collectionViewLiveShaping.LiveGroupingProperties);
				}
			}
			this.IsLiveGroupingSet = true;
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x001655AC File Offset: 0x001637AC
		private void OnInnerLiveGroupingChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!this.IsShapingActive || this.LiveGroupingMonitor.Busy)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = this._collectionView as ICollectionViewLiveShaping;
			if (collectionViewLiveShaping != null)
			{
				using (this.LiveGroupingMonitor.Enter())
				{
					this.SynchronizeCollections<string>(e, collectionViewLiveShaping.LiveGroupingProperties, this.MyLiveGroupingProperties);
				}
			}
			this.IsLiveGroupingSet = false;
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x00165620 File Offset: 0x00163820
		private void SynchronizeCollections<T>(NotifyCollectionChangedEventArgs e, Collection<T> origin, Collection<T> clone)
		{
			if (clone == null)
			{
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (clone.Count + e.NewItems.Count == origin.Count)
				{
					for (int i = 0; i < e.NewItems.Count; i++)
					{
						clone.Insert(e.NewStartingIndex + i, (T)((object)e.NewItems[i]));
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (clone.Count - e.OldItems.Count == origin.Count)
				{
					for (int j = 0; j < e.OldItems.Count; j++)
					{
						clone.RemoveAt(e.OldStartingIndex);
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (clone.Count == origin.Count)
				{
					for (int k = 0; k < e.OldItems.Count; k++)
					{
						clone[e.OldStartingIndex + k] = (T)((object)e.NewItems[k]);
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Move:
				if (clone.Count == origin.Count)
				{
					if (e.NewItems.Count == 1)
					{
						clone.RemoveAt(e.OldStartingIndex);
						clone.Insert(e.NewStartingIndex, (T)((object)e.NewItems[0]));
						return;
					}
					for (int l = 0; l < e.OldItems.Count; l++)
					{
						clone.RemoveAt(e.OldStartingIndex);
					}
					for (int m = 0; m < e.NewItems.Count; m++)
					{
						clone.Insert(e.NewStartingIndex + m, (T)((object)e.NewItems[m]));
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Reset:
				break;
			default:
				throw new NotSupportedException(SR.Get("UnexpectedCollectionChangeAction", new object[]
				{
					e.Action
				}));
			}
			this.CloneList(clone, origin);
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x00165808 File Offset: 0x00163A08
		private void CloneList(IList clone, IList master)
		{
			if (clone == null || master == null)
			{
				return;
			}
			if (clone.Count > 0)
			{
				clone.Clear();
			}
			int i = 0;
			int count = master.Count;
			while (i < count)
			{
				clone.Add(master[i]);
				i++;
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x06004FB9 RID: 20409 RVA: 0x0016584C File Offset: 0x00163A4C
		private bool IsShapingActive
		{
			get
			{
				return this._shapingStorage != null;
			}
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x00165857 File Offset: 0x00163A57
		private void EnsureShapingStorage()
		{
			if (!this.IsShapingActive)
			{
				this._shapingStorage = new ItemCollection.ShapingStorage();
			}
		}

		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x0016586C File Offset: 0x00163A6C
		// (set) Token: 0x06004FBC RID: 20412 RVA: 0x00165883 File Offset: 0x00163A83
		private SortDescriptionCollection MySortDescriptions
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._sort;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._sort = value;
			}
		}

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x06004FBD RID: 20413 RVA: 0x00165897 File Offset: 0x00163A97
		// (set) Token: 0x06004FBE RID: 20414 RVA: 0x001658AE File Offset: 0x00163AAE
		private bool IsSortingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isSortingSet;
			}
			set
			{
				this._shapingStorage._isSortingSet = value;
			}
		}

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x06004FBF RID: 20415 RVA: 0x001658BC File Offset: 0x00163ABC
		private MonitorWrapper SortDescriptionsMonitor
		{
			get
			{
				if (this._shapingStorage._sortDescriptionsMonitor == null)
				{
					this._shapingStorage._sortDescriptionsMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._sortDescriptionsMonitor;
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x06004FC0 RID: 20416 RVA: 0x001658E6 File Offset: 0x00163AE6
		// (set) Token: 0x06004FC1 RID: 20417 RVA: 0x001658FD File Offset: 0x00163AFD
		private Predicate<object> MyFilter
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._filter;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._filter = value;
			}
		}

		// Token: 0x17001372 RID: 4978
		// (get) Token: 0x06004FC2 RID: 20418 RVA: 0x00165911 File Offset: 0x00163B11
		// (set) Token: 0x06004FC3 RID: 20419 RVA: 0x00165928 File Offset: 0x00163B28
		private ObservableCollection<GroupDescription> MyGroupDescriptions
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._groupBy;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._groupBy = value;
			}
		}

		// Token: 0x17001373 RID: 4979
		// (get) Token: 0x06004FC4 RID: 20420 RVA: 0x0016593C File Offset: 0x00163B3C
		// (set) Token: 0x06004FC5 RID: 20421 RVA: 0x00165953 File Offset: 0x00163B53
		private bool IsGroupingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isGroupingSet;
			}
			set
			{
				if (this.IsShapingActive)
				{
					this._shapingStorage._isGroupingSet = value;
				}
			}
		}

		// Token: 0x17001374 RID: 4980
		// (get) Token: 0x06004FC6 RID: 20422 RVA: 0x00165969 File Offset: 0x00163B69
		private MonitorWrapper GroupDescriptionsMonitor
		{
			get
			{
				if (this._shapingStorage._groupDescriptionsMonitor == null)
				{
					this._shapingStorage._groupDescriptionsMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._groupDescriptionsMonitor;
			}
		}

		// Token: 0x17001375 RID: 4981
		// (get) Token: 0x06004FC7 RID: 20423 RVA: 0x00165994 File Offset: 0x00163B94
		// (set) Token: 0x06004FC8 RID: 20424 RVA: 0x001659BE File Offset: 0x00163BBE
		private bool? MyIsLiveSorting
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._isLiveSorting;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._isLiveSorting = value;
			}
		}

		// Token: 0x17001376 RID: 4982
		// (get) Token: 0x06004FC9 RID: 20425 RVA: 0x001659D2 File Offset: 0x00163BD2
		// (set) Token: 0x06004FCA RID: 20426 RVA: 0x001659E9 File Offset: 0x00163BE9
		private ObservableCollection<string> MyLiveSortingProperties
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._liveSortingProperties;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._liveSortingProperties = value;
			}
		}

		// Token: 0x17001377 RID: 4983
		// (get) Token: 0x06004FCB RID: 20427 RVA: 0x001659FD File Offset: 0x00163BFD
		// (set) Token: 0x06004FCC RID: 20428 RVA: 0x00165A14 File Offset: 0x00163C14
		private bool IsLiveSortingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isLiveSortingSet;
			}
			set
			{
				this._shapingStorage._isLiveSortingSet = value;
			}
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x06004FCD RID: 20429 RVA: 0x00165A22 File Offset: 0x00163C22
		private MonitorWrapper LiveSortingMonitor
		{
			get
			{
				if (this._shapingStorage._liveSortingMonitor == null)
				{
					this._shapingStorage._liveSortingMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._liveSortingMonitor;
			}
		}

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x00165A4C File Offset: 0x00163C4C
		// (set) Token: 0x06004FCF RID: 20431 RVA: 0x00165A76 File Offset: 0x00163C76
		private bool? MyIsLiveFiltering
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._isLiveFiltering;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._isLiveFiltering = value;
			}
		}

		// Token: 0x1700137A RID: 4986
		// (get) Token: 0x06004FD0 RID: 20432 RVA: 0x00165A8A File Offset: 0x00163C8A
		// (set) Token: 0x06004FD1 RID: 20433 RVA: 0x00165AA1 File Offset: 0x00163CA1
		private ObservableCollection<string> MyLiveFilteringProperties
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._liveFilteringProperties;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._liveFilteringProperties = value;
			}
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x06004FD2 RID: 20434 RVA: 0x00165AB5 File Offset: 0x00163CB5
		// (set) Token: 0x06004FD3 RID: 20435 RVA: 0x00165ACC File Offset: 0x00163CCC
		private bool IsLiveFilteringSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isLiveFilteringSet;
			}
			set
			{
				this._shapingStorage._isLiveFilteringSet = value;
			}
		}

		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x06004FD4 RID: 20436 RVA: 0x00165ADA File Offset: 0x00163CDA
		private MonitorWrapper LiveFilteringMonitor
		{
			get
			{
				if (this._shapingStorage._liveFilteringMonitor == null)
				{
					this._shapingStorage._liveFilteringMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._liveFilteringMonitor;
			}
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x00165B04 File Offset: 0x00163D04
		// (set) Token: 0x06004FD6 RID: 20438 RVA: 0x00165B2E File Offset: 0x00163D2E
		private bool? MyIsLiveGrouping
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._isLiveGrouping;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._isLiveGrouping = value;
			}
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x00165B42 File Offset: 0x00163D42
		// (set) Token: 0x06004FD8 RID: 20440 RVA: 0x00165B59 File Offset: 0x00163D59
		private ObservableCollection<string> MyLiveGroupingProperties
		{
			get
			{
				if (!this.IsShapingActive)
				{
					return null;
				}
				return this._shapingStorage._liveGroupingProperties;
			}
			set
			{
				this.EnsureShapingStorage();
				this._shapingStorage._liveGroupingProperties = value;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x06004FD9 RID: 20441 RVA: 0x00165B6D File Offset: 0x00163D6D
		// (set) Token: 0x06004FDA RID: 20442 RVA: 0x00165B84 File Offset: 0x00163D84
		private bool IsLiveGroupingSet
		{
			get
			{
				return this.IsShapingActive && this._shapingStorage._isLiveGroupingSet;
			}
			set
			{
				this._shapingStorage._isLiveGroupingSet = value;
			}
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x06004FDB RID: 20443 RVA: 0x00165B92 File Offset: 0x00163D92
		private MonitorWrapper LiveGroupingMonitor
		{
			get
			{
				if (this._shapingStorage._liveGroupingMonitor == null)
				{
					this._shapingStorage._liveGroupingMonitor = new MonitorWrapper();
				}
				return this._shapingStorage._liveGroupingMonitor;
			}
		}

		// Token: 0x04002C0B RID: 11275
		private InnerItemCollectionView _internalView;

		// Token: 0x04002C0C RID: 11276
		private IEnumerable _itemsSource;

		// Token: 0x04002C0D RID: 11277
		private CollectionView _collectionView;

		// Token: 0x04002C0E RID: 11278
		private int _defaultCapacity = 16;

		// Token: 0x04002C0F RID: 11279
		private bool _isUsingItemsSource;

		// Token: 0x04002C10 RID: 11280
		private bool _isInitializing;

		// Token: 0x04002C11 RID: 11281
		private int _deferLevel;

		// Token: 0x04002C12 RID: 11282
		private IDisposable _deferInnerRefresh;

		// Token: 0x04002C13 RID: 11283
		private ItemCollection.ShapingStorage _shapingStorage;

		// Token: 0x04002C14 RID: 11284
		private WeakReference _modelParent;

		// Token: 0x02000995 RID: 2453
		private class ShapingStorage
		{
			// Token: 0x040044CA RID: 17610
			public bool _isSortingSet;

			// Token: 0x040044CB RID: 17611
			public bool _isGroupingSet;

			// Token: 0x040044CC RID: 17612
			public bool _isLiveSortingSet;

			// Token: 0x040044CD RID: 17613
			public bool _isLiveFilteringSet;

			// Token: 0x040044CE RID: 17614
			public bool _isLiveGroupingSet;

			// Token: 0x040044CF RID: 17615
			public SortDescriptionCollection _sort;

			// Token: 0x040044D0 RID: 17616
			public Predicate<object> _filter;

			// Token: 0x040044D1 RID: 17617
			public ObservableCollection<GroupDescription> _groupBy;

			// Token: 0x040044D2 RID: 17618
			public bool? _isLiveSorting;

			// Token: 0x040044D3 RID: 17619
			public bool? _isLiveFiltering;

			// Token: 0x040044D4 RID: 17620
			public bool? _isLiveGrouping;

			// Token: 0x040044D5 RID: 17621
			public ObservableCollection<string> _liveSortingProperties;

			// Token: 0x040044D6 RID: 17622
			public ObservableCollection<string> _liveFilteringProperties;

			// Token: 0x040044D7 RID: 17623
			public ObservableCollection<string> _liveGroupingProperties;

			// Token: 0x040044D8 RID: 17624
			public MonitorWrapper _sortDescriptionsMonitor;

			// Token: 0x040044D9 RID: 17625
			public MonitorWrapper _groupDescriptionsMonitor;

			// Token: 0x040044DA RID: 17626
			public MonitorWrapper _liveSortingMonitor;

			// Token: 0x040044DB RID: 17627
			public MonitorWrapper _liveFilteringMonitor;

			// Token: 0x040044DC RID: 17628
			public MonitorWrapper _liveGroupingMonitor;
		}

		// Token: 0x02000996 RID: 2454
		private class DeferHelper : IDisposable
		{
			// Token: 0x060087DC RID: 34780 RVA: 0x00250E55 File Offset: 0x0024F055
			public DeferHelper(ItemCollection itemCollection)
			{
				this._itemCollection = itemCollection;
			}

			// Token: 0x060087DD RID: 34781 RVA: 0x00250E64 File Offset: 0x0024F064
			public void Dispose()
			{
				if (this._itemCollection != null)
				{
					this._itemCollection.EndDefer();
					this._itemCollection = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x040044DD RID: 17629
			private ItemCollection _itemCollection;
		}
	}
}
