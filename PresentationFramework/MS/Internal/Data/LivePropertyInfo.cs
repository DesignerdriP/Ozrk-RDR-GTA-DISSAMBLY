using System;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x0200072E RID: 1838
	internal struct LivePropertyInfo
	{
		// Token: 0x060075A9 RID: 30121 RVA: 0x002196BF File Offset: 0x002178BF
		public LivePropertyInfo(string path, DependencyProperty dp)
		{
			this._path = path;
			this._dp = dp;
		}

		// Token: 0x17001C06 RID: 7174
		// (get) Token: 0x060075AA RID: 30122 RVA: 0x002196CF File Offset: 0x002178CF
		public string Path
		{
			get
			{
				return this._path;
			}
		}

		// Token: 0x17001C07 RID: 7175
		// (get) Token: 0x060075AB RID: 30123 RVA: 0x002196D7 File Offset: 0x002178D7
		public DependencyProperty Property
		{
			get
			{
				return this._dp;
			}
		}

		// Token: 0x04003835 RID: 14389
		private string _path;

		// Token: 0x04003836 RID: 14390
		private DependencyProperty _dp;
	}
}
