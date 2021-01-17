using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000715 RID: 1813
	internal class DataBindEngine : DispatcherObject
	{
		// Token: 0x060074A5 RID: 29861 RVA: 0x00215ED0 File Offset: 0x002140D0
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private DataBindEngine()
		{
			DataBindEngine.DataBindEngineShutDownListener dataBindEngineShutDownListener = new DataBindEngine.DataBindEngineShutDownListener(this);
			this._head = new DataBindEngine.Task(null, TaskOps.TransferValue, null);
			this._tail = this._head;
			this._mostRecentTask = new HybridDictionary();
			this._cleanupHelper = new CleanupHelper(new Func<bool, bool>(this.DoCleanup), 400, 10000, 5000);
		}

		// Token: 0x17001BC1 RID: 7105
		// (get) Token: 0x060074A6 RID: 29862 RVA: 0x00215F9F File Offset: 0x0021419F
		internal PathParser PathParser
		{
			get
			{
				return this._pathParser;
			}
		}

		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x060074A7 RID: 29863 RVA: 0x00215FA7 File Offset: 0x002141A7
		internal ValueConverterContext ValueConverterContext
		{
			get
			{
				return this._valueConverterContext;
			}
		}

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x060074A8 RID: 29864 RVA: 0x00215FAF File Offset: 0x002141AF
		internal AccessorTable AccessorTable
		{
			get
			{
				return this._accessorTable;
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x060074A9 RID: 29865 RVA: 0x00215FB7 File Offset: 0x002141B7
		internal bool IsShutDown
		{
			get
			{
				return this._viewManager == null;
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x060074AA RID: 29866 RVA: 0x00215FC2 File Offset: 0x002141C2
		// (set) Token: 0x060074AB RID: 29867 RVA: 0x00215FCA File Offset: 0x002141CA
		internal bool CleanupEnabled
		{
			get
			{
				return this._cleanupEnabled;
			}
			set
			{
				this._cleanupEnabled = value;
				WeakEventManager.SetCleanupEnabled(value);
			}
		}

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x060074AC RID: 29868 RVA: 0x00215FD9 File Offset: 0x002141D9
		internal IAsyncDataDispatcher AsyncDataDispatcher
		{
			get
			{
				if (this._defaultAsyncDataDispatcher == null)
				{
					this._defaultAsyncDataDispatcher = new DefaultAsyncDataDispatcher();
				}
				return this._defaultAsyncDataDispatcher;
			}
		}

		// Token: 0x17001BC7 RID: 7111
		// (get) Token: 0x060074AD RID: 29869 RVA: 0x00215FF4 File Offset: 0x002141F4
		internal static DataBindEngine CurrentDataBindEngine
		{
			get
			{
				if (DataBindEngine._currentEngine == null)
				{
					DataBindEngine._currentEngine = new DataBindEngine();
				}
				return DataBindEngine._currentEngine;
			}
		}

		// Token: 0x17001BC8 RID: 7112
		// (get) Token: 0x060074AE RID: 29870 RVA: 0x0021600C File Offset: 0x0021420C
		internal ViewManager ViewManager
		{
			get
			{
				return this._viewManager;
			}
		}

		// Token: 0x17001BC9 RID: 7113
		// (get) Token: 0x060074AF RID: 29871 RVA: 0x00216014 File Offset: 0x00214214
		internal CommitManager CommitManager
		{
			get
			{
				if (!this._commitManager.IsEmpty)
				{
					this.ScheduleCleanup();
				}
				return this._commitManager;
			}
		}

		// Token: 0x060074B0 RID: 29872 RVA: 0x00216030 File Offset: 0x00214230
		internal void AddTask(IDataBindEngineClient c, TaskOps op)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			if (this._head == this._tail)
			{
				this.RequestRun();
			}
			DataBindEngine.Task previousForClient = (DataBindEngine.Task)this._mostRecentTask[c];
			DataBindEngine.Task task = new DataBindEngine.Task(c, op, previousForClient);
			this._tail.Next = task;
			this._tail = task;
			this._mostRecentTask[c] = task;
			if (op == TaskOps.AttachToContext && this._layoutElement == null && (this._layoutElement = (c.TargetElement as UIElement)) != null)
			{
				this._layoutElement.LayoutUpdated += this.OnLayoutUpdated;
			}
		}

		// Token: 0x060074B1 RID: 29873 RVA: 0x002160D0 File Offset: 0x002142D0
		internal void CancelTask(IDataBindEngineClient c, TaskOps op)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			for (DataBindEngine.Task task = (DataBindEngine.Task)this._mostRecentTask[c]; task != null; task = task.PreviousForClient)
			{
				if (task.op == op && task.status == DataBindEngine.Task.Status.Pending)
				{
					task.status = DataBindEngine.Task.Status.Cancelled;
					return;
				}
			}
		}

		// Token: 0x060074B2 RID: 29874 RVA: 0x00216120 File Offset: 0x00214320
		internal void CancelTasks(IDataBindEngineClient c)
		{
			if (this._mostRecentTask == null)
			{
				return;
			}
			for (DataBindEngine.Task task = (DataBindEngine.Task)this._mostRecentTask[c]; task != null; task = task.PreviousForClient)
			{
				Invariant.Assert(task.client == c, "task list is corrupt");
				if (task.status == DataBindEngine.Task.Status.Pending)
				{
					task.status = DataBindEngine.Task.Status.Cancelled;
				}
			}
			this._mostRecentTask.Remove(c);
		}

		// Token: 0x060074B3 RID: 29875 RVA: 0x00216184 File Offset: 0x00214384
		internal object Run(object arg)
		{
			bool flag = (bool)arg;
			DataBindEngine.Task task = flag ? null : new DataBindEngine.Task(null, TaskOps.TransferValue, null);
			DataBindEngine.Task task2 = task;
			if (this._layoutElement != null)
			{
				this._layoutElement.LayoutUpdated -= this.OnLayoutUpdated;
				this._layoutElement = null;
			}
			if (this.IsShutDown)
			{
				return null;
			}
			DataBindEngine.Task next;
			for (DataBindEngine.Task task3 = this._head.Next; task3 != null; task3 = next)
			{
				task3.PreviousForClient = null;
				if (task3.status == DataBindEngine.Task.Status.Pending)
				{
					task3.Run(flag);
					next = task3.Next;
					if (task3.status == DataBindEngine.Task.Status.Retry && !flag)
					{
						task3.status = DataBindEngine.Task.Status.Pending;
						task2.Next = task3;
						task2 = task3;
						task2.Next = null;
					}
				}
				else
				{
					next = task3.Next;
				}
			}
			this._head.Next = null;
			this._tail = this._head;
			this._mostRecentTask.Clear();
			if (!flag)
			{
				DataBindEngine.Task head = this._head;
				this._head = null;
				for (DataBindEngine.Task next2 = task.Next; next2 != null; next2 = next2.Next)
				{
					this.AddTask(next2.client, next2.op);
				}
				this._head = head;
			}
			return null;
		}

		// Token: 0x060074B4 RID: 29876 RVA: 0x002162AC File Offset: 0x002144AC
		internal ViewRecord GetViewRecord(object collection, CollectionViewSource key, Type collectionViewType, bool createView, Func<object, object> GetSourceItem)
		{
			if (this.IsShutDown)
			{
				return null;
			}
			ViewRecord viewRecord = this._viewManager.GetViewRecord(collection, key, collectionViewType, createView, GetSourceItem);
			if (viewRecord != null && !viewRecord.IsInitialized)
			{
				this.ScheduleCleanup();
			}
			return viewRecord;
		}

		// Token: 0x060074B5 RID: 29877 RVA: 0x002162E8 File Offset: 0x002144E8
		internal void RegisterCollectionSynchronizationCallback(IEnumerable collection, object context, CollectionSynchronizationCallback synchronizationCallback)
		{
			this._viewManager.RegisterCollectionSynchronizationCallback(collection, context, synchronizationCallback);
		}

		// Token: 0x060074B6 RID: 29878 RVA: 0x002162F8 File Offset: 0x002144F8
		internal IValueConverter GetDefaultValueConverter(Type sourceType, Type targetType, bool targetToSource)
		{
			IValueConverter valueConverter = this._valueConverterTable[sourceType, targetType, targetToSource];
			if (valueConverter == null)
			{
				valueConverter = DefaultValueConverter.Create(sourceType, targetType, targetToSource, this);
				if (valueConverter != null)
				{
					this._valueConverterTable.Add(sourceType, targetType, targetToSource, valueConverter);
				}
			}
			return valueConverter;
		}

		// Token: 0x060074B7 RID: 29879 RVA: 0x00216334 File Offset: 0x00214534
		internal void AddAsyncRequest(DependencyObject target, AsyncDataRequest request)
		{
			if (target == null)
			{
				return;
			}
			IAsyncDataDispatcher asyncDataDispatcher = this.AsyncDataDispatcher;
			if (this._asyncDispatchers == null)
			{
				this._asyncDispatchers = new HybridDictionary(1);
			}
			this._asyncDispatchers[asyncDataDispatcher] = null;
			asyncDataDispatcher.AddRequest(request);
		}

		// Token: 0x060074B8 RID: 29880 RVA: 0x00216374 File Offset: 0x00214574
		internal object GetValue(object item, PropertyDescriptor pd, bool indexerIsNext)
		{
			return this._valueTable.GetValue(item, pd, indexerIsNext);
		}

		// Token: 0x060074B9 RID: 29881 RVA: 0x00216384 File Offset: 0x00214584
		internal void RegisterForCacheChanges(object item, object descriptor)
		{
			PropertyDescriptor propertyDescriptor = descriptor as PropertyDescriptor;
			if (item != null && propertyDescriptor != null && ValueTable.ShouldCache(item, propertyDescriptor))
			{
				this._valueTable.RegisterForChanges(item, propertyDescriptor, this);
			}
		}

		// Token: 0x060074BA RID: 29882 RVA: 0x002163B5 File Offset: 0x002145B5
		internal void ScheduleCleanup()
		{
			if (!BaseAppContextSwitches.EnableCleanupSchedulingImprovements)
			{
				if (Interlocked.Increment(ref this._cleanupRequests) == 1)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new DispatcherOperationCallback(this.CleanupOperation), null);
					return;
				}
			}
			else
			{
				this._cleanupHelper.ScheduleCleanup();
			}
		}

		// Token: 0x060074BB RID: 29883 RVA: 0x002163F2 File Offset: 0x002145F2
		private bool DoCleanup(bool forceCleanup)
		{
			return (this.CleanupEnabled || forceCleanup) && this.DoCleanup();
		}

		// Token: 0x060074BC RID: 29884 RVA: 0x00216406 File Offset: 0x00214606
		internal bool Cleanup()
		{
			if (!BaseAppContextSwitches.EnableCleanupSchedulingImprovements)
			{
				return this.DoCleanup();
			}
			return this._cleanupHelper.DoCleanup(true);
		}

		// Token: 0x060074BD RID: 29885 RVA: 0x00216424 File Offset: 0x00214624
		private bool DoCleanup()
		{
			bool flag = false;
			if (!this.IsShutDown)
			{
				flag = (this._viewManager.Purge() || flag);
				flag = (WeakEventManager.Cleanup() || flag);
				flag = (this._valueTable.Purge() || flag);
				flag = (this._commitManager.Purge() || flag);
			}
			return flag;
		}

		// Token: 0x060074BE RID: 29886 RVA: 0x00216470 File Offset: 0x00214670
		internal DataBindOperation Marshal(DispatcherOperationCallback method, object arg, int cost = 1)
		{
			DataBindOperation dataBindOperation = new DataBindOperation(method, arg, cost);
			object crossThreadQueueLock = this._crossThreadQueueLock;
			lock (crossThreadQueueLock)
			{
				this._crossThreadQueue.Enqueue(dataBindOperation);
				this._crossThreadCost += cost;
				if (this._crossThreadDispatcherOperation == null)
				{
					this._crossThreadDispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.ProcessCrossThreadRequests));
				}
			}
			return dataBindOperation;
		}

		// Token: 0x060074BF RID: 29887 RVA: 0x002164F4 File Offset: 0x002146F4
		internal void ChangeCost(DataBindOperation op, int delta)
		{
			object crossThreadQueueLock = this._crossThreadQueueLock;
			lock (crossThreadQueueLock)
			{
				op.Cost += delta;
				this._crossThreadCost += delta;
			}
		}

		// Token: 0x060074C0 RID: 29888 RVA: 0x0021654C File Offset: 0x0021474C
		private void ProcessCrossThreadRequests()
		{
			if (this.IsShutDown)
			{
				return;
			}
			try
			{
				long ticks = DateTime.Now.Ticks;
				do
				{
					object crossThreadQueueLock = this._crossThreadQueueLock;
					DataBindOperation dataBindOperation;
					lock (crossThreadQueueLock)
					{
						if (this._crossThreadQueue.Count > 0)
						{
							dataBindOperation = this._crossThreadQueue.Dequeue();
							this._crossThreadCost -= dataBindOperation.Cost;
						}
						else
						{
							dataBindOperation = null;
						}
					}
					if (dataBindOperation == null)
					{
						break;
					}
					dataBindOperation.Invoke();
				}
				while (DateTime.Now.Ticks - ticks <= 50000L);
			}
			finally
			{
				object crossThreadQueueLock2 = this._crossThreadQueueLock;
				lock (crossThreadQueueLock2)
				{
					if (this._crossThreadQueue.Count > 0)
					{
						this._crossThreadDispatcherOperation = base.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(this.ProcessCrossThreadRequests));
					}
					else
					{
						this._crossThreadDispatcherOperation = null;
						this._crossThreadCost = 0;
					}
				}
			}
		}

		// Token: 0x060074C1 RID: 29889 RVA: 0x0021666C File Offset: 0x0021486C
		private void RequestRun()
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new DispatcherOperationCallback(this.Run), false);
			base.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new DispatcherOperationCallback(this.Run), true);
		}

		// Token: 0x060074C2 RID: 29890 RVA: 0x002166AC File Offset: 0x002148AC
		private object CleanupOperation(object arg)
		{
			Interlocked.Exchange(ref this._cleanupRequests, 0);
			if (!this._cleanupEnabled)
			{
				return null;
			}
			this.Cleanup();
			return null;
		}

		// Token: 0x060074C3 RID: 29891 RVA: 0x002166D0 File Offset: 0x002148D0
		private void OnShutDown()
		{
			this._viewManager = null;
			this._commitManager = null;
			this._valueConverterTable = null;
			this._mostRecentTask = null;
			this._head = (this._tail = null);
			this._crossThreadQueue.Clear();
			HybridDictionary hybridDictionary = Interlocked.Exchange<HybridDictionary>(ref this._asyncDispatchers, null);
			if (hybridDictionary != null)
			{
				foreach (object obj in hybridDictionary.Keys)
				{
					IAsyncDataDispatcher asyncDataDispatcher = obj as IAsyncDataDispatcher;
					if (asyncDataDispatcher != null)
					{
						asyncDataDispatcher.CancelAllRequests();
					}
				}
			}
			this._defaultAsyncDataDispatcher = null;
		}

		// Token: 0x060074C4 RID: 29892 RVA: 0x00216784 File Offset: 0x00214984
		private void OnLayoutUpdated(object sender, EventArgs e)
		{
			this.Run(false);
		}

		// Token: 0x040037E2 RID: 14306
		private HybridDictionary _mostRecentTask;

		// Token: 0x040037E3 RID: 14307
		private DataBindEngine.Task _head;

		// Token: 0x040037E4 RID: 14308
		private DataBindEngine.Task _tail;

		// Token: 0x040037E5 RID: 14309
		private UIElement _layoutElement;

		// Token: 0x040037E6 RID: 14310
		private ViewManager _viewManager = new ViewManager();

		// Token: 0x040037E7 RID: 14311
		private CommitManager _commitManager = new CommitManager();

		// Token: 0x040037E8 RID: 14312
		private DataBindEngine.ValueConverterTable _valueConverterTable = new DataBindEngine.ValueConverterTable();

		// Token: 0x040037E9 RID: 14313
		private PathParser _pathParser = new PathParser();

		// Token: 0x040037EA RID: 14314
		private IAsyncDataDispatcher _defaultAsyncDataDispatcher;

		// Token: 0x040037EB RID: 14315
		private HybridDictionary _asyncDispatchers;

		// Token: 0x040037EC RID: 14316
		private ValueConverterContext _valueConverterContext = new ValueConverterContext();

		// Token: 0x040037ED RID: 14317
		private bool _cleanupEnabled = true;

		// Token: 0x040037EE RID: 14318
		private ValueTable _valueTable = new ValueTable();

		// Token: 0x040037EF RID: 14319
		private AccessorTable _accessorTable = new AccessorTable();

		// Token: 0x040037F0 RID: 14320
		private int _cleanupRequests;

		// Token: 0x040037F1 RID: 14321
		private CleanupHelper _cleanupHelper;

		// Token: 0x040037F2 RID: 14322
		private Queue<DataBindOperation> _crossThreadQueue = new Queue<DataBindOperation>();

		// Token: 0x040037F3 RID: 14323
		private object _crossThreadQueueLock = new object();

		// Token: 0x040037F4 RID: 14324
		private int _crossThreadCost;

		// Token: 0x040037F5 RID: 14325
		private DispatcherOperation _crossThreadDispatcherOperation;

		// Token: 0x040037F6 RID: 14326
		internal const int CrossThreadThreshold = 50000;

		// Token: 0x040037F7 RID: 14327
		[ThreadStatic]
		private static DataBindEngine _currentEngine;

		// Token: 0x02000B49 RID: 2889
		private class Task
		{
			// Token: 0x06008DA5 RID: 36261 RVA: 0x00259CB6 File Offset: 0x00257EB6
			public Task(IDataBindEngineClient c, TaskOps o, DataBindEngine.Task previousForClient)
			{
				this.client = c;
				this.op = o;
				this.PreviousForClient = previousForClient;
				this.status = DataBindEngine.Task.Status.Pending;
			}

			// Token: 0x06008DA6 RID: 36262 RVA: 0x00259CDC File Offset: 0x00257EDC
			public void Run(bool lastChance)
			{
				this.status = DataBindEngine.Task.Status.Running;
				DataBindEngine.Task.Status status = DataBindEngine.Task.Status.Completed;
				switch (this.op)
				{
				case TaskOps.TransferValue:
					this.client.TransferValue();
					break;
				case TaskOps.UpdateValue:
					this.client.UpdateValue();
					break;
				case TaskOps.AttachToContext:
					if (!this.client.AttachToContext(lastChance) && !lastChance)
					{
						status = DataBindEngine.Task.Status.Retry;
					}
					break;
				case TaskOps.VerifySourceReference:
					this.client.VerifySourceReference(lastChance);
					break;
				case TaskOps.RaiseTargetUpdatedEvent:
					this.client.OnTargetUpdated();
					break;
				}
				this.status = status;
			}

			// Token: 0x04004AD8 RID: 19160
			public IDataBindEngineClient client;

			// Token: 0x04004AD9 RID: 19161
			public TaskOps op;

			// Token: 0x04004ADA RID: 19162
			public DataBindEngine.Task.Status status;

			// Token: 0x04004ADB RID: 19163
			public DataBindEngine.Task Next;

			// Token: 0x04004ADC RID: 19164
			public DataBindEngine.Task PreviousForClient;

			// Token: 0x02000BBB RID: 3003
			public enum Status
			{
				// Token: 0x04004F00 RID: 20224
				Pending,
				// Token: 0x04004F01 RID: 20225
				Running,
				// Token: 0x04004F02 RID: 20226
				Completed,
				// Token: 0x04004F03 RID: 20227
				Retry,
				// Token: 0x04004F04 RID: 20228
				Cancelled
			}
		}

		// Token: 0x02000B4A RID: 2890
		private class ValueConverterTable : Hashtable
		{
			// Token: 0x17001F7F RID: 8063
			public IValueConverter this[Type sourceType, Type targetType, bool targetToSource]
			{
				get
				{
					DataBindEngine.ValueConverterTable.Key key = new DataBindEngine.ValueConverterTable.Key(sourceType, targetType, targetToSource);
					object obj = base[key];
					return (IValueConverter)obj;
				}
			}

			// Token: 0x06008DA8 RID: 36264 RVA: 0x00259D92 File Offset: 0x00257F92
			public void Add(Type sourceType, Type targetType, bool targetToSource, IValueConverter value)
			{
				base.Add(new DataBindEngine.ValueConverterTable.Key(sourceType, targetType, targetToSource), value);
			}

			// Token: 0x02000BBC RID: 3004
			private struct Key
			{
				// Token: 0x060091FE RID: 37374 RVA: 0x0025F6A2 File Offset: 0x0025D8A2
				public Key(Type sourceType, Type targetType, bool targetToSource)
				{
					this._sourceType = sourceType;
					this._targetType = targetType;
					this._targetToSource = targetToSource;
				}

				// Token: 0x060091FF RID: 37375 RVA: 0x0025F6B9 File Offset: 0x0025D8B9
				public override int GetHashCode()
				{
					return this._sourceType.GetHashCode() + this._targetType.GetHashCode();
				}

				// Token: 0x06009200 RID: 37376 RVA: 0x0025F6D2 File Offset: 0x0025D8D2
				public override bool Equals(object o)
				{
					return o is DataBindEngine.ValueConverterTable.Key && this == (DataBindEngine.ValueConverterTable.Key)o;
				}

				// Token: 0x06009201 RID: 37377 RVA: 0x0025F6EF File Offset: 0x0025D8EF
				public static bool operator ==(DataBindEngine.ValueConverterTable.Key k1, DataBindEngine.ValueConverterTable.Key k2)
				{
					return k1._sourceType == k2._sourceType && k1._targetType == k2._targetType && k1._targetToSource == k2._targetToSource;
				}

				// Token: 0x06009202 RID: 37378 RVA: 0x0025F727 File Offset: 0x0025D927
				public static bool operator !=(DataBindEngine.ValueConverterTable.Key k1, DataBindEngine.ValueConverterTable.Key k2)
				{
					return !(k1 == k2);
				}

				// Token: 0x04004F05 RID: 20229
				private Type _sourceType;

				// Token: 0x04004F06 RID: 20230
				private Type _targetType;

				// Token: 0x04004F07 RID: 20231
				private bool _targetToSource;
			}
		}

		// Token: 0x02000B4B RID: 2891
		private sealed class DataBindEngineShutDownListener : ShutDownListener
		{
			// Token: 0x06008DAA RID: 36266 RVA: 0x002576B2 File Offset: 0x002558B2
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public DataBindEngineShutDownListener(DataBindEngine target) : base(target)
			{
			}

			// Token: 0x06008DAB RID: 36267 RVA: 0x00259DB4 File Offset: 0x00257FB4
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				DataBindEngine dataBindEngine = (DataBindEngine)target;
				dataBindEngine.OnShutDown();
			}
		}
	}
}
