using System;

namespace MS.Internal.Data
{
	// Token: 0x02000703 RID: 1795
	internal class AsyncDataRequest
	{
		// Token: 0x06007350 RID: 29520 RVA: 0x00210F66 File Offset: 0x0020F166
		internal AsyncDataRequest(object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args)
		{
			this._bindingState = bindingState;
			this._workCallback = workCallback;
			this._completedCallback = completedCallback;
			this._args = args;
		}

		// Token: 0x17001B60 RID: 7008
		// (get) Token: 0x06007351 RID: 29521 RVA: 0x00210F96 File Offset: 0x0020F196
		public object Result
		{
			get
			{
				return this._result;
			}
		}

		// Token: 0x17001B61 RID: 7009
		// (get) Token: 0x06007352 RID: 29522 RVA: 0x00210F9E File Offset: 0x0020F19E
		public AsyncRequestStatus Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x17001B62 RID: 7010
		// (get) Token: 0x06007353 RID: 29523 RVA: 0x00210FA6 File Offset: 0x0020F1A6
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x06007354 RID: 29524 RVA: 0x00210FAE File Offset: 0x0020F1AE
		public object DoWork()
		{
			if (this.DoBeginWork() && this._workCallback != null)
			{
				return this._workCallback(this);
			}
			return null;
		}

		// Token: 0x06007355 RID: 29525 RVA: 0x00210FCE File Offset: 0x0020F1CE
		public bool DoBeginWork()
		{
			return this.ChangeStatus(AsyncRequestStatus.Working);
		}

		// Token: 0x06007356 RID: 29526 RVA: 0x00210FD7 File Offset: 0x0020F1D7
		public void Complete(object result)
		{
			if (this.ChangeStatus(AsyncRequestStatus.Completed))
			{
				this._result = result;
				if (this._completedCallback != null)
				{
					this._completedCallback(this);
				}
			}
		}

		// Token: 0x06007357 RID: 29527 RVA: 0x00210FFE File Offset: 0x0020F1FE
		public void Cancel()
		{
			this.ChangeStatus(AsyncRequestStatus.Cancelled);
		}

		// Token: 0x06007358 RID: 29528 RVA: 0x00211008 File Offset: 0x0020F208
		public void Fail(Exception exception)
		{
			if (this.ChangeStatus(AsyncRequestStatus.Failed))
			{
				this._exception = exception;
				if (this._completedCallback != null)
				{
					this._completedCallback(this);
				}
			}
		}

		// Token: 0x17001B63 RID: 7011
		// (get) Token: 0x06007359 RID: 29529 RVA: 0x0021102F File Offset: 0x0020F22F
		internal object[] Args
		{
			get
			{
				return this._args;
			}
		}

		// Token: 0x0600735A RID: 29530 RVA: 0x00211038 File Offset: 0x0020F238
		private bool ChangeStatus(AsyncRequestStatus newStatus)
		{
			bool flag = false;
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				switch (newStatus)
				{
				case AsyncRequestStatus.Working:
					flag = (this._status == AsyncRequestStatus.Waiting);
					break;
				case AsyncRequestStatus.Completed:
					flag = (this._status == AsyncRequestStatus.Working);
					break;
				case AsyncRequestStatus.Cancelled:
					flag = (this._status == AsyncRequestStatus.Waiting || this._status == AsyncRequestStatus.Working);
					break;
				case AsyncRequestStatus.Failed:
					flag = (this._status == AsyncRequestStatus.Working);
					break;
				}
				if (flag)
				{
					this._status = newStatus;
				}
			}
			return flag;
		}

		// Token: 0x04003799 RID: 14233
		private AsyncRequestStatus _status;

		// Token: 0x0400379A RID: 14234
		private object _result;

		// Token: 0x0400379B RID: 14235
		private object _bindingState;

		// Token: 0x0400379C RID: 14236
		private object[] _args;

		// Token: 0x0400379D RID: 14237
		private Exception _exception;

		// Token: 0x0400379E RID: 14238
		private AsyncRequestCallback _workCallback;

		// Token: 0x0400379F RID: 14239
		private AsyncRequestCallback _completedCallback;

		// Token: 0x040037A0 RID: 14240
		private object SyncRoot = new object();
	}
}
