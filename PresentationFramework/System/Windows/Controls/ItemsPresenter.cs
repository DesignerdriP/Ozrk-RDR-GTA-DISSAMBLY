using System;
using System.Windows.Media;
using System.Xaml;
using MS.Internal;

namespace System.Windows.Controls
{
	/// <summary>Used within the template of an item control to specify the place in the control’s visual tree where the <see cref="P:System.Windows.Controls.ItemsControl.ItemsPanel" /> defined by the <see cref="T:System.Windows.Controls.ItemsControl" /> is to be added.</summary>
	// Token: 0x020004F8 RID: 1272
	[Localizability(LocalizationCategory.NeverLocalize)]
	public class ItemsPresenter : FrameworkElement
	{
		// Token: 0x060050FE RID: 20734 RVA: 0x0016B9A3 File Offset: 0x00169BA3
		internal override void OnPreApplyTemplate()
		{
			base.OnPreApplyTemplate();
			this.AttachToOwner();
		}

		/// <summary>Called when an internal process or application calls <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />, which is used to build the current template's visual tree.</summary>
		// Token: 0x060050FF RID: 20735 RVA: 0x0016B9B4 File Offset: 0x00169BB4
		public override void OnApplyTemplate()
		{
			Panel panel = this.GetVisualChild(0) as Panel;
			if (panel == null || VisualTreeHelper.GetChildrenCount(panel) > 0)
			{
				throw new InvalidOperationException(SR.Get("ItemsPanelNotSingleNode"));
			}
			this.OnPanelChanged(this, EventArgs.Empty);
			base.OnApplyTemplate();
		}

		/// <summary>Overrides the base class implementation of <see cref="M:System.Windows.FrameworkElement.MeasureOverride(System.Windows.Size)" /> to measure the size of the <see cref="T:System.Windows.Controls.ItemsPresenter" /> object and return proper sizes to the layout engine.</summary>
		/// <param name="constraint">Constraint size is an "upper limit." The return value should not exceed this size.</param>
		/// <returns>The desired size.</returns>
		// Token: 0x06005100 RID: 20736 RVA: 0x00136504 File Offset: 0x00134704
		protected override Size MeasureOverride(Size constraint)
		{
			return Helper.MeasureElementWithSingleChild(this, constraint);
		}

		/// <summary> Called to arrange and size the content of an <see cref="T:System.Windows.Controls.ItemsPresenter" /> object. </summary>
		/// <param name="arrangeSize">Computed size used to arrange the content.</param>
		/// <returns>Size of content.</returns>
		// Token: 0x06005101 RID: 20737 RVA: 0x0013650D File Offset: 0x0013470D
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			return Helper.ArrangeElementWithSingleChild(this, arrangeSize);
		}

		// Token: 0x170013AB RID: 5035
		// (get) Token: 0x06005102 RID: 20738 RVA: 0x0016B9FC File Offset: 0x00169BFC
		internal ItemsControl Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170013AC RID: 5036
		// (get) Token: 0x06005103 RID: 20739 RVA: 0x0016BA04 File Offset: 0x00169C04
		internal ItemContainerGenerator Generator
		{
			get
			{
				return this._generator;
			}
		}

		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06005104 RID: 20740 RVA: 0x0016BA0C File Offset: 0x00169C0C
		internal override FrameworkTemplate TemplateInternal
		{
			get
			{
				return this.Template;
			}
		}

