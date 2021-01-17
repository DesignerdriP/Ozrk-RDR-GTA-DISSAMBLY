using System;
using System.Collections.Specialized;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Provides a framework for <see cref="T:System.Windows.Controls.Panel" /> elements that virtualize their child data collection. This is an abstract class.</summary>
	// Token: 0x0200055D RID: 1373
	public abstract class VirtualizingPanel : Panel
	{
		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> can virtualize items that are grouped or organized in a hierarchy.</summary>
		/// <returns>A value that indicates whether the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> can virtualize items that are grouped or organized in a hierarchy.</returns>
		// Token: 0x170015E2 RID: 5602
		// (get) Token: 0x06005A0F RID: 23055 RVA: 0x0018D421 File Offset: 0x0018B621
		public bool CanHierarchicallyScrollAndVirtualize
		{
			get
			{
				return this.CanHierarchicallyScrollAndVirtualizeCore;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> can virtualize items that are grouped or organized in a hierarchy.</summary>
		/// <returns>
		///     <see langword="false" /> in all cases.</returns>
		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x06005A10 RID: 23056 RVA: 0x0000B02A File Offset: 0x0000922A
		protected virtual bool CanHierarchicallyScrollAndVirtualizeCore
		{
			get
			{
				return false;
			}
		}

		/// <summary>Returns the position of the specified item, relative to the <see cref="T:System.Windows.Controls.VirtualizingPanel" />.</summary>
		/// <param name="child">The element whose position to find.</param>
		/// <returns>The position of the specified item, relative to the <see cref="T:System.Windows.Controls.VirtualizingPanel" />.</returns>
		// Token: 0x06005A11 RID: 23057 RVA: 0x0018D429 File Offset: 0x0018B629
		public double GetItemOffset(UIElement child)
		{
			return this.GetItemOffsetCore(child);
		}

		/// <summary>Returns the position of the specified item, relative to the <see cref="T:System.Windows.Controls.VirtualizingPanel" />.</summary>
		/// <param name="child">The element whose position to find.</param>
		/// <returns>0 in all cases.</returns>
		// Token: 0x06005A12 RID: 23058 RVA: 0x0018D432 File Offset: 0x0018B632
		protected virtual double GetItemOffsetCore(UIElement child)
		{
			return 0.0;
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizing" /> attached property.</summary>
		/// <param name="element">The object from which the attached property value is read.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> is virtualizing its content; otherwise <see langword="false" />.</returns>
		// Token: 0x06005A13 RID: 23059 RVA: 0x0018D43D File Offset: 0x0018B63D
		public static bool GetIsVirtualizing(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(VirtualizingPanel.IsVirtualizingProperty);
		}

		/// <summary>Sets the value of the <see cref="F:System.Windows.Controls.VirtualizingStackPanel.IsVirtualizingProperty" /> attached property.</summary>
		/// <param name="element">The object to which the attached property value is set.</param>
		/// <param name="value">
		///       <see langword="true" /> if the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> is virtualizing; otherwise <see langword="false" />.</param>
		// Token: 0x06005A14 RID: 23060 RVA: 0x0018D45D File Offset: 0x0018B65D
		public static void SetIsVirtualizing(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.IsVirtualizingProperty, value);
		}

		/// <summary>Returns the <see cref="P:System.Windows.Controls.VirtualizingPanel.VirtualizationMode" /> attached property for the specified object.</summary>
		/// <param name="element">The object from which the <see cref="P:System.Windows.Controls.VirtualizingPanel.VirtualizationMode" /> property is read.</param>
		/// <returns>One of the enumeration values that specifies whether the object uses container recycling.</returns>
		// Token: 0x06005A15 RID: 23061 RVA: 0x0018D479 File Offset: 0x0018B679
		public static VirtualizationMode GetVirtualizationMode(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (VirtualizationMode)element.GetValue(VirtualizingPanel.VirtualizationModeProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.VirtualizingPanel.VirtualizationMode" /> attached property on the specified object.</summary>
		/// <param name="element">The element on which to set the <see cref="P:System.Windows.Controls.VirtualizingPanel.VirtualizationMode" /> property.</param>
		/// <param name="value">One of the enumeration values that specifies whether <paramref name="element" /> uses container recycling.</param>
		// Token: 0x06005A16 RID: 23062 RVA: 0x0018D499 File Offset: 0x0018B699
		public static void SetVirtualizationMode(DependencyObject element, VirtualizationMode value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.VirtualizationModeProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizingWhenGrouping" /> property.</summary>
		/// <param name="element">The element to get the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizingWhenGrouping" /> attached property from.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> virtualizes the grouped items in its collection; otherwise, <see langword="false" />. </returns>
		// Token: 0x06005A17 RID: 23063 RVA: 0x0018D4BA File Offset: 0x0018B6BA
		public static bool GetIsVirtualizingWhenGrouping(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizingWhenGrouping" /> attached property.</summary>
		/// <param name="element">The object to set the property on.</param>
		/// <param name="value">
		///       <see langword="true" /> to specify that the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> virtualizes the grouped items in its collection; otherwise, <see langword="false" />.</param>
		// Token: 0x06005A18 RID: 23064 RVA: 0x0018D4DA File Offset: 0x0018B6DA
		public static void SetIsVirtualizingWhenGrouping(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.IsVirtualizingWhenGroupingProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> property.</summary>
		/// <param name="element">The element to get the <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> attached property from.</param>
		/// <returns>A value that indicates whether scrolling is measured as items in the collection or as pixels.</returns>
		// Token: 0x06005A19 RID: 23065 RVA: 0x0018D4F6 File Offset: 0x0018B6F6
		public static ScrollUnit GetScrollUnit(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (ScrollUnit)element.GetValue(VirtualizingPanel.ScrollUnitProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> attached property.</summary>
		/// <param name="element">The object to set the property on.</param>
		/// <param name="value">A value that indicates whether scrolling is measured as items in the collection or as pixels.</param>
		// Token: 0x06005A1A RID: 23066 RVA: 0x0018D516 File Offset: 0x0018B716
		public static void SetScrollUnit(DependencyObject element, ScrollUnit value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.ScrollUnitProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> property.</summary>
		/// <param name="element">The element to get the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> attached property from.</param>
		/// <returns>The size of the cache before and after the viewport when the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> is virtualizing. </returns>
		// Token: 0x06005A1B RID: 23067 RVA: 0x0018D537 File Offset: 0x0018B737
		public static VirtualizationCacheLength GetCacheLength(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (VirtualizationCacheLength)element.GetValue(VirtualizingPanel.CacheLengthProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> attached property.</summary>
		/// <param name="element">The object to set the property on.</param>
		/// <param name="value">The size of the cache before and after the viewport when the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> is virtualizing.</param>
		// Token: 0x06005A1C RID: 23068 RVA: 0x0018D557 File Offset: 0x0018B757
		public static void SetCacheLength(DependencyObject element, VirtualizationCacheLength value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.CacheLengthProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLengthUnit" /> property.</summary>
		/// <param name="element">The element to get the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLengthUnit" /> attached property from.</param>
		/// <returns>The type of unit that is used by the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> property. </returns>
		// Token: 0x06005A1D RID: 23069 RVA: 0x0018D578 File Offset: 0x0018B778
		public static VirtualizationCacheLengthUnit GetCacheLengthUnit(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (VirtualizationCacheLengthUnit)element.GetValue(VirtualizingPanel.CacheLengthUnitProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLengthUnit" /> attached property.</summary>
		/// <param name="element">The object to set the property on.</param>
		/// <param name="value">The type of unit that is used by the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> property. </param>
		// Token: 0x06005A1E RID: 23070 RVA: 0x0018D598 File Offset: 0x0018B798
		public static void SetCacheLengthUnit(DependencyObject element, VirtualizationCacheLengthUnit value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.CacheLengthUnitProperty, value);
		}

		/// <summary>Gets the value of the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsContainerVirtualizable" /> property.</summary>
		/// <param name="element">The element to get the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsContainerVirtualizable" /> attached property from.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> should virtualize an item; otherwise, <see langword="false" />. </returns>
		// Token: 0x06005A1F RID: 23071 RVA: 0x0018D5B9 File Offset: 0x0018B7B9
		public static bool GetIsContainerVirtualizable(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (bool)element.GetValue(VirtualizingPanel.IsContainerVirtualizableProperty);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsContainerVirtualizable" /> attached property.</summary>
		/// <param name="element">The object to set the property on.</param>
		/// <param name="value">
		///       <see langword="true" /> to indicate that the <see cref="T:System.Windows.Controls.VirtualizingPanel" /> should virtualize an item; otherwise, <see langword="false" />. </param>
		// Token: 0x06005A20 RID: 23072 RVA: 0x0018D5D9 File Offset: 0x0018B7D9
		public static void SetIsContainerVirtualizable(DependencyObject element, bool value)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(VirtualizingPanel.IsContainerVirtualizableProperty, value);
		}

		// Token: 0x06005A21 RID: 23073 RVA: 0x0018D5F5 File Offset: 0x0018B7F5
		internal static bool GetShouldCacheContainerSize(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return !VirtualizingStackPanel.IsVSP45Compat || (bool)element.GetValue(VirtualizingPanel.ShouldCacheContainerSizeProperty);
		}

		// Token: 0x06005A22 RID: 23074 RVA: 0x0018D620 File Offset: 0x0018B820
		private static bool ValidateCacheSizeBeforeOrAfterViewport(object value)
		{
			VirtualizationCacheLength virtualizationCacheLength = (VirtualizationCacheLength)value;
			return DoubleUtil.GreaterThanOrClose(virtualizationCacheLength.CacheBeforeViewport, 0.0) && DoubleUtil.GreaterThanOrClose(virtualizationCacheLength.CacheAfterViewport, 0.0);
		}

		// Token: 0x06005A23 RID: 23075 RVA: 0x0018D664 File Offset: 0x0018B864
		private static object CoerceIsVirtualizingWhenGrouping(DependencyObject d, object baseValue)
		{
			bool isVirtualizing = VirtualizingPanel.GetIsVirtualizing(d);
			return isVirtualizing && (bool)baseValue;
		}

		// Token: 0x06005A24 RID: 23076 RVA: 0x0018D68C File Offset: 0x0018B88C
		internal static void OnVirtualizationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsControl itemsControl = d as ItemsControl;
			if (itemsControl != null)
			{
				Panel itemsHost = itemsControl.ItemsHost;
				if (itemsHost != null)
				{
					itemsHost.InvalidateMeasure();
					ItemsPresenter itemsPresenter = VisualTreeHelper.GetParent(itemsHost) as ItemsPresenter;
					if (itemsPresenter != null)
					{
						itemsPresenter.InvalidateMeasure();
					}
					if (d is TreeView)
					{
						DependencyProperty property = e.Property;
						if (property == VirtualizingStackPanel.IsVirtualizingProperty || property == VirtualizingPanel.IsVirtualizingWhenGroupingProperty || property == VirtualizingStackPanel.VirtualizationModeProperty || property == VirtualizingPanel.ScrollUnitProperty)
						{
							VirtualizingPanel.VirtualizationPropertyChangePropagationRecursive(itemsControl, itemsHost);
						}
					}
				}
			}
		}

		// Token: 0x06005A25 RID: 23077 RVA: 0x0018D700 File Offset: 0x0018B900
		private static void VirtualizationPropertyChangePropagationRecursive(DependencyObject parent, Panel itemsHost)
		{
			UIElementCollection internalChildren = itemsHost.InternalChildren;
			int count = internalChildren.Count;
			for (int i = 0; i < count; i++)
			{
				IHierarchicalVirtualizationAndScrollInfo hierarchicalVirtualizationAndScrollInfo = internalChildren[i] as IHierarchicalVirtualizationAndScrollInfo;
				if (hierarchicalVirtualizationAndScrollInfo != null)
				{
					TreeViewItem.IsVirtualizingPropagationHelper(parent, (DependencyObject)hierarchicalVirtualizationAndScrollInfo);
					Panel itemsHost2 = hierarchicalVirtualizationAndScrollInfo.ItemsHost;
					if (itemsHost2 != null)
					{
						VirtualizingPanel.VirtualizationPropertyChangePropagationRecursive((DependencyObject)hierarchicalVirtualizationAndScrollInfo, itemsHost2);
					}
				}
			}
		}

		/// <summary>Gets a value that identifies the <see cref="T:System.Windows.Controls.ItemContainerGenerator" /> for this <see cref="T:System.Windows.Controls.VirtualizingPanel" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ItemContainerGenerator" /> for this <see cref="T:System.Windows.Controls.VirtualizingPanel" />.</returns>
		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x06005A26 RID: 23078 RVA: 0x0018D75C File Offset: 0x0018B95C
		public IItemContainerGenerator ItemContainerGenerator
		{
			get
			{
				return base.Generator;
			}
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x00002137 File Offset: 0x00000337
		internal override void GenerateChildren()
		{
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.UIElement" /> to the <see cref="P:System.Windows.Controls.Panel.InternalChildren" /> collection of a <see cref="T:System.Windows.Controls.VirtualizingPanel" /> element.</summary>
		/// <param name="child">The <see cref="T:System.Windows.UIElement" /> child to add to the collection.</param>
		// Token: 0x06005A28 RID: 23080 RVA: 0x0018D764 File Offset: 0x0018B964
		protected void AddInternalChild(UIElement child)
		{
			VirtualizingPanel.AddInternalChild(base.InternalChildren, child);
		}

		/// <summary>Adds the specified <see cref="T:System.Windows.UIElement" /> to the <see cref="P:System.Windows.Controls.Panel.InternalChildren" /> collection of a <see cref="T:System.Windows.Controls.VirtualizingPanel" /> element at the specified index position.</summary>
		/// <param name="index">The index position within the collection at which the child element is inserted.</param>
		/// <param name="child">The <see cref="T:System.Windows.UIElement" /> child to add to the collection.</param>
		// Token: 0x06005A29 RID: 23081 RVA: 0x0018D772 File Offset: 0x0018B972
		protected void InsertInternalChild(int index, UIElement child)
		{
			VirtualizingPanel.InsertInternalChild(base.InternalChildren, index, child);
		}

		/// <summary>Removes child elements from the <see cref="P:System.Windows.Controls.Panel.InternalChildren" /> collection.</summary>
		/// <param name="index">The beginning index position within the collection at which the first child element is removed.</param>
		/// <param name="range">The total number of child elements to remove from the collection.</param>
		// Token: 0x06005A2A RID: 23082 RVA: 0x0018D781 File Offset: 0x0018B981
		protected void RemoveInternalChildRange(int index, int range)
		{
			VirtualizingPanel.RemoveInternalChildRange(base.InternalChildren, index, range);
		}

		// Token: 0x06005A2B RID: 23083 RVA: 0x0018D790 File Offset: 0x0018B990
		internal static void AddInternalChild(UIElementCollection children, UIElement child)
		{
			children.AddInternal(child);
		}

		// Token: 0x06005A2C RID: 23084 RVA: 0x0018D79A File Offset: 0x0018B99A
		internal static void InsertInternalChild(UIElementCollection children, int index, UIElement child)
		{
			children.InsertInternal(index, child);
		}

		// Token: 0x06005A2D RID: 23085 RVA: 0x0018D7A4 File Offset: 0x0018B9A4
		internal static void RemoveInternalChildRange(UIElementCollection children, int index, int range)
		{
			children.RemoveRangeInternal(index, range);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection that is associated with the <see cref="T:System.Windows.Controls.ItemsControl" /> for this <see cref="T:System.Windows.Controls.Panel" /> changes.</summary>
		/// <param name="sender">The <see cref="T:System.Object" /> that raised the event.</param>
		/// <param name="args">Provides data for the <see cref="E:System.Windows.Controls.ItemContainerGenerator.ItemsChanged" /> event.</param>
		// Token: 0x06005A2E RID: 23086 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnItemsChanged(object sender, ItemsChangedEventArgs args)
		{
		}

		/// <summary>Returns a value that indicates whether a changed item in an <see cref="T:System.Windows.Controls.ItemsControl" /> affects the layout for this panel.</summary>
		/// <param name="areItemChangesLocal">
		///       <see langword="true" /> if the changed item is a direct child of this <see cref="T:System.Windows.Controls.VirtualizingPanel" />; <see langword="false" /> if the changed item is an indirect descendant of the <see cref="T:System.Windows.Controls.VirtualizingPanel" />.  </param>
		/// <param name="args">Contains data regarding the changed item.</param>
		/// <returns>
		///     <see langword="true" /> if the changed item in an <see cref="T:System.Windows.Controls.ItemsControl" /> affects the layout for this panel; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005A2F RID: 23087 RVA: 0x0018D7AE File Offset: 0x0018B9AE
		public bool ShouldItemsChangeAffectLayout(bool areItemChangesLocal, ItemsChangedEventArgs args)
		{
			return this.ShouldItemsChangeAffectLayoutCore(areItemChangesLocal, args);
		}

		/// <summary>Returns a value that indicates whether a changed item in an <see cref="T:System.Windows.Controls.ItemsControl" /> affects the layout for this panel.</summary>
		/// <param name="areItemChangesLocal">
		///       <see langword="true" /> if the changed item is a direct child of this <see cref="T:System.Windows.Controls.VirtualizingPanel" />; <see langword="false" /> if the changed item is an indirect descendant of the <see cref="T:System.Windows.Controls.VirtualizingPanel" />.  </param>
		/// <param name="args">Contains data regarding the changed item.</param>
		/// <returns>
		///     <see langword="true" /> if the changed item in an <see cref="T:System.Windows.Controls.ItemsControl" /> affects the layout for this panel; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005A30 RID: 23088 RVA: 0x00016748 File Offset: 0x00014948
		protected virtual bool ShouldItemsChangeAffectLayoutCore(bool areItemChangesLocal, ItemsChangedEventArgs args)
		{
			return true;
		}

		/// <summary>Called when the collection of child elements is cleared by the base <see cref="T:System.Windows.Controls.Panel" /> class.</summary>
		// Token: 0x06005A31 RID: 23089 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnClearChildren()
		{
		}

		/// <summary>Generates the item at the specified index location and makes it visible.</summary>
		/// <param name="index">The index position of the item that is generated and made visible.</param>
		// Token: 0x06005A32 RID: 23090 RVA: 0x00144757 File Offset: 0x00142957
		public void BringIndexIntoViewPublic(int index)
		{
			this.BringIndexIntoView(index);
		}

		/// <summary>When implemented in a derived class, generates the item at the specified index location and makes it visible.</summary>
		/// <param name="index">The index position of the item that is generated and made visible.</param>
		// Token: 0x06005A33 RID: 23091 RVA: 0x00002137 File Offset: 0x00000337
		protected internal virtual void BringIndexIntoView(int index)
		{
		}

		// Token: 0x06005A34 RID: 23092 RVA: 0x0018D7B8 File Offset: 0x0018B9B8
		internal override bool OnItemsChangedInternal(object sender, ItemsChangedEventArgs args)
		{
			NotifyCollectionChangedAction action = args.Action;
			if (action > NotifyCollectionChangedAction.Move)
			{
				base.OnItemsChangedInternal(sender, args);
			}
			this.OnItemsChanged(sender, args);
			return this.ShouldItemsChangeAffectLayout(true, args);
		}

		// Token: 0x06005A35 RID: 23093 RVA: 0x0018D7E9 File Offset: 0x0018B9E9
		internal override void OnClearChildrenInternal()
		{
			this.OnClearChildren();
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizing" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizing" /> dependency property.</returns>
		// Token: 0x04002F2E RID: 12078
		public static readonly DependencyProperty IsVirtualizingProperty = DependencyProperty.RegisterAttached("IsVirtualizing", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.VirtualizationMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.VirtualizationMode" /> dependency property.</returns>
		// Token: 0x04002F2F RID: 12079
		public static readonly DependencyProperty VirtualizationModeProperty = DependencyProperty.RegisterAttached("VirtualizationMode", typeof(VirtualizationMode), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(VirtualizationMode.Standard, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizingWhenGrouping" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsVirtualizingWhenGrouping" /> attached property.</returns>
		// Token: 0x04002F30 RID: 12080
		public static readonly DependencyProperty IsVirtualizingWhenGroupingProperty = DependencyProperty.RegisterAttached("IsVirtualizingWhenGrouping", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged), new CoerceValueCallback(VirtualizingPanel.CoerceIsVirtualizingWhenGrouping)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.ScrollUnit" /> attached property.</returns>
		// Token: 0x04002F31 RID: 12081
		public static readonly DependencyProperty ScrollUnitProperty = DependencyProperty.RegisterAttached("ScrollUnit", typeof(ScrollUnit), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(ScrollUnit.Item, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> attached property.</returns>
		// Token: 0x04002F32 RID: 12082
		public static readonly DependencyProperty CacheLengthProperty = DependencyProperty.RegisterAttached("CacheLength", typeof(VirtualizationCacheLength), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(new VirtualizationCacheLength(1.0), FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)), new ValidateValueCallback(VirtualizingPanel.ValidateCacheSizeBeforeOrAfterViewport));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLengthUnit" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLengthUnit" /> attached property.</returns>
		// Token: 0x04002F33 RID: 12083
		public static readonly DependencyProperty CacheLengthUnitProperty = DependencyProperty.RegisterAttached("CacheLengthUnit", typeof(VirtualizationCacheLengthUnit), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(VirtualizationCacheLengthUnit.Page, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(VirtualizingPanel.OnVirtualizationPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsContainerVirtualizable" /> attached property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.VirtualizingPanel.IsContainerVirtualizable" /> attached property.</returns>
		// Token: 0x04002F34 RID: 12084
		public static readonly DependencyProperty IsContainerVirtualizableProperty = DependencyProperty.RegisterAttached("IsContainerVirtualizable", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(true));

		// Token: 0x04002F35 RID: 12085
		internal static readonly DependencyProperty ShouldCacheContainerSizeProperty = DependencyProperty.RegisterAttached("ShouldCacheContainerSize", typeof(bool), typeof(VirtualizingPanel), new FrameworkPropertyMetadata(true));
	}
}
