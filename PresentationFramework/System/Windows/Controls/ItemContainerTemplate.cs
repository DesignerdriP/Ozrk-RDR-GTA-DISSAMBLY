using System;
using System.Windows.Markup;

namespace System.Windows.Controls
{
	/// <summary>Provides the template for producing a container for an <see cref="T:System.Windows.Controls.ItemsControl" /> object. </summary>
	// Token: 0x020004F1 RID: 1265
	[DictionaryKeyProperty("ItemContainerTemplateKey")]
	public class ItemContainerTemplate : DataTemplate
	{
		/// <summary>Gets the default key of the <see cref="T:System.Windows.Controls.ItemContainerTemplate" />. </summary>
		/// <returns>The default key of the <see cref="T:System.Windows.Controls.ItemContainerTemplate" />.</returns>
		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x06005033 RID: 20531 RVA: 0x00168434 File Offset: 0x00166634
		public object ItemContainerTemplateKey
		{
			get
			{
				if (base.DataType == null)
				{
					return null;
				}
				return new ItemContainerTemplateKey(base.DataType);
			}
		}
	}
}
