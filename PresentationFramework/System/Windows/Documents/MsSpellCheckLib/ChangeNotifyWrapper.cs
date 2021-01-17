using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045E RID: 1118
	internal class ChangeNotifyWrapper<T> : IChangeNotifyWrapper<T>, IChangeNotifyWrapper, INotifyPropertyChanged
	{
		// Token: 0x060040A0 RID: 16544 RVA: 0x0012725E File Offset: 0x0012545E
		internal ChangeNotifyWrapper(T value = default(T), bool shouldThrowInvalidCastException = false)
		{
			this.Value = value;
			this._shouldThrowInvalidCastException = shouldThrowInvalidCastException;
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x060040A1 RID: 16545 RVA: 0x00127274 File Offset: 0x00125474
		// (set) Token: 0x060040A2 RID: 16546 RVA: 0x0012727C File Offset: 0x0012547C
		public T Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("Value"));
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x060040A3 RID: 16547 RVA: 0x001272A0 File Offset: 0x001254A0
		// (set) Token: 0x060040A4 RID: 16548 RVA: 0x001272B0 File Offset: 0x001254B0
		object IChangeNotifyWrapper.Value
		{
			get
			{
				return this.Value;
			}
			set
			{
				T value2 = default(T);
				try
				{
					value2 = (T)((object)value);
				}
				catch (InvalidCastException obj) when (!this._shouldThrowInvalidCastException)
				{
					return;
				}
				this.Value = value2;
			}
		}

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x060040A5 RID: 16549 RVA: 0x00127304 File Offset: 0x00125504
		// (remove) Token: 0x060040A6 RID: 16550 RVA: 0x0012733C File Offset: 0x0012553C
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400276F RID: 10095
		private T _value;

		// Token: 0x04002770 RID: 10096
		private bool _shouldThrowInvalidCastException;
	}
}
