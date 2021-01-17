using System;
using System.Windows.Automation.Peers;
using MS.Internal.KnownBoxes;
using MS.Internal.Telemetry.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary> Control that is used to separate items in items controls. </summary>
	// Token: 0x02000531 RID: 1329
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public class Separator : Control
	{
		// Token: 0x060055F3 RID: 22003 RVA: 0x0017CF8C File Offset: 0x0017B18C
		static Separator()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(typeof(Separator)));
			Separator._dType = DependencyObjectType.FromSystemTypeInternal(typeof(Separator));
			UIElement.IsEnabledProperty.OverrideMetadata(typeof(Separator), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox));
			ControlsTraceLogger.AddControl(TelemetryControls.Separator);
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x0017CFF9 File Offset: 0x0017B1F9
		internal static void PrepareContainer(Control container)
		{
			if (container != null)
			{
				container.IsEnabled = false;
				container.HorizontalContentAlignment = HorizontalAlignment.Stretch;
			}
		}

		/// <summary>Provides an appropriate <see cref="T:System.Windows.Automation.Peers.SeparatorAutomationPeer" /> implementation for this control, as part of the WPF automation infrastructure.</summary>
		/// <returns>The type-specific <see cref="T:System.Windows.Automation.Peers.AutomationPeer" /> implementation.</returns>
		// Token: 0x060055F5 RID: 22005 RVA: 0x0017D00C File Offset: 0x0017B20C
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new SeparatorAutomationPeer(this);
		}

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x060055F6 RID: 22006 RVA: 0x0017D014 File Offset: 0x0017B214
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return Separator._dType;
			}
		}

		// Token: 0x04002E20 RID: 11808
		private static DependencyObjectType _dType;
	}
}
