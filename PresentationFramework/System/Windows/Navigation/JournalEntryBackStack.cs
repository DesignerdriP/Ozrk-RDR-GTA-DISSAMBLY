using System;
using System.Collections;

namespace System.Windows.Navigation
{
	// Token: 0x020002F8 RID: 760
	internal class JournalEntryBackStack : JournalEntryStack
	{
		// Token: 0x060028AE RID: 10414 RVA: 0x000BD045 File Offset: 0x000BB245
		public JournalEntryBackStack(Journal journal) : base(journal)
		{
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000BD04E File Offset: 0x000BB24E
		public override IEnumerator GetEnumerator()
		{
			return new JournalEntryStackEnumerator(this._journal, this._journal.CurrentIndex - 1, -1, base.Filter);
		}
	}
}
