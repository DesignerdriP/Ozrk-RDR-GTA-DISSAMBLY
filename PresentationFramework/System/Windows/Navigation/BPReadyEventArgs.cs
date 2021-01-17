using System;
using System.ComponentModel;

namespace System.Windows.Navigation
{
	// Token: 0x02000319 RID: 793
	internal class BPReadyEventArgs : CancelEventArgs
	{
		// Token: 0x060029F5 RID: 10741 RVA: 0x000C19DF File Offset: 0x000BFBDF
		internal BPReadyEventArgs(object content, Uri uri)
		{
			this._content = content;
			this._uri = uri;
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x060029F6 RID: 10742 RVA: 0x000C19F5 File Offset: 0x000BFBF5
		internal object Content
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x060029F7 RID: 10743 RVA: 0x000C19FD File Offset: 0x000BFBFD
		internal Uri Uri
		{
			get
			{
				return this._uri;
			}
		}

		// Token: 0x04001C1E RID: 7198
		private object _content;

		// Token: 0x04001C1F RID: 7199
		private Uri _uri;
	}
}
