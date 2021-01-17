using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Windows.Navigation
{
	// Token: 0x020002F7 RID: 759
	internal abstract class JournalEntryStack : IEnumerable, INotifyCollectionChanged
	{
		// Token: 0x060028A6 RID: 10406 RVA: 0x000BCF80 File Offset: 0x000BB180
		internal JournalEntryStack(Journal journal)
		{
			this._journal = journal;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000BCF8F File Offset: 0x000BB18F
		internal void OnCollectionChanged()
		{
			if (this.CollectionChanged != null)
			{
				this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x000BCFAB File Offset: 0x000BB1AB
		// (set) Token: 0x060028A9 RID: 10409 RVA: 0x000BCFB3 File Offset: 0x000BB1B3
		internal JournalEntryFilter Filter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000BCFBC File Offset: 0x000BB1BC
		internal IEnumerable GetLimitedJournalEntryStackEnumerable()
		{
			if (this._ljese == null)
			{
				this._ljese = new LimitedJournalEntryStackEnumerable(this);
			}
			return this._ljese;
		}

		// Token: 0x14000058 RID: 88
		// (add) Token: 0x060028AB RID: 10411 RVA: 0x000BCFD8 File Offset: 0x000BB1D8
		// (remove) Token: 0x060028AC RID: 10412 RVA: 0x000BD010 File Offset: 0x000BB210
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		// Token: 0x060028AD RID: 10413
		public abstract IEnumerator GetEnumerator();

		// Token: 0x04001B9A RID: 7066
		private LimitedJournalEntryStackEnumerable _ljese;

		// Token: 0x04001B9B RID: 7067
		protected JournalEntryFilter _filter;

		// Token: 0x04001B9D RID: 7069
		protected Journal _journal;
	}
}
