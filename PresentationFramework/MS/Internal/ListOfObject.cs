using System;
using System.Collections;
using System.Collections.Generic;

namespace MS.Internal
{
	// Token: 0x020005EC RID: 1516
	internal class ListOfObject : IList<object>, ICollection<object>, IEnumerable<object>, IEnumerable
	{
		// Token: 0x0600651F RID: 25887 RVA: 0x001C6660 File Offset: 0x001C4860
		internal ListOfObject(IList list)
		{
			if (list == null)
			{
				throw new ArgumentNullException("list");
			}
			this._list = list;
		}

		// Token: 0x06006520 RID: 25888 RVA: 0x001C667D File Offset: 0x001C487D
		int IList<object>.IndexOf(object item)
		{
			return this._list.IndexOf(item);
		}

		// Token: 0x06006521 RID: 25889 RVA: 0x0003E264 File Offset: 0x0003C464
		void IList<object>.Insert(int index, object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006522 RID: 25890 RVA: 0x0003E264 File Offset: 0x0003C464
		void IList<object>.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17001844 RID: 6212
		object IList<object>.this[int index]
		{
			get
			{
				return this._list[index];
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06006525 RID: 25893 RVA: 0x0003E264 File Offset: 0x0003C464
		void ICollection<object>.Add(object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006526 RID: 25894 RVA: 0x0003E264 File Offset: 0x0003C464
		void ICollection<object>.Clear()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006527 RID: 25895 RVA: 0x001C6699 File Offset: 0x001C4899
		bool ICollection<object>.Contains(object item)
		{
			return this._list.Contains(item);
		}

		// Token: 0x06006528 RID: 25896 RVA: 0x001C66A7 File Offset: 0x001C48A7
		void ICollection<object>.CopyTo(object[] array, int arrayIndex)
		{
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x17001845 RID: 6213
		// (get) Token: 0x06006529 RID: 25897 RVA: 0x001C66B6 File Offset: 0x001C48B6
		int ICollection<object>.Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x17001846 RID: 6214
		// (get) Token: 0x0600652A RID: 25898 RVA: 0x00016748 File Offset: 0x00014948
		bool ICollection<object>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600652B RID: 25899 RVA: 0x0003E264 File Offset: 0x0003C464
		bool ICollection<object>.Remove(object item)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600652C RID: 25900 RVA: 0x001C66C3 File Offset: 0x001C48C3
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			return new ListOfObject.ObjectEnumerator(this._list);
		}

		// Token: 0x0600652D RID: 25901 RVA: 0x001C66D0 File Offset: 0x001C48D0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}

		// Token: 0x040032BA RID: 12986
		private IList _list;

		// Token: 0x02000A08 RID: 2568
		private class ObjectEnumerator : IEnumerator<object>, IDisposable, IEnumerator
		{
			// Token: 0x06008A00 RID: 35328 RVA: 0x0025683D File Offset: 0x00254A3D
			public ObjectEnumerator(IList list)
			{
				this._ie = list.GetEnumerator();
			}

			// Token: 0x17001F27 RID: 7975
			// (get) Token: 0x06008A01 RID: 35329 RVA: 0x00256851 File Offset: 0x00254A51
			object IEnumerator<object>.Current
			{
				get
				{
					return this._ie.Current;
				}
			}

			// Token: 0x06008A02 RID: 35330 RVA: 0x0025685E File Offset: 0x00254A5E
			void IDisposable.Dispose()
			{
				this._ie = null;
			}

			// Token: 0x17001F28 RID: 7976
			// (get) Token: 0x06008A03 RID: 35331 RVA: 0x00256851 File Offset: 0x00254A51
			object IEnumerator.Current
			{
				get
				{
					return this._ie.Current;
				}
			}

			// Token: 0x06008A04 RID: 35332 RVA: 0x00256867 File Offset: 0x00254A67
			bool IEnumerator.MoveNext()
			{
				return this._ie.MoveNext();
			}

			// Token: 0x06008A05 RID: 35333 RVA: 0x00256874 File Offset: 0x00254A74
			void IEnumerator.Reset()
			{
				this._ie.Reset();
			}

			// Token: 0x040046B4 RID: 18100
			private IEnumerator _ie;
		}
	}
}
