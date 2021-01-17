using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020004AC RID: 1196
	internal static class DataGridHelper
	{
		// Token: 0x060048C1 RID: 18625 RVA: 0x0014A63C File Offset: 0x0014883C
		public static Size SubtractFromSize(Size size, double thickness, bool height)
		{
			if (height)
			{
				return new Size(size.Width, Math.Max(0.0, size.Height - thickness));
			}
			return new Size(Math.Max(0.0, size.Width - thickness), size.Height);
		}

		// Token: 0x060048C2 RID: 18626 RVA: 0x0014A694 File Offset: 0x00148894
		public static bool IsGridLineVisible(DataGrid dataGrid, bool isHorizontal)
		{
			if (dataGrid != null)
			{
				switch (dataGrid.GridLinesVisibility)
				{
				case DataGridGridLinesVisibility.All:
					return true;
				case DataGridGridLinesVisibility.Horizontal:
					return isHorizontal;
				case DataGridGridLinesVisibility.None:
					return false;
				case DataGridGridLinesVisibility.Vertical:
					return !isHorizontal;
				}
			}
			return false;
		}

		// Token: 0x060048C3 RID: 18627 RVA: 0x0014A6CF File Offset: 0x001488CF
		public static bool ShouldNotifyCells(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.Cells);
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x0014A6D8 File Offset: 0x001488D8
		public static bool ShouldNotifyCellsPresenter(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.CellsPresenter);
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x0014A6E1 File Offset: 0x001488E1
		public static bool ShouldNotifyColumns(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.Columns);
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x0014A6EA File Offset: 0x001488EA
		public static bool ShouldNotifyColumnHeaders(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.ColumnHeaders);
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x0014A6F4 File Offset: 0x001488F4
		public static bool ShouldNotifyColumnHeadersPresenter(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.ColumnHeadersPresenter);
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x0014A6FE File Offset: 0x001488FE
		public static bool ShouldNotifyColumnCollection(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.ColumnCollection);
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x0014A707 File Offset: 0x00148907
		public static bool ShouldNotifyDataGrid(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.DataGrid);
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x0014A711 File Offset: 0x00148911
		public static bool ShouldNotifyDetailsPresenter(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.DetailsPresenter);
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x0014A71E File Offset: 0x0014891E
		public static bool ShouldRefreshCellContent(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.RefreshCellContent);
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x0014A72B File Offset: 0x0014892B
		public static bool ShouldNotifyRowHeaders(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.RowHeaders);
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x0014A738 File Offset: 0x00148938
		public static bool ShouldNotifyRows(DataGridNotificationTarget target)
		{
			return DataGridHelper.TestTarget(target, DataGridNotificationTarget.Rows);
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x0014A748 File Offset: 0x00148948
		public static bool ShouldNotifyRowSubtree(DataGridNotificationTarget target)
		{
			DataGridNotificationTarget value = DataGridNotificationTarget.Cells | DataGridNotificationTarget.CellsPresenter | DataGridNotificationTarget.DetailsPresenter | DataGridNotificationTarget.RefreshCellContent | DataGridNotificationTarget.RowHeaders | DataGridNotificationTarget.Rows;
			return DataGridHelper.TestTarget(target, value);
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x00003EA7 File Offset: 0x000020A7
		private static bool TestTarget(DataGridNotificationTarget target, DataGridNotificationTarget value)
		{
			return (target & value) > DataGridNotificationTarget.None;
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x0014A764 File Offset: 0x00148964
		public static T FindParent<T>(FrameworkElement element) where T : FrameworkElement
		{
			for (FrameworkElement frameworkElement = element.TemplatedParent as FrameworkElement; frameworkElement != null; frameworkElement = (frameworkElement.TemplatedParent as FrameworkElement))
			{
				T t = frameworkElement as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x0014A7B0 File Offset: 0x001489B0
		public static T FindVisualParent<T>(UIElement element) where T : UIElement
		{
			for (UIElement uielement = element; uielement != null; uielement = (VisualTreeHelper.GetParent(uielement) as UIElement))
			{
				T t = uielement as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0014A7F0 File Offset: 0x001489F0
		public static bool TreeHasFocusAndTabStop(DependencyObject element)
		{
			if (element == null)
			{
				return false;
			}
			UIElement uielement = element as UIElement;
			if (uielement != null)
			{
				if (uielement.Focusable && KeyboardNavigation.GetIsTabStop(uielement))
				{
					return true;
				}
			}
			else
			{
				ContentElement contentElement = element as ContentElement;
				if (contentElement != null && contentElement.Focusable && KeyboardNavigation.GetIsTabStop(contentElement))
				{
					return true;
				}
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				if (DataGridHelper.TreeHasFocusAndTabStop(child))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060048D3 RID: 18643 RVA: 0x0014A864 File Offset: 0x00148A64
		public static void OnColumnWidthChanged(IProvideDataGridColumn cell, DependencyPropertyChangedEventArgs e)
		{
			UIElement uielement = (UIElement)cell;
			DataGridColumn column = cell.Column;
			bool flag = cell is DataGridColumnHeader;
			if (column != null)
			{
				DataGridLength width = column.Width;
				if (width.IsAuto || (!flag && width.IsSizeToCells) || (flag && width.IsSizeToHeader))
				{
					DataGridLength dataGridLength = (DataGridLength)e.OldValue;
					double num;
					if (dataGridLength.UnitType != width.UnitType)
					{
						double constraintWidth = column.GetConstraintWidth(flag);
						if (!DoubleUtil.AreClose(uielement.DesiredSize.Width, constraintWidth))
						{
							uielement.InvalidateMeasure();
							uielement.Measure(new Size(constraintWidth, double.PositiveInfinity));
						}
						num = uielement.DesiredSize.Width;
					}
					else
					{
						num = dataGridLength.DesiredValue;
					}
					if (DoubleUtil.IsNaN(width.DesiredValue) || DoubleUtil.LessThan(width.DesiredValue, num))
					{
						column.SetWidthInternal(new DataGridLength(width.Value, width.UnitType, num, width.DisplayValue));
					}
				}
			}
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x0014A980 File Offset: 0x00148B80
		public static Geometry GetFrozenClipForCell(IProvideDataGridColumn cell)
		{
			DataGridCellsPanel parentPanelForCell = DataGridHelper.GetParentPanelForCell(cell);
			if (parentPanelForCell != null)
			{
				return parentPanelForCell.GetFrozenClipForChild((UIElement)cell);
			}
			return null;
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x0014A9A8 File Offset: 0x00148BA8
		public static DataGridCellsPanel GetParentPanelForCell(IProvideDataGridColumn cell)
		{
			UIElement reference = (UIElement)cell;
			return VisualTreeHelper.GetParent(reference) as DataGridCellsPanel;
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0014A9C8 File Offset: 0x00148BC8
		public static double GetParentCellsPanelHorizontalOffset(IProvideDataGridColumn cell)
		{
			DataGridCellsPanel parentPanelForCell = DataGridHelper.GetParentPanelForCell(cell);
			if (parentPanelForCell != null)
			{
				return parentPanelForCell.ComputeCellsPanelHorizontalOffset();
			}
			return 0.0;
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x0014A9F0 File Offset: 0x00148BF0
		public static bool IsDefaultValue(DependencyObject d, DependencyProperty dp)
		{
			return DependencyPropertyHelper.GetValueSource(d, dp).BaseValueSource == BaseValueSource.Default;
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x0014AA0F File Offset: 0x00148C0F
		public static object GetCoercedTransferPropertyValue(DependencyObject baseObject, object baseValue, DependencyProperty baseProperty, DependencyObject parentObject, DependencyProperty parentProperty)
		{
			return DataGridHelper.GetCoercedTransferPropertyValue(baseObject, baseValue, baseProperty, parentObject, parentProperty, null, null);
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x0014AA20 File Offset: 0x00148C20
		public static object GetCoercedTransferPropertyValue(DependencyObject baseObject, object baseValue, DependencyProperty baseProperty, DependencyObject parentObject, DependencyProperty parentProperty, DependencyObject grandParentObject, DependencyProperty grandParentProperty)
		{
			object result = baseValue;
			if (DataGridHelper.IsPropertyTransferEnabled(baseObject, baseProperty))
			{
				BaseValueSource baseValueSource = DependencyPropertyHelper.GetValueSource(baseObject, baseProperty).BaseValueSource;
				if (parentObject != null)
				{
					ValueSource valueSource = DependencyPropertyHelper.GetValueSource(parentObject, parentProperty);
					if (valueSource.BaseValueSource > baseValueSource)
					{
						result = parentObject.GetValue(parentProperty);
						baseValueSource = valueSource.BaseValueSource;
					}
				}
				if (grandParentObject != null)
				{
					ValueSource valueSource2 = DependencyPropertyHelper.GetValueSource(grandParentObject, grandParentProperty);
					if (valueSource2.BaseValueSource > baseValueSource)
					{
						result = grandParentObject.GetValue(grandParentProperty);
						baseValueSource = valueSource2.BaseValueSource;
					}
				}
			}
			return result;
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x0014AA9C File Offset: 0x00148C9C
		public static void TransferProperty(DependencyObject d, DependencyProperty p)
		{
			Dictionary<DependencyProperty, bool> propertyTransferEnabledMapForObject = DataGridHelper.GetPropertyTransferEnabledMapForObject(d);
			propertyTransferEnabledMapForObject[p] = true;
			d.CoerceValue(p);
			propertyTransferEnabledMapForObject[p] = false;
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x0014AAC8 File Offset: 0x00148CC8
		private static Dictionary<DependencyProperty, bool> GetPropertyTransferEnabledMapForObject(DependencyObject d)
		{
			Dictionary<DependencyProperty, bool> dictionary;
			if (!DataGridHelper._propertyTransferEnabledMap.TryGetValue(d, out dictionary))
			{
				dictionary = new Dictionary<DependencyProperty, bool>();
				DataGridHelper._propertyTransferEnabledMap.Add(d, dictionary);
			}
			return dictionary;
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x0014AAF8 File Offset: 0x00148CF8
		internal static bool IsPropertyTransferEnabled(DependencyObject d, DependencyProperty p)
		{
			Dictionary<DependencyProperty, bool> dictionary;
			bool flag;
			return DataGridHelper._propertyTransferEnabledMap.TryGetValue(d, out dictionary) && dictionary.TryGetValue(p, out flag) && flag;
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x0014AB24 File Offset: 0x00148D24
		internal static bool IsOneWay(BindingBase bindingBase)
		{
			if (bindingBase == null)
			{
				return false;
			}
			Binding binding = bindingBase as Binding;
			if (binding != null)
			{
				return binding.Mode == BindingMode.OneWay;
			}
			MultiBinding multiBinding = bindingBase as MultiBinding;
			if (multiBinding != null)
			{
				return multiBinding.Mode == BindingMode.OneWay;
			}
			PriorityBinding priorityBinding = bindingBase as PriorityBinding;
			if (priorityBinding != null)
			{
				Collection<BindingBase> bindings = priorityBinding.Bindings;
				int count = bindings.Count;
				for (int i = 0; i < count; i++)
				{
					if (DataGridHelper.IsOneWay(bindings[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060048DE RID: 18654 RVA: 0x0014AB9A File Offset: 0x00148D9A
		internal static BindingExpression GetBindingExpression(FrameworkElement element, DependencyProperty dp)
		{
			if (element != null)
			{
				return element.GetBindingExpression(dp);
			}
			return null;
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x0014ABA8 File Offset: 0x00148DA8
		internal static bool ValidateWithoutUpdate(FrameworkElement element)
		{
			bool flag = true;
			BindingGroup bindingGroup = element.BindingGroup;
			DataGridCell dataGridCell = (element != null) ? (element.Parent as DataGridCell) : null;
			if (bindingGroup != null && dataGridCell != null)
			{
				Collection<BindingExpressionBase> bindingExpressions = bindingGroup.BindingExpressions;
				BindingExpressionBase[] array = new BindingExpressionBase[bindingExpressions.Count];
				bindingExpressions.CopyTo(array, 0);
				foreach (BindingExpressionBase bindingExpressionBase in array)
				{
					if (DataGridHelper.BindingExpressionBelongsToElement<DataGridCell>(bindingExpressionBase, dataGridCell))
					{
						flag = (bindingExpressionBase.ValidateWithoutUpdate() && flag);
					}
				}
			}
			return flag;
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x0014AC24 File Offset: 0x00148E24
		internal static bool BindingExpressionBelongsToElement<T>(BindingExpressionBase beb, T element) where T : FrameworkElement
		{
			DependencyObject targetElement = beb.TargetElement;
			if (targetElement != null)
			{
				DependencyObject dependencyObject = DataGridHelper.FindContextElement(beb);
				if (dependencyObject == null)
				{
					dependencyObject = targetElement;
				}
				if (dependencyObject is Visual || dependencyObject is Visual3D)
				{
					return VisualTreeHelper.IsAncestorOf(element, dependencyObject, typeof(T));
				}
			}
			return false;
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x0014AC70 File Offset: 0x00148E70
		private static DependencyObject FindContextElement(BindingExpressionBase beb)
		{
			BindingExpression bindingExpression;
			if ((bindingExpression = (beb as BindingExpression)) != null)
			{
				return bindingExpression.ContextElement;
			}
			ReadOnlyCollection<BindingExpressionBase> readOnlyCollection = null;
			MultiBindingExpression multiBindingExpression;
			PriorityBindingExpression priorityBindingExpression;
			if ((multiBindingExpression = (beb as MultiBindingExpression)) != null)
			{
				readOnlyCollection = multiBindingExpression.BindingExpressions;
			}
			else if ((priorityBindingExpression = (beb as PriorityBindingExpression)) != null)
			{
				readOnlyCollection = priorityBindingExpression.BindingExpressions;
			}
			if (readOnlyCollection != null)
			{
				foreach (BindingExpressionBase beb2 in readOnlyCollection)
				{
					DependencyObject dependencyObject = DataGridHelper.FindContextElement(beb2);
					if (dependencyObject != null)
					{
						return dependencyObject;
					}
				}
			}
			return null;
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0014AD08 File Offset: 0x00148F08
		internal static void CacheFlowDirection(FrameworkElement element, DataGridCell cell)
		{
			if (element != null && cell != null)
			{
				object obj = element.ReadLocalValue(FrameworkElement.FlowDirectionProperty);
				if (obj != DependencyProperty.UnsetValue)
				{
					cell.SetValue(DataGridHelper.FlowDirectionCacheProperty, obj);
				}
			}
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x0014AD3C File Offset: 0x00148F3C
		internal static void RestoreFlowDirection(FrameworkElement element, DataGridCell cell)
		{
			if (element != null && cell != null)
			{
				object obj = cell.ReadLocalValue(DataGridHelper.FlowDirectionCacheProperty);
				if (obj != DependencyProperty.UnsetValue)
				{
					element.SetValue(FrameworkElement.FlowDirectionProperty, obj);
				}
			}
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x0014AD70 File Offset: 0x00148F70
		internal static void UpdateTarget(FrameworkElement element)
		{
			BindingGroup bindingGroup = element.BindingGroup;
			DataGridCell dataGridCell = (element != null) ? (element.Parent as DataGridCell) : null;
			if (bindingGroup != null && dataGridCell != null)
			{
				Collection<BindingExpressionBase> bindingExpressions = bindingGroup.BindingExpressions;
				BindingExpressionBase[] array = new BindingExpressionBase[bindingExpressions.Count];
				bindingExpressions.CopyTo(array, 0);
				foreach (BindingExpressionBase bindingExpressionBase in array)
				{
					DependencyObject targetElement = bindingExpressionBase.TargetElement;
					if (targetElement != null && VisualTreeHelper.IsAncestorOf(dataGridCell, targetElement, typeof(DataGridCell)))
					{
						bindingExpressionBase.UpdateTarget();
					}
				}
			}
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x0014ADF7 File Offset: 0x00148FF7
		internal static void SyncColumnProperty(DependencyObject column, DependencyObject content, DependencyProperty contentProperty, DependencyProperty columnProperty)
		{
			if (DataGridHelper.IsDefaultValue(column, columnProperty))
			{
				content.ClearValue(contentProperty);
				return;
			}
			content.SetValue(contentProperty, column.GetValue(columnProperty));
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x0014AE18 File Offset: 0x00149018
		internal static string GetPathFromBinding(Binding binding)
		{
			if (binding != null)
			{
				if (!string.IsNullOrEmpty(binding.XPath))
				{
					return binding.XPath;
				}
				if (binding.Path != null)
				{
					return binding.Path.Path;
				}
			}
			return null;
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x0013F4E4 File Offset: 0x0013D6E4
		public static bool AreRowHeadersVisible(DataGridHeadersVisibility headersVisibility)
		{
			return (headersVisibility & DataGridHeadersVisibility.Row) == DataGridHeadersVisibility.Row;
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x0014AE46 File Offset: 0x00149046
		public static double CoerceToMinMax(double value, double minValue, double maxValue)
		{
			value = Math.Max(value, minValue);
			value = Math.Min(value, maxValue);
			return value;
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x0014AE5C File Offset: 0x0014905C
		public static bool HasNonEscapeCharacters(TextCompositionEventArgs textArgs)
		{
			if (textArgs != null)
			{
				string text = textArgs.Text;
				int i = 0;
				int length = text.Length;
				while (i < length)
				{
					if (text[i] != '\u001b')
					{
						return true;
					}
					i++;
				}
			}
			return false;
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x0014AE94 File Offset: 0x00149094
		public static bool IsImeProcessed(KeyEventArgs keyArgs)
		{
			return keyArgs != null && keyArgs.Key == Key.ImeProcessed;
		}

		// Token: 0x040029BA RID: 10682
		private static ConditionalWeakTable<DependencyObject, Dictionary<DependencyProperty, bool>> _propertyTransferEnabledMap = new ConditionalWeakTable<DependencyObject, Dictionary<DependencyProperty, bool>>();

		// Token: 0x040029BB RID: 10683
		private static readonly DependencyProperty FlowDirectionCacheProperty = DependencyProperty.Register("FlowDirectionCache", typeof(FlowDirection), typeof(DataGridHelper));

		// Token: 0x040029BC RID: 10684
		private const char _escapeChar = '\u001b';
	}
}
