using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020002F9 RID: 761
	internal class JournalEntryForwardStack : JournalEntryStack
	{
		// Token: 0x060028B0 RID: 10416 RVA: 0x000BD045 File Offset: 0x000BB245
		public JournalEntryForwardStack(Journal journal) : base(journal)
		{
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000BD06F File Offset: 0x000BB26F
		public override IEnumerator GetEnumerator()
		{
			return new JournalEntryStackEnumerator(this._journal, this._journal.CurrentIndex + 1, 1, base.Filter);
		}
	}
}
