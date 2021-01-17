using System;
using System.Collections;
using System.Security.Permissions;

namespace MS.Internal
{
	// Token: 0x020005F4 RID: 1524
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class WeakHashtable : Hashtable, IWeakHashtable
	{
		// Token: 0x06006562 RID: 25954 RVA: 0x001C6F2B File Offset: 0x001C512B
		internal WeakHashtable() : base(WeakHashtable._comparer)
		{
		}

		// Token: 0x06006563 RID: 25955 RVA: 0x001C6F38 File Offset: 0x001C5138
		public override void Clear()
		{
			base.Clear();
		}

		// Token: 0x06006564 RID: 25956 RVA: 0x001C6F40 File Offset: 0x001C5140
		public override void Remove(object key)
		{
			base.Remove(key);
		}

		// Token: 0x06006565 RID: 25957 RVA: 0x001C6F49 File Offset: 0x001C5149
		public void SetWeak(object key, object value)
		{
			this.ScavengeKeys();
			this[new WeakHashtable.EqualityWeakReference(key)] = value;
		}

		// Token: 0x06006566 RID: 25958 RVA: 0x001C6F60 File Offset: 0x001C5160
		public object UnwrapKey(object key)
		{
			WeakHashtable.EqualityWeakReference equalityWeakReference = key as WeakHashtable.EqualityWeakReference;
			if (equalityWeakReference == null)
			{
				return null;
			}
			return equalityWeakReference.Target;
		}

		// Token: 0x06006567 RID: 25959 RVA: 0x001C6F80 File Offset: 0x001C5180
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
			float num = (float)(totalMemory - this._lastGlobalMem) / (float)this._lastGlobalMem;
			float num2 = (float)(count - this._lastHashCount) / (float)this._lastHashCount;
			if (num < 0f && num2 >= 0f)
			{
				ArrayList arrayList = null;
				foreach (object obj in this.Keys)
				{
					WeakHashtable.EqualityWeakReference equalityWeakReference = obj as WeakHashtable.EqualityWeakReference;
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

		// Token: 0x06006568 RID: 25960 RVA: 0x001C70CC File Offset: 0x001C52CC
		public static IWeakHashtable FromKeyType(Type tKey)
		{
			if (tKey == typeof(object) || tKey.IsValueType)
			{
				return new WeakObjectHashtable();
			}
			return new WeakHashtable();
		}

		// Token: 0x040032C1 RID: 12993
		private static IEqualityComparer _comparer = new WeakHashtable.WeakKeyComparer();

		// Token: 0x040032C2 RID: 12994
		private long _lastGlobalMem;

		// Token: 0x040032C3 RID: 12995
		private int _lastHashCount;

		// Token: 0x02000A0C RID: 2572
		private class WeakKeyComparer : IEqualityComparer
		{
			// Token: 0x06008A25 RID: 35365 RVA: 0x00256A88 File Offset: 0x00254C88
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
				WeakHashtable.EqualityWeakReference equalityWeakReference = x as WeakHashtable.EqualityWeakReference;
				WeakHashtable.EqualityWeakReference equalityWeakReference2 = y as WeakHashtable.EqualityWeakReference;
				if (equalityWeakReference != null && equalityWeakReference2 != null && !equalityWeakReference2.IsAlive && !equalityWeakReference.IsAlive)
				{
					return true;
				}
				if (equalityWeakReference != null)
				{
					x = equalityWeakReference.Target;
				}
				if (equalityWeakReference2 != null)
				{
					y = equalityWeakReference2.Target;
				}
				return x == y;
			}

			// Token: 0x06008A26 RID: 35366 RVA: 0x0025650D File Offset: 0x0025470D
			int IEqualityComparer.GetHashCode(object obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x02000A0D RID: 2573
		internal sealed class EqualityWeakReference
		{
			// Token: 0x06008A28 RID: 35368 RVA: 0x00256AF0 File Offset: 0x00254CF0
			internal EqualityWeakReference(object o)
			{
				this._weakRef = new WeakReference(o);
				this._hashCode = o.GetHashCode();
			}

			// Token: 0x17001F31 RID: 7985
			// (get) Token: 0x06008A29 RID: 35369 RVA: 0x00256B10 File Offset: 0x00254D10
			public bool IsAlive
			{
				get
				{
					return this._weakRef.IsAlive;
				}
			}

			// Token: 0x17001F32 RID: 7986
			// (get) Token: 0x06008A2A RID: 35370 RVA: 0x00256B1D File Offset: 0x00254D1D
			public object Target
			{
				get
				{
					return this._weakRef.Target;
				}
			}

			// Token: 0x06008A2B RID: 35371 RVA: 0x00256B2A File Offset: 0x00254D2A
			public override bool Equals(object o)
			{
				return o != null && o.GetHashCode() == this._hashCode && (o == this || o == this.Target);
			}

			// Token: 0x06008A2C RID: 35372 RVA: 0x00256B51 File Offset: 0x00254D51
			public override int GetHashCode()
			{
				return this._hashCode;
			}

			// Token: 0x040046BB RID: 18107
			private int _hashCode;

			// Token: 0x040046BC RID: 18108
			private WeakReference _weakRef;
		}
	}
}
