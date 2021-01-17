using System;

namespace System.Windows.Markup
{
	// Token: 0x02000200 RID: 512
	internal class BamlTextRecord : BamlStringValueRecord
	{
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002039 RID: 8249 RVA: 0x00095E8B File Offset: 0x0009408B
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.Text;
			}
		}
	}
}
