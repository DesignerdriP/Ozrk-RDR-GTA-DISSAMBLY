using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000706 RID: 1798
	internal class BindingCollection : Collection<BindingBase>
	{
		// Token: 0x06007360 RID: 29536 RVA: 0x0021112E File Offset: 0x0020F32E
		internal BindingCollection(BindingBase owner, BindingCollectionChangedCallback callback)
		{
			Invariant.Assert(owner != null && callback != null);
			this._owner = owner;
			this._collectionChangedCallback = callback;
		}

		// Token: 0x06007361 RID: 29537 RVA: 0x00211153 File Offset: 0x0020F353
		private BindingCollection()
		{
		}

		// Token: 0x06007362 RID: 29538 RVA: 0x0021115B File Offset: 0x0020F35B
		protected override void ClearItems()
		{
			this._owner.CheckSealed();
			base.ClearItems();
			this.OnBindingCollectionChanged();
		}

		// Token: 0x06007363 RID: 29539 RVA: 0x00211174 File Offset: 0x0020F374
		protected override void RemoveItem(int index)
		{
			this._owner.CheckSealed();
			base.RemoveItem(index);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x06007364 RID: 29540 RVA: 0x0021118E File Offset: 0x0020F38E
		protected override void InsertItem(int index, BindingBase item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateItem(item);
			this._owner.CheckSealed();
			base.InsertItem(index, item);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x06007365 RID: 29541 RVA: 0x002111BE File Offset: 0x0020F3BE
		protected override void SetItem(int index, BindingBase item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			this.ValidateItem(item);
			this._owner.CheckSealed();
			base.SetItem(index, item);
			this.OnBindingCollectionChanged();
		}

		// Token: 0x06007366 RID: 29542 RVA: 0x002111EE File Offset: 0x0020F3EE
		private void ValidateItem(BindingBase binding)
		{
			if (!(binding is Binding))
			{
				throw new NotSupportedException(SR.Get("BindingCollectionContainsNonBinding", new object[]
				{
					binding.GetType().Name
				}));
			}
		}

		// Token: 0x06007367 RID: 29543 RVA: 0x0021121C File Offset: 0x0020F41C
		private void OnBindingCollectionChanged()
		{
			if (this._collectionChangedCallback != null)
			{
				this._collectionChangedCallback();
			}
		}

		// Token: 0x040037A6 RID: 14246
		private BindingBase _owner;

		// Token: 0x040037A7 RID: 14247
		private BindingCollectionChangedCallback _collectionChangedCallback;
	}
}