		// Token: 0x170013AE RID: 5038
		// (get) Token: 0x06005105 RID: 20741 RVA: 0x0016BA14 File Offset: 0x00169C14
		// (set) Token: 0x06005106 RID: 20742 RVA: 0x0016BA1C File Offset: 0x00169C1C
		internal override FrameworkTemplate TemplateCache
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				this._templateCache = (ItemsPanelTemplate)value;
			}
		}

		// Token: 0x170013AF RID: 5039
		// (get) Token: 0x06005107 RID: 20743 RVA: 0x0016BA14 File Offset: 0x00169C14
		// (set) Token: 0x06005108 RID: 20744 RVA: 0x0016BA2A File Offset: 0x00169C2A
		private ItemsPanelTemplate Template
		{
			get
			{
				return this._templateCache;
			}
			set
			{
				base.SetValue(ItemsPresenter.TemplateProperty, value);
			}
		}

		// Token: 0x06005109 RID: 20745 RVA: 0x0016BA38 File Offset: 0x00169C38
		internal override void OnTemplateChangedInternal(FrameworkTemplate oldTemplate, FrameworkTemplate newTemplate)
		{
			this.OnTemplateChanged((ItemsPanelTemplate)oldTemplate, (ItemsPanelTemplate)newTemplate);
		}

		// Token: 0x0600510A RID: 20746 RVA: 0x0016BA4C File Offset: 0x00169C4C
		private static void OnTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ItemsPresenter fe = (ItemsPresenter)d;
			StyleHelper.UpdateTemplateCache(fe, (FrameworkTemplate)e.OldValue, (FrameworkTemplate)e.NewValue, ItemsPresenter.TemplateProperty);
		}

		/// <summary>Called when the control template changes.</summary>
		/// <param name="oldTemplate">Value of the old template.</param>
		/// <param name="newTemplate">Value of the new template.</param>
		// Token: 0x0600510B RID: 20747 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnTemplateChanged(ItemsPanelTemplate oldTemplate, ItemsPanelTemplate newTemplate)
		{
		}

		// Token: 0x0600510C RID: 20748 RVA: 0x0016BA83 File Offset: 0x00169C83
		internal static ItemsPresenter FromPanel(Panel panel)
		{
			if (panel == null)
			{
				return null;
			}
			return panel.TemplatedParent as ItemsPresenter;
		}

		// Token: 0x0600510D RID: 20749 RVA: 0x0016BA98 File Offset: 0x00169C98
		internal static ItemsPresenter FromGroupItem(GroupItem groupItem)
		{
			if (groupItem == null)
			{
				return null;
			}
			Visual visual = VisualTreeHelper.GetParent(groupItem) as Visual;
			if (visual == null)
			{
				return null;
			}
			return VisualTreeHelper.GetParent(visual) as ItemsPresenter;
		}

		// Token: 0x0600510E RID: 20750 RVA: 0x0016BAC6 File Offset: 0x00169CC6
		internal override void OnAncestorChanged()
		{
			if (base.TemplatedParent == null)
			{
				this.UseGenerator(null);
				this.ClearPanel();
			}
			base.OnAncestorChanged();
		}

		// Token: 0x0600510F RID: 20751 RVA: 0x0016BAE4 File Offset: 0x00169CE4
		private void AttachToOwner()
		{
			DependencyObject templatedParent = base.TemplatedParent;
			ItemsControl itemsControl = templatedParent as ItemsControl;
			ItemContainerGenerator generator;
			if (itemsControl != null)
			{
				generator = itemsControl.ItemContainerGenerator;
			}
			else
			{
				GroupItem groupItem = templatedParent as GroupItem;
				ItemsPresenter itemsPresenter = ItemsPresenter.FromGroupItem(groupItem);
				if (itemsPresenter != null)
				{
					itemsControl = itemsPresenter.Owner;
				}
				generator = ((groupItem != null) ? groupItem.Generator : null);
			}
			this._owner = itemsControl;
			this.UseGenerator(generator);
			GroupStyle groupStyle = (this._generator != null) ? this._generator.GroupStyle : null;
			ItemsPanelTemplate itemsPanelTemplate;
			if (groupStyle != null)
			{
				itemsPanelTemplate = groupStyle.Panel;
				if (itemsPanelTemplate == null)
				{
					if (VirtualizingPanel.GetIsVirtualizingWhenGrouping(itemsControl))
					{
						itemsPanelTemplate = GroupStyle.DefaultVirtualizingStackPanel;
					}
					else
					{
						itemsPanelTemplate = GroupStyle.DefaultStackPanel;
					}
				}
			}
			else
			{
				itemsPanelTemplate = ((this._owner != null) ? this._owner.ItemsPanel : null);
			}
			this.Template = itemsPanelTemplate;
		}

		// Token: 0x06005110 RID: 20752 RVA: 0x0016BBA4 File Offset: 0x00169DA4
		private void UseGenerator(ItemContainerGenerator generator)
		{
			if (generator == this._generator)
			{
				return;
			}
			if (this._generator != null)
			{
				this._generator.PanelChanged -= this.OnPanelChanged;
			}
			this._generator = generator;
			if (this._generator != null)
			{
				this._generator.PanelChanged += this.OnPanelChanged;
			}
		}

		// Token: 0x06005111 RID: 20753 RVA: 0x0016BC00 File Offset: 0x00169E00
		private void OnPanelChanged(object sender, EventArgs e)
		{
			base.InvalidateMeasure();
			ScrollViewer scrollViewer = base.Parent as ScrollViewer;
			if (scrollViewer != null)
			{
				ScrollContentPresenter scrollContentPresenter = VisualTreeHelper.GetParent(this) as ScrollContentPresenter;
				if (scrollContentPresenter != null)
				{
					scrollContentPresenter.HookupScrollingComponents();
				}
			}
		}

		// Token: 0x06005112 RID: 20754 RVA: 0x0016BC38 File Offset: 0x00169E38
		private void ClearPanel()
		{
			Panel panel = (this.VisualChildrenCount > 0) ? (this.GetVisualChild(0) as Panel) : null;
			Type right = null;
			if (this.Template != null)
			{
				if (this.Template.VisualTree != null)
				{
					right = this.Template.VisualTree.Type;
				}
				else if (this.Template.HasXamlNodeContent)
				{
					XamlType rootType = this.Template.Template.RootType;
					right = rootType.UnderlyingType;
				}
			}
			if (panel != null && panel.GetType() == right)
			{
				panel.IsItemsHost = false;
			}
		}

		// Token: 0x04002C4E RID: 11342
		internal static readonly DependencyProperty TemplateProperty = DependencyProperty.Register("Template", typeof(ItemsPanelTemplate), typeof(ItemsPresenter), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(ItemsPresenter.OnTemplateChanged)));

		// Token: 0x04002C4F RID: 11343
		private ItemsControl _owner;

		// Token: 0x04002C50 RID: 11344
		private ItemContainerGenerator _generator;

		// Token: 0x04002C51 RID: 11345
		private ItemsPanelTemplate _templateCache;
	}
}
