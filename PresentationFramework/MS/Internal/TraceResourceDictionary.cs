using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020005F8 RID: 1528
	internal static class TraceResourceDictionary
	{
		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x0600658A RID: 25994 RVA: 0x001C7681 File Offset: 0x001C5881
		public static AvTraceDetails AddResource
		{
			get
			{
				if (TraceResourceDictionary._AddResource == null)
				{
					TraceResourceDictionary._AddResource = new AvTraceDetails(1, new string[]
					{
						"Resource has been added to ResourceDictionary",
						"Dictionary",
						"ResourceKey",
						"Value"
					});
				}
				return TraceResourceDictionary._AddResource;
			}
		}

		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x0600658B RID: 25995 RVA: 0x001C76C0 File Offset: 0x001C58C0
		public static AvTraceDetails RealizeDeferContent
		{
			get
			{
				if (TraceResourceDictionary._RealizeDeferContent == null)
				{
					TraceResourceDictionary._RealizeDeferContent = new AvTraceDetails(2, new string[]
					{
						"Delayed creation of resource",
						"Dictionary",
						"ResourceKey",
						"Value"
					});
				}
				return TraceResourceDictionary._RealizeDeferContent;
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x0600658C RID: 25996 RVA: 0x001C76FF File Offset: 0x001C58FF
		public static AvTraceDetails FoundResourceOnElement
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceOnElement == null)
				{
					TraceResourceDictionary._FoundResourceOnElement = new AvTraceDetails(3, new string[]
					{
						"Found resource item on an element",
						"Element",
						"ResourceKey",
						"Value"
					});
				}
				return TraceResourceDictionary._FoundResourceOnElement;
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x0600658D RID: 25997 RVA: 0x001C7740 File Offset: 0x001C5940
		public static AvTraceDetails FoundResourceInStyle
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInStyle == null)
				{
					TraceResourceDictionary._FoundResourceInStyle = new AvTraceDetails(4, new string[]
					{
						"Found resource item in a style",
						"Dictionary",
						"ResourceKey",
						"Style",
						"Element",
						"Value"
					});
				}
				return TraceResourceDictionary._FoundResourceInStyle;
			}
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x0600658E RID: 25998 RVA: 0x001C779C File Offset: 0x001C599C
		public static AvTraceDetails FoundResourceInTemplate
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInTemplate == null)
				{
					TraceResourceDictionary._FoundResourceInTemplate = new AvTraceDetails(5, new string[]
					{
						"Found resource item in a template",
						"Dictionary",
						"ResourceKey",
						"Template",
						"Element",
						"Value"
					});
				}
				return TraceResourceDictionary._FoundResourceInTemplate;
			}
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x0600658F RID: 25999 RVA: 0x001C77F8 File Offset: 0x001C59F8
		public static AvTraceDetails FoundResourceInThemeStyle
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInThemeStyle == null)
				{
					TraceResourceDictionary._FoundResourceInThemeStyle = new AvTraceDetails(6, new string[]
					{
						"Found resource item in a theme style",
						"Dictionary",
						"ResourceKey",
						"ThemeStyle",
						"Element",
						"Value"
					});
				}
				return TraceResourceDictionary._FoundResourceInThemeStyle;
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x06006590 RID: 26000 RVA: 0x001C7852 File Offset: 0x001C5A52
		public static AvTraceDetails FoundResourceInApplication
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInApplication == null)
				{
					TraceResourceDictionary._FoundResourceInApplication = new AvTraceDetails(7, new string[]
					{
						"Found resource item in application",
						"ResourceKey",
						"Value"
					});
				}
				return TraceResourceDictionary._FoundResourceInApplication;
			}
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06006591 RID: 26001 RVA: 0x001C7889 File Offset: 0x001C5A89
		public static AvTraceDetails FoundResourceInTheme
		{
			get
			{
				if (TraceResourceDictionary._FoundResourceInTheme == null)
				{
					TraceResourceDictionary._FoundResourceInTheme = new AvTraceDetails(8, new string[]
					{
						"Found resource item in theme",
						"Dictionary",
						"ResourceKey",
						"Value"
					});
				}
				return TraceResourceDictionary._FoundResourceInTheme;
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06006592 RID: 26002 RVA: 0x001C78C8 File Offset: 0x001C5AC8
		public static AvTraceDetails ResourceNotFound
		{
			get
			{
				if (TraceResourceDictionary._ResourceNotFound == null)
				{
					TraceResourceDictionary._ResourceNotFound = new AvTraceDetails(9, new string[]
					{
						"Resource not found",
						"ResourceKey"
					});
				}
				return TraceResourceDictionary._ResourceNotFound;
			}
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06006593 RID: 26003 RVA: 0x001C78F8 File Offset: 0x001C5AF8
		public static AvTraceDetails NewResourceDictionary
		{
			get
			{
				if (TraceResourceDictionary._NewResourceDictionary == null)
				{
					TraceResourceDictionary._NewResourceDictionary = new AvTraceDetails(10, new string[]
					{
						"New resource dictionary set",
						"Owner",
						"OldDictionary",
						"NewDictionary"
					});
				}
				return TraceResourceDictionary._NewResourceDictionary;
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06006594 RID: 26004 RVA: 0x001C7938 File Offset: 0x001C5B38
		public static AvTraceDetails FindResource
		{
			get
			{
				if (TraceResourceDictionary._FindResource == null)
				{
					TraceResourceDictionary._FindResource = new AvTraceDetails(11, new string[]
					{
						"Searching for resource",
						"Element",
						"ResourceKey"
					});
				}
				return TraceResourceDictionary._FindResource;
			}
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06006595 RID: 26005 RVA: 0x001C7970 File Offset: 0x001C5B70
		public static AvTraceDetails SetKey
		{
			get
			{
				if (TraceResourceDictionary._SetKey == null)
				{
					TraceResourceDictionary._SetKey = new AvTraceDetails(12, new string[]
					{
						"Deferred resource has been added to ResourceDictionary",
						"Dictionary",
						"ResourceKey"
					});
				}
				return TraceResourceDictionary._SetKey;
			}
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x001C79A8 File Offset: 0x001C5BA8
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006597 RID: 26007 RVA: 0x001C79C8 File Offset: 0x001C5BC8
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x001C79F0 File Offset: 0x001C5BF0
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006599 RID: 26009 RVA: 0x001C7A24 File Offset: 0x001C5C24
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x001C7A5C File Offset: 0x001C5C5C
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceResourceDictionary._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x0600659B RID: 26011 RVA: 0x001C7A99 File Offset: 0x001C5C99
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x0600659C RID: 26012 RVA: 0x001C7AB8 File Offset: 0x001C5CB8
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600659D RID: 26013 RVA: 0x001C7ADC File Offset: 0x001C5CDC
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600659E RID: 26014 RVA: 0x001C7B04 File Offset: 0x001C5D04
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600659F RID: 26015 RVA: 0x001C7B30 File Offset: 0x001C5D30
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceResourceDictionary._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x060065A0 RID: 26016 RVA: 0x001C7B60 File Offset: 0x001C5D60
		public static bool IsEnabled
		{
			get
			{
				return TraceResourceDictionary._avTrace != null && TraceResourceDictionary._avTrace.IsEnabled;
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x060065A1 RID: 26017 RVA: 0x001C7B75 File Offset: 0x001C5D75
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceResourceDictionary._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x060065A2 RID: 26018 RVA: 0x001C7B81 File Offset: 0x001C5D81
		public static void Refresh()
		{
			TraceResourceDictionary._avTrace.Refresh();
		}

		// Token: 0x040032CB RID: 13003
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.ResourceDictionarySource, delegate()
		{
			PresentationTraceSources._ResourceDictionarySource = null;
		});

		// Token: 0x040032CC RID: 13004
		private static AvTraceDetails _AddResource;

		// Token: 0x040032CD RID: 13005
		private static AvTraceDetails _RealizeDeferContent;

		// Token: 0x040032CE RID: 13006
		private static AvTraceDetails _FoundResourceOnElement;

		// Token: 0x040032CF RID: 13007
		private static AvTraceDetails _FoundResourceInStyle;

		// Token: 0x040032D0 RID: 13008
		private static AvTraceDetails _FoundResourceInTemplate;

		// Token: 0x040032D1 RID: 13009
		private static AvTraceDetails _FoundResourceInThemeStyle;

		// Token: 0x040032D2 RID: 13010
		private static AvTraceDetails _FoundResourceInApplication;

		// Token: 0x040032D3 RID: 13011
		private static AvTraceDetails _FoundResourceInTheme;

		// Token: 0x040032D4 RID: 13012
		private static AvTraceDetails _ResourceNotFound;

		// Token: 0x040032D5 RID: 13013
		private static AvTraceDetails _NewResourceDictionary;

		// Token: 0x040032D6 RID: 13014
		private static AvTraceDetails _FindResource;

		// Token: 0x040032D7 RID: 13015
		private static AvTraceDetails _SetKey;
	}
}
