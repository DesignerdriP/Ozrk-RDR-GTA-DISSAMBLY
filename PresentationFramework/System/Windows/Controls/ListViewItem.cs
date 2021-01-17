using System;

namespace System.Windows.Controls
{
	/// <summary>Represents an item in a <see cref="T:System.Windows.Controls.ListView" /> control.</summary>
	// Token: 0x020004FE RID: 1278
	public class ListViewItem : ListBoxItem
	{
		// Token: 0x06005172 RID: 20850 RVA: 0x0016D434 File Offset: 0x0016B634
		internal void SetDefaultStyleKey(object key)
		{
			base.DefaultStyleKey = key;
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x0016D43D File Offset: 0x0016B63D
		internal void ClearDefaultStyleKey()
		{
			base.ClearValue(FrameworkElement.DefaultStyleKeyProperty);
		}
	}
}
