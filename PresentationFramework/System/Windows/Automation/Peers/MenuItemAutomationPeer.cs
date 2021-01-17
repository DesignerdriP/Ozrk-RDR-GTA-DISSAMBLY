using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using MS.Internal.KnownBoxes;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.MenuItem" /> types to UI Automation.</summary>
	// Token: 0x020002D2 RID: 722
	public class MenuItemAutomationPeer : FrameworkElementAutomationPeer, IExpandCollapseProvider, IInvokeProvider, IToggleProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.MenuItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" />.</param>
		// Token: 0x06002782 RID: 10114 RVA: 0x000B2FD9 File Offset: 0x000B11D9
		public MenuItemAutomationPeer(MenuItem owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.Controls.MenuItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "MenuItem".</returns>
		// Token: 0x06002783 RID: 10115 RVA: 0x000BA161 File Offset: 0x000B8361
		protected override string GetClassNameCore()
		{
			return "MenuItem";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.MenuItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />.</summary>
		/// <returns>The <see cref="F:System.Windows.Automation.Peers.AutomationControlType.MenuItem" /> enumeration value.</returns>
		// Token: 0x06002784 RID: 10116 RVA: 0x000956F3 File Offset: 0x000938F3
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.MenuItem;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.MenuItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" />.</summary>
		/// <param name="patternInterface">One of the enumeration values.</param>
		/// <returns>An object that supports the control pattern if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />. </returns>
		// Token: 0x06002785 RID: 10117 RVA: 0x000BA168 File Offset: 0x000B8368
		public override object GetPattern(PatternInterface patternInterface)
		{
			object result = null;
			MenuItem menuItem = (MenuItem)base.Owner;
			if (patternInterface == PatternInterface.ExpandCollapse)
			{
				MenuItemRole role = menuItem.Role;
				if ((role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader) && menuItem.HasItems)
				{
					result = this;
				}
			}
			else if (patternInterface == PatternInterface.Toggle)
			{
				if (menuItem.IsCheckable)
				{
					result = this;
				}
			}
			else if (patternInterface == PatternInterface.Invoke)
			{
				MenuItemRole role2 = menuItem.Role;
				if ((role2 == MenuItemRole.TopLevelItem || role2 == MenuItemRole.SubmenuItem) && !menuItem.HasItems)
				{
					result = this;
				}
			}
			else if (patternInterface == PatternInterface.SynchronizedInput)
			{
				result = base.GetPattern(patternInterface);
			}
			return result;
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000BA1E0 File Offset: 0x000B83E0
		protected override int GetSizeOfSetCore()
		{
			int num = base.GetSizeOfSetCore();
			if (num == -1)
			{
				MenuItem menuItem = (MenuItem)base.Owner;
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(menuItem);
				num = ItemAutomationPeer.GetSizeOfSetFromItemsControl(itemsControl, menuItem);
			}
			return num;
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000BA214 File Offset: 0x000B8414
		protected override int GetPositionInSetCore()
		{
			int num = base.GetPositionInSetCore();
			if (num == -1)
			{
				MenuItem menuItem = (MenuItem)base.Owner;
				ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(menuItem);
				num = ItemAutomationPeer.GetPositionInSetFromItemsControl(itemsControl, menuItem);
			}
			return num;
		}

		/// <summary>Gets the access key for the <see cref="T:System.Windows.Controls.MenuItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAccessKey" />.</summary>
		/// <returns>The access key for the <see cref="T:System.Windows.Controls.MenuItem" />.</returns>
		// Token: 0x06002788 RID: 10120 RVA: 0x000BA248 File Offset: 0x000B8448
		protected override string GetAccessKeyCore()
		{
			string text = base.GetAccessKeyCore();
			if (!string.IsNullOrEmpty(text))
			{
				MenuItem menuItem = (MenuItem)base.Owner;
				MenuItemRole role = menuItem.Role;
				if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.TopLevelItem)
				{
					text = "Alt+" + text;
				}
			}
			return text;
		}

		/// <summary>Gets the collection of child elements of the <see cref="T:System.Windows.Controls.MenuItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.</summary>
		/// <returns>The collection of child elements.</returns>
		// Token: 0x06002789 RID: 10121 RVA: 0x000BA28C File Offset: 0x000B848C
		protected override List<AutomationPeer> GetChildrenCore()
		{
			List<AutomationPeer> list = base.GetChildrenCore();
			if (ExpandCollapseState.Expanded == ((IExpandCollapseProvider)this).ExpandCollapseState)
			{
				ItemsControl itemsControl = (ItemsControl)base.Owner;
				ItemCollection items = itemsControl.Items;
				if (items.Count > 0)
				{
					list = new List<AutomationPeer>(items.Count);
					for (int i = 0; i < items.Count; i++)
					{
						UIElement uielement = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as UIElement;
						if (uielement != null)
						{
							AutomationPeer automationPeer = UIElementAutomationPeer.FromElement(uielement);
							if (automationPeer == null)
							{
								automationPeer = UIElementAutomationPeer.CreatePeerForElement(uielement);
							}
							if (automationPeer != null)
							{
								list.Add(automationPeer);
							}
						}
					}
				}
			}
			return list;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600278A RID: 10122 RVA: 0x000BA31C File Offset: 0x000B851C
		void IExpandCollapseProvider.Expand()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			MenuItemRole role = menuItem.Role;
			if ((role != MenuItemRole.TopLevelHeader && role != MenuItemRole.SubmenuHeader) || !menuItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			menuItem.OpenMenu();
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600278B RID: 10123 RVA: 0x000BA374 File Offset: 0x000B8574
		void IExpandCollapseProvider.Collapse()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			MenuItemRole role = menuItem.Role;
			if ((role != MenuItemRole.TopLevelHeader && role != MenuItemRole.SubmenuHeader) || !menuItem.HasItems)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			menuItem.SetCurrentValueInternal(MenuItem.IsSubmenuOpenProperty, BooleanBoxes.FalseBox);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The state (expanded or collapsed) of the control.</returns>
		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x0600278C RID: 10124 RVA: 0x000BA3D4 File Offset: 0x000B85D4
		ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				ExpandCollapseState result = ExpandCollapseState.Collapsed;
				MenuItem menuItem = (MenuItem)base.Owner;
				MenuItemRole role = menuItem.Role;
				if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem || !menuItem.HasItems)
				{
					result = ExpandCollapseState.LeafNode;
				}
				else if (menuItem.IsSubmenuOpen)
				{
					result = ExpandCollapseState.Expanded;
				}
				return result;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600278D RID: 10125 RVA: 0x000BA414 File Offset: 0x000B8614
		void IInvokeProvider.Invoke()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			MenuItemRole role = menuItem.Role;
			if (role == MenuItemRole.TopLevelItem || role == MenuItemRole.SubmenuItem)
			{
				menuItem.ClickItem();
				return;
			}
			if (role == MenuItemRole.TopLevelHeader || role == MenuItemRole.SubmenuHeader)
			{
				menuItem.ClickHeader();
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x0600278E RID: 10126 RVA: 0x000BA460 File Offset: 0x000B8660
		void IToggleProvider.Toggle()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			MenuItem menuItem = (MenuItem)base.Owner;
			if (!menuItem.IsCheckable)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
			menuItem.SetCurrentValueInternal(MenuItem.IsCheckedProperty, BooleanBoxes.Box(!menuItem.IsChecked));
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The toggle state of the control.</returns>
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000BA4B8 File Offset: 0x000B86B8
		ToggleState IToggleProvider.ToggleState
		{
			get
			{
				MenuItem menuItem = (MenuItem)base.Owner;
				if (!menuItem.IsChecked)
				{
					return ToggleState.Off;
				}
				return ToggleState.On;
			}
		}

		// Token: 0x06002790 RID: 10128 RVA: 0x000B3F68 File Offset: 0x000B2168
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void RaiseExpandCollapseAutomationEvent(bool oldValue, bool newValue)
		{
			base.RaisePropertyChangedEvent(ExpandCollapsePatternIdentifiers.ExpandCollapseStateProperty, oldValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed, newValue ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed);
		}

		/// <summary>Gets the text label of the <see cref="T:System.Windows.Controls.MenuItem" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.MenuItemAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetName" />.</summary>
		/// <returns>The string that contains the label.</returns>
		// Token: 0x06002791 RID: 10129 RVA: 0x000BA4DC File Offset: 0x000B86DC
		protected override string GetNameCore()
		{
			string nameCore = base.GetNameCore();
			if (!string.IsNullOrEmpty(nameCore))
			{
				MenuItem menuItem = (MenuItem)base.Owner;
				if (menuItem.Header is string)
				{
					return AccessText.RemoveAccessKeyMarker(nameCore);
				}
			}
			return nameCore;
		}
	}
}
