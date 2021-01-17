using System;

namespace MS.Internal.Data
{
	// Token: 0x02000704 RID: 1796
	internal class AsyncGetValueRequest : AsyncDataRequest
	{
		// Token: 0x0600735B RID: 29531 RVA: 0x002110D4 File Offset: 0x0020F2D4
		internal AsyncGetValueRequest(object item, string propertyName, object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args) : base(bindingState, workCallback, completedCallback, args)
		{
			this._item = item;
			this._propertyName = propertyName;
		}

		// Token: 0x17001B64 RID: 7012
		// (get) Token: 0x0600735C RID: 29532 RVA: 0x002110F1 File Offset: 0x0020F2F1
		public object SourceItem
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x040037A1 RID: 14241
		private object _item;

		// Token: 0x040037A2 RID: 14242
		private string _propertyName;
	}
}
