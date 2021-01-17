using System;

namespace System.Windows.Markup
{
	// Token: 0x02000207 RID: 519
	internal class BamlDocumentEndRecord : BamlRecord
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x00094B24 File Offset: 0x00092D24
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DocumentEnd;
			}
		}
	}
}
