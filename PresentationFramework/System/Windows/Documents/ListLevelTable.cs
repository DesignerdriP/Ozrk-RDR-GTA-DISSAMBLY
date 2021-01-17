using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003C9 RID: 969
	internal class ListLevelTable : ArrayList
	{
		// Token: 0x06003410 RID: 13328 RVA: 0x000E7B4C File Offset: 0x000E5D4C
		internal ListLevelTable() : base(1)
		{
		}

		// Token: 0x06003411 RID: 13329 RVA: 0x000E7B55 File Offset: 0x000E5D55
		internal ListLevel EntryAt(int index)
		{
			if (index > this.Count)
			{
				index = this.Count - 1;
			}
			return (ListLevel)((this.Count > index && index >= 0) ? this[index] : null);
		}

		// Token: 0x06003412 RID: 13330 RVA: 0x000E7B88 File Offset: 0x000E5D88
		internal ListLevel AddEntry()
		{
			ListLevel listLevel = new ListLevel();
			this.Add(listLevel);
			return listLevel;
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06003413 RID: 13331 RVA: 0x000E7BA4 File Offset: 0x000E5DA4
		internal ListLevel CurrentEntry
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this.EntryAt(this.Count - 1);
			}
		}
	}
}
