using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071E RID: 1822
	internal class ObjectSourceConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x060074EA RID: 29930 RVA: 0x0021731E File Offset: 0x0021551E
		public ObjectSourceConverter(Type targetType, DataBindEngine engine) : base(null, typeof(object), targetType, true, false, engine)
		{
		}

		// Token: 0x060074EB RID: 29931 RVA: 0x00217338 File Offset: 0x00215538
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			if ((o != null && this._targetType.IsAssignableFrom(o.GetType())) || (o == null && !this._targetType.IsValueType))
			{
				return o;
			}
			if (this._targetType == typeof(string))
			{
				return string.Format(culture, "{0}", new object[]
				{
					o
				});
			}
			base.EnsureConverter(this._targetType);
			return base.ConvertFrom(o, this._targetType, parameter as DependencyObject, culture);
		}

		// Token: 0x060074EC RID: 29932 RVA: 0x00012630 File Offset: 0x00010830
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return o;
		}
	}
}
