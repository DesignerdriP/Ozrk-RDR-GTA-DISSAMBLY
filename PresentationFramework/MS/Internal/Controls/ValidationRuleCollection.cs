using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000765 RID: 1893
	internal class ValidationRuleCollection : Collection<ValidationRule>
	{
		// Token: 0x0600784E RID: 30798 RVA: 0x00224323 File Offset: 0x00222523
		protected override void InsertItem(int index, ValidationRule item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x0600784F RID: 30799 RVA: 0x0022433B File Offset: 0x0022253B
		protected override void SetItem(int index, ValidationRule item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}
	}
}
