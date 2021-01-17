using System;
using System.Windows.Media;

namespace MS.Internal.AppModel
{
	// Token: 0x02000782 RID: 1922
	internal interface INavigatorImpl
	{
		// Token: 0x0600793F RID: 31039
		void OnSourceUpdatedFromNavService(bool journalOrCancel);

		// Token: 0x06007940 RID: 31040
		Visual FindRootViewer();
	}
}
