using System;
using System.Globalization;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000720 RID: 1824
	internal class InterfaceConverter : IValueConverter
	{
		// Token: 0x060074F0 RID: 29936 RVA: 0x002173DD File Offset: 0x002155DD
		internal InterfaceConverter(Type sourceType, Type targetType)
		{
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		// Token: 0x060074F1 RID: 29937 RVA: 0x002173F3 File Offset: 0x002155F3
		public object Convert(object o, Type type, object parameter, CultureInfo culture)
		{
			return this.ConvertTo(o, this._targetType);
		}

		// Token: 0x060074F2 RID: 29938 RVA: 0x00217402 File Offset: 0x00215602
		public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
		{
			return this.ConvertTo(o, this._sourceType);
		}

		// Token: 0x060074F3 RID: 29939 RVA: 0x00217411 File Offset: 0x00215611
		private object ConvertTo(object o, Type type)
		{
			if (!type.IsInstanceOfType(o))
			{
				return null;
			}
			return o;
		}

		// Token: 0x04003808 RID: 14344
		private Type _sourceType;

		// Token: 0x04003809 RID: 14345
		private Type _targetType;
	}
}
