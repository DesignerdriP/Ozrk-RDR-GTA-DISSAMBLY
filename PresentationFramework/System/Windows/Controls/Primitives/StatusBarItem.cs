using System;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an item of a <see cref="T:System.Windows.Controls.Primitives.StatusBar" /> control. </summary>
	// Token: 0x020005AA RID: 1450
	[Localizability(LocalizationCategory.Inherit)]
	public class StatusBarItem : ContentControl
	{
		// Token: 0x06006020 RID: 24608 RVA: 0x001AF4B0 File Offset: 0x001AD6B0
		static StatusBarItem()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(typeof(StatusBarItem)));
			StatusBarItem._dType = DependencyObjectType.FromSystemTypeInternal(typeof(StatusBarItem));
			Control.IsTabStopProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			AutomationProperties.IsOffscreenBehaviorProperty.OverrideMetadata(typeof(StatusBarItem), new FrameworkPropertyMetadata(IsOffscreenBehavior.FromClip));
		}

		/// <summary>Specifies an <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> for the <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" />.</summary>
		/// <returns>A <see cref="T:System.Windows.Automation.Peers.StatusBarItemAutomationPeer" /> for this <see cref="T:System.Windows.Controls.Primitives.StatusBarItem" />.</returns>
		// Token: 0x06006021 RID: 24609 RVA: 0x001AF531 File Offset: 0x001AD731
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new StatusBarItemAutomationPeer(this);
		}

		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x06006022 RID: 24610 RVA: 0x001AF539 File Offset: 0x001AD739
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return StatusBarItem._dType;
			}
		}

		// Token: 0x040030EE RID: 12526
		private static DependencyObjectType _dType;
	}
}
