using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000789 RID: 1929
	internal class JournalEntryKeepAlive : JournalEntry
	{
		// Token: 0x06007952 RID: 31058 RVA: 0x00226F62 File Offset: 0x00225162
		internal JournalEntryKeepAlive(JournalEntryGroupState jeGroupState, Uri uri, object keepAliveRoot) : base(jeGroupState, uri)
		{
			Invariant.Assert(keepAliveRoot != null);
			this._keepAliveRoot = keepAliveRoot;
		}

		// Token: 0x17001CAA RID: 7338
		// (get) Token: 0x06007953 RID: 31059 RVA: 0x00226F7C File Offset: 0x0022517C
		internal object KeepAliveRoot
		{
			get
			{
				return this._keepAliveRoot;
			}
		}

		// Token: 0x06007954 RID: 31060 RVA: 0x00226F84 File Offset: 0x00225184
		internal override bool IsAlive()
		{
			return this.KeepAliveRoot != null;
		}

		// Token: 0x06007955 RID: 31061 RVA: 0x00226F8F File Offset: 0x0022518F
		internal override void SaveState(object contentObject)
		{
			this._keepAliveRoot = contentObject;
		}

		// Token: 0x06007956 RID: 31062 RVA: 0x00226F98 File Offset: 0x00225198
		internal override bool Navigate(INavigator navigator, NavigationMode navMode)
		{
			return navigator.Navigate(this.KeepAliveRoot, new NavigateInfo(base.Source, navMode, this));
		}

		// Token: 0x0400397D RID: 14717
		private object _keepAliveRoot;
	}
}
