using System;
using System.Runtime.CompilerServices;

namespace MS.Internal
{
	// Token: 0x020005DD RID: 1501
	internal static class FrameworkAppContextSwitches
	{
		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x060063AA RID: 25514 RVA: 0x001C098E File Offset: 0x001BEB8E
		public static bool DoNotApplyLayoutRoundingToMarginsAndBorderThickness
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness", ref FrameworkAppContextSwitches._doNotApplyLayoutRoundingToMarginsAndBorderThickness);
			}
		}

		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x060063AB RID: 25515 RVA: 0x001C099F File Offset: 0x001BEB9F
		public static bool GridStarDefinitionsCanExceedAvailableSpace
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.Grid.StarDefinitionsCanExceedAvailableSpace", ref FrameworkAppContextSwitches._gridStarDefinitionsCanExceedAvailableSpace);
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x060063AC RID: 25516 RVA: 0x001C09B0 File Offset: 0x001BEBB0
		public static bool SelectionPropertiesCanLagBehindSelectionChangedEvent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.TabControl.SelectionPropertiesCanLagBehindSelectionChangedEvent", ref FrameworkAppContextSwitches._selectionPropertiesCanLagBehindSelectionChangedEvent);
			}
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x060063AD RID: 25517 RVA: 0x001C09C1 File Offset: 0x001BEBC1
		public static bool DoNotUseFollowParentWhenBindingToADODataRelation
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Data.DoNotUseFollowParentWhenBindingToADODataRelation", ref FrameworkAppContextSwitches._doNotUseFollowParentWhenBindingToADODataRelation);
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x060063AE RID: 25518 RVA: 0x001C09D2 File Offset: 0x001BEBD2
		public static bool UseAdornerForTextboxSelectionRendering
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", ref FrameworkAppContextSwitches._useAdornerForTextboxSelectionRendering);
			}
		}

		// Token: 0x170017F0 RID: 6128
		// (get) Token: 0x060063AF RID: 25519 RVA: 0x001C09E3 File Offset: 0x001BEBE3
		public static bool AppendLocalAssemblyVersionForSourceUri
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Baml2006.AppendLocalAssemblyVersionForSourceUri", ref FrameworkAppContextSwitches._AppendLocalAssemblyVersionForSourceUriSwitchName);
			}
		}

		// Token: 0x170017F1 RID: 6129
		// (get) Token: 0x060063B0 RID: 25520 RVA: 0x001C09F4 File Offset: 0x001BEBF4
		public static bool IListIndexerHidesCustomIndexer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Data.Binding.IListIndexerHidesCustomIndexer", ref FrameworkAppContextSwitches._IListIndexerHidesCustomIndexer);
			}
		}

		// Token: 0x170017F2 RID: 6130
		// (get) Token: 0x060063B1 RID: 25521 RVA: 0x001C0A05 File Offset: 0x001BEC05
		public static bool KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement", ref FrameworkAppContextSwitches._KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement);
			}
		}

		// Token: 0x170017F3 RID: 6131
		// (get) Token: 0x060063B2 RID: 25522 RVA: 0x001C0A16 File Offset: 0x001BEC16
		public static bool ItemAutomationPeerKeepsItsItemAlive
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Automation.Peers.ItemAutomationPeerKeepsItsItemAlive", ref FrameworkAppContextSwitches._ItemAutomationPeerKeepsItsItemAlive);
			}
		}

		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x060063B3 RID: 25523 RVA: 0x001C0A27 File Offset: 0x001BEC27
		public static bool SelectorUpdatesSelectionPropertiesWhenDisconnected
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.SelectorUpdatesSelectionPropertiesWhenDisconnected", ref FrameworkAppContextSwitches._SelectorUpdatesSelectionPropertiesWhenDisconnected);
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x060063B4 RID: 25524 RVA: 0x001C0A38 File Offset: 0x001BEC38
		public static bool SelectorInDataGridUpdatesSelectionPropertiesWhenDisconnected
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.SelectorInDataGridUpdatesSelectionPropertiesWhenDisconnected", ref FrameworkAppContextSwitches._SelectorInDataGridUpdatesSelectionPropertiesWhenDisconnected);
			}
		}

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x060063B5 RID: 25525 RVA: 0x001C0A49 File Offset: 0x001BEC49
		public static bool SharedSizeGroupDoesRedundantLayout
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.Grid.SharedSizeGroupDoesRedundantLayout", ref FrameworkAppContextSwitches._SharedSizeGroupDoesRedundantLayout);
			}
		}

		// Token: 0x170017F7 RID: 6135
		// (get) Token: 0x060063B6 RID: 25526 RVA: 0x001C0A5A File Offset: 0x001BEC5A
		public static bool TextBlockReflowsAfterMeasure
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.TextBlock.ReflowsAfterMeasure", ref FrameworkAppContextSwitches._TextBlockReflowsAfterMeasure);
			}
		}

		// Token: 0x170017F8 RID: 6136
		// (get) Token: 0x060063B7 RID: 25527 RVA: 0x001C0A6B File Offset: 0x001BEC6B
		public static bool DisableDevDiv1035544
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.Disable.1035544", ref FrameworkAppContextSwitches._DisableDevDiv1035544);
			}
		}

		// Token: 0x170017F9 RID: 6137
		// (get) Token: 0x060063B8 RID: 25528 RVA: 0x001C0A7C File Offset: 0x001BEC7C
		public static bool DoNotAugmentWordBreakingUsingSpeller
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.DoNotAugmentWordBreakingUsingSpeller", ref FrameworkAppContextSwitches._DoNotAugmentWordBreakingUsingSpeller);
			}
		}

		// Token: 0x170017FA RID: 6138
		// (get) Token: 0x060063B9 RID: 25529 RVA: 0x001C0A8D File Offset: 0x001BEC8D
		public static bool OptOutOfEffectiveOffsetHangFix
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Controls.VirtualizingStackPanel.OptOutOfEffectiveOffsetHangFix", ref FrameworkAppContextSwitches._OptOutOfEffectiveOffsetHangFix);
			}
		}

		// Token: 0x170017FB RID: 6139
		// (get) Token: 0x060063BA RID: 25530 RVA: 0x001C0A9E File Offset: 0x001BEC9E
		public static bool OptOutOfFixedDocumentModelConstructionFix
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return LocalAppContext.GetCachedSwitchValue("Switch.System.Windows.Documents.FixedDocument.OptOutOfModelConstructionFix", ref FrameworkAppContextSwitches._OptOutOfFixedDocumentModelConstructionFix);
			}
		}

		// Token: 0x040031ED RID: 12781
		internal const string DoNotApplyLayoutRoundingToMarginsAndBorderThicknessSwitchName = "Switch.MS.Internal.DoNotApplyLayoutRoundingToMarginsAndBorderThickness";

		// Token: 0x040031EE RID: 12782
		private static int _doNotApplyLayoutRoundingToMarginsAndBorderThickness;

		// Token: 0x040031EF RID: 12783
		internal const string GridStarDefinitionsCanExceedAvailableSpaceSwitchName = "Switch.System.Windows.Controls.Grid.StarDefinitionsCanExceedAvailableSpace";

		// Token: 0x040031F0 RID: 12784
		private static int _gridStarDefinitionsCanExceedAvailableSpace;

		// Token: 0x040031F1 RID: 12785
		internal const string SelectionPropertiesCanLagBehindSelectionChangedEventSwitchName = "Switch.System.Windows.Controls.TabControl.SelectionPropertiesCanLagBehindSelectionChangedEvent";

		// Token: 0x040031F2 RID: 12786
		private static int _selectionPropertiesCanLagBehindSelectionChangedEvent;

		// Token: 0x040031F3 RID: 12787
		internal const string DoNotUseFollowParentWhenBindingToADODataRelationSwitchName = "Switch.System.Windows.Data.DoNotUseFollowParentWhenBindingToADODataRelation";

		// Token: 0x040031F4 RID: 12788
		private static int _doNotUseFollowParentWhenBindingToADODataRelation;

		// Token: 0x040031F5 RID: 12789
		internal const string UseAdornerForTextboxSelectionRenderingSwitchName = "Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering";

		// Token: 0x040031F6 RID: 12790
		private static int _useAdornerForTextboxSelectionRendering;

		// Token: 0x040031F7 RID: 12791
		internal const string AppendLocalAssemblyVersionForSourceUriSwitchName = "Switch.System.Windows.Baml2006.AppendLocalAssemblyVersionForSourceUri";

		// Token: 0x040031F8 RID: 12792
		private static int _AppendLocalAssemblyVersionForSourceUriSwitchName;

		// Token: 0x040031F9 RID: 12793
		internal const string IListIndexerHidesCustomIndexerSwitchName = "Switch.System.Windows.Data.Binding.IListIndexerHidesCustomIndexer";

		// Token: 0x040031FA RID: 12794
		private static int _IListIndexerHidesCustomIndexer;

		// Token: 0x040031FB RID: 12795
		internal const string KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElementSwitchName = "Switch.System.Windows.Controls.KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement";

		// Token: 0x040031FC RID: 12796
		private static int _KeyboardNavigationFromHyperlinkInItemsControlIsNotRelativeToFocusedElement;

		// Token: 0x040031FD RID: 12797
		internal const string ItemAutomationPeerKeepsItsItemAliveSwitchName = "Switch.System.Windows.Automation.Peers.ItemAutomationPeerKeepsItsItemAlive";

		// Token: 0x040031FE RID: 12798
		private static int _ItemAutomationPeerKeepsItsItemAlive;

		// Token: 0x040031FF RID: 12799
		internal const string SelectorUpdatesSelectionPropertiesWhenDisconnectedSwitchName = "Switch.System.Windows.Controls.SelectorUpdatesSelectionPropertiesWhenDisconnected";

		// Token: 0x04003200 RID: 12800
		private static int _SelectorUpdatesSelectionPropertiesWhenDisconnected;

		// Token: 0x04003201 RID: 12801
		internal const string SelectorInDataGridUpdatesSelectionPropertiesWhenDisconnectedSwitchName = "Switch.System.Windows.Controls.SelectorInDataGridUpdatesSelectionPropertiesWhenDisconnected";

		// Token: 0x04003202 RID: 12802
		private static int _SelectorInDataGridUpdatesSelectionPropertiesWhenDisconnected;

		// Token: 0x04003203 RID: 12803
		internal const string SharedSizeGroupDoesRedundantLayoutSwitchName = "Switch.System.Windows.Controls.Grid.SharedSizeGroupDoesRedundantLayout";

		// Token: 0x04003204 RID: 12804
		private static int _SharedSizeGroupDoesRedundantLayout;

		// Token: 0x04003205 RID: 12805
		internal const string TextBlockReflowsAfterMeasureSwitchName = "Switch.System.Windows.Controls.TextBlock.ReflowsAfterMeasure";

		// Token: 0x04003206 RID: 12806
		private static int _TextBlockReflowsAfterMeasure;

		// Token: 0x04003207 RID: 12807
		internal const string DisableDevDiv1035544SwitchName = "Switch.System.Windows.Controls.Disable.1035544";

		// Token: 0x04003208 RID: 12808
		private static int _DisableDevDiv1035544;

		// Token: 0x04003209 RID: 12809
		internal const string DoNotAugmentWordBreakingUsingSpellerSwitchName = "Switch.System.Windows.Controls.DoNotAugmentWordBreakingUsingSpeller";

		// Token: 0x0400320A RID: 12810
		private static int _DoNotAugmentWordBreakingUsingSpeller;

		// Token: 0x0400320B RID: 12811
		internal const string OptOutOfEffectiveOffsetHangFixSwitchName = "Switch.System.Windows.Controls.VirtualizingStackPanel.OptOutOfEffectiveOffsetHangFix";

		// Token: 0x0400320C RID: 12812
		private static int _OptOutOfEffectiveOffsetHangFix;

		// Token: 0x0400320D RID: 12813
		internal const string OptOutOfFixedDocumentModelConstructionFixSwitchName = "Switch.System.Windows.Documents.FixedDocument.OptOutOfModelConstructionFix";

		// Token: 0x0400320E RID: 12814
		private static int _OptOutOfFixedDocumentModelConstructionFix;
	}
}
