using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005F2 RID: 1522
	internal struct UncommonValueTable
	{
		// Token: 0x06006548 RID: 25928 RVA: 0x001C6AD4 File Offset: 0x001C4CD4
		public bool HasValue(int id)
		{
			return (this._bitmask & 1U << id) > 0U;
		}

		// Token: 0x06006549 RID: 25929 RVA: 0x001C6AE6 File Offset: 0x001C4CE6
		public object GetValue(int id)
		{
			return this.GetValue(id, DependencyProperty.UnsetValue);
		}

		// Token: 0x0600654A RID: 25930 RVA: 0x001C6AF4 File Offset: 0x001C4CF4
		public object GetValue(int id, object defaultValue)
		{
			int num = this.IndexOf(id);
			if (num >= 0)
			{
				return this._table[num];
			}
			return defaultValue;
		}

		// Token: 0x0600654B RID: 25931 RVA: 0x001C6B18 File Offset: 0x001C4D18
		public void SetValue(int id, object value)
		{
			int num = this.Find(id);
			if (num < 0)
			{
				if (this._table == null)
				{
					this._table = new object[1];
					num = 0;
				}
				else
				{
					int num2 = this._table.Length;
					object[] array = new object[num2 + 1];
					num = ~num;
					Array.Copy(this._table, 0, array, 0, num);
					Array.Copy(this._table, num, array, num + 1, num2 - num);
					this._table = array;
				}
				this._bitmask |= 1U << id;
			}
			this._table[num] = value;
		}

		// Token: 0x0600654C RID: 25932 RVA: 0x001C6BA4 File Offset: 0x001C4DA4
		public void ClearValue(int id)
		{
			int num = this.Find(id);
			if (num >= 0)
			{
				int num2 = this._table.Length - 1;
				if (num2 == 0)
				{
					this._table = null;
				}
				else
				{
					object[] array = new object[num2];
					Array.Copy(this._table, 0, array, 0, num);
					Array.Copy(this._table, num + 1, array, num, num2 - num);
					this._table = array;
				}
				this._bitmask &= ~(1U << id);
			}
		}

		// Token: 0x0600654D RID: 25933 RVA: 0x001C6C18 File Offset: 0x001C4E18
		private int IndexOf(int id)
		{
			if (!this.HasValue(id))
			{
				return -1;
			}
			return this.GetIndex(id);
		}

		// Token: 0x0600654E RID: 25934 RVA: 0x001C6C2C File Offset: 0x001C4E2C
		private int Find(int id)
		{
			int num = this.GetIndex(id);
			if (!this.HasValue(id))
			{
				num = ~num;
			}
			return num;
		}

		// Token: 0x0600654F RID: 25935 RVA: 0x001C6C50 File Offset: 0x001C4E50
		private int GetIndex(int id)
		{
			uint num = this._bitmask << 31 - id << 1;
			num -= (num >> 1 & 1431655765U);
			num = (num & 858993459U) + (num >> 2 & 858993459U);
			num = (num + (num >> 4) & 252645135U);
			return (int)(num * 16843009U >> 24);
		}

		// Token: 0x040032BC RID: 12988
		private object[] _table;

		// Token: 0x040032BD RID: 12989
		private uint _bitmask;
	}
}
