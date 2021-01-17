using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071C RID: 1820
	internal class SystemConvertConverter : IValueConverter
	{
		// Token: 0x060074E1 RID: 29921 RVA: 0x00216FA3 File Offset: 0x002151A3
		public SystemConvertConverter(Type sourceType, Type targetType)
		{
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		// Token: 0x060074E2 RID: 29922 RVA: 0x00216FB9 File Offset: 0x002151B9
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return System.Convert.ChangeType(o, this._targetType, culture);
		}

		// Token: 0x060074E3 RID: 29923 RVA: 0x00216FCC File Offset: 0x002151CC
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			object obj = DefaultValueConverter.TryParse(o, this._sourceType, culture);
			if (obj == DependencyProperty.UnsetValue)
			{
				return System.Convert.ChangeType(o, this._sourceType, culture);
			}
			return obj;
		}

		// Token: 0x060074E4 RID: 29924 RVA: 0x00217000 File Offset: 0x00215200
		public static bool CanConvert(Type sourceType, Type targetType)
		{
			if (sourceType == typeof(DateTime))
			{
				return targetType == typeof(string);
			}
			if (targetType == typeof(DateTime))
			{
				return sourceType == typeof(string);
			}
			if (sourceType == typeof(char))
			{
				return SystemConvertConverter.CanConvertChar(targetType);
			}
			if (targetType == typeof(char))
			{
				return SystemConvertConverter.CanConvertChar(sourceType);
			}
			for (int i = 0; i < SystemConvertConverter.SupportedTypes.Length; i++)
			{
				if (sourceType == SystemConvertConverter.SupportedTypes[i])
				{
					for (i++; i < SystemConvertConverter.SupportedTypes.Length; i++)
					{
						if (targetType == SystemConvertConverter.SupportedTypes[i])
						{
							return true;
						}
					}
				}
				else if (targetType == SystemConvertConverter.SupportedTypes[i])
				{
					for (i++; i < SystemConvertConverter.SupportedTypes.Length; i++)
					{
						if (sourceType == SystemConvertConverter.SupportedTypes[i])
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060074E5 RID: 29925 RVA: 0x00217104 File Offset: 0x00215304
		private static bool CanConvertChar(Type type)
		{
			for (int i = 0; i < SystemConvertConverter.CharSupportedTypes.Length; i++)
			{
				if (type == SystemConvertConverter.CharSupportedTypes[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04003804 RID: 14340
		private Type _sourceType;

		// Token: 0x04003805 RID: 14341
		private Type _targetType;

		// Token: 0x04003806 RID: 14342
		private static readonly Type[] SupportedTypes = new Type[]
		{
			typeof(string),
			typeof(int),
			typeof(long),
			typeof(float),
			typeof(double),
			typeof(decimal),
			typeof(bool),
			typeof(byte),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte)
		};

		// Token: 0x04003807 RID: 14343
		private static readonly Type[] CharSupportedTypes = new Type[]
		{
			typeof(string),
			typeof(int),
			typeof(long),
			typeof(byte),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte)
		};
	}
}
