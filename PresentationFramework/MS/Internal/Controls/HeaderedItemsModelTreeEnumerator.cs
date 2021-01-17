using System;
using System.Collections;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000762 RID: 1890
	internal class HeaderedItemsModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x0600783D RID: 30781 RVA: 0x00224083 File Offset: 0x00222283
		internal HeaderedItemsModelTreeEnumerator(HeaderedItemsControl headeredItemsControl, IEnumerator items, object header) : base(header)
		{
			this._owner = headeredItemsControl;
			this._items = items;
		}

		// Token: 0x17001C81 RID: 7297
		// (get) Token: 0x0600783E RID: 30782 RVA: 0x0022409A File Offset: 0x0022229A
		protected override object Current
		{
			get
			{
				if (base.Index > 0)
				{
					return this._items.Current;
				}
				return base.Current;
			}
		}

		// Token: 0x0600783F RID: 30783 RVA: 0x002240B8 File Offset: 0x002222B8
		protected override bool MoveNext()
		{
			if (base.Index >= 0)
			{
				int index = base.Index;
				base.Index = index + 1;
				return this._items.MoveNext();
			}
			return base.MoveNext();
		}

		// Token: 0x06007840 RID: 30784 RVA: 0x002240F0 File Offset: 0x002222F0
		protected override void Reset()
		{
			base.Reset();
			this._items.Reset();
		}

		// Token: 0x17001C82 RID: 7298
		// (get) Token: 0x06007841 RID: 30785 RVA: 0x00224104 File Offset: 0x00222304
		protected override bool IsUnchanged
		{
			get
			{
				object content = base.Content;
				return content == this._owner.Header;
			}
		}

		// Token: 0x040038EC RID: 14572
		private HeaderedItemsControl _owner;

		// Token: 0x040038ED RID: 14573
		private IEnumerator _items;
	}
}
