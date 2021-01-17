using System;

namespace MS.Internal.Data
{
	// Token: 0x02000747 RID: 1863
	internal class CollectionRecord
	{
		// Token: 0x17001C46 RID: 7238
		// (get) Token: 0x060076ED RID: 30445 RVA: 0x0021FA94 File Offset: 0x0021DC94
		// (set) Token: 0x060076EE RID: 30446 RVA: 0x0021FAA6 File Offset: 0x0021DCA6
		public ViewTable ViewTable
		{
			get
			{
				return (ViewTable)this._wrViewTable.Target;
			}
			set
			{
				this._wrViewTable = new WeakReference(value);
			}
		}

		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x060076EF RID: 30447 RVA: 0x0021FAB4 File Offset: 0x0021DCB4
		public bool IsAlive
		{
			get
			{
				return this.SynchronizationInfo.IsAlive || this._wrViewTable.IsAlive;
			}
		}

		// Token: 0x0400389D RID: 14493
		public SynchronizationInfo SynchronizationInfo;

		// Token: 0x0400389E RID: 14494
		private WeakReference _wrViewTable = ViewManager.NullWeakRef;
	}
}
