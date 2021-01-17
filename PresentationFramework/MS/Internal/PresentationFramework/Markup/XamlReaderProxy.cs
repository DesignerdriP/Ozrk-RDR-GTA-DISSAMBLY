using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace MS.Internal.PresentationFramework.Markup
{
	// Token: 0x02000803 RID: 2051
	internal class XamlReaderProxy
	{
		// Token: 0x06007DE7 RID: 32231 RVA: 0x00234B98 File Offset: 0x00232D98
		static XamlReaderProxy()
		{
			MethodInfo method = XamlReaderProxy._xamlReaderType.GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
			{
				typeof(Stream),
				typeof(ParserContext),
				typeof(bool)
			}, null);
			if (method != null)
			{
				XamlReaderProxy._xamlLoad3 = (XamlReaderProxy.XamlLoadDelegate3)method.CreateDelegate(typeof(XamlReaderProxy.XamlLoadDelegate3));
			}
			method = XamlReaderProxy._xamlReaderType.GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[]
			{
				typeof(XmlReader),
				typeof(bool)
			}, null);
			if (method != null)
			{
				XamlReaderProxy._xamlLoad2 = (XamlReaderProxy.XamlLoadDelegate2)method.CreateDelegate(typeof(XamlReaderProxy.XamlLoadDelegate2));
			}
		}

		// Token: 0x06007DE8 RID: 32232 RVA: 0x00234C6F File Offset: 0x00232E6F
		public static object Load(Stream stream, ParserContext parserContext, bool useRestrictiveXamlReader)
		{
			if (XamlReaderProxy._xamlLoad3 != null)
			{
				return XamlReaderProxy._xamlLoad3(stream, parserContext, XamlReaderProxy.DisableLegacyDangerousXamlDeserializationMode && useRestrictiveXamlReader);
			}
			return XamlReader.Load(stream, parserContext);
		}

		// Token: 0x06007DE9 RID: 32233 RVA: 0x00234C93 File Offset: 0x00232E93
		public static object Load(XmlReader reader, bool useRestrictiveXamlReader)
		{
			if (XamlReaderProxy._xamlLoad2 != null)
			{
				return XamlReaderProxy._xamlLoad2(reader, XamlReaderProxy.DisableLegacyDangerousXamlDeserializationMode && useRestrictiveXamlReader);
			}
			return XamlReader.Load(reader);
		}

		// Token: 0x17001D41 RID: 7489
		// (get) Token: 0x06007DEA RID: 32234 RVA: 0x00234CB5 File Offset: 0x00232EB5
		private static bool DisableLegacyDangerousXamlDeserializationMode
		{
			get
			{
				return FrameworkCompatibilityPreferences.DisableLegacyDangerousXamlDeserializationMode;
			}
		}

		// Token: 0x04003B70 RID: 15216
		private static XamlReaderProxy.XamlLoadDelegate3 _xamlLoad3;

		// Token: 0x04003B71 RID: 15217
		private static XamlReaderProxy.XamlLoadDelegate2 _xamlLoad2;

		// Token: 0x04003B72 RID: 15218
		private static readonly Type _xamlReaderType = typeof(XamlReader);

		// Token: 0x04003B73 RID: 15219
		private const string XamlLoadMethodName = "Load";

		// Token: 0x02000B8B RID: 2955
		// (Invoke) Token: 0x06008E74 RID: 36468
		private delegate object XamlLoadDelegate3(Stream stream, ParserContext parserContext, bool useRestrictiveXamlReader);

		// Token: 0x02000B8C RID: 2956
		// (Invoke) Token: 0x06008E78 RID: 36472
		private delegate object XamlLoadDelegate2(XmlReader reader, bool useRestrictiveXamlReader);
	}
}
