using System;
using System.Collections.Specialized;

namespace System.Windows.Controls
{
	// Token: 0x0200052B RID: 1323
	internal sealed class SelectedCellsCollection : VirtualizedCellInfoCollection
	{
		// Token: 0x060055C2 RID: 21954 RVA: 0x0017C1BD File Offset: 0x0017A3BD
		internal SelectedCellsCollection(DataGrid owner) : base(owner)
		{
		}

		// Token: 0x060055C3 RID: 21955 RVA: 0x0017C1C6 File Offset: 0x0017A3C6
		internal bool GetSelectionRange(out int minColumnDisplayIndex, out int maxColumnDisplayIndex, out int minRowIndex, out int maxRowIndex)
		{
			if (base.IsEmpty)
			{
				minColumnDisplayIndex = -1;
				maxColumnDisplayIndex = -1;
				minRowIndex = -1;
				maxRowIndex = -1;
				return false;
			}
			base.GetBoundingRegion(out minColumnDisplayIndex, out minRowIndex, out maxColumnDisplayIndex, out maxRowIndex);
			return true;
		}

		// Token: 0x060055C4 RID: 21956 RVA: 0x0017C1EB File Offset: 0x0017A3EB
		protected override void OnCollectionChanged(NotifyCollectionChangedAction action, VirtualizedCellInfoCollection oldItems, VirtualizedCellInfoCollection newItems)
		{
			base.Owner.OnSelectedCellsChanged(action, oldItems, newItems);
		}
	}
}
