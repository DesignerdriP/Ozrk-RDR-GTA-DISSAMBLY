using System;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	// Token: 0x02000470 RID: 1136
	[Localizability(LocalizationCategory.NeverLocalize)]
	internal sealed class BooleanToSelectiveScrollingOrientationConverter : IValueConverter
	{
		// Token: 0x0600424F RID: 16975 RVA: 0x0012F4C8 File Offset: 0x0012D6C8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool && parameter is SelectiveScrollingOrientation)
			{
				bool flag = (bool)value;
				SelectiveScrollingOrientation selectiveScrollingOrientation = (SelectiveScrollingOrientation)parameter;
				if (flag)
				{
					return selectiveScrollingOrientation;
				}
			}
			return SelectiveScrollingOrientation.Both;
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0003E264 File Offset: 0x0003C464
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
