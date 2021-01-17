using System;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace System.Windows.Controls
{
	/// <summary>Represents a <see cref="T:System.Windows.Controls.DataGrid" /> column that hosts textual content in its cells.</summary>
	// Token: 0x020004BF RID: 1215
	public class DataGridTextColumn : DataGridBoundColumn
	{
		// Token: 0x060049B0 RID: 18864 RVA: 0x0014D3A0 File Offset: 0x0014B5A0
		static DataGridTextColumn()
		{
			DataGridBoundColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(DataGridTextColumn.DefaultElementStyle));
			DataGridBoundColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(DataGridTextColumn.DefaultEditingElementStyle));
		}

		/// <summary>The default value of the <see cref="P:System.Windows.Controls.DataGridBoundColumn.ElementStyle" /> property.</summary>
		/// <returns>An object that represents the style.</returns>
		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x060049B1 RID: 18865 RVA: 0x0014D4F0 File Offset: 0x0014B6F0
		public static Style DefaultElementStyle
		{
			get
			{
				if (DataGridTextColumn._defaultElementStyle == null)
				{
					Style style = new Style(typeof(TextBlock));
					style.Setters.Add(new Setter(FrameworkElement.MarginProperty, new Thickness(2.0, 0.0, 2.0, 0.0)));
					style.Seal();
					DataGridTextColumn._defaultElementStyle = style;
				}
				return DataGridTextColumn._defaultElementStyle;
			}
		}

		/// <summary>The default value of the <see cref="P:System.Windows.Controls.DataGridBoundColumn.EditingElementStyle" /> property.</summary>
		/// <returns>An object that represents the style.</returns>
		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x060049B2 RID: 18866 RVA: 0x0014D568 File Offset: 0x0014B768
		public static Style DefaultEditingElementStyle
		{
			get
			{
				if (DataGridTextColumn._defaultEditingElementStyle == null)
				{
					Style style = new Style(typeof(TextBox));
					style.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0.0)));
					style.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(0.0)));
					style.Seal();
					DataGridTextColumn._defaultEditingElementStyle = style;
				}
				return DataGridTextColumn._defaultEditingElementStyle;
			}
		}

		/// <summary>Gets a read-only <see cref="T:System.Windows.Controls.TextBlock" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new, read-only text block control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x060049B3 RID: 18867 RVA: 0x0014D5F0 File Offset: 0x0014B7F0
		protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
		{
			TextBlock textBlock = new TextBlock();
			this.SyncProperties(textBlock);
			base.ApplyStyle(false, false, textBlock);
			base.ApplyBinding(textBlock, TextBlock.TextProperty);
			DataGridHelper.RestoreFlowDirection(textBlock, cell);
			return textBlock;
		}

		/// <summary>Gets a <see cref="T:System.Windows.Controls.TextBox" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</summary>
		/// <param name="cell">The cell that will contain the generated element.</param>
		/// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
		/// <returns>A new text box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.</returns>
		// Token: 0x060049B4 RID: 18868 RVA: 0x0014D628 File Offset: 0x0014B828
		protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
		{
			TextBox textBox = new TextBox();
			this.SyncProperties(textBox);
			base.ApplyStyle(true, false, textBox);
			base.ApplyBinding(textBox, TextBox.TextProperty);
			DataGridHelper.RestoreFlowDirection(textBox, cell);
			return textBox;
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x0014D660 File Offset: 0x0014B860
		private void SyncProperties(FrameworkElement e)
		{
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontFamilyProperty, DataGridTextColumn.FontFamilyProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontSizeProperty, DataGridTextColumn.FontSizeProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontStyleProperty, DataGridTextColumn.FontStyleProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.FontWeightProperty, DataGridTextColumn.FontWeightProperty);
			DataGridHelper.SyncColumnProperty(this, e, TextElement.ForegroundProperty, DataGridTextColumn.ForegroundProperty);
		}

		/// <summary>Refreshes the contents of a cell in the column in response to a column property value change.</summary>
		/// <param name="element">The cell to update.</param>
		/// <param name="propertyName">The name of the column property that has changed.</param>
		// Token: 0x060049B6 RID: 18870 RVA: 0x0014D6C4 File Offset: 0x0014B8C4
		protected internal override void RefreshCellContent(FrameworkElement element, string propertyName)
		{
			DataGridCell dataGridCell = element as DataGridCell;
			if (dataGridCell != null)
			{
				FrameworkElement frameworkElement = dataGridCell.Content as FrameworkElement;
				if (frameworkElement != null)
				{
					if (!(propertyName == "FontFamily"))
					{
						if (!(propertyName == "FontSize"))
						{
							if (!(propertyName == "FontStyle"))
							{
								if (!(propertyName == "FontWeight"))
								{
									if (propertyName == "Foreground")
									{
										DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.ForegroundProperty, DataGridTextColumn.ForegroundProperty);
									}
								}
								else
								{
									DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontWeightProperty, DataGridTextColumn.FontWeightProperty);
								}
							}
							else
							{
								DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontStyleProperty, DataGridTextColumn.FontStyleProperty);
							}
						}
						else
						{
							DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontSizeProperty, DataGridTextColumn.FontSizeProperty);
						}
					}
					else
					{
						DataGridHelper.SyncColumnProperty(this, frameworkElement, TextElement.FontFamilyProperty, DataGridTextColumn.FontFamilyProperty);
					}
				}
			}
			base.RefreshCellContent(element, propertyName);
		}

		/// <summary>Called when a cell in the column enters editing mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
		/// <returns>The unedited value of the cell.</returns>
		// Token: 0x060049B7 RID: 18871 RVA: 0x0014D798 File Offset: 0x0014B998
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
					string text2 = this.ConvertTextForEdit(textCompositionEventArgs.Text);
					textBox.Text = text2;
					textBox.Select(text2.Length, 0);
				}
				else
				{
					MouseButtonEventArgs mouseButtonEventArgs = editingEventArgs as MouseButtonEventArgs;
					if (mouseButtonEventArgs == null || !DataGridTextColumn.PlaceCaretOnTextBox(textBox, Mouse.GetPosition(textBox)))
					{
						textBox.SelectAll();
					}
				}
				return text;
			}
			return null;
		}

		// Token: 0x060049B8 RID: 18872 RVA: 0x0014D80D File Offset: 0x0014BA0D
		private string ConvertTextForEdit(string s)
		{
			if (s == "\b")
			{
				s = string.Empty;
			}
			return s;
		}

		/// <summary>Causes the column cell being edited to revert to the specified value.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <param name="uneditedValue">The previous, unedited value in the cell being edited.</param>
		// Token: 0x060049B9 RID: 18873 RVA: 0x0014B17D File Offset: 0x0014937D
		protected override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			base.CancelCellEdit(editingElement, uneditedValue);
		}

		/// <summary>Performs any required validation before exiting the edit mode.</summary>
		/// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
		/// <returns>
		///     <see langword="false" /> if validation fails; otherwise, <see langword="true" />.</returns>
		// Token: 0x060049BA RID: 18874 RVA: 0x0014B19E File Offset: 0x0014939E
		protected override bool CommitCellEdit(FrameworkElement editingElement)
		{
			DataGridHelper.CacheFlowDirection(editingElement, (editingElement != null) ? (editingElement.Parent as DataGridCell) : null);
			return base.CommitCellEdit(editingElement);
		}

		// Token: 0x060049BB RID: 18875 RVA: 0x0014D824 File Offset: 0x0014BA24
		private static bool PlaceCaretOnTextBox(TextBox textBox, Point position)
		{
			int characterIndexFromPoint = textBox.GetCharacterIndexFromPoint(position, false);
			if (characterIndexFromPoint >= 0)
			{
				textBox.Select(characterIndexFromPoint, 0);
				return true;
			}
			return false;
		}

		// Token: 0x060049BC RID: 18876 RVA: 0x0014D84C File Offset: 0x0014BA4C
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

		/// <summary>Gets or sets the font family for the content of cells in the column.</summary>
		/// <returns>The font family of the content for cells in the column. The registered default is <see cref="P:System.Windows.SystemFonts.MessageFontFamily" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x060049BD RID: 18877 RVA: 0x0014D8CE File Offset: 0x0014BACE
		// (set) Token: 0x060049BE RID: 18878 RVA: 0x0014D8E0 File Offset: 0x0014BAE0
		public FontFamily FontFamily
		{
			get
			{
				return (FontFamily)base.GetValue(DataGridTextColumn.FontFamilyProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontFamilyProperty, value);
			}
		}

		/// <summary>Gets or sets the font size for the content of cells in the column.</summary>
		/// <returns>The font size of the content of cells in the column. The registered default is <see cref="P:System.Windows.SystemFonts.MessageFontSize" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x060049BF RID: 18879 RVA: 0x0014D8EE File Offset: 0x0014BAEE
		// (set) Token: 0x060049C0 RID: 18880 RVA: 0x0014D900 File Offset: 0x0014BB00
		[TypeConverter(typeof(FontSizeConverter))]
		[Localizability(LocalizationCategory.None)]
		public double FontSize
		{
			get
			{
				return (double)base.GetValue(DataGridTextColumn.FontSizeProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontSizeProperty, value);
			}
		}

		/// <summary>Gets or sets the font style for the content of cells in the column.</summary>
		/// <returns>The font style of the content of cells in the column. The registered default is <see cref="P:System.Windows.SystemFonts.MessageFontStyle" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x060049C1 RID: 18881 RVA: 0x0014D913 File Offset: 0x0014BB13
		// (set) Token: 0x060049C2 RID: 18882 RVA: 0x0014D925 File Offset: 0x0014BB25
		public FontStyle FontStyle
		{
			get
			{
				return (FontStyle)base.GetValue(DataGridTextColumn.FontStyleProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontStyleProperty, value);
			}
		}

		/// <summary>Gets or sets the font weight for the content of cells in the column.</summary>
		/// <returns>The font weight of the contents of cells in the column. The registered default is <see cref="P:System.Windows.SystemFonts.MessageFontWeight" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x060049C3 RID: 18883 RVA: 0x0014D938 File Offset: 0x0014BB38
		// (set) Token: 0x060049C4 RID: 18884 RVA: 0x0014D94A File Offset: 0x0014BB4A
		public FontWeight FontWeight
		{
			get
			{
				return (FontWeight)base.GetValue(DataGridTextColumn.FontWeightProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.FontWeightProperty, value);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Brush" /> that is used to paint the text contents of cells in the column.</summary>
		/// <returns>The brush that is used to paint the contents of cells in the column. The registered default is <see cref="P:System.Windows.SystemColors.ControlTextBrush" />. For information about what can influence the value, see <see cref="T:System.Windows.DependencyProperty" />.</returns>
		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x060049C5 RID: 18885 RVA: 0x0014D95D File Offset: 0x0014BB5D
		// (set) Token: 0x060049C6 RID: 18886 RVA: 0x0014D96F File Offset: 0x0014BB6F
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(DataGridTextColumn.ForegroundProperty);
			}
			set
			{
				base.SetValue(DataGridTextColumn.ForegroundProperty, value);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontFamily" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontFamily" /> dependency property.</returns>
		// Token: 0x04002A27 RID: 10791
		public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontSize" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontSize" /> dependency property.</returns>
		// Token: 0x04002A28 RID: 10792
		public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontSize, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontStyle" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontStyle" /> dependency property.</returns>
		// Token: 0x04002A29 RID: 10793
		public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontWeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTextColumn.FontWeight" /> dependency property.</returns>
		// Token: 0x04002A2A RID: 10794
		public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.DataGridTextColumn.Foreground" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.DataGridTextColumn.Foreground" /> dependency property.</returns>
		// Token: 0x04002A2B RID: 10795
		public static readonly DependencyProperty ForegroundProperty = TextElement.ForegroundProperty.AddOwner(typeof(DataGridTextColumn), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush, FrameworkPropertyMetadataOptions.Inherits, new PropertyChangedCallback(DataGridColumn.NotifyPropertyChangeForRefreshContent)));

		// Token: 0x04002A2C RID: 10796
		private static Style _defaultElementStyle;

		// Token: 0x04002A2D RID: 10797
		private static Style _defaultEditingElementStyle;
	}
}
