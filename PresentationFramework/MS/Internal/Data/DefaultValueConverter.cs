using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Data;
using System.Windows.Markup;

namespace MS.Internal.Data
{
	// Token: 0x02000719 RID: 1817
	internal class DefaultValueConverter
	{
		// Token: 0x060074D1 RID: 29905 RVA: 0x00216A37 File Offset: 0x00214C37
		protected DefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine)
		{
			this._typeConverter = typeConverter;
			this._sourceType = sourceType;
			this._targetType = targetType;
			this._shouldConvertFrom = shouldConvertFrom;
			this._shouldConvertTo = shouldConvertTo;
			this._engine = engine;
		}

		// Token: 0x060074D2 RID: 29906 RVA: 0x00216A6C File Offset: 0x00214C6C
		internal static IValueConverter Create(Type sourceType, Type targetType, bool targetToSource, DataBindEngine engine)
		{
			bool flag = false;
			bool flag2 = false;
			if (sourceType == targetType || (!targetToSource && targetType.IsAssignableFrom(sourceType)))
			{
				return DefaultValueConverter.ValueConverterNotNeeded;
			}
			if (targetType == typeof(object))
			{
				return new ObjectTargetConverter(sourceType, engine);
			}
			if (sourceType == typeof(object))
			{
				return new ObjectSourceConverter(targetType, engine);
			}
			if (SystemConvertConverter.CanConvert(sourceType, targetType))
			{
				return new SystemConvertConverter(sourceType, targetType);
			}
			Type underlyingType = Nullable.GetUnderlyingType(sourceType);
			if (underlyingType != null)
			{
				sourceType = underlyingType;
				flag = true;
			}
			underlyingType = Nullable.GetUnderlyingType(targetType);
			if (underlyingType != null)
			{
				targetType = underlyingType;
				flag2 = true;
			}
			if (flag || flag2)
			{
				return DefaultValueConverter.Create(sourceType, targetType, targetToSource, engine);
			}
			if (typeof(IListSource).IsAssignableFrom(sourceType) && targetType.IsAssignableFrom(typeof(IList)))
			{
				return new ListSourceConverter();
			}
			if (sourceType.IsInterface || targetType.IsInterface)
			{
				return new InterfaceConverter(sourceType, targetType);
			}
			TypeConverter converter = DefaultValueConverter.GetConverter(sourceType);
			bool flag3 = converter != null && converter.CanConvertTo(targetType);
			bool flag4 = converter != null && converter.CanConvertFrom(targetType);
			if ((flag3 || targetType.IsAssignableFrom(sourceType)) && (!targetToSource || flag4 || sourceType.IsAssignableFrom(targetType)))
			{
				return new SourceDefaultValueConverter(converter, sourceType, targetType, targetToSource && flag4, flag3, engine);
			}
			converter = DefaultValueConverter.GetConverter(targetType);
			flag3 = (converter != null && converter.CanConvertTo(sourceType));
			flag4 = (converter != null && converter.CanConvertFrom(sourceType));
			if ((flag4 || targetType.IsAssignableFrom(sourceType)) && (!targetToSource || flag3 || sourceType.IsAssignableFrom(targetType)))
			{
				return new TargetDefaultValueConverter(converter, sourceType, targetType, flag4, targetToSource && flag3, engine);
			}
			return null;
		}

		// Token: 0x060074D3 RID: 29907 RVA: 0x00216BFC File Offset: 0x00214DFC
		internal static TypeConverter GetConverter(Type type)
		{
			TypeConverter typeConverter = null;
			WpfKnownType wpfKnownType = XamlReader.BamlSharedSchemaContext.GetKnownXamlType(type) as WpfKnownType;
			if (wpfKnownType != null && wpfKnownType.TypeConverter != null)
			{
				typeConverter = wpfKnownType.TypeConverter.ConverterInstance;
			}
			if (typeConverter == null)
			{
				typeConverter = TypeDescriptor.GetConverter(type);
			}
			return typeConverter;
		}

