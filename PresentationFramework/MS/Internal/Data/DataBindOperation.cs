using System;
using System.Windows.Threading;

namespace MS.Internal.Data
{
	// Token: 0x02000716 RID: 1814
	internal class DataBindOperation
	{
		// Token: 0x060074C5 RID: 29893 RVA: 0x00216793 File Offset: 0x00214993
		public DataBindOperation(DispatcherOperationCallback method, object arg, int cost = 1)
		{
			this._method = method;
			this._arg = arg;
			this._cost = cost;
		}

		// Token: 0x17001BCA RID: 7114
		// (get) Token: 0x060074C6 RID: 29894 RVA: 0x002167B0 File Offset: 0x002149B0
		// (set) Token: 0x060074C7 RID: 29895 RVA: 0x002167B8 File Offset: 0x002149B8
		public int Cost
		{
			get
			{
				return this._cost;
			}
			set
			{
				this._cost = value;
			}
		}

		// Token: 0x060074C8 RID: 29896 RVA: 0x002167C1 File Offset: 0x002149C1
		public void Invoke()
		{
			this._method(this._arg);
		}

		// Token: 0x040037F8 RID: 14328
		private DispatcherOperationCallback _method;

		// Token: 0x040037F9 RID: 14329
		private object _arg;

		// Token: 0x040037FA RID: 14330
		private int _cost;
	}
}
