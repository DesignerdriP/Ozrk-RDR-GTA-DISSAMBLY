using System;

namespace System.Windows.Documents
{
	// Token: 0x02000361 RID: 865
	internal abstract class FixedSOMPageElement : FixedSOMContainer
	{
		// Token: 0x06002E07 RID: 11783 RVA: 0x000CFF42 File Offset: 0x000CE142
		public FixedSOMPageElement(FixedSOMPage page)
		{
			this._page = page;
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06002E08 RID: 11784 RVA: 0x000CFF51 File Offset: 0x000CE151
		public FixedSOMPage FixedSOMPage
		{
			get
			{
				return this._page;
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06002E09 RID: 11785
		public abstract bool IsRTL { get; }

		// Token: 0x04001DE7 RID: 7655
		protected FixedSOMPage _page;
	}
}
