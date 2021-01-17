using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006C6 RID: 1734
	internal class FixedDocumentSequencePaginator : DynamicDocumentPaginator, IServiceProvider
	{
		// Token: 0x06006FEB RID: 28651 RVA: 0x00202834 File Offset: 0x00200A34
		internal FixedDocumentSequencePaginator(FixedDocumentSequence document)
		{
			this._document = document;
		}

		// Token: 0x06006FEC RID: 28652 RVA: 0x00202843 File Offset: 0x00200A43
		public override DocumentPage GetPage(int pageNumber)
		{
			return this._document.GetPage(pageNumber);
		}

		// Token: 0x06006FED RID: 28653 RVA: 0x00202851 File Offset: 0x00200A51
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._document.GetPageAsync(pageNumber, userState);
		}

		// Token: 0x06006FEE RID: 28654 RVA: 0x00202860 File Offset: 0x00200A60
		public override void CancelAsync(object userState)
		{
			this._document.CancelAsync(userState);
		}

		// Token: 0x06006FEF RID: 28655 RVA: 0x0020286E File Offset: 0x00200A6E
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			return this._document.GetPageNumber(contentPosition);
		}

		// Token: 0x06006FF0 RID: 28656 RVA: 0x0020287C File Offset: 0x00200A7C
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			return this._document.GetPagePosition(page);
		}

		// Token: 0x06006FF1 RID: 28657 RVA: 0x0020288A File Offset: 0x00200A8A
		public override ContentPosition GetObjectPosition(object o)
		{
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x17001A97 RID: 6807
		// (get) Token: 0x06006FF2 RID: 28658 RVA: 0x00202898 File Offset: 0x00200A98
		public override bool IsPageCountValid
		{
			get
			{
				return this._document.IsPageCountValid;
			}
		}

		// Token: 0x17001A98 RID: 6808
		// (get) Token: 0x06006FF3 RID: 28659 RVA: 0x002028A5 File Offset: 0x00200AA5
		public override int PageCount
		{
			get
			{
				return this._document.PageCount;
			}
		}

		// Token: 0x17001A99 RID: 6809
		// (get) Token: 0x06006FF4 RID: 28660 RVA: 0x002028B2 File Offset: 0x00200AB2
		// (set) Token: 0x06006FF5 RID: 28661 RVA: 0x002028BF File Offset: 0x00200ABF
		public override Size PageSize
		{
			get
			{
				return this._document.PageSize;
			}
			set
			{
				this._document.PageSize = value;
			}
		}

		// Token: 0x17001A9A RID: 6810
		// (get) Token: 0x06006FF6 RID: 28662 RVA: 0x002028CD File Offset: 0x00200ACD
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x06006FF7 RID: 28663 RVA: 0x00202802 File Offset: 0x00200A02
		internal void NotifyGetPageCompleted(GetPageCompletedEventArgs e)
		{
			this.OnGetPageCompleted(e);
		}

		// Token: 0x06006FF8 RID: 28664 RVA: 0x0020280B File Offset: 0x00200A0B
		internal void NotifyPaginationCompleted(EventArgs e)
		{
			this.OnPaginationCompleted(e);
		}

		// Token: 0x06006FF9 RID: 28665 RVA: 0x00202814 File Offset: 0x00200A14
		internal void NotifyPaginationProgress(PaginationProgressEventArgs e)
		{
			this.OnPaginationProgress(e);
		}

		// Token: 0x06006FFA RID: 28666 RVA: 0x0020281D File Offset: 0x00200A1D
		internal void NotifyPagesChanged(PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x06006FFB RID: 28667 RVA: 0x002028D5 File Offset: 0x00200AD5
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x040036DB RID: 14043
		private readonly FixedDocumentSequence _document;
	}
}
