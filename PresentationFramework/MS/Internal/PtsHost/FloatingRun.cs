using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000641 RID: 1601
	internal sealed class FloatingRun : TextHidden
	{
		// Token: 0x06006A37 RID: 27191 RVA: 0x001E3CCA File Offset: 0x001E1ECA
		internal FloatingRun(int length, bool figure) : base(length)
		{
			this._figure = figure;
		}

		// Token: 0x1700198A RID: 6538
		// (get) Token: 0x06006A38 RID: 27192 RVA: 0x001E3CDA File Offset: 0x001E1EDA
		internal bool Figure
		{
			get
			{
				return this._figure;
			}
		}

		// Token: 0x0400342F RID: 13359
		private readonly bool _figure;
	}
}
