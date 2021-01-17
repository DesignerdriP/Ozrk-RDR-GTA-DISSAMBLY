using System;
using System.Security;
using System.Security.Permissions;

namespace MS.Internal.Permissions
{
	// Token: 0x020005FC RID: 1532
	[Serializable]
	internal class UserInitiatedNavigationPermission : InternalParameterlessPermissionBase
	{
		// Token: 0x060065FC RID: 26108 RVA: 0x001CAA58 File Offset: 0x001C8C58
		public UserInitiatedNavigationPermission() : this(PermissionState.Unrestricted)
		{
		}

		// Token: 0x060065FD RID: 26109 RVA: 0x001CAA61 File Offset: 0x001C8C61
		public UserInitiatedNavigationPermission(PermissionState state) : base(state)
		{
		}

		// Token: 0x060065FE RID: 26110 RVA: 0x001CAA6A File Offset: 0x001C8C6A
		public override IPermission Copy()
		{
			return new UserInitiatedNavigationPermission();
		}
	}
}
