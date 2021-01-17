using System;

namespace MS.Internal.Data
{
	// Token: 0x02000729 RID: 1833
	internal interface IAsyncDataDispatcher
	{
		// Token: 0x06007543 RID: 30019
		void AddRequest(AsyncDataRequest request);

		// Token: 0x06007544 RID: 30020
		void CancelAllRequests();
	}
}
