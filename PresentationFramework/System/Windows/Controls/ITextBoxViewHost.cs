using System;
using System.Windows.Documents;

namespace System.Windows.Controls
{
	// Token: 0x02000464 RID: 1124
	internal interface ITextBoxViewHost
	{
		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x0600412D RID: 16685
		ITextContainer TextContainer { get; }

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x0600412E RID: 16686
		bool IsTypographyDefaultValue { get; }
	}
}
