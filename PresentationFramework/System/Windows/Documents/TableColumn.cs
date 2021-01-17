using System;
using System.Windows.Controls;
using System.Windows.Media;
using MS.Internal.Documents;

namespace System.Windows.Documents
{
	/// <summary>A flow content element that defines a column within a <see cref="T:System.Windows.Documents.Table" />.</summary>
	// Token: 0x020003E5 RID: 997
	public class TableColumn : FrameworkContentElement, IIndexedChild<Table>
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.TableColumn" /> class. </summary>
		// Token: 0x06003670 RID: 13936 RVA: 0x000F5735 File Offset: 0x000F3935
		public TableColumn()
		{
			this._parentIndex = -1;
		}

		/// <summary>Gets or sets the width of a <see cref="T:System.Windows.Documents.TableColumn" /> element. The <see cref="P:System.Windows.Documents.TableColumn.Width" /> property measures the sum of the <see cref="T:System.Windows.Documents.TableColumn" /> content, padding, and border from side to side.  </summary>
		/// <returns>The width of the <see cref="T:System.Windows.Documents.TableColumn" /> element, as a <see cref="T:System.Windows.GridLength" />.</returns>
		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003671 RID: 13937 RVA: 0x000F5744 File Offset: 0x000F3944
		// (set) Token: 0x06003672 RID: 13938 RVA: 0x000F5756 File Offset: 0x000F3956
		public GridLength Width
		{
			get
			{
				return (GridLength)base.GetValue(TableColumn.WidthProperty);
			}
			set
			{
				base.SetValue(TableColumn.WidthProperty, value);
			}
		}

		/// <summary>Gets or sets the background <see cref="T:System.Windows.Media.Brush" /> used to fill the content of the <see cref="T:System.Windows.Documents.TableColumn" />.  </summary>
		/// <returns>The background <see cref="T:System.Windows.Media.Brush" /> used to fill the content of the <see cref="T:System.Windows.Documents.TableColumn" />.</returns>
		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06003673 RID: 13939 RVA: 0x000F5769 File Offset: 0x000F3969
		// (set) Token: 0x06003674 RID: 13940 RVA: 0x000F577B File Offset: 0x000F397B
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(TableColumn.BackgroundProperty);
			}
			set
			{
				base.SetValue(TableColumn.BackgroundProperty, value);
			}
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000F5789 File Offset: 0x000F3989
		void IIndexedChild<Table>.OnEnterParentTree()
		{
			this.OnEnterParentTree();
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x000F5791 File Offset: 0x000F3991
		void IIndexedChild<Table>.OnExitParentTree()
		{
			this.OnExitParentTree();
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x00002137 File Offset: 0x00000337
		void IIndexedChild<Table>.OnAfterExitParentTree(Table parent)
		{
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06003678 RID: 13944 RVA: 0x000F5799 File Offset: 0x000F3999
		// (set) Token: 0x06003679 RID: 13945 RVA: 0x000F57A1 File Offset: 0x000F39A1
		int IIndexedChild<Table>.Index
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

		// Token: 0x0600367A RID: 13946 RVA: 0x000F57AA File Offset: 0x000F39AA
		internal void OnEnterParentTree()
		{
			this.Table.InvalidateColumns();
		}

		// Token: 0x0600367B RID: 13947 RVA: 0x000F57AA File Offset: 0x000F39AA
		internal void OnExitParentTree()
		{
			this.Table.InvalidateColumns();
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x0600367C RID: 13948 RVA: 0x000F57B7 File Offset: 0x000F39B7
		internal Table Table
		{
			get
			{
				return base.Parent as Table;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x0600367D RID: 13949 RVA: 0x000F57C4 File Offset: 0x000F39C4
		// (set) Token: 0x0600367E RID: 13950 RVA: 0x000F57CC File Offset: 0x000F39CC
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

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x0600367F RID: 13951 RVA: 0x000F57D5 File Offset: 0x000F39D5
		internal static GridLength DefaultWidth
		{
			get
			{
				return new GridLength(0.0, GridUnitType.Auto);
			}
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x000F57E8 File Offset: 0x000F39E8
		private static bool IsValidWidth(object value)
		{
			GridLength gridLength = (GridLength)value;
			if ((gridLength.GridUnitType == GridUnitType.Pixel || gridLength.GridUnitType == GridUnitType.Star) && gridLength.Value < 0.0)
			{
				return false;
			}
			double num = (double)Math.Min(1000000, 3500000);
			return gridLength.GridUnitType != GridUnitType.Pixel || gridLength.Value <= num;
		}

		// Token: 0x06003681 RID: 13953 RVA: 0x000F584C File Offset: 0x000F3A4C
		private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Table table = ((TableColumn)d).Table;
			if (table != null)
			{
				table.InvalidateColumns();
			}
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x000F5870 File Offset: 0x000F3A70
		private static void OnBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Table table = ((TableColumn)d).Table;
			if (table != null)
			{
				table.InvalidateColumns();
			}
		}

		// Token: 0x04002547 RID: 9543
		private int _parentIndex;

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableColumn.Width" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableColumn.Width" /> dependency property.</returns>
		// Token: 0x04002548 RID: 9544
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(TableColumn), new FrameworkPropertyMetadata(new GridLength(0.0, GridUnitType.Auto), FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(TableColumn.OnWidthChanged)), new ValidateValueCallback(TableColumn.IsValidWidth));

		/// <summary>Identifies the <see cref="P:System.Windows.Documents.TableColumn.Background" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Documents.TableColumn.Background" /> dependency property.</returns>
		// Token: 0x04002549 RID: 9545
		public static readonly DependencyProperty BackgroundProperty = Panel.BackgroundProperty.AddOwner(typeof(TableColumn), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(TableColumn.OnBackgroundChanged)));
	}
}
