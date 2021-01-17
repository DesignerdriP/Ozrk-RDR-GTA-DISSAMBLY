using System;
using System.Collections.Generic;

namespace System.Windows.Controls
{
	// Token: 0x020004AE RID: 1198
	internal class DataGridItemAttachedStorage
	{
		// Token: 0x060048FD RID: 18685 RVA: 0x0014B244 File Offset: 0x00149444
		public void SetValue(object item, DependencyProperty property, object value)
		{
			Dictionary<DependencyProperty, object> dictionary = this.EnsureItem(item);
			dictionary[property] = value;
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x0014B264 File Offset: 0x00149464
		public bool TryGetValue(object item, DependencyProperty property, out object value)
		{
			value = null;
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			return this._itemStorageMap.TryGetValue(item, out dictionary) && dictionary.TryGetValue(property, out value);
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x0014B294 File Offset: 0x00149494
		public void ClearValue(object item, DependencyProperty property)
		{
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			if (this._itemStorageMap.TryGetValue(item, out dictionary))
			{
				dictionary.Remove(property);
			}
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x0014B2BF File Offset: 0x001494BF
		public void ClearItem(object item)
		{
			this.EnsureItemStorageMap();
			this._itemStorageMap.Remove(item);
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x0014B2D4 File Offset: 0x001494D4
		public void Clear()
		{
			this._itemStorageMap = null;
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x0014B2DD File Offset: 0x001494DD
		private void EnsureItemStorageMap()
		{
			if (this._itemStorageMap == null)
			{
				this._itemStorageMap = new Dictionary<object, Dictionary<DependencyProperty, object>>();
			}
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x0014B2F4 File Offset: 0x001494F4
		private Dictionary<DependencyProperty, object> EnsureItem(object item)
		{
			this.EnsureItemStorageMap();
			Dictionary<DependencyProperty, object> dictionary;
			if (!this._itemStorageMap.TryGetValue(item, out dictionary))
			{
				dictionary = new Dictionary<DependencyProperty, object>();
				this._itemStorageMap[item] = dictionary;
			}
			return dictionary;
		}

		// Token: 0x040029BF RID: 10687
		private Dictionary<object, Dictionary<DependencyProperty, object>> _itemStorageMap;
	}
}
