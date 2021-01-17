using System;
using System.Windows.Data;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column that hosts template-specified content in its cells.</summary>
	// Token: 0x020004BE RID: 1214
	public class DataGridTemplateColumn : DataGridColumn
	{
		// Token: 0x0600499F RID: 18847 RVA: 0x0014D0A8 File Offset: 0x0014B2A8
		static DataGridTemplateColumn()
		{
			DataGridColumn.CanUserSortProperty.OverrideMetadata(typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridTemplateColumn.OnCoerceTemplateColumnCanUserSort)));
			DataGridColumn.SortMemberPathProperty.OverrideMetadata(typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataGridTemplateColumn.OnTemplateColumnSortMemberPathChanged)));
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x0014D1D4 File Offset: 0x0014B3D4
		private static void OnTemplateColumnSortMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DataGridTemplateColumn dataGridTemplateColumn = (DataGridTemplateColumn)d;
			dataGridTemplateColumn.CoerceValue(DataGridColumn.CanUserSortProperty);
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x0014D1F4 File Offset: 0x0014B3F4
		private static object OnCoerceTemplateColumnCanUserSort(DependencyObject d, object baseValue)
		{
			DataGridTemplateColumn dataGridTemplateColumn = (DataGridTemplateColumn)d;
			if (string.IsNullOrEmpty(dataGridTemplateColumn.SortMemberPath))
			{
				return false;
			}
			return DataGridColumn.OnCoerceCanUserSort(d, baseValue);
		}

		/// <summary>Gets or sets the template to use to display the contents of a cell that is not in editing mode.</summary>
		/// <returns>The template to use to display the contents of a cell that is not in editing mode. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FC RID: 4604
		// (get) Token: 0x060049A2 RID: 18850 RVA: 0x0014D223 File Offset: 0x0014B423
		// (set) Token: 0x060049A3 RID: 18851 RVA: 0x0014D235 File Offset: 0x0014B435
		public DataTemplate CellTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridTemplateColumn.CellTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the object that determines which template to use to display the contents of a cell that is not in editing mode. </summary>
		/// <returns>The object that determines which template to use. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FD RID: 4605
		// (get) Token: 0x060049A4 RID: 18852 RVA: 0x0014D243 File Offset: 0x0014B443
		// (set) Token: 0x060049A5 RID: 18853 RVA: 0x0014D255 File Offset: 0x0014B455
		public DataTemplateSelector CellTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridTemplateColumn.CellTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellTemplateSelectorProperty, value);
			}
		}

		/// <summary>Gets or sets the template to use to display the contents of a cell that is in editing mode.</summary>
		/// <returns>The template that is used to display the contents of a cell that is in editing mode. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FE RID: 4606
		// (get) Token: 0x060049A6 RID: 18854 RVA: 0x0014D263 File Offset: 0x0014B463
		// (set) Token: 0x060049A7 RID: 18855 RVA: 0x0014D275 File Offset: 0x0014B475
		public DataTemplate CellEditingTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(DataGridTemplateColumn.CellEditingTemplateProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellEditingTemplateProperty, value);
			}
		}

		/// <summary>Gets or sets the object that determines which template to use to display the contents of a cell that is in editing mode.</summary>
		/// <returns>The object that determines which template to use to display the contents of a cell that is in editing mode. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x060049A8 RID: 18856 RVA: 0x0014D283 File Offset: 0x0014B483
		// (set) Token: 0x060049A9 RID: 18857 RVA: 0x0014D295 File Offset: 0x0014B495
		public DataTemplateSelector CellEditingTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(DataGridTemplateColumn.CellEditingTemplateSelectorProperty, value);
			}
		}

		// Token: 0x060049AA RID: 18858 RVA: 0x0014D2A3 File Offset: 0x0014B4A3
		private void ChooseCellTemplateAndSelector(bool isEditing, out DataTemplate template, out DataTemplateSelector templateSelector)
		{
			template = null;
			templateSelector = null;
			if (isEditing)
			{
				template = this.CellEditingTemplate;
				templateSelector = this.CellEditingTemplateSelector;
			}
			if (template == null && templateSelector == null)
			{
				template = this.CellTemplate;
				templateSelector = this.CellTemplateSelector;
			}
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x0014D2D8 File Offset: 0x0014B4D8
		private FrameworkElement LoadTemplateContent(bool isEditing, object dataItem, DataGridCell cell)
		{
			DataTemplate dataTemplate;
			DataTemplateSelector dataTemplateSelector;
			this.ChooseCellTemplateAndSelector(isEditing, out dataTemplate, out dataTemplateSelector);
			if (dataTemplate != null || dataTemplateSelector != null)
			{
				ContentPresenter contentPresenter = new ContentPresenter();
				BindingOperations.SetBinding(contentPresenter, ContentPresenter.ContentProperty, new Binding());
				contentPresenter.ContentTemplate = dataTemplate;
				contentPresenter.ContentTemplateSelector = dataTemplateSelector;
				return contentPresenter;
			}
			return null;
		}

		/// <summary>Gets an element defined by the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellTemplate" /> that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new, read-only element that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x060049AC RID: 18860 RVA: 0x0014D31E File Offset: 0x0014B51E
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			return this.LoadTemplateContent(false, dataItem, cell);
		}

		/// <summary>Gets an element defined by the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplate" /> that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new editing element that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x060049AD RID: 18861 RVA: 0x0014D329 File Offset: 0x0014B529
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return this.LoadTemplateContent(true, dataItem, cell);
		}

		/// <summary>Refreshes the contents of a cell in the column in response to a template property value change.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that has changed.</param>
		// Token: 0x060049AE RID: 18862 RVA: 0x0014D334 File Offset: 0x0014B534
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				bool isEditing = dataGridCell.IsEditing;
				if ((!isEditing && (string.Compare(propertyName, "CellTemplate", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "CellTemplateSelector", StringComparison.Ordinal) == 0)) || (isEditing && (string.Compare(propertyName, "CellEditingTemplate", StringComparison.Ordinal) == 0 || string.Compare(propertyName, "CellEditingTemplateSelector", StringComparison.Ordinal) == 0)))
				{
					dataGridCell.BuildVisualTree();
					return;
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellTemplate" /> dependency property.</returns>
		// Token: 0x04002A23 RID: 10787
		public static readonly DependencyProperty CellTemplateProperty = DependencyProperty.Register("CellTemplate", typeof(DataTemplate), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002A24 RID: 10788
		public static readonly DependencyProperty CellTemplateSelectorProperty = DependencyProperty.Register("CellTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplate" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplate" /> dependency property.</returns>
		// Token: 0x04002A25 RID: 10789
		public static readonly DependencyProperty CellEditingTemplateProperty = DependencyProperty.Register("CellEditingTemplate", typeof(DataTemplate), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTemplateColumn.CellEditingTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002A26 RID: 10790
		public static readonly DependencyProperty CellEditingTemplateSelectorProperty = DependencyProperty.Register("CellEditingTemplateSelector", typeof(DataTemplateSelector), typeof(DataGridTemplateColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));
	}
}
