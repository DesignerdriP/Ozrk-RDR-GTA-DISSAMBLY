using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x0200074A RID: 1866
	internal class ValueChangedEventManager : WeakEventManager
	{
		// Token: 0x06007704 RID: 30468 RVA: 0x002202E0 File Offset: 0x0021E4E0
		private ValueChangedEventManager()
		{
		}

		// Token: 0x06007705 RID: 30469 RVA: 0x002202F3 File Offset: 0x0021E4F3
		public static void AddListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			ValueChangedEventManager.CurrentManager.PrivateAddListener(source, listener, pd);
		}

		// Token: 0x06007706 RID: 30470 RVA: 0x0022031E File Offset: 0x0021E51E
		public static void RemoveListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (listener == null)
			{
				throw new ArgumentNullException("listener");
			}
			ValueChangedEventManager.CurrentManager.PrivateRemoveListener(source, listener, pd);
		}

		// Token: 0x06007707 RID: 30471 RVA: 0x00220349 File Offset: 0x0021E549
		public static void AddHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetInvocationList().Length != 1)
			{
				throw new NotSupportedException(SR.Get("NoMulticastHandlers"));
			}
			ValueChangedEventManager.CurrentManager.PrivateAddHandler(source, handler, pd);
		}

		// Token: 0x06007708 RID: 30472 RVA: 0x00220381 File Offset: 0x0021E581
		public static void RemoveHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			if (handler.GetInvocationList().Length != 1)
			{
				throw new NotSupportedException(SR.Get("NoMulticastHandlers"));
			}
			ValueChangedEventManager.CurrentManager.PrivateRemoveHandler(source, handler, pd);
		}

		// Token: 0x06007709 RID: 30473 RVA: 0x002203B9 File Offset: 0x0021E5B9
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<ValueChangedEventArgs>();
		}

		// Token: 0x0600770A RID: 30474 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StartListening(object source)
		{
		}

		// Token: 0x0600770B RID: 30475 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StopListening(object source)
		{
		}

		// Token: 0x0600770C RID: 30476 RVA: 0x002203C0 File Offset: 0x0021E5C0
		protected override bool Purge(object source, object data, bool purgeAll)
		{
			bool result = false;
			HybridDictionary hybridDictionary = (HybridDictionary)data;
			if (!BaseAppContextSwitches.EnableWeakEventMemoryImprovements)
			{
				ICollection keys = hybridDictionary.Keys;
				PropertyDescriptor[] array = new PropertyDescriptor[keys.Count];
				keys.CopyTo(array, 0);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					bool flag = purgeAll || source == null;
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[array[i]];
					if (!flag)
					{
						if (valueChangedRecord.Purge())
						{
							result = true;
						}
						flag = valueChangedRecord.IsEmpty;
					}
					if (flag)
					{
						valueChangedRecord.StopListening();
						if (!purgeAll)
						{
							hybridDictionary.Remove(array[i]);
						}
					}
				}
			}
			else
			{
				IDictionaryEnumerator enumerator = hybridDictionary.GetEnumerator();
				while (enumerator.MoveNext())
				{
					bool flag2 = purgeAll || source == null;
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord2 = (ValueChangedEventManager.ValueChangedRecord)enumerator.Value;
					if (!flag2)
					{
						if (valueChangedRecord2.Purge())
						{
							result = true;
						}
						flag2 = valueChangedRecord2.IsEmpty;
					}
					if (flag2)
					{
						valueChangedRecord2.StopListening();
						if (!purgeAll)
						{
							this._toRemove.Add((PropertyDescriptor)enumerator.Key);
						}
					}
				}
				if (this._toRemove.Count > 0)
				{
					foreach (PropertyDescriptor key in this._toRemove)
					{
						hybridDictionary.Remove(key);
					}
					this._toRemove.Clear();
					this._toRemove.TrimExcess();
				}
			}
			if (hybridDictionary.Count == 0)
			{
				result = true;
				if (source != null)
				{
					base.Remove(source);
				}
			}
			return result;
		}

		// Token: 0x17001C4C RID: 7244
		// (get) Token: 0x0600770D RID: 30477 RVA: 0x00220550 File Offset: 0x0021E750
		private static ValueChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(ValueChangedEventManager);
				ValueChangedEventManager valueChangedEventManager = (ValueChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (valueChangedEventManager == null)
				{
					valueChangedEventManager = new ValueChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, valueChangedEventManager);
				}
				return valueChangedEventManager;
			}
		}

		// Token: 0x0600770E RID: 30478 RVA: 0x00220585 File Offset: 0x0021E785
		private void PrivateAddListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			this.AddListener(source, pd, listener, null);
		}

		// Token: 0x0600770F RID: 30479 RVA: 0x00220591 File Offset: 0x0021E791
		private void PrivateRemoveListener(object source, IWeakEventListener listener, PropertyDescriptor pd)
		{
			this.RemoveListener(source, pd, listener, null);
		}

		// Token: 0x06007710 RID: 30480 RVA: 0x0022059D File Offset: 0x0021E79D
		private void PrivateAddHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			this.AddListener(source, pd, null, handler);
		}

		// Token: 0x06007711 RID: 30481 RVA: 0x002205A9 File Offset: 0x0021E7A9
		private void PrivateRemoveHandler(object source, EventHandler<ValueChangedEventArgs> handler, PropertyDescriptor pd)
		{
			this.RemoveListener(source, pd, null, handler);
		}

		// Token: 0x06007712 RID: 30482 RVA: 0x002205B8 File Offset: 0x0021E7B8
		private void AddListener(object source, PropertyDescriptor pd, IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
		{
			using (base.WriteLock)
			{
				HybridDictionary hybridDictionary = (HybridDictionary)base[source];
				if (hybridDictionary == null)
				{
					hybridDictionary = new HybridDictionary();
					base[source] = hybridDictionary;
				}
				ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[pd];
				if (valueChangedRecord == null)
				{
					valueChangedRecord = new ValueChangedEventManager.ValueChangedRecord(this, source, pd);
					hybridDictionary[pd] = valueChangedRecord;
				}
				valueChangedRecord.Add(listener, handler);
				base.ScheduleCleanup();
			}
		}

		// Token: 0x06007713 RID: 30483 RVA: 0x00220638 File Offset: 0x0021E838
		private void RemoveListener(object source, PropertyDescriptor pd, IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
		{
			using (base.WriteLock)
			{
				HybridDictionary hybridDictionary = (HybridDictionary)base[source];
				if (hybridDictionary != null)
				{
					ValueChangedEventManager.ValueChangedRecord valueChangedRecord = (ValueChangedEventManager.ValueChangedRecord)hybridDictionary[pd];
					if (valueChangedRecord != null)
					{
						valueChangedRecord.Remove(listener, handler);
						if (valueChangedRecord.IsEmpty)
						{
							hybridDictionary.Remove(pd);
						}
					}
					if (hybridDictionary.Count == 0)
					{
						base.Remove(source);
					}
				}
			}
		}

		// Token: 0x040038A8 RID: 14504
		private List<PropertyDescriptor> _toRemove = new List<PropertyDescriptor>();

		// Token: 0x02000B62 RID: 2914
		private class ValueChangedRecord
		{
			// Token: 0x06008DF6 RID: 36342 RVA: 0x0025AC58 File Offset: 0x00258E58
			public ValueChangedRecord(ValueChangedEventManager manager, object source, PropertyDescriptor pd)
			{
				this._manager = manager;
				this._source = source;
				this._pd = pd;
				this._eventArgs = new ValueChangedEventArgs(pd);
				pd.AddValueChanged(source, new EventHandler(this.OnValueChanged));
			}

			// Token: 0x17001F90 RID: 8080
			// (get) Token: 0x06008DF7 RID: 36343 RVA: 0x0025ACAC File Offset: 0x00258EAC
			public bool IsEmpty
			{
				get
				{
					bool flag = this._listeners.IsEmpty;
					if (!flag && this.HasIgnorableListeners)
					{
						flag = true;
						int i = 0;
						int count = this._listeners.Count;
						while (i < count)
						{
							if (!this.IsIgnorable(this._listeners.GetListener(i).Target))
							{
								flag = false;
								break;
							}
							i++;
						}
					}
					return flag;
				}
			}

			// Token: 0x06008DF8 RID: 36344 RVA: 0x0025AD0C File Offset: 0x00258F0C
			public void Add(IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				if (handler != null)
				{
					this._listeners.AddHandler(handler);
					if (!this.HasIgnorableListeners && this.IsIgnorable(handler.Target))
					{
						this.HasIgnorableListeners = true;
						return;
					}
				}
				else
				{
					this._listeners.Add(listener);
				}
			}

			// Token: 0x06008DF9 RID: 36345 RVA: 0x0025AD70 File Offset: 0x00258F70
			public void Remove(IWeakEventListener listener, EventHandler<ValueChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				if (handler != null)
				{
					this._listeners.RemoveHandler(handler);
				}
				else
				{
					this._listeners.Remove(listener);
				}
				if (this.IsEmpty)
				{
					this.StopListening();
				}
			}

			// Token: 0x06008DFA RID: 36346 RVA: 0x0025ADC4 File Offset: 0x00258FC4
			public bool Purge()
			{
				WeakEventManager.ListenerList listeners = this._listeners;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref listeners))
				{
					this._listeners = (WeakEventManager.ListenerList<ValueChangedEventArgs>)listeners;
				}
				return this._listeners.Purge();
			}

			// Token: 0x06008DFB RID: 36347 RVA: 0x0025ADF8 File Offset: 0x00258FF8
			public void StopListening()
			{
				if (this._source != null)
				{
					this._pd.RemoveValueChanged(this._source, new EventHandler(this.OnValueChanged));
					this._source = null;
				}
			}

			// Token: 0x06008DFC RID: 36348 RVA: 0x0025AE28 File Offset: 0x00259028
			private void OnValueChanged(object sender, EventArgs e)
			{
				using (this._manager.ReadLock)
				{
					this._listeners.BeginUse();
				}
				try
				{
					this._manager.DeliverEventToList(sender, this._eventArgs, this._listeners);
				}
				finally
				{
					this._listeners.EndUse();
				}
			}

			// Token: 0x17001F91 RID: 8081
			// (get) Token: 0x06008DFD RID: 36349 RVA: 0x0025AE9C File Offset: 0x0025909C
			// (set) Token: 0x06008DFE RID: 36350 RVA: 0x0025AEA4 File Offset: 0x002590A4
			private bool HasIgnorableListeners { get; set; }

			// Token: 0x06008DFF RID: 36351 RVA: 0x0025AEAD File Offset: 0x002590AD
			private bool IsIgnorable(object target)
			{
				return target is ValueTable;
			}

			// Token: 0x04004B25 RID: 19237
			private PropertyDescriptor _pd;

			// Token: 0x04004B26 RID: 19238
			private ValueChangedEventManager _manager;

			// Token: 0x04004B27 RID: 19239
			private object _source;

			// Token: 0x04004B28 RID: 19240
			private WeakEventManager.ListenerList<ValueChangedEventArgs> _listeners = new WeakEventManager.ListenerList<ValueChangedEventArgs>();

			// Token: 0x04004B29 RID: 19241
			private ValueChangedEventArgs _eventArgs;
		}
	}
}
