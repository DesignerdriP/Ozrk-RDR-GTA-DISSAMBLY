using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020007EB RID: 2027
	internal static class ControlsTraceLogger
	{
		// Token: 0x06007D23 RID: 32035 RVA: 0x00232D3C File Offset: 0x00230F3C
		internal static void LogUsedControlsDetails()
		{
			EventSource provider = TraceLoggingProvider.GetProvider();
			if (provider != null)
			{
				provider.Write(ControlsTraceLogger.ControlsUsed, TelemetryEventSource.MeasuresOptions(), new
				{
					ControlsUsedInApp = ControlsTraceLogger._telemetryControls
				});
			}
		}

		// Token: 0x06007D24 RID: 32036 RVA: 0x00232D6C File Offset: 0x00230F6C
		internal static void AddControl(TelemetryControls control)
		{
			ControlsTraceLogger._telemetryControls |= control;
		}

		// Token: 0x04003AE0 RID: 15072
		private static readonly string ControlsUsed = "ControlsUsed";

		// Token: 0x04003AE1 RID: 15073
		private static TelemetryControls _telemetryControls = TelemetryControls.None;
	}
}
