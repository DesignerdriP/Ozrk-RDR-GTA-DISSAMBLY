using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000790 RID: 1936
	internal interface IJournalNavigationScopeHost : INavigatorBase
	{
		// Token: 0x17001CB9 RID: 7353
		// (get) Token: 0x060079AB RID: 31147
		NavigationService NavigationService { get; }

		// Token: 0x060079AC RID: 31148
		void VerifyContextAndObjectState();

		// Token: 0x060079AD RID: 31149
		void OnJournalAvailable();

		// Token: 0x060079AE RID: 31150
		bool GoBackOverride();

		// Token: 0x060079AF RID: 31151
		bool GoForwardOverride();
	}
}
