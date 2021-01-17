using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Appears as the root of the visual subtree generated for a group. </summary>
	// Token: 0x020004E4 RID: 1252
	public class GroupItem : ContentControl, IHierarchicalVirtualizationAndScrollInfo, IContainItemStorage
	{
		// Token: 0x06004E05 RID: 19973 RVA: 0x0015FA6C File Offset: 0x0015DC6C
		static GroupItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupItem), new FrameworkPropertyMetadata(typeof(GroupItem)));
			GroupItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(GroupItem));
			UIElement.FocusableProperty.OverrideMetadata(typeof(GroupItem), new FrameworkPropertyMetadata(false));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(GroupItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.GroupItem" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Controls.GroupItem" />.</returns>
		// Token: 0x06004E06 RID: 19974 RVA: 0x0015FB2A File Offset: 0x0015DD2A
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new GroupItemAutomationPeer(this);
		}

		/// <summary>Builds the visual tree for the <see cref="T:System.Windows.Controls.GroupItem" /> when a new template is applied. </summary>
		// Token: 0x06004E07 RID: 19975 RVA: 0x0015FB34 File Offset: 0x0015DD34
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._header = (base.GetTemplateChild("PART_Header") as FrameworkElement);
			this._expander = Helper.FindTemplatedDescendant<Expander>(this, this);
			if (this._expander != null)
			{
				ItemsControl parentItemsControl = this.ParentItemsControl;
				if (parentItemsControl != null && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parentItemsControl))
				{
					Helper.SetItemValuesOnContainer(parentItemsControl, this._expander, parentItemsControl.ItemContainerGenerator.ItemFromContainer(this));
				}
				this._expander.Expanded += GroupItem.OnExpanded;
			}
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x0015FBB4 File Offset: 0x0015DDB4
		private static void OnExpanded(object sender, RoutedEventArgs e)
		{
			GroupItem groupItem = sender as GroupItem;
			if (groupItem != null && groupItem._expander != null && groupItem._expander.IsExpanded)
			{
				ItemsControl parentItemsControl = groupItem.ParentItemsControl;
				if (parentItemsControl != null && VirtualizingPanel.GetIsVirtualizing(parentItemsControl) && VirtualizingPanel.GetVirtualizationMode(parentItemsControl) == VirtualizationMode.Recycling)
				{
					ItemsPresenter itemsHostPresenter = groupItem.ItemsHostPresenter;
					if (itemsHostPresenter != null)
					{
						groupItem.InvalidateMeasure();
						Helper.InvalidateMeasureOnPath(itemsHostPresenter, groupItem, false);
					}
				}
			}
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x0015FC13 File Offset: 0x0015DE13
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			base.OnTemplateChangedInternal(oldTemplate, newTemplate);
			if (this._expander != null)
			{
				this._expander.Expanded -= GroupItem.OnExpanded;
				this._expander = null;
			}
			this._itemsHost = null;
		}

		/// <summary>Arranges the content of the <see cref="T:System.Windows.Controls.GroupItem" />.</summary>
		/// <param name="arrangeSize">The final area within the parent that the <see cref="T:System.Windows.Controls.GroupItem" /> should use to arrange itself and its children.</param>
		/// <returns>The actual sized used by the <see cref="T:System.Windows.Controls.GroupItem" />.</returns>
		// Token: 0x06004E0A RID: 19978 RVA: 0x0015FC4A File Offset: 0x0015DE4A
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			arrangeSize = base.ArrangeOverride(arrangeSize);
			Helper.ComputeCorrectionFactor(this.ParentItemsControl, this, this.ItemsHost, this.HeaderElement);
			return arrangeSize;
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x0015FC70 File Offset: 0x0015DE70
		internal override string GetPlainText()
		{
			CollectionViewGroup collectionViewGroup = base.Content as CollectionViewGroup;
			if (collectionViewGroup != null && collectionViewGroup.Name != null)
			{
				return collectionViewGroup.Name.ToString();
			}
			return base.GetPlainText();
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06004E0C RID: 19980 RVA: 0x0015FCA6 File Offset: 0x0015DEA6
		// (set) Token: 0x06004E0D RID: 19981 RVA: 0x0015FCAE File Offset: 0x0015DEAE
		internal ItemContainerGenerator Generator
		{
			get
			{
				return this._generator;
			}
			set
			{
				this._generator = value;
			}
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x0015FCB8 File Offset: 0x0015DEB8
		internal void PrepareItemContainer(object item, ItemsControl parentItemsControl)
		{
			if (this.Generator == null)
			{
				return;
			}
			if (this._itemsHost != null)
			{
				this._itemsHost.IsItemsHost = true;
			}
			bool flag = parentItemsControl != null && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parentItemsControl);
			if (this.Generator != null)
			{
				if (!flag)
				{
					this.Generator.Release();
				}
				else
				{
					this.Generator.RemoveAllInternal(true);
				}
			}
			ItemContainerGenerator parent = this.Generator.Parent;
			GroupStyle groupStyle = parent.GroupStyle;
			Style style = groupStyle.ContainerStyle;
			if (style == null && groupStyle.ContainerStyleSelector != null)
			{
				style = groupStyle.ContainerStyleSelector.SelectStyle(item, this);
			}
			if (style != null)
			{
				if (!style.TargetType.IsInstanceOfType(this))
				{
					throw new InvalidOperationException(SR.Get("StyleForWrongType", new object[]
					{
						style.TargetType.Name,
						base.GetType().Name
					}));
				}
				base.Style = style;
				base.WriteInternalFlag2(InternalFlags2.IsStyleSetFromGenerator, true);
			}
			if (base.ContentIsItem || !base.HasNonDefaultValue(ContentControl.ContentProperty))
			{
				base.Content = item;
				base.ContentIsItem = true;
			}
			if (!base.HasNonDefaultValue(ContentControl.ContentTemplateProperty))
			{
				base.ContentTemplate = groupStyle.HeaderTemplate;
			}
			if (!base.HasNonDefaultValue(ContentControl.ContentTemplateSelectorProperty))
			{
				base.ContentTemplateSelector = groupStyle.HeaderTemplateSelector;
			}
			if (!base.HasNonDefaultValue(ContentControl.ContentStringFormatProperty))
			{
				base.ContentStringFormat = groupStyle.HeaderStringFormat;
			}
			Helper.ClearVirtualizingElement(this);
			if (flag)
			{
				Helper.SetItemValuesOnContainer(parentItemsControl, this, item);
				if (this._expander != null)
				{
					Helper.SetItemValuesOnContainer(parentItemsControl, this._expander, item);
				}
			}
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x0015FE30 File Offset: 0x0015E030
		internal void ClearItemContainer(object item, ItemsControl parentItemsControl)
		{
			if (this.Generator == null)
			{
				return;
			}
			if (parentItemsControl != null && VirtualizingPanel.GetIsVirtualizingWhenGrouping(parentItemsControl))
			{
				Helper.StoreItemValues(parentItemsControl, this, item);
				if (this._expander != null)
				{
					Helper.StoreItemValues(parentItemsControl, this._expander, item);
				}
				VirtualizingPanel virtualizingPanel = this._itemsHost as VirtualizingPanel;
				if (virtualizingPanel != null)
				{
					virtualizingPanel.OnClearChildrenInternal();
				}
				this.Generator.RemoveAllInternal(true);
			}
			else
			{
				this.Generator.Release();
			}
			base.ClearContentControl(item);
		}

		/// <summary>Gets or sets an object that represents the sizes of the control's viewport and cache.</summary>
		/// <returns>An object that represents the sizes of the control's viewport and cache.</returns>
		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06004E10 RID: 19984 RVA: 0x0015FEA3 File Offset: 0x0015E0A3
		// (set) Token: 0x06004E11 RID: 19985 RVA: 0x0015FEB0 File Offset: 0x0015E0B0
		HierarchicalVirtualizationConstraints IHierarchicalVirtualizationAndScrollInfo.Constraints
		{
			get
			{
				return GroupItem.HierarchicalVirtualizationConstraintsField.GetValue(this);
			}
			set
			{
				if (value.CacheLengthUnit == VirtualizationCacheLengthUnit.Page)
				{
					throw new InvalidOperationException(SR.Get("PageCacheSizeNotAllowed"));
				}
				GroupItem.HierarchicalVirtualizationConstraintsField.SetValue(this, value);
			}
		}

		/// <summary>Gets an object that represents the desired size of the control's header.</summary>
		/// <returns>An object that represents the desired size of the control's header.</returns>
		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06004E12 RID: 19986 RVA: 0x0015FED8 File Offset: 0x0015E0D8
		HierarchicalVirtualizationHeaderDesiredSizes IHierarchicalVirtualizationAndScrollInfo.HeaderDesiredSizes
		{
			get
			{
				FrameworkElement headerElement = this.HeaderElement;
				Size pixelSize = default(Size);
				if (base.IsVisible && headerElement != null)
				{
					pixelSize = headerElement.DesiredSize;
					Helper.ApplyCorrectionFactorToPixelHeaderSize(this.ParentItemsControl, this, this._itemsHost, ref pixelSize);
				}
				Size logicalSize = new Size((double)(DoubleUtil.GreaterThan(pixelSize.Width, 0.0) ? 1 : 0), (double)(DoubleUtil.GreaterThan(pixelSize.Height, 0.0) ? 1 : 0));
				return new HierarchicalVirtualizationHeaderDesiredSizes(logicalSize, pixelSize);
			}
		}

		/// <summary>Gets or sets an object that represents the desired size of the control's items.</summary>
		/// <returns>An object that represents the desired size of the control's items.</returns>
		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x06004E13 RID: 19987 RVA: 0x0015FF60 File Offset: 0x0015E160
		// (set) Token: 0x06004E14 RID: 19988 RVA: 0x0015FF6E File Offset: 0x0015E16E
		HierarchicalVirtualizationItemDesiredSizes IHierarchicalVirtualizationAndScrollInfo.ItemDesiredSizes
		{
			get
			{
				return Helper.ApplyCorrectionFactorToItemDesiredSizes(this, this._itemsHost);
			}
			set
			{
				GroupItem.HierarchicalVirtualizationItemDesiredSizesField.SetValue(this, value);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.Panel" /> that displays the items of the control.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.Panel" /> that displays the items of the control.</returns>
		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06004E15 RID: 19989 RVA: 0x0015FF7C File Offset: 0x0015E17C
		Panel IHierarchicalVirtualizationAndScrollInfo.ItemsHost
		{
			get
			{
				return this._itemsHost;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the owning <see cref="T:System.Windows.Controls.ItemsControl" /> should virtualize its items.</summary>
		/// <returns>
		///     <see langword="true" /> if the owning <see cref="T:System.Windows.Controls.ItemsControl" /> should virtualize its items; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06004E16 RID: 19990 RVA: 0x0015FF84 File Offset: 0x0015E184
		// (set) Token: 0x06004E17 RID: 19991 RVA: 0x0015FF91 File Offset: 0x0015E191
		bool IHierarchicalVirtualizationAndScrollInfo.MustDisableVirtualization
		{
			get
			{
				return GroupItem.MustDisableVirtualizationField.GetValue(this);
			}
			set
			{
				GroupItem.MustDisableVirtualizationField.SetValue(this, value);
			}
		}

		/// <summary>Gets a value that indicates whether the control's layout pass occurs at a lower priority.</summary>
		/// <returns>
		///     <see langword="true" /> if the control's layout pass occurs at a lower priority; otherwise, <see langword="false" />.</returns>
		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x06004E18 RID: 19992 RVA: 0x0015FF9F File Offset: 0x0015E19F
		// (set) Token: 0x06004E19 RID: 19993 RVA: 0x0015FFAC File Offset: 0x0015E1AC
		bool IHierarchicalVirtualizationAndScrollInfo.InBackgroundLayout
		{
			get
			{
				return GroupItem.InBackgroundLayoutField.GetValue(this);
			}
			set
			{
				GroupItem.InBackgroundLayoutField.SetValue(this, value);
			}
		}

		/// <summary>Returns the value of the specified property that is associated with the specified item.</summary>
		/// <param name="item">The item that has the specified property associated with it.</param>
		/// <param name="dp">The property whose value to return.</param>
		/// <returns>The value of the specified property that is associated with the specified item.</returns>
		// Token: 0x06004E1A RID: 19994 RVA: 0x0015FFBA File Offset: 0x0015E1BA
		object IContainItemStorage.ReadItemValue(object item, DependencyProperty dp)
		{
			return Helper.ReadItemValue(this, item, dp.GlobalIndex);
		}

		/// <summary>Stores the specified property and value and associates them with the specified item.</summary>
		/// <param name="item">The item to associate the value and property with.</param>
		/// <param name="dp">The property that is associated with the specified item.</param>
		/// <param name="value">The value of the associated property.</param>
		// Token: 0x06004E1B RID: 19995 RVA: 0x0015FFC9 File Offset: 0x0015E1C9
		void IContainItemStorage.StoreItemValue(object item, DependencyProperty dp, object value)
		{
			Helper.StoreItemValue(this, item, dp.GlobalIndex, value);
		}

		/// <summary>Removes the association between the specified item and property.</summary>
		/// <param name="item">The associated item.</param>
		/// <param name="dp">The associated property.</param>
		// Token: 0x06004E1C RID: 19996 RVA: 0x0015FFD9 File Offset: 0x0015E1D9
		void IContainItemStorage.ClearItemValue(object item, DependencyProperty dp)
		{
			Helper.ClearItemValue(this, item, dp.GlobalIndex);
		}

		/// <summary>Removes the specified property from all property lists.</summary>
		/// <param name="dp">The property to remove.</param>
		// Token: 0x06004E1D RID: 19997 RVA: 0x0015FFE8 File Offset: 0x0015E1E8
		void IContainItemStorage.ClearValue(DependencyProperty dp)
		{
			Helper.ClearItemValueStorage(this, new int[]
			{
				dp.GlobalIndex
			});
		}

		/// <summary>Clears all property associations.</summary>
		// Token: 0x06004E1E RID: 19998 RVA: 0x0015FFFF File Offset: 0x0015E1FF
		void IContainItemStorage.Clear()
		{
			Helper.ClearItemValueStorage(this);
		}

		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x06004E1F RID: 19999 RVA: 0x00160008 File Offset: 0x0015E208
		private ItemsControl ParentItemsControl
		{
			get
			{
				DependencyObject dependencyObject = this;
				ItemsControl itemsControl;
				for (;;)
				{
					dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
					itemsControl = (dependencyObject as ItemsControl);
					if (itemsControl != null)
					{
						break;
					}
					if (dependencyObject == null)
					{
						goto Block_2;
					}
				}
				return itemsControl;
				Block_2:
				return null;
			}
		}

		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x06004E20 RID: 20000 RVA: 0x00160030 File Offset: 0x0015E230
		internal IContainItemStorage ParentItemStorageProvider
		{
			get
			{
				DependencyObject parent = VisualTreeHelper.GetParent(this);
				if (parent != null)
				{
					DependencyObject itemsOwnerInternal = ItemsControl.GetItemsOwnerInternal(parent);
					return itemsOwnerInternal as IContainItemStorage;
				}
				return null;
			}
		}

		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x06004E21 RID: 20001 RVA: 0x0015FF7C File Offset: 0x0015E17C
		// (set) Token: 0x06004E22 RID: 20002 RVA: 0x00160056 File Offset: 0x0015E256
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

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x06004E23 RID: 20003 RVA: 0x0016005F File Offset: 0x0015E25F
		private ItemsPresenter ItemsHostPresenter
		{
			get
			{
				if (this._expander != null)
				{
					return Helper.FindTemplatedDescendant<ItemsPresenter>(this._expander, this._expander);
				}
				return Helper.FindTemplatedDescendant<ItemsPresenter>(this, this);
			}
		}

		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06004E24 RID: 20004 RVA: 0x00160082 File Offset: 0x0015E282
		internal Expander Expander
		{
			get
			{
				return this._expander;
			}
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x06004E25 RID: 20005 RVA: 0x0016008A File Offset: 0x0015E28A
		private FrameworkElement ExpanderHeader
		{
			get
			{
				if (this._expander != null)
				{
					return this._expander.GetTemplateChild("HeaderSite") as FrameworkElement;
				}
				return null;
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x06004E26 RID: 20006 RVA: 0x001600AC File Offset: 0x0015E2AC
		private FrameworkElement HeaderElement
		{
			get
			{
				FrameworkElement result = null;
				if (this._header != null)
				{
					result = this._header;
				}
				else if (this._expander != null)
				{
					result = this.ExpanderHeader;
				}
				return result;
			}
		}

		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06004E27 RID: 20007 RVA: 0x001600DC File Offset: 0x0015E2DC
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return GroupItem._dType;
			}
		}

		// Token: 0x04002BAA RID: 11178
		private ItemContainerGenerator _generator;

		// Token: 0x04002BAB RID: 11179
		private Panel _itemsHost;

		// Token: 0x04002BAC RID: 11180
		private FrameworkElement _header;

		// Token: 0x04002BAD RID: 11181
		private Expander _expander;

		// Token: 0x04002BAE RID: 11182
		internal static readonly UncommonField<bool> MustDisableVirtualizationField = new UncommonField<bool>();

		// Token: 0x04002BAF RID: 11183
		internal static readonly UncommonField<bool> InBackgroundLayoutField = new UncommonField<bool>();

		// Token: 0x04002BB0 RID: 11184
		internal static readonly UncommonField<Thickness> DesiredPixelItemsSizeCorrectionFactorField = new UncommonField<Thickness>();

		// Token: 0x04002BB1 RID: 11185
		internal static readonly UncommonField<HierarchicalVirtualizationConstraints> HierarchicalVirtualizationConstraintsField = new UncommonField<HierarchicalVirtualizationConstraints>();

		// Token: 0x04002BB2 RID: 11186
		internal static readonly UncommonField<HierarchicalVirtualizationHeaderDesiredSizes> HierarchicalVirtualizationHeaderDesiredSizesField = new UncommonField<HierarchicalVirtualizationHeaderDesiredSizes>();

		// Token: 0x04002BB3 RID: 11187
		internal static readonly UncommonField<HierarchicalVirtualizationItemDesiredSizes> HierarchicalVirtualizationItemDesiredSizesField = new UncommonField<HierarchicalVirtualizationItemDesiredSizes>();

		// Token: 0x04002BB4 RID: 11188
		private static DependencyObjectType _dType;

		// Token: 0x04002BB5 RID: 11189
		private const string ExpanderHeaderPartName = "HeaderSite";
	}
}
