using System;

namespace System.Windows.Controls
{
	/// <summary>Provides a resource key for an <see cref="T:System.Windows.Controls.ItemContainerTemplate" /> object.</summary>
	// Token: 0x020004F2 RID: 1266
	public class ItemContainerTemplateKey : TemplateKey
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ItemContainerTemplateKey" /> class.</summary>
		// Token: 0x06005035 RID: 20533 RVA: 0x0016844B File Offset: 0x0016664B
		public ItemContainerTemplateKey() : base(TemplateKey.TemplateType.TableTemplate)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ItemContainerTemplateKey" /> class with the specified data type.</summary>
		/// <param name="dataType">The type for which this template is designed.</param>
		// Token: 0x06005036 RID: 20534 RVA: 0x00168454 File Offset: 0x00166654
		public ItemContainerTemplateKey(object dataType) : base(TemplateKey.TemplateType.TableTemplate, dataType)
		{
		}
	}
}
