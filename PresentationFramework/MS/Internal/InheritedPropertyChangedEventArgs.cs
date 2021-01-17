using System;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005E8 RID: 1512
	internal class InheritedPropertyChangedEventArgs : EventArgs
	{
		// Token: 0x060064EB RID: 25835 RVA: 0x001C52D2 File Offset: 0x001C34D2
		internal InheritedPropertyChangedEventArgs(ref InheritablePropertyChangeInfo info)
		{
			this._info = info;
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x060064EC RID: 25836 RVA: 0x001C52E6 File Offset: 0x001C34E6
		internal InheritablePropertyChangeInfo Info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x040032B7 RID: 12983
		private InheritablePropertyChangeInfo _info;
	}
}
