using System;
using System.IO;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x02000349 RID: 841
	internal sealed class XpsS0ResourceDictionarySchema : XpsS0Schema
	{
		// Token: 0x06002D0B RID: 11531 RVA: 0x000CB4B8 File Offset: 0x000C96B8
		public XpsS0ResourceDictionarySchema()
		{
			XpsSchema.RegisterSchema(this, new ContentType[]
			{
				XpsS0Schema._resourceDictionaryContentType
			});
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000CB4D4 File Offset: 0x000C96D4
		public override string[] ExtractUriFromAttr(string attrName, string attrValue)
		{
			if (attrName.Equals("Source", StringComparison.Ordinal))
			{
				throw new FileFormatException(SR.Get("XpsValidatingLoaderUnsupportedMimeType"));
			}
			return base.ExtractUriFromAttr(attrName, attrValue);
		}
	}
}
