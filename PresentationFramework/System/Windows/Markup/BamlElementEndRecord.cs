using System;

namespace System.Windows.Markup
{
	// Token: 0x02000204 RID: 516
	internal class BamlElementEndRecord : BamlRecord
	{
		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x06002058 RID: 8280 RVA: 0x00094BDC File Offset: 0x00092DDC
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ElementEnd;
			}
		}
	}
}
