using System;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000652 RID: 1618
	internal class UnmanagedHandle : IDisposable
	{
		// Token: 0x06006B73 RID: 27507 RVA: 0x001F0BB4 File Offset: 0x001EEDB4
		protected UnmanagedHandle(PtsContext ptsContext)
		{
			this._ptsContext = ptsContext;
			this._handle = ptsContext.CreateHandle(this);
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x001F0BD0 File Offset: 0x001EEDD0
		public virtual void Dispose()
		{
			try
			{
				this._ptsContext.ReleaseHandle(this._handle);
			}
			finally
			{
				this._handle = IntPtr.Zero;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x170019BF RID: 6591
		// (get) Token: 0x06006B75 RID: 27509 RVA: 0x001F0C14 File Offset: 0x001EEE14
		internal IntPtr Handle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x170019C0 RID: 6592
		// (get) Token: 0x06006B76 RID: 27510 RVA: 0x001F0C1C File Offset: 0x001EEE1C
		internal PtsContext PtsContext
		{
			get
			{
				return this._ptsContext;
			}
		}

		// Token: 0x04003468 RID: 13416
		private IntPtr _handle;

		// Token: 0x04003469 RID: 13417
		private readonly PtsContext _ptsContext;
	}
}
