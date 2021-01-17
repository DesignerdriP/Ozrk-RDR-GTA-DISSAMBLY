using System;
using System.Collections;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006D8 RID: 1752
	internal class PageDestroyedWatcher
	{
		// Token: 0x06007157 RID: 29015 RVA: 0x00207032 File Offset: 0x00205232
		public PageDestroyedWatcher()
		{
			this._table = new Hashtable(16);
		}

		// Token: 0x06007158 RID: 29016 RVA: 0x00207048 File Offset: 0x00205248
		public void AddPage(DocumentPage page)
		{
			if (!this._table.Contains(page))
			{
				this._table.Add(page, false);
				page.PageDestroyed += this.OnPageDestroyed;
				return;
			}
			this._table[page] = false;
		}

		// Token: 0x06007159 RID: 29017 RVA: 0x0020709A File Offset: 0x0020529A
		public void RemovePage(DocumentPage page)
		{
			if (this._table.Contains(page))
			{
				this._table.Remove(page);
				page.PageDestroyed -= this.OnPageDestroyed;
			}
		}

		// Token: 0x0600715A RID: 29018 RVA: 0x002070C8 File Offset: 0x002052C8
		public bool IsDestroyed(DocumentPage page)
		{
			return !this._table.Contains(page) || (bool)this._table[page];
		}

		// Token: 0x0600715B RID: 29019 RVA: 0x002070EC File Offset: 0x002052EC
		private void OnPageDestroyed(object sender, EventArgs e)
		{
			DocumentPage documentPage = sender as DocumentPage;
			Invariant.Assert(documentPage != null, "Invalid type in PageDestroyedWatcher");
			if (this._table.Contains(documentPage))
			{
				this._table[documentPage] = true;
			}
		}

		// Token: 0x04003714 RID: 14100
		private Hashtable _table;
	}
}
