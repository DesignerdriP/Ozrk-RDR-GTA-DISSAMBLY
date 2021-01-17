using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Controls
{
	// Token: 0x02000756 RID: 1878
	internal interface IGeneratorHost
	{
		// Token: 0x17001C5F RID: 7263
		// (get) Token: 0x0600779B RID: 30619
		ItemCollection View { get; }

		// Token: 0x0600779C RID: 30620
		bool IsItemItsOwnContainer(object item);

		// Token: 0x0600779D RID: 30621
		DependencyObject GetContainerForItem(object item);

		// Token: 0x0600779E RID: 30622
		void PrepareItemContainer(DependencyObject container, object item);

		// Token: 0x0600779F RID: 30623
		void ClearContainerForItem(DependencyObject container, object item);

		// Token: 0x060077A0 RID: 30624
		bool IsHostForItemContainer(DependencyObject container);

		// Token: 0x060077A1 RID: 30625
		GroupStyle GetGroupStyle(CollectionViewGroup group, int level);

		// Token: 0x060077A2 RID: 30626
		void SetIsGrouping(bool isGrouping);

		// Token: 0x17001C60 RID: 7264
		// (get) Token: 0x060077A3 RID: 30627
		int AlternationCount { get; }
	}
}
