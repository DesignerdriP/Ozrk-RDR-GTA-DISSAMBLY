using System;

namespace MS.Internal.Data
{
	// Token: 0x02000709 RID: 1801
	internal class BindingValueChangedEventArgs : EventArgs
	{
		// Token: 0x0600736F RID: 29551 RVA: 0x00211271 File Offset: 0x0020F471
		internal BindingValueChangedEventArgs(object oldValue, object newValue)
		{
			this._oldValue = oldValue;
			this._newValue = newValue;
		}

		// Token: 0x17001B67 RID: 7015
		// (get) Token: 0x06007370 RID: 29552 RVA: 0x00211287 File Offset: 0x0020F487
		public object OldValue
		{
			get
			{
				return this._oldValue;
			}
		}

		// Token: 0x17001B68 RID: 7016
		// (get) Token: 0x06007371 RID: 29553 RVA: 0x0021128F File Offset: 0x0020F48F
		public object NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x040037A8 RID: 14248
		private object _oldValue;

		// Token: 0x040037A9 RID: 14249
		private object _newValue;
	}
}
