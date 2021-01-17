using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls.Primitives;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes the items in the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection of a <see cref="T:System.Windows.Controls.Primitives.Selector" /> to UI Automation. </summary>
	// Token: 0x020002DE RID: 734
	public abstract class SelectorItemAutomationPeer : ItemAutomationPeer, ISelectionItemProvider
	{
		/// <summary>Provides initialization for base class values when they are called by the constructor of a derived class.</summary>
		/// <param name="owner">The item object that is associated with this <see cref="T:System.Windows.Automation.Peers.SelectorItemAutomationPeer" />.</param>
		/// <param name="selectorAutomationPeer">The <see cref="T:System.Windows.Automation.Peers.SelectorAutomationPeer" /> that is associated with the <see cref="T:System.Windows.Controls.Primitives.Selector" /> that holds the <see cref="P:System.Windows.Controls.ItemsControl.Items" /> collection.</param>
		// Token: 0x060027F2 RID: 10226 RVA: 0x000B9F49 File Offset: 0x000B8149
		protected SelectorItemAutomationPeer(object owner, SelectorAutomationPeer selectorAutomationPeer) : base(owner, selectorAutomationPeer)
		{
		}

		/// <summary>Gets the control pattern that is associated with the specified <see cref="T:System.Windows.Automation.Peers.PatternInterface" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>If <paramref name="patternInterface" /> is <see cref="F:System.Windows.Automation.Peers.PatternInterface.Selection" />, this method returns the current instance of this <see cref="T:System.Windows.Automation.Peers.SelectorItemAutomationPeer" />.</returns>
		// Token: 0x060027F3 RID: 10227 RVA: 0x000BB3A5 File Offset: 0x000B95A5
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.SelectionItem)
			{
				return this;
			}
			return base.GetPattern(patternInterface);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060027F4 RID: 10228 RVA: 0x000BB3B8 File Offset: 0x000B95B8
		void ISelectionItemProvider.Select()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			if (selector == null)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			selector.SelectionChange.SelectJustThisItem(selector.NewItemInfo(base.Item, null, -1), true);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060027F5 RID: 10229 RVA: 0x000BB414 File Offset: 0x000B9614
		void ISelectionItemProvider.AddToSelection()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			if (selector == null || (!selector.CanSelectMultiple && selector.SelectedItem != null && selector.SelectedItem != base.Item))
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			selector.SelectionChange.Begin();
			selector.SelectionChange.Select(selector.NewItemInfo(base.Item, null, -1), true);
			selector.SelectionChange.End();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060027F6 RID: 10230 RVA: 0x000BB4A4 File Offset: 0x000B96A4
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
			selector.SelectionChange.Begin();
			selector.SelectionChange.Unselect(selector.NewItemInfo(base.Item, null, -1));
			selector.SelectionChange.End();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element is selected; otherwise <see langword="false" />.</returns>
		// Token: 0x170009B8 RID: 2488
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000BB500 File Offset: 0x000B9700
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				Selector selector = (Selector)base.ItemsControlAutomationPeer.Owner;
				return selector._selectedItems.Contains(selector.NewItemInfo(base.Item, null, -1));
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The selection container.</returns>
		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000BB537 File Offset: 0x000B9737
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return base.ProviderFromPeer(base.ItemsControlAutomationPeer);
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000BB545 File Offset: 0x000B9745
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseAutomationIsSelectedChanged(bool isSelected)
		{
			base.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, !isSelected, isSelected);
		}
	}
}
