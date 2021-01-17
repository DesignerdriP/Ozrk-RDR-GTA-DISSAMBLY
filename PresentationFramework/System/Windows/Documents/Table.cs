using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>A block-level flow content element that provides a grid-based presentation organized by rows and columns.</summary>
	// Token: 0x020003E2 RID: 994
	[ContentProperty("RowGroups")]
	public class Table : Block, IAddChild, IAcceptInsertion
	{
		// Token: 0x060035FA RID: 13818 RVA: 0x000F4C0C File Offset: 0x000F2E0C
		static Table()
		{
			Block.MarginProperty.OverrideMetadata(typeof(Table), new FrameworkPropertyMetadata(new Thickness(double.NaN)));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.Table" /> class.</summary>
		// Token: 0x060035FB RID: 13819 RVA: 0x000F4C88 File Offset: 0x000F2E88
		public Table()
		{
			this.PrivateInitialize();
		}

		/// <summary>Adds a child object. </summary>
		/// <param name="value">The child object to add.</param>
		// Token: 0x060035FC RID: 13820 RVA: 0x000F4C98 File Offset: 0x000F2E98
		void IAddChild.AddChild(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			TableRowGroup tableRowGroup = value as TableRowGroup;
			if (tableRowGroup != null)
			{
				this.RowGroups.Add(tableRowGroup);
				return;
			}
			throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
			{
				value.GetType(),
				typeof(TableRowGroup)
			}), "value");
		}

		/// <summary>Adds the text content of a node to the object. </summary>
		/// <param name="text">The text to add to the object.</param>
		// Token: 0x060035FD RID: 13821 RVA: 0x0000B31C File Offset: 0x0000951C
		void IAddChild.AddText(string text)
		{
			XamlSerializerUtil.ThrowIfNonWhiteSpaceInAddText(text, this);
		}

		/// <summary>Called before an element is initialized.</summary>
		// Token: 0x060035FE RID: 13822 RVA: 0x000F4CFA File Offset: 0x000F2EFA
		public override void BeginInit()
		{
			base.BeginInit();
			this._initializing = true;
		}

		/// <summary>Called immediately after an element is initialized.</summary>
		// Token: 0x060035FF RID: 13823 RVA: 0x000F4D09 File Offset: 0x000F2F09
		public override void EndInit()
		{
			this._initializing = false;
			this.OnStructureChanged();
			base.EndInit();
		}

		/// <summary>Gets an enumerator that can be used to iterate the logical children of the <see cref="T:System.Windows.Documents.Table" />.</summary>
		/// <returns>An enumerator for the logical children of the <see cref="T:System.Windows.Documents.Table" />.</returns>
		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06003600 RID: 13824 RVA: 0x000F4D1E File Offset: 0x000F2F1E
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				return new Table.TableChildrenCollectionEnumeratorSimple(this);
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TableColumnCollection" /> object that contains the columns hosted by the table.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TableColumnCollection" /> object that contains the columns (represented by <see cref="T:System.Windows.Documents.TableColumn" /> objects) hosted by the table. Note that this number might not be the actual number of columns rendered in the table. It is the <see cref="T:System.Windows.Documents.TableCell" /> objects in a table that determine how many columns are actually rendered.This property has no default value.</returns>
		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06003601 RID: 13825 RVA: 0x000F4D26 File Offset: 0x000F2F26
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableColumnCollection Columns
		{
			get
			{
				return this._columns;
			}
		}

		/// <summary>Gets a value that indicates whether or not the effective value of the <see cref="P:System.Windows.Documents.Table.Columns" /> property should be serialized during serialization of a <see cref="T:System.Windows.Documents.Table" /> object.</summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Documents.Table.Columns" /> property should be serialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x06003602 RID: 13826 RVA: 0x000F4D2E File Offset: 0x000F2F2E
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool ShouldSerializeColumns()
		{
			return this.Columns.Count > 0;
		}

		/// <summary>Gets a <see cref="T:System.Windows.Documents.TableRowGroupCollection" /> collection object that contains the row groups hosted by the table.</summary>
		/// <returns>A <see cref="T:System.Windows.Documents.TableRowGroupCollection" /> collection object that contains the row groups (represented by <see cref="T:System.Windows.Documents.TableRowGroup" /> objects) hosted by the table.This property has no default value.</returns>
		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06003603 RID: 13827 RVA: 0x000F4D3E File Offset: 0x000F2F3E
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TableRowGroupCollection RowGroups
		{
			get
			{
				return this._rowGroups;
			}
		}

		/// <summary>Gets or sets the amount of spacing between cells in a table.  </summary>
		/// <returns>The amount of spacing between cells in a table, in device independent pixels.The default value is 2.0.</returns>
		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06003604 RID: 13828 RVA: 0x000F4D46 File Offset: 0x000F2F46
		// (set) Token: 0x06003605 RID: 13829 RVA: 0x000F4D58 File Offset: 0x000F2F58
		[TypeConverter(typeof(LengthConverter))]
		public double CellSpacing
		{
			get
			{
				return (double)base.GetValue(Table.CellSpacingProperty);
			}
			set
			{
				base.SetValue(Table.CellSpacingProperty, value);
			}
		}

		/// <summary>Creates and returns an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Documents.Table" />.</summary>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> object for this <see cref="T:System.Windows.Documents.Table" />.</returns>
		// Token: 0x06003606 RID: 13830 RVA: 0x000F4D6B File Offset: 0x000F2F6B
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new TableAutomationPeer(this);
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06003607 RID: 13831 RVA: 0x000F4D73 File Offset: 0x000F2F73
		internal double InternalCellSpacing
		{
			get
			{
				return Math.Max(this.CellSpacing, 0.0);
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06003608 RID: 13832 RVA: 0x000F4D89 File Offset: 0x000F2F89
		// (set) Token: 0x06003609 RID: 13833 RVA: 0x000F4D91 File Offset: 0x000F2F91
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

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x0600360A RID: 13834 RVA: 0x000F4D9A File Offset: 0x000F2F9A
		// (set) Token: 0x0600360B RID: 13835 RVA: 0x000F4DA2 File Offset: 0x000F2FA2
		internal int InsertionIndex
		{
			get
			{
				return this._rowGroupInsertionIndex;
			}
			set
			{
				this._rowGroupInsertionIndex = value;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x0600360C RID: 13836 RVA: 0x000F4DAB File Offset: 0x000F2FAB
		internal int ColumnCount
		{
			get
			{
				return this._columnCount;
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x000F4DB3 File Offset: 0x000F2FB3
		internal void EnsureColumnCount(int columnCount)
		{
			if (this._columnCount < columnCount)
			{
				this._columnCount = columnCount;
			}
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x000F4DC8 File Offset: 0x000F2FC8
		internal void OnStructureChanged()
		{
			if (!this._initializing)
			{
				if (this.TableStructureChanged != null)
				{
					this.TableStructureChanged(this, EventArgs.Empty);
				}
				this.ValidateStructure();
				TableAutomationPeer tableAutomationPeer = ContentElementAutomationPeer.FromElement(this) as TableAutomationPeer;
				if (tableAutomationPeer != null)
				{
					tableAutomationPeer.OnStructureInvalidated();
				}
			}
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x000F4E14 File Offset: 0x000F3014
		internal void ValidateStructure()
		{
			if (!this._initializing)
			{
				this._columnCount = 0;
				for (int i = 0; i < this._rowGroups.Count; i++)
				{
					this._rowGroups[i].ValidateStructure();
				}
				this._version++;
			}
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x000F4E65 File Offset: 0x000F3065
		internal void InvalidateColumns()
		{
			base.NotifyTypographicPropertyChanged(true, true, null);
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x000F4E70 File Offset: 0x000F3070
		internal bool IsFirstNonEmptyRowGroup(int rowGroupIndex)
		{
			for (rowGroupIndex--; rowGroupIndex >= 0; rowGroupIndex--)
			{
				if (this.RowGroups[rowGroupIndex].Rows.Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x000F4E9E File Offset: 0x000F309E
		internal bool IsLastNonEmptyRowGroup(int rowGroupIndex)
		{
			for (rowGroupIndex++; rowGroupIndex < this.RowGroups.Count; rowGroupIndex++)
			{
				if (this.RowGroups[rowGroupIndex].Rows.Count > 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06003613 RID: 13843 RVA: 0x000F4ED8 File Offset: 0x000F30D8
		// (remove) Token: 0x06003614 RID: 13844 RVA: 0x000F4F10 File Offset: 0x000F3110
		internal event EventHandler TableStructureChanged;

		// Token: 0x06003615 RID: 13845 RVA: 0x000F4F45 File Offset: 0x000F3145
		private void PrivateInitialize()
		{
			this._columns = new TableColumnCollection(this);
			this._rowGroups = new TableRowGroupCollection(this);
			this._rowGroupInsertionIndex = -1;
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x000F4F68 File Offset: 0x000F3168
		private static bool IsValidCellSpacing(object o)
		{
			double num = (double)o;
			double num2 = (double)Math.Min(1000000, 3500000);
			return !double.IsNaN(num) && num >= 0.0 && num <= num2;
		}

		// Token: 0x04002533 RID: 9523
		private TableColumnCollection _columns;

		// Token: 0x04002534 RID: 9524
		private TableRowGroupCollection _rowGroups;

		// Token: 0x04002535 RID: 9525
		private int _rowGroupInsertionIndex;

		// Token: 0x04002536 RID: 9526
		private const double c_defaultCellSpacing = 2.0;

		// Token: 0x04002537 RID: 9527
		private int _columnCount;

		// Token: 0x04002538 RID: 9528
		private int _version;

		// Token: 0x04002539 RID: 9529
		private bool _initializing;

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.Table.CellSpacing" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.Table.CellSpacing" /> dependency property.</returns>
		// Token: 0x0400253A RID: 9530
		public static readonly DependencyProperty CellSpacingProperty = DependencyProperty.Register("CellSpacing", typeof(double), typeof(Table), new FrameworkPropertyMetadata(2.0, FrameworkPropertyMetadataOptions.AffectsMeasure), new ValidateValueCallback(Table.IsValidCellSpacing));

		// Token: 0x020008F7 RID: 2295
		private class TableChildrenCollectionEnumeratorSimple : IEnumerator, ICloneable
		{
			// Token: 0x06008598 RID: 34200 RVA: 0x00249DC4 File Offset: 0x00247FC4
			internal TableChildrenCollectionEnumeratorSimple(Table table)
			{
				this._table = table;
				this._version = this._table._version;
				this._columns = ((IEnumerable)this._table._columns).GetEnumerator();
				this._rowGroups = ((IEnumerable)this._table._rowGroups).GetEnumerator();
			}

			// Token: 0x06008599 RID: 34201 RVA: 0x00249E1B File Offset: 0x0024801B
			public object Clone()
			{
				return base.MemberwiseClone();
			}

			// Token: 0x0600859A RID: 34202 RVA: 0x00249E24 File Offset: 0x00248024
			public bool MoveNext()
			{
				if (this._version != this._table._version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				if (this._currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.Columns && this._currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.RowGroups)
				{
					this._currentChildType++;
				}
				object obj = null;
				while (this._currentChildType < Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.AfterLast)
				{
					Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes currentChildType = this._currentChildType;
					if (currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.Columns)
					{
						if (currentChildType == Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.RowGroups)
						{
							if (this._rowGroups.MoveNext())
							{
								obj = this._rowGroups.Current;
							}
						}
					}
					else if (this._columns.MoveNext())
					{
						obj = this._columns.Current;
					}
					if (obj != null)
					{
						this._currentChild = obj;
						break;
					}
					this._currentChildType++;
				}
				return this._currentChildType != Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.AfterLast;
			}

			// Token: 0x17001E2E RID: 7726
			// (get) Token: 0x0600859B RID: 34203 RVA: 0x00249EEC File Offset: 0x002480EC
			public object Current
			{
				get
				{
					if (this._currentChildType == Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.BeforeFirst)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorNotStarted"));
					}
					if (this._currentChildType == Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.AfterLast)
					{
						throw new InvalidOperationException(SR.Get("EnumeratorReachedEnd"));
					}
					return this._currentChild;
				}
			}

			// Token: 0x0600859C RID: 34204 RVA: 0x00249F28 File Offset: 0x00248128
			public void Reset()
			{
				if (this._version != this._table._version)
				{
					throw new InvalidOperationException(SR.Get("EnumeratorVersionChanged"));
				}
				this._columns.Reset();
				this._rowGroups.Reset();
				this._currentChildType = Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes.BeforeFirst;
				this._currentChild = null;
			}

			// Token: 0x040042DC RID: 17116
			private Table _table;

			// Token: 0x040042DD RID: 17117
			private int _version;

			// Token: 0x040042DE RID: 17118
			private IEnumerator _columns;

			// Token: 0x040042DF RID: 17119
			private IEnumerator _rowGroups;

			// Token: 0x040042E0 RID: 17120
			private Table.TableChildrenCollectionEnumeratorSimple.ChildrenTypes _currentChildType;

			// Token: 0x040042E1 RID: 17121
			private object _currentChild;

			// Token: 0x02000BA8 RID: 2984
			private enum ChildrenTypes
			{
				// Token: 0x04004ED0 RID: 20176
				BeforeFirst,
				// Token: 0x04004ED1 RID: 20177
				Columns,
				// Token: 0x04004ED2 RID: 20178
				RowGroups,
				// Token: 0x04004ED3 RID: 20179
				AfterLast
			}
		}
	}
}
