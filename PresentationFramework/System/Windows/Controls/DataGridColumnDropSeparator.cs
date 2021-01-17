using System;
using System.Windows.Controls.Primitives;
using MS.Internal;

namespace System.Windows.Controls
{
	// Token: 0x020004A1 RID: 1185
	internal class DataGridColumnDropSeparator : Separator
	{
		// Token: 0x0600486D RID: 18541 RVA: 0x001496D4 File Offset: 0x001478D4
		static DataGridColumnDropSeparator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(DataGridColumnHeader.ColumnHeaderDropSeparatorStyleKey));
			FrameworkElement.WidthProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnDropSeparator.OnCoerceWidth)));
			FrameworkElement.HeightProperty.OverrideMetadata(typeof(DataGridColumnDropSeparator), new FrameworkPropertyMetadata(null, new CoerceValueCallback(DataGridColumnDropSeparator.OnCoerceHeight)));
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x0014974C File Offset: 0x0014794C
		private static object OnCoerceWidth(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			if (DoubleUtil.IsNaN(value))
			{
				return 2.0;
			}
			return baseValue;
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x00149778 File Offset: 0x00147978
		private static object OnCoerceHeight(DependencyObject d, object baseValue)
		{
			double value = (double)baseValue;
			DataGridColumnDropSeparator dataGridColumnDropSeparator = (DataGridColumnDropSeparator)d;
			if (dataGridColumnDropSeparator._referenceHeader != null && DoubleUtil.IsNaN(value))
			{
				return dataGridColumnDropSeparator._referenceHeader.ActualHeight;
			}
			return baseValue;
		}

		// Token: 0x170011B1 RID: 4529
		// (get) Token: 0x06004870 RID: 18544 RVA: 0x001497B5 File Offset: 0x001479B5
		// (set) Token: 0x06004871 RID: 18545 RVA: 0x001497BD File Offset: 0x001479BD
		internal DataGridColumnHeader ReferenceHeader
		{
			get
			{
				return this._referenceHeader;
			}
			set
			{
				this._referenceHeader = value;
			}
		}

		// Token: 0x04002997 RID: 10647
		private DataGridColumnHeader _referenceHeader;
	}
}
