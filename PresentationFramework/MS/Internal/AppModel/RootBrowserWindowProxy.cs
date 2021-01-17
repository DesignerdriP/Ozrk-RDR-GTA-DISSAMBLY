using System;

namespace MS.Internal.AppModel
{
	// Token: 0x0200079B RID: 1947
	internal class RootBrowserWindowProxy : MarshalByRefObject
	{
		// Token: 0x06007A15 RID: 31253 RVA: 0x0022957F File Offset: 0x0022777F
		internal RootBrowserWindowProxy(RootBrowserWindow rbw)
		{
			this._rbw = rbw;
		}

		// Token: 0x17001CC6 RID: 7366
		// (get) Token: 0x06007A16 RID: 31254 RVA: 0x0022958E File Offset: 0x0022778E
		internal RootBrowserWindow RootBrowserWindow
		{
			get
			{
				return this._rbw;
			}
		}

		// Token: 0x06007A17 RID: 31255 RVA: 0x00229596 File Offset: 0x00227796
		internal void TabInto(bool forward)
		{
			this._rbw.TabInto(forward);
		}

		// Token: 0x040039B4 RID: 14772
		private RootBrowserWindow _rbw;
	}
}
