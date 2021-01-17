﻿using System;
using System.Runtime.CompilerServices;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace System.Windows.Automation.Peers
{
	/// <summary>Exposes <see cref="T:System.Windows.Controls.RadioButton" /> types to UI Automation.</summary>
	// Token: 0x020002D7 RID: 727
	public class RadioButtonAutomationPeer : ToggleButtonAutomationPeer, ISelectionItemProvider
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Automation.Peers.RadioButtonAutomationPeer" /> class.</summary>
		/// <param name="owner">The <see cref="T:System.Windows.Controls.RadioButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RadioButtonAutomationPeer" />.</param>
		// Token: 0x060027AB RID: 10155 RVA: 0x000B3CC4 File Offset: 0x000B1EC4
		public RadioButtonAutomationPeer(RadioButton owner) : base(owner)
		{
		}

		/// <summary>Gets the name of the <see cref="T:System.Windows.ContentElement" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.ContentElementAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.</summary>
		/// <returns>A string that contains "RadioButton".</returns>
		// Token: 0x060027AC RID: 10156 RVA: 0x000BA6E8 File Offset: 0x000B88E8
		protected override string GetClassNameCore()
		{
			return "RadioButton";
		}

		/// <summary>Gets the control type for the <see cref="T:System.Windows.Controls.RadioButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RadioButtonAutomationPeer" />. Called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetAutomationControlType" />..</summary>
		/// <returns>The control type for the <see cref="T:System.Windows.Controls.RadioButton" /> object.</returns>
		// Token: 0x060027AD RID: 10157 RVA: 0x0003B2EB File Offset: 0x000394EB
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.RadioButton;
		}

		/// <summary>Gets the control pattern for the <see cref="T:System.Windows.Controls.RadioButton" /> that is associated with this <see cref="T:System.Windows.Automation.Peers.RadioButtonAutomationPeer" />.</summary>
		/// <param name="patternInterface">A value in the enumeration.</param>
		/// <returns>An object that supports the control pattern if <paramref name="patternInterface" /> is a supported value; otherwise, <see langword="null" />.</returns>
		// Token: 0x060027AE RID: 10158 RVA: 0x000BA6EF File Offset: 0x000B88EF
		public override object GetPattern(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.SelectionItem)
			{
				return this;
			}
			if (patternInterface == PatternInterface.SynchronizedInput)
			{
				return base.GetPattern(patternInterface);
			}
			return null;
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060027AF RID: 10159 RVA: 0x000BA706 File Offset: 0x000B8906
		void ISelectionItemProvider.Select()
		{
			if (!base.IsEnabled())
			{
				throw new ElementNotEnabledException();
			}
			((RadioButton)base.Owner).SetCurrentValueInternal(ToggleButton.IsCheckedProperty, BooleanBoxes.TrueBox);
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060027B0 RID: 10160 RVA: 0x000BA730 File Offset: 0x000B8930
		void ISelectionItemProvider.AddToSelection()
		{
			if (((RadioButton)base.Owner).IsChecked != true)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x060027B1 RID: 10161 RVA: 0x000BA77C File Offset: 0x000B897C
		void ISelectionItemProvider.RemoveFromSelection()
		{
			if (((RadioButton)base.Owner).IsChecked == true)
			{
				throw new InvalidOperationException(SR.Get("UIA_OperationCannotBePerformed"));
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>
		///     <see langword="true" /> if the element is selected; otherwise <see langword="false" />.</returns>
		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060027B2 RID: 10162 RVA: 0x000BA7C4 File Offset: 0x000B89C4
		bool ISelectionItemProvider.IsSelected
		{
			get
			{
				return ((RadioButton)base.Owner).IsChecked == true;
			}
		}

		/// <summary>This type or member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		/// <returns>The selection container.</returns>
		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060027B3 RID: 10163 RVA: 0x0000C238 File Offset: 0x0000A438
		IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060027B4 RID: 10164 RVA: 0x000BA7F8 File Offset: 0x000B89F8
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal override void RaiseToggleStatePropertyChangedEvent(bool? oldValue, bool? newValue)
		{
			base.RaisePropertyChangedEvent(SelectionItemPatternIdentifiers.IsSelectedProperty, oldValue == true, newValue == true);
		}
	}
}
