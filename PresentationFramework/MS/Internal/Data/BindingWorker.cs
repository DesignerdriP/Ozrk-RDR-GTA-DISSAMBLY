using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x0200070A RID: 1802
	internal abstract class BindingWorker
	{
		// Token: 0x06007373 RID: 29555 RVA: 0x00211297 File Offset: 0x0020F497
		protected BindingWorker(BindingExpression b)
		{
			this._bindingExpression = b;
		}

		// Token: 0x17001B69 RID: 7017
		// (get) Token: 0x06007374 RID: 29556 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual Type SourcePropertyType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B6A RID: 7018
		// (get) Token: 0x06007375 RID: 29557 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool CanUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x06007376 RID: 29558 RVA: 0x002112A6 File Offset: 0x0020F4A6
		internal BindingExpression ParentBindingExpression
		{
			get
			{
				return this._bindingExpression;
			}
		}

		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x06007377 RID: 29559 RVA: 0x002112AE File Offset: 0x0020F4AE
		internal Type TargetPropertyType
		{
			get
			{
				return this.TargetProperty.PropertyType;
			}
		}

		// Token: 0x17001B6D RID: 7021
		// (get) Token: 0x06007378 RID: 29560 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsDBNullValidForUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x06007379 RID: 29561 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object SourceItem
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x0600737A RID: 29562 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual string SourcePropertyName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600737B RID: 29563 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void AttachDataItem()
		{
		}

		// Token: 0x0600737C RID: 29564 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void DetachDataItem()
		{
		}

		// Token: 0x0600737D RID: 29565 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnCurrentChanged(ICollectionView collectionView, EventArgs args)
		{
		}

		// Token: 0x0600737E RID: 29566 RVA: 0x0000C238 File Offset: 0x0000A438
		internal virtual object RawValue()
		{
			return null;
		}

		// Token: 0x0600737F RID: 29567 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void UpdateValue(object value)
		{
		}

		// Token: 0x06007380 RID: 29568 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void RefreshValue()
		{
		}

		// Token: 0x06007381 RID: 29569 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool UsesDependencyProperty(DependencyObject d, DependencyProperty dp)
		{
			return false;
		}

		// Token: 0x06007382 RID: 29570 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnSourceInvalidation(DependencyObject d, DependencyProperty dp, bool isASubPropertyChange)
		{
		}

		// Token: 0x06007383 RID: 29571 RVA: 0x00016748 File Offset: 0x00014948
		internal virtual bool IsPathCurrent()
		{
			return true;
		}

		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x06007384 RID: 29572 RVA: 0x002112BB File Offset: 0x0020F4BB
		protected Binding ParentBinding
		{
			get
			{
				return this.ParentBindingExpression.ParentBinding;
			}
		}

		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x06007385 RID: 29573 RVA: 0x002112C8 File Offset: 0x0020F4C8
		protected bool IsDynamic
		{
			get
			{
				return this.ParentBindingExpression.IsDynamic;
			}
		}

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x06007386 RID: 29574 RVA: 0x002112D5 File Offset: 0x0020F4D5
		internal bool IsReflective
		{
			get
			{
				return this.ParentBindingExpression.IsReflective;
			}
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x06007387 RID: 29575 RVA: 0x002112E2 File Offset: 0x0020F4E2
		protected bool IgnoreSourcePropertyChange
		{
			get
			{
				return this.ParentBindingExpression.IgnoreSourcePropertyChange;
			}
		}

		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x06007388 RID: 29576 RVA: 0x002112EF File Offset: 0x0020F4EF
		protected object DataItem
		{
			get
			{
				return this.ParentBindingExpression.DataItem;
			}
		}

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x06007389 RID: 29577 RVA: 0x002112FC File Offset: 0x0020F4FC
		protected DependencyObject TargetElement
		{
			get
			{
				return this.ParentBindingExpression.TargetElement;
			}
		}

		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x0600738A RID: 29578 RVA: 0x00211309 File Offset: 0x0020F509
		protected DependencyProperty TargetProperty
		{
			get
			{
				return this.ParentBindingExpression.TargetProperty;
			}
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x0600738B RID: 29579 RVA: 0x00211316 File Offset: 0x0020F516
		protected DataBindEngine Engine
		{
			get
			{
				return this.ParentBindingExpression.Engine;
			}
		}

		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x0600738C RID: 29580 RVA: 0x00211323 File Offset: 0x0020F523
		protected Dispatcher Dispatcher
		{
			get
			{
				return this.ParentBindingExpression.Dispatcher;
			}
		}

		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x0600738D RID: 29581 RVA: 0x00211330 File Offset: 0x0020F530
		// (set) Token: 0x0600738E RID: 29582 RVA: 0x0021133D File Offset: 0x0020F53D
		protected BindingStatusInternal Status
		{
			get
			{
				return this.ParentBindingExpression.StatusInternal;
			}
			set
			{
				this.ParentBindingExpression.SetStatus(value);
			}
		}

		// Token: 0x0600738F RID: 29583 RVA: 0x0021134B File Offset: 0x0020F54B
		protected void SetTransferIsPending(bool value)
		{
			this.ParentBindingExpression.IsTransferPending = value;
		}

		// Token: 0x06007390 RID: 29584 RVA: 0x00211359 File Offset: 0x0020F559
		internal bool HasValue(BindingWorker.Feature id)
		{
			return this._values.HasValue((int)id);
		}

		// Token: 0x06007391 RID: 29585 RVA: 0x00211367 File Offset: 0x0020F567
		internal object GetValue(BindingWorker.Feature id, object defaultValue)
		{
			return this._values.GetValue((int)id, defaultValue);
		}

		// Token: 0x06007392 RID: 29586 RVA: 0x00211376 File Offset: 0x0020F576
		internal void SetValue(BindingWorker.Feature id, object value)
		{
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06007393 RID: 29587 RVA: 0x00211385 File Offset: 0x0020F585
		internal void SetValue(BindingWorker.Feature id, object value, object defaultValue)
		{
			if (object.Equals(value, defaultValue))
			{
				this._values.ClearValue((int)id);
				return;
			}
			this._values.SetValue((int)id, value);
		}

		// Token: 0x06007394 RID: 29588 RVA: 0x002113AA File Offset: 0x0020F5AA
		internal void ClearValue(BindingWorker.Feature id)
		{
			this._values.ClearValue((int)id);
		}

		// Token: 0x040037AA RID: 14250
		private BindingExpression _bindingExpression;

		// Token: 0x040037AB RID: 14251
		private UncommonValueTable _values;

		// Token: 0x02000B41 RID: 2881
		internal enum Feature
		{
			// Token: 0x04004ABF RID: 19135
			XmlWorker,
			// Token: 0x04004AC0 RID: 19136
			PendingGetValueRequest,
			// Token: 0x04004AC1 RID: 19137
			PendingSetValueRequest,
			// Token: 0x04004AC2 RID: 19138
			LastFeatureId
		}
	}
}