		// Token: 0x060074D4 RID: 29908 RVA: 0x00216C4C File Offset: 0x00214E4C
		internal static object TryParse(object o, Type targetType, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			string text = o as string;
			if (text != null)
			{
				try
				{
					MethodInfo method;
					if (culture != null && (method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						DefaultValueConverter.StringType,
						typeof(NumberStyles),
						typeof(IFormatProvider)
					}, null)) != null)
					{
						result = method.Invoke(null, new object[]
						{
							text,
							NumberStyles.Any,
							culture
						});
					}
					else if (culture != null && (method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						DefaultValueConverter.StringType,
						typeof(IFormatProvider)
					}, null)) != null)
					{
						result = method.Invoke(null, new object[]
						{
							text,
							culture
						});
					}
					else if ((method = targetType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						DefaultValueConverter.StringType
					}, null)) != null)
					{
						result = method.Invoke(null, new object[]
						{
							text
						});
					}
				}
				catch (TargetInvocationException)
				{
				}
			}
			return result;
		}

		// Token: 0x060074D5 RID: 29909 RVA: 0x00216D78 File Offset: 0x00214F78
		protected object ConvertFrom(object o, Type destinationType, DependencyObject targetElement, CultureInfo culture)
		{
			return this.ConvertHelper(o, destinationType, targetElement, culture, false);
		}

		// Token: 0x060074D6 RID: 29910 RVA: 0x00216D86 File Offset: 0x00214F86
		protected object ConvertTo(object o, Type destinationType, DependencyObject targetElement, CultureInfo culture)
		{
			return this.ConvertHelper(o, destinationType, targetElement, culture, true);
		}

		// Token: 0x060074D7 RID: 29911 RVA: 0x00216D94 File Offset: 0x00214F94
		protected void EnsureConverter(Type type)
		{
			if (this._typeConverter == null)
			{
				this._typeConverter = DefaultValueConverter.GetConverter(type);
			}
		}

		// Token: 0x060074D8 RID: 29912 RVA: 0x00216DAC File Offset: 0x00214FAC
		private object ConvertHelper(object o, Type destinationType, DependencyObject targetElement, CultureInfo culture, bool isForward)
		{
			object obj = DependencyProperty.UnsetValue;
			bool flag = isForward ? (!this._shouldConvertTo) : (!this._shouldConvertFrom);
			NotSupportedException ex = null;
			if (!flag)
			{
				obj = DefaultValueConverter.TryParse(o, destinationType, culture);
				if (obj == DependencyProperty.UnsetValue)
				{
					ValueConverterContext valueConverterContext = this.Engine.ValueConverterContext;
					if (valueConverterContext.IsInUse)
					{
						valueConverterContext = new ValueConverterContext();
					}
					try
					{
						valueConverterContext.SetTargetElement(targetElement);
						if (isForward)
						{
							obj = this._typeConverter.ConvertTo(valueConverterContext, culture, o, destinationType);
						}
						else
						{
							obj = this._typeConverter.ConvertFrom(valueConverterContext, culture, o);
						}
					}
					catch (NotSupportedException ex2)
					{
						flag = true;
						ex = ex2;
					}
					finally
					{
						valueConverterContext.SetTargetElement(null);
					}
				}
			}
			if (flag && ((o != null && destinationType.IsAssignableFrom(o.GetType())) || (o == null && !destinationType.IsValueType)))
			{
				obj = o;
				flag = false;
			}
			if (TraceData.IsEnabled)
			{
				if (culture != null && ex != null)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.DefaultValueConverterFailedForCulture(new object[]
					{
						AvTrace.ToStringHelper(o),
						AvTrace.TypeName(o),
						destinationType.ToString(),
						culture
					}), ex);
				}
				else if (flag)
				{
					TraceData.Trace(TraceEventType.Error, TraceData.DefaultValueConverterFailed(new object[]
					{
						AvTrace.ToStringHelper(o),
						AvTrace.TypeName(o),
						destinationType.ToString()
					}), ex);
				}
			}
			if (flag && ex != null)
			{
				throw ex;
			}
			return obj;
		}

		// Token: 0x17001BCB RID: 7115
		// (get) Token: 0x060074D9 RID: 29913 RVA: 0x00216F08 File Offset: 0x00215108
		protected DataBindEngine Engine
		{
			get
			{
				return this._engine;
			}
		}

		// Token: 0x040037FC RID: 14332
		internal static readonly IValueConverter ValueConverterNotNeeded = new ObjectTargetConverter(typeof(object), null);

		// Token: 0x040037FD RID: 14333
		protected Type _sourceType;

		// Token: 0x040037FE RID: 14334
		protected Type _targetType;

		// Token: 0x040037FF RID: 14335
		private TypeConverter _typeConverter;

		// Token: 0x04003800 RID: 14336
		private bool _shouldConvertFrom;

		// Token: 0x04003801 RID: 14337
		private bool _shouldConvertTo;

		// Token: 0x04003802 RID: 14338
		private DataBindEngine _engine;

		// Token: 0x04003803 RID: 14339
		private static Type StringType = typeof(string);
	}
}
