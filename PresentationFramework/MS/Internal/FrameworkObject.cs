using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MS.Internal
{
	// Token: 0x020005E6 RID: 1510
	internal struct FrameworkObject
	{
		// Token: 0x0600647D RID: 25725 RVA: 0x001C3300 File Offset: 0x001C1500
		internal FrameworkObject(DependencyObject d)
		{
			this._do = d;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				this._fe = (FrameworkElement)d;
				this._fce = null;
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				this._fe = null;
				this._fce = (FrameworkContentElement)d;
				return;
			}
			this._fe = null;
			this._fce = null;
		}

		// Token: 0x0600647E RID: 25726 RVA: 0x001C3364 File Offset: 0x001C1564
		internal FrameworkObject(DependencyObject d, bool throwIfNeither)
		{
			this = new FrameworkObject(d);
			if (throwIfNeither && this._fe == null && this._fce == null)
			{
				object obj = (d != null) ? d.GetType() : "NULL";
				throw new InvalidOperationException(SR.Get("MustBeFrameworkDerived", new object[]
				{
					obj
				}));
			}
		}

		// Token: 0x0600647F RID: 25727 RVA: 0x001C33B6 File Offset: 0x001C15B6
		internal FrameworkObject(FrameworkElement fe, FrameworkContentElement fce)
		{
			this._fe = fe;
			this._fce = fce;
			if (fe != null)
			{
				this._do = fe;
				return;
			}
			this._do = fce;
		}

		// Token: 0x06006480 RID: 25728 RVA: 0x001C33D8 File Offset: 0x001C15D8
		internal void Reset(DependencyObject d)
		{
			this._do = d;
			if (FrameworkElement.DType.IsInstanceOfType(d))
			{
				this._fe = (FrameworkElement)d;
				this._fce = null;
				return;
			}
			if (FrameworkContentElement.DType.IsInstanceOfType(d))
			{
				this._fe = null;
				this._fce = (FrameworkContentElement)d;
				return;
			}
			this._fe = null;
			this._fce = null;
		}

		// Token: 0x17001826 RID: 6182
		// (get) Token: 0x06006481 RID: 25729 RVA: 0x001C343C File Offset: 0x001C163C
		internal FrameworkElement FE
		{
			get
			{
				return this._fe;
			}
		}

		// Token: 0x17001827 RID: 6183
		// (get) Token: 0x06006482 RID: 25730 RVA: 0x001C3444 File Offset: 0x001C1644
		internal FrameworkContentElement FCE
		{
			get
			{
				return this._fce;
			}
		}

		// Token: 0x17001828 RID: 6184
		// (get) Token: 0x06006483 RID: 25731 RVA: 0x001C344C File Offset: 0x001C164C
		internal DependencyObject DO
		{
			get
			{
				return this._do;
			}
		}

		// Token: 0x17001829 RID: 6185
		// (get) Token: 0x06006484 RID: 25732 RVA: 0x001C3454 File Offset: 0x001C1654
		internal bool IsFE
		{
			get
			{
				return this._fe != null;
			}
		}

		// Token: 0x1700182A RID: 6186
		// (get) Token: 0x06006485 RID: 25733 RVA: 0x001C345F File Offset: 0x001C165F
		internal bool IsFCE
		{
			get
			{
				return this._fce != null;
			}
		}

		// Token: 0x1700182B RID: 6187
		// (get) Token: 0x06006486 RID: 25734 RVA: 0x001C346A File Offset: 0x001C166A
		internal bool IsValid
		{
			get
			{
				return this._fe != null || this._fce != null;
			}
		}

		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06006487 RID: 25735 RVA: 0x001C347F File Offset: 0x001C167F
		internal DependencyObject Parent
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.Parent;
				}
				if (this.IsFCE)
				{
					return this._fce.Parent;
				}
				return null;
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06006488 RID: 25736 RVA: 0x001C34AA File Offset: 0x001C16AA
		internal int TemplateChildIndex
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.TemplateChildIndex;
				}
				if (this.IsFCE)
				{
					return this._fce.TemplateChildIndex;
				}
				return -1;
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x06006489 RID: 25737 RVA: 0x001C34D5 File Offset: 0x001C16D5
		internal DependencyObject TemplatedParent
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.TemplatedParent;
				}
				if (this.IsFCE)
				{
					return this._fce.TemplatedParent;
				}
				return null;
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x0600648A RID: 25738 RVA: 0x001C3500 File Offset: 0x001C1700
		internal Style ThemeStyle
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.ThemeStyle;
				}
				if (this.IsFCE)
				{
					return this._fce.ThemeStyle;
				}
				return null;
			}
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x0600648B RID: 25739 RVA: 0x001C352B File Offset: 0x001C172B
		internal XmlLanguage Language
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.Language;
				}
				if (this.IsFCE)
				{
					return this._fce.Language;
				}
				return null;
			}
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x0600648C RID: 25740 RVA: 0x001C3556 File Offset: 0x001C1756
		internal FrameworkTemplate TemplateInternal
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.TemplateInternal;
				}
				return null;
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x0600648D RID: 25741 RVA: 0x001C3570 File Offset: 0x001C1770
		internal FrameworkObject FrameworkParent
		{
			get
			{
				if (this.IsFE)
				{
					DependencyObject dependencyObject = this._fe.ContextVerifiedGetParent();
					if (dependencyObject != null)
					{
						Invariant.Assert(dependencyObject is FrameworkElement || dependencyObject is FrameworkContentElement);
						if (this._fe.IsParentAnFE)
						{
							return new FrameworkObject((FrameworkElement)dependencyObject, null);
						}
						return new FrameworkObject(null, (FrameworkContentElement)dependencyObject);
					}
					else
					{
						FrameworkObject containingFrameworkElement = FrameworkObject.GetContainingFrameworkElement(this._fe.InternalVisualParent);
						if (containingFrameworkElement.IsValid)
						{
							return containingFrameworkElement;
						}
						containingFrameworkElement.Reset(this._fe.GetUIParentCore());
						if (containingFrameworkElement.IsValid)
						{
							return containingFrameworkElement;
						}
						containingFrameworkElement.Reset(Helper.FindMentor(this._fe.InheritanceContext));
						return containingFrameworkElement;
					}
				}
				else
				{
					if (!this.IsFCE)
					{
						return FrameworkObject.GetContainingFrameworkElement(this._do);
					}
					DependencyObject parent = this._fce.Parent;
					if (parent != null)
					{
						Invariant.Assert(parent is FrameworkElement || parent is FrameworkContentElement);
						if (this._fce.IsParentAnFE)
						{
							return new FrameworkObject((FrameworkElement)parent, null);
						}
						return new FrameworkObject(null, (FrameworkContentElement)parent);
					}
					else
					{
						parent = ContentOperations.GetParent(this._fce);
						FrameworkObject containingFrameworkElement2 = FrameworkObject.GetContainingFrameworkElement(parent);
						if (containingFrameworkElement2.IsValid)
						{
							return containingFrameworkElement2;
						}
						containingFrameworkElement2.Reset(Helper.FindMentor(this._fce.InheritanceContext));
						return containingFrameworkElement2;
					}
				}
			}
		}

		// Token: 0x0600648E RID: 25742 RVA: 0x001C36C8 File Offset: 0x001C18C8
		internal static FrameworkObject GetContainingFrameworkElement(DependencyObject current)
		{
			FrameworkObject result = new FrameworkObject(current);
			while (!result.IsValid && result.DO != null)
			{
				Visual reference;
				ContentElement reference2;
				Visual3D reference3;
				if ((reference = (result.DO as Visual)) != null)
				{
					result.Reset(VisualTreeHelper.GetParent(reference));
				}
				else if ((reference2 = (result.DO as ContentElement)) != null)
				{
					result.Reset(ContentOperations.GetParent(reference2));
				}
				else if ((reference3 = (result.DO as Visual3D)) != null)
				{
					result.Reset(VisualTreeHelper.GetParent(reference3));
				}
				else
				{
					result.Reset(null);
				}
			}
			return result;
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x0600648F RID: 25743 RVA: 0x001C3757 File Offset: 0x001C1957
		// (set) Token: 0x06006490 RID: 25744 RVA: 0x001C3782 File Offset: 0x001C1982
		internal Style Style
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.Style;
				}
				if (this.IsFCE)
				{
					return this._fce.Style;
				}
				return null;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.Style = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Style = value;
				}
			}
		}

		// Token: 0x17001834 RID: 6196
		// (get) Token: 0x06006491 RID: 25745 RVA: 0x001C37AD File Offset: 0x001C19AD
		// (set) Token: 0x06006492 RID: 25746 RVA: 0x001C37D8 File Offset: 0x001C19D8
		internal bool IsStyleSetFromGenerator
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.IsStyleSetFromGenerator;
				}
				return this.IsFCE && this._fce.IsStyleSetFromGenerator;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.IsStyleSetFromGenerator = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.IsStyleSetFromGenerator = value;
				}
			}
		}

		// Token: 0x17001835 RID: 6197
		// (get) Token: 0x06006493 RID: 25747 RVA: 0x001C3804 File Offset: 0x001C1A04
		internal DependencyObject EffectiveParent
		{
			get
			{
				DependencyObject dependencyObject;
				Visual reference;
				ContentElement reference2;
				Visual3D reference3;
				if (this.IsFE)
				{
					dependencyObject = VisualTreeHelper.GetParent(this._fe);
				}
				else if (this.IsFCE)
				{
					dependencyObject = this._fce.Parent;
				}
				else if ((reference = (this._do as Visual)) != null)
				{
					dependencyObject = VisualTreeHelper.GetParent(reference);
				}
				else if ((reference2 = (this._do as ContentElement)) != null)
				{
					dependencyObject = ContentOperations.GetParent(reference2);
				}
				else if ((reference3 = (this._do as Visual3D)) != null)
				{
					dependencyObject = VisualTreeHelper.GetParent(reference3);
				}
				else
				{
					dependencyObject = null;
				}
				if (dependencyObject == null && this._do != null)
				{
					dependencyObject = this._do.InheritanceContext;
				}
				return dependencyObject;
			}
		}

		// Token: 0x17001836 RID: 6198
		// (get) Token: 0x06006494 RID: 25748 RVA: 0x001C389F File Offset: 0x001C1A9F
		internal FrameworkObject PreferVisualParent
		{
			get
			{
				return this.GetPreferVisualParent(false);
			}
		}

		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x06006495 RID: 25749 RVA: 0x001C38A8 File Offset: 0x001C1AA8
		internal bool IsLoaded
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.IsLoaded;
				}
				if (this.IsFCE)
				{
					return this._fce.IsLoaded;
				}
				return BroadcastEventHelper.IsParentLoaded(this._do);
			}
		}

		// Token: 0x17001838 RID: 6200
		// (get) Token: 0x06006496 RID: 25750 RVA: 0x001C38DD File Offset: 0x001C1ADD
		internal bool IsInitialized
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.IsInitialized;
				}
				return !this.IsFCE || this._fce.IsInitialized;
			}
		}

		// Token: 0x17001839 RID: 6201
		// (get) Token: 0x06006497 RID: 25751 RVA: 0x001C3908 File Offset: 0x001C1B08
		internal bool ThisHasLoadedChangeEventHandler
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.ThisHasLoadedChangeEventHandler;
				}
				return this.IsFCE && this._fce.ThisHasLoadedChangeEventHandler;
			}
		}

		// Token: 0x1700183A RID: 6202
		// (get) Token: 0x06006498 RID: 25752 RVA: 0x001C3933 File Offset: 0x001C1B33
		// (set) Token: 0x06006499 RID: 25753 RVA: 0x001C395E File Offset: 0x001C1B5E
		internal bool SubtreeHasLoadedChangeHandler
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.SubtreeHasLoadedChangeHandler;
				}
				return this.IsFCE && this._fce.SubtreeHasLoadedChangeHandler;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.SubtreeHasLoadedChangeHandler = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.SubtreeHasLoadedChangeHandler = value;
				}
			}
		}

		// Token: 0x1700183B RID: 6203
		// (get) Token: 0x0600649A RID: 25754 RVA: 0x001C3989 File Offset: 0x001C1B89
		internal InheritanceBehavior InheritanceBehavior
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.InheritanceBehavior;
				}
				if (this.IsFCE)
				{
					return this._fce.InheritanceBehavior;
				}
				return InheritanceBehavior.Default;
			}
		}

		// Token: 0x1700183C RID: 6204
		// (get) Token: 0x0600649B RID: 25755 RVA: 0x001C39B4 File Offset: 0x001C1BB4
		// (set) Token: 0x0600649C RID: 25756 RVA: 0x001C39DF File Offset: 0x001C1BDF
		internal bool StoresParentTemplateValues
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.StoresParentTemplateValues;
				}
				return this.IsFCE && this._fce.StoresParentTemplateValues;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.StoresParentTemplateValues = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.StoresParentTemplateValues = value;
				}
			}
		}

		// Token: 0x1700183D RID: 6205
		// (set) Token: 0x0600649D RID: 25757 RVA: 0x001C3A0A File Offset: 0x001C1C0A
		internal bool HasResourceReference
		{
			set
			{
				if (this.IsFE)
				{
					this._fe.HasResourceReference = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.HasResourceReference = value;
				}
			}
		}

		// Token: 0x1700183E RID: 6206
		// (set) Token: 0x0600649E RID: 25758 RVA: 0x001C3A35 File Offset: 0x001C1C35
		internal bool HasTemplateChanged
		{
			set
			{
				if (this.IsFE)
				{
					this._fe.HasTemplateChanged = value;
				}
			}
		}

		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x0600649F RID: 25759 RVA: 0x001C3A4B File Offset: 0x001C1C4B
		// (set) Token: 0x060064A0 RID: 25760 RVA: 0x001C3A76 File Offset: 0x001C1C76
		internal bool ShouldLookupImplicitStyles
		{
			get
			{
				if (this.IsFE)
				{
					return this._fe.ShouldLookupImplicitStyles;
				}
				return this.IsFCE && this._fce.ShouldLookupImplicitStyles;
			}
			set
			{
				if (this.IsFE)
				{
					this._fe.ShouldLookupImplicitStyles = value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.ShouldLookupImplicitStyles = value;
				}
			}
		}

		// Token: 0x060064A1 RID: 25761 RVA: 0x001C3AA4 File Offset: 0x001C1CA4
		internal static bool IsEffectiveAncestor(DependencyObject d1, DependencyObject d2)
		{
			FrameworkObject frameworkObject = new FrameworkObject(d2);
			while (frameworkObject.DO != null)
			{
				if (frameworkObject.DO == d1)
				{
					return true;
				}
				frameworkObject.Reset(frameworkObject.EffectiveParent);
			}
			return false;
		}

		// Token: 0x060064A2 RID: 25762 RVA: 0x001C3ADF File Offset: 0x001C1CDF
		internal void ChangeLogicalParent(DependencyObject newParent)
		{
			if (this.IsFE)
			{
				this._fe.ChangeLogicalParent(newParent);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.ChangeLogicalParent(newParent);
			}
		}

		// Token: 0x060064A3 RID: 25763 RVA: 0x001C3B0A File Offset: 0x001C1D0A
		internal void BeginInit()
		{
			if (this.IsFE)
			{
				this._fe.BeginInit();
				return;
			}
			if (this.IsFCE)
			{
				this._fce.BeginInit();
				return;
			}
			this.UnexpectedCall();
		}

		// Token: 0x060064A4 RID: 25764 RVA: 0x001C3B3A File Offset: 0x001C1D3A
		internal void EndInit()
		{
			if (this.IsFE)
			{
				this._fe.EndInit();
				return;
			}
			if (this.IsFCE)
			{
				this._fce.EndInit();
				return;
			}
			this.UnexpectedCall();
		}

		// Token: 0x060064A5 RID: 25765 RVA: 0x001C3B6A File Offset: 0x001C1D6A
		internal object FindName(string name, out DependencyObject scopeOwner)
		{
			if (this.IsFE)
			{
				return this._fe.FindName(name, out scopeOwner);
			}
			if (this.IsFCE)
			{
				return this._fce.FindName(name, out scopeOwner);
			}
			scopeOwner = null;
			return null;
		}

		// Token: 0x060064A6 RID: 25766 RVA: 0x001C3B9C File Offset: 0x001C1D9C
		internal FrameworkObject GetPreferVisualParent(bool force)
		{
			InheritanceBehavior inheritanceBehavior = force ? InheritanceBehavior.Default : this.InheritanceBehavior;
			if (inheritanceBehavior != InheritanceBehavior.Default)
			{
				return new FrameworkObject(null);
			}
			FrameworkObject rawPreferVisualParent = this.GetRawPreferVisualParent();
			switch (rawPreferVisualParent.InheritanceBehavior)
			{
			case InheritanceBehavior.SkipToAppNow:
			case InheritanceBehavior.SkipToThemeNow:
			case InheritanceBehavior.SkipAllNow:
				rawPreferVisualParent.Reset(null);
				break;
			}
			return rawPreferVisualParent;
		}

		// Token: 0x060064A7 RID: 25767 RVA: 0x001C3BF8 File Offset: 0x001C1DF8
		private FrameworkObject GetRawPreferVisualParent()
		{
			if (this._do == null)
			{
				return new FrameworkObject(null);
			}
			DependencyObject dependencyObject;
			if (this.IsFE)
			{
				dependencyObject = VisualTreeHelper.GetParent(this._fe);
			}
			else if (this.IsFCE)
			{
				dependencyObject = null;
			}
			else if (this._do != null)
			{
				Visual visual = this._do as Visual;
				dependencyObject = ((visual != null) ? VisualTreeHelper.GetParent(visual) : null);
			}
			else
			{
				dependencyObject = null;
			}
			if (dependencyObject != null)
			{
				return new FrameworkObject(dependencyObject);
			}
			DependencyObject dependencyObject2;
			if (this.IsFE)
			{
				dependencyObject2 = this._fe.Parent;
			}
			else if (this.IsFCE)
			{
				dependencyObject2 = this._fce.Parent;
			}
			else if (this._do != null)
			{
				ContentElement contentElement = this._do as ContentElement;
				dependencyObject2 = ((contentElement != null) ? ContentOperations.GetParent(contentElement) : null);
			}
			else
			{
				dependencyObject2 = null;
			}
			if (dependencyObject2 != null)
			{
				return new FrameworkObject(dependencyObject2);
			}
			UIElement uielement;
			DependencyObject dependencyObject3;
			ContentElement contentElement2;
			if ((uielement = (this._do as UIElement)) != null)
			{
				dependencyObject3 = uielement.GetUIParentCore();
			}
			else if ((contentElement2 = (this._do as ContentElement)) != null)
			{
				dependencyObject3 = contentElement2.GetUIParentCore();
			}
			else
			{
				dependencyObject3 = null;
			}
			if (dependencyObject3 != null)
			{
				return new FrameworkObject(dependencyObject3);
			}
			return new FrameworkObject(this._do.InheritanceContext);
		}

		// Token: 0x060064A8 RID: 25768 RVA: 0x001C3D17 File Offset: 0x001C1F17
		internal void RaiseEvent(RoutedEventArgs args)
		{
			if (this.IsFE)
			{
				this._fe.RaiseEvent(args);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.RaiseEvent(args);
			}
		}

		// Token: 0x060064A9 RID: 25769 RVA: 0x001C3D42 File Offset: 0x001C1F42
		internal void OnLoaded(RoutedEventArgs args)
		{
			if (this.IsFE)
			{
				this._fe.OnLoaded(args);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.OnLoaded(args);
			}
		}

		// Token: 0x060064AA RID: 25770 RVA: 0x001C3D6D File Offset: 0x001C1F6D
		internal void OnUnloaded(RoutedEventArgs args)
		{
			if (this.IsFE)
			{
				this._fe.OnUnloaded(args);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.OnUnloaded(args);
			}
		}

		// Token: 0x060064AB RID: 25771 RVA: 0x001C3D98 File Offset: 0x001C1F98
		internal void ChangeSubtreeHasLoadedChangedHandler(DependencyObject mentor)
		{
			if (this.IsFE)
			{
				this._fe.ChangeSubtreeHasLoadedChangedHandler(mentor);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.ChangeSubtreeHasLoadedChangedHandler(mentor);
			}
		}

		// Token: 0x060064AC RID: 25772 RVA: 0x001C3DC3 File Offset: 0x001C1FC3
		internal void OnInheritedPropertyChanged(ref InheritablePropertyChangeInfo info)
		{
			if (this.IsFE)
			{
				this._fe.RaiseInheritedPropertyChangedEvent(ref info);
				return;
			}
			if (this.IsFCE)
			{
				this._fce.RaiseInheritedPropertyChangedEvent(ref info);
			}
		}

		// Token: 0x060064AD RID: 25773 RVA: 0x001C3DF0 File Offset: 0x001C1FF0
		internal void SetShouldLookupImplicitStyles()
		{
			if (!this.ShouldLookupImplicitStyles)
			{
				FrameworkObject frameworkParent = this.FrameworkParent;
				if (frameworkParent.IsValid && frameworkParent.ShouldLookupImplicitStyles)
				{
					this.ShouldLookupImplicitStyles = true;
				}
			}
		}

		// Token: 0x14000134 RID: 308
		// (add) Token: 0x060064AE RID: 25774 RVA: 0x001C3E25 File Offset: 0x001C2025
		// (remove) Token: 0x060064AF RID: 25775 RVA: 0x001C3E57 File Offset: 0x001C2057
		internal event RoutedEventHandler Loaded
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.Loaded += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Loaded += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.Loaded -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Loaded -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x14000135 RID: 309
		// (add) Token: 0x060064B0 RID: 25776 RVA: 0x001C3E89 File Offset: 0x001C2089
		// (remove) Token: 0x060064B1 RID: 25777 RVA: 0x001C3EBB File Offset: 0x001C20BB
		internal event RoutedEventHandler Unloaded
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.Unloaded += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Unloaded += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.Unloaded -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.Unloaded -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x14000136 RID: 310
		// (add) Token: 0x060064B2 RID: 25778 RVA: 0x001C3EED File Offset: 0x001C20ED
		// (remove) Token: 0x060064B3 RID: 25779 RVA: 0x001C3F1F File Offset: 0x001C211F
		internal event InheritedPropertyChangedEventHandler InheritedPropertyChanged
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.InheritedPropertyChanged += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.InheritedPropertyChanged += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.InheritedPropertyChanged -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.InheritedPropertyChanged -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x14000137 RID: 311
		// (add) Token: 0x060064B4 RID: 25780 RVA: 0x001C3F51 File Offset: 0x001C2151
		// (remove) Token: 0x060064B5 RID: 25781 RVA: 0x001C3F83 File Offset: 0x001C2183
		internal event EventHandler ResourcesChanged
		{
			add
			{
				if (this.IsFE)
				{
					this._fe.ResourcesChanged += value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.ResourcesChanged += value;
					return;
				}
				this.UnexpectedCall();
			}
			remove
			{
				if (this.IsFE)
				{
					this._fe.ResourcesChanged -= value;
					return;
				}
				if (this.IsFCE)
				{
					this._fce.ResourcesChanged -= value;
					return;
				}
				this.UnexpectedCall();
			}
		}

		// Token: 0x060064B6 RID: 25782 RVA: 0x001C3FB5 File Offset: 0x001C21B5
		private void UnexpectedCall()
		{
			Invariant.Assert(false, "Call to FrameworkObject expects either FE or FCE");
		}

		// Token: 0x060064B7 RID: 25783 RVA: 0x001C3FC2 File Offset: 0x001C21C2
		public override string ToString()
		{
			if (this.IsFE)
			{
				return this._fe.ToString();
			}
			if (this.IsFCE)
			{
				return this._fce.ToString();
			}
			return "Null";
		}

		// Token: 0x040032B1 RID: 12977
		private FrameworkElement _fe;

		// Token: 0x040032B2 RID: 12978
		private FrameworkContentElement _fce;

		// Token: 0x040032B3 RID: 12979
		private DependencyObject _do;
	}
}
