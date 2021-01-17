using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000490 RID: 1168
	internal class CustomDictionarySources : IList<Uri>, ICollection<Uri>, IEnumerable<Uri>, IEnumerable, IList, ICollection
	{
		// Token: 0x060044C2 RID: 17602 RVA: 0x0013829B File Offset: 0x0013649B
		internal CustomDictionarySources(TextBoxBase owner)
		{
			this._owner = owner;
			this._uriList = new List<Uri>();
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x001382B5 File Offset: 0x001364B5
		public IEnumerator<Uri> GetEnumerator()
		{
			return this._uriList.GetEnumerator();
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x001382B5 File Offset: 0x001364B5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._uriList.GetEnumerator();
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x001382C7 File Offset: 0x001364C7
		int IList<Uri>.IndexOf(Uri item)
		{
			return this._uriList.IndexOf(item);
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x001382D8 File Offset: 0x001364D8
		void IList<Uri>.Insert(int index, Uri item)
		{
			if (this._uriList.Contains(item))
			{
				throw new ArgumentException(SR.Get("CustomDictionaryItemAlreadyExists"), "item");
			}
			CustomDictionarySources.ValidateUri(item);
			this._uriList.Insert(index, item);
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriAdded(item);
			}
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x00138330 File Offset: 0x00136530
		void IList<Uri>.RemoveAt(int index)
		{
			Uri uri = this._uriList[index];
			this._uriList.RemoveAt(index);
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriRemoved(uri);
			}
		}

		// Token: 0x170010E2 RID: 4322
		Uri IList<Uri>.this[int index]
		{
			get
			{
				return this._uriList[index];
			}
			set
			{
				CustomDictionarySources.ValidateUri(value);
				Uri uri = this._uriList[index];
				if (this.Speller != null)
				{
					this.Speller.OnDictionaryUriRemoved(uri);
				}
				this._uriList[index] = value;
				if (this.Speller != null)
				{
					this.Speller.OnDictionaryUriAdded(value);
				}
			}
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x001383CD File Offset: 0x001365CD
		void ICollection<Uri>.Add(Uri item)
		{
			CustomDictionarySources.ValidateUri(item);
			if (!this._uriList.Contains(item))
			{
				this._uriList.Add(item);
			}
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriAdded(item);
			}
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x00138403 File Offset: 0x00136603
		void ICollection<Uri>.Clear()
		{
			this._uriList.Clear();
			if (this.Speller != null)
			{
				this.Speller.OnDictionaryUriCollectionCleared();
			}
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x00138423 File Offset: 0x00136623
		bool ICollection<Uri>.Contains(Uri item)
		{
			return this._uriList.Contains(item);
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x00138431 File Offset: 0x00136631
		void ICollection<Uri>.CopyTo(Uri[] array, int arrayIndex)
		{
			this._uriList.CopyTo(array, arrayIndex);
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x060044CE RID: 17614 RVA: 0x00138440 File Offset: 0x00136640
		int ICollection<Uri>.Count
		{
			get
			{
				return this._uriList.Count;
			}
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x060044CF RID: 17615 RVA: 0x0013844D File Offset: 0x0013664D
		bool ICollection<Uri>.IsReadOnly
		{
			get
			{
				return ((ICollection<Uri>)this._uriList).IsReadOnly;
			}
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x0013845C File Offset: 0x0013665C
		bool ICollection<Uri>.Remove(Uri item)
		{
			bool flag = this._uriList.Remove(item);
			if (flag && this.Speller != null)
			{
				this.Speller.OnDictionaryUriRemoved(item);
			}
			return flag;
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0013848E File Offset: 0x0013668E
		int IList.Add(object value)
		{
			((ICollection<Uri>)this).Add((Uri)value);
			return this._uriList.IndexOf((Uri)value);
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x001384AD File Offset: 0x001366AD
		void IList.Clear()
		{
			((ICollection<Uri>)this).Clear();
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x001384B5 File Offset: 0x001366B5
		bool IList.Contains(object value)
		{
			return ((IList)this._uriList).Contains(value);
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x001384C3 File Offset: 0x001366C3
		int IList.IndexOf(object value)
		{
			return ((IList)this._uriList).IndexOf(value);
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x001384D1 File Offset: 0x001366D1
		void IList.Insert(int index, object value)
		{
			((IList<Uri>)this).Insert(index, (Uri)value);
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x060044D6 RID: 17622 RVA: 0x001384E0 File Offset: 0x001366E0
		bool IList.IsFixedSize
		{
			get
			{
				return ((IList)this._uriList).IsFixedSize;
			}
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x060044D7 RID: 17623 RVA: 0x001384ED File Offset: 0x001366ED
		bool IList.IsReadOnly
		{
			get
			{
				return ((IList)this._uriList).IsReadOnly;
			}
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x001384FA File Offset: 0x001366FA
		void IList.Remove(object value)
		{
			((ICollection<Uri>)this).Remove((Uri)value);
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x00138509 File Offset: 0x00136709
		void IList.RemoveAt(int index)
		{
			((IList<Uri>)this).RemoveAt(index);
		}

		// Token: 0x170010E7 RID: 4327
		object IList.this[int index]
		{
			get
			{
				return this._uriList[index];
			}
			set
			{
				((IList<Uri>)this)[index] = (Uri)value;
			}
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x00138521 File Offset: 0x00136721
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)this._uriList).CopyTo(array, index);
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x060044DD RID: 17629 RVA: 0x00138530 File Offset: 0x00136730
		int ICollection.Count
		{
			get
			{
				return ((ICollection<Uri>)this).Count;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x060044DE RID: 17630 RVA: 0x00138538 File Offset: 0x00136738
		bool ICollection.IsSynchronized
		{
			get
			{
				return ((ICollection)this._uriList).IsSynchronized;
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x060044DF RID: 17631 RVA: 0x00138545 File Offset: 0x00136745
		object ICollection.SyncRoot
		{
			get
			{
				return ((ICollection)this._uriList).SyncRoot;
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x00138552 File Offset: 0x00136752
		private Speller Speller
		{
			get
			{
				if (this._owner.TextEditor == null)
				{
					return null;
				}
				return this._owner.TextEditor.Speller;
			}
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x00138574 File Offset: 0x00136774
		private static void ValidateUri(Uri item)
		{
			if (item == null)
			{
				throw new ArgumentException(SR.Get("CustomDictionaryNullItem"));
			}
			if (item.IsAbsoluteUri && !item.IsUnc && !item.IsFile && !PackUriHelper.IsPackUri(item))
			{
				throw new NotSupportedException(SR.Get("CustomDictionarySourcesUnsupportedURI"));
			}
		}

		// Token: 0x040028B0 RID: 10416
		private readonly List<Uri> _uriList;

		// Token: 0x040028B1 RID: 10417
		private readonly TextBoxBase _owner;
	}
}
