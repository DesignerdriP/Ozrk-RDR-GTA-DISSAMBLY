using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020005E1 RID: 1505
	internal static class TraceShell
	{
		// Token: 0x0600645C RID: 25692 RVA: 0x001C293D File Offset: 0x001C0B3D
		static TraceShell()
		{
			TraceShell._avTrace.EnabledByDebugger = true;
		}

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x0600645D RID: 25693 RVA: 0x001C2974 File Offset: 0x001C0B74
		public static AvTraceDetails NotOnWindows7
		{
			get
			{
				if (TraceShell._NotOnWindows7 == null)
				{
					TraceShell._NotOnWindows7 = new AvTraceDetails(1, new string[]
					{
						"Shell integration features are not being applied because the host OS does not support the feature."
					});
				}
				return TraceShell._NotOnWindows7;
			}
		}

		// Token: 0x17001820 RID: 6176
		// (get) Token: 0x0600645E RID: 25694 RVA: 0x001C299B File Offset: 0x001C0B9B
		public static AvTraceDetails ExplorerTaskbarTimeout
		{
			get
			{
				if (TraceShell._ExplorerTaskbarTimeout == null)
				{
					TraceShell._ExplorerTaskbarTimeout = new AvTraceDetails(2, new string[]
					{
						"Communication with Explorer timed out while trying to update the taskbar item for the window."
					});
				}
				return TraceShell._ExplorerTaskbarTimeout;
			}
		}

		// Token: 0x17001821 RID: 6177
		// (get) Token: 0x0600645F RID: 25695 RVA: 0x001C29C2 File Offset: 0x001C0BC2
		public static AvTraceDetails ExplorerTaskbarRetrying
		{
			get
			{
				if (TraceShell._ExplorerTaskbarRetrying == null)
				{
					TraceShell._ExplorerTaskbarRetrying = new AvTraceDetails(3, new string[]
					{
						"Making another attempt to update the taskbar."
					});
				}
				return TraceShell._ExplorerTaskbarRetrying;
			}
		}

		// Token: 0x17001822 RID: 6178
		// (get) Token: 0x06006460 RID: 25696 RVA: 0x001C29E9 File Offset: 0x001C0BE9
		public static AvTraceDetails ExplorerTaskbarNotRunning
		{
			get
			{
				if (TraceShell._ExplorerTaskbarNotRunning == null)
				{
					TraceShell._ExplorerTaskbarNotRunning = new AvTraceDetails(4, new string[]
					{
						"Halting attempts at Shell integration with the taskbar because it appears that Explorer is not running."
					});
				}
				return TraceShell._ExplorerTaskbarNotRunning;
			}
		}

		// Token: 0x06006461 RID: 25697 RVA: 0x001C2A10 File Offset: 0x001C0C10
		public static AvTraceDetails NativeTaskbarError(params object[] args)
		{
			if (TraceShell._NativeTaskbarError == null)
			{
				TraceShell._NativeTaskbarError = new AvTraceDetails(5, new string[]
				{
					"The native ITaskbarList3 interface failed a method call with error {0}."
				});
			}
			return new AvTraceFormat(TraceShell._NativeTaskbarError, args);
		}

		// Token: 0x17001823 RID: 6179
		// (get) Token: 0x06006462 RID: 25698 RVA: 0x001C2A3D File Offset: 0x001C0C3D
		public static AvTraceDetails RejectingJumpItemsBecauseCatastrophicFailure
		{
			get
			{
				if (TraceShell._RejectingJumpItemsBecauseCatastrophicFailure == null)
				{
					TraceShell._RejectingJumpItemsBecauseCatastrophicFailure = new AvTraceDetails(6, new string[]
					{
						"Failed to apply items to the JumpList because the native interfaces failed."
					});
				}
				return TraceShell._RejectingJumpItemsBecauseCatastrophicFailure;
			}
		}

		// Token: 0x06006463 RID: 25699 RVA: 0x001C2A64 File Offset: 0x001C0C64
		public static AvTraceDetails RejectingJumpListCategoryBecauseNoRegisteredHandler(params object[] args)
		{
			if (TraceShell._RejectingJumpListCategoryBecauseNoRegisteredHandler == null)
			{
				TraceShell._RejectingJumpListCategoryBecauseNoRegisteredHandler = new AvTraceDetails(7, new string[]
				{
					"Rejecting the category {0} from the jump list because this application is not registered for file types contained in the list.  JumpPath items will be removed and the operation will be retried."
				});
			}
			return new AvTraceFormat(TraceShell._RejectingJumpListCategoryBecauseNoRegisteredHandler, args);
		}

		// Token: 0x06006464 RID: 25700 RVA: 0x001C2A91 File Offset: 0x001C0C91
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006465 RID: 25701 RVA: 0x001C2AB1 File Offset: 0x001C0CB1
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006466 RID: 25702 RVA: 0x001C2AD8 File Offset: 0x001C0CD8
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006467 RID: 25703 RVA: 0x001C2B0C File Offset: 0x001C0D0C
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006468 RID: 25704 RVA: 0x001C2B44 File Offset: 0x001C0D44
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceShell._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06006469 RID: 25705 RVA: 0x001C2B81 File Offset: 0x001C0D81
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x0600646A RID: 25706 RVA: 0x001C2BA0 File Offset: 0x001C0DA0
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600646B RID: 25707 RVA: 0x001C2BC4 File Offset: 0x001C0DC4
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600646C RID: 25708 RVA: 0x001C2BEC File Offset: 0x001C0DEC
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600646D RID: 25709 RVA: 0x001C2C18 File Offset: 0x001C0E18
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceShell._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x17001824 RID: 6180
		// (get) Token: 0x0600646E RID: 25710 RVA: 0x001C2C48 File Offset: 0x001C0E48
		public static bool IsEnabled
		{
			get
			{
				return TraceShell._avTrace != null && TraceShell._avTrace.IsEnabled;
			}
		}

		// Token: 0x17001825 RID: 6181
		// (get) Token: 0x0600646F RID: 25711 RVA: 0x001C2C5D File Offset: 0x001C0E5D
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceShell._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x06006470 RID: 25712 RVA: 0x001C2C69 File Offset: 0x001C0E69
		public static void Refresh()
		{
			TraceShell._avTrace.Refresh();
		}

		// Token: 0x0400329C RID: 12956
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.ShellSource, delegate()
		{
			PresentationTraceSources._ShellSource = null;
		});

		// Token: 0x0400329D RID: 12957
		private static AvTraceDetails _NotOnWindows7;

		// Token: 0x0400329E RID: 12958
		private static AvTraceDetails _ExplorerTaskbarTimeout;

		// Token: 0x0400329F RID: 12959
		private static AvTraceDetails _ExplorerTaskbarRetrying;

		// Token: 0x040032A0 RID: 12960
		private static AvTraceDetails _ExplorerTaskbarNotRunning;

		// Token: 0x040032A1 RID: 12961
		private static AvTraceDetails _NativeTaskbarError;

		// Token: 0x040032A2 RID: 12962
		private static AvTraceDetails _RejectingJumpItemsBecauseCatastrophicFailure;

		// Token: 0x040032A3 RID: 12963
		private static AvTraceDetails _RejectingJumpListCategoryBecauseNoRegisteredHandler;
	}
}
