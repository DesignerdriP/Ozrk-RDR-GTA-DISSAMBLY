using System;
using System.Collections;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000700 RID: 1792
	internal sealed class AccessorTable
	{
		// Token: 0x06007344 RID: 29508 RVA: 0x00210DEC File Offset: 0x0020EFEC
		internal AccessorTable()
		{
		}

		// Token: 0x17001B5E RID: 7006
		internal AccessorInfo this[SourceValueType sourceValueType, Type type, string name]
		{
			get
			{
				if (type == null || name == null)
				{
					return null;
				}
				AccessorInfo accessorInfo = (AccessorInfo)this._table[new AccessorTable.AccessorTableKey(sourceValueType, type, name)];
				if (accessorInfo != null)
				{
					accessorInfo.Generation = this._generation;
				}
				return accessorInfo;
			}
			set
			{
				if (type != null && name != null)
				{
					value.Generation = this._generation;
					this._table[new AccessorTable.AccessorTableKey(sourceValueType, type, name)] = value;
					if (!this._cleanupRequested)
					{
						this.RequestCleanup();
					}
				}
			}
		}

		// Token: 0x06007347 RID: 29511 RVA: 0x00210E9A File Offset: 0x0020F09A
		private void RequestCleanup()
		{
			this._cleanupRequested = true;
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(this.CleanupOperation), null);
		}

		// Token: 0x06007348 RID: 29512 RVA: 0x00210EBC File Offset: 0x0020F0BC
		private object CleanupOperation(object arg)
		{
			object[] array = new object[this._table.Count];
			int num = 0;
			IDictionaryEnumerator enumerator = this._table.GetEnumerator();
			while (enumerator.MoveNext())
			{
				AccessorInfo accessorInfo = (AccessorInfo)enumerator.Value;
				int num2 = this._generation - accessorInfo.Generation;
				if (num2 >= 10)
				{
					array[num++] = enumerator.Key;
				}
			}
			for (int i = 0; i < num; i++)
			{
				this._table.Remove(array[i]);
			}
			this._generation++;
			this._cleanupRequested = false;
			return null;
		}

		// Token: 0x06007349 RID: 29513 RVA: 0x00002137 File Offset: 0x00000337
		internal void PrintStats()
		{
		}

		// Token: 0x17001B5F RID: 7007
		// (get) Token: 0x0600734A RID: 29514 RVA: 0x00210F55 File Offset: 0x0020F155
		// (set) Token: 0x0600734B RID: 29515 RVA: 0x00210F5D File Offset: 0x0020F15D
		internal bool TraceSize
		{
			get
			{
				return this._traceSize;
			}
			set
			{
				this._traceSize = value;
			}
		}

		// Token: 0x0400378E RID: 14222
		private const int AgeLimit = 10;

		// Token: 0x0400378F RID: 14223
		private Hashtable _table = new Hashtable();

		// Token: 0x04003790 RID: 14224
		private int _generation;

		// Token: 0x04003791 RID: 14225
		private bool _cleanupRequested;

		// Token: 0x04003792 RID: 14226
		private bool _traceSize;

		// Token: 0x02000B40 RID: 2880
		private struct AccessorTableKey
		{
			// Token: 0x06008D79 RID: 36217 RVA: 0x00259734 File Offset: 0x00257934
			public AccessorTableKey(SourceValueType sourceValueType, Type type, string name)
			{
				Invariant.Assert(type != null && type != null);
				this._sourceValueType = sourceValueType;
				this._type = type;
				this._name = name;
			}

			// Token: 0x06008D7A RID: 36218 RVA: 0x00259763 File Offset: 0x00257963
			public override bool Equals(object o)
			{
				return o is AccessorTable.AccessorTableKey && this == (AccessorTable.AccessorTableKey)o;
			}

			// Token: 0x06008D7B RID: 36219 RVA: 0x00259780 File Offset: 0x00257980
			public static bool operator ==(AccessorTable.AccessorTableKey k1, AccessorTable.AccessorTableKey k2)
			{
				return k1._sourceValueType == k2._sourceValueType && k1._type == k2._type && k1._name == k2._name;
			}

			// Token: 0x06008D7C RID: 36220 RVA: 0x002597B6 File Offset: 0x002579B6
			public static bool operator !=(AccessorTable.AccessorTableKey k1, AccessorTable.AccessorTableKey k2)
			{
				return !(k1 == k2);
			}

			// Token: 0x06008D7D RID: 36221 RVA: 0x002597C2 File Offset: 0x002579C2
			public override int GetHashCode()
			{
				return this._type.GetHashCode() + this._name.GetHashCode();
			}

			// Token: 0x04004ABB RID: 19131
			private SourceValueType _sourceValueType;

			// Token: 0x04004ABC RID: 19132
			private Type _type;

			// Token: 0x04004ABD RID: 19133
			private string _name;
		}
	}
}
