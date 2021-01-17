using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C4 RID: 1988
	internal class AnnotationObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged2, IOwnedObject
	{
		// Token: 0x06007B6E RID: 31598 RVA: 0x0022B634 File Offset: 0x00229834
		public AnnotationObservableCollection()
		{
			this._listener = new PropertyChangedEventHandler(this.OnItemPropertyChanged);
		}

		// Token: 0x06007B6F RID: 31599 RVA: 0x0022B665 File Offset: 0x00229865
		public AnnotationObservableCollection(List<T> list) : base(list)
		{
			this._listener = new PropertyChangedEventHandler(this.OnItemPropertyChanged);
		}

		// Token: 0x06007B70 RID: 31600 RVA: 0x0022B698 File Offset: 0x00229898
		protected override void ClearItems()
		{
			foreach (!0 ! in this)
			{
				INotifyPropertyChanged2 item = !;
				this.SetOwned(item, false);
			}
			this.ProtectedClearItems();
		}

		// Token: 0x06007B71 RID: 31601 RVA: 0x0022B6EC File Offset: 0x002298EC
		protected override void RemoveItem(int index)
		{
			T t = base[index];
			this.SetOwned(t, false);
			base.RemoveItem(index);
		}

		// Token: 0x06007B72 RID: 31602 RVA: 0x0022B715 File Offset: 0x00229915
		protected override void InsertItem(int index, T item)
		{
			if (this.ItemOwned(item))
			{
				throw new ArgumentException(SR.Get("AlreadyHasParent"));
			}
			base.InsertItem(index, item);
			this.SetOwned(item, true);
		}

		// Token: 0x06007B73 RID: 31603 RVA: 0x0022B74C File Offset: 0x0022994C
		protected override void SetItem(int index, T item)
		{
			if (this.ItemOwned(item))
			{
				throw new ArgumentException(SR.Get("AlreadyHasParent"));
			}
			T t = base[index];
			this.SetOwned(t, false);
			this.ProtectedSetItem(index, item);
			this.SetOwned(item, true);
		}

		// Token: 0x06007B74 RID: 31604 RVA: 0x0022B7A1 File Offset: 0x002299A1
		protected virtual void ProtectedClearItems()
		{
			base.ClearItems();
		}

		// Token: 0x06007B75 RID: 31605 RVA: 0x0022B7A9 File Offset: 0x002299A9
		protected virtual void ProtectedSetItem(int index, T item)
		{
			base.Items[index] = item;
			this.OnPropertyChanged(new PropertyChangedEventArgs(this.CountString));
			this.OnPropertyChanged(new PropertyChangedEventArgs(this.IndexerName));
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06007B76 RID: 31606 RVA: 0x0022B7E6 File Offset: 0x002299E6
		protected void ObservableCollectionSetItem(int index, T item)
		{
			base.SetItem(index, item);
		}

		// Token: 0x06007B77 RID: 31607 RVA: 0x0022B7F0 File Offset: 0x002299F0
		protected virtual void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x06007B78 RID: 31608 RVA: 0x0022B800 File Offset: 0x00229A00
		private bool ItemOwned(object item)
		{
			if (item != null)
			{
				IOwnedObject ownedObject = item as IOwnedObject;
				return ownedObject.Owned;
			}
			return false;
		}

		// Token: 0x06007B79 RID: 31609 RVA: 0x0022B820 File Offset: 0x00229A20
		private void SetOwned(object item, bool owned)
		{
			if (item != null)
			{
				IOwnedObject ownedObject = item as IOwnedObject;
				ownedObject.Owned = owned;
				if (owned)
				{
					((INotifyPropertyChanged2)item).PropertyChanged += this._listener;
					return;
				}
				((INotifyPropertyChanged2)item).PropertyChanged -= this._listener;
			}
		}

		// Token: 0x04003A1A RID: 14874
		private readonly PropertyChangedEventHandler _listener;

		// Token: 0x04003A1B RID: 14875
		internal readonly string CountString = "Count";

		// Token: 0x04003A1C RID: 14876
		internal readonly string IndexerName = "Item[]";
	}
}
