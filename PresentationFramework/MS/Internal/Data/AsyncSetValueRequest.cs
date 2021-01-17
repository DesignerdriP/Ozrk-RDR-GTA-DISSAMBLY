using System;

namespace MS.Internal.Data
{
	// Token: 0x02000705 RID: 1797
	internal class AsyncSetValueRequest : AsyncDataRequest
	{
		// Token: 0x0600735D RID: 29533 RVA: 0x002110F9 File Offset: 0x0020F2F9
		internal AsyncSetValueRequest(object item, string propertyName, object value, object bindingState, AsyncRequestCallback workCallback, AsyncRequestCallback completedCallback, params object[] args) : base(bindingState, workCallback, completedCallback, args)
		{
			this._item = item;
			this._propertyName = propertyName;
			this._value = value;
		}

		// Token: 0x17001B65 RID: 7013
		// (get) Token: 0x0600735E RID: 29534 RVA: 0x0021111E File Offset: 0x0020F31E
		public object TargetItem
		{
			get
			{
				return this._item;
			}
		}

		// Token: 0x17001B66 RID: 7014
		// (get) Token: 0x0600735F RID: 29535 RVA: 0x00211126 File Offset: 0x0020F326
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x040037A3 RID: 14243
		private object _item;

		// Token: 0x040037A4 RID: 14244
		private string _propertyName;

		// Token: 0x040037A5 RID: 14245
		private object _value;
	}
}
