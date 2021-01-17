﻿using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006DE RID: 1758
	internal abstract class ParagraphResult
	{
		// Token: 0x06007166 RID: 29030 RVA: 0x0020717C File Offset: 0x0020537C
		internal ParagraphResult(BaseParaClient paraClient)
		{
			this._paraClient = paraClient;
			this._layoutBox = this._paraClient.Rect.FromTextDpi();
			this._element = paraClient.Paragraph.Element;
		}

		// Token: 0x06007167 RID: 29031 RVA: 0x002071C0 File Offset: 0x002053C0
		internal ParagraphResult(BaseParaClient paraClient, Rect layoutBox, DependencyObject element) : this(paraClient)
		{
			this._layoutBox = layoutBox;
			this._element = element;
		}

		// Token: 0x06007168 RID: 29032 RVA: 0x002071D7 File Offset: 0x002053D7
		internal virtual bool Contains(ITextPointer position, bool strict)
		{
			this.EnsureTextContentRange();
			return this._contentRange.Contains(position, strict);
		}

		// Token: 0x17001AEA RID: 6890
		// (get) Token: 0x06007169 RID: 29033 RVA: 0x002071EC File Offset: 0x002053EC
		internal ITextPointer StartPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.StartPosition;
			}
		}

		// Token: 0x17001AEB RID: 6891
		// (get) Token: 0x0600716A RID: 29034 RVA: 0x002071FF File Offset: 0x002053FF
		internal ITextPointer EndPosition
		{
			get
			{
				this.EnsureTextContentRange();
				return this._contentRange.EndPosition;
			}
		}

		// Token: 0x17001AEC RID: 6892
		// (get) Token: 0x0600716B RID: 29035 RVA: 0x00207212 File Offset: 0x00205412
		internal Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x0600716C RID: 29036 RVA: 0x0020721A File Offset: 0x0020541A
		internal DependencyObject Element
		{
			get
			{
				return this._element;
			}
		}

		// Token: 0x17001AEE RID: 6894
		// (get) Token: 0x0600716D RID: 29037 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool HasTextContent
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600716E RID: 29038 RVA: 0x00207222 File Offset: 0x00205422
		private void EnsureTextContentRange()
		{
			if (this._contentRange == null)
			{
				this._contentRange = this._paraClient.GetTextContentRange();
				Invariant.Assert(this._contentRange != null);
			}
		}

		// Token: 0x0400371F RID: 14111
		protected readonly BaseParaClient _paraClient;

		// Token: 0x04003720 RID: 14112
		protected readonly Rect _layoutBox;

		// Token: 0x04003721 RID: 14113
		protected readonly DependencyObject _element;

		// Token: 0x04003722 RID: 14114
		private TextContentRange _contentRange;

		// Token: 0x04003723 RID: 14115
		protected bool _hasTextContent;
	}
}
