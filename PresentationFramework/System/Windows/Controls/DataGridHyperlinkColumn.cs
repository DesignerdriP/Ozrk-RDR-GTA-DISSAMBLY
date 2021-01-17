using System;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column that hosts <see cref="T:System.Uri" /> elements in its cells.</summary>
	// Token: 0x020004AD RID: 1197
	public class DataGridHyperlinkColumn : DataGridBoundColumn
	{
		// Token: 0x060048EC RID: 18668 RVA: 0x0014AED8 File Offset: 0x001490D8
		static DataGridHyperlinkColumn()
		{
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridHyperlinkColumn), new FrameworkPropertyMetadata(DataGridHyperlinkColumn.DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridHyperlinkColumn), new FrameworkPropertyMetadata(DataGridHyperlinkColumn.DefaultEditingElementStyle));
		}

		/// <summary>Gets or sets the name of a target window or frame for the hyperlink.</summary>
		/// <returns>The name of the target window or frame. The registered default is <see langword="null" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x060048ED RID: 18669 RVA: 0x0014AF4C File Offset: 0x0014914C
		// (set) Token: 0x060048EE RID: 18670 RVA: 0x0014AF5E File Offset: 0x0014915E
		public string TargetName
		{
			get
			{
				return (string)base.GetValue(DataGridHyperlinkColumn.TargetNameProperty);
			}
			set
			{
				base.SetValue(DataGridHyperlinkColumn.TargetNameProperty, value);
			}
		}

		/// <summary>Gets or sets the binding to the text of the hyperlink.</summary>
		/// <returns>The binding to the text of the hyperlink.</returns>
		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x060048EF RID: 18671 RVA: 0x0014AF6C File Offset: 0x0014916C
		// (set) Token: 0x060048F0 RID: 18672 RVA: 0x0014AF74 File Offset: 0x00149174
		public BindingBase ContentBinding
		{
			get
			{
				return this._contentBinding;
			}
			set
			{
				if (this._contentBinding != value)
				{
					BindingBase contentBinding = this._contentBinding;
					this._contentBinding = value;
					this.OnContentBindingChanged(contentBinding, value);
				}
			}
		}

		/// <summary>Notifies the <see cref="T:System.Windows.Controls.DataGrid" /> when the <see cref="P:System.Windows.Controls.DataGridHyperlinkColumn.ContentBinding" /> property changes.</summary>
		/// <param name="oldBinding">The previous binding.</param>
		/// <param name="newBinding">The binding that the column has been changed to.</param>
		// Token: 0x060048F1 RID: 18673 RVA: 0x0014AFA0 File Offset: 0x001491A0
		protected virtual void OnContentBindingChanged(BindingBase oldBinding, BindingBase newBinding)
		{
			base.NotifyPropertyChanged("ContentBinding");
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x0014AFAD File Offset: 0x001491AD
		private void ApplyContentBinding(DependencyObject target, DependencyProperty property)
		{
			if (this.ContentBinding != null)
			{
				BindingOperations.SetBinding(target, property, this.ContentBinding);
				return;
			}
			if (this.Binding != null)
			{
				BindingOperations.SetBinding(target, property, this.Binding);
				return;
			}
			BindingOperations.ClearBinding(target, property);
		}

		/// <summary>Refreshes the contents of a cell in the column in response to a column property value change.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that has changed.</param>
		// Token: 0x060048F3 RID: 18675 RVA: 0x0014AFE4 File Offset: 0x001491E4
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null && !dataGridCell.IsEditing)
			{
				if (string.Compare(propertyName, "ContentBinding", StringComparison.Ordinal) == 0)
				{
					dataGridCell.BuildVisualTree();
					return;
				}
				if (string.Compare(propertyName, "TargetName", StringComparison.Ordinal) == 0)
				{
					TextBlock textBlock = dataGridCell.Content as TextBlock;
					if (textBlock != null && textBlock.Inlines.Count > 0)
					{
						Hyperlink hyperlink = textBlock.Inlines.FirstInline as Hyperlink;
						if (hyperlink != null)
						{
							hyperlink.TargetName = this.TargetName;
							return;
						}
					}
				}
			}
			else
			{
				base.RefreshCellContent(element, propertyName);
			}
		}

		/// <summary>The default value of the <see cref="P:System.Windows.Controls.DataGridBoundColumn.ElementStyle" /> property.</summary>
		/// <returns>An object that represents the style.</returns>
		// Token: 0x170011C6 RID: 4550
		// (get) Token: 0x060048F4 RID: 18676 RVA: 0x0014B06C File Offset: 0x0014926C
		public static Style DefaultElementStyle
		{
			get
			{
				return DataGridTextColumn.DefaultElementStyle;
			}
		}

		/// <summary>The default value of the <see cref="P:System.Windows.Controls.DataGridBoundColumn.EditingElementStyle" /> property.</summary>
		/// <returns>An object that represents the style.</returns>
		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x060048F5 RID: 18677 RVA: 0x0014B073 File Offset: 0x00149273
		public static Style DefaultEditingElementStyle
		{
			get
			{
				return DataGridTextColumn.DefaultEditingElementStyle;
			}
		}

		/// <summary>Gets a read-only <see cref="T:System.Windows.Documents.Hyperlink" /> element that is bound to the column's <see cref="P:System.Windows.Controls.DataGridHyperlinkColumn.ContentBinding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new, read-only hyperlink element that is bound to the column's <see cref="P:System.Windows.Controls.DataGridHyperlinkColumn.ContentBinding" /> property value.</returns>
		// Token: 0x060048F6 RID: 18678 RVA: 0x0014B07C File Offset: 0x0014927C
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			TextBlock textBlock = new TextBlock();
			Hyperlink hyperlink = new Hyperlink();
			InlineUIContainer inlineUIContainer = new InlineUIContainer();
			ContentPresenter contentPresenter = new ContentPresenter();
			textBlock.Inlines.Add(hyperlink);
			hyperlink.Inlines.Add(inlineUIContainer);
			inlineUIContainer.Child = contentPresenter;
			hyperlink.TargetName = this.TargetName;
			base.ApplyStyle(false, false, textBlock);
			base.ApplyBinding(hyperlink, Hyperlink.NavigateUriProperty);
			this.ApplyContentBinding(contentPresenter, ContentPresenter.ContentProperty);
			DataGridHelper.RestoreFlowDirection(textBlock, cell);
			return textBlock;
		}

		/// <summary>Gets an editable <see cref="T:System.Windows.Controls.TextBox" /> element that is bound to the column's <see cref="P:System.Windows.Controls.DataGridHyperlinkColumn.ContentBinding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new text box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridHyperlinkColumn.ContentBinding" /> property value.</returns>
		// Token: 0x060048F7 RID: 18679 RVA: 0x0014B0F8 File Offset: 0x001492F8
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			TextBox textBox = new TextBox();
			base.ApplyStyle(true, false, textBox);
			base.ApplyBinding(textBox, TextBox.TextProperty);
			DataGridHelper.RestoreFlowDirection(textBox, cell);
			return textBox;
		}

		/// <summary>Called when a cell in the column enters editing mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
		/// <returns>The unedited value of the cell.</returns>
		// Token: 0x060048F8 RID: 18680 RVA: 0x0014B128 File Offset: 0x00149328
		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			TextBox textBox = editingElement as TextBox;
			if (textBox != null)
			{
				textBox.Focus();
				string text = textBox.Text;
				TextCompositionEventArgs textCompositionEventArgs = editingEventArgs as TextCompositionEventArgs;
				if (textCompositionEventArgs != null)
				{
					string text2 = textCompositionEventArgs.Text;
					textBox.Text = text2;
					textBox.Select(text2.Length, 0);
				}
				else
				{
					textBox.SelectAll();
				}
				return text;
			}
			return null;
		}

		/// <summary>Causes the column cell being edited to revert to the specified value.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="uneditedValue">The previous, unedited value in the cell being edited.</param>
		// Token: 0x060048F9 RID: 18681 RVA: 0x0014B17D File Offset: 0x0014937D
		protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			base.CancelCellEdit(editingElement, uneditedValue);
		}

		/// <summary>Performs any required validation before exiting edit mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <returns>
		///     <see langword="false" /> if validation fails; otherwise, <see langword="true" />.</returns>
		// Token: 0x060048FA RID: 18682 RVA: 0x0014B19E File Offset: 0x0014939E
		protected override bool CommitCellEdit(FrameworkElement editingElement)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			return base.CommitCellEdit(editingElement);
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x0014B1C0 File Offset: 0x001493C0
		internal override void OnInput(InputEventArgs e)
		{
			if (DataGridHelper.HasNonEscapeCharacters(e as TextCompositionEventArgs))
			{
				base.BeginEdit(e, true);
				return;
			}
			if (DataGridHelper.IsImeProcessed(e as KeyEventArgs) && base.DataGridOwner != null)
			{
				DataGridCell currentCellContainer = base.DataGridOwner.CurrentCellContainer;
				if (currentCellContainer != null && !currentCellContainer.IsEditing)
				{
					base.BeginEdit(e, false);
					base.Dispatcher.Invoke(delegate()
					{
					}, DispatcherPriority.Background);
				}
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridHyperlinkColumn.TargetName" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridHyperlinkColumn.TargetName" /> dependency property.</returns>
		// Token: 0x040029BD RID: 10685
		public static readonly DependencyProperty TargetNameProperty = Hyperlink.TargetNameProperty.AddOwner(typeof(DataGridHyperlinkColumn), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x040029BE RID: 10686
		private BindingBase _contentBinding;
	}
}
