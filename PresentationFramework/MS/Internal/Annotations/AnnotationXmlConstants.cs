using System;

namespace MS.Internal.Annotations
{
	// Token: 0x020007C6 RID: 1990
	internal struct AnnotationXmlConstants
	{
		// Token: 0x02000B78 RID: 2936
		internal struct Namespaces
		{
			// Token: 0x04004B5E RID: 19294
			public const string CoreSchemaNamespace = "http://schemas.microsoft.com/windows/annotations/2003/11/core";

			// Token: 0x04004B5F RID: 19295
			public const string BaseSchemaNamespace = "http://schemas.microsoft.com/windows/annotations/2003/11/base";
		}

		// Token: 0x02000B79 RID: 2937
		internal struct Prefixes
		{
			// Token: 0x04004B60 RID: 19296
			internal const string XmlPrefix = "xml";

			// Token: 0x04004B61 RID: 19297
			internal const string XmlnsPrefix = "xmlns";

			// Token: 0x04004B62 RID: 19298
			internal const string CoreSchemaPrefix = "anc";

			// Token: 0x04004B63 RID: 19299
			internal const string BaseSchemaPrefix = "anb";
		}

		// Token: 0x02000B7A RID: 2938
		internal struct Elements
		{
			// Token: 0x04004B64 RID: 19300
			internal const string Annotation = "Annotation";

			// Token: 0x04004B65 RID: 19301
			internal const string Resource = "Resource";

			// Token: 0x04004B66 RID: 19302
			internal const string ContentLocator = "ContentLocator";

			// Token: 0x04004B67 RID: 19303
			internal const string ContentLocatorGroup = "ContentLocatorGroup";

			// Token: 0x04004B68 RID: 19304
			internal const string AuthorCollection = "Authors";

			// Token: 0x04004B69 RID: 19305
			internal const string AnchorCollection = "Anchors";

			// Token: 0x04004B6A RID: 19306
			internal const string CargoCollection = "Cargos";

			// Token: 0x04004B6B RID: 19307
			internal const string Item = "Item";

			// Token: 0x04004B6C RID: 19308
			internal const string StringAuthor = "StringAuthor";
		}

		// Token: 0x02000B7B RID: 2939
		internal struct Attributes
		{
			// Token: 0x04004B6D RID: 19309
			internal const string Id = "Id";

			// Token: 0x04004B6E RID: 19310
			internal const string CreationTime = "CreationTime";

			// Token: 0x04004B6F RID: 19311
			internal const string LastModificationTime = "LastModificationTime";

			// Token: 0x04004B70 RID: 19312
			internal const string TypeName = "Type";

			// Token: 0x04004B71 RID: 19313
			internal const string ResourceName = "Name";

			// Token: 0x04004B72 RID: 19314
			internal const string ItemName = "Name";

			// Token: 0x04004B73 RID: 19315
			internal const string ItemValue = "Value";
		}
	}
}
