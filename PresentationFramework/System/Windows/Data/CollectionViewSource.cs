using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>The Extensible Application Markup Language (XAML) proxy of a <see cref="T:System.Windows.Data.CollectionView" /> class.</summary>
	// Token: 0x020001AA RID: 426
	public class CollectionViewSource : DependencyObject, ISupportInitialize, IWeakEventListener
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.CollectionViewSource" /> class.</summary>
		// Token: 0x06001AE0 RID: 6880 RVA: 0x0007F3F8 File Offset: 0x0007D5F8
		public CollectionViewSource()
		{
			this._sort = new SortDescriptionCollection();
			((INotifyCollectionChanged)this._sort).CollectionChanged += this.OnForwardedCollectionChanged;
			this._groupBy = new ObservableCollection<GroupDescription>();
			((INotifyCollectionChanged)this._groupBy).CollectionChanged += this.OnForwardedCollectionChanged;
		}

		/// <summary>Gets the view object that is currently associated with this instance of <see cref="T:System.Windows.Data.CollectionViewSource" />.  </summary>
		/// <returns>The view object that is currently associated with this instance of <see cref="T:System.Windows.Data.CollectionViewSource" />.</returns>
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x0007F44F File Offset: 0x0007D64F
		[ReadOnly(true)]
		public ICollectionView View
		{
			get
			{
				return CollectionViewSource.GetOriginalView(this.CollectionView);
			}
		}

		/// <summary>Gets or sets the collection object from which to create this view.   </summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x0007F45C File Offset: 0x0007D65C
		// (set) Token: 0x06001AE3 RID: 6883 RVA: 0x0007F469 File Offset: 0x0007D669
		public object Source
		{
			get
			{
				return base.GetValue(CollectionViewSource.SourceProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.SourceProperty, value);
			}
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x0007F478 File Offset: 0x0007D678
		private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionViewSource collectionViewSource = (CollectionViewSource)d;
			collectionViewSource.OnSourceChanged(e.OldValue, e.NewValue);
			collectionViewSource.EnsureView();
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Data.CollectionViewSource.Source" /> property changes.</summary>
		/// <param name="oldSource">The old value of the <see cref="P:System.Windows.Data.CollectionViewSource.Source" /> property.</param>
		/// <param name="newSource">The new value of the <see cref="P:System.Windows.Data.CollectionViewSource.Source" /> property.</param>
		// Token: 0x06001AE5 RID: 6885 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnSourceChanged(object oldSource, object newSource)
		{
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0007F4A6 File Offset: 0x0007D6A6
		private static bool IsSourceValid(object o)
		{
			return (o == null || o is IEnumerable || o is IListSource || o is DataSourceProvider) && !(o is ICollectionView);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0007F4D1 File Offset: 0x0007D6D1
		private static bool IsValidSourceForView(object o)
		{
			return o is IEnumerable || o is IListSource;
		}

		/// <summary>Gets or sets the desired view type.  </summary>
		/// <returns>The desired view type.</returns>
		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001AE8 RID: 6888 RVA: 0x0007F4E6 File Offset: 0x0007D6E6
		// (set) Token: 0x06001AE9 RID: 6889 RVA: 0x0007F4F8 File Offset: 0x0007D6F8
		public Type CollectionViewType
		{
			get
			{
				return (Type)base.GetValue(CollectionViewSource.CollectionViewTypeProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.CollectionViewTypeProperty, value);
			}
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0007F508 File Offset: 0x0007D708
		private static void OnCollectionViewTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionViewSource collectionViewSource = (CollectionViewSource)d;
			Type oldCollectionViewType = (Type)e.OldValue;
			Type newCollectionViewType = (Type)e.NewValue;
			if (!collectionViewSource._isInitializing)
			{
				throw new InvalidOperationException(SR.Get("CollectionViewTypeIsInitOnly"));
			}
			collectionViewSource.OnCollectionViewTypeChanged(oldCollectionViewType, newCollectionViewType);
			collectionViewSource.EnsureView();
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Data.CollectionViewSource.CollectionViewType" /> property changes.</summary>
		/// <param name="oldCollectionViewType">The old value of the <see cref="P:System.Windows.Data.CollectionViewSource.CollectionViewType" /> property.</param>
		/// <param name="newCollectionViewType">The new value of the <see cref="P:System.Windows.Data.CollectionViewSource.CollectionViewType" /> property.</param>
		// Token: 0x06001AEB RID: 6891 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnCollectionViewTypeChanged(Type oldCollectionViewType, Type newCollectionViewType)
		{
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0007F55C File Offset: 0x0007D75C
		private static bool IsCollectionViewTypeValid(object o)
		{
			Type type = (Type)o;
			return type == null || typeof(ICollectionView).IsAssignableFrom(type);
		}

		/// <summary>Gets or sets the culture that is used for operations such as sorting and comparisons. </summary>
		/// <returns>The culture that is used for operations such as sorting and comparisons.</returns>
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x0007F58B File Offset: 0x0007D78B
		// (set) Token: 0x06001AEE RID: 6894 RVA: 0x0007F593 File Offset: 0x0007D793
		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				this._culture = value;
				this.OnForwardedPropertyChanged();
			}
		}

		/// <summary>Gets or sets a collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describes how the items in the collection are sorted in the view.</summary>
		/// <returns>A collection of <see cref="T:System.ComponentModel.SortDescription" /> objects that describes how the items in the collection are sorted in the view.</returns>
		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x0007F5A2 File Offset: 0x0007D7A2
		public SortDescriptionCollection SortDescriptions
		{
			get
			{
				return this._sort;
			}
		}

		/// <summary>Gets or sets a collection of <see cref="T:System.ComponentModel.GroupDescription" /> objects that describes how the items in the collection are grouped in the view.</summary>
		/// <returns>An <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> of <see cref="T:System.ComponentModel.GroupDescription" /> objects that describes how the items in the collection are grouped in the view.</returns>
		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001AF0 RID: 6896 RVA: 0x0007F5AA File Offset: 0x0007D7AA
		public ObservableCollection<GroupDescription> GroupDescriptions
		{
			get
			{
				return this._groupBy;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning sorting data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view supports turning live sorting on or off; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x0007F5B2 File Offset: 0x0007D7B2
		// (set) Token: 0x06001AF2 RID: 6898 RVA: 0x0007F5C4 File Offset: 0x0007D7C4
		[ReadOnly(true)]
		public bool CanChangeLiveSorting
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.CanChangeLiveSortingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.CanChangeLiveSortingPropertyKey, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Windows.Data.CollectionViewSource" /> should sort the data in real time if it can.</summary>
		/// <returns>
		///     <see langword="true" /> if live sorting has been requested; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x0007F5D2 File Offset: 0x0007D7D2
		// (set) Token: 0x06001AF4 RID: 6900 RVA: 0x0007F5E4 File Offset: 0x0007D7E4
		public bool IsLiveSortingRequested
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.IsLiveSortingRequestedProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.IsLiveSortingRequestedProperty, value);
			}
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0007F5F4 File Offset: 0x0007D7F4
		private static void OnIsLiveSortingRequestedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionViewSource collectionViewSource = (CollectionViewSource)d;
			collectionViewSource.OnForwardedPropertyChanged();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Data.CollectionViewSource" /> sorts data in real time.</summary>
		/// <returns>
		///     <see langword="true" /> if sorting data in real time is enable; <see langword="false" /> if live sorting is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live sorting. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06001AF6 RID: 6902 RVA: 0x0007F60E File Offset: 0x0007D80E
		// (set) Token: 0x06001AF7 RID: 6903 RVA: 0x0007F620 File Offset: 0x0007D820
		[ReadOnly(true)]
		public bool? IsLiveSorting
		{
			get
			{
				return (bool?)base.GetValue(CollectionViewSource.IsLiveSortingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.IsLiveSortingPropertyKey, value);
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in sorting data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in sorting data in real time.</returns>
		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06001AF8 RID: 6904 RVA: 0x0007F633 File Offset: 0x0007D833
		public ObservableCollection<string> LiveSortingProperties
		{
			get
			{
				if (this._liveSortingProperties == null)
				{
					this._liveSortingProperties = new ObservableCollection<string>();
					((INotifyCollectionChanged)this._liveSortingProperties).CollectionChanged += this.OnForwardedCollectionChanged;
				}
				return this._liveSortingProperties;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning filtering data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view supports turning live filtering on or off; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x0007F665 File Offset: 0x0007D865
		// (set) Token: 0x06001AFA RID: 6906 RVA: 0x0007F677 File Offset: 0x0007D877
		[ReadOnly(true)]
		public bool CanChangeLiveFiltering
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.CanChangeLiveFilteringProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.CanChangeLiveFilteringPropertyKey, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Windows.Data.CollectionViewSource" /> should filter the data in real time if it can.</summary>
		/// <returns>
		///     <see langword="true" /> if live filtering has been requested; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x0007F685 File Offset: 0x0007D885
		// (set) Token: 0x06001AFC RID: 6908 RVA: 0x0007F697 File Offset: 0x0007D897
		public bool IsLiveFilteringRequested
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.IsLiveFilteringRequestedProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.IsLiveFilteringRequestedProperty, value);
			}
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0007F6A8 File Offset: 0x0007D8A8
		private static void OnIsLiveFilteringRequestedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionViewSource collectionViewSource = (CollectionViewSource)d;
			collectionViewSource.OnForwardedPropertyChanged();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Data.CollectionViewSource" /> is filtering data in real time.</summary>
		/// <returns>
		///     <see langword="true" /> if filtering data in real time is enabled; <see langword="false" /> if live filtering is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live filtering. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x0007F6C2 File Offset: 0x0007D8C2
		// (set) Token: 0x06001AFF RID: 6911 RVA: 0x0007F6D4 File Offset: 0x0007D8D4
		[ReadOnly(true)]
		public bool? IsLiveFiltering
		{
			get
			{
				return (bool?)base.GetValue(CollectionViewSource.IsLiveFilteringProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.IsLiveFilteringPropertyKey, value);
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in filtering data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in filtering data in real time.</returns>
		// Token: 0x17000647 RID: 1607
		// (get) Token: 0x06001B00 RID: 6912 RVA: 0x0007F6E7 File Offset: 0x0007D8E7
		public ObservableCollection<string> LiveFilteringProperties
		{
			get
			{
				if (this._liveFilteringProperties == null)
				{
					this._liveFilteringProperties = new ObservableCollection<string>();
					((INotifyCollectionChanged)this._liveFilteringProperties).CollectionChanged += this.OnForwardedCollectionChanged;
				}
				return this._liveFilteringProperties;
			}
		}

		/// <summary>Gets a value that indicates whether the collection view supports turning grouping data in real time on or off.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection view supports turning live grouping on or off; otherwise, <see langword="false" />.The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000648 RID: 1608
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x0007F719 File Offset: 0x0007D919
		// (set) Token: 0x06001B02 RID: 6914 RVA: 0x0007F72B File Offset: 0x0007D92B
		[ReadOnly(true)]
		public bool CanChangeLiveGrouping
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.CanChangeLiveGroupingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.CanChangeLiveGroupingPropertyKey, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Windows.Data.CollectionViewSource" /> should group the data in real time if it can.</summary>
		/// <returns>
		///     <see langword="true" /> if live grouping has been requested; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x06001B03 RID: 6915 RVA: 0x0007F739 File Offset: 0x0007D939
		// (set) Token: 0x06001B04 RID: 6916 RVA: 0x0007F74B File Offset: 0x0007D94B
		public bool IsLiveGroupingRequested
		{
			get
			{
				return (bool)base.GetValue(CollectionViewSource.IsLiveGroupingRequestedProperty);
			}
			set
			{
				base.SetValue(CollectionViewSource.IsLiveGroupingRequestedProperty, value);
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0007F75C File Offset: 0x0007D95C
		private static void OnIsLiveGroupingRequestedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CollectionViewSource collectionViewSource = (CollectionViewSource)d;
			collectionViewSource.OnForwardedPropertyChanged();
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Data.CollectionViewSource" /> groups data in real time.</summary>
		/// <returns>
		///     <see langword="true" /> if grouping data in real time is enable; <see langword="false" /> if live grouping is not enabled; <see langword="null" /> if it cannot be determined whether the collection view implements live grouping. The registered default is <see langword="false" />. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x1700064A RID: 1610
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x0007F776 File Offset: 0x0007D976
		// (set) Token: 0x06001B07 RID: 6919 RVA: 0x0007F788 File Offset: 0x0007D988
		[ReadOnly(true)]
		public bool? IsLiveGrouping
		{
			get
			{
				return (bool?)base.GetValue(CollectionViewSource.IsLiveGroupingProperty);
			}
			private set
			{
				base.SetValue(CollectionViewSource.IsLiveGroupingPropertyKey, value);
			}
		}

		/// <summary>Gets a collection of strings that specify the properties that participate in grouping data in real time.</summary>
		/// <returns>A collection of strings that specify the properties that participate in grouping data in real time.</returns>
		// Token: 0x1700064B RID: 1611
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x0007F79B File Offset: 0x0007D99B
		public ObservableCollection<string> LiveGroupingProperties
		{
			get
			{
				if (this._liveGroupingProperties == null)
				{
					this._liveGroupingProperties = new ObservableCollection<string>();
					((INotifyCollectionChanged)this._liveGroupingProperties).CollectionChanged += this.OnForwardedCollectionChanged;
				}
				return this._liveGroupingProperties;
			}
		}

		/// <summary>Provides filtering logic.</summary>
		// Token: 0x14000052 RID: 82
		// (add) Token: 0x06001B09 RID: 6921 RVA: 0x0007F7D0 File Offset: 0x0007D9D0
		// (remove) Token: 0x06001B0A RID: 6922 RVA: 0x0007F810 File Offset: 0x0007DA10
		public event FilterEventHandler Filter
		{
			add
			{
				FilterEventHandler filterEventHandler = CollectionViewSource.FilterHandlersField.GetValue(this);
				if (filterEventHandler != null)
				{
					filterEventHandler = (FilterEventHandler)Delegate.Combine(filterEventHandler, value);
				}
				else
				{
					filterEventHandler = value;
				}
				CollectionViewSource.FilterHandlersField.SetValue(this, filterEventHandler);
				this.OnForwardedPropertyChanged();
			}
			remove
			{
				FilterEventHandler filterEventHandler = CollectionViewSource.FilterHandlersField.GetValue(this);
				if (filterEventHandler != null)
				{
					filterEventHandler = (FilterEventHandler)Delegate.Remove(filterEventHandler, value);
					if (filterEventHandler == null)
					{
						CollectionViewSource.FilterHandlersField.ClearValue(this);
					}
					else
					{
						CollectionViewSource.FilterHandlersField.SetValue(this, filterEventHandler);
					}
				}
				this.OnForwardedPropertyChanged();
			}
		}

		/// <summary>Returns the default view for the given source.</summary>
		/// <param name="source">An object reference to the binding source.</param>
		/// <returns>Returns an <see cref="T:System.ComponentModel.ICollectionView" /> object that is the default view for the given source collection.</returns>
		// Token: 0x06001B0B RID: 6923 RVA: 0x0007F85B File Offset: 0x0007DA5B
		public static ICollectionView GetDefaultView(object source)
		{
			return CollectionViewSource.GetOriginalView(CollectionViewSource.GetDefaultCollectionView(source, true, null));
		}

		// Token: 0x06001B0C RID: 6924 RVA: 0x0007F86A File Offset: 0x0007DA6A
		private static ICollectionView LazyGetDefaultView(object source)
		{
			return CollectionViewSource.GetOriginalView(CollectionViewSource.GetDefaultCollectionView(source, false, null));
		}

		/// <summary>Returns a value that indicates whether the given view is the default view for the <see cref="P:System.Windows.Data.CollectionViewSource.Source" /> collection.</summary>
		/// <param name="view">The view object to check.</param>
		/// <returns>
		///     <see langword="true" /> if the given view is the default view for the <see cref="P:System.Windows.Data.CollectionViewSource.Source" /> collection or if the given view is <see langword="nulll" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B0D RID: 6925 RVA: 0x0007F87C File Offset: 0x0007DA7C
		public static bool IsDefaultView(ICollectionView view)
		{
			if (view != null)
			{
				object sourceCollection = view.SourceCollection;
				return CollectionViewSource.GetOriginalView(view) == CollectionViewSource.LazyGetDefaultView(sourceCollection);
			}
			return true;
		}

		/// <summary>Enters a defer cycle that you can use to merge changes to the view and delay automatic refresh.</summary>
		/// <returns>An <see cref="T:System.IDisposable" /> object that you can use to dispose of the calling object.</returns>
		// Token: 0x06001B0E RID: 6926 RVA: 0x0007F8A3 File Offset: 0x0007DAA3
		public IDisposable DeferRefresh()
		{
			return new CollectionViewSource.DeferHelper(this);
		}

		/// <summary>Signals the object that initialization is starting.</summary>
		// Token: 0x06001B0F RID: 6927 RVA: 0x0007F8AB File Offset: 0x0007DAAB
		void ISupportInitialize.BeginInit()
		{
			this._isInitializing = true;
		}

		/// <summary>Signals the object that initialization is complete.</summary>
		// Token: 0x06001B10 RID: 6928 RVA: 0x0007F8B4 File Offset: 0x0007DAB4
		void ISupportInitialize.EndInit()
		{
			this._isInitializing = false;
			this.EnsureView();
		}

		/// <summary>Receives events from the centralized event manager.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Windows.Data.DataChangedEventManager" />.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B11 RID: 6929 RVA: 0x0007F8C3 File Offset: 0x0007DAC3
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return this.ReceiveWeakEvent(managerType, sender, e);
		}

		/// <summary>Handles events from the centralized event table.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Windows.Data.DataChangedEventManager" />.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x06001B12 RID: 6930 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x0007F8D0 File Offset: 0x0007DAD0
		internal CollectionView CollectionView
		{
			get
			{
				ICollectionView collectionView = (ICollectionView)base.GetValue(CollectionViewSource.ViewProperty);
				if (collectionView != null && !this._isViewInitialized)
				{
					object obj = this.Source;
					DataSourceProvider dataSourceProvider = obj as DataSourceProvider;
					if (dataSourceProvider != null)
					{
						obj = dataSourceProvider.Data;
					}
					if (obj != null)
					{
						DataBindEngine currentDataBindEngine = DataBindEngine.CurrentDataBindEngine;
						ViewRecord viewRecord = currentDataBindEngine.GetViewRecord(obj, this, this.CollectionViewType, true, null);
						if (viewRecord != null)
						{
							viewRecord.InitializeView();
							this._isViewInitialized = true;
						}
					}
				}
				return (CollectionView)collectionView;
			}
		}

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001B14 RID: 6932 RVA: 0x0007F944 File Offset: 0x0007DB44
		internal DependencyProperty PropertyForInheritanceContext
		{
			get
			{
				return this._propertyForInheritanceContext;
			}
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0007F94C File Offset: 0x0007DB4C
		internal static CollectionView GetDefaultCollectionView(object source, bool createView, Func<object, object> GetSourceItem = null)
		{
			if (!CollectionViewSource.IsValidSourceForView(source))
			{
				return null;
			}
			DataBindEngine currentDataBindEngine = DataBindEngine.CurrentDataBindEngine;
			ViewRecord viewRecord = currentDataBindEngine.GetViewRecord(source, CollectionViewSource.DefaultSource, null, createView, GetSourceItem);
			if (viewRecord == null)
			{
				return null;
			}
			return (CollectionView)viewRecord.View;
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0007F98C File Offset: 0x0007DB8C
		internal static CollectionView GetDefaultCollectionView(object source, DependencyObject d, Func<object, object> GetSourceItem = null)
		{
			CollectionView defaultCollectionView = CollectionViewSource.GetDefaultCollectionView(source, true, GetSourceItem);
			if (defaultCollectionView != null && defaultCollectionView.Culture == null)
			{
				XmlLanguage xmlLanguage = (d != null) ? ((XmlLanguage)d.GetValue(FrameworkElement.LanguageProperty)) : null;
				if (xmlLanguage != null)
				{
					try
					{
						defaultCollectionView.Culture = xmlLanguage.GetSpecificCulture();
					}
					catch (InvalidOperationException)
					{
					}
				}
			}
			return defaultCollectionView;
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x0007F9EC File Offset: 0x0007DBEC
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x0007F9F4 File Offset: 0x0007DBF4
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (!this._hasMultipleInheritanceContexts && this._inheritanceContext == null)
			{
				this._propertyForInheritanceContext = property;
			}
			else
			{
				this._propertyForInheritanceContext = null;
			}
			InheritanceContextHelper.AddInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0007FA29 File Offset: 0x0007DC29
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			InheritanceContextHelper.RemoveInheritanceContext(context, this, ref this._hasMultipleInheritanceContexts, ref this._inheritanceContext);
			this._propertyForInheritanceContext = null;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x06001B1A RID: 6938 RVA: 0x0007FA45 File Offset: 0x0007DC45
		internal override bool HasMultipleInheritanceContexts
		{
			get
			{
				return this._hasMultipleInheritanceContexts;
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0000B02A File Offset: 0x0000922A
		internal bool IsShareableInTemplate()
		{
			return false;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0007FA4D File Offset: 0x0007DC4D
		private void EnsureView()
		{
			this.EnsureView(this.Source, this.CollectionViewType);
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0007FA64 File Offset: 0x0007DC64
		private void EnsureView(object source, Type collectionViewType)
		{
			if (this._isInitializing || this._deferLevel > 0)
			{
				return;
			}
			DataSourceProvider dataSourceProvider = source as DataSourceProvider;
			if (dataSourceProvider != this._dataProvider)
			{
				if (this._dataProvider != null)
				{
					DataChangedEventManager.RemoveHandler(this._dataProvider, new EventHandler<EventArgs>(this.OnDataChanged));
				}
				this._dataProvider = dataSourceProvider;
				if (this._dataProvider != null)
				{
					DataChangedEventManager.AddHandler(this._dataProvider, new EventHandler<EventArgs>(this.OnDataChanged));
					this._dataProvider.InitialLoad();
				}
			}
			if (dataSourceProvider != null)
			{
				source = dataSourceProvider.Data;
			}
			ICollectionView collectionView = null;
			if (source != null)
			{
				DataBindEngine currentDataBindEngine = DataBindEngine.CurrentDataBindEngine;
				ViewRecord viewRecord = currentDataBindEngine.GetViewRecord(source, this, collectionViewType, true, delegate(object x)
				{
					BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(this, CollectionViewSource.SourceProperty);
					if (bindingExpressionBase == null)
					{
						return null;
					}
					return bindingExpressionBase.GetSourceItem(x);
				});
				if (viewRecord != null)
				{
					collectionView = viewRecord.View;
					this._isViewInitialized = viewRecord.IsInitialized;
					if (this._version != viewRecord.Version)
					{
						this.ApplyPropertiesToView(collectionView);
						viewRecord.Version = this._version;
					}
				}
			}
			base.SetValue(CollectionViewSource.ViewPropertyKey, collectionView);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0007FB54 File Offset: 0x0007DD54
		private void ApplyPropertiesToView(ICollectionView view)
		{
			if (view == null || this._deferLevel > 0)
			{
				return;
			}
			ICollectionViewLiveShaping collectionViewLiveShaping = view as ICollectionViewLiveShaping;
			using (view.DeferRefresh())
			{
				if (this.Culture != null)
				{
					view.Culture = this.Culture;
				}
				if (view.CanSort)
				{
					view.SortDescriptions.Clear();
					int i = 0;
					int count = this.SortDescriptions.Count;
					while (i < count)
					{
						view.SortDescriptions.Add(this.SortDescriptions[i]);
						i++;
					}
				}
				else if (this.SortDescriptions.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("CannotSortView", new object[]
					{
						view
					}));
				}
				Predicate<object> predicate;
				if (CollectionViewSource.FilterHandlersField.GetValue(this) != null)
				{
					predicate = this.FilterWrapper;
				}
				else
				{
					predicate = null;
				}
				if (view.CanFilter)
				{
					view.Filter = predicate;
				}
				else if (predicate != null)
				{
					throw new InvalidOperationException(SR.Get("CannotFilterView", new object[]
					{
						view
					}));
				}
				if (view.CanGroup)
				{
					view.GroupDescriptions.Clear();
					int i = 0;
					int count = this.GroupDescriptions.Count;
					while (i < count)
					{
						view.GroupDescriptions.Add(this.GroupDescriptions[i]);
						i++;
					}
				}
				else if (this.GroupDescriptions.Count > 0)
				{
					throw new InvalidOperationException(SR.Get("CannotGroupView", new object[]
					{
						view
					}));
				}
				if (collectionViewLiveShaping != null)
				{
					if (collectionViewLiveShaping.CanChangeLiveSorting)
					{
						collectionViewLiveShaping.IsLiveSorting = new bool?(this.IsLiveSortingRequested);
						ObservableCollection<string> observableCollection = collectionViewLiveShaping.LiveSortingProperties;
						observableCollection.Clear();
						if (this.IsLiveSortingRequested)
						{
							foreach (string item in this.LiveSortingProperties)
							{
								observableCollection.Add(item);
							}
						}
					}
					this.CanChangeLiveSorting = collectionViewLiveShaping.CanChangeLiveSorting;
					this.IsLiveSorting = collectionViewLiveShaping.IsLiveSorting;
					if (collectionViewLiveShaping.CanChangeLiveFiltering)
					{
						collectionViewLiveShaping.IsLiveFiltering = new bool?(this.IsLiveFilteringRequested);
						ObservableCollection<string> observableCollection = collectionViewLiveShaping.LiveFilteringProperties;
						observableCollection.Clear();
						if (this.IsLiveFilteringRequested)
						{
							foreach (string item2 in this.LiveFilteringProperties)
							{
								observableCollection.Add(item2);
							}
						}
					}
					this.CanChangeLiveFiltering = collectionViewLiveShaping.CanChangeLiveFiltering;
					this.IsLiveFiltering = collectionViewLiveShaping.IsLiveFiltering;
					if (collectionViewLiveShaping.CanChangeLiveGrouping)
					{
						collectionViewLiveShaping.IsLiveGrouping = new bool?(this.IsLiveGroupingRequested);
						ObservableCollection<string> observableCollection = collectionViewLiveShaping.LiveGroupingProperties;
						observableCollection.Clear();
						if (this.IsLiveGroupingRequested)
						{
							foreach (string item3 in this.LiveGroupingProperties)
							{
								observableCollection.Add(item3);
							}
						}
					}
					this.CanChangeLiveGrouping = collectionViewLiveShaping.CanChangeLiveGrouping;
					this.IsLiveGrouping = collectionViewLiveShaping.IsLiveGrouping;
				}
				else
				{
					this.CanChangeLiveSorting = false;
					this.IsLiveSorting = null;
					this.CanChangeLiveFiltering = false;
					this.IsLiveFiltering = null;
					this.CanChangeLiveGrouping = false;
					this.IsLiveGrouping = null;
				}
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0007FEF0 File Offset: 0x0007E0F0
		private static ICollectionView GetOriginalView(ICollectionView view)
		{
			for (CollectionViewProxy collectionViewProxy = view as CollectionViewProxy; collectionViewProxy != null; collectionViewProxy = (view as CollectionViewProxy))
			{
				view = collectionViewProxy.ProxiedView;
			}
			return view;
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x06001B20 RID: 6944 RVA: 0x0007FF19 File Offset: 0x0007E119
		private Predicate<object> FilterWrapper
		{
			get
			{
				if (this._filterStub == null)
				{
					this._filterStub = new CollectionViewSource.FilterStub(this);
				}
				return this._filterStub.FilterWrapper;
			}
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0007FF3C File Offset: 0x0007E13C
		private bool WrapFilter(object item)
		{
			FilterEventArgs filterEventArgs = new FilterEventArgs(item);
			FilterEventHandler value = CollectionViewSource.FilterHandlersField.GetValue(this);
			if (value != null)
			{
				value(this, filterEventArgs);
			}
			return filterEventArgs.Accepted;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0007FF6D File Offset: 0x0007E16D
		private void OnDataChanged(object sender, EventArgs e)
		{
			this.EnsureView();
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0007FF75 File Offset: 0x0007E175
		private void OnForwardedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.OnForwardedPropertyChanged();
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0007FF7D File Offset: 0x0007E17D
		private void OnForwardedPropertyChanged()
		{
			this._version++;
			this.ApplyPropertiesToView(this.View);
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0007FF99 File Offset: 0x0007E199
		private void BeginDefer()
		{
			this._deferLevel++;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0007FFAC File Offset: 0x0007E1AC
		private void EndDefer()
		{
			int num = this._deferLevel - 1;
			this._deferLevel = num;
			if (num == 0)
			{
				this.EnsureView();
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x0007FFD2 File Offset: 0x0007E1D2
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x0400137A RID: 4986
		private static readonly DependencyPropertyKey ViewPropertyKey = DependencyProperty.RegisterReadOnly("View", typeof(ICollectionView), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.CollectionViewType" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.View" /> dependency property. </returns>
		// Token: 0x0400137B RID: 4987
		public static readonly DependencyProperty ViewProperty = CollectionViewSource.ViewPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.Source" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.Source" /> dependency property. </returns>
		// Token: 0x0400137C RID: 4988
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(object), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(CollectionViewSource.OnSourceChanged)), new ValidateValueCallback(CollectionViewSource.IsSourceValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.CollectionViewType" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.CollectionViewType" /> dependency property.</returns>
		// Token: 0x0400137D RID: 4989
		public static readonly DependencyProperty CollectionViewTypeProperty = DependencyProperty.Register("CollectionViewType", typeof(Type), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(CollectionViewSource.OnCollectionViewTypeChanged)), new ValidateValueCallback(CollectionViewSource.IsCollectionViewTypeValid));

		// Token: 0x0400137E RID: 4990
		private static readonly DependencyPropertyKey CanChangeLiveSortingPropertyKey = DependencyProperty.RegisterReadOnly("CanChangeLiveSorting", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.CanChangeLiveSorting" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.CanChangeLiveSorting" /> dependency property.</returns>
		// Token: 0x0400137F RID: 4991
		public static readonly DependencyProperty CanChangeLiveSortingProperty = CollectionViewSource.CanChangeLiveSortingPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveSortingRequested" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveSortingRequested" /> dependency property.</returns>
		// Token: 0x04001380 RID: 4992
		public static readonly DependencyProperty IsLiveSortingRequestedProperty = DependencyProperty.Register("IsLiveSortingRequested", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(CollectionViewSource.OnIsLiveSortingRequestedChanged)));

		// Token: 0x04001381 RID: 4993
		private static readonly DependencyPropertyKey IsLiveSortingPropertyKey = DependencyProperty.RegisterReadOnly("IsLiveSorting", typeof(bool?), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveSorting" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveSorting" /> dependency property.</returns>
		// Token: 0x04001382 RID: 4994
		public static readonly DependencyProperty IsLiveSortingProperty = CollectionViewSource.IsLiveSortingPropertyKey.DependencyProperty;

		// Token: 0x04001383 RID: 4995
		private static readonly DependencyPropertyKey CanChangeLiveFilteringPropertyKey = DependencyProperty.RegisterReadOnly("CanChangeLiveFiltering", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.CanChangeLiveFiltering" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.CanChangeLiveFiltering" /> dependency property.</returns>
		// Token: 0x04001384 RID: 4996
		public static readonly DependencyProperty CanChangeLiveFilteringProperty = CollectionViewSource.CanChangeLiveFilteringPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveFilteringRequested" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveFilteringRequested" /> dependency property.</returns>
		// Token: 0x04001385 RID: 4997
		public static readonly DependencyProperty IsLiveFilteringRequestedProperty = DependencyProperty.Register("IsLiveFilteringRequested", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(CollectionViewSource.OnIsLiveFilteringRequestedChanged)));

		// Token: 0x04001386 RID: 4998
		private static readonly DependencyPropertyKey IsLiveFilteringPropertyKey = DependencyProperty.RegisterReadOnly("IsLiveFiltering", typeof(bool?), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveFiltering" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveFiltering" /> dependency property.</returns>
		// Token: 0x04001387 RID: 4999
		public static readonly DependencyProperty IsLiveFilteringProperty = CollectionViewSource.IsLiveFilteringPropertyKey.DependencyProperty;

		// Token: 0x04001388 RID: 5000
		private static readonly DependencyPropertyKey CanChangeLiveGroupingPropertyKey = DependencyProperty.RegisterReadOnly("CanChangeLiveGrouping", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.CanChangeLiveGrouping" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.CanChangeLiveGrouping" /> dependency property.</returns>
		// Token: 0x04001389 RID: 5001
		public static readonly DependencyProperty CanChangeLiveGroupingProperty = CollectionViewSource.CanChangeLiveGroupingPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveGroupingRequested" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveGroupingRequested" /> dependency property.</returns>
		// Token: 0x0400138A RID: 5002
		public static readonly DependencyProperty IsLiveGroupingRequestedProperty = DependencyProperty.Register("IsLiveGroupingRequested", typeof(bool), typeof(CollectionViewSource), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(CollectionViewSource.OnIsLiveGroupingRequestedChanged)));

		// Token: 0x0400138B RID: 5003
		private static readonly DependencyPropertyKey IsLiveGroupingPropertyKey = DependencyProperty.RegisterReadOnly("IsLiveGrouping", typeof(bool?), typeof(CollectionViewSource), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveGrouping" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Data.CollectionViewSource.IsLiveGrouping" /> dependency property.</returns>
		// Token: 0x0400138C RID: 5004
		public static readonly DependencyProperty IsLiveGroupingProperty = CollectionViewSource.IsLiveGroupingPropertyKey.DependencyProperty;

		// Token: 0x0400138D RID: 5005
		private CultureInfo _culture;

		// Token: 0x0400138E RID: 5006
		private SortDescriptionCollection _sort;

		// Token: 0x0400138F RID: 5007
		private ObservableCollection<GroupDescription> _groupBy;

		// Token: 0x04001390 RID: 5008
		private ObservableCollection<string> _liveSortingProperties;

		// Token: 0x04001391 RID: 5009
		private ObservableCollection<string> _liveFilteringProperties;

		// Token: 0x04001392 RID: 5010
		private ObservableCollection<string> _liveGroupingProperties;

		// Token: 0x04001393 RID: 5011
		private bool _isInitializing;

		// Token: 0x04001394 RID: 5012
		private bool _isViewInitialized;

		// Token: 0x04001395 RID: 5013
		private int _version;

		// Token: 0x04001396 RID: 5014
		private int _deferLevel;

		// Token: 0x04001397 RID: 5015
		private DataSourceProvider _dataProvider;

		// Token: 0x04001398 RID: 5016
		private CollectionViewSource.FilterStub _filterStub;

		// Token: 0x04001399 RID: 5017
		private DependencyObject _inheritanceContext;

		// Token: 0x0400139A RID: 5018
		private bool _hasMultipleInheritanceContexts;

		// Token: 0x0400139B RID: 5019
		private DependencyProperty _propertyForInheritanceContext;

		// Token: 0x0400139C RID: 5020
		internal static readonly CollectionViewSource DefaultSource = new CollectionViewSource();

		// Token: 0x0400139D RID: 5021
		private static readonly UncommonField<FilterEventHandler> FilterHandlersField = new UncommonField<FilterEventHandler>();

		// Token: 0x0200087E RID: 2174
		private class DeferHelper : IDisposable
		{
			// Token: 0x06008324 RID: 33572 RVA: 0x00244935 File Offset: 0x00242B35
			public DeferHelper(CollectionViewSource target)
			{
				this._target = target;
				this._target.BeginDefer();
			}

			// Token: 0x06008325 RID: 33573 RVA: 0x00244950 File Offset: 0x00242B50
			public void Dispose()
			{
				if (this._target != null)
				{
					CollectionViewSource target = this._target;
					this._target = null;
					target.EndDefer();
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x0400414C RID: 16716
			private CollectionViewSource _target;
		}

		// Token: 0x0200087F RID: 2175
		private class FilterStub
		{
			// Token: 0x06008326 RID: 33574 RVA: 0x0024497F File Offset: 0x00242B7F
			public FilterStub(CollectionViewSource parent)
			{
				this._parent = new WeakReference(parent);
				this._filterWrapper = new Predicate<object>(this.WrapFilter);
			}

			// Token: 0x17001DB5 RID: 7605
			// (get) Token: 0x06008327 RID: 33575 RVA: 0x002449A5 File Offset: 0x00242BA5
			public Predicate<object> FilterWrapper
			{
				get
				{
					return this._filterWrapper;
				}
			}

			// Token: 0x06008328 RID: 33576 RVA: 0x002449B0 File Offset: 0x00242BB0
			private bool WrapFilter(object item)
			{
				CollectionViewSource collectionViewSource = (CollectionViewSource)this._parent.Target;
				return collectionViewSource == null || collectionViewSource.WrapFilter(item);
			}

			// Token: 0x0400414D RID: 16717
			private WeakReference _parent;

			// Token: 0x0400414E RID: 16718
			private Predicate<object> _filterWrapper;
		}
	}
}
