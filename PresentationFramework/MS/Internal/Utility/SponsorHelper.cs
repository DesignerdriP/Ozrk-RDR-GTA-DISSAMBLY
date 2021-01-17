using System;
using System.Runtime.Remoting.Lifetime;
using System.Security;
using System.Security.Permissions;

namespace MS.Internal.Utility
{
	// Token: 0x020007EE RID: 2030
	[Serializable]
	internal class SponsorHelper : ISponsor
	{
		// Token: 0x06007D37 RID: 32055 RVA: 0x002330B3 File Offset: 0x002312B3
		internal SponsorHelper(ILease lease, TimeSpan timespan)
		{
			this._lease = lease;
			this._timespan = timespan;
		}

		// Token: 0x06007D38 RID: 32056 RVA: 0x002330C9 File Offset: 0x002312C9
		TimeSpan ISponsor.Renewal(ILease lease)
		{
			if (lease == null)
			{
				throw new ArgumentNullException("lease");
			}
			return this._timespan;
		}

		// Token: 0x06007D39 RID: 32057 RVA: 0x002330DF File Offset: 0x002312DF
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[SecurityPermission(SecurityAction.Assert, RemotingConfiguration = true)]
		internal void Register()
		{
			this._lease.Register(this);
		}

		// Token: 0x06007D3A RID: 32058 RVA: 0x002330ED File Offset: 0x002312ED
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[SecurityPermission(SecurityAction.Assert, RemotingConfiguration = true)]
		internal void Unregister()
		{
			this._lease.Unregister(this);
		}

		// Token: 0x04003AEA RID: 15082
		private ILease _lease;

		// Token: 0x04003AEB RID: 15083
		private TimeSpan _timespan;
	}
}
