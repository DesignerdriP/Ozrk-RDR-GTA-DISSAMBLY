using System;
using System.ComponentModel;
using System.Reflection;

namespace MS.Internal
{
	// Token: 0x020005EF RID: 1519
	internal static class SystemDataHelper
	{
		// Token: 0x06006534 RID: 25908 RVA: 0x001C6800 File Offset: 0x001C4A00
		internal static bool IsDataView(IBindingList list)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataView(list);
		}

		// Token: 0x06006535 RID: 25909 RVA: 0x001C6820 File Offset: 0x001C4A20
		internal static bool IsDataRowView(object item)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataRowView(item);
		}

		// Token: 0x06006536 RID: 25910 RVA: 0x001C6840 File Offset: 0x001C4A40
		internal static bool IsSqlNull(object value)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsSqlNull(value);
		}

		// Token: 0x06006537 RID: 25911 RVA: 0x001C6860 File Offset: 0x001C4A60
		internal static bool IsSqlNullableType(Type type)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsSqlNullableType(type);
		}

		// Token: 0x06006538 RID: 25912 RVA: 0x001C6880 File Offset: 0x001C4A80
		internal static bool IsDataSetCollectionProperty(PropertyDescriptor pd)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.IsDataSetCollectionProperty(pd);
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x001C68A0 File Offset: 0x001C4AA0
		internal static object GetValue(object item, PropertyDescriptor pd, bool useFollowParent)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			if (systemDataExtensionMethods == null)
			{
				return null;
			}
			return systemDataExtensionMethods.GetValue(item, pd, useFollowParent);
		}

		// Token: 0x0600653A RID: 25914 RVA: 0x001C68C4 File Offset: 0x001C4AC4
		internal static bool DetermineWhetherDBNullIsValid(object item, string columnName, object arg)
		{
			SystemDataExtensionMethods systemDataExtensionMethods = AssemblyHelper.ExtensionsForSystemData(false);
			return systemDataExtensionMethods != null && systemDataExtensionMethods.DetermineWhetherDBNullIsValid(item, columnName, arg);
		}

		// Token: 0x0600653B RID: 25915 RVA: 0x001C68E8 File Offset: 0x001C4AE8
		internal static object NullValueForSqlNullableType(Type type)
		{
			FieldInfo field = type.GetField("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (field != null)
			{
				return field.GetValue(null);
			}
			PropertyInfo property = type.GetProperty("Null", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (property != null)
			{
				return property.GetValue(null, null);
			}
			return null;
		}
	}
}
