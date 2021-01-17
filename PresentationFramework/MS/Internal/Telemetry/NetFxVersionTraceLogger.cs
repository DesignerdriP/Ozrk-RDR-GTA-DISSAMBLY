using System;
using System.Diagnostics.Tracing;
using MS.Internal.Telemetry.PresentationFramework;

namespace MS.Internal.Telemetry
{
	// Token: 0x020007E6 RID: 2022
	internal static class NetFxVersionTraceLogger
	{
		// Token: 0x06007D06 RID: 32006 RVA: 0x002328DC File Offset: 0x00230ADC
		internal static void LogVersionDetails()
		{
			EventSource provider = TraceLoggingProvider.GetProvider();
			NetFxVersionTraceLogger.VersionInfo data = default(NetFxVersionTraceLogger.VersionInfo);
			data.TargetFrameworkVersion = NetfxVersionHelper.GetTargetFrameworkVersion();
			data.NetfxReleaseVersion = NetfxVersionHelper.GetNetFXReleaseVersion();
			if (provider != null)
			{
				provider.Write<NetFxVersionTraceLogger.VersionInfo>(NetFxVersionTraceLogger.NetFxVersion, TelemetryEventSource.MeasuresOptions(), data);
			}
		}

		// Token: 0x04003A99 RID: 15001
		private static readonly string NetFxVersion = "NetFxVersion";

		// Token: 0x02000B85 RID: 2949
		[EventData]
		private struct VersionInfo
		{
			// Token: 0x17001FB2 RID: 8114
			// (get) Token: 0x06008E69 RID: 36457 RVA: 0x0025C3AD File Offset: 0x0025A5AD
			// (set) Token: 0x06008E6A RID: 36458 RVA: 0x0025C3B5 File Offset: 0x0025A5B5
			public string TargetFrameworkVersion
			{
				get
				{
					return this._targetFrameworkVersion;
				}
				set
				{
					this._targetFrameworkVersion = value;
				}
			}

			// Token: 0x17001FB3 RID: 8115
			// (get) Token: 0x06008E6B RID: 36459 RVA: 0x0025C3BE File Offset: 0x0025A5BE
			// (set) Token: 0x06008E6C RID: 36460 RVA: 0x0025C3C6 File Offset: 0x0025A5C6
			public int NetfxReleaseVersion
			{
				get
				{
					return this._netfxReleaseVersion;
				}
				set
				{
					this._netfxReleaseVersion = value;
				}
			}

			// Token: 0x04004B8F RID: 19343
			private string _targetFrameworkVersion;

			// Token: 0x04004B90 RID: 19344
			private int _netfxReleaseVersion;
		}
	}
}
