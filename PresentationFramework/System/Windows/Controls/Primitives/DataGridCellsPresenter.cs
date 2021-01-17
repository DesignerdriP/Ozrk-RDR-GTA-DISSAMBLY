﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using MS.Internal;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Used within the template of a <see cref="T:System.Windows.Controls.DataGrid" /> to specify the location in the control's visual tree where the cells are to be added. </summary>
	// Token: 0x0200057C RID: 1404
	public class DataGridCellsPresenter : ItemsControl
	{
		// Token: 0x06005C8E RID: 23694 RVA: 0x001A0968 File Offset: 0x0019EB68
		static DataGridCellsPresenter()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(typeof(DataGridCellsPresenter)));
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DataGridCellsPanel)))));
			UIElement.FocusableProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(false));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridCellsPresenter.OnNotifyHeightPropertyChanged), new CoerceValueCallback(DataGridCellsPresenter.OnCoerceHeight)));
			FrameworkElement.MinHeightProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridCellsPresenter.OnNotifyHeightPropertyChanged), new CoerceValueCallback(DataGridCellsPresenter.OnCoerceMinHeight)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridCellsPresenter.OnIsVirtualizingPropertyChanged), new CoerceValueCallback(DataGridCellsPresenter.OnCoerceIsVirtualizingProperty)));
			VirtualizingPanel.VirtualizationModeProperty.OverrideMetadata(typeof(DataGridCellsPresenter), new FrameworkPropertyMetadata(VirtualizationMode.Recycling));
		}

		/// <summary>It's invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.</summary>
		// Token: 0x06005C90 RID: 23696 RVA: 0x001A0A9C File Offset: 0x0019EC9C
		public override void OnApplyTemplate()
		{
			if (this.InternalItemsHost != null && !base.IsAncestorOf(this.InternalItemsHost))
			{
				this.InternalItemsHost = null;
			}
			base.OnApplyTemplate();
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner != null)
			{
				dataGridRowOwner.CellsPresenter = this;
				this.Item = dataGridRowOwner.Item;
			}
			this.SyncProperties(false);
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x001A0AF0 File Offset: 0x0019ECF0
		internal void SyncProperties(bool forcePrepareCells)
		{
			DataGrid dataGridOwner = this.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
			DataGridHelper.TransferProperty(this, FrameworkElement.MinHeightProperty);
			DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
			this.NotifyPropertyChanged(this, new DependencyPropertyChangedEventArgs(DataGrid.CellStyleProperty, null, null), DataGridNotificationTarget.Cells);
			MultipleCopiesCollection multipleCopiesCollection = base.ItemsSource as MultipleCopiesCollection;
			if (multipleCopiesCollection != null)
			{
				ObservableCollection<DataGridColumn> columns = dataGridOwner.Columns;
				int count = columns.Count;
				int count2 = multipleCopiesCollection.Count;
				int num = 0;
				bool flag = false;
				if (count != count2)
				{
					multipleCopiesCollection.SyncToCount(count);
					num = Math.Min(count, count2);
				}
				else if (forcePrepareCells)
				{
					num = count;
				}
				DataGridCellsPanel dataGridCellsPanel = this.InternalItemsHost as DataGridCellsPanel;
				if (dataGridCellsPanel != null)
				{
					if (dataGridCellsPanel.HasCorrectRealizedColumns)
					{
						dataGridCellsPanel.InvalidateArrange();
					}
					else
					{
						this.InvalidateDataGridCellsPanelMeasureAndArrange();
						flag = true;
					}
				}
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				for (int i = 0; i < num; i++)
				{
					DataGridCell dataGridCell = (DataGridCell)base.ItemContainerGenerator.ContainerFromIndex(i);
					if (dataGridCell != null)
					{
						dataGridCell.PrepareCell(dataGridRowOwner.Item, this, dataGridRowOwner);
						if (!flag && !DoubleUtil.AreClose(dataGridCell.ActualWidth, columns[i].Width.DisplayValue))
						{
							this.InvalidateDataGridCellsPanelMeasureAndArrange();
							flag = true;
						}
					}
				}
				if (!flag)
				{
					for (int j = num; j < count; j++)
					{
						DataGridCell dataGridCell = (DataGridCell)base.ItemContainerGenerator.ContainerFromIndex(j);
						if (dataGridCell != null && !DoubleUtil.AreClose(dataGridCell.ActualWidth, columns[j].Width.DisplayValue))
						{
							this.InvalidateDataGridCellsPanelMeasureAndArrange();
							return;
						}
					}
				}
			}
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x001A0C80 File Offset: 0x0019EE80
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			DataGridCellsPresenter dataGridCellsPresenter = d as DataGridCellsPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCellsPresenter, baseValue, FrameworkElement.HeightProperty, dataGridCellsPresenter.DataGridOwner, DataGrid.RowHeightProperty);
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x001A0CAC File Offset: 0x0019EEAC
		private static object OnCoerceMinHeight(DependencyObject d, object baseValue)
		{
			DataGridCellsPresenter dataGridCellsPresenter = d as DataGridCellsPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCellsPresenter, baseValue, FrameworkElement.MinHeightProperty, dataGridCellsPresenter.DataGridOwner, DataGrid.MinRowHeightProperty);
		}

		/// <summary>Gets the data item that the row represents. </summary>
		/// <returns>The data item that the row represents. </returns>
		// Token: 0x1700166A RID: 5738
		// (get) Token: 0x06005C94 RID: 23700 RVA: 0x001A0CD7 File Offset: 0x0019EED7
		// (set) Token: 0x06005C95 RID: 23701 RVA: 0x001A0CE0 File Offset: 0x0019EEE0
		public object Item
		{
			get
			{
				return this._item;
			}
			internal set
			{
				if (this._item != value)
				{
					object item = this._item;
					this._item = value;
					this.OnItemChanged(item, this._item);
				}
			}
		}

		/// <summary>Updates the displayed cells when the <see cref="P:System.Windows.Controls.Primitives.DataGridCellsPresenter.Item" /> property value has changed. </summary>
		/// <param name="oldItem">The previous value of the <see cref="P:System.Windows.Controls.Primitives.DataGridCellsPresenter.Item" /> property.</param>
		/// <param name="newItem">The new value of the <see cref="P:System.Windows.Controls.Primitives.DataGridCellsPresenter.Item" /> property.</param>
		// Token: 0x06005C96 RID: 23702 RVA: 0x001A0D14 File Offset: 0x0019EF14
		protected virtual void OnItemChanged(object oldItem, object newItem)
		{
			ObservableCollection<DataGridColumn> columns = this.Columns;
			if (columns != null)
			{
				MultipleCopiesCollection multipleCopiesCollection = base.ItemsSource as MultipleCopiesCollection;
				if (multipleCopiesCollection == null)
				{
					multipleCopiesCollection = new MultipleCopiesCollection(newItem, columns.Count);
					base.ItemsSource = multipleCopiesCollection;
					return;
				}
				multipleCopiesCollection.CopiedItem = newItem;
			}
		}

		/// <summary>Determines if the specified item is (or is eligible to be) its own container.</summary>
		/// <param name="item">The item to check.</param>
		/// <returns>
		///     <see langword="true" /> if the item is (or is eligible to be) its own container; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005C97 RID: 23703 RVA: 0x001A0D56 File Offset: 0x0019EF56
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is DataGridCell;
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x00169699 File Offset: 0x00167899
		internal bool IsItemItsOwnContainerInternal(object item)
		{
			return this.IsItemItsOwnContainerOverride(item);
		}

		/// <summary>Returns a new <see cref="T:System.Windows.Controls.DataGridCell" /> instance to contain a cell value.</summary>
		/// <returns>A new <see cref="T:System.Windows.Controls.DataGridCell" /> instance.</returns>
		// Token: 0x06005C99 RID: 23705 RVA: 0x001A0D61 File Offset: 0x0019EF61
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new DataGridCell();
		}

		/// <summary>Prepares the cell to display the specified item.</summary>
		/// <param name="element">The cell to prepare.</param>
		/// <param name="item">The item to display.</param>
		// Token: 0x06005C9A RID: 23706 RVA: 0x001A0D68 File Offset: 0x0019EF68
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridCell dataGridCell = (DataGridCell)element;
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridCell.RowOwner != dataGridRowOwner)
			{
				dataGridCell.Tracker.StartTracking(ref this._cellTrackingRoot);
			}
			dataGridCell.PrepareCell(item, this, dataGridRowOwner);
		}

		/// <summary>Clears the container reference for the specified cell.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Controls.DataGridCell" /> to clear.</param>
		/// <param name="item">The data item. This value is not used.</param>
		// Token: 0x06005C9B RID: 23707 RVA: 0x001A0DA8 File Offset: 0x0019EFA8
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			DataGridCell dataGridCell = (DataGridCell)element;
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridCell.RowOwner == dataGridRowOwner)
			{
				dataGridCell.Tracker.StopTracking(ref this._cellTrackingRoot);
			}
			dataGridCell.ClearCell(dataGridRowOwner);
		}

		/// <summary>Updates the displayed cells when the <see cref="P:System.Windows.Controls.DataGrid.Columns" /> collection has changed. </summary>
		/// <param name="columns">The <see cref="P:System.Windows.Controls.DataGrid.Columns" /> collection.</param>
		/// <param name="e">The event data from the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged" /> event of the <see cref="P:System.Windows.Controls.DataGrid.Columns" /> collection.</param>
		// Token: 0x06005C9C RID: 23708 RVA: 0x001A0DE4 File Offset: 0x0019EFE4
		protected internal virtual void OnColumnsChanged(ObservableCollection<DataGridColumn> columns, NotifyCollectionChangedEventArgs e)
		{
			MultipleCopiesCollection multipleCopiesCollection = base.ItemsSource as MultipleCopiesCollection;
			if (multipleCopiesCollection != null)
			{
				multipleCopiesCollection.MirrorCollectionChange(e);
			}
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x001A0E07 File Offset: 0x0019F007
		private static void OnNotifyHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGridCellsPresenter)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter);
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x001A0E17 File Offset: 0x0019F017
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x001A0E28 File Offset: 0x0019F028
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyCellsPresenter(target))
			{
				if (e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.DisplayIndexProperty)
				{
					if (((DataGridColumn)d).IsVisible)
					{
						this.InvalidateDataGridCellsPanelMeasureAndArrangeImpl(e.Property == DataGridColumn.WidthProperty);
					}
				}
				else if (e.Property == DataGrid.FrozenColumnCountProperty || e.Property == DataGridColumn.VisibilityProperty || e.Property == DataGrid.CellsPanelHorizontalOffsetProperty || e.Property == DataGrid.HorizontalScrollOffsetProperty || string.Compare(propertyName, "ViewportWidth", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "DelayedColumnWidthComputation", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange();
				}
				else if (string.Compare(propertyName, "RealizedColumnsBlockListForNonVirtualizedRows", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange(false);
				}
				else if (string.Compare(propertyName, "RealizedColumnsBlockListForVirtualizedRows", StringComparison.Ordinal) == 0)
				{
					this.InvalidateDataGridCellsPanelMeasureAndArrange(true);
				}
				else if (e.Property == DataGrid.RowHeightProperty || e.Property == FrameworkElement.HeightProperty)
				{
					DataGridHelper.TransferProperty(this, FrameworkElement.HeightProperty);
				}
				else if (e.Property == DataGrid.MinRowHeightProperty || e.Property == FrameworkElement.MinHeightProperty)
				{
					DataGridHelper.TransferProperty(this, FrameworkElement.MinHeightProperty);
				}
				else if (e.Property == DataGrid.EnableColumnVirtualizationProperty)
				{
					DataGridHelper.TransferProperty(this, VirtualizingPanel.IsVirtualizingProperty);
				}
			}
			if (DataGridHelper.ShouldNotifyCells(target) || DataGridHelper.ShouldRefreshCellContent(target))
			{
				for (ContainerTracking<DataGridCell> containerTracking = this._cellTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
				{
					containerTracking.Container.NotifyPropertyChanged(d, propertyName, e, target);
				}
			}
		}

		/// <summary>Called to remeasure a control.</summary>
		/// <param name="availableSize">The available size that this element can give to child elements.</param>
		/// <returns>The size of the control, up to the maximum specified by constraint.</returns>
		// Token: 0x06005CA0 RID: 23712 RVA: 0x001A0FB6 File Offset: 0x0019F1B6
		protected override Size MeasureOverride(Size availableSize)
		{
			return base.MeasureOverride(availableSize);
		}

		/// <summary>Called to arrange and size the content of a <see cref="T:System.Windows.Controls.Control" /> object.</summary>
		/// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
		/// <returns>The size of the control.</returns>
		// Token: 0x06005CA1 RID: 23713 RVA: 0x001A0FBF File Offset: 0x0019F1BF
		protected override Size ArrangeOverride(Size finalSize)
		{
			return base.ArrangeOverride(finalSize);
		}

		/// <summary>Called by the layout system to draw a horizontal line below the cells if horizontal grid lines are visible.</summary>
		/// <param name="drawingContext">The drawing instructions for the cells. This context is provided to the layout system.</param>
		// Token: 0x06005CA2 RID: 23714 RVA: 0x001A0FC8 File Offset: 0x0019F1C8
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			DataGridRow dataGridRowOwner = this.DataGridRowOwner;
			if (dataGridRowOwner == null)
			{
				return;
			}
			DataGrid dataGridOwner = dataGridRowOwner.DataGridOwner;
			if (dataGridOwner == null)
			{
				return;
			}
			if (DataGridHelper.IsGridLineVisible(dataGridOwner, true))
			{
				double horizontalGridLineThickness = dataGridOwner.HorizontalGridLineThickness;
				Rect rectangle = new Rect(new Size(base.RenderSize.Width, horizontalGridLineThickness));
				rectangle.Y = base.RenderSize.Height - horizontalGridLineThickness;
				drawingContext.DrawRectangle(dataGridOwner.HorizontalGridLinesBrush, null, rectangle);
			}
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x001A1044 File Offset: 0x0019F244
		private static void OnIsVirtualizingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridCellsPresenter dataGridCellsPresenter = (DataGridCellsPresenter)d;
			DataGridHelper.TransferProperty(dataGridCellsPresenter, VirtualizingPanel.IsVirtualizingProperty);
			if (e.OldValue != dataGridCellsPresenter.GetValue(VirtualizingPanel.IsVirtualizingProperty))
			{
				dataGridCellsPresenter.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x001A1080 File Offset: 0x0019F280
		private static object OnCoerceIsVirtualizingProperty(DependencyObject d, object baseValue)
		{
			DataGridCellsPresenter dataGridCellsPresenter = d as DataGridCellsPresenter;
			return DataGridHelper.GetCoercedTransferPropertyValue(dataGridCellsPresenter, baseValue, VirtualizingPanel.IsVirtualizingProperty, dataGridCellsPresenter.DataGridOwner, DataGrid.EnableColumnVirtualizationProperty);
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x001A10AB File Offset: 0x0019F2AB
		internal void InvalidateDataGridCellsPanelMeasureAndArrange()
		{
			this.InvalidateDataGridCellsPanelMeasureAndArrangeImpl(false);
		}

		// Token: 0x06005CA6 RID: 23718 RVA: 0x001A10B4 File Offset: 0x0019F2B4
		private void InvalidateDataGridCellsPanelMeasureAndArrangeImpl(bool invalidateMeasureUptoRowsPresenter)
		{
			if (this._internalItemsHost != null)
			{
				this._internalItemsHost.InvalidateMeasure();
				this._internalItemsHost.InvalidateArrange();
				if (invalidateMeasureUptoRowsPresenter)
				{
					DataGrid dataGridOwner = this.DataGridOwner;
					if (dataGridOwner != null && dataGridOwner.InternalItemsHost != null)
					{
						Helper.InvalidateMeasureOnPath(this._internalItemsHost, dataGridOwner.InternalItemsHost, false, true);
					}
				}
			}
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x001A1107 File Offset: 0x0019F307
		private void InvalidateDataGridCellsPanelMeasureAndArrange(bool withColumnVirtualization)
		{
			if (withColumnVirtualization == VirtualizingPanel.GetIsVirtualizing(this))
			{
				this.InvalidateDataGridCellsPanelMeasureAndArrange();
			}
		}

		// Token: 0x1700166B RID: 5739
		// (get) Token: 0x06005CA8 RID: 23720 RVA: 0x001A1118 File Offset: 0x0019F318
		// (set) Token: 0x06005CA9 RID: 23721 RVA: 0x001A1120 File Offset: 0x0019F320
		internal Panel InternalItemsHost
		{
			get
			{
				return this._internalItemsHost;
			}
			set
			{
				this._internalItemsHost = value;
			}
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x001A112C File Offset: 0x0019F32C
		internal void ScrollCellIntoView(int index)
		{
			DataGridCellsPanel dataGridCellsPanel = this.InternalItemsHost as DataGridCellsPanel;
			if (dataGridCellsPanel != null)
			{
				dataGridCellsPanel.InternalBringIndexIntoView(index);
				return;
			}
		}

		// Token: 0x1700166C RID: 5740
		// (get) Token: 0x06005CAB RID: 23723 RVA: 0x001A1150 File Offset: 0x0019F350
		internal DataGrid DataGridOwner
		{
			get
			{
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				if (dataGridRowOwner != null)
				{
					return dataGridRowOwner.DataGridOwner;
				}
				return null;
			}
		}

		// Token: 0x1700166D RID: 5741
		// (get) Token: 0x06005CAC RID: 23724 RVA: 0x001A116F File Offset: 0x0019F36F
		internal DataGridRow DataGridRowOwner
		{
			get
			{
				return DataGridHelper.FindParent<DataGridRow>(this);
			}
		}

		// Token: 0x1700166E RID: 5742
		// (get) Token: 0x06005CAD RID: 23725 RVA: 0x001A1178 File Offset: 0x0019F378
		private ObservableCollection<DataGridColumn> Columns
		{
			get
			{
				DataGridRow dataGridRowOwner = this.DataGridRowOwner;
				DataGrid dataGrid = (dataGridRowOwner != null) ? dataGridRowOwner.DataGridOwner : null;
				if (dataGrid == null)
				{
					return null;
				}
				return dataGrid.Columns;
			}
		}

		// Token: 0x1700166F RID: 5743
		// (get) Token: 0x06005CAE RID: 23726 RVA: 0x001A11A4 File Offset: 0x0019F3A4
		internal ContainerTracking<DataGridCell> CellTrackingRoot
		{
			get
			{
				return this._cellTrackingRoot;
			}
		}

		// Token: 0x04002FD4 RID: 12244
		private object _item;

		// Token: 0x04002FD5 RID: 12245
		private ContainerTracking<DataGridCell> _cellTrackingRoot;

		// Token: 0x04002FD6 RID: 12246
		private Panel _internalItemsHost;
	}
}
