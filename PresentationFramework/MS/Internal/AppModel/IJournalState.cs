using System;

namespace MS.Internal.AppModel
{
	// Token: 0x02000786 RID: 1926
	internal interface IJournalState
	{
		// Token: 0x06007943 RID: 31043
		CustomJournalStateInternal GetJournalState(JournalReason journalReason);

		// Token: 0x06007944 RID: 31044
		void RestoreJournalState(CustomJournalStateInternal state);
	}
}
