using System;
using System.Diagnostics.Tracing;

namespace MS.Internal.Telemetry.PresentationFramework
{
	// Token: 0x020007E9 RID: 2025
	internal static class TraceLoggingProvider
	{
		// Token: 0x06007D21 RID: 32033 RVA: 0x00232CB8 File Offset: 0x00230EB8
		internal static EventSource GetProvider()
		{
			if (TraceLoggingProvider._logger == null)
			{
				object lockObject = TraceLoggingProvider._lockObject;
				lock (lockObject)
				{
					if (TraceLoggingProvider._logger == null)
					{
						try
						{
							TraceLoggingProvider._logger = new TelemetryEventSource(TraceLoggingProvider.ProviderName);
						}
						catch (ArgumentException)
						{
						}
					}
				}
			}
			return TraceLoggingProvider._logger;
		}

		// Token: 0x04003AB0 RID: 15024
		private static EventSource _logger;

		// Token: 0x04003AB1 RID: 15025
		private static object _lockObject = new object();

		// Token: 0x04003AB2 RID: 15026
		private static readonly string ProviderName = "Microsoft.DOTNET.WPF.PresentationFramework";
	}
}
