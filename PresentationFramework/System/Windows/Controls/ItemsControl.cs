using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.Data;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using MS.Win32;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that can be used to present a collection of items.</summary>
	// Token: 0x020004F5 RID: 1269
	[DefaultEvent("OnItemsChanged")]
	[DefaultProperty("Items")]
	[ContentProperty("Items")]
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(FrameworkElement))]
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class ItemsControl : Control, IAddChild, IGeneratorHost, IContainItemStorage
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ItemsControl" /> class.</summary>
		// Token: 0x0600503B RID: 20539 RVA: 0x0016847E File Offset: 0x0016667E
		public ItemsControl()
		{
			ItemsControl.ShouldCoerceCacheSizeField.SetValue(this, true);
			base.CoerceValue(VirtualizingPanel.CacheLengthUnitProperty);
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x001684A8 File Offset: 0x001666A8
		static ItemsControl()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(typeof(ItemsControl)));
			ItemsControl._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ItemsControl));
			EventManager.RegisterClassHandler(typeof(ItemsControl), Keyboard.GotKeyboardFocusEvent, new KeyboardFocusChangedEventHandler(ItemsControl.OnGotFocus));
			VirtualizingPanel.ScrollUnitProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsControl.OnScrollingModeChanged), new CoerceValueCallback(ItemsControl.CoerceScrollingMode)));
			VirtualizingPanel.CacheLengthProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsControl.OnCacheSizeChanged)));
			VirtualizingPanel.CacheLengthUnitProperty.OverrideMetadata(typeof(ItemsControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(ItemsControl.OnCacheSizeChanged), new CoerceValueCallback(ItemsControl.CoerceVirtualizationCacheLengthUnit)));
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x00168922 File Offset: 0x00166B22
		private static void OnScrollingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl.ShouldCoerceScrollUnitField.SetValue(d, true);
			d.CoerceValue(VirtualizingPanel.ScrollUnitProperty);
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x0016893C File Offset: 0x00166B3C
		private static object CoerceScrollingMode(DependencyObject d, object baseValue)
		{
			if (ItemsControl.ShouldCoerceScrollUnitField.GetValue(d))
			{
				ItemsControl.ShouldCoerceScrollUnitField.SetValue(d, false);
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(d, VirtualizingPanel.ScrollUnitProperty).BaseValueSource;
				if (((ItemsControl)d).IsGrouping && baseValueSource == BaseValueSource.Default)
				{
					return ScrollUnit.Pixel;
				}
			}
			return baseValue;
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x0016898F File Offset: 0x00166B8F
		private static void OnCacheSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl.ShouldCoerceCacheSizeField.SetValue(d, true);
			d.CoerceValue(e.Property);
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x001689AC File Offset: 0x00166BAC
		private static object CoerceVirtualizationCacheLengthUnit(DependencyObject d, object baseValue)
		{
			if (ItemsControl.ShouldCoerceCacheSizeField.GetValue(d))
			{
				ItemsControl.ShouldCoerceCacheSizeField.SetValue(d, false);
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(d, VirtualizingPanel.CacheLengthUnitProperty).BaseValueSource;
				if (!((ItemsControl)d).IsGrouping && !(d is TreeView) && baseValueSource == BaseValueSource.Default)
				{
					return VirtualizationCacheLengthUnit.Item;
				}
			}
			return baseValue;
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x00168A08 File Offset: 0x00166C08
		private void CreateItemCollectionAndGenerator()
		{
			this._items = new ItemCollection(this);
			((INotifyCollectionChanged)this._items).CollectionChanged += this.OnItemCollectionChanged1;
			this._itemContainerGenerator = new ItemContainerGenerator(this);
			this._itemContainerGenerator.ChangeAlternationCount();
			((INotifyCollectionChanged)this._items).CollectionChanged += this.OnItemCollectionChanged2;
			if (this.IsInitPending)
			{
				this._items.BeginInit();
			}
			else if (base.IsInitialized)
			{
				this._items.BeginInit();
				this._items.EndInit();
			}
			((INotifyCollectionChanged)this._groupStyle).CollectionChanged += this.OnGroupStyleChanged;
		}

		/// <summary>Gets the collection used to generate the content of the <see cref="T:System.Windows.Controls.ItemsControl" />.</summary>
		/// <returns>The collection that is used to generate the content of the <see cref="T:System.Windows.Controls.ItemsControl" />. The default is an empty collection.</returns>
		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x00168AB0 File Offset: 0x00166CB0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Bindable(true)]
		[CustomCategory("Content")]
		public ItemCollection Items
		{
			get
			{
				if (this._items == null)
				{
					this.CreateItemCollectionAndGenerator();
				}
				return this._items;
			}
		}

		/// <summary>Returns a value that indicates whether serialization processes should serialize the effective value of the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005043 RID: 20547 RVA: 0x00168AC6 File Offset: 0x00166CC6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeItems()
		{
			return this.HasItems;
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x00168AD0 File Offset: 0x00166CD0
		private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			IEnumerable oldValue = (IEnumerable)e.OldValue;
			IEnumerable enumerable = (IEnumerable)e.NewValue;
			((IContainItemStorage)itemsControl).Clear();
			BindingExpressionBase beb = BindingOperations.GetBindingExpressionBase(d, ItemsControl.ItemsSourceProperty);
			if (beb != null)
			{
				itemsControl.Items.SetItemsSource(enumerable, (object x) => beb.GetSourceItem(x));
			}
			else if (e.NewValue != null)
			{
				itemsControl.Items.SetItemsSource(enumerable, null);
			}
			else
			{
				itemsControl.Items.ClearItemsSource();
			}
			itemsControl.OnItemsSourceChanged(oldValue, enumerable);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> property changes.</summary>
		/// <param name="oldValue">Old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> property.</param>
		/// <param name="newValue">New value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> property.</param>
		// Token: 0x06005045 RID: 20549 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
		}

		/// <summary>Gets or sets a collection used to generate the content of the <see cref="T:System.Windows.Controls.ItemsControl" />.  </summary>
		/// <returns>A collection that is used to generate the content of the <see cref="T:System.Windows.Controls.ItemsControl" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x06005046 RID: 20550 RVA: 0x00168B68 File Offset: 0x00166D68
		// (set) Token: 0x06005047 RID: 20551 RVA: 0x00168B75 File Offset: 0x00166D75
		[Bindable(true)]
		[CustomCategory("Content")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IEnumerable ItemsSource
		{
			get
			{
				return this.Items.ItemsSource;
			}
			set
			{
				if (value == null)
				{
					base.ClearValue(ItemsControl.ItemsSourceProperty);
					return;
				}
				base.SetValue(ItemsControl.ItemsSourceProperty, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.ItemContainerGenerator" /> that is associated with the control. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ItemContainerGenerator" /> that is associated with the control. The default is <see langword="null" />.</returns>
		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x06005048 RID: 20552 RVA: 0x00168B92 File Offset: 0x00166D92
		[Bindable(false)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public ItemContainerGenerator ItemContainerGenerator
		{
			get
			{
				if (this._itemContainerGenerator == null)
				{
					this.CreateItemCollectionAndGenerator();
				}
				return this._itemContainerGenerator;
			}
		}

		/// <summary>Gets an enumerator for the logical child objects of the <see cref="T:System.Windows.Controls.ItemsControl" /> object.</summary>
		/// <returns>An enumerator for the logical child objects of the <see cref="T:System.Windows.Controls.ItemsControl" /> object. The default is <see langword="null" />.</returns>
		// Token: 0x1700138F RID: 5007
		// (get) Token: 0x06005049 RID: 20553 RVA: 0x00168BA8 File Offset: 0x00166DA8
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				if (!this.HasItems)
				{
					return EmptyEnumerator.Instance;
				}
				return this.Items.LogicalChildren;
			}
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x00168BC3 File Offset: 0x00166DC3
		private void OnItemCollectionChanged1(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.AdjustItemInfoOverride(e);
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x00168BCC File Offset: 0x00166DCC
		private void OnItemCollectionChanged2(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.SetValue(ItemsControl.HasItemsPropertyKey, this._items != null && !this._items.IsEmpty);
			if (this._focusedInfo != null && this._focusedInfo.Index < 0)
			{
				this._focusedInfo = null;
			}
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				((IContainItemStorage)this).Clear();
			}
			this.OnItemsChanged(e);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> property changes.</summary>
		/// <param name="e">Information about the change.</param>
		// Token: 0x0600504C RID: 20556 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x00168C36 File Offset: 0x00166E36
		internal virtual void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			this.AdjustItemInfo(e, this._focusedInfo);
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.ItemsControl" /> contains items.  </summary>
		/// <returns>
		///     <see langword="true" /> if the items count is greater than 0; otherwise, <see langword="false" />.The default is <see langword="false" />.</returns>
		// Token: 0x17001390 RID: 5008
		// (get) Token: 0x0600504E RID: 20558 RVA: 0x00168C45 File Offset: 0x00166E45
		[Bindable(false)]
		[Browsable(false)]
		public bool HasItems
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.HasItemsProperty);
			}
		}

		/// <summary>Gets or sets a path to a value on the source object to serve as the visual representation of the object.  </summary>
		/// <returns>The path to a value on the source object. This can be any path, or an XPath such as "@Name". The default is an empty string ("").</returns>
		// Token: 0x17001391 RID: 5009
		// (get) Token: 0x0600504F RID: 20559 RVA: 0x00168C57 File Offset: 0x00166E57
		// (set) Token: 0x06005050 RID: 20560 RVA: 0x00168C69 File Offset: 0x00166E69
		[Bindable(true)]
		[CustomCategory("Content")]
		public string DisplayMemberPath
		{
			get
			{
				return (string)base.GetValue(ItemsControl.DisplayMemberPathProperty);
			}
			set
			{
				base.SetValue(ItemsControl.DisplayMemberPathProperty, value);
			}
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x00168C78 File Offset: 0x00166E78
		private static void OnDisplayMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			itemsControl.OnDisplayMemberPathChanged((string)e.OldValue, (string)e.NewValue);
			itemsControl.UpdateDisplayMemberTemplateSelector();
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x00168CB0 File Offset: 0x00166EB0
		private void UpdateDisplayMemberTemplateSelector()
		{
			string displayMemberPath = this.DisplayMemberPath;
			string itemStringFormat = this.ItemStringFormat;
			if (string.IsNullOrEmpty(displayMemberPath) && string.IsNullOrEmpty(itemStringFormat))
			{
				if (this.ItemTemplateSelector is DisplayMemberTemplateSelector)
				{
					base.ClearValue(ItemsControl.ItemTemplateSelectorProperty);
				}
				return;
			}
			DataTemplateSelector itemTemplateSelector = this.ItemTemplateSelector;
			if (itemTemplateSelector != null && !(itemTemplateSelector is DisplayMemberTemplateSelector) && (base.ReadLocalValue(ItemsControl.ItemTemplateSelectorProperty) != DependencyProperty.UnsetValue || base.ReadLocalValue(ItemsControl.DisplayMemberPathProperty) == DependencyProperty.UnsetValue))
			{
				throw new InvalidOperationException(SR.Get("DisplayMemberPathAndItemTemplateSelectorDefined"));
			}
			this.ItemTemplateSelector = new DisplayMemberTemplateSelector(this.DisplayMemberPath, this.ItemStringFormat);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.DisplayMemberPath" /> property changes.</summary>
		/// <param name="oldDisplayMemberPath">The old value of the <see cref="P:System.Windows.Controls.ItemsControl.DisplayMemberPath" /> property.</param>
		/// <param name="newDisplayMemberPath">New value of the <see cref="P:System.Windows.Controls.ItemsControl.DisplayMemberPath" /> property.</param>
		// Token: 0x06005053 RID: 20563 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnDisplayMemberPathChanged(string oldDisplayMemberPath, string newDisplayMemberPath)
		{
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.DataTemplate" /> used to display each item.  </summary>
		/// <returns>A <see cref="T:System.Windows.DataTemplate" /> that specifies the visualization of the data objects. The default is <see langword="null" />.</returns>
		// Token: 0x17001392 RID: 5010
		// (get) Token: 0x06005054 RID: 20564 RVA: 0x00168D51 File Offset: 0x00166F51
		// (set) Token: 0x06005055 RID: 20565 RVA: 0x00168D63 File Offset: 0x00166F63
		[Bindable(true)]
		[CustomCategory("Content")]
		public DataTemplate ItemTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(ItemsControl.ItemTemplateProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemTemplateProperty, value);
			}
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x00168D71 File Offset: 0x00166F71
		private static void OnItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplate" /> property changes.</summary>
		/// <param name="oldItemTemplate">The old <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplate" /> property value.</param>
		/// <param name="newItemTemplate">The new <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplate" /> property value.</param>
		// Token: 0x06005057 RID: 20567 RVA: 0x00168D96 File Offset: 0x00166F96
		protected virtual void OnItemTemplateChanged(DataTemplate oldItemTemplate, DataTemplate newItemTemplate)
		{
			this.CheckTemplateSource();
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		/// <summary>Gets or sets the custom logic for choosing a template used to display each item.  </summary>
		/// <returns>A custom <see cref="T:System.Windows.Controls.DataTemplateSelector" /> object that provides logic and returns a <see cref="T:System.Windows.DataTemplate" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x06005058 RID: 20568 RVA: 0x00168DB1 File Offset: 0x00166FB1
		// (set) Token: 0x06005059 RID: 20569 RVA: 0x00168DC3 File Offset: 0x00166FC3
		[Bindable(true)]
		[CustomCategory("Content")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector ItemTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(ItemsControl.ItemTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemTemplateSelectorProperty, value);
			}
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x00168DD1 File Offset: 0x00166FD1
		private static void OnItemTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplateSelector" /> property changes.</summary>
		/// <param name="oldItemTemplateSelector">Old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplateSelector" /> property.</param>
		/// <param name="newItemTemplateSelector">New value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplateSelector" /> property.</param>
		// Token: 0x0600505B RID: 20571 RVA: 0x00168DF6 File Offset: 0x00166FF6
		protected virtual void OnItemTemplateSelectorChanged(DataTemplateSelector oldItemTemplateSelector, DataTemplateSelector newItemTemplateSelector)
		{
			this.CheckTemplateSource();
			if (this._itemContainerGenerator != null && this.ItemTemplate == null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		/// <summary>Gets or sets a composite string that specifies how to format the items in the <see cref="T:System.Windows.Controls.ItemsControl" /> if they are displayed as strings.</summary>
		/// <returns>A composite string that specifies how to format the items in the <see cref="T:System.Windows.Controls.ItemsControl" /> if they are displayed as strings.</returns>
		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x0600505C RID: 20572 RVA: 0x00168E19 File Offset: 0x00167019
		// (set) Token: 0x0600505D RID: 20573 RVA: 0x00168E2B File Offset: 0x0016702B
		[Bindable(true)]
		[CustomCategory("Content")]
		public string ItemStringFormat
		{
			get
			{
				return (string)base.GetValue(ItemsControl.ItemStringFormatProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemStringFormatProperty, value);
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x00168E3C File Offset: 0x0016703C
		private static void OnItemStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			itemsControl.OnItemStringFormatChanged((string)e.OldValue, (string)e.NewValue);
			itemsControl.UpdateDisplayMemberTemplateSelector();
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemStringFormat" /> property changes.</summary>
		/// <param name="oldItemStringFormat">The old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemStringFormat" /> property.</param>
		/// <param name="newItemStringFormat">The new value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemStringFormat" /> property.</param>
		// Token: 0x0600505F RID: 20575 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnItemStringFormatChanged(string oldItemStringFormat, string newItemStringFormat)
		{
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Data.BindingGroup" /> that is copied to each item in the <see cref="T:System.Windows.Controls.ItemsControl" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.BindingGroup" /> that is copied to each item in the <see cref="T:System.Windows.Controls.ItemsControl" />.</returns>
		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x06005060 RID: 20576 RVA: 0x00168E74 File Offset: 0x00167074
		// (set) Token: 0x06005061 RID: 20577 RVA: 0x00168E86 File Offset: 0x00167086
		[Bindable(true)]
		[CustomCategory("Content")]
		public BindingGroup ItemBindingGroup
		{
			get
			{
				return (BindingGroup)base.GetValue(ItemsControl.ItemBindingGroupProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemBindingGroupProperty, value);
			}
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x00168E94 File Offset: 0x00167094
		private static void OnItemBindingGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			itemsControl.OnItemBindingGroupChanged((BindingGroup)e.OldValue, (BindingGroup)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemBindingGroup" /> property changes.</summary>
		/// <param name="oldItemBindingGroup">The old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemBindingGroup" />.</param>
		/// <param name="newItemBindingGroup">The new value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemBindingGroup" />.</param>
		// Token: 0x06005063 RID: 20579 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnItemBindingGroupChanged(BindingGroup oldItemBindingGroup, BindingGroup newItemBindingGroup)
		{
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x00168EC8 File Offset: 0x001670C8
		private void CheckTemplateSource()
		{
			if (string.IsNullOrEmpty(this.DisplayMemberPath))
			{
				Helper.CheckTemplateAndTemplateSelector("Item", ItemsControl.ItemTemplateProperty, ItemsControl.ItemTemplateSelectorProperty, this);
				return;
			}
			if (!(this.ItemTemplateSelector is DisplayMemberTemplateSelector))
			{
				throw new InvalidOperationException(SR.Get("ItemTemplateSelectorBreaksDisplayMemberPath"));
			}
			if (Helper.IsTemplateDefined(ItemsControl.ItemTemplateProperty, this))
			{
				throw new InvalidOperationException(SR.Get("DisplayMemberPathAndItemTemplateDefined"));
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Style" /> that is applied to the container element generated for each item.  </summary>
		/// <returns>The <see cref="T:System.Windows.Style" /> that is applied to the container element generated for each item. The default is <see langword="null" />.</returns>
		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x06005065 RID: 20581 RVA: 0x00168F32 File Offset: 0x00167132
		// (set) Token: 0x06005066 RID: 20582 RVA: 0x00168F44 File Offset: 0x00167144
		[Bindable(true)]
		[Category("Content")]
		public Style ItemContainerStyle
		{
			get
			{
				return (Style)base.GetValue(ItemsControl.ItemContainerStyleProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemContainerStyleProperty, value);
			}
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x00168F52 File Offset: 0x00167152
		private static void OnItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemContainerStyleChanged((Style)e.OldValue, (Style)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyle" /> property changes.</summary>
		/// <param name="oldItemContainerStyle">Old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyle" /> property.</param>
		/// <param name="newItemContainerStyle">New value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyle" /> property.</param>
		// Token: 0x06005068 RID: 20584 RVA: 0x00168F77 File Offset: 0x00167177
		protected virtual void OnItemContainerStyleChanged(Style oldItemContainerStyle, Style newItemContainerStyle)
		{
			Helper.CheckStyleAndStyleSelector("ItemContainer", ItemsControl.ItemContainerStyleProperty, ItemsControl.ItemContainerStyleSelectorProperty, this);
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		/// <summary>Gets or sets custom style-selection logic for a style that can be applied to each generated container element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.StyleSelector" /> object that contains logic that chooses the style to use as the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyle" />. The default is <see langword="null" />.</returns>
		// Token: 0x17001397 RID: 5015
		// (get) Token: 0x06005069 RID: 20585 RVA: 0x00168FA1 File Offset: 0x001671A1
		// (set) Token: 0x0600506A RID: 20586 RVA: 0x00168FB3 File Offset: 0x001671B3
		[Bindable(true)]
		[Category("Content")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StyleSelector ItemContainerStyleSelector
		{
			get
			{
				return (StyleSelector)base.GetValue(ItemsControl.ItemContainerStyleSelectorProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemContainerStyleSelectorProperty, value);
			}
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x00168FC1 File Offset: 0x001671C1
		private static void OnItemContainerStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemContainerStyleSelectorChanged((StyleSelector)e.OldValue, (StyleSelector)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyleSelector" /> property changes.</summary>
		/// <param name="oldItemContainerStyleSelector">Old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyleSelector" /> property.</param>
		/// <param name="newItemContainerStyleSelector">New value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyleSelector" /> property.</param>
		// Token: 0x0600506C RID: 20588 RVA: 0x00168FE6 File Offset: 0x001671E6
		protected virtual void OnItemContainerStyleSelectorChanged(StyleSelector oldItemContainerStyleSelector, StyleSelector newItemContainerStyleSelector)
		{
			Helper.CheckStyleAndStyleSelector("ItemContainer", ItemsControl.ItemContainerStyleProperty, ItemsControl.ItemContainerStyleSelectorProperty, this);
			if (this._itemContainerGenerator != null && this.ItemContainerStyle == null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Controls.ItemsControl" /> that the specified element hosts items for.</summary>
		/// <param name="element">The host element.</param>
		/// <returns>The <see cref="T:System.Windows.Controls.ItemsControl" /> that the specified element hosts items for, or <see langword="null" />.</returns>
		// Token: 0x0600506D RID: 20589 RVA: 0x00169018 File Offset: 0x00167218
		public static ItemsControl GetItemsOwner(DependencyObject element)
		{
			ItemsControl result = null;
			Panel panel = element as Panel;
			if (panel != null && panel.IsItemsHost)
			{
				ItemsPresenter itemsPresenter = ItemsPresenter.FromPanel(panel);
				if (itemsPresenter != null)
				{
					result = itemsPresenter.Owner;
				}
				else
				{
					result = (panel.TemplatedParent as ItemsControl);
				}
			}
			return result;
		}

		// Token: 0x0600506E RID: 20590 RVA: 0x0016905C File Offset: 0x0016725C
		internal static DependencyObject GetItemsOwnerInternal(DependencyObject element)
		{
			ItemsControl itemsControl;
			return ItemsControl.GetItemsOwnerInternal(element, out itemsControl);
		}

		// Token: 0x0600506F RID: 20591 RVA: 0x00169074 File Offset: 0x00167274
		internal static DependencyObject GetItemsOwnerInternal(DependencyObject element, out ItemsControl itemsControl)
		{
			DependencyObject dependencyObject = null;
			Panel panel = element as Panel;
			itemsControl = null;
			if (panel != null && panel.IsItemsHost)
			{
				ItemsPresenter itemsPresenter = ItemsPresenter.FromPanel(panel);
				if (itemsPresenter != null)
				{
					dependencyObject = itemsPresenter.TemplatedParent;
					itemsControl = itemsPresenter.Owner;
				}
				else
				{
					dependencyObject = panel.TemplatedParent;
					itemsControl = (dependencyObject as ItemsControl);
				}
			}
			return dependencyObject;
		}

		// Token: 0x06005070 RID: 20592 RVA: 0x001690C4 File Offset: 0x001672C4
		private static ItemsPanelTemplate GetDefaultItemsPanelTemplate()
		{
			ItemsPanelTemplate itemsPanelTemplate = new ItemsPanelTemplate(new FrameworkElementFactory(typeof(StackPanel)));
			itemsPanelTemplate.Seal();
			return itemsPanelTemplate;
		}

		/// <summary>Gets or sets the template that defines the panel that controls the layout of items.  </summary>
		/// <returns>An <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> that defines the panel to use for the layout of the items. The default value for the <see cref="T:System.Windows.Controls.ItemsControl" /> is an <see cref="T:System.Windows.Controls.ItemsPanelTemplate" /> that specifies a <see cref="T:System.Windows.Controls.StackPanel" />.</returns>
		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06005071 RID: 20593 RVA: 0x001690ED File Offset: 0x001672ED
		// (set) Token: 0x06005072 RID: 20594 RVA: 0x001690FF File Offset: 0x001672FF
		[Bindable(false)]
		public ItemsPanelTemplate ItemsPanel
		{
			get
			{
				return (ItemsPanelTemplate)base.GetValue(ItemsControl.ItemsPanelProperty);
			}
			set
			{
				base.SetValue(ItemsControl.ItemsPanelProperty, value);
			}
		}

		// Token: 0x06005073 RID: 20595 RVA: 0x0016910D File Offset: 0x0016730D
		private static void OnItemsPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnItemsPanelChanged((ItemsPanelTemplate)e.OldValue, (ItemsPanelTemplate)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemsPanel" /> property changes.</summary>
		/// <param name="oldItemsPanel">Old value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemsPanel" /> property.</param>
		/// <param name="newItemsPanel">New value of the <see cref="P:System.Windows.Controls.ItemsControl.ItemsPanel" /> property.</param>
		// Token: 0x06005074 RID: 20596 RVA: 0x00169132 File Offset: 0x00167332
		protected virtual void OnItemsPanelChanged(ItemsPanelTemplate oldItemsPanel, ItemsPanelTemplate newItemsPanel)
		{
			this.ItemContainerGenerator.OnPanelChanged();
		}

		/// <summary>Gets a value that indicates whether the control is using grouping.  </summary>
		/// <returns>
		///     <see langword="true" /> if a control is using grouping; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06005075 RID: 20597 RVA: 0x0016913F File Offset: 0x0016733F
		[Bindable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsGrouping
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.IsGroupingProperty);
			}
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x00169151 File Offset: 0x00167351
		private static void OnIsGroupingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnIsGroupingChanged(e);
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x0016915F File Offset: 0x0016735F
		internal virtual void OnIsGroupingChanged(DependencyPropertyChangedEventArgs e)
		{
			ItemsControl.ShouldCoerceScrollUnitField.SetValue(this, true);
			base.CoerceValue(VirtualizingPanel.ScrollUnitProperty);
			ItemsControl.ShouldCoerceCacheSizeField.SetValue(this, true);
			base.CoerceValue(VirtualizingPanel.CacheLengthUnitProperty);
			((IContainItemStorage)this).Clear();
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.Controls.GroupStyle" /> objects that define the appearance of each level of groups.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.Controls.GroupStyle" /> objects that define the appearance of each level of groups.</returns>
		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06005078 RID: 20600 RVA: 0x00169195 File Offset: 0x00167395
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ObservableCollection<GroupStyle> GroupStyle
		{
			get
			{
				return this._groupStyle;
			}
		}

		/// <summary>Returns a value that indicates whether serialization processes should serialize the effective value of the <see cref="P:System.Windows.Controls.ItemsControl.GroupStyle" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.ItemsControl.GroupStyle" /> property value should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005079 RID: 20601 RVA: 0x0016919D File Offset: 0x0016739D
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeGroupStyle()
		{
			return this.GroupStyle.Count > 0;
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x001691AD File Offset: 0x001673AD
		private void OnGroupStyleChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		/// <summary>Gets or sets a method that enables you to provide custom selection logic for a <see cref="T:System.Windows.Controls.GroupStyle" /> to apply to each group in a collection.  </summary>
		/// <returns>A method that enables you to provide custom selection logic for a <see cref="T:System.Windows.Controls.GroupStyle" /> to apply to each group in a collection.</returns>
		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x0600507B RID: 20603 RVA: 0x001691C2 File Offset: 0x001673C2
		// (set) Token: 0x0600507C RID: 20604 RVA: 0x001691D4 File Offset: 0x001673D4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Bindable(true)]
		[CustomCategory("Content")]
		public GroupStyleSelector GroupStyleSelector
		{
			get
			{
				return (GroupStyleSelector)base.GetValue(ItemsControl.GroupStyleSelectorProperty);
			}
			set
			{
				base.SetValue(ItemsControl.GroupStyleSelectorProperty, value);
			}
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x001691E2 File Offset: 0x001673E2
		private static void OnGroupStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((ItemsControl)d).OnGroupStyleSelectorChanged((GroupStyleSelector)e.OldValue, (GroupStyleSelector)e.NewValue);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.GroupStyleSelector" /> property changes.</summary>
		/// <param name="oldGroupStyleSelector">Old value of the <see cref="P:System.Windows.Controls.ItemsControl.GroupStyleSelector" /> property.</param>
		/// <param name="newGroupStyleSelector">New value of the <see cref="P:System.Windows.Controls.ItemsControl.GroupStyleSelector" /> property.</param>
		// Token: 0x0600507E RID: 20606 RVA: 0x001691AD File Offset: 0x001673AD
		protected virtual void OnGroupStyleSelectorChanged(GroupStyleSelector oldGroupStyleSelector, GroupStyleSelector newGroupStyleSelector)
		{
			if (this._itemContainerGenerator != null)
			{
				this._itemContainerGenerator.Refresh();
			}
		}

		/// <summary>Gets or sets the number of alternating item containers in the <see cref="T:System.Windows.Controls.ItemsControl" />, which enables alternating containers to have a unique appearance. </summary>
		/// <returns>The number of alternating item containers in the <see cref="T:System.Windows.Controls.ItemsControl" />. </returns>
		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x0600507F RID: 20607 RVA: 0x00169207 File Offset: 0x00167407
		// (set) Token: 0x06005080 RID: 20608 RVA: 0x00169219 File Offset: 0x00167419
		[Bindable(true)]
		[CustomCategory("Content")]
		public int AlternationCount
		{
			get
			{
				return (int)base.GetValue(ItemsControl.AlternationCountProperty);
			}
			set
			{
				base.SetValue(ItemsControl.AlternationCountProperty, value);
			}
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x0016922C File Offset: 0x0016742C
		private static void OnAlternationCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)d;
			int oldAlternationCount = (int)e.OldValue;
			int newAlternationCount = (int)e.NewValue;
			itemsControl.OnAlternationCountChanged(oldAlternationCount, newAlternationCount);
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.AlternationCount" /> property changes.</summary>
		/// <param name="oldAlternationCount">The old value of <see cref="P:System.Windows.Controls.ItemsControl.AlternationCount" />.</param>
		/// <param name="newAlternationCount">The new value of <see cref="P:System.Windows.Controls.ItemsControl.AlternationCount" />.</param>
		// Token: 0x06005082 RID: 20610 RVA: 0x00169262 File Offset: 0x00167462
		protected virtual void OnAlternationCountChanged(int oldAlternationCount, int newAlternationCount)
		{
			this.ItemContainerGenerator.ChangeAlternationCount();
		}

		/// <summary>Gets the <see cref="P:System.Windows.Controls.ItemsControl.AlternationIndex" /> for the specified object.</summary>
		/// <param name="element">The object from which to get the <see cref="P:System.Windows.Controls.ItemsControl.AlternationIndex" />.</param>
		/// <returns>The value of the <see cref="P:System.Windows.Controls.ItemsControl.AlternationIndex" />.</returns>
		// Token: 0x06005083 RID: 20611 RVA: 0x0016926F File Offset: 0x0016746F
		public static int GetAlternationIndex(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (int)element.GetValue(ItemsControl.AlternationIndexProperty);
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x0016928F File Offset: 0x0016748F
		internal static void SetAlternationIndex(DependencyObject d, int value)
		{
			d.SetValue(ItemsControl.AlternationIndexPropertyKey, value);
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001692A2 File Offset: 0x001674A2
		internal static void ClearAlternationIndex(DependencyObject d)
		{
			d.ClearValue(ItemsControl.AlternationIndexPropertyKey);
		}

		/// <summary>Gets or sets a value that indicates whether <see cref="T:System.Windows.Controls.TextSearch" /> is enabled on the <see cref="T:System.Windows.Controls.ItemsControl" /> instance.  </summary>
		/// <returns>
		///     <see langword="true" /> if <see cref="T:System.Windows.Controls.TextSearch" /> is enabled; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06005086 RID: 20614 RVA: 0x001692AF File Offset: 0x001674AF
		// (set) Token: 0x06005087 RID: 20615 RVA: 0x001692C1 File Offset: 0x001674C1
		public bool IsTextSearchEnabled
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.IsTextSearchEnabledProperty);
			}
			set
			{
				base.SetValue(ItemsControl.IsTextSearchEnabledProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Gets or sets a value that indicates whether case is a condition when searching for items.</summary>
		/// <returns>
		///     <see langword="true" /> if text searches are case-sensitive; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06005088 RID: 20616 RVA: 0x001692D4 File Offset: 0x001674D4
		// (set) Token: 0x06005089 RID: 20617 RVA: 0x001692E6 File Offset: 0x001674E6
		public bool IsTextSearchCaseSensitive
		{
			get
			{
				return (bool)base.GetValue(ItemsControl.IsTextSearchCaseSensitiveProperty);
			}
			set
			{
				base.SetValue(ItemsControl.IsTextSearchCaseSensitiveProperty, BooleanBoxes.Box(value));
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Controls.ItemsControl" /> that owns the specified container element.</summary>
		/// <param name="container">The container element to return the <see cref="T:System.Windows.Controls.ItemsControl" /> for.</param>
		/// <returns>The <see cref="T:System.Windows.Controls.ItemsControl" /> that owns the specified container element.</returns>
		// Token: 0x0600508A RID: 20618 RVA: 0x001692FC File Offset: 0x001674FC
		public static ItemsControl ItemsControlFromItemContainer(DependencyObject container)
		{
			UIElement uielement = container as UIElement;
			if (uielement == null)
			{
				return null;
			}
			ItemsControl itemsControl = LogicalTreeHelper.GetParent(uielement) as ItemsControl;
			if (itemsControl == null)
			{
				uielement = (VisualTreeHelper.GetParent(uielement) as UIElement);
				return ItemsControl.GetItemsOwner(uielement);
			}
			IGeneratorHost generatorHost = itemsControl;
			if (generatorHost.IsItemItsOwnContainer(uielement))
			{
				return itemsControl;
			}
			return null;
		}

		/// <summary>Returns the container that belongs to the specified <see cref="T:System.Windows.Controls.ItemsControl" /> that owns the given container element.</summary>
		/// <param name="itemsControl">The <see cref="T:System.Windows.Controls.ItemsControl" /> to return the container for.</param>
		/// <param name="element">The element to return the container for.</param>
		/// <returns>The container that belongs to the specified <see cref="T:System.Windows.Controls.ItemsControl" /> that owns the given element, if <paramref name="itemsControl" /> is not <see langword="null" />. If <paramref name="itemsControl" /> is <see langword="null" />, returns the closest container that belongs to any <see cref="T:System.Windows.Controls.ItemsControl" />.</returns>
		// Token: 0x0600508B RID: 20619 RVA: 0x00169348 File Offset: 0x00167548
		public static DependencyObject ContainerFromElement(ItemsControl itemsControl, DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (ItemsControl.IsContainerForItemsControl(element, itemsControl))
			{
				return element;
			}
			FrameworkObject frameworkObject = new FrameworkObject(element);
			frameworkObject.Reset(frameworkObject.GetPreferVisualParent(true).DO);
			while (frameworkObject.DO != null && !ItemsControl.IsContainerForItemsControl(frameworkObject.DO, itemsControl))
			{
				frameworkObject.Reset(frameworkObject.PreferVisualParent.DO);
			}
			return frameworkObject.DO;
		}

		/// <summary>Returns the container that belongs to the current <see cref="T:System.Windows.Controls.ItemsControl" /> that owns the given element.</summary>
		/// <param name="element">The element to return the container for.</param>
		/// <returns>The container that belongs to the current <see cref="T:System.Windows.Controls.ItemsControl" /> that owns the given element or <see langword="null" /> if no such container exists.</returns>
		// Token: 0x0600508C RID: 20620 RVA: 0x001693C4 File Offset: 0x001675C4
		public DependencyObject ContainerFromElement(DependencyObject element)
		{
			return ItemsControl.ContainerFromElement(this, element);
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x001693CD File Offset: 0x001675CD
		private static bool IsContainerForItemsControl(DependencyObject element, ItemsControl itemsControl)
		{
			return element.ContainsValue(ItemContainerGenerator.ItemForItemContainerProperty) && (itemsControl == null || itemsControl == ItemsControl.ItemsControlFromItemContainer(element));
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="value">The object to add as a child.</param>
		// Token: 0x0600508E RID: 20622 RVA: 0x001693EB File Offset: 0x001675EB
		void IAddChild.AddChild(object value)
		{
			this.AddChild(value);
		}

		/// <summary>Adds the specified object as the child of the <see cref="T:System.Windows.Controls.ItemsControl" /> object. </summary>
		/// <param name="value">The object to add as a child.</param>
		// Token: 0x0600508F RID: 20623 RVA: 0x001693F4 File Offset: 0x001675F4
		protected virtual void AddChild(object value)
		{
			this.Items.Add(value);
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="text">The text to add.</param>
		// Token: 0x06005090 RID: 20624 RVA: 0x00169403 File Offset: 0x00167603
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		/// <summary>Adds the specified text string to the <see cref="T:System.Windows.Controls.ItemsControl" /> object.</summary>
		/// <param name="text">The string to add.</param>
		// Token: 0x06005091 RID: 20625 RVA: 0x001693F4 File Offset: 0x001675F4
		protected virtual void AddText(string text)
		{
			this.Items.Add(text);
		}

		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06005092 RID: 20626 RVA: 0x0013C760 File Offset: 0x0013A960
		ItemCollection IGeneratorHost.View
		{
			get
			{
				return this.Items;
			}
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x0016940C File Offset: 0x0016760C
		bool IGeneratorHost.IsItemItsOwnContainer(object item)
		{
			return this.IsItemItsOwnContainer(item);
		}

		// Token: 0x06005094 RID: 20628 RVA: 0x00169418 File Offset: 0x00167618
		DependencyObject IGeneratorHost.GetContainerForItem(object item)
		{
			DependencyObject dependencyObject;
			if (this.IsItemItsOwnContainerOverride(item))
			{
				dependencyObject = (item as DependencyObject);
			}
			else
			{
				dependencyObject = this.GetContainerForItemOverride();
			}
			Visual visual = dependencyObject as Visual;
			if (visual != null)
			{
				Visual visual2 = VisualTreeHelper.GetParent(visual) as Visual;
				if (visual2 != null)
				{
					Invariant.Assert(visual2 is FrameworkElement, SR.Get("ItemsControl_ParentNotFrameworkElement"));
					Panel panel = visual2 as Panel;
					if (panel != null && visual is UIElement)
					{
						panel.Children.RemoveNoVerify((UIElement)visual);
					}
					else
					{
						((FrameworkElement)visual2).TemplateChild = null;
					}
				}
			}
			return dependencyObject;
		}

		// Token: 0x06005095 RID: 20629 RVA: 0x001694A4 File Offset: 0x001676A4
		void IGeneratorHost.PrepareItemContainer(DependencyObject container, object item)
		{
			GroupItem groupItem = container as GroupItem;
			if (groupItem != null)
			{
				groupItem.PrepareItemContainer(item, this);
				return;
			}
			if (this.ShouldApplyItemContainerStyle(container, item))
			{
				this.ApplyItemContainerStyle(container, item);
			}
			this.PrepareContainerForItemOverride(container, item);
			if (!Helper.HasUnmodifiedDefaultValue(this, ItemsControl.ItemBindingGroupProperty) && Helper.HasUnmodifiedDefaultOrInheritedValue(container, FrameworkElement.BindingGroupProperty))
			{
				BindingGroup itemBindingGroup = this.ItemBindingGroup;
				BindingGroup value = (itemBindingGroup != null) ? new BindingGroup(itemBindingGroup) : null;
				container.SetValue(FrameworkElement.BindingGroupProperty, value);
			}
			if (container == item && TraceData.IsEnabled && (this.ItemTemplate != null || this.ItemTemplateSelector != null))
			{
				TraceData.Trace(TraceEventType.Error, TraceData.ItemTemplateForDirectItem, AvTrace.TypeName(item));
			}
			TreeViewItem treeViewItem = container as TreeViewItem;
			if (treeViewItem != null)
			{
				treeViewItem.PrepareItemContainer(item, this);
			}
		}

		// Token: 0x06005096 RID: 20630 RVA: 0x00169558 File Offset: 0x00167758
		void IGeneratorHost.ClearContainerForItem(DependencyObject container, object item)
		{
			GroupItem groupItem = container as GroupItem;
			if (groupItem == null)
			{
				this.ClearContainerForItemOverride(container, item);
				TreeViewItem treeViewItem = container as TreeViewItem;
				if (treeViewItem != null)
				{
					treeViewItem.ClearItemContainer(item, this);
					return;
				}
			}
			else
			{
				groupItem.ClearItemContainer(item, this);
			}
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x00169594 File Offset: 0x00167794
		bool IGeneratorHost.IsHostForItemContainer(DependencyObject container)
		{
			ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);
			if (itemsControl != null)
			{
				return itemsControl == this;
			}
			return LogicalTreeHelper.GetParent(container) == null && (this.IsItemItsOwnContainerOverride(container) && this.HasItems) && this.Items.Contains(container);
		}

		// Token: 0x06005098 RID: 20632 RVA: 0x001695DC File Offset: 0x001677DC
		GroupStyle IGeneratorHost.GetGroupStyle(CollectionViewGroup group, int level)
		{
			GroupStyle groupStyle = null;
			if (this.GroupStyleSelector != null)
			{
				groupStyle = this.GroupStyleSelector(group, level);
			}
			if (groupStyle == null)
			{
				if (level >= this.GroupStyle.Count)
				{
					level = this.GroupStyle.Count - 1;
				}
				if (level >= 0)
				{
					groupStyle = this.GroupStyle[level];
				}
			}
			return groupStyle;
		}

		// Token: 0x06005099 RID: 20633 RVA: 0x00169633 File Offset: 0x00167833
		void IGeneratorHost.SetIsGrouping(bool isGrouping)
		{
			base.SetValue(ItemsControl.IsGroupingPropertyKey, BooleanBoxes.Box(isGrouping));
		}

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x0600509A RID: 20634 RVA: 0x00169646 File Offset: 0x00167846
		int IGeneratorHost.AlternationCount
		{
			get
			{
				return this.AlternationCount;
			}
		}

		/// <summary>Indicates that the initialization of the <see cref="T:System.Windows.Controls.ItemsControl" /> object is about to start.</summary>
		// Token: 0x0600509B RID: 20635 RVA: 0x0016964E File Offset: 0x0016784E
		public override void BeginInit()
		{
			base.BeginInit();
			if (this._items != null)
			{
				this._items.BeginInit();
			}
		}

		/// <summary>Indicates that the initialization of the <see cref="T:System.Windows.Controls.ItemsControl" /> object is complete.</summary>
		// Token: 0x0600509C RID: 20636 RVA: 0x00169669 File Offset: 0x00167869
		public override void EndInit()
		{
			if (this.IsInitPending)
			{
				if (this._items != null)
				{
					this._items.EndInit();
				}
				base.EndInit();
			}
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x0600509D RID: 20637 RVA: 0x0016968C File Offset: 0x0016788C
		private bool IsInitPending
		{
			get
			{
				return base.ReadInternalFlag(InternalFlags.InitPending);
			}
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own container.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the item is (or is eligible to be) its own container; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600509E RID: 20638 RVA: 0x00169699 File Offset: 0x00167899
		public bool IsItemItsOwnContainer(object item)
		{
			return this.IsItemItsOwnContainerOverride(item);
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own container.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the item is (or is eligible to be) its own container; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600509F RID: 20639 RVA: 0x001696A2 File Offset: 0x001678A2
		protected virtual bool IsItemItsOwnContainerOverride(object item)
		{
			return item is UIElement;
		}

		/// <summary>Creates or identifies the element that is used to display the given item.</summary>
		/// <returns>The element that is used to display the given item.</returns>
		// Token: 0x060050A0 RID: 20640 RVA: 0x001696AD File Offset: 0x001678AD
		protected virtual DependencyObject GetContainerForItemOverride()
		{
			return new ContentPresenter();
		}

		/// <summary>Prepares the specified element to display the specified item. </summary>
		/// <param name="element">Element used to display the specified item.</param>
		/// <param name="item">Specified item.</param>
		// Token: 0x060050A1 RID: 20641 RVA: 0x001696B4 File Offset: 0x001678B4
		protected virtual void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			HeaderedContentControl headeredContentControl;
			if ((headeredContentControl = (element as HeaderedContentControl)) != null)
			{
				headeredContentControl.PrepareHeaderedContentControl(item, this.ItemTemplate, this.ItemTemplateSelector, this.ItemStringFormat);
				return;
			}
			ContentControl contentControl;
			if ((contentControl = (element as ContentControl)) != null)
			{
				contentControl.PrepareContentControl(item, this.ItemTemplate, this.ItemTemplateSelector, this.ItemStringFormat);
				return;
			}
			ContentPresenter contentPresenter;
			if ((contentPresenter = (element as ContentPresenter)) != null)
			{
				contentPresenter.PrepareContentPresenter(item, this.ItemTemplate, this.ItemTemplateSelector, this.ItemStringFormat);
				return;
			}
			HeaderedItemsControl headeredItemsControl;
			if ((headeredItemsControl = (element as HeaderedItemsControl)) != null)
			{
				headeredItemsControl.PrepareHeaderedItemsControl(item, this);
				return;
			}
			ItemsControl itemsControl;
			if ((itemsControl = (element as ItemsControl)) != null && itemsControl != this)
			{
				itemsControl.PrepareItemsControl(item, this);
			}
		}

		/// <summary>When overridden in a derived class, undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)" /> method.</summary>
		/// <param name="element">The container element.</param>
		/// <param name="item">The item.</param>
		// Token: 0x060050A2 RID: 20642 RVA: 0x00169758 File Offset: 0x00167958
		protected virtual void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			HeaderedContentControl headeredContentControl;
			if ((headeredContentControl = (element as HeaderedContentControl)) != null)
			{
				headeredContentControl.ClearHeaderedContentControl(item);
				return;
			}
			ContentControl contentControl;
			if ((contentControl = (element as ContentControl)) != null)
			{
				contentControl.ClearContentControl(item);
				return;
			}
			ContentPresenter contentPresenter;
			if ((contentPresenter = (element as ContentPresenter)) != null)
			{
				contentPresenter.ClearContentPresenter(item);
				return;
			}
			HeaderedItemsControl headeredItemsControl;
			if ((headeredItemsControl = (element as HeaderedItemsControl)) != null)
			{
				headeredItemsControl.ClearHeaderedItemsControl(item);
				return;
			}
			ItemsControl itemsControl;
			if ((itemsControl = (element as ItemsControl)) != null && itemsControl != this)
			{
				itemsControl.ClearItemsControl(item);
			}
		}

		/// <summary>Invoked when the <see cref="E:System.Windows.UIElement.TextInput" /> event is received.</summary>
		/// <param name="e">Information about the event.</param>
		// Token: 0x060050A3 RID: 20643 RVA: 0x001697C4 File Offset: 0x001679C4
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (!string.IsNullOrEmpty(e.Text) && this.IsTextSearchEnabled && (e.OriginalSource == this || ItemsControl.ItemsControlFromItemContainer(e.OriginalSource as DependencyObject) == this))
			{
				TextSearch textSearch = TextSearch.EnsureInstance(this);
				if (textSearch != null)
				{
					textSearch.DoSearch(e.Text);
					e.Handled = true;
				}
			}
		}

		/// <summary>Invoked when the <see cref="E:System.Windows.UIElement.KeyDown" /> event is received.</summary>
		/// <param name="e">Information about the event.</param>
		// Token: 0x060050A4 RID: 20644 RVA: 0x00169828 File Offset: 0x00167A28
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (this.IsTextSearchEnabled && e.Key == Key.Back)
			{
				TextSearch textSearch = TextSearch.EnsureInstance(this);
				if (textSearch != null)
				{
					textSearch.DeleteLastCharacter();
				}
			}
		}

		// Token: 0x060050A5 RID: 20645 RVA: 0x0016985E File Offset: 0x00167A5E
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this._itemsHost = null;
			this._scrollHost = null;
			base.WriteControlFlag(Control.ControlBoolFlags.ScrollHostValid, false);
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
		}

		/// <summary>Returns a value that indicates whether to apply the style from the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyle" /> or <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyleSelector" /> property to the container element of the specified item.</summary>
		/// <param name="container">The container element.</param>
		/// <param name="item">The item of interest.</param>
		/// <returns>Always <see langword="true" /> for the base implementation.</returns>
		// Token: 0x060050A6 RID: 20646 RVA: 0x00016748 File Offset: 0x00014948
		protected virtual bool ShouldApplyItemContainerStyle(DependencyObject container, object item)
		{
			return true;
		}

		// Token: 0x060050A7 RID: 20647 RVA: 0x00169880 File Offset: 0x00167A80
		internal void PrepareItemsControl(object item, ItemsControl parentItemsControl)
		{
			if (item != this)
			{
				DataTemplate itemTemplate = parentItemsControl.ItemTemplate;
				DataTemplateSelector itemTemplateSelector = parentItemsControl.ItemTemplateSelector;
				string itemStringFormat = parentItemsControl.ItemStringFormat;
				Style itemContainerStyle = parentItemsControl.ItemContainerStyle;
				StyleSelector itemContainerStyleSelector = parentItemsControl.ItemContainerStyleSelector;
				int alternationCount = parentItemsControl.AlternationCount;
				BindingGroup itemBindingGroup = parentItemsControl.ItemBindingGroup;
				if (itemTemplate != null)
				{
					base.SetValue(ItemsControl.ItemTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(ItemsControl.ItemTemplateSelectorProperty, itemTemplateSelector);
				}
				if (itemStringFormat != null && Helper.HasDefaultValue(this, ItemsControl.ItemStringFormatProperty))
				{
					base.SetValue(ItemsControl.ItemStringFormatProperty, itemStringFormat);
				}
				if (itemContainerStyle != null && Helper.HasDefaultValue(this, ItemsControl.ItemContainerStyleProperty))
				{
					base.SetValue(ItemsControl.ItemContainerStyleProperty, itemContainerStyle);
				}
				if (itemContainerStyleSelector != null && Helper.HasDefaultValue(this, ItemsControl.ItemContainerStyleSelectorProperty))
				{
					base.SetValue(ItemsControl.ItemContainerStyleSelectorProperty, itemContainerStyleSelector);
				}
				if (alternationCount != 0 && Helper.HasDefaultValue(this, ItemsControl.AlternationCountProperty))
				{
					base.SetValue(ItemsControl.AlternationCountProperty, alternationCount);
				}
				if (itemBindingGroup != null && Helper.HasDefaultValue(this, ItemsControl.ItemBindingGroupProperty))
				{
					base.SetValue(ItemsControl.ItemBindingGroupProperty, itemBindingGroup);
				}
			}
		}

		// Token: 0x060050A8 RID: 20648 RVA: 0x0016997D File Offset: 0x00167B7D
		internal void ClearItemsControl(object item)
		{
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x00169984 File Offset: 0x00167B84
		internal object OnBringItemIntoView(object arg)
		{
			ItemsControl.ItemInfo itemInfo = arg as ItemsControl.ItemInfo;
			if (itemInfo == null)
			{
				itemInfo = this.NewItemInfo(arg, null, -1);
			}
			return this.OnBringItemIntoView(itemInfo);
		}

		// Token: 0x060050AA RID: 20650 RVA: 0x001699B4 File Offset: 0x00167BB4
		internal object OnBringItemIntoView(ItemsControl.ItemInfo info)
		{
			FrameworkElement frameworkElement = info.Container as FrameworkElement;
			if (frameworkElement != null)
			{
				frameworkElement.BringIntoView();
			}
			else if ((info = this.LeaseItemInfo(info, true)).Index >= 0)
			{
				if (!FrameworkCompatibilityPreferences.GetVSP45Compat())
				{
					base.UpdateLayout();
				}
				VirtualizingPanel virtualizingPanel = this.ItemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.BringIndexIntoView(info.Index);
				}
			}
			return null;
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x060050AB RID: 20651 RVA: 0x00169A14 File Offset: 0x00167C14
		// (set) Token: 0x060050AC RID: 20652 RVA: 0x00169A1C File Offset: 0x00167C1C
		internal Panel ItemsHost
		{
			get
			{
				return this._itemsHost;
			}
			set
			{
				this._itemsHost = value;
			}
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x00169A28 File Offset: 0x00167C28
		internal bool NavigateByLine(FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			DependencyObject dependencyObject = Keyboard.FocusedElement as DependencyObject;
			if (!FrameworkAppContextSwitches.KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement)
			{
				while (dependencyObject != null && !(dependencyObject is FrameworkElement))
				{
					dependencyObject = KeyboardNavigation.GetParent(dependencyObject);
				}
			}
			return this.NavigateByLine(this.FocusedInfo, dependencyObject as FrameworkElement, direction, itemNavigateArgs);
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x00169A70 File Offset: 0x00167C70
		internal void PrepareNavigateByLine(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost == null)
			{
				return;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			this.NavigateByLineInternal(startingItem, direction, startingElement, itemNavigateArgs, false, out container);
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x00169AC4 File Offset: 0x00167CC4
		internal bool NavigateByLine(ItemsControl.ItemInfo startingInfo, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			return this.NavigateByLine(startingInfo, null, direction, itemNavigateArgs);
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x00169AD0 File Offset: 0x00167CD0
		internal bool NavigateByLine(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			if (this.ItemsHost == null)
			{
				return false;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			FrameworkElement frameworkElement;
			return this.NavigateByLineInternal(startingItem, direction, startingElement, itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x00169B20 File Offset: 0x00167D20
		private bool NavigateByLineInternal(object startingItem, FocusNavigationDirection direction, FrameworkElement startingElement, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (startingItem == null && (startingElement == null || startingElement == this))
			{
				return this.NavigateToStartInternal(itemNavigateArgs, shouldFocus, out container);
			}
			if (startingElement == null || !this.ItemsHost.IsAncestorOf(startingElement))
			{
				startingElement = this.ScrollHost;
			}
			else
			{
				DependencyObject parent = VisualTreeHelper.GetParent(startingElement);
				while (parent != null && parent != this.ItemsHost)
				{
					KeyboardNavigationMode directionalNavigation = KeyboardNavigation.GetDirectionalNavigation(parent);
					if (directionalNavigation == KeyboardNavigationMode.Contained || directionalNavigation == KeyboardNavigationMode.Cycle)
					{
						return false;
					}
					parent = VisualTreeHelper.GetParent(parent);
				}
			}
			bool flag = this.ItemsHost != null && this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
			bool treeViewNavigation = this is TreeView;
			FrameworkElement frameworkElement = KeyboardNavigation.Current.PredictFocusedElement(startingElement, direction, treeViewNavigation) as FrameworkElement;
			if (this.ScrollHost != null)
			{
				bool flag2 = false;
				FrameworkElement viewportElement = this.GetViewportElement();
				VirtualizingPanel virtualizingPanel = this.ItemsHost as VirtualizingPanel;
				bool flag3 = KeyboardNavigation.GetDirectionalNavigation(this) == KeyboardNavigationMode.Cycle;
				for (;;)
				{
					if (frameworkElement != null)
					{
						if (virtualizingPanel == null || !this.ScrollHost.CanContentScroll || !VirtualizingPanel.GetIsVirtualizing(this))
						{
							goto IL_27F;
						}
						Rect toRect;
						ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportElement, ItemsControl.TryGetTreeViewItemHeader(frameworkElement) as FrameworkElement, direction, false, out toRect);
						if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport || elementViewportPosition == ElementViewportPosition.PartiallyInViewport)
						{
							if (!flag3)
							{
								goto IL_27F;
							}
							Rect fromRect;
							ItemsControl.GetElementViewportPosition(viewportElement, startingElement, direction, false, out fromRect);
							if (this.IsInDirectionForLineNavigation(fromRect, toRect, direction, flag))
							{
								goto IL_27F;
							}
						}
						frameworkElement = null;
					}
					double horizontalOffset = this.ScrollHost.HorizontalOffset;
					double verticalOffset = this.ScrollHost.VerticalOffset;
					if (direction != FocusNavigationDirection.Up)
					{
						if (direction == FocusNavigationDirection.Down)
						{
							flag2 = true;
							if (flag)
							{
								this.ScrollHost.LineRight();
							}
							else
							{
								this.ScrollHost.LineDown();
							}
						}
					}
					else
					{
						flag2 = true;
						if (flag)
						{
							this.ScrollHost.LineLeft();
						}
						else
						{
							this.ScrollHost.LineUp();
						}
					}
					this.ScrollHost.UpdateLayout();
					if ((DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset)) || (direction == FocusNavigationDirection.Down && (this.ScrollHost.VerticalOffset > this.ScrollHost.ExtentHeight || this.ScrollHost.HorizontalOffset > this.ScrollHost.ExtentWidth)) || (direction == FocusNavigationDirection.Up && (this.ScrollHost.VerticalOffset < 0.0 || this.ScrollHost.HorizontalOffset < 0.0)))
					{
						break;
					}
					frameworkElement = (KeyboardNavigation.Current.PredictFocusedElement(startingElement, direction, treeViewNavigation) as FrameworkElement);
				}
				if (flag3)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						return this.NavigateToEndInternal(itemNavigateArgs, true, out container);
					}
					if (direction == FocusNavigationDirection.Down)
					{
						return this.NavigateToStartInternal(itemNavigateArgs, true, out container);
					}
				}
				IL_27F:
				if (flag2 && frameworkElement != null && this.ItemsHost.IsAncestorOf(frameworkElement))
				{
					this.AdjustOffsetToAlignWithEdge(frameworkElement, direction);
				}
			}
			if (frameworkElement != null && this.ItemsHost.IsAncestorOf(frameworkElement))
			{
				ItemsControl itemsControl = null;
				object encapsulatingItem = ItemsControl.GetEncapsulatingItem(frameworkElement, out container, out itemsControl);
				container = frameworkElement;
				if (!shouldFocus)
				{
					return false;
				}
				if (encapsulatingItem == DependencyProperty.UnsetValue || encapsulatingItem is CollectionViewGroupInternal)
				{
					return frameworkElement.Focus();
				}
				if (itemsControl != null)
				{
					return itemsControl.FocusItem(this.NewItemInfo(encapsulatingItem, container, -1), itemNavigateArgs);
				}
			}
			return false;
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x00169E28 File Offset: 0x00168028
		internal void PrepareToNavigateByPage(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost == null)
			{
				return;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			this.NavigateByPageInternal(startingItem, direction, startingElement, itemNavigateArgs, false, out container);
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x00169E7C File Offset: 0x0016807C
		internal bool NavigateByPage(FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			return this.NavigateByPage(this.FocusedInfo, Keyboard.FocusedElement as FrameworkElement, direction, itemNavigateArgs);
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x00169E96 File Offset: 0x00168096
		internal bool NavigateByPage(ItemsControl.ItemInfo startingInfo, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			return this.NavigateByPage(startingInfo, null, direction, itemNavigateArgs);
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x00169EA4 File Offset: 0x001680A4
		internal bool NavigateByPage(ItemsControl.ItemInfo startingInfo, FrameworkElement startingElement, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			if (this.ItemsHost == null)
			{
				return false;
			}
			if (startingElement != null)
			{
				this.MakeVisible(startingElement, direction, false);
			}
			else
			{
				this.MakeVisible(startingInfo, direction, out startingElement);
			}
			object startingItem = (startingInfo != null) ? startingInfo.Item : null;
			FrameworkElement frameworkElement;
			return this.NavigateByPageInternal(startingItem, direction, startingElement, itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x00169EF4 File Offset: 0x001680F4
		private bool NavigateByPageInternal(object startingItem, FocusNavigationDirection direction, FrameworkElement startingElement, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (startingItem == null && (startingElement == null || startingElement == this))
			{
				return this.NavigateToFirstItemOnCurrentPage(startingItem, direction, itemNavigateArgs, shouldFocus, out container);
			}
			FrameworkElement frameworkElement;
			object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, direction, out frameworkElement);
			if ((object.Equals(startingItem, firstItemOnCurrentPage) || object.Equals(startingElement, frameworkElement)) && this.ScrollHost != null)
			{
				bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
				do
				{
					double horizontalOffset = this.ScrollHost.HorizontalOffset;
					double verticalOffset = this.ScrollHost.VerticalOffset;
					if (direction != FocusNavigationDirection.Up)
					{
						if (direction == FocusNavigationDirection.Down)
						{
							if (flag)
							{
								this.ScrollHost.PageRight();
							}
							else
							{
								this.ScrollHost.PageDown();
							}
						}
					}
					else if (flag)
					{
						this.ScrollHost.PageLeft();
					}
					else
					{
						this.ScrollHost.PageUp();
					}
					this.ScrollHost.UpdateLayout();
					if (DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset))
					{
						break;
					}
					firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, direction, out frameworkElement);
				}
				while (firstItemOnCurrentPage == DependencyProperty.UnsetValue);
			}
			container = frameworkElement;
			if (shouldFocus)
			{
				if (frameworkElement != null && (firstItemOnCurrentPage == DependencyProperty.UnsetValue || firstItemOnCurrentPage is CollectionViewGroupInternal))
				{
					return frameworkElement.Focus();
				}
				ItemsControl encapsulatingItemsControl = ItemsControl.GetEncapsulatingItemsControl(frameworkElement);
				if (encapsulatingItemsControl != null)
				{
					return encapsulatingItemsControl.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, frameworkElement, -1), itemNavigateArgs);
				}
			}
			return false;
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x0016A050 File Offset: 0x00168250
		internal void NavigateToStart(ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			FrameworkElement frameworkElement;
			this.NavigateToStartInternal(itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x0016A068 File Offset: 0x00168268
		internal bool NavigateToStartInternal(ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost != null)
			{
				if (this.ScrollHost != null)
				{
					bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
					double horizontalOffset;
					double verticalOffset;
					do
					{
						horizontalOffset = this.ScrollHost.HorizontalOffset;
						verticalOffset = this.ScrollHost.VerticalOffset;
						if (flag)
						{
							this.ScrollHost.ScrollToLeftEnd();
						}
						else
						{
							this.ScrollHost.ScrollToTop();
						}
						this.ItemsHost.UpdateLayout();
					}
					while (!DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) || !DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset));
				}
				FrameworkElement startingElement = this.FindEndFocusableLeafContainer(this.ItemsHost, false);
				FrameworkElement frameworkElement;
				object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, FocusNavigationDirection.Up, out frameworkElement);
				container = frameworkElement;
				if (shouldFocus)
				{
					if (frameworkElement != null && (firstItemOnCurrentPage == DependencyProperty.UnsetValue || firstItemOnCurrentPage is CollectionViewGroupInternal))
					{
						return frameworkElement.Focus();
					}
					ItemsControl encapsulatingItemsControl = ItemsControl.GetEncapsulatingItemsControl(frameworkElement);
					if (encapsulatingItemsControl != null)
					{
						return encapsulatingItemsControl.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, frameworkElement, -1), itemNavigateArgs);
					}
				}
			}
			return false;
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x0016A184 File Offset: 0x00168384
		internal void NavigateToEnd(ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			FrameworkElement frameworkElement;
			this.NavigateToEndInternal(itemNavigateArgs, true, out frameworkElement);
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x0016A19C File Offset: 0x0016839C
		internal bool NavigateToEndInternal(ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			container = null;
			if (this.ItemsHost != null)
			{
				if (this.ScrollHost != null)
				{
					bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
					double horizontalOffset;
					double verticalOffset;
					do
					{
						horizontalOffset = this.ScrollHost.HorizontalOffset;
						verticalOffset = this.ScrollHost.VerticalOffset;
						if (flag)
						{
							this.ScrollHost.ScrollToRightEnd();
						}
						else
						{
							this.ScrollHost.ScrollToBottom();
						}
						this.ItemsHost.UpdateLayout();
					}
					while (!DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) || !DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset));
				}
				FrameworkElement startingElement = this.FindEndFocusableLeafContainer(this.ItemsHost, true);
				FrameworkElement frameworkElement;
				object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(startingElement, FocusNavigationDirection.Down, out frameworkElement);
				container = frameworkElement;
				if (shouldFocus)
				{
					if (frameworkElement != null && (firstItemOnCurrentPage == DependencyProperty.UnsetValue || firstItemOnCurrentPage is CollectionViewGroupInternal))
					{
						return frameworkElement.Focus();
					}
					ItemsControl encapsulatingItemsControl = ItemsControl.GetEncapsulatingItemsControl(frameworkElement);
					if (encapsulatingItemsControl != null)
					{
						return encapsulatingItemsControl.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, frameworkElement, -1), itemNavigateArgs);
					}
				}
			}
			return false;
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x0016A2B8 File Offset: 0x001684B8
		private FrameworkElement FindEndFocusableLeafContainer(Panel itemsHost, bool last)
		{
			if (itemsHost == null)
			{
				return null;
			}
			UIElementCollection children = itemsHost.Children;
			if (children != null)
			{
				int count = children.Count;
				int num = last ? (count - 1) : 0;
				int num2 = last ? -1 : 1;
				while (num >= 0 && num < count)
				{
					FrameworkElement frameworkElement = children[num] as FrameworkElement;
					if (frameworkElement != null)
					{
						ItemsControl itemsControl = frameworkElement as ItemsControl;
						FrameworkElement frameworkElement2 = null;
						if (itemsControl != null)
						{
							if (itemsControl.ItemsHost != null)
							{
								frameworkElement2 = this.FindEndFocusableLeafContainer(itemsControl.ItemsHost, last);
							}
						}
						else
						{
							GroupItem groupItem = frameworkElement as GroupItem;
							if (groupItem != null && groupItem.ItemsHost != null)
							{
								frameworkElement2 = this.FindEndFocusableLeafContainer(groupItem.ItemsHost, last);
							}
						}
						if (frameworkElement2 != null)
						{
							return frameworkElement2;
						}
						if (FrameworkElement.KeyboardNavigation.IsFocusableInternal(frameworkElement))
						{
							return frameworkElement;
						}
					}
					num += num2;
				}
			}
			return null;
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x0016A37D File Offset: 0x0016857D
		internal void NavigateToItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool alwaysAtTopOfViewport = false)
		{
			if (info != null)
			{
				this.NavigateToItem(info.Item, info.Index, itemNavigateArgs, alwaysAtTopOfViewport);
			}
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x0016A39C File Offset: 0x0016859C
		internal void NavigateToItem(object item, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			this.NavigateToItem(item, -1, itemNavigateArgs, false);
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x0016A3A8 File Offset: 0x001685A8
		internal void NavigateToItem(object item, int itemIndex, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			this.NavigateToItem(item, itemIndex, itemNavigateArgs, false);
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x0016A3B4 File Offset: 0x001685B4
		internal void NavigateToItem(object item, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool alwaysAtTopOfViewport)
		{
			this.NavigateToItem(item, -1, itemNavigateArgs, alwaysAtTopOfViewport);
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x0016A3C0 File Offset: 0x001685C0
		private void NavigateToItem(object item, int elementIndex, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool alwaysAtTopOfViewport)
		{
			if (item == DependencyProperty.UnsetValue)
			{
				return;
			}
			if (elementIndex == -1)
			{
				elementIndex = this.Items.IndexOf(item);
				if (elementIndex == -1)
				{
					return;
				}
			}
			bool flag = false;
			if (this.ItemsHost != null)
			{
				flag = (this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal);
			}
			FocusNavigationDirection direction = flag ? FocusNavigationDirection.Right : FocusNavigationDirection.Down;
			FrameworkElement container;
			this.MakeVisible(elementIndex, direction, alwaysAtTopOfViewport, out container);
			this.FocusItem(this.NewItemInfo(item, container, -1), itemNavigateArgs);
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x0016A43C File Offset: 0x0016863C
		private object FindFocusable(int startIndex, int direction, out int foundIndex, out FrameworkElement foundContainer)
		{
			if (this.HasItems)
			{
				int count = this.Items.Count;
				while (startIndex >= 0 && startIndex < count)
				{
					FrameworkElement frameworkElement = this.ItemContainerGenerator.ContainerFromIndex(startIndex) as FrameworkElement;
					if (frameworkElement == null || Keyboard.IsFocusable(frameworkElement))
					{
						foundIndex = startIndex;
						foundContainer = frameworkElement;
						return this.Items[startIndex];
					}
					startIndex += direction;
				}
			}
			foundIndex = -1;
			foundContainer = null;
			return null;
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x0016A4A8 File Offset: 0x001686A8
		private void AdjustOffsetToAlignWithEdge(FrameworkElement element, FocusNavigationDirection direction)
		{
			if (VirtualizingPanel.GetScrollUnit(this) != ScrollUnit.Item)
			{
				ScrollViewer scrollHost = this.ScrollHost;
				FrameworkElement viewportElement = this.GetViewportElement();
				element = (ItemsControl.TryGetTreeViewItemHeader(element) as FrameworkElement);
				Rect rect = new Rect(default(Point), element.RenderSize);
				rect = element.TransformToAncestor(viewportElement).TransformBounds(rect);
				bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
				if (direction == FocusNavigationDirection.Down)
				{
					if (flag)
					{
						scrollHost.ScrollToHorizontalOffset(scrollHost.HorizontalOffset - scrollHost.ViewportWidth + rect.Right);
						return;
					}
					scrollHost.ScrollToVerticalOffset(scrollHost.VerticalOffset - scrollHost.ViewportHeight + rect.Bottom);
					return;
				}
				else if (direction == FocusNavigationDirection.Up)
				{
					if (flag)
					{
						scrollHost.ScrollToHorizontalOffset(scrollHost.HorizontalOffset + rect.Left);
						return;
					}
					scrollHost.ScrollToVerticalOffset(scrollHost.VerticalOffset + rect.Top);
				}
			}
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x0016A590 File Offset: 0x00168790
		private void MakeVisible(int index, FocusNavigationDirection direction, bool alwaysAtTopOfViewport, out FrameworkElement container)
		{
			container = null;
			if (index >= 0)
			{
				container = (this.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement);
				if (container == null)
				{
					VirtualizingPanel virtualizingPanel = this.ItemsHost as VirtualizingPanel;
					if (virtualizingPanel != null)
					{
						virtualizingPanel.BringIndexIntoView(index);
						base.UpdateLayout();
						container = (this.ItemContainerGenerator.ContainerFromIndex(index) as FrameworkElement);
					}
				}
				this.MakeVisible(container, direction, alwaysAtTopOfViewport);
			}
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x0016A5F9 File Offset: 0x001687F9
		private void MakeVisible(ItemsControl.ItemInfo info, FocusNavigationDirection direction, out FrameworkElement container)
		{
			if (info != null)
			{
				this.MakeVisible(info.Index, direction, false, out container);
				info.Container = container;
				return;
			}
			this.MakeVisible(-1, direction, false, out container);
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x0016A628 File Offset: 0x00168828
		internal void MakeVisible(FrameworkElement container, FocusNavigationDirection direction, bool alwaysAtTopOfViewport)
		{
			if (this.ScrollHost != null && this.ItemsHost != null)
			{
				FrameworkElement viewportElement = this.GetViewportElement();
				while (container != null && !this.IsOnCurrentPage(viewportElement, container, direction, false))
				{
					double horizontalOffset = this.ScrollHost.HorizontalOffset;
					double verticalOffset = this.ScrollHost.VerticalOffset;
					container.BringIntoView();
					this.ItemsHost.UpdateLayout();
					if (DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset))
					{
						break;
					}
				}
				if (container != null && alwaysAtTopOfViewport)
				{
					bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
					FrameworkElement frameworkElement;
					this.GetFirstItemOnCurrentPage(container, FocusNavigationDirection.Up, out frameworkElement);
					while (frameworkElement != container)
					{
						double horizontalOffset = this.ScrollHost.HorizontalOffset;
						double verticalOffset = this.ScrollHost.VerticalOffset;
						if (flag)
						{
							this.ScrollHost.LineRight();
						}
						else
						{
							this.ScrollHost.LineDown();
						}
						this.ScrollHost.UpdateLayout();
						if (DoubleUtil.AreClose(horizontalOffset, this.ScrollHost.HorizontalOffset) && DoubleUtil.AreClose(verticalOffset, this.ScrollHost.VerticalOffset))
						{
							break;
						}
						this.GetFirstItemOnCurrentPage(container, FocusNavigationDirection.Up, out frameworkElement);
					}
				}
			}
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x0016A760 File Offset: 0x00168960
		private bool NavigateToFirstItemOnCurrentPage(object startingItem, FocusNavigationDirection direction, ItemsControl.ItemNavigateArgs itemNavigateArgs, bool shouldFocus, out FrameworkElement container)
		{
			object firstItemOnCurrentPage = this.GetFirstItemOnCurrentPage(this.ItemContainerGenerator.ContainerFromItem(startingItem) as FrameworkElement, direction, out container);
			return firstItemOnCurrentPage != DependencyProperty.UnsetValue && shouldFocus && this.FocusItem(this.NewItemInfo(firstItemOnCurrentPage, container, -1), itemNavigateArgs);
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x0016A7A8 File Offset: 0x001689A8
		private object GetFirstItemOnCurrentPage(FrameworkElement startingElement, FocusNavigationDirection direction, out FrameworkElement firstElement)
		{
			bool flag = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal;
			bool flag2 = this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Vertical;
			if (this.ScrollHost != null && this.ScrollHost.CanContentScroll && VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item && !(this is TreeView) && !this.IsGrouping)
			{
				int num = -1;
				if (flag2)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						return this.FindFocusable((int)this.ScrollHost.VerticalOffset, 1, out num, out firstElement);
					}
					return this.FindFocusable((int)(this.ScrollHost.VerticalOffset + Math.Max(this.ScrollHost.ViewportHeight - 1.0, 0.0)), -1, out num, out firstElement);
				}
				else if (flag)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						return this.FindFocusable((int)this.ScrollHost.HorizontalOffset, 1, out num, out firstElement);
					}
					return this.FindFocusable((int)(this.ScrollHost.HorizontalOffset + Math.Max(this.ScrollHost.ViewportWidth - 1.0, 0.0)), -1, out num, out firstElement);
				}
			}
			if (startingElement != null)
			{
				if (flag)
				{
					if (direction == FocusNavigationDirection.Up)
					{
						direction = FocusNavigationDirection.Left;
					}
					else if (direction == FocusNavigationDirection.Down)
					{
						direction = FocusNavigationDirection.Right;
					}
				}
				FrameworkElement viewportElement = this.GetViewportElement();
				bool treeViewNavigation = this is TreeView;
				FrameworkElement frameworkElement = KeyboardNavigation.Current.PredictFocusedElementAtViewportEdge(startingElement, direction, treeViewNavigation, viewportElement, viewportElement) as FrameworkElement;
				object obj = null;
				firstElement = null;
				if (frameworkElement != null)
				{
					obj = ItemsControl.GetEncapsulatingItem(frameworkElement, out firstElement);
				}
				if (frameworkElement == null || obj == DependencyProperty.UnsetValue)
				{
					ElementViewportPosition elementViewportPosition = ItemsControl.GetElementViewportPosition(viewportElement, startingElement, direction, false);
					if (elementViewportPosition == ElementViewportPosition.CompletelyInViewport || elementViewportPosition == ElementViewportPosition.PartiallyInViewport)
					{
						frameworkElement = startingElement;
						obj = ItemsControl.GetEncapsulatingItem(frameworkElement, out firstElement);
					}
				}
				if (obj != null && obj is CollectionViewGroupInternal)
				{
					firstElement = frameworkElement;
				}
				return obj;
			}
			firstElement = null;
			return null;
		}

		// Token: 0x060050C8 RID: 20680 RVA: 0x0016A980 File Offset: 0x00168B80
		internal FrameworkElement GetViewportElement()
		{
			FrameworkElement frameworkElement = this.ScrollHost;
			if (frameworkElement == null)
			{
				frameworkElement = this.ItemsHost;
			}
			else
			{
				ScrollContentPresenter scrollContentPresenter = frameworkElement.GetTemplateChild("PART_ScrollContentPresenter") as ScrollContentPresenter;
				if (scrollContentPresenter != null)
				{
					frameworkElement = scrollContentPresenter;
				}
			}
			return frameworkElement;
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x0016A9B8 File Offset: 0x00168BB8
		private bool IsOnCurrentPage(object item, FocusNavigationDirection axis)
		{
			FrameworkElement frameworkElement = this.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
			return frameworkElement != null && ItemsControl.GetElementViewportPosition(this.GetViewportElement(), frameworkElement, axis, false) == ElementViewportPosition.CompletelyInViewport;
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x0016A9ED File Offset: 0x00168BED
		private bool IsOnCurrentPage(FrameworkElement element, FocusNavigationDirection axis)
		{
			return ItemsControl.GetElementViewportPosition(this.GetViewportElement(), element, axis, false) == ElementViewportPosition.CompletelyInViewport;
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x0016AA00 File Offset: 0x00168C00
		private bool IsOnCurrentPage(FrameworkElement viewPort, FrameworkElement element, FocusNavigationDirection axis, bool fullyVisible)
		{
			return ItemsControl.GetElementViewportPosition(viewPort, element, axis, fullyVisible) == ElementViewportPosition.CompletelyInViewport;
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x0016AA10 File Offset: 0x00168C10
		internal static ElementViewportPosition GetElementViewportPosition(FrameworkElement viewPort, UIElement element, FocusNavigationDirection axis, bool fullyVisible)
		{
			Rect rect;
			return ItemsControl.GetElementViewportPosition(viewPort, element, axis, fullyVisible, out rect);
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x0016AA28 File Offset: 0x00168C28
		internal static ElementViewportPosition GetElementViewportPosition(FrameworkElement viewPort, UIElement element, FocusNavigationDirection axis, bool fullyVisible, out Rect elementRect)
		{
			return ItemsControl.GetElementViewportPosition(viewPort, element, axis, fullyVisible, false, out elementRect);
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x0016AA38 File Offset: 0x00168C38
		internal static ElementViewportPosition GetElementViewportPosition(FrameworkElement viewPort, UIElement element, FocusNavigationDirection axis, bool fullyVisible, bool ignorePerpendicularAxis, out Rect elementRect)
		{
			elementRect = Rect.Empty;
			if (viewPort == null)
			{
				return ElementViewportPosition.None;
			}
			if (element == null || !viewPort.IsAncestorOf(element))
			{
				return ElementViewportPosition.None;
			}
			Rect viewportRect = new Rect(default(Point), viewPort.RenderSize);
			Rect rect = new Rect(default(Point), element.RenderSize);
			rect = ItemsControl.CorrectCatastrophicCancellation(element.TransformToAncestor(viewPort)).TransformBounds(rect);
			bool flag = axis == FocusNavigationDirection.Up || axis == FocusNavigationDirection.Down;
			bool flag2 = axis == FocusNavigationDirection.Left || axis == FocusNavigationDirection.Right;
			elementRect = rect;
			if (ignorePerpendicularAxis)
			{
				if (flag)
				{
					viewportRect = new Rect(double.NegativeInfinity, viewportRect.Top, double.PositiveInfinity, viewportRect.Height);
				}
				else if (flag2)
				{
					viewportRect = new Rect(viewportRect.Left, double.NegativeInfinity, viewportRect.Width, double.PositiveInfinity);
				}
			}
			if (fullyVisible)
			{
				if (viewportRect.Contains(rect))
				{
					return ElementViewportPosition.CompletelyInViewport;
				}
			}
			else if (flag)
			{
				if (DoubleUtil.LessThanOrClose(viewportRect.Top, rect.Top) && DoubleUtil.LessThanOrClose(rect.Bottom, viewportRect.Bottom))
				{
					return ElementViewportPosition.CompletelyInViewport;
				}
			}
			else if (flag2 && DoubleUtil.LessThanOrClose(viewportRect.Left, rect.Left) && DoubleUtil.LessThanOrClose(rect.Right, viewportRect.Right))
			{
				return ElementViewportPosition.CompletelyInViewport;
			}
			if (ItemsControl.ElementIntersectsViewport(viewportRect, rect))
			{
				return ElementViewportPosition.PartiallyInViewport;
			}
			if ((flag && DoubleUtil.LessThanOrClose(rect.Bottom, viewportRect.Top)) || (flag2 && DoubleUtil.LessThanOrClose(rect.Right, viewportRect.Left)))
			{
				return ElementViewportPosition.BeforeViewport;
			}
			if ((flag && DoubleUtil.LessThanOrClose(viewportRect.Bottom, rect.Top)) || (flag2 && DoubleUtil.LessThanOrClose(viewportRect.Right, rect.Left)))
			{
				return ElementViewportPosition.AfterViewport;
			}
			return ElementViewportPosition.None;
		}

		// Token: 0x060050CF RID: 20687 RVA: 0x0016ABFC File Offset: 0x00168DFC
		private static GeneralTransform CorrectCatastrophicCancellation(GeneralTransform transform)
		{
			MatrixTransform matrixTransform = transform as MatrixTransform;
			if (matrixTransform != null)
			{
				bool flag = false;
				Matrix matrix = matrixTransform.Matrix;
				if (matrix.OffsetX != 0.0 && LayoutDoubleUtil.AreClose(matrix.OffsetX, 0.0))
				{
					matrix.OffsetX = 0.0;
					flag = true;
				}
				if (matrix.OffsetY != 0.0 && LayoutDoubleUtil.AreClose(matrix.OffsetY, 0.0))
				{
					matrix.OffsetY = 0.0;
					flag = true;
				}
				if (flag)
				{
					transform = new MatrixTransform(matrix);
				}
			}
			return transform;
		}

		// Token: 0x060050D0 RID: 20688 RVA: 0x0016ACA4 File Offset: 0x00168EA4
		private static bool ElementIntersectsViewport(Rect viewportRect, Rect elementRect)
		{
			return !viewportRect.IsEmpty && !elementRect.IsEmpty && !DoubleUtil.LessThan(elementRect.Right, viewportRect.Left) && !LayoutDoubleUtil.AreClose(elementRect.Right, viewportRect.Left) && !DoubleUtil.GreaterThan(elementRect.Left, viewportRect.Right) && !LayoutDoubleUtil.AreClose(elementRect.Left, viewportRect.Right) && !DoubleUtil.LessThan(elementRect.Bottom, viewportRect.Top) && !LayoutDoubleUtil.AreClose(elementRect.Bottom, viewportRect.Top) && !DoubleUtil.GreaterThan(elementRect.Top, viewportRect.Bottom) && !LayoutDoubleUtil.AreClose(elementRect.Top, viewportRect.Bottom);
		}

		// Token: 0x060050D1 RID: 20689 RVA: 0x0016AD74 File Offset: 0x00168F74
		private bool IsInDirectionForLineNavigation(Rect fromRect, Rect toRect, FocusNavigationDirection direction, bool isHorizontal)
		{
			if (direction == FocusNavigationDirection.Down)
			{
				if (isHorizontal)
				{
					return DoubleUtil.GreaterThanOrClose(toRect.Left, fromRect.Left);
				}
				return DoubleUtil.GreaterThanOrClose(toRect.Top, fromRect.Top);
			}
			else
			{
				if (direction != FocusNavigationDirection.Up)
				{
					return false;
				}
				if (isHorizontal)
				{
					return DoubleUtil.LessThanOrClose(toRect.Right, fromRect.Right);
				}
				return DoubleUtil.LessThanOrClose(toRect.Bottom, fromRect.Bottom);
			}
		}

		// Token: 0x060050D2 RID: 20690 RVA: 0x0016ADE4 File Offset: 0x00168FE4
		private static void OnGotFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			ItemsControl itemsControl = (ItemsControl)sender;
			UIElement uielement = e.OriginalSource as UIElement;
			if (uielement != null && uielement != itemsControl)
			{
				object obj = itemsControl.ItemContainerGenerator.ItemFromContainer(uielement);
				if (obj != DependencyProperty.UnsetValue)
				{
					itemsControl._focusedInfo = itemsControl.NewItemInfo(obj, uielement, -1);
					return;
				}
				if (itemsControl._focusedInfo != null)
				{
					UIElement uielement2 = itemsControl._focusedInfo.Container as UIElement;
					if (uielement2 == null || !Helper.IsAnyAncestorOf(uielement2, uielement))
					{
						itemsControl._focusedInfo = null;
					}
				}
			}
		}

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x060050D3 RID: 20691 RVA: 0x0016AE62 File Offset: 0x00169062
		internal ItemsControl.ItemInfo FocusedInfo
		{
			get
			{
				return this._focusedInfo;
			}
		}

		// Token: 0x060050D4 RID: 20692 RVA: 0x0016AE6C File Offset: 0x0016906C
		internal virtual bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			object item = info.Item;
			bool result = false;
			if (item != null)
			{
				UIElement uielement = info.Container as UIElement;
				if (uielement != null)
				{
					result = uielement.Focus();
				}
			}
			if (itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				KeyboardNavigation.ShowFocusVisual();
			}
			return result;
		}

		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x060050D5 RID: 20693 RVA: 0x0016AEB0 File Offset: 0x001690B0
		internal bool IsLogicalVertical
		{
			get
			{
				return this.ItemsHost != null && this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Vertical && this.ScrollHost != null && this.ScrollHost.CanContentScroll && VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item;
			}
		}

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x060050D6 RID: 20694 RVA: 0x0016AF00 File Offset: 0x00169100
		internal bool IsLogicalHorizontal
		{
			get
			{
				return this.ItemsHost != null && this.ItemsHost.HasLogicalOrientation && this.ItemsHost.LogicalOrientation == Orientation.Horizontal && this.ScrollHost != null && this.ScrollHost.CanContentScroll && VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item;
			}
		}

		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x060050D7 RID: 20695 RVA: 0x0016AF50 File Offset: 0x00169150
		internal ScrollViewer ScrollHost
		{
			get
			{
				if (!base.ReadControlFlag(Control.ControlBoolFlags.ScrollHostValid))
				{
					if (this._itemsHost == null)
					{
						return null;
					}
					DependencyObject dependencyObject = this._itemsHost;
					while (dependencyObject != this && dependencyObject != null)
					{
						ScrollViewer scrollViewer = dependencyObject as ScrollViewer;
						if (scrollViewer != null)
						{
							this._scrollHost = scrollViewer;
							break;
						}
						dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
					}
					base.WriteControlFlag(Control.ControlBoolFlags.ScrollHostValid, true);
				}
				return this._scrollHost;
			}
		}

		// Token: 0x170013A7 RID: 5031
		// (get) Token: 0x060050D8 RID: 20696 RVA: 0x0016AFAA File Offset: 0x001691AA
		internal static TimeSpan AutoScrollTimeout
		{
			get
			{
				return TimeSpan.FromMilliseconds((double)SafeNativeMethods.GetDoubleClickTime() * 0.8);
			}
		}

		// Token: 0x060050D9 RID: 20697 RVA: 0x0016AFC1 File Offset: 0x001691C1
		internal void DoAutoScroll()
		{
			this.DoAutoScroll(this.FocusedInfo);
		}

		// Token: 0x060050DA RID: 20698 RVA: 0x0016AFD0 File Offset: 0x001691D0
		internal void DoAutoScroll(ItemsControl.ItemInfo startingInfo)
		{
			FrameworkElement frameworkElement = (this.ScrollHost != null) ? this.ScrollHost : this.ItemsHost;
			if (frameworkElement != null)
			{
				Point position = Mouse.GetPosition(frameworkElement);
				Rect rect = new Rect(default(Point), frameworkElement.RenderSize);
				bool flag = false;
				if (position.Y < rect.Top)
				{
					this.NavigateByLine(startingInfo, FocusNavigationDirection.Up, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
					flag = (startingInfo != this.FocusedInfo);
				}
				else if (position.Y >= rect.Bottom)
				{
					this.NavigateByLine(startingInfo, FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
					flag = (startingInfo != this.FocusedInfo);
				}
				if (!flag)
				{
					if (position.X < rect.Left)
					{
						FocusNavigationDirection direction = FocusNavigationDirection.Left;
						if (this.IsRTL(frameworkElement))
						{
							direction = FocusNavigationDirection.Right;
						}
						this.NavigateByLine(startingInfo, direction, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
						return;
					}
					if (position.X >= rect.Right)
					{
						FocusNavigationDirection direction2 = FocusNavigationDirection.Right;
						if (this.IsRTL(frameworkElement))
						{
							direction2 = FocusNavigationDirection.Left;
						}
						this.NavigateByLine(startingInfo, direction2, new ItemsControl.ItemNavigateArgs(Mouse.PrimaryDevice, Keyboard.Modifiers));
					}
				}
			}
		}

		// Token: 0x060050DB RID: 20699 RVA: 0x0016B0FC File Offset: 0x001692FC
		private bool IsRTL(FrameworkElement element)
		{
			FlowDirection flowDirection = element.FlowDirection;
			return flowDirection == FlowDirection.RightToLeft;
		}

		// Token: 0x060050DC RID: 20700 RVA: 0x0016B114 File Offset: 0x00169314
		private static ItemsControl GetEncapsulatingItemsControl(FrameworkElement element)
		{
			while (element != null)
			{
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(element);
				if (itemsControl != null)
				{
					return itemsControl;
				}
				element = (VisualTreeHelper.GetParent(element) as FrameworkElement);
			}
			return null;
		}

		// Token: 0x060050DD RID: 20701 RVA: 0x0016B140 File Offset: 0x00169340
		private static object GetEncapsulatingItem(FrameworkElement element, out FrameworkElement container)
		{
			ItemsControl itemsControl = null;
			return ItemsControl.GetEncapsulatingItem(element, out container, out itemsControl);
		}

		// Token: 0x060050DE RID: 20702 RVA: 0x0016B158 File Offset: 0x00169358
		private static object GetEncapsulatingItem(FrameworkElement element, out FrameworkElement container, out ItemsControl itemsControl)
		{
			object obj = DependencyProperty.UnsetValue;
			itemsControl = null;
			while (element != null)
			{
				itemsControl = ItemsControl.ItemsControlFromItemContainer(element);
				if (itemsControl != null)
				{
					obj = itemsControl.ItemContainerGenerator.ItemFromContainer(element);
					if (obj != DependencyProperty.UnsetValue)
					{
						break;
					}
				}
				element = (VisualTreeHelper.GetParent(element) as FrameworkElement);
			}
			container = element;
			return obj;
		}

		// Token: 0x060050DF RID: 20703 RVA: 0x0016B1A8 File Offset: 0x001693A8
		internal static DependencyObject TryGetTreeViewItemHeader(DependencyObject element)
		{
			TreeViewItem treeViewItem = element as TreeViewItem;
			if (treeViewItem != null)
			{
				return treeViewItem.TryGetHeaderElement();
			}
			return element;
		}

		// Token: 0x060050E0 RID: 20704 RVA: 0x0016B1C8 File Offset: 0x001693C8
		private void ApplyItemContainerStyle(DependencyObject container, object item)
		{
			FrameworkObject frameworkObject = new FrameworkObject(container);
			if (!frameworkObject.IsStyleSetFromGenerator && container.ReadLocalValue(FrameworkElement.StyleProperty) != DependencyProperty.UnsetValue)
			{
				return;
			}
			Style style = this.ItemContainerStyle;
			if (style == null && this.ItemContainerStyleSelector != null)
			{
				style = this.ItemContainerStyleSelector.SelectStyle(item, container);
			}
			if (style == null)
			{
				if (frameworkObject.IsStyleSetFromGenerator)
				{
					frameworkObject.IsStyleSetFromGenerator = false;
					container.ClearValue(FrameworkElement.StyleProperty);
				}
				return;
			}
			if (!style.TargetType.IsInstanceOfType(container))
			{
				throw new InvalidOperationException(SR.Get("StyleForWrongType", new object[]
				{
					style.TargetType.Name,
					container.GetType().Name
				}));
			}
			frameworkObject.Style = style;
			frameworkObject.IsStyleSetFromGenerator = true;
		}

		// Token: 0x060050E1 RID: 20705 RVA: 0x0016B28C File Offset: 0x0016948C
		private void RemoveItemContainerStyle(DependencyObject container)
		{
			FrameworkObject frameworkObject = new FrameworkObject(container);
			if (frameworkObject.IsStyleSetFromGenerator)
			{
				container.ClearValue(FrameworkElement.StyleProperty);
			}
		}

		// Token: 0x060050E2 RID: 20706 RVA: 0x0016B2B8 File Offset: 0x001694B8
		internal object GetItemOrContainerFromContainer(DependencyObject container)
		{
			object obj = this.ItemContainerGenerator.ItemFromContainer(container);
			if (obj == DependencyProperty.UnsetValue && ItemsControl.ItemsControlFromItemContainer(container) == this && ((IGeneratorHost)this).IsItemItsOwnContainer(container))
			{
				obj = container;
			}
			return obj;
		}

		// Token: 0x060050E3 RID: 20707 RVA: 0x0016B2F0 File Offset: 0x001694F0
		internal static bool EqualsEx(object o1, object o2)
		{
			bool result;
			try
			{
				result = object.Equals(o1, o2);
			}
			catch (InvalidCastException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060050E4 RID: 20708 RVA: 0x0016B320 File Offset: 0x00169520
		internal ItemsControl.ItemInfo NewItemInfo(object item, DependencyObject container = null, int index = -1)
		{
			return new ItemsControl.ItemInfo(item, container, index).Refresh(this.ItemContainerGenerator);
		}

		// Token: 0x060050E5 RID: 20709 RVA: 0x0016B335 File Offset: 0x00169535
		internal ItemsControl.ItemInfo ItemInfoFromContainer(DependencyObject container)
		{
			return this.NewItemInfo(this.ItemContainerGenerator.ItemFromContainer(container), container, this.ItemContainerGenerator.IndexFromContainer(container));
		}

		// Token: 0x060050E6 RID: 20710 RVA: 0x0016B356 File Offset: 0x00169556
		internal ItemsControl.ItemInfo ItemInfoFromIndex(int index)
		{
			if (index < 0)
			{
				return null;
			}
			return this.NewItemInfo(this.Items[index], this.ItemContainerGenerator.ContainerFromIndex(index), index);
		}

		// Token: 0x060050E7 RID: 20711 RVA: 0x0016B37D File Offset: 0x0016957D
		internal ItemsControl.ItemInfo NewUnresolvedItemInfo(object item)
		{
			return new ItemsControl.ItemInfo(item, ItemsControl.ItemInfo.UnresolvedContainer, -1);
		}

		// Token: 0x060050E8 RID: 20712 RVA: 0x0016B38C File Offset: 0x0016958C
		internal DependencyObject ContainerFromItemInfo(ItemsControl.ItemInfo info)
		{
			DependencyObject dependencyObject = info.Container;
			if (dependencyObject == null)
			{
				if (info.Index >= 0)
				{
					dependencyObject = this.ItemContainerGenerator.ContainerFromIndex(info.Index);
					info.Container = dependencyObject;
				}
				else
				{
					dependencyObject = this.ItemContainerGenerator.ContainerFromItem(info.Item);
				}
			}
			return dependencyObject;
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x0016B3DC File Offset: 0x001695DC
		internal void AdjustItemInfoAfterGeneratorChange(ItemsControl.ItemInfo info)
		{
			if (info != null)
			{
				ItemsControl.ItemInfo[] list = new ItemsControl.ItemInfo[]
				{
					info
				};
				this.AdjustItemInfosAfterGeneratorChange(list, false);
			}
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x0016B408 File Offset: 0x00169608
		internal void AdjustItemInfosAfterGeneratorChange(IEnumerable<ItemsControl.ItemInfo> list, bool claimUniqueContainer)
		{
			bool flag = false;
			foreach (ItemsControl.ItemInfo itemInfo in list)
			{
				DependencyObject container = itemInfo.Container;
				if (container == null)
				{
					flag = true;
				}
				else if (itemInfo.IsRemoved || !ItemsControl.EqualsEx(itemInfo.Item, container.ReadLocalValue(ItemContainerGenerator.ItemForItemContainerProperty)))
				{
					itemInfo.Container = null;
					flag = true;
				}
			}
			if (flag)
			{
				List<DependencyObject> claimedContainers = new List<DependencyObject>();
				if (claimUniqueContainer)
				{
					foreach (ItemsControl.ItemInfo itemInfo2 in list)
					{
						DependencyObject container2 = itemInfo2.Container;
						if (container2 != null)
						{
							claimedContainers.Add(container2);
						}
					}
				}
				foreach (ItemsControl.ItemInfo itemInfo3 in list)
				{
					DependencyObject dependencyObject = itemInfo3.Container;
					if (dependencyObject == null)
					{
						int index = itemInfo3.Index;
						if (index >= 0)
						{
							dependencyObject = this.ItemContainerGenerator.ContainerFromIndex(index);
						}
						else
						{
							object item = itemInfo3.Item;
							this.ItemContainerGenerator.FindItem((object o, DependencyObject d) => ItemsControl.EqualsEx(o, item) && !claimedContainers.Contains(d), out dependencyObject, out index);
						}
						if (dependencyObject != null)
						{
							itemInfo3.Container = dependencyObject;
							itemInfo3.Index = index;
							if (claimUniqueContainer)
							{
								claimedContainers.Add(dependencyObject);
							}
						}
					}
				}
			}
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x0016B5BC File Offset: 0x001697BC
		internal void AdjustItemInfo(NotifyCollectionChangedEventArgs e, ItemsControl.ItemInfo info)
		{
			if (info != null)
			{
				ItemsControl.ItemInfo[] list = new ItemsControl.ItemInfo[]
				{
					info
				};
				this.AdjustItemInfos(e, list);
			}
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x0016B5E8 File Offset: 0x001697E8
		internal void AdjustItemInfos(NotifyCollectionChangedEventArgs e, IEnumerable<ItemsControl.ItemInfo> list)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				using (IEnumerator<ItemsControl.ItemInfo> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ItemsControl.ItemInfo itemInfo = enumerator.Current;
						int index = itemInfo.Index;
						if (index >= e.NewStartingIndex)
						{
							itemInfo.Index = index + 1;
						}
					}
					return;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				break;
			case NotifyCollectionChangedAction.Replace:
				return;
			case NotifyCollectionChangedAction.Move:
				goto IL_CC;
			case NotifyCollectionChangedAction.Reset:
				goto IL_161;
			default:
				return;
			}
			using (IEnumerator<ItemsControl.ItemInfo> enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ItemsControl.ItemInfo itemInfo2 = enumerator2.Current;
					int index2 = itemInfo2.Index;
					if (index2 > e.OldStartingIndex)
					{
						itemInfo2.Index = index2 - 1;
					}
					else if (index2 == e.OldStartingIndex)
					{
						itemInfo2.Index = -1;
					}
				}
				return;
			}
			IL_CC:
			int num;
			int num2;
			int num3;
			if (e.OldStartingIndex < e.NewStartingIndex)
			{
				num = e.OldStartingIndex + 1;
				num2 = e.NewStartingIndex;
				num3 = -1;
			}
			else
			{
				num = e.NewStartingIndex;
				num2 = e.OldStartingIndex - 1;
				num3 = 1;
			}
			using (IEnumerator<ItemsControl.ItemInfo> enumerator3 = list.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ItemsControl.ItemInfo itemInfo3 = enumerator3.Current;
					int index3 = itemInfo3.Index;
					if (index3 == e.OldStartingIndex)
					{
						itemInfo3.Index = e.NewStartingIndex;
					}
					else if (num <= index3 && index3 <= num2)
					{
						itemInfo3.Index = index3 + num3;
					}
				}
				return;
			}
			IL_161:
			foreach (ItemsControl.ItemInfo itemInfo4 in list)
			{
				itemInfo4.Index = -1;
				itemInfo4.Container = null;
			}
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x0016B7C4 File Offset: 0x001699C4
		internal ItemsControl.ItemInfo LeaseItemInfo(ItemsControl.ItemInfo info, bool ensureIndex = false)
		{
			if (info.Index < 0)
			{
				info = this.NewItemInfo(info.Item, null, -1);
				if (ensureIndex && info.Index < 0)
				{
					info.Index = this.Items.IndexOf(info.Item);
				}
			}
			return info;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x0016B803 File Offset: 0x00169A03
		internal void RefreshItemInfo(ItemsControl.ItemInfo info)
		{
			if (info != null)
			{
				info.Refresh(this.ItemContainerGenerator);
			}
		}

		/// <summary>Returns the value of the specified property that is associated with the specified item.</summary>
		/// <param name="item">The item that has the specified property associated with it.</param>
		/// <param name="dp">The property whose value to return.</param>
		/// <returns>The value of the specified property that is associated with the specified item.</returns>
		// Token: 0x060050EF RID: 20719 RVA: 0x0015FFBA File Offset: 0x0015E1BA
		object IContainItemStorage.ReadItemValue(object item, DependencyProperty dp)
		{
			return Helper.ReadItemValue(this, item, dp.GlobalIndex);
		}

		/// <summary>Stores the specified property and value and associates them with the specified item.</summary>
		/// <param name="item">The item to associate the value and property with.</param>
		/// <param name="dp">The property that is associated with the specified item.</param>
		/// <param name="value">The value of the associated property.</param>
		// Token: 0x060050F0 RID: 20720 RVA: 0x0015FFC9 File Offset: 0x0015E1C9
		void IContainItemStorage.StoreItemValue(object item, DependencyProperty dp, object value)
		{
			Helper.StoreItemValue(this, item, dp.GlobalIndex, value);
		}

		/// <summary>Removes the association between the specified item and property.</summary>
		/// <param name="item">The associated item.</param>
		/// <param name="dp">The associated property.</param>
		// Token: 0x060050F1 RID: 20721 RVA: 0x0015FFD9 File Offset: 0x0015E1D9
		void IContainItemStorage.ClearItemValue(object item, DependencyProperty dp)
		{
			Helper.ClearItemValue(this, item, dp.GlobalIndex);
		}

		/// <summary>Removes the specified property from all property lists.</summary>
		/// <param name="dp">The property to remove.</param>
		// Token: 0x060050F2 RID: 20722 RVA: 0x0015FFE8 File Offset: 0x0015E1E8
		void IContainItemStorage.ClearValue(DependencyProperty dp)
		{
			Helper.ClearItemValueStorage(this, new int[]
			{
				dp.GlobalIndex
			});
		}

		/// <summary>Clears all property associations.</summary>
		// Token: 0x060050F3 RID: 20723 RVA: 0x0015FFFF File Offset: 0x0015E1FF
		void IContainItemStorage.Clear()
		{
			Helper.ClearItemValueStorage(this);
		}

		/// <summary>Provides a string representation of the <see cref="T:System.Windows.Controls.ItemsControl" /> object.</summary>
		/// <returns>The string representation of the object.</returns>
		// Token: 0x060050F4 RID: 20724 RVA: 0x0016B81C File Offset: 0x00169A1C
		public override string ToString()
		{
			int num = this.HasItems ? this.Items.Count : 0;
			return SR.Get("ToStringFormatString_ItemsControl", new object[]
			{
				base.GetType(),
				num
			});
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x0016B862 File Offset: 0x00169A62
		internal override AutomationPeer OnCreateAutomationPeerInternal()
		{
			return new ItemsControlWrapperAutomationPeer(this);
		}

		// Token: 0x170013A8 RID: 5032
		// (get) Token: 0x060050F6 RID: 20726 RVA: 0x0016B86A File Offset: 0x00169A6A
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ItemsControl._dType;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> dependency property.</returns>
		// Token: 0x04002C2C RID: 11308
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemsSourceChanged)));

		// Token: 0x04002C2D RID: 11309
		internal static readonly DependencyPropertyKey HasItemsPropertyKey = DependencyProperty.RegisterReadOnly("HasItems", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(Control.OnVisualStatePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.HasItems" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.HasItems" /> dependency property.</returns>
		// Token: 0x04002C2E RID: 11310
		public static readonly DependencyProperty HasItemsProperty = ItemsControl.HasItemsPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.DisplayMemberPath" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.DisplayMemberPath" /> dependency property.</returns>
		// Token: 0x04002C2F RID: 11311
		public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ItemsControl), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(ItemsControl.OnDisplayMemberPathChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplate" /> dependency property.</returns>
		// Token: 0x04002C30 RID: 11312
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplateSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002C31 RID: 11313
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemStringFormat" /> dependency property.</returns>
		// Token: 0x04002C32 RID: 11314
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemStringFormatProperty = DependencyProperty.Register("ItemStringFormat", typeof(string), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemStringFormatChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemBindingGroup" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemBindingGroup" /> dependency property.</returns>
		// Token: 0x04002C33 RID: 11315
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemBindingGroupProperty = DependencyProperty.Register("ItemBindingGroup", typeof(BindingGroup), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemBindingGroupChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyle" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyle" /> dependency property.</returns>
		// Token: 0x04002C34 RID: 11316
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle", typeof(Style), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemContainerStyleChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyleSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemContainerStyleSelector" /> dependency property.</returns>
		// Token: 0x04002C35 RID: 11317
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemContainerStyleSelectorProperty = DependencyProperty.Register("ItemContainerStyleSelector", typeof(StyleSelector), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnItemContainerStyleSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.ItemsPanel" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.ItemsPanel" /> dependency property.</returns>
		// Token: 0x04002C36 RID: 11318
		[CommonDependencyProperty]
		public static readonly DependencyProperty ItemsPanelProperty = DependencyProperty.Register("ItemsPanel", typeof(ItemsPanelTemplate), typeof(ItemsControl), new FrameworkPropertyMetadata(ItemsControl.GetDefaultItemsPanelTemplate(), new PropertyChangedCallback(ItemsControl.OnItemsPanelChanged)));

		// Token: 0x04002C37 RID: 11319
		private static readonly DependencyPropertyKey IsGroupingPropertyKey = DependencyProperty.RegisterReadOnly("IsGrouping", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, new PropertyChangedCallback(ItemsControl.OnIsGroupingChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.IsGrouping" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.IsGrouping" /> dependency property.</returns>
		// Token: 0x04002C38 RID: 11320
		public static readonly DependencyProperty IsGroupingProperty = ItemsControl.IsGroupingPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.GroupStyleSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.GroupStyleSelector" /> dependency property.</returns>
		// Token: 0x04002C39 RID: 11321
		public static readonly DependencyProperty GroupStyleSelectorProperty = DependencyProperty.Register("GroupStyleSelector", typeof(GroupStyleSelector), typeof(ItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ItemsControl.OnGroupStyleSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.AlternationCount" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.AlternationCount" /> dependency property.</returns>
		// Token: 0x04002C3A RID: 11322
		public static readonly DependencyProperty AlternationCountProperty = DependencyProperty.Register("AlternationCount", typeof(int), typeof(ItemsControl), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(ItemsControl.OnAlternationCountChanged)));

		// Token: 0x04002C3B RID: 11323
		private static readonly DependencyPropertyKey AlternationIndexPropertyKey = DependencyProperty.RegisterAttachedReadOnly("AlternationIndex", typeof(int), typeof(ItemsControl), new FrameworkPropertyMetadata(0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.AlternationIndex" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.AlternationIndex" /> dependency property.</returns>
		// Token: 0x04002C3C RID: 11324
		public static readonly DependencyProperty AlternationIndexProperty = ItemsControl.AlternationIndexPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.IsTextSearchEnabled" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.IsTextSearchEnabled" /> dependency property.</returns>
		// Token: 0x04002C3D RID: 11325
		public static readonly DependencyProperty IsTextSearchEnabledProperty = DependencyProperty.Register("IsTextSearchEnabled", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ItemsControl.IsTextSearchCaseSensitive" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ItemsControl.IsTextSearchCaseSensitive" /> dependency property.</returns>
		// Token: 0x04002C3E RID: 11326
		public static readonly DependencyProperty IsTextSearchCaseSensitiveProperty = DependencyProperty.Register("IsTextSearchCaseSensitive", typeof(bool), typeof(ItemsControl), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));

		// Token: 0x04002C3F RID: 11327
		private ItemsControl.ItemInfo _focusedInfo;

		// Token: 0x04002C40 RID: 11328
		private ItemCollection _items;

		// Token: 0x04002C41 RID: 11329
		private ItemContainerGenerator _itemContainerGenerator;

		// Token: 0x04002C42 RID: 11330
		private Panel _itemsHost;

		// Token: 0x04002C43 RID: 11331
		private ScrollViewer _scrollHost;

		// Token: 0x04002C44 RID: 11332
		private ObservableCollection<GroupStyle> _groupStyle = new ObservableCollection<GroupStyle>();

		// Token: 0x04002C45 RID: 11333
		private static readonly UncommonField<bool> ShouldCoerceScrollUnitField = new UncommonField<bool>();

		// Token: 0x04002C46 RID: 11334
		private static readonly UncommonField<bool> ShouldCoerceCacheSizeField = new UncommonField<bool>();

		// Token: 0x04002C47 RID: 11335
		private static DependencyObjectType _dType;

		// Token: 0x020009A2 RID: 2466
		internal class ItemNavigateArgs
		{
			// Token: 0x06008818 RID: 34840 RVA: 0x00251886 File Offset: 0x0024FA86
			public ItemNavigateArgs(InputDevice deviceUsed, ModifierKeys modifierKeys)
			{
				this._deviceUsed = deviceUsed;
				this._modifierKeys = modifierKeys;
			}

			// Token: 0x17001EB6 RID: 7862
			// (get) Token: 0x06008819 RID: 34841 RVA: 0x0025189C File Offset: 0x0024FA9C
			public InputDevice DeviceUsed
			{
				get
				{
					return this._deviceUsed;
				}
			}

			// Token: 0x17001EB7 RID: 7863
			// (get) Token: 0x0600881A RID: 34842 RVA: 0x002518A4 File Offset: 0x0024FAA4
			public static ItemsControl.ItemNavigateArgs Empty
			{
				get
				{
					if (ItemsControl.ItemNavigateArgs._empty == null)
					{
						ItemsControl.ItemNavigateArgs._empty = new ItemsControl.ItemNavigateArgs(null, ModifierKeys.None);
					}
					return ItemsControl.ItemNavigateArgs._empty;
				}
			}

			// Token: 0x040044F0 RID: 17648
			private InputDevice _deviceUsed;

			// Token: 0x040044F1 RID: 17649
			private ModifierKeys _modifierKeys;

			// Token: 0x040044F2 RID: 17650
			private static ItemsControl.ItemNavigateArgs _empty;
		}

		// Token: 0x020009A3 RID: 2467
		[DebuggerDisplay("Index: {Index}  Item: {Item}")]
		internal class ItemInfo
		{
			// Token: 0x17001EB8 RID: 7864
			// (get) Token: 0x0600881B RID: 34843 RVA: 0x002518BE File Offset: 0x0024FABE
			// (set) Token: 0x0600881C RID: 34844 RVA: 0x002518C6 File Offset: 0x0024FAC6
			internal object Item { get; private set; }

			// Token: 0x17001EB9 RID: 7865
			// (get) Token: 0x0600881D RID: 34845 RVA: 0x002518CF File Offset: 0x0024FACF
			// (set) Token: 0x0600881E RID: 34846 RVA: 0x002518D7 File Offset: 0x0024FAD7
			internal DependencyObject Container { get; set; }

			// Token: 0x17001EBA RID: 7866
			// (get) Token: 0x0600881F RID: 34847 RVA: 0x002518E0 File Offset: 0x0024FAE0
			// (set) Token: 0x06008820 RID: 34848 RVA: 0x002518E8 File Offset: 0x0024FAE8
			internal int Index { get; set; }

			// Token: 0x06008821 RID: 34849 RVA: 0x002518F4 File Offset: 0x0024FAF4
			static ItemInfo()
			{
				ItemsControl.ItemInfo.SentinelContainer.MakeSentinel();
				ItemsControl.ItemInfo.UnresolvedContainer.MakeSentinel();
				ItemsControl.ItemInfo.KeyContainer.MakeSentinel();
				ItemsControl.ItemInfo.RemovedContainer.MakeSentinel();
			}

			// Token: 0x06008822 RID: 34850 RVA: 0x00251951 File Offset: 0x0024FB51
			public ItemInfo(object item, DependencyObject container = null, int index = -1)
			{
				this.Item = item;
				this.Container = container;
				this.Index = index;
			}

			// Token: 0x17001EBB RID: 7867
			// (get) Token: 0x06008823 RID: 34851 RVA: 0x0025196E File Offset: 0x0024FB6E
			internal bool IsResolved
			{
				get
				{
					return this.Container != ItemsControl.ItemInfo.UnresolvedContainer;
				}
			}

			// Token: 0x17001EBC RID: 7868
			// (get) Token: 0x06008824 RID: 34852 RVA: 0x00251980 File Offset: 0x0024FB80
			internal bool IsKey
			{
				get
				{
					return this.Container == ItemsControl.ItemInfo.KeyContainer;
				}
			}

			// Token: 0x17001EBD RID: 7869
			// (get) Token: 0x06008825 RID: 34853 RVA: 0x0025198F File Offset: 0x0024FB8F
			internal bool IsRemoved
			{
				get
				{
					return this.Container == ItemsControl.ItemInfo.RemovedContainer;
				}
			}

			// Token: 0x06008826 RID: 34854 RVA: 0x0025199E File Offset: 0x0024FB9E
			internal ItemsControl.ItemInfo Clone()
			{
				return new ItemsControl.ItemInfo(this.Item, this.Container, this.Index);
			}

			// Token: 0x06008827 RID: 34855 RVA: 0x002519B7 File Offset: 0x0024FBB7
			internal static ItemsControl.ItemInfo Key(ItemsControl.ItemInfo info)
			{
				if (info.Container != ItemsControl.ItemInfo.UnresolvedContainer)
				{
					return info;
				}
				return new ItemsControl.ItemInfo(info.Item, ItemsControl.ItemInfo.KeyContainer, -1);
			}

			// Token: 0x06008828 RID: 34856 RVA: 0x002519D9 File Offset: 0x0024FBD9
			public override int GetHashCode()
			{
				if (this.Item == null)
				{
					return 314159;
				}
				return this.Item.GetHashCode();
			}

			// Token: 0x06008829 RID: 34857 RVA: 0x002519F4 File Offset: 0x0024FBF4
			public override bool Equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				ItemsControl.ItemInfo itemInfo = o as ItemsControl.ItemInfo;
				return !(itemInfo == null) && this.Equals(itemInfo, false);
			}

			// Token: 0x0600882A RID: 34858 RVA: 0x00251A24 File Offset: 0x0024FC24
			internal bool Equals(ItemsControl.ItemInfo that, bool matchUnresolved)
			{
				if (this.IsRemoved || that.IsRemoved)
				{
					return false;
				}
				if (!ItemsControl.EqualsEx(this.Item, that.Item))
				{
					return false;
				}
				if (this.Container == ItemsControl.ItemInfo.KeyContainer)
				{
					return matchUnresolved || that.Container != ItemsControl.ItemInfo.UnresolvedContainer;
				}
				if (that.Container == ItemsControl.ItemInfo.KeyContainer)
				{
					return matchUnresolved || this.Container != ItemsControl.ItemInfo.UnresolvedContainer;
				}
				if (this.Container == ItemsControl.ItemInfo.UnresolvedContainer || that.Container == ItemsControl.ItemInfo.UnresolvedContainer)
				{
					return false;
				}
				if (this.Container != that.Container)
				{
					return this.Container == ItemsControl.ItemInfo.SentinelContainer || that.Container == ItemsControl.ItemInfo.SentinelContainer || ((this.Container == null || that.Container == null) && (this.Index < 0 || that.Index < 0 || this.Index == that.Index));
				}
				if (this.Container != ItemsControl.ItemInfo.SentinelContainer)
				{
					return this.Index < 0 || that.Index < 0 || this.Index == that.Index;
				}
				return this.Index == that.Index;
			}

			// Token: 0x0600882B RID: 34859 RVA: 0x0018B7A9 File Offset: 0x001899A9
			public static bool operator ==(ItemsControl.ItemInfo info1, ItemsControl.ItemInfo info2)
			{
				return object.Equals(info1, info2);
			}

			// Token: 0x0600882C RID: 34860 RVA: 0x0018B7B2 File Offset: 0x001899B2
			public static bool operator !=(ItemsControl.ItemInfo info1, ItemsControl.ItemInfo info2)
			{
				return !object.Equals(info1, info2);
			}

			// Token: 0x0600882D RID: 34861 RVA: 0x00251B58 File Offset: 0x0024FD58
			internal ItemsControl.ItemInfo Refresh(ItemContainerGenerator generator)
			{
				if (this.Container == null && this.Index < 0)
				{
					this.Container = generator.ContainerFromItem(this.Item);
				}
				if (this.Index < 0 && this.Container != null)
				{
					this.Index = generator.IndexFromContainer(this.Container);
				}
				if (this.Container == null && this.Index >= 0)
				{
					this.Container = generator.ContainerFromIndex(this.Index);
				}
				if (this.Container == ItemsControl.ItemInfo.SentinelContainer && this.Index >= 0)
				{
					this.Container = null;
				}
				return this;
			}

			// Token: 0x0600882E RID: 34862 RVA: 0x00251BEC File Offset: 0x0024FDEC
			internal void Reset(object item)
			{
				this.Item = item;
			}

			// Token: 0x040044F6 RID: 17654
			internal static readonly DependencyObject SentinelContainer = new DependencyObject();

			// Token: 0x040044F7 RID: 17655
			internal static readonly DependencyObject UnresolvedContainer = new DependencyObject();

			// Token: 0x040044F8 RID: 17656
			internal static readonly DependencyObject KeyContainer = new DependencyObject();

			// Token: 0x040044F9 RID: 17657
			internal static readonly DependencyObject RemovedContainer = new DependencyObject();
		}
	}
}
