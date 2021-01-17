using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020005F9 RID: 1529
	internal static class TraceMarkup
	{
		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x060065A4 RID: 26020 RVA: 0x001C7BB9 File Offset: 0x001C5DB9
		public static AvTraceDetails AddValueToAddChild
		{
			get
			{
				if (TraceMarkup._AddValueToAddChild == null)
				{
					TraceMarkup._AddValueToAddChild = new AvTraceDetails(1, new string[]
					{
						"Add value to IAddChild",
						"Object",
						"Value"
					});
				}
				return TraceMarkup._AddValueToAddChild;
			}
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x060065A5 RID: 26021 RVA: 0x001C7BF0 File Offset: 0x001C5DF0
		public static AvTraceDetails AddValueToArray
		{
			get
			{
				if (TraceMarkup._AddValueToArray == null)
				{
					TraceMarkup._AddValueToArray = new AvTraceDetails(2, new string[]
					{
						"Add value to an array property",
						"Object",
						"Property",
						"Value"
					});
				}
				return TraceMarkup._AddValueToArray;
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x060065A6 RID: 26022 RVA: 0x001C7C30 File Offset: 0x001C5E30
		public static AvTraceDetails AddValueToDictionary
		{
			get
			{
				if (TraceMarkup._AddValueToDictionary == null)
				{
					TraceMarkup._AddValueToDictionary = new AvTraceDetails(3, new string[]
					{
						"Add value to a dictionary property",
						"Object",
						"Property",
						"Key",
						"Value"
					});
				}
				return TraceMarkup._AddValueToDictionary;
			}
		}

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x060065A7 RID: 26023 RVA: 0x001C7C82 File Offset: 0x001C5E82
		public static AvTraceDetails AddValueToList
		{
			get
			{
				if (TraceMarkup._AddValueToList == null)
				{
					TraceMarkup._AddValueToList = new AvTraceDetails(4, new string[]
					{
						"Add value to a collection property",
						"Object",
						"Property",
						"Value"
					});
				}
				return TraceMarkup._AddValueToList;
			}
		}

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x060065A8 RID: 26024 RVA: 0x001C7CC1 File Offset: 0x001C5EC1
		public static AvTraceDetails BeginInit
		{
			get
			{
				if (TraceMarkup._BeginInit == null)
				{
					TraceMarkup._BeginInit = new AvTraceDetails(5, new string[]
					{
						"Start initialization of object (ISupportInitialize.BeginInit)",
						"Object"
					});
				}
				return TraceMarkup._BeginInit;
			}
		}

		// Token: 0x17001865 RID: 6245
		// (get) Token: 0x060065A9 RID: 26025 RVA: 0x001C7CF0 File Offset: 0x001C5EF0
		public static AvTraceDetails CreateMarkupExtension
		{
			get
			{
				if (TraceMarkup._CreateMarkupExtension == null)
				{
					TraceMarkup._CreateMarkupExtension = new AvTraceDetails(6, new string[]
					{
						"Create MarkupExtension",
						"Type",
						"MarkupExtension"
					});
				}
				return TraceMarkup._CreateMarkupExtension;
			}
		}

		// Token: 0x17001866 RID: 6246
		// (get) Token: 0x060065AA RID: 26026 RVA: 0x001C7D27 File Offset: 0x001C5F27
		public static AvTraceDetails CreateObject
		{
			get
			{
				if (TraceMarkup._CreateObject == null)
				{
					TraceMarkup._CreateObject = new AvTraceDetails(7, new string[]
					{
						"Create object",
						"Type",
						"Value"
					});
				}
				return TraceMarkup._CreateObject;
			}
		}

		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x060065AB RID: 26027 RVA: 0x001C7D5E File Offset: 0x001C5F5E
		public static AvTraceDetails EndInit
		{
			get
			{
				if (TraceMarkup._EndInit == null)
				{
					TraceMarkup._EndInit = new AvTraceDetails(8, new string[]
					{
						"End initialization of object (ISupportInitialize.EndInit)",
						"Object"
					});
				}
				return TraceMarkup._EndInit;
			}
		}

		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x060065AC RID: 26028 RVA: 0x001C7D8D File Offset: 0x001C5F8D
		public static AvTraceDetails Load
		{
			get
			{
				if (TraceMarkup._Load == null)
				{
					TraceMarkup._Load = new AvTraceDetails(9, new string[]
					{
						"Load xaml/baml",
						"Value"
					});
				}
				return TraceMarkup._Load;
			}
		}

		// Token: 0x17001869 RID: 6249
		// (get) Token: 0x060065AD RID: 26029 RVA: 0x001C7DBD File Offset: 0x001C5FBD
		public static AvTraceDetails ProcessConstructorParameter
		{
			get
			{
				if (TraceMarkup._ProcessConstructorParameter == null)
				{
					TraceMarkup._ProcessConstructorParameter = new AvTraceDetails(10, new string[]
					{
						"Convert constructor parameter",
						"Type",
						"ConverterType",
						"Value"
					});
				}
				return TraceMarkup._ProcessConstructorParameter;
			}
		}

		// Token: 0x1700186A RID: 6250
		// (get) Token: 0x060065AE RID: 26030 RVA: 0x001C7E00 File Offset: 0x001C6000
		public static AvTraceDetails ProvideValue
		{
			get
			{
				if (TraceMarkup._ProvideValue == null)
				{
					TraceMarkup._ProvideValue = new AvTraceDetails(11, new string[]
					{
						"Converted a MarkupExtension",
						"MarkupExtension",
						"Object",
						"Property",
						"Value"
					});
				}
				return TraceMarkup._ProvideValue;
			}
		}

		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x060065AF RID: 26031 RVA: 0x001C7E53 File Offset: 0x001C6053
		public static AvTraceDetails SetCPA
		{
			get
			{
				if (TraceMarkup._SetCPA == null)
				{
					TraceMarkup._SetCPA = new AvTraceDetails(12, new string[]
					{
						"Set property value to the ContentProperty",
						"Object",
						"Value"
					});
				}
				return TraceMarkup._SetCPA;
			}
		}

		// Token: 0x1700186C RID: 6252
		// (get) Token: 0x060065B0 RID: 26032 RVA: 0x001C7E8B File Offset: 0x001C608B
		public static AvTraceDetails SetPropertyValue
		{
			get
			{
				if (TraceMarkup._SetPropertyValue == null)
				{
					TraceMarkup._SetPropertyValue = new AvTraceDetails(13, new string[]
					{
						"Set property value",
						"Object",
						"PropertyName",
						"Value"
					});
				}
				return TraceMarkup._SetPropertyValue;
			}
		}

		// Token: 0x1700186D RID: 6253
		// (get) Token: 0x060065B1 RID: 26033 RVA: 0x001C7ECB File Offset: 0x001C60CB
		public static AvTraceDetails ThrowException
		{
			get
			{
				if (TraceMarkup._ThrowException == null)
				{
					TraceMarkup._ThrowException = new AvTraceDetails(14, new string[]
					{
						"A xaml exception has been thrown",
						"Exception"
					});
				}
				return TraceMarkup._ThrowException;
			}
		}

		// Token: 0x1700186E RID: 6254
		// (get) Token: 0x060065B2 RID: 26034 RVA: 0x001C7EFB File Offset: 0x001C60FB
		public static AvTraceDetails TypeConvert
		{
			get
			{
				if (TraceMarkup._TypeConvert == null)
				{
					TraceMarkup._TypeConvert = new AvTraceDetails(15, new string[]
					{
						"Converted value",
						"TypeConverter",
						"String",
						"Value"
					});
				}
				return TraceMarkup._TypeConvert;
			}
		}

		// Token: 0x1700186F RID: 6255
		// (get) Token: 0x060065B3 RID: 26035 RVA: 0x001C7F3B File Offset: 0x001C613B
		public static AvTraceDetails TypeConvertFallback
		{
			get
			{
				if (TraceMarkup._TypeConvertFallback == null)
				{
					TraceMarkup._TypeConvertFallback = new AvTraceDetails(16, new string[]
					{
						"Type conversion failed, using fallback",
						"TypeConverter",
						"String",
						"Value"
					});
				}
				return TraceMarkup._TypeConvertFallback;
			}
		}

		// Token: 0x060065B4 RID: 26036 RVA: 0x001C7F7B File Offset: 0x001C617B
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x060065B5 RID: 26037 RVA: 0x001C7F9B File Offset: 0x001C619B
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x060065B6 RID: 26038 RVA: 0x001C7FC0 File Offset: 0x001C61C0
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x060065B7 RID: 26039 RVA: 0x001C7FF4 File Offset: 0x001C61F4
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x060065B8 RID: 26040 RVA: 0x001C802C File Offset: 0x001C622C
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceMarkup._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x060065B9 RID: 26041 RVA: 0x001C8069 File Offset: 0x001C6269
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x060065BA RID: 26042 RVA: 0x001C8088 File Offset: 0x001C6288
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x060065BB RID: 26043 RVA: 0x001C80AC File Offset: 0x001C62AC
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x060065BC RID: 26044 RVA: 0x001C80D4 File Offset: 0x001C62D4
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x060065BD RID: 26045 RVA: 0x001C8100 File Offset: 0x001C6300
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceMarkup._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x17001870 RID: 6256
		// (get) Token: 0x060065BE RID: 26046 RVA: 0x001C8130 File Offset: 0x001C6330
		public static bool IsEnabled
		{
			get
			{
				return TraceMarkup._avTrace != null && TraceMarkup._avTrace.IsEnabled;
			}
		}

		// Token: 0x17001871 RID: 6257
		// (get) Token: 0x060065BF RID: 26047 RVA: 0x001C8145 File Offset: 0x001C6345
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceMarkup._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x060065C0 RID: 26048 RVA: 0x001C8151 File Offset: 0x001C6351
		public static void Refresh()
		{
			TraceMarkup._avTrace.Refresh();
		}

		// Token: 0x040032D8 RID: 13016
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.MarkupSource, delegate()
		{
			PresentationTraceSources._MarkupSource = null;
		});

		// Token: 0x040032D9 RID: 13017
		private static AvTraceDetails _AddValueToAddChild;

		// Token: 0x040032DA RID: 13018
		private static AvTraceDetails _AddValueToArray;

		// Token: 0x040032DB RID: 13019
		private static AvTraceDetails _AddValueToDictionary;

		// Token: 0x040032DC RID: 13020
		private static AvTraceDetails _AddValueToList;

		// Token: 0x040032DD RID: 13021
		private static AvTraceDetails _BeginInit;

		// Token: 0x040032DE RID: 13022
		private static AvTraceDetails _CreateMarkupExtension;

		// Token: 0x040032DF RID: 13023
		private static AvTraceDetails _CreateObject;

		// Token: 0x040032E0 RID: 13024
		private static AvTraceDetails _EndInit;

		// Token: 0x040032E1 RID: 13025
		private static AvTraceDetails _Load;

		// Token: 0x040032E2 RID: 13026
		private static AvTraceDetails _ProcessConstructorParameter;

		// Token: 0x040032E3 RID: 13027
		private static AvTraceDetails _ProvideValue;

		// Token: 0x040032E4 RID: 13028
		private static AvTraceDetails _SetCPA;

		// Token: 0x040032E5 RID: 13029
		private static AvTraceDetails _SetPropertyValue;

		// Token: 0x040032E6 RID: 13030
		private static AvTraceDetails _ThrowException;

		// Token: 0x040032E7 RID: 13031
		private static AvTraceDetails _TypeConvert;

		// Token: 0x040032E8 RID: 13032
		private static AvTraceDetails _TypeConvertFallback;
	}
}
