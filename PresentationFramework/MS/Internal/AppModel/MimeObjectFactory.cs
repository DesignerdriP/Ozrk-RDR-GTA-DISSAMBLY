using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x020007A0 RID: 1952
	internal static class MimeObjectFactory
	{
		// Token: 0x06007A60 RID: 31328 RVA: 0x0022ACB0 File Offset: 0x00228EB0
		internal static object GetObjectAndCloseStream(Stream s, ContentType contentType, Uri baseUri, bool canUseTopLevelBrowser, bool sandboxExternalContent, bool allowAsync, bool isJournalNavigation, out XamlReader asyncObjectConverter)
		{
			object result = null;
			asyncObjectConverter = null;
			StreamToObjectFactoryDelegate streamToObjectFactoryDelegate;
			if (contentType != null && MimeObjectFactory._objectConverters.TryGetValue(contentType, out streamToObjectFactoryDelegate))
			{
				result = streamToObjectFactoryDelegate(s, baseUri, canUseTopLevelBrowser, sandboxExternalContent, allowAsync, isJournalNavigation, out asyncObjectConverter);
			}
			return result;
		}

		// Token: 0x06007A61 RID: 31329 RVA: 0x0022ACE8 File Offset: 0x00228EE8
		internal static void Register(ContentType contentType, StreamToObjectFactoryDelegate method)
		{
			MimeObjectFactory._objectConverters[contentType] = method;
		}

		// Token: 0x040039DE RID: 14814
		private static readonly Dictionary<ContentType, StreamToObjectFactoryDelegate> _objectConverters = new Dictionary<ContentType, StreamToObjectFactoryDelegate>(5, new ContentType.WeakComparer());
	}
}
