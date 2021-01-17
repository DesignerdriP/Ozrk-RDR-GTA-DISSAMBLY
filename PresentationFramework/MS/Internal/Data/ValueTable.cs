using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x0200074C RID: 1868
	internal sealed class ValueTable : IWeakEventListener
	{
		// Token: 0x06007716 RID: 30486 RVA: 0x002206C7 File Offset: 0x0021E8C7
		internal static bool ShouldCache(object item, PropertyDescriptor pd)
		{
			return SystemDataHelper.IsDataSetCollectionProperty(pd) || SystemXmlLinqHelper.IsXLinqCollectionProperty(pd);
		}

		// Token: 0x06007717 RID: 30487 RVA: 0x002206E0 File Offset: 0x0021E8E0
		internal object GetValue(object item, PropertyDescriptor pd, bool indexerIsNext)
		{
			ValueTable.<>c__DisplayClass1_1 CS$<>8__locals1 = new ValueTable.<>c__DisplayClass1_1();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pd = pd;
			CS$<>8__locals1.item = item;
			if (!ValueTable.ShouldCache(CS$<>8__locals1.item, CS$<>8__locals1.pd))
			{
				return CS$<>8__locals1.pd.GetValue(CS$<>8__locals1.item);
			}
			if (this._table == null)
			{
				this._table = new HybridDictionary();
			}
			bool isXLinqCollectionProperty = SystemXmlLinqHelper.IsXLinqCollectionProperty(CS$<>8__locals1.pd);
			ValueTable.ValueTableKey key = new ValueTable.ValueTableKey(CS$<>8__locals1.item, CS$<>8__locals1.pd);
			object value = this._table[key];
			Action action = delegate()
			{
				if (value == null)
				{
					if (SystemDataHelper.IsDataSetCollectionProperty(CS$<>8__locals1.pd))
					{
						value = SystemDataHelper.GetValue(CS$<>8__locals1.item, CS$<>8__locals1.pd, !FrameworkAppContextSwitches.DoNotUseFollowParentWhenBindingToADODataRelation);
					}
					else if (isXLinqCollectionProperty)
					{
						value = new XDeferredAxisSource(CS$<>8__locals1.item, CS$<>8__locals1.pd);
					}
					else
					{
						value = CS$<>8__locals1.pd.GetValue(CS$<>8__locals1.item);
					}
					if (value == null)
					{
						value = ValueTable.CachedNull;
					}
					if (SystemDataHelper.IsDataSetCollectionProperty(CS$<>8__locals1.pd))
					{
						value = new WeakReference(value);
					}
					CS$<>8__locals1.<>4__this._table[key] = value;
				}
				if (SystemDataHelper.IsDataSetCollectionProperty(CS$<>8__locals1.pd))
				{
					WeakReference weakReference = value as WeakReference;
					if (weakReference != null)
					{
						value = weakReference.Target;
					}
				}
			};
			action();
			if (value == null)
			{
				action();
			}
			if (value == ValueTable.CachedNull)
			{
				value = null;
			}
			else if (isXLinqCollectionProperty && !indexerIsNext)
			{
				XDeferredAxisSource xdeferredAxisSource = (XDeferredAxisSource)value;
				value = xdeferredAxisSource.FullCollection;
			}
			return value;
		}

		// Token: 0x06007718 RID: 30488 RVA: 0x002207FC File Offset: 0x0021E9FC
		internal void RegisterForChanges(object item, PropertyDescriptor pd, DataBindEngine engine)
		{
			if (this._table == null)
			{
				this._table = new HybridDictionary();
			}
			ValueTable.ValueTableKey key = new ValueTable.ValueTableKey(item, pd);
			if (this._table[key] == null)
			{
				INotifyPropertyChanged notifyPropertyChanged = item as INotifyPropertyChanged;
				if (notifyPropertyChanged != null)
				{
					PropertyChangedEventManager.AddHandler(notifyPropertyChanged, new EventHandler<PropertyChangedEventArgs>(this.OnPropertyChanged), pd.Name);
					return;
				}
				ValueChangedEventManager.AddHandler(item, new EventHandler<ValueChangedEventArgs>(this.OnValueChanged), pd);
			}
		}

		// Token: 0x06007719 RID: 30489 RVA: 0x0022086C File Offset: 0x0021EA6C
		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			string text = e.PropertyName;
			if (text == null)
			{
				text = string.Empty;
			}
			this.InvalidateCache(sender, text);
		}

		// Token: 0x0600771A RID: 30490 RVA: 0x00220891 File Offset: 0x0021EA91
		private void OnValueChanged(object sender, ValueChangedEventArgs e)
		{
			this.InvalidateCache(sender, e.PropertyDescriptor);
		}

		// Token: 0x0600771B RID: 30491 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x0600771C RID: 30492 RVA: 0x002208A0 File Offset: 0x0021EAA0
		private void InvalidateCache(object item, string name)
		{
			if (name == string.Empty)
			{
				foreach (PropertyDescriptor pd in this.GetPropertiesForItem(item))
				{
					this.InvalidateCache(item, pd);
				}
				return;
			}
			PropertyDescriptor pd2;
			if (item is ICustomTypeDescriptor)
			{
				pd2 = TypeDescriptor.GetProperties(item)[name];
			}
			else
			{
				pd2 = TypeDescriptor.GetProperties(item.GetType())[name];
			}
			this.InvalidateCache(item, pd2);
		}

		// Token: 0x0600771D RID: 30493 RVA: 0x00220930 File Offset: 0x0021EB30
		private void InvalidateCache(object item, PropertyDescriptor pd)
		{
			if (SystemXmlLinqHelper.IsXLinqCollectionProperty(pd))
			{
				return;
			}
			ValueTable.ValueTableKey key = new ValueTable.ValueTableKey(item, pd);
			this._table.Remove(key);
		}

		// Token: 0x0600771E RID: 30494 RVA: 0x0022095C File Offset: 0x0021EB5C
		private IEnumerable<PropertyDescriptor> GetPropertiesForItem(object item)
		{
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			foreach (object obj in this._table)
			{
				ValueTable.ValueTableKey valueTableKey = (ValueTable.ValueTableKey)((DictionaryEntry)obj).Key;
				if (object.Equals(item, valueTableKey.Item))
				{
					list.Add(valueTableKey.PropertyDescriptor);
				}
			}
			return list;
		}

		// Token: 0x0600771F RID: 30495 RVA: 0x002209E0 File Offset: 0x0021EBE0
		internal bool Purge()
		{
			if (this._table == null)
			{
				return false;
			}
			bool flag = false;
			ICollection keys = this._table.Keys;
			foreach (object obj in keys)
			{
				ValueTable.ValueTableKey valueTableKey = (ValueTable.ValueTableKey)obj;
				if (valueTableKey.IsStale)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				ValueTable.ValueTableKey[] array = new ValueTable.ValueTableKey[keys.Count];
				keys.CopyTo(array, 0);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					if (array[i].IsStale)
					{
						this._table.Remove(array[i]);
					}
				}
			}
			return flag;
		}

		// Token: 0x040038AA RID: 14506
		private HybridDictionary _table;

		// Token: 0x040038AB RID: 14507
		private static object CachedNull = new object();

		// Token: 0x02000B63 RID: 2915
		private class ValueTableKey
		{
			// Token: 0x06008E00 RID: 36352 RVA: 0x0025AEB8 File Offset: 0x002590B8
			public ValueTableKey(object item, PropertyDescriptor pd)
			{
				Invariant.Assert(item != null && pd != null);
				this._item = new WeakReference(item);
				this._descriptor = new WeakReference(pd);
				this._hashCode = item.GetHashCode() + pd.GetHashCode();
			}

			// Token: 0x17001F92 RID: 8082
			// (get) Token: 0x06008E01 RID: 36353 RVA: 0x0025AF05 File Offset: 0x00259105
			public object Item
			{
				get
				{
					return this._item.Target;
				}
			}

			// Token: 0x17001F93 RID: 8083
			// (get) Token: 0x06008E02 RID: 36354 RVA: 0x0025AF12 File Offset: 0x00259112
			public PropertyDescriptor PropertyDescriptor
			{
				get
				{
					return (PropertyDescriptor)this._descriptor.Target;
				}
			}

			// Token: 0x17001F94 RID: 8084
			// (get) Token: 0x06008E03 RID: 36355 RVA: 0x0025AF24 File Offset: 0x00259124
			public bool IsStale
			{
				get
				{
					return this.Item == null || this.PropertyDescriptor == null;
				}
			}

			// Token: 0x06008E04 RID: 36356 RVA: 0x0025AF3C File Offset: 0x0025913C
			public override bool Equals(object o)
			{
				if (o == this)
				{
					return true;
				}
				ValueTable.ValueTableKey valueTableKey = o as ValueTable.ValueTableKey;
				if (valueTableKey != null)
				{
					object item = this.Item;
					PropertyDescriptor propertyDescriptor = this.PropertyDescriptor;
					return item != null && propertyDescriptor != null && (this._hashCode == valueTableKey._hashCode && object.Equals(item, valueTableKey.Item)) && object.Equals(propertyDescriptor, valueTableKey.PropertyDescriptor);
				}
				return false;
			}

			// Token: 0x06008E05 RID: 36357 RVA: 0x0025AF9B File Offset: 0x0025919B
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x04004B2A RID: 19242
			private WeakReference _item;

			// Token: 0x04004B2B RID: 19243
			private WeakReference _descriptor;

			// Token: 0x04004B2C RID: 19244
			private int _hashCode;
		}
	}
}
