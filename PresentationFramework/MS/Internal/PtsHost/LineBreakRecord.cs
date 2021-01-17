using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000628 RID: 1576
	internal sealed class LineBreakRecord : UnmanagedHandle
	{
		// Token: 0x06006895 RID: 26773 RVA: 0x001D81A7 File Offset: 0x001D63A7
		internal LineBreakRecord(PtsContext ptsContext, TextLineBreak textLineBreak) : base(ptsContext)
		{
			this._textLineBreak = textLineBreak;
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x001D81B7 File Offset: 0x001D63B7
		public override void Dispose()
		{
			if (this._textLineBreak != null)
			{
				this._textLineBreak.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x06006897 RID: 26775 RVA: 0x001D81D2 File Offset: 0x001D63D2
		internal LineBreakRecord Clone()
		{
			return new LineBreakRecord(base.PtsContext, this._textLineBreak.Clone());
		}

		// Token: 0x17001947 RID: 6471
		// (get) Token: 0x06006898 RID: 26776 RVA: 0x001D81EA File Offset: 0x001D63EA
		internal TextLineBreak TextLineBreak
		{
			get
			{
				return this._textLineBreak;
			}
		}

		// Token: 0x040033E0 RID: 13280
		private TextLineBreak _textLineBreak;
	}
}
