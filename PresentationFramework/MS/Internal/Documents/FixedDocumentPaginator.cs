using System;
using System.Windows;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006C5 RID: 1733
	internal class FixedDocumentPaginator : DynamicDocumentPaginator, IServiceProvider
	{
		// Token: 0x06006FDA RID: 28634 RVA: 0x00202761 File Offset: 0x00200961
		internal FixedDocumentPaginator(FixedDocument document)
		{
			this._document = document;
		}

		// Token: 0x06006FDB RID: 28635 RVA: 0x00202770 File Offset: 0x00200970
		public override DocumentPage GetPage(int pageNumber)
		{
			return this._document.GetPage(pageNumber);
		}

		// Token: 0x06006FDC RID: 28636 RVA: 0x0020277E File Offset: 0x0020097E
		public override void GetPageAsync(int pageNumber, object userState)
		{
			this._document.GetPageAsync(pageNumber, userState);
		}

		// Token: 0x06006FDD RID: 28637 RVA: 0x0020278D File Offset: 0x0020098D
		public override void CancelAsync(object userState)
		{
			this._document.CancelAsync(userState);
		}

		// Token: 0x06006FDE RID: 28638 RVA: 0x0020279B File Offset: 0x0020099B
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			return this._document.GetPageNumber(contentPosition);
		}

		// Token: 0x06006FDF RID: 28639 RVA: 0x002027A9 File Offset: 0x002009A9
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			return this._document.GetPagePosition(page);
		}

		// Token: 0x06006FE0 RID: 28640 RVA: 0x002027B7 File Offset: 0x002009B7
		public override ContentPosition GetObjectPosition(object o)
		{
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x17001A93 RID: 6803
		// (get) Token: 0x06006FE1 RID: 28641 RVA: 0x002027C5 File Offset: 0x002009C5
		public override bool IsPageCountValid
		{
			get
			{
				return this._document.IsPageCountValid;
			}
		}

		// Token: 0x17001A94 RID: 6804
		// (get) Token: 0x06006FE2 RID: 28642 RVA: 0x002027D2 File Offset: 0x002009D2
		public override int PageCount
		{
			get
			{
				return this._document.PageCount;
			}
		}

		// Token: 0x17001A95 RID: 6805
		// (get) Token: 0x06006FE3 RID: 28643 RVA: 0x002027DF File Offset: 0x002009DF
		// (set) Token: 0x06006FE4 RID: 28644 RVA: 0x002027EC File Offset: 0x002009EC
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

		// Token: 0x17001A96 RID: 6806
		// (get) Token: 0x06006FE5 RID: 28645 RVA: 0x002027FA File Offset: 0x002009FA
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x06006FE6 RID: 28646 RVA: 0x00202802 File Offset: 0x00200A02
		internal void NotifyGetPageCompleted(GetPageCompletedEventArgs e)
		{
			this.OnGetPageCompleted(e);
		}

		// Token: 0x06006FE7 RID: 28647 RVA: 0x0020280B File Offset: 0x00200A0B
		internal void NotifyPaginationCompleted(EventArgs e)
		{
			this.OnPaginationCompleted(e);
		}

		// Token: 0x06006FE8 RID: 28648 RVA: 0x00202814 File Offset: 0x00200A14
		internal void NotifyPaginationProgress(PaginationProgressEventArgs e)
		{
			this.OnPaginationProgress(e);
		}

		// Token: 0x06006FE9 RID: 28649 RVA: 0x0020281D File Offset: 0x00200A1D
		internal void NotifyPagesChanged(PagesChangedEventArgs e)
		{
			this.OnPagesChanged(e);
		}

		// Token: 0x06006FEA RID: 28650 RVA: 0x00202826 File Offset: 0x00200A26
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x040036DA RID: 14042
		private readonly FixedDocument _document;
	}
}
