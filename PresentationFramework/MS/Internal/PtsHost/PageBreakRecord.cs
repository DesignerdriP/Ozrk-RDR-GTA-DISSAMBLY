using System;
using System.Security;
using System.Threading;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000634 RID: 1588
	internal sealed class PageBreakRecord : IDisposable
	{
		// Token: 0x060068EB RID: 26859 RVA: 0x001D9960 File Offset: 0x001D7B60
		internal PageBreakRecord(PtsContext ptsContext, SecurityCriticalDataForSet<IntPtr> br, int pageNumber)
		{
			Invariant.Assert(ptsContext != null, "Invalid PtsContext object.");
			Invariant.Assert(br.Value != IntPtr.Zero, "Invalid break record object.");
			this._br = br;
			this._pageNumber = pageNumber;
			this._ptsContext = new WeakReference(ptsContext);
			ptsContext.OnPageBreakRecordCreated(this._br);
		}

		// Token: 0x060068EC RID: 26860 RVA: 0x001D99C4 File Offset: 0x001D7BC4
		~PageBreakRecord()
		{
			this.Dispose(false);
		}

		// Token: 0x060068ED RID: 26861 RVA: 0x001D99F4 File Offset: 0x001D7BF4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x060068EE RID: 26862 RVA: 0x001D9A03 File Offset: 0x001D7C03
		internal IntPtr BreakRecord
		{
			get
			{
				return this._br.Value;
			}
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x060068EF RID: 26863 RVA: 0x001D9A10 File Offset: 0x001D7C10
		internal int PageNumber
		{
			get
			{
				return this._pageNumber;
			}
		}

		// Token: 0x060068F0 RID: 26864 RVA: 0x001D9A18 File Offset: 0x001D7C18
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				PtsContext ptsContext = this._ptsContext.Target as PtsContext;
				if (ptsContext != null && !ptsContext.Disposed)
				{
					ptsContext.OnPageBreakRecordDisposed(this._br, disposing);
				}
				this._br.Value = IntPtr.Zero;
				this._ptsContext = null;
			}
		}

		// Token: 0x040033FC RID: 13308
		private SecurityCriticalDataForSet<IntPtr> _br;

		// Token: 0x040033FD RID: 13309
		private readonly int _pageNumber;

		// Token: 0x040033FE RID: 13310
		private WeakReference _ptsContext;

		// Token: 0x040033FF RID: 13311
		private int _disposed;
	}
}
