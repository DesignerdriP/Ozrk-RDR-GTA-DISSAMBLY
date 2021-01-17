using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Utility;

namespace MS.Internal.AppModel
{
	// Token: 0x02000797 RID: 1943
	internal sealed class RequestSetStatusBarEventArgs : RoutedEventArgs
	{
		// Token: 0x060079D4 RID: 31188 RVA: 0x0022860B File Offset: 0x0022680B
		[SecurityCritical]
		internal RequestSetStatusBarEventArgs(string text)
		{
			this._text.Value = text;
			base.RoutedEvent = Hyperlink.RequestSetStatusBarEvent;
		}

		// Token: 0x060079D5 RID: 31189 RVA: 0x0022862A File Offset: 0x0022682A
		[SecurityCritical]
		internal RequestSetStatusBarEventArgs(Uri targetUri)
		{
			if (targetUri == null)
			{
				this._text.Value = string.Empty;
			}
			else
			{
				this._text.Value = BindUriHelper.UriToString(targetUri);
			}
			base.RoutedEvent = Hyperlink.RequestSetStatusBarEvent;
		}

		// Token: 0x17001CC0 RID: 7360
		// (get) Token: 0x060079D6 RID: 31190 RVA: 0x00228669 File Offset: 0x00226869
		internal string Text
		{
			get
			{
				return this._text.Value;
			}
		}

		// Token: 0x17001CC1 RID: 7361
		// (get) Token: 0x060079D7 RID: 31191 RVA: 0x00228676 File Offset: 0x00226876
		internal static RequestSetStatusBarEventArgs Clear
		{
			[SecurityCritical]
			[SecurityTreatAsSafe]
			get
			{
				return new RequestSetStatusBarEventArgs(string.Empty);
			}
		}

		// Token: 0x0400399E RID: 14750
		private SecurityCriticalDataForSet<string> _text;
	}
}
