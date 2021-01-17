using System;

namespace System.Windows.Documents
{
	// Token: 0x02000351 RID: 849
	internal interface IFixedNavigate
	{
		// Token: 0x06002D41 RID: 11585
		UIElement FindElementByID(string elementID, out FixedPage rootFixedPage);

		// Token: 0x06002D42 RID: 11586
		void NavigateAsync(string elementID);
	}
}
