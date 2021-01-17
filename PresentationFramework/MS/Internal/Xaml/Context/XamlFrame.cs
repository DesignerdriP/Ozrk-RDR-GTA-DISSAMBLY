using System;

namespace MS.Internal.Xaml.Context
{
	// Token: 0x02000805 RID: 2053
	internal abstract class XamlFrame
	{
		// Token: 0x06007DFA RID: 32250 RVA: 0x00235025 File Offset: 0x00233225
		protected XamlFrame()
		{
			this._depth = -1;
		}

		// Token: 0x06007DFB RID: 32251 RVA: 0x00235034 File Offset: 0x00233234
		protected XamlFrame(XamlFrame source)
		{
			this._depth = source._depth;
		}

		// Token: 0x06007DFC RID: 32252 RVA: 0x0003E264 File Offset: 0x0003C464
		public virtual XamlFrame Clone()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06007DFD RID: 32253
		public abstract void Reset();

		// Token: 0x17001D47 RID: 7495
		// (get) Token: 0x06007DFE RID: 32254 RVA: 0x00235048 File Offset: 0x00233248
		public int Depth
		{
			get
			{
				return this._depth;
			}
		}

		// Token: 0x17001D48 RID: 7496
		// (get) Token: 0x06007DFF RID: 32255 RVA: 0x00235050 File Offset: 0x00233250
		// (set) Token: 0x06007E00 RID: 32256 RVA: 0x00235058 File Offset: 0x00233258
		public XamlFrame Previous
		{
			get
			{
				return this._previous;
			}
			set
			{
				this._previous = value;
				this._depth = ((this._previous == null) ? 0 : (this._previous._depth + 1));
			}
		}

		// Token: 0x04003B78 RID: 15224
		private int _depth;

		// Token: 0x04003B79 RID: 15225
		private XamlFrame _previous;
	}
}
