using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace MS.Internal.Data
{
	// Token: 0x0200074D RID: 1869
	internal sealed class XDeferredAxisSource
	{
		// Token: 0x06007722 RID: 30498 RVA: 0x00220AAC File Offset: 0x0021ECAC
		internal XDeferredAxisSource(object component, PropertyDescriptor pd)
		{
			this._component = new WeakReference(component);
			this._propertyDescriptor = pd;
			this._table = new HybridDictionary();
		}

		// Token: 0x17001C4E RID: 7246
		public IEnumerable this[string name]
		{
			get
			{
				XDeferredAxisSource.Record record = (XDeferredAxisSource.Record)this._table[name];
				if (record == null)
				{
					object target = this._component.Target;
					if (target == null)
					{
						return null;
					}
					IEnumerable enumerable = this._propertyDescriptor.GetValue(target) as IEnumerable;
					if (enumerable != null && name != "%%FullCollection%%")
					{
						MemberInfo[] defaultMembers = enumerable.GetType().GetDefaultMembers();
						PropertyInfo propertyInfo = (defaultMembers.Length != 0) ? (defaultMembers[0] as PropertyInfo) : null;
						enumerable = ((propertyInfo == null) ? null : (propertyInfo.GetValue(enumerable, BindingFlags.GetProperty, null, new object[]
						{
							name
						}, CultureInfo.InvariantCulture) as IEnumerable));
					}
					record = new XDeferredAxisSource.Record(enumerable);
					this._table[name] = record;
				}
				else
				{
					record.DC.Update(record.XDA);
				}
				return record.Collection;
			}
		}

		// Token: 0x17001C4F RID: 7247
		// (get) Token: 0x06007724 RID: 30500 RVA: 0x00220BA6 File Offset: 0x0021EDA6
		internal IEnumerable FullCollection
		{
			get
			{
				return this["%%FullCollection%%"];
			}
		}

		// Token: 0x040038AC RID: 14508
		private WeakReference _component;

		// Token: 0x040038AD RID: 14509
		private PropertyDescriptor _propertyDescriptor;

		// Token: 0x040038AE RID: 14510
		private HybridDictionary _table;

		// Token: 0x040038AF RID: 14511
		private const string FullCollectionKey = "%%FullCollection%%";

		// Token: 0x02000B66 RID: 2918
		private class Record
		{
			// Token: 0x06008E09 RID: 36361 RVA: 0x0025B0C9 File Offset: 0x002592C9
			public Record(IEnumerable xda)
			{
				this._xda = xda;
				if (xda != null)
				{
					this._dc = new DifferencingCollection(xda);
					this._rooc = new ReadOnlyObservableCollection<object>(this._dc);
				}
			}

			// Token: 0x17001F95 RID: 8085
			// (get) Token: 0x06008E0A RID: 36362 RVA: 0x0025B0F8 File Offset: 0x002592F8
			public IEnumerable XDA
			{
				get
				{
					return this._xda;
				}
			}

			// Token: 0x17001F96 RID: 8086
			// (get) Token: 0x06008E0B RID: 36363 RVA: 0x0025B100 File Offset: 0x00259300
			public DifferencingCollection DC
			{
				get
				{
					return this._dc;
				}
			}

			// Token: 0x17001F97 RID: 8087
			// (get) Token: 0x06008E0C RID: 36364 RVA: 0x0025B108 File Offset: 0x00259308
			public ReadOnlyObservableCollection<object> Collection
			{
				get
				{
					return this._rooc;
				}
			}

			// Token: 0x04004B34 RID: 19252
			private IEnumerable _xda;

			// Token: 0x04004B35 RID: 19253
			private DifferencingCollection _dc;

			// Token: 0x04004B36 RID: 19254
			private ReadOnlyObservableCollection<object> _rooc;
		}
	}
}
