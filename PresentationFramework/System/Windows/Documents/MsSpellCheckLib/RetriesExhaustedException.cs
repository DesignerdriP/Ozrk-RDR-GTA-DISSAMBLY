using System;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x02000461 RID: 1121
	internal class RetriesExhaustedException : Exception
	{
		// Token: 0x060040B0 RID: 16560 RVA: 0x00127777 File Offset: 0x00125977
		internal RetriesExhaustedException()
		{
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0012777F File Offset: 0x0012597F
		internal RetriesExhaustedException(string message) : base(message)
		{
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x00127788 File Offset: 0x00125988
		internal RetriesExhaustedException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
