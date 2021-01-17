using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.Data;

namespace System.Windows.Data
{
	/// <summary>Contains information about a single instance of a <see cref="T:System.Windows.Data.Binding" />. </summary>
	// Token: 0x0200019F RID: 415
	public sealed class BindingExpression : BindingExpressionBase, IDataBindEngineClient, IWeakEventListener
	{
		// Token: 0x06001844 RID: 6212 RVA: 0x00074B2C File Offset: 0x00072D2C
		private BindingExpression(Binding binding, BindingExpressionBase owner) : base(binding, owner)
		{
			base.UseDefaultValueConverter = (this.ParentBinding.Converter == null);
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.CreateExpression))
			{
				PropertyPath path = binding.Path;
				string o = (path != null) ? path.Path : string.Empty;
				if (string.IsNullOrEmpty(binding.XPath))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.BindingPath(new object[]
					{
						TraceData.Identify(o)
					}));
					return;
				}
				TraceData.Trace(TraceEventType.Warning, TraceData.BindingXPathAndPath(new object[]
				{
					TraceData.Identify(binding.XPath),
					TraceData.Identify(o)
				}));
			}
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x00074BC6 File Offset: 0x00072DC6
		void IDataBindEngineClient.TransferValue()
		{
			this.TransferValue();
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x00074BCE File Offset: 0x00072DCE
		void IDataBindEngineClient.UpdateValue()
		{
			base.UpdateValue();
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x00074BD7 File Offset: 0x00072DD7
		bool IDataBindEngineClient.AttachToContext(bool lastChance)
		{
			this.AttachToContext(lastChance ? BindingExpression.AttachAttempt.Last : BindingExpression.AttachAttempt.Again);
			return base.StatusInternal > BindingStatusInternal.Unattached;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x00074BF0 File Offset: 0x00072DF0
		void IDataBindEngineClient.VerifySourceReference(bool lastChance)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			ObjectRef sourceReference = this.ParentBinding.SourceReference;
			DependencyObject d = (!base.UsingMentor) ? targetElement : Helper.FindMentor(targetElement);
			ObjectRefArgs args = new ObjectRefArgs
			{
				ResolveNamesInTemplate = base.ResolveNamesInTemplate
			};
			object dataObject = sourceReference.GetDataObject(d, args);
			if (dataObject != this.DataItem)
			{
				this.AttachToContext(lastChance ? BindingExpression.AttachAttempt.Last : BindingExpression.AttachAttempt.Again);
			}
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x00074C59 File Offset: 0x00072E59
		void IDataBindEngineClient.OnTargetUpdated()
		{
			this.OnTargetUpdated();
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x0600184A RID: 6218 RVA: 0x00074C61 File Offset: 0x00072E61
		DependencyObject IDataBindEngineClient.TargetElement
		{
			get
			{
				if (base.UsingMentor)
				{
					return Helper.FindMentor(base.TargetElement);
				}
				return base.TargetElement;
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Data.Binding" /> object of the current <see cref="T:System.Windows.Data.BindingExpression" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Data.Binding" /> object of the current binding expression.</returns>
		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x0600184B RID: 6219 RVA: 0x00074C7D File Offset: 0x00072E7D
		public Binding ParentBinding
		{
			get
			{
				return (Binding)base.ParentBindingBase;
			}
		}

		/// <summary>Gets the binding source object that this <see cref="T:System.Windows.Data.BindingExpression" /> uses.</summary>
		/// <returns>The binding source object that this <see cref="T:System.Windows.Data.BindingExpression" /> uses.</returns>
		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x00074C8A File Offset: 0x00072E8A
		public object DataItem
		{
			get
			{
				return BindingExpressionBase.GetReference(this._dataItem);
			}
		}

		/// <summary>Gets the binding source object for this <see cref="T:System.Windows.Data.BindingExpression" />.</summary>
		/// <returns>The binding source object for this <see cref="T:System.Windows.Data.BindingExpression" />.</returns>
		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x00074C97 File Offset: 0x00072E97
		public object ResolvedSource
		{
			get
			{
				return this.SourceItem;
			}
		}

		/// <summary>Gets the name of the binding source property for this <see cref="T:System.Windows.Data.BindingExpression" />. </summary>
		/// <returns>The name of the binding source property for this <see cref="T:System.Windows.Data.BindingExpression" />.</returns>
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x0600184E RID: 6222 RVA: 0x00074C9F File Offset: 0x00072E9F
		public string ResolvedSourcePropertyName
		{
			get
			{
				return this.SourcePropertyName;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x0600184F RID: 6223 RVA: 0x00074CA8 File Offset: 0x00072EA8
		internal object DataSource
		{
			get
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement == null)
				{
					return null;
				}
				if (this._ctxElement != null)
				{
					return this.GetDataSourceForDataContext(this.ContextElement);
				}
				ObjectRef sourceReference = this.ParentBinding.SourceReference;
				return sourceReference.GetObject(targetElement, new ObjectRefArgs());
			}
		}

		/// <summary>Sends the current binding target value to the binding source property in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> or <see cref="F:System.Windows.Data.BindingMode.OneWayToSource" /> bindings.</summary>
		/// <exception cref="T:System.InvalidOperationException">The binding has been detached from its target.</exception>
		// Token: 0x06001850 RID: 6224 RVA: 0x00074CEE File Offset: 0x00072EEE
		public override void UpdateSource()
		{
			if (base.IsDetached)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			base.NeedsUpdate = true;
			base.Update();
		}

		/// <summary>Forces a data transfer from the binding source property to the binding target property.</summary>
		/// <exception cref="T:System.InvalidOperationException">The binding has been detached from its target.</exception>
		// Token: 0x06001851 RID: 6225 RVA: 0x00074D16 File Offset: 0x00072F16
		public override void UpdateTarget()
		{
			if (base.IsDetached)
			{
				throw new InvalidOperationException(SR.Get("BindingExpressionIsDetached"));
			}
			if (this.Worker != null)
			{
				this.Worker.RefreshValue();
			}
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x00074D44 File Offset: 0x00072F44
		internal override void OnPropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			DependencyProperty property = args.Property;
			if (property == null)
			{
				throw new InvalidOperationException(SR.Get("ArgumentPropertyMustNotBeNull", new object[]
				{
					"Property",
					"args"
				}));
			}
			bool flag = !this.IgnoreSourcePropertyChange;
			if (property == FrameworkElement.DataContextProperty && d == this.ContextElement)
			{
				flag = true;
			}
			else if (property == CollectionViewSource.ViewProperty && d == this.CollectionViewSource)
			{
				flag = true;
			}
			else if (property == FrameworkElement.LanguageProperty && base.UsesLanguage && d == base.TargetElement)
			{
				flag = true;
			}
			else if (flag)
			{
				flag = (this.Worker != null && this.Worker.UsesDependencyProperty(d, property));
			}
			if (!flag)
			{
				return;
			}
			base.OnPropertyInvalidation(d, args);
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x00002137 File Offset: 0x00000337
		internal override void InvalidateChild(BindingExpressionBase bindingExpression)
		{
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x00002137 File Offset: 0x00000337
		internal override void ChangeSourcesForChild(BindingExpressionBase bindingExpression, WeakDependencySource[] newSources)
		{
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x00002137 File Offset: 0x00000337
		internal override void ReplaceChild(BindingExpressionBase bindingExpression)
		{
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x00074E09 File Offset: 0x00073009
		internal override void UpdateBindingGroup(BindingGroup bg)
		{
			bg.UpdateTable(this);
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001857 RID: 6231 RVA: 0x00074E12 File Offset: 0x00073012
		internal DependencyObject ContextElement
		{
			get
			{
				if (this._ctxElement != null)
				{
					return this._ctxElement.Target as DependencyObject;
				}
				return null;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001858 RID: 6232 RVA: 0x00074E30 File Offset: 0x00073030
		// (set) Token: 0x06001859 RID: 6233 RVA: 0x00074E5C File Offset: 0x0007305C
		internal CollectionViewSource CollectionViewSource
		{
			get
			{
				WeakReference weakReference = (WeakReference)base.GetValue(BindingExpressionBase.Feature.CollectionViewSource, null);
				if (weakReference != null)
				{
					return (CollectionViewSource)weakReference.Target;
				}
				return null;
			}
			set
			{
				if (value == null)
				{
					base.ClearValue(BindingExpressionBase.Feature.CollectionViewSource);
					return;
				}
				base.SetValue(BindingExpressionBase.Feature.CollectionViewSource, new WeakReference(value));
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600185A RID: 6234 RVA: 0x00074E78 File Offset: 0x00073078
		internal bool IgnoreSourcePropertyChange
		{
			get
			{
				return base.IsTransferPending || base.IsInUpdate;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600185B RID: 6235 RVA: 0x00074E8D File Offset: 0x0007308D
		internal PropertyPath Path
		{
			get
			{
				return this.ParentBinding.Path;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x0600185C RID: 6236 RVA: 0x00074E9A File Offset: 0x0007309A
		// (set) Token: 0x0600185D RID: 6237 RVA: 0x00074EA9 File Offset: 0x000730A9
		internal IValueConverter Converter
		{
			get
			{
				return (IValueConverter)base.GetValue(BindingExpressionBase.Feature.Converter, null);
			}
			set
			{
				base.SetValue(BindingExpressionBase.Feature.Converter, value, null);
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x0600185E RID: 6238 RVA: 0x00074EB4 File Offset: 0x000730B4
		internal Type ConverterSourceType
		{
			get
			{
				return this._sourceType;
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x0600185F RID: 6239 RVA: 0x00074EBC File Offset: 0x000730BC
		internal object SourceItem
		{
			get
			{
				if (this.Worker == null)
				{
					return null;
				}
				return this.Worker.SourceItem;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001860 RID: 6240 RVA: 0x00074ED3 File Offset: 0x000730D3
		internal string SourcePropertyName
		{
			get
			{
				if (this.Worker == null)
				{
					return null;
				}
				return this.Worker.SourcePropertyName;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x00074EEA File Offset: 0x000730EA
		internal object SourceValue
		{
			get
			{
				if (this.Worker == null)
				{
					return DependencyProperty.UnsetValue;
				}
				return this.Worker.RawValue();
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x00074F05 File Offset: 0x00073105
		internal override bool IsParentBindingUpdateTriggerDefault
		{
			get
			{
				return this.ParentBinding.UpdateSourceTrigger == UpdateSourceTrigger.Default;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001863 RID: 6243 RVA: 0x00074F15 File Offset: 0x00073115
		internal override bool IsDisconnected
		{
			get
			{
				return BindingExpressionBase.GetReference(this._dataItem) == BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x00074F2C File Offset: 0x0007312C
		internal static BindingExpression CreateBindingExpression(DependencyObject d, DependencyProperty dp, Binding binding, BindingExpressionBase parent)
		{
			FrameworkPropertyMetadata frameworkPropertyMetadata = dp.GetMetadata(d.DependencyObjectType) as FrameworkPropertyMetadata;
			if ((frameworkPropertyMetadata != null && !frameworkPropertyMetadata.IsDataBindingAllowed) || dp.ReadOnly)
			{
				throw new ArgumentException(SR.Get("PropertyNotBindable", new object[]
				{
					dp.Name
				}), "dp");
			}
			BindingExpression bindingExpression = new BindingExpression(binding, parent);
			bindingExpression.ResolvePropertyDefaultSettings(binding.Mode, binding.UpdateSourceTrigger, frameworkPropertyMetadata);
			if (bindingExpression.IsReflective && binding.XPath == null && (binding.Path == null || string.IsNullOrEmpty(binding.Path.Path)))
			{
				throw new InvalidOperationException(SR.Get("TwoWayBindingNeedsPath"));
			}
			return bindingExpression;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x00074FD8 File Offset: 0x000731D8
		internal void SetupDefaultValueConverter(Type type)
		{
			if (!base.UseDefaultValueConverter)
			{
				return;
			}
			if (base.IsInMultiBindingExpression)
			{
				this.Converter = null;
				this._sourceType = type;
				return;
			}
			if (type != null && type != this._sourceType)
			{
				this._sourceType = type;
				IValueConverter valueConverter = base.Engine.GetDefaultValueConverter(type, base.TargetProperty.PropertyType, base.IsReflective);
				if (valueConverter == null && TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.CannotCreateDefaultValueConverter(new object[]
					{
						type,
						base.TargetProperty.PropertyType,
						base.IsReflective ? "two-way" : "one-way"
					}), this);
				}
				if (valueConverter == DefaultValueConverter.ValueConverterNotNeeded)
				{
					valueConverter = null;
				}
				this.Converter = valueConverter;
			}
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0007509C File Offset: 0x0007329C
		internal static bool HasLocalDataContext(DependencyObject d)
		{
			bool flag;
			BaseValueSourceInternal valueSource = d.GetValueSource(FrameworkElement.DataContextProperty, null, out flag);
			return valueSource != BaseValueSourceInternal.Inherited && (valueSource != BaseValueSourceInternal.Default || flag);
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001867 RID: 6247 RVA: 0x000750C7 File Offset: 0x000732C7
		private bool CanActivate
		{
			get
			{
				return base.StatusInternal > BindingStatusInternal.Unattached;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001868 RID: 6248 RVA: 0x000750D2 File Offset: 0x000732D2
		private BindingWorker Worker
		{
			get
			{
				return this._worker;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001869 RID: 6249 RVA: 0x000750DC File Offset: 0x000732DC
		private DynamicValueConverter DynamicConverter
		{
			get
			{
				if (!base.HasValue(BindingExpressionBase.Feature.DynamicConverter))
				{
					Invariant.Assert(this.Worker != null);
					base.SetValue(BindingExpressionBase.Feature.DynamicConverter, new DynamicValueConverter(base.IsReflective, this.Worker.SourcePropertyType, this.Worker.TargetPropertyType), null);
				}
				return (DynamicValueConverter)base.GetValue(BindingExpressionBase.Feature.DynamicConverter, null);
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x0600186A RID: 6250 RVA: 0x00075139 File Offset: 0x00073339
		// (set) Token: 0x0600186B RID: 6251 RVA: 0x00075149 File Offset: 0x00073349
		private DataSourceProvider DataProvider
		{
			get
			{
				return (DataSourceProvider)base.GetValue(BindingExpressionBase.Feature.DataProvider, null);
			}
			set
			{
				base.SetValue(BindingExpressionBase.Feature.DataProvider, value, null);
			}
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x00075158 File Offset: 0x00073358
		internal override bool AttachOverride(DependencyObject target, DependencyProperty dp)
		{
			if (!base.AttachOverride(target, dp))
			{
				return false;
			}
			if (this.ParentBinding.SourceReference == null || this.ParentBinding.SourceReference.UsesMentor)
			{
				DependencyObject dependencyObject = Helper.FindMentor(target);
				if (dependencyObject != target)
				{
					InheritanceContextChangedEventManager.AddHandler(target, new EventHandler<EventArgs>(this.OnInheritanceContextChanged));
					base.UsingMentor = true;
					if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.UseMentor(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(dependencyObject)
						}));
					}
				}
			}
			if (base.IsUpdateOnLostFocus)
			{
				Invariant.Assert(!base.IsInMultiBindingExpression, "Source BindingExpressions of a MultiBindingExpression should never be UpdateOnLostFocus.");
				LostFocusEventManager.AddHandler(target, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			this.AttachToContext(BindingExpression.AttachAttempt.First);
			if (base.StatusInternal == BindingStatusInternal.Unattached)
			{
				base.Engine.AddTask(this, TaskOps.AttachToContext);
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.DeferAttachToContext(new object[]
					{
						TraceData.Identify(this)
					}));
				}
			}
			GC.KeepAlive(target);
			return true;
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x00075254 File Offset: 0x00073454
		internal override void DetachOverride()
		{
			this.Deactivate();
			this.DetachFromContext();
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null && base.IsUpdateOnLostFocus)
			{
				LostFocusEventManager.RemoveHandler(targetElement, new EventHandler<RoutedEventArgs>(this.OnLostFocus));
			}
			if (base.ValidatesOnNotifyDataErrors)
			{
				WeakReference weakReference = (WeakReference)base.GetValue(BindingExpressionBase.Feature.DataErrorValue, null);
				INotifyDataErrorInfo notifyDataErrorInfo = (weakReference == null) ? null : (weakReference.Target as INotifyDataErrorInfo);
				if (notifyDataErrorInfo != null)
				{
					ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
					base.SetValue(BindingExpressionBase.Feature.DataErrorValue, null, null);
				}
			}
			base.ChangeValue(DependencyProperty.UnsetValue, false);
			base.DetachOverride();
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x000752EC File Offset: 0x000734EC
		private void AttachToContext(BindingExpression.AttachAttempt attempt)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			bool isTracing = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			if (attempt == BindingExpression.AttachAttempt.First)
			{
				ObjectRef sourceReference = this.ParentBinding.SourceReference;
				if (sourceReference != null && sourceReference.TreeContextIsRequired(targetElement))
				{
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.SourceRequiresTreeContext(new object[]
						{
							TraceData.Identify(this),
							sourceReference.Identify()
						}));
					}
					return;
				}
			}
			bool flag2 = attempt == BindingExpression.AttachAttempt.Last;
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.AttachToContext(new object[]
				{
					TraceData.Identify(this),
					flag2 ? " (last chance)" : string.Empty
				}));
			}
			if (!flag2 && this.ParentBinding.TreeContextIsRequired && (targetElement.GetValue(XmlAttributeProperties.XmlnsDictionaryProperty) == null || targetElement.GetValue(XmlAttributeProperties.XmlNamespaceMapsProperty) == null))
			{
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.PathRequiresTreeContext(new object[]
					{
						TraceData.Identify(this),
						this.ParentBinding.Path.Path
					}));
				}
				return;
			}
			DependencyObject dependencyObject = (!base.UsingMentor) ? targetElement : Helper.FindMentor(targetElement);
			if (dependencyObject == null)
			{
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.NoMentorExtended(new object[]
					{
						TraceData.Identify(this)
					}));
				}
				if (flag2)
				{
					base.SetStatus(BindingStatusInternal.PathError);
					if (TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Error, TraceData.NoMentor, this);
					}
				}
				return;
			}
			DependencyObject dependencyObject2 = null;
			bool flag3 = true;
			if (this.ParentBinding.SourceReference == null)
			{
				dependencyObject2 = dependencyObject;
				CollectionViewSource collectionViewSource;
				if (base.TargetProperty == FrameworkElement.DataContextProperty || (base.TargetProperty == ContentPresenter.ContentProperty && targetElement is ContentPresenter) || (base.UsingMentor && (collectionViewSource = (targetElement as CollectionViewSource)) != null && collectionViewSource.PropertyForInheritanceContext == FrameworkElement.DataContextProperty))
				{
					dependencyObject2 = FrameworkElement.GetFrameworkParent(dependencyObject2);
					flag3 = (dependencyObject2 != null);
				}
			}
			else
			{
				RelativeObjectRef relativeObjectRef = this.ParentBinding.SourceReference as RelativeObjectRef;
				if (relativeObjectRef != null && relativeObjectRef.ReturnsDataContext)
				{
					object @object = relativeObjectRef.GetObject(dependencyObject, new ObjectRefArgs
					{
						IsTracing = isTracing
					});
					dependencyObject2 = (@object as DependencyObject);
					flag3 = (@object != DependencyProperty.UnsetValue);
				}
			}
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.ContextElement(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(dependencyObject2),
					flag3 ? "OK" : "error"
				}));
			}
			if (!flag3)
			{
				if (flag2)
				{
					base.SetStatus(BindingStatusInternal.PathError);
					if (TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Error, TraceData.NoDataContext, this);
					}
				}
				return;
			}
			object obj;
			ObjectRef sourceReference2;
			if (dependencyObject2 != null)
			{
				obj = dependencyObject2.GetValue(FrameworkElement.DataContextProperty);
				if (obj == null && !flag2 && !BindingExpression.HasLocalDataContext(dependencyObject2))
				{
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.NullDataContext(new object[]
						{
							TraceData.Identify(this)
						}));
					}
					return;
				}
			}
			else if ((sourceReference2 = this.ParentBinding.SourceReference) != null)
			{
				ObjectRefArgs objectRefArgs = new ObjectRefArgs
				{
					IsTracing = isTracing,
					ResolveNamesInTemplate = base.ResolveNamesInTemplate
				};
				obj = sourceReference2.GetDataObject(dependencyObject, objectRefArgs);
				if (obj == DependencyProperty.UnsetValue)
				{
					if (flag2)
					{
						base.SetStatus(BindingStatusInternal.PathError);
						if (TraceData.IsEnabled)
						{
							TraceData.Trace(base.TraceLevel, TraceData.NoSource(new object[]
							{
								sourceReference2
							}), this);
						}
					}
					return;
				}
				if (!flag2 && objectRefArgs.NameResolvedInOuterScope)
				{
					base.Engine.AddTask(this, TaskOps.VerifySourceReference);
				}
			}
			else
			{
				obj = null;
			}
			if (dependencyObject2 != null)
			{
				this._ctxElement = new WeakReference(dependencyObject2);
			}
			this.ChangeWorkerSources(null, 0);
			if (!base.UseDefaultValueConverter)
			{
				this.Converter = this.ParentBinding.Converter;
				if (this.Converter == null)
				{
					throw new InvalidOperationException(SR.Get("MissingValueConverter"));
				}
			}
			base.JoinBindingGroup(base.IsReflective, dependencyObject2);
			base.SetStatus(BindingStatusInternal.Inactive);
			if (base.IsInPriorityBindingExpression)
			{
				base.ParentPriorityBindingExpression.InvalidateChild(this);
			}
			else
			{
				this.Activate(obj);
			}
			GC.KeepAlive(targetElement);
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x000756A8 File Offset: 0x000738A8
		private void DetachFromContext()
		{
			if (base.HasValue(BindingExpressionBase.Feature.DataProvider))
			{
				DataChangedEventManager.RemoveHandler(this.DataProvider, new EventHandler<EventArgs>(this.OnDataChanged));
			}
			if (!base.UseDefaultValueConverter)
			{
				this.Converter = null;
			}
			if (!base.IsInBindingExpressionCollection)
			{
				base.ChangeSources(null);
			}
			if (base.UsingMentor)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null)
				{
					InheritanceContextChangedEventManager.RemoveHandler(targetElement, new EventHandler<EventArgs>(this.OnInheritanceContextChanged));
				}
			}
			this._ctxElement = null;
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x00075720 File Offset: 0x00073920
		internal override void Activate()
		{
			if (!this.CanActivate)
			{
				return;
			}
			if (this._ctxElement == null)
			{
				if (base.StatusInternal == BindingStatusInternal.Inactive)
				{
					DependencyObject dependencyObject = base.TargetElement;
					if (dependencyObject != null)
					{
						if (base.UsingMentor)
						{
							dependencyObject = Helper.FindMentor(dependencyObject);
							if (dependencyObject == null)
							{
								base.SetStatus(BindingStatusInternal.PathError);
								if (TraceData.IsEnabled)
								{
									TraceData.Trace(TraceEventType.Error, TraceData.NoMentor, this);
								}
								return;
							}
						}
						this.Activate(this.ParentBinding.SourceReference.GetDataObject(dependencyObject, new ObjectRefArgs
						{
							ResolveNamesInTemplate = base.ResolveNamesInTemplate
						}));
						return;
					}
				}
			}
			else
			{
				DependencyObject contextElement = this.ContextElement;
				if (contextElement == null)
				{
					base.SetStatus(BindingStatusInternal.PathError);
					if (TraceData.IsEnabled)
					{
						TraceData.Trace(TraceEventType.Error, TraceData.NoDataContext, this);
					}
					return;
				}
				object value = contextElement.GetValue(FrameworkElement.DataContextProperty);
				if (base.StatusInternal == BindingStatusInternal.Inactive || !ItemsControl.EqualsEx(value, this.DataItem))
				{
					this.Activate(value);
				}
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x000757FC File Offset: 0x000739FC
		internal void Activate(object item)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			if (item == BindingExpressionBase.DisconnectedItem)
			{
				this.Disconnect();
				return;
			}
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach);
			this.Deactivate();
			if (!this.ParentBinding.BindsDirectlyToSource)
			{
				CollectionViewSource collectionViewSource = item as CollectionViewSource;
				this.CollectionViewSource = collectionViewSource;
				if (collectionViewSource != null)
				{
					item = collectionViewSource.CollectionView;
					this.ChangeWorkerSources(null, 0);
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.UseCVS(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(collectionViewSource)
						}));
					}
				}
				else
				{
					item = this.DereferenceDataProvider(item);
				}
			}
			this._dataItem = BindingExpressionBase.CreateReference(item);
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.ActivateItem(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(item)
				}));
			}
			if (this.Worker == null)
			{
				this.CreateWorker();
			}
			base.SetStatus(BindingStatusInternal.Active);
			this.Worker.AttachDataItem();
			bool flag2 = base.IsOneWayToSource;
			object newValue;
			if (base.ShouldUpdateWithCurrentValue(targetElement, out newValue))
			{
				flag2 = true;
				base.ChangeValue(newValue, false);
				base.NeedsUpdate = true;
			}
			if (!flag2)
			{
				ValidationError validationError;
				object initialValue = this.GetInitialValue(targetElement, out validationError);
				bool flag3 = initialValue == BindingExpression.NullDataItem;
				if (!flag3)
				{
					this.TransferValue(initialValue, false);
				}
				if (validationError != null)
				{
					base.UpdateValidationError(validationError, flag3);
				}
			}
			else if (!base.IsInMultiBindingExpression)
			{
				base.UpdateValue();
			}
			GC.KeepAlive(targetElement);
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x00075958 File Offset: 0x00073B58
		private object GetInitialValue(DependencyObject target, out ValidationError error)
		{
			BindingGroup bindingGroup = base.RootBindingExpression.FindBindingGroup(true, this.ContextElement);
			BindingGroup.ProposedValueEntry proposedValueEntry;
			object obj;
			if (bindingGroup == null || (proposedValueEntry = bindingGroup.GetProposedValueEntry(this.SourceItem, this.SourcePropertyName)) == null)
			{
				error = null;
				obj = DependencyProperty.UnsetValue;
			}
			else
			{
				error = proposedValueEntry.ValidationError;
				if (base.IsReflective && base.TargetProperty.IsValidValue(proposedValueEntry.RawValue))
				{
					target.SetValue(base.TargetProperty, proposedValueEntry.RawValue);
					obj = BindingExpression.NullDataItem;
					bindingGroup.RemoveProposedValueEntry(proposedValueEntry);
				}
				else if (proposedValueEntry.ConvertedValue == DependencyProperty.UnsetValue)
				{
					obj = base.UseFallbackValue();
				}
				else
				{
					obj = proposedValueEntry.ConvertedValue;
				}
				if (obj != BindingExpression.NullDataItem)
				{
					bindingGroup.AddBindingForProposedValue(this, this.SourceItem, this.SourcePropertyName);
				}
			}
			return obj;
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00075A1C File Offset: 0x00073C1C
		internal override void Deactivate()
		{
			if (base.StatusInternal == BindingStatusInternal.Inactive)
			{
				return;
			}
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.Deactivate(new object[]
				{
					TraceData.Identify(this)
				}));
			}
			this.CancelPendingTasks();
			if (this.Worker != null)
			{
				this.Worker.DetachDataItem();
			}
			base.ChangeValue(BindingExpressionBase.DefaultValueObject, false);
			this._dataItem = null;
			base.SetStatus(BindingStatusInternal.Inactive);
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00075A89 File Offset: 0x00073C89
		internal override void Disconnect()
		{
			this._dataItem = BindingExpressionBase.CreateReference(BindingExpressionBase.DisconnectedItem);
			if (this.Worker == null)
			{
				return;
			}
			this.Worker.AttachDataItem();
			base.Disconnect();
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00075AB8 File Offset: 0x00073CB8
		private object DereferenceDataProvider(object item)
		{
			DataSourceProvider dataSourceProvider = item as DataSourceProvider;
			DataSourceProvider dataSourceProvider2 = this.DataProvider;
			if (dataSourceProvider != dataSourceProvider2)
			{
				if (dataSourceProvider2 != null)
				{
					DataChangedEventManager.RemoveHandler(dataSourceProvider2, new EventHandler<EventArgs>(this.OnDataChanged));
				}
				this.DataProvider = dataSourceProvider;
				dataSourceProvider2 = dataSourceProvider;
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Attach))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.UseDataProvider(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(dataSourceProvider)
					}));
				}
				if (dataSourceProvider != null)
				{
					DataChangedEventManager.AddHandler(dataSourceProvider, new EventHandler<EventArgs>(this.OnDataChanged));
					dataSourceProvider.InitialLoad();
				}
			}
			if (dataSourceProvider2 == null)
			{
				return item;
			}
			return dataSourceProvider2.Data;
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00074C97 File Offset: 0x00072E97
		internal override object GetSourceItem(object newValue)
		{
			return this.SourceItem;
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00075B47 File Offset: 0x00073D47
		private void CreateWorker()
		{
			Invariant.Assert(this.Worker == null, "duplicate call to CreateWorker");
			this._worker = new ClrBindingWorker(this, base.Engine);
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00075B70 File Offset: 0x00073D70
		internal void ChangeWorkerSources(WeakDependencySource[] newWorkerSources, int n)
		{
			int destinationIndex = 0;
			int num = n;
			DependencyObject contextElement = this.ContextElement;
			CollectionViewSource collectionViewSource = this.CollectionViewSource;
			bool usesLanguage = base.UsesLanguage;
			if (contextElement != null)
			{
				num++;
			}
			if (collectionViewSource != null)
			{
				num++;
			}
			if (usesLanguage)
			{
				num++;
			}
			WeakDependencySource[] array = (num > 0) ? new WeakDependencySource[num] : null;
			if (contextElement != null)
			{
				array[destinationIndex++] = new WeakDependencySource(this._ctxElement, FrameworkElement.DataContextProperty);
			}
			if (collectionViewSource != null)
			{
				WeakReference weakReference = base.GetValue(BindingExpressionBase.Feature.CollectionViewSource, null) as WeakReference;
				array[destinationIndex++] = ((weakReference != null) ? new WeakDependencySource(weakReference, CollectionViewSource.ViewProperty) : new WeakDependencySource(collectionViewSource, CollectionViewSource.ViewProperty));
			}
			if (usesLanguage)
			{
				array[destinationIndex++] = new WeakDependencySource(base.TargetElementReference, FrameworkElement.LanguageProperty);
			}
			if (n > 0)
			{
				Array.Copy(newWorkerSources, 0, array, destinationIndex, n);
			}
			base.ChangeSources(array);
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00075C42 File Offset: 0x00073E42
		private void TransferValue()
		{
			this.TransferValue(DependencyProperty.UnsetValue, false);
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00075C50 File Offset: 0x00073E50
		internal void TransferValue(object newValue, bool isASubPropertyChange)
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement == null)
			{
				return;
			}
			if (this.Worker == null)
			{
				return;
			}
			Type effectiveTargetType = base.GetEffectiveTargetType();
			IValueConverter valueConverter = null;
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
			base.IsTransferPending = false;
			base.IsInTransfer = true;
			base.UsingFallbackValue = false;
			object obj = (newValue == DependencyProperty.UnsetValue) ? this.Worker.RawValue() : newValue;
			this.UpdateNotifyDataErrors(obj);
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GetRawValue(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(obj)
				}));
			}
			if (obj != DependencyProperty.UnsetValue)
			{
				bool flag2 = false;
				if (!base.UseDefaultValueConverter)
				{
					obj = this.Converter.Convert(obj, effectiveTargetType, this.ParentBinding.ConverterParameter, base.GetCulture());
					if (base.IsDetached)
					{
						return;
					}
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.UserConverter(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj)
						}));
					}
					if (obj != null && obj != Binding.DoNothing && obj != DependencyProperty.UnsetValue && !effectiveTargetType.IsAssignableFrom(obj.GetType()))
					{
						valueConverter = this.DynamicConverter;
					}
				}
				else
				{
					valueConverter = this.Converter;
				}
				if (obj != Binding.DoNothing && obj != DependencyProperty.UnsetValue)
				{
					if (base.EffectiveTargetNullValue != DependencyProperty.UnsetValue && BindingExpressionBase.IsNullValue(obj))
					{
						obj = base.EffectiveTargetNullValue;
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.NullConverter(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}));
						}
					}
					else if (obj == DBNull.Value && (this.Converter == null || base.UseDefaultValueConverter))
					{
						if (effectiveTargetType.IsGenericType && effectiveTargetType.GetGenericTypeDefinition() == typeof(Nullable<>))
						{
							obj = null;
						}
						else
						{
							obj = DependencyProperty.UnsetValue;
							flag2 = true;
						}
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.ConvertDBNull(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}));
						}
					}
					else if (valueConverter != null || base.EffectiveStringFormat != null)
					{
						obj = this.ConvertHelper(valueConverter, obj, effectiveTargetType, base.TargetElement, base.GetCulture());
						if (flag)
						{
							TraceData.Trace(TraceEventType.Warning, TraceData.DefaultConverter(new object[]
							{
								TraceData.Identify(this),
								TraceData.Identify(obj)
							}));
						}
					}
				}
				if (!flag2 && obj == DependencyProperty.UnsetValue)
				{
					base.SetStatus(BindingStatusInternal.UpdateTargetError);
				}
			}
			if (obj != Binding.DoNothing)
			{
				if (!base.IsInMultiBindingExpression && obj != BindingExpression.IgnoreDefaultValue && obj != DependencyProperty.UnsetValue && !base.TargetProperty.IsValidValue(obj))
				{
					if (TraceData.IsEnabled && !base.IsInBindingExpressionCollection)
					{
						TraceData.Trace(base.TraceLevel, TraceData.BadValueAtTransfer, obj, this);
					}
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.BadValueAtTransferExtended(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj)
						}));
					}
					obj = DependencyProperty.UnsetValue;
					if (base.StatusInternal == BindingStatusInternal.Active)
					{
						base.SetStatus(BindingStatusInternal.UpdateTargetError);
					}
				}
				if (obj == DependencyProperty.UnsetValue)
				{
					obj = base.UseFallbackValue();
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.UseFallback(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(obj)
						}));
					}
				}
				if (obj == BindingExpression.IgnoreDefaultValue)
				{
					obj = Expression.NoValue;
				}
				if (flag)
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.TransferValue(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(obj)
					}));
				}
				bool flag3 = !base.IsInUpdate || !ItemsControl.EqualsEx(obj, base.Value);
				if (flag3)
				{
					base.ChangeValue(obj, true);
					base.Invalidate(isASubPropertyChange);
					this.ValidateOnTargetUpdated();
				}
				base.Clean();
				if (flag3)
				{
					this.OnTargetUpdated();
				}
			}
			base.IsInTransfer = false;
			GC.KeepAlive(targetElement);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x0007600C File Offset: 0x0007420C
		private void ValidateOnTargetUpdated()
		{
			ValidationError validationError = null;
			Collection<ValidationRule> validationRulesInternal = this.ParentBinding.ValidationRulesInternal;
			CultureInfo cultureInfo = null;
			bool flag = this.ParentBinding.ValidatesOnDataErrors;
			if (validationRulesInternal != null)
			{
				object obj = DependencyProperty.UnsetValue;
				object obj2 = DependencyProperty.UnsetValue;
				foreach (ValidationRule validationRule in validationRulesInternal)
				{
					if (validationRule.ValidatesOnTargetUpdated)
					{
						if (validationRule is DataErrorValidationRule)
						{
							flag = false;
						}
						ValidationStep validationStep = validationRule.ValidationStep;
						object value;
						if (validationStep != ValidationStep.RawProposedValue)
						{
							if (validationStep - ValidationStep.ConvertedProposedValue > 2)
							{
								throw new InvalidOperationException(SR.Get("ValidationRule_UnknownStep", new object[]
								{
									validationRule.ValidationStep,
									validationRule
								}));
							}
							if (obj2 == DependencyProperty.UnsetValue)
							{
								obj2 = this.Worker.RawValue();
							}
							value = obj2;
						}
						else
						{
							if (obj == DependencyProperty.UnsetValue)
							{
								obj = this.GetRawProposedValue();
							}
							value = obj;
						}
						if (cultureInfo == null)
						{
							cultureInfo = base.GetCulture();
						}
						validationError = this.RunValidationRule(validationRule, value, cultureInfo);
						if (validationError != null)
						{
							break;
						}
					}
				}
			}
			if (flag && validationError == null)
			{
				if (cultureInfo == null)
				{
					cultureInfo = base.GetCulture();
				}
				validationError = this.RunValidationRule(DataErrorValidationRule.Instance, this, cultureInfo);
			}
			base.UpdateValidationError(validationError, false);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x00076154 File Offset: 0x00074354
		private ValidationError RunValidationRule(ValidationRule validationRule, object value, CultureInfo culture)
		{
			ValidationResult validationResult = validationRule.Validate(value, culture, this);
			ValidationError result;
			if (validationResult.IsValid)
			{
				result = null;
			}
			else
			{
				if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
				{
					TraceData.Trace(TraceEventType.Warning, TraceData.ValidationRuleFailed(new object[]
					{
						TraceData.Identify(this),
						TraceData.Identify(validationRule)
					}));
				}
				result = new ValidationError(validationRule, this, validationResult.ErrorContent, null);
			}
			return result;
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x000761B4 File Offset: 0x000743B4
		private object ConvertHelper(IValueConverter converter, object value, Type targetType, object parameter, CultureInfo culture)
		{
			string effectiveStringFormat = base.EffectiveStringFormat;
			Invariant.Assert(converter != null || effectiveStringFormat != null);
			object result = null;
			try
			{
				if (effectiveStringFormat != null)
				{
					result = string.Format(culture, effectiveStringFormat, new object[]
					{
						value
					});
				}
				else
				{
					result = converter.Convert(value, targetType, parameter, culture);
				}
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				if (TraceData.IsEnabled)
				{
					string text = string.IsNullOrEmpty(effectiveStringFormat) ? converter.GetType().Name : "StringFormat";
					TraceData.Trace(base.TraceLevel, TraceData.BadConverterForTransfer(new object[]
					{
						text,
						AvTrace.ToStringHelper(value),
						AvTrace.TypeName(value)
					}), this, ex);
				}
				result = DependencyProperty.UnsetValue;
			}
			catch
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(base.TraceLevel, TraceData.BadConverterForTransfer(new object[]
					{
						converter.GetType().Name,
						AvTrace.ToStringHelper(value),
						AvTrace.TypeName(value)
					}), this);
				}
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x000762CC File Offset: 0x000744CC
		private object ConvertBackHelper(IValueConverter converter, object value, Type sourceType, object parameter, CultureInfo culture)
		{
			Invariant.Assert(converter != null);
			object result = null;
			try
			{
				result = converter.ConvertBack(value, sourceType, parameter, culture);
			}
			catch (Exception ex)
			{
				ex = CriticalExceptions.Unwrap(ex);
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.BadConverterForUpdate(new object[]
					{
						AvTrace.ToStringHelper(base.Value),
						AvTrace.TypeName(value)
					}), this, ex);
				}
				this.ProcessException(ex, base.ValidatesOnExceptions);
				result = DependencyProperty.UnsetValue;
			}
			catch
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.BadConverterForUpdate(new object[]
					{
						AvTrace.ToStringHelper(base.Value),
						AvTrace.TypeName(value)
					}), this);
				}
				result = DependencyProperty.UnsetValue;
			}
			return result;
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000763A4 File Offset: 0x000745A4
		internal void ScheduleTransfer(bool isASubPropertyChange)
		{
			if (isASubPropertyChange && this.Converter != null)
			{
				isASubPropertyChange = false;
			}
			this.TransferValue(DependencyProperty.UnsetValue, isASubPropertyChange);
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x000763C0 File Offset: 0x000745C0
		private void OnTargetUpdated()
		{
			if (base.NotifyOnTargetUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null && ((!base.IsInMultiBindingExpression && (!base.IsInPriorityBindingExpression || this == base.ParentPriorityBindingExpression.ActiveBindingExpression)) || (base.IsAttaching && (base.StatusInternal == BindingStatusInternal.Active || base.UsingFallbackValue))))
				{
					if (base.IsAttaching && base.RootBindingExpression == targetElement.ReadLocalValue(base.TargetProperty))
					{
						base.Engine.AddTask(this, TaskOps.RaiseTargetUpdatedEvent);
						return;
					}
					BindingExpression.OnTargetUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x0007644C File Offset: 0x0007464C
		private void OnSourceUpdated()
		{
			if (base.NotifyOnSourceUpdated)
			{
				DependencyObject targetElement = base.TargetElement;
				if (targetElement != null && !base.IsInMultiBindingExpression && (!base.IsInPriorityBindingExpression || this == base.ParentPriorityBindingExpression.ActiveBindingExpression))
				{
					BindingExpression.OnSourceUpdated(targetElement, base.TargetProperty);
				}
			}
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x00076495 File Offset: 0x00074695
		internal override bool ShouldReactToDirtyOverride()
		{
			return this.DataItem != BindingExpressionBase.DisconnectedItem;
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x000764A7 File Offset: 0x000746A7
		internal override bool UpdateOverride()
		{
			return !base.NeedsUpdate || !base.IsReflective || base.IsInTransfer || this.Worker == null || !this.Worker.CanUpdate || base.UpdateValue();
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x000764E0 File Offset: 0x000746E0
		internal override object ConvertProposedValue(object value)
		{
			object obj = value;
			bool flag = TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer);
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.UpdateRawValue(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(value)
				}));
			}
			Type sourcePropertyType = this.Worker.SourcePropertyType;
			IValueConverter valueConverter = null;
			CultureInfo culture = base.GetCulture();
			if (this.Converter != null)
			{
				if (!base.UseDefaultValueConverter)
				{
					value = this.Converter.ConvertBack(value, sourcePropertyType, this.ParentBinding.ConverterParameter, culture);
					if (base.IsDetached)
					{
						return Binding.DoNothing;
					}
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.UserConvertBack(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(value)
						}));
					}
					if (value != Binding.DoNothing && value != DependencyProperty.UnsetValue && !this.IsValidValueForUpdate(value, sourcePropertyType))
					{
						valueConverter = this.DynamicConverter;
					}
				}
				else
				{
					valueConverter = this.Converter;
				}
			}
			if (value != Binding.DoNothing && value != DependencyProperty.UnsetValue)
			{
				if (BindingExpressionBase.IsNullValue(value))
				{
					if (value == null || !this.IsValidValueForUpdate(value, sourcePropertyType))
					{
						if (this.Worker.IsDBNullValidForUpdate)
						{
							value = DBNull.Value;
						}
						else
						{
							value = base.NullValueForType(sourcePropertyType);
						}
					}
				}
				else if (valueConverter != null)
				{
					value = this.ConvertBackHelper(valueConverter, value, sourcePropertyType, base.TargetElement, culture);
					if (flag)
					{
						TraceData.Trace(TraceEventType.Warning, TraceData.DefaultConvertBack(new object[]
						{
							TraceData.Identify(this),
							TraceData.Identify(value)
						}));
					}
				}
			}
			if (flag)
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.Update(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(value)
				}));
			}
			if (value == DependencyProperty.UnsetValue && this.ValidationError == null)
			{
				ValidationError validationError = new ValidationError(ConversionValidationRule.Instance, this, SR.Get("Validation_ConversionFailed", new object[]
				{
					obj
				}), null);
				base.UpdateValidationError(validationError, false);
			}
			return value;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x000766A4 File Offset: 0x000748A4
		internal override bool ObtainConvertedProposedValue(BindingGroup bindingGroup)
		{
			bool result = true;
			object obj;
			if (base.NeedsUpdate)
			{
				obj = bindingGroup.GetValue(this);
				if (obj != DependencyProperty.UnsetValue)
				{
					obj = this.ConvertProposedValue(obj);
					if (obj == DependencyProperty.UnsetValue)
					{
						result = false;
					}
				}
			}
			else
			{
				obj = BindingGroup.DeferredSourceValue;
			}
			bindingGroup.SetValue(this, obj);
			return result;
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x000766F0 File Offset: 0x000748F0
		internal override object UpdateSource(object value)
		{
			if (value == DependencyProperty.UnsetValue)
			{
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
			}
			if (value == Binding.DoNothing || value == DependencyProperty.UnsetValue || this.ShouldIgnoreUpdate())
			{
				return value;
			}
			try
			{
				base.BeginSourceUpdate();
				this.Worker.UpdateValue(value);
			}
			catch (Exception ex)
			{
				ex = CriticalExceptions.Unwrap(ex);
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.WorkerUpdateFailed, this, ex);
				}
				this.ProcessException(ex, base.ValidatesOnExceptions || base.BindingGroup != null);
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				value = DependencyProperty.UnsetValue;
			}
			catch
			{
				if (TraceData.IsEnabled)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.WorkerUpdateFailed, this);
				}
				base.SetStatus(BindingStatusInternal.UpdateSourceError);
				value = DependencyProperty.UnsetValue;
			}
			finally
			{
				base.EndSourceUpdate();
			}
			this.OnSourceUpdated();
			return value;
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x000767E8 File Offset: 0x000749E8
		internal override bool UpdateSource(BindingGroup bindingGroup)
		{
			bool result = true;
			if (base.NeedsUpdate)
			{
				object obj = bindingGroup.GetValue(this);
				obj = this.UpdateSource(obj);
				bindingGroup.SetValue(this, obj);
				if (obj == DependencyProperty.UnsetValue)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x00076822 File Offset: 0x00074A22
		internal override void StoreValueInBindingGroup(object value, BindingGroup bindingGroup)
		{
			bindingGroup.SetValue(this, value);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x0007682C File Offset: 0x00074A2C
		internal override bool Validate(object value, ValidationStep validationStep)
		{
			bool flag = base.Validate(value, validationStep);
			if (validationStep == ValidationStep.UpdatedValue)
			{
				if (flag && this.ParentBinding.ValidatesOnDataErrors)
				{
					ValidationError validationError = base.GetValidationErrors(validationStep);
					if (validationError != null && validationError.RuleInError != DataErrorValidationRule.Instance)
					{
						validationError = null;
					}
					ValidationError validationError2 = this.RunValidationRule(DataErrorValidationRule.Instance, this, base.GetCulture());
					if (validationError2 != null)
					{
						base.UpdateValidationError(validationError2, false);
						flag = false;
					}
					else if (validationError != null)
					{
						base.UpdateValidationError(null, false);
					}
				}
			}
			else if (validationStep == ValidationStep.CommittedValue && flag)
			{
				base.NeedsValidation = false;
			}
			return flag;
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x000768B0 File Offset: 0x00074AB0
		internal override bool CheckValidationRules(BindingGroup bindingGroup, ValidationStep validationStep)
		{
			if (!base.NeedsValidation)
			{
				return true;
			}
			object value;
			switch (validationStep)
			{
			case ValidationStep.RawProposedValue:
				value = this.GetRawProposedValue();
				break;
			case ValidationStep.ConvertedProposedValue:
				value = bindingGroup.GetValue(this);
				break;
			case ValidationStep.UpdatedValue:
			case ValidationStep.CommittedValue:
				value = this;
				break;
			default:
				throw new InvalidOperationException(SR.Get("ValidationRule_UnknownStep", new object[]
				{
					validationStep,
					bindingGroup
				}));
			}
			return this.Validate(value, validationStep);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x00076924 File Offset: 0x00074B24
		internal override bool ValidateAndConvertProposedValue(out Collection<BindingExpressionBase.ProposedValue> values)
		{
			values = null;
			object rawProposedValue = this.GetRawProposedValue();
			bool flag = this.Validate(rawProposedValue, ValidationStep.RawProposedValue);
			object obj = flag ? this.ConvertProposedValue(rawProposedValue) : DependencyProperty.UnsetValue;
			if (obj == Binding.DoNothing)
			{
				obj = DependencyProperty.UnsetValue;
			}
			else
			{
				flag = (obj != DependencyProperty.UnsetValue && this.Validate(obj, ValidationStep.ConvertedProposedValue));
			}
			values = new Collection<BindingExpressionBase.ProposedValue>();
			values.Add(new BindingExpressionBase.ProposedValue(this, rawProposedValue, obj));
			return flag;
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x00076992 File Offset: 0x00074B92
		private bool IsValidValueForUpdate(object value, Type sourceType)
		{
			return value == null || sourceType.IsAssignableFrom(value.GetType()) || (Convert.IsDBNull(value) && this.Worker.IsDBNullValidForUpdate);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x000769C0 File Offset: 0x00074BC0
		private void ProcessException(Exception ex, bool validate)
		{
			object obj = null;
			ValidationError validationError = null;
			if (this.ExceptionFilterExists())
			{
				obj = this.CallDoFilterException(ex);
				if (obj == null)
				{
					return;
				}
				validationError = (obj as ValidationError);
			}
			if (validationError == null && validate)
			{
				ValidationRule instance = ExceptionValidationRule.Instance;
				if (obj == null)
				{
					validationError = new ValidationError(instance, this, ex.Message, ex);
				}
				else
				{
					validationError = new ValidationError(instance, this, obj, ex);
				}
			}
			if (validationError != null)
			{
				base.UpdateValidationError(validationError, false);
			}
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x00076A24 File Offset: 0x00074C24
		private bool ShouldIgnoreUpdate()
		{
			if (base.TargetProperty.OwnerType != typeof(Selector) && base.TargetProperty != ComboBox.TextProperty)
			{
				return false;
			}
			DependencyObject contextElement = this.ContextElement;
			object obj;
			if (contextElement == null)
			{
				DependencyObject dependencyObject = base.TargetElement;
				if (dependencyObject != null && base.UsingMentor)
				{
					dependencyObject = Helper.FindMentor(dependencyObject);
				}
				if (dependencyObject == null)
				{
					return true;
				}
				obj = this.ParentBinding.SourceReference.GetDataObject(dependencyObject, new ObjectRefArgs
				{
					ResolveNamesInTemplate = base.ResolveNamesInTemplate
				});
			}
			else
			{
				obj = contextElement.GetValue(FrameworkElement.DataContextProperty);
			}
			if (!this.ParentBinding.BindsDirectlyToSource)
			{
				CollectionViewSource collectionViewSource;
				DataSourceProvider dataSourceProvider;
				if ((collectionViewSource = (obj as CollectionViewSource)) != null)
				{
					obj = collectionViewSource.CollectionView;
				}
				else if ((dataSourceProvider = (obj as DataSourceProvider)) != null)
				{
					obj = dataSourceProvider.Data;
				}
			}
			return !ItemsControl.EqualsEx(this.DataItem, obj) || !this.Worker.IsPathCurrent();
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x00076B08 File Offset: 0x00074D08
		internal void UpdateNotifyDataErrors(INotifyDataErrorInfo indei, string propertyName, object value)
		{
			if (!base.ValidatesOnNotifyDataErrors || base.IsDetached)
			{
				return;
			}
			WeakReference weakReference = (WeakReference)base.GetValue(BindingExpressionBase.Feature.DataErrorValue, null);
			INotifyDataErrorInfo notifyDataErrorInfo = (weakReference == null) ? null : (weakReference.Target as INotifyDataErrorInfo);
			if (value != DependencyProperty.UnsetValue && value != notifyDataErrorInfo && base.IsDynamic)
			{
				if (notifyDataErrorInfo != null)
				{
					ErrorsChangedEventManager.RemoveHandler(notifyDataErrorInfo, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
				}
				INotifyDataErrorInfo notifyDataErrorInfo2 = value as INotifyDataErrorInfo;
				object value2 = BindingExpressionBase.ReplaceReference(weakReference, notifyDataErrorInfo2);
				base.SetValue(BindingExpressionBase.Feature.DataErrorValue, value2, null);
				notifyDataErrorInfo = notifyDataErrorInfo2;
				if (notifyDataErrorInfo2 != null)
				{
					ErrorsChangedEventManager.AddHandler(notifyDataErrorInfo2, new EventHandler<DataErrorsChangedEventArgs>(this.OnErrorsChanged));
				}
			}
			base.IsDataErrorsChangedPending = false;
			try
			{
				List<object> dataErrors = BindingExpression.GetDataErrors(indei, propertyName);
				List<object> dataErrors2 = BindingExpression.GetDataErrors(notifyDataErrorInfo, string.Empty);
				List<object> errors = this.MergeErrors(dataErrors, dataErrors2);
				base.UpdateNotifyDataErrorValidationErrors(errors);
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x00076BF4 File Offset: 0x00074DF4
		private void UpdateNotifyDataErrors(object value)
		{
			if (!base.ValidatesOnNotifyDataErrors)
			{
				return;
			}
			this.UpdateNotifyDataErrors(this.SourceItem as INotifyDataErrorInfo, this.SourcePropertyName, value);
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x00076C18 File Offset: 0x00074E18
		internal static List<object> GetDataErrors(INotifyDataErrorInfo indei, string propertyName)
		{
			List<object> list = null;
			if (indei != null && indei.HasErrors)
			{
				for (int i = 3; i >= 0; i--)
				{
					try
					{
						list = new List<object>();
						IEnumerable errors = indei.GetErrors(propertyName);
						if (errors != null)
						{
							foreach (object item in errors)
							{
								list.Add(item);
							}
						}
						break;
					}
					catch (InvalidOperationException)
					{
						if (i == 0)
						{
							throw;
						}
					}
				}
			}
			if (list != null && list.Count == 0)
			{
				list = null;
			}
			return list;
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x00076CBC File Offset: 0x00074EBC
		private List<object> MergeErrors(List<object> list1, List<object> list2)
		{
			if (list1 == null)
			{
				return list2;
			}
			if (list2 == null)
			{
				return list1;
			}
			foreach (object item in list2)
			{
				list1.Add(item);
			}
			return list1;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x00076D18 File Offset: 0x00074F18
		private void OnDataContextChanged(DependencyObject contextElement)
		{
			if (!base.IsInUpdate && this.CanActivate)
			{
				if (base.IsReflective && base.RootBindingExpression.ParentBindingBase.BindingGroupName == string.Empty)
				{
					base.RejoinBindingGroup(base.IsReflective, contextElement);
				}
				object value = contextElement.GetValue(FrameworkElement.DataContextProperty);
				if (!ItemsControl.EqualsEx(this.DataItem, value))
				{
					this.Activate(value);
				}
			}
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x00076D88 File Offset: 0x00074F88
		internal void OnCurrentChanged(object sender, EventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"CurrentChanged",
					TraceData.Identify(sender)
				}));
			}
			this.Worker.OnCurrentChanged(sender as ICollectionView, e);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x00076DDB File Offset: 0x00074FDB
		internal void OnCurrentChanging(object sender, CurrentChangingEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"CurrentChanging",
					TraceData.Identify(sender)
				}));
			}
			base.Update();
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x00076E18 File Offset: 0x00075018
		private void OnDataChanged(object sender, EventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"DataChanged",
					TraceData.Identify(sender)
				}));
			}
			this.Activate(sender);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x00076E58 File Offset: 0x00075058
		private void OnInheritanceContextChanged(object sender, EventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"InheritanceContextChanged",
					TraceData.Identify(sender)
				}));
			}
			if (base.StatusInternal == BindingStatusInternal.Unattached)
			{
				base.Engine.CancelTask(this, TaskOps.AttachToContext);
				this.AttachToContext(BindingExpression.AttachAttempt.Again);
				if (base.StatusInternal == BindingStatusInternal.Unattached)
				{
					base.Engine.AddTask(this, TaskOps.AttachToContext);
					return;
				}
			}
			else
			{
				this.AttachToContext(BindingExpression.AttachAttempt.Last);
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x00076ED2 File Offset: 0x000750D2
		internal override void OnLostFocus(object sender, RoutedEventArgs e)
		{
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotEvent(new object[]
				{
					TraceData.Identify(this),
					"LostFocus",
					TraceData.Identify(sender)
				}));
			}
			base.Update();
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x00076F10 File Offset: 0x00075110
		private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
		{
			if (base.Dispatcher.Thread == Thread.CurrentThread)
			{
				this.UpdateNotifyDataErrors(DependencyProperty.UnsetValue);
				return;
			}
			if (!base.IsDataErrorsChangedPending)
			{
				base.IsDataErrorsChangedPending = true;
				base.Engine.Marshal(delegate(object arg)
				{
					this.UpdateNotifyDataErrors(DependencyProperty.UnsetValue);
					return null;
				}, null, 1);
			}
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <param name="managerType">The type of the <see cref="T:System.Windows.WeakEventManager" /> calling this method. This only recognizes manager objects of type <see cref="T:System.Collections.Specialized.CollectionChangedEventManager" />.</param>
		/// <param name="sender">Object that originated the event.</param>
		/// <param name="e">Event data.</param>
		/// <returns>
		///     <see langword="true" /> if the listener handled the event; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600189A RID: 6298 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			return false;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x00076F64 File Offset: 0x00075164
		private object CallDoFilterException(Exception ex)
		{
			if (this.ParentBinding.UpdateSourceExceptionFilter != null)
			{
				return this.ParentBinding.DoFilterException(this, ex);
			}
			if (base.IsInMultiBindingExpression)
			{
				return base.ParentMultiBindingExpression.ParentMultiBinding.DoFilterException(this, ex);
			}
			return null;
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x00076F9D File Offset: 0x0007519D
		private bool ExceptionFilterExists()
		{
			return this.ParentBinding.UpdateSourceExceptionFilter != null || (base.IsInMultiBindingExpression && base.ParentMultiBindingExpression.ParentMultiBinding.UpdateSourceExceptionFilter != null);
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x00076FCB File Offset: 0x000751CB
		internal IDisposable ChangingValue()
		{
			return new BindingExpression.ChangingValueHelper(this);
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x00076FD3 File Offset: 0x000751D3
		internal void CancelPendingTasks()
		{
			base.Engine.CancelTasks(this);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x00076FE4 File Offset: 0x000751E4
		private void Replace()
		{
			DependencyObject targetElement = base.TargetElement;
			if (targetElement != null)
			{
				if (base.IsInBindingExpressionCollection)
				{
					base.ParentBindingExpressionBase.ReplaceChild(this);
					return;
				}
				BindingOperations.SetBinding(targetElement, base.TargetProperty, this.ParentBinding);
			}
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x00077024 File Offset: 0x00075224
		internal static void OnTargetUpdated(DependencyObject d, DependencyProperty dp)
		{
			DataTransferEventArgs dataTransferEventArgs = new DataTransferEventArgs(d, dp);
			dataTransferEventArgs.RoutedEvent = Binding.TargetUpdatedEvent;
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (!frameworkObject.IsValid && d != null)
			{
				frameworkObject.Reset(Helper.FindMentor(d));
			}
			frameworkObject.RaiseEvent(dataTransferEventArgs);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x00077070 File Offset: 0x00075270
		internal static void OnSourceUpdated(DependencyObject d, DependencyProperty dp)
		{
			DataTransferEventArgs dataTransferEventArgs = new DataTransferEventArgs(d, dp);
			dataTransferEventArgs.RoutedEvent = Binding.SourceUpdatedEvent;
			FrameworkObject frameworkObject = new FrameworkObject(d);
			if (!frameworkObject.IsValid && d != null)
			{
				frameworkObject.Reset(Helper.FindMentor(d));
			}
			frameworkObject.RaiseEvent(dataTransferEventArgs);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x000770BC File Offset: 0x000752BC
		internal override void HandlePropertyInvalidation(DependencyObject d, DependencyPropertyChangedEventArgs args)
		{
			DependencyProperty property = args.Property;
			if (TraceData.IsExtendedTraceEnabled(this, TraceDataLevel.Transfer))
			{
				TraceData.Trace(TraceEventType.Warning, TraceData.GotPropertyChanged(new object[]
				{
					TraceData.Identify(this),
					TraceData.Identify(d),
					property.Name
				}));
			}
			if (property == FrameworkElement.DataContextProperty)
			{
				DependencyObject contextElement = this.ContextElement;
				if (d == contextElement)
				{
					base.IsTransferPending = false;
					this.OnDataContextChanged(contextElement);
				}
			}
			if (property == CollectionViewSource.ViewProperty)
			{
				CollectionViewSource collectionViewSource = this.CollectionViewSource;
				if (d == collectionViewSource)
				{
					this.Activate(collectionViewSource);
				}
			}
			if (property == FrameworkElement.LanguageProperty && base.UsesLanguage && d == base.TargetElement)
			{
				base.InvalidateCulture();
				this.TransferValue();
			}
			if (this.Worker != null)
			{
				this.Worker.OnSourceInvalidation(d, property, args.IsASubPropertyChange);
			}
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x00077182 File Offset: 0x00075382
		private void SetDataItem(object newItem)
		{
			this._dataItem = BindingExpressionBase.CreateReference(newItem);
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x00077190 File Offset: 0x00075390
		private object GetDataSourceForDataContext(DependencyObject d)
		{
			BindingExpression bindingExpression = null;
			for (DependencyObject dependencyObject = d; dependencyObject != null; dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject))
			{
				if (BindingExpression.HasLocalDataContext(dependencyObject))
				{
					bindingExpression = BindingOperations.GetBindingExpression(dependencyObject, FrameworkElement.DataContextProperty);
					break;
				}
			}
			if (bindingExpression != null)
			{
				return bindingExpression.DataSource;
			}
			return null;
		}

		// Token: 0x04001304 RID: 4868
		private WeakReference _ctxElement;

		// Token: 0x04001305 RID: 4869
		private object _dataItem;

		// Token: 0x04001306 RID: 4870
		private BindingWorker _worker;

		// Token: 0x04001307 RID: 4871
		private Type _sourceType;

		// Token: 0x04001308 RID: 4872
		internal static readonly object NullDataItem = new NamedObject("NullDataItem");

		// Token: 0x04001309 RID: 4873
		internal static readonly object IgnoreDefaultValue = new NamedObject("IgnoreDefaultValue");

		// Token: 0x0400130A RID: 4874
		internal static readonly object StaticSource = new NamedObject("StaticSource");

		// Token: 0x02000863 RID: 2147
		internal enum SourceType
		{
			// Token: 0x040040C1 RID: 16577
			Unknown,
			// Token: 0x040040C2 RID: 16578
			CLR,
			// Token: 0x040040C3 RID: 16579
			XML
		}

		// Token: 0x02000864 RID: 2148
		private enum AttachAttempt
		{
			// Token: 0x040040C5 RID: 16581
			First,
			// Token: 0x040040C6 RID: 16582
			Again,
			// Token: 0x040040C7 RID: 16583
			Last
		}

		// Token: 0x02000865 RID: 2149
		private class ChangingValueHelper : IDisposable
		{
			// Token: 0x060082CE RID: 33486 RVA: 0x00243C73 File Offset: 0x00241E73
			internal ChangingValueHelper(BindingExpression b)
			{
				this._bindingExpression = b;
				b.CancelPendingTasks();
			}

			// Token: 0x060082CF RID: 33487 RVA: 0x00243C88 File Offset: 0x00241E88
			public void Dispose()
			{
				this._bindingExpression.TransferValue();
				GC.SuppressFinalize(this);
			}

			// Token: 0x040040C8 RID: 16584
			private BindingExpression _bindingExpression;
		}
	}
}
