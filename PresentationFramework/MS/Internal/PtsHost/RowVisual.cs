using System;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063F RID: 1599
	internal sealed class RowVisual : ContainerVisual
	{
		// Token: 0x06006A2A RID: 27178 RVA: 0x001E3B70 File Offset: 0x001E1D70
		internal RowVisual(TableRow row)
		{
			this._row = row;
		}

		// Token: 0x17001982 RID: 6530
		// (get) Token: 0x06006A2B RID: 27179 RVA: 0x001E3B7F File Offset: 0x001E1D7F
		internal TableRow Row
		{
			get
			{
				return this._row;
			}
		}

		// Token: 0x0400342A RID: 13354
		private readonly TableRow _row;
	}
}
