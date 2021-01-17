using System;

namespace System.Windows.Markup
{
	// Token: 0x02000220 RID: 544
	internal static class XmlParserDefaults
	{
		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060021A5 RID: 8613 RVA: 0x000A7B92 File Offset: 0x000A5D92
		internal static XamlTypeMapper DefaultMapper
		{
			get
			{
				return new XamlTypeMapper(XmlParserDefaults.GetDefaultAssemblyNames(), XmlParserDefaults.GetDefaultNamespaceMaps());
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000A7BA3 File Offset: 0x000A5DA3
		internal static string[] GetDefaultAssemblyNames()
		{
			return (string[])XmlParserDefaults._defaultAssemblies.Clone();
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000A7BB4 File Offset: 0x000A5DB4
		internal static NamespaceMapEntry[] GetDefaultNamespaceMaps()
		{
			return (NamespaceMapEntry[])XmlParserDefaults._defaultNamespaceMapTable.Clone();
		}

		// Token: 0x040019AD RID: 6573
		private static readonly string[] _defaultAssemblies = new string[]
		{
			"WindowsBase",
			"PresentationCore",
			"PresentationFramework"
		};

		// Token: 0x040019AE RID: 6574
		private static readonly NamespaceMapEntry[] _defaultNamespaceMapTable = new NamespaceMapEntry[0];
	}
}
