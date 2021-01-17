using System;
using System.Collections.Specialized;
using System.Windows.Automation.Peers;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Represents a control that displays a list of data items.</summary>
	// Token: 0x020004FD RID: 1277
	[StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ListViewItem))]
	public class ListView : ListBox
	{
		// Token: 0x06005165 RID: 20837 RVA: 0x0016D1C4 File Offset: 0x0016B3C4
		static ListView()
		{
			ListBox.SelectionModeProperty.OverrideMetadata(typeof(ListView), new FrameworkPropertyMetadata(SelectionMode.Extended));
			ControlsTraceLogger.AddControl(TelemetryControls.ListView);
		}

		/// <summary>Gets or sets an object that defines how the data is styled and organized in a <see cref="T:System.Windows.Controls.ListView" /> control.  </summary>
		/// <returns>A <see cref="T:System.Windows.Controls.ViewBase" /> object that specifies how to display information in the <see cref="T:System.Windows.Controls.ListView" />.</returns>
		// Token: 0x170013BD RID: 5053
		// (get) Token: 0x06005166 RID: 20838 RVA: 0x0016D22F File Offset: 0x0016B42F
		// (set) Token: 0x06005167 RID: 20839 RVA: 0x0016D241 File Offset: 0x0016B441
		public ViewBase View
		{
			get
			{
				return (ViewBase)base.GetValue(ListView.ViewProperty);
			}
			set
			{
				base.SetValue(ListView.ViewProperty, value);
			}
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x0016D250 File Offset: 0x0016B450
		private static void OnViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ListView listView = (ListView)d;
			ViewBase viewBase = (ViewBase)e.OldValue;
			ViewBase viewBase2 = (ViewBase)e.NewValue;
			if (viewBase2 != null)
			{
				if (viewBase2.IsUsed)
				{
					throw new InvalidOperationException(SR.Get("ListView_ViewCannotBeShared"));
				}
				viewBase2.IsUsed = true;
			}
			listView._previousView = viewBase;
			listView.ApplyNewView();
			listView._previousView = viewBase2;
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(listView) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null)
			{
				if (listViewAutomationPeer.ViewAutomationPeer != null)
				{
					listViewAutomationPeer.ViewAutomationPeer.ViewDetached();
				}
				if (viewBase2 != null)
				{
					listViewAutomationPeer.ViewAutomationPeer = viewBase2.GetAutomationPeer(listView);
				}
				else
				{
					listViewAutomationPeer.ViewAutomationPeer = null;
				}
				listViewAutomationPeer.InvalidatePeer();
			}
			if (viewBase != null)
			{
				viewBase.IsUsed = false;
			}
		}

		/// <summary>Sets the styles, templates, and bindings for a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="element">An object that is a <see cref="T:System.Windows.Controls.ListViewItem" /> or that can be converted into one.</param>
		/// <param name="item">The object to use to create the <see cref="T:System.Windows.Controls.ListViewItem" />.</param>
		// Token: 0x06005169 RID: 20841 RVA: 0x0016D300 File Offset: 0x0016B500
		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
			ListViewItem listViewItem = element as ListViewItem;
			if (listViewItem != null)
			{
				ViewBase view = this.View;
				if (view != null)
				{
					listViewItem.SetDefaultStyleKey(view.ItemContainerDefaultStyleKey);
					view.PrepareItem(listViewItem);
					return;
				}
				listViewItem.ClearDefaultStyleKey();
			}
		}

		/// <summary>Removes all templates, styles, and bindings for the object that is displayed as a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="element">The <see cref="T:System.Windows.Controls.ListViewItem" /> container to clear.</param>
		/// <param name="item">The object that the <see cref="T:System.Windows.Controls.ListViewItem" /> contains.</param>
		// Token: 0x0600516A RID: 20842 RVA: 0x0016D343 File Offset: 0x0016B543
		protected override void ClearContainerForItemOverride(DependencyObject element, object item)
		{
			base.ClearContainerForItemOverride(element, item);
		}

		/// <summary>Determines whether an object is a <see cref="T:System.Windows.Controls.ListViewItem" />.</summary>
		/// <param name="item">The object to evaluate.</param>
		/// <returns>
		///     <see langword="true" /> if the <paramref name="item" /> is a <see cref="T:System.Windows.Controls.ListViewItem" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600516B RID: 20843 RVA: 0x0016D34D File Offset: 0x0016B54D
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ListViewItem;
		}

		/// <summary>Creates and returns a new <see cref="T:System.Windows.Controls.ListViewItem" /> container.</summary>
		/// <returns>A new <see cref="T:System.Windows.Controls.ListViewItem" /> control.</returns>
		// Token: 0x0600516C RID: 20844 RVA: 0x0016D358 File Offset: 0x0016B558
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ListViewItem();
		}

		/// <summary>Responds to an <see cref="M:System.Windows.Controls.ItemsControl.OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs)" />. </summary>
		/// <param name="e">The event arguments.</param>
		// Token: 0x0600516D RID: 20845 RVA: 0x0016D360 File Offset: 0x0016B560
		protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
		{
			base.OnItemsChanged(e);
			ListViewAutomationPeer listViewAutomationPeer = UIElementAutomationPeer.FromElement(this) as ListViewAutomationPeer;
			if (listViewAutomationPeer != null && listViewAutomationPeer.ViewAutomationPeer != null)
			{
				listViewAutomationPeer.ViewAutomationPeer.ItemsChanged(e);
			}
		}

		/// <summary>Defines an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.ListView" /> control.</summary>
		/// <returns>Returns a <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" /> object for the <see cref="T:System.Windows.Controls.ListView" /> control.</returns>
		// Token: 0x0600516E RID: 20846 RVA: 0x0016D398 File Offset: 0x0016B598
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			ListViewAutomationPeer listViewAutomationPeer = new ListViewAutomationPeer(this);
			if (listViewAutomationPeer != null && this.View != null)
			{
				listViewAutomationPeer.ViewAutomationPeer = this.View.GetAutomationPeer(this);
			}
			return listViewAutomationPeer;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x0016D3CC File Offset: 0x0016B5CC
		private void ApplyNewView()
		{
			ViewBase view = this.View;
			if (view != null)
			{
				base.DefaultStyleKey = view.DefaultStyleKey;
			}
			else
			{
				base.ClearValue(FrameworkElement.DefaultStyleKeyProperty);
			}
			if (base.IsLoaded)
			{
				base.ItemContainerGenerator.Refresh();
			}
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x0016D40F File Offset: 0x0016B60F
		internal override void OnThemeChanged()
		{
			if (!base.HasTemplateGeneratedSubTree && this.View != null)
			{
				this.View.OnThemeChanged();
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ListView.View" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ListView.View" /> dependency property.</returns>
		// Token: 0x04002C65 RID: 11365
		public static readonly DependencyProperty ViewProperty = DependencyProperty.Register("View", typeof(ViewBase), typeof(ListView), new PropertyMetadata(new PropertyChangedCallback(ListView.OnViewChanged)));

		// Token: 0x04002C66 RID: 11366
		private ViewBase _previousView;
	}
}
