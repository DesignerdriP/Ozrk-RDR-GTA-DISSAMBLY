using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Data
{
	// Token: 0x0200074E RID: 1870
	internal class XmlBindingWorker : BindingWorker, IWeakEventListener
	{
		// Token: 0x06007725 RID: 30501 RVA: 0x00220BB3 File Offset: 0x0021EDB3
		internal XmlBindingWorker(ClrBindingWorker worker, bool collectionMode) : base(worker.ParentBindingExpression)
		{
			this._hostWorker = worker;
			this._xpath = base.ParentBinding.XPath;
			this._collectionMode = collectionMode;
			this._xpathType = XmlBindingWorker.GetXPathType(this._xpath);
		}

		// Token: 0x06007726 RID: 30502 RVA: 0x00220BF4 File Offset: 0x0021EDF4
		internal override void AttachDataItem()
		{
			if (this.XPath.Length > 0)
			{
				this.CollectionView = (base.DataItem as CollectionView);
				if (this.CollectionView == null && base.DataItem is ICollection)
				{
					this.CollectionView = CollectionViewSource.GetDefaultCollectionView(base.DataItem, base.TargetElement, null);
				}
			}
			if (this.CollectionView != null)
			{
				CurrentChangedEventManager.AddHandler(this.CollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.AddHandler(this.CollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
			this.UpdateContextNode(true);
		}

		// Token: 0x06007727 RID: 30503 RVA: 0x00220C9C File Offset: 0x0021EE9C
		internal override void DetachDataItem()
		{
			if (this.CollectionView != null)
			{
				CurrentChangedEventManager.RemoveHandler(this.CollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.RemoveHandler(this.CollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
			this.UpdateContextNode(false);
			this.CollectionView = null;
		}

		// Token: 0x06007728 RID: 30504 RVA: 0x00220D00 File Offset: 0x0021EF00
		internal override void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
			if (collectionView == this.CollectionView)
			{
				using (base.ParentBindingExpression.ChangingValue())
				{
					this.UpdateContextNode(true);
					this._hostWorker.UseNewXmlItem(this.RawValue());
				}
			}
		}

		// Token: 0x06007729 RID: 30505 RVA: 0x00220D58 File Offset: 0x0021EF58
		internal override object RawValue()
		{
			if (this.XPath.Length == 0)
			{
				return base.DataItem;
			}
			if (this.ContextNode == null)
			{
				this.QueriedCollection = null;
				return null;
			}
			XmlNodeList xmlNodeList = this.SelectNodes();
			if (xmlNodeList == null)
			{
				this.QueriedCollection = null;
				return DependencyProperty.UnsetValue;
			}
			return this.BuildQueriedCollection(xmlNodeList);
		}

		// Token: 0x0600772A RID: 30506 RVA: 0x00220DA8 File Offset: 0x0021EFA8
		internal void ReportBadXPath(TraceEventType traceType)
		{
			if (TraceData.IsEnabled)
			{
				TraceData.Trace(traceType, TraceData.BadXPath(new object[]
				{
					this.XPath,
					this.IdentifyNode(this.ContextNode)
				}));
			}
		}

		// Token: 0x17001C50 RID: 7248
		// (get) Token: 0x0600772B RID: 30507 RVA: 0x00220DDA File Offset: 0x0021EFDA
		// (set) Token: 0x0600772C RID: 30508 RVA: 0x00220DE2 File Offset: 0x0021EFE2
		private XmlDataCollection QueriedCollection
		{
			get
			{
				return this._queriedCollection;
			}
			set
			{
				this._queriedCollection = value;
			}
		}

		// Token: 0x17001C51 RID: 7249
		// (get) Token: 0x0600772D RID: 30509 RVA: 0x00220DEB File Offset: 0x0021EFEB
		// (set) Token: 0x0600772E RID: 30510 RVA: 0x00220DF3 File Offset: 0x0021EFF3
		private ICollectionView CollectionView
		{
			get
			{
				return this._collectionView;
			}
			set
			{
				this._collectionView = value;
			}
		}

		// Token: 0x17001C52 RID: 7250
		// (get) Token: 0x0600772F RID: 30511 RVA: 0x00220DFC File Offset: 0x0021EFFC
		// (set) Token: 0x06007730 RID: 30512 RVA: 0x00220E04 File Offset: 0x0021F004
		private XmlNode ContextNode
		{
			get
			{
				return this._contextNode;
			}
			set
			{
				if (this._contextNode != value && TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.Transfer))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.XmlContextNode(new object[]
					{
						TraceData.Identify(base.ParentBindingExpression),
						this.IdentifyNode(value)
					}));
				}
				this._contextNode = value;
			}
		}

		// Token: 0x17001C53 RID: 7251
		// (get) Token: 0x06007731 RID: 30513 RVA: 0x00220E58 File Offset: 0x0021F058
		private string XPath
		{
			get
			{
				return this._xpath;
			}
		}

		// Token: 0x17001C54 RID: 7252
		// (get) Token: 0x06007732 RID: 30514 RVA: 0x00220E60 File Offset: 0x0021F060
		private XmlNamespaceManager NamespaceManager
		{
			get
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement == null)
				{
					return null;
				}
				XmlNamespaceManager xmlNamespaceManager = Binding.GetXmlNamespaceManager(targetElement);
				if (xmlNamespaceManager == null && this.XmlDataProvider != null)
				{
					xmlNamespaceManager = this.XmlDataProvider.XmlNamespaceManager;
				}
				return xmlNamespaceManager;
			}
		}

		// Token: 0x17001C55 RID: 7253
		// (get) Token: 0x06007733 RID: 30515 RVA: 0x00220E98 File Offset: 0x0021F098
		private XmlDataProvider XmlDataProvider
		{
			get
			{
				if (this._xmlDataProvider == null && (this._xmlDataProvider = (base.ParentBindingExpression.DataSource as XmlDataProvider)) == null)
				{
					XmlDataCollection xmlDataCollection;
					ItemsControl itemsControl;
					if ((xmlDataCollection = (base.DataItem as XmlDataCollection)) != null)
					{
						this._xmlDataProvider = xmlDataCollection.ParentXmlDataProvider;
					}
					else if (this.CollectionView != null && (xmlDataCollection = (this.CollectionView.SourceCollection as XmlDataCollection)) != null)
					{
						this._xmlDataProvider = xmlDataCollection.ParentXmlDataProvider;
					}
					else if (base.TargetProperty == BindingExpressionBase.NoTargetProperty && (itemsControl = (base.TargetElement as ItemsControl)) != null)
					{
						object itemsSource = itemsControl.ItemsSource;
						if ((xmlDataCollection = (itemsSource as XmlDataCollection)) == null)
						{
							ICollectionView collectionView = itemsSource as ICollectionView;
							xmlDataCollection = (((collectionView != null) ? collectionView.SourceCollection : null) as XmlDataCollection);
						}
						if (xmlDataCollection != null)
						{
							this._xmlDataProvider = xmlDataCollection.ParentXmlDataProvider;
						}
					}
					else
					{
						this._xmlDataProvider = Helper.XmlDataProviderForElement(base.TargetElement);
					}
				}
				return this._xmlDataProvider;
			}
		}

		// Token: 0x06007734 RID: 30516 RVA: 0x00220F8C File Offset: 0x0021F18C
		private void UpdateContextNode(bool hookNotifications)
		{
			this.UnHookNotifications();
			if (base.DataItem == BindingExpressionBase.DisconnectedItem)
			{
				this.ContextNode = null;
				return;
			}
			if (this.CollectionView != null)
			{
				this.ContextNode = (this.CollectionView.CurrentItem as XmlNode);
				if (this.ContextNode != this.CollectionView.CurrentItem && TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.XmlBindingToNonXmlCollection, this.XPath, base.ParentBindingExpression, base.DataItem);
				}
			}
			else
			{
				this.ContextNode = (base.DataItem as XmlNode);
				if (this.ContextNode != base.DataItem && TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.XmlBindingToNonXml, this.XPath, base.ParentBindingExpression, base.DataItem);
				}
			}
			if (hookNotifications)
			{
				this.HookNotifications();
			}
		}

		// Token: 0x06007735 RID: 30517 RVA: 0x00221058 File Offset: 0x0021F258
		private void HookNotifications()
		{
			if (base.IsDynamic && this.ContextNode != null)
			{
				XmlDocument xmlDocument = this.DocumentFor(this.ContextNode);
				if (xmlDocument != null)
				{
					XmlNodeChangedEventManager.AddHandler(xmlDocument, new EventHandler<XmlNodeChangedEventArgs>(this.OnXmlNodeChanged));
				}
			}
		}

		// Token: 0x06007736 RID: 30518 RVA: 0x00221098 File Offset: 0x0021F298
		private void UnHookNotifications()
		{
			if (base.IsDynamic && this.ContextNode != null)
			{
				XmlDocument xmlDocument = this.DocumentFor(this.ContextNode);
				if (xmlDocument != null)
				{
					XmlNodeChangedEventManager.RemoveHandler(xmlDocument, new EventHandler<XmlNodeChangedEventArgs>(this.OnXmlNodeChanged));
				}
			}
		}

		// Token: 0x06007737 RID: 30519 RVA: 0x002210D8 File Offset: 0x0021F2D8
		private XmlDocument DocumentFor(XmlNode node)
		{
			XmlDocument xmlDocument = node.OwnerDocument;
			if (xmlDocument == null)
			{
				xmlDocument = (node as XmlDocument);
			}
			return xmlDocument;
		}

		// Token: 0x06007738 RID: 30520 RVA: 0x002210F8 File Offset: 0x0021F2F8
		private XmlDataCollection BuildQueriedCollection(XmlNodeList nodes)
		{
			if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.CreateExpression))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.XmlNewCollection(new object[]
				{
					TraceData.Identify(base.ParentBindingExpression),
					this.IdentifyNodeList(nodes)
				}));
			}
			this.QueriedCollection = new XmlDataCollection(this.XmlDataProvider);
			this.QueriedCollection.XmlNamespaceManager = this.NamespaceManager;
			this.QueriedCollection.SynchronizeCollection(nodes);
			return this.QueriedCollection;
		}

		// Token: 0x06007739 RID: 30521 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs args)
		{
			return false;
		}

		// Token: 0x0600773A RID: 30522 RVA: 0x00221170 File Offset: 0x0021F370
		private void OnXmlNodeChanged(object sender, XmlNodeChangedEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(base.ParentBindingExpression),
					"XmlNodeChanged",
					TraceData.Identify(sender)
				}));
			}
			this.ProcessXmlNodeChanged(e);
		}

		// Token: 0x0600773B RID: 30523 RVA: 0x002211C4 File Offset: 0x0021F3C4
		private void ProcessXmlNodeChanged(EventArgs args)
		{
			DependencyObject targetElement = base.ParentBindingExpression.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			if (base.IgnoreSourcePropertyChange)
			{
				return;
			}
			if (base.DataItem == BindingExpressionBase.DisconnectedItem)
			{
				return;
			}
			if (!this.IsChangeRelevant(args))
			{
				return;
			}
			if (this.XPath.Length == 0)
			{
				this._hostWorker.OnXmlValueChanged();
			}
			else if (this.QueriedCollection == null)
			{
				this._hostWorker.UseNewXmlItem(this.RawValue());
			}
			else
			{
				XmlNodeList xmlNodeList = this.SelectNodes();
				if (xmlNodeList == null)
				{
					this.QueriedCollection = null;
					this._hostWorker.UseNewXmlItem(DependencyProperty.UnsetValue);
				}
				else if (this._collectionMode)
				{
					if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.CreateExpression))
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.XmlSynchronizeCollection(new object[]
						{
							TraceData.Identify(base.ParentBindingExpression),
							this.IdentifyNodeList(xmlNodeList)
						}));
					}
					this.QueriedCollection.SynchronizeCollection(xmlNodeList);
				}
				else if (this.QueriedCollection.CollectionHasChanged(xmlNodeList))
				{
					this._hostWorker.UseNewXmlItem(this.BuildQueriedCollection(xmlNodeList));
				}
				else
				{
					this._hostWorker.OnXmlValueChanged();
				}
			}
			GC.KeepAlive(targetElement);
		}

		// Token: 0x0600773C RID: 30524 RVA: 0x002212E0 File Offset: 0x0021F4E0
		private XmlNodeList SelectNodes()
		{
			XmlNamespaceManager namespaceManager = this.NamespaceManager;
			XmlNodeList xmlNodeList = null;
			try
			{
				if (namespaceManager != null)
				{
					xmlNodeList = this.ContextNode.SelectNodes(this.XPath, namespaceManager);
				}
				else
				{
					xmlNodeList = this.ContextNode.SelectNodes(this.XPath);
				}
			}
			catch (XPathException ex)
			{
				base.Status = BindingStatusInternal.PathError;
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.CannotGetXmlNodeCollection, new object[]
					{
						(this.ContextNode != null) ? this.ContextNode.Name : null,
						this.XPath,
						base.ParentBindingExpression,
						ex
					});
				}
			}
			if (TraceData.IsExtendedTraceEnabled(base.ParentBindingExpression, TraceDataLevel.CreateExpression))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.SelectNodes(new object[]
				{
					TraceData.Identify(base.ParentBindingExpression),
					this.IdentifyNode(this.ContextNode),
					TraceData.Identify(this.XPath),
					this.IdentifyNodeList(xmlNodeList)
				}));
			}
			return xmlNodeList;
		}

		// Token: 0x0600773D RID: 30525 RVA: 0x002213D8 File Offset: 0x0021F5D8
		private string IdentifyNode(XmlNode node)
		{
			if (node == null)
			{
				return "<null>";
			}
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0} ({1})", new object[]
			{
				node.GetType().Name,
				node.Name
			});
		}

		// Token: 0x0600773E RID: 30526 RVA: 0x00221410 File Offset: 0x0021F610
		private string IdentifyNodeList(XmlNodeList nodeList)
		{
			if (nodeList == null)
			{
				return "<null>";
			}
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0} (hash={1} Count={2})", new object[]
			{
				nodeList.GetType().Name,
				AvTrace.GetHashCodeHelper(nodeList),
				nodeList.Count
			});
		}

		// Token: 0x0600773F RID: 30527 RVA: 0x00221468 File Offset: 0x0021F668
		private static XmlBindingWorker.XPathType GetXPathType(string xpath)
		{
			int length = xpath.Length;
			if (length == 0)
			{
				return XmlBindingWorker.XPathType.SimpleName;
			}
			bool flag = xpath[0] == '@';
			int i = flag ? 1 : 0;
			if (i >= length)
			{
				return XmlBindingWorker.XPathType.Default;
			}
			char c = xpath[i];
			if (!char.IsLetter(c) && c != '_' && c != ':')
			{
				return XmlBindingWorker.XPathType.Default;
			}
			for (i++; i < length; i++)
			{
				c = xpath[i];
				if (!char.IsLetterOrDigit(c) && c != '.' && c != '-' && c != '_' && c != ':')
				{
					return XmlBindingWorker.XPathType.Default;
				}
			}
			if (!flag)
			{
				return XmlBindingWorker.XPathType.SimpleName;
			}
			return XmlBindingWorker.XPathType.SimpleAttribute;
		}

		// Token: 0x06007740 RID: 30528 RVA: 0x002214F4 File Offset: 0x0021F6F4
		private bool IsChangeRelevant(EventArgs rawArgs)
		{
			if (this._xpathType == XmlBindingWorker.XPathType.Default)
			{
				return true;
			}
			XmlNodeChangedEventArgs xmlNodeChangedEventArgs = (XmlNodeChangedEventArgs)rawArgs;
			XmlNode xmlNode = null;
			XmlNode xmlNode2 = null;
			switch (xmlNodeChangedEventArgs.Action)
			{
			case XmlNodeChangedAction.Insert:
				xmlNode = xmlNodeChangedEventArgs.NewParent;
				break;
			case XmlNodeChangedAction.Remove:
				xmlNode = xmlNodeChangedEventArgs.OldParent;
				break;
			case XmlNodeChangedAction.Change:
				xmlNode2 = xmlNodeChangedEventArgs.Node;
				break;
			}
			if (this._collectionMode)
			{
				return xmlNode == this.ContextNode;
			}
			if (xmlNode == this.ContextNode)
			{
				return true;
			}
			XmlNode xmlNode3 = this._hostWorker.GetResultNode() as XmlNode;
			if (xmlNode3 == null)
			{
				return false;
			}
			if (xmlNode2 != null)
			{
				xmlNode = xmlNode2;
			}
			while (xmlNode != null)
			{
				if (xmlNode == xmlNode3)
				{
					return true;
				}
				xmlNode = xmlNode.ParentNode;
			}
			return false;
		}

		// Token: 0x040038B0 RID: 14512
		private bool _collectionMode;

		// Token: 0x040038B1 RID: 14513
		private XmlBindingWorker.XPathType _xpathType;

		// Token: 0x040038B2 RID: 14514
		private XmlNode _contextNode;

		// Token: 0x040038B3 RID: 14515
		private XmlDataCollection _queriedCollection;

		// Token: 0x040038B4 RID: 14516
		private ICollectionView _collectionView;

		// Token: 0x040038B5 RID: 14517
		private XmlDataProvider _xmlDataProvider;

		// Token: 0x040038B6 RID: 14518
		private ClrBindingWorker _hostWorker;

		// Token: 0x040038B7 RID: 14519
		private string _xpath;

		// Token: 0x02000B67 RID: 2919
		private enum XPathType : byte
		{
			// Token: 0x04004B38 RID: 19256
			Default,
			// Token: 0x04004B39 RID: 19257
			SimpleName,
			// Token: 0x04004B3A RID: 19258
			SimpleAttribute
		}
	}
}
