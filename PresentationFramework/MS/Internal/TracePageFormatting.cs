using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020005F7 RID: 1527
	internal static class TracePageFormatting
	{
		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x0600657A RID: 25978 RVA: 0x001C7404 File Offset: 0x001C5604
		public static AvTraceDetails FormatPage
		{
			get
			{
				if (TracePageFormatting._FormatPage == null)
				{
					TracePageFormatting._FormatPage = new AvTraceDetails(1, new string[]
					{
						"Formatting page",
						"PageContext",
						"PtsContext"
					});
				}
				return TracePageFormatting._FormatPage;
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x0600657B RID: 25979 RVA: 0x001C743B File Offset: 0x001C563B
		public static AvTraceDetails PageFormattingError
		{
			get
			{
				if (TracePageFormatting._PageFormattingError == null)
				{
					TracePageFormatting._PageFormattingError = new AvTraceDetails(2, new string[]
					{
						"Error. Page formatting engine could not complete the formatting operation.",
						"PtsContext",
						"Message"
					});
				}
				return TracePageFormatting._PageFormattingError;
			}
		}

		// Token: 0x0600657C RID: 25980 RVA: 0x001C7472 File Offset: 0x001C5672
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x001C7492 File Offset: 0x001C5692
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x001C74B8 File Offset: 0x001C56B8
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x001C74EC File Offset: 0x001C56EC
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006580 RID: 25984 RVA: 0x001C7524 File Offset: 0x001C5724
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06006581 RID: 25985 RVA: 0x001C7561 File Offset: 0x001C5761
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x001C7580 File Offset: 0x001C5780
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x001C75A4 File Offset: 0x001C57A4
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x001C75CC File Offset: 0x001C57CC
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x001C75F8 File Offset: 0x001C57F8
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x06006586 RID: 25990 RVA: 0x001C7628 File Offset: 0x001C5828
		public static bool IsEnabled
		{
			get
			{
				return TracePageFormatting._avTrace != null && TracePageFormatting._avTrace.IsEnabled;
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x06006587 RID: 25991 RVA: 0x001C763D File Offset: 0x001C583D
		public static bool IsEnabledOverride
		{
			get
			{
				return TracePageFormatting._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x001C7649 File Offset: 0x001C5849
		public static void Refresh()
		{
			TracePageFormatting._avTrace.Refresh();
		}

		// Token: 0x040032C8 RID: 13000
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.DocumentsSource, delegate()
		{
			PresentationTraceSources._DocumentsSource = null;
		});

		// Token: 0x040032C9 RID: 13001
		private static AvTraceDetails _FormatPage;

		// Token: 0x040032CA RID: 13002
		private static AvTraceDetails _PageFormattingError;
	}
}
