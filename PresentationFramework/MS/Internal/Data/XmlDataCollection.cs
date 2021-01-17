using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Xml;

namespace MS.Internal.Data
{
	// Token: 0x0200074F RID: 1871
	internal class XmlDataCollection : ReadOnlyObservableCollection<XmlNode>
	{
		// Token: 0x06007741 RID: 30529 RVA: 0x00221598 File Offset: 0x0021F798
		internal XmlDataCollection(XmlDataProvider xmlDataProvider) : base(new ObservableCollection<XmlNode>())
		{
			this._xds = xmlDataProvider;
		}

		// Token: 0x17001C56 RID: 7254
		// (get) Token: 0x06007742 RID: 30530 RVA: 0x002215AC File Offset: 0x0021F7AC
		// (set) Token: 0x06007743 RID: 30531 RVA: 0x002215D5 File Offset: 0x0021F7D5
		internal XmlNamespaceManager XmlNamespaceManager
		{
			get
			{
				if (this._nsMgr == null && this._xds != null)
				{
					this._nsMgr = this._xds.XmlNamespaceManager;
				}
				return this._nsMgr;
			}
			set
			{
				this._nsMgr = value;
			}
		}

		// Token: 0x17001C57 RID: 7255
		// (get) Token: 0x06007744 RID: 30532 RVA: 0x002215DE File Offset: 0x0021F7DE
		internal XmlDataProvider ParentXmlDataProvider
		{
			get
			{
				return this._xds;
			}
		}

		// Token: 0x06007745 RID: 30533 RVA: 0x002215E8 File Offset: 0x0021F7E8
		internal bool CollectionHasChanged(XmlNodeList nodes)
		{
			int count = base.Count;
			if (count != nodes.Count)
			{
				return true;
			}
			for (int i = 0; i < count; i++)
			{
				if (base[i] != nodes[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007746 RID: 30534 RVA: 0x00221628 File Offset: 0x0021F828
		internal void SynchronizeCollection(XmlNodeList nodes)
		{
			if (nodes == null)
			{
				base.Items.Clear();
				return;
			}
			int i = 0;
			while (i < base.Count)
			{
				if (i >= nodes.Count)
				{
					break;
				}
				if (base[i] != nodes[i])
				{
					int num = i + 1;
					while (num < nodes.Count && base[i] != nodes[num])
					{
						num++;
					}
					if (num < nodes.Count)
					{
						while (i < num)
						{
							base.Items.Insert(i, nodes[i]);
							i++;
						}
						i++;
					}
					else
					{
						base.Items.RemoveAt(i);
					}
				}
				else
				{
					i++;
				}
			}
			while (i < base.Count)
			{
				base.Items.RemoveAt(i);
			}
			while (i < nodes.Count)
			{
				base.Items.Insert(i, nodes[i]);
				i++;
			}
		}

		// Token: 0x040038B8 RID: 14520
		private XmlDataProvider _xds;

		// Token: 0x040038B9 RID: 14521
		private XmlNamespaceManager _nsMgr;
	}
}
