using System;

namespace System.Windows.Markup
{
	// Token: 0x020001FC RID: 508
	internal class BamlStaticResourceEndRecord : BamlElementEndRecord
	{
		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06002012 RID: 8210 RVA: 0x00095B80 File Offset: 0x00093D80
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceEnd;
			}
		}
	}
}
