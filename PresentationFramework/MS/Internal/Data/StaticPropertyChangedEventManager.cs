using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000743 RID: 1859
	internal class StaticPropertyChangedEventManager : WeakEventManager
	{
		// Token: 0x060076D1 RID: 30417 RVA: 0x0001737C File Offset: 0x0001557C
		private StaticPropertyChangedEventManager()
		{
		}

		// Token: 0x060076D2 RID: 30418 RVA: 0x0021F624 File Offset: 0x0021D824
		public static void AddHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			StaticPropertyChangedEventManager.CurrentManager.PrivateAddHandler(type, handler, propertyName);
		}

		// Token: 0x060076D3 RID: 30419 RVA: 0x0021F655 File Offset: 0x0021D855
		public static void RemoveHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (handler == null)
			{
				throw new ArgumentNullException("handler");
			}
			StaticPropertyChangedEventManager.CurrentManager.PrivateRemoveHandler(type, handler, propertyName);
		}

		// Token: 0x060076D4 RID: 30420 RVA: 0x0021F686 File Offset: 0x0021D886
		protected override WeakEventManager.ListenerList NewListenerList()
		{
			return new WeakEventManager.ListenerList<PropertyChangedEventArgs>();
		}

		// Token: 0x060076D5 RID: 30421 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StartListening(object source)
		{
		}

		// Token: 0x060076D6 RID: 30422 RVA: 0x00002137 File Offset: 0x00000337
		protected override void StopListening(object source)
		{
		}

		// Token: 0x060076D7 RID: 30423 RVA: 0x0021F690 File Offset: 0x0021D890
		protected override bool Purge(object source, object data, bool purgeAll)
		{
			StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)data;
			bool result = typeRecord.Purge(purgeAll);
			if (!purgeAll && typeRecord.IsEmpty)
			{
				base.Remove(typeRecord.Type);
			}
			return result;
		}

		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x060076D8 RID: 30424 RVA: 0x0021F6C4 File Offset: 0x0021D8C4
		private static StaticPropertyChangedEventManager CurrentManager
		{
			get
			{
				Type typeFromHandle = typeof(StaticPropertyChangedEventManager);
				StaticPropertyChangedEventManager staticPropertyChangedEventManager = (StaticPropertyChangedEventManager)WeakEventManager.GetCurrentManager(typeFromHandle);
				if (staticPropertyChangedEventManager == null)
				{
					staticPropertyChangedEventManager = new StaticPropertyChangedEventManager();
					WeakEventManager.SetCurrentManager(typeFromHandle, staticPropertyChangedEventManager);
				}
				return staticPropertyChangedEventManager;
			}
		}

		// Token: 0x060076D9 RID: 30425 RVA: 0x0021F6FC File Offset: 0x0021D8FC
		private void PrivateAddHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			using (base.WriteLock)
			{
				StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)base[type];
				if (typeRecord == null)
				{
					typeRecord = new StaticPropertyChangedEventManager.TypeRecord(type, this);
					base[type] = typeRecord;
					typeRecord.StartListening();
				}
				typeRecord.AddHandler(handler, propertyName);
			}
		}

		// Token: 0x060076DA RID: 30426 RVA: 0x0021F75C File Offset: 0x0021D95C
		private void PrivateRemoveHandler(Type type, EventHandler<PropertyChangedEventArgs> handler, string propertyName)
		{
			using (base.WriteLock)
			{
				StaticPropertyChangedEventManager.TypeRecord typeRecord = (StaticPropertyChangedEventManager.TypeRecord)base[type];
				if (typeRecord != null)
				{
					typeRecord.RemoveHandler(handler, propertyName);
					if (typeRecord.IsEmpty)
					{
						typeRecord.StopListening();
						base.Remove(typeRecord.Type);
					}
				}
			}
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x0021F7C0 File Offset: 0x0021D9C0
		private void OnStaticPropertyChanged(StaticPropertyChangedEventManager.TypeRecord typeRecord, PropertyChangedEventArgs args)
		{
			WeakEventManager.ListenerList listenerList;
			using (base.ReadLock)
			{
				listenerList = typeRecord.GetListenerList(args.PropertyName);
				listenerList.BeginUse();
			}
			try
			{
				base.DeliverEventToList(null, args, listenerList);
			}
			finally
			{
				listenerList.EndUse();
			}
			if (listenerList == typeRecord.ProposedAllListenersList)
			{
				using (base.WriteLock)
				{
					typeRecord.StoreAllListenersList((WeakEventManager.ListenerList<PropertyChangedEventArgs>)listenerList);
				}
			}
		}

		// Token: 0x04003896 RID: 14486
		private static readonly string AllListenersKey = "<All Listeners>";

		// Token: 0x04003897 RID: 14487
		private static readonly string StaticPropertyChanged = "StaticPropertyChanged";

		// Token: 0x02000B60 RID: 2912
		private class TypeRecord
		{
			// Token: 0x06008DDD RID: 36317 RVA: 0x0025A474 File Offset: 0x00258674
			public TypeRecord(Type type, StaticPropertyChangedEventManager manager)
			{
				this._type = type;
				this._manager = manager;
				this._dict = new HybridDictionary(true);
			}

			// Token: 0x17001F89 RID: 8073
			// (get) Token: 0x06008DDE RID: 36318 RVA: 0x0025A4A1 File Offset: 0x002586A1
			public Type Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x17001F8A RID: 8074
			// (get) Token: 0x06008DDF RID: 36319 RVA: 0x0025A4A9 File Offset: 0x002586A9
			public bool IsEmpty
			{
				get
				{
					return this._dict.Count == 0;
				}
			}

			// Token: 0x17001F8B RID: 8075
			// (get) Token: 0x06008DE0 RID: 36320 RVA: 0x0025A4B9 File Offset: 0x002586B9
			public WeakEventManager.ListenerList ProposedAllListenersList
			{
				get
				{
					return this._proposedAllListenersList;
				}
			}

			// Token: 0x17001F8C RID: 8076
			// (get) Token: 0x06008DE1 RID: 36321 RVA: 0x0025A4C1 File Offset: 0x002586C1
			private static MethodInfo OnStaticPropertyChangedMethodInfo
			{
				get
				{
					return typeof(StaticPropertyChangedEventManager.TypeRecord).GetMethod("OnStaticPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
				}
			}

			// Token: 0x06008DE2 RID: 36322 RVA: 0x0025A4DC File Offset: 0x002586DC
			public void StartListening()
			{
				EventInfo @event = this._type.GetEvent(StaticPropertyChangedEventManager.StaticPropertyChanged, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.TypeRecord.OnStaticPropertyChangedMethodInfo);
					@event.AddEventHandler(null, handler);
				}
			}

			// Token: 0x06008DE3 RID: 36323 RVA: 0x0025A520 File Offset: 0x00258720
			public void StopListening()
			{
				EventInfo @event = this._type.GetEvent(StaticPropertyChangedEventManager.StaticPropertyChanged, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.TypeRecord.OnStaticPropertyChangedMethodInfo);
					@event.RemoveEventHandler(null, handler);
				}
			}

			// Token: 0x06008DE4 RID: 36324 RVA: 0x0025A563 File Offset: 0x00258763
			private void OnStaticPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				this.HandleStaticPropertyChanged(e);
			}

			// Token: 0x06008DE5 RID: 36325 RVA: 0x0025A56C File Offset: 0x0025876C
			public void HandleStaticPropertyChanged(PropertyChangedEventArgs e)
			{
				this._manager.OnStaticPropertyChanged(this, e);
			}

			// Token: 0x06008DE6 RID: 36326 RVA: 0x0025A57C File Offset: 0x0025877C
			public void AddHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
			{
				StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
				if (propertyRecord == null)
				{
					propertyRecord = new StaticPropertyChangedEventManager.PropertyRecord(propertyName, this);
					this._dict[propertyName] = propertyRecord;
					propertyRecord.StartListening(this._type);
				}
				propertyRecord.AddHandler(handler);
				this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
				this._proposedAllListenersList = null;
				this._manager.ScheduleCleanup();
			}

			// Token: 0x06008DE7 RID: 36327 RVA: 0x0025A5E8 File Offset: 0x002587E8
			public void RemoveHandler(EventHandler<PropertyChangedEventArgs> handler, string propertyName)
			{
				StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
				if (propertyRecord != null)
				{
					propertyRecord.RemoveHandler(handler);
					if (propertyRecord.IsEmpty)
					{
						this._dict.Remove(propertyName);
					}
					this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
					this._proposedAllListenersList = null;
				}
			}

			// Token: 0x06008DE8 RID: 36328 RVA: 0x0025A63C File Offset: 0x0025883C
			public WeakEventManager.ListenerList GetListenerList(string propertyName)
			{
				WeakEventManager.ListenerList listenerList3;
				if (!string.IsNullOrEmpty(propertyName))
				{
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[propertyName];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList = (propertyRecord == null) ? null : propertyRecord.List;
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord2 = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[string.Empty];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList2 = (propertyRecord2 == null) ? null : propertyRecord2.List;
					if (listenerList2 == null)
					{
						if (listenerList != null)
						{
							listenerList3 = listenerList;
						}
						else
						{
							listenerList3 = WeakEventManager.ListenerList.Empty;
						}
					}
					else if (listenerList != null)
					{
						listenerList3 = new WeakEventManager.ListenerList<PropertyChangedEventArgs>(listenerList.Count + listenerList2.Count);
						int i = 0;
						int count = listenerList.Count;
						while (i < count)
						{
							listenerList3.Add(listenerList[i]);
							i++;
						}
						int j = 0;
						int count2 = listenerList2.Count;
						while (j < count2)
						{
							listenerList3.Add(listenerList2[j]);
							j++;
						}
					}
					else
					{
						listenerList3 = listenerList2;
					}
				}
				else
				{
					StaticPropertyChangedEventManager.PropertyRecord propertyRecord3 = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[StaticPropertyChangedEventManager.AllListenersKey];
					WeakEventManager.ListenerList<PropertyChangedEventArgs> listenerList4 = (propertyRecord3 == null) ? null : propertyRecord3.List;
					if (listenerList4 == null)
					{
						int num = 0;
						foreach (object obj in this._dict)
						{
							num += ((StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj).Value).List.Count;
						}
						listenerList4 = new WeakEventManager.ListenerList<PropertyChangedEventArgs>(num);
						foreach (object obj2 in this._dict)
						{
							WeakEventManager.ListenerList list = ((StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj2).Value).List;
							int k = 0;
							int count3 = list.Count;
							while (k < count3)
							{
								listenerList4.Add(list.GetListener(k));
								k++;
							}
						}
						this._proposedAllListenersList = listenerList4;
					}
					listenerList3 = listenerList4;
				}
				return listenerList3;
			}

			// Token: 0x06008DE9 RID: 36329 RVA: 0x0025A858 File Offset: 0x00258A58
			public void StoreAllListenersList(WeakEventManager.ListenerList<PropertyChangedEventArgs> list)
			{
				if (this._proposedAllListenersList == list)
				{
					this._dict[StaticPropertyChangedEventManager.AllListenersKey] = new StaticPropertyChangedEventManager.PropertyRecord(StaticPropertyChangedEventManager.AllListenersKey, this, list);
					this._proposedAllListenersList = null;
				}
			}

			// Token: 0x06008DEA RID: 36330 RVA: 0x0025A888 File Offset: 0x00258A88
			public bool Purge(bool purgeAll)
			{
				bool flag = false;
				if (!purgeAll)
				{
					if (!BaseAppContextSwitches.EnableWeakEventMemoryImprovements)
					{
						ICollection keys = this._dict.Keys;
						string[] array = new string[keys.Count];
						keys.CopyTo(array, 0);
						for (int i = array.Length - 1; i >= 0; i--)
						{
							if (!(array[i] == StaticPropertyChangedEventManager.AllListenersKey))
							{
								StaticPropertyChangedEventManager.PropertyRecord propertyRecord = (StaticPropertyChangedEventManager.PropertyRecord)this._dict[array[i]];
								if (propertyRecord.Purge())
								{
									flag = true;
								}
								if (propertyRecord.IsEmpty)
								{
									propertyRecord.StopListening(this._type);
									this._dict.Remove(array[i]);
								}
							}
						}
					}
					else
					{
						IDictionaryEnumerator enumerator = this._dict.GetEnumerator();
						while (enumerator.MoveNext())
						{
							string text = (string)enumerator.Key;
							if (!(text == StaticPropertyChangedEventManager.AllListenersKey))
							{
								StaticPropertyChangedEventManager.PropertyRecord propertyRecord2 = (StaticPropertyChangedEventManager.PropertyRecord)enumerator.Value;
								if (propertyRecord2.Purge())
								{
									flag = true;
								}
								if (propertyRecord2.IsEmpty)
								{
									propertyRecord2.StopListening(this._type);
									this._toRemove.Add(text);
								}
							}
						}
						if (this._toRemove.Count > 0)
						{
							foreach (string key in this._toRemove)
							{
								this._dict.Remove(key);
							}
							this._toRemove.Clear();
							this._toRemove.TrimExcess();
						}
					}
					if (flag)
					{
						this._dict.Remove(StaticPropertyChangedEventManager.AllListenersKey);
						this._proposedAllListenersList = null;
					}
					if (this.IsEmpty)
					{
						this.StopListening();
					}
				}
				else
				{
					flag = true;
					this.StopListening();
					foreach (object obj in this._dict)
					{
						StaticPropertyChangedEventManager.PropertyRecord propertyRecord3 = (StaticPropertyChangedEventManager.PropertyRecord)((DictionaryEntry)obj).Value;
						propertyRecord3.StopListening(this._type);
					}
				}
				return flag;
			}

			// Token: 0x04004B1C RID: 19228
			private Type _type;

			// Token: 0x04004B1D RID: 19229
			private HybridDictionary _dict;

			// Token: 0x04004B1E RID: 19230
			private StaticPropertyChangedEventManager _manager;

			// Token: 0x04004B1F RID: 19231
			private WeakEventManager.ListenerList<PropertyChangedEventArgs> _proposedAllListenersList;

			// Token: 0x04004B20 RID: 19232
			private List<string> _toRemove = new List<string>();
		}

		// Token: 0x02000B61 RID: 2913
		private class PropertyRecord
		{
			// Token: 0x06008DEB RID: 36331 RVA: 0x0025AAA8 File Offset: 0x00258CA8
			public PropertyRecord(string propertyName, StaticPropertyChangedEventManager.TypeRecord owner) : this(propertyName, owner, new WeakEventManager.ListenerList<PropertyChangedEventArgs>())
			{
			}

			// Token: 0x06008DEC RID: 36332 RVA: 0x0025AAB7 File Offset: 0x00258CB7
			public PropertyRecord(string propertyName, StaticPropertyChangedEventManager.TypeRecord owner, WeakEventManager.ListenerList<PropertyChangedEventArgs> list)
			{
				this._propertyName = propertyName;
				this._typeRecord = owner;
				this._list = list;
			}

			// Token: 0x17001F8D RID: 8077
			// (get) Token: 0x06008DED RID: 36333 RVA: 0x0025AAD4 File Offset: 0x00258CD4
			public bool IsEmpty
			{
				get
				{
					return this._list.IsEmpty;
				}
			}

			// Token: 0x17001F8E RID: 8078
			// (get) Token: 0x06008DEE RID: 36334 RVA: 0x0025AAE1 File Offset: 0x00258CE1
			public WeakEventManager.ListenerList<PropertyChangedEventArgs> List
			{
				get
				{
					return this._list;
				}
			}

			// Token: 0x17001F8F RID: 8079
			// (get) Token: 0x06008DEF RID: 36335 RVA: 0x0025AAE9 File Offset: 0x00258CE9
			private static MethodInfo OnStaticPropertyChangedMethodInfo
			{
				get
				{
					return typeof(StaticPropertyChangedEventManager.PropertyRecord).GetMethod("OnStaticPropertyChanged", BindingFlags.Instance | BindingFlags.NonPublic);
				}
			}

			// Token: 0x06008DF0 RID: 36336 RVA: 0x0025AB04 File Offset: 0x00258D04
			public void StartListening(Type type)
			{
				string name = this._propertyName + "Changed";
				EventInfo @event = type.GetEvent(name, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.PropertyRecord.OnStaticPropertyChangedMethodInfo);
					@event.AddEventHandler(null, handler);
				}
			}

			// Token: 0x06008DF1 RID: 36337 RVA: 0x0025AB50 File Offset: 0x00258D50
			public void StopListening(Type type)
			{
				string name = this._propertyName + "Changed";
				EventInfo @event = type.GetEvent(name, BindingFlags.Static | BindingFlags.Public);
				if (@event != null)
				{
					Delegate handler = Delegate.CreateDelegate(@event.EventHandlerType, this, StaticPropertyChangedEventManager.PropertyRecord.OnStaticPropertyChangedMethodInfo);
					@event.RemoveEventHandler(null, handler);
				}
			}

			// Token: 0x06008DF2 RID: 36338 RVA: 0x0025AB9B File Offset: 0x00258D9B
			private void OnStaticPropertyChanged(object sender, EventArgs e)
			{
				this._typeRecord.HandleStaticPropertyChanged(new PropertyChangedEventArgs(this._propertyName));
			}

			// Token: 0x06008DF3 RID: 36339 RVA: 0x0025ABB4 File Offset: 0x00258DB4
			public void AddHandler(EventHandler<PropertyChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				this._list.AddHandler(handler);
			}

			// Token: 0x06008DF4 RID: 36340 RVA: 0x0025ABEC File Offset: 0x00258DEC
			public void RemoveHandler(EventHandler<PropertyChangedEventArgs> handler)
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				this._list.RemoveHandler(handler);
			}

			// Token: 0x06008DF5 RID: 36341 RVA: 0x0025AC24 File Offset: 0x00258E24
			public bool Purge()
			{
				WeakEventManager.ListenerList list = this._list;
				if (WeakEventManager.ListenerList.PrepareForWriting(ref list))
				{
					this._list = (WeakEventManager.ListenerList<PropertyChangedEventArgs>)list;
				}
				return this._list.Purge();
			}

			// Token: 0x04004B21 RID: 19233
			private string _propertyName;

			// Token: 0x04004B22 RID: 19234
			private WeakEventManager.ListenerList<PropertyChangedEventArgs> _list;

			// Token: 0x04004B23 RID: 19235
			private StaticPropertyChangedEventManager.TypeRecord _typeRecord;
		}
	}
}
