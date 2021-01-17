using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000632 RID: 1586
	internal sealed class LineBreakpoint : UnmanagedHandle
	{
		// Token: 0x060068E1 RID: 26849 RVA: 0x001D95E4 File Offset: 0x001D77E4
		internal LineBreakpoint(OptimalBreakSession optimalBreakSession, TextBreakpoint textBreakpoint) : base(optimalBreakSession.PtsContext)
		{
			this._textBreakpoint = textBreakpoint;
			this._optimalBreakSession = optimalBreakSession;
		}

		// Token: 0x060068E2 RID: 26850 RVA: 0x001D9600 File Offset: 0x001D7800
		public override void Dispose()
		{
			if (this._textBreakpoint != null)
			{
				this._textBreakpoint.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x060068E3 RID: 26851 RVA: 0x001D961B File Offset: 0x001D781B
		internal OptimalBreakSession OptimalBreakSession
		{
			get
			{
				return this._optimalBreakSession;
			}
		}

		// Token: 0x040033F6 RID: 13302
		private TextBreakpoint _textBreakpoint;

		// Token: 0x040033F7 RID: 13303
		private OptimalBreakSession _optimalBreakSession;
	}
}
