using System;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000780 RID: 1920
	internal interface INavigatorBase
	{
		// Token: 0x17001C9F RID: 7327
		// (get) Token: 0x0600791D RID: 31005
		// (set) Token: 0x0600791E RID: 31006
		Uri Source { get; set; }

		// Token: 0x17001CA0 RID: 7328
		// (get) Token: 0x0600791F RID: 31007
		Uri CurrentSource { get; }

		// Token: 0x17001CA1 RID: 7329
		// (get) Token: 0x06007920 RID: 31008
		// (set) Token: 0x06007921 RID: 31009
		object Content { get; set; }

		// Token: 0x06007922 RID: 31010
		bool Navigate(Uri source);

		// Token: 0x06007923 RID: 31011
		bool Navigate(Uri source, object extraData);

		// Token: 0x06007924 RID: 31012
		bool Navigate(object content);

		// Token: 0x06007925 RID: 31013
		bool Navigate(object content, object extraData);

		// Token: 0x06007926 RID: 31014
		void StopLoading();

		// Token: 0x06007927 RID: 31015
		void Refresh();

		// Token: 0x1400015E RID: 350
		// (add) Token: 0x06007928 RID: 31016
		// (remove) Token: 0x06007929 RID: 31017
		event NavigatingCancelEventHandler Navigating;

		// Token: 0x1400015F RID: 351
		// (add) Token: 0x0600792A RID: 31018
		// (remove) Token: 0x0600792B RID: 31019
		event NavigationProgressEventHandler NavigationProgress;

		// Token: 0x14000160 RID: 352
		// (add) Token: 0x0600792C RID: 31020
		// (remove) Token: 0x0600792D RID: 31021
		event NavigationFailedEventHandler NavigationFailed;

		// Token: 0x14000161 RID: 353
		// (add) Token: 0x0600792E RID: 31022
		// (remove) Token: 0x0600792F RID: 31023
		event NavigatedEventHandler Navigated;

		// Token: 0x14000162 RID: 354
		// (add) Token: 0x06007930 RID: 31024
		// (remove) Token: 0x06007931 RID: 31025
		event LoadCompletedEventHandler LoadCompleted;

		// Token: 0x14000163 RID: 355
		// (add) Token: 0x06007932 RID: 31026
		// (remove) Token: 0x06007933 RID: 31027
		event NavigationStoppedEventHandler NavigationStopped;

		// Token: 0x14000164 RID: 356
		// (add) Token: 0x06007934 RID: 31028
		// (remove) Token: 0x06007935 RID: 31029
		event FragmentNavigationEventHandler FragmentNavigation;
	}
}
