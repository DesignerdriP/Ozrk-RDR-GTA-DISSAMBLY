using System;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using System.Windows.Media;
using MS.Internal;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>A flow content element that defines a cell of content within a <see cref="T:System.Windows.Documents.Table" />.</summary>
	// Token: 0x020003E3 RID: 995
	[ContentProperty("Blocks")]
	public class TableCell : TextElement, IIndexedChild<TableRow>
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.TableCell" /> class.</summary>
		// Token: 0x06003617 RID: 13847 RVA: 0x000F4FAA File Offset: 0x000F31AA
		public TableCell()
		{
			this.PrivateInitialize();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.TableCell" /> class, taking a specified <see cref="T:System.Windows.Documents.Block" /> object as the initial contents of the new <see cref="T:System.Windows.Documents.TableCell" />.</summary>
		/// <param name="blockItem">A <see cref="T:System.Windows.Documents.Block" /> object specifying the initial contents of the new <see cref="T:System.Windows.Documents.TableCell" />.</param>
		// Token: 0x06003618 RID: 13848 RVA: 0x000F4FB8 File Offset: 0x000F31B8
		public TableCell(Block blockItem)
		{
			this.PrivateInitialize();
			if (blockItem == null)
			{
				throw new ArgumentNullException("blockItem");
			}
			this.Blocks.Add(blockItem);
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x000F4FE0 File Offset: 0x000F31E0
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			TableRow tableRow = newParent as TableRow;
			if (newParent != null && tableRow == null)
			{
				throw new InvalidOperationException(SR.Get("TableInvalidParentNodeType", new object[]
				{
					newParent.GetType().ToString()
				}));
			}
			if (parent != null)
			{
				((TableRow)parent).Cells.InternalRemove(this);
			}
			base.OnNewParent(newParent);
			if (tableRow != null && tableRow.Cells != null)
			{
				tableRow.Cells.InternalAdd(this);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.BlockCollection" /> containing the top-level <see cref="T:System.Windows.Documents.Block" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.TableCell" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.BlockCollection" /> containing the <see cref="T:System.Windows.Documents.Block" /> elements that comprise the contents of the <see cref="T:System.Windows.Documents.TableCell" />This property has no default value.</returns>
		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x0600361A RID: 13850 RVA: 0x000C3A60 File Offset: 0x000C1C60
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public BlockCollection Blocks
		{
			get
			{
				return new BlockCollection(this, true);
			}
		}

		/// <summary>Gets or sets the number of columns that the <see cref="T:System.Windows.Documents.TableCell" /> should span.  </summary>
		/// <returns>The number of columns the <see cref="T:System.Windows.Documents.TableCell" /> should span.The default value is 1 (no spanning).</returns>
		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x0600361B RID: 13851 RVA: 0x000F5057 File Offset: 0x000F3257
		// (set) Token: 0x0600361C RID: 13852 RVA: 0x000F5069 File Offset: 0x000F3269
		public int ColumnSpan
		{
			get
			{
				return (int)base.GetValue(TableCell.ColumnSpanProperty);
			}
			set
			{
				base.SetValue(TableCell.ColumnSpanProperty, value);
			}
		}

		/// <summary>Gets or sets the number of rows that the <see cref="T:System.Windows.Documents.TableCell" /> should span.  </summary>
		/// <returns>The number of rows the <see cref="T:System.Windows.Documents.TableCell" /> should span.The default value is 1 (no spanning).</returns>
		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x0600361D RID: 13853 RVA: 0x000F507C File Offset: 0x000F327C
		// (set) Token: 0x0600361E RID: 13854 RVA: 0x000F508E File Offset: 0x000F328E
		public int RowSpan
		{
			get
			{
				return (int)base.GetValue(TableCell.RowSpanProperty);
			}
			set
			{
				base.SetValue(TableCell.RowSpanProperty, value);
			}
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Documents.TableCell" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Documents.TableCell" />.</returns>
		// Token: 0x0600361F RID: 13855 RVA: 0x000F50A1 File Offset: 0x000F32A1
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TableCellAutomationPeer(this);
		}

		/// <summary>Gets or sets the padding thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of padding to apply, in device independent pixels.The default value is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x06003620 RID: 13856 RVA: 0x000F50A9 File Offset: 0x000F32A9
		// (set) Token: 0x06003621 RID: 13857 RVA: 0x000F50BB File Offset: 0x000F32BB
		public Thickness Padding
		{
			get
			{
				return (Thickness)base.GetValue(TableCell.PaddingProperty);
			}
			set
			{
				base.SetValue(TableCell.PaddingProperty, value);
			}
		}

		/// <summary>Gets or sets the border thickness for the element.  </summary>
		/// <returns>A <see cref="T:System.Windows.Thickness" /> structure specifying the amount of border to apply, in device independent pixels.The default value is a uniform thickness of zero (0.0).</returns>
		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x06003622 RID: 13858 RVA: 0x000F50CE File Offset: 0x000F32CE
		// (set) Token: 0x06003623 RID: 13859 RVA: 0x000F50E0 File Offset: 0x000F32E0
		public Thickness BorderThickness
		{
			get
			{
				return (Thickness)base.GetValue(TableCell.BorderThicknessProperty);
			}
			set
			{
				base.SetValue(TableCell.BorderThicknessProperty, value);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Media.Brush" /> to use when painting the element's border.  </summary>
		/// <returns>The brush used to apply to the element's border.The default value is a null brush.</returns>
		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06003624 RID: 13860 RVA: 0x000F50F3 File Offset: 0x000F32F3
		// (set) Token: 0x06003625 RID: 13861 RVA: 0x000F5105 File Offset: 0x000F3305
		public Brush BorderBrush
		{
			get
			{
				return (Brush)base.GetValue(TableCell.BorderBrushProperty);
			}
			set
			{
				base.SetValue(TableCell.BorderBrushProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates the horizontal alignment of text content.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.TextAlignment" /> enumerations specifying the desired alignment.The default value is <see cref="F:System.Windows.TextAlignment.Left" />.</returns>
		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06003626 RID: 13862 RVA: 0x000F5113 File Offset: 0x000F3313
		// (set) Token: 0x06003627 RID: 13863 RVA: 0x000F5125 File Offset: 0x000F3325
		public TextAlignment TextAlignment
		{
			get
			{
				return (TextAlignment)base.GetValue(TableCell.TextAlignmentProperty);
			}
			set
			{
				base.SetValue(TableCell.TextAlignmentProperty, value);
			}
		}

		/// <summary>Gets or sets a value that specifies the relative direction for flow of content within a <see cref="T:System.Windows.Documents.TableCell" /> element.  </summary>
		/// <returns>A member of the <see cref="T:System.Windows.FlowDirection" /> enumeration specifying the relative flow direction.  Getting this property returns the currently effective flow direction.  Setting this property causes the contents of the <see cref="T:System.Windows.Documents.TableCell" /> element to re-flow in the indicated direction.The default value is <see cref="F:System.Windows.FlowDirection.LeftToRight" />.</returns>
		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06003628 RID: 13864 RVA: 0x000F5138 File Offset: 0x000F3338
		// (set) Token: 0x06003629 RID: 13865 RVA: 0x000F514A File Offset: 0x000F334A
		public FlowDirection FlowDirection
		{
			get
			{
				return (FlowDirection)base.GetValue(TableCell.FlowDirectionProperty);
			}
			set
			{
				base.SetValue(TableCell.FlowDirectionProperty, value);
			}
		}

		/// <summary>Gets or sets the height of each line of content.  </summary>
		/// <returns>A double value specifying the height of line in device independent pixels.  <see cref="P:System.Windows.Documents.TableCell.LineHeight" /> must be equal to or greater than 0.0034 and equal to or less then 160000.A value of <see cref="F:System.Double.NaN" /> (equivalent to an attribute value of "Auto") causes the line height to be determined automatically from the current font characteristics.  The default value is <see cref="F:System.Double.NaN" />.</returns>
		/// <exception cref="T:System.ArgumentException">Raised if an attempt is made to set <see cref="P:System.Windows.Controls.TextBlock.LineHeight" /> to a non-positive value.</exception>
		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x0600362A RID: 13866 RVA: 0x000F515D File Offset: 0x000F335D
		// (set) Token: 0x0600362B RID: 13867 RVA: 0x000F516F File Offset: 0x000F336F
		[TypeConverter(typeof(LengthConverter))]
		public double LineHeight
		{
			get
			{
				return (double)base.GetValue(TableCell.LineHeightProperty);
			}
			set
			{
				base.SetValue(TableCell.LineHeightProperty, value);
			}
		}

		/// <summary>Gets or sets the mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Documents.TableCell" />.  </summary>
		/// <returns>The mechanism by which a line box is determined for each line of text within the <see cref="T:System.Windows.Documents.TableCell" />. The default value is <see cref="F:System.Windows.LineStackingStrategy.MaxHeight" />.</returns>
		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x0600362C RID: 13868 RVA: 0x000F5182 File Offset: 0x000F3382
		// (set) Token: 0x0600362D RID: 13869 RVA: 0x000F5194 File Offset: 0x000F3394
		public LineStackingStrategy LineStackingStrategy
		{
			get
			{
				return (LineStackingStrategy)base.GetValue(TableCell.LineStackingStrategyProperty);
			}
			set
			{
				base.SetValue(TableCell.LineStackingStrategyProperty, value);
			}
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000F51A7 File Offset: 0x000F33A7
		void IIndexedChild<TableRow>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000F51AF File Offset: 0x000F33AF
		void IIndexedChild<TableRow>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000F51B7 File Offset: 0x000F33B7
		void IIndexedChild<TableRow>.OnAfterExitParentTree(TableRow parent)
		{
			this.OnAfterExitParentTree(parent);
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06003631 RID: 13873 RVA: 0x000F51C0 File Offset: 0x000F33C0
		// (set) Token: 0x06003632 RID: 13874 RVA: 0x000F51C8 File Offset: 0x000F33C8
		int IIndexedChild<TableRow>.Index
		{
			get
			{
				return this.Index;
			}
			set
			{
				this.Index = value;
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000F51D1 File Offset: 0x000F33D1
		internal void OnEnterParentTree()
		{
			if (this.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x00002137 File Offset: 0x00000337
		internal void OnExitParentTree()
		{
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000F51E6 File Offset: 0x000F33E6
		internal void OnAfterExitParentTree(TableRow row)
		{
			if (row.Table != null)
			{
				row.Table.OnStructureChanged();
			}
		}

		// Token: 0x06003636 RID: 13878 RVA: 0x000F51FB File Offset: 0x000F33FB
		internal void ValidateStructure(int columnIndex)
		{
			Invariant.Assert(columnIndex >= 0);
			this._columnIndex = columnIndex;
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06003637 RID: 13879 RVA: 0x000F5210 File Offset: 0x000F3410
		internal TableRow Row
		{
			get
			{
				return base.Parent as TableRow;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06003638 RID: 13880 RVA: 0x000F521D File Offset: 0x000F341D
		internal Table Table
		{
			get
			{
				if (this.Row == null)
				{
					return null;
				}
				return this.Row.Table;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06003639 RID: 13881 RVA: 0x000F5234 File Offset: 0x000F3434
		// (set) Token: 0x0600363A RID: 13882 RVA: 0x000F523C File Offset: 0x000F343C
		internal int Index
		{
			get
			{
				return this._parentIndex;
			}
			set
			{
				this._parentIndex = value;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x0600363B RID: 13883 RVA: 0x000F5245 File Offset: 0x000F3445
		internal int RowIndex
		{
			get
			{
				return this.Row.Index;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x0600363C RID: 13884 RVA: 0x000F5252 File Offset: 0x000F3452
		internal int RowGroupIndex
		{
			get
			{
				return this.Row.RowGroup.Index;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x0600363D RID: 13885 RVA: 0x000F5264 File Offset: 0x000F3464
		// (set) Token: 0x0600363E RID: 13886 RVA: 0x000F526C File Offset: 0x000F346C
		internal int ColumnIndex
		{
			get
			{
				return this._columnIndex;
			}
			set
			{
				this._columnIndex = value;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x0600363F RID: 13887 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x000F5275 File Offset: 0x000F3475
		private void PrivateInitialize()
		{
			this._parentIndex = -1;
			this._columnIndex = -1;
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x000F5288 File Offset: 0x000F3488
		private static bool IsValidRowSpan(object value)
		{
			int num = (int)value;
			return num >= 1 && num <= 1000000;
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x000F52B0 File Offset: 0x000F34B0
		private static bool IsValidColumnSpan(object value)
		{
			int num = (int)value;
			return num >= 1 && num <= 1000;
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000F52D8 File Offset: 0x000F34D8
		private static void OnColumnSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TableCell tableCell = (TableCell)d;
			if (tableCell.Table != null)
			{
				tableCell.Table.OnStructureChanged();
			}
			TableCellAutomationPeer tableCellAutomationPeer = ContentElementAutomationPeer.FromElement(tableCell) as TableCellAutomationPeer;
			if (tableCellAutomationPeer != null)
			{
				tableCellAutomationPeer.OnColumnSpanChanged((int)e.OldValue, (int)e.NewValue);
			}
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000F532C File Offset: 0x000F352C
		private static void OnRowSpanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TableCell tableCell = (TableCell)d;
			if (tableCell.Table != null)
			{
				tableCell.Table.OnStructureChanged();
			}
			TableCellAutomationPeer tableCellAutomationPeer = ContentElementAutomationPeer.FromElement(tableCell) as TableCellAutomationPeer;
			if (tableCellAutomationPeer != null)
			{
				tableCellAutomationPeer.OnRowSpanChanged((int)e.OldValue, (int)e.NewValue);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.Padding" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.Padding" /> dependency property.</returns>
		// Token: 0x0400253B RID: 9531
		public static readonly DependencyProperty PaddingProperty = Block.PaddingProperty.AddOwner(typeof(TableCell), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.BorderThickness" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.BorderThickness" /> dependency property.</returns>
		// Token: 0x0400253C RID: 9532
		public static readonly DependencyProperty BorderThicknessProperty = Block.BorderThicknessProperty.AddOwner(typeof(TableCell), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.AffectsMeasure));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.BorderBrush" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.BorderBrush" /> dependency property.</returns>
		// Token: 0x0400253D RID: 9533
		public static readonly DependencyProperty BorderBrushProperty = Block.BorderBrushProperty.AddOwner(typeof(TableCell), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.TextAlignment" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.TextAlignment" /> dependency property.</returns>
		// Token: 0x0400253E RID: 9534
		public static readonly DependencyProperty TextAlignmentProperty = Block.TextAlignmentProperty.AddOwner(typeof(TableCell));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.FlowDirection" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.FlowDirection" /> dependency property.</returns>
		// Token: 0x0400253F RID: 9535
		public static readonly DependencyProperty FlowDirectionProperty = Block.FlowDirectionProperty.AddOwner(typeof(TableCell));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.LineHeight" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.LineHeight" /> dependency property.</returns>
		// Token: 0x04002540 RID: 9536
		public static readonly DependencyProperty LineHeightProperty = Block.LineHeightProperty.AddOwner(typeof(TableCell));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.LineStackingStrategy" />  dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.LineStackingStrategy" /> dependency property.</returns>
		// Token: 0x04002541 RID: 9537
		public static readonly DependencyProperty LineStackingStrategyProperty = Block.LineStackingStrategyProperty.AddOwner(typeof(TableCell));

		// Token: 0x04002542 RID: 9538
		private int _parentIndex;

		// Token: 0x04002543 RID: 9539
		private int _columnIndex;

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.ColumnSpan" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.ColumnSpan" /> dependency property.</returns>
		// Token: 0x04002544 RID: 9540
		public static readonly DependencyProperty ColumnSpanProperty = DependencyProperty.Register("ColumnSpan", typeof(int), typeof(TableCell), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TableCell.OnColumnSpanChanged)), new ValidateValueCallback(TableCell.IsValidColumnSpan));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableCell.RowSpan" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableCell.RowSpan" /> dependency property.</returns>
		// Token: 0x04002545 RID: 9541
		public static readonly DependencyProperty RowSpanProperty = DependencyProperty.Register("RowSpan", typeof(int), typeof(TableCell), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TableCell.OnRowSpanChanged)), new ValidateValueCallback(TableCell.IsValidRowSpan));
	}
}
