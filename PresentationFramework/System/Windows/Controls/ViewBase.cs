using System;
using System.Windows.Automation.Peers;

namespace System.Windows.Controls
{
	/// <summary>Represents the base class for views that define the appearance of data in a <see cref="T:System.Windows.Controls.ListView" /> control.</summary>
	// Token: 0x02000557 RID: 1367
	public abstract class ViewBase : DependencyObject
	{
		/// <summary>Prepares an item in the view for display, by setting bindings and styles.</summary>
		/// <param name="item">The item to prepare for display.</param>
		// Token: 0x060059A5 RID: 22949 RVA: 0x00002137 File Offset: 0x00000337
		protected internal virtual void PrepareItem(ListViewItem item)
		{
		}

		/// <summary>Removes all bindings and styling that are set for an item.</summary>
		/// <param name="item">The <see cref="T:System.Windows.Controls.ListViewItem" /> to remove settings from.</param>
		// Token: 0x060059A6 RID: 22950 RVA: 0x00002137 File Offset: 0x00000337
		protected internal virtual void ClearItem(ListViewItem item)
		{
		}

		/// <summary>Gets the object that is associated with the style for the view mode.</summary>
		/// <returns>The style to use for the view mode. The default value is the style for the <see cref="T:System.Windows.Controls.ListBox" />.</returns>
		// Token: 0x170015CE RID: 5582
		// (get) Token: 0x060059A7 RID: 22951 RVA: 0x0018B8C1 File Offset: 0x00189AC1
		protected internal virtual object DefaultStyleKey
		{
			get
			{
				return typeof(ListBox);
			}
		}

		/// <summary>Gets the style to use for the items in the view mode.</summary>
		/// <returns>The style of a <see cref="T:System.Windows.Controls.ListViewItem" />. The default value is the style for the <see cref="T:System.Windows.Controls.ListBoxItem" /> control.</returns>
		// Token: 0x170015CF RID: 5583
		// (get) Token: 0x060059A8 RID: 22952 RVA: 0x0018B8CD File Offset: 0x00189ACD
		protected internal virtual object ItemContainerDefaultStyleKey
		{
			get
			{
				return typeof(ListBoxItem);
			}
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void OnThemeChanged()
		{
		}

		// Token: 0x170015D0 RID: 5584
		// (get) Token: 0x060059AA RID: 22954 RVA: 0x0018B8D9 File Offset: 0x00189AD9
		internal override DependencyObject InheritanceContext
		{
			get
			{
				return this._inheritanceContext;
			}
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x0018B8E1 File Offset: 0x00189AE1
		internal override void AddInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext != context)
			{
				this._inheritanceContext = context;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x0018B8FE File Offset: 0x00189AFE
		internal override void RemoveInheritanceContext(DependencyObject context, DependencyProperty property)
		{
			if (this._inheritanceContext == context)
			{
				this._inheritanceContext = null;
				base.OnInheritanceContextChanged(EventArgs.Empty);
			}
		}

		/// <summary>Is called when a <see cref="T:System.Windows.Controls.ListView" /> control creates a <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" /> for its <see cref="P:System.Windows.Controls.ListView.View" />.</summary>
		/// <param name="parent">The <see cref="T:System.Windows.Controls.ListView" /> control to use to create the <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" />.</param>
		/// <returns>The <see cref="T:System.Windows.Automation.Peers.IViewAutomationPeer" /> interface that implements the <see cref="T:System.Windows.Automation.Peers.ListViewAutomationPeer" /> for a custom <see cref="P:System.Windows.Controls.ListView.View" />. </returns>
		// Token: 0x060059AD RID: 22957 RVA: 0x0000C238 File Offset: 0x0000A438
		protected internal virtual IViewAutomationPeer GetAutomationPeer(ListView parent)
		{
			return null;
		}

		// Token: 0x170015D1 RID: 5585
		// (get) Token: 0x060059AE RID: 22958 RVA: 0x0018B91B File Offset: 0x00189B1B
		// (set) Token: 0x060059AF RID: 22959 RVA: 0x0018B923 File Offset: 0x00189B23
		internal bool IsUsed
		{
			get
			{
				return this._isUsed;
			}
			set
			{
				this._isUsed = value;
			}
		}

		// Token: 0x04002F1B RID: 12059
		private DependencyObject _inheritanceContext;

		// Token: 0x04002F1C RID: 12060
		private bool _isUsed;
	}
}
