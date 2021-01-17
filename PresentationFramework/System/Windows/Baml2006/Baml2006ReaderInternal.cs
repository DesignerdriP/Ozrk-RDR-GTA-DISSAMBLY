using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xaml;
using MS.Internal;

namespace System.Windows.Baml2006
{
	// Token: 0x02000161 RID: 353
	internal class Baml2006ReaderInternal : Baml2006Reader
	{
		// Token: 0x0600103B RID: 4155 RVA: 0x0004135C File Offset: 0x0003F55C
		internal Baml2006ReaderInternal(Stream stream, Baml2006SchemaContext schemaContext, Baml2006ReaderSettings settings) : base(stream, schemaContext, settings)
		{
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x00041367 File Offset: 0x0003F567
		internal Baml2006ReaderInternal(Stream stream, Baml2006SchemaContext baml2006SchemaContext, Baml2006ReaderSettings baml2006ReaderSettings, object root) : base(stream, baml2006SchemaContext, baml2006ReaderSettings, root)
		{
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00041374 File Offset: 0x0003F574
		internal override string GetAssemblyNameForNamespace(Assembly asm)
		{
			return asm.FullName;
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x0004137C File Offset: 0x0003F57C
		internal override object CreateTypeConverterMarkupExtension(XamlMember property, TypeConverter converter, object propertyValue, Baml2006ReaderSettings settings)
		{
			if (FrameworkAppContextSwitches.AppendLocalAssemblyVersionForSourceUri && property.DeclaringType.UnderlyingType == typeof(ResourceDictionary) && property.Name.Equals("Source"))
			{
				return new SourceUriTypeConverterMarkupExtension(converter, propertyValue, settings.LocalAssembly);
			}
			return base.CreateTypeConverterMarkupExtension(property, converter, propertyValue, settings);
		}
	}
}
