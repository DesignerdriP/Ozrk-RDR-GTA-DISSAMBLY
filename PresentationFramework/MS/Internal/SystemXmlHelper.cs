using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace MS.Internal
{
	// Token: 0x020005F0 RID: 1520
	internal static class SystemXmlHelper
	{
		// Token: 0x0600653C RID: 25916 RVA: 0x001C6938 File Offset: 0x001C4B38
		internal static bool IsXmlNode(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsXmlNode(item);
		}

		// Token: 0x0600653D RID: 25917 RVA: 0x001C6958 File Offset: 0x001C4B58
		internal static bool IsXmlNamespaceManager(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsXmlNamespaceManager(item);
		}

		// Token: 0x0600653E RID: 25918 RVA: 0x001C6978 File Offset: 0x001C4B78
		internal static bool TryGetValueFromXmlNode(object item, string name, out object value)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods != null)
			{
				return systemXmlExtensionMethods.TryGetValueFromXmlNode(item, name, out value);
			}
			value = null;
			return false;
		}

		// Token: 0x0600653F RID: 25919 RVA: 0x001C69A0 File Offset: 0x001C4BA0
		internal static IComparer PrepareXmlComparer(IEnumerable collection, SortDescriptionCollection sort, CultureInfo culture)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods != null)
			{
				return systemXmlExtensionMethods.PrepareXmlComparer(collection, sort, culture);
			}
			return null;
		}

		// Token: 0x06006540 RID: 25920 RVA: 0x001C69C4 File Offset: 0x001C4BC4
		internal static bool IsEmptyXmlDataCollection(object parent)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			return systemXmlExtensionMethods != null && systemXmlExtensionMethods.IsEmptyXmlDataCollection(parent);
		}

		// Token: 0x06006541 RID: 25921 RVA: 0x001C69E4 File Offset: 0x001C4BE4
		internal static string GetXmlTagName(object item, DependencyObject target)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return null;
			}
			return systemXmlExtensionMethods.GetXmlTagName(item, target);
		}

		// Token: 0x06006542 RID: 25922 RVA: 0x001C6A08 File Offset: 0x001C4C08
		internal static object FindXmlNodeWithInnerText(IEnumerable items, object innerText, out int index)
		{
			index = -1;
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return DependencyProperty.UnsetValue;
			}
			return systemXmlExtensionMethods.FindXmlNodeWithInnerText(items, innerText, out index);
		}

		// Token: 0x06006543 RID: 25923 RVA: 0x001C6A34 File Offset: 0x001C4C34
		internal static object GetInnerText(object item)
		{
			SystemXmlExtensionMethods systemXmlExtensionMethods = AssemblyHelper.ExtensionsForSystemXml(false);
			if (systemXmlExtensionMethods == null)
			{
				return null;
			}
			return systemXmlExtensionMethods.GetInnerText(item);
		}
	}
}
