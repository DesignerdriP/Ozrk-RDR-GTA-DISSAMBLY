﻿using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MS.Internal.Controls;

namespace System.Windows.Automation.Peers
{
	// Token: 0x020002C7 RID: 711
	internal class RecyclableWrapper : IDisposable
	{
		// Token: 0x06002751 RID: 10065 RVA: 0x000B9EB4 File Offset: 0x000B80B4
		public RecyclableWrapper(ItemsControl itemsControl, object item)
		{
			this._itemsControl = itemsControl;
			this._container = ((IGeneratorHost)itemsControl).GetContainerForItem(item);
			this.LinkItem(item);
		}

		// Token: 0x06002752 RID: 10066 RVA: 0x000B9ED7 File Offset: 0x000B80D7
		public void LinkItem(object item)
		{
			this._item = item;
			ItemContainerGenerator.LinkContainerToItem(this._container, this._item);
			((IItemContainerGenerator)this._itemsControl.ItemContainerGenerator).PrepareItemContainer(this._container);
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000B9F07 File Offset: 0x000B8107
		private void UnlinkItem()
		{
			if (this._item != null)
			{
				ItemContainerGenerator.UnlinkContainerFromItem(this._container, this._item, this._itemsControl);
				this._item = null;
			}
		}

		// Token: 0x06002754 RID: 10068 RVA: 0x000B9F2F File Offset: 0x000B812F
		void IDisposable.Dispose()
		{
			this.UnlinkItem();
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x06002755 RID: 10069 RVA: 0x000B9F37 File Offset: 0x000B8137
		public AutomationPeer Peer
		{
			get
			{
				return UIElementAutomationPeer.CreatePeerForElement((UIElement)this._container);
			}
		}

		// Token: 0x04001B8E RID: 7054
		private ItemsControl _itemsControl;

		// Token: 0x04001B8F RID: 7055
		private DependencyObject _container;

		// Token: 0x04001B90 RID: 7056
		private object _item;
	}
}
