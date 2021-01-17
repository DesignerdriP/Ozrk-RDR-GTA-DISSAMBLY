using System;
using System.IO;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;
using System.Xml;
using MS.Internal;
using MS.Internal.AppModel;

namespace System.Windows.Interop
{
	// Token: 0x020005BC RID: 1468
	internal class ApplicationLauncherXappDebug
	{
		// Token: 0x060061DC RID: 25052 RVA: 0x001B7054 File Offset: 0x001B5254
		[SecurityCritical]
		public ApplicationLauncherXappDebug(string path, string debugSecurityZoneURL)
		{
			this._deploymentManifestPath = path;
			this._deploymentManifest = new Uri(path);
			if (!string.IsNullOrEmpty(debugSecurityZoneURL))
			{
				this._debugSecurityZoneURL.Value = new Uri(debugSecurityZoneURL);
			}
			this._applicationManifestPath = Path.ChangeExtension(path, ".exe.manifest");
			this._exePath = Path.ChangeExtension(path, ".exe");
		}

		// Token: 0x060061DD RID: 25053 RVA: 0x001B70C4 File Offset: 0x001B52C4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public ApplicationProxyInternal Initialize()
		{
			SecurityHelper.DemandUIWindowPermission();
			this._context = ActivationContext.CreatePartialActivationContext(this.GetApplicationIdentity(), new string[]
			{
				this._deploymentManifestPath,
				this._applicationManifestPath
			});
			ApplicationTrust trust = new ApplicationTrust(this.GetApplicationIdentity());
			ApplicationSecurityManager.UserApplicationTrusts.Remove(trust);
			PresentationAppDomainManager.IsDebug = true;
			PresentationAppDomainManager.DebugSecurityZoneURL = this._debugSecurityZoneURL.Value;
			PresentationAppDomainManager.SaveAppDomain = true;
			ObjectHandle objectHandle = Activator.CreateInstance(this._context);
			if (PresentationAppDomainManager.SaveAppDomain)
			{
				AppDomain newAppDomain = objectHandle.Unwrap() as AppDomain;
				PresentationAppDomainManager.NewAppDomain = newAppDomain;
			}
			PresentationAppDomainManager presentationAppDomainManager = PresentationAppDomainManager.NewAppDomain.DomainManager as PresentationAppDomainManager;
			ApplicationProxyInternal applicationProxyInternal = presentationAppDomainManager.CreateApplicationProxyInternal();
			applicationProxyInternal.SetDebugSecurityZoneURL(this._debugSecurityZoneURL.Value);
			PresentationAppDomainManager.SaveAppDomain = false;
			return applicationProxyInternal;
		}

		// Token: 0x060061DE RID: 25054 RVA: 0x001B7188 File Offset: 0x001B5388
		private ApplicationIdentity GetApplicationIdentity()
		{
			return new ApplicationIdentity(string.Concat(new string[]
			{
				this._deploymentManifest.ToString(),
				"#",
				this.GetIdFromManifest(this._deploymentManifestPath),
				"/",
				this.GetIdFromManifest(this._applicationManifestPath)
			}));
		}

		// Token: 0x060061DF RID: 25055 RVA: 0x001B71E4 File Offset: 0x001B53E4
		private string GetIdFromManifest(string manifestName)
		{
			FileStream fileStream = new FileStream(manifestName, FileMode.Open, FileAccess.Read);
			try
			{
				using (XmlTextReader xmlTextReader = new XmlTextReader(fileStream))
				{
					xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
					while (xmlTextReader.Read())
					{
						if (xmlTextReader.NodeType == XmlNodeType.Element && !(xmlTextReader.NamespaceURI != "urn:schemas-microsoft-com:asm.v1") && xmlTextReader.LocalName == "assemblyIdentity")
						{
							string text = string.Empty;
							while (xmlTextReader.MoveToNextAttribute())
							{
								if (xmlTextReader.Name == "name")
								{
									text = xmlTextReader.Value + text;
								}
								else if (!(xmlTextReader.Name == "xmlns"))
								{
									text = string.Concat(new string[]
									{
										text,
										", ",
										xmlTextReader.Name,
										"=",
										xmlTextReader.Value
									});
								}
							}
							return text;
						}
					}
				}
			}
			finally
			{
				fileStream.Close();
			}
			return string.Empty;
		}

		// Token: 0x04003172 RID: 12658
		private string _deploymentManifestPath;

		// Token: 0x04003173 RID: 12659
		private Uri _deploymentManifest;

		// Token: 0x04003174 RID: 12660
		private string _applicationManifestPath;

		// Token: 0x04003175 RID: 12661
		private string _exePath;

		// Token: 0x04003176 RID: 12662
		private SecurityCriticalDataForSet<Uri> _debugSecurityZoneURL = new SecurityCriticalDataForSet<Uri>(null);

		// Token: 0x04003177 RID: 12663
		private ActivationContext _context;
	}
}
