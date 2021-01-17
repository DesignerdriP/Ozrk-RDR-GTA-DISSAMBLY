using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000727 RID: 1831
	internal class DynamicValueConverter : IValueConverter
	{
		// Token: 0x06007512 RID: 29970 RVA: 0x0021790F File Offset: 0x00215B0F
		internal DynamicValueConverter(bool targetToSourceNeeded)
		{
			this._targetToSourceNeeded = targetToSourceNeeded;
		}

		// Token: 0x06007513 RID: 29971 RVA: 0x0021791E File Offset: 0x00215B1E
		internal DynamicValueConverter(bool targetToSourceNeeded, Type sourceType, Type targetType)
		{
			this._targetToSourceNeeded = targetToSourceNeeded;
			this.EnsureConverter(sourceType, targetType);
		}

		// Token: 0x06007514 RID: 29972 RVA: 0x00217935 File Offset: 0x00215B35
		internal object Convert(object value, Type targetType)
		{
			return this.Convert(value, targetType, null, CultureInfo.InvariantCulture);
		}

		// Token: 0x06007515 RID: 29973 RVA: 0x00217948 File Offset: 0x00215B48
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			if (value != null)
			{
				Type type = value.GetType();
				this.EnsureConverter(type, targetType);
				if (this._converter != null)
				{
					result = this._converter.Convert(value, targetType, parameter, culture);
				}
			}
			else if (!targetType.IsValueType)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06007516 RID: 29974 RVA: 0x00217994 File Offset: 0x00215B94
		public object ConvertBack(object value, Type sourceType, object parameter, CultureInfo culture)
		{
			object result = DependencyProperty.UnsetValue;
			if (value != null)
			{
				Type type = value.GetType();
				this.EnsureConverter(sourceType, type);
				if (this._converter != null)
				{
					result = this._converter.ConvertBack(value, sourceType, parameter, culture);
				}
			}
			else if (!sourceType.IsValueType)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06007517 RID: 29975 RVA: 0x002179E0 File Offset: 0x00215BE0
		private void EnsureConverter(Type sourceType, Type targetType)
		{
			if (this._sourceType != sourceType || this._targetType != targetType)
			{
				if (sourceType != null && targetType != null)
				{
					if (this._engine == null)
					{
						this._engine = DataBindEngine.CurrentDataBindEngine;
					}
					Invariant.Assert(this._engine != null);
					this._converter = this._engine.GetDefaultValueConverter(sourceType, targetType, this._targetToSourceNeeded);
				}
				else
				{
					this._converter = null;
				}
				this._sourceType = sourceType;
				this._targetType = targetType;
			}
		}

		// Token: 0x04003814 RID: 14356
		private Type _sourceType;

		// Token: 0x04003815 RID: 14357
		private Type _targetType;

		// Token: 0x04003816 RID: 14358
		private IValueConverter _converter;

		// Token: 0x04003817 RID: 14359
		private bool _targetToSourceNeeded;

		// Token: 0x04003818 RID: 14360
		private DataBindEngine _engine;
	}
}
