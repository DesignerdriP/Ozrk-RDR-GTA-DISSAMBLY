using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000711 RID: 1809
	internal class ClrBindingWorker : BindingWorker
	{
		// Token: 0x06007467 RID: 29799 RVA: 0x00215050 File Offset: 0x00213250
		internal ClrBindingWorker(BindingExpression b, DataBindEngine engine) : base(b)
		{
			PropertyPath propertyPath = base.ParentBinding.Path;
			if (base.ParentBinding.XPath != null)
			{
				propertyPath = this.PrepareXmlBinding(propertyPath);
			}
			if (propertyPath == null)
			{
				propertyPath = new PropertyPath(string.Empty, new object[0]);
			}
			if (base.ParentBinding.Path == null)
			{
				base.ParentBinding.UsePath(propertyPath);
			}
			this._pathWorker = new PropertyPathWorker(propertyPath, this, base.IsDynamic, engine);
			this._pathWorker.SetTreeContext(base.ParentBindingExpression.TargetElementReference);
		}

		// Token: 0x06007468 RID: 29800 RVA: 0x002150DC File Offset: 0x002132DC
		[MethodImpl(MethodImplOptions.NoInlining)]
		private PropertyPath PrepareXmlBinding(PropertyPath path)
		{
			if (path == null)
			{
				DependencyProperty targetProperty = base.TargetProperty;
				Type propertyType = targetProperty.PropertyType;
				string path2;
				if (propertyType == typeof(object))
				{
					if (targetProperty == BindingExpressionBase.NoTargetProperty || targetProperty == Selector.SelectedValueProperty || targetProperty.OwnerType == typeof(LiveShapingList))
					{
						path2 = "/InnerText";
					}
					else if (targetProperty == FrameworkElement.DataContextProperty || targetProperty == CollectionViewSource.SourceProperty)
					{
						path2 = string.Empty;
					}
					else
					{
						path2 = "/";
					}
				}
				else if (propertyType.IsAssignableFrom(typeof(XmlDataCollection)))
				{
					path2 = string.Empty;
				}
				else
				{
					path2 = "/InnerText";
				}
				path = new PropertyPath(path2, new object[0]);
			}
			if (path.SVI.Length != 0)
			{
				base.SetValue(BindingWorker.Feature.XmlWorker, new XmlBindingWorker(this, path.SVI[0].drillIn == DrillIn.Never));
			}
			return path;
		}

		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x06007469 RID: 29801 RVA: 0x002151B7 File Offset: 0x002133B7
		internal override Type SourcePropertyType
		{
			get
			{
				return this.PW.GetType(this.PW.Length - 1);
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x0600746A RID: 29802 RVA: 0x002151D1 File Offset: 0x002133D1
		internal override bool IsDBNullValidForUpdate
		{
			get
			{
				return this.PW.IsDBNullValidForUpdate;
			}
		}

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x0600746B RID: 29803 RVA: 0x002151DE File Offset: 0x002133DE
		internal override object SourceItem
		{
			get
			{
				return this.PW.SourceItem;
			}
		}

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x0600746C RID: 29804 RVA: 0x002151EB File Offset: 0x002133EB
		internal override string SourcePropertyName
		{
			get
			{
				return this.PW.SourcePropertyName;
			}
		}

		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x0600746D RID: 29805 RVA: 0x002151F8 File Offset: 0x002133F8
		internal override bool CanUpdate
		{
			get
			{
				PropertyPathWorker pw = this.PW;
				int num = this.PW.Length - 1;
				if (num < 0)
				{
					return false;
				}
				object item = pw.GetItem(num);
				if (item == null || item == BindingExpression.NullDataItem)
				{
					return false;
				}
				object accessor = pw.GetAccessor(num);
				return accessor != null && (accessor != DependencyProperty.UnsetValue || this.XmlWorker != null);
			}
		}

		// Token: 0x0600746E RID: 29806 RVA: 0x00215254 File Offset: 0x00213454
		internal override void AttachDataItem()
		{
			object obj;
			if (this.XmlWorker == null)
			{
				obj = base.DataItem;
			}
			else
			{
				this.XmlWorker.AttachDataItem();
				obj = this.XmlWorker.RawValue();
			}
			this.PW.AttachToRootItem(obj);
			if (this.PW.Length == 0)
			{
				base.ParentBindingExpression.SetupDefaultValueConverter(obj.GetType());
			}
		}

		// Token: 0x0600746F RID: 29807 RVA: 0x002152B4 File Offset: 0x002134B4
		internal override void DetachDataItem()
		{
			this.PW.DetachFromRootItem();
			if (this.XmlWorker != null)
			{
				this.XmlWorker.DetachDataItem();
			}
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest != null)
			{
				asyncGetValueRequest.Cancel();
				base.ClearValue(BindingWorker.Feature.PendingGetValueRequest);
			}
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest != null)
			{
				asyncSetValueRequest.Cancel();
				base.ClearValue(BindingWorker.Feature.PendingSetValueRequest);
			}
		}

		// Token: 0x06007470 RID: 29808 RVA: 0x0021531C File Offset: 0x0021351C
		internal override object RawValue()
		{
			object result = this.PW.RawValue();
			this.SetStatus(this.PW.Status);
			return result;
		}

		// Token: 0x06007471 RID: 29809 RVA: 0x00215347 File Offset: 0x00213547
		internal override void RefreshValue()
		{
			this.PW.RefreshValue();
		}

		// Token: 0x06007472 RID: 29810 RVA: 0x00215354 File Offset: 0x00213554
		internal override void UpdateValue(object value)
		{
			int level = this.PW.Length - 1;
			object item = this.PW.GetItem(level);
			if (item == null || item == BindingExpression.NullDataItem)
			{
				return;
			}
			if (base.ParentBinding.IsAsync && !(this.PW.GetAccessor(level) is DependencyProperty))
			{
				this.RequestAsyncSetValue(item, value);
				return;
			}
			this.PW.SetValue(item, value);
		}

		// Token: 0x06007473 RID: 29811 RVA: 0x002153BE File Offset: 0x002135BE
		internal override void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
			if (this.XmlWorker != null)
			{
				this.XmlWorker.OnCurrentChanged(collectionView, args);
			}
			this.PW.OnCurrentChanged(collectionView);
		}

		// Token: 0x06007474 RID: 29812 RVA: 0x002153E1 File Offset: 0x002135E1
		internal override bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			return this.PW.UsesDependencyProperty(d, dp);
		}

		// Token: 0x06007475 RID: 29813 RVA: 0x002153F0 File Offset: 0x002135F0
		internal override void OnSourceInvalidation(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
			this.PW.OnDependencyPropertyChanged(d, dp, isASubPropertyChange);
		}

		// Token: 0x06007476 RID: 29814 RVA: 0x00215400 File Offset: 0x00213600
		internal override bool IsPathCurrent()
		{
			object rootItem = (this.XmlWorker == null) ? base.DataItem : this.XmlWorker.RawValue();
			return this.PW.IsPathCurrent(rootItem);
		}

		// Token: 0x17001BBA RID: 7098
		// (get) Token: 0x06007477 RID: 29815 RVA: 0x00215435 File Offset: 0x00213635
		internal bool TransfersDefaultValue
		{
			get
			{
				return base.ParentBinding.TransfersDefaultValue;
			}
		}

		// Token: 0x17001BBB RID: 7099
		// (get) Token: 0x06007478 RID: 29816 RVA: 0x00215442 File Offset: 0x00213642
		internal bool ValidatesOnNotifyDataErrors
		{
			get
			{
				return base.ParentBindingExpression.ValidatesOnNotifyDataErrors;
			}
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x0021544F File Offset: 0x0021364F
		internal void CancelPendingTasks()
		{
			base.ParentBindingExpression.CancelPendingTasks();
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x0021545C File Offset: 0x0021365C
		internal bool AsyncGet(object item, int level)
		{
			if (base.ParentBinding.IsAsync)
			{
				this.RequestAsyncGetValue(item, level);
				return true;
			}
			return false;
		}

		// Token: 0x0600747B RID: 29819 RVA: 0x00215478 File Offset: 0x00213678
		internal void ReplaceCurrentItem(ICollectionView oldCollectionView, ICollectionView newCollectionView)
		{
			if (oldCollectionView != null)
			{
				CurrentChangedEventManager.RemoveHandler(oldCollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.RemoveHandler(oldCollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
			if (newCollectionView != null)
			{
				CurrentChangedEventManager.AddHandler(newCollectionView, new EventHandler<EventArgs>(base.ParentBindingExpression.OnCurrentChanged));
				if (base.IsReflective)
				{
					CurrentChangingEventManager.AddHandler(newCollectionView, new EventHandler<CurrentChangingEventArgs>(base.ParentBindingExpression.OnCurrentChanging));
				}
			}
		}

		// Token: 0x0600747C RID: 29820 RVA: 0x002154F8 File Offset: 0x002136F8
		internal void NewValueAvailable(bool dependencySourcesChanged, bool initialValue, bool isASubPropertyChange)
		{
			this.SetStatus(this.PW.Status);
			BindingExpression parentBindingExpression = base.ParentBindingExpression;
			BindingGroup bindingGroup = parentBindingExpression.BindingGroup;
			if (bindingGroup != null)
			{
				bindingGroup.UpdateTable(parentBindingExpression);
			}
			if (dependencySourcesChanged)
			{
				this.ReplaceDependencySources();
			}
			if (base.Status != BindingStatusInternal.AsyncRequestPending)
			{
				if (!initialValue)
				{
					parentBindingExpression.ScheduleTransfer(isASubPropertyChange);
					return;
				}
				base.SetTransferIsPending(false);
			}
		}

		// Token: 0x0600747D RID: 29821 RVA: 0x00215552 File Offset: 0x00213752
		internal void SetupDefaultValueConverter(Type type)
		{
			base.ParentBindingExpression.SetupDefaultValueConverter(type);
		}

		// Token: 0x0600747E RID: 29822 RVA: 0x00215560 File Offset: 0x00213760
		internal bool IsValidValue(object value)
		{
			return base.TargetProperty.IsValidValue(value);
		}

		// Token: 0x0600747F RID: 29823 RVA: 0x00215570 File Offset: 0x00213770
		internal void OnSourcePropertyChanged(object o, string propName)
		{
			int level;
			if (!base.IgnoreSourcePropertyChange && (level = this.PW.LevelForPropertyChange(o, propName)) >= 0)
			{
				if (base.Dispatcher.Thread == Thread.CurrentThread)
				{
					this.PW.OnPropertyChangedAtLevel(level);
					return;
				}
				base.SetTransferIsPending(true);
				if (base.ParentBindingExpression.TargetWantsCrossThreadNotifications)
				{
					LiveShapingItem liveShapingItem = base.TargetElement as LiveShapingItem;
					if (liveShapingItem != null)
					{
						liveShapingItem.OnCrossThreadPropertyChange(base.TargetProperty);
					}
				}
				base.Engine.Marshal(new DispatcherOperationCallback(this.ScheduleTransferOperation), null, 1);
			}
		}

		// Token: 0x06007480 RID: 29824 RVA: 0x00215600 File Offset: 0x00213800
		internal void OnDataErrorsChanged(INotifyDataErrorInfo indei, string propName)
		{
			if (base.Dispatcher.Thread == Thread.CurrentThread)
			{
				base.ParentBindingExpression.UpdateNotifyDataErrors(indei, propName, DependencyProperty.UnsetValue);
				return;
			}
			if (!base.ParentBindingExpression.IsDataErrorsChangedPending)
			{
				base.ParentBindingExpression.IsDataErrorsChangedPending = true;
				base.Engine.Marshal(delegate(object arg)
				{
					object[] array = (object[])arg;
					base.ParentBindingExpression.UpdateNotifyDataErrors((INotifyDataErrorInfo)array[0], (string)array[1], DependencyProperty.UnsetValue);
					return null;
				}, new object[]
				{
					indei,
					propName
				}, 1);
			}
		}

		// Token: 0x06007481 RID: 29825 RVA: 0x00215674 File Offset: 0x00213874
		internal void OnXmlValueChanged()
		{
			object item = this.PW.GetItem(0);
			this.OnSourcePropertyChanged(item, null);
		}

		// Token: 0x06007482 RID: 29826 RVA: 0x00215696 File Offset: 0x00213896
		internal void UseNewXmlItem(object item)
		{
			this.PW.DetachFromRootItem();
			this.PW.AttachToRootItem(item);
			if (base.Status != BindingStatusInternal.AsyncRequestPending)
			{
				base.ParentBindingExpression.ScheduleTransfer(false);
			}
		}

		// Token: 0x06007483 RID: 29827 RVA: 0x002156C4 File Offset: 0x002138C4
		internal object GetResultNode()
		{
			return this.PW.GetItem(0);
		}

		// Token: 0x06007484 RID: 29828 RVA: 0x002156D2 File Offset: 0x002138D2
		internal DependencyObject CheckTarget()
		{
			return base.TargetElement;
		}

		// Token: 0x06007485 RID: 29829 RVA: 0x002156DC File Offset: 0x002138DC
		internal void ReportGetValueError(int k, object item, Exception ex)
		{
			if (TraceData.IsEnabled)
			{
				SourceValueInfo sourceValueInfo = this.PW.GetSourceValueInfo(k);
				Type type = this.PW.GetType(k);
				string text = (k > 0) ? this.PW.GetSourceValueInfo(k - 1).name : string.Empty;
				TraceData.Trace(base.ParentBindingExpression.TraceLevel, TraceData.CannotGetClrRawValue(new object[]
				{
					sourceValueInfo.propertyName,
					type.Name,
					text,
					AvTrace.TypeName(item)
				}), base.ParentBindingExpression, ex);
			}
		}

		// Token: 0x06007486 RID: 29830 RVA: 0x0021576C File Offset: 0x0021396C
		internal void ReportSetValueError(int k, object item, object value, Exception ex)
		{
			if (TraceData.IsEnabled)
			{
				SourceValueInfo sourceValueInfo = this.PW.GetSourceValueInfo(k);
				Type type = this.PW.GetType(k);
				TraceData.Trace(TraceEventType.Error, TraceData.CannotSetClrRawValue(new object[]
				{
					sourceValueInfo.propertyName,
					type.Name,
					AvTrace.TypeName(item),
					AvTrace.ToStringHelper(value),
					AvTrace.TypeName(value)
				}), base.ParentBindingExpression, ex);
			}
		}

		// Token: 0x06007487 RID: 29831 RVA: 0x002157E0 File Offset: 0x002139E0
		internal void ReportRawValueErrors(int k, object item, object info)
		{
			if (TraceData.IsEnabled)
			{
				if (item == null)
				{
					TraceData.Trace(TraceEventType.Information, TraceData.MissingDataItem, base.ParentBindingExpression);
				}
				if (info == null)
				{
					TraceData.Trace(TraceEventType.Information, TraceData.MissingInfo, base.ParentBindingExpression);
				}
				if (item == BindingExpression.NullDataItem)
				{
					TraceData.Trace(TraceEventType.Information, TraceData.NullDataItem, base.ParentBindingExpression);
				}
			}
		}

		// Token: 0x06007488 RID: 29832 RVA: 0x00215838 File Offset: 0x00213A38
		internal void ReportBadXPath(TraceEventType traceType)
		{
			XmlBindingWorker xmlWorker = this.XmlWorker;
			if (xmlWorker != null)
			{
				xmlWorker.ReportBadXPath(traceType);
			}
		}

		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x06007489 RID: 29833 RVA: 0x00215856 File Offset: 0x00213A56
		private PropertyPathWorker PW
		{
			get
			{
				return this._pathWorker;
			}
		}

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x0600748A RID: 29834 RVA: 0x0021585E File Offset: 0x00213A5E
		private XmlBindingWorker XmlWorker
		{
			get
			{
				return (XmlBindingWorker)base.GetValue(BindingWorker.Feature.XmlWorker, null);
			}
		}

		// Token: 0x0600748B RID: 29835 RVA: 0x0021586D File Offset: 0x00213A6D
		private void SetStatus(PropertyPathStatus status)
		{
			switch (status)
			{
			case PropertyPathStatus.Inactive:
				base.Status = BindingStatusInternal.Inactive;
				return;
			case PropertyPathStatus.Active:
				base.Status = BindingStatusInternal.Active;
				return;
			case PropertyPathStatus.PathError:
				base.Status = BindingStatusInternal.PathError;
				return;
			case PropertyPathStatus.AsyncRequestPending:
				base.Status = BindingStatusInternal.AsyncRequestPending;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600748C RID: 29836 RVA: 0x002158A8 File Offset: 0x00213AA8
		private void ReplaceDependencySources()
		{
			if (!base.ParentBindingExpression.IsDetaching)
			{
				int num = this.PW.Length;
				if (this.PW.NeedsDirectNotification)
				{
					num++;
				}
				WeakDependencySource[] array = new WeakDependencySource[num];
				int n = 0;
				if (base.IsDynamic)
				{
					for (int i = 0; i < this.PW.Length; i++)
					{
						DependencyProperty dependencyProperty = this.PW.GetAccessor(i) as DependencyProperty;
						if (dependencyProperty != null)
						{
							DependencyObject dependencyObject = this.PW.GetItem(i) as DependencyObject;
							if (dependencyObject != null)
							{
								array[n++] = new WeakDependencySource(dependencyObject, dependencyProperty);
							}
						}
					}
					if (this.PW.NeedsDirectNotification)
					{
						DependencyObject dependencyObject2 = this.PW.RawValue() as Freezable;
						if (dependencyObject2 != null)
						{
							array[n++] = new WeakDependencySource(dependencyObject2, DependencyObject.DirectDependencyProperty);
						}
					}
				}
				base.ParentBindingExpression.ChangeWorkerSources(array, n);
			}
		}

		// Token: 0x0600748D RID: 29837 RVA: 0x00215990 File Offset: 0x00213B90
		private void RequestAsyncGetValue(object item, int level)
		{
			string nameFromInfo = this.GetNameFromInfo(this.PW.GetAccessor(level));
			Invariant.Assert(nameFromInfo != null, "Async GetValue expects a name");
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest != null)
			{
				asyncGetValueRequest.Cancel();
			}
			asyncGetValueRequest = new AsyncGetValueRequest(item, nameFromInfo, base.ParentBinding.AsyncState, ClrBindingWorker.DoGetValueCallback, ClrBindingWorker.CompleteGetValueCallback, new object[]
			{
				this,
				level
			});
			base.SetValue(BindingWorker.Feature.PendingGetValueRequest, asyncGetValueRequest);
			base.Engine.AddAsyncRequest(base.TargetElement, asyncGetValueRequest);
		}

		// Token: 0x0600748E RID: 29838 RVA: 0x00215A20 File Offset: 0x00213C20
		private static object OnGetValueCallback(AsyncDataRequest adr)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncGetValueRequest.Args[0];
			object value = clrBindingWorker.PW.GetValue(asyncGetValueRequest.SourceItem, (int)asyncGetValueRequest.Args[1]);
			if (value == PropertyPathWorker.IListIndexOutOfRange)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return value;
		}

		// Token: 0x0600748F RID: 29839 RVA: 0x00215A78 File Offset: 0x00213C78
		private static object OnCompleteGetValueCallback(AsyncDataRequest adr)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncGetValueRequest.Args[0];
			DataBindEngine engine = clrBindingWorker.Engine;
			if (engine != null)
			{
				engine.Marshal(ClrBindingWorker.CompleteGetValueLocalCallback, asyncGetValueRequest, 1);
			}
			return null;
		}

		// Token: 0x06007490 RID: 29840 RVA: 0x00215AB4 File Offset: 0x00213CB4
		private static object OnCompleteGetValueOperation(object arg)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)arg;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncGetValueRequest.Args[0];
			clrBindingWorker.CompleteGetValue(asyncGetValueRequest);
			return null;
		}

		// Token: 0x06007491 RID: 29841 RVA: 0x00215AE0 File Offset: 0x00213CE0
		private void CompleteGetValue(AsyncGetValueRequest request)
		{
			AsyncGetValueRequest asyncGetValueRequest = (AsyncGetValueRequest)base.GetValue(BindingWorker.Feature.PendingGetValueRequest, null);
			if (asyncGetValueRequest == request)
			{
				base.ClearValue(BindingWorker.Feature.PendingGetValueRequest);
				int num = (int)request.Args[1];
				if (this.CheckTarget() == null)
				{
					return;
				}
				AsyncRequestStatus status = request.Status;
				if (status != AsyncRequestStatus.Completed)
				{
					if (status != AsyncRequestStatus.Failed)
					{
						return;
					}
					this.ReportGetValueError(num, request.SourceItem, request.Exception);
					this.PW.OnNewValue(num, DependencyProperty.UnsetValue);
				}
				else
				{
					this.PW.OnNewValue(num, request.Result);
					this.SetStatus(this.PW.Status);
					if (num == this.PW.Length - 1)
					{
						base.ParentBindingExpression.TransferValue(request.Result, false);
						return;
					}
				}
			}
		}

		// Token: 0x06007492 RID: 29842 RVA: 0x00215B9C File Offset: 0x00213D9C
		private void RequestAsyncSetValue(object item, object value)
		{
			string nameFromInfo = this.GetNameFromInfo(this.PW.GetAccessor(this.PW.Length - 1));
			Invariant.Assert(nameFromInfo != null, "Async SetValue expects a name");
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest != null)
			{
				asyncSetValueRequest.Cancel();
			}
			asyncSetValueRequest = new AsyncSetValueRequest(item, nameFromInfo, value, base.ParentBinding.AsyncState, ClrBindingWorker.DoSetValueCallback, ClrBindingWorker.CompleteSetValueCallback, new object[]
			{
				this
			});
			base.SetValue(BindingWorker.Feature.PendingSetValueRequest, asyncSetValueRequest);
			base.Engine.AddAsyncRequest(base.TargetElement, asyncSetValueRequest);
		}

		// Token: 0x06007493 RID: 29843 RVA: 0x00215C30 File Offset: 0x00213E30
		private static object OnSetValueCallback(AsyncDataRequest adr)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncSetValueRequest.Args[0];
			clrBindingWorker.PW.SetValue(asyncSetValueRequest.TargetItem, asyncSetValueRequest.Value);
			return null;
		}

		// Token: 0x06007494 RID: 29844 RVA: 0x00215C6C File Offset: 0x00213E6C
		private static object OnCompleteSetValueCallback(AsyncDataRequest adr)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)adr;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncSetValueRequest.Args[0];
			DataBindEngine engine = clrBindingWorker.Engine;
			if (engine != null)
			{
				engine.Marshal(ClrBindingWorker.CompleteSetValueLocalCallback, asyncSetValueRequest, 1);
			}
			return null;
		}

		// Token: 0x06007495 RID: 29845 RVA: 0x00215CA8 File Offset: 0x00213EA8
		private static object OnCompleteSetValueOperation(object arg)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)arg;
			ClrBindingWorker clrBindingWorker = (ClrBindingWorker)asyncSetValueRequest.Args[0];
			clrBindingWorker.CompleteSetValue(asyncSetValueRequest);
			return null;
		}

		// Token: 0x06007496 RID: 29846 RVA: 0x00215CD4 File Offset: 0x00213ED4
		private void CompleteSetValue(AsyncSetValueRequest request)
		{
			AsyncSetValueRequest asyncSetValueRequest = (AsyncSetValueRequest)base.GetValue(BindingWorker.Feature.PendingSetValueRequest, null);
			if (asyncSetValueRequest == request)
			{
				base.ClearValue(BindingWorker.Feature.PendingSetValueRequest);
				if (this.CheckTarget() == null)
				{
					return;
				}
				AsyncRequestStatus status = request.Status;
				if (status != AsyncRequestStatus.Completed && status == AsyncRequestStatus.Failed)
				{
					object obj = base.ParentBinding.DoFilterException(base.ParentBindingExpression, request.Exception);
					Exception ex = obj as Exception;
					ValidationError validationError;
					if (ex != null)
					{
						if (TraceData.IsEnabled)
						{
							int k = this.PW.Length - 1;
							object value = request.Value;
							this.ReportSetValueError(k, request.TargetItem, request.Value, ex);
							return;
						}
					}
					else if ((validationError = (obj as ValidationError)) != null)
					{
						Validation.MarkInvalid(base.ParentBindingExpression, validationError);
					}
				}
			}
		}

		// Token: 0x06007497 RID: 29847 RVA: 0x00215D84 File Offset: 0x00213F84
		private string GetNameFromInfo(object info)
		{
			MemberInfo memberInfo;
			if ((memberInfo = (info as MemberInfo)) != null)
			{
				return memberInfo.Name;
			}
			PropertyDescriptor propertyDescriptor;
			if ((propertyDescriptor = (info as PropertyDescriptor)) != null)
			{
				return propertyDescriptor.Name;
			}
			DynamicObjectAccessor dynamicObjectAccessor;
			if ((dynamicObjectAccessor = (info as DynamicObjectAccessor)) != null)
			{
				return dynamicObjectAccessor.PropertyName;
			}
			return null;
		}

		// Token: 0x06007498 RID: 29848 RVA: 0x00215DCB File Offset: 0x00213FCB
		private object ScheduleTransferOperation(object arg)
		{
			this.PW.RefreshValue();
			return null;
		}

		// Token: 0x040037D3 RID: 14291
		private static readonly AsyncRequestCallback DoGetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnGetValueCallback);

		// Token: 0x040037D4 RID: 14292
		private static readonly AsyncRequestCallback CompleteGetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnCompleteGetValueCallback);

		// Token: 0x040037D5 RID: 14293
		private static readonly DispatcherOperationCallback CompleteGetValueLocalCallback = new DispatcherOperationCallback(ClrBindingWorker.OnCompleteGetValueOperation);

		// Token: 0x040037D6 RID: 14294
		private static readonly AsyncRequestCallback DoSetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnSetValueCallback);

		// Token: 0x040037D7 RID: 14295
		private static readonly AsyncRequestCallback CompleteSetValueCallback = new AsyncRequestCallback(ClrBindingWorker.OnCompleteSetValueCallback);

		// Token: 0x040037D8 RID: 14296
		private static readonly DispatcherOperationCallback CompleteSetValueLocalCallback = new DispatcherOperationCallback(ClrBindingWorker.OnCompleteSetValueOperation);

		// Token: 0x040037D9 RID: 14297
		private PropertyPathWorker _pathWorker;
	}
}
