using System;

namespace System.Windows.Markup
{
	// Token: 0x020001F3 RID: 499
	internal class BamlPropertyIListStartRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x000956F3 File Offset: 0x000938F3
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyIListStart;
			}
		}
	}
}
