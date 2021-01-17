using System;
using System.Security;
using System.Security.Permissions;

namespace MS.Internal.AppModel
{
	// Token: 0x0200077E RID: 1918
	internal interface IHostService
	{
		// Token: 0x17001C9C RID: 7324
		// (get) Token: 0x06007918 RID: 31000
		RootBrowserWindowProxy RootBrowserWindowProxy { get; }

		// Token: 0x17001C9D RID: 7325
		// (get) Token: 0x06007919 RID: 31001
		IntPtr HostWindowHandle { [SecurityCritical] [UIPermission(SecurityAction.InheritanceDemand, Unrestricted = true)] get; }
	}
}
