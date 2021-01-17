using System;

namespace MS.Internal.Data
{
	// Token: 0x0200073C RID: 1852
	internal struct IndexerParamInfo
	{
		// Token: 0x06007622 RID: 30242 RVA: 0x0021AC09 File Offset: 0x00218E09
		public IndexerParamInfo(string paren, string value)
		{
			this.parenString = paren;
			this.valueString = value;
		}

		// Token: 0x04003864 RID: 14436
		public string parenString;

		// Token: 0x04003865 RID: 14437
		public string valueString;
	}
}
