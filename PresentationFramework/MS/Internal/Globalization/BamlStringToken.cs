using System;

namespace MS.Internal.Globalization
{
	// Token: 0x02000699 RID: 1689
	internal struct BamlStringToken
	{
		// Token: 0x06006E0D RID: 28173 RVA: 0x001FA83F File Offset: 0x001F8A3F
		internal BamlStringToken(BamlStringToken.TokenType type, string value)
		{
			this.Type = type;
			this.Value = value;
		}

		// Token: 0x04003627 RID: 13863
		internal readonly BamlStringToken.TokenType Type;

		// Token: 0x04003628 RID: 13864
		internal readonly string Value;

		// Token: 0x02000B26 RID: 2854
		internal enum TokenType
		{
			// Token: 0x04004A6F RID: 19055
			Text,
			// Token: 0x04004A70 RID: 19056
			ChildPlaceHolder
		}
	}
}
