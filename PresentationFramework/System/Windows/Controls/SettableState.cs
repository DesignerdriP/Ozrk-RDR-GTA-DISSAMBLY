using System;

namespace System.Windows.Controls
{
	// Token: 0x0200046E RID: 1134
	internal struct SettableState<T>
	{
		// Token: 0x0600422F RID: 16943 RVA: 0x0012EB48 File Offset: 0x0012CD48
		internal SettableState(T value)
		{
			this._value = value;
			this._isSet = (this._wasSet = false);
		}

		// Token: 0x040027D7 RID: 10199
		internal T _value;

		// Token: 0x040027D8 RID: 10200
		internal bool _isSet;

		// Token: 0x040027D9 RID: 10201
		internal bool _wasSet;
	}
}
