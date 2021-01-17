using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using MS.Internal.Utility;
using MS.Utility;
using MS.Win32;

namespace System.Windows.Interop
{
	// Token: 0x020005B6 RID: 1462
	internal class PresentationHostSecurityManager : HostSecurityManager
	{
		// Token: 0x0600614D RID: 24909 RVA: 0x001B502A File Offset: 0x001B322A
		[SecurityCritical]
		internal PresentationHostSecurityManager()
		{
		}

		// Token: 0x0600614E RID: 24910 RVA: 0x001B5034 File Offset: 0x001B3234
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override ApplicationTrust DetermineApplicationTrust(Evidence applicationEvidence, Evidence activatorEvidence, TrustManagerContext context)
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_DetermineApplicationTrustStart);
			Uri uriFromActivationData = this.GetUriFromActivationData(0);
			bool flag = PresentationAppDomainManager.IsDebug || this.GetBoolFromActivationData(1);
			BrowserInteropHelper.SetBrowserHosted(true);
			ApplicationTrust applicationTrust;
			if (flag)
			{
				context.IgnorePersistedDecision = true;
				context.Persist = false;
				context.KeepAlive = false;
				context.NoPrompt = true;
				applicationTrust = base.DetermineApplicationTrust(applicationEvidence, activatorEvidence, context);
			}
			else
			{
				Zone hostEvidence = applicationEvidence.GetHostEvidence<Zone>();
				context.NoPrompt = (hostEvidence.SecurityZone != SecurityZone.Intranet && hostEvidence.SecurityZone != SecurityZone.Trusted);
				bool flag2 = !context.NoPrompt && PresentationHostSecurityManager.ElevationPromptOwnerWindow != IntPtr.Zero;
				if (flag2)
				{
					IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(null, PresentationHostSecurityManager.ElevationPromptOwnerWindow), 2);
					PresentationHostSecurityManager.SetFakeActiveWindow(ancestor);
					PresentationHostSecurityManager.ElevationPromptOwnerWindow = IntPtr.Zero;
				}
				try
				{
					applicationTrust = base.DetermineApplicationTrust(applicationEvidence, activatorEvidence, context);
				}
				finally
				{
					if (flag2)
					{
						PresentationHostSecurityManager.SetFakeActiveWindow((IntPtr)0);
					}
				}
			}
			if (applicationTrust != null)
			{
				PermissionSet permissionSet = applicationTrust.DefaultGrantSet.PermissionSet;
				if (flag)
				{
					Uri uriFromActivationData2 = this.GetUriFromActivationData(2);
					if (uriFromActivationData2 != null)
					{
						permissionSet = PresentationHostSecurityManager.AddPermissionForUri(permissionSet, uriFromActivationData2);
					}
				}
				if (permissionSet is ReadOnlyPermissionSet)
				{
					permissionSet = new PermissionSet(permissionSet);
				}
				applicationTrust.DefaultGrantSet.PermissionSet = permissionSet;
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordPerf | EventTrace.Keyword.KeywordHosting, EventTrace.Level.Verbose, EventTrace.Event.WpfHost_DetermineApplicationTrustEnd);
			return applicationTrust;
		}

		// Token: 0x0600614F RID: 24911
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationHost_v0400.dll")]
		private static extern void SetFakeActiveWindow(IntPtr hwnd);

		// Token: 0x06006150 RID: 24912 RVA: 0x001B5194 File Offset: 0x001B3394
		[SecurityCritical]
		internal static PermissionSet AddPermissionForUri(PermissionSet originalPermSet, Uri srcUri)
		{
			PermissionSet result = originalPermSet;
			if (srcUri != null)
			{
				Evidence evidence = new Evidence();
				evidence.AddHost(new Url(BindUriHelper.UriToString(srcUri)));
				IMembershipCondition membershipCondition = new UrlMembershipCondition(BindUriHelper.UriToString(srcUri));
				CodeGroup codeGroup = srcUri.IsFile ? new FileCodeGroup(membershipCondition, FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery) : new NetCodeGroup(membershipCondition);
				PolicyStatement policyStatement = codeGroup.Resolve(evidence);
				if (!policyStatement.PermissionSet.IsEmpty())
				{
					result = originalPermSet.Union(policyStatement.PermissionSet);
				}
			}
			return result;
		}

		// Token: 0x06006151 RID: 24913 RVA: 0x001B5210 File Offset: 0x001B3410
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private bool GetBoolFromActivationData(int index)
		{
			bool result = false;
			if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > index && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[index] == true.ToString())
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06006152 RID: 24914 RVA: 0x001B5274 File Offset: 0x001B3474
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private Uri GetUriFromActivationData(int index)
		{
			Uri result = null;
			if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > index && !string.IsNullOrEmpty(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[index]))
			{
				result = new UriBuilder(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[index]).Uri;
			}
			return result;
		}

		// Token: 0x0400314A RID: 12618
		[SecurityCritical]
		internal static IntPtr ElevationPromptOwnerWindow;
	}
}
