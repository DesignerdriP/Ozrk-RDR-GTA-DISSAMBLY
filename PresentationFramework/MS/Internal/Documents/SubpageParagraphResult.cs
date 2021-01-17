using System;
using System.Collections.ObjectModel;
using System.Windows;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006E3 RID: 1763
	internal sealed class SubpageParagraphResult : ParagraphResult
	{
		// Token: 0x0600718E RID: 29070 RVA: 0x0020724B File Offset: 0x0020544B
		internal SubpageParagraphResult(BaseParaClient paraClient) : base(paraClient)
		{
		}

		// Token: 0x17001AFA RID: 6906
		// (get) Token: 0x0600718F RID: 29071 RVA: 0x0020760E File Offset: 0x0020580E
		internal ReadOnlyCollection<ColumnResult> Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = ((SubpageParaClient)this._paraClient).GetColumnResults(out this._hasTextContent);
					Invariant.Assert(this._columns != null, "Columns collection is null");
				}
				return this._columns;
			}
		}

		// Token: 0x17001AFB RID: 6907
		// (get) Token: 0x06007190 RID: 29072 RVA: 0x00207650 File Offset: 0x00205850
		internal override bool HasTextContent
		{
			get
			{
				if (this._columns == null)
				{
					ReadOnlyCollection<ColumnResult> columns = this.Columns;
				}
				return this._hasTextContent;
			}
		}

		// Token: 0x17001AFC RID: 6908
		// (get) Token: 0x06007191 RID: 29073 RVA: 0x00207672 File Offset: 0x00205872
		internal ReadOnlyCollection<ParagraphResult> FloatingElements
		{
			get
			{
				if (this._floatingElements == null)
				{
					this._floatingElements = ((SubpageParaClient)this._paraClient).FloatingElementResults;
					Invariant.Assert(this._floatingElements != null, "Floating elements collection is null");
				}
				return this._floatingElements;
			}
		}

		// Token: 0x17001AFD RID: 6909
		// (get) Token: 0x06007192 RID: 29074 RVA: 0x002076AC File Offset: 0x002058AC
		internal Vector ContentOffset
		{
			get
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this._paraClient.Paragraph.Element, this._paraClient.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				return new Vector(base.LayoutBox.X + TextDpi.FromTextDpi(mbpInfo.BPLeft), base.LayoutBox.Y + TextDpi.FromTextDpi(mbpInfo.BPTop));
			}
		}

		// Token: 0x0400372B RID: 14123
		private ReadOnlyCollection<ColumnResult> _columns;

		// Token: 0x0400372C RID: 14124
		private ReadOnlyCollection<ParagraphResult> _floatingElements;
	}
}
