using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.Ink
{
	// Token: 0x02000696 RID: 1686
	internal class XamlClipboardData : ElementsClipboardData
	{
		// Token: 0x06006E02 RID: 28162 RVA: 0x001FA438 File Offset: 0x001F8638
		internal XamlClipboardData()
		{
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x001FA440 File Offset: 0x001F8640
		internal XamlClipboardData(UIElement[] elements) : base(elements)
		{
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x001FA44C File Offset: 0x001F864C
		internal override bool CanPaste(IDataObject dataObject)
		{
			bool result = false;
			try
			{
				result = dataObject.GetDataPresent(DataFormats.Xaml, false);
			}
			catch (SecurityException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x001FA480 File Offset: 0x001F8680
		protected override bool CanCopy()
		{
			return base.Elements != null && base.Elements.Count != 0;
		}

		// Token: 0x06006E06 RID: 28166 RVA: 0x001FA49C File Offset: 0x001F869C
		[SecurityCritical]
		protected override void DoCopy(IDataObject dataObject)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (UIElement obj in base.Elements)
			{
				string value = XamlWriter.Save(obj);
				stringBuilder.Append(value);
			}
			dataObject.SetData(DataFormats.Xaml, stringBuilder.ToString());
			PermissionSet permissionSet = SecurityHelper.ExtractAppDomainPermissionSetMinusSiteOfOrigin();
			string data = permissionSet.ToString();
			dataObject.SetData(DataFormats.ApplicationTrust, data);
		}

		// Token: 0x06006E07 RID: 28167 RVA: 0x001FA52C File Offset: 0x001F872C
		protected override void DoPaste(IDataObject dataObject)
		{
			base.ElementList = new List<UIElement>();
			string text = dataObject.GetData(DataFormats.Xaml) as string;
			if (!string.IsNullOrEmpty(text))
			{
				bool useRestrictiveXamlReader = !Clipboard.UseLegacyDangerousClipboardDeserializationMode();
				UIElement uielement = XamlReader.Load(new XmlTextReader(new StringReader(text)), useRestrictiveXamlReader) as UIElement;
				if (uielement != null)
				{
					base.ElementList.Add(uielement);
				}
			}
		}
	}
}
