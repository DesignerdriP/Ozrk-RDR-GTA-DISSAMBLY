using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;
using MS.Internal;
using MS.Internal.Controls;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that contains multiple items and has a header.</summary>
	// Token: 0x020004E6 RID: 1254
	[DefaultProperty("Header")]
	[Localizability(LocalizationCategory.Menu)]
	public class HeaderedItemsControl : ItemsControl
	{
		/// <summary>Gets or sets the item that labels the control.  </summary>
		/// <returns>An object that labels the <see cref="T:System.Windows.Controls.HeaderedItemsControl" />. The default is <see langword="null" />. A header can be a string or a <see cref="T:System.Windows.UIElement" />.</returns>
		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x06004E47 RID: 20039 RVA: 0x001605FF File Offset: 0x0015E7FF
		// (set) Token: 0x06004E48 RID: 20040 RVA: 0x0016060C File Offset: 0x0015E80C
		[Bindable(true)]
		[CustomCategory("Content")]
		public object Header
		{
			get
			{
				return base.GetValue(HeaderedItemsControl.HeaderProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderProperty, value);
			}
		}

		// Token: 0x06004E49 RID: 20041 RVA: 0x0016061C File Offset: 0x0015E81C
		private static void OnHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = (HeaderedItemsControl)d;
			headeredItemsControl.SetValue(HeaderedItemsControl.HasHeaderPropertyKey, (e.NewValue != null) ? BooleanBoxes.TrueBox : BooleanBoxes.FalseBox);
			headeredItemsControl.OnHeaderChanged(e.OldValue, e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" /> property of a <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> changes. </summary>
		/// <param name="oldHeader">The old value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" /> property.</param>
		/// <param name="newHeader">The new value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" /> property.</param>
		// Token: 0x06004E4A RID: 20042 RVA: 0x00160664 File Offset: 0x0015E864
		protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
		{
			if (!this.IsHeaderLogical())
			{
				return;
			}
			base.RemoveLogicalChild(oldHeader);
			base.AddLogicalChild(newHeader);
		}

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> has a header.  </summary>
		/// <returns>
		///     <see langword="true" /> if the control has a header; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x06004E4B RID: 20043 RVA: 0x0016067D File Offset: 0x0015E87D
		[Bindable(false)]
		[Browsable(false)]
		public bool HasHeader
		{
			get
			{
				return (bool)base.GetValue(HeaderedItemsControl.HasHeaderProperty);
			}
		}

		/// <summary>Gets or sets the template used to display the contents of the control's header.  </summary>
		/// <returns>A data template used to display a control's header. The default is <see langword="null" />.</returns>
		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x06004E4C RID: 20044 RVA: 0x0016068F File Offset: 0x0015E88F
		// (set) Token: 0x06004E4D RID: 20045 RVA: 0x001606A1 File Offset: 0x0015E8A1
		[Bindable(true)]
		[CustomCategory("Content")]
		public DataTemplate HeaderTemplate
		{
			get
			{
				return (DataTemplate)base.GetValue(HeaderedItemsControl.HeaderTemplateProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderTemplateProperty, value);
			}
		}

		// Token: 0x06004E4E RID: 20046 RVA: 0x001606B0 File Offset: 0x0015E8B0
		private static void OnHeaderTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = (HeaderedItemsControl)d;
			headeredItemsControl.OnHeaderTemplateChanged((DataTemplate)e.OldValue, (DataTemplate)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" /> property changes. </summary>
		/// <param name="oldHeaderTemplate">The old value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" /> property.</param>
		/// <param name="newHeaderTemplate">The new value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" /> property.</param>
		// Token: 0x06004E4F RID: 20047 RVA: 0x001606E2 File Offset: 0x0015E8E2
		protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedItemsControl.HeaderTemplateProperty, HeaderedItemsControl.HeaderTemplateSelectorProperty, this);
		}

		/// <summary>Gets or sets the object that provides custom selection logic for a template used to display the header of each item.  </summary>
		/// <returns>A data template selector. The default is <see langword="null" />.</returns>
		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x06004E50 RID: 20048 RVA: 0x001606F9 File Offset: 0x0015E8F9
		// (set) Token: 0x06004E51 RID: 20049 RVA: 0x0016070B File Offset: 0x0015E90B
		[Bindable(true)]
		[CustomCategory("Content")]
		public DataTemplateSelector HeaderTemplateSelector
		{
			get
			{
				return (DataTemplateSelector)base.GetValue(HeaderedItemsControl.HeaderTemplateSelectorProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderTemplateSelectorProperty, value);
			}
		}

		// Token: 0x06004E52 RID: 20050 RVA: 0x0016071C File Offset: 0x0015E91C
		private static void OnHeaderTemplateSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = (HeaderedItemsControl)d;
			headeredItemsControl.OnHeaderTemplateSelectorChanged((DataTemplateSelector)e.OldValue, (DataTemplateSelector)e.NewValue);
		}

		/// <summary>Called when the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplateSelector" /> property changes. </summary>
		/// <param name="oldHeaderTemplateSelector">The old value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplateSelector" /> property.</param>
		/// <param name="newHeaderTemplateSelector">The new value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplateSelector" /> property.</param>
		// Token: 0x06004E53 RID: 20051 RVA: 0x001606E2 File Offset: 0x0015E8E2
		protected virtual void OnHeaderTemplateSelectorChanged(DataTemplateSelector oldHeaderTemplateSelector, DataTemplateSelector newHeaderTemplateSelector)
		{
			Helper.CheckTemplateAndTemplateSelector("Header", HeaderedItemsControl.HeaderTemplateProperty, HeaderedItemsControl.HeaderTemplateSelectorProperty, this);
		}

		/// <summary>Gets or sets a composite string that specifies how to format the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" /> property if it is displayed as a string.</summary>
		/// <returns>A composite string that specifies how to format the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" /> property if it is displayed as a string.</returns>
		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x06004E54 RID: 20052 RVA: 0x0016074E File Offset: 0x0015E94E
		// (set) Token: 0x06004E55 RID: 20053 RVA: 0x00160760 File Offset: 0x0015E960
		[Bindable(true)]
		[CustomCategory("Content")]
		public string HeaderStringFormat
		{
			get
			{
				return (string)base.GetValue(HeaderedItemsControl.HeaderStringFormatProperty);
			}
			set
			{
				base.SetValue(HeaderedItemsControl.HeaderStringFormatProperty, value);
			}
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x00160770 File Offset: 0x0015E970
		private static void OnHeaderStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HeaderedItemsControl headeredItemsControl = (HeaderedItemsControl)d;
			headeredItemsControl.OnHeaderStringFormatChanged((string)e.OldValue, (string)e.NewValue);
		}

		/// <summary>Occurs when the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderStringFormat" /> property changes.</summary>
		/// <param name="oldHeaderStringFormat">The old value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderStringFormat" /> property.</param>
		/// <param name="newHeaderStringFormat">The new value of the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderStringFormat" /> property.</param>
		// Token: 0x06004E57 RID: 20055 RVA: 0x00002137 File Offset: 0x00000337
		protected virtual void OnHeaderStringFormatChanged(string oldHeaderStringFormat, string newHeaderStringFormat)
		{
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x001607A4 File Offset: 0x0015E9A4
		internal void PrepareHeaderedItemsControl(object item, ItemsControl parentItemsControl)
		{
			bool flag = item != this;
			base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical, flag);
			base.PrepareItemsControl(item, parentItemsControl);
			if (flag)
			{
				if (this.HeaderIsItem || !base.HasNonDefaultValue(HeaderedItemsControl.HeaderProperty))
				{
					this.Header = item;
					this.HeaderIsItem = true;
				}
				DataTemplate itemTemplate = parentItemsControl.ItemTemplate;
				DataTemplateSelector itemTemplateSelector = parentItemsControl.ItemTemplateSelector;
				string itemStringFormat = parentItemsControl.ItemStringFormat;
				if (itemTemplate != null)
				{
					base.SetValue(HeaderedItemsControl.HeaderTemplateProperty, itemTemplate);
				}
				if (itemTemplateSelector != null)
				{
					base.SetValue(HeaderedItemsControl.HeaderTemplateSelectorProperty, itemTemplateSelector);
				}
				if (itemStringFormat != null && Helper.HasDefaultValue(this, HeaderedItemsControl.HeaderStringFormatProperty))
				{
					base.SetValue(HeaderedItemsControl.HeaderStringFormatProperty, itemStringFormat);
				}
				this.PrepareHierarchy(item, parentItemsControl);
			}
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x00160846 File Offset: 0x0015EA46
		internal void ClearHeaderedItemsControl(object item)
		{
			base.ClearItemsControl(item);
			if (item != this && this.HeaderIsItem)
			{
				this.Header = BindingExpressionBase.DisconnectedItem;
			}
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x00160866 File Offset: 0x0015EA66
		internal override string GetPlainText()
		{
			return ContentControl.ContentObjectToString(this.Header);
		}

		/// <summary>Returns the string representation of a <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> object. </summary>
		/// <returns>A string that represents this object.</returns>
		// Token: 0x06004E5B RID: 20059 RVA: 0x00160874 File Offset: 0x0015EA74
		public override string ToString()
		{
			string text = base.GetType().ToString();
			string headerText = string.Empty;
			int itemCount = 0;
			bool valuesDefined = false;
			if (base.CheckAccess())
			{
				headerText = ContentControl.ContentObjectToString(this.Header);
				itemCount = (base.HasItems ? base.Items.Count : 0);
				valuesDefined = true;
			}
			else
			{
				base.Dispatcher.Invoke(DispatcherPriority.Send, new TimeSpan(0, 0, 0, 0, 20), new DispatcherOperationCallback(delegate(object o)
				{
					headerText = ContentControl.ContentObjectToString(this.Header);
					itemCount = (this.HasItems ? this.Items.Count : 0);
					valuesDefined = true;
					return null;
				}), null);
			}
			if (valuesDefined)
			{
				return SR.Get("ToStringFormatString_HeaderedItemsControl", new object[]
				{
					text,
					headerText,
					itemCount
				});
			}
			return text;
		}

		/// <summary>Gets an enumerator to the logical child elements of the <see cref="T:System.Windows.Controls.HeaderedItemsControl" />. </summary>
		/// <returns>An enumerator. The default is <see langword="null" />.</returns>
		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06004E5C RID: 20060 RVA: 0x0016094C File Offset: 0x0015EB4C
		protected internal override IEnumerator LogicalChildren
		{
			get
			{
				object header = this.Header;
				if (base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical) || header == null)
				{
					return base.LogicalChildren;
				}
				return new HeaderedItemsModelTreeEnumerator(this, base.LogicalChildren, header);
			}
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x00160980 File Offset: 0x0015EB80
		private void PrepareHierarchy(object item, ItemsControl parentItemsControl)
		{
			DataTemplate dataTemplate = this.HeaderTemplate;
			if (dataTemplate == null)
			{
				DataTemplateSelector headerTemplateSelector = this.HeaderTemplateSelector;
				if (headerTemplateSelector != null)
				{
					dataTemplate = headerTemplateSelector.SelectTemplate(item, this);
				}
				if (dataTemplate == null)
				{
					dataTemplate = (DataTemplate)FrameworkElement.FindTemplateResourceInternal(this, item, typeof(DataTemplate));
				}
			}
			HierarchicalDataTemplate hierarchicalDataTemplate = dataTemplate as HierarchicalDataTemplate;
			if (hierarchicalDataTemplate != null)
			{
				bool flag = base.ItemTemplate == parentItemsControl.ItemTemplate;
				bool flag2 = base.ItemContainerStyle == parentItemsControl.ItemContainerStyle;
				if (hierarchicalDataTemplate.ItemsSource != null && !base.HasNonDefaultValue(ItemsControl.ItemsSourceProperty))
				{
					base.SetBinding(ItemsControl.ItemsSourceProperty, hierarchicalDataTemplate.ItemsSource);
				}
				if (hierarchicalDataTemplate.IsItemStringFormatSet && base.ItemStringFormat == parentItemsControl.ItemStringFormat)
				{
					base.ClearValue(ItemsControl.ItemTemplateProperty);
					base.ClearValue(ItemsControl.ItemTemplateSelectorProperty);
					base.ClearValue(ItemsControl.ItemStringFormatProperty);
					bool flag3 = hierarchicalDataTemplate.ItemStringFormat != null;
					if (flag3)
					{
						base.ItemStringFormat = hierarchicalDataTemplate.ItemStringFormat;
					}
				}
				if (hierarchicalDataTemplate.IsItemTemplateSelectorSet && base.ItemTemplateSelector == parentItemsControl.ItemTemplateSelector)
				{
					base.ClearValue(ItemsControl.ItemTemplateProperty);
					base.ClearValue(ItemsControl.ItemTemplateSelectorProperty);
					bool flag4 = hierarchicalDataTemplate.ItemTemplateSelector != null;
					if (flag4)
					{
						base.ItemTemplateSelector = hierarchicalDataTemplate.ItemTemplateSelector;
					}
				}
				if (hierarchicalDataTemplate.IsItemTemplateSet && flag)
				{
					base.ClearValue(ItemsControl.ItemTemplateProperty);
					bool flag5 = hierarchicalDataTemplate.ItemTemplate != null;
					if (flag5)
					{
						base.ItemTemplate = hierarchicalDataTemplate.ItemTemplate;
					}
				}
				if (hierarchicalDataTemplate.IsItemContainerStyleSelectorSet && base.ItemContainerStyleSelector == parentItemsControl.ItemContainerStyleSelector)
				{
					base.ClearValue(ItemsControl.ItemContainerStyleProperty);
					base.ClearValue(ItemsControl.ItemContainerStyleSelectorProperty);
					bool flag6 = hierarchicalDataTemplate.ItemContainerStyleSelector != null;
					if (flag6)
					{
						base.ItemContainerStyleSelector = hierarchicalDataTemplate.ItemContainerStyleSelector;
					}
				}
				if (hierarchicalDataTemplate.IsItemContainerStyleSet && flag2)
				{
					base.ClearValue(ItemsControl.ItemContainerStyleProperty);
					bool flag7 = hierarchicalDataTemplate.ItemContainerStyle != null;
					if (flag7)
					{
						base.ItemContainerStyle = hierarchicalDataTemplate.ItemContainerStyle;
					}
				}
				if (hierarchicalDataTemplate.IsAlternationCountSet && base.AlternationCount == parentItemsControl.AlternationCount)
				{
					base.ClearValue(ItemsControl.AlternationCountProperty);
					bool flag8 = true;
					if (flag8)
					{
						base.AlternationCount = hierarchicalDataTemplate.AlternationCount;
					}
				}
				if (hierarchicalDataTemplate.IsItemBindingGroupSet && base.ItemBindingGroup == parentItemsControl.ItemBindingGroup)
				{
					base.ClearValue(ItemsControl.ItemBindingGroupProperty);
					bool flag9 = hierarchicalDataTemplate.ItemBindingGroup != null;
					if (flag9)
					{
						base.ItemBindingGroup = hierarchicalDataTemplate.ItemBindingGroup;
					}
				}
			}
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x00160BD0 File Offset: 0x0015EDD0
		private bool IsBound(DependencyProperty dp, Binding binding)
		{
			BindingExpressionBase bindingExpression = BindingOperations.GetBindingExpression(this, dp);
			return bindingExpression != null && bindingExpression.ParentBindingBase == binding;
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x00160BF3 File Offset: 0x0015EDF3
		private bool IsHeaderLogical()
		{
			if (base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical))
			{
				return false;
			}
			if (BindingOperations.IsDataBound(this, HeaderedItemsControl.HeaderProperty))
			{
				base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsNotLogical, true);
				return false;
			}
			return true;
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x06004E60 RID: 20064 RVA: 0x0016043A File Offset: 0x0015E63A
		// (set) Token: 0x06004E61 RID: 20065 RVA: 0x00160444 File Offset: 0x0015E644
		private bool HeaderIsItem
		{
			get
			{
				return base.ReadControlFlag(Control.ControlBoolFlags.HeaderIsItem);
			}
			set
			{
				base.WriteControlFlag(Control.ControlBoolFlags.HeaderIsItem, value);
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" /> dependency property.</returns>
		// Token: 0x04002BBD RID: 11197
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderProperty = HeaderedContentControl.HeaderProperty.AddOwner(typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderChanged)));

		// Token: 0x04002BBE RID: 11198
		private static readonly DependencyPropertyKey HasHeaderPropertyKey = HeaderedContentControl.HasHeaderPropertyKey;

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HasHeader" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HasHeader" /> dependency property.</returns>
		// Token: 0x04002BBF RID: 11199
		[CommonDependencyProperty]
		public static readonly DependencyProperty HasHeaderProperty = HeaderedContentControl.HasHeaderProperty.AddOwner(typeof(HeaderedItemsControl));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" /> dependency property.</returns>
		// Token: 0x04002BC0 RID: 11200
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateProperty = HeaderedContentControl.HeaderTemplateProperty.AddOwner(typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderTemplateChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplateSelector" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplateSelector" /> dependency property.</returns>
		// Token: 0x04002BC1 RID: 11201
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderTemplateSelectorProperty = HeaderedContentControl.HeaderTemplateSelectorProperty.AddOwner(typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderTemplateSelectorChanged)));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderStringFormat" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderStringFormat" /> dependency property.</returns>
		// Token: 0x04002BC2 RID: 11202
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeaderStringFormatProperty = DependencyProperty.Register("HeaderStringFormat", typeof(string), typeof(HeaderedItemsControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(HeaderedItemsControl.OnHeaderStringFormatChanged)));
	}
}
