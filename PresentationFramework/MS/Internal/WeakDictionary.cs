using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x020005F3 RID: 1523
	internal class WeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable where TKey : class
	{
		// Token: 0x06006550 RID: 25936 RVA: 0x001C6CA2 File Offset: 0x001C4EA2
		public void Add(TKey key, TValue value)
		{
			this._hashTable.SetWeak(key, value);
		}

		// Token: 0x06006551 RID: 25937 RVA: 0x001C6CBB File Offset: 0x001C4EBB
		public bool ContainsKey(TKey key)
		{
			return this._hashTable.ContainsKey(key);
		}

		// Token: 0x17001847 RID: 6215
		// (get) Token: 0x06006552 RID: 25938 RVA: 0x001C6CCE File Offset: 0x001C4ECE
		public ICollection<TKey> Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new WeakDictionary<TKey, TValue>.KeyCollection<TKey, TValue>(this);
				}
				return this._keys;
			}
		}

		// Token: 0x06006553 RID: 25939 RVA: 0x001C6CEA File Offset: 0x001C4EEA
		public bool Remove(TKey key)
		{
			if (this._hashTable.ContainsKey(key))
			{
				this._hashTable.Remove(key);
				return true;
			}
			return false;
		}

		// Token: 0x06006554 RID: 25940 RVA: 0x001C6D13 File Offset: 0x001C4F13
		public bool TryGetValue(TKey key, out TValue value)
		{
			if (this._hashTable.ContainsKey(key))
			{
				value = (TValue)((object)this._hashTable[key]);
				return true;
			}
			value = default(TValue);
			return false;
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x06006555 RID: 25941 RVA: 0x001C6D4E File Offset: 0x001C4F4E
		public ICollection<TValue> Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new WeakDictionary<TKey, TValue>.ValueCollection<TKey, TValue>(this);
				}
				return this._values;
			}
		}

		// Token: 0x17001849 RID: 6217
		public TValue this[TKey key]
		{
			get
			{
				if (!this._hashTable.ContainsKey(key))
				{
					throw new KeyNotFoundException();
				}
				return (TValue)((object)this._hashTable[key]);
			}
			set
			{
				this._hashTable.SetWeak(key, value);
			}
		}

		// Token: 0x06006558 RID: 25944 RVA: 0x001C6D9B File Offset: 0x001C4F9B
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06006559 RID: 25945 RVA: 0x001C6DB1 File Offset: 0x001C4FB1
		public void Clear()
		{
			this._hashTable.Clear();
		}

		// Token: 0x0600655A RID: 25946 RVA: 0x001C6DC0 File Offset: 0x001C4FC0
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this._hashTable.ContainsKey(item.Key) && object.Equals(this._hashTable[item.Key], item.Value);
		}

		// Token: 0x0600655B RID: 25947 RVA: 0x001C6E18 File Offset: 0x001C5018
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int num = 0;
			foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
			{
				num++;
			}
			if (num + arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in this)
			{
				array[arrayIndex++] = keyValuePair2;
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x0600655C RID: 25948 RVA: 0x001C6ED0 File Offset: 0x001C50D0
		public int Count
		{
			get
			{
				return this._hashTable.Count;
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x0600655D RID: 25949 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600655E RID: 25950 RVA: 0x001C6EDD File Offset: 0x001C50DD
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			return this.Contains(item) && this.Remove(item.Key);
		}

		// Token: 0x0600655F RID: 25951 RVA: 0x001C6EF7 File Offset: 0x001C50F7
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			foreach (object key in this._hashTable.Keys)
			{
				TKey tkey = this._hashTable.UnwrapKey(key) as TKey;
				if (tkey != null)
				{
					yield return new KeyValuePair<TKey, TValue>(tkey, (TValue)((object)this._hashTable[key]));
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006560 RID: 25952 RVA: 0x001C6F06 File Offset: 0x001C5106
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040032BE RID: 12990
		private IWeakHashtable _hashTable = WeakHashtable.FromKeyType(typeof(TKey));

		// Token: 0x040032BF RID: 12991
		private WeakDictionary<TKey, TValue>.KeyCollection<TKey, TValue> _keys;

		// Token: 0x040032C0 RID: 12992
		private WeakDictionary<TKey, TValue>.ValueCollection<TKey, TValue> _values;

		// Token: 0x02000A09 RID: 2569
		private class KeyCollection<KeyType, ValueType> : ICollection<KeyType>, IEnumerable<KeyType>, IEnumerable where KeyType : class
		{
			// Token: 0x06008A06 RID: 35334 RVA: 0x00256881 File Offset: 0x00254A81
			public KeyCollection(WeakDictionary<KeyType, ValueType> dict)
			{
				this.Dict = dict;
			}

			// Token: 0x17001F29 RID: 7977
			// (get) Token: 0x06008A07 RID: 35335 RVA: 0x00256890 File Offset: 0x00254A90
			// (set) Token: 0x06008A08 RID: 35336 RVA: 0x00256898 File Offset: 0x00254A98
			public WeakDictionary<KeyType, ValueType> Dict { get; private set; }

			// Token: 0x06008A09 RID: 35337 RVA: 0x0003E264 File Offset: 0x0003C464
			public void Add(KeyType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A0A RID: 35338 RVA: 0x0003E264 File Offset: 0x0003C464
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A0B RID: 35339 RVA: 0x002568A1 File Offset: 0x00254AA1
			public bool Contains(KeyType item)
			{
				return this.Dict.ContainsKey(item);
			}

			// Token: 0x06008A0C RID: 35340 RVA: 0x0003E264 File Offset: 0x0003C464
			public void CopyTo(KeyType[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001F2A RID: 7978
			// (get) Token: 0x06008A0D RID: 35341 RVA: 0x002568AF File Offset: 0x00254AAF
			public int Count
			{
				get
				{
					return this.Dict.Count;
				}
			}

			// Token: 0x17001F2B RID: 7979
			// (get) Token: 0x06008A0E RID: 35342 RVA: 0x00016748 File Offset: 0x00014948
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06008A0F RID: 35343 RVA: 0x0003E264 File Offset: 0x0003C464
			public bool Remove(KeyType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A10 RID: 35344 RVA: 0x002568BC File Offset: 0x00254ABC
			public IEnumerator<KeyType> GetEnumerator()
			{
				IWeakHashtable hashTable = this.Dict._hashTable;
				foreach (object key in hashTable.Keys)
				{
					KeyType keyType = hashTable.UnwrapKey(key) as KeyType;
					if (keyType != null)
					{
						yield return keyType;
					}
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x06008A11 RID: 35345 RVA: 0x002568CB File Offset: 0x00254ACB
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}

		// Token: 0x02000A0A RID: 2570
		private class ValueCollection<KeyType, ValueType> : ICollection<ValueType>, IEnumerable<ValueType>, IEnumerable where KeyType : class
		{
			// Token: 0x06008A12 RID: 35346 RVA: 0x002568D3 File Offset: 0x00254AD3
			public ValueCollection(WeakDictionary<KeyType, ValueType> dict)
			{
				this.Dict = dict;
			}

			// Token: 0x17001F2C RID: 7980
			// (get) Token: 0x06008A13 RID: 35347 RVA: 0x002568E2 File Offset: 0x00254AE2
			// (set) Token: 0x06008A14 RID: 35348 RVA: 0x002568EA File Offset: 0x00254AEA
			public WeakDictionary<KeyType, ValueType> Dict { get; private set; }

			// Token: 0x06008A15 RID: 35349 RVA: 0x0003E264 File Offset: 0x0003C464
			public void Add(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A16 RID: 35350 RVA: 0x0003E264 File Offset: 0x0003C464
			public void Clear()
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A17 RID: 35351 RVA: 0x0003E264 File Offset: 0x0003C464
			public bool Contains(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A18 RID: 35352 RVA: 0x0003E264 File Offset: 0x0003C464
			public void CopyTo(ValueType[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			// Token: 0x17001F2D RID: 7981
			// (get) Token: 0x06008A19 RID: 35353 RVA: 0x002568F3 File Offset: 0x00254AF3
			public int Count
			{
				get
				{
					return this.Dict.Count;
				}
			}

			// Token: 0x17001F2E RID: 7982
			// (get) Token: 0x06008A1A RID: 35354 RVA: 0x00016748 File Offset: 0x00014948
			public bool IsReadOnly
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06008A1B RID: 35355 RVA: 0x0003E264 File Offset: 0x0003C464
			public bool Remove(ValueType item)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06008A1C RID: 35356 RVA: 0x00256900 File Offset: 0x00254B00
			public IEnumerator<ValueType> GetEnumerator()
			{
				IWeakHashtable hashTable = this.Dict._hashTable;
				foreach (object key in hashTable.Keys)
				{
					KeyType keyType = hashTable.UnwrapKey(key) as KeyType;
					if (keyType != null)
					{
						yield return (ValueType)((object)hashTable[key]);
					}
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x06008A1D RID: 35357 RVA: 0x0025690F File Offset: 0x00254B0F
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}
