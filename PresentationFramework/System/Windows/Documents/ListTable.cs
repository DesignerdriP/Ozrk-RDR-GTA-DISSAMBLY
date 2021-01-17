using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003CB RID: 971
	internal class ListTable : ArrayList
	{
		// Token: 0x0600341A RID: 13338 RVA: 0x000E3B22 File Offset: 0x000E1D22
		internal ListTable() : base(20)
		{
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x000E7C0D File Offset: 0x000E5E0D
		internal ListTableEntry EntryAt(int index)
		{
			return (ListTableEntry)this[index];
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x000E7C1C File Offset: 0x000E5E1C
		internal ListTableEntry FindEntry(long id)
		{
			for (int i = 0; i < this.Count; i++)
			{
				ListTableEntry listTableEntry = this.EntryAt(i);
				if (listTableEntry.ID == id)
				{
					return listTableEntry;
				}
			}
			return null;
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x000E7C50 File Offset: 0x000E5E50
		internal ListTableEntry AddEntry()
		{
			ListTableEntry listTableEntry = new ListTableEntry();
			this.Add(listTableEntry);
			return listTableEntry;
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x000E7C6C File Offset: 0x000E5E6C
		internal ListTableEntry CurrentEntry
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
