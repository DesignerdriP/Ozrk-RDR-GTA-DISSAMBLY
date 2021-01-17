using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000787 RID: 1927
	[Serializable]
	internal class JournalEntryGroupState
	{
		// Token: 0x06007945 RID: 31045 RVA: 0x0000326D File Offset: 0x0000146D
		internal JournalEntryGroupState()
		{
		}

		// Token: 0x06007946 RID: 31046 RVA: 0x00226ED5 File Offset: 0x002250D5
		internal JournalEntryGroupState(Guid navSvcId, uint contentId)
		{
			this._navigationServiceId = navSvcId;
			this._contentId = contentId;
		}

		// Token: 0x17001CA6 RID: 7334
		// (get) Token: 0x06007947 RID: 31047 RVA: 0x00226EEB File Offset: 0x002250EB
		// (set) Token: 0x06007948 RID: 31048 RVA: 0x00226EF3 File Offset: 0x002250F3
		internal Guid NavigationServiceId
		{
			get
			{
				return this._navigationServiceId;
			}
			set
			{
				this._navigationServiceId = value;
			}
		}

		// Token: 0x17001CA7 RID: 7335
		// (get) Token: 0x06007949 RID: 31049 RVA: 0x00226EFC File Offset: 0x002250FC
		// (set) Token: 0x0600794A RID: 31050 RVA: 0x00226F04 File Offset: 0x00225104
		internal uint ContentId
		{
			get
			{
				return this._contentId;
			}
			set
			{
				this._contentId = value;
			}
		}

		// Token: 0x17001CA8 RID: 7336
		// (get) Token: 0x0600794B RID: 31051 RVA: 0x00226F0D File Offset: 0x0022510D
		// (set) Token: 0x0600794C RID: 31052 RVA: 0x00226F15 File Offset: 0x00225115
		internal DataStreams JournalDataStreams
		{
			get
			{
				return this._journalDataStreams;
			}
			set
			{
				this._journalDataStreams = value;
			}
		}

		// Token: 0x17001CA9 RID: 7337
		// (get) Token: 0x0600794D RID: 31053 RVA: 0x00226F1E File Offset: 0x0022511E
		// (set) Token: 0x0600794E RID: 31054 RVA: 0x00226F26 File Offset: 0x00225126
		internal JournalEntry GroupExitEntry
		{
			get
			{
				return this._groupExitEntry;
			}
			set
			{
				this._groupExitEntry = value;
			}
		}

		// Token: 0x04003979 RID: 14713
		private Guid _navigationServiceId;

		// Token: 0x0400397A RID: 14714
		private uint _contentId;

		// Token: 0x0400397B RID: 14715
		private DataStreams _journalDataStreams;

		// Token: 0x0400397C RID: 14716
		private JournalEntry _groupExitEntry;
	}
}
