using System;
using System.Diagnostics;
using System.Reflection;

namespace System.Windows.Diagnostics
{
	/// <summary>Represents information about a <see cref="T:System.Windows.ResourceDictionary" /> object.</summary>
	// Token: 0x02000196 RID: 406
	[DebuggerDisplay("Assembly = {Assembly?.GetName()?.Name}, ResourceDictionary SourceUri = {SourceUri?.AbsoluteUri}")]
	public class ResourceDictionaryInfo
	{
		// Token: 0x060017BE RID: 6078 RVA: 0x00073CD7 File Offset: 0x00071ED7
		internal ResourceDictionaryInfo(Assembly assembly, Assembly resourceDictionaryAssembly, ResourceDictionary resourceDictionary, Uri sourceUri)
		{
			this.Assembly = assembly;
			this.ResourceDictionaryAssembly = resourceDictionaryAssembly;
			this.ResourceDictionary = resourceDictionary;
			this.SourceUri = sourceUri;
		}

		/// <summary>Gets the assembly that uses the <see cref="T:System.Windows.ResourceDictionary" /> object referenced by the <see cref="P:System.Windows.Diagnostics.ResourceDictionaryInfo.ResourceDictionaryAssembly" /> property.</summary>
		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x060017BF RID: 6079 RVA: 0x00073CFC File Offset: 0x00071EFC
		// (set) Token: 0x060017C0 RID: 6080 RVA: 0x00073D04 File Offset: 0x00071F04
		public Assembly Assembly { get; private set; }

		/// <summary>Gets the name of the assembly from which the resource dictionary is loaded.</summary>
		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x00073D0D File Offset: 0x00071F0D
		// (set) Token: 0x060017C2 RID: 6082 RVA: 0x00073D15 File Offset: 0x00071F15
		public Assembly ResourceDictionaryAssembly { get; private set; }

		/// <summary>Gets the resource dictionary for which additional information is described by this <see cref="T:System.Windows.Diagnostics.ResourceDictionaryInfo" /> object instance.</summary>
		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x00073D1E File Offset: 0x00071F1E
		// (set) Token: 0x060017C4 RID: 6084 RVA: 0x00073D26 File Offset: 0x00071F26
		public ResourceDictionary ResourceDictionary { get; private set; }

		/// <summary>Gets the URI of the compiled BAML file embedded in the <see cref="P:System.Windows.Diagnostics.ResourceDictionaryInfo.ResourceDictionaryAssembly" /> property from which the resource dictionary is loaded.</summary>
		// Token: 0x17000556 RID: 1366
		// (get) Token: 0x060017C5 RID: 6085 RVA: 0x00073D2F File Offset: 0x00071F2F
		// (set) Token: 0x060017C6 RID: 6086 RVA: 0x00073D37 File Offset: 0x00071F37
		public Uri SourceUri { get; private set; }
	}
}
