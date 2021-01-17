using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020005E0 RID: 1504
	internal static class TraceHwndHost
	{
		// Token: 0x0600644D RID: 25677 RVA: 0x001C26FD File Offset: 0x001C08FD
		static TraceHwndHost()
		{
			TraceHwndHost._avTrace.EnabledByDebugger = true;
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x0600644E RID: 25678 RVA: 0x001C2734 File Offset: 0x001C0934
		public static AvTraceDetails HwndHostIn3D
		{
			get
			{
				if (TraceHwndHost._HwndHostIn3D == null)
				{
					TraceHwndHost._HwndHostIn3D = new AvTraceDetails(1, new string[]
					{
						"An HwndHost may not be embedded in a 3D scene."
					});
				}
				return TraceHwndHost._HwndHostIn3D;
			}
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x001C275B File Offset: 0x001C095B
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006450 RID: 25680 RVA: 0x001C277B File Offset: 0x001C097B
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x001C27A0 File Offset: 0x001C09A0
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006452 RID: 25682 RVA: 0x001C27D4 File Offset: 0x001C09D4
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006453 RID: 25683 RVA: 0x001C280C File Offset: 0x001C0A0C
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06006454 RID: 25684 RVA: 0x001C2849 File Offset: 0x001C0A49
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006455 RID: 25685 RVA: 0x001C2868 File Offset: 0x001C0A68
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006456 RID: 25686 RVA: 0x001C288C File Offset: 0x001C0A8C
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x001C28B4 File Offset: 0x001C0AB4
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x001C28E0 File Offset: 0x001C0AE0
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x06006459 RID: 25689 RVA: 0x001C2910 File Offset: 0x001C0B10
		public static bool IsEnabled
		{
			get
			{
				return TraceHwndHost._avTrace != null && TraceHwndHost._avTrace.IsEnabled;
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x0600645A RID: 25690 RVA: 0x001C2925 File Offset: 0x001C0B25
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceHwndHost._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x0600645B RID: 25691 RVA: 0x001C2931 File Offset: 0x001C0B31
		public static void Refresh()
		{
			TraceHwndHost._avTrace.Refresh();
		}

		// Token: 0x0400329A RID: 12954
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.HwndHostSource, delegate()
		{
			PresentationTraceSources._HwndHostSource = null;
		});

		// Token: 0x0400329B RID: 12955
		private static AvTraceDetails _HwndHostIn3D;
	}
}
