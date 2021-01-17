using System;

namespace MS.Internal.AppModel
{
	// Token: 0x0200077C RID: 1916
	internal interface IContentContainer
	{
		// Token: 0x06007915 RID: 30997
		void OnContentReady(ContentType contentType, object content, Uri uri, object navState);

		// Token: 0x06007916 RID: 30998
		void OnNavigationProgress(Uri uri, long bytesRead, long maxBytes);

		// Token: 0x06007917 RID: 30999
		void OnStreamClosed(Uri uri);
	}
}
