using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Xml;

namespace MS.Internal.Annotations
{
	// Token: 0x020007CF RID: 1999
	internal sealed class XmlElementCollection : ObservableCollection<XmlElement>
	{
		// Token: 0x06007BB1 RID: 31665 RVA: 0x0022BDE0 File Offset: 0x00229FE0
		public XmlElementCollection()
		{
			this._xmlDocsRefCounts = new Dictionary<XmlDocument, int>();
		}

		// Token: 0x06007BB2 RID: 31666 RVA: 0x0022BDF4 File Offset: 0x00229FF4
		protected override void ClearItems()
		{
			foreach (XmlElement element in this)
			{
				this.UnregisterForElement(element);
			}
			base.ClearItems();
		}

		// Token: 0x06007BB3 RID: 31667 RVA: 0x0022BE44 File Offset: 0x0022A044
		protected override void RemoveItem(int index)
		{
			XmlElement element = base[index];
			this.UnregisterForElement(element);
			base.RemoveItem(index);
		}

		// Token: 0x06007BB4 RID: 31668 RVA: 0x0022BE68 File Offset: 0x0022A068
		protected override void InsertItem(int index, XmlElement item)
		{
			if (item != null && base.Contains(item))
			{
				throw new ArgumentException(SR.Get("XmlNodeAlreadyOwned", new object[]
				{
					"change",
					"change"
				}), "item");
			}
			base.InsertItem(index, item);
			this.RegisterForElement(item);
		}

		// Token: 0x06007BB5 RID: 31669 RVA: 0x0022BEBC File Offset: 0x0022A0BC
		protected override void SetItem(int index, XmlElement item)
		{
			if (item != null && base.Contains(item))
			{
				throw new ArgumentException(SR.Get("XmlNodeAlreadyOwned", new object[]
				{
					"change",
					"change"
				}), "item");
			}
			XmlElement element = base[index];
			this.UnregisterForElement(element);
			base.Items[index] = item;
			this.OnCollectionReset();
			this.RegisterForElement(item);
		}

		// Token: 0x06007BB6 RID: 31670 RVA: 0x0022BF2C File Offset: 0x0022A12C
		private void UnregisterForElement(XmlElement element)
		{
			if (element == null)
			{
				return;
			}
			Invariant.Assert(this._xmlDocsRefCounts.ContainsKey(element.OwnerDocument), "Not registered on XmlElement");
			Dictionary<XmlDocument, int> xmlDocsRefCounts = this._xmlDocsRefCounts;
			XmlDocument ownerDocument = element.OwnerDocument;
			int num = xmlDocsRefCounts[ownerDocument];
			xmlDocsRefCounts[ownerDocument] = num - 1;
			if (this._xmlDocsRefCounts[element.OwnerDocument] == 0)
			{
				element.OwnerDocument.NodeChanged -= this.OnNodeChanged;
				element.OwnerDocument.NodeInserted -= this.OnNodeChanged;
				element.OwnerDocument.NodeRemoved -= this.OnNodeChanged;
				this._xmlDocsRefCounts.Remove(element.OwnerDocument);
			}
		}

		// Token: 0x06007BB7 RID: 31671 RVA: 0x0022BFE0 File Offset: 0x0022A1E0
		private void RegisterForElement(XmlElement element)
		{
			if (element == null)
			{
				return;
			}
			if (!this._xmlDocsRefCounts.ContainsKey(element.OwnerDocument))
			{
				this._xmlDocsRefCounts[element.OwnerDocument] = 1;
				XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnNodeChanged);
				element.OwnerDocument.NodeChanged += value;
				element.OwnerDocument.NodeInserted += value;
				element.OwnerDocument.NodeRemoved += value;
				return;
			}
			Dictionary<XmlDocument, int> xmlDocsRefCounts = this._xmlDocsRefCounts;
			XmlDocument ownerDocument = element.OwnerDocument;
			int num = xmlDocsRefCounts[ownerDocument];
			xmlDocsRefCounts[ownerDocument] = num + 1;
		}

		// Token: 0x06007BB8 RID: 31672 RVA: 0x0022C068 File Offset: 0x0022A268
		private void OnNodeChanged(object sender, XmlNodeChangedEventArgs args)
		{
			Invariant.Assert(this._xmlDocsRefCounts.ContainsKey(sender as XmlDocument), "Not expecting a notification from this sender");
			XmlNode xmlNode = args.Node;
			while (xmlNode != null)
			{
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null && base.Contains(xmlElement))
				{
					this.OnCollectionReset();
					return;
				}
				XmlAttribute xmlAttribute = xmlNode as XmlAttribute;
				if (xmlAttribute != null)
				{
					xmlNode = xmlAttribute.OwnerElement;
				}
				else
				{
					xmlNode = xmlNode.ParentNode;
				}
			}
		}

		// Token: 0x06007BB9 RID: 31673 RVA: 0x0022C0D4 File Offset: 0x0022A2D4
		private void OnCollectionReset()
		{
			this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
		}

		// Token: 0x04003A33 RID: 14899
		private Dictionary<XmlDocument, int> _xmlDocsRefCounts;
	}
}
