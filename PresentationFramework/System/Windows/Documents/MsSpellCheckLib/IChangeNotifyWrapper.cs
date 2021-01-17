using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045C RID: 1116
	internal interface IChangeNotifyWrapper : INotifyPropertyChanged
	{
		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x0600409C RID: 16540
		// (set) Token: 0x0600409D RID: 16541
		object Value { get; set; }
	}
}
