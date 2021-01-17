using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003C3 RID: 963
	internal class MarkerList : ArrayList
	{
		// Token: 0x060033DB RID: 13275 RVA: 0x000E6FA5 File Offset: 0x000E51A5
		internal MarkerList() : base(5)
		{
		}

		// Token: 0x060033DC RID: 13276 RVA: 0x000E6FAE File Offset: 0x000E51AE
		internal MarkerListEntry EntryAt(int index)
		{
			return (MarkerListEntry)this[index];
		}

		// Token: 0x060033DD RID: 13277 RVA: 0x000E6FBC File Offset: 0x000E51BC
		internal void AddEntry(MarkerStyle m, long nILS, long nStartIndexOverride, long nStartIndexDefault, long nLevel)
		{
			this.Add(new MarkerListEntry
			{
				Marker = m,
				StartIndexOverride = nStartIndexOverride,
				StartIndexDefault = nStartIndexDefault,
				VirtualListLevel = nLevel,
				ILS = nILS
			});
		}
	}
}
