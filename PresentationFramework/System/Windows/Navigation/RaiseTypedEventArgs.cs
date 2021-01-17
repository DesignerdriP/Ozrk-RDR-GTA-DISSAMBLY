using System;

namespace System.Windows.Navigation
{
	// Token: 0x02000321 RID: 801
	internal class RaiseTypedEventArgs : EventArgs
	{
		// Token: 0x06002A73 RID: 10867 RVA: 0x000C26E9 File Offset: 0x000C08E9
		internal RaiseTypedEventArgs(Delegate d, object o)
		{
			this.D = d;
			this.O = o;
		}

		// Token: 0x04001C40 RID: 7232
		internal Delegate D;

		// Token: 0x04001C41 RID: 7233
		internal object O;
	}
}
