using System;
using System.Runtime.Hosting;
using System.Security;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Utility;

namespace System.Windows.Interop
{
	// Token: 0x020005B8 RID: 1464
	internal class PresentationAppDomainManager : AppDomainManager
	{
		// Token: 0x06006155 RID: 24917 RVA: 0x001B5420 File Offset: 0x001B3620
		[SecurityCritical]
		[SecurityTreatAsSafe]
		static PresentationAppDomainManager()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_AppDomainManagerCctor);
		}

		// Token: 0x06006156 RID: 24918 RVA: 0x001B5432 File Offset: 0x001B3632
		[SecurityCritical]
		public PresentationAppDomainManager()
		{
		}

		// Token: 0x1700176C RID: 5996
		// (get) Token: 0x06006157 RID: 24919 RVA: 0x001B543A File Offset: 0x001B363A
		public override ApplicationActivator ApplicationActivator
		{
			get
			{
				if (this._appActivator == null)
				{
					this._appActivator = new PresentationApplicationActivator();
				}
				return this._appActivator;
			}
		}

		// Token: 0x06006158 RID: 24920 RVA: 0x001B5455 File Offset: 0x001B3655
		[SecurityCritical]
		public override void InitializeNewDomain(AppDomainSetup appDomainInfo)
		{
			this._assemblyFilter = new AssemblyFilter();
			AppDomain.CurrentDomain.AssemblyLoad += this._assemblyFilter.FilterCallback;
		}

		// Token: 0x1700176D RID: 5997
		// (get) Token: 0x06006159 RID: 24921 RVA: 0x001B547D File Offset: 0x001B367D
		public override HostSecurityManager HostSecurityManager
		{
			[SecurityCritical]
			get
			{
				if (this._hostsecuritymanager == null)
				{
					this._hostsecuritymanager = new PresentationHostSecurityManager();
				}
				return this._hostsecuritymanager;
			}
		}

		// Token: 0x0600615A RID: 24922 RVA: 0x001B5498 File Offset: 0x001B3698
		[SecurityCritical]
		internal ApplicationProxyInternal CreateApplicationProxyInternal()
		{
			return new ApplicationProxyInternal();
		}

		// Token: 0x1700176E RID: 5998
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x001B549F File Offset: 0x001B369F
		// (set) Token: 0x0600615C RID: 24924 RVA: 0x001B54A6 File Offset: 0x001B36A6
		internal static AppDomain NewAppDomain
		{
			get
			{
				return PresentationAppDomainManager._newAppDomain;
			}
			set
			{
				PresentationAppDomainManager._newAppDomain = value;
			}
		}

		// Token: 0x1700176F RID: 5999
		// (get) Token: 0x0600615D RID: 24925 RVA: 0x001B54AE File Offset: 0x001B36AE
		// (set) Token: 0x0600615E RID: 24926 RVA: 0x001B54B5 File Offset: 0x001B36B5
		internal static bool SaveAppDomain
		{
			get
			{
				return PresentationAppDomainManager._saveAppDomain;
			}
			set
			{
				PresentationAppDomainManager._saveAppDomain = value;
				PresentationAppDomainManager._newAppDomain = null;
			}
		}

		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x0600615F RID: 24927 RVA: 0x001B54C3 File Offset: 0x001B36C3
		// (set) Token: 0x06006160 RID: 24928 RVA: 0x001B54CA File Offset: 0x001B36CA
		internal static Uri ActivationUri
		{
			get
			{
				return PresentationAppDomainManager._activationUri;
			}
			set
			{
				PresentationAppDomainManager._activationUri = value;
			}
		}

		// Token: 0x17001771 RID: 6001
		// (get) Token: 0x06006161 RID: 24929 RVA: 0x001B54D2 File Offset: 0x001B36D2
		// (set) Token: 0x06006162 RID: 24930 RVA: 0x001B54D9 File Offset: 0x001B36D9
		internal static Uri DebugSecurityZoneURL
		{
			get
			{
				return PresentationAppDomainManager._debugSecurityZoneURL;
			}
			set
			{
				PresentationAppDomainManager._debugSecurityZoneURL = value;
			}
		}

		// Token: 0x17001772 RID: 6002
		// (get) Token: 0x06006163 RID: 24931 RVA: 0x001B54E1 File Offset: 0x001B36E1
		// (set) Token: 0x06006164 RID: 24932 RVA: 0x001B54E8 File Offset: 0x001B36E8
		internal static bool IsDebug
		{
			get
			{
				return PresentationAppDomainManager._isdebug;
			}
			set
			{
				PresentationAppDomainManager._isdebug = value;
			}
		}

		// Token: 0x0400314B RID: 12619
		private static bool _isdebug;

		// Token: 0x0400314C RID: 12620
		private ApplicationActivator _appActivator;

		// Token: 0x0400314D RID: 12621
		[SecurityCritical]
		private HostSecurityManager _hostsecuritymanager;

		// Token: 0x0400314E RID: 12622
		private static AppDomain _newAppDomain;

		// Token: 0x0400314F RID: 12623
		private static bool _saveAppDomain;

		// Token: 0x04003150 RID: 12624
		private static Uri _activationUri;

		// Token: 0x04003151 RID: 12625
		private static Uri _debugSecurityZoneURL;

		// Token: 0x04003152 RID: 12626
		[SecurityCritical]
		private AssemblyFilter _assemblyFilter;
	}
}
