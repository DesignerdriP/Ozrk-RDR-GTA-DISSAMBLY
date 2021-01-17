using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071D RID: 1821
	internal class ObjectTargetConverter : DefaultValueConverter, IValueConverter
	{
		// Token: 0x060074E7 RID: 29927 RVA: 0x0021727F File Offset: 0x0021547F
		public ObjectTargetConverter(Type sourceType, DataBindEngine engine) : base(null, sourceType, typeof(object), true, false, engine)
		{
		}

		// Token: 0x060074E8 RID: 29928 RVA: 0x00012630 File Offset: 0x00010830
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return o;
		}

		// Token: 0x060074E9 RID: 29929 RVA: 0x00217298 File Offset: 0x00215498
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			if (o == null && !this._sourceType.IsValueType)
			{
				return o;
			}
			if (o != null && this._sourceType.IsAssignableFrom(o.GetType()))
			{
				return o;
			}
			if (this._sourceType == typeof(string))
			{
				return string.Format(culture, "{0}", new object[]
				{
					o
				});
			}
			base.EnsureConverter(this._sourceType);
			return base.ConvertFrom(o, this._sourceType, parameter as DependencyObject, culture);
		}
	}
}
