using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x020005F5 RID: 1525
	internal class WeakHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : class
	{
		// Token: 0x0600656A RID: 25962 RVA: 0x001C70FF File Offset: 0x001C52FF
		public void Add(T item)
		{
			if (!this._hashTable.ContainsKey(item))
			{
				this._hashTable.SetWeak(item, null);
			}
		}

		// Token: 0x0600656B RID: 25963 RVA: 0x001C7126 File Offset: 0x001C5326
		public void Clear()
		{
			this._hashTable.Clear();
		}

		// Token: 0x0600656C RID: 25964 RVA: 0x001C7133 File Offset: 0x001C5333
		public bool Contains(T item)
		{
			return this._hashTable.ContainsKey(item);
		}

		// Token: 0x0600656D RID: 25965 RVA: 0x001C7148 File Offset: 0x001C5348
		public void CopyTo(T[] array, int arrayIndex)
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
			foreach (T t in this)
			{
				num++;
			}
			if (num + arrayIndex > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex");
			}
			foreach (T t2 in this)
			{
				array[arrayIndex++] = t2;
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x0600656E RID: 25966 RVA: 0x001C7200 File Offset: 0x001C5400
		public int Count
		{
			get
			{
				return this._hashTable.Count;
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x0600656F RID: 25967 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006570 RID: 25968 RVA: 0x001C720D File Offset: 0x001C540D
		public bool Remove(T item)
		{
			if (this._hashTable.ContainsKey(item))
			{
				this._hashTable.Remove(item);
				return true;
			}
			return false;
		}

		// Token: 0x06006571 RID: 25969 RVA: 0x001C7236 File Offset: 0x001C5436
		public IEnumerator<T> GetEnumerator()
		{
			foreach (object obj in this._hashTable.Keys)
			{
				WeakHashtable.EqualityWeakReference equalityWeakReference = obj as WeakHashtable.EqualityWeakReference;
				if (equalityWeakReference != null)
				{
					T t = equalityWeakReference.Target as T;
					if (t != null)
					{
						yield return t;
					}
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006572 RID: 25970 RVA: 0x001C7245 File Offset: 0x001C5445
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040032C4 RID: 12996
		private WeakHashtable _hashTable = new WeakHashtable();
	}
}
