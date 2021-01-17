using System;
using System.Windows.Media.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000631 RID: 1585
	internal sealed class OptimalBreakSession : UnmanagedHandle
	{
		// Token: 0x060068DB RID: 26843 RVA: 0x001D953D File Offset: 0x001D773D
		internal OptimalBreakSession(TextParagraph textParagraph, TextParaClient textParaClient, TextParagraphCache TextParagraphCache, OptimalTextSource optimalTextSource) : base(textParagraph.PtsContext)
		{
			this._textParagraph = textParagraph;
			this._textParaClient = textParaClient;
			this._textParagraphCache = TextParagraphCache;
			this._optimalTextSource = optimalTextSource;
		}

		// Token: 0x060068DC RID: 26844 RVA: 0x001D9568 File Offset: 0x001D7768
		public override void Dispose()
		{
			try
			{
				if (this._textParagraphCache != null)
				{
					this._textParagraphCache.Dispose();
				}
				if (this._optimalTextSource != null)
				{
					this._optimalTextSource.Dispose();
				}
			}
			finally
			{
				this._textParagraphCache = null;
				this._optimalTextSource = null;
			}
			base.Dispose();
		}

		// Token: 0x1700195F RID: 6495
		// (get) Token: 0x060068DD RID: 26845 RVA: 0x001D95C4 File Offset: 0x001D77C4
		internal TextParagraphCache TextParagraphCache
		{
			get
			{
				return this._textParagraphCache;
			}
		}

		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x060068DE RID: 26846 RVA: 0x001D95CC File Offset: 0x001D77CC
		internal TextParagraph TextParagraph
		{
			get
			{
				return this._textParagraph;
			}
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x060068DF RID: 26847 RVA: 0x001D95D4 File Offset: 0x001D77D4
		internal TextParaClient TextParaClient
		{
			get
			{
				return this._textParaClient;
			}
		}

		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x060068E0 RID: 26848 RVA: 0x001D95DC File Offset: 0x001D77DC
		internal OptimalTextSource OptimalTextSource
		{
			get
			{
				return this._optimalTextSource;
			}
		}

		// Token: 0x040033F2 RID: 13298
		private TextParagraphCache _textParagraphCache;

		// Token: 0x040033F3 RID: 13299
		private TextParagraph _textParagraph;

		// Token: 0x040033F4 RID: 13300
		private TextParaClient _textParaClient;

		// Token: 0x040033F5 RID: 13301
		private OptimalTextSource _optimalTextSource;
	}
}
