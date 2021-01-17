using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace MS.Internal
{
	// Token: 0x020005DF RID: 1503
	internal static class TraceData
	{
		// Token: 0x060063BB RID: 25531 RVA: 0x001C0AB0 File Offset: 0x001BECB0
		static TraceData()
		{
			TraceData._avTrace.TraceExtraMessages += TraceData.OnTrace;
			TraceData._avTrace.EnabledByDebugger = true;
			TraceData._avTrace.SuppressGeneratedParameters = true;
		}

		// Token: 0x060063BC RID: 25532 RVA: 0x001C0B14 File Offset: 0x001BED14
		public static bool IsExtendedTraceEnabled(object element, TraceDataLevel level)
		{
			if (TraceData.IsEnabled)
			{
				PresentationTraceLevel traceLevel = PresentationTraceSources.GetTraceLevel(element);
				return traceLevel >= (PresentationTraceLevel)level;
			}
			return false;
		}

		// Token: 0x060063BD RID: 25533 RVA: 0x001C0B38 File Offset: 0x001BED38
		public static void OnTrace(AvTraceBuilder traceBuilder, object[] parameters, int start)
		{
			for (int i = start; i < parameters.Length; i++)
			{
				object obj = parameters[i];
				string text = obj as string;
				traceBuilder.Append(" ");
				if (text != null)
				{
					traceBuilder.Append(text);
				}
				else if (obj != null)
				{
					traceBuilder.Append(obj.GetType().Name);
					traceBuilder.Append(":");
					TraceData.Describe(traceBuilder, obj);
				}
				else
				{
					traceBuilder.Append("null");
				}
			}
		}

		// Token: 0x060063BE RID: 25534 RVA: 0x001C0BA8 File Offset: 0x001BEDA8
		public static void Describe(AvTraceBuilder traceBuilder, object o)
		{
			if (o == null)
			{
				traceBuilder.Append("null");
				return;
			}
			if (o is BindingExpression)
			{
				BindingExpression bindingExpression = o as BindingExpression;
				TraceData.Describe(traceBuilder, bindingExpression.ParentBinding);
				traceBuilder.Append("; DataItem=");
				TraceData.DescribeSourceObject(traceBuilder, bindingExpression.DataItem);
				traceBuilder.Append("; ");
				TraceData.DescribeTarget(traceBuilder, bindingExpression.TargetElement, bindingExpression.TargetProperty);
				return;
			}
			if (o is Binding)
			{
				Binding binding = o as Binding;
				if (binding.Path != null)
				{
					traceBuilder.AppendFormat("Path={0}", binding.Path.Path);
					return;
				}
				if (binding.XPath != null)
				{
					traceBuilder.AppendFormat("XPath={0}", binding.XPath);
					return;
				}
				traceBuilder.Append("(no path)");
				return;
			}
			else
			{
				if (o is BindingExpressionBase)
				{
					BindingExpressionBase bindingExpressionBase = o as BindingExpressionBase;
					TraceData.DescribeTarget(traceBuilder, bindingExpressionBase.TargetElement, bindingExpressionBase.TargetProperty);
					return;
				}
				if (o is DependencyObject)
				{
					TraceData.DescribeSourceObject(traceBuilder, o);
					return;
				}
				traceBuilder.AppendFormat("'{0}'", AvTrace.ToStringHelper(o));
				return;
			}
		}

		// Token: 0x060063BF RID: 25535 RVA: 0x001C0CAC File Offset: 0x001BEEAC
		public static void DescribeSourceObject(AvTraceBuilder traceBuilder, object o)
		{
			if (o == null)
			{
				traceBuilder.Append("null");
				return;
			}
			FrameworkElement frameworkElement = o as FrameworkElement;
			if (frameworkElement != null)
			{
				traceBuilder.AppendFormat("'{0}' (Name='{1}')", frameworkElement.GetType().Name, frameworkElement.Name);
				return;
			}
			traceBuilder.AppendFormat("'{0}' (HashCode={1})", o.GetType().Name, o.GetHashCode());
		}

		// Token: 0x060063C0 RID: 25536 RVA: 0x001C0D10 File Offset: 0x001BEF10
		public static string DescribeSourceObject(object o)
		{
			AvTraceBuilder avTraceBuilder = new AvTraceBuilder(null);
			TraceData.DescribeSourceObject(avTraceBuilder, o);
			return avTraceBuilder.ToString();
		}

		// Token: 0x060063C1 RID: 25537 RVA: 0x001C0D34 File Offset: 0x001BEF34
		public static void DescribeTarget(AvTraceBuilder traceBuilder, DependencyObject targetElement, DependencyProperty targetProperty)
		{
			if (targetElement != null)
			{
				traceBuilder.Append("target element is ");
				TraceData.DescribeSourceObject(traceBuilder, targetElement);
				if (targetProperty != null)
				{
					traceBuilder.Append("; ");
				}
			}
			if (targetProperty != null)
			{
				traceBuilder.AppendFormat("target property is '{0}' (type '{1}')", targetProperty.Name, targetProperty.PropertyType.Name);
			}
		}

		// Token: 0x060063C2 RID: 25538 RVA: 0x001C0D84 File Offset: 0x001BEF84
		public static string DescribeTarget(DependencyObject targetElement, DependencyProperty targetProperty)
		{
			AvTraceBuilder avTraceBuilder = new AvTraceBuilder(null);
			TraceData.DescribeTarget(avTraceBuilder, targetElement, targetProperty);
			return avTraceBuilder.ToString();
		}

		// Token: 0x060063C3 RID: 25539 RVA: 0x001C0DA8 File Offset: 0x001BEFA8
		public static string Identify(object o)
		{
			if (o == null)
			{
				return "<null>";
			}
			Type type = o.GetType();
			if (type.IsPrimitive || type.IsEnum)
			{
				return TraceData.Format("'{0}'", new object[]
				{
					o
				});
			}
			string text = o as string;
			if (text != null)
			{
				return TraceData.Format("'{0}'", new object[]
				{
					AvTrace.AntiFormat(text)
				});
			}
			NamedObject namedObject = o as NamedObject;
			if (namedObject != null)
			{
				return AvTrace.AntiFormat(namedObject.ToString());
			}
			ICollection collection = o as ICollection;
			if (collection != null)
			{
				return TraceData.Format("{0} (hash={1} Count={2})", new object[]
				{
					type.Name,
					AvTrace.GetHashCodeHelper(o),
					collection.Count
				});
			}
			return TraceData.Format("{0} (hash={1})", new object[]
			{
				type.Name,
				AvTrace.GetHashCodeHelper(o)
			});
		}

		// Token: 0x060063C4 RID: 25540 RVA: 0x001C0E8C File Offset: 0x001BF08C
		public static string IdentifyWeakEvent(Type type)
		{
			string text = type.Name;
			if (text.EndsWith("EventManager", StringComparison.Ordinal))
			{
				text = text.Substring(0, text.Length - "EventManager".Length);
			}
			return text;
		}

		// Token: 0x060063C5 RID: 25541 RVA: 0x001C0EC8 File Offset: 0x001BF0C8
		public static string IdentifyAccessor(object accessor)
		{
			DependencyProperty dependencyProperty = accessor as DependencyProperty;
			if (dependencyProperty != null)
			{
				return TraceData.Format("{0}({1})", new object[]
				{
					dependencyProperty.GetType().Name,
					dependencyProperty.Name
				});
			}
			PropertyInfo propertyInfo = accessor as PropertyInfo;
			if (propertyInfo != null)
			{
				return TraceData.Format("{0}({1})", new object[]
				{
					propertyInfo.GetType().Name,
					propertyInfo.Name
				});
			}
			PropertyDescriptor propertyDescriptor = accessor as PropertyDescriptor;
			if (propertyDescriptor != null)
			{
				return TraceData.Format("{0}({1})", new object[]
				{
					propertyDescriptor.GetType().Name,
					propertyDescriptor.Name
				});
			}
			return TraceData.Identify(accessor);
		}

		// Token: 0x060063C6 RID: 25542 RVA: 0x001C0F77 File Offset: 0x001BF177
		public static string IdentifyException(Exception ex)
		{
			if (ex == null)
			{
				return "<no error>";
			}
			return TraceData.Format("{0} ({1})", new object[]
			{
				ex.GetType().Name,
				AvTrace.AntiFormat(ex.Message)
			});
		}

		// Token: 0x060063C7 RID: 25543 RVA: 0x001C0FAE File Offset: 0x001BF1AE
		private static string Format(string format, params object[] args)
		{
			return string.Format(TypeConverterHelper.InvariantEnglishUS, format, args);
		}

		// Token: 0x060063C8 RID: 25544 RVA: 0x001C0FBC File Offset: 0x001BF1BC
		public static AvTraceDetails CannotCreateDefaultValueConverter(params object[] args)
		{
			if (TraceData._CannotCreateDefaultValueConverter == null)
			{
				TraceData._CannotCreateDefaultValueConverter = new AvTraceDetails(1, new string[]
				{
					"Cannot create default converter to perform '{2}' conversions between types '{0}' and '{1}'. Consider using Converter property of Binding."
				});
			}
			return new AvTraceFormat(TraceData._CannotCreateDefaultValueConverter, args);
		}

		// Token: 0x170017FC RID: 6140
		// (get) Token: 0x060063C9 RID: 25545 RVA: 0x001C0FE9 File Offset: 0x001BF1E9
		public static AvTraceDetails NoMentor
		{
			get
			{
				if (TraceData._NoMentor == null)
				{
					TraceData._NoMentor = new AvTraceDetails(2, new string[]
					{
						"Cannot find governing FrameworkElement or FrameworkContentElement for target element."
					});
				}
				return TraceData._NoMentor;
			}
		}

		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x060063CA RID: 25546 RVA: 0x001C1010 File Offset: 0x001BF210
		public static AvTraceDetails NoDataContext
		{
			get
			{
				if (TraceData._NoDataContext == null)
				{
					TraceData._NoDataContext = new AvTraceDetails(3, new string[]
					{
						"Cannot find element that provides DataContext."
					});
				}
				return TraceData._NoDataContext;
			}
		}

		// Token: 0x060063CB RID: 25547 RVA: 0x001C1037 File Offset: 0x001BF237
		public static AvTraceDetails NoSource(params object[] args)
		{
			if (TraceData._NoSource == null)
			{
				TraceData._NoSource = new AvTraceDetails(4, new string[]
				{
					"Cannot find source for binding with reference '{0}'."
				});
			}
			return new AvTraceFormat(TraceData._NoSource, args);
		}

		// Token: 0x170017FE RID: 6142
		// (get) Token: 0x060063CC RID: 25548 RVA: 0x001C1064 File Offset: 0x001BF264
		public static AvTraceDetails BadValueAtTransfer
		{
			get
			{
				if (TraceData._BadValueAtTransfer == null)
				{
					TraceData._BadValueAtTransfer = new AvTraceDetails(5, new string[]
					{
						"Value produced by BindingExpression is not valid for target property.",
						"Value"
					});
				}
				return TraceData._BadValueAtTransfer;
			}
		}

		// Token: 0x060063CD RID: 25549 RVA: 0x001C1093 File Offset: 0x001BF293
		public static AvTraceDetails BadConverterForTransfer(params object[] args)
		{
			if (TraceData._BadConverterForTransfer == null)
			{
				TraceData._BadConverterForTransfer = new AvTraceDetails(6, new string[]
				{
					"'{0}' converter failed to convert value '{1}' (type '{2}'); fallback value will be used, if available."
				});
			}
			return new AvTraceFormat(TraceData._BadConverterForTransfer, args);
		}

		// Token: 0x060063CE RID: 25550 RVA: 0x001C10C0 File Offset: 0x001BF2C0
		public static AvTraceDetails BadConverterForUpdate(params object[] args)
		{
			if (TraceData._BadConverterForUpdate == null)
			{
				TraceData._BadConverterForUpdate = new AvTraceDetails(7, new string[]
				{
					"ConvertBack cannot convert value '{0}' (type '{1}')."
				});
			}
			return new AvTraceFormat(TraceData._BadConverterForUpdate, args);
		}

		// Token: 0x170017FF RID: 6143
		// (get) Token: 0x060063CF RID: 25551 RVA: 0x001C10ED File Offset: 0x001BF2ED
		public static AvTraceDetails WorkerUpdateFailed
		{
			get
			{
				if (TraceData._WorkerUpdateFailed == null)
				{
					TraceData._WorkerUpdateFailed = new AvTraceDetails(8, new string[]
					{
						"Cannot save value from target back to source."
					});
				}
				return TraceData._WorkerUpdateFailed;
			}
		}

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x060063D0 RID: 25552 RVA: 0x001C1114 File Offset: 0x001BF314
		public static AvTraceDetails RequiresExplicitCulture
		{
			get
			{
				if (TraceData._RequiresExplicitCulture == null)
				{
					TraceData._RequiresExplicitCulture = new AvTraceDetails(9, new string[]
					{
						"Binding for property cannot use the target element's Language for conversion; if a culture is required, ConverterCulture must be explicitly specified on the Binding.",
						"Property"
					});
				}
				return TraceData._RequiresExplicitCulture;
			}
		}

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x060063D1 RID: 25553 RVA: 0x001C1144 File Offset: 0x001BF344
		public static AvTraceDetails NoValueToTransfer
		{
			get
			{
				if (TraceData._NoValueToTransfer == null)
				{
					TraceData._NoValueToTransfer = new AvTraceDetails(10, new string[]
					{
						"Cannot retrieve value using the binding and no valid fallback value exists; using default instead."
					});
				}
				return TraceData._NoValueToTransfer;
			}
		}

		// Token: 0x060063D2 RID: 25554 RVA: 0x001C116C File Offset: 0x001BF36C
		public static AvTraceDetails FallbackConversionFailed(params object[] args)
		{
			if (TraceData._FallbackConversionFailed == null)
			{
				TraceData._FallbackConversionFailed = new AvTraceDetails(11, new string[]
				{
					"Fallback value '{0}' (type '{1}') cannot be converted for use in '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._FallbackConversionFailed, args);
		}

		// Token: 0x060063D3 RID: 25555 RVA: 0x001C119A File Offset: 0x001BF39A
		public static AvTraceDetails TargetNullValueConversionFailed(params object[] args)
		{
			if (TraceData._TargetNullValueConversionFailed == null)
			{
				TraceData._TargetNullValueConversionFailed = new AvTraceDetails(12, new string[]
				{
					"TargetNullValue '{0}' (type '{1}') cannot be converted for use in '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._TargetNullValueConversionFailed, args);
		}

		// Token: 0x060063D4 RID: 25556 RVA: 0x001C11C8 File Offset: 0x001BF3C8
		public static AvTraceDetails BindingGroupNameMatchFailed(params object[] args)
		{
			if (TraceData._BindingGroupNameMatchFailed == null)
			{
				TraceData._BindingGroupNameMatchFailed = new AvTraceDetails(13, new string[]
				{
					"No BindingGroup found with name matching '{0}'."
				});
			}
			return new AvTraceFormat(TraceData._BindingGroupNameMatchFailed, args);
		}

		// Token: 0x060063D5 RID: 25557 RVA: 0x001C11F6 File Offset: 0x001BF3F6
		public static AvTraceDetails BindingGroupWrongProperty(params object[] args)
		{
			if (TraceData._BindingGroupWrongProperty == null)
			{
				TraceData._BindingGroupWrongProperty = new AvTraceDetails(14, new string[]
				{
					"BindingGroup used as a value of property '{0}' on object of type '{1}'.  This may disable its normal behavior."
				});
			}
			return new AvTraceFormat(TraceData._BindingGroupWrongProperty, args);
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x060063D6 RID: 25558 RVA: 0x001C1224 File Offset: 0x001BF424
		public static AvTraceDetails BindingGroupMultipleInheritance
		{
			get
			{
				if (TraceData._BindingGroupMultipleInheritance == null)
				{
					TraceData._BindingGroupMultipleInheritance = new AvTraceDetails(15, new string[]
					{
						"BindingGroup used as a value of multiple properties.  This disables its normal behavior."
					});
				}
				return TraceData._BindingGroupMultipleInheritance;
			}
		}

		// Token: 0x060063D7 RID: 25559 RVA: 0x001C124C File Offset: 0x001BF44C
		public static AvTraceDetails SharesProposedValuesRequriesImplicitBindingGroup(params object[] args)
		{
			if (TraceData._SharesProposedValuesRequriesImplicitBindingGroup == null)
			{
				TraceData._SharesProposedValuesRequriesImplicitBindingGroup = new AvTraceDetails(16, new string[]
				{
					"Binding expression '{0}' with BindingGroupName '{1}' has joined BindingGroup '{2}' with SharesProposedValues='true'.  The SharesProposedValues feature only works for binding expressions that implicitly join a binding group."
				});
			}
			return new AvTraceFormat(TraceData._SharesProposedValuesRequriesImplicitBindingGroup, args);
		}

		// Token: 0x060063D8 RID: 25560 RVA: 0x001C127A File Offset: 0x001BF47A
		public static AvTraceDetails CannotGetClrRawValue(params object[] args)
		{
			if (TraceData._CannotGetClrRawValue == null)
			{
				TraceData._CannotGetClrRawValue = new AvTraceDetails(17, new string[]
				{
					"Cannot get '{0}' value (type '{1}') from '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._CannotGetClrRawValue, args);
		}

		// Token: 0x060063D9 RID: 25561 RVA: 0x001C12A8 File Offset: 0x001BF4A8
		public static AvTraceDetails CannotSetClrRawValue(params object[] args)
		{
			if (TraceData._CannotSetClrRawValue == null)
			{
				TraceData._CannotSetClrRawValue = new AvTraceDetails(18, new string[]
				{
					"'{3}' is not a valid value for '{0}' of '{2}'."
				});
			}
			return new AvTraceFormat(TraceData._CannotSetClrRawValue, args);
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x060063DA RID: 25562 RVA: 0x001C12D6 File Offset: 0x001BF4D6
		public static AvTraceDetails MissingDataItem
		{
			get
			{
				if (TraceData._MissingDataItem == null)
				{
					TraceData._MissingDataItem = new AvTraceDetails(19, new string[]
					{
						"BindingExpression has no source data item. This could happen when currency is moved to a null data item or moved off the list."
					});
				}
				return TraceData._MissingDataItem;
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x060063DB RID: 25563 RVA: 0x001C12FE File Offset: 0x001BF4FE
		public static AvTraceDetails MissingInfo
		{
			get
			{
				if (TraceData._MissingInfo == null)
				{
					TraceData._MissingInfo = new AvTraceDetails(20, new string[]
					{
						"BindingExpression cannot retrieve value due to missing information."
					});
				}
				return TraceData._MissingInfo;
			}
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x060063DC RID: 25564 RVA: 0x001C1326 File Offset: 0x001BF526
		public static AvTraceDetails NullDataItem
		{
			get
			{
				if (TraceData._NullDataItem == null)
				{
					TraceData._NullDataItem = new AvTraceDetails(21, new string[]
					{
						"BindingExpression cannot retrieve value from null data item. This could happen when binding is detached or when binding to a Nullable type that has no value."
					});
				}
				return TraceData._NullDataItem;
			}
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x001C134E File Offset: 0x001BF54E
		public static AvTraceDetails DefaultValueConverterFailed(params object[] args)
		{
			if (TraceData._DefaultValueConverterFailed == null)
			{
				TraceData._DefaultValueConverterFailed = new AvTraceDetails(22, new string[]
				{
					"Cannot convert '{0}' from type '{1}' to type '{2}' with default conversions; consider using Converter property of Binding."
				});
			}
			return new AvTraceFormat(TraceData._DefaultValueConverterFailed, args);
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x001C137C File Offset: 0x001BF57C
		public static AvTraceDetails DefaultValueConverterFailedForCulture(params object[] args)
		{
			if (TraceData._DefaultValueConverterFailedForCulture == null)
			{
				TraceData._DefaultValueConverterFailedForCulture = new AvTraceDetails(23, new string[]
				{
					"Cannot convert '{0}' from type '{1}' to type '{2}' for '{3}' culture with default conversions; consider using Converter property of Binding."
				});
			}
			return new AvTraceFormat(TraceData._DefaultValueConverterFailedForCulture, args);
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x001C13AA File Offset: 0x001BF5AA
		public static AvTraceDetails StyleAndStyleSelectorDefined(params object[] args)
		{
			if (TraceData._StyleAndStyleSelectorDefined == null)
			{
				TraceData._StyleAndStyleSelectorDefined = new AvTraceDetails(24, new string[]
				{
					"Both '{0}Style' and '{0}StyleSelector' are set;  '{0}StyleSelector' will be ignored."
				});
			}
			return new AvTraceFormat(TraceData._StyleAndStyleSelectorDefined, args);
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x001C13D8 File Offset: 0x001BF5D8
		public static AvTraceDetails TemplateAndTemplateSelectorDefined(params object[] args)
		{
			if (TraceData._TemplateAndTemplateSelectorDefined == null)
			{
				TraceData._TemplateAndTemplateSelectorDefined = new AvTraceDetails(25, new string[]
				{
					"Both '{0}Template' and '{0}TemplateSelector' are set;  '{0}TemplateSelector' will be ignored."
				});
			}
			return new AvTraceFormat(TraceData._TemplateAndTemplateSelectorDefined, args);
		}

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x060063E1 RID: 25569 RVA: 0x001C1406 File Offset: 0x001BF606
		public static AvTraceDetails ItemTemplateForDirectItem
		{
			get
			{
				if (TraceData._ItemTemplateForDirectItem == null)
				{
					TraceData._ItemTemplateForDirectItem = new AvTraceDetails(26, new string[]
					{
						"ItemTemplate and ItemTemplateSelector are ignored for items already of the ItemsControl's container type",
						"Type"
					});
				}
				return TraceData._ItemTemplateForDirectItem;
			}
		}

		// Token: 0x060063E2 RID: 25570 RVA: 0x001C1436 File Offset: 0x001BF636
		public static AvTraceDetails BadMultiConverterForUpdate(params object[] args)
		{
			if (TraceData._BadMultiConverterForUpdate == null)
			{
				TraceData._BadMultiConverterForUpdate = new AvTraceDetails(27, new string[]
				{
					"'{0}' MultiValueConverter failed to convert back value '{1}' (type '{2}'). Check the converter's ConvertBack method."
				});
			}
			return new AvTraceFormat(TraceData._BadMultiConverterForUpdate, args);
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x060063E3 RID: 25571 RVA: 0x001C1464 File Offset: 0x001BF664
		public static AvTraceDetails MultiValueConverterMissingForTransfer
		{
			get
			{
				if (TraceData._MultiValueConverterMissingForTransfer == null)
				{
					TraceData._MultiValueConverterMissingForTransfer = new AvTraceDetails(28, new string[]
					{
						"MultiBinding failed because it has no valid Converter."
					});
				}
				return TraceData._MultiValueConverterMissingForTransfer;
			}
		}

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x060063E4 RID: 25572 RVA: 0x001C148C File Offset: 0x001BF68C
		public static AvTraceDetails MultiValueConverterMissingForUpdate
		{
			get
			{
				if (TraceData._MultiValueConverterMissingForUpdate == null)
				{
					TraceData._MultiValueConverterMissingForUpdate = new AvTraceDetails(29, new string[]
					{
						"MultiBinding cannot update value on source item because there is no valid Converter."
					});
				}
				return TraceData._MultiValueConverterMissingForUpdate;
			}
		}

		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x060063E5 RID: 25573 RVA: 0x001C14B4 File Offset: 0x001BF6B4
		public static AvTraceDetails MultiValueConverterMismatch
		{
			get
			{
				if (TraceData._MultiValueConverterMismatch == null)
				{
					TraceData._MultiValueConverterMismatch = new AvTraceDetails(30, new string[]
					{
						"MultiValueConverter did not return the same number of values as the count of inner bindings.",
						"Converter",
						"Expected",
						"Returned"
					});
				}
				return TraceData._MultiValueConverterMismatch;
			}
		}

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x060063E6 RID: 25574 RVA: 0x001C14F4 File Offset: 0x001BF6F4
		public static AvTraceDetails MultiBindingHasNoConverter
		{
			get
			{
				if (TraceData._MultiBindingHasNoConverter == null)
				{
					TraceData._MultiBindingHasNoConverter = new AvTraceDetails(31, new string[]
					{
						"Cannot set MultiBinding because MultiValueConverter must be specified."
					});
				}
				return TraceData._MultiBindingHasNoConverter;
			}
		}

		// Token: 0x060063E7 RID: 25575 RVA: 0x001C151C File Offset: 0x001BF71C
		public static AvTraceDetails UnsetValueInMultiBindingExpressionUpdate(params object[] args)
		{
			if (TraceData._UnsetValueInMultiBindingExpressionUpdate == null)
			{
				TraceData._UnsetValueInMultiBindingExpressionUpdate = new AvTraceDetails(32, new string[]
				{
					"'{0}' MultiValueConverter returned UnsetValue after converting '{1}' for source binding '{2}' (type '{3}')."
				});
			}
			return new AvTraceFormat(TraceData._UnsetValueInMultiBindingExpressionUpdate, args);
		}

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x060063E8 RID: 25576 RVA: 0x001C154A File Offset: 0x001BF74A
		public static AvTraceDetails ObjectDataProviderHasNoSource
		{
			get
			{
				if (TraceData._ObjectDataProviderHasNoSource == null)
				{
					TraceData._ObjectDataProviderHasNoSource = new AvTraceDetails(33, new string[]
					{
						"ObjectDataProvider needs either an ObjectType or ObjectInstance."
					});
				}
				return TraceData._ObjectDataProviderHasNoSource;
			}
		}

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x060063E9 RID: 25577 RVA: 0x001C1572 File Offset: 0x001BF772
		public static AvTraceDetails ObjDPCreateFailed
		{
			get
			{
				if (TraceData._ObjDPCreateFailed == null)
				{
					TraceData._ObjDPCreateFailed = new AvTraceDetails(34, new string[]
					{
						"ObjectDataProvider cannot create object",
						"Type",
						"Error"
					});
				}
				return TraceData._ObjDPCreateFailed;
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x060063EA RID: 25578 RVA: 0x001C15AA File Offset: 0x001BF7AA
		public static AvTraceDetails ObjDPInvokeFailed
		{
			get
			{
				if (TraceData._ObjDPInvokeFailed == null)
				{
					TraceData._ObjDPInvokeFailed = new AvTraceDetails(35, new string[]
					{
						"ObjectDataProvider: Failure trying to invoke method on type",
						"Method",
						"Type",
						"Error"
					});
				}
				return TraceData._ObjDPInvokeFailed;
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x060063EB RID: 25579 RVA: 0x001C15EA File Offset: 0x001BF7EA
		public static AvTraceDetails RefPreviousNotInContext
		{
			get
			{
				if (TraceData._RefPreviousNotInContext == null)
				{
					TraceData._RefPreviousNotInContext = new AvTraceDetails(36, new string[]
					{
						"Cannot find previous element for use as RelativeSource because there is no parent in generated context."
					});
				}
				return TraceData._RefPreviousNotInContext;
			}
		}

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x060063EC RID: 25580 RVA: 0x001C1612 File Offset: 0x001BF812
		public static AvTraceDetails RefNoWrapperInChildren
		{
			get
			{
				if (TraceData._RefNoWrapperInChildren == null)
				{
					TraceData._RefNoWrapperInChildren = new AvTraceDetails(37, new string[]
					{
						"Cannot find previous element for use as RelativeSource because children cannot be found for parent element."
					});
				}
				return TraceData._RefNoWrapperInChildren;
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x060063ED RID: 25581 RVA: 0x001C163A File Offset: 0x001BF83A
		public static AvTraceDetails RefAncestorTypeNotSpecified
		{
			get
			{
				if (TraceData._RefAncestorTypeNotSpecified == null)
				{
					TraceData._RefAncestorTypeNotSpecified = new AvTraceDetails(38, new string[]
					{
						"Reference error: cannot find ancestor element; no AncestorType was specified on RelativeSource."
					});
				}
				return TraceData._RefAncestorTypeNotSpecified;
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x060063EE RID: 25582 RVA: 0x001C1662 File Offset: 0x001BF862
		public static AvTraceDetails RefAncestorLevelInvalid
		{
			get
			{
				if (TraceData._RefAncestorLevelInvalid == null)
				{
					TraceData._RefAncestorLevelInvalid = new AvTraceDetails(39, new string[]
					{
						"Reference error: cannot find ancestor element; AncestorLevel on RelativeSource must be greater than 0."
					});
				}
				return TraceData._RefAncestorLevelInvalid;
			}
		}

		// Token: 0x060063EF RID: 25583 RVA: 0x001C168A File Offset: 0x001BF88A
		public static AvTraceDetails ClrReplaceItem(params object[] args)
		{
			if (TraceData._ClrReplaceItem == null)
			{
				TraceData._ClrReplaceItem = new AvTraceDetails(40, new string[]
				{
					"BindingExpression path error: '{0}' property not found on '{2}' '{1}'."
				});
			}
			return new AvTraceFormat(TraceData._ClrReplaceItem, args);
		}

		// Token: 0x060063F0 RID: 25584 RVA: 0x001C16B8 File Offset: 0x001BF8B8
		public static AvTraceDetails NullItem(params object[] args)
		{
			if (TraceData._NullItem == null)
			{
				TraceData._NullItem = new AvTraceDetails(41, new string[]
				{
					"BindingExpression path error: '{0}' property not found for '{1}' because data item is null.  This could happen because the data provider has not produced any data yet."
				});
			}
			return new AvTraceFormat(TraceData._NullItem, args);
		}

		// Token: 0x060063F1 RID: 25585 RVA: 0x001C16E6 File Offset: 0x001BF8E6
		public static AvTraceDetails PlaceholderItem(params object[] args)
		{
			if (TraceData._PlaceholderItem == null)
			{
				TraceData._PlaceholderItem = new AvTraceDetails(42, new string[]
				{
					"BindingExpression path error: '{0}' property not found for '{1}' because data item is the NewItemPlaceholder."
				});
			}
			return new AvTraceFormat(TraceData._PlaceholderItem, args);
		}

		// Token: 0x060063F2 RID: 25586 RVA: 0x001C1714 File Offset: 0x001BF914
		public static AvTraceDetails DataErrorInfoFailed(params object[] args)
		{
			if (TraceData._DataErrorInfoFailed == null)
			{
				TraceData._DataErrorInfoFailed = new AvTraceDetails(43, new string[]
				{
					"Cannot obtain IDataErrorInfo.Error[{0}] from source of type {1} - {2} '{3}'"
				});
			}
			return new AvTraceFormat(TraceData._DataErrorInfoFailed, args);
		}

		// Token: 0x060063F3 RID: 25587 RVA: 0x001C1742 File Offset: 0x001BF942
		public static AvTraceDetails DisallowTwoWay(params object[] args)
		{
			if (TraceData._DisallowTwoWay == null)
			{
				TraceData._DisallowTwoWay = new AvTraceDetails(44, new string[]
				{
					"Binding mode has been changed to OneWay because source property '{0}.{1}' has a non-public setter."
				});
			}
			return new AvTraceFormat(TraceData._DisallowTwoWay, args);
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x060063F4 RID: 25588 RVA: 0x001C1770 File Offset: 0x001BF970
		public static AvTraceDetails XmlBindingToNonXml
		{
			get
			{
				if (TraceData._XmlBindingToNonXml == null)
				{
					TraceData._XmlBindingToNonXml = new AvTraceDetails(45, new string[]
					{
						"BindingExpression with XPath cannot bind to non-XML object.",
						"XPath"
					});
				}
				return TraceData._XmlBindingToNonXml;
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x060063F5 RID: 25589 RVA: 0x001C17A0 File Offset: 0x001BF9A0
		public static AvTraceDetails XmlBindingToNonXmlCollection
		{
			get
			{
				if (TraceData._XmlBindingToNonXmlCollection == null)
				{
					TraceData._XmlBindingToNonXmlCollection = new AvTraceDetails(46, new string[]
					{
						"BindingExpression with XPath cannot bind to a collection with non-XML objects.",
						"XPath"
					});
				}
				return TraceData._XmlBindingToNonXmlCollection;
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x060063F6 RID: 25590 RVA: 0x001C17D0 File Offset: 0x001BF9D0
		public static AvTraceDetails CannotGetXmlNodeCollection
		{
			get
			{
				if (TraceData._CannotGetXmlNodeCollection == null)
				{
					TraceData._CannotGetXmlNodeCollection = new AvTraceDetails(47, new string[]
					{
						"XML binding failed. Cannot obtain result node collection because of bad source node or bad Path.",
						"SourceNode",
						"Path"
					});
				}
				return TraceData._CannotGetXmlNodeCollection;
			}
		}

		// Token: 0x060063F7 RID: 25591 RVA: 0x001C1808 File Offset: 0x001BFA08
		public static AvTraceDetails BadXPath(params object[] args)
		{
			if (TraceData._BadXPath == null)
			{
				TraceData._BadXPath = new AvTraceDetails(48, new string[]
				{
					"XPath '{0}' returned no results on XmlNode '{1}'"
				});
			}
			return new AvTraceFormat(TraceData._BadXPath, args);
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x060063F8 RID: 25592 RVA: 0x001C1836 File Offset: 0x001BFA36
		public static AvTraceDetails XmlDPInlineDocError
		{
			get
			{
				if (TraceData._XmlDPInlineDocError == null)
				{
					TraceData._XmlDPInlineDocError = new AvTraceDetails(49, new string[]
					{
						"XmlDataProvider cannot load inline document because of load or parse error in XML."
					});
				}
				return TraceData._XmlDPInlineDocError;
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x060063F9 RID: 25593 RVA: 0x001C185E File Offset: 0x001BFA5E
		public static AvTraceDetails XmlNamespaceNotSet
		{
			get
			{
				if (TraceData._XmlNamespaceNotSet == null)
				{
					TraceData._XmlNamespaceNotSet = new AvTraceDetails(50, new string[]
					{
						"XmlDataProvider has inline XML that does not explicitly set its XmlNamespace (xmlns=\"\")."
					});
				}
				return TraceData._XmlNamespaceNotSet;
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x060063FA RID: 25594 RVA: 0x001C1886 File Offset: 0x001BFA86
		public static AvTraceDetails XmlDPAsyncDocError
		{
			get
			{
				if (TraceData._XmlDPAsyncDocError == null)
				{
					TraceData._XmlDPAsyncDocError = new AvTraceDetails(51, new string[]
					{
						"XmlDataProvider cannot load asynchronous document from Source because of load or parse error in XML stream.",
						"Source"
					});
				}
				return TraceData._XmlDPAsyncDocError;
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x060063FB RID: 25595 RVA: 0x001C18B6 File Offset: 0x001BFAB6
		public static AvTraceDetails XmlDPSelectNodesFailed
		{
			get
			{
				if (TraceData._XmlDPSelectNodesFailed == null)
				{
					TraceData._XmlDPSelectNodesFailed = new AvTraceDetails(52, new string[]
					{
						"Cannot select nodes because XPath for Binding is not valid",
						"XPath"
					});
				}
				return TraceData._XmlDPSelectNodesFailed;
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x060063FC RID: 25596 RVA: 0x001C18E6 File Offset: 0x001BFAE6
		public static AvTraceDetails CollectionViewIsUnsupported
		{
			get
			{
				if (TraceData._CollectionViewIsUnsupported == null)
				{
					TraceData._CollectionViewIsUnsupported = new AvTraceDetails(53, new string[]
					{
						"Using CollectionView directly is not fully supported.  The basic features work, although with some inefficiencies, but advanced features may encounter known bugs.  Consider using a derived class to avoid these problems."
					});
				}
				return TraceData._CollectionViewIsUnsupported;
			}
		}

		// Token: 0x060063FD RID: 25597 RVA: 0x001C190E File Offset: 0x001BFB0E
		public static AvTraceDetails CollectionChangedWithoutNotification(params object[] args)
		{
			if (TraceData._CollectionChangedWithoutNotification == null)
			{
				TraceData._CollectionChangedWithoutNotification = new AvTraceDetails(54, new string[]
				{
					"Collection of type '{0}' has been changed without raising a CollectionChanged event.  Support for this is incomplete and inconsistent, and will be removed completely in a future version of WPF.  Consider either (a) implementing INotifyCollectionChanged, or (b) avoiding changes to this type of collection."
				});
			}
			return new AvTraceFormat(TraceData._CollectionChangedWithoutNotification, args);
		}

		// Token: 0x060063FE RID: 25598 RVA: 0x001C193C File Offset: 0x001BFB3C
		public static AvTraceDetails CannotSort(params object[] args)
		{
			if (TraceData._CannotSort == null)
			{
				TraceData._CannotSort = new AvTraceDetails(55, new string[]
				{
					"Cannot sort by '{0}'"
				});
			}
			return new AvTraceFormat(TraceData._CannotSort, args);
		}

		// Token: 0x060063FF RID: 25599 RVA: 0x001C196A File Offset: 0x001BFB6A
		public static AvTraceDetails CreatedExpression(params object[] args)
		{
			if (TraceData._CreatedExpression == null)
			{
				TraceData._CreatedExpression = new AvTraceDetails(56, new string[]
				{
					"Created {0} for {1}"
				});
			}
			return new AvTraceFormat(TraceData._CreatedExpression, args);
		}

		// Token: 0x06006400 RID: 25600 RVA: 0x001C1998 File Offset: 0x001BFB98
		public static AvTraceDetails CreatedExpressionInParent(params object[] args)
		{
			if (TraceData._CreatedExpressionInParent == null)
			{
				TraceData._CreatedExpressionInParent = new AvTraceDetails(57, new string[]
				{
					"Created {0} for {1} within {2}"
				});
			}
			return new AvTraceFormat(TraceData._CreatedExpressionInParent, args);
		}

		// Token: 0x06006401 RID: 25601 RVA: 0x001C19C6 File Offset: 0x001BFBC6
		public static AvTraceDetails BindingPath(params object[] args)
		{
			if (TraceData._BindingPath == null)
			{
				TraceData._BindingPath = new AvTraceDetails(58, new string[]
				{
					"  Path: {0}"
				});
			}
			return new AvTraceFormat(TraceData._BindingPath, args);
		}

		// Token: 0x06006402 RID: 25602 RVA: 0x001C19F4 File Offset: 0x001BFBF4
		public static AvTraceDetails BindingXPathAndPath(params object[] args)
		{
			if (TraceData._BindingXPathAndPath == null)
			{
				TraceData._BindingXPathAndPath = new AvTraceDetails(59, new string[]
				{
					"  XPath: {0}  Path: {1}"
				});
			}
			return new AvTraceFormat(TraceData._BindingXPathAndPath, args);
		}

		// Token: 0x06006403 RID: 25603 RVA: 0x001C1A22 File Offset: 0x001BFC22
		public static AvTraceDetails ResolveDefaultMode(params object[] args)
		{
			if (TraceData._ResolveDefaultMode == null)
			{
				TraceData._ResolveDefaultMode = new AvTraceDetails(60, new string[]
				{
					"{0}: Default mode resolved to {1}"
				});
			}
			return new AvTraceFormat(TraceData._ResolveDefaultMode, args);
		}

		// Token: 0x06006404 RID: 25604 RVA: 0x001C1A50 File Offset: 0x001BFC50
		public static AvTraceDetails ResolveDefaultUpdate(params object[] args)
		{
			if (TraceData._ResolveDefaultUpdate == null)
			{
				TraceData._ResolveDefaultUpdate = new AvTraceDetails(61, new string[]
				{
					"{0}: Default update trigger resolved to {1}"
				});
			}
			return new AvTraceFormat(TraceData._ResolveDefaultUpdate, args);
		}

		// Token: 0x06006405 RID: 25605 RVA: 0x001C1A7E File Offset: 0x001BFC7E
		public static AvTraceDetails AttachExpression(params object[] args)
		{
			if (TraceData._AttachExpression == null)
			{
				TraceData._AttachExpression = new AvTraceDetails(62, new string[]
				{
					"{0}: Attach to {1}.{2} (hash={3})"
				});
			}
			return new AvTraceFormat(TraceData._AttachExpression, args);
		}

		// Token: 0x06006406 RID: 25606 RVA: 0x001C1AAC File Offset: 0x001BFCAC
		public static AvTraceDetails DetachExpression(params object[] args)
		{
			if (TraceData._DetachExpression == null)
			{
				TraceData._DetachExpression = new AvTraceDetails(63, new string[]
				{
					"{0}: Detach"
				});
			}
			return new AvTraceFormat(TraceData._DetachExpression, args);
		}

		// Token: 0x06006407 RID: 25607 RVA: 0x001C1ADA File Offset: 0x001BFCDA
		public static AvTraceDetails UseMentor(params object[] args)
		{
			if (TraceData._UseMentor == null)
			{
				TraceData._UseMentor = new AvTraceDetails(64, new string[]
				{
					"{0}: Use Framework mentor {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseMentor, args);
		}

		// Token: 0x06006408 RID: 25608 RVA: 0x001C1B08 File Offset: 0x001BFD08
		public static AvTraceDetails DeferAttachToContext(params object[] args)
		{
			if (TraceData._DeferAttachToContext == null)
			{
				TraceData._DeferAttachToContext = new AvTraceDetails(65, new string[]
				{
					"{0}: Resolve source deferred"
				});
			}
			return new AvTraceFormat(TraceData._DeferAttachToContext, args);
		}

		// Token: 0x06006409 RID: 25609 RVA: 0x001C1B36 File Offset: 0x001BFD36
		public static AvTraceDetails SourceRequiresTreeContext(params object[] args)
		{
			if (TraceData._SourceRequiresTreeContext == null)
			{
				TraceData._SourceRequiresTreeContext = new AvTraceDetails(66, new string[]
				{
					"{0}: {1} requires tree context"
				});
			}
			return new AvTraceFormat(TraceData._SourceRequiresTreeContext, args);
		}

		// Token: 0x0600640A RID: 25610 RVA: 0x001C1B64 File Offset: 0x001BFD64
		public static AvTraceDetails AttachToContext(params object[] args)
		{
			if (TraceData._AttachToContext == null)
			{
				TraceData._AttachToContext = new AvTraceDetails(67, new string[]
				{
					"{0}: Resolving source {1}"
				});
			}
			return new AvTraceFormat(TraceData._AttachToContext, args);
		}

		// Token: 0x0600640B RID: 25611 RVA: 0x001C1B92 File Offset: 0x001BFD92
		public static AvTraceDetails PathRequiresTreeContext(params object[] args)
		{
			if (TraceData._PathRequiresTreeContext == null)
			{
				TraceData._PathRequiresTreeContext = new AvTraceDetails(68, new string[]
				{
					"{0}: Path '{1}' requires namespace information"
				});
			}
			return new AvTraceFormat(TraceData._PathRequiresTreeContext, args);
		}

		// Token: 0x0600640C RID: 25612 RVA: 0x001C1BC0 File Offset: 0x001BFDC0
		public static AvTraceDetails NoMentorExtended(params object[] args)
		{
			if (TraceData._NoMentorExtended == null)
			{
				TraceData._NoMentorExtended = new AvTraceDetails(69, new string[]
				{
					"{0}: Framework mentor not found"
				});
			}
			return new AvTraceFormat(TraceData._NoMentorExtended, args);
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x001C1BEE File Offset: 0x001BFDEE
		public static AvTraceDetails ContextElement(params object[] args)
		{
			if (TraceData._ContextElement == null)
			{
				TraceData._ContextElement = new AvTraceDetails(70, new string[]
				{
					"{0}: Found data context element: {1} ({2})"
				});
			}
			return new AvTraceFormat(TraceData._ContextElement, args);
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x001C1C1C File Offset: 0x001BFE1C
		public static AvTraceDetails NullDataContext(params object[] args)
		{
			if (TraceData._NullDataContext == null)
			{
				TraceData._NullDataContext = new AvTraceDetails(71, new string[]
				{
					"{0}: DataContext is null"
				});
			}
			return new AvTraceFormat(TraceData._NullDataContext, args);
		}

		// Token: 0x0600640F RID: 25615 RVA: 0x001C1C4A File Offset: 0x001BFE4A
		public static AvTraceDetails RelativeSource(params object[] args)
		{
			if (TraceData._RelativeSource == null)
			{
				TraceData._RelativeSource = new AvTraceDetails(72, new string[]
				{
					"  RelativeSource.{0} found {1}"
				});
			}
			return new AvTraceFormat(TraceData._RelativeSource, args);
		}

		// Token: 0x06006410 RID: 25616 RVA: 0x001C1C78 File Offset: 0x001BFE78
		public static AvTraceDetails AncestorLookup(params object[] args)
		{
			if (TraceData._AncestorLookup == null)
			{
				TraceData._AncestorLookup = new AvTraceDetails(73, new string[]
				{
					"    Lookup ancestor of type {0}:  queried {1}"
				});
			}
			return new AvTraceFormat(TraceData._AncestorLookup, args);
		}

		// Token: 0x06006411 RID: 25617 RVA: 0x001C1CA6 File Offset: 0x001BFEA6
		public static AvTraceDetails ElementNameQuery(params object[] args)
		{
			if (TraceData._ElementNameQuery == null)
			{
				TraceData._ElementNameQuery = new AvTraceDetails(74, new string[]
				{
					"    Lookup name {0}:  queried {1}"
				});
			}
			return new AvTraceFormat(TraceData._ElementNameQuery, args);
		}

		// Token: 0x06006412 RID: 25618 RVA: 0x001C1CD4 File Offset: 0x001BFED4
		public static AvTraceDetails ElementNameQueryTemplate(params object[] args)
		{
			if (TraceData._ElementNameQueryTemplate == null)
			{
				TraceData._ElementNameQueryTemplate = new AvTraceDetails(75, new string[]
				{
					"    Lookup name {0}:  queried template of {1}"
				});
			}
			return new AvTraceFormat(TraceData._ElementNameQueryTemplate, args);
		}

		// Token: 0x06006413 RID: 25619 RVA: 0x001C1D02 File Offset: 0x001BFF02
		public static AvTraceDetails UseCVS(params object[] args)
		{
			if (TraceData._UseCVS == null)
			{
				TraceData._UseCVS = new AvTraceDetails(76, new string[]
				{
					"{0}: Use View from {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseCVS, args);
		}

		// Token: 0x06006414 RID: 25620 RVA: 0x001C1D30 File Offset: 0x001BFF30
		public static AvTraceDetails UseDataProvider(params object[] args)
		{
			if (TraceData._UseDataProvider == null)
			{
				TraceData._UseDataProvider = new AvTraceDetails(77, new string[]
				{
					"{0}: Use Data from {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseDataProvider, args);
		}

		// Token: 0x06006415 RID: 25621 RVA: 0x001C1D5E File Offset: 0x001BFF5E
		public static AvTraceDetails ActivateItem(params object[] args)
		{
			if (TraceData._ActivateItem == null)
			{
				TraceData._ActivateItem = new AvTraceDetails(78, new string[]
				{
					"{0}: Activate with root item {1}"
				});
			}
			return new AvTraceFormat(TraceData._ActivateItem, args);
		}

		// Token: 0x06006416 RID: 25622 RVA: 0x001C1D8C File Offset: 0x001BFF8C
		public static AvTraceDetails Deactivate(params object[] args)
		{
			if (TraceData._Deactivate == null)
			{
				TraceData._Deactivate = new AvTraceDetails(79, new string[]
				{
					"{0}: Deactivate"
				});
			}
			return new AvTraceFormat(TraceData._Deactivate, args);
		}

		// Token: 0x06006417 RID: 25623 RVA: 0x001C1DBA File Offset: 0x001BFFBA
		public static AvTraceDetails GetRawValue(params object[] args)
		{
			if (TraceData._GetRawValue == null)
			{
				TraceData._GetRawValue = new AvTraceDetails(80, new string[]
				{
					"{0}: TransferValue - got raw value {1}"
				});
			}
			return new AvTraceFormat(TraceData._GetRawValue, args);
		}

		// Token: 0x06006418 RID: 25624 RVA: 0x001C1DE8 File Offset: 0x001BFFE8
		public static AvTraceDetails ConvertDBNull(params object[] args)
		{
			if (TraceData._ConvertDBNull == null)
			{
				TraceData._ConvertDBNull = new AvTraceDetails(81, new string[]
				{
					"{0}: TransferValue - converted DBNull to {1}"
				});
			}
			return new AvTraceFormat(TraceData._ConvertDBNull, args);
		}

		// Token: 0x06006419 RID: 25625 RVA: 0x001C1E16 File Offset: 0x001C0016
		public static AvTraceDetails UserConverter(params object[] args)
		{
			if (TraceData._UserConverter == null)
			{
				TraceData._UserConverter = new AvTraceDetails(82, new string[]
				{
					"{0}: TransferValue - user's converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._UserConverter, args);
		}

		// Token: 0x0600641A RID: 25626 RVA: 0x001C1E44 File Offset: 0x001C0044
		public static AvTraceDetails NullConverter(params object[] args)
		{
			if (TraceData._NullConverter == null)
			{
				TraceData._NullConverter = new AvTraceDetails(83, new string[]
				{
					"{0}: TransferValue - null-value conversion produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._NullConverter, args);
		}

		// Token: 0x0600641B RID: 25627 RVA: 0x001C1E72 File Offset: 0x001C0072
		public static AvTraceDetails DefaultConverter(params object[] args)
		{
			if (TraceData._DefaultConverter == null)
			{
				TraceData._DefaultConverter = new AvTraceDetails(84, new string[]
				{
					"{0}: TransferValue - implicit converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._DefaultConverter, args);
		}

		// Token: 0x0600641C RID: 25628 RVA: 0x001C1EA0 File Offset: 0x001C00A0
		public static AvTraceDetails FormattedValue(params object[] args)
		{
			if (TraceData._FormattedValue == null)
			{
				TraceData._FormattedValue = new AvTraceDetails(85, new string[]
				{
					"{0}: TransferValue - string formatting produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._FormattedValue, args);
		}

		// Token: 0x0600641D RID: 25629 RVA: 0x001C1ECE File Offset: 0x001C00CE
		public static AvTraceDetails FormattingFailed(params object[] args)
		{
			if (TraceData._FormattingFailed == null)
			{
				TraceData._FormattingFailed = new AvTraceDetails(86, new string[]
				{
					"{0}: TransferValue - string formatting failed, using format '{1}'"
				});
			}
			return new AvTraceFormat(TraceData._FormattingFailed, args);
		}

		// Token: 0x0600641E RID: 25630 RVA: 0x001C1EFC File Offset: 0x001C00FC
		public static AvTraceDetails BadValueAtTransferExtended(params object[] args)
		{
			if (TraceData._BadValueAtTransferExtended == null)
			{
				TraceData._BadValueAtTransferExtended = new AvTraceDetails(87, new string[]
				{
					"{0}: TransferValue - value {1} is not valid for target"
				});
			}
			return new AvTraceFormat(TraceData._BadValueAtTransferExtended, args);
		}

		// Token: 0x0600641F RID: 25631 RVA: 0x001C1F2A File Offset: 0x001C012A
		public static AvTraceDetails UseFallback(params object[] args)
		{
			if (TraceData._UseFallback == null)
			{
				TraceData._UseFallback = new AvTraceDetails(88, new string[]
				{
					"{0}: TransferValue - using fallback/default value {1}"
				});
			}
			return new AvTraceFormat(TraceData._UseFallback, args);
		}

		// Token: 0x06006420 RID: 25632 RVA: 0x001C1F58 File Offset: 0x001C0158
		public static AvTraceDetails TransferValue(params object[] args)
		{
			if (TraceData._TransferValue == null)
			{
				TraceData._TransferValue = new AvTraceDetails(89, new string[]
				{
					"{0}: TransferValue - using final value {1}"
				});
			}
			return new AvTraceFormat(TraceData._TransferValue, args);
		}

		// Token: 0x06006421 RID: 25633 RVA: 0x001C1F86 File Offset: 0x001C0186
		public static AvTraceDetails UpdateRawValue(params object[] args)
		{
			if (TraceData._UpdateRawValue == null)
			{
				TraceData._UpdateRawValue = new AvTraceDetails(90, new string[]
				{
					"{0}: Update - got raw value {1}"
				});
			}
			return new AvTraceFormat(TraceData._UpdateRawValue, args);
		}

		// Token: 0x06006422 RID: 25634 RVA: 0x001C1FB4 File Offset: 0x001C01B4
		public static AvTraceDetails ValidationRuleFailed(params object[] args)
		{
			if (TraceData._ValidationRuleFailed == null)
			{
				TraceData._ValidationRuleFailed = new AvTraceDetails(91, new string[]
				{
					"{0}: Update - {1} failed"
				});
			}
			return new AvTraceFormat(TraceData._ValidationRuleFailed, args);
		}

		// Token: 0x06006423 RID: 25635 RVA: 0x001C1FE2 File Offset: 0x001C01E2
		public static AvTraceDetails UserConvertBack(params object[] args)
		{
			if (TraceData._UserConvertBack == null)
			{
				TraceData._UserConvertBack = new AvTraceDetails(92, new string[]
				{
					"{0}: Update - user's converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._UserConvertBack, args);
		}

		// Token: 0x06006424 RID: 25636 RVA: 0x001C2010 File Offset: 0x001C0210
		public static AvTraceDetails DefaultConvertBack(params object[] args)
		{
			if (TraceData._DefaultConvertBack == null)
			{
				TraceData._DefaultConvertBack = new AvTraceDetails(93, new string[]
				{
					"{0}: Update - implicit converter produced {1}"
				});
			}
			return new AvTraceFormat(TraceData._DefaultConvertBack, args);
		}

		// Token: 0x06006425 RID: 25637 RVA: 0x001C203E File Offset: 0x001C023E
		public static AvTraceDetails Update(params object[] args)
		{
			if (TraceData._Update == null)
			{
				TraceData._Update = new AvTraceDetails(94, new string[]
				{
					"{0}: Update - using final value {1}"
				});
			}
			return new AvTraceFormat(TraceData._Update, args);
		}

		// Token: 0x06006426 RID: 25638 RVA: 0x001C206C File Offset: 0x001C026C
		public static AvTraceDetails GotEvent(params object[] args)
		{
			if (TraceData._GotEvent == null)
			{
				TraceData._GotEvent = new AvTraceDetails(95, new string[]
				{
					"{0}: Got {1} event from {2}"
				});
			}
			return new AvTraceFormat(TraceData._GotEvent, args);
		}

		// Token: 0x06006427 RID: 25639 RVA: 0x001C209A File Offset: 0x001C029A
		public static AvTraceDetails GotPropertyChanged(params object[] args)
		{
			if (TraceData._GotPropertyChanged == null)
			{
				TraceData._GotPropertyChanged = new AvTraceDetails(96, new string[]
				{
					"{0}: Got PropertyChanged event from {1} for {2}"
				});
			}
			return new AvTraceFormat(TraceData._GotPropertyChanged, args);
		}

		// Token: 0x06006428 RID: 25640 RVA: 0x001C20C8 File Offset: 0x001C02C8
		public static AvTraceDetails PriorityTransfer(params object[] args)
		{
			if (TraceData._PriorityTransfer == null)
			{
				TraceData._PriorityTransfer = new AvTraceDetails(97, new string[]
				{
					"{0}: TransferValue '{1}' from child {2} - {3}"
				});
			}
			return new AvTraceFormat(TraceData._PriorityTransfer, args);
		}

		// Token: 0x06006429 RID: 25641 RVA: 0x001C20F6 File Offset: 0x001C02F6
		public static AvTraceDetails ChildNotAttached(params object[] args)
		{
			if (TraceData._ChildNotAttached == null)
			{
				TraceData._ChildNotAttached = new AvTraceDetails(98, new string[]
				{
					"{0}: One or more children have not resolved sources"
				});
			}
			return new AvTraceFormat(TraceData._ChildNotAttached, args);
		}

		// Token: 0x0600642A RID: 25642 RVA: 0x001C2124 File Offset: 0x001C0324
		public static AvTraceDetails GetRawValueMulti(params object[] args)
		{
			if (TraceData._GetRawValueMulti == null)
			{
				TraceData._GetRawValueMulti = new AvTraceDetails(99, new string[]
				{
					"{0}: TransferValue - got raw value {1}: {2}"
				});
			}
			return new AvTraceFormat(TraceData._GetRawValueMulti, args);
		}

		// Token: 0x0600642B RID: 25643 RVA: 0x001C2152 File Offset: 0x001C0352
		public static AvTraceDetails UserConvertBackMulti(params object[] args)
		{
			if (TraceData._UserConvertBackMulti == null)
			{
				TraceData._UserConvertBackMulti = new AvTraceDetails(100, new string[]
				{
					"{0}: Update - multiconverter produced value {1}: {2}"
				});
			}
			return new AvTraceFormat(TraceData._UserConvertBackMulti, args);
		}

		// Token: 0x0600642C RID: 25644 RVA: 0x001C2180 File Offset: 0x001C0380
		public static AvTraceDetails GetValue(params object[] args)
		{
			if (TraceData._GetValue == null)
			{
				TraceData._GetValue = new AvTraceDetails(101, new string[]
				{
					"{0}: GetValue at level {1} from {2} using {3}: {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetValue, args);
		}

		// Token: 0x0600642D RID: 25645 RVA: 0x001C21AE File Offset: 0x001C03AE
		public static AvTraceDetails SetValue(params object[] args)
		{
			if (TraceData._SetValue == null)
			{
				TraceData._SetValue = new AvTraceDetails(102, new string[]
				{
					"{0}: SetValue at level {1} to {2} using {3}: {4}"
				});
			}
			return new AvTraceFormat(TraceData._SetValue, args);
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x001C21DC File Offset: 0x001C03DC
		public static AvTraceDetails ReplaceItemShort(params object[] args)
		{
			if (TraceData._ReplaceItemShort == null)
			{
				TraceData._ReplaceItemShort = new AvTraceDetails(103, new string[]
				{
					"{0}: Replace item at level {1} with {2}"
				});
			}
			return new AvTraceFormat(TraceData._ReplaceItemShort, args);
		}

		// Token: 0x0600642F RID: 25647 RVA: 0x001C220A File Offset: 0x001C040A
		public static AvTraceDetails ReplaceItemLong(params object[] args)
		{
			if (TraceData._ReplaceItemLong == null)
			{
				TraceData._ReplaceItemLong = new AvTraceDetails(104, new string[]
				{
					"{0}: Replace item at level {1} with {2}, using accessor {3}"
				});
			}
			return new AvTraceFormat(TraceData._ReplaceItemLong, args);
		}

		// Token: 0x06006430 RID: 25648 RVA: 0x001C2238 File Offset: 0x001C0438
		public static AvTraceDetails GetInfo_Reuse(params object[] args)
		{
			if (TraceData._GetInfo_Reuse == null)
			{
				TraceData._GetInfo_Reuse = new AvTraceDetails(105, new string[]
				{
					"{0}:   Item at level {1} has same type - reuse accessor {2}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Reuse, args);
		}

		// Token: 0x06006431 RID: 25649 RVA: 0x001C2266 File Offset: 0x001C0466
		public static AvTraceDetails GetInfo_Null(params object[] args)
		{
			if (TraceData._GetInfo_Null == null)
			{
				TraceData._GetInfo_Null = new AvTraceDetails(106, new string[]
				{
					"{0}:   Item at level {1} is null - no accessor"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Null, args);
		}

		// Token: 0x06006432 RID: 25650 RVA: 0x001C2294 File Offset: 0x001C0494
		public static AvTraceDetails GetInfo_Cache(params object[] args)
		{
			if (TraceData._GetInfo_Cache == null)
			{
				TraceData._GetInfo_Cache = new AvTraceDetails(107, new string[]
				{
					"{0}:   At level {1} using cached accessor for {2}.{3}: {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Cache, args);
		}

		// Token: 0x06006433 RID: 25651 RVA: 0x001C22C2 File Offset: 0x001C04C2
		public static AvTraceDetails GetInfo_Property(params object[] args)
		{
			if (TraceData._GetInfo_Property == null)
			{
				TraceData._GetInfo_Property = new AvTraceDetails(108, new string[]
				{
					"{0}:   At level {1} - for {2}.{3} found accessor {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Property, args);
		}

		// Token: 0x06006434 RID: 25652 RVA: 0x001C22F0 File Offset: 0x001C04F0
		public static AvTraceDetails GetInfo_Indexer(params object[] args)
		{
			if (TraceData._GetInfo_Indexer == null)
			{
				TraceData._GetInfo_Indexer = new AvTraceDetails(109, new string[]
				{
					"{0}:   At level {1} - for {2}[{3}] found accessor {4}"
				});
			}
			return new AvTraceFormat(TraceData._GetInfo_Indexer, args);
		}

		// Token: 0x06006435 RID: 25653 RVA: 0x001C231E File Offset: 0x001C051E
		public static AvTraceDetails XmlContextNode(params object[] args)
		{
			if (TraceData._XmlContextNode == null)
			{
				TraceData._XmlContextNode = new AvTraceDetails(110, new string[]
				{
					"{0}: Context for XML binding set to {1}"
				});
			}
			return new AvTraceFormat(TraceData._XmlContextNode, args);
		}

		// Token: 0x06006436 RID: 25654 RVA: 0x001C234C File Offset: 0x001C054C
		public static AvTraceDetails XmlNewCollection(params object[] args)
		{
			if (TraceData._XmlNewCollection == null)
			{
				TraceData._XmlNewCollection = new AvTraceDetails(111, new string[]
				{
					"{0}: Building collection from {1}"
				});
			}
			return new AvTraceFormat(TraceData._XmlNewCollection, args);
		}

		// Token: 0x06006437 RID: 25655 RVA: 0x001C237A File Offset: 0x001C057A
		public static AvTraceDetails XmlSynchronizeCollection(params object[] args)
		{
			if (TraceData._XmlSynchronizeCollection == null)
			{
				TraceData._XmlSynchronizeCollection = new AvTraceDetails(112, new string[]
				{
					"{0}: Synchronizing collection with {1}"
				});
			}
			return new AvTraceFormat(TraceData._XmlSynchronizeCollection, args);
		}

		// Token: 0x06006438 RID: 25656 RVA: 0x001C23A8 File Offset: 0x001C05A8
		public static AvTraceDetails SelectNodes(params object[] args)
		{
			if (TraceData._SelectNodes == null)
			{
				TraceData._SelectNodes = new AvTraceDetails(113, new string[]
				{
					"{0}: SelectNodes at {1} using XPath {2}: {3}"
				});
			}
			return new AvTraceFormat(TraceData._SelectNodes, args);
		}

		// Token: 0x06006439 RID: 25657 RVA: 0x001C23D6 File Offset: 0x001C05D6
		public static AvTraceDetails BeginQuery(params object[] args)
		{
			if (TraceData._BeginQuery == null)
			{
				TraceData._BeginQuery = new AvTraceDetails(114, new string[]
				{
					"{0}: Begin query ({1})"
				});
			}
			return new AvTraceFormat(TraceData._BeginQuery, args);
		}

		// Token: 0x0600643A RID: 25658 RVA: 0x001C2404 File Offset: 0x001C0604
		public static AvTraceDetails QueryFinished(params object[] args)
		{
			if (TraceData._QueryFinished == null)
			{
				TraceData._QueryFinished = new AvTraceDetails(115, new string[]
				{
					"{0}: Query finished ({1}) with data {2} and error {3}"
				});
			}
			return new AvTraceFormat(TraceData._QueryFinished, args);
		}

		// Token: 0x0600643B RID: 25659 RVA: 0x001C2432 File Offset: 0x001C0632
		public static AvTraceDetails QueryResult(params object[] args)
		{
			if (TraceData._QueryResult == null)
			{
				TraceData._QueryResult = new AvTraceDetails(116, new string[]
				{
					"{0}: Update result (on UI thread) with data {1}"
				});
			}
			return new AvTraceFormat(TraceData._QueryResult, args);
		}

		// Token: 0x0600643C RID: 25660 RVA: 0x001C2460 File Offset: 0x001C0660
		public static AvTraceDetails XmlLoadSource(params object[] args)
		{
			if (TraceData._XmlLoadSource == null)
			{
				TraceData._XmlLoadSource = new AvTraceDetails(117, new string[]
				{
					"{0}: Request download ({1}) from {2}"
				});
			}
			return new AvTraceFormat(TraceData._XmlLoadSource, args);
		}

		// Token: 0x0600643D RID: 25661 RVA: 0x001C248E File Offset: 0x001C068E
		public static AvTraceDetails XmlLoadDoc(params object[] args)
		{
			if (TraceData._XmlLoadDoc == null)
			{
				TraceData._XmlLoadDoc = new AvTraceDetails(118, new string[]
				{
					"{0}: Load document from stream"
				});
			}
			return new AvTraceFormat(TraceData._XmlLoadDoc, args);
		}

		// Token: 0x0600643E RID: 25662 RVA: 0x001C24BC File Offset: 0x001C06BC
		public static AvTraceDetails XmlLoadInline(params object[] args)
		{
			if (TraceData._XmlLoadInline == null)
			{
				TraceData._XmlLoadInline = new AvTraceDetails(119, new string[]
				{
					"{0}: Load inline document"
				});
			}
			return new AvTraceFormat(TraceData._XmlLoadInline, args);
		}

		// Token: 0x0600643F RID: 25663 RVA: 0x001C24EA File Offset: 0x001C06EA
		public static AvTraceDetails XmlBuildCollection(params object[] args)
		{
			if (TraceData._XmlBuildCollection == null)
			{
				TraceData._XmlBuildCollection = new AvTraceDetails(120, new string[]
				{
					"{0}: Build XmlNode collection"
				});
			}
			return new AvTraceFormat(TraceData._XmlBuildCollection, args);
		}

		// Token: 0x06006440 RID: 25664 RVA: 0x001C2518 File Offset: 0x001C0718
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006441 RID: 25665 RVA: 0x001C2538 File Offset: 0x001C0738
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006442 RID: 25666 RVA: 0x001C2560 File Offset: 0x001C0760
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x001C2594 File Offset: 0x001C0794
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x001C25CC File Offset: 0x001C07CC
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceData._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06006445 RID: 25669 RVA: 0x001C2609 File Offset: 0x001C0809
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006446 RID: 25670 RVA: 0x001C2628 File Offset: 0x001C0828
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x001C264C File Offset: 0x001C084C
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x001C2674 File Offset: 0x001C0874
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006449 RID: 25673 RVA: 0x001C26A0 File Offset: 0x001C08A0
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceData._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x0600644A RID: 25674 RVA: 0x001C26D0 File Offset: 0x001C08D0
		public static bool IsEnabled
		{
			get
			{
				return TraceData._avTrace != null && TraceData._avTrace.IsEnabled;
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x0600644B RID: 25675 RVA: 0x001C26E5 File Offset: 0x001C08E5
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceData._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x001C26F1 File Offset: 0x001C08F1
		public static void Refresh()
		{
			TraceData._avTrace.Refresh();
		}

		// Token: 0x04003221 RID: 12833
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.DataBindingSource, delegate()
		{
			PresentationTraceSources._DataBindingSource = null;
		});

		// Token: 0x04003222 RID: 12834
		private static AvTraceDetails _CannotCreateDefaultValueConverter;

		// Token: 0x04003223 RID: 12835
		private static AvTraceDetails _NoMentor;

		// Token: 0x04003224 RID: 12836
		private static AvTraceDetails _NoDataContext;

		// Token: 0x04003225 RID: 12837
		private static AvTraceDetails _NoSource;

		// Token: 0x04003226 RID: 12838
		private static AvTraceDetails _BadValueAtTransfer;

		// Token: 0x04003227 RID: 12839
		private static AvTraceDetails _BadConverterForTransfer;

		// Token: 0x04003228 RID: 12840
		private static AvTraceDetails _BadConverterForUpdate;

		// Token: 0x04003229 RID: 12841
		private static AvTraceDetails _WorkerUpdateFailed;

		// Token: 0x0400322A RID: 12842
		private static AvTraceDetails _RequiresExplicitCulture;

		// Token: 0x0400322B RID: 12843
		private static AvTraceDetails _NoValueToTransfer;

		// Token: 0x0400322C RID: 12844
		private static AvTraceDetails _FallbackConversionFailed;

		// Token: 0x0400322D RID: 12845
		private static AvTraceDetails _TargetNullValueConversionFailed;

		// Token: 0x0400322E RID: 12846
		private static AvTraceDetails _BindingGroupNameMatchFailed;

		// Token: 0x0400322F RID: 12847
		private static AvTraceDetails _BindingGroupWrongProperty;

		// Token: 0x04003230 RID: 12848
		private static AvTraceDetails _BindingGroupMultipleInheritance;

		// Token: 0x04003231 RID: 12849
		private static AvTraceDetails _SharesProposedValuesRequriesImplicitBindingGroup;

		// Token: 0x04003232 RID: 12850
		private static AvTraceDetails _CannotGetClrRawValue;

		// Token: 0x04003233 RID: 12851
		private static AvTraceDetails _CannotSetClrRawValue;

		// Token: 0x04003234 RID: 12852
		private static AvTraceDetails _MissingDataItem;

		// Token: 0x04003235 RID: 12853
		private static AvTraceDetails _MissingInfo;

		// Token: 0x04003236 RID: 12854
		private static AvTraceDetails _NullDataItem;

		// Token: 0x04003237 RID: 12855
		private static AvTraceDetails _DefaultValueConverterFailed;

		// Token: 0x04003238 RID: 12856
		private static AvTraceDetails _DefaultValueConverterFailedForCulture;

		// Token: 0x04003239 RID: 12857
		private static AvTraceDetails _StyleAndStyleSelectorDefined;

		// Token: 0x0400323A RID: 12858
		private static AvTraceDetails _TemplateAndTemplateSelectorDefined;

		// Token: 0x0400323B RID: 12859
		private static AvTraceDetails _ItemTemplateForDirectItem;

		// Token: 0x0400323C RID: 12860
		private static AvTraceDetails _BadMultiConverterForUpdate;

		// Token: 0x0400323D RID: 12861
		private static AvTraceDetails _MultiValueConverterMissingForTransfer;

		// Token: 0x0400323E RID: 12862
		private static AvTraceDetails _MultiValueConverterMissingForUpdate;

		// Token: 0x0400323F RID: 12863
		private static AvTraceDetails _MultiValueConverterMismatch;

		// Token: 0x04003240 RID: 12864
		private static AvTraceDetails _MultiBindingHasNoConverter;

		// Token: 0x04003241 RID: 12865
		private static AvTraceDetails _UnsetValueInMultiBindingExpressionUpdate;

		// Token: 0x04003242 RID: 12866
		private static AvTraceDetails _ObjectDataProviderHasNoSource;

		// Token: 0x04003243 RID: 12867
		private static AvTraceDetails _ObjDPCreateFailed;

		// Token: 0x04003244 RID: 12868
		private static AvTraceDetails _ObjDPInvokeFailed;

		// Token: 0x04003245 RID: 12869
		private static AvTraceDetails _RefPreviousNotInContext;

		// Token: 0x04003246 RID: 12870
		private static AvTraceDetails _RefNoWrapperInChildren;

		// Token: 0x04003247 RID: 12871
		private static AvTraceDetails _RefAncestorTypeNotSpecified;

		// Token: 0x04003248 RID: 12872
		private static AvTraceDetails _RefAncestorLevelInvalid;

		// Token: 0x04003249 RID: 12873
		private static AvTraceDetails _ClrReplaceItem;

		// Token: 0x0400324A RID: 12874
		private static AvTraceDetails _NullItem;

		// Token: 0x0400324B RID: 12875
		private static AvTraceDetails _PlaceholderItem;

		// Token: 0x0400324C RID: 12876
		private static AvTraceDetails _DataErrorInfoFailed;

		// Token: 0x0400324D RID: 12877
		private static AvTraceDetails _DisallowTwoWay;

		// Token: 0x0400324E RID: 12878
		private static AvTraceDetails _XmlBindingToNonXml;

		// Token: 0x0400324F RID: 12879
		private static AvTraceDetails _XmlBindingToNonXmlCollection;

		// Token: 0x04003250 RID: 12880
		private static AvTraceDetails _CannotGetXmlNodeCollection;

		// Token: 0x04003251 RID: 12881
		private static AvTraceDetails _BadXPath;

		// Token: 0x04003252 RID: 12882
		private static AvTraceDetails _XmlDPInlineDocError;

		// Token: 0x04003253 RID: 12883
		private static AvTraceDetails _XmlNamespaceNotSet;

		// Token: 0x04003254 RID: 12884
		private static AvTraceDetails _XmlDPAsyncDocError;

		// Token: 0x04003255 RID: 12885
		private static AvTraceDetails _XmlDPSelectNodesFailed;

		// Token: 0x04003256 RID: 12886
		private static AvTraceDetails _CollectionViewIsUnsupported;

		// Token: 0x04003257 RID: 12887
		private static AvTraceDetails _CollectionChangedWithoutNotification;

		// Token: 0x04003258 RID: 12888
		private static AvTraceDetails _CannotSort;

		// Token: 0x04003259 RID: 12889
		private static AvTraceDetails _CreatedExpression;

		// Token: 0x0400325A RID: 12890
		private static AvTraceDetails _CreatedExpressionInParent;

		// Token: 0x0400325B RID: 12891
		private static AvTraceDetails _BindingPath;

		// Token: 0x0400325C RID: 12892
		private static AvTraceDetails _BindingXPathAndPath;

		// Token: 0x0400325D RID: 12893
		private static AvTraceDetails _ResolveDefaultMode;

		// Token: 0x0400325E RID: 12894
		private static AvTraceDetails _ResolveDefaultUpdate;

		// Token: 0x0400325F RID: 12895
		private static AvTraceDetails _AttachExpression;

		// Token: 0x04003260 RID: 12896
		private static AvTraceDetails _DetachExpression;

		// Token: 0x04003261 RID: 12897
		private static AvTraceDetails _UseMentor;

		// Token: 0x04003262 RID: 12898
		private static AvTraceDetails _DeferAttachToContext;

		// Token: 0x04003263 RID: 12899
		private static AvTraceDetails _SourceRequiresTreeContext;

		// Token: 0x04003264 RID: 12900
		private static AvTraceDetails _AttachToContext;

		// Token: 0x04003265 RID: 12901
		private static AvTraceDetails _PathRequiresTreeContext;

		// Token: 0x04003266 RID: 12902
		private static AvTraceDetails _NoMentorExtended;

		// Token: 0x04003267 RID: 12903
		private static AvTraceDetails _ContextElement;

		// Token: 0x04003268 RID: 12904
		private static AvTraceDetails _NullDataContext;

		// Token: 0x04003269 RID: 12905
		private static AvTraceDetails _RelativeSource;

		// Token: 0x0400326A RID: 12906
		private static AvTraceDetails _AncestorLookup;

		// Token: 0x0400326B RID: 12907
		private static AvTraceDetails _ElementNameQuery;

		// Token: 0x0400326C RID: 12908
		private static AvTraceDetails _ElementNameQueryTemplate;

		// Token: 0x0400326D RID: 12909
		private static AvTraceDetails _UseCVS;

		// Token: 0x0400326E RID: 12910
		private static AvTraceDetails _UseDataProvider;

		// Token: 0x0400326F RID: 12911
		private static AvTraceDetails _ActivateItem;

		// Token: 0x04003270 RID: 12912
		private static AvTraceDetails _Deactivate;

		// Token: 0x04003271 RID: 12913
		private static AvTraceDetails _GetRawValue;

		// Token: 0x04003272 RID: 12914
		private static AvTraceDetails _ConvertDBNull;

		// Token: 0x04003273 RID: 12915
		private static AvTraceDetails _UserConverter;

		// Token: 0x04003274 RID: 12916
		private static AvTraceDetails _NullConverter;

		// Token: 0x04003275 RID: 12917
		private static AvTraceDetails _DefaultConverter;

		// Token: 0x04003276 RID: 12918
		private static AvTraceDetails _FormattedValue;

		// Token: 0x04003277 RID: 12919
		private static AvTraceDetails _FormattingFailed;

		// Token: 0x04003278 RID: 12920
		private static AvTraceDetails _BadValueAtTransferExtended;

		// Token: 0x04003279 RID: 12921
		private static AvTraceDetails _UseFallback;

		// Token: 0x0400327A RID: 12922
		private static AvTraceDetails _TransferValue;

		// Token: 0x0400327B RID: 12923
		private static AvTraceDetails _UpdateRawValue;

		// Token: 0x0400327C RID: 12924
		private static AvTraceDetails _ValidationRuleFailed;

		// Token: 0x0400327D RID: 12925
		private static AvTraceDetails _UserConvertBack;

		// Token: 0x0400327E RID: 12926
		private static AvTraceDetails _DefaultConvertBack;

		// Token: 0x0400327F RID: 12927
		private static AvTraceDetails _Update;

		// Token: 0x04003280 RID: 12928
		private static AvTraceDetails _GotEvent;

		// Token: 0x04003281 RID: 12929
		private static AvTraceDetails _GotPropertyChanged;

		// Token: 0x04003282 RID: 12930
		private static AvTraceDetails _PriorityTransfer;

		// Token: 0x04003283 RID: 12931
		private static AvTraceDetails _ChildNotAttached;

		// Token: 0x04003284 RID: 12932
		private static AvTraceDetails _GetRawValueMulti;

		// Token: 0x04003285 RID: 12933
		private static AvTraceDetails _UserConvertBackMulti;

		// Token: 0x04003286 RID: 12934
		private static AvTraceDetails _GetValue;

		// Token: 0x04003287 RID: 12935
		private static AvTraceDetails _SetValue;

		// Token: 0x04003288 RID: 12936
		private static AvTraceDetails _ReplaceItemShort;

		// Token: 0x04003289 RID: 12937
		private static AvTraceDetails _ReplaceItemLong;

		// Token: 0x0400328A RID: 12938
		private static AvTraceDetails _GetInfo_Reuse;

		// Token: 0x0400328B RID: 12939
		private static AvTraceDetails _GetInfo_Null;

		// Token: 0x0400328C RID: 12940
		private static AvTraceDetails _GetInfo_Cache;

		// Token: 0x0400328D RID: 12941
		private static AvTraceDetails _GetInfo_Property;

		// Token: 0x0400328E RID: 12942
		private static AvTraceDetails _GetInfo_Indexer;

		// Token: 0x0400328F RID: 12943
		private static AvTraceDetails _XmlContextNode;

		// Token: 0x04003290 RID: 12944
		private static AvTraceDetails _XmlNewCollection;

		// Token: 0x04003291 RID: 12945
		private static AvTraceDetails _XmlSynchronizeCollection;

		// Token: 0x04003292 RID: 12946
		private static AvTraceDetails _SelectNodes;

		// Token: 0x04003293 RID: 12947
		private static AvTraceDetails _BeginQuery;

		// Token: 0x04003294 RID: 12948
		private static AvTraceDetails _QueryFinished;

		// Token: 0x04003295 RID: 12949
		private static AvTraceDetails _QueryResult;

		// Token: 0x04003296 RID: 12950
		private static AvTraceDetails _XmlLoadSource;

		// Token: 0x04003297 RID: 12951
		private static AvTraceDetails _XmlLoadDoc;

		// Token: 0x04003298 RID: 12952
		private static AvTraceDetails _XmlLoadInline;

		// Token: 0x04003299 RID: 12953
		private static AvTraceDetails _XmlBuildCollection;
	}
}
