using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal.Documents;
using MS.Internal.PtsTable;

namespace System.Windows.Documents
{
	/// <summary>A flow content element that defines a row within a <see cref="T:System.Windows.Documents.Table" />.</summary>
	// Token: 0x020003E7 RID: 999
	[ContentProperty("Cells")]
	public class TableRow : TextElement, IAddChild, IIndexedChild<TableRowGroup>, IAcceptInsertion
	{
		/// <summary>Initializes a new, empty instance of the <see cref="T:System.Windows.Documents.TableRow" /> class.</summary>
		// Token: 0x060036A5 RID: 13989 RVA: 0x000F5B32 File Offset: 0x000F3D32
		public TableRow()
		{
			this.PrivateInitialize();
		}

		/// <summary>
		///     <see cref="P:System.Windows.Documents.TableRow.Cells" /> property to add child <see cref="T:System.Windows.Documents.TableCell" /> elements to a <see cref="T:System.Windows.Documents.TableRow" />.</summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x060036A6 RID: 13990 RVA: 0x000F5B40 File Offset: 0x000F3D40
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TableCell tableCell = value as TableCell;
			if (tableCell != null)
			{
				this.Cells.Add(tableCell);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableCell)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x060036A7 RID: 13991 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000F5BA4 File Offset: 0x000F3DA4
		internal override void OnNewParent(DependencyObject newParent)
		{
			DependencyObject parent = base.Parent;
			if (newParent != null && !(newParent is TableRowGroup))
			{
				throw new InvalidOperationException(SR.Get("TableInvalidParentNodeType", new object[]
				{
					newParent.GetType().ToString()
				}));
			}
			if (parent != null)
			{
				((TableRowGroup)parent).Rows.InternalRemove(this);
			}
			base.OnNewParent(newParent);
			if (newParent != null)
			{
				((TableRowGroup)newParent).Rows.InternalAdd(this);
			}
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x000F5C16 File Offset: 0x000F3E16
		void IIndexedChild<TableRowGroup>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000F5C1E File Offset: 0x000F3E1E
		void IIndexedChild<TableRowGroup>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x000F5C26 File Offset: 0x000F3E26
		void IIndexedChild<TableRowGroup>.OnAfterExitParentTree(TableRowGroup parent)
		{
			this.OnAfterExitParentTree(parent);
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x060036AC RID: 13996 RVA: 0x000F5C2F File Offset: 0x000F3E2F
		// (set) Token: 0x060036AD RID: 13997 RVA: 0x000F5C37 File Offset: 0x000F3E37
		int IIndexedChild<TableRowGroup>.Index
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

		// Token: 0x060036AE RID: 13998 RVA: 0x000F5C40 File Offset: 0x000F3E40
		internal void OnEnterParentTree()
		{
			if (this.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x00002137 File Offset: 0x00000337
		internal void OnExitParentTree()
		{
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x000F5C55 File Offset: 0x000F3E55
		internal void OnAfterExitParentTree(TableRowGroup rowGroup)
		{
			if (rowGroup.Table != null)
			{
				this.Table.OnStructureChanged();
			}
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000F5C6C File Offset: 0x000F3E6C
		internal void ValidateStructure(RowSpanVector rowSpanVector)
		{
			this.SetFlags(!rowSpanVector.Empty(), TableRow.Flags.HasForeignCells);
			this.SetFlags(false, TableRow.Flags.HasRealCells);
			this._formatCellCount = 0;
			this._columnCount = 0;
			int num;
			int num2;
			rowSpanVector.GetFirstAvailableRange(out num, out num2);
			for (int i = 0; i < this._cells.Count; i++)
			{
				TableCell tableCell = this._cells[i];
				int columnSpan = tableCell.ColumnSpan;
				int rowSpan = tableCell.RowSpan;
				while (num + columnSpan > num2)
				{
					rowSpanVector.GetNextAvailableRange(out num, out num2);
				}
				tableCell.ValidateStructure(num);
				if (rowSpan > 1)
				{
					rowSpanVector.Register(tableCell);
				}
				else
				{
					this._formatCellCount++;
				}
				num += columnSpan;
			}
			this._columnCount = num;
			bool flag = false;
			rowSpanVector.GetSpanCells(out this._spannedCells, out flag);
			if (this._formatCellCount > 0 || flag)
			{
				this.SetFlags(true, TableRow.Flags.HasRealCells);
			}
			this._formatCellCount += this._spannedCells.Length;
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x060036B2 RID: 14002 RVA: 0x000F5D60 File Offset: 0x000F3F60
		internal TableRowGroup RowGroup
		{
			get
			{
				return base.Parent as TableRowGroup;
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x060036B3 RID: 14003 RVA: 0x000F5D6D File Offset: 0x000F3F6D
		internal Table Table
		{
			get
			{
				if (this.RowGroup == null)
				{
					return null;
				}
				return this.RowGroup.Table;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TableCellCollection" /> that contains cells of a <see cref="T:System.Windows.Documents.TableRow" />. </summary>
		/// <returns>A collection of child cells.</returns>
		// Token: 0x17000DFD RID: 3581
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x000F5D84 File Offset: 0x000F3F84
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableCellCollection Cells
		{
			get
			{
				return this._cells;
			}
		}

		/// <summary>Returns a value that indicates whether or not the effective value of the <see cref="P:System.Windows.Documents.TableRow.Cells" /> property should be serialized during serialization of a <see cref="T:System.Windows.Documents.TableRow" /> object.</summary>
		/// <returns>
		///     true if the <see cref="P:System.Windows.Documents.TableRow.Cells" /> property should be serialized; otherwise, false.</returns>
		// Token: 0x060036B5 RID: 14005 RVA: 0x000F5D8C File Offset: 0x000F3F8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeCells()
		{
			return this.Cells.Count > 0;
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x060036B6 RID: 14006 RVA: 0x000F5D9C File Offset: 0x000F3F9C
		// (set) Token: 0x060036B7 RID: 14007 RVA: 0x000F5DA4 File Offset: 0x000F3FA4
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

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x000F5DAD File Offset: 0x000F3FAD
		// (set) Token: 0x060036B9 RID: 14009 RVA: 0x000F5DB5 File Offset: 0x000F3FB5
		int IAcceptInsertion.InsertionIndex
		{
			get
			{
				return this.InsertionIndex;
			}
			set
			{
				this.InsertionIndex = value;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x060036BA RID: 14010 RVA: 0x000F5DBE File Offset: 0x000F3FBE
		// (set) Token: 0x060036BB RID: 14011 RVA: 0x000F5DC6 File Offset: 0x000F3FC6
		internal int InsertionIndex
		{
			get
			{
				return this._cellInsertionIndex;
			}
			set
			{
				this._cellInsertionIndex = value;
			}
		}

		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x060036BC RID: 14012 RVA: 0x000F5DCF File Offset: 0x000F3FCF
		internal TableCell[] SpannedCells
		{
			get
			{
				return this._spannedCells;
			}
		}

		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x060036BD RID: 14013 RVA: 0x000F5DD7 File Offset: 0x000F3FD7
		internal int ColumnCount
		{
			get
			{
				return this._columnCount;
			}
		}

		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x000F5DDF File Offset: 0x000F3FDF
		internal bool HasForeignCells
		{
			get
			{
				return this.CheckFlags(TableRow.Flags.HasForeignCells);
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x060036BF RID: 14015 RVA: 0x000F5DE9 File Offset: 0x000F3FE9
		internal bool HasRealCells
		{
			get
			{
				return this.CheckFlags(TableRow.Flags.HasRealCells);
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x060036C0 RID: 14016 RVA: 0x000F5DF3 File Offset: 0x000F3FF3
		internal int FormatCellCount
		{
			get
			{
				return this._formatCellCount;
			}
		}

		// Token: 0x17000E06 RID: 3590
		// (get) Token: 0x060036C1 RID: 14017 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool IsIMEStructuralElement
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x000F5DFB File Offset: 0x000F3FFB
		private void PrivateInitialize()
		{
			this._cells = new TableCellCollection(this);
			this._parentIndex = -1;
			this._cellInsertionIndex = -1;
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x000F5E17 File Offset: 0x000F4017
		private void SetFlags(bool value, TableRow.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000F5E35 File Offset: 0x000F4035
		private bool CheckFlags(TableRow.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x0400254B RID: 9547
		private TableCellCollection _cells;

		// Token: 0x0400254C RID: 9548
		private TableCell[] _spannedCells;

		// Token: 0x0400254D RID: 9549
		private int _parentIndex;

		// Token: 0x0400254E RID: 9550
		private int _cellInsertionIndex;

		// Token: 0x0400254F RID: 9551
		private int _columnCount;

		// Token: 0x04002550 RID: 9552
		private TableRow.Flags _flags;

		// Token: 0x04002551 RID: 9553
		private int _formatCellCount;

		// Token: 0x020008F8 RID: 2296
		[Flags]
		private enum Flags
		{
			// Token: 0x040042E3 RID: 17123
			HasForeignCells = 16,
			// Token: 0x040042E4 RID: 17124
			HasRealCells = 32
		}
	}
}
