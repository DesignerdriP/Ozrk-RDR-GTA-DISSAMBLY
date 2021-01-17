using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column that hosts <see cref="T:System.Windows.Controls.CheckBox" /> controls in its cells.</summary>
	// Token: 0x0200049B RID: 1179
	public class DataGridCheckBoxColumn : DataGridBoundColumn
	{
		// Token: 0x0600477E RID: 18302 RVA: 0x00144B40 File Offset: 0x00142D40
		static DataGridCheckBoxColumn()
		{
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridCheckBoxColumn), new FrameworkPropertyMetadata(DataGridCheckBoxColumn.DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridCheckBoxColumn), new FrameworkPropertyMetadata(DataGridCheckBoxColumn.DefaultEditingElementStyle));
		}

		/// <summary>Gets the default value of the <see cref="P:System.Windows.Controls.DataGridBoundColumn.ElementStyle" /> property. </summary>
		/// <returns>An object that represents the style.</returns>
		// Token: 0x17001183 RID: 4483
		// (get) Token: 0x0600477F RID: 18303 RVA: 0x00144BBC File Offset: 0x00142DBC
		public static Style DefaultElementStyle
		{
			get
			{
				if (DataGridCheckBoxColumn._defaultElementStyle == null)
				{
					Style style = new Style(typeof(CheckBox));
					style.Setters.Add(new Setter(UIElement.IsHitTestVisibleProperty, false));
					style.Setters.Add(new Setter(UIElement.FocusableProperty, false));
					style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
					style.Seal();
					DataGridCheckBoxColumn._defaultElementStyle = style;
				}
				return DataGridCheckBoxColumn._defaultElementStyle;
			}
		}

		/// <summary>Gets the default value of the <see cref="P:System.Windows.Controls.DataGridBoundColumn.EditingElementStyle" /> property.</summary>
		/// <returns>An object that represents the style.</returns>
		// Token: 0x17001184 RID: 4484
		// (get) Token: 0x06004780 RID: 18304 RVA: 0x00144C60 File Offset: 0x00142E60
		public static Style DefaultEditingElementStyle
		{
			get
			{
				if (DataGridCheckBoxColumn._defaultEditingElementStyle == null)
				{
					Style style = new Style(typeof(CheckBox));
					style.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Center));
					style.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Top));
					style.Seal();
					DataGridCheckBoxColumn._defaultEditingElementStyle = style;
				}
				return DataGridCheckBoxColumn._defaultEditingElementStyle;
			}
		}

		/// <summary>Gets a read-only <see cref="T:System.Windows.Controls.CheckBox" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new, read-only check box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x06004781 RID: 18305 RVA: 0x00144CCB File Offset: 0x00142ECB
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			return this.GenerateCheckBox(false, cell);
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.CheckBox" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new check box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x06004782 RID: 18306 RVA: 0x00144CD5 File Offset: 0x00142ED5
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			return this.GenerateCheckBox(true, cell);
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x00144CE0 File Offset: 0x00142EE0
		private CheckBox GenerateCheckBox(bool isEditing, DataGridCell cell)
		{
			CheckBox checkBox = (cell != null) ? (cell.Content as CheckBox) : null;
			if (checkBox == null)
			{
				checkBox = new CheckBox();
			}
			checkBox.IsThreeState = this.IsThreeState;
			base.ApplyStyle(isEditing, true, checkBox);
			base.ApplyBinding(checkBox, ToggleButton.IsCheckedProperty);
			return checkBox;
		}

		/// <summary>Refreshes the contents of a cell in the column in response to a column property value change.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that has changed.</param>
		// Token: 0x06004784 RID: 18308 RVA: 0x00144D2C File Offset: 0x00142F2C
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null && string.Compare(propertyName, "IsThreeState", StringComparison.Ordinal) == 0)
			{
				CheckBox checkBox = dataGridCell.Content as CheckBox;
				if (checkBox != null)
				{
					checkBox.IsThreeState = this.IsThreeState;
					return;
				}
			}
			else
			{
				base.RefreshCellContent(element, propertyName);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the hosted <see cref="T:System.Windows.Controls.CheckBox" /> controls enable three states or two.</summary>
		/// <returns>
		///     <see langword="true" /> if the hosted controls support three states; <see langword="false" /> if they support two states. The default is <see langword="false" />.</returns>
		// Token: 0x17001185 RID: 4485
		// (get) Token: 0x06004785 RID: 18309 RVA: 0x00144D75 File Offset: 0x00142F75
		// (set) Token: 0x06004786 RID: 18310 RVA: 0x00144D87 File Offset: 0x00142F87
		public bool IsThreeState
		{
			get
			{
				return (bool)base.GetValue(DataGridCheckBoxColumn.IsThreeStateProperty);
			}
			set
			{
				base.SetValue(DataGridCheckBoxColumn.IsThreeStateProperty, value);
			}
		}

		/// <summary>Called when a cell in the column enters editing mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
		/// <returns>The unedited value.</returns>
		// Token: 0x06004787 RID: 18311 RVA: 0x00144D98 File Offset: 0x00142F98
		protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
		{
			CheckBox checkBox = editingElement as CheckBox;
			if (checkBox != null)
			{
				checkBox.Focus();
				bool? isChecked = checkBox.IsChecked;
				if ((DataGridCheckBoxColumn.IsMouseLeftButtonDown(editingEventArgs) && DataGridCheckBoxColumn.IsMouseOver(checkBox, editingEventArgs)) || DataGridCheckBoxColumn.IsSpaceKeyDown(editingEventArgs))
				{
					checkBox.IsChecked = new bool?(isChecked != true);
				}
				return isChecked;
			}
			return false;
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x00144E09 File Offset: 0x00143009
		internal override void OnInput(InputEventArgs e)
		{
			if (DataGridCheckBoxColumn.IsSpaceKeyDown(e))
			{
				base.BeginEdit(e, true);
			}
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x00144E1C File Offset: 0x0014301C
		private static bool IsMouseLeftButtonDown(RoutedEventArgs e)
		{
			MouseButtonEventArgs mouseButtonEventArgs = e as MouseButtonEventArgs;
			return mouseButtonEventArgs != null && mouseButtonEventArgs.ChangedButton == MouseButton.Left && mouseButtonEventArgs.ButtonState == MouseButtonState.Pressed;
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x00144E46 File Offset: 0x00143046
		private static bool IsMouseOver(CheckBox checkBox, RoutedEventArgs e)
		{
			return checkBox.InputHitTest(((MouseButtonEventArgs)e).GetPosition(checkBox)) != null;
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x00144E60 File Offset: 0x00143060
		private static bool IsSpaceKeyDown(RoutedEventArgs e)
		{
			KeyEventArgs keyEventArgs = e as KeyEventArgs;
			return keyEventArgs != null && keyEventArgs.RoutedEvent == Keyboard.KeyDownEvent && (keyEventArgs.KeyStates & KeyStates.Down) == KeyStates.Down && keyEventArgs.Key == Key.Space;
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridCheckBoxColumn.IsThreeState" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridCheckBoxColumn.IsThreeState" /> dependency property.</returns>
		// Token: 0x0400295A RID: 10586
		public static readonly DependencyProperty IsThreeStateProperty = ToggleButton.IsThreeStateProperty.AddOwner(typeof(DataGridCheckBoxColumn), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x0400295B RID: 10587
		private static Style _defaultElementStyle;

		// Token: 0x0400295C RID: 10588
		private static Style _defaultEditingElementStyle;
	}
}
