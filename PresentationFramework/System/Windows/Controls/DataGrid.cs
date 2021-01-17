using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Data;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that displays data in a customizable grid.</summary>
	// Token: 0x02000492 RID: 1170
	public class DataGrid : MultiSelector
	{
		// Token: 0x060044E5 RID: 17637 RVA: 0x00138724 File Offset: 0x00136924
		static DataGrid()
		{
			Type typeFromHandle = typeof(DataGrid);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(typeof(DataGrid)));
			FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(DataGridRowsPresenter));
			frameworkElementFactory.SetValue(FrameworkElement.NameProperty, "PART_RowsPresenter");
			ItemsControl.ItemsPanelProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new ItemsPanelTemplate(frameworkElementFactory)));
			VirtualizingPanel.IsVirtualizingProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(true, null, new CoerceValueCallback(DataGrid.OnCoerceIsVirtualizingProperty)));
			VirtualizingPanel.VirtualizationModeProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(VirtualizationMode.Recycling));
			ItemsControl.ItemContainerStyleProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceItemContainerStyle)));
			ItemsControl.ItemContainerStyleSelectorProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceItemContainerStyleSelector)));
			ItemsControl.ItemsSourceProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceItemsSourceProperty)));
			ItemsControl.AlternationCountProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(0, null, new CoerceValueCallback(DataGrid.OnCoerceAlternationCount)));
			UIElement.IsEnabledProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGrid.OnIsEnabledChanged)));
			UIElement.IsKeyboardFocusWithinPropertyKey.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGrid.OnIsKeyboardFocusWithinChanged)));
			Selector.IsSynchronizedWithCurrentItemProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGrid.OnCoerceIsSynchronizedWithCurrentItem)));
			Control.IsTabStopProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(false));
			KeyboardNavigation.DirectionalNavigationProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(KeyboardNavigationMode.Contained));
			KeyboardNavigation.ControlTabNavigationProperty.OverrideMetadata(typeFromHandle, new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));
			CommandManager.RegisterClassInputBinding(typeFromHandle, new InputBinding(DataGrid.BeginEditCommand, new KeyGesture(Key.F2)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.BeginEditCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedBeginEdit), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteBeginEdit)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.CommitEditCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedCommitEdit), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteCommitEdit)));
			CommandManager.RegisterClassInputBinding(typeFromHandle, new InputBinding(DataGrid.CancelEditCommand, new KeyGesture(Key.Escape)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.CancelEditCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedCancelEdit), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteCancelEdit)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.SelectAllCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedSelectAll), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteSelectAll)));
			CommandManager.RegisterClassCommandBinding(typeFromHandle, new CommandBinding(DataGrid.DeleteCommand, new ExecutedRoutedEventHandler(DataGrid.OnExecutedDelete), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteDelete)));
			CommandManager.RegisterClassCommandBinding(typeof(DataGrid), new CommandBinding(ApplicationCommands.Copy, new ExecutedRoutedEventHandler(DataGrid.OnExecutedCopy), new CanExecuteRoutedEventHandler(DataGrid.OnCanExecuteCopy)));
			EventManager.RegisterClassHandler(typeof(DataGrid), UIElement.MouseUpEvent, new MouseButtonEventHandler(DataGrid.OnAnyMouseUpThunk), true);
			ControlsTraceLogger.AddControl(TelemetryControls.DataGrid);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.DataGrid" /> class. </summary>
		// Token: 0x060044E6 RID: 17638 RVA: 0x00139690 File Offset: 0x00137890
		public DataGrid()
		{
			this._columns = new DataGridColumnCollection(this);
			this._columns.CollectionChanged += this.OnColumnsChanged;
			this._rowValidationRules = new ObservableCollection<ValidationRule>();
			this._rowValidationRules.CollectionChanged += this.OnRowValidationRulesChanged;
			this._selectedCells = new SelectedCellsCollection(this);
			((INotifyCollectionChanged)base.Items).CollectionChanged += this.OnItemsCollectionChanged;
			((INotifyCollectionChanged)base.Items.SortDescriptions).CollectionChanged += this.OnItemsSortDescriptionsChanged;
			base.Items.GroupDescriptions.CollectionChanged += this.OnItemsGroupDescriptionsChanged;
			this.InternalColumns.InvalidateColumnWidthsComputation();
			this.CellsPanelHorizontalOffsetComputationPending = false;
		}

		/// <summary>Gets a collection that contains all the columns in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The collection of columns in the <see cref="T:System.Windows.Controls.DataGrid" />. </returns>
		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x060044E7 RID: 17639 RVA: 0x00139779 File Offset: 0x00137979
		public ObservableCollection<DataGridColumn> Columns
		{
			get
			{
				return this._columns;
			}
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x060044E8 RID: 17640 RVA: 0x00139779 File Offset: 0x00137979
		internal DataGridColumnCollection InternalColumns
		{
			get
			{
				return this._columns;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can adjust the width of columns by using the mouse.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can adjust the column width; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x060044E9 RID: 17641 RVA: 0x00139781 File Offset: 0x00137981
		// (set) Token: 0x060044EA RID: 17642 RVA: 0x00139793 File Offset: 0x00137993
		public bool CanUserResizeColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserResizeColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserResizeColumnsProperty, value);
			}
		}

		/// <summary>Gets or sets the standard width and sizing mode of columns and headers in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The width and sizing mode of the columns and headers, in device-independent units (1/96th inch per unit). The registered default is <see cref="P:System.Windows.Controls.DataGridLength.SizeToHeader" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x060044EB RID: 17643 RVA: 0x001397A1 File Offset: 0x001379A1
		// (set) Token: 0x060044EC RID: 17644 RVA: 0x001397B3 File Offset: 0x001379B3
		public DataGridLength ColumnWidth
		{
			get
			{
				return (DataGridLength)base.GetValue(DataGrid.ColumnWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.ColumnWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum width constraint of the columns and headers in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The minimum width of the columns and headers, in device-independent units (1/96th inch per unit). The registered default is 20. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x060044ED RID: 17645 RVA: 0x001397C6 File Offset: 0x001379C6
		// (set) Token: 0x060044EE RID: 17646 RVA: 0x001397D8 File Offset: 0x001379D8
		public double MinColumnWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.MinColumnWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.MinColumnWidthProperty, value);
			}
		}

		/// <summary>Gets or sets the maximum width constraint of the columns and headers in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The maximum width of the columns and headers in the <see cref="T:System.Windows.Controls.DataGrid" />, in device-independent units (1/96th inch per unit). The registered default is <see cref="F:System.Double.PositiveInfinity" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x060044EF RID: 17647 RVA: 0x001397EB File Offset: 0x001379EB
		// (set) Token: 0x060044F0 RID: 17648 RVA: 0x001397FD File Offset: 0x001379FD
		public double MaxColumnWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.MaxColumnWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.MaxColumnWidthProperty, value);
			}
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x00139810 File Offset: 0x00137A10
		private static void OnColumnSizeConstraintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x00139820 File Offset: 0x00137A20
		private static bool ValidateMinColumnWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num) && !double.IsPositiveInfinity(num);
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x00139854 File Offset: 0x00137A54
		private static bool ValidateMaxColumnWidth(object v)
		{
			double num = (double)v;
			return num >= 0.0 && !DoubleUtil.IsNaN(num);
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x00139880 File Offset: 0x00137A80
		private void OnColumnsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.UpdateDataGridReference(e.NewItems, false);
				DataGrid.UpdateColumnSizeConstraints(e.NewItems);
				break;
			case NotifyCollectionChangedAction.Remove:
				this.UpdateDataGridReference(e.OldItems, true);
				break;
			case NotifyCollectionChangedAction.Replace:
				this.UpdateDataGridReference(e.OldItems, true);
				this.UpdateDataGridReference(e.NewItems, false);
				DataGrid.UpdateColumnSizeConstraints(e.NewItems);
				break;
			case NotifyCollectionChangedAction.Reset:
				this._selectedCells.Clear();
				break;
			}
			if (this.InternalColumns.DisplayIndexMapInitialized)
			{
				base.CoerceValue(DataGrid.FrozenColumnCountProperty);
			}
			bool flag = DataGrid.HasVisibleColumns(e.OldItems);
			flag |= DataGrid.HasVisibleColumns(e.NewItems);
			flag |= (e.Action == NotifyCollectionChangedAction.Reset);
			if (flag)
			{
				this.InternalColumns.InvalidateColumnRealization(true);
			}
			this.UpdateColumnsOnRows(e);
			if (flag && e.Action != NotifyCollectionChangedAction.Move)
			{
				this.InternalColumns.InvalidateColumnWidthsComputation();
			}
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x00139978 File Offset: 0x00137B78
		internal void UpdateDataGridReference(IList list, bool clear)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumn dataGridColumn = (DataGridColumn)list[i];
				if (clear)
				{
					if (dataGridColumn.DataGridOwner == this)
					{
						dataGridColumn.DataGridOwner = null;
					}
				}
				else
				{
					if (dataGridColumn.DataGridOwner != null && dataGridColumn.DataGridOwner != this)
					{
						dataGridColumn.DataGridOwner.Columns.Remove(dataGridColumn);
					}
					dataGridColumn.DataGridOwner = this;
				}
			}
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x001399E4 File Offset: 0x00137BE4
		private static void UpdateColumnSizeConstraints(IList list)
		{
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				DataGridColumn dataGridColumn = (DataGridColumn)list[i];
				dataGridColumn.SyncProperties();
			}
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x00139A18 File Offset: 0x00137C18
		private static bool HasVisibleColumns(IList columns)
		{
			if (columns != null && columns.Count > 0)
			{
				foreach (object obj in columns)
				{
					DataGridColumn dataGridColumn = (DataGridColumn)obj;
					if (dataGridColumn.IsVisible)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x00139A80 File Offset: 0x00137C80
		internal bool RetryBringColumnIntoView(bool retryRequested)
		{
			if (retryRequested)
			{
				int value = DataGrid.BringColumnIntoViewRetryCountField.GetValue(this);
				if (value < 4)
				{
					DataGrid.BringColumnIntoViewRetryCountField.SetValue(this, value + 1);
					return true;
				}
			}
			DataGrid.BringColumnIntoViewRetryCountField.ClearValue(this);
			return false;
		}

		/// <summary>Gets the <see cref="T:System.Windows.Controls.DataGridColumn" /> at the specified index.</summary>
		/// <param name="displayIndex">The zero-based index of the column to retrieve. </param>
		/// <returns>The <see cref="T:System.Windows.Controls.DataGridColumn" /> at the specified <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" />.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="displayIndex" /> is less than 0, or greater than or equal to the number of columns.</exception>
		// Token: 0x060044F9 RID: 17657 RVA: 0x00139ABC File Offset: 0x00137CBC
		public DataGridColumn ColumnFromDisplayIndex(int displayIndex)
		{
			if (displayIndex < 0 || displayIndex >= this.Columns.Count)
			{
				throw new ArgumentOutOfRangeException("displayIndex", displayIndex, SR.Get("DataGrid_DisplayIndexOutOfRange"));
			}
			return this.InternalColumns.ColumnFromDisplayIndex(displayIndex);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.DataGridColumn.DisplayIndex" /> property on one of the columns changes.</summary>
		// Token: 0x140000AD RID: 173
		// (add) Token: 0x060044FA RID: 17658 RVA: 0x00139AF8 File Offset: 0x00137CF8
		// (remove) Token: 0x060044FB RID: 17659 RVA: 0x00139B30 File Offset: 0x00137D30
		public event EventHandler<DataGridColumnEventArgs> ColumnDisplayIndexChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.ColumnDisplayIndexChanged" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060044FC RID: 17660 RVA: 0x00139B65 File Offset: 0x00137D65
		protected internal virtual void OnColumnDisplayIndexChanged(DataGridColumnEventArgs e)
		{
			if (this.ColumnDisplayIndexChanged != null)
			{
				this.ColumnDisplayIndexChanged(this, e);
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x060044FD RID: 17661 RVA: 0x00139B7C File Offset: 0x00137D7C
		internal List<int> DisplayIndexMap
		{
			get
			{
				return this.InternalColumns.DisplayIndexMap;
			}
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x00139B89 File Offset: 0x00137D89
		internal void ValidateDisplayIndex(DataGridColumn column, int displayIndex)
		{
			this.InternalColumns.ValidateDisplayIndex(column, displayIndex);
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x00139B98 File Offset: 0x00137D98
		internal int ColumnIndexFromDisplayIndex(int displayIndex)
		{
			if (displayIndex >= 0 && displayIndex < this.DisplayIndexMap.Count)
			{
				return this.DisplayIndexMap[displayIndex];
			}
			return -1;
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x00139BBC File Offset: 0x00137DBC
		internal DataGridColumnHeader ColumnHeaderFromDisplayIndex(int displayIndex)
		{
			int num = this.ColumnIndexFromDisplayIndex(displayIndex);
			if (num != -1 && this.ColumnHeadersPresenter != null && this.ColumnHeadersPresenter.ItemContainerGenerator != null)
			{
				return (DataGridColumnHeader)this.ColumnHeadersPresenter.ItemContainerGenerator.ContainerFromIndex(num);
			}
			return null;
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00139C02 File Offset: 0x00137E02
		private static void OnNotifyCellsPresenterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter);
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x00139C12 File Offset: 0x00137E12
		private static void OnNotifyColumnAndCellPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.Columns);
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x00139810 File Offset: 0x00137A10
		private static void OnNotifyColumnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns);
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x00139C22 File Offset: 0x00137E22
		private static void OnNotifyColumnAndColumnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Columns | DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00139C33 File Offset: 0x00137E33
		private static void OnNotifyColumnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00139C44 File Offset: 0x00137E44
		private static void OnNotifyHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeaders | DataGridNotificationTarget.RowHeaders);
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00139C58 File Offset: 0x00137E58
		private static void OnNotifyDataGridAndRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.DataGrid | DataGridNotificationTarget.Rows);
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00139C6C File Offset: 0x00137E6C
		private static void OnNotifyGridLinePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.OldValue != e.NewValue)
			{
				((DataGrid)d).OnItemTemplateChanged(null, null);
			}
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00139C8B File Offset: 0x00137E8B
		private static void OnNotifyRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Rows);
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x00139C9F File Offset: 0x00137E9F
		private static void OnNotifyRowHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.RowHeaders);
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x00139CB3 File Offset: 0x00137EB3
		private static void OnNotifyRowAndRowHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x00139CC7 File Offset: 0x00137EC7
		private static void OnNotifyRowAndDetailsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.Rows);
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00139CDB File Offset: 0x00137EDB
		private static void OnNotifyHorizontalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		// Token: 0x0600450E RID: 17678 RVA: 0x00139CEC File Offset: 0x00137EEC
		internal void NotifyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			this.NotifyPropertyChanged(d, string.Empty, e, target);
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x00139CFC File Offset: 0x00137EFC
		internal void NotifyPropertyChanged(DependencyObject d, string propertyName, DependencyPropertyChangedEventArgs e, DataGridNotificationTarget target)
		{
			if (DataGridHelper.ShouldNotifyDataGrid(target))
			{
				if (e.Property == DataGrid.AlternatingRowBackgroundProperty)
				{
					base.CoerceValue(ItemsControl.AlternationCountProperty);
				}
				else if (e.Property == DataGridColumn.VisibilityProperty || e.Property == DataGridColumn.WidthProperty || e.Property == DataGridColumn.DisplayIndexProperty)
				{
					foreach (object obj in base.ItemContainerGenerator.RecyclableContainers)
					{
						DependencyObject dependencyObject = (DependencyObject)obj;
						DataGridRow dataGridRow = dependencyObject as DataGridRow;
						if (dataGridRow != null)
						{
							DataGridCellsPresenter cellsPresenter = dataGridRow.CellsPresenter;
							if (cellsPresenter != null)
							{
								cellsPresenter.InvalidateDataGridCellsPanelMeasureAndArrange();
							}
						}
					}
				}
			}
			if (DataGridHelper.ShouldNotifyRowSubtree(target))
			{
				for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
				{
					containerTracking.Container.NotifyPropertyChanged(d, propertyName, e, target);
				}
			}
			if (DataGridHelper.ShouldNotifyColumnCollection(target) || DataGridHelper.ShouldNotifyColumns(target))
			{
				this.InternalColumns.NotifyPropertyChanged(d, propertyName, e, target);
			}
			if ((DataGridHelper.ShouldNotifyColumnHeadersPresenter(target) || DataGridHelper.ShouldNotifyColumnHeaders(target)) && this.ColumnHeadersPresenter != null)
			{
				this.ColumnHeadersPresenter.NotifyPropertyChanged(d, propertyName, e, target);
			}
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x00139E44 File Offset: 0x00138044
		internal void UpdateColumnsOnVirtualizedCellInfoCollections(NotifyCollectionChangedAction action, int oldDisplayIndex, DataGridColumn oldColumn, int newDisplayIndex)
		{
			using (this.UpdateSelectedCells())
			{
				this._selectedCells.OnColumnsChanged(action, oldDisplayIndex, oldColumn, newDisplayIndex, base.SelectedItems);
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06004511 RID: 17681 RVA: 0x00139E8C File Offset: 0x0013808C
		// (set) Token: 0x06004512 RID: 17682 RVA: 0x00139E94 File Offset: 0x00138094
		internal DataGridColumnHeadersPresenter ColumnHeadersPresenter
		{
			get
			{
				return this._columnHeadersPresenter;
			}
			set
			{
				this._columnHeadersPresenter = value;
			}
		}

		/// <summary>Called whenever the template of the <see cref="T:System.Windows.Controls.DataGrid" /> changes.</summary>
		/// <param name="oldTemplate">The old template.</param>
		/// <param name="newTemplate">The new template.</param>
		// Token: 0x06004513 RID: 17683 RVA: 0x00139E9D File Offset: 0x0013809D
		protected override void OnTemplateChanged(ControlTemplate oldTemplate, ControlTemplate newTemplate)
		{
			base.OnTemplateChanged(oldTemplate, newTemplate);
			this.ColumnHeadersPresenter = null;
		}

		/// <summary>Gets or sets a value that indicates which grid lines are shown.</summary>
		/// <returns>One of the enumeration values that specifies which grid lines are shown in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see cref="F:System.Windows.Controls.DataGridGridLinesVisibility.All" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06004514 RID: 17684 RVA: 0x00139EAE File Offset: 0x001380AE
		// (set) Token: 0x06004515 RID: 17685 RVA: 0x00139EC0 File Offset: 0x001380C0
		public DataGridGridLinesVisibility GridLinesVisibility
		{
			get
			{
				return (DataGridGridLinesVisibility)base.GetValue(DataGrid.GridLinesVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.GridLinesVisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets the brush that is used to draw the horizontal grid lines.</summary>
		/// <returns>The brush that is used to draw the horizontal grid lines in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is a black <see cref="T:System.Windows.Media.SolidColorBrush" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06004516 RID: 17686 RVA: 0x00139ED3 File Offset: 0x001380D3
		// (set) Token: 0x06004517 RID: 17687 RVA: 0x00139EE5 File Offset: 0x001380E5
		public Brush HorizontalGridLinesBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.HorizontalGridLinesBrushProperty);
			}
			set
			{
				base.SetValue(DataGrid.HorizontalGridLinesBrushProperty, value);
			}
		}

		/// <summary>Gets or sets the brush that is used to draw the vertical grid lines.</summary>
		/// <returns>The brush that is used to draw the vertical grid lines in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is a black <see cref="T:System.Windows.Media.SolidColorBrush" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06004518 RID: 17688 RVA: 0x00139EF3 File Offset: 0x001380F3
		// (set) Token: 0x06004519 RID: 17689 RVA: 0x00139F05 File Offset: 0x00138105
		public Brush VerticalGridLinesBrush
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.VerticalGridLinesBrushProperty);
			}
			set
			{
				base.SetValue(DataGrid.VerticalGridLinesBrushProperty, value);
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x0600451A RID: 17690 RVA: 0x00139F13 File Offset: 0x00138113
		internal double HorizontalGridLineThickness
		{
			get
			{
				return 1.0;
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x0600451B RID: 17691 RVA: 0x00139F13 File Offset: 0x00138113
		internal double VerticalGridLineThickness
		{
			get
			{
				return 1.0;
			}
		}

		/// <summary>Determines if an item is a <see cref="T:System.Windows.Controls.DataGridRow" />.</summary>
		/// <param name="item">The item to test.</param>
		/// <returns>
		///     <see langword="true" /> if the item is a <see cref="T:System.Windows.Controls.DataGridRow" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600451C RID: 17692 RVA: 0x00139F1E File Offset: 0x0013811E
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is DataGridRow;
		}

		/// <summary>Instantiates a new <see cref="T:System.Windows.Controls.DataGridRow" />.</summary>
		/// <returns>The row that is the container.</returns>
		// Token: 0x0600451D RID: 17693 RVA: 0x00139F29 File Offset: 0x00138129
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new DataGridRow();
		}

		/// <summary>Prepares a new row for the specified item.</summary>
		/// <param name="element">The new <see cref="T:System.Windows.Controls.DataGridRow" />.</param>
		/// <param name="item">The data item that the row contains.</param>
		// Token: 0x0600451E RID: 17694 RVA: 0x00139F30 File Offset: 0x00138130
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			DataGridRow dataGridRow = (DataGridRow)element;
			if (dataGridRow.DataGridOwner != this)
			{
				dataGridRow.Tracker.StartTracking(ref this._rowTrackingRoot);
				if (item == CollectionView.NewItemPlaceholder || (this.IsAddingNewItem && item == this.EditableItems.CurrentAddItem))
				{
					dataGridRow.IsNewItem = true;
				}
				else
				{
					dataGridRow.ClearValue(DataGridRow.IsNewItemPropertyKey);
				}
				this.EnsureInternalScrollControls();
				this.EnqueueNewItemMarginComputation();
			}
			dataGridRow.PrepareRow(item, this);
			this.OnLoadingRow(new DataGridRowEventArgs(dataGridRow));
		}

		/// <summary>Unloads the row for the specified item.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Controls.DataGridRow" /> to clear.</param>
		/// <param name="item">The data item that the row contains.</param>
		// Token: 0x0600451F RID: 17695 RVA: 0x00139FB8 File Offset: 0x001381B8
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
			DataGridRow dataGridRow = (DataGridRow)element;
			if (dataGridRow.DataGridOwner == this)
			{
				dataGridRow.Tracker.StopTracking(ref this._rowTrackingRoot);
				dataGridRow.ClearValue(DataGridRow.IsNewItemPropertyKey);
				this.EnqueueNewItemMarginComputation();
			}
			this.OnUnloadingRow(new DataGridRowEventArgs(dataGridRow));
			dataGridRow.ClearRow(this);
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x0013A014 File Offset: 0x00138214
		private void UpdateColumnsOnRows(NotifyCollectionChangedEventArgs e)
		{
			for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
			{
				containerTracking.Container.OnColumnsChanged(this._columns, e);
			}
		}

		/// <summary>Gets or sets the style applied to all rows.</summary>
		/// <returns>The style applied to all rows in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x06004521 RID: 17697 RVA: 0x0013A046 File Offset: 0x00138246
		// (set) Token: 0x06004522 RID: 17698 RVA: 0x0013A058 File Offset: 0x00138258
		public Style RowStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.RowStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowStyleProperty, value);
			}
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x0013A066 File Offset: 0x00138266
		private static void OnRowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(ItemsControl.ItemContainerStyleProperty);
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x0013A073 File Offset: 0x00138273
		private static object OnCoerceItemContainerStyle(DependencyObject d, object baseValue)
		{
			if (!DataGridHelper.IsDefaultValue(d, DataGrid.RowStyleProperty))
			{
				return d.GetValue(DataGrid.RowStyleProperty);
			}
			return baseValue;
		}

		/// <summary>Gets or sets the template that is used to visually indicate an error in row validation.</summary>
		/// <returns>The template that is used to visually indicate an error in row validation. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x06004525 RID: 17701 RVA: 0x0013A08F File Offset: 0x0013828F
		// (set) Token: 0x06004526 RID: 17702 RVA: 0x0013A0A1 File Offset: 0x001382A1
		public ControlTemplate RowValidationErrorTemplate
		{
			get
			{
				return (ControlTemplate)base.GetValue(DataGrid.RowValidationErrorTemplateProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowValidationErrorTemplateProperty, value);
			}
		}

		/// <summary>Gets the rules that are used to validate the data in each row.</summary>
		/// <returns>The rules that are used to validate the data in each row. </returns>
		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06004527 RID: 17703 RVA: 0x0013A0AF File Offset: 0x001382AF
		public ObservableCollection<ValidationRule> RowValidationRules
		{
			get
			{
				return this._rowValidationRules;
			}
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x0013A0B8 File Offset: 0x001382B8
		private void OnRowValidationRulesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.EnsureItemBindingGroup();
			if (this._defaultBindingGroup != null)
			{
				if (base.ItemBindingGroup == this._defaultBindingGroup)
				{
					switch (e.Action)
					{
					case NotifyCollectionChangedAction.Add:
						using (IEnumerator enumerator = e.NewItems.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								ValidationRule item = (ValidationRule)obj;
								this._defaultBindingGroup.ValidationRules.Add(item);
							}
							return;
						}
						break;
					case NotifyCollectionChangedAction.Remove:
						break;
					case NotifyCollectionChangedAction.Replace:
						goto IL_DD;
					case NotifyCollectionChangedAction.Move:
						return;
					case NotifyCollectionChangedAction.Reset:
						goto IL_176;
					default:
						return;
					}
					using (IEnumerator enumerator2 = e.OldItems.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							ValidationRule item2 = (ValidationRule)obj2;
							this._defaultBindingGroup.ValidationRules.Remove(item2);
						}
						return;
					}
					IL_DD:
					foreach (object obj3 in e.OldItems)
					{
						ValidationRule item3 = (ValidationRule)obj3;
						this._defaultBindingGroup.ValidationRules.Remove(item3);
					}
					using (IEnumerator enumerator4 = e.NewItems.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							object obj4 = enumerator4.Current;
							ValidationRule item4 = (ValidationRule)obj4;
							this._defaultBindingGroup.ValidationRules.Add(item4);
						}
						return;
					}
					IL_176:
					this._defaultBindingGroup.ValidationRules.Clear();
					return;
				}
				this._defaultBindingGroup = null;
			}
		}

		/// <summary>Gets or sets the style selector for the rows.</summary>
		/// <returns>The style selector for the rows. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06004529 RID: 17705 RVA: 0x0013A288 File Offset: 0x00138488
		// (set) Token: 0x0600452A RID: 17706 RVA: 0x0013A29A File Offset: 0x0013849A
		public StyleSelector RowStyleSelector
		{
			get
			{
				return (StyleSelector)base.GetValue(DataGrid.RowStyleSelectorProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowStyleSelectorProperty, value);
			}
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x0013A2A8 File Offset: 0x001384A8
		private static void OnRowStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(ItemsControl.ItemContainerStyleSelectorProperty);
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x0013A2B5 File Offset: 0x001384B5
		private static object OnCoerceItemContainerStyleSelector(DependencyObject d, object baseValue)
		{
			if (!DataGridHelper.IsDefaultValue(d, DataGrid.RowStyleSelectorProperty))
			{
				return d.GetValue(DataGrid.RowStyleSelectorProperty);
			}
			return baseValue;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x0013A2D4 File Offset: 0x001384D4
		private static object OnCoerceIsSynchronizedWithCurrentItem(DependencyObject d, object baseValue)
		{
			DataGrid dataGrid = (DataGrid)d;
			if (dataGrid.SelectionUnit == DataGridSelectionUnit.Cell)
			{
				return false;
			}
			return baseValue;
		}

		/// <summary>Gets or sets the default brush for the row background.</summary>
		/// <returns>The brush that paints the background of a row. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x0600452E RID: 17710 RVA: 0x0013A2F8 File Offset: 0x001384F8
		// (set) Token: 0x0600452F RID: 17711 RVA: 0x0013A30A File Offset: 0x0013850A
		public Brush RowBackground
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.RowBackgroundProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowBackgroundProperty, value);
			}
		}

		/// <summary>Gets or sets the background brush for use on alternating rows.</summary>
		/// <returns>The <see cref="T:System.Windows.Media.Brush" /> that paints the background of every nth row where n is defined by the <see cref="P:System.Windows.Controls.ItemsControl.AlternationCount" /> property. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06004530 RID: 17712 RVA: 0x0013A318 File Offset: 0x00138518
		// (set) Token: 0x06004531 RID: 17713 RVA: 0x0013A32A File Offset: 0x0013852A
		public Brush AlternatingRowBackground
		{
			get
			{
				return (Brush)base.GetValue(DataGrid.AlternatingRowBackgroundProperty);
			}
			set
			{
				base.SetValue(DataGrid.AlternatingRowBackgroundProperty, value);
			}
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x0013A338 File Offset: 0x00138538
		private static object OnCoerceAlternationCount(DependencyObject d, object baseValue)
		{
			if ((int)baseValue < 2)
			{
				DataGrid dataGrid = (DataGrid)d;
				if (dataGrid.AlternatingRowBackground != null)
				{
					return 2;
				}
			}
			return baseValue;
		}

		/// <summary>Gets or sets the suggested height for all rows.</summary>
		/// <returns>The height of the rows, in device-independent units (1/96th inch per unit). The registered default is <see cref="F:System.Double.NaN" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x06004533 RID: 17715 RVA: 0x0013A365 File Offset: 0x00138565
		// (set) Token: 0x06004534 RID: 17716 RVA: 0x0013A377 File Offset: 0x00138577
		public double RowHeight
		{
			get
			{
				return (double)base.GetValue(DataGrid.RowHeightProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the minimum height constraint of the rows and headers in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The minimum height constraint of rows, in device-independent units (1/96th inch per unit). The registered default is 0.0. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06004535 RID: 17717 RVA: 0x0013A38A File Offset: 0x0013858A
		// (set) Token: 0x06004536 RID: 17718 RVA: 0x0013A39C File Offset: 0x0013859C
		public double MinRowHeight
		{
			get
			{
				return (double)base.GetValue(DataGrid.MinRowHeightProperty);
			}
			set
			{
				base.SetValue(DataGrid.MinRowHeightProperty, value);
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06004537 RID: 17719 RVA: 0x0013A3AF File Offset: 0x001385AF
		internal Visibility PlaceholderVisibility
		{
			get
			{
				return this._placeholderVisibility;
			}
		}

		/// <summary>Occurs after a <see cref="T:System.Windows.Controls.DataGridRow" /> is instantiated, so that you can customize it before it is used.</summary>
		// Token: 0x140000AE RID: 174
		// (add) Token: 0x06004538 RID: 17720 RVA: 0x0013A3B8 File Offset: 0x001385B8
		// (remove) Token: 0x06004539 RID: 17721 RVA: 0x0013A3F0 File Offset: 0x001385F0
		public event EventHandler<DataGridRowEventArgs> LoadingRow;

		/// <summary>Occurs when a <see cref="T:System.Windows.Controls.DataGridRow" /> object becomes available for reuse.</summary>
		// Token: 0x140000AF RID: 175
		// (add) Token: 0x0600453A RID: 17722 RVA: 0x0013A428 File Offset: 0x00138628
		// (remove) Token: 0x0600453B RID: 17723 RVA: 0x0013A460 File Offset: 0x00138660
		public event EventHandler<DataGridRowEventArgs> UnloadingRow;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.LoadingRow" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600453C RID: 17724 RVA: 0x0013A498 File Offset: 0x00138698
		protected virtual void OnLoadingRow(DataGridRowEventArgs e)
		{
			if (this.LoadingRow != null)
			{
				this.LoadingRow(this, e);
			}
			DataGridRow row = e.Row;
			if (row.DetailsVisibility == Visibility.Visible && row.DetailsPresenter != null)
			{
				Dispatcher.CurrentDispatcher.BeginInvoke(new DispatcherOperationCallback(DataGrid.DelayedOnLoadingRowDetails), DispatcherPriority.Loaded, new object[]
				{
					row
				});
			}
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x0013A4F4 File Offset: 0x001386F4
		internal static object DelayedOnLoadingRowDetails(object arg)
		{
			DataGridRow dataGridRow = (DataGridRow)arg;
			DataGrid dataGridOwner = dataGridRow.DataGridOwner;
			if (dataGridOwner != null)
			{
				dataGridOwner.OnLoadingRowDetailsWrapper(dataGridRow);
			}
			return null;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.UnloadingRow" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600453E RID: 17726 RVA: 0x0013A51C File Offset: 0x0013871C
		protected virtual void OnUnloadingRow(DataGridRowEventArgs e)
		{
			if (this.UnloadingRow != null)
			{
				this.UnloadingRow(this, e);
			}
			DataGridRow row = e.Row;
			this.OnUnloadingRowDetailsWrapper(row);
		}

		/// <summary>Gets or sets the width of the row header column.</summary>
		/// <returns>The width of the row header column, in device-independent units (1/96th inch per unit). The registered default is <see cref="F:System.Double.NaN" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x0600453F RID: 17727 RVA: 0x0013A54C File Offset: 0x0013874C
		// (set) Token: 0x06004540 RID: 17728 RVA: 0x0013A55E File Offset: 0x0013875E
		public double RowHeaderWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.RowHeaderWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderWidthProperty, value);
			}
		}

		/// <summary>Gets the rendered width of the row headers column.</summary>
		/// <returns>The rendered width of the row header, in device-independent units (1/96th inch per unit). The registered default is 0.0. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x06004541 RID: 17729 RVA: 0x0013A571 File Offset: 0x00138771
		// (set) Token: 0x06004542 RID: 17730 RVA: 0x0013A583 File Offset: 0x00138783
		public double RowHeaderActualWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.RowHeaderActualWidthProperty);
			}
			internal set
			{
				base.SetValue(DataGrid.RowHeaderActualWidthPropertyKey, value);
			}
		}

		/// <summary>Gets or sets the height of the column headers row.</summary>
		/// <returns>The height of the column headers row, in device-independent units (1/96th inch per unit). The registered default is <see cref="F:System.Double.NaN" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x06004543 RID: 17731 RVA: 0x0013A596 File Offset: 0x00138796
		// (set) Token: 0x06004544 RID: 17732 RVA: 0x0013A5A8 File Offset: 0x001387A8
		public double ColumnHeaderHeight
		{
			get
			{
				return (double)base.GetValue(DataGrid.ColumnHeaderHeightProperty);
			}
			set
			{
				base.SetValue(DataGrid.ColumnHeaderHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the value that specifies the visibility of the row and column headers.</summary>
		/// <returns>One of the enumeration values that indicates the visibility of row and column headers. The registered default is <see cref="F:System.Windows.Controls.DataGridHeadersVisibility.All" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06004545 RID: 17733 RVA: 0x0013A5BB File Offset: 0x001387BB
		// (set) Token: 0x06004546 RID: 17734 RVA: 0x0013A5CD File Offset: 0x001387CD
		public DataGridHeadersVisibility HeadersVisibility
		{
			get
			{
				return (DataGridHeadersVisibility)base.GetValue(DataGrid.HeadersVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.HeadersVisibilityProperty, value);
			}
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x0013A5E0 File Offset: 0x001387E0
		private static void OnNotifyRowHeaderWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			double num = (double)e.NewValue;
			if (!DoubleUtil.IsNaN(num))
			{
				dataGrid.RowHeaderActualWidth = num;
			}
			else
			{
				dataGrid.RowHeaderActualWidth = 0.0;
			}
			DataGrid.OnNotifyRowHeaderPropertyChanged(d, e);
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x0013A628 File Offset: 0x00138828
		private void ResetRowHeaderActualWidth()
		{
			if (DoubleUtil.IsNaN(this.RowHeaderWidth))
			{
				this.RowHeaderActualWidth = 0.0;
			}
		}

		/// <summary>Sets the value of the <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> property for the <see cref="T:System.Windows.Controls.DataGridRow" /> that contains the specified object.</summary>
		/// <param name="item">The object in the row for which <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> is being set.</param>
		/// <param name="detailsVisibility">The <see cref="T:System.Windows.Visibility" /> to set for the row that contains the item.</param>
		// Token: 0x06004549 RID: 17737 RVA: 0x0013A648 File Offset: 0x00138848
		public void SetDetailsVisibilityForItem(object item, Visibility detailsVisibility)
		{
			this._itemAttachedStorage.SetValue(item, DataGridRow.DetailsVisibilityProperty, detailsVisibility);
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			if (dataGridRow != null)
			{
				dataGridRow.DetailsVisibility = detailsVisibility;
			}
		}

		/// <summary>Gets the <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> property for the <see cref="T:System.Windows.Controls.DataGridRow" /> that represents the specified data item.</summary>
		/// <param name="item">The data item in the row for which <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> is returned.</param>
		/// <returns>The visibility for the row that contains the <paramref name="item" />.</returns>
		// Token: 0x0600454A RID: 17738 RVA: 0x0013A688 File Offset: 0x00138888
		public Visibility GetDetailsVisibilityForItem(object item)
		{
			object obj;
			if (this._itemAttachedStorage.TryGetValue(item, DataGridRow.DetailsVisibilityProperty, out obj))
			{
				return (Visibility)obj;
			}
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			if (dataGridRow != null)
			{
				return dataGridRow.DetailsVisibility;
			}
			DataGridRowDetailsVisibilityMode rowDetailsVisibilityMode = this.RowDetailsVisibilityMode;
			if (rowDetailsVisibilityMode == DataGridRowDetailsVisibilityMode.Visible)
			{
				return Visibility.Visible;
			}
			if (rowDetailsVisibilityMode != DataGridRowDetailsVisibilityMode.VisibleWhenSelected)
			{
				return Visibility.Collapsed;
			}
			if (!base.SelectedItems.Contains(item))
			{
				return Visibility.Collapsed;
			}
			return Visibility.Visible;
		}

		/// <summary>Clears the <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> property for the <see cref="T:System.Windows.Controls.DataGridRow" /> that represents the specified data item.</summary>
		/// <param name="item">The data item in the row for which <see cref="P:System.Windows.Controls.DataGridRow.DetailsVisibility" /> is cleared.</param>
		// Token: 0x0600454B RID: 17739 RVA: 0x0013A6F4 File Offset: 0x001388F4
		public void ClearDetailsVisibilityForItem(object item)
		{
			this._itemAttachedStorage.ClearValue(item, DataGridRow.DetailsVisibilityProperty);
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			if (dataGridRow != null)
			{
				dataGridRow.ClearValue(DataGridRow.DetailsVisibilityProperty);
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x0600454C RID: 17740 RVA: 0x0013A732 File Offset: 0x00138932
		internal DataGridItemAttachedStorage ItemAttachedStorage
		{
			get
			{
				return this._itemAttachedStorage;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x0600454D RID: 17741 RVA: 0x0013A73C File Offset: 0x0013893C
		private bool ShouldSelectRowHeader
		{
			get
			{
				return this._selectionAnchor != null && base.SelectedItems.Contains(this._selectionAnchor.Value.Item) && this.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
			}
		}

		/// <summary>Gets or sets the style applied to all cells in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The style applied to the cells in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x0600454E RID: 17742 RVA: 0x0013A78B File Offset: 0x0013898B
		// (set) Token: 0x0600454F RID: 17743 RVA: 0x0013A79D File Offset: 0x0013899D
		public Style CellStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.CellStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.CellStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the style applied to all column headers in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The style applied to all column headers in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06004550 RID: 17744 RVA: 0x0013A7AB File Offset: 0x001389AB
		// (set) Token: 0x06004551 RID: 17745 RVA: 0x0013A7BD File Offset: 0x001389BD
		public Style ColumnHeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.ColumnHeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.ColumnHeaderStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the style applied to all row headers.</summary>
		/// <returns>The style applied to all row headers in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x06004552 RID: 17746 RVA: 0x0013A7CB File Offset: 0x001389CB
		// (set) Token: 0x06004553 RID: 17747 RVA: 0x0013A7DD File Offset: 0x001389DD
		public Style RowHeaderStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.RowHeaderStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderStyleProperty, value);
			}
		}

		/// <summary>Gets or set the template for the row headers.</summary>
		/// <returns>The template for the row headers. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x06004554 RID: 17748 RVA: 0x0013A7EB File Offset: 0x001389EB
		// (set) Token: 0x06004555 RID: 17749 RVA: 0x0013A7FD File Offset: 0x001389FD
		public DataTemplate RowHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGrid.RowHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the template selector for row headers.</summary>
		/// <returns>The template selector for row headers. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x06004556 RID: 17750 RVA: 0x0013A80B File Offset: 0x00138A0B
		// (set) Token: 0x06004557 RID: 17751 RVA: 0x0013A81D File Offset: 0x00138A1D
		public DataTemplateSelector RowHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGrid.RowHeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowHeaderTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets the key that references the default border brush for a focused cell.</summary>
		/// <returns>The key that references the default brush for a focused cell. </returns>
		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x0013A82B File Offset: 0x00138A2B
		public static ComponentResourceKey FocusBorderBrushKey
		{
			get
			{
				return SystemResourceKey.DataGridFocusBorderBrushKey;
			}
		}

		/// <summary>Gets the converter that converts a <see cref="T:System.Windows.Controls.DataGridHeadersVisibility" /> to a <see cref="T:System.Windows.Visibility" />.</summary>
		/// <returns>The converter that converts a <see cref="T:System.Windows.Controls.DataGridHeadersVisibility" /> to a <see cref="T:System.Windows.Visibility" />.</returns>
		// Token: 0x1700110E RID: 4366
		// (get) Token: 0x06004559 RID: 17753 RVA: 0x0013A832 File Offset: 0x00138A32
		public static IValueConverter HeadersVisibilityConverter
		{
			get
			{
				if (DataGrid._headersVisibilityConverter == null)
				{
					DataGrid._headersVisibilityConverter = new DataGridHeadersVisibilityToVisibilityConverter();
				}
				return DataGrid._headersVisibilityConverter;
			}
		}

		/// <summary>Gets the converter that converts a Boolean value to a <see cref="T:System.Windows.Controls.SelectiveScrollingOrientation" />.</summary>
		/// <returns>The converter that converts a Boolean value to a <see cref="T:System.Windows.Controls.SelectiveScrollingOrientation" />.</returns>
		// Token: 0x1700110F RID: 4367
		// (get) Token: 0x0600455A RID: 17754 RVA: 0x0013A84A File Offset: 0x00138A4A
		public static IValueConverter RowDetailsScrollingConverter
		{
			get
			{
				if (DataGrid._rowDetailsScrollingConverter == null)
				{
					DataGrid._rowDetailsScrollingConverter = new BooleanToSelectiveScrollingOrientationConverter();
				}
				return DataGrid._rowDetailsScrollingConverter;
			}
		}

		/// <summary>Gets or sets a value that indicates how horizontal scroll bars are displayed in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>One of the enumeration values that specifies the visibility of horizontal scroll bars in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Auto" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001110 RID: 4368
		// (get) Token: 0x0600455B RID: 17755 RVA: 0x0013A862 File Offset: 0x00138A62
		// (set) Token: 0x0600455C RID: 17756 RVA: 0x0013A874 File Offset: 0x00138A74
		public ScrollBarVisibility HorizontalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(DataGrid.HorizontalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.HorizontalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates how vertical scroll bars are displayed in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>One of the enumeration values that specifies the visibility of vertical scroll bars in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see cref="F:System.Windows.Controls.ScrollBarVisibility.Auto" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001111 RID: 4369
		// (get) Token: 0x0600455D RID: 17757 RVA: 0x0013A887 File Offset: 0x00138A87
		// (set) Token: 0x0600455E RID: 17758 RVA: 0x0013A899 File Offset: 0x00138A99
		public ScrollBarVisibility VerticalScrollBarVisibility
		{
			get
			{
				return (ScrollBarVisibility)base.GetValue(DataGrid.VerticalScrollBarVisibilityProperty);
			}
			set
			{
				base.SetValue(DataGrid.VerticalScrollBarVisibilityProperty, value);
			}
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.DataGrid" /> vertically to display the row for the specified data item.</summary>
		/// <param name="item">The data item to bring into view.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="item" /> is <see langword="null" />.</exception>
		// Token: 0x0600455F RID: 17759 RVA: 0x0013A8AC File Offset: 0x00138AAC
		public void ScrollIntoView(object item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ScrollIntoView(base.NewItemInfo(item, null, -1));
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x0013A8CB File Offset: 0x00138ACB
		internal void ScrollIntoView(ItemsControl.ItemInfo info)
		{
			if (base.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
			{
				base.OnBringItemIntoView(info);
				return;
			}
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(base.OnBringItemIntoView), info);
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.DataGrid" /> vertically and horizontally to display a cell for the specified data item and column. </summary>
		/// <param name="item">The data item to bring into view.</param>
		/// <param name="column">The column to bring into view.</param>
		// Token: 0x06004561 RID: 17761 RVA: 0x0013A900 File Offset: 0x00138B00
		public void ScrollIntoView(object item, DataGridColumn column)
		{
			ItemsControl.ItemInfo info = (item == null) ? null : base.NewItemInfo(item, null, -1);
			this.ScrollIntoView(info, column);
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x0013A928 File Offset: 0x00138B28
		private void ScrollIntoView(ItemsControl.ItemInfo info, DataGridColumn column)
		{
			if (column == null)
			{
				this.ScrollIntoView(info);
				return;
			}
			if (!column.IsVisible)
			{
				return;
			}
			if (base.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
			{
				base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.OnScrollIntoView), new object[]
				{
					info,
					column
				});
				return;
			}
			if (info == null)
			{
				this.ScrollColumnIntoView(column);
				return;
			}
			this.ScrollCellIntoView(info, column);
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x0013A998 File Offset: 0x00138B98
		private object OnScrollIntoView(object arg)
		{
			object[] array = arg as object[];
			if (array != null)
			{
				if (array[0] != null)
				{
					this.ScrollCellIntoView((ItemsControl.ItemInfo)array[0], (DataGridColumn)array[1]);
				}
				else
				{
					this.ScrollColumnIntoView((DataGridColumn)array[1]);
				}
			}
			else
			{
				base.OnBringItemIntoView((ItemsControl.ItemInfo)arg);
			}
			return null;
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x0013A9EC File Offset: 0x00138BEC
		private void ScrollColumnIntoView(DataGridColumn column)
		{
			if (this._rowTrackingRoot != null)
			{
				DataGridRow container = this._rowTrackingRoot.Container;
				if (container != null)
				{
					int index = this._columns.IndexOf(column);
					container.ScrollCellIntoView(index);
				}
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x0013AA24 File Offset: 0x00138C24
		private void ScrollCellIntoView(ItemsControl.ItemInfo info, DataGridColumn column)
		{
			if (!column.IsVisible)
			{
				return;
			}
			DataGridRow dataGridRow = base.ContainerFromItemInfo(info) as DataGridRow;
			if (dataGridRow == null)
			{
				base.OnBringItemIntoView(info);
				base.UpdateLayout();
				dataGridRow = (base.ContainerFromItemInfo(info) as DataGridRow);
			}
			else
			{
				dataGridRow.BringIntoView();
				base.UpdateLayout();
			}
			if (dataGridRow != null)
			{
				int index = this._columns.IndexOf(column);
				dataGridRow.ScrollCellIntoView(index);
			}
		}

		/// <summary>Called when the <see cref="P:System.Windows.UIElement.IsMouseCaptured" /> property changes on this element.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004566 RID: 17766 RVA: 0x0013AA8A File Offset: 0x00138C8A
		protected override void OnIsMouseCapturedChanged(DependencyPropertyChangedEventArgs e)
		{
			if (!base.IsMouseCaptured)
			{
				this.StopAutoScroll();
			}
			base.OnIsMouseCapturedChanged(e);
		}

		// Token: 0x06004567 RID: 17767 RVA: 0x0013AAA4 File Offset: 0x00138CA4
		private void StartAutoScroll()
		{
			if (this._autoScrollTimer == null)
			{
				this._hasAutoScrolled = false;
				this._autoScrollTimer = new DispatcherTimer(DispatcherPriority.SystemIdle);
				this._autoScrollTimer.Interval = ItemsControl.AutoScrollTimeout;
				this._autoScrollTimer.Tick += this.OnAutoScrollTimeout;
				this._autoScrollTimer.Start();
			}
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x0013AAFE File Offset: 0x00138CFE
		private void StopAutoScroll()
		{
			if (this._autoScrollTimer != null)
			{
				this._autoScrollTimer.Stop();
				this._autoScrollTimer = null;
				this._hasAutoScrolled = false;
			}
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x0013AB21 File Offset: 0x00138D21
		private void OnAutoScrollTimeout(object sender, EventArgs e)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				this.DoAutoScroll();
				return;
			}
			this.StopAutoScroll();
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x0013AB3C File Offset: 0x00138D3C
		private new bool DoAutoScroll()
		{
			DataGrid.RelativeMousePositions relativeMousePosition = this.RelativeMousePosition;
			if (relativeMousePosition != DataGrid.RelativeMousePositions.Over)
			{
				DataGridCell dataGridCell = this.GetCellNearMouse();
				if (dataGridCell != null)
				{
					DataGridColumn dataGridColumn = dataGridCell.Column;
					ItemsControl.ItemInfo itemInfo = base.ItemInfoFromContainer(dataGridCell.RowOwner);
					if (DataGrid.IsMouseToLeft(relativeMousePosition))
					{
						int displayIndex = dataGridColumn.DisplayIndex;
						if (displayIndex > 0)
						{
							dataGridColumn = this.ColumnFromDisplayIndex(displayIndex - 1);
						}
					}
					else if (DataGrid.IsMouseToRight(relativeMousePosition))
					{
						int displayIndex2 = dataGridColumn.DisplayIndex;
						if (displayIndex2 < this._columns.Count - 1)
						{
							dataGridColumn = this.ColumnFromDisplayIndex(displayIndex2 + 1);
						}
					}
					if (DataGrid.IsMouseAbove(relativeMousePosition))
					{
						int index = itemInfo.Index;
						if (index > 0)
						{
							itemInfo = base.ItemInfoFromIndex(index - 1);
						}
					}
					else if (DataGrid.IsMouseBelow(relativeMousePosition))
					{
						int index2 = itemInfo.Index;
						if (index2 < base.Items.Count - 1)
						{
							itemInfo = base.ItemInfoFromIndex(index2 + 1);
						}
					}
					if (this._isRowDragging)
					{
						base.OnBringItemIntoView(itemInfo);
						DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromIndex(itemInfo.Index);
						if (dataGridRow != null)
						{
							this._hasAutoScrolled = true;
							this.HandleSelectionForRowHeaderAndDetailsInput(dataGridRow, false);
							this.SetCurrentItem(itemInfo.Item);
							return true;
						}
					}
					else
					{
						this.ScrollCellIntoView(itemInfo, dataGridColumn);
						dataGridCell = this.TryFindCell(itemInfo, dataGridColumn);
						if (dataGridCell != null)
						{
							this._hasAutoScrolled = true;
							this.HandleSelectionForCellInput(dataGridCell, false, true, true);
							dataGridCell.Focus();
							return true;
						}
					}
				}
			}
			return false;
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Controls.DataGrid" /> supports custom keyboard scrolling.</summary>
		/// <returns>
		///     <see langword="true" />
		///      in all cases.</returns>
		// Token: 0x17001112 RID: 4370
		// (get) Token: 0x0600456B RID: 17771 RVA: 0x00016748 File Offset: 0x00014948
		protected internal override bool HandlesScrolling
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x0600456C RID: 17772 RVA: 0x0013AC8C File Offset: 0x00138E8C
		// (set) Token: 0x0600456D RID: 17773 RVA: 0x0013AC94 File Offset: 0x00138E94
		internal Panel InternalItemsHost
		{
			get
			{
				return this._internalItemsHost;
			}
			set
			{
				if (this._internalItemsHost != value)
				{
					this._internalItemsHost = value;
					if (this._internalItemsHost != null)
					{
						this.DetermineItemsHostStarBehavior();
						this.EnsureInternalScrollControls();
					}
				}
			}
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x0013ACBA File Offset: 0x00138EBA
		internal ScrollViewer InternalScrollHost
		{
			get
			{
				this.EnsureInternalScrollControls();
				return this._internalScrollHost;
			}
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x0600456F RID: 17775 RVA: 0x0013ACC8 File Offset: 0x00138EC8
		internal ScrollContentPresenter InternalScrollContentPresenter
		{
			get
			{
				this.EnsureInternalScrollControls();
				return this._internalScrollContentPresenter;
			}
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x0013ACD8 File Offset: 0x00138ED8
		private void DetermineItemsHostStarBehavior()
		{
			VirtualizingStackPanel virtualizingStackPanel = this._internalItemsHost as VirtualizingStackPanel;
			if (virtualizingStackPanel != null)
			{
				virtualizingStackPanel.IgnoreMaxDesiredSize = this.InternalColumns.HasVisibleStarColumns;
			}
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x0013AD08 File Offset: 0x00138F08
		private void EnsureInternalScrollControls()
		{
			if (this._internalScrollContentPresenter == null)
			{
				if (this._internalItemsHost != null)
				{
					this._internalScrollContentPresenter = DataGridHelper.FindVisualParent<ScrollContentPresenter>(this._internalItemsHost);
				}
				else if (this._rowTrackingRoot != null)
				{
					DataGridRow container = this._rowTrackingRoot.Container;
					this._internalScrollContentPresenter = DataGridHelper.FindVisualParent<ScrollContentPresenter>(container);
				}
				if (this._internalScrollContentPresenter != null)
				{
					this._internalScrollContentPresenter.SizeChanged += this.OnInternalScrollContentPresenterSizeChanged;
				}
			}
			if (this._internalScrollHost == null)
			{
				if (this._internalItemsHost != null)
				{
					this._internalScrollHost = DataGridHelper.FindVisualParent<ScrollViewer>(this._internalItemsHost);
				}
				else if (this._rowTrackingRoot != null)
				{
					DataGridRow container2 = this._rowTrackingRoot.Container;
					this._internalScrollHost = DataGridHelper.FindVisualParent<ScrollViewer>(container2);
				}
				if (this._internalScrollHost != null)
				{
					Binding binding = new Binding("ContentHorizontalOffset");
					binding.Source = this._internalScrollHost;
					base.SetBinding(DataGrid.HorizontalScrollOffsetProperty, binding);
				}
			}
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x0013ADE6 File Offset: 0x00138FE6
		private void CleanUpInternalScrollControls()
		{
			BindingOperations.ClearBinding(this, DataGrid.HorizontalScrollOffsetProperty);
			this._internalScrollHost = null;
			if (this._internalScrollContentPresenter != null)
			{
				this._internalScrollContentPresenter.SizeChanged -= this.OnInternalScrollContentPresenterSizeChanged;
				this._internalScrollContentPresenter = null;
			}
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x0013AE20 File Offset: 0x00139020
		private void OnInternalScrollContentPresenterSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (this._internalScrollContentPresenter != null && !this._internalScrollContentPresenter.CanContentScroll)
			{
				this.OnViewportSizeChanged(e.PreviousSize, e.NewSize);
			}
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x0013AE4C File Offset: 0x0013904C
		internal void OnViewportSizeChanged(Size oldSize, Size newSize)
		{
			if (!this.InternalColumns.ColumnWidthsComputationPending)
			{
				double value = newSize.Width - oldSize.Width;
				if (!DoubleUtil.AreClose(value, 0.0))
				{
					this._finalViewportWidth = newSize.Width;
					if (!this._viewportWidthChangeNotificationPending)
					{
						this._originalViewportWidth = oldSize.Width;
						base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.OnDelayedViewportWidthChanged), DispatcherPriority.Loaded, new object[]
						{
							this
						});
						this._viewportWidthChangeNotificationPending = true;
					}
				}
			}
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x0013AED4 File Offset: 0x001390D4
		private object OnDelayedViewportWidthChanged(object args)
		{
			if (!this._viewportWidthChangeNotificationPending)
			{
				return null;
			}
			double num = this._finalViewportWidth - this._originalViewportWidth;
			if (!DoubleUtil.AreClose(num, 0.0))
			{
				this.NotifyPropertyChanged(this, "ViewportWidth", default(DependencyPropertyChangedEventArgs), DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
				double num2 = this._finalViewportWidth;
				num2 -= this.CellsPanelHorizontalOffset;
				this.InternalColumns.RedistributeColumnWidthsOnAvailableSpaceChange(num, num2);
			}
			this._viewportWidthChangeNotificationPending = false;
			return null;
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x0013AF46 File Offset: 0x00139146
		internal void OnHasVisibleStarColumnsChanged()
		{
			this.DetermineItemsHostStarBehavior();
		}

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06004577 RID: 17783 RVA: 0x0013AF4E File Offset: 0x0013914E
		internal double HorizontalScrollOffset
		{
			get
			{
				return (double)base.GetValue(DataGrid.HorizontalScrollOffsetProperty);
			}
		}

		/// <summary>Represents the command that indicates the intention to delete the current row.</summary>
		/// <returns>The <see cref="P:System.Windows.Input.ApplicationCommands.Delete" /> command that indicates the intention to delete the current row.</returns>
		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06004578 RID: 17784 RVA: 0x0013AF60 File Offset: 0x00139160
		public static RoutedUICommand DeleteCommand
		{
			get
			{
				return ApplicationCommands.Delete;
			}
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x0013AF67 File Offset: 0x00139167
		private static void OnCanExecuteBeginEdit(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteBeginEdit(e);
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x0013AF75 File Offset: 0x00139175
		private static void OnExecutedBeginEdit(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedBeginEdit(e);
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.CanExecute" /> event associated with the <see cref="F:System.Windows.Controls.DataGrid.BeginEditCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600457B RID: 17787 RVA: 0x0013AF84 File Offset: 0x00139184
		protected virtual void OnCanExecuteBeginEdit(CanExecuteRoutedEventArgs e)
		{
			bool flag = !this.IsReadOnly && this.CurrentCellContainer != null && !this.IsEditingCurrentCell && !this.IsCurrentCellReadOnly && !this.HasCellValidationError;
			if (flag && this.HasRowValidationError)
			{
				DataGridCell eventCellOrCurrentCell = this.GetEventCellOrCurrentCell(e);
				if (eventCellOrCurrentCell != null)
				{
					object rowDataItem = eventCellOrCurrentCell.RowDataItem;
					flag = this.IsAddingOrEditingRowItem(rowDataItem);
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				e.CanExecute = true;
				e.Handled = true;
				return;
			}
			e.ContinueRouting = true;
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.Executed" /> event associated with the <see cref="F:System.Windows.Controls.DataGrid.BeginEditCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600457C RID: 17788 RVA: 0x0013B000 File Offset: 0x00139200
		protected virtual void OnExecutedBeginEdit(ExecutedRoutedEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null && !currentCellContainer.IsReadOnly && !currentCellContainer.IsEditing)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				List<int> list = null;
				int num = -1;
				object obj = null;
				bool flag4 = this.EditableItems.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning;
				if (this.IsNewItemPlaceholder(currentCellContainer.RowDataItem))
				{
					if (base.SelectedItems.Contains(CollectionView.NewItemPlaceholder))
					{
						this.UnselectItem(base.NewItemInfo(CollectionView.NewItemPlaceholder, null, -1));
						flag2 = true;
					}
					else
					{
						num = base.ItemContainerGenerator.IndexFromContainer(currentCellContainer.RowOwner);
						flag3 = (num >= 0 && this._selectedCells.Intersects(num, out list));
					}
					obj = this.AddNewItem();
					this.SetCurrentCellToNewItem(obj);
					currentCellContainer = this.CurrentCellContainer;
					if (this.CurrentCellContainer == null)
					{
						base.UpdateLayout();
						currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer != null && !currentCellContainer.IsKeyboardFocusWithin)
						{
							currentCellContainer.Focus();
						}
					}
					if (flag2)
					{
						this.SelectItem(base.NewItemInfo(obj, null, -1));
					}
					else if (flag3)
					{
						using (this.UpdateSelectedCells())
						{
							int num2 = num;
							if (flag4)
							{
								this._selectedCells.RemoveRegion(num, 0, 1, this.Columns.Count);
								num2++;
							}
							int i = 0;
							int count = list.Count;
							while (i < count)
							{
								this._selectedCells.AddRegion(num2, list[i], 1, list[i + 1]);
								i += 2;
							}
						}
					}
					flag = true;
				}
				RoutedEventArgs routedEventArgs = e.Parameter as RoutedEventArgs;
				DataGridBeginningEditEventArgs dataGridBeginningEditEventArgs = null;
				if (currentCellContainer != null)
				{
					dataGridBeginningEditEventArgs = new DataGridBeginningEditEventArgs(currentCellContainer.Column, currentCellContainer.RowOwner, routedEventArgs);
					this.OnBeginningEdit(dataGridBeginningEditEventArgs);
				}
				if (currentCellContainer == null || dataGridBeginningEditEventArgs.Cancel)
				{
					if (flag2)
					{
						this.UnselectItem(base.NewItemInfo(obj, null, -1));
					}
					else if (flag3 && flag4)
					{
						this._selectedCells.RemoveRegion(num + 1, 0, 1, this.Columns.Count);
					}
					if (flag)
					{
						this.CancelRowItem();
						this.UpdateNewItemPlaceholder(false);
						this.SetCurrentItemToPlaceholder();
					}
					if (flag2)
					{
						this.SelectItem(base.NewItemInfo(CollectionView.NewItemPlaceholder, null, -1));
					}
					else if (flag3)
					{
						int j = 0;
						int count2 = list.Count;
						while (j < count2)
						{
							this._selectedCells.AddRegion(num, list[j], 1, list[j + 1]);
							j += 2;
						}
					}
				}
				else
				{
					if (!flag && !this.IsEditingRowItem)
					{
						this.EditRowItem(currentCellContainer.RowDataItem);
						BindingGroup bindingGroup = currentCellContainer.RowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							bindingGroup.BeginEdit();
						}
						this._editingRowInfo = base.ItemInfoFromContainer(currentCellContainer.RowOwner);
					}
					currentCellContainer.BeginEdit(routedEventArgs);
					currentCellContainer.RowOwner.IsEditing = true;
					this.EnsureCellAutomationValueHolder(currentCellContainer);
				}
			}
			CommandManager.InvalidateRequerySuggested();
			e.Handled = true;
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x0013B2EC File Offset: 0x001394EC
		private static void OnCanExecuteCommitEdit(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteCommitEdit(e);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x0013B2FA File Offset: 0x001394FA
		private static void OnExecutedCommitEdit(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedCommitEdit(e);
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x0013B308 File Offset: 0x00139508
		private DataGridCell GetEventCellOrCurrentCell(RoutedEventArgs e)
		{
			UIElement uielement = e.OriginalSource as UIElement;
			if (uielement != this && uielement != null)
			{
				return DataGridHelper.FindVisualParent<DataGridCell>(uielement);
			}
			return this.CurrentCellContainer;
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x0013B338 File Offset: 0x00139538
		private bool CanEndEdit(CanExecuteRoutedEventArgs e, bool commit)
		{
			DataGridCell eventCellOrCurrentCell = this.GetEventCellOrCurrentCell(e);
			if (eventCellOrCurrentCell == null)
			{
				return false;
			}
			DataGridEditingUnit editingUnit = this.GetEditingUnit(e.Parameter);
			IEditableCollectionView editableItems = this.EditableItems;
			object rowDataItem = eventCellOrCurrentCell.RowDataItem;
			return eventCellOrCurrentCell.IsEditing || (!this.HasCellValidationError && this.IsAddingOrEditingRowItem(editingUnit, rowDataItem));
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.CanExecute" /> event associated with the <see cref="F:System.Windows.Controls.DataGrid.CommitEditCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004581 RID: 17793 RVA: 0x0013B389 File Offset: 0x00139589
		protected virtual void OnCanExecuteCommitEdit(CanExecuteRoutedEventArgs e)
		{
			if (this.CanEndEdit(e, true))
			{
				e.CanExecute = true;
				e.Handled = true;
				return;
			}
			e.ContinueRouting = true;
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.Executed" /> event associated with the <see cref="F:System.Windows.Controls.DataGrid.CommitEditCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004582 RID: 17794 RVA: 0x0013B3AC File Offset: 0x001395AC
		protected virtual void OnExecutedCommitEdit(ExecutedRoutedEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			bool flag = true;
			if (currentCellContainer != null)
			{
				DataGridEditingUnit editingUnit = this.GetEditingUnit(e.Parameter);
				bool flag2 = false;
				if (currentCellContainer.IsEditing)
				{
					DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs = new DataGridCellEditEndingEventArgs(currentCellContainer.Column, currentCellContainer.RowOwner, currentCellContainer.EditingElement, DataGridEditAction.Commit);
					this.OnCellEditEnding(dataGridCellEditEndingEventArgs);
					flag2 = dataGridCellEditEndingEventArgs.Cancel;
					if (!flag2)
					{
						flag = currentCellContainer.CommitEdit();
						this.HasCellValidationError = !flag;
						this.UpdateCellAutomationValueHolder(currentCellContainer);
					}
				}
				if (flag && !flag2 && this.IsAddingOrEditingRowItem(editingUnit, currentCellContainer.RowDataItem))
				{
					DataGridRowEditEndingEventArgs dataGridRowEditEndingEventArgs = new DataGridRowEditEndingEventArgs(currentCellContainer.RowOwner, DataGridEditAction.Commit);
					this.OnRowEditEnding(dataGridRowEditEndingEventArgs);
					if (!dataGridRowEditEndingEventArgs.Cancel)
					{
						BindingGroup bindingGroup = currentCellContainer.RowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							base.Dispatcher.Invoke(new DispatcherOperationCallback(DataGrid.DoNothing), DispatcherPriority.DataBind, new object[]
							{
								bindingGroup
							});
							flag = bindingGroup.CommitEdit();
						}
						this.HasRowValidationError = !flag;
						if (flag)
						{
							this.CommitRowItem();
						}
					}
				}
				if (flag)
				{
					this.UpdateRowEditing(currentCellContainer);
					if (!currentCellContainer.RowOwner.IsEditing)
					{
						this.ReleaseCellAutomationValueHolders();
					}
				}
				CommandManager.InvalidateRequerySuggested();
			}
			e.Handled = true;
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x0000C238 File Offset: 0x0000A438
		private static object DoNothing(object arg)
		{
			return null;
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x0013B4D4 File Offset: 0x001396D4
		private DataGridEditingUnit GetEditingUnit(object parameter)
		{
			if (parameter != null && parameter is DataGridEditingUnit)
			{
				return (DataGridEditingUnit)parameter;
			}
			if (!this.IsEditingCurrentCell)
			{
				return DataGridEditingUnit.Row;
			}
			return DataGridEditingUnit.Cell;
		}

		/// <summary>Occurs before a row edit is committed or canceled. </summary>
		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06004585 RID: 17797 RVA: 0x0013B4F4 File Offset: 0x001396F4
		// (remove) Token: 0x06004586 RID: 17798 RVA: 0x0013B52C File Offset: 0x0013972C
		public event EventHandler<DataGridRowEditEndingEventArgs> RowEditEnding;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.RowEditEnding" /> event. </summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004587 RID: 17799 RVA: 0x0013B564 File Offset: 0x00139764
		protected virtual void OnRowEditEnding(DataGridRowEditEndingEventArgs e)
		{
			if (this.RowEditEnding != null)
			{
				this.RowEditEnding(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationRowInvokeEvents(e.Row);
				}
			}
		}

		/// <summary>Occurs before a cell edit is committed or canceled. </summary>
		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06004588 RID: 17800 RVA: 0x0013B5AC File Offset: 0x001397AC
		// (remove) Token: 0x06004589 RID: 17801 RVA: 0x0013B5E4 File Offset: 0x001397E4
		public event EventHandler<DataGridCellEditEndingEventArgs> CellEditEnding;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.CellEditEnding" /> event. </summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600458A RID: 17802 RVA: 0x0013B61C File Offset: 0x0013981C
		protected virtual void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
		{
			if (this.CellEditEnding != null)
			{
				this.CellEditEnding(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationCellInvokeEvents(e.Column, e.Row);
				}
			}
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x0013B667 File Offset: 0x00139867
		private static void OnCanExecuteCancelEdit(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteCancelEdit(e);
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x0013B675 File Offset: 0x00139875
		private static void OnExecutedCancelEdit(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedCancelEdit(e);
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.CanExecute" /> event associated with the <see cref="F:System.Windows.Controls.DataGrid.CancelEditCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600458D RID: 17805 RVA: 0x0013B683 File Offset: 0x00139883
		protected virtual void OnCanExecuteCancelEdit(CanExecuteRoutedEventArgs e)
		{
			if (this.CanEndEdit(e, false))
			{
				e.CanExecute = true;
				e.Handled = true;
				return;
			}
			e.ContinueRouting = true;
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.Executed" /> event associated with the <see cref="F:System.Windows.Controls.DataGrid.CancelEditCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600458E RID: 17806 RVA: 0x0013B6A8 File Offset: 0x001398A8
		protected virtual void OnExecutedCancelEdit(ExecutedRoutedEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null)
			{
				DataGridEditingUnit editingUnit = this.GetEditingUnit(e.Parameter);
				bool flag = false;
				if (currentCellContainer.IsEditing)
				{
					DataGridCellEditEndingEventArgs dataGridCellEditEndingEventArgs = new DataGridCellEditEndingEventArgs(currentCellContainer.Column, currentCellContainer.RowOwner, currentCellContainer.EditingElement, DataGridEditAction.Cancel);
					this.OnCellEditEnding(dataGridCellEditEndingEventArgs);
					flag = dataGridCellEditEndingEventArgs.Cancel;
					if (!flag)
					{
						currentCellContainer.CancelEdit();
						this.HasCellValidationError = false;
						this.UpdateCellAutomationValueHolder(currentCellContainer);
					}
				}
				if (!flag && this.IsAddingOrEditingRowItem(editingUnit, currentCellContainer.RowDataItem))
				{
					DataGridRowEditEndingEventArgs dataGridRowEditEndingEventArgs = new DataGridRowEditEndingEventArgs(currentCellContainer.RowOwner, DataGridEditAction.Cancel);
					this.OnRowEditEnding(dataGridRowEditEndingEventArgs);
					bool flag2 = !dataGridRowEditEndingEventArgs.Cancel;
					if (flag2)
					{
						BindingGroup bindingGroup = currentCellContainer.RowOwner.BindingGroup;
						if (bindingGroup != null)
						{
							bindingGroup.CancelEdit();
						}
						this.CancelRowItem();
					}
				}
				this.UpdateRowEditing(currentCellContainer);
				if (!currentCellContainer.RowOwner.IsEditing)
				{
					this.HasRowValidationError = false;
					this.ReleaseCellAutomationValueHolders();
				}
				CommandManager.InvalidateRequerySuggested();
			}
			e.Handled = true;
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x0013B79D File Offset: 0x0013999D
		private static void OnCanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			((DataGrid)sender).OnCanExecuteDelete(e);
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x0013B7AB File Offset: 0x001399AB
		private static void OnExecutedDelete(object sender, ExecutedRoutedEventArgs e)
		{
			((DataGrid)sender).OnExecutedDelete(e);
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.CanExecute" /> event associated with the <see cref="P:System.Windows.Controls.DataGrid.DeleteCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004591 RID: 17809 RVA: 0x0013B7B9 File Offset: 0x001399B9
		protected virtual void OnCanExecuteDelete(CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = (this.CanUserDeleteRows && this.DataItemsSelected > 0 && (this._currentCellContainer == null || !this._currentCellContainer.IsEditing));
			e.Handled = true;
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.Executed" /> event associated with the <see cref="P:System.Windows.Controls.DataGrid.DeleteCommand" /> command.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004592 RID: 17810 RVA: 0x0013B7F8 File Offset: 0x001399F8
		protected virtual void OnExecutedDelete(ExecutedRoutedEventArgs e)
		{
			if (this.DataItemsSelected > 0)
			{
				bool flag = false;
				bool isEditingRowItem = this.IsEditingRowItem;
				if (isEditingRowItem || this.IsAddingNewItem)
				{
					if (this.CancelEdit(DataGridEditingUnit.Row) && isEditingRowItem)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					int count = base.SelectedItems.Count;
					int num = -1;
					ItemsControl.ItemInfo currentInfo = this.CurrentInfo;
					if (base.SelectedItems.Contains(currentInfo.Item))
					{
						num = currentInfo.Index;
						if (this._selectionAnchor != null)
						{
							int index = this._selectionAnchor.Value.ItemInfo.Index;
							if (index >= 0 && index < num)
							{
								num = index;
							}
						}
						num = Math.Min(base.Items.Count - count - 1, num);
					}
					ArrayList arrayList = new ArrayList(base.SelectedItems);
					using (this.UpdateSelectedCells())
					{
						bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
						if (!isUpdatingSelectedItems)
						{
							base.BeginUpdateSelectedItems();
						}
						try
						{
							this._selectedCells.ClearFullRows(base.SelectedItems);
							base.SelectedItems.Clear();
						}
						finally
						{
							if (!isUpdatingSelectedItems)
							{
								base.EndUpdateSelectedItems();
							}
						}
					}
					for (int i = 0; i < count; i++)
					{
						object obj = arrayList[i];
						if (obj != CollectionView.NewItemPlaceholder)
						{
							this.EditableItems.Remove(obj);
						}
					}
					if (num >= 0)
					{
						object currentItem = base.Items[num];
						this.SetCurrentItem(currentItem);
						DataGridCell currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer != null)
						{
							this._selectionAnchor = null;
							this.HandleSelectionForCellInput(currentCellContainer, false, false, false);
						}
					}
				}
			}
			e.Handled = true;
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x0013B9A8 File Offset: 0x00139BA8
		private void SetCurrentCellToNewItem(object newItem)
		{
			ItemsControl.ItemInfo itemInfo = null;
			NewItemPlaceholderPosition newItemPlaceholderPosition = this.EditableItems.NewItemPlaceholderPosition;
			if (newItemPlaceholderPosition != NewItemPlaceholderPosition.AtBeginning)
			{
				if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
				{
					int num = base.Items.Count - 2;
					if (num >= 0 && ItemsControl.EqualsEx(newItem, base.Items[num]))
					{
						itemInfo = base.ItemInfoFromIndex(num);
					}
				}
			}
			else
			{
				int num = 1;
				if (num < base.Items.Count && ItemsControl.EqualsEx(newItem, base.Items[num]))
				{
					itemInfo = base.ItemInfoFromIndex(num);
				}
			}
			if (itemInfo == null)
			{
				itemInfo = base.ItemInfoFromIndex(base.Items.IndexOf(newItem));
			}
			DataGridCellInfo dataGridCellInfo = this.CurrentCell;
			dataGridCellInfo = ((itemInfo != null) ? new DataGridCellInfo(itemInfo, dataGridCellInfo.Column, this) : DataGridCellInfo.CreatePossiblyPartialCellInfo(newItem, dataGridCellInfo.Column, this));
			base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, dataGridCellInfo);
		}

		/// <summary>Gets or sets a value that indicates whether the user can edit values in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the rows and cells are read-only; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x06004594 RID: 17812 RVA: 0x0013BA83 File Offset: 0x00139C83
		// (set) Token: 0x06004595 RID: 17813 RVA: 0x0013BA95 File Offset: 0x00139C95
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(DataGrid.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(DataGrid.IsReadOnlyProperty, value);
			}
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x0013BAA3 File Offset: 0x00139CA3
		private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				((DataGrid)d).CancelAnyEdit();
			}
			CommandManager.InvalidateRequerySuggested();
			d.CoerceValue(DataGrid.CanUserAddRowsProperty);
			d.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
			DataGrid.OnNotifyColumnAndCellPropertyChanged(d, e);
		}

		/// <summary>Gets the data item bound to the row that contains the current cell.</summary>
		/// <returns>The data item bound to the row that contains the current cell. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001119 RID: 4377
		// (get) Token: 0x06004597 RID: 17815 RVA: 0x0013BAE0 File Offset: 0x00139CE0
		// (set) Token: 0x06004598 RID: 17816 RVA: 0x0013BAED File Offset: 0x00139CED
		public object CurrentItem
		{
			get
			{
				return base.GetValue(DataGrid.CurrentItemProperty);
			}
			set
			{
				base.SetValue(DataGrid.CurrentItemProperty, value);
			}
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x0013BAFC File Offset: 0x00139CFC
		private static void OnCurrentItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridCellInfo currentCell = dataGrid.CurrentCell;
			object newValue = e.NewValue;
			if (currentCell.Item != newValue)
			{
				dataGrid.SetCurrentValueInternal(DataGrid.CurrentCellProperty, DataGridCellInfo.CreatePossiblyPartialCellInfo(newValue, currentCell.Column, dataGrid));
			}
			DataGrid.OnNotifyRowHeaderPropertyChanged(d, e);
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x0013BB4E File Offset: 0x00139D4E
		private void SetCurrentItem(object item)
		{
			if (item == DependencyProperty.UnsetValue)
			{
				item = null;
			}
			base.SetCurrentValueInternal(DataGrid.CurrentItemProperty, item);
		}

		/// <summary>Gets or sets the column that contains the current cell.</summary>
		/// <returns>The column that contains the current cell. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700111A RID: 4378
		// (get) Token: 0x0600459B RID: 17819 RVA: 0x0013BB67 File Offset: 0x00139D67
		// (set) Token: 0x0600459C RID: 17820 RVA: 0x0013BB79 File Offset: 0x00139D79
		public DataGridColumn CurrentColumn
		{
			get
			{
				return (DataGridColumn)base.GetValue(DataGrid.CurrentColumnProperty);
			}
			set
			{
				base.SetValue(DataGrid.CurrentColumnProperty, value);
			}
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x0013BB88 File Offset: 0x00139D88
		private static void OnCurrentColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridCellInfo currentCell = dataGrid.CurrentCell;
			DataGridColumn dataGridColumn = (DataGridColumn)e.NewValue;
			if (currentCell.Column != dataGridColumn)
			{
				dataGrid.SetCurrentValueInternal(DataGrid.CurrentCellProperty, DataGridCellInfo.CreatePossiblyPartialCellInfo(currentCell.Item, dataGridColumn, dataGrid));
			}
		}

		/// <summary>Gets or sets the cell that has focus.</summary>
		/// <returns>Information about the cell that has focus. </returns>
		// Token: 0x1700111B RID: 4379
		// (get) Token: 0x0600459E RID: 17822 RVA: 0x0013BBD8 File Offset: 0x00139DD8
		// (set) Token: 0x0600459F RID: 17823 RVA: 0x0013BBEA File Offset: 0x00139DEA
		public DataGridCellInfo CurrentCell
		{
			get
			{
				return (DataGridCellInfo)base.GetValue(DataGrid.CurrentCellProperty);
			}
			set
			{
				base.SetValue(DataGrid.CurrentCellProperty, value);
			}
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x0013BC00 File Offset: 0x00139E00
		private static void OnCurrentCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridCellInfo dataGridCellInfo = (DataGridCellInfo)e.OldValue;
			DataGridCellInfo dataGridCellInfo2 = (DataGridCellInfo)e.NewValue;
			if (dataGrid.CurrentItem != dataGridCellInfo2.Item)
			{
				dataGrid.SetCurrentItem(dataGridCellInfo2.Item);
			}
			if (dataGrid.CurrentColumn != dataGridCellInfo2.Column)
			{
				dataGrid.SetCurrentValueInternal(DataGrid.CurrentColumnProperty, dataGridCellInfo2.Column);
			}
			if (dataGrid._currentCellContainer != null)
			{
				if ((dataGrid.IsAddingNewItem || dataGrid.IsEditingRowItem) && dataGridCellInfo.Item != dataGridCellInfo2.Item)
				{
					dataGrid.EndEdit(DataGrid.CommitEditCommand, dataGrid._currentCellContainer, DataGridEditingUnit.Row, true);
				}
				else if (dataGrid._currentCellContainer.IsEditing)
				{
					dataGrid.EndEdit(DataGrid.CommitEditCommand, dataGrid._currentCellContainer, DataGridEditingUnit.Cell, true);
				}
			}
			DataGridCell currentCellContainer = dataGrid._currentCellContainer;
			dataGrid._currentCellContainer = null;
			if (dataGridCellInfo2.IsValid && dataGrid.IsKeyboardFocusWithin)
			{
				DataGridCell dataGridCell = dataGrid._pendingCurrentCellContainer;
				if (dataGridCell == null)
				{
					dataGridCell = dataGrid.CurrentCellContainer;
					if (dataGridCell == null)
					{
						dataGrid.ScrollCellIntoView(dataGridCellInfo2.ItemInfo, dataGridCellInfo2.Column);
						dataGridCell = dataGrid.CurrentCellContainer;
					}
				}
				if (dataGridCell != null)
				{
					if (!dataGridCell.IsKeyboardFocusWithin)
					{
						dataGridCell.Focus();
					}
					if (currentCellContainer != dataGridCell)
					{
						if (currentCellContainer != null)
						{
							currentCellContainer.NotifyCurrentCellContainerChanged();
						}
						dataGridCell.NotifyCurrentCellContainerChanged();
					}
				}
				else if (currentCellContainer != null)
				{
					currentCellContainer.NotifyCurrentCellContainerChanged();
				}
			}
			dataGrid.OnCurrentCellChanged(EventArgs.Empty);
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Controls.DataGrid.CurrentCell" /> property has changed.</summary>
		// Token: 0x140000B2 RID: 178
		// (add) Token: 0x060045A1 RID: 17825 RVA: 0x0013BD60 File Offset: 0x00139F60
		// (remove) Token: 0x060045A2 RID: 17826 RVA: 0x0013BD98 File Offset: 0x00139F98
		public event EventHandler<EventArgs> CurrentCellChanged;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.CurrentCellChanged" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045A3 RID: 17827 RVA: 0x0013BDCD File Offset: 0x00139FCD
		protected virtual void OnCurrentCellChanged(EventArgs e)
		{
			if (this.CurrentCellChanged != null)
			{
				this.CurrentCellChanged(this, e);
			}
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x0013BDE4 File Offset: 0x00139FE4
		private void UpdateCurrentCell(DataGridCell cell, bool isFocusWithinCell)
		{
			if (isFocusWithinCell)
			{
				this.CurrentCellContainer = cell;
				return;
			}
			if (!base.IsKeyboardFocusWithin)
			{
				this.CurrentCellContainer = null;
			}
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x060045A5 RID: 17829 RVA: 0x0013BE00 File Offset: 0x0013A000
		// (set) Token: 0x060045A6 RID: 17830 RVA: 0x0013BE38 File Offset: 0x0013A038
		internal DataGridCell CurrentCellContainer
		{
			get
			{
				if (this._currentCellContainer == null)
				{
					DataGridCellInfo currentCell = this.CurrentCell;
					if (currentCell.IsValid)
					{
						this._currentCellContainer = this.TryFindCell(currentCell);
					}
				}
				return this._currentCellContainer;
			}
			set
			{
				if (this._currentCellContainer != value && (value == null || value != this._pendingCurrentCellContainer))
				{
					this._pendingCurrentCellContainer = value;
					if (value == null)
					{
						base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, DataGridCellInfo.Unset);
					}
					else
					{
						base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, new DataGridCellInfo(value));
					}
					this._pendingCurrentCellContainer = null;
					this._currentCellContainer = value;
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x060045A7 RID: 17831 RVA: 0x0013BEA4 File Offset: 0x0013A0A4
		private bool IsEditingCurrentCell
		{
			get
			{
				DataGridCell currentCellContainer = this.CurrentCellContainer;
				return currentCellContainer != null && currentCellContainer.IsEditing;
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x060045A8 RID: 17832 RVA: 0x0013BEC4 File Offset: 0x0013A0C4
		private bool IsCurrentCellReadOnly
		{
			get
			{
				DataGridCell currentCellContainer = this.CurrentCellContainer;
				return currentCellContainer != null && currentCellContainer.IsReadOnly;
			}
		}

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x060045A9 RID: 17833 RVA: 0x0013BEE4 File Offset: 0x0013A0E4
		internal ItemsControl.ItemInfo CurrentInfo
		{
			get
			{
				return base.LeaseItemInfo(this.CurrentCell.ItemInfo, false);
			}
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x0013BF08 File Offset: 0x0013A108
		internal bool IsCurrent(DataGridRow row, DataGridColumn column = null)
		{
			DataGridCellInfo dataGridCellInfo = this.CurrentCell;
			if (dataGridCellInfo.ItemInfo == null)
			{
				dataGridCellInfo = DataGridCellInfo.Unset;
			}
			DependencyObject container = dataGridCellInfo.ItemInfo.Container;
			int index = dataGridCellInfo.ItemInfo.Index;
			return (column == null || column == dataGridCellInfo.Column) && ((container != null && container == row) || (ItemsControl.EqualsEx(this.CurrentItem, row.Item) && (index < 0 || index == base.ItemContainerGenerator.IndexFromContainer(row))));
		}

		/// <summary>Occurs before a row or cell enters edit mode.</summary>
		// Token: 0x140000B3 RID: 179
		// (add) Token: 0x060045AB RID: 17835 RVA: 0x0013BF90 File Offset: 0x0013A190
		// (remove) Token: 0x060045AC RID: 17836 RVA: 0x0013BFC8 File Offset: 0x0013A1C8
		public event EventHandler<DataGridBeginningEditEventArgs> BeginningEdit;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.BeginningEdit" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045AD RID: 17837 RVA: 0x0013C000 File Offset: 0x0013A200
		protected virtual void OnBeginningEdit(DataGridBeginningEditEventArgs e)
		{
			if (this.BeginningEdit != null)
			{
				this.BeginningEdit(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.InvokePatternOnInvoked))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationCellInvokeEvents(e.Column, e.Row);
				}
			}
		}

		/// <summary>Occurs when a cell enters edit mode. </summary>
		// Token: 0x140000B4 RID: 180
		// (add) Token: 0x060045AE RID: 17838 RVA: 0x0013C04C File Offset: 0x0013A24C
		// (remove) Token: 0x060045AF RID: 17839 RVA: 0x0013C084 File Offset: 0x0013A284
		public event EventHandler<DataGridPreparingCellForEditEventArgs> PreparingCellForEdit;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.PreparingCellForEdit" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045B0 RID: 17840 RVA: 0x0013C0B9 File Offset: 0x0013A2B9
		protected internal virtual void OnPreparingCellForEdit(DataGridPreparingCellForEditEventArgs e)
		{
			if (this.PreparingCellForEdit != null)
			{
				this.PreparingCellForEdit(this, e);
			}
		}

		/// <summary>Invokes the <see cref="M:System.Windows.Controls.DataGrid.BeginEdit" /> command, which will place the current cell or row into edit mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the current cell or row enters edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x060045B1 RID: 17841 RVA: 0x0013C0D0 File Offset: 0x0013A2D0
		public bool BeginEdit()
		{
			return this.BeginEdit(null);
		}

		/// <summary>Invokes the <see cref="M:System.Windows.Controls.DataGrid.BeginEdit" /> command, which will place the current cell or row into edit mode.</summary>
		/// <param name="editingEventArgs">If called from an event handler, the event arguments. May be <see langword="null" />.</param>
		/// <returns>
		///     <see langword="true" /> if the current cell or row enters edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x060045B2 RID: 17842 RVA: 0x0013C0DC File Offset: 0x0013A2DC
		public bool BeginEdit(RoutedEventArgs editingEventArgs)
		{
			if (!this.IsReadOnly)
			{
				DataGridCell currentCellContainer = this.CurrentCellContainer;
				if (currentCellContainer != null)
				{
					if (!currentCellContainer.IsEditing && DataGrid.BeginEditCommand.CanExecute(editingEventArgs, currentCellContainer))
					{
						DataGrid.BeginEditCommand.Execute(editingEventArgs, currentCellContainer);
						currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer == null)
						{
							return false;
						}
					}
					return currentCellContainer.IsEditing;
				}
			}
			return false;
		}

		/// <summary>Invokes the <see cref="F:System.Windows.Controls.DataGrid.CancelEditCommand" /> command for the cell or row currently in edit mode.</summary>
		/// <returns>
		///     <see langword="true" /> if the current cell or row exits edit mode, or if no cells or rows are in edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x060045B3 RID: 17843 RVA: 0x0013C131 File Offset: 0x0013A331
		public bool CancelEdit()
		{
			if (this.IsEditingCurrentCell)
			{
				return this.CancelEdit(DataGridEditingUnit.Cell);
			}
			return (!this.IsEditingRowItem && !this.IsAddingNewItem) || this.CancelEdit(DataGridEditingUnit.Row);
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x0013C15C File Offset: 0x0013A35C
		internal bool CancelEdit(DataGridCell cell)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			return currentCellContainer == null || currentCellContainer != cell || !currentCellContainer.IsEditing || this.CancelEdit(DataGridEditingUnit.Cell);
		}

		/// <summary>Invokes the <see cref="F:System.Windows.Controls.DataGrid.CancelEditCommand" /> command for the specified cell or row in edit mode. </summary>
		/// <param name="editingUnit">One of the enumeration values that specifies whether to cancel row or cell edits.</param>
		/// <returns>
		///     <see langword="true" /> if the current cell or row exits edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x060045B5 RID: 17845 RVA: 0x0013C188 File Offset: 0x0013A388
		public bool CancelEdit(DataGridEditingUnit editingUnit)
		{
			return this.EndEdit(DataGrid.CancelEditCommand, this.CurrentCellContainer, editingUnit, true);
		}

		// Token: 0x060045B6 RID: 17846 RVA: 0x0013C19D File Offset: 0x0013A39D
		private void CancelAnyEdit()
		{
			if (this.IsAddingNewItem || this.IsEditingRowItem)
			{
				this.CancelEdit(DataGridEditingUnit.Row);
				return;
			}
			if (this.IsEditingCurrentCell)
			{
				this.CancelEdit(DataGridEditingUnit.Cell);
			}
		}

		/// <summary>Invokes the <see cref="F:System.Windows.Controls.DataGrid.CommitEditCommand" /> command for the cell or row currently in edit mode. </summary>
		/// <returns>
		///     <see langword="true" /> if the current cell or row exits edit mode, or if no cells or rows are in edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x060045B7 RID: 17847 RVA: 0x0013C1C8 File Offset: 0x0013A3C8
		public bool CommitEdit()
		{
			if (this.IsEditingCurrentCell)
			{
				return this.CommitEdit(DataGridEditingUnit.Cell, true);
			}
			return (!this.IsEditingRowItem && !this.IsAddingNewItem) || this.CommitEdit(DataGridEditingUnit.Row, true);
		}

		/// <summary>Invokes the <see cref="F:System.Windows.Controls.DataGrid.CommitEditCommand" /> command for the specified cell or row currently in edit mode. </summary>
		/// <param name="editingUnit">One of the enumeration values that specifies whether to commit row or cell edits.</param>
		/// <param name="exitEditingMode">
		///       <see langword="true" /> to exit edit mode; otherwise, <see langword="false" />.</param>
		/// <returns>
		///     <see langword="true" /> if the current cell or row exits edit mode; otherwise, <see langword="false" />.</returns>
		// Token: 0x060045B8 RID: 17848 RVA: 0x0013C1F5 File Offset: 0x0013A3F5
		public bool CommitEdit(DataGridEditingUnit editingUnit, bool exitEditingMode)
		{
			return this.EndEdit(DataGrid.CommitEditCommand, this.CurrentCellContainer, editingUnit, exitEditingMode);
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x0013C20A File Offset: 0x0013A40A
		private bool CommitAnyEdit()
		{
			if (this.IsAddingNewItem || this.IsEditingRowItem)
			{
				return this.CommitEdit(DataGridEditingUnit.Row, true);
			}
			return !this.IsEditingCurrentCell || this.CommitEdit(DataGridEditingUnit.Cell, true);
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x0013C238 File Offset: 0x0013A438
		private bool EndEdit(RoutedCommand command, DataGridCell cellContainer, DataGridEditingUnit editingUnit, bool exitEditMode)
		{
			bool flag = true;
			bool flag2 = true;
			if (cellContainer != null)
			{
				if (command.CanExecute(editingUnit, cellContainer))
				{
					command.Execute(editingUnit, cellContainer);
				}
				flag = !cellContainer.IsEditing;
				flag2 = (!this.IsEditingRowItem && !this.IsAddingNewItem);
			}
			if (!exitEditMode)
			{
				if (editingUnit != DataGridEditingUnit.Cell)
				{
					if (flag2)
					{
						object rowDataItem = cellContainer.RowDataItem;
						if (rowDataItem != null)
						{
							this.EditRowItem(rowDataItem);
							return this.IsEditingRowItem;
						}
					}
					return false;
				}
				if (cellContainer == null)
				{
					return false;
				}
				if (flag)
				{
					return this.BeginEdit(null);
				}
			}
			return flag && (editingUnit == DataGridEditingUnit.Cell || flag2);
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x060045BB RID: 17851 RVA: 0x0013C2C6 File Offset: 0x0013A4C6
		// (set) Token: 0x060045BC RID: 17852 RVA: 0x0013C2CE File Offset: 0x0013A4CE
		private bool HasCellValidationError
		{
			get
			{
				return this._hasCellValidationError;
			}
			set
			{
				if (this._hasCellValidationError != value)
				{
					this._hasCellValidationError = value;
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x17001121 RID: 4385
		// (get) Token: 0x060045BD RID: 17853 RVA: 0x0013C2E5 File Offset: 0x0013A4E5
		// (set) Token: 0x060045BE RID: 17854 RVA: 0x0013C2ED File Offset: 0x0013A4ED
		private bool HasRowValidationError
		{
			get
			{
				return this._hasRowValidationError;
			}
			set
			{
				if (this._hasRowValidationError != value)
				{
					this._hasRowValidationError = value;
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		// Token: 0x17001122 RID: 4386
		// (get) Token: 0x060045BF RID: 17855 RVA: 0x0013C304 File Offset: 0x0013A504
		// (set) Token: 0x060045C0 RID: 17856 RVA: 0x0013C30C File Offset: 0x0013A50C
		internal DataGridCell FocusedCell
		{
			get
			{
				return this._focusedCell;
			}
			set
			{
				if (this._focusedCell != value)
				{
					if (this._focusedCell != null)
					{
						this.UpdateCurrentCell(this._focusedCell, false);
					}
					this._focusedCell = value;
					if (this._focusedCell != null)
					{
						this.UpdateCurrentCell(this._focusedCell, true);
					}
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can add new rows to the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can add new rows; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x060045C1 RID: 17857 RVA: 0x0013C348 File Offset: 0x0013A548
		// (set) Token: 0x060045C2 RID: 17858 RVA: 0x0013C35A File Offset: 0x0013A55A
		public bool CanUserAddRows
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserAddRowsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserAddRowsProperty, value);
			}
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x0013C368 File Offset: 0x0013A568
		private static void OnCanUserAddRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).UpdateNewItemPlaceholder(false);
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x0013C376 File Offset: 0x0013A576
		private static object OnCoerceCanUserAddRows(DependencyObject d, object baseValue)
		{
			return DataGrid.OnCoerceCanUserAddOrDeleteRows((DataGrid)d, (bool)baseValue, true);
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x0013C38F File Offset: 0x0013A58F
		private static bool OnCoerceCanUserAddOrDeleteRows(DataGrid dataGrid, bool baseValue, bool canUserAddRowsProperty)
		{
			if (baseValue)
			{
				if (dataGrid.IsReadOnly || !dataGrid.IsEnabled)
				{
					return false;
				}
				if ((canUserAddRowsProperty && !dataGrid.EditableItems.CanAddNew) || (!canUserAddRowsProperty && !dataGrid.EditableItems.CanRemove))
				{
					return false;
				}
			}
			return baseValue;
		}

		/// <summary>Gets or sets a value that indicates whether the user can delete rows from the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can delete rows; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x060045C6 RID: 17862 RVA: 0x0013C3C9 File Offset: 0x0013A5C9
		// (set) Token: 0x060045C7 RID: 17863 RVA: 0x0013C3DB File Offset: 0x0013A5DB
		public bool CanUserDeleteRows
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserDeleteRowsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserDeleteRowsProperty, value);
			}
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x0013C3E9 File Offset: 0x0013A5E9
		private static void OnCanUserDeleteRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0013C3F0 File Offset: 0x0013A5F0
		private static object OnCoerceCanUserDeleteRows(DependencyObject d, object baseValue)
		{
			return DataGrid.OnCoerceCanUserAddOrDeleteRows((DataGrid)d, (bool)baseValue, false);
		}

		/// <summary>Occurs before a new item is added to the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		// Token: 0x140000B5 RID: 181
		// (add) Token: 0x060045CA RID: 17866 RVA: 0x0013C40C File Offset: 0x0013A60C
		// (remove) Token: 0x060045CB RID: 17867 RVA: 0x0013C444 File Offset: 0x0013A644
		public event EventHandler<AddingNewItemEventArgs> AddingNewItem;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.AddingNewItem" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045CC RID: 17868 RVA: 0x0013C479 File Offset: 0x0013A679
		protected virtual void OnAddingNewItem(AddingNewItemEventArgs e)
		{
			if (this.AddingNewItem != null)
			{
				this.AddingNewItem(this, e);
			}
		}

		/// <summary>Occurs when a new item is created. </summary>
		// Token: 0x140000B6 RID: 182
		// (add) Token: 0x060045CD RID: 17869 RVA: 0x0013C490 File Offset: 0x0013A690
		// (remove) Token: 0x060045CE RID: 17870 RVA: 0x0013C4C8 File Offset: 0x0013A6C8
		public event InitializingNewItemEventHandler InitializingNewItem;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.InitializingNewItem" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045CF RID: 17871 RVA: 0x0013C4FD File Offset: 0x0013A6FD
		protected virtual void OnInitializingNewItem(InitializingNewItemEventArgs e)
		{
			if (this.InitializingNewItem != null)
			{
				this.InitializingNewItem(this, e);
			}
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x0013C514 File Offset: 0x0013A714
		private object AddNewItem()
		{
			this.UpdateNewItemPlaceholder(true);
			object obj = null;
			IEditableCollectionViewAddNewItem items = base.Items;
			if (items.CanAddNewItem)
			{
				AddingNewItemEventArgs addingNewItemEventArgs = new AddingNewItemEventArgs();
				this.OnAddingNewItem(addingNewItemEventArgs);
				obj = addingNewItemEventArgs.NewItem;
			}
			obj = ((obj != null) ? items.AddNewItem(obj) : this.EditableItems.AddNew());
			if (obj != null)
			{
				this.OnInitializingNewItem(new InitializingNewItemEventArgs(obj));
			}
			CommandManager.InvalidateRequerySuggested();
			return obj;
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x0013C57A File Offset: 0x0013A77A
		private void EditRowItem(object rowItem)
		{
			this.EditableItems.EditItem(rowItem);
			CommandManager.InvalidateRequerySuggested();
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x0013C58D File Offset: 0x0013A78D
		private void CommitRowItem()
		{
			if (this.IsEditingRowItem)
			{
				this.EditableItems.CommitEdit();
				return;
			}
			this.EditableItems.CommitNew();
			this.UpdateNewItemPlaceholder(false);
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x0013C5B8 File Offset: 0x0013A7B8
		private void CancelRowItem()
		{
			if (this.IsEditingRowItem)
			{
				if (this.EditableItems.CanCancelEdit)
				{
					this.EditableItems.CancelEdit();
					return;
				}
				this.EditableItems.CommitEdit();
				return;
			}
			else
			{
				object currentAddItem = this.EditableItems.CurrentAddItem;
				bool flag = currentAddItem == this.CurrentItem;
				bool flag2 = base.SelectedItems.Contains(currentAddItem);
				bool flag3 = false;
				List<int> list = null;
				int num = -1;
				if (flag2)
				{
					this.UnselectItem(base.NewItemInfo(currentAddItem, null, -1));
				}
				else
				{
					num = base.Items.IndexOf(currentAddItem);
					flag3 = (num >= 0 && this._selectedCells.Intersects(num, out list));
				}
				this.EditableItems.CancelNew();
				this.UpdateNewItemPlaceholder(false);
				if (flag)
				{
					this.SetCurrentItem(CollectionView.NewItemPlaceholder);
				}
				if (flag2)
				{
					this.SelectItem(base.NewItemInfo(CollectionView.NewItemPlaceholder, null, -1));
					return;
				}
				if (flag3)
				{
					using (this.UpdateSelectedCells())
					{
						int num2 = num;
						bool flag4 = this.EditableItems.NewItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning;
						if (flag4)
						{
							this._selectedCells.RemoveRegion(num, 0, 1, this.Columns.Count);
							num2--;
						}
						int i = 0;
						int count = list.Count;
						while (i < count)
						{
							this._selectedCells.AddRegion(num2, list[i], 1, list[i + 1]);
							i += 2;
						}
					}
				}
				return;
			}
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x0013C730 File Offset: 0x0013A930
		private void UpdateRowEditing(DataGridCell cell)
		{
			object rowDataItem = cell.RowDataItem;
			if (!this.IsAddingOrEditingRowItem(rowDataItem))
			{
				cell.RowOwner.IsEditing = false;
				this._editingRowInfo = null;
			}
		}

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x060045D5 RID: 17877 RVA: 0x0013C760 File Offset: 0x0013A960
		private IEditableCollectionView EditableItems
		{
			get
			{
				return base.Items;
			}
		}

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x060045D6 RID: 17878 RVA: 0x0013C768 File Offset: 0x0013A968
		private bool IsAddingNewItem
		{
			get
			{
				return this.EditableItems.IsAddingNew;
			}
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x060045D7 RID: 17879 RVA: 0x0013C775 File Offset: 0x0013A975
		private bool IsEditingRowItem
		{
			get
			{
				return this.EditableItems.IsEditingItem;
			}
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x0013C782 File Offset: 0x0013A982
		private bool IsAddingOrEditingRowItem(object item)
		{
			return this.IsEditingItem(item) || (this.IsAddingNewItem && this.EditableItems.CurrentAddItem == item);
		}

		// Token: 0x060045D9 RID: 17881 RVA: 0x0013C7A7 File Offset: 0x0013A9A7
		private bool IsAddingOrEditingRowItem(DataGridEditingUnit editingUnit, object item)
		{
			return editingUnit == DataGridEditingUnit.Row && this.IsAddingOrEditingRowItem(item);
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x0013C7B6 File Offset: 0x0013A9B6
		private bool IsEditingItem(object item)
		{
			return this.IsEditingRowItem && this.EditableItems.CurrentEditItem == item;
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x0013C7D0 File Offset: 0x0013A9D0
		private void UpdateNewItemPlaceholder(bool isAddingNewItem)
		{
			IEditableCollectionView editableItems = this.EditableItems;
			bool flag = this.CanUserAddRows;
			if (DataGridHelper.IsDefaultValue(this, DataGrid.CanUserAddRowsProperty))
			{
				flag = DataGrid.OnCoerceCanUserAddOrDeleteRows(this, flag, true);
			}
			if (!isAddingNewItem)
			{
				if (flag)
				{
					if (editableItems.NewItemPlaceholderPosition == NewItemPlaceholderPosition.None)
					{
						editableItems.NewItemPlaceholderPosition = NewItemPlaceholderPosition.AtEnd;
					}
					this._placeholderVisibility = Visibility.Visible;
				}
				else
				{
					if (editableItems.NewItemPlaceholderPosition != NewItemPlaceholderPosition.None)
					{
						editableItems.NewItemPlaceholderPosition = NewItemPlaceholderPosition.None;
					}
					this._placeholderVisibility = Visibility.Collapsed;
				}
			}
			else
			{
				this._placeholderVisibility = Visibility.Collapsed;
			}
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(CollectionView.NewItemPlaceholder);
			if (dataGridRow != null)
			{
				dataGridRow.CoerceValue(UIElement.VisibilityProperty);
			}
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x0013C864 File Offset: 0x0013AA64
		private void SetCurrentItemToPlaceholder()
		{
			NewItemPlaceholderPosition newItemPlaceholderPosition = this.EditableItems.NewItemPlaceholderPosition;
			if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd)
			{
				int count = base.Items.Count;
				if (count > 0)
				{
					this.SetCurrentItem(base.Items[count - 1]);
					return;
				}
			}
			else if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning && base.Items.Count > 0)
			{
				this.SetCurrentItem(base.Items[0]);
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x060045DD RID: 17885 RVA: 0x0013C8CC File Offset: 0x0013AACC
		private int DataItemsCount
		{
			get
			{
				int num = base.Items.Count;
				if (this.HasNewItemPlaceholder)
				{
					num--;
				}
				return num;
			}
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x060045DE RID: 17886 RVA: 0x0013C8F4 File Offset: 0x0013AAF4
		private int DataItemsSelected
		{
			get
			{
				int num = base.SelectedItems.Count;
				if (this.HasNewItemPlaceholder && base.SelectedItems.Contains(CollectionView.NewItemPlaceholder))
				{
					num--;
				}
				return num;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x060045DF RID: 17887 RVA: 0x0013C92C File Offset: 0x0013AB2C
		private bool HasNewItemPlaceholder
		{
			get
			{
				IEditableCollectionView editableItems = this.EditableItems;
				return editableItems.NewItemPlaceholderPosition > NewItemPlaceholderPosition.None;
			}
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x0013C949 File Offset: 0x0013AB49
		private bool IsNewItemPlaceholder(object item)
		{
			return item == CollectionView.NewItemPlaceholder || item == DataGrid.NewItemPlaceholder;
		}

		/// <summary>Gets or sets a value that indicates when the details section of a row is displayed.</summary>
		/// <returns>One of the enumeration values that specifies the visibility of row details. The registered default is <see cref="F:System.Windows.Controls.DataGridRowDetailsVisibilityMode.VisibleWhenSelected" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x060045E1 RID: 17889 RVA: 0x0013C95D File Offset: 0x0013AB5D
		// (set) Token: 0x060045E2 RID: 17890 RVA: 0x0013C96F File Offset: 0x0013AB6F
		public DataGridRowDetailsVisibilityMode RowDetailsVisibilityMode
		{
			get
			{
				return (DataGridRowDetailsVisibilityMode)base.GetValue(DataGrid.RowDetailsVisibilityModeProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowDetailsVisibilityModeProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the row details can scroll horizontally.</summary>
		/// <returns>
		///     <see langword="true" /> if row details cannot scroll; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x0013C982 File Offset: 0x0013AB82
		// (set) Token: 0x060045E4 RID: 17892 RVA: 0x0013C994 File Offset: 0x0013AB94
		public bool AreRowDetailsFrozen
		{
			get
			{
				return (bool)base.GetValue(DataGrid.AreRowDetailsFrozenProperty);
			}
			set
			{
				base.SetValue(DataGrid.AreRowDetailsFrozenProperty, value);
			}
		}

		/// <summary>Gets or sets the template that is used to display the row details.</summary>
		/// <returns>The template that is used to display the row details. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x060045E5 RID: 17893 RVA: 0x0013C9A2 File Offset: 0x0013ABA2
		// (set) Token: 0x060045E6 RID: 17894 RVA: 0x0013C9B4 File Offset: 0x0013ABB4
		public DataTemplate RowDetailsTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGrid.RowDetailsTemplateProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowDetailsTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the template selector that is used for the row details.</summary>
		/// <returns>The template selector that is used for the row details. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x060045E7 RID: 17895 RVA: 0x0013C9C2 File Offset: 0x0013ABC2
		// (set) Token: 0x060045E8 RID: 17896 RVA: 0x0013C9D4 File Offset: 0x0013ABD4
		public DataTemplateSelector RowDetailsTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGrid.RowDetailsTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGrid.RowDetailsTemplateSelectorProperty, value);
			}
		}

		/// <summary>Occurs when a new row details template is applied to a row.</summary>
		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x060045E9 RID: 17897 RVA: 0x0013C9E4 File Offset: 0x0013ABE4
		// (remove) Token: 0x060045EA RID: 17898 RVA: 0x0013CA1C File Offset: 0x0013AC1C
		public event EventHandler<DataGridRowDetailsEventArgs> LoadingRowDetails;

		/// <summary>Occurs when a row details element becomes available for reuse.</summary>
		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x060045EB RID: 17899 RVA: 0x0013CA54 File Offset: 0x0013AC54
		// (remove) Token: 0x060045EC RID: 17900 RVA: 0x0013CA8C File Offset: 0x0013AC8C
		public event EventHandler<DataGridRowDetailsEventArgs> UnloadingRowDetails;

		/// <summary>Occurs when the visibility of a row details element changes.</summary>
		// Token: 0x140000B9 RID: 185
		// (add) Token: 0x060045ED RID: 17901 RVA: 0x0013CAC4 File Offset: 0x0013ACC4
		// (remove) Token: 0x060045EE RID: 17902 RVA: 0x0013CAFC File Offset: 0x0013ACFC
		public event EventHandler<DataGridRowDetailsEventArgs> RowDetailsVisibilityChanged;

		// Token: 0x060045EF RID: 17903 RVA: 0x0013CB34 File Offset: 0x0013AD34
		internal void OnLoadingRowDetailsWrapper(DataGridRow row)
		{
			if (row != null && !row.DetailsLoaded && row.DetailsVisibility == Visibility.Visible && row.DetailsPresenter != null)
			{
				DataGridRowDetailsEventArgs e = new DataGridRowDetailsEventArgs(row, row.DetailsPresenter.DetailsElement);
				this.OnLoadingRowDetails(e);
				row.DetailsLoaded = true;
			}
		}

		// Token: 0x060045F0 RID: 17904 RVA: 0x0013CB7C File Offset: 0x0013AD7C
		internal void OnUnloadingRowDetailsWrapper(DataGridRow row)
		{
			if (row != null && row.DetailsLoaded && row.DetailsPresenter != null)
			{
				DataGridRowDetailsEventArgs e = new DataGridRowDetailsEventArgs(row, row.DetailsPresenter.DetailsElement);
				this.OnUnloadingRowDetails(e);
				row.DetailsLoaded = false;
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.LoadingRowDetails" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045F1 RID: 17905 RVA: 0x0013CBBC File Offset: 0x0013ADBC
		protected virtual void OnLoadingRowDetails(DataGridRowDetailsEventArgs e)
		{
			if (this.LoadingRowDetails != null)
			{
				this.LoadingRowDetails(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.UnloadingRowDetails" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045F2 RID: 17906 RVA: 0x0013CBD3 File Offset: 0x0013ADD3
		protected virtual void OnUnloadingRowDetails(DataGridRowDetailsEventArgs e)
		{
			if (this.UnloadingRowDetails != null)
			{
				this.UnloadingRowDetails(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.RowDetailsVisibilityChanged" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060045F3 RID: 17907 RVA: 0x0013CBEC File Offset: 0x0013ADEC
		protected internal virtual void OnRowDetailsVisibilityChanged(DataGridRowDetailsEventArgs e)
		{
			if (this.RowDetailsVisibilityChanged != null)
			{
				this.RowDetailsVisibilityChanged(this, e);
			}
			DataGridRow row = e.Row;
			this.OnLoadingRowDetailsWrapper(row);
		}

		/// <summary>Gets or sets a value that indicates whether the user can adjust the height of rows by using the mouse.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can change the height of the rows; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x060045F4 RID: 17908 RVA: 0x0013CC1C File Offset: 0x0013AE1C
		// (set) Token: 0x060045F5 RID: 17909 RVA: 0x0013CC2E File Offset: 0x0013AE2E
		public bool CanUserResizeRows
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserResizeRowsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserResizeRowsProperty, value);
			}
		}

		/// <summary>Gets or sets the margin for the new item row.</summary>
		/// <returns>The margin for the new item row.The registered default is 0. For more information about what can influence the value, see Dependency Property Value Precedence.</returns>
		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x060045F6 RID: 17910 RVA: 0x0013CC3C File Offset: 0x0013AE3C
		// (set) Token: 0x060045F7 RID: 17911 RVA: 0x0013CC4E File Offset: 0x0013AE4E
		public Thickness NewItemMargin
		{
			get
			{
				return (Thickness)base.GetValue(DataGrid.NewItemMarginProperty);
			}
			private set
			{
				base.SetValue(DataGrid.NewItemMarginPropertyKey, value);
			}
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x0013CC61 File Offset: 0x0013AE61
		private void EnqueueNewItemMarginComputation()
		{
			if (!this._newItemMarginComputationPending)
			{
				this._newItemMarginComputationPending = true;
				base.Dispatcher.BeginInvoke(new Action(delegate()
				{
					double left = 0.0;
					if (base.IsGrouping && this.InternalScrollHost != null)
					{
						ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot;
						while (containerTracking != null)
						{
							DataGridRow container = containerTracking.Container;
							if (!container.IsNewItem)
							{
								GeneralTransform generalTransform = container.TransformToAncestor(this.InternalScrollHost);
								if (generalTransform != null)
								{
									left = generalTransform.Transform(default(Point)).X;
									break;
								}
								break;
							}
							else
							{
								containerTracking = containerTracking.Next;
							}
						}
					}
					this.NewItemMargin = new Thickness(left, 0.0, 0.0, 0.0);
					this._newItemMarginComputationPending = false;
				}), DispatcherPriority.Input, new object[0]);
			}
		}

		// Token: 0x060045F9 RID: 17913 RVA: 0x0013CC91 File Offset: 0x0013AE91
		internal override void OnIsGroupingChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnIsGroupingChanged(e);
			this.EnqueueNewItemMarginComputation();
		}

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x060045FA RID: 17914 RVA: 0x0013CCA0 File Offset: 0x0013AEA0
		internal SelectedItemCollection SelectedItemCollection
		{
			get
			{
				return (SelectedItemCollection)base.SelectedItems;
			}
		}

		/// <summary>Gets the list of cells that are currently selected.</summary>
		/// <returns>The list of cells that are currently selected. </returns>
		// Token: 0x17001132 RID: 4402
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x0013CCAD File Offset: 0x0013AEAD
		public IList<DataGridCellInfo> SelectedCells
		{
			get
			{
				return this._selectedCells;
			}
		}

		// Token: 0x17001133 RID: 4403
		// (get) Token: 0x060045FC RID: 17916 RVA: 0x0013CCAD File Offset: 0x0013AEAD
		internal SelectedCellsCollection SelectedCellsInternal
		{
			get
			{
				return this._selectedCells;
			}
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.DataGrid.SelectedCells" /> collection changes.</summary>
		// Token: 0x140000BA RID: 186
		// (add) Token: 0x060045FD RID: 17917 RVA: 0x0013CCB8 File Offset: 0x0013AEB8
		// (remove) Token: 0x060045FE RID: 17918 RVA: 0x0013CCF0 File Offset: 0x0013AEF0
		public event SelectedCellsChangedEventHandler SelectedCellsChanged;

		// Token: 0x060045FF RID: 17919 RVA: 0x0013CD28 File Offset: 0x0013AF28
		internal void OnSelectedCellsChanged(NotifyCollectionChangedAction action, VirtualizedCellInfoCollection oldItems, VirtualizedCellInfoCollection newItems)
		{
			DataGridSelectionMode selectionMode = this.SelectionMode;
			DataGridSelectionUnit selectionUnit = this.SelectionUnit;
			if (!this.IsUpdatingSelectedCells && selectionUnit == DataGridSelectionUnit.FullRow)
			{
				throw new InvalidOperationException(SR.Get("DataGrid_CannotSelectCell"));
			}
			if (oldItems != null)
			{
				if (this._pendingSelectedCells != null)
				{
					VirtualizedCellInfoCollection.Xor(this._pendingSelectedCells, oldItems);
				}
				if (this._pendingUnselectedCells == null)
				{
					this._pendingUnselectedCells = oldItems;
				}
				else
				{
					this._pendingUnselectedCells.Union(oldItems);
				}
			}
			if (newItems != null)
			{
				if (this._pendingUnselectedCells != null)
				{
					VirtualizedCellInfoCollection.Xor(this._pendingUnselectedCells, newItems);
				}
				if (this._pendingSelectedCells == null)
				{
					this._pendingSelectedCells = newItems;
				}
				else
				{
					this._pendingSelectedCells.Union(newItems);
				}
			}
			if (!this.IsUpdatingSelectedCells)
			{
				using (this.UpdateSelectedCells())
				{
					if (selectionMode == DataGridSelectionMode.Single && action == NotifyCollectionChangedAction.Add && this._selectedCells.Count > 1)
					{
						this._selectedCells.RemoveAllButOne(newItems[0]);
					}
					else if (action == NotifyCollectionChangedAction.Remove && oldItems != null && selectionUnit == DataGridSelectionUnit.CellOrRowHeader)
					{
						bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
						if (!isUpdatingSelectedItems)
						{
							base.BeginUpdateSelectedItems();
						}
						try
						{
							object obj = null;
							foreach (DataGridCellInfo dataGridCellInfo in oldItems)
							{
								object item = dataGridCellInfo.Item;
								if (item != obj)
								{
									obj = item;
									if (base.SelectedItems.Contains(item))
									{
										base.SelectedItems.Remove(item);
									}
								}
							}
						}
						finally
						{
							if (!isUpdatingSelectedItems)
							{
								base.EndUpdateSelectedItems();
							}
						}
					}
				}
			}
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x0013CEBC File Offset: 0x0013B0BC
		private void NotifySelectedCellsChanged()
		{
			if ((this._pendingSelectedCells != null && this._pendingSelectedCells.Count > 0) || (this._pendingUnselectedCells != null && this._pendingUnselectedCells.Count > 0))
			{
				SelectedCellsChangedEventArgs e = new SelectedCellsChangedEventArgs(this, this._pendingSelectedCells, this._pendingUnselectedCells);
				int count = this._selectedCells.Count;
				int num = (this._pendingUnselectedCells != null) ? this._pendingUnselectedCells.Count : 0;
				int num2 = (this._pendingSelectedCells != null) ? this._pendingSelectedCells.Count : 0;
				int num3 = count - num2 + num;
				this._pendingSelectedCells = null;
				this._pendingUnselectedCells = null;
				this.OnSelectedCellsChanged(e);
				if (num3 == 0 || count == 0)
				{
					CommandManager.InvalidateRequerySuggested();
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.SelectedCellsChanged" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004601 RID: 17921 RVA: 0x0013CF70 File Offset: 0x0013B170
		protected virtual void OnSelectedCellsChanged(SelectedCellsChangedEventArgs e)
		{
			if (this.SelectedCellsChanged != null)
			{
				this.SelectedCellsChanged(this, e);
			}
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationCellSelectedEvent(e);
				}
			}
		}

		/// <summary>Represents the command that indicates the intention to select all cells in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The <see cref="P:System.Windows.Input.ApplicationCommands.SelectAll" /> command that indicates the intention to select all cells in the <see cref="T:System.Windows.Controls.DataGrid" />.</returns>
		// Token: 0x17001134 RID: 4404
		// (get) Token: 0x06004602 RID: 17922 RVA: 0x0013CFC0 File Offset: 0x0013B1C0
		public static RoutedUICommand SelectAllCommand
		{
			get
			{
				return ApplicationCommands.SelectAll;
			}
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x0013CFC8 File Offset: 0x0013B1C8
		private static void OnCanExecuteSelectAll(object sender, CanExecuteRoutedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)sender;
			e.CanExecute = (dataGrid.SelectionMode == DataGridSelectionMode.Extended && dataGrid.IsEnabled);
			e.Handled = true;
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x0013CFFC File Offset: 0x0013B1FC
		private static void OnExecutedSelectAll(object sender, ExecutedRoutedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)sender;
			if (dataGrid.SelectionUnit == DataGridSelectionUnit.Cell)
			{
				dataGrid.SelectAllCells();
			}
			else
			{
				dataGrid.SelectAll();
			}
			e.Handled = true;
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x0013D030 File Offset: 0x0013B230
		internal override void SelectAllImpl()
		{
			int count = base.Items.Count;
			int count2 = this._columns.Count;
			if (count2 > 0 && count > 0)
			{
				using (this.UpdateSelectedCells())
				{
					this._selectedCells.AddRegion(0, 0, count, count2);
					base.SelectAllImpl();
				}
			}
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x0013D098 File Offset: 0x0013B298
		internal void SelectOnlyThisCell(DataGridCellInfo currentCellInfo)
		{
			using (this.UpdateSelectedCells())
			{
				this._selectedCells.Clear();
				this._selectedCells.Add(currentCellInfo);
			}
		}

		/// <summary>Selects all the cells in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		// Token: 0x06004607 RID: 17927 RVA: 0x0013D0E0 File Offset: 0x0013B2E0
		public void SelectAllCells()
		{
			if (this.SelectionUnit == DataGridSelectionUnit.FullRow)
			{
				base.SelectAll();
				return;
			}
			int count = base.Items.Count;
			int count2 = this._columns.Count;
			if (count > 0 && count2 > 0)
			{
				using (this.UpdateSelectedCells())
				{
					if (this._selectedCells.Count > 0)
					{
						this._selectedCells.Clear();
					}
					this._selectedCells.AddRegion(0, 0, count, count2);
				}
			}
		}

		/// <summary>Unselects all the cells in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		// Token: 0x06004608 RID: 17928 RVA: 0x0013D168 File Offset: 0x0013B368
		public void UnselectAllCells()
		{
			using (this.UpdateSelectedCells())
			{
				this._selectedCells.Clear();
				if (this.SelectionUnit != DataGridSelectionUnit.Cell)
				{
					base.UnselectAll();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates how rows and cells are selected in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>One of the enumeration values that specifies how rows and cells are selected in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see cref="F:System.Windows.Controls.DataGridSelectionMode.Extended" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001135 RID: 4405
		// (get) Token: 0x06004609 RID: 17929 RVA: 0x0013D1B4 File Offset: 0x0013B3B4
		// (set) Token: 0x0600460A RID: 17930 RVA: 0x0013D1C6 File Offset: 0x0013B3C6
		public DataGridSelectionMode SelectionMode
		{
			get
			{
				return (DataGridSelectionMode)base.GetValue(DataGrid.SelectionModeProperty);
			}
			set
			{
				base.SetValue(DataGrid.SelectionModeProperty, value);
			}
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x0013D1DC File Offset: 0x0013B3DC
		private static void OnSelectionModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridSelectionMode dataGridSelectionMode = (DataGridSelectionMode)e.NewValue;
			bool flag = dataGridSelectionMode == DataGridSelectionMode.Single;
			DataGridSelectionUnit selectionUnit = dataGrid.SelectionUnit;
			if (flag && selectionUnit == DataGridSelectionUnit.Cell)
			{
				using (dataGrid.UpdateSelectedCells())
				{
					dataGrid._selectedCells.RemoveAllButOne();
				}
			}
			dataGrid.CanSelectMultipleItems = (dataGridSelectionMode > DataGridSelectionMode.Single);
			if (flag && selectionUnit == DataGridSelectionUnit.CellOrRowHeader)
			{
				if (dataGrid.SelectedItems.Count > 0)
				{
					using (dataGrid.UpdateSelectedCells())
					{
						dataGrid._selectedCells.RemoveAllButOneRow(dataGrid.InternalSelectedInfo.Index);
						return;
					}
				}
				using (dataGrid.UpdateSelectedCells())
				{
					dataGrid._selectedCells.RemoveAllButOne();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether rows, cells, or both can be selected in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>One of the enumeration values that specifies whether rows, cells, or both can be selected in the <see cref="T:System.Windows.Controls.DataGrid" />. The registered default is <see cref="F:System.Windows.Controls.DataGridSelectionUnit.FullRow" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001136 RID: 4406
		// (get) Token: 0x0600460C RID: 17932 RVA: 0x0013D2C4 File Offset: 0x0013B4C4
		// (set) Token: 0x0600460D RID: 17933 RVA: 0x0013D2D6 File Offset: 0x0013B4D6
		public DataGridSelectionUnit SelectionUnit
		{
			get
			{
				return (DataGridSelectionUnit)base.GetValue(DataGrid.SelectionUnitProperty);
			}
			set
			{
				base.SetValue(DataGrid.SelectionUnitProperty, value);
			}
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x0013D2EC File Offset: 0x0013B4EC
		private static void OnSelectionUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			DataGridSelectionUnit dataGridSelectionUnit = (DataGridSelectionUnit)e.OldValue;
			if (dataGridSelectionUnit != DataGridSelectionUnit.Cell)
			{
				dataGrid.UnselectAll();
			}
			if (dataGridSelectionUnit != DataGridSelectionUnit.FullRow)
			{
				using (dataGrid.UpdateSelectedCells())
				{
					dataGrid._selectedCells.Clear();
				}
			}
			dataGrid.CoerceValue(Selector.IsSynchronizedWithCurrentItemProperty);
		}

		/// <summary>Invoked when the selection changes.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600460F RID: 17935 RVA: 0x0013D354 File Offset: 0x0013B554
		protected override void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			if (!this.IsUpdatingSelectedCells)
			{
				using (this.UpdateSelectedCells())
				{
					int count = e.RemovedInfos.Count;
					for (int i = 0; i < count; i++)
					{
						ItemsControl.ItemInfo rowInfo = e.RemovedInfos[i];
						this.UpdateSelectionOfCellsInRow(rowInfo, false);
					}
					count = e.AddedInfos.Count;
					for (int j = 0; j < count; j++)
					{
						ItemsControl.ItemInfo rowInfo2 = e.AddedInfos[j];
						this.UpdateSelectionOfCellsInRow(rowInfo2, true);
					}
				}
			}
			CommandManager.InvalidateRequerySuggested();
			if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementSelected) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection) || AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
			{
				DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(this) as DataGridAutomationPeer;
				if (dataGridAutomationPeer != null)
				{
					dataGridAutomationPeer.RaiseAutomationSelectionEvents(e);
				}
			}
			base.OnSelectionChanged(e);
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x0013D42C File Offset: 0x0013B62C
		private void UpdateIsSelected()
		{
			this.UpdateIsSelected(this._pendingUnselectedCells, false);
			this.UpdateIsSelected(this._pendingSelectedCells, true);
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x0013D448 File Offset: 0x0013B648
		private void UpdateIsSelected(VirtualizedCellInfoCollection cells, bool isSelected)
		{
			if (cells != null)
			{
				int count = cells.Count;
				if (count > 0)
				{
					bool flag = false;
					if (count > 750)
					{
						int num = 0;
						int count2 = this._columns.Count;
						for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
						{
							num += count2;
							if (num >= count)
							{
								break;
							}
						}
						flag = (count > num);
					}
					if (flag)
					{
						for (ContainerTracking<DataGridRow> containerTracking2 = this._rowTrackingRoot; containerTracking2 != null; containerTracking2 = containerTracking2.Next)
						{
							DataGridRow container = containerTracking2.Container;
							DataGridCellsPresenter cellsPresenter = container.CellsPresenter;
							if (cellsPresenter != null)
							{
								for (ContainerTracking<DataGridCell> containerTracking3 = cellsPresenter.CellTrackingRoot; containerTracking3 != null; containerTracking3 = containerTracking3.Next)
								{
									DataGridCell container2 = containerTracking3.Container;
									DataGridCellInfo cell = new DataGridCellInfo(container2);
									if (cells.Contains(cell))
									{
										container2.SyncIsSelected(isSelected);
									}
								}
							}
						}
						return;
					}
					foreach (DataGridCellInfo info in cells)
					{
						DataGridCell dataGridCell = this.TryFindCell(info);
						if (dataGridCell != null)
						{
							dataGridCell.SyncIsSelected(isSelected);
						}
					}
				}
			}
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x0013D564 File Offset: 0x0013B764
		private void UpdateSelectionOfCellsInRow(ItemsControl.ItemInfo rowInfo, bool isSelected)
		{
			int count = this._columns.Count;
			if (count > 0)
			{
				if (!isSelected && this._pendingInfos != null)
				{
					this._pendingInfos.Remove(rowInfo);
				}
				int index = rowInfo.Index;
				if (index >= 0)
				{
					if (isSelected)
					{
						this._selectedCells.AddRegion(index, 0, 1, count);
						return;
					}
					this._selectedCells.RemoveRegion(index, 0, 1, count);
					return;
				}
				else if (isSelected)
				{
					this.EnsurePendingInfos();
					this._pendingInfos.Add(rowInfo);
				}
			}
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x0013D5DC File Offset: 0x0013B7DC
		private void EnsurePendingInfos()
		{
			if (this._pendingInfos == null)
			{
				this._pendingInfos = new List<ItemsControl.ItemInfo>();
			}
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x0013D5F4 File Offset: 0x0013B7F4
		internal void CellIsSelectedChanged(DataGridCell cell, bool isSelected)
		{
			if (!this.IsUpdatingSelectedCells)
			{
				DataGridCellInfo cell2 = new DataGridCellInfo(cell);
				if (isSelected)
				{
					this._selectedCells.AddValidatedCell(cell2);
					return;
				}
				if (this._selectedCells.Contains(cell2))
				{
					this._selectedCells.Remove(cell2);
				}
			}
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x0013D63C File Offset: 0x0013B83C
		internal void HandleSelectionForCellInput(DataGridCell cell, bool startDragging, bool allowsExtendSelect, bool allowsMinimalSelect)
		{
			DataGridSelectionUnit selectionUnit = this.SelectionUnit;
			if (selectionUnit == DataGridSelectionUnit.FullRow)
			{
				this.MakeFullRowSelection(base.ItemInfoFromContainer(cell.RowOwner), allowsExtendSelect, allowsMinimalSelect);
			}
			else
			{
				this.MakeCellSelection(new DataGridCellInfo(cell), allowsExtendSelect, allowsMinimalSelect);
			}
			if (startDragging)
			{
				this.BeginDragging();
			}
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x0013D684 File Offset: 0x0013B884
		internal void HandleSelectionForRowHeaderAndDetailsInput(DataGridRow row, bool startDragging)
		{
			ItemsControl.ItemInfo itemInfo = base.ItemInfoFromContainer(row);
			if (!this._isDraggingSelection && this._columns.Count > 0)
			{
				if (!base.IsKeyboardFocusWithin)
				{
					base.Focus();
				}
				if (this.CurrentCell.ItemInfo != itemInfo)
				{
					base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, new DataGridCellInfo(itemInfo, this.ColumnFromDisplayIndex(0), this));
				}
				else if (this._currentCellContainer != null && this._currentCellContainer.IsEditing)
				{
					this.EndEdit(DataGrid.CommitEditCommand, this._currentCellContainer, DataGridEditingUnit.Cell, true);
				}
			}
			if (this.CanSelectRows)
			{
				this.MakeFullRowSelection(itemInfo, true, true);
				if (startDragging)
				{
					this.BeginRowDragging();
				}
			}
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x0013D737 File Offset: 0x0013B937
		private void BeginRowDragging()
		{
			this.BeginDragging();
			this._isRowDragging = true;
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x0013D746 File Offset: 0x0013B946
		private void BeginDragging()
		{
			if (Mouse.Capture(this, CaptureMode.SubTree))
			{
				this._isDraggingSelection = true;
				this._dragPoint = Mouse.GetPosition(this);
			}
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x0013D764 File Offset: 0x0013B964
		private void EndDragging()
		{
			this.StopAutoScroll();
			if (Mouse.Captured == this)
			{
				base.ReleaseMouseCapture();
			}
			this._isDraggingSelection = false;
			this._isRowDragging = false;
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x0013D788 File Offset: 0x0013B988
		private void MakeFullRowSelection(ItemsControl.ItemInfo info, bool allowsExtendSelect, bool allowsMinimalSelect)
		{
			bool flag = allowsExtendSelect && this.ShouldExtendSelection;
			bool flag2 = allowsMinimalSelect && DataGrid.ShouldMinimallyModifySelection;
			using (this.UpdateSelectedCells())
			{
				bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
				if (!isUpdatingSelectedItems)
				{
					base.BeginUpdateSelectedItems();
				}
				try
				{
					if (flag)
					{
						int count = this._columns.Count;
						if (count > 0)
						{
							int num = this._selectionAnchor.Value.ItemInfo.Index;
							int num2 = info.Index;
							if (num > num2)
							{
								int num3 = num;
								num = num2;
								num2 = num3;
							}
							if (num >= 0 && num2 >= 0)
							{
								int count2 = this._selectedItems.Count;
								if (!flag2)
								{
									bool flag3 = false;
									for (int i = 0; i < count2; i++)
									{
										ItemsControl.ItemInfo itemInfo = this._selectedItems[i];
										int index = itemInfo.Index;
										if (index < num || num2 < index)
										{
											base.SelectionChange.Unselect(itemInfo);
											if (!flag3)
											{
												this._selectedCells.Clear();
												flag3 = true;
											}
										}
									}
								}
								else
								{
									int index2 = this.CurrentCell.ItemInfo.Index;
									int num4 = -1;
									int num5 = -1;
									if (index2 < num)
									{
										num4 = index2;
										num5 = num - 1;
									}
									else if (index2 > num2)
									{
										num4 = num2 + 1;
										num5 = index2;
									}
									if (num4 >= 0 && num5 >= 0)
									{
										for (int j = 0; j < count2; j++)
										{
											ItemsControl.ItemInfo itemInfo2 = this._selectedItems[j];
											int index3 = itemInfo2.Index;
											if (num4 <= index3 && index3 <= num5)
											{
												base.SelectionChange.Unselect(itemInfo2);
											}
										}
										this._selectedCells.RemoveRegion(num4, 0, num5 - num4 + 1, this.Columns.Count);
									}
								}
								IEnumerator enumerator = ((IEnumerable)base.Items).GetEnumerator();
								int num6 = 0;
								while (num6 <= num2 && enumerator.MoveNext())
								{
									if (num6 >= num)
									{
										base.SelectionChange.Select(base.ItemInfoFromIndex(num6), true);
									}
									num6++;
								}
								IDisposable disposable2 = enumerator as IDisposable;
								if (disposable2 != null)
								{
									disposable2.Dispose();
								}
								this._selectedCells.AddRegion(num, 0, num2 - num + 1, this._columns.Count);
							}
						}
					}
					else
					{
						if (flag2 && this._selectedItems.Contains(info))
						{
							this.UnselectItem(info);
						}
						else
						{
							if (!flag2 || !base.CanSelectMultipleItems)
							{
								if (this._selectedCells.Count > 0)
								{
									this._selectedCells.Clear();
								}
								if (base.SelectedItems.Count > 0)
								{
									base.SelectedItems.Clear();
								}
							}
							if (this._editingRowInfo == info)
							{
								int count3 = this._columns.Count;
								if (count3 > 0)
								{
									this._selectedCells.AddRegion(this._editingRowInfo.Index, 0, 1, count3);
								}
								this.SelectItem(info, false);
							}
							else
							{
								this.SelectItem(info);
							}
						}
						this._selectionAnchor = new DataGridCellInfo?(new DataGridCellInfo(info.Clone(), this.ColumnFromDisplayIndex(0), this));
					}
				}
				finally
				{
					if (!isUpdatingSelectedItems)
					{
						base.EndUpdateSelectedItems();
					}
				}
			}
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x0013DAC8 File Offset: 0x0013BCC8
		private void MakeCellSelection(DataGridCellInfo cellInfo, bool allowsExtendSelect, bool allowsMinimalSelect)
		{
			bool flag = allowsExtendSelect && this.ShouldExtendSelection;
			bool flag2 = allowsMinimalSelect && DataGrid.ShouldMinimallyModifySelection;
			using (this.UpdateSelectedCells())
			{
				int displayIndex = cellInfo.Column.DisplayIndex;
				if (flag)
				{
					ItemCollection items = base.Items;
					int index = this._selectionAnchor.Value.ItemInfo.Index;
					int index2 = cellInfo.ItemInfo.Index;
					DataGridColumn column = this._selectionAnchor.Value.Column;
					int displayIndex2 = column.DisplayIndex;
					int num = displayIndex;
					if (index >= 0 && index2 >= 0 && displayIndex2 >= 0 && num >= 0)
					{
						int num2 = Math.Abs(index2 - index) + 1;
						int num3 = Math.Abs(num - displayIndex2) + 1;
						if (!flag2)
						{
							if (base.SelectedItems.Count > 0)
							{
								base.UnselectAll();
							}
							this._selectedCells.Clear();
						}
						else
						{
							int index3 = this.CurrentCell.ItemInfo.Index;
							int displayIndex3 = this.CurrentCell.Column.DisplayIndex;
							int num4 = Math.Min(index, index3);
							int num5 = Math.Abs(index3 - index) + 1;
							int columnIndex = Math.Min(displayIndex2, displayIndex3);
							int num6 = Math.Abs(displayIndex3 - displayIndex2) + 1;
							this._selectedCells.RemoveRegion(num4, columnIndex, num5, num6);
							if (this.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader)
							{
								int num7 = num4;
								int num8 = num4 + num5 - 1;
								if (num6 <= num3)
								{
									if (num5 > num2)
									{
										int num9 = num5 - num2;
										num7 = ((num4 == index3) ? index3 : (index3 - num9 + 1));
										num8 = num7 + num9 - 1;
									}
									else
									{
										num8 = num7 - 1;
									}
								}
								for (int i = num7; i <= num8; i++)
								{
									object value = base.Items[i];
									if (base.SelectedItems.Contains(value))
									{
										base.SelectedItems.Remove(value);
									}
								}
							}
						}
						this._selectedCells.AddRegion(Math.Min(index, index2), Math.Min(displayIndex2, num), num2, num3);
					}
				}
				else
				{
					bool flag3 = this._selectedCells.Contains(cellInfo);
					bool flag4 = this._editingRowInfo != null && this._editingRowInfo.Index == cellInfo.ItemInfo.Index;
					if (!flag3 && flag4)
					{
						flag3 = this._selectedCells.Contains(this._editingRowInfo.Index, displayIndex);
					}
					if (flag2 && flag3)
					{
						if (flag4)
						{
							this._selectedCells.RemoveRegion(this._editingRowInfo.Index, displayIndex, 1, 1);
						}
						else
						{
							this._selectedCells.Remove(cellInfo);
						}
						if (this.SelectionUnit == DataGridSelectionUnit.CellOrRowHeader && base.SelectedItems.Contains(cellInfo.Item))
						{
							base.SelectedItems.Remove(cellInfo.Item);
						}
					}
					else
					{
						if (!flag2 || !base.CanSelectMultipleItems)
						{
							if (base.SelectedItems.Count > 0)
							{
								base.UnselectAll();
							}
							this._selectedCells.Clear();
						}
						if (flag4)
						{
							this._selectedCells.AddRegion(this._editingRowInfo.Index, displayIndex, 1, 1);
						}
						else
						{
							this._selectedCells.AddValidatedCell(cellInfo);
						}
					}
					this._selectionAnchor = new DataGridCellInfo?(new DataGridCellInfo(cellInfo));
				}
			}
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x0013DE30 File Offset: 0x0013C030
		private void SelectItem(ItemsControl.ItemInfo info)
		{
			this.SelectItem(info, true);
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x0013DE3C File Offset: 0x0013C03C
		private void SelectItem(ItemsControl.ItemInfo info, bool selectCells)
		{
			if (selectCells)
			{
				using (this.UpdateSelectedCells())
				{
					int index = info.Index;
					int count = this._columns.Count;
					if (index >= 0 && count > 0)
					{
						this._selectedCells.AddRegion(index, 0, 1, count);
					}
				}
			}
			this.UpdateSelectedItems(info, true);
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x0013DEA4 File Offset: 0x0013C0A4
		private void UnselectItem(ItemsControl.ItemInfo info)
		{
			using (this.UpdateSelectedCells())
			{
				int index = info.Index;
				int count = this._columns.Count;
				if (index >= 0 && count > 0)
				{
					this._selectedCells.RemoveRegion(index, 0, 1, count);
				}
			}
			this.UpdateSelectedItems(info, false);
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x0013DF08 File Offset: 0x0013C108
		private void UpdateSelectedItems(ItemsControl.ItemInfo info, bool add)
		{
			bool isUpdatingSelectedItems = base.IsUpdatingSelectedItems;
			if (!isUpdatingSelectedItems)
			{
				base.BeginUpdateSelectedItems();
			}
			try
			{
				if (add)
				{
					this.SelectedItemCollection.Add(info.Clone());
				}
				else
				{
					this.SelectedItemCollection.Remove(info);
				}
			}
			finally
			{
				if (!isUpdatingSelectedItems)
				{
					base.EndUpdateSelectedItems();
				}
			}
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x0013DF64 File Offset: 0x0013C164
		private IDisposable UpdateSelectedCells()
		{
			return new DataGrid.ChangingSelectedCellsHelper(this);
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x0013DF6C File Offset: 0x0013C16C
		private void BeginUpdateSelectedCells()
		{
			this._updatingSelectedCells = true;
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x0013DF75 File Offset: 0x0013C175
		private void EndUpdateSelectedCells()
		{
			this.UpdateIsSelected();
			this._updatingSelectedCells = false;
			this.NotifySelectedCellsChanged();
		}

		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x06004623 RID: 17955 RVA: 0x0013DF8A File Offset: 0x0013C18A
		private bool IsUpdatingSelectedCells
		{
			get
			{
				return this._updatingSelectedCells;
			}
		}

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x06004624 RID: 17956 RVA: 0x0013DF92 File Offset: 0x0013C192
		private bool ShouldExtendSelection
		{
			get
			{
				return base.CanSelectMultipleItems && this._selectionAnchor != null && (this._isDraggingSelection || (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift);
			}
		}

		// Token: 0x17001139 RID: 4409
		// (get) Token: 0x06004625 RID: 17957 RVA: 0x0013DFBF File Offset: 0x0013C1BF
		private static bool ShouldMinimallyModifySelection
		{
			get
			{
				return (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
			}
		}

		// Token: 0x1700113A RID: 4410
		// (get) Token: 0x06004626 RID: 17958 RVA: 0x0013DFCC File Offset: 0x0013C1CC
		private bool CanSelectRows
		{
			get
			{
				DataGridSelectionUnit selectionUnit = this.SelectionUnit;
				return selectionUnit != DataGridSelectionUnit.Cell && selectionUnit - DataGridSelectionUnit.FullRow <= 1;
			}
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x0013DFF0 File Offset: 0x0013C1F0
		private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this._currentCellContainer = null;
			List<Tuple<int, int>> ranges = null;
			using (this.UpdateSelectedCells())
			{
				if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					ranges = new List<Tuple<int, int>>();
					base.LocateSelectedItems(ranges, false);
				}
				this._selectedCells.OnItemsCollectionChanged(e, ranges);
			}
			if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
			{
				using (IEnumerator enumerator = e.OldItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object item = enumerator.Current;
						this._itemAttachedStorage.ClearItem(item);
					}
					return;
				}
			}
			if (e.Action == NotifyCollectionChangedAction.Reset)
			{
				this._itemAttachedStorage.Clear();
			}
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x0013E0C0 File Offset: 0x0013C2C0
		private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.CoerceValue(DataGrid.CanUserAddRowsProperty);
			d.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
			CommandManager.InvalidateRequerySuggested();
			((DataGrid)d).UpdateVisualState();
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0013E0E8 File Offset: 0x0013C2E8
		private static void OnIsKeyboardFocusWithinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.Cells | DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows);
		}

		/// <summary>Provides class handling for the <see cref="E:System.Windows.UIElement.TextInput" /> routed event.</summary>
		/// <param name="e">The event data.</param>
		// Token: 0x0600462A RID: 17962 RVA: 0x0013E0FC File Offset: 0x0013C2FC
		protected override void OnTextInput(TextCompositionEventArgs e)
		{
			base.OnTextInput(e);
			if (!e.Handled && !string.IsNullOrEmpty(e.Text) && base.IsTextSearchEnabled)
			{
				bool flag = e.OriginalSource == this;
				if (!flag)
				{
					ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(e.OriginalSource as DependencyObject);
					flag = (itemsControl == this);
					if (!flag)
					{
						DataGridCellsPresenter dataGridCellsPresenter = itemsControl as DataGridCellsPresenter;
						if (dataGridCellsPresenter != null)
						{
							flag = (dataGridCellsPresenter.DataGridOwner == this);
						}
					}
				}
				if (flag)
				{
					TextSearch textSearch = TextSearch.EnsureInstance(this);
					if (textSearch != null)
					{
						textSearch.DoSearch(e.Text);
						e.Handled = true;
					}
				}
			}
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0013E188 File Offset: 0x0013C388
		internal override bool FocusItem(ItemsControl.ItemInfo info, ItemsControl.ItemNavigateArgs itemNavigateArgs)
		{
			object item = info.Item;
			bool result = false;
			if (item != null)
			{
				DataGridColumn currentColumn = this.CurrentColumn;
				if (currentColumn == null)
				{
					this.SetCurrentItem(item);
				}
				else
				{
					DataGridCell dataGridCell = this.TryFindCell(info, currentColumn);
					if (dataGridCell != null)
					{
						dataGridCell.Focus();
						if (this.ShouldSelectRowHeader)
						{
							this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell.RowOwner, false);
						}
						else
						{
							this.HandleSelectionForCellInput(dataGridCell, false, false, false);
						}
					}
				}
			}
			if (itemNavigateArgs.DeviceUsed is KeyboardDevice)
			{
				KeyboardNavigation.ShowFocusVisual();
			}
			return result;
		}

		/// <summary>Invoked when the <see cref="E:System.Windows.UIElement.KeyDown" /> event is received.</summary>
		/// <param name="e">The Information about the event.</param>
		// Token: 0x0600462C RID: 17964 RVA: 0x0013E1FC File Offset: 0x0013C3FC
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Key key = e.Key;
			if (key != Key.Tab)
			{
				if (key != Key.Return)
				{
					switch (key)
					{
					case Key.Prior:
					case Key.Next:
						this.OnPageUpOrDownKeyDown(e);
						break;
					case Key.End:
					case Key.Home:
						this.OnHomeOrEndKeyDown(e);
						break;
					case Key.Left:
					case Key.Up:
					case Key.Right:
					case Key.Down:
						this.OnArrowKeyDown(e);
						break;
					}
				}
				else
				{
					this.OnEnterKeyDown(e);
				}
			}
			else
			{
				this.OnTabKeyDown(e);
			}
			if (!e.Handled)
			{
				base.OnKeyDown(e);
			}
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x0013E27D File Offset: 0x0013C47D
		private static FocusNavigationDirection KeyToTraversalDirection(Key key)
		{
			switch (key)
			{
			case Key.Left:
				return FocusNavigationDirection.Left;
			case Key.Up:
				return FocusNavigationDirection.Up;
			case Key.Right:
				return FocusNavigationDirection.Right;
			}
			return FocusNavigationDirection.Down;
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x0013E2A4 File Offset: 0x0013C4A4
		private void OnArrowKeyDown(KeyEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null)
			{
				e.Handled = true;
				bool isEditing = currentCellContainer.IsEditing;
				KeyboardNavigation keyboardNavigation = KeyboardNavigation.Current;
				UIElement uielement = Keyboard.FocusedElement as UIElement;
				ContentElement contentElement = (uielement == null) ? (Keyboard.FocusedElement as ContentElement) : null;
				if (uielement != null || contentElement != null)
				{
					bool flag = e.OriginalSource == currentCellContainer;
					if (flag)
					{
						KeyboardNavigationMode directionalNavigation = KeyboardNavigation.GetDirectionalNavigation(this);
						if (directionalNavigation == KeyboardNavigationMode.Once)
						{
							DependencyObject dependencyObject = this.PredictFocus(DataGrid.KeyToTraversalDirection(e.Key));
							if (dependencyObject != null && !keyboardNavigation.IsAncestorOfEx(this, dependencyObject))
							{
								Keyboard.Focus(dependencyObject as IInputElement);
							}
							return;
						}
						int displayIndex = this.CurrentColumn.DisplayIndex;
						ItemsControl.ItemInfo currentInfo = this.CurrentInfo;
						int index = currentInfo.Index;
						int i = displayIndex;
						int num = index;
						bool flag2 = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
						if (!flag2 && (e.Key == Key.Up || e.Key == Key.Down))
						{
							bool flag3 = false;
							if (currentInfo.Item == CollectionView.NewItemPlaceholder)
							{
								flag3 = true;
							}
							else if (base.IsGrouping)
							{
								GroupItem groupItem = DataGridHelper.FindVisualParent<GroupItem>(currentCellContainer);
								if (groupItem != null)
								{
									CollectionViewGroupInternal collectionViewGroupInternal = base.ItemContainerGenerator.ItemFromContainer(groupItem) as CollectionViewGroupInternal;
									if (collectionViewGroupInternal != null && collectionViewGroupInternal.Items.Count > 0 && ((e.Key == Key.Up && ItemsControl.EqualsEx(collectionViewGroupInternal.Items[0], currentInfo.Item)) || (e.Key == Key.Down && ItemsControl.EqualsEx(collectionViewGroupInternal.Items[collectionViewGroupInternal.Items.Count - 1], currentInfo.Item))))
									{
										int num2 = collectionViewGroupInternal.LeafIndexFromItem(null, 0);
										if (e.Key == Key.Down)
										{
											num2 += collectionViewGroupInternal.ItemCount - 1;
										}
										if (index == num2)
										{
											flag3 = true;
										}
									}
								}
							}
							else if ((e.Key == Key.Up && index == 0) || (e.Key == Key.Down && index == base.Items.Count - 1))
							{
								flag3 = true;
							}
							if (flag3 && this.TryDefaultNavigation(e, currentInfo))
							{
								return;
							}
						}
						Key key = e.Key;
						if (base.FlowDirection == FlowDirection.RightToLeft)
						{
							if (key == Key.Left)
							{
								key = Key.Right;
							}
							else if (key == Key.Right)
							{
								key = Key.Left;
							}
						}
						switch (key)
						{
						case Key.Left:
							if (flag2)
							{
								i = this.InternalColumns.FirstVisibleDisplayIndex;
								goto IL_4BA;
							}
							for (i--; i >= 0; i--)
							{
								DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
								if (dataGridColumn.IsVisible)
								{
									break;
								}
							}
							if (i >= 0)
							{
								goto IL_4BA;
							}
							if (directionalNavigation == KeyboardNavigationMode.Cycle)
							{
								i = this.InternalColumns.LastVisibleDisplayIndex;
								goto IL_4BA;
							}
							if (directionalNavigation == KeyboardNavigationMode.Contained)
							{
								DependencyObject dependencyObject2 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
								if (dependencyObject2 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject2))
								{
									Keyboard.Focus(dependencyObject2 as IInputElement);
								}
								return;
							}
							this.MoveFocus(new TraversalRequest((e.Key == Key.Left) ? FocusNavigationDirection.Left : FocusNavigationDirection.Right));
							return;
						case Key.Up:
							if (flag2)
							{
								num = 0;
								goto IL_4BA;
							}
							num--;
							if (num >= 0)
							{
								goto IL_4BA;
							}
							if (directionalNavigation == KeyboardNavigationMode.Cycle)
							{
								num = base.Items.Count - 1;
								goto IL_4BA;
							}
							if (directionalNavigation == KeyboardNavigationMode.Contained)
							{
								DependencyObject dependencyObject3 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
								if (dependencyObject3 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject3))
								{
									Keyboard.Focus(dependencyObject3 as IInputElement);
								}
								return;
							}
							this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
							return;
						case Key.Right:
						{
							if (flag2)
							{
								i = Math.Max(0, this.InternalColumns.LastVisibleDisplayIndex);
								goto IL_4BA;
							}
							i++;
							int count = this.Columns.Count;
							while (i < count)
							{
								DataGridColumn dataGridColumn2 = this.ColumnFromDisplayIndex(i);
								if (dataGridColumn2.IsVisible)
								{
									break;
								}
								i++;
							}
							if (i < this.Columns.Count)
							{
								goto IL_4BA;
							}
							if (directionalNavigation == KeyboardNavigationMode.Cycle)
							{
								i = this.InternalColumns.FirstVisibleDisplayIndex;
								goto IL_4BA;
							}
							if (directionalNavigation == KeyboardNavigationMode.Contained)
							{
								DependencyObject dependencyObject4 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
								if (dependencyObject4 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject4))
								{
									Keyboard.Focus(dependencyObject4 as IInputElement);
								}
								return;
							}
							this.MoveFocus(new TraversalRequest((e.Key == Key.Left) ? FocusNavigationDirection.Left : FocusNavigationDirection.Right));
							return;
						}
						}
						if (flag2)
						{
							num = Math.Max(0, base.Items.Count - 1);
						}
						else
						{
							num++;
							if (num >= base.Items.Count)
							{
								if (directionalNavigation == KeyboardNavigationMode.Cycle)
								{
									num = 0;
								}
								else
								{
									if (directionalNavigation == KeyboardNavigationMode.Contained)
									{
										DependencyObject dependencyObject5 = keyboardNavigation.PredictFocusedElement(currentCellContainer, DataGrid.KeyToTraversalDirection(key), false, false);
										if (dependencyObject5 != null && keyboardNavigation.IsAncestorOfEx(this, dependencyObject5))
										{
											Keyboard.Focus(dependencyObject5 as IInputElement);
										}
										return;
									}
									this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
									return;
								}
							}
						}
						IL_4BA:
						DataGridColumn column = this.ColumnFromDisplayIndex(i);
						ItemsControl.ItemInfo info = base.ItemInfoFromIndex(num);
						this.ScrollCellIntoView(info, column);
						DataGridCell dataGridCell = this.TryFindCell(info, column);
						if (dataGridCell == null || dataGridCell == currentCellContainer || !dataGridCell.Focus())
						{
							return;
						}
					}
					else if (this.TryDefaultNavigation(e, null))
					{
						return;
					}
					TraversalRequest request = new TraversalRequest(DataGrid.KeyToTraversalDirection(e.Key));
					if (flag || (uielement != null && uielement.MoveFocus(request)) || (contentElement != null && contentElement.MoveFocus(request)))
					{
						this.SelectAndEditOnFocusMove(e, currentCellContainer, isEditing, true, true);
					}
				}
			}
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x0013E7F0 File Offset: 0x0013C9F0
		private bool TryDefaultNavigation(KeyEventArgs e, ItemsControl.ItemInfo currentInfo)
		{
			FrameworkElement frameworkElement = Keyboard.FocusedElement as FrameworkElement;
			if (frameworkElement != null && base.ItemsHost.IsAncestorOf(frameworkElement))
			{
				FrameworkElement frameworkElement2;
				base.PrepareNavigateByLine(currentInfo, frameworkElement, (e.Key == Key.Up) ? FocusNavigationDirection.Up : FocusNavigationDirection.Down, new ItemsControl.ItemNavigateArgs(e.KeyboardDevice, Keyboard.Modifiers), out frameworkElement2);
				if (frameworkElement2 != null)
				{
					DataGridRow dataGridRow = DataGridHelper.FindVisualParent<DataGridRow>(frameworkElement2);
					if (dataGridRow == null || dataGridRow.DataGridOwner != this)
					{
						frameworkElement2.Focus();
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x0013E864 File Offset: 0x0013CA64
		private void OnTabKeyDown(KeyEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null)
			{
				bool isEditing = currentCellContainer.IsEditing;
				bool flag = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
				UIElement uielement = Keyboard.FocusedElement as UIElement;
				ContentElement contentElement = (uielement == null) ? (Keyboard.FocusedElement as ContentElement) : null;
				if (uielement != null || contentElement != null)
				{
					e.Handled = true;
					FocusNavigationDirection focusNavigationDirection = flag ? FocusNavigationDirection.Previous : FocusNavigationDirection.Next;
					TraversalRequest request = new TraversalRequest(focusNavigationDirection);
					if ((uielement != null && uielement.MoveFocus(request)) || (contentElement != null && contentElement.MoveFocus(request)))
					{
						if (isEditing && flag && Keyboard.FocusedElement == currentCellContainer)
						{
							currentCellContainer.MoveFocus(request);
						}
						if (base.IsGrouping && isEditing)
						{
							DataGridCell cellForSelectAndEditOnFocusMove = this.GetCellForSelectAndEditOnFocusMove();
							if (cellForSelectAndEditOnFocusMove != null && cellForSelectAndEditOnFocusMove.RowDataItem == currentCellContainer.RowDataItem)
							{
								DataGridCell dataGridCell = this.TryFindCell(cellForSelectAndEditOnFocusMove.RowDataItem, cellForSelectAndEditOnFocusMove.Column);
								if (dataGridCell == null)
								{
									base.UpdateLayout();
									dataGridCell = this.TryFindCell(cellForSelectAndEditOnFocusMove.RowDataItem, cellForSelectAndEditOnFocusMove.Column);
								}
								if (dataGridCell != null && dataGridCell != cellForSelectAndEditOnFocusMove)
								{
									dataGridCell.Focus();
								}
							}
						}
						this.SelectAndEditOnFocusMove(e, currentCellContainer, isEditing, false, true);
					}
				}
			}
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x0013E988 File Offset: 0x0013CB88
		private void OnEnterKeyDown(KeyEventArgs e)
		{
			DataGridCell currentCellContainer = this.CurrentCellContainer;
			if (currentCellContainer != null && this._columns.Count > 0)
			{
				e.Handled = true;
				DataGridColumn column = currentCellContainer.Column;
				if (this.CommitAnyEdit() && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					bool flag = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
					int count = base.Items.Count;
					int num = this.CurrentInfo.Index;
					if (num < 0)
					{
						num = base.Items.IndexOf(this.CurrentItem);
					}
					num = Math.Max(0, Math.Min(count - 1, num + (flag ? -1 : 1)));
					if (num < count)
					{
						ItemsControl.ItemInfo itemInfo = base.ItemInfoFromIndex(num);
						this.ScrollIntoView(itemInfo, column);
						if (!ItemsControl.EqualsEx(this.CurrentCell.Item, itemInfo.Item))
						{
							base.SetCurrentValueInternal(DataGrid.CurrentCellProperty, new DataGridCellInfo(itemInfo, column, this));
							this.SelectAndEditOnFocusMove(e, currentCellContainer, false, false, true);
							return;
						}
						currentCellContainer = this.CurrentCellContainer;
						if (currentCellContainer != null)
						{
							currentCellContainer.Focus();
						}
					}
				}
			}
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x0013EAA8 File Offset: 0x0013CCA8
		private DataGridCell GetCellForSelectAndEditOnFocusMove()
		{
			DataGridCell dataGridCell = Keyboard.FocusedElement as DataGridCell;
			if (dataGridCell == null && this.CurrentCellContainer != null && this.CurrentCellContainer.IsKeyboardFocusWithin)
			{
				dataGridCell = this.CurrentCellContainer;
			}
			return dataGridCell;
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x0013EAE0 File Offset: 0x0013CCE0
		private void SelectAndEditOnFocusMove(KeyEventArgs e, DataGridCell oldCell, bool wasEditing, bool allowsExtendSelect, bool ignoreControlKey)
		{
			DataGridCell cellForSelectAndEditOnFocusMove = this.GetCellForSelectAndEditOnFocusMove();
			if (cellForSelectAndEditOnFocusMove != null && cellForSelectAndEditOnFocusMove.DataGridOwner == this)
			{
				if (ignoreControlKey || (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.None)
				{
					if (this.ShouldSelectRowHeader && allowsExtendSelect)
					{
						this.HandleSelectionForRowHeaderAndDetailsInput(cellForSelectAndEditOnFocusMove.RowOwner, false);
					}
					else
					{
						this.HandleSelectionForCellInput(cellForSelectAndEditOnFocusMove, false, allowsExtendSelect, false);
					}
				}
				if (wasEditing && !cellForSelectAndEditOnFocusMove.IsEditing && oldCell.RowDataItem == cellForSelectAndEditOnFocusMove.RowDataItem)
				{
					this.BeginEdit(e);
				}
			}
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x0013EB5C File Offset: 0x0013CD5C
		private void OnHomeOrEndKeyDown(KeyEventArgs e)
		{
			if (this._columns.Count > 0 && base.Items.Count > 0)
			{
				e.Handled = true;
				bool flag = e.Key == Key.Home;
				bool flag2 = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
				if (flag2)
				{
					ScrollViewer internalScrollHost = this.InternalScrollHost;
					if (internalScrollHost != null)
					{
						if (flag)
						{
							internalScrollHost.ScrollToHome();
						}
						else
						{
							internalScrollHost.ScrollToEnd();
						}
					}
				}
				ItemsControl.ItemInfo info = flag2 ? base.ItemInfoFromIndex(flag ? 0 : (base.Items.Count - 1)) : this.CurrentInfo;
				DataGridColumn column = this.ColumnFromDisplayIndex(flag ? this.InternalColumns.FirstVisibleDisplayIndex : this.InternalColumns.LastVisibleDisplayIndex);
				this.ScrollCellIntoView(info, column);
				DataGridCell dataGridCell = this.TryFindCell(info, column);
				if (dataGridCell != null)
				{
					dataGridCell.Focus();
					if (this.ShouldSelectRowHeader)
					{
						this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell.RowOwner, false);
						return;
					}
					this.HandleSelectionForCellInput(dataGridCell, false, true, false);
				}
			}
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x0013EC58 File Offset: 0x0013CE58
		private void OnPageUpOrDownKeyDown(KeyEventArgs e)
		{
			ScrollViewer internalScrollHost = this.InternalScrollHost;
			if (internalScrollHost != null)
			{
				e.Handled = true;
				ItemsControl.ItemInfo currentInfo = this.CurrentInfo;
				if (VirtualizingPanel.GetScrollUnit(this) == ScrollUnit.Item && !base.IsGrouping)
				{
					int index = currentInfo.Index;
					if (index >= 0)
					{
						int num = Math.Max(1, (int)internalScrollHost.ViewportHeight - 1);
						int num2 = (e.Key == Key.Prior) ? (index - num) : (index + num);
						num2 = Math.Max(0, Math.Min(num2, base.Items.Count - 1));
						ItemsControl.ItemInfo itemInfo = base.ItemInfoFromIndex(num2);
						DataGridColumn currentColumn = this.CurrentColumn;
						if (currentColumn == null)
						{
							base.OnBringItemIntoView(itemInfo);
							this.SetCurrentItem(itemInfo.Item);
							return;
						}
						this.ScrollCellIntoView(itemInfo, currentColumn);
						DataGridCell dataGridCell = this.TryFindCell(itemInfo, currentColumn);
						if (dataGridCell != null)
						{
							dataGridCell.Focus();
							if (this.ShouldSelectRowHeader)
							{
								this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell.RowOwner, false);
								return;
							}
							this.HandleSelectionForCellInput(dataGridCell, false, true, false);
							return;
						}
					}
				}
				else
				{
					FocusNavigationDirection direction = (e.Key == Key.Prior) ? FocusNavigationDirection.Up : FocusNavigationDirection.Down;
					ItemsControl.ItemInfo startingInfo = currentInfo;
					FrameworkElement frameworkElement = null;
					if (base.IsGrouping)
					{
						frameworkElement = (Keyboard.FocusedElement as FrameworkElement);
						if (frameworkElement != null)
						{
							startingInfo = null;
							DataGridRow dataGridRow = frameworkElement as DataGridRow;
							if (dataGridRow == null)
							{
								dataGridRow = DataGridHelper.FindVisualParent<DataGridRow>(frameworkElement);
							}
							if (dataGridRow != null)
							{
								DataGrid dataGrid = ItemsControl.ItemsControlFromItemContainer(dataGridRow) as DataGrid;
								if (dataGrid == this)
								{
									startingInfo = base.ItemInfoFromContainer(dataGridRow);
								}
							}
						}
					}
					FrameworkElement frameworkElement2;
					base.PrepareToNavigateByPage(startingInfo, frameworkElement, direction, new ItemsControl.ItemNavigateArgs(Keyboard.PrimaryDevice, Keyboard.Modifiers), out frameworkElement2);
					DataGridRow dataGridRow2 = frameworkElement2 as DataGridRow;
					if (dataGridRow2 == null)
					{
						dataGridRow2 = DataGridHelper.FindVisualParent<DataGridRow>(frameworkElement2);
					}
					if (dataGridRow2 != null)
					{
						ItemsControl.ItemInfo itemInfo2 = base.ItemInfoFromContainer(dataGridRow2);
						DataGridColumn currentColumn2 = this.CurrentColumn;
						if (currentColumn2 == null)
						{
							this.SetCurrentItem(itemInfo2.Item);
							return;
						}
						DataGridCell dataGridCell2 = this.TryFindCell(itemInfo2, currentColumn2);
						if (dataGridCell2 != null)
						{
							dataGridCell2.Focus();
							if (this.ShouldSelectRowHeader)
							{
								this.HandleSelectionForRowHeaderAndDetailsInput(dataGridCell2.RowOwner, false);
								return;
							}
							this.HandleSelectionForCellInput(dataGridCell2, false, true, false);
							return;
						}
					}
					else if (frameworkElement2 != null)
					{
						frameworkElement2.Focus();
					}
				}
			}
		}

		/// <summary>Updates the collection of items that are selected due to the user dragging the mouse in the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <param name="e">The mouse data.</param>
		// Token: 0x06004636 RID: 17974 RVA: 0x0013EE64 File Offset: 0x0013D064
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this._isDraggingSelection)
			{
				if (e.LeftButton == MouseButtonState.Pressed)
				{
					Point position = Mouse.GetPosition(this);
					if (!DoubleUtil.AreClose(position, this._dragPoint))
					{
						this._dragPoint = position;
						DataGrid.RelativeMousePositions relativeMousePosition = this.RelativeMousePosition;
						if (relativeMousePosition == DataGrid.RelativeMousePositions.Over)
						{
							if (this._isRowDragging)
							{
								DataGridRow mouseOverRow = this.MouseOverRow;
								if (mouseOverRow != null && mouseOverRow.Item != this.CurrentItem)
								{
									this.HandleSelectionForRowHeaderAndDetailsInput(mouseOverRow, false);
									this.SetCurrentItem(mouseOverRow.Item);
									e.Handled = true;
									return;
								}
							}
							else
							{
								DataGridCell dataGridCell = this.MouseOverCell;
								if (dataGridCell == null)
								{
									DataGridRow mouseOverRow2 = this.MouseOverRow;
									if (mouseOverRow2 != null)
									{
										dataGridCell = this.GetCellNearMouse();
									}
								}
								if (dataGridCell != null && dataGridCell != this.CurrentCellContainer)
								{
									this.HandleSelectionForCellInput(dataGridCell, false, true, true);
									dataGridCell.Focus();
									e.Handled = true;
									return;
								}
							}
						}
						else if (this._isRowDragging && DataGrid.IsMouseToLeftOrRightOnly(relativeMousePosition))
						{
							DataGridRow rowNearMouse = this.GetRowNearMouse();
							if (rowNearMouse != null && rowNearMouse.Item != this.CurrentItem)
							{
								this.HandleSelectionForRowHeaderAndDetailsInput(rowNearMouse, false);
								this.SetCurrentItem(rowNearMouse.Item);
								e.Handled = true;
								return;
							}
						}
						else
						{
							if (!this._hasAutoScrolled)
							{
								this.StartAutoScroll();
								return;
							}
							if (this.DoAutoScroll())
							{
								e.Handled = true;
								return;
							}
						}
					}
				}
				else
				{
					this.EndDragging();
				}
			}
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x0013EFAC File Offset: 0x0013D1AC
		private static void OnAnyMouseUpThunk(object sender, MouseButtonEventArgs e)
		{
			((DataGrid)sender).OnAnyMouseUp(e);
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x0013EFBA File Offset: 0x0013D1BA
		private void OnAnyMouseUp(MouseButtonEventArgs e)
		{
			this.EndDragging();
		}

		/// <summary>Selects a cell if its context menu is opened.</summary>
		/// <param name="e">The item whose context menu was opened.</param>
		// Token: 0x06004639 RID: 17977 RVA: 0x0013EFC4 File Offset: 0x0013D1C4
		protected override void OnContextMenuOpening(ContextMenuEventArgs e)
		{
			if (!base.IsEnabled)
			{
				return;
			}
			DataGridCell dataGridCell = null;
			DataGridRowHeader dataGridRowHeader = null;
			for (UIElement uielement = e.OriginalSource as UIElement; uielement != null; uielement = (VisualTreeHelper.GetParent(uielement) as UIElement))
			{
				dataGridCell = (uielement as DataGridCell);
				if (dataGridCell != null)
				{
					break;
				}
				dataGridRowHeader = (uielement as DataGridRowHeader);
				if (dataGridRowHeader != null)
				{
					break;
				}
			}
			if (dataGridCell != null && !dataGridCell.IsSelected && !dataGridCell.IsKeyboardFocusWithin)
			{
				dataGridCell.Focus();
				this.HandleSelectionForCellInput(dataGridCell, false, true, true);
			}
			if (dataGridRowHeader != null)
			{
				DataGridRow parentRow = dataGridRowHeader.ParentRow;
				if (parentRow != null && !parentRow.IsSelected)
				{
					this.HandleSelectionForRowHeaderAndDetailsInput(parentRow, false);
				}
			}
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x0013F050 File Offset: 0x0013D250
		private DataGridRow GetRowNearMouse()
		{
			Panel internalItemsHost = this.InternalItemsHost;
			if (internalItemsHost != null)
			{
				bool isGrouping = base.IsGrouping;
				for (int i = isGrouping ? (base.Items.Count - 1) : (internalItemsHost.Children.Count - 1); i >= 0; i--)
				{
					DataGridRow dataGridRow;
					if (isGrouping)
					{
						dataGridRow = (base.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow);
					}
					else
					{
						dataGridRow = (internalItemsHost.Children[i] as DataGridRow);
					}
					if (dataGridRow != null)
					{
						Point position = Mouse.GetPosition(dataGridRow);
						Rect rect = new Rect(default(Point), dataGridRow.RenderSize);
						if (position.Y >= rect.Top && position.Y <= rect.Bottom)
						{
							return dataGridRow;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x0013F10C File Offset: 0x0013D30C
		private DataGridCell GetCellNearMouse()
		{
			Panel internalItemsHost = this.InternalItemsHost;
			if (internalItemsHost != null)
			{
				Rect itemsHostBounds = new Rect(default(Point), internalItemsHost.RenderSize);
				double num = double.PositiveInfinity;
				DataGridCell dataGridCell = null;
				bool isMouseInCorner = DataGrid.IsMouseInCorner(this.RelativeMousePosition);
				bool isGrouping = base.IsGrouping;
				for (int i = isGrouping ? (base.Items.Count - 1) : (internalItemsHost.Children.Count - 1); i >= 0; i--)
				{
					DataGridRow dataGridRow;
					if (isGrouping)
					{
						dataGridRow = (base.ItemContainerGenerator.ContainerFromIndex(i) as DataGridRow);
					}
					else
					{
						dataGridRow = (internalItemsHost.Children[i] as DataGridRow);
					}
					if (dataGridRow != null)
					{
						DataGridCellsPresenter cellsPresenter = dataGridRow.CellsPresenter;
						if (cellsPresenter != null)
						{
							for (ContainerTracking<DataGridCell> containerTracking = cellsPresenter.CellTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
							{
								DataGridCell container = containerTracking.Container;
								double num2;
								if (DataGrid.CalculateCellDistance(container, dataGridRow, internalItemsHost, itemsHostBounds, isMouseInCorner, out num2) && (dataGridCell == null || num2 < num))
								{
									num = num2;
									dataGridCell = container;
								}
							}
							DataGridRowHeader rowHeader = dataGridRow.RowHeader;
							double num3;
							if (rowHeader != null && DataGrid.CalculateCellDistance(rowHeader, dataGridRow, internalItemsHost, itemsHostBounds, isMouseInCorner, out num3) && (dataGridCell == null || num3 < num))
							{
								DataGridCell dataGridCell2 = dataGridRow.TryGetCell(this.DisplayIndexMap[0]);
								if (dataGridCell2 != null)
								{
									num = num3;
									dataGridCell = dataGridCell2;
								}
							}
						}
					}
				}
				return dataGridCell;
			}
			return null;
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0013F264 File Offset: 0x0013D464
		private static bool CalculateCellDistance(FrameworkElement cell, DataGridRow rowOwner, Panel itemsHost, Rect itemsHostBounds, bool isMouseInCorner, out double distance)
		{
			GeneralTransform generalTransform = cell.TransformToAncestor(itemsHost);
			Rect rect = new Rect(default(Point), cell.RenderSize);
			if (itemsHostBounds.Contains(generalTransform.TransformBounds(rect)))
			{
				Point position = Mouse.GetPosition(cell);
				if (isMouseInCorner)
				{
					Vector vector = new Vector(position.X - rect.Width * 0.5, position.Y - rect.Height * 0.5);
					distance = vector.Length;
					return true;
				}
				Point position2 = Mouse.GetPosition(rowOwner);
				Rect rect2 = new Rect(default(Point), rowOwner.RenderSize);
				if (position.X >= rect.Left && position.X <= rect.Right)
				{
					if (position2.Y >= rect2.Top && position2.Y <= rect2.Bottom)
					{
						distance = 0.0;
					}
					else
					{
						distance = Math.Abs(position.Y - rect.Top);
					}
					return true;
				}
				if (position2.Y >= rect2.Top && position2.Y <= rect2.Bottom)
				{
					distance = Math.Abs(position.X - rect.Left);
					return true;
				}
			}
			distance = double.PositiveInfinity;
			return false;
		}

		// Token: 0x1700113B RID: 4411
		// (get) Token: 0x0600463D RID: 17981 RVA: 0x0013F3C0 File Offset: 0x0013D5C0
		private DataGridRow MouseOverRow
		{
			get
			{
				UIElement element = Mouse.DirectlyOver as UIElement;
				DataGridRow dataGridRow = null;
				while (element != null)
				{
					dataGridRow = DataGridHelper.FindVisualParent<DataGridRow>(element);
					if (dataGridRow == null || dataGridRow.DataGridOwner == this)
					{
						break;
					}
					element = (VisualTreeHelper.GetParent(dataGridRow) as UIElement);
				}
				return dataGridRow;
			}
		}

		// Token: 0x1700113C RID: 4412
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x0013F400 File Offset: 0x0013D600
		private DataGridCell MouseOverCell
		{
			get
			{
				UIElement element = Mouse.DirectlyOver as UIElement;
				DataGridCell dataGridCell = null;
				while (element != null)
				{
					dataGridCell = DataGridHelper.FindVisualParent<DataGridCell>(element);
					if (dataGridCell == null || dataGridCell.DataGridOwner == this)
					{
						break;
					}
					element = (VisualTreeHelper.GetParent(dataGridCell) as UIElement);
				}
				return dataGridCell;
			}
		}

		// Token: 0x1700113D RID: 4413
		// (get) Token: 0x0600463F RID: 17983 RVA: 0x0013F440 File Offset: 0x0013D640
		private DataGrid.RelativeMousePositions RelativeMousePosition
		{
			get
			{
				DataGrid.RelativeMousePositions relativeMousePositions = DataGrid.RelativeMousePositions.Over;
				Panel internalItemsHost = this.InternalItemsHost;
				if (internalItemsHost != null)
				{
					Point position = Mouse.GetPosition(internalItemsHost);
					Rect rect = new Rect(default(Point), internalItemsHost.RenderSize);
					if (position.X < rect.Left)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Left;
					}
					else if (position.X > rect.Right)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Right;
					}
					if (position.Y < rect.Top)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Above;
					}
					else if (position.Y > rect.Bottom)
					{
						relativeMousePositions |= DataGrid.RelativeMousePositions.Below;
					}
				}
				return relativeMousePositions;
			}
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x0013F4CC File Offset: 0x0013D6CC
		private static bool IsMouseToLeft(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Left) == DataGrid.RelativeMousePositions.Left;
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x0013F4D4 File Offset: 0x0013D6D4
		private static bool IsMouseToRight(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Right) == DataGrid.RelativeMousePositions.Right;
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x0013F4DC File Offset: 0x0013D6DC
		private static bool IsMouseAbove(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Above) == DataGrid.RelativeMousePositions.Above;
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x0013F4E4 File Offset: 0x0013D6E4
		private static bool IsMouseBelow(DataGrid.RelativeMousePositions position)
		{
			return (position & DataGrid.RelativeMousePositions.Below) == DataGrid.RelativeMousePositions.Below;
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x0013F4EC File Offset: 0x0013D6EC
		private static bool IsMouseToLeftOrRightOnly(DataGrid.RelativeMousePositions position)
		{
			return position == DataGrid.RelativeMousePositions.Left || position == DataGrid.RelativeMousePositions.Right;
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x0013F4F8 File Offset: 0x0013D6F8
		private static bool IsMouseInCorner(DataGrid.RelativeMousePositions position)
		{
			return position != DataGrid.RelativeMousePositions.Over && position != DataGrid.RelativeMousePositions.Above && position != DataGrid.RelativeMousePositions.Below && position != DataGrid.RelativeMousePositions.Left && position != DataGrid.RelativeMousePositions.Right;
		}

		/// <summary>Returns the automation peer for this <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <returns>The automation peer for this <see cref="T:System.Windows.Controls.DataGrid" />.</returns>
		// Token: 0x06004646 RID: 17990 RVA: 0x0013F512 File Offset: 0x0013D712
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new DataGridAutomationPeer(this);
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x0013F51C File Offset: 0x0013D71C
		private DataGrid.CellAutomationValueHolder GetCellAutomationValueHolder(object item, DataGridColumn column)
		{
			DataGrid.CellAutomationValueHolder result;
			if (this._editingRowInfo == null || !ItemsControl.EqualsEx(item, this._editingRowInfo.Item) || !this._editingCellAutomationValueHolders.TryGetValue(column, out result))
			{
				DataGridCell dataGridCell = this.TryFindCell(item, column);
				result = ((dataGridCell != null) ? new DataGrid.CellAutomationValueHolder(dataGridCell) : new DataGrid.CellAutomationValueHolder(item, column));
			}
			return result;
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x0013F578 File Offset: 0x0013D778
		internal string GetCellAutomationValue(object item, DataGridColumn column)
		{
			DataGrid.CellAutomationValueHolder cellAutomationValueHolder = this.GetCellAutomationValueHolder(item, column);
			return cellAutomationValueHolder.Value;
		}

		// Token: 0x06004649 RID: 17993 RVA: 0x0013F594 File Offset: 0x0013D794
		internal object GetCellClipboardValue(object item, DataGridColumn column)
		{
			DataGrid.CellAutomationValueHolder cellAutomationValueHolder = this.GetCellAutomationValueHolder(item, column);
			return cellAutomationValueHolder.GetClipboardValue();
		}

		// Token: 0x0600464A RID: 17994 RVA: 0x0013F5B0 File Offset: 0x0013D7B0
		internal void SetCellAutomationValue(object item, DataGridColumn column, string value)
		{
			this.SetCellValue(item, column, value, false);
		}

		// Token: 0x0600464B RID: 17995 RVA: 0x0013F5BC File Offset: 0x0013D7BC
		internal void SetCellClipboardValue(object item, DataGridColumn column, object value)
		{
			this.SetCellValue(item, column, value, true);
		}

		// Token: 0x0600464C RID: 17996 RVA: 0x0013F5C8 File Offset: 0x0013D7C8
		private void SetCellValue(object item, DataGridColumn column, object value, bool clipboard)
		{
			this.CurrentCellContainer = this.TryFindCell(item, column);
			if (this.CurrentCellContainer == null)
			{
				this.ScrollCellIntoView(base.NewItemInfo(item, null, -1), column);
				this.CurrentCellContainer = this.TryFindCell(item, column);
			}
			if (this.CurrentCellContainer == null)
			{
				return;
			}
			if (this.BeginEdit())
			{
				DataGrid.CellAutomationValueHolder cellAutomationValueHolder;
				if (this._editingCellAutomationValueHolders.TryGetValue(column, out cellAutomationValueHolder))
				{
					cellAutomationValueHolder.SetValue(this, value, clipboard);
					return;
				}
				this.CancelEdit();
			}
		}

		// Token: 0x0600464D RID: 17997 RVA: 0x0013F63C File Offset: 0x0013D83C
		private void EnsureCellAutomationValueHolder(DataGridCell cell)
		{
			if (!this._editingCellAutomationValueHolders.ContainsKey(cell.Column))
			{
				this._editingCellAutomationValueHolders.Add(cell.Column, new DataGrid.CellAutomationValueHolder(cell));
			}
		}

		// Token: 0x0600464E RID: 17998 RVA: 0x0013F668 File Offset: 0x0013D868
		private void UpdateCellAutomationValueHolder(DataGridCell cell)
		{
			DataGrid.CellAutomationValueHolder cellAutomationValueHolder;
			if (this._editingCellAutomationValueHolders.TryGetValue(cell.Column, out cellAutomationValueHolder))
			{
				cellAutomationValueHolder.TrackValue();
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x0013F690 File Offset: 0x0013D890
		private void ReleaseCellAutomationValueHolders()
		{
			foreach (KeyValuePair<DataGridColumn, DataGrid.CellAutomationValueHolder> keyValuePair in this._editingCellAutomationValueHolders)
			{
				keyValuePair.Value.TrackValue();
			}
			this._editingCellAutomationValueHolders.Clear();
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x0013F6F4 File Offset: 0x0013D8F4
		internal DataGridCell TryFindCell(DataGridCellInfo info)
		{
			return this.TryFindCell(base.LeaseItemInfo(info.ItemInfo, false), info.Column);
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x0013F714 File Offset: 0x0013D914
		internal DataGridCell TryFindCell(ItemsControl.ItemInfo info, DataGridColumn column)
		{
			DataGridRow dataGridRow = (DataGridRow)info.Container;
			int num = this._columns.IndexOf(column);
			if (dataGridRow != null && num >= 0)
			{
				return dataGridRow.TryGetCell(num);
			}
			return null;
		}

		// Token: 0x06004652 RID: 18002 RVA: 0x0013F74C File Offset: 0x0013D94C
		internal DataGridCell TryFindCell(object item, DataGridColumn column)
		{
			DataGridRow dataGridRow = (DataGridRow)base.ItemContainerGenerator.ContainerFromItem(item);
			int num = this._columns.IndexOf(column);
			if (dataGridRow != null && num >= 0)
			{
				return dataGridRow.TryGetCell(num);
			}
			return null;
		}

		/// <summary>Gets or sets a value that indicates whether the user can sort columns by clicking the column header.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can sort the columns; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700113E RID: 4414
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x0013F788 File Offset: 0x0013D988
		// (set) Token: 0x06004654 RID: 18004 RVA: 0x0013F79A File Offset: 0x0013D99A
		public bool CanUserSortColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserSortColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserSortColumnsProperty, value);
			}
		}

		// Token: 0x06004655 RID: 18005 RVA: 0x0013F7A8 File Offset: 0x0013D9A8
		private static object OnCoerceCanUserSortColumns(DependencyObject d, object baseValue)
		{
			DataGrid dataGrid = (DataGrid)d;
			if (DataGridHelper.IsPropertyTransferEnabled(dataGrid, DataGrid.CanUserSortColumnsProperty) && DataGridHelper.IsDefaultValue(dataGrid, DataGrid.CanUserSortColumnsProperty) && !dataGrid.Items.CanSort)
			{
				return false;
			}
			return baseValue;
		}

		// Token: 0x06004656 RID: 18006 RVA: 0x0013F7EC File Offset: 0x0013D9EC
		private static void OnCanUserSortColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid d2 = (DataGrid)d;
			DataGridHelper.TransferProperty(d2, DataGrid.CanUserSortColumnsProperty);
			DataGrid.OnNotifyColumnPropertyChanged(d, e);
		}

		/// <summary>Occurs when a column is being sorted.</summary>
		// Token: 0x140000BB RID: 187
		// (add) Token: 0x06004657 RID: 18007 RVA: 0x0013F814 File Offset: 0x0013DA14
		// (remove) Token: 0x06004658 RID: 18008 RVA: 0x0013F84C File Offset: 0x0013DA4C
		public event DataGridSortingEventHandler Sorting;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.Sorting" /> event.</summary>
		/// <param name="eventArgs">The data for the event.</param>
		// Token: 0x06004659 RID: 18009 RVA: 0x0013F881 File Offset: 0x0013DA81
		protected virtual void OnSorting(DataGridSortingEventArgs eventArgs)
		{
			eventArgs.Handled = false;
			if (this.Sorting != null)
			{
				this.Sorting(this, eventArgs);
			}
			if (!eventArgs.Handled)
			{
				this.DefaultSort(eventArgs.Column, (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift);
			}
		}

		// Token: 0x0600465A RID: 18010 RVA: 0x0013F8C0 File Offset: 0x0013DAC0
		internal void PerformSort(DataGridColumn sortColumn)
		{
			if (!this.CanUserSortColumns || !sortColumn.CanUserSort)
			{
				return;
			}
			if (this.CommitAnyEdit())
			{
				this.PrepareForSort(sortColumn);
				DataGridSortingEventArgs eventArgs = new DataGridSortingEventArgs(sortColumn);
				this.OnSorting(eventArgs);
				if (base.Items.NeedsRefresh)
				{
					try
					{
						base.Items.Refresh();
					}
					catch (InvalidOperationException innerException)
					{
						base.Items.SortDescriptions.Clear();
						throw new InvalidOperationException(SR.Get("DataGrid_ProbableInvalidSortDescription"), innerException);
					}
				}
			}
		}

		// Token: 0x0600465B RID: 18011 RVA: 0x0013F948 File Offset: 0x0013DB48
		private void PrepareForSort(DataGridColumn sortColumn)
		{
			if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
			{
				return;
			}
			if (this.Columns != null)
			{
				foreach (DataGridColumn dataGridColumn in this.Columns)
				{
					if (dataGridColumn != sortColumn)
					{
						dataGridColumn.SortDirection = null;
					}
				}
			}
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x0013F9B4 File Offset: 0x0013DBB4
		private void DefaultSort(DataGridColumn column, bool clearExistingSortDescriptions)
		{
			ListSortDirection listSortDirection = ListSortDirection.Ascending;
			ListSortDirection? sortDirection = column.SortDirection;
			if (sortDirection != null && sortDirection.Value == ListSortDirection.Ascending)
			{
				listSortDirection = ListSortDirection.Descending;
			}
			string sortMemberPath = column.SortMemberPath;
			if (!string.IsNullOrEmpty(sortMemberPath))
			{
				try
				{
					using (base.Items.DeferRefresh())
					{
						int num = -1;
						if (clearExistingSortDescriptions)
						{
							base.Items.SortDescriptions.Clear();
						}
						else
						{
							for (int i = 0; i < base.Items.SortDescriptions.Count; i++)
							{
								if (string.Compare(base.Items.SortDescriptions[i].PropertyName, sortMemberPath, StringComparison.Ordinal) == 0 && (this.GroupingSortDescriptionIndices == null || !this.GroupingSortDescriptionIndices.Contains(i)))
								{
									num = i;
									break;
								}
							}
						}
						SortDescription sortDescription = new SortDescription(sortMemberPath, listSortDirection);
						if (num >= 0)
						{
							base.Items.SortDescriptions[num] = sortDescription;
						}
						else
						{
							base.Items.SortDescriptions.Add(sortDescription);
						}
						if (clearExistingSortDescriptions || !this._sortingStarted)
						{
							this.RegenerateGroupingSortDescriptions();
							this._sortingStarted = true;
						}
					}
					column.SortDirection = new ListSortDirection?(listSortDirection);
				}
				catch (InvalidOperationException p)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.CannotSort(new object[]
					{
						sortMemberPath
					}), p);
					base.Items.SortDescriptions.Clear();
				}
			}
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x0600465D RID: 18013 RVA: 0x0013FB24 File Offset: 0x0013DD24
		// (set) Token: 0x0600465E RID: 18014 RVA: 0x0013FB2C File Offset: 0x0013DD2C
		private List<int> GroupingSortDescriptionIndices
		{
			get
			{
				return this._groupingSortDescriptionIndices;
			}
			set
			{
				this._groupingSortDescriptionIndices = value;
			}
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x0013FB38 File Offset: 0x0013DD38
		private void OnItemsSortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this._ignoreSortDescriptionsChange || this.GroupingSortDescriptionIndices == null)
			{
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				int i = 0;
				int count = this.GroupingSortDescriptionIndices.Count;
				while (i < count)
				{
					if (this.GroupingSortDescriptionIndices[i] >= e.NewStartingIndex)
					{
						List<int> groupingSortDescriptionIndices = this.GroupingSortDescriptionIndices;
						int num = i;
						int num2 = groupingSortDescriptionIndices[num];
						groupingSortDescriptionIndices[num] = num2 + 1;
					}
					i++;
				}
				return;
			}
			case NotifyCollectionChangedAction.Remove:
			{
				int j = 0;
				int num3 = this.GroupingSortDescriptionIndices.Count;
				while (j < num3)
				{
					if (this.GroupingSortDescriptionIndices[j] > e.OldStartingIndex)
					{
						List<int> groupingSortDescriptionIndices2 = this.GroupingSortDescriptionIndices;
						int num2 = j;
						int num = groupingSortDescriptionIndices2[num2];
						groupingSortDescriptionIndices2[num2] = num - 1;
					}
					else if (this.GroupingSortDescriptionIndices[j] == e.OldStartingIndex)
					{
						this.GroupingSortDescriptionIndices.RemoveAt(j);
						j--;
						num3--;
					}
					j++;
				}
				return;
			}
			case NotifyCollectionChangedAction.Replace:
				this.GroupingSortDescriptionIndices.Remove(e.OldStartingIndex);
				return;
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Reset:
				this.GroupingSortDescriptionIndices.Clear();
				break;
			default:
				return;
			}
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x0013FC60 File Offset: 0x0013DE60
		private void RemoveGroupingSortDescriptions()
		{
			if (this.GroupingSortDescriptionIndices == null)
			{
				return;
			}
			bool ignoreSortDescriptionsChange = this._ignoreSortDescriptionsChange;
			this._ignoreSortDescriptionsChange = true;
			try
			{
				int i = 0;
				int count = this.GroupingSortDescriptionIndices.Count;
				while (i < count)
				{
					base.Items.SortDescriptions.RemoveAt(this.GroupingSortDescriptionIndices[i] - i);
					i++;
				}
				this.GroupingSortDescriptionIndices.Clear();
			}
			finally
			{
				this._ignoreSortDescriptionsChange = ignoreSortDescriptionsChange;
			}
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x0013FCE0 File Offset: 0x0013DEE0
		private static bool CanConvertToSortDescription(PropertyGroupDescription propertyGroupDescription)
		{
			return propertyGroupDescription != null && propertyGroupDescription.Converter == null && propertyGroupDescription.StringComparison == StringComparison.Ordinal;
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x0013FCFC File Offset: 0x0013DEFC
		private void AddGroupingSortDescriptions()
		{
			bool ignoreSortDescriptionsChange = this._ignoreSortDescriptionsChange;
			this._ignoreSortDescriptionsChange = true;
			try
			{
				int index = 0;
				foreach (GroupDescription groupDescription in base.Items.GroupDescriptions)
				{
					PropertyGroupDescription propertyGroupDescription = groupDescription as PropertyGroupDescription;
					if (DataGrid.CanConvertToSortDescription(propertyGroupDescription))
					{
						SortDescription item = new SortDescription(propertyGroupDescription.PropertyName, ListSortDirection.Ascending);
						base.Items.SortDescriptions.Insert(index, item);
						if (this.GroupingSortDescriptionIndices == null)
						{
							this.GroupingSortDescriptionIndices = new List<int>();
						}
						this.GroupingSortDescriptionIndices.Add(index++);
					}
				}
			}
			finally
			{
				this._ignoreSortDescriptionsChange = ignoreSortDescriptionsChange;
			}
		}

		// Token: 0x06004663 RID: 18019 RVA: 0x0013FDC4 File Offset: 0x0013DFC4
		private void RegenerateGroupingSortDescriptions()
		{
			this.RemoveGroupingSortDescriptions();
			this.AddGroupingSortDescriptions();
		}

		// Token: 0x06004664 RID: 18020 RVA: 0x0013FDD4 File Offset: 0x0013DFD4
		private void OnItemsGroupDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.EnqueueNewItemMarginComputation();
			if (!this._sortingStarted)
			{
				return;
			}
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
				if (DataGrid.CanConvertToSortDescription(e.NewItems[0] as PropertyGroupDescription))
				{
					this.RegenerateGroupingSortDescriptions();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Remove:
				if (DataGrid.CanConvertToSortDescription(e.OldItems[0] as PropertyGroupDescription))
				{
					this.RegenerateGroupingSortDescriptions();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Replace:
				if (DataGrid.CanConvertToSortDescription(e.OldItems[0] as PropertyGroupDescription) || DataGrid.CanConvertToSortDescription(e.NewItems[0] as PropertyGroupDescription))
				{
					this.RegenerateGroupingSortDescriptions();
					return;
				}
				break;
			case NotifyCollectionChangedAction.Move:
				break;
			case NotifyCollectionChangedAction.Reset:
				this.RemoveGroupingSortDescriptions();
				break;
			default:
				return;
			}
		}

		/// <summary>Occurs when auto generation of all columns is completed.</summary>
		// Token: 0x140000BC RID: 188
		// (add) Token: 0x06004665 RID: 18021 RVA: 0x0013FE90 File Offset: 0x0013E090
		// (remove) Token: 0x06004666 RID: 18022 RVA: 0x0013FEC8 File Offset: 0x0013E0C8
		public event EventHandler AutoGeneratedColumns;

		/// <summary>Occurs when an individual column is auto-generated.</summary>
		// Token: 0x140000BD RID: 189
		// (add) Token: 0x06004667 RID: 18023 RVA: 0x0013FF00 File Offset: 0x0013E100
		// (remove) Token: 0x06004668 RID: 18024 RVA: 0x0013FF38 File Offset: 0x0013E138
		public event EventHandler<DataGridAutoGeneratingColumnEventArgs> AutoGeneratingColumn;

		/// <summary>Gets or sets a value that indicates whether the columns are created automatically. </summary>
		/// <returns>
		///     <see langword="true" /> if columns are created automatically; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x06004669 RID: 18025 RVA: 0x0013FF6D File Offset: 0x0013E16D
		// (set) Token: 0x0600466A RID: 18026 RVA: 0x0013FF7F File Offset: 0x0013E17F
		public bool AutoGenerateColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.AutoGenerateColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.AutoGenerateColumnsProperty, value);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.AutoGeneratedColumns" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600466B RID: 18027 RVA: 0x0013FF8D File Offset: 0x0013E18D
		protected virtual void OnAutoGeneratedColumns(EventArgs e)
		{
			if (this.AutoGeneratedColumns != null)
			{
				this.AutoGeneratedColumns(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.AutoGeneratingColumn" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600466C RID: 18028 RVA: 0x0013FFA4 File Offset: 0x0013E1A4
		protected virtual void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
		{
			if (this.AutoGeneratingColumn != null)
			{
				this.AutoGeneratingColumn(this, e);
			}
		}

		/// <summary>Determines the desired size of the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		/// <param name="availableSize">The maximum size that the <see cref="T:System.Windows.Controls.DataGrid" /> can occupy.</param>
		/// <returns>The desired size of the <see cref="T:System.Windows.Controls.DataGrid" />.</returns>
		// Token: 0x0600466D RID: 18029 RVA: 0x0013FFBC File Offset: 0x0013E1BC
		protected override Size MeasureOverride(Size availableSize)
		{
			if (this._measureNeverInvoked)
			{
				this._measureNeverInvoked = false;
				if (this.AutoGenerateColumns)
				{
					this.AddAutoColumns();
				}
				this.InternalColumns.InitializeDisplayIndexMap();
				base.CoerceValue(DataGrid.FrozenColumnCountProperty);
				base.CoerceValue(DataGrid.CanUserAddRowsProperty);
				base.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
				this.UpdateNewItemPlaceholder(false);
				this.EnsureItemBindingGroup();
				base.ItemBindingGroup.SharesProposedValues = true;
			}
			else if (this.DeferAutoGeneration && this.AutoGenerateColumns)
			{
				this.AddAutoColumns();
			}
			return base.MeasureOverride(availableSize);
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x0014004A File Offset: 0x0013E24A
		private void EnsureItemBindingGroup()
		{
			if (base.ItemBindingGroup == null)
			{
				this._defaultBindingGroup = new BindingGroup();
				base.SetCurrentValue(ItemsControl.ItemBindingGroupProperty, this._defaultBindingGroup);
			}
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x00140070 File Offset: 0x0013E270
		private void ClearSortDescriptionsOnItemsSourceChange()
		{
			base.Items.SortDescriptions.Clear();
			this._sortingStarted = false;
			List<int> groupingSortDescriptionIndices = this.GroupingSortDescriptionIndices;
			if (groupingSortDescriptionIndices != null)
			{
				groupingSortDescriptionIndices.Clear();
			}
			foreach (DataGridColumn dataGridColumn in this.Columns)
			{
				dataGridColumn.SortDirection = null;
			}
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x001400EC File Offset: 0x0013E2EC
		private static object OnCoerceItemsSourceProperty(DependencyObject d, object baseValue)
		{
			DataGrid dataGrid = (DataGrid)d;
			if (baseValue != dataGrid._cachedItemsSource && dataGrid._cachedItemsSource != null)
			{
				dataGrid.ClearSortDescriptionsOnItemsSourceChange();
			}
			return baseValue;
		}

		/// <summary>Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.ItemsSource" /> property changes. </summary>
		/// <param name="oldValue">The old source.</param>
		/// <param name="newValue">The new source.</param>
		// Token: 0x06004671 RID: 18033 RVA: 0x00140118 File Offset: 0x0013E318
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			base.OnItemsSourceChanged(oldValue, newValue);
			if (newValue == null)
			{
				this.ClearSortDescriptionsOnItemsSourceChange();
			}
			this._cachedItemsSource = newValue;
			using (this.UpdateSelectedCells())
			{
				List<Tuple<int, int>> ranges = new List<Tuple<int, int>>();
				base.LocateSelectedItems(ranges, false);
				this._selectedCells.RestoreOnlyFullRows(ranges);
			}
			if (this.AutoGenerateColumns)
			{
				this.RegenerateAutoColumns();
			}
			this.InternalColumns.RefreshAutoWidthColumns = true;
			this.InternalColumns.InvalidateColumnWidthsComputation();
			base.CoerceValue(DataGrid.CanUserAddRowsProperty);
			base.CoerceValue(DataGrid.CanUserDeleteRowsProperty);
			DataGridHelper.TransferProperty(this, DataGrid.CanUserSortColumnsProperty);
			this.ResetRowHeaderActualWidth();
			this.UpdateNewItemPlaceholder(false);
			this.HasCellValidationError = false;
			this.HasRowValidationError = false;
		}

		// Token: 0x17001141 RID: 4417
		// (get) Token: 0x06004672 RID: 18034 RVA: 0x001401DC File Offset: 0x0013E3DC
		// (set) Token: 0x06004673 RID: 18035 RVA: 0x001401E4 File Offset: 0x0013E3E4
		private bool DeferAutoGeneration { get; set; }

		/// <summary>Performs column auto generation and updates validation flags when items change.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x06004674 RID: 18036 RVA: 0x001401F0 File Offset: 0x0013E3F0
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				if (this.DeferAutoGeneration)
				{
					this.AddAutoColumns();
					return;
				}
			}
			else
			{
				if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
				{
					if (!this.HasRowValidationError && !this.HasCellValidationError)
					{
						return;
					}
					using (IEnumerator enumerator = e.OldItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object item = enumerator.Current;
							if (this.IsAddingOrEditingRowItem(item))
							{
								this.HasRowValidationError = false;
								this.HasCellValidationError = false;
								break;
							}
						}
						return;
					}
				}
				if (e.Action == NotifyCollectionChangedAction.Reset)
				{
					this.ResetRowHeaderActualWidth();
					this.HasRowValidationError = false;
					this.HasCellValidationError = false;
				}
			}
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x001402B8 File Offset: 0x0013E4B8
		internal override void AdjustItemInfoOverride(NotifyCollectionChangedEventArgs e)
		{
			List<ItemsControl.ItemInfo> list = new List<ItemsControl.ItemInfo>();
			if (this._selectionAnchor != null)
			{
				list.Add(this._selectionAnchor.Value.ItemInfo);
			}
			if (this._editingRowInfo != null)
			{
				list.Add(this._editingRowInfo);
			}
			if (DataGrid.CellInfoNeedsAdjusting(this.CurrentCell))
			{
				list.Add(this.CurrentCell.ItemInfo);
			}
			base.AdjustItemInfos(e, list);
			base.AdjustItemInfoOverride(e);
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x0014033C File Offset: 0x0013E53C
		internal override void AdjustItemInfosAfterGeneratorChangeOverride()
		{
			List<ItemsControl.ItemInfo> list = new List<ItemsControl.ItemInfo>();
			if (this._selectionAnchor != null)
			{
				list.Add(this._selectionAnchor.Value.ItemInfo);
			}
			if (this._editingRowInfo != null)
			{
				list.Add(this._editingRowInfo);
			}
			if (DataGrid.CellInfoNeedsAdjusting(this.CurrentCell))
			{
				list.Add(this.CurrentCell.ItemInfo);
			}
			base.AdjustItemInfosAfterGeneratorChange(list, false);
			base.AdjustItemInfosAfterGeneratorChangeOverride();
			this.AdjustPendingInfos();
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x001403C4 File Offset: 0x0013E5C4
		private static bool CellInfoNeedsAdjusting(DataGridCellInfo cellInfo)
		{
			ItemsControl.ItemInfo itemInfo = cellInfo.ItemInfo;
			return itemInfo != null && itemInfo.Index != -1;
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x001403F0 File Offset: 0x0013E5F0
		private void AdjustPendingInfos()
		{
			int count;
			if (this._pendingInfos != null && this._pendingInfos.Count > 0 && (count = this._columns.Count) > 0)
			{
				using (this.UpdateSelectedCells())
				{
					for (int i = this._pendingInfos.Count - 1; i >= 0; i--)
					{
						ItemsControl.ItemInfo itemInfo = this._pendingInfos[i];
						if (itemInfo.Index >= 0)
						{
							this._pendingInfos.RemoveAt(i);
							this._selectedCells.AddRegion(itemInfo.Index, 0, 1, count);
						}
					}
				}
			}
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x00140494 File Offset: 0x0013E694
		private void AddAutoColumns()
		{
			ReadOnlyCollection<ItemPropertyInfo> itemProperties = ((IItemProperties)base.Items).ItemProperties;
			if (itemProperties == null && this.DataItemsCount == 0)
			{
				this.DeferAutoGeneration = true;
				return;
			}
			if (!this._measureNeverInvoked)
			{
				DataGrid.GenerateColumns(itemProperties, this, null);
				this.DeferAutoGeneration = false;
				this.OnAutoGeneratedColumns(EventArgs.Empty);
			}
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x001404E4 File Offset: 0x0013E6E4
		private void DeleteAutoColumns()
		{
			if (!this.DeferAutoGeneration && !this._measureNeverInvoked)
			{
				for (int i = this.Columns.Count - 1; i >= 0; i--)
				{
					if (this.Columns[i].IsAutoGenerated)
					{
						this.Columns.RemoveAt(i);
					}
				}
				return;
			}
			this.DeferAutoGeneration = false;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x00140540 File Offset: 0x0013E740
		private void RegenerateAutoColumns()
		{
			this.DeleteAutoColumns();
			this.AddAutoColumns();
		}

		/// <summary>Generates columns for the specified properties of an object.</summary>
		/// <param name="itemProperties">The properties of the object to be in the columns.</param>
		/// <returns>The collection of columns for the properties of the object.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="itemProperties" /> is <see langword="null" />.</exception>
		// Token: 0x0600467C RID: 18044 RVA: 0x00140550 File Offset: 0x0013E750
		public static Collection<DataGridColumn> GenerateColumns(IItemProperties itemProperties)
		{
			if (itemProperties == null)
			{
				throw new ArgumentNullException("itemProperties");
			}
			Collection<DataGridColumn> collection = new Collection<DataGridColumn>();
			DataGrid.GenerateColumns(itemProperties.ItemProperties, null, collection);
			return collection;
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00140580 File Offset: 0x0013E780
		private static void GenerateColumns(ReadOnlyCollection<ItemPropertyInfo> itemProperties, DataGrid dataGrid, Collection<DataGridColumn> columnCollection)
		{
			if (itemProperties != null && itemProperties.Count > 0)
			{
				foreach (ItemPropertyInfo itemPropertyInfo in itemProperties)
				{
					DataGridColumn dataGridColumn = DataGridColumn.CreateDefaultColumn(itemPropertyInfo);
					if (dataGrid != null)
					{
						DataGridAutoGeneratingColumnEventArgs dataGridAutoGeneratingColumnEventArgs = new DataGridAutoGeneratingColumnEventArgs(dataGridColumn, itemPropertyInfo);
						dataGrid.OnAutoGeneratingColumn(dataGridAutoGeneratingColumnEventArgs);
						if (!dataGridAutoGeneratingColumnEventArgs.Cancel && dataGridAutoGeneratingColumnEventArgs.Column != null)
						{
							dataGridAutoGeneratingColumnEventArgs.Column.IsAutoGenerated = true;
							dataGrid.Columns.Add(dataGridAutoGeneratingColumnEventArgs.Column);
						}
					}
					else
					{
						columnCollection.Add(dataGridColumn);
					}
				}
			}
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x0014061C File Offset: 0x0013E81C
		private static void OnAutoGenerateColumnsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool flag = (bool)e.NewValue;
			DataGrid dataGrid = (DataGrid)d;
			if (flag)
			{
				dataGrid.AddAutoColumns();
				return;
			}
			dataGrid.DeleteAutoColumns();
		}

		/// <summary>Gets or sets the number of non-scrolling columns.</summary>
		/// <returns>The number of non-scrolling columns. The registered default is 0. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001142 RID: 4418
		// (get) Token: 0x0600467F RID: 18047 RVA: 0x0014064D File Offset: 0x0013E84D
		// (set) Token: 0x06004680 RID: 18048 RVA: 0x0014065F File Offset: 0x0013E85F
		public int FrozenColumnCount
		{
			get
			{
				return (int)base.GetValue(DataGrid.FrozenColumnCountProperty);
			}
			set
			{
				base.SetValue(DataGrid.FrozenColumnCountProperty, value);
			}
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x00140674 File Offset: 0x0013E874
		private static object OnCoerceFrozenColumnCount(DependencyObject d, object baseValue)
		{
			DataGrid dataGrid = (DataGrid)d;
			int num = (int)baseValue;
			if (num > dataGrid.Columns.Count)
			{
				return dataGrid.Columns.Count;
			}
			return baseValue;
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00139CDB File Offset: 0x00137EDB
		private static void OnFrozenColumnCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x001406B0 File Offset: 0x0013E8B0
		private static bool ValidateFrozenColumnCount(object value)
		{
			int num = (int)value;
			return num >= 0;
		}

		/// <summary>Gets the horizontal offset of the scrollable columns in the view port.</summary>
		/// <returns>The horizontal offset of the scrollable columns. The registered default is 0.0. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001143 RID: 4419
		// (get) Token: 0x06004684 RID: 18052 RVA: 0x001406CB File Offset: 0x0013E8CB
		// (set) Token: 0x06004685 RID: 18053 RVA: 0x001406DD File Offset: 0x0013E8DD
		public double NonFrozenColumnsViewportHorizontalOffset
		{
			get
			{
				return (double)base.GetValue(DataGrid.NonFrozenColumnsViewportHorizontalOffsetProperty);
			}
			internal set
			{
				base.SetValue(DataGrid.NonFrozenColumnsViewportHorizontalOffsetPropertyKey, value);
			}
		}

		/// <summary>Invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.</summary>
		// Token: 0x06004686 RID: 18054 RVA: 0x001406F0 File Offset: 0x0013E8F0
		public override void OnApplyTemplate()
		{
			if (this.InternalItemsHost != null && !base.IsAncestorOf(this.InternalItemsHost))
			{
				this.InternalItemsHost = null;
			}
			this.CleanUpInternalScrollControls();
			base.OnApplyTemplate();
		}

		/// <summary>Gets or sets a value that indicates whether row virtualization is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if row virtualization is enabled; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x0014071B File Offset: 0x0013E91B
		// (set) Token: 0x06004688 RID: 18056 RVA: 0x0014072D File Offset: 0x0013E92D
		public bool EnableRowVirtualization
		{
			get
			{
				return (bool)base.GetValue(DataGrid.EnableRowVirtualizationProperty);
			}
			set
			{
				base.SetValue(DataGrid.EnableRowVirtualizationProperty, value);
			}
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x0014073C File Offset: 0x0013E93C
		private static void OnEnableRowVirtualizationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGrid dataGrid = (DataGrid)d;
			dataGrid.CoerceValue(VirtualizingPanel.IsVirtualizingProperty);
			Panel internalItemsHost = dataGrid.InternalItemsHost;
			if (internalItemsHost != null)
			{
				internalItemsHost.InvalidateMeasure();
				internalItemsHost.InvalidateArrange();
			}
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x00140771 File Offset: 0x0013E971
		private static object OnCoerceIsVirtualizingProperty(DependencyObject d, object baseValue)
		{
			if (!DataGridHelper.IsDefaultValue(d, DataGrid.EnableRowVirtualizationProperty))
			{
				return d.GetValue(DataGrid.EnableRowVirtualizationProperty);
			}
			return baseValue;
		}

		/// <summary>Gets or sets a value that indicates whether column virtualization is enabled.</summary>
		/// <returns>
		///     <see langword="true" /> if column virtualization is enabled; otherwise, <see langword="false" />. The registered default is <see langword="false" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x0600468B RID: 18059 RVA: 0x0014078D File Offset: 0x0013E98D
		// (set) Token: 0x0600468C RID: 18060 RVA: 0x0014079F File Offset: 0x0013E99F
		public bool EnableColumnVirtualization
		{
			get
			{
				return (bool)base.GetValue(DataGrid.EnableColumnVirtualizationProperty);
			}
			set
			{
				base.SetValue(DataGrid.EnableColumnVirtualizationProperty, value);
			}
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x00139CDB File Offset: 0x00137EDB
		private static void OnEnableColumnVirtualizationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.ColumnCollection | DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		/// <summary>Gets or sets a value that indicates whether the user can change the column display order by dragging column headers with the mouse.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can reorder columns; otherwise, <see langword="false" />. The registered default is <see langword="true" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x0600468E RID: 18062 RVA: 0x001407AD File Offset: 0x0013E9AD
		// (set) Token: 0x0600468F RID: 18063 RVA: 0x001407BF File Offset: 0x0013E9BF
		public bool CanUserReorderColumns
		{
			get
			{
				return (bool)base.GetValue(DataGrid.CanUserReorderColumnsProperty);
			}
			set
			{
				base.SetValue(DataGrid.CanUserReorderColumnsProperty, value);
			}
		}

		/// <summary>Gets or sets the style that is used when rendering the drag indicator that is displayed while dragging a column header.</summary>
		/// <returns>The style applied to a column header when dragging. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x06004690 RID: 18064 RVA: 0x001407CD File Offset: 0x0013E9CD
		// (set) Token: 0x06004691 RID: 18065 RVA: 0x001407DF File Offset: 0x0013E9DF
		public Style DragIndicatorStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.DragIndicatorStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.DragIndicatorStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the style that is applied to indicate the drop location when dragging a column header.</summary>
		/// <returns>The style applied to a column header. The registered default is <see langword="null" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001148 RID: 4424
		// (get) Token: 0x06004692 RID: 18066 RVA: 0x001407ED File Offset: 0x0013E9ED
		// (set) Token: 0x06004693 RID: 18067 RVA: 0x001407FF File Offset: 0x0013E9FF
		public Style DropLocationIndicatorStyle
		{
			get
			{
				return (Style)base.GetValue(DataGrid.DropLocationIndicatorStyleProperty);
			}
			set
			{
				base.SetValue(DataGrid.DropLocationIndicatorStyleProperty, value);
			}
		}

		/// <summary>Occurs before a column moves to a new position in the display order.</summary>
		// Token: 0x140000BE RID: 190
		// (add) Token: 0x06004694 RID: 18068 RVA: 0x00140810 File Offset: 0x0013EA10
		// (remove) Token: 0x06004695 RID: 18069 RVA: 0x00140848 File Offset: 0x0013EA48
		public event EventHandler<DataGridColumnReorderingEventArgs> ColumnReordering;

		/// <summary>Occurs when the user begins dragging a column header by using the mouse.</summary>
		// Token: 0x140000BF RID: 191
		// (add) Token: 0x06004696 RID: 18070 RVA: 0x00140880 File Offset: 0x0013EA80
		// (remove) Token: 0x06004697 RID: 18071 RVA: 0x001408B8 File Offset: 0x0013EAB8
		public event EventHandler<DragStartedEventArgs> ColumnHeaderDragStarted;

		/// <summary>Occurs every time the mouse position changes while the user drags a column header. </summary>
		// Token: 0x140000C0 RID: 192
		// (add) Token: 0x06004698 RID: 18072 RVA: 0x001408F0 File Offset: 0x0013EAF0
		// (remove) Token: 0x06004699 RID: 18073 RVA: 0x00140928 File Offset: 0x0013EB28
		public event EventHandler<DragDeltaEventArgs> ColumnHeaderDragDelta;

		/// <summary>Occurs when the user releases a column header after dragging it by using the mouse.</summary>
		// Token: 0x140000C1 RID: 193
		// (add) Token: 0x0600469A RID: 18074 RVA: 0x00140960 File Offset: 0x0013EB60
		// (remove) Token: 0x0600469B RID: 18075 RVA: 0x00140998 File Offset: 0x0013EB98
		public event EventHandler<DragCompletedEventArgs> ColumnHeaderDragCompleted;

		/// <summary>Occurs when a column moves to a new position in the display order.</summary>
		// Token: 0x140000C2 RID: 194
		// (add) Token: 0x0600469C RID: 18076 RVA: 0x001409D0 File Offset: 0x0013EBD0
		// (remove) Token: 0x0600469D RID: 18077 RVA: 0x00140A08 File Offset: 0x0013EC08
		public event EventHandler<DataGridColumnEventArgs> ColumnReordered;

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.ColumnHeaderDragStarted" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600469E RID: 18078 RVA: 0x00140A3D File Offset: 0x0013EC3D
		protected internal virtual void OnColumnHeaderDragStarted(DragStartedEventArgs e)
		{
			if (this.ColumnHeaderDragStarted != null)
			{
				this.ColumnHeaderDragStarted(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.ColumnReordering" /> event.</summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x0600469F RID: 18079 RVA: 0x00140A54 File Offset: 0x0013EC54
		protected internal virtual void OnColumnReordering(DataGridColumnReorderingEventArgs e)
		{
			if (this.ColumnReordering != null)
			{
				this.ColumnReordering(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.ColumnHeaderDragDelta" /> event. </summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060046A0 RID: 18080 RVA: 0x00140A6B File Offset: 0x0013EC6B
		protected internal virtual void OnColumnHeaderDragDelta(DragDeltaEventArgs e)
		{
			if (this.ColumnHeaderDragDelta != null)
			{
				this.ColumnHeaderDragDelta(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.ColumnHeaderDragCompleted" /> event. </summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060046A1 RID: 18081 RVA: 0x00140A82 File Offset: 0x0013EC82
		protected internal virtual void OnColumnHeaderDragCompleted(DragCompletedEventArgs e)
		{
			if (this.ColumnHeaderDragCompleted != null)
			{
				this.ColumnHeaderDragCompleted(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.ColumnReordered" /> event. </summary>
		/// <param name="e">The data for the event.</param>
		// Token: 0x060046A2 RID: 18082 RVA: 0x00140A99 File Offset: 0x0013EC99
		protected internal virtual void OnColumnReordered(DataGridColumnEventArgs e)
		{
			if (this.ColumnReordered != null)
			{
				this.ColumnReordered(this, e);
			}
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x0013C3E9 File Offset: 0x0013A5E9
		private static void OnClipboardCopyModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			CommandManager.InvalidateRequerySuggested();
		}

		/// <summary>Gets or sets a value that indicates how content is copied to the clipboard.</summary>
		/// <returns>One of the enumeration values that indicates how content is copied to the clipboard. The registered default is <see cref="F:System.Windows.Controls.DataGridClipboardCopyMode.ExcludeHeader" />. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001149 RID: 4425
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x00140AB0 File Offset: 0x0013ECB0
		// (set) Token: 0x060046A5 RID: 18085 RVA: 0x00140AC2 File Offset: 0x0013ECC2
		public DataGridClipboardCopyMode ClipboardCopyMode
		{
			get
			{
				return (DataGridClipboardCopyMode)base.GetValue(DataGrid.ClipboardCopyModeProperty);
			}
			set
			{
				base.SetValue(DataGrid.ClipboardCopyModeProperty, value);
			}
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x00140AD5 File Offset: 0x0013ECD5
		private static void OnCanExecuteCopy(object target, CanExecuteRoutedEventArgs args)
		{
			((DataGrid)target).OnCanExecuteCopy(args);
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.CanExecute" /> event associated with the <see cref="P:System.Windows.Input.ApplicationCommands.Copy" /> command.</summary>
		/// <param name="args">The data for the event.</param>
		// Token: 0x060046A7 RID: 18087 RVA: 0x00140AE3 File Offset: 0x0013ECE3
		protected virtual void OnCanExecuteCopy(CanExecuteRoutedEventArgs args)
		{
			args.CanExecute = (this.ClipboardCopyMode != DataGridClipboardCopyMode.None && this._selectedCells.Count > 0);
			args.Handled = true;
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x00140B0B File Offset: 0x0013ED0B
		private static void OnExecutedCopy(object target, ExecutedRoutedEventArgs args)
		{
			((DataGrid)target).OnExecutedCopy(args);
		}

		/// <summary>Provides handling for the <see cref="E:System.Windows.Input.CommandBinding.Executed" /> event associated with the <see cref="P:System.Windows.Input.ApplicationCommands.Copy" /> command.</summary>
		/// <param name="args">The data for the event.</param>
		/// <exception cref="T:System.NotSupportedException">
		///         <see cref="P:System.Windows.Controls.DataGrid.ClipboardCopyMode" /> is set to <see cref="F:System.Windows.Controls.DataGridClipboardCopyMode.None" />.</exception>
		// Token: 0x060046A9 RID: 18089 RVA: 0x00140B1C File Offset: 0x0013ED1C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected virtual void OnExecutedCopy(ExecutedRoutedEventArgs args)
		{
			if (this.ClipboardCopyMode == DataGridClipboardCopyMode.None)
			{
				throw new NotSupportedException(SR.Get("ClipboardCopyMode_Disabled"));
			}
			args.Handled = true;
			Collection<string> collection = new Collection<string>(new string[]
			{
				DataFormats.Html,
				DataFormats.Text,
				DataFormats.UnicodeText,
				DataFormats.CommaSeparatedValue
			});
			Dictionary<string, StringBuilder> dictionary = new Dictionary<string, StringBuilder>(collection.Count);
			foreach (string key in collection)
			{
				dictionary[key] = new StringBuilder();
			}
			int startColumnDisplayIndex;
			int endColumnDisplayIndex;
			int num;
			int num2;
			if (this._selectedCells.GetSelectionRange(out startColumnDisplayIndex, out endColumnDisplayIndex, out num, out num2))
			{
				if (this.ClipboardCopyMode == DataGridClipboardCopyMode.IncludeHeader)
				{
					DataGridRowClipboardEventArgs dataGridRowClipboardEventArgs = new DataGridRowClipboardEventArgs(null, startColumnDisplayIndex, endColumnDisplayIndex, true);
					this.OnCopyingRowClipboardContent(dataGridRowClipboardEventArgs);
					foreach (string text in collection)
					{
						dictionary[text].Append(dataGridRowClipboardEventArgs.FormatClipboardCellValues(text));
					}
				}
				for (int i = num; i <= num2; i++)
				{
					object item = base.Items[i];
					if (this._selectedCells.Intersects(i))
					{
						DataGridRowClipboardEventArgs dataGridRowClipboardEventArgs2 = new DataGridRowClipboardEventArgs(item, startColumnDisplayIndex, endColumnDisplayIndex, false, i);
						this.OnCopyingRowClipboardContent(dataGridRowClipboardEventArgs2);
						foreach (string text2 in collection)
						{
							dictionary[text2].Append(dataGridRowClipboardEventArgs2.FormatClipboardCellValues(text2));
						}
					}
				}
			}
			DataGridClipboardHelper.GetClipboardContentForHtml(dictionary[DataFormats.Html]);
			if ((SecurityHelper.CallerHasAllClipboardPermission() && SecurityHelper.CallerHasSerializationPermission()) || args.UserInitiated)
			{
				new UIPermission(UIPermissionClipboard.AllClipboard).Assert();
				DataObject dataObject;
				try
				{
					dataObject = new DataObject();
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				foreach (string text3 in collection)
				{
					dataObject.CriticalSetData(text3, dictionary[text3].ToString(), false);
				}
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode | SecurityPermissionFlag.SerializationFormatter).Assert();
				try
				{
					Clipboard.CriticalSetDataObject(dataObject, true);
				}
				catch (ExternalException obj) when (!FrameworkCompatibilityPreferences.ShouldThrowOnDataGridCopyOrCutFailure)
				{
				}
				finally
				{
					CodeAccessPermission.RevertAll();
				}
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Controls.DataGrid.CopyingRowClipboardContent" /> event. </summary>
		/// <param name="args">The data for the event.</param>
		// Token: 0x060046AA RID: 18090 RVA: 0x00140DD8 File Offset: 0x0013EFD8
		protected virtual void OnCopyingRowClipboardContent(DataGridRowClipboardEventArgs args)
		{
			if (args.IsColumnHeadersRow)
			{
				for (int i = args.StartColumnDisplayIndex; i <= args.EndColumnDisplayIndex; i++)
				{
					DataGridColumn dataGridColumn = this.ColumnFromDisplayIndex(i);
					if (dataGridColumn.IsVisible)
					{
						args.ClipboardRowContent.Add(new DataGridClipboardCellContent(args.Item, dataGridColumn, dataGridColumn.Header));
					}
				}
			}
			else
			{
				int num = args.RowIndexHint;
				if (num < 0)
				{
					num = base.Items.IndexOf(args.Item);
				}
				if (this._selectedCells.Intersects(num))
				{
					for (int j = args.StartColumnDisplayIndex; j <= args.EndColumnDisplayIndex; j++)
					{
						DataGridColumn dataGridColumn2 = this.ColumnFromDisplayIndex(j);
						if (dataGridColumn2.IsVisible)
						{
							object content = null;
							if (this._selectedCells.Contains(num, j))
							{
								content = dataGridColumn2.OnCopyingCellClipboardContent(args.Item);
							}
							args.ClipboardRowContent.Add(new DataGridClipboardCellContent(args.Item, dataGridColumn2, content));
						}
					}
				}
			}
			if (this.CopyingRowClipboardContent != null)
			{
				this.CopyingRowClipboardContent(this, args);
			}
		}

		/// <summary>Occurs after the default row content is prepared. </summary>
		// Token: 0x140000C3 RID: 195
		// (add) Token: 0x060046AB RID: 18091 RVA: 0x00140ED8 File Offset: 0x0013F0D8
		// (remove) Token: 0x060046AC RID: 18092 RVA: 0x00140F10 File Offset: 0x0013F110
		public event EventHandler<DataGridRowClipboardEventArgs> CopyingRowClipboardContent;

		// Token: 0x1700114A RID: 4426
		// (get) Token: 0x060046AD RID: 18093 RVA: 0x00140F45 File Offset: 0x0013F145
		// (set) Token: 0x060046AE RID: 18094 RVA: 0x00140F57 File Offset: 0x0013F157
		internal double CellsPanelActualWidth
		{
			get
			{
				return (double)base.GetValue(DataGrid.CellsPanelActualWidthProperty);
			}
			set
			{
				base.SetValue(DataGrid.CellsPanelActualWidthProperty, value);
			}
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x00140F6C File Offset: 0x0013F16C
		private static void CellsPanelActualWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			double value = (double)e.OldValue;
			double value2 = (double)e.NewValue;
			if (!DoubleUtil.AreClose(value, value2))
			{
				((DataGrid)d).NotifyPropertyChanged(d, e, DataGridNotificationTarget.ColumnHeadersPresenter);
			}
		}

		/// <summary>Gets the horizontal offset for the <see cref="T:System.Windows.Controls.DataGridCellsPanel" />.</summary>
		/// <returns>The horizontal offset for the cells panel. The registered default is 0.0. For more information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x060046B0 RID: 18096 RVA: 0x00140FAB File Offset: 0x0013F1AB
		// (set) Token: 0x060046B1 RID: 18097 RVA: 0x00140FBD File Offset: 0x0013F1BD
		public double CellsPanelHorizontalOffset
		{
			get
			{
				return (double)base.GetValue(DataGrid.CellsPanelHorizontalOffsetProperty);
			}
			private set
			{
				base.SetValue(DataGrid.CellsPanelHorizontalOffsetPropertyKey, value);
			}
		}

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x060046B2 RID: 18098 RVA: 0x00140FD0 File Offset: 0x0013F1D0
		// (set) Token: 0x060046B3 RID: 18099 RVA: 0x00140FD8 File Offset: 0x0013F1D8
		private bool CellsPanelHorizontalOffsetComputationPending { get; set; }

		// Token: 0x060046B4 RID: 18100 RVA: 0x00140FE1 File Offset: 0x0013F1E1
		internal void QueueInvalidateCellsPanelHorizontalOffset()
		{
			if (!this.CellsPanelHorizontalOffsetComputationPending)
			{
				base.Dispatcher.BeginInvoke(new DispatcherOperationCallback(this.InvalidateCellsPanelHorizontalOffset), DispatcherPriority.Loaded, new object[]
				{
					this
				});
				this.CellsPanelHorizontalOffsetComputationPending = true;
			}
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x00141018 File Offset: 0x0013F218
		private object InvalidateCellsPanelHorizontalOffset(object args)
		{
			if (!this.CellsPanelHorizontalOffsetComputationPending)
			{
				return null;
			}
			IProvideDataGridColumn anyCellOrColumnHeader = this.GetAnyCellOrColumnHeader();
			if (anyCellOrColumnHeader != null)
			{
				this.CellsPanelHorizontalOffset = DataGridHelper.GetParentCellsPanelHorizontalOffset(anyCellOrColumnHeader);
			}
			else if (!double.IsNaN(this.RowHeaderWidth))
			{
				this.CellsPanelHorizontalOffset = this.RowHeaderWidth;
			}
			else
			{
				this.CellsPanelHorizontalOffset = 0.0;
			}
			this.CellsPanelHorizontalOffsetComputationPending = false;
			return null;
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0014107C File Offset: 0x0013F27C
		internal IProvideDataGridColumn GetAnyCellOrColumnHeader()
		{
			if (this._rowTrackingRoot != null)
			{
				for (ContainerTracking<DataGridRow> containerTracking = this._rowTrackingRoot; containerTracking != null; containerTracking = containerTracking.Next)
				{
					if (containerTracking.Container.IsVisible)
					{
						DataGridCellsPresenter cellsPresenter = containerTracking.Container.CellsPresenter;
						if (cellsPresenter != null)
						{
							for (ContainerTracking<DataGridCell> containerTracking2 = cellsPresenter.CellTrackingRoot; containerTracking2 != null; containerTracking2 = containerTracking2.Next)
							{
								if (containerTracking2.Container.IsVisible)
								{
									return containerTracking2.Container;
								}
							}
						}
					}
				}
			}
			if (this.ColumnHeadersPresenter != null)
			{
				for (ContainerTracking<DataGridColumnHeader> containerTracking3 = this.ColumnHeadersPresenter.HeaderTrackingRoot; containerTracking3 != null; containerTracking3 = containerTracking3.Next)
				{
					if (containerTracking3.Container.IsVisible)
					{
						return containerTracking3.Container;
					}
				}
			}
			return null;
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x0014111C File Offset: 0x0013F31C
		internal double GetViewportWidthForColumns()
		{
			if (this.InternalScrollHost == null)
			{
				return 0.0;
			}
			double viewportWidth = this.InternalScrollHost.ViewportWidth;
			return viewportWidth - this.CellsPanelHorizontalOffset;
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x00141154 File Offset: 0x0013F354
		internal override void ChangeVisualState(bool useTransitions)
		{
			if (!base.IsEnabled)
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Disabled",
					"Normal"
				});
			}
			else
			{
				VisualStates.GoToState(this, useTransitions, new string[]
				{
					"Normal"
				});
			}
			base.ChangeVisualState(useTransitions);
		}

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x001411A4 File Offset: 0x0013F3A4
		internal static object NewItemPlaceholder
		{
			get
			{
				return DataGrid._newItemPlaceholder;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CanUserResizeColumns" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CanUserResizeColumns" /> dependency property.</returns>
		// Token: 0x040028B3 RID: 10419
		public static readonly DependencyProperty CanUserResizeColumnsProperty = DependencyProperty.Register("CanUserResizeColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnNotifyColumnAndColumnHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.ColumnWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.ColumnWidth" /> dependency property.</returns>
		// Token: 0x040028B4 RID: 10420
		public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth", typeof(DataGridLength), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridLength.SizeToHeader));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.MinColumnWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.MinColumnWidth" /> dependency property.</returns>
		// Token: 0x040028B5 RID: 10421
		public static readonly DependencyProperty MinColumnWidthProperty = DependencyProperty.Register("MinColumnWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(20.0, new PropertyChangedCallback(DataGrid.OnColumnSizeConstraintChanged)), new ValidateValueCallback(DataGrid.ValidateMinColumnWidth));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.MaxColumnWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.MaxColumnWidth" /> dependency property.</returns>
		// Token: 0x040028B6 RID: 10422
		public static readonly DependencyProperty MaxColumnWidthProperty = DependencyProperty.Register("MaxColumnWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DataGrid.OnColumnSizeConstraintChanged)), new ValidateValueCallback(DataGrid.ValidateMaxColumnWidth));

		// Token: 0x040028B7 RID: 10423
		private static readonly UncommonField<int> BringColumnIntoViewRetryCountField = new UncommonField<int>(0);

		// Token: 0x040028B8 RID: 10424
		private const int MaxBringColumnIntoViewRetries = 4;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.GridLinesVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.GridLinesVisibility" /> dependency property.</returns>
		// Token: 0x040028BA RID: 10426
		public static readonly DependencyProperty GridLinesVisibilityProperty = DependencyProperty.Register("GridLinesVisibility", typeof(DataGridGridLinesVisibility), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridGridLinesVisibility.All, new PropertyChangedCallback(DataGrid.OnNotifyGridLinePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.HorizontalGridLinesBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.HorizontalGridLinesBrush" /> dependency property.</returns>
		// Token: 0x040028BB RID: 10427
		public static readonly DependencyProperty HorizontalGridLinesBrushProperty = DependencyProperty.Register("HorizontalGridLinesBrush", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(Brushes.Black, new PropertyChangedCallback(DataGrid.OnNotifyGridLinePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.VerticalGridLinesBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.VerticalGridLinesBrush" /> dependency property.</returns>
		// Token: 0x040028BC RID: 10428
		public static readonly DependencyProperty VerticalGridLinesBrushProperty = DependencyProperty.Register("VerticalGridLinesBrush", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(Brushes.Black, new PropertyChangedCallback(DataGrid.OnNotifyGridLinePropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowStyle" /> dependency property.</returns>
		// Token: 0x040028BD RID: 10429
		public static readonly DependencyProperty RowStyleProperty = DependencyProperty.Register("RowStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnRowStyleChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowValidationErrorTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowValidationErrorTemplate" /> dependency property. </returns>
		// Token: 0x040028BE RID: 10430
		public static readonly DependencyProperty RowValidationErrorTemplateProperty = DependencyProperty.Register("RowValidationErrorTemplate", typeof(ControlTemplate), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowStyleSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowStyleSelector" /> dependency property.</returns>
		// Token: 0x040028BF RID: 10431
		public static readonly DependencyProperty RowStyleSelectorProperty = DependencyProperty.Register("RowStyleSelector", typeof(StyleSelector), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnRowStyleSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowBackground" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowBackground" /> dependency property.</returns>
		// Token: 0x040028C0 RID: 10432
		public static readonly DependencyProperty RowBackgroundProperty = DependencyProperty.Register("RowBackground", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.AlternatingRowBackground" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.AlternatingRowBackground" /> dependency property.</returns>
		// Token: 0x040028C1 RID: 10433
		public static readonly DependencyProperty AlternatingRowBackgroundProperty = DependencyProperty.Register("AlternatingRowBackground", typeof(Brush), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyDataGridAndRowPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowHeight" /> dependency property.</returns>
		// Token: 0x040028C2 RID: 10434
		public static readonly DependencyProperty RowHeightProperty = DependencyProperty.Register("RowHeight", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(DataGrid.OnNotifyCellsPresenterPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.MinRowHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.MinRowHeight" /> dependency property.</returns>
		// Token: 0x040028C3 RID: 10435
		public static readonly DependencyProperty MinRowHeightProperty = DependencyProperty.Register("MinRowHeight", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyCellsPresenterPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderWidth" /> dependency property.</returns>
		// Token: 0x040028C6 RID: 10438
		public static readonly DependencyProperty RowHeaderWidthProperty = DependencyProperty.Register("RowHeaderWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(DataGrid.OnNotifyRowHeaderWidthPropertyChanged)));

		// Token: 0x040028C7 RID: 10439
		private static readonly DependencyPropertyKey RowHeaderActualWidthPropertyKey = DependencyProperty.RegisterReadOnly("RowHeaderActualWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyRowHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderActualWidth" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderActualWidth" /> dependency property.</returns>
		// Token: 0x040028C8 RID: 10440
		public static readonly DependencyProperty RowHeaderActualWidthProperty = DataGrid.RowHeaderActualWidthPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.ColumnHeaderHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.ColumnHeaderHeight" /> dependency property.</returns>
		// Token: 0x040028C9 RID: 10441
		public static readonly DependencyProperty ColumnHeaderHeightProperty = DependencyProperty.Register("ColumnHeaderHeight", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(double.NaN, new PropertyChangedCallback(DataGrid.OnNotifyColumnHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.HeadersVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.HeadersVisibility" /> dependency property.</returns>
		// Token: 0x040028CA RID: 10442
		public static readonly DependencyProperty HeadersVisibilityProperty = DependencyProperty.Register("HeadersVisibility", typeof(DataGridHeadersVisibility), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridHeadersVisibility.All));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CellStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CellStyle" /> dependency property.</returns>
		// Token: 0x040028CB RID: 10443
		public static readonly DependencyProperty CellStyleProperty = DependencyProperty.Register("CellStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyColumnAndCellPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.ColumnHeaderStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.ColumnHeaderStyle" /> dependency property.</returns>
		// Token: 0x040028CC RID: 10444
		public static readonly DependencyProperty ColumnHeaderStyleProperty = DependencyProperty.Register("ColumnHeaderStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyColumnAndColumnHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderStyle" /> dependency property.</returns>
		// Token: 0x040028CD RID: 10445
		public static readonly DependencyProperty RowHeaderStyleProperty = DependencyProperty.Register("RowHeaderStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndRowHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderTemplate" /> dependency property.</returns>
		// Token: 0x040028CE RID: 10446
		public static readonly DependencyProperty RowHeaderTemplateProperty = DependencyProperty.Register("RowHeaderTemplate", typeof(DataTemplate), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndRowHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowHeaderTemplateSelector" /> dependency property.</returns>
		// Token: 0x040028CF RID: 10447
		public static readonly DependencyProperty RowHeaderTemplateSelectorProperty = DependencyProperty.Register("RowHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndRowHeaderPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.HorizontalScrollBarVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.HorizontalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x040028D0 RID: 10448
		public static readonly DependencyProperty HorizontalScrollBarVisibilityProperty = ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(typeof(DataGrid), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.VerticalScrollBarVisibility" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.VerticalScrollBarVisibility" /> dependency property.</returns>
		// Token: 0x040028D1 RID: 10449
		public static readonly DependencyProperty VerticalScrollBarVisibilityProperty = ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(typeof(DataGrid), new FrameworkPropertyMetadata(ScrollBarVisibility.Auto));

		// Token: 0x040028D2 RID: 10450
		internal static readonly DependencyProperty HorizontalScrollOffsetProperty = DependencyProperty.Register("HorizontalScrollOffset", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyHorizontalOffsetPropertyChanged)));

		/// <summary>Represents the command that indicates the intention to begin editing the current cell or row of the <see cref="T:System.Windows.Controls.DataGrid" />.</summary>
		// Token: 0x040028D3 RID: 10451
		public static readonly RoutedCommand BeginEditCommand = new RoutedCommand("BeginEdit", typeof(DataGrid));

		/// <summary>Represents the command that indicates the intention to commit pending changes to the current cell or row and exit edit mode.</summary>
		// Token: 0x040028D4 RID: 10452
		public static readonly RoutedCommand CommitEditCommand = new RoutedCommand("CommitEdit", typeof(DataGrid));

		/// <summary>Represents the command that indicates the intention to cancel any pending changes to the current cell or row and revert to the state before the <see cref="F:System.Windows.Controls.DataGrid.BeginEditCommand" /> command was executed.</summary>
		// Token: 0x040028D5 RID: 10453
		public static readonly RoutedCommand CancelEditCommand = new RoutedCommand("CancelEdit", typeof(DataGrid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.IsReadOnly" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.IsReadOnly" /> dependency property.</returns>
		// Token: 0x040028D8 RID: 10456
		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGrid.OnIsReadOnlyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CurrentItem" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CurrentItem" /> dependency property.</returns>
		// Token: 0x040028D9 RID: 10457
		public static readonly DependencyProperty CurrentItemProperty = DependencyProperty.Register("CurrentItem", typeof(object), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnCurrentItemChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CurrentColumn" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CurrentColumn" /> dependency property.</returns>
		// Token: 0x040028DA RID: 10458
		public static readonly DependencyProperty CurrentColumnProperty = DependencyProperty.Register("CurrentColumn", typeof(DataGridColumn), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnCurrentColumnChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CurrentCell" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CurrentCell" /> dependency property.</returns>
		// Token: 0x040028DB RID: 10459
		public static readonly DependencyProperty CurrentCellProperty = DependencyProperty.Register("CurrentCell", typeof(DataGridCellInfo), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridCellInfo.Unset, new PropertyChangedCallback(DataGrid.OnCurrentCellChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CanUserAddRows" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CanUserAddRows" /> dependency property.</returns>
		// Token: 0x040028DF RID: 10463
		public static readonly DependencyProperty CanUserAddRowsProperty = DependencyProperty.Register("CanUserAddRows", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnCanUserAddRowsChanged), new CoerceValueCallback(DataGrid.OnCoerceCanUserAddRows)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CanUserDeleteRows" /> dependency property.</summary>
		/// <returns>Identifier for the <see cref="P:System.Windows.Controls.DataGrid.CanUserDeleteRows" /> dependency property.</returns>
		// Token: 0x040028E0 RID: 10464
		public static readonly DependencyProperty CanUserDeleteRowsProperty = DependencyProperty.Register("CanUserDeleteRows", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnCanUserDeleteRowsChanged), new CoerceValueCallback(DataGrid.OnCoerceCanUserDeleteRows)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowDetailsVisibilityMode" /> dependency property.</summary>
		/// <returns>Identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowDetailsVisibilityMode" /> dependency property.</returns>
		// Token: 0x040028E3 RID: 10467
		public static readonly DependencyProperty RowDetailsVisibilityModeProperty = DependencyProperty.Register("RowDetailsVisibilityMode", typeof(DataGridRowDetailsVisibilityMode), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridRowDetailsVisibilityMode.VisibleWhenSelected, new PropertyChangedCallback(DataGrid.OnNotifyRowAndDetailsPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.AreRowDetailsFrozen" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.AreRowDetailsFrozen" /> dependency property.</returns>
		// Token: 0x040028E4 RID: 10468
		public static readonly DependencyProperty AreRowDetailsFrozenProperty = DependencyProperty.Register("AreRowDetailsFrozen", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(false));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowDetailsTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowDetailsTemplate" /> dependency property.</returns>
		// Token: 0x040028E5 RID: 10469
		public static readonly DependencyProperty RowDetailsTemplateProperty = DependencyProperty.Register("RowDetailsTemplate", typeof(DataTemplate), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndDetailsPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.RowDetailsTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.RowDetailsTemplateSelector" /> dependency property.</returns>
		// Token: 0x040028E6 RID: 10470
		public static readonly DependencyProperty RowDetailsTemplateSelectorProperty = DependencyProperty.Register("RowDetailsTemplateSelector", typeof(DataTemplateSelector), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyRowAndDetailsPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CanUserResizeRows" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CanUserResizeRows" /> dependency property.</returns>
		// Token: 0x040028EA RID: 10474
		public static readonly DependencyProperty CanUserResizeRowsProperty = DependencyProperty.Register("CanUserResizeRows", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnNotifyRowHeaderPropertyChanged)));

		// Token: 0x040028EB RID: 10475
		private static readonly DependencyPropertyKey NewItemMarginPropertyKey = DependencyProperty.RegisterReadOnly("NewItemMargin", typeof(Thickness), typeof(DataGrid), new FrameworkPropertyMetadata(new Thickness(0.0)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.NewItemMargin" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.NewItemMargin" /> dependency property.</returns>
		// Token: 0x040028EC RID: 10476
		public static readonly DependencyProperty NewItemMarginProperty = DataGrid.NewItemMarginPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.SelectionMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.SelectionMode" /> dependency property.</returns>
		// Token: 0x040028EE RID: 10478
		public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode", typeof(DataGridSelectionMode), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridSelectionMode.Extended, new PropertyChangedCallback(DataGrid.OnSelectionModeChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.SelectionUnit" /> dependency property.</summary>
		/// <returns>Identifier for the <see cref="P:System.Windows.Controls.DataGrid.SelectionUnit" /> dependency property.</returns>
		// Token: 0x040028EF RID: 10479
		public static readonly DependencyProperty SelectionUnitProperty = DependencyProperty.Register("SelectionUnit", typeof(DataGridSelectionUnit), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridSelectionUnit.FullRow, new PropertyChangedCallback(DataGrid.OnSelectionUnitChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CanUserSortColumns" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CanUserSortColumns" /> dependency property.</returns>
		// Token: 0x040028F0 RID: 10480
		public static readonly DependencyProperty CanUserSortColumnsProperty = DependencyProperty.Register("CanUserSortColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnCanUserSortColumnsPropertyChanged), new CoerceValueCallback(DataGrid.OnCoerceCanUserSortColumns)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.AutoGenerateColumns" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.AutoGenerateColumns" /> dependency property.</returns>
		// Token: 0x040028F4 RID: 10484
		public static readonly DependencyProperty AutoGenerateColumnsProperty = DependencyProperty.Register("AutoGenerateColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnAutoGenerateColumnsPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.FrozenColumnCount" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.FrozenColumnCount" /> dependency property.</returns>
		// Token: 0x040028F6 RID: 10486
		public static readonly DependencyProperty FrozenColumnCountProperty = DependencyProperty.Register("FrozenColumnCount", typeof(int), typeof(DataGrid), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(DataGrid.OnFrozenColumnCountPropertyChanged), new CoerceValueCallback(DataGrid.OnCoerceFrozenColumnCount)), new ValidateValueCallback(DataGrid.ValidateFrozenColumnCount));

		// Token: 0x040028F7 RID: 10487
		private static readonly DependencyPropertyKey NonFrozenColumnsViewportHorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("NonFrozenColumnsViewportHorizontalOffset", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.NonFrozenColumnsViewportHorizontalOffset" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.NonFrozenColumnsViewportHorizontalOffset" /> dependency property.</returns>
		// Token: 0x040028F8 RID: 10488
		public static readonly DependencyProperty NonFrozenColumnsViewportHorizontalOffsetProperty = DataGrid.NonFrozenColumnsViewportHorizontalOffsetPropertyKey.DependencyProperty;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.EnableRowVirtualization" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.EnableRowVirtualization" /> dependency property.</returns>
		// Token: 0x040028F9 RID: 10489
		public static readonly DependencyProperty EnableRowVirtualizationProperty = DependencyProperty.Register("EnableRowVirtualization", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnEnableRowVirtualizationChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.EnableColumnVirtualization" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.EnableColumnVirtualization" /> dependency property.</returns>
		// Token: 0x040028FA RID: 10490
		public static readonly DependencyProperty EnableColumnVirtualizationProperty = DependencyProperty.Register("EnableColumnVirtualization", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGrid.OnEnableColumnVirtualizationChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CanUserReorderColumns" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CanUserReorderColumns" /> dependency property.</returns>
		// Token: 0x040028FB RID: 10491
		public static readonly DependencyProperty CanUserReorderColumnsProperty = DependencyProperty.Register("CanUserReorderColumns", typeof(bool), typeof(DataGrid), new FrameworkPropertyMetadata(true, new PropertyChangedCallback(DataGrid.OnNotifyColumnPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.DragIndicatorStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.DragIndicatorStyle" /> dependency property.</returns>
		// Token: 0x040028FC RID: 10492
		public static readonly DependencyProperty DragIndicatorStyleProperty = DependencyProperty.Register("DragIndicatorStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGrid.OnNotifyColumnPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.DropLocationIndicatorStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.DropLocationIndicatorStyle" /> dependency property.</returns>
		// Token: 0x040028FD RID: 10493
		public static readonly DependencyProperty DropLocationIndicatorStyleProperty = DependencyProperty.Register("DropLocationIndicatorStyle", typeof(Style), typeof(DataGrid), new FrameworkPropertyMetadata(null));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.ClipboardCopyMode" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.ClipboardCopyMode" /> dependency property.</returns>
		// Token: 0x04002903 RID: 10499
		public static readonly DependencyProperty ClipboardCopyModeProperty = DependencyProperty.Register("ClipboardCopyMode", typeof(DataGridClipboardCopyMode), typeof(DataGrid), new FrameworkPropertyMetadata(DataGridClipboardCopyMode.ExcludeHeader, new PropertyChangedCallback(DataGrid.OnClipboardCopyModeChanged)));

		// Token: 0x04002905 RID: 10501
		internal static readonly DependencyProperty CellsPanelActualWidthProperty = DependencyProperty.Register("CellsPanelActualWidth", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.CellsPanelActualWidthChanged)));

		// Token: 0x04002906 RID: 10502
		private static readonly DependencyPropertyKey CellsPanelHorizontalOffsetPropertyKey = DependencyProperty.RegisterReadOnly("CellsPanelHorizontalOffset", typeof(double), typeof(DataGrid), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DataGrid.OnNotifyHorizontalOffsetPropertyChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGrid.CellsPanelHorizontalOffset" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGrid.CellsPanelHorizontalOffset" /> dependency property.</returns>
		// Token: 0x04002907 RID: 10503
		public static readonly DependencyProperty CellsPanelHorizontalOffsetProperty = DataGrid.CellsPanelHorizontalOffsetPropertyKey.DependencyProperty;

		// Token: 0x04002909 RID: 10505
		private static IValueConverter _headersVisibilityConverter;

		// Token: 0x0400290A RID: 10506
		private static IValueConverter _rowDetailsScrollingConverter;

		// Token: 0x0400290B RID: 10507
		private static object _newItemPlaceholder = new NamedObject("DataGrid.NewItemPlaceholder");

		// Token: 0x0400290C RID: 10508
		private DataGridColumnCollection _columns;

		// Token: 0x0400290D RID: 10509
		private ContainerTracking<DataGridRow> _rowTrackingRoot;

		// Token: 0x0400290E RID: 10510
		private DataGridColumnHeadersPresenter _columnHeadersPresenter;

		// Token: 0x0400290F RID: 10511
		private DataGridCell _currentCellContainer;

		// Token: 0x04002910 RID: 10512
		private DataGridCell _pendingCurrentCellContainer;

		// Token: 0x04002911 RID: 10513
		private SelectedCellsCollection _selectedCells;

		// Token: 0x04002912 RID: 10514
		private List<ItemsControl.ItemInfo> _pendingInfos;

		// Token: 0x04002913 RID: 10515
		private DataGridCellInfo? _selectionAnchor;

		// Token: 0x04002914 RID: 10516
		private bool _isDraggingSelection;

		// Token: 0x04002915 RID: 10517
		private bool _isRowDragging;

		// Token: 0x04002916 RID: 10518
		private Panel _internalItemsHost;

		// Token: 0x04002917 RID: 10519
		private ScrollViewer _internalScrollHost;

		// Token: 0x04002918 RID: 10520
		private ScrollContentPresenter _internalScrollContentPresenter;

		// Token: 0x04002919 RID: 10521
		private DispatcherTimer _autoScrollTimer;

		// Token: 0x0400291A RID: 10522
		private bool _hasAutoScrolled;

		// Token: 0x0400291B RID: 10523
		private VirtualizedCellInfoCollection _pendingSelectedCells;

		// Token: 0x0400291C RID: 10524
		private VirtualizedCellInfoCollection _pendingUnselectedCells;

		// Token: 0x0400291D RID: 10525
		private bool _measureNeverInvoked = true;

		// Token: 0x0400291E RID: 10526
		private bool _updatingSelectedCells;

		// Token: 0x0400291F RID: 10527
		private Visibility _placeholderVisibility = Visibility.Collapsed;

		// Token: 0x04002920 RID: 10528
		private Point _dragPoint;

		// Token: 0x04002921 RID: 10529
		private List<int> _groupingSortDescriptionIndices;

		// Token: 0x04002922 RID: 10530
		private bool _ignoreSortDescriptionsChange;

		// Token: 0x04002923 RID: 10531
		private bool _sortingStarted;

		// Token: 0x04002924 RID: 10532
		private ObservableCollection<ValidationRule> _rowValidationRules;

		// Token: 0x04002925 RID: 10533
		private BindingGroup _defaultBindingGroup;

		// Token: 0x04002926 RID: 10534
		private ItemsControl.ItemInfo _editingRowInfo;

		// Token: 0x04002927 RID: 10535
		private bool _hasCellValidationError;

		// Token: 0x04002928 RID: 10536
		private bool _hasRowValidationError;

		// Token: 0x04002929 RID: 10537
		private IEnumerable _cachedItemsSource;

		// Token: 0x0400292A RID: 10538
		private DataGridItemAttachedStorage _itemAttachedStorage = new DataGridItemAttachedStorage();

		// Token: 0x0400292B RID: 10539
		private bool _viewportWidthChangeNotificationPending;

		// Token: 0x0400292C RID: 10540
		private double _originalViewportWidth;

		// Token: 0x0400292D RID: 10541
		private double _finalViewportWidth;

		// Token: 0x0400292E RID: 10542
		private Dictionary<DataGridColumn, DataGrid.CellAutomationValueHolder> _editingCellAutomationValueHolders = new Dictionary<DataGridColumn, DataGrid.CellAutomationValueHolder>();

		// Token: 0x0400292F RID: 10543
		private DataGridCell _focusedCell;

		// Token: 0x04002930 RID: 10544
		private bool _newItemMarginComputationPending;

		// Token: 0x04002931 RID: 10545
		private const string ItemsPanelPartName = "PART_RowsPresenter";

		// Token: 0x02000963 RID: 2403
		private class ChangingSelectedCellsHelper : IDisposable
		{
			// Token: 0x06008736 RID: 34614 RVA: 0x0024F3C5 File Offset: 0x0024D5C5
			internal ChangingSelectedCellsHelper(DataGrid dataGrid)
			{
				this._dataGrid = dataGrid;
				this._wasUpdatingSelectedCells = this._dataGrid.IsUpdatingSelectedCells;
				if (!this._wasUpdatingSelectedCells)
				{
					this._dataGrid.BeginUpdateSelectedCells();
				}
			}

			// Token: 0x06008737 RID: 34615 RVA: 0x0024F3F8 File Offset: 0x0024D5F8
			public void Dispose()
			{
				GC.SuppressFinalize(this);
				if (!this._wasUpdatingSelectedCells)
				{
					this._dataGrid.EndUpdateSelectedCells();
				}
			}

			// Token: 0x04004418 RID: 17432
			private DataGrid _dataGrid;

			// Token: 0x04004419 RID: 17433
			private bool _wasUpdatingSelectedCells;
		}

		// Token: 0x02000964 RID: 2404
		[Flags]
		private enum RelativeMousePositions
		{
			// Token: 0x0400441B RID: 17435
			Over = 0,
			// Token: 0x0400441C RID: 17436
			Above = 1,
			// Token: 0x0400441D RID: 17437
			Below = 2,
			// Token: 0x0400441E RID: 17438
			Left = 4,
			// Token: 0x0400441F RID: 17439
			Right = 8
		}

		// Token: 0x02000965 RID: 2405
		internal class CellAutomationValueHolder
		{
			// Token: 0x06008738 RID: 34616 RVA: 0x0024F413 File Offset: 0x0024D613
			public CellAutomationValueHolder(DataGridCell cell)
			{
				this._cell = cell;
				this.Initialize(cell.RowDataItem, cell.Column);
			}

			// Token: 0x06008739 RID: 34617 RVA: 0x0024F434 File Offset: 0x0024D634
			public CellAutomationValueHolder(object item, DataGridColumn column)
			{
				this.Initialize(item, column);
			}

			// Token: 0x0600873A RID: 34618 RVA: 0x0024F444 File Offset: 0x0024D644
			private void Initialize(object item, DataGridColumn column)
			{
				this._item = item;
				this._column = column;
				this._value = this.GetValue();
			}

			// Token: 0x17001E8C RID: 7820
			// (get) Token: 0x0600873B RID: 34619 RVA: 0x0024F460 File Offset: 0x0024D660
			public string Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x0600873C RID: 34620 RVA: 0x0024F468 File Offset: 0x0024D668
			public void TrackValue()
			{
				string value = this.GetValue();
				if (value != this._value)
				{
					if (AutomationPeer.ListenerExists(AutomationEvents.PropertyChanged))
					{
						DataGridColumn dataGridColumn = (this._cell != null) ? this._cell.Column : this._column;
						DataGridAutomationPeer dataGridAutomationPeer = UIElementAutomationPeer.FromElement(dataGridColumn.DataGridOwner) as DataGridAutomationPeer;
						if (dataGridAutomationPeer != null)
						{
							object item = (this._cell != null) ? this._cell.DataContext : this._item;
							DataGridItemAutomationPeer dataGridItemAutomationPeer = dataGridAutomationPeer.FindOrCreateItemAutomationPeer(item) as DataGridItemAutomationPeer;
							if (dataGridItemAutomationPeer != null)
							{
								DataGridCellItemAutomationPeer orCreateCellItemPeer = dataGridItemAutomationPeer.GetOrCreateCellItemPeer(dataGridColumn);
								if (orCreateCellItemPeer != null)
								{
									orCreateCellItemPeer.RaisePropertyChangedEvent(ValuePatternIdentifiers.ValueProperty, this._value, value);
								}
							}
						}
					}
					this._value = value;
				}
			}

			// Token: 0x0600873D RID: 34621 RVA: 0x0024F51C File Offset: 0x0024D71C
			private string GetValue()
			{
				string result;
				if (this._column.ClipboardContentBinding == null)
				{
					result = null;
				}
				else if (this._inSetValue)
				{
					result = (string)this._cell.GetValue(DataGrid.CellAutomationValueHolder.CellContentProperty);
				}
				else
				{
					FrameworkElement frameworkElement;
					if (this._cell != null)
					{
						frameworkElement = this._cell;
					}
					else
					{
						frameworkElement = new FrameworkElement();
						frameworkElement.DataContext = this._item;
					}
					BindingOperations.SetBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellContentProperty, this._column.ClipboardContentBinding);
					result = (string)frameworkElement.GetValue(DataGrid.CellAutomationValueHolder.CellContentProperty);
					BindingOperations.ClearBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellContentProperty);
				}
				return result;
			}

			// Token: 0x0600873E RID: 34622 RVA: 0x0024F5B4 File Offset: 0x0024D7B4
			public object GetClipboardValue()
			{
				object result;
				if (this._column.ClipboardContentBinding == null)
				{
					result = null;
				}
				else
				{
					FrameworkElement frameworkElement;
					if (this._cell != null)
					{
						frameworkElement = this._cell;
					}
					else
					{
						frameworkElement = new FrameworkElement();
						frameworkElement.DataContext = this._item;
					}
					BindingOperations.SetBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellClipboardProperty, this._column.ClipboardContentBinding);
					result = frameworkElement.GetValue(DataGrid.CellAutomationValueHolder.CellClipboardProperty);
					BindingOperations.ClearBinding(frameworkElement, DataGrid.CellAutomationValueHolder.CellClipboardProperty);
				}
				return result;
			}

			// Token: 0x0600873F RID: 34623 RVA: 0x0024F624 File Offset: 0x0024D824
			public void SetValue(DataGrid dataGrid, object value, bool clipboard)
			{
				if (this._column.ClipboardContentBinding == null)
				{
					return;
				}
				this._inSetValue = true;
				DependencyProperty dp = clipboard ? DataGrid.CellAutomationValueHolder.CellClipboardProperty : DataGrid.CellAutomationValueHolder.CellContentProperty;
				BindingBase binding = this._column.ClipboardContentBinding.Clone(BindingMode.TwoWay);
				BindingOperations.SetBinding(this._cell, dp, binding);
				this._cell.SetValue(dp, value);
				dataGrid.CommitEdit();
				BindingOperations.ClearBinding(this._cell, dp);
				this._inSetValue = false;
			}

			// Token: 0x04004420 RID: 17440
			private static DependencyProperty CellContentProperty = DependencyProperty.RegisterAttached("CellContent", typeof(string), typeof(DataGrid.CellAutomationValueHolder));

			// Token: 0x04004421 RID: 17441
			private static DependencyProperty CellClipboardProperty = DependencyProperty.RegisterAttached("CellClipboard", typeof(object), typeof(DataGrid.CellAutomationValueHolder));

			// Token: 0x04004422 RID: 17442
			private DataGridCell _cell;

			// Token: 0x04004423 RID: 17443
			private DataGridColumn _column;

			// Token: 0x04004424 RID: 17444
			private object _item;

			// Token: 0x04004425 RID: 17445
			private string _value;

			// Token: 0x04004426 RID: 17446
			private bool _inSetValue;
		}
	}
}
