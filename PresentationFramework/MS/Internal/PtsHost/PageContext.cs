using System;
using System.Collections.Generic;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063D RID: 1597
	internal class PageContext
	{
		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x06006A15 RID: 27157 RVA: 0x001E355A File Offset: 0x001E175A
		// (set) Token: 0x06006A16 RID: 27158 RVA: 0x001E3562 File Offset: 0x001E1762
		internal PTS.FSRECT PageRect
		{
			get
			{
				return this._pageRect;
			}
			set
			{
				this._pageRect = value;
			}
		}

		// Token: 0x1700197E RID: 6526
		// (get) Token: 0x06006A17 RID: 27159 RVA: 0x001E356B File Offset: 0x001E176B
		internal List<BaseParaClient> FloatingElementList
		{
			get
			{
				return this._floatingElementList;
			}
		}

		// Token: 0x06006A18 RID: 27160 RVA: 0x001E3573 File Offset: 0x001E1773
		internal void AddFloatingParaClient(BaseParaClient floatingElement)
		{
			if (this._floatingElementList == null)
			{
				this._floatingElementList = new List<BaseParaClient>();
			}
			if (!this._floatingElementList.Contains(floatingElement))
			{
				this._floatingElementList.Add(floatingElement);
			}
		}

		// Token: 0x06006A19 RID: 27161 RVA: 0x001E35A2 File Offset: 0x001E17A2
		internal void RemoveFloatingParaClient(BaseParaClient floatingElement)
		{
			if (this._floatingElementList.Contains(floatingElement))
			{
				this._floatingElementList.Remove(floatingElement);
			}
			if (this._floatingElementList.Count == 0)
			{
				this._floatingElementList = null;
			}
		}

		// Token: 0x04003426 RID: 13350
		private List<BaseParaClient> _floatingElementList;

		// Token: 0x04003427 RID: 13351
		private PTS.FSRECT _pageRect;
	}
}
