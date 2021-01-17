using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x0200071F RID: 1823
	internal class ListSourceConverter : IValueConverter
	{
		// Token: 0x060074ED RID: 29933 RVA: 0x002173BC File Offset: 0x002155BC
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			IList result = null;
			IListSource listSource = o as IListSource;
			if (listSource != null)
			{
				result = listSource.GetList();
			}
			return result;
		}

		// Token: 0x060074EE RID: 29934 RVA: 0x0000C238 File Offset: 0x0000A438
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
