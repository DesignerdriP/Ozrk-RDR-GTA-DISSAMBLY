using System;
using System.ComponentModel;

namespace MS.Internal.Data
{
	// Token: 0x0200074B RID: 1867
	internal class ValueChangedEventArgs : EventArgs
	{
		// Token: 0x06007714 RID: 30484 RVA: 0x002206B0 File Offset: 0x0021E8B0
		internal ValueChangedEventArgs(PropertyDescriptor pd)
		{
			this._pd = pd;
		}

		// Token: 0x17001C4D RID: 7245
		// (get) Token: 0x06007715 RID: 30485 RVA: 0x002206BF File Offset: 0x0021E8BF
		internal PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return this._pd;
			}
		}

		// Token: 0x040038A9 RID: 14505
		private PropertyDescriptor _pd;
	}
}
