using System;

namespace System.Windows.Diagnostics
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.GenericResourceDictionaryLoaded" /> and <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.ThemedResourceDictionaryLoaded" /> events.</summary>
	// Token: 0x02000194 RID: 404
	public class ResourceDictionaryLoadedEventArgs : EventArgs
	{
		// Token: 0x060017B8 RID: 6072 RVA: 0x00073C97 File Offset: 0x00071E97
		internal ResourceDictionaryLoadedEventArgs(ResourceDictionaryInfo resourceDictionaryInfo)
		{
			this.ResourceDictionaryInfo = resourceDictionaryInfo;
		}

		/// <summary>Gets data for the <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.GenericResourceDictionaryLoaded" /> and <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.ThemedResourceDictionaryLoaded" /> events .</summary>
		/// <returns>Data for the <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.GenericResourceDictionaryLoaded" /> and <see cref="E:System.Windows.Diagnostics.ResourceDictionaryDiagnostics.ThemedResourceDictionaryLoaded" /> events.</returns>
		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x060017B9 RID: 6073 RVA: 0x00073CA6 File Offset: 0x00071EA6
		// (set) Token: 0x060017BA RID: 6074 RVA: 0x00073CAE File Offset: 0x00071EAE
		public ResourceDictionaryInfo ResourceDictionaryInfo { get; private set; }
	}
}
