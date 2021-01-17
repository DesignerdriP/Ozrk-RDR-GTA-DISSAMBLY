using System;

namespace System.Windows.Markup
{
	// Token: 0x02000206 RID: 518
	internal class BamlKeyElementEndRecord : BamlElementEndRecord
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x000960B8 File Offset: 0x000942B8
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.KeyElementEnd;
			}
		}
	}
}
