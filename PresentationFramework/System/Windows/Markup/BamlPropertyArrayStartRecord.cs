using System;

namespace System.Windows.Markup
{
	// Token: 0x020001F2 RID: 498
	internal class BamlPropertyArrayStartRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x000956EF File Offset: 0x000938EF
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyArrayStart;
			}
		}
	}
}
