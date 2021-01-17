using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200077F RID: 1919
	internal interface IDownloader
	{
		// Token: 0x17001C9E RID: 7326
		// (get) Token: 0x0600791A RID: 31002
		NavigationService Downloader { get; }

		// Token: 0x1400015D RID: 349
		// (add) Token: 0x0600791B RID: 31003
		// (remove) Token: 0x0600791C RID: 31004
		event EventHandler ContentRendered;
	}
}
