using System;
using System.Windows.Controls.Primitives;

namespace System.Windows.Controls
{
	// Token: 0x020004C7 RID: 1223
	internal class DeferredSelectedIndexReference : DeferredReference
	{
		// Token: 0x06004A4B RID: 19019 RVA: 0x0014F8D1 File Offset: 0x0014DAD1
		internal DeferredSelectedIndexReference(Selector selector)
		{
			this._selector = selector;
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x0014F8E0 File Offset: 0x0014DAE0
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			return this._selector.InternalSelectedIndex;
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x0014F8F2 File Offset: 0x0014DAF2
		internal override Type GetValueType()
		{
			return typeof(int);
		}

		// Token: 0x04002A51 RID: 10833
		private readonly Selector _selector;
	}
}
