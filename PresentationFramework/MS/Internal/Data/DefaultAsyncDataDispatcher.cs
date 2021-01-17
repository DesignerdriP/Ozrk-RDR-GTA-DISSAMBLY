using System;
using System.Collections;
using System.Threading;
using System.Windows;

namespace MS.Internal.Data
{
	// Token: 0x02000718 RID: 1816
	internal class DefaultAsyncDataDispatcher : IAsyncDataDispatcher
	{
		// Token: 0x060074CD RID: 29901 RVA: 0x00216898 File Offset: 0x00214A98
		void IAsyncDataDispatcher.AddRequest(AsyncDataRequest request)
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Add(request);
			}
			ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcessRequest), request);
		}

		// Token: 0x060074CE RID: 29902 RVA: 0x002168F8 File Offset: 0x00214AF8
		void IAsyncDataDispatcher.CancelAllRequests()
		{
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					AsyncDataRequest asyncDataRequest = (AsyncDataRequest)this._list[i];
					asyncDataRequest.Cancel();
				}
				this._list.Clear();
			}
		}

		// Token: 0x060074CF RID: 29903 RVA: 0x00216970 File Offset: 0x00214B70
		private void ProcessRequest(object o)
		{
			AsyncDataRequest asyncDataRequest = (AsyncDataRequest)o;
			try
			{
				asyncDataRequest.Complete(asyncDataRequest.DoWork());
			}
			catch (Exception ex)
			{
				if (CriticalExceptions.IsCriticalApplicationException(ex))
				{
					throw;
				}
				asyncDataRequest.Fail(ex);
			}
			catch
			{
				asyncDataRequest.Fail(new InvalidOperationException(SR.Get("NonCLSException", new object[]
				{
					"processing an async data request"
				})));
			}
			object syncRoot = this._list.SyncRoot;
			lock (syncRoot)
			{
				this._list.Remove(asyncDataRequest);
			}
		}

		// Token: 0x040037FB RID: 14331
		private ArrayList _list = new ArrayList();
	}
}
