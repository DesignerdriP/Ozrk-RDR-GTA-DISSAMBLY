using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal.Automation;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.ItemsControl" /> types to UI Automation. </summary>
	// Token: 0x020002C5 RID: 709
	public abstract class ItemsControlAutomationPeer : FrameworkElementAutomationPeer, IItemContainerProvider
	{
		/// <summary>Provides initialization for base class values when called by the constructor of a derived class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.ItemsControl" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" />.</param>
		// Token: 0x06002731 RID: 10033 RVA: 0x000B938C File Offset: 0x000B758C
		protected ItemsControlAutomationPeer(ItemsControl owner) : base(owner)
		{
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.ItemsControl" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>The <see cref="T:System.Windows.Automation.Peers.ScrollViewerAutomationPeer" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" />.</returns>
		// Token: 0x06002732 RID: 10034 RVA: 0x000B93AC File Offset: 0x000B75AC
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Scroll)
			{
				ItemsControl itemsControl = (ItemsControl)base.Owner;
				if (itemsControl.ScrollHost != null)
				{
					AutomationPeer automationPeer = UIElementAutomationPeer.CreatePeerForElement(itemsControl.ScrollHost);
					if (automationPeer != null && automationPeer is IScrollProvider)
					{
						automationPeer.EventsSource = this;
						return (IScrollProvider)automationPeer;
					}
				}
			}
			else if (patternInterface == PatternInterface.ItemContainer)
			{
				if (base.Owner is ItemsControl)
				{
					return this;
				}
				return null;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.ItemsControl" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" />. This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x06002733 RID: 10035 RVA: 0x000B9414 File Offset: 0x000B7614
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = null;
			ItemPeersStorage<ItemAutomationPeer> dataChildren = this._dataChildren;
			this._dataChildren = new ItemPeersStorage<ItemAutomationPeer>();
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			ItemCollection items = itemsControl.Items;
			Panel itemsHost = itemsControl.ItemsHost;
			bool useNetFx472CompatibleAccessibilityFeatures = AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures;
			if (itemsControl.IsGrouping)
			{
				if (itemsHost == null)
				{
					return null;
				}
				if (!useNetFx472CompatibleAccessibilityFeatures)
				{
					this._reusablePeers = dataChildren;
				}
				IList list2 = itemsHost.Children;
				list = new List<AutomationPeer>(list2.Count);
				foreach (object obj in list2)
				{
					UIElement uielement = (UIElement)obj;
					UIElementAutomationPeer uielementAutomationPeer = uielement.CreateAutomationPeer() as UIElementAutomationPeer;
					if (uielementAutomationPeer != null)
					{
						list.Add(uielementAutomationPeer);
						if (useNetFx472CompatibleAccessibilityFeatures)
						{
							if (this._recentlyRealizedPeers != null && this._recentlyRealizedPeers.Count > 0 && this.AncestorsInvalid)
							{
								GroupItemAutomationPeer groupItemAutomationPeer = uielementAutomationPeer as GroupItemAutomationPeer;
								if (groupItemAutomationPeer != null)
								{
									groupItemAutomationPeer.InvalidateGroupItemPeersContainingRecentlyRealizedPeers(this._recentlyRealizedPeers);
								}
							}
						}
						else if (this.AncestorsInvalid)
						{
							GroupItemAutomationPeer groupItemAutomationPeer2 = uielementAutomationPeer as GroupItemAutomationPeer;
							if (groupItemAutomationPeer2 != null)
							{
								groupItemAutomationPeer2.AncestorsInvalid = true;
								groupItemAutomationPeer2.ChildrenValid = true;
							}
						}
					}
				}
				return list;
			}
			else
			{
				if (items.Count > 0)
				{
					IList list2;
					if (this.IsVirtualized)
					{
						if (itemsHost == null)
						{
							return null;
						}
						list2 = itemsHost.Children;
					}
					else
					{
						list2 = items;
					}
					list = new List<AutomationPeer>(list2.Count);
					foreach (object obj2 in list2)
					{
						object obj3;
						if (this.IsVirtualized)
						{
							DependencyObject dependencyObject = obj2 as DependencyObject;
							obj3 = ((dependencyObject != null) ? itemsControl.ItemContainerGenerator.ItemFromContainer(dependencyObject) : null);
							if (obj3 == DependencyProperty.UnsetValue)
							{
								continue;
							}
						}
						else
						{
							obj3 = obj2;
						}
						ItemAutomationPeer itemAutomationPeer = dataChildren[obj3];
						itemAutomationPeer = this.ReusePeerForItem(itemAutomationPeer, obj3);
						if (itemAutomationPeer == null)
						{
							itemAutomationPeer = this.CreateItemAutomationPeer(obj3);
						}
						if (itemAutomationPeer != null)
						{
							AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
							if (wrapperPeer != null)
							{
								wrapperPeer.EventsSource = itemAutomationPeer;
							}
						}
						if (itemAutomationPeer != null && this._dataChildren[obj3] == null)
						{
							list.Add(itemAutomationPeer);
							this._dataChildren[obj3] = itemAutomationPeer;
						}
					}
					return list;
				}
				return null;
			}
		}

		// Token: 0x06002734 RID: 10036 RVA: 0x000B9678 File Offset: 0x000B7878
		internal ItemAutomationPeer ReusePeerForItem(ItemAutomationPeer peer, object item)
		{
			if (peer == null)
			{
				peer = this.GetPeerFromWeakRefStorage(item);
				if (peer != null)
				{
					peer.AncestorsInvalid = false;
					peer.ChildrenValid = false;
				}
			}
			if (peer != null)
			{
				peer.ReuseForItem(item);
			}
			return peer;
		}

		// Token: 0x06002735 RID: 10037 RVA: 0x000B96A4 File Offset: 0x000B78A4
		internal void AddProxyToWeakRefStorage(WeakReference wr, ItemAutomationPeer itemPeer)
		{
			ItemsControl itemsControl = base.Owner as ItemsControl;
			ItemCollection items = itemsControl.Items;
			if (items != null && this.GetPeerFromWeakRefStorage(itemPeer.Item) == null)
			{
				this.WeakRefElementProxyStorage[itemPeer.Item] = wr;
			}
		}

		/// <summary>Retrieves an element by the specified property value.</summary>
		/// <param name="startAfter">The item in the container after which to begin the search.</param>
		/// <param name="propertyId">The property that contains the value to retrieve.</param>
		/// <param name="value">The value to retrieve.</param>
		/// <returns>The first item that matches the search criterion; otherwise, <see langword="null" /> if no items match.</returns>
		// Token: 0x06002736 RID: 10038 RVA: 0x000B96E8 File Offset: 0x000B78E8
		IRawElementProviderSimple IItemContainerProvider.FindItemByProperty(IRawElementProviderSimple startAfter, int propertyId, object value)
		{
			base.ResetChildrenCache();
			if (propertyId != 0 && !this.IsPropertySupportedByControlForFindItem(propertyId))
			{
				throw new ArgumentException(SR.Get("PropertyNotSupported"));
			}
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			ItemCollection itemCollection = null;
			if (itemsControl != null)
			{
				itemCollection = itemsControl.Items;
			}
			if (itemCollection != null && itemCollection.Count > 0)
			{
				ItemAutomationPeer itemAutomationPeer = null;
				if (startAfter != null)
				{
					itemAutomationPeer = (base.PeerFromProvider(startAfter) as ItemAutomationPeer);
					if (itemAutomationPeer == null)
					{
						return null;
					}
				}
				int num = 0;
				if (itemAutomationPeer != null)
				{
					if (itemAutomationPeer.Item == null)
					{
						throw new InvalidOperationException(SR.Get("InavalidStartItem"));
					}
					num = itemCollection.IndexOf(itemAutomationPeer.Item) + 1;
					if (num == 0 || num == itemCollection.Count)
					{
						return null;
					}
				}
				if (propertyId == 0)
				{
					for (int i = num; i < itemCollection.Count; i++)
					{
						if (itemCollection.IndexOf(itemCollection[i]) == i)
						{
							return base.ProviderFromPeer(this.FindOrCreateItemAutomationPeer(itemCollection[i]));
						}
					}
				}
				object obj = null;
				for (int j = num; j < itemCollection.Count; j++)
				{
					ItemAutomationPeer itemAutomationPeer2 = this.FindOrCreateItemAutomationPeer(itemCollection[j]);
					if (itemAutomationPeer2 != null)
					{
						try
						{
							obj = this.GetSupportedPropertyValue(itemAutomationPeer2, propertyId);
						}
						catch (Exception ex)
						{
							if (ex is ElementNotAvailableException)
							{
								goto IL_166;
							}
						}
						if (value == null || obj == null)
						{
							if (obj == null && value == null && itemCollection.IndexOf(itemCollection[j]) == j)
							{
								return base.ProviderFromPeer(itemAutomationPeer2);
							}
						}
						else if (value.Equals(obj) && itemCollection.IndexOf(itemCollection[j]) == j)
						{
							return base.ProviderFromPeer(itemAutomationPeer2);
						}
					}
					IL_166:;
				}
			}
			return null;
		}

		// Token: 0x06002737 RID: 10039 RVA: 0x000B9880 File Offset: 0x000B7A80
		internal virtual bool IsPropertySupportedByControlForFindItem(int id)
		{
			return ItemsControlAutomationPeer.IsPropertySupportedByControlForFindItemInternal(id);
		}

		// Token: 0x06002738 RID: 10040 RVA: 0x000B9888 File Offset: 0x000B7A88
		internal static bool IsPropertySupportedByControlForFindItemInternal(int id)
		{
			return AutomationElementIdentifiers.NameProperty.Id == id || AutomationElementIdentifiers.AutomationIdProperty.Id == id || AutomationElementIdentifiers.ControlTypeProperty.Id == id;
		}

		// Token: 0x06002739 RID: 10041 RVA: 0x000B98B8 File Offset: 0x000B7AB8
		internal virtual object GetSupportedPropertyValue(ItemAutomationPeer itemPeer, int propertyId)
		{
			return ItemsControlAutomationPeer.GetSupportedPropertyValueInternal(itemPeer, propertyId);
		}

		// Token: 0x0600273A RID: 10042 RVA: 0x000B98C1 File Offset: 0x000B7AC1
		internal static object GetSupportedPropertyValueInternal(AutomationPeer itemPeer, int propertyId)
		{
			return itemPeer.GetPropertyValue(propertyId);
		}

		/// <summary>Returns an <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> for the specified object.</summary>
		/// <param name="item">The item to get an <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> for.</param>
		/// <returns>An <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> for the specified object.</returns>
		// Token: 0x0600273B RID: 10043 RVA: 0x000B98CC File Offset: 0x000B7ACC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected internal virtual ItemAutomationPeer FindOrCreateItemAutomationPeer(object item)
		{
			ItemAutomationPeer itemAutomationPeer = this.ItemPeers[item];
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = this.GetPeerFromWeakRefStorage(item);
			}
			if (itemAutomationPeer == null)
			{
				itemAutomationPeer = this.CreateItemAutomationPeer(item);
				if (itemAutomationPeer != null)
				{
					itemAutomationPeer.TrySetParentInfo(this);
				}
			}
			if (itemAutomationPeer != null)
			{
				AutomationPeer wrapperPeer = itemAutomationPeer.GetWrapperPeer();
				if (wrapperPeer != null)
				{
					wrapperPeer.EventsSource = itemAutomationPeer;
				}
			}
			return itemAutomationPeer;
		}

		// Token: 0x0600273C RID: 10044 RVA: 0x000B991C File Offset: 0x000B7B1C
		internal ItemAutomationPeer CreateItemAutomationPeerInternal(object item)
		{
			return this.CreateItemAutomationPeer(item);
		}

		/// <summary>When overridden in a derived class, creates a new instance of the <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> for a data item in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection of this <see cref="T:System.Windows.Controls.ItemsControl" />.</summary>
		/// <param name="item">The data item that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" />.</param>
		/// <returns>The new <see cref="T:System.Windows.Automation.Peers.ItemAutomationPeer" /> created.</returns>
		// Token: 0x0600273D RID: 10045
		protected abstract ItemAutomationPeer CreateItemAutomationPeer(object item);

		// Token: 0x0600273E RID: 10046 RVA: 0x000B9928 File Offset: 0x000B7B28
		internal RecyclableWrapper GetRecyclableWrapperPeer(object item)
		{
			ItemsControl itemsControl = (ItemsControl)base.Owner;
			if (this._recyclableWrapperCache == null)
			{
				this._recyclableWrapperCache = new RecyclableWrapper(itemsControl, item);
			}
			else
			{
				this._recyclableWrapperCache.LinkItem(item);
			}
			return this._recyclableWrapperCache;
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000B996A File Offset: 0x000B7B6A
		internal override IDisposable UpdateChildren()
		{
			base.UpdateChildrenInternal(5);
			this.WeakRefElementProxyStorage.PurgeWeakRefCollection();
			if (!AccessibilitySwitches.UseNetFx472CompatibleAccessibilityFeatures)
			{
				return new ItemsControlAutomationPeer.UpdateChildrenHelper(this);
			}
			return null;
		}

		// Token: 0x06002740 RID: 10048 RVA: 0x000B9990 File Offset: 0x000B7B90
		internal ItemAutomationPeer GetPeerFromWeakRefStorage(object item)
		{
			ItemAutomationPeer itemAutomationPeer = null;
			WeakReference weakReference = this.WeakRefElementProxyStorage[item];
			if (weakReference != null)
			{
				ElementProxy elementProxy = weakReference.Target as ElementProxy;
				if (elementProxy != null)
				{
					itemAutomationPeer = (base.PeerFromProvider(elementProxy) as ItemAutomationPeer);
					if (itemAutomationPeer == null)
					{
						this.WeakRefElementProxyStorage.Remove(item);
					}
				}
				else
				{
					this.WeakRefElementProxyStorage.Remove(item);
				}
			}
			return itemAutomationPeer;
		}

		// Token: 0x06002741 RID: 10049 RVA: 0x000B99EC File Offset: 0x000B7BEC
		internal AutomationPeer GetExistingPeerByItem(object item, bool checkInWeakRefStorage)
		{
			AutomationPeer automationPeer = null;
			if (checkInWeakRefStorage)
			{
				automationPeer = this.GetPeerFromWeakRefStorage(item);
			}
			if (automationPeer == null)
			{
				automationPeer = this.ItemPeers[item];
			}
			return automationPeer;
		}

		// Token: 0x06002742 RID: 10050 RVA: 0x000B9A17 File Offset: 0x000B7C17
		internal ItemAutomationPeer ReusablePeerFor(object item)
		{
			if (this._reusablePeers != null)
			{
				return this._reusablePeers[item];
			}
			return this.ItemPeers[item];
		}

		// Token: 0x06002743 RID: 10051 RVA: 0x000B9A3A File Offset: 0x000B7C3A
		private void ClearReusablePeers(ItemPeersStorage<ItemAutomationPeer> oldChildren)
		{
			if (this._reusablePeers == oldChildren)
			{
				this._reusablePeers = null;
			}
		}

		/// <summary>Gets a value that indicates whether the <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" /> should return <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> objects for child items that are not virtualized. </summary>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" /> should return <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> objects for child items that are not virtualized; <see langword="false" /> if the <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" /> should return <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> objects all child items. </returns>
		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06002744 RID: 10052 RVA: 0x000B9A4C File Offset: 0x000B7C4C
		protected virtual bool IsVirtualized
		{
			get
			{
				return ItemContainerPatternIdentifiers.Pattern != null;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06002745 RID: 10053 RVA: 0x000B9A56 File Offset: 0x000B7C56
		// (set) Token: 0x06002746 RID: 10054 RVA: 0x000B9A5E File Offset: 0x000B7C5E
		internal ItemPeersStorage<ItemAutomationPeer> ItemPeers
		{
			get
			{
				return this._dataChildren;
			}
			set
			{
				this._dataChildren = value;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06002747 RID: 10055 RVA: 0x000B9A67 File Offset: 0x000B7C67
		// (set) Token: 0x06002748 RID: 10056 RVA: 0x000B9A6F File Offset: 0x000B7C6F
		internal ItemPeersStorage<WeakReference> WeakRefElementProxyStorage
		{
			get
			{
				return this._WeakRefElementProxyStorage;
			}
			set
			{
				this._WeakRefElementProxyStorage = value;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x000B9A78 File Offset: 0x000B7C78
		internal List<ItemAutomationPeer> RecentlyRealizedPeers
		{
			get
			{
				if (this._recentlyRealizedPeers == null)
				{
					this._recentlyRealizedPeers = new List<ItemAutomationPeer>();
				}
				return this._recentlyRealizedPeers;
			}
		}

		// Token: 0x04001B85 RID: 7045
		private ItemPeersStorage<ItemAutomationPeer> _dataChildren = new ItemPeersStorage<ItemAutomationPeer>();

		// Token: 0x04001B86 RID: 7046
		private ItemPeersStorage<ItemAutomationPeer> _reusablePeers;

		// Token: 0x04001B87 RID: 7047
		private ItemPeersStorage<WeakReference> _WeakRefElementProxyStorage = new ItemPeersStorage<WeakReference>();

		// Token: 0x04001B88 RID: 7048
		private List<ItemAutomationPeer> _recentlyRealizedPeers;

		// Token: 0x04001B89 RID: 7049
		private RecyclableWrapper _recyclableWrapperCache;

		// Token: 0x020008BC RID: 2236
		private class UpdateChildrenHelper : IDisposable
		{
			// Token: 0x0600843E RID: 33854 RVA: 0x00247E03 File Offset: 0x00246003
			internal UpdateChildrenHelper(ItemsControlAutomationPeer peer)
			{
				this._peer = peer;
				this._oldChildren = peer.ItemPeers;
			}

			// Token: 0x0600843F RID: 33855 RVA: 0x00247E1E File Offset: 0x0024601E
			void IDisposable.Dispose()
			{
				if (this._peer != null)
				{
					this._peer.ClearReusablePeers(this._oldChildren);
					this._peer = null;
				}
			}

			// Token: 0x0400420E RID: 16910
			private ItemsControlAutomationPeer _peer;

			// Token: 0x0400420F RID: 16911
			private ItemPeersStorage<ItemAutomationPeer> _oldChildren;
		}
	}
}
