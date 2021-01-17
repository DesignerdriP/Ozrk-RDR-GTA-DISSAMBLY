using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200070E RID: 1806
	internal class CollectionViewProxy : CollectionView, IEditableCollectionViewAddNewItem, IEditableCollectionView, ICollectionViewLiveShaping, IItemProperties
	{
		// Token: 0x060073E1 RID: 29665 RVA: 0x00212A48 File Offset: 0x00210C48
		internal CollectionViewProxy(ICollectionView view) : base(view.SourceCollection, false)
		{
			this._view = view;
			view.CollectionChanged += this._OnViewChanged;
			view.CurrentChanging += this._OnCurrentChanging;
			view.CurrentChanged += this._OnCurrentChanged;
			INotifyPropertyChanged notifyPropertyChanged = view as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += this._OnPropertyChanged;
			}
		}

		// Token: 0x17001B89 RID: 7049
		// (get) Token: 0x060073E2 RID: 29666 RVA: 0x00212ABB File Offset: 0x00210CBB
		// (set) Token: 0x060073E3 RID: 29667 RVA: 0x00212AC8 File Offset: 0x00210CC8
		public override CultureInfo Culture
		{
			get
			{
				return this.ProxiedView.Culture;
			}
			set
			{
				this.ProxiedView.Culture = value;
			}
		}

		// Token: 0x060073E4 RID: 29668 RVA: 0x00212AD6 File Offset: 0x00210CD6
		public override bool Contains(object item)
		{
			return this.ProxiedView.Contains(item);
		}

		// Token: 0x17001B8A RID: 7050
		// (get) Token: 0x060073E5 RID: 29669 RVA: 0x00212AE4 File Offset: 0x00210CE4
		public override IEnumerable SourceCollection
		{
			get
			{
				return base.SourceCollection;
			}
		}

		// Token: 0x17001B8B RID: 7051
		// (get) Token: 0x060073E6 RID: 29670 RVA: 0x00212AEC File Offset: 0x00210CEC
		// (set) Token: 0x060073E7 RID: 29671 RVA: 0x00212AF9 File Offset: 0x00210CF9
		public override Predicate<object> Filter
		{
			get
			{
				return this.ProxiedView.Filter;
			}
			set
			{
				this.ProxiedView.Filter = value;
			}
		}

		// Token: 0x17001B8C RID: 7052
		// (get) Token: 0x060073E8 RID: 29672 RVA: 0x00212B07 File Offset: 0x00210D07
		public override bool CanFilter
		{
			get
			{
				return this.ProxiedView.CanFilter;
			}
		}

		// Token: 0x17001B8D RID: 7053
		// (get) Token: 0x060073E9 RID: 29673 RVA: 0x00212B14 File Offset: 0x00210D14
		public override SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this.ProxiedView.SortDescriptions;
			}
		}

		// Token: 0x17001B8E RID: 7054
		// (get) Token: 0x060073EA RID: 29674 RVA: 0x00212B21 File Offset: 0x00210D21
		public override bool CanSort
		{
			get
			{
				return this.ProxiedView.CanSort;
			}
		}

		// Token: 0x17001B8F RID: 7055
		// (get) Token: 0x060073EB RID: 29675 RVA: 0x00212B2E File Offset: 0x00210D2E
		public override bool CanGroup
		{
			get
			{
				return this.ProxiedView.CanGroup;
			}
		}

		// Token: 0x17001B90 RID: 7056
		// (get) Token: 0x060073EC RID: 29676 RVA: 0x00212B3B File Offset: 0x00210D3B
		public override ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this.ProxiedView.GroupDescriptions;
			}
		}

		// Token: 0x17001B91 RID: 7057
		// (get) Token: 0x060073ED RID: 29677 RVA: 0x00212B48 File Offset: 0x00210D48
		public override ReadOnlyObservableCollection<object> Groups
		{
			get
			{
				return this.ProxiedView.Groups;
			}
		}

		// Token: 0x060073EE RID: 29678 RVA: 0x00212B58 File Offset: 0x00210D58
		public override void Refresh()
		{
			IndexedEnumerable indexedEnumerable = Interlocked.Exchange<IndexedEnumerable>(ref this._indexer, null);
			if (indexedEnumerable != null)
			{
				indexedEnumerable.Invalidate();
			}
			this.ProxiedView.Refresh();
		}

		// Token: 0x060073EF RID: 29679 RVA: 0x00212B86 File Offset: 0x00210D86
		public override IDisposable DeferRefresh()
		{
			return this.ProxiedView.DeferRefresh();
		}

		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x060073F0 RID: 29680 RVA: 0x00212B93 File Offset: 0x00210D93
		public override object CurrentItem
		{
			get
			{
				return this.ProxiedView.CurrentItem;
			}
		}

		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x060073F1 RID: 29681 RVA: 0x00212BA0 File Offset: 0x00210DA0
		public override int CurrentPosition
		{
			get
			{
				return this.ProxiedView.CurrentPosition;
			}
		}

		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x060073F2 RID: 29682 RVA: 0x00212BAD File Offset: 0x00210DAD
		public override bool IsCurrentAfterLast
		{
			get
			{
				return this.ProxiedView.IsCurrentAfterLast;
			}
		}

		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x060073F3 RID: 29683 RVA: 0x00212BBA File Offset: 0x00210DBA
		public override bool IsCurrentBeforeFirst
		{
			get
			{
				return this.ProxiedView.IsCurrentBeforeFirst;
			}
		}

		// Token: 0x060073F4 RID: 29684 RVA: 0x00212BC7 File Offset: 0x00210DC7
		public override bool MoveCurrentToFirst()
		{
			return this.ProxiedView.MoveCurrentToFirst();
		}

		// Token: 0x060073F5 RID: 29685 RVA: 0x00212BD4 File Offset: 0x00210DD4
		public override bool MoveCurrentToPrevious()
		{
			return this.ProxiedView.MoveCurrentToPrevious();
		}

		// Token: 0x060073F6 RID: 29686 RVA: 0x00212BE1 File Offset: 0x00210DE1
		public override bool MoveCurrentToNext()
		{
			return this.ProxiedView.MoveCurrentToNext();
		}

		// Token: 0x060073F7 RID: 29687 RVA: 0x00212BEE File Offset: 0x00210DEE
		public override bool MoveCurrentToLast()
		{
			return this.ProxiedView.MoveCurrentToLast();
		}

		// Token: 0x060073F8 RID: 29688 RVA: 0x00212BFB File Offset: 0x00210DFB
		public override bool MoveCurrentTo(object item)
		{
			return this.ProxiedView.MoveCurrentTo(item);
		}

		// Token: 0x060073F9 RID: 29689 RVA: 0x00212C09 File Offset: 0x00210E09
		public override bool MoveCurrentToPosition(int position)
		{
			return this.ProxiedView.MoveCurrentToPosition(position);
		}

		// Token: 0x14000157 RID: 343
		// (add) Token: 0x060073FA RID: 29690 RVA: 0x00212C17 File Offset: 0x00210E17
		// (remove) Token: 0x060073FB RID: 29691 RVA: 0x00212C20 File Offset: 0x00210E20
		public override event CurrentChangingEventHandler CurrentChanging
		{
			add
			{
				this.PrivateCurrentChanging += value;
			}
			remove
			{
				this.PrivateCurrentChanging -= value;
			}
		}

		// Token: 0x14000158 RID: 344
		// (add) Token: 0x060073FC RID: 29692 RVA: 0x00212C29 File Offset: 0x00210E29
		// (remove) Token: 0x060073FD RID: 29693 RVA: 0x00212C32 File Offset: 0x00210E32
		public override event EventHandler CurrentChanged
		{
			add
			{
				this.PrivateCurrentChanged += value;
			}
			remove
			{
				this.PrivateCurrentChanged -= value;
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x060073FE RID: 29694 RVA: 0x00212C3B File Offset: 0x00210E3B
		public override int Count
		{
			get
			{
				return this.EnumerableWrapper.Count;
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x060073FF RID: 29695 RVA: 0x00212C48 File Offset: 0x00210E48
		public override bool IsEmpty
		{
			get
			{
				return this.ProxiedView.IsEmpty;
			}
		}

		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x06007400 RID: 29696 RVA: 0x00212C55 File Offset: 0x00210E55
		public ICollectionView ProxiedView
		{
			get
			{
				return this._view;
			}
		}

		// Token: 0x06007401 RID: 29697 RVA: 0x00212C5D File Offset: 0x00210E5D
		public override int IndexOf(object item)
		{
			return this.EnumerableWrapper.IndexOf(item);
		}

		// Token: 0x06007402 RID: 29698 RVA: 0x00212C6B File Offset: 0x00210E6B
		public override bool PassesFilter(object item)
		{
			return !this.ProxiedView.CanFilter || this.ProxiedView.Filter == null || item == CollectionView.NewItemPlaceholder || item == ((IEditableCollectionView)this).CurrentAddItem || this.ProxiedView.Filter(item);
		}

		// Token: 0x06007403 RID: 29699 RVA: 0x00212CAB File Offset: 0x00210EAB
		public override object GetItemAt(int index)
		{
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return this.EnumerableWrapper[index];
		}

		// Token: 0x06007404 RID: 29700 RVA: 0x00212CC8 File Offset: 0x00210EC8
		public override void DetachFromSourceCollection()
		{
			if (this._view != null)
			{
				this._view.CollectionChanged -= this._OnViewChanged;
				this._view.CurrentChanging -= this._OnCurrentChanging;
				this._view.CurrentChanged -= this._OnCurrentChanged;
				INotifyPropertyChanged notifyPropertyChanged = this._view as INotifyPropertyChanged;
				if (notifyPropertyChanged != null)
				{
					notifyPropertyChanged.PropertyChanged -= this._OnPropertyChanged;
				}
				this._view = null;
			}
			base.DetachFromSourceCollection();
		}

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x06007405 RID: 29701 RVA: 0x00212D50 File Offset: 0x00210F50
		// (set) Token: 0x06007406 RID: 29702 RVA: 0x00212D74 File Offset: 0x00210F74
		NewItemPlaceholderPosition IEditableCollectionView.NewItemPlaceholderPosition
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.NewItemPlaceholderPosition;
				}
				return NewItemPlaceholderPosition.None;
			}
			set
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x06007407 RID: 29703 RVA: 0x00212DB8 File Offset: 0x00210FB8
		bool IEditableCollectionView.CanAddNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanAddNew;
			}
		}

		// Token: 0x06007408 RID: 29704 RVA: 0x00212DDC File Offset: 0x00210FDC
		object IEditableCollectionView.AddNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
			if (editableCollectionView != null)
			{
				return editableCollectionView.AddNew();
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNew"
			}));
		}

		// Token: 0x06007409 RID: 29705 RVA: 0x00212E1C File Offset: 0x0021101C
		void IEditableCollectionView.CommitNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x0600740A RID: 29706 RVA: 0x00212E5C File Offset: 0x0021105C
		void IEditableCollectionView.CancelNew()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x17001B9B RID: 7067
		// (get) Token: 0x0600740B RID: 29707 RVA: 0x00212E9C File Offset: 0x0021109C
		bool IEditableCollectionView.IsAddingNew
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsAddingNew;
			}
		}

		// Token: 0x17001B9C RID: 7068
		// (get) Token: 0x0600740C RID: 29708 RVA: 0x00212EC0 File Offset: 0x002110C0
		object IEditableCollectionView.CurrentAddItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentAddItem;
				}
				return null;
			}
		}

		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x0600740D RID: 29709 RVA: 0x00212EE4 File Offset: 0x002110E4
		bool IEditableCollectionView.CanRemove
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanRemove;
			}
		}

		// Token: 0x0600740E RID: 29710 RVA: 0x00212F08 File Offset: 0x00211108
		void IEditableCollectionView.RemoveAt(int index)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x0600740F RID: 29711 RVA: 0x00212F4C File Offset: 0x0021114C
		void IEditableCollectionView.Remove(object item)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x06007410 RID: 29712 RVA: 0x00212F90 File Offset: 0x00211190
		void IEditableCollectionView.EditItem(object item)
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x06007411 RID: 29713 RVA: 0x00212FD4 File Offset: 0x002111D4
		void IEditableCollectionView.CommitEdit()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x06007412 RID: 29714 RVA: 0x00213014 File Offset: 0x00211214
		void IEditableCollectionView.CancelEdit()
		{
			IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
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

		// Token: 0x17001B9E RID: 7070
		// (get) Token: 0x06007413 RID: 29715 RVA: 0x00213054 File Offset: 0x00211254
		bool IEditableCollectionView.CanCancelEdit
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.CanCancelEdit;
			}
		}

		// Token: 0x17001B9F RID: 7071
		// (get) Token: 0x06007414 RID: 29716 RVA: 0x00213078 File Offset: 0x00211278
		bool IEditableCollectionView.IsEditingItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				return editableCollectionView != null && editableCollectionView.IsEditingItem;
			}
		}

		// Token: 0x17001BA0 RID: 7072
		// (get) Token: 0x06007415 RID: 29717 RVA: 0x0021309C File Offset: 0x0021129C
		object IEditableCollectionView.CurrentEditItem
		{
			get
			{
				IEditableCollectionView editableCollectionView = this.ProxiedView as IEditableCollectionView;
				if (editableCollectionView != null)
				{
					return editableCollectionView.CurrentEditItem;
				}
				return null;
			}
		}

		// Token: 0x17001BA1 RID: 7073
		// (get) Token: 0x06007416 RID: 29718 RVA: 0x002130C0 File Offset: 0x002112C0
		bool IEditableCollectionViewAddNewItem.CanAddNewItem
		{
			get
			{
				IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this.ProxiedView as IEditableCollectionViewAddNewItem;
				return editableCollectionViewAddNewItem != null && editableCollectionViewAddNewItem.CanAddNewItem;
			}
		}

		// Token: 0x06007417 RID: 29719 RVA: 0x002130E4 File Offset: 0x002112E4
		object IEditableCollectionViewAddNewItem.AddNewItem(object newItem)
		{
			IEditableCollectionViewAddNewItem editableCollectionViewAddNewItem = this.ProxiedView as IEditableCollectionViewAddNewItem;
			if (editableCollectionViewAddNewItem != null)
			{
				return editableCollectionViewAddNewItem.AddNewItem(newItem);
			}
			throw new InvalidOperationException(SR.Get("MemberNotAllowedForView", new object[]
			{
				"AddNewItem"
			}));
		}

		// Token: 0x17001BA2 RID: 7074
		// (get) Token: 0x06007418 RID: 29720 RVA: 0x00213128 File Offset: 0x00211328
		bool ICollectionViewLiveShaping.CanChangeLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveSorting;
			}
		}

		// Token: 0x17001BA3 RID: 7075
		// (get) Token: 0x06007419 RID: 29721 RVA: 0x0021314C File Offset: 0x0021134C
		bool ICollectionViewLiveShaping.CanChangeLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveFiltering;
			}
		}

		// Token: 0x17001BA4 RID: 7076
		// (get) Token: 0x0600741A RID: 29722 RVA: 0x00213170 File Offset: 0x00211370
		bool ICollectionViewLiveShaping.CanChangeLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				return collectionViewLiveShaping != null && collectionViewLiveShaping.CanChangeLiveGrouping;
			}
		}

		// Token: 0x17001BA5 RID: 7077
		// (get) Token: 0x0600741B RID: 29723 RVA: 0x00213194 File Offset: 0x00211394
		// (set) Token: 0x0600741C RID: 29724 RVA: 0x002131C0 File Offset: 0x002113C0
		bool? ICollectionViewLiveShaping.IsLiveSorting
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveSorting;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveSorting = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveSorting",
					"CanChangeLiveSorting"
				}));
			}
		}

		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x0600741D RID: 29725 RVA: 0x0021320C File Offset: 0x0021140C
		// (set) Token: 0x0600741E RID: 29726 RVA: 0x00213238 File Offset: 0x00211438
		bool? ICollectionViewLiveShaping.IsLiveFiltering
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveFiltering;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveFiltering = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveFiltering",
					"CanChangeLiveFiltering"
				}));
			}
		}

		// Token: 0x17001BA7 RID: 7079
		// (get) Token: 0x0600741F RID: 29727 RVA: 0x00213284 File Offset: 0x00211484
		// (set) Token: 0x06007420 RID: 29728 RVA: 0x002132B0 File Offset: 0x002114B0
		bool? ICollectionViewLiveShaping.IsLiveGrouping
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping == null)
				{
					return null;
				}
				return collectionViewLiveShaping.IsLiveGrouping;
			}
			set
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					collectionViewLiveShaping.IsLiveGrouping = value;
					return;
				}
				throw new InvalidOperationException(SR.Get("CannotChangeLiveShaping", new object[]
				{
					"IsLiveGrouping",
					"CanChangeLiveGrouping"
				}));
			}
		}

		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x06007421 RID: 29729 RVA: 0x002132FC File Offset: 0x002114FC
		ObservableCollection<string> ICollectionViewLiveShaping.LiveSortingProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveSortingProperties;
				}
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
				}
				return this._liveSortingProperties;
			}
		}

		// Token: 0x17001BA9 RID: 7081
		// (get) Token: 0x06007422 RID: 29730 RVA: 0x00213338 File Offset: 0x00211538
		ObservableCollection<string> ICollectionViewLiveShaping.LiveFilteringProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveFilteringProperties;
				}
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
				}
				return this._liveFilteringProperties;
			}
		}

		// Token: 0x17001BAA RID: 7082
		// (get) Token: 0x06007423 RID: 29731 RVA: 0x00213374 File Offset: 0x00211574
		ObservableCollection<string> ICollectionViewLiveShaping.LiveGroupingProperties
		{
			get
			{
				ICollectionViewLiveShaping collectionViewLiveShaping = this.ProxiedView as ICollectionViewLiveShaping;
				if (collectionViewLiveShaping != null)
				{
					return collectionViewLiveShaping.LiveGroupingProperties;
				}
				if (this._liveGroupingProperties == null)
				{
					this._liveGroupingProperties = new ObservableCollection<string>();
				}
				return this._liveGroupingProperties;
			}
		}

		// Token: 0x17001BAB RID: 7083
		// (get) Token: 0x06007424 RID: 29732 RVA: 0x002133B0 File Offset: 0x002115B0
		ReadOnlyCollection<ItemPropertyInfo> IItemProperties.ItemProperties
		{
			get
			{
				IItemProperties itemProperties = this.ProxiedView as IItemProperties;
				if (itemProperties != null)
				{
					return itemProperties.ItemProperties;
				}
				return null;
			}
		}

		// Token: 0x06007425 RID: 29733 RVA: 0x002133D4 File Offset: 0x002115D4
		protected override IEnumerator GetEnumerator()
		{
			return this.ProxiedView.GetEnumerator();
		}

		// Token: 0x06007426 RID: 29734 RVA: 0x002133E4 File Offset: 0x002115E4
		internal override void GetCollectionChangedSources(int level, Action<int, object, bool?, List<string>> format, List<string> sources)
		{
			format(level, this, new bool?(false), sources);
			if (this._view != null)
			{
				format(level + 1, this._view, new bool?(true), sources);
				object sourceCollection = this._view.SourceCollection;
				if (sourceCollection != null)
				{
					format(level + 2, sourceCollection, null, sources);
				}
			}
		}

		// Token: 0x06007427 RID: 29735 RVA: 0x00165117 File Offset: 0x00163317
		private void _OnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			this.OnPropertyChanged(args);
		}

		// Token: 0x06007428 RID: 29736 RVA: 0x00213441 File Offset: 0x00211641
		private void _OnViewChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			this.OnCollectionChanged(args);
		}

		// Token: 0x06007429 RID: 29737 RVA: 0x0021344A File Offset: 0x0021164A
		private void _OnCurrentChanging(object sender, CurrentChangingEventArgs args)
		{
			if (this.PrivateCurrentChanging != null)
			{
				this.PrivateCurrentChanging(this, args);
			}
		}

		// Token: 0x0600742A RID: 29738 RVA: 0x00213461 File Offset: 0x00211661
		private void _OnCurrentChanged(object sender, EventArgs args)
		{
			if (this.PrivateCurrentChanged != null)
			{
				this.PrivateCurrentChanged(this, args);
			}
		}

		// Token: 0x17001BAC RID: 7084
		// (get) Token: 0x0600742B RID: 29739 RVA: 0x00213478 File Offset: 0x00211678
		private IndexedEnumerable EnumerableWrapper
		{
			get
			{
				if (this._indexer == null)
				{
					IndexedEnumerable value = new IndexedEnumerable(this.ProxiedView, new Predicate<object>(this.PassesFilter));
					Interlocked.CompareExchange<IndexedEnumerable>(ref this._indexer, value, null);
				}
				return this._indexer;
			}
		}

		// Token: 0x14000159 RID: 345
		// (add) Token: 0x0600742C RID: 29740 RVA: 0x002134BC File Offset: 0x002116BC
		// (remove) Token: 0x0600742D RID: 29741 RVA: 0x002134F4 File Offset: 0x002116F4
		private event CurrentChangingEventHandler PrivateCurrentChanging;

		// Token: 0x1400015A RID: 346
		// (add) Token: 0x0600742E RID: 29742 RVA: 0x0021352C File Offset: 0x0021172C
		// (remove) Token: 0x0600742F RID: 29743 RVA: 0x00213564 File Offset: 0x00211764
		private event EventHandler PrivateCurrentChanged;

		// Token: 0x040037C1 RID: 14273
		private ICollectionView _view;

		// Token: 0x040037C2 RID: 14274
		private IndexedEnumerable _indexer;

		// Token: 0x040037C5 RID: 14277
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x040037C6 RID: 14278
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x040037C7 RID: 14279
		private ObservableCollection<string> _liveGroupingProperties;
	}
}
