using System;
using System.Collections;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000781 RID: 1921
	internal interface INavigator : INavigatorBase
	{
		// Token: 0x06007936 RID: 31030
		JournalNavigationScope GetJournal(bool create);

		// Token: 0x17001CA2 RID: 7330
		// (get) Token: 0x06007937 RID: 31031
		bool CanGoForward { get; }

		// Token: 0x17001CA3 RID: 7331
		// (get) Token: 0x06007938 RID: 31032
		bool CanGoBack { get; }

		// Token: 0x06007939 RID: 31033
		void GoForward();

		// Token: 0x0600793A RID: 31034
		void GoBack();

		// Token: 0x0600793B RID: 31035
		void AddBackEntry(CustomContentState state);

		// Token: 0x0600793C RID: 31036
		JournalEntry RemoveBackEntry();

		// Token: 0x17001CA4 RID: 7332
		// (get) Token: 0x0600793D RID: 31037
		IEnumerable BackStack { get; }

		// Token: 0x17001CA5 RID: 7333
		// (get) Token: 0x0600793E RID: 31038
		IEnumerable ForwardStack { get; }
	}
}
