using System;
using System.Windows;
using System.Xml;

namespace MS.Internal.Data
{
	// Token: 0x02000750 RID: 1872
	internal class XmlNodeChangedEventManager : WeakEventManager
	{
		// Token: 0x06007747 RID: 30535 RVA: 0x0001737C File Offset: 0x0001557C
		private XmlNodeChangedEventManager()
		{
		}

		// Token: 0x06007748 RID: 30536 RVA: 0x00221707 File Offset: 0x0021F907
		public static void AddListener(XmlDocument source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x06007749 RID: 30537 RVA: 0x00221731 File Offset: 0x0021F931
		public static void RemoveListener(XmlDocument source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x0600774A RID: 30538 RVA: 0x0022175B File Offset: 0x0021F95B
		public static void AddHandler(XmlDocument source, EventHandler<XmlNodeChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x0600774B RID: 30539 RVA: 0x00221777 File Offset: 0x0021F977
		public static void RemoveHandler(XmlDocument source, EventHandler<XmlNodeChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			XmlNodeChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x0600774C RID: 30540 RVA: 0x00221793 File Offset: 0x0021F993
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<XmlNodeChangedEventArgs>();
		}

		// Token: 0x0600774D RID: 30541 RVA: 0x0022179C File Offset: 0x0021F99C
		protected override void StartListening(object source)
		{
			XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnXmlNodeChanged);
			XmlDocument xmlDocument = (XmlDocument)source;
			xmlDocument.NodeInserted += value;
			xmlDocument.NodeRemoved += value;
			xmlDocument.NodeChanged += value;
		}

		// Token: 0x0600774E RID: 30542 RVA: 0x002217D4 File Offset: 0x0021F9D4
		protected override void StopListening(object source)
		{
			XmlNodeChangedEventHandler value = new XmlNodeChangedEventHandler(this.OnXmlNodeChanged);
			XmlDocument xmlDocument = (XmlDocument)source;
			xmlDocument.NodeInserted -= value;
			xmlDocument.NodeRemoved -= value;
			xmlDocument.NodeChanged -= value;
		}

		// Token: 0x17001C58 RID: 7256
		// (get) Token: 0x0600774F RID: 30543 RVA: 0x0022180C File Offset: 0x0021FA0C
		private static XmlNodeChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(XmlNodeChangedEventManager);
				XmlNodeChangedEventManager xmlNodeChangedEventManager = (XmlNodeChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (xmlNodeChangedEventManager == null)
				{
					xmlNodeChangedEventManager = new XmlNodeChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, xmlNodeChangedEventManager);
				}
				return xmlNodeChangedEventManager;
			}
		}

		// Token: 0x06007750 RID: 30544 RVA: 0x000174E5 File Offset: 0x000156E5
		private void OnXmlNodeChanged(object sender, XmlNodeChangedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
