using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071A RID: 1818
	internal class SourceDefaultValueConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x060074DB RID: 29915 RVA: 0x00216F36 File Offset: 0x00215136
		public SourceDefaultValueConverter(TypeConverter typeConverter, Type sourceType, Type targetType, bool shouldConvertFrom, bool shouldConvertTo, DataBindEngine engine) : base(typeConverter, sourceType, targetType, shouldConvertFrom, shouldConvertTo, engine)
		{
		}

		// Token: 0x060074DC RID: 29916 RVA: 0x00216F47 File Offset: 0x00215147
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertTo(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x060074DD RID: 29917 RVA: 0x00216F5E File Offset: 0x0021515E
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return base.ConvertFrom(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
