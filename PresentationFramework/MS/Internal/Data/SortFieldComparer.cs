using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000742 RID: 1858
	internal class SortFieldComparer : IComparer
	{
		// Token: 0x060076CC RID: 30412 RVA: 0x0021F430 File Offset: 0x0021D630
		internal SortFieldComparer(SortDescriptionCollection sortFields, CultureInfo culture)
		{
			this._sortFields = sortFields;
			this._fields = this.CreatePropertyInfo(this._sortFields);
			this._comparer = ((culture == null || culture == CultureInfo.InvariantCulture) ? Comparer.DefaultInvariant : ((culture == CultureInfo.CurrentCulture) ? Comparer.Default : new Comparer(culture)));
		}

		// Token: 0x17001C3F RID: 7231
		// (get) Token: 0x060076CD RID: 30413 RVA: 0x0021F489 File Offset: 0x0021D689
		internal IComparer BaseComparer
		{
			get
			{
				return this._comparer;
			}
		}

		// Token: 0x060076CE RID: 30414 RVA: 0x0021F494 File Offset: 0x0021D694
		public int Compare(object o1, object o2)
		{
			int num = 0;
			for (int i = 0; i < this._fields.Length; i++)
			{
				object value = this._fields[i].GetValue(o1);
				object value2 = this._fields[i].GetValue(o2);
				num = this._comparer.Compare(value, value2);
				if (this._fields[i].descending)
				{
					num = -num;
				}
				if (num != 0)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x060076CF RID: 30415 RVA: 0x0021F504 File Offset: 0x0021D704
		internal static void SortHelper(ArrayList al, IComparer comparer)
		{
			SortFieldComparer sortFieldComparer = comparer as SortFieldComparer;
			if (sortFieldComparer == null)
			{
				al.Sort(comparer);
				return;
			}
			int count = al.Count;
			int nFields = sortFieldComparer._fields.Length;
			SortFieldComparer.CachedValueItem[] array = new SortFieldComparer.CachedValueItem[count];
			for (int i = 0; i < count; i++)
			{
				array[i].Initialize(al[i], nFields);
			}
			Array.Sort(array, sortFieldComparer);
			for (int j = 0; j < count; j++)
			{
				al[j] = array[j].OriginalItem;
			}
		}

		// Token: 0x060076D0 RID: 30416 RVA: 0x0021F58C File Offset: 0x0021D78C
		private SortFieldComparer.SortPropertyInfo[] CreatePropertyInfo(SortDescriptionCollection sortFields)
		{
			SortFieldComparer.SortPropertyInfo[] array = new SortFieldComparer.SortPropertyInfo[sortFields.Count];
			for (int i = 0; i < sortFields.Count; i++)
			{
				PropertyPath info;
				if (string.IsNullOrEmpty(sortFields[i].PropertyName))
				{
					info = null;
				}
				else
				{
					info = new PropertyPath(sortFields[i].PropertyName, new object[0]);
				}
				array[i].index = i;
				array[i].info = info;
				array[i].descending = (sortFields[i].Direction == ListSortDirection.Descending);
			}
			return array;
		}

		// Token: 0x04003893 RID: 14483
		private SortFieldComparer.SortPropertyInfo[] _fields;

		// Token: 0x04003894 RID: 14484
		private SortDescriptionCollection _sortFields;

		// Token: 0x04003895 RID: 14485
		private Comparer _comparer;

		// Token: 0x02000B5E RID: 2910
		private struct SortPropertyInfo
		{
			// Token: 0x06008DD6 RID: 36310 RVA: 0x0025A34F File Offset: 0x0025854F
			internal object GetValue(object o)
			{
				if (o is SortFieldComparer.CachedValueItem)
				{
					return this.GetValueFromCVI((SortFieldComparer.CachedValueItem)o);
				}
				return this.GetValueCore(o);
			}

			// Token: 0x06008DD7 RID: 36311 RVA: 0x0025A370 File Offset: 0x00258570
			private object GetValueFromCVI(SortFieldComparer.CachedValueItem cvi)
			{
				object obj = cvi[this.index];
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = (cvi[this.index] = this.GetValueCore(cvi.OriginalItem));
				}
				return obj;
			}

			// Token: 0x06008DD8 RID: 36312 RVA: 0x0025A3B4 File Offset: 0x002585B4
			private object GetValueCore(object o)
			{
				object obj;
				if (this.info == null)
				{
					obj = o;
				}
				else
				{
					using (this.info.SetContext(o))
					{
						obj = this.info.GetValue();
					}
				}
				if (obj == DependencyProperty.UnsetValue || BindingExpressionBase.IsNullValue(obj))
				{
					obj = null;
				}
				return obj;
			}

			// Token: 0x04004B17 RID: 19223
			internal int index;

			// Token: 0x04004B18 RID: 19224
			internal PropertyPath info;

			// Token: 0x04004B19 RID: 19225
			internal bool descending;
		}

		// Token: 0x02000B5F RID: 2911
		private struct CachedValueItem
		{
			// Token: 0x17001F87 RID: 8071
			// (get) Token: 0x06008DD9 RID: 36313 RVA: 0x0025A418 File Offset: 0x00258618
			public object OriginalItem
			{
				get
				{
					return this._item;
				}
			}

			// Token: 0x06008DDA RID: 36314 RVA: 0x0025A420 File Offset: 0x00258620
			public void Initialize(object item, int nFields)
			{
				this._item = item;
				this._values = new object[nFields];
				this._values[0] = DependencyProperty.UnsetValue;
			}

			// Token: 0x17001F88 RID: 8072
			public object this[int index]
			{
				get
				{
					return this._values[index];
				}
				set
				{
					this._values[index] = value;
					if (++index < this._values.Length)
					{
						this._values[index] = DependencyProperty.UnsetValue;
					}
				}
			}

			// Token: 0x04004B1A RID: 19226
			private object _item;

			// Token: 0x04004B1B RID: 19227
			private object[] _values;
		}
	}
}
