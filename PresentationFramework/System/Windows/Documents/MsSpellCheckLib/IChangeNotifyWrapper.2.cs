using System;
using System.ComponentModel;

namespace System.Windows.Documents.MsSpellCheckLib
{
	// Token: 0x0200045D RID: 1117
	internal interface IChangeNotifyWrapper<T> : IChangeNotifyWrapper, INotifyPropertyChanged
	{
		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x0600409E RID: 16542
		// (set) Token: 0x0600409F RID: 16543
		T Value { get; set; }
	}
}
