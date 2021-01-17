using System;
using System.Reflection;
using MS.Internal.WindowsBase;

namespace System.Windows.Baml2006
{
	// Token: 0x02000167 RID: 359
	internal class Baml6Assembly
	{
		// Token: 0x06001073 RID: 4211 RVA: 0x00041767 File Offset: 0x0003F967
		public Baml6Assembly(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.Name = name;
			this._assembly = null;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x0004178B File Offset: 0x0003F98B
		public Baml6Assembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.Name = null;
			this._assembly = assembly;
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001075 RID: 4213 RVA: 0x000417B8 File Offset: 0x0003F9B8
		public Assembly Assembly
		{
			get
			{
				if (this._assembly != null)
				{
					return this._assembly;
				}
				AssemblyName assemblyName = new AssemblyName(this.Name);
				this._assembly = SafeSecurityHelper.GetLoadedAssembly(assemblyName);
				if (this._assembly == null)
				{
					byte[] publicKeyToken = assemblyName.GetPublicKeyToken();
					if (assemblyName.Version != null || assemblyName.CultureInfo != null || publicKeyToken != null)
					{
						try
						{
							this._assembly = Assembly.Load(assemblyName.FullName);
							goto IL_A5;
						}
						catch
						{
							AssemblyName assemblyName2 = new AssemblyName(assemblyName.Name);
							if (publicKeyToken != null)
							{
								assemblyName2.SetPublicKeyToken(publicKeyToken);
							}
							this._assembly = Assembly.Load(assemblyName2);
							goto IL_A5;
						}
					}
					this._assembly = Assembly.LoadWithPartialName(assemblyName.Name);
				}
				IL_A5:
				return this._assembly;
			}
		}

		// Token: 0x04001229 RID: 4649
		public readonly string Name;

		// Token: 0x0400122A RID: 4650
		private Assembly _assembly;
	}
}
