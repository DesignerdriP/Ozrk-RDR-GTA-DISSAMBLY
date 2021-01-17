using System;
using System.Threading;

namespace MS.Internal.Utility
{
	// Token: 0x020007ED RID: 2029
	internal class MonitorWrapper
	{
		// Token: 0x06007D33 RID: 32051 RVA: 0x00233040 File Offset: 0x00231240
		public IDisposable Enter()
		{
			Monitor.Enter(this._syncRoot);
			Interlocked.Increment(ref this._enterCount);
			return new MonitorWrapper.MonitorHelper(this);
		}

		// Token: 0x06007D34 RID: 32052 RVA: 0x00233060 File Offset: 0x00231260
		public void Exit()
		{
			int num = Interlocked.Decrement(ref this._enterCount);
			Invariant.Assert(num >= 0, "unmatched call to MonitorWrapper.Exit");
			Monitor.Exit(this._syncRoot);
		}

		// Token: 0x17001D18 RID: 7448
		// (get) Token: 0x06007D35 RID: 32053 RVA: 0x00233095 File Offset: 0x00231295
		public bool Busy
		{
			get
			{
				return this._enterCount > 0;
			}
		}

		// Token: 0x04003AE8 RID: 15080
		private int _enterCount;

		// Token: 0x04003AE9 RID: 15081
		private object _syncRoot = new object();

		// Token: 0x02000B88 RID: 2952
		private class MonitorHelper : IDisposable
		{
			// Token: 0x06008E6F RID: 36463 RVA: 0x0025C3E7 File Offset: 0x0025A5E7
			public MonitorHelper(MonitorWrapper monitorWrapper)
			{
				this._monitorWrapper = monitorWrapper;
			}

			// Token: 0x06008E70 RID: 36464 RVA: 0x0025C3F6 File Offset: 0x0025A5F6
			public void Dispose()
			{
				if (this._monitorWrapper != null)
				{
					this._monitorWrapper.Exit();
					this._monitorWrapper = null;
				}
				GC.SuppressFinalize(this);
			}

			// Token: 0x04004B96 RID: 19350
			private MonitorWrapper _monitorWrapper;
		}
	}
}
