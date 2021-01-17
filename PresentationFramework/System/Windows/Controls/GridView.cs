using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls
{
	/// <summary>Represents a view mode that displays data items in columns for a <see cref="T:System.Windows.Controls.ListView" /> control.</summary>
	// Token: 0x020004D5 RID: 1237
	[StyleTypedProperty(Property = "ColumnHeaderContainerStyle", StyleTargetType = typeof(GridViewColumnHeader))]
	[ContentProperty("Columns")]
	public class GridView : ViewBase, IAddChild
	{
		/// <summary>Adds a child object. </summary>
		/// <param name="column">The child object to add.</param>
		// Token: 0x06004CA5 RID: 19621 RVA: 0x0015A606 File Offset: 0x00158806
		void IAddChild.AddChild(object column)
		{
			this.AddChild(column);
		}

		/// <summary>Adds a <see cref="T:System.Windows.Controls.GridViewColumn" /> object to a <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <param name="column">The column to add </param>
		// Token: 0x06004CA6 RID: 19622 RVA: 0x0015A610 File Offset: 0x00158810
		protected virtual void AddChild(object column)
		{
			GridViewColumn gridViewColumn = column as GridViewColumn;
			if (gridViewColumn != null)
			{
				this.Columns.Add(gridViewColumn);
				return;
			}
			throw new InvalidOperationException(SR.Get("ListView_IllegalChildrenType"));
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x06004CA7 RID: 19623 RVA: 0x0015A643 File Offset: 0x00158843
		void IAddChild.AddText(string text)
		{
			this.AddText(text);
		}

		/// <summary>Not supported.</summary>
		/// <param name="text">Text string</param>
		// Token: 0x06004CA8 RID: 19624 RVA: 0x0015A606 File Offset: 0x00158806
		protected virtual void AddText(string text)
		{
			this.AddChild(text);
		}

		/// <summary>Returns the string representation of the <see cref="T:System.Windows.Controls.GridView" /> object.</summary>
		/// <returns>A string that indicates the number of columns in the <see cref="T:System.Windows.Controls.GridView" />.</returns>
		// Token: 0x06004CA9 RID: 19625 RVA: 0x0015A64C File Offset: 0x0015884C
		public override string ToString()
		{
			return SR.Get("ToStringFormatString_GridView", new object[]
			{
				base.GetType(),
				this.Columns.Count
			});
		}

		/// <summary>Gets the <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation for this <see cref="T:System.Windows.Controls.GridView" /> object.</summary>
		/// <param name="parent">The <see cref="T:System.Windows.Controls.ListView" /> control that implements this <see cref="T:System.Windows.Controls.GridView" /> view.</param>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.GridViewAutomationPeer" /> for this <see cref="T:System.Windows.Controls.GridView" />.</returns>
		// Token: 0x06004CAA RID: 19626 RVA: 0x0015A67A File Offset: 0x0015887A
		protected internal override IViewAutomationPeer GetAutomationPeer(ListView parent)
		{
			return new GridViewAutomationPeer(this, parent);
		}

		/// <summary>Gets the key that references the style that is defined for the <see cref="T:System.Windows.Controls.ScrollViewer" /> control that encloses the content that is displayed by a <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceKey" /> that references the <see cref="T:System.Windows.Style" /> that is applied to the <see cref="T:System.Windows.Controls.ScrollViewer" /> control for a <see cref="T:System.Windows.Controls.GridView" />. The default value is the style for the <see cref="T:System.Windows.Controls.ScrollViewer" /> object of a <see cref="T:System.Windows.Controls.ListView" /> in the current theme.</returns>
		// Token: 0x170012AE RID: 4782
		// (get) Token: 0x06004CAB RID: 19627 RVA: 0x0015A683 File Offset: 0x00158883
		public static ResourceKey GridViewScrollViewerStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewScrollViewerStyleKey;
			}
		}

		/// <summary>Gets the key that references the style that is defined for the <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceKey" /> that references the <see cref="T:System.Windows.Style" /> that is applied to the <see cref="T:System.Windows.Controls.GridView" />. The default value is the style for the <see cref="T:System.Windows.Controls.ListView" /> in the current theme.</returns>
		// Token: 0x170012AF RID: 4783
		// (get) Token: 0x06004CAC RID: 19628 RVA: 0x0015A68A File Offset: 0x0015888A
		public static ResourceKey GridViewStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewStyleKey;
			}
		}

		/// <summary>Gets the key that references the style that is defined for each <see cref="T:System.Windows.Controls.ListViewItem" /> in a <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>A <see cref="T:System.Windows.ResourceKey" /> that references the style for each <see cref="T:System.Windows.Controls.ListViewItem" />. The default value references the default style for a <see cref="T:System.Windows.Controls.ListViewItem" /> control in the current theme.</returns>
		// Token: 0x170012B0 RID: 4784
		// (get) Token: 0x06004CAD RID: 19629 RVA: 0x0015A691 File Offset: 0x00158891
		public static ResourceKey GridViewItemContainerStyleKey
		{
			get
			{
				return SystemResourceKey.GridViewItemContainerStyleKey;
			}
		}

		/// <summary>Gets the contents of the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> attached property.</summary>
		/// <param name="element">The <see cref="T:System.Windows.DependencyObject" /> that is associated with the collection.</param>
		/// <returns>The <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> of the specified <see cref="T:System.Windows.DependencyObject" />.</returns>
		// Token: 0x06004CAE RID: 19630 RVA: 0x0015A698 File Offset: 0x00158898
		public static GridViewColumnCollection GetColumnCollection(DependencyObject element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (GridViewColumnCollection)element.GetValue(GridView.ColumnCollectionProperty);
		}

		/// <summary>Sets the contents of the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> attached property.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Controls.GridView" /> object.</param>
		/// <param name="collection">The <see cref="T:System.Windows.Controls.GridViewColumnCollection" /> object to assign.</param>
		// Token: 0x06004CAF RID: 19631 RVA: 0x0015A6B8 File Offset: 0x001588B8
		public static void SetColumnCollection(DependencyObject element, GridViewColumnCollection collection)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			element.SetValue(GridView.ColumnCollectionProperty, collection);
		}

		/// <summary>Determines whether to serialize the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> attached property.</summary>
		/// <param name="obj">The object on which the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> is set.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Controls.GridView.ColumnCollection" /> must be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06004CB0 RID: 19632 RVA: 0x0015A6D4 File Offset: 0x001588D4
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool ShouldSerializeColumnCollection(DependencyObject obj)
		{
			ListViewItem listViewItem = obj as ListViewItem;
			if (listViewItem != null)
			{
				ListView listView = listViewItem.ParentSelector as ListView;
				if (listView != null)
				{
					GridView gridView = listView.View as GridView;
					if (gridView != null)
					{
						GridViewColumnCollection gridViewColumnCollection = listViewItem.ReadLocalValue(GridView.ColumnCollectionProperty) as GridViewColumnCollection;
						return gridViewColumnCollection != gridView.Columns;
					}
				}
			}
			return true;
		}

		/// <summary>Gets the collection of <see cref="T:System.Windows.Controls.GridViewColumn" /> objects that is defined for this <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>The collection of columns in the <see cref="T:System.Windows.Controls.GridView" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B1 RID: 4785
		// (get) Token: 0x06004CB1 RID: 19633 RVA: 0x0015A728 File Offset: 0x00158928
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public GridViewColumnCollection Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = new GridViewColumnCollection();
					this._columns.Owner = this;
					this._columns.InViewMode = true;
				}
				return this._columns;
			}
		}

		/// <summary>Gets or sets the style to apply to column headers. </summary>
		/// <returns>The <see cref="T:System.Windows.Style" /> that is used to define the display properties for column headers. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B2 RID: 4786
		// (get) Token: 0x06004CB2 RID: 19634 RVA: 0x0015A75B File Offset: 0x0015895B
		// (set) Token: 0x06004CB3 RID: 19635 RVA: 0x0015A76D File Offset: 0x0015896D
		public Style ColumnHeaderContainerStyle
		{
			get
			{
				return (Style)base.GetValue(GridView.ColumnHeaderContainerStyleProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderContainerStyleProperty, value);
			}
		}

		/// <summary>Gets or sets a template to use to display the column headers. </summary>
		/// <returns>The <see cref="T:System.Windows.DataTemplate" /> to use to display the column headers as part of the <see cref="T:System.Windows.Controls.GridView" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B3 RID: 4787
		// (get) Token: 0x06004CB4 RID: 19636 RVA: 0x0015A77B File Offset: 0x0015897B
		// (set) Token: 0x06004CB5 RID: 19637 RVA: 0x0015A78D File Offset: 0x0015898D
		public DataTemplate ColumnHeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(GridView.ColumnHeaderTemplateProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderTemplateProperty, value);
			}
		}

		// Token: 0x06004CB6 RID: 19638 RVA: 0x0015A79C File Offset: 0x0015899C
		private static void OnColumnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridView d2 = (GridView)d;
			Helper.CheckTemplateAndTemplateSelector("GridViewColumnHeader", GridView.ColumnHeaderTemplateProperty, GridView.ColumnHeaderTemplateSelectorProperty, d2);
		}

		/// <summary>Gets or sets the selector object that provides logic for selecting a template to use for each column header. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.DataTemplateSelector" /> object that determines the data template to use for each column header. The default value is <see langword="null" />. </returns>
		// Token: 0x170012B4 RID: 4788
		// (get) Token: 0x06004CB7 RID: 19639 RVA: 0x0015A7C5 File Offset: 0x001589C5
		// (set) Token: 0x06004CB8 RID: 19640 RVA: 0x0015A7D7 File Offset: 0x001589D7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DataTemplateSelector ColumnHeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(GridView.ColumnHeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06004CB9 RID: 19641 RVA: 0x0015A7E8 File Offset: 0x001589E8
		private static void OnColumnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			GridView d2 = (GridView)d;
			Helper.CheckTemplateAndTemplateSelector("GridViewColumnHeader", GridView.ColumnHeaderTemplateProperty, GridView.ColumnHeaderTemplateSelectorProperty, d2);
		}

		/// <summary>Gets or sets a composite string that specifies how to format the column headers of the <see cref="T:System.Windows.Controls.GridView" /> if they are displayed as strings.</summary>
		/// <returns>A composite string that specifies how to format the column headers of the <see cref="T:System.Windows.Controls.GridView" /> if they are displayed as strings. The default is <see langword="null" />.</returns>
		// Token: 0x170012B5 RID: 4789
		// (get) Token: 0x06004CBA RID: 19642 RVA: 0x0015A811 File Offset: 0x00158A11
		// (set) Token: 0x06004CBB RID: 19643 RVA: 0x0015A823 File Offset: 0x00158A23
		public string ColumnHeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(GridView.ColumnHeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderStringFormatProperty, value);
			}
		}

		/// <summary>Gets or sets whether columns in a <see cref="T:System.Windows.Controls.GridView" /> can be reordered by a drag-and-drop operation. </summary>
		/// <returns>
		///     <see langword="true" /> if columns can be reordered; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x170012B6 RID: 4790
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x0015A831 File Offset: 0x00158A31
		// (set) Token: 0x06004CBD RID: 19645 RVA: 0x0015A843 File Offset: 0x00158A43
		public bool AllowsColumnReorder
		{
			get
			{
				return (bool)base.GetValue(GridView.AllowsColumnReorderProperty);
			}
			set
			{
				base.SetValue(GridView.AllowsColumnReorderProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.ContextMenu" /> for the <see cref="T:System.Windows.Controls.GridView" />. </summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ContextMenu" /> for the column headers in a <see cref="T:System.Windows.Controls.GridView" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170012B7 RID: 4791
		// (get) Token: 0x06004CBE RID: 19646 RVA: 0x0015A851 File Offset: 0x00158A51
		// (set) Token: 0x06004CBF RID: 19647 RVA: 0x0015A863 File Offset: 0x00158A63
		public ContextMenu ColumnHeaderContextMenu
		{
			get
			{
				return (ContextMenu)base.GetValue(GridView.ColumnHeaderContextMenuProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderContextMenuProperty, value);
			}
		}

		/// <summary>Gets or sets the content of a tooltip that appears when the mouse pointer pauses over one of the column headers. </summary>
		/// <returns>An object that represents the content that appears as a tooltip when the mouse pointer is paused over one of the column headers. The default value is not defined.</returns>
		// Token: 0x170012B8 RID: 4792
		// (get) Token: 0x06004CC0 RID: 19648 RVA: 0x0015A871 File Offset: 0x00158A71
		// (set) Token: 0x06004CC1 RID: 19649 RVA: 0x0015A87E File Offset: 0x00158A7E
		public object ColumnHeaderToolTip
		{
			get
			{
				return base.GetValue(GridView.ColumnHeaderToolTipProperty);
			}
			set
			{
				base.SetValue(GridView.ColumnHeaderToolTipProperty, value);
			}
		}

		/// <summary>Prepares a <see cref="T:System.Windows.Controls.ListViewItem" /> for display according to the definition of this <see cref="T:System.Windows.Controls.GridView" /> object.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> to display.</param>
		// Token: 0x06004CC2 RID: 19650 RVA: 0x0015A88C File Offset: 0x00158A8C
		protected internal override void PrepareItem(ListViewItem item)
		{
			base.PrepareItem(item);
			GridView.SetColumnCollection(item, this._columns);
		}

		/// <summary>Removes all settings, bindings, and styling from a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> to clear.</param>
		// Token: 0x06004CC3 RID: 19651 RVA: 0x0015A8A1 File Offset: 0x00158AA1
		protected internal override void ClearItem(ListViewItem item)
		{
			item.ClearValue(GridView.ColumnCollectionProperty);
			base.ClearItem(item);
		}

		/// <summary>Gets the reference for the default style for the <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>The <see cref="P:System.Windows.Controls.GridView.GridViewStyleKey" />. The default value is the <see cref="P:System.Windows.Controls.GridView.GridViewStyleKey" /> in the current theme.</returns>
		// Token: 0x170012B9 RID: 4793
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x0015A8B5 File Offset: 0x00158AB5
		protected internal override object DefaultStyleKey
		{
			get
			{
				return GridView.GridViewStyleKey;
			}
		}

		/// <summary>Gets the reference to the default style for the container of the data items in the <see cref="T:System.Windows.Controls.GridView" />.</summary>
		/// <returns>The <see cref="P:System.Windows.Controls.GridView.GridViewItemContainerStyleKey" />. The default value is the <see cref="P:System.Windows.Controls.GridView.GridViewItemContainerStyleKey" /> in the current theme.</returns>
		// Token: 0x170012BA RID: 4794
		// (get) Token: 0x06004CC5 RID: 19653 RVA: 0x0015A8BC File Offset: 0x00158ABC
		protected internal override object ItemContainerDefaultStyleKey
		{
			get
			{
				return GridView.GridViewItemContainerStyleKey;
			}
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0015A8C4 File Offset: 0x00158AC4
		internal override void OnInheritanceContextChangedCore(EventArgs args)
		{
			base.OnInheritanceContextChangedCore(args);
			if (this._columns != null)
			{
				foreach (GridViewColumn gridViewColumn in this._columns)
				{
					gridViewColumn.OnInheritanceContextChanged(args);
				}
			}
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0015A920 File Offset: 0x00158B20
		internal override void OnThemeChanged()
		{
			if (this._columns != null)
			{
				for (int i = 0; i < this._columns.Count; i++)
				{
					this._columns[i].OnThemeChanged();
				}
			}
		}

		// Token: 0x170012BB RID: 4795
		// (get) Token: 0x06004CC8 RID: 19656 RVA: 0x0015A95C File Offset: 0x00158B5C
		// (set) Token: 0x06004CC9 RID: 19657 RVA: 0x0015A964 File Offset: 0x00158B64
		internal GridViewHeaderRowPresenter HeaderRowPresenter
		{
			get
			{
				return this._gvheaderRP;
			}
			set
			{
				this._gvheaderRP = value;
			}
		}

		/// <summary>Identifies the <see cref="F:System.Windows.Controls.GridView.ColumnCollectionProperty" /> attachedproperty. </summary>
		// Token: 0x04002B28 RID: 11048
		public static readonly DependencyProperty ColumnCollectionProperty = DependencyProperty.RegisterAttached("ColumnCollection", typeof(GridViewColumnCollection), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderContainerStyle" /> dependency property. </summary>
		// Token: 0x04002B29 RID: 11049
		public static readonly DependencyProperty ColumnHeaderContainerStyleProperty = DependencyProperty.Register("ColumnHeaderContainerStyle", typeof(Style), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderTemplate" /> dependency property. </summary>
		// Token: 0x04002B2A RID: 11050
		public static readonly DependencyProperty ColumnHeaderTemplateProperty = DependencyProperty.Register("ColumnHeaderTemplate", typeof(DataTemplate), typeof(GridView), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridView.OnColumnHeaderTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderTemplateSelector" /> dependency property. </summary>
		// Token: 0x04002B2B RID: 11051
		public static readonly DependencyProperty ColumnHeaderTemplateSelectorProperty = DependencyProperty.Register("ColumnHeaderTemplateSelector", typeof(DataTemplateSelector), typeof(GridView), new FrameworkPropertyMetadata(new PropertyChangedCallback(GridView.OnColumnHeaderTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderStringFormat" /> dependency property.</returns>
		// Token: 0x04002B2C RID: 11052
		public static readonly DependencyProperty ColumnHeaderStringFormatProperty = DependencyProperty.Register("ColumnHeaderStringFormat", typeof(string), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.AllowsColumnReorder" /> dependency property. </summary>
		// Token: 0x04002B2D RID: 11053
		public static readonly DependencyProperty AllowsColumnReorderProperty = DependencyProperty.Register("AllowsColumnReorder", typeof(bool), typeof(GridView), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderContextMenu" /> dependency property. </summary>
		// Token: 0x04002B2E RID: 11054
		public static readonly DependencyProperty ColumnHeaderContextMenuProperty = DependencyProperty.Register("ColumnHeaderContextMenu", typeof(ContextMenu), typeof(GridView));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.GridView.ColumnHeaderToolTip" /> dependency property. </summary>
		// Token: 0x04002B2F RID: 11055
		public static readonly DependencyProperty ColumnHeaderToolTipProperty = DependencyProperty.Register("ColumnHeaderToolTip", typeof(object), typeof(GridView));

		// Token: 0x04002B30 RID: 11056
		private GridViewColumnCollection _columns;

		// Token: 0x04002B31 RID: 11057
		private GridViewHeaderRowPresenter _gvheaderRP;
	}
}
