using System;
using System.ComponentModel;

namespace System.Windows.Documents
{
	// Token: 0x0200038A RID: 906
	internal class BringPositionIntoViewCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x0600317A RID: 12666 RVA: 0x000DB811 File Offset: 0x000D9A11
		public BringPositionIntoViewCompletedEventArgs(ITextPointer position, bool succeeded, Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
		{
		}
	}
}
