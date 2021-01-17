using System;
using System.ComponentModel;

namespace MS.Internal
{
	// Token: 0x020005F1 RID: 1521
	internal static class SystemXmlLinqHelper
	{
		// Token: 0x06006544 RID: 25924 RVA: 0x001C6A54 File Offset: 0x001C4C54
		internal static bool IsXElement(object item)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXElement(item);
		}

		// Token: 0x06006545 RID: 25925 RVA: 0x001C6A74 File Offset: 0x001C4C74
		internal static string GetXElementTagName(object item)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			if (systemXmlLinqExtensionMethods == null)
			{
				return null;
			}
			return systemXmlLinqExtensionMethods.GetXElementTagName(item);
		}

		// Token: 0x06006546 RID: 25926 RVA: 0x001C6A94 File Offset: 0x001C4C94
		internal static bool IsXLinqCollectionProperty(PropertyDescriptor pd)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXLinqCollectionProperty(pd);
		}

		// Token: 0x06006547 RID: 25927 RVA: 0x001C6AB4 File Offset: 0x001C4CB4
		internal static bool IsXLinqNonIdempotentProperty(PropertyDescriptor pd)
		{
			SystemXmlLinqExtensionMethods systemXmlLinqExtensionMethods = AssemblyHelper.ExtensionsForSystemXmlLinq(false);
			return systemXmlLinqExtensionMethods != null && systemXmlLinqExtensionMethods.IsXLinqNonIdempotentProperty(pd);
		}
	}
}
