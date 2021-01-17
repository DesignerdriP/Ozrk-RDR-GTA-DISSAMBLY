using System;
using System.Collections.Specialized;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200058C RID: 1420
	internal class InternalCollectionChangedEventManager : WeakEventManager
	{
		// Token: 0x06005E10 RID: 24080 RVA: 0x0001737C File Offset: 0x0001557C
		private InternalCollectionChangedEventManager()
		{
		}

		// Token: 0x06005E11 RID: 24081 RVA: 0x001A71E0 File Offset: 0x001A53E0
		public static void AddListener(GridViewColumnCollection source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedAddListener(source, listener);
		}

		// Token: 0x06005E12 RID: 24082 RVA: 0x001A720A File Offset: 0x001A540A
		public static void RemoveListener(GridViewColumnCollection source, IWeakEventListener listener)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedRemoveListener(source, listener);
		}

		// Token: 0x06005E13 RID: 24083 RVA: 0x001A7234 File Offset: 0x001A5434
		public static void AddHandler(GridViewColumnCollection source, EventHandler<NotifyCollectionChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedAddHandler(source, handler);
		}

		// Token: 0x06005E14 RID: 24084 RVA: 0x001A7250 File Offset: 0x001A5450
		public static void RemoveHandler(GridViewColumnCollection source, EventHandler<NotifyCollectionChangedEventArgs> handler)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			InternalCollectionChangedEventManager.CurrentManager.ProtectedRemoveHandler(source, handler);
		}

		// Token: 0x06005E15 RID: 24085 RVA: 0x001A726C File Offset: 0x001A546C
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<NotifyCollectionChangedEventArgs>();
		}

		// Token: 0x06005E16 RID: 24086 RVA: 0x001A7274 File Offset: 0x001A5474
		protected override void StartListening(object source)
		{
			GridViewColumnCollection gridViewColumnCollection = (GridViewColumnCollection)source;
			gridViewColumnCollection.InternalCollectionChanged += this.OnCollectionChanged;
		}

		// Token: 0x06005E17 RID: 24087 RVA: 0x001A729C File Offset: 0x001A549C
		protected override void StopListening(object source)
		{
			GridViewColumnCollection gridViewColumnCollection = (GridViewColumnCollection)source;
			gridViewColumnCollection.InternalCollectionChanged -= this.OnCollectionChanged;
		}

		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x06005E18 RID: 24088 RVA: 0x001A72C4 File Offset: 0x001A54C4
		private static InternalCollectionChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(InternalCollectionChangedEventManager);
				InternalCollectionChangedEventManager internalCollectionChangedEventManager = (InternalCollectionChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (internalCollectionChangedEventManager == null)
				{
					internalCollectionChangedEventManager = new InternalCollectionChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, internalCollectionChangedEventManager);
				}
				return internalCollectionChangedEventManager;
			}
		}

		// Token: 0x06005E19 RID: 24089 RVA: 0x000174E5 File Offset: 0x000156E5
		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			base.DeliverEvent(sender, args);
		}
	}
}
