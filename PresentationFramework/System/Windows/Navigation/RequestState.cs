using System;
using System.Net;
using System.Windows.Threading;

namespace System.Windows.Navigation
{
	// Token: 0x02000318 RID: 792
	internal class RequestState
	{
		// Token: 0x060029F0 RID: 10736 RVA: 0x000C199A File Offset: 0x000BFB9A
		internal RequestState(WebRequest request, Uri source, object navState, Dispatcher callbackDispatcher)
		{
			this._request = request;
			this._source = source;
			this._navState = navState;
			this._callbackDispatcher = callbackDispatcher;
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x060029F1 RID: 10737 RVA: 0x000C19BF File Offset: 0x000BFBBF
		internal WebRequest Request
		{
			get
			{
				return this._request;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x060029F2 RID: 10738 RVA: 0x000C19C7 File Offset: 0x000BFBC7
		internal Uri Source
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x060029F3 RID: 10739 RVA: 0x000C19CF File Offset: 0x000BFBCF
		internal object NavState
		{
			get
			{
				return this._navState;
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x060029F4 RID: 10740 RVA: 0x000C19D7 File Offset: 0x000BFBD7
		internal Dispatcher CallbackDispatcher
		{
			get
			{
				return this._callbackDispatcher;
			}
		}

		// Token: 0x04001C1A RID: 7194
		private WebRequest _request;

		// Token: 0x04001C1B RID: 7195
		private Uri _source;

		// Token: 0x04001C1C RID: 7196
		private object _navState;

		// Token: 0x04001C1D RID: 7197
		private Dispatcher _callbackDispatcher;
	}
}
