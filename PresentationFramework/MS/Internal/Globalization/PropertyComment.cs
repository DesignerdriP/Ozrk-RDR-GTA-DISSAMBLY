using System;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B7 RID: 1719
	internal class PropertyComment
	{
		// Token: 0x06006EC4 RID: 28356 RVA: 0x0000326D File Offset: 0x0000146D
		internal PropertyComment()
		{
		}

		// Token: 0x17001A47 RID: 6727
		// (get) Token: 0x06006EC5 RID: 28357 RVA: 0x001FD9E2 File Offset: 0x001FBBE2
		// (set) Token: 0x06006EC6 RID: 28358 RVA: 0x001FD9EA File Offset: 0x001FBBEA
		internal string PropertyName
		{
			get
			{
				return this._target;
			}
			set
			{
				this._target = value;
			}
		}

		// Token: 0x17001A48 RID: 6728
		// (get) Token: 0x06006EC7 RID: 28359 RVA: 0x001FD9F3 File Offset: 0x001FBBF3
		// (set) Token: 0x06006EC8 RID: 28360 RVA: 0x001FD9FB File Offset: 0x001FBBFB
		internal object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x0400367F RID: 13951
		private string _target;

		// Token: 0x04003680 RID: 13952
		private object _value;
	}
}
