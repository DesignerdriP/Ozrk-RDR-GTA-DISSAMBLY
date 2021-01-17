using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071B RID: 1819
	internal class TargetDefaultValueConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x060074DE RID: 29918 RVA: 0x00216F36 File Offset: 0x00215136
		public TargetDefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine) : base(typeConverter, sourceType, targetType, shouldConvertFrom, shouldConvertTo, engine)
		{
		}

		// Token: 0x060074DF RID: 29919 RVA: 0x00216F75 File Offset: 0x00215175
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertFrom(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x060074E0 RID: 29920 RVA: 0x00216F8C File Offset: 0x0021518C
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertTo(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
