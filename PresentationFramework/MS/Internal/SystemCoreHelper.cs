using System;

namespace MS.Internal
{
	// Token: 0x020005EE RID: 1518
	internal static class SystemCoreHelper
	{
		// Token: 0x06006531 RID: 25905 RVA: 0x001C679C File Offset: 0x001C499C
		internal static bool IsIDynamicMetaObjectProvider(object item)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			return systemCoreExtensionMethods != null && systemCoreExtensionMethods.IsIDynamicMetaObjectProvider(item);
		}

		// Token: 0x06006532 RID: 25906 RVA: 0x001C67BC File Offset: 0x001C49BC
		internal static object NewDynamicPropertyAccessor(Type ownerType, string propertyName)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			if (systemCoreExtensionMethods == null)
			{
				return null;
			}
			return systemCoreExtensionMethods.NewDynamicPropertyAccessor(ownerType, propertyName);
		}

		// Token: 0x06006533 RID: 25907 RVA: 0x001C67E0 File Offset: 0x001C49E0
		internal static object GetIndexerAccessor(int rank)
		{
			SystemCoreExtensionMethods systemCoreExtensionMethods = AssemblyHelper.ExtensionsForSystemCore(false);
			if (systemCoreExtensionMethods == null)
			{
				return null;
			}
			return systemCoreExtensionMethods.GetIndexerAccessor(rank);
		}
	}
}
