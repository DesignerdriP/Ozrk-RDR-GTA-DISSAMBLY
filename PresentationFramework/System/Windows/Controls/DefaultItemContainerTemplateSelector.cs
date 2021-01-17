using System;

namespace System.Windows.Controls
{
	// Token: 0x020004F4 RID: 1268
	internal class DefaultItemContainerTemplateSelector : ItemContainerTemplateSelector
	{
		// Token: 0x06005039 RID: 20537 RVA: 0x0016845E File Offset: 0x0016665E
		public override DataTemplate SelectTemplate(object item, ItemsControl parentItemsControl)
		{
			return FrameworkElement.FindTemplateResourceInternal(parentItemsControl, item, typeof(ItemContainerTemplate)) as DataTemplate;
		}
	}
}
