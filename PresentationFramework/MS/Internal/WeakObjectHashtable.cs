using System;
using System.Collections;
using System.Security.Permissions;

namespace MS.Internal
{
	// Token: 0x020005F6 RID: 1526
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakObjectHashtable : Hashtable, IWeakHashtable
	{
		// Token: 0x06006574 RID: 25972 RVA: 0x001C7260 File Offset: 0x001C5460
		internal WeakObjectHashtable() : base(WeakObjectHashtable._comparer)
		{
		}

		// Token: 0x06006575 RID: 25973 RVA: 0x001C726D File Offset: 0x001C546D
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this.WrapKey(ref key);
			this[key] = value;
		}

		// Token: 0x06006576 RID: 25974 RVA: 0x001C7285 File Offset: 0x001C5485
		private void WrapKey(ref object key)
		{
			if (key != null && !key.GetType().IsValueType)
			{
				key = new WeakObjectHashtable.EqualityWeakReference(key);
			}
		}

		// Token: 0x06006577 RID: 25975 RVA: 0x001C72A4 File Offset: 0x001C54A4
		public object UnwrapKey(object key)
		{
			WeakObjectHashtable.EqualityWeakReference equalityWeakReference = key as WeakObjectHashtable.EqualityWeakReference;
			if (equalityWeakReference == null)
			{
				return key;
			}
			return equalityWeakReference.Target;
		}

		// Token: 0x06006578 RID: 25976 RVA: 0x001C72C4 File Offset: 0x001C54C4
		private void ScavengeKeys()
		{
			int count = this.Count;
			if (count == 0)
			{
				return;
			}
			if (this._lastHashCount == 0)
			{
				this._lastHashCount = count;
				return;
			}
			long totalMemory = GC.GetTotalMemory(false);
			if (this._lastGlobalMem == 0L)
			{
				this._lastGlobalMem = totalMemory;
				return;
			}
			long num = totalMemory - this._lastGlobalMem;
			long num2 = (long)(count - this._lastHashCount);
			if (num < 0L && num2 >= 0L)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakObjectHashtable.EqualityWeakReference equalityWeakReference = obj as WeakObjectHashtable.EqualityWeakReference;
					if (equalityWeakReference != null && !equalityWeakReference.IsAlive)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						arrayList.Add(equalityWeakReference);
					}
				}
				if (arrayList != null)
				{
					foreach (object key in arrayList)
					{
						this.Remove(key);
					}
				}
			}
			this._lastGlobalMem = totalMemory;
			this._lastHashCount = count;
		}

		// Token: 0x040032C5 RID: 12997
		private static IEqualityComparer _comparer = new WeakObjectHashtable.WeakKeyComparer();

		// Token: 0x040032C6 RID: 12998
		private long _lastGlobalMem;

		// Token: 0x040032C7 RID: 12999
		private int _lastHashCount;

		// Token: 0x02000A0F RID: 2575
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x06008A34 RID: 35380 RVA: 0x00256CB8 File Offset: 0x00254EB8
			bool IEqualityComparer.Equals(object x, object y)
			{
				if (x == null)
				{
					return y == null;
				}
				if (y == null || x.GetHashCode() != y.GetHashCode())
				{
					return false;
				}
				if (x == y)
				{
					return true;
				}
				WeakObjectHashtable.EqualityWeakReference equalityWeakReference;
				if ((equalityWeakReference = (x as WeakObjectHashtable.EqualityWeakReference)) != null)
				{
					x = equalityWeakReference.Target;
					if (x == null)
					{
						return false;
					}
				}
				WeakObjectHashtable.EqualityWeakReference equalityWeakReference2;
				if ((equalityWeakReference2 = (y as WeakObjectHashtable.EqualityWeakReference)) != null)
				{
					y = equalityWeakReference2.Target;
					if (y == null)
					{
						return false;
					}
				}
				return object.Equals(x, y);
			}

			// Token: 0x06008A35 RID: 35381 RVA: 0x0025650D File Offset: 0x0025470D
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x02000A10 RID: 2576
		internal sealed class EqualityWeakReference
		{
			// Token: 0x06008A37 RID: 35383 RVA: 0x00256D1B File Offset: 0x00254F1B
			internal EqualityWeakReference(object o)
			{
				this._weakRef = new WeakReference(o);
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x17001F35 RID: 7989
			// (get) Token: 0x06008A38 RID: 35384 RVA: 0x00256D3B File Offset: 0x00254F3B
			public bool IsAlive
			{
				get
				{
					return this._weakRef.IsAlive;
				}
			}

			// Token: 0x17001F36 RID: 7990
			// (get) Token: 0x06008A39 RID: 35385 RVA: 0x00256D48 File Offset: 0x00254F48
			public object Target
			{
				get
				{
					return this._weakRef.Target;
				}
			}

			// Token: 0x06008A3A RID: 35386 RVA: 0x00256D55 File Offset: 0x00254F55
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || o == this.Target);
			}

			// Token: 0x06008A3B RID: 35387 RVA: 0x00256D7C File Offset: 0x00254F7C
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x040046C1 RID: 18113
			private int _hashCode;

			// Token: 0x040046C2 RID: 18114
			private WeakReference _weakRef;
		}
	}
}
