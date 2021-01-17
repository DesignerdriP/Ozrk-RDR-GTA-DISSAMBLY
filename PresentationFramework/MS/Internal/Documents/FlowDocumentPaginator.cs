using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Internal.PtsHost;

namespace MS.Internal.Documents
{
	// Token: 0x020006C9 RID: 1737
	internal class FlowDocumentPaginator : DynamicDocumentPaginator, IServiceProvider, IFlowDocumentFormatter
	{
		// Token: 0x0600700E RID: 28686 RVA: 0x00202E44 File Offset: 0x00201044
		internal FlowDocumentPaginator(FlowDocument document)
		{
			this._pageSize = FlowDocumentPaginator._defaultPageSize;
			this._document = document;
			this._brt = new BreakRecordTable(this);
			this._dispatcherObject = new FlowDocumentPaginator.CustomDispatcherObject();
			this._backgroundPagination = true;
			this.InitiateNextAsyncOperation();
		}

		// Token: 0x0600700F RID: 28687 RVA: 0x00202E9C File Offset: 0x0020109C
		public override void GetPageAsync(int pageNumber, object userState)
		{
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			DocumentPage documentPage = null;
			if (!this._backgroundPagination)
			{
				documentPage = this.GetPage(pageNumber);
			}
			else
			{
				if (this._brt.IsClean && !this._brt.HasPageBreakRecord(pageNumber))
				{
					documentPage = DocumentPage.Missing;
				}
				if (this._brt.HasPageBreakRecord(pageNumber))
				{
					documentPage = this.GetPage(pageNumber);
				}
				if (documentPage == null)
				{
					this._asyncRequests.Add(new FlowDocumentPaginator.GetPageAsyncRequest(pageNumber, userState, this));
					this.InitiateNextAsyncOperation();
				}
			}
			if (documentPage != null)
			{
				this.OnGetPageCompleted(new GetPageCompletedEventArgs(documentPage, pageNumber, null, false, userState));
			}
		}

		// Token: 0x06007010 RID: 28688 RVA: 0x00202F80 File Offset: 0x00201180
		public override DocumentPage GetPage(int pageNumber)
		{
			this._dispatcherObject.VerifyAccess();
			if (pageNumber < 0)
			{
				throw new ArgumentOutOfRangeException("pageNumber", SR.Get("IDPNegativePageNumber"));
			}
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			DocumentPage documentPage;
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				try
				{
					if (this._brt.IsClean && !this._brt.HasPageBreakRecord(pageNumber))
					{
						documentPage = DocumentPage.Missing;
					}
					else
					{
						documentPage = this._brt.GetCachedDocumentPage(pageNumber);
						if (documentPage == null)
						{
							if (!this._brt.HasPageBreakRecord(pageNumber))
							{
								documentPage = this.FormatPagesTill(pageNumber);
							}
							else
							{
								documentPage = this.FormatPage(pageNumber);
							}
						}
					}
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			return documentPage;
		}

		// Token: 0x06007011 RID: 28689 RVA: 0x002030AC File Offset: 0x002012AC
		public override void GetPageNumberAsync(ContentPosition contentPosition, object userState)
		{
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			if (contentPosition == ContentPosition.Missing)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			TextPointer textPointer = contentPosition as TextPointer;
			if (textPointer == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			if (textPointer.TextContainer != this._document.StructuralCache.TextContainer)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			int pageNumber = 0;
			if (!this._backgroundPagination)
			{
				pageNumber = this.GetPageNumber(contentPosition);
				this.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(contentPosition, pageNumber, null, false, userState));
				return;
			}
			if (this._brt.GetPageNumberForContentPosition(textPointer, ref pageNumber))
			{
				this.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(contentPosition, pageNumber, null, false, userState));
				return;
			}
			this._asyncRequests.Add(new FlowDocumentPaginator.GetPageNumberAsyncRequest(textPointer, userState, this));
			this.InitiateNextAsyncOperation();
		}

		// Token: 0x06007012 RID: 28690 RVA: 0x00203190 File Offset: 0x00201390
		public override int GetPageNumber(ContentPosition contentPosition)
		{
			this._dispatcherObject.VerifyAccess();
			if (contentPosition == null)
			{
				throw new ArgumentNullException("contentPosition");
			}
			TextPointer textPointer = contentPosition as TextPointer;
			if (textPointer == null)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			if (textPointer.TextContainer != this._document.StructuralCache.TextContainer)
			{
				throw new ArgumentException(SR.Get("IDPInvalidContentPosition"), "contentPosition");
			}
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			int num;
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				num = 0;
				try
				{
					while (!this._brt.GetPageNumberForContentPosition(textPointer, ref num))
					{
						if (this._brt.IsClean)
						{
							num = -1;
							break;
						}
						this.FormatPage(num);
					}
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			return num;
		}

		// Token: 0x06007013 RID: 28691 RVA: 0x002032D4 File Offset: 0x002014D4
		public override ContentPosition GetPagePosition(DocumentPage page)
		{
			this._dispatcherObject.VerifyAccess();
			if (page == null)
			{
				throw new ArgumentNullException("page");
			}
			FlowDocumentPage flowDocumentPage = page as FlowDocumentPage;
			if (flowDocumentPage == null || flowDocumentPage.IsDisposed)
			{
				return ContentPosition.Missing;
			}
			Point point = new Point(0.0, 0.0);
			if (this._document.FlowDirection == FlowDirection.RightToLeft)
			{
				MatrixTransform matrixTransform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, flowDocumentPage.Size.Width, 0.0);
				Point point2;
				matrixTransform.TryTransform(point, out point2);
				point = point2;
			}
			ITextView textView = (ITextView)((IServiceProvider)flowDocumentPage).GetService(typeof(ITextView));
			Invariant.Assert(textView != null, "Cannot access ITextView for FlowDocumentPage.");
			if (textView.TextSegments.Count == 0)
			{
				return ContentPosition.Missing;
			}
			ITextPointer textPointer = textView.GetTextPositionFromPoint(point, true);
			if (textPointer == null)
			{
				textPointer = textView.TextSegments[0].Start;
			}
			if (!(textPointer is TextPointer))
			{
				return ContentPosition.Missing;
			}
			return (ContentPosition)textPointer;
		}

		// Token: 0x06007014 RID: 28692 RVA: 0x002033F7 File Offset: 0x002015F7
		public override ContentPosition GetObjectPosition(object o)
		{
			this._dispatcherObject.VerifyAccess();
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			return this._document.GetObjectPosition(o);
		}

		// Token: 0x06007015 RID: 28693 RVA: 0x00203420 File Offset: 0x00201620
		public override void CancelAsync(object userState)
		{
			if (userState == null)
			{
				this.CancelAllAsyncOperations();
				return;
			}
			for (int i = 0; i < this._asyncRequests.Count; i++)
			{
				FlowDocumentPaginator.AsyncRequest asyncRequest = this._asyncRequests[i];
				if (asyncRequest.UserState == userState)
				{
					asyncRequest.Cancel();
					this._asyncRequests.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x17001A9D RID: 6813
		// (get) Token: 0x06007016 RID: 28694 RVA: 0x00203479 File Offset: 0x00201679
		public override bool IsPageCountValid
		{
			get
			{
				this._dispatcherObject.VerifyAccess();
				return this._brt.IsClean;
			}
		}

		// Token: 0x17001A9E RID: 6814
		// (get) Token: 0x06007017 RID: 28695 RVA: 0x00203491 File Offset: 0x00201691
		public override int PageCount
		{
			get
			{
				this._dispatcherObject.VerifyAccess();
				return this._brt.Count;
			}
		}

		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x06007018 RID: 28696 RVA: 0x002034A9 File Offset: 0x002016A9
		// (set) Token: 0x06007019 RID: 28697 RVA: 0x002034B4 File Offset: 0x002016B4
		public override Size PageSize
		{
			get
			{
				return this._pageSize;
			}
			set
			{
				this._dispatcherObject.VerifyAccess();
				Size pageSize = value;
				if (DoubleUtil.IsNaN(pageSize.Width))
				{
					pageSize.Width = FlowDocumentPaginator._defaultPageSize.Width;
				}
				if (DoubleUtil.IsNaN(pageSize.Height))
				{
					pageSize.Height = FlowDocumentPaginator._defaultPageSize.Height;
				}
				Size size = this.ComputePageSize();
				this._pageSize = pageSize;
				Size size2 = this.ComputePageSize();
				if (!DoubleUtil.AreClose(size, size2))
				{
					if (this._document.StructuralCache.IsFormattingInProgress)
					{
						this._document.StructuralCache.OnInvalidOperationDetected();
						throw new InvalidOperationException(SR.Get("FlowDocumentInvalidContnetChange"));
					}
					this.InvalidateBRT();
				}
			}
		}

		// Token: 0x17001AA0 RID: 6816
		// (get) Token: 0x0600701A RID: 28698 RVA: 0x00203562 File Offset: 0x00201762
		// (set) Token: 0x0600701B RID: 28699 RVA: 0x0020356A File Offset: 0x0020176A
		public override bool IsBackgroundPaginationEnabled
		{
			get
			{
				return this._backgroundPagination;
			}
			set
			{
				this._dispatcherObject.VerifyAccess();
				if (value != this._backgroundPagination)
				{
					this._backgroundPagination = value;
					this.InitiateNextAsyncOperation();
				}
				if (!this._backgroundPagination)
				{
					this.CancelAllAsyncOperations();
				}
			}
		}

		// Token: 0x17001AA1 RID: 6817
		// (get) Token: 0x0600701C RID: 28700 RVA: 0x0020359B File Offset: 0x0020179B
		public override IDocumentPaginatorSource Source
		{
			get
			{
				return this._document;
			}
		}

		// Token: 0x0600701D RID: 28701 RVA: 0x002035A4 File Offset: 0x002017A4
		internal void InitiateNextAsyncOperation()
		{
			if (this._backgroundPagination && this._backgroundPaginationOperation == null && (!this._brt.IsClean || this._asyncRequests.Count > 0))
			{
				this._backgroundPaginationOperation = this._document.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnBackgroundPagination), this);
			}
		}

		// Token: 0x0600701E RID: 28702 RVA: 0x00203600 File Offset: 0x00201800
		internal void CancelAllAsyncOperations()
		{
			for (int i = 0; i < this._asyncRequests.Count; i++)
			{
				this._asyncRequests[i].Cancel();
			}
			this._asyncRequests.Clear();
		}

		// Token: 0x0600701F RID: 28703 RVA: 0x0020363F File Offset: 0x0020183F
		internal void OnPagesChanged(int pageStart, int pageCount)
		{
			this.OnPagesChanged(new PagesChangedEventArgs(pageStart, pageCount));
		}

		// Token: 0x06007020 RID: 28704 RVA: 0x0020364E File Offset: 0x0020184E
		internal void OnPaginationProgress(int pageStart, int pageCount)
		{
			this.OnPaginationProgress(new PaginationProgressEventArgs(pageStart, pageCount));
		}

		// Token: 0x06007021 RID: 28705 RVA: 0x0020365D File Offset: 0x0020185D
		internal void OnPaginationCompleted()
		{
			this.OnPaginationCompleted(EventArgs.Empty);
		}

		// Token: 0x1400013B RID: 315
		// (add) Token: 0x06007022 RID: 28706 RVA: 0x0020366C File Offset: 0x0020186C
		// (remove) Token: 0x06007023 RID: 28707 RVA: 0x002036A4 File Offset: 0x002018A4
		internal event BreakRecordTableInvalidatedEventHandler BreakRecordTableInvalidated;

		// Token: 0x06007024 RID: 28708 RVA: 0x002036D9 File Offset: 0x002018D9
		private void InvalidateBRT()
		{
			if (this.BreakRecordTableInvalidated != null)
			{
				this.BreakRecordTableInvalidated(this, EventArgs.Empty);
			}
			this._brt.OnInvalidateLayout();
		}

		// Token: 0x06007025 RID: 28709 RVA: 0x002036FF File Offset: 0x002018FF
		private void InvalidateBRTLayout(ITextPointer start, ITextPointer end)
		{
			this._brt.OnInvalidateLayout(start, end);
		}

		// Token: 0x06007026 RID: 28710 RVA: 0x00203710 File Offset: 0x00201910
		private DocumentPage FormatPagesTill(int pageNumber)
		{
			while (!this._brt.HasPageBreakRecord(pageNumber) && !this._brt.IsClean)
			{
				this.FormatPage(this._brt.Count);
			}
			if (this._brt.IsClean)
			{
				return DocumentPage.Missing;
			}
			return this.FormatPage(pageNumber);
		}

		// Token: 0x06007027 RID: 28711 RVA: 0x00203768 File Offset: 0x00201968
		private DocumentPage FormatPage(int pageNumber)
		{
			Invariant.Assert(this._brt.HasPageBreakRecord(pageNumber), "BreakRecord for specified page number does not exist.");
			PageBreakRecord pageBreakRecord = this._brt.GetPageBreakRecord(pageNumber);
			FlowDocumentPage flowDocumentPage = new FlowDocumentPage(this._document.StructuralCache);
			Size size = this.ComputePageSize();
			Thickness pageMargin = this._document.ComputePageMargin();
			PageBreakRecord brOut = flowDocumentPage.FormatFinite(size, pageMargin, pageBreakRecord);
			flowDocumentPage.Arrange(size);
			this._brt.UpdateEntry(pageNumber, flowDocumentPage, brOut, flowDocumentPage.DependentMax);
			return flowDocumentPage;
		}

		// Token: 0x06007028 RID: 28712 RVA: 0x002037E8 File Offset: 0x002019E8
		private object OnBackgroundPagination(object arg)
		{
			DateTime now = DateTime.Now;
			this._backgroundPaginationOperation = null;
			this._dispatcherObject.VerifyAccess();
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.PtsContext.Disposed)
			{
				return null;
			}
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				try
				{
					for (int i = 0; i < this._asyncRequests.Count; i++)
					{
						FlowDocumentPaginator.AsyncRequest asyncRequest = this._asyncRequests[i];
						if (asyncRequest.Process())
						{
							this._asyncRequests.RemoveAt(i);
							i--;
						}
					}
					DateTime now2 = DateTime.Now;
					if (this._backgroundPagination && !this._brt.IsClean)
					{
						while (!this._brt.IsClean)
						{
							this.FormatPage(this._brt.Count);
							long num = (DateTime.Now.Ticks - now.Ticks) / 10000L;
							if (num > 30L)
							{
								break;
							}
						}
						this.InitiateNextAsyncOperation();
					}
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			return null;
		}

		// Token: 0x06007029 RID: 28713 RVA: 0x00203950 File Offset: 0x00201B50
		private Size ComputePageSize()
		{
			Size result = new Size(this._document.PageWidth, this._document.PageHeight);
			if (DoubleUtil.IsNaN(result.Width))
			{
				result.Width = this._pageSize.Width;
				double num = this._document.MaxPageWidth;
				if (result.Width > num)
				{
					result.Width = num;
				}
				double num2 = this._document.MinPageWidth;
				if (result.Width < num2)
				{
					result.Width = num2;
				}
			}
			if (DoubleUtil.IsNaN(result.Height))
			{
				result.Height = this._pageSize.Height;
				double num = this._document.MaxPageHeight;
				if (result.Height > num)
				{
					result.Height = num;
				}
				double num2 = this._document.MinPageHeight;
				if (result.Height < num2)
				{
					result.Height = num2;
				}
			}
			return result;
		}

		// Token: 0x0600702A RID: 28714 RVA: 0x00203A33 File Offset: 0x00201C33
		object IServiceProvider.GetService(Type serviceType)
		{
			return ((IServiceProvider)this._document).GetService(serviceType);
		}

		// Token: 0x0600702B RID: 28715 RVA: 0x00203A41 File Offset: 0x00201C41
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout)
		{
			if (affectsLayout)
			{
				this.InvalidateBRT();
				return;
			}
			this._brt.OnInvalidateRender();
		}

		// Token: 0x0600702C RID: 28716 RVA: 0x00203A58 File Offset: 0x00201C58
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end)
		{
			if (affectsLayout)
			{
				this.InvalidateBRTLayout(start, end);
				return;
			}
			this._brt.OnInvalidateRender(start, end);
		}

		// Token: 0x0600702D RID: 28717 RVA: 0x00203A73 File Offset: 0x00201C73
		void IFlowDocumentFormatter.Suspend()
		{
			this.IsBackgroundPaginationEnabled = false;
			this.InvalidateBRT();
		}

		// Token: 0x17001AA2 RID: 6818
		// (get) Token: 0x0600702E RID: 28718 RVA: 0x00203A82 File Offset: 0x00201C82
		bool IFlowDocumentFormatter.IsLayoutDataValid
		{
			get
			{
				return !this._document.StructuralCache.IsContentChangeInProgress;
			}
		}

		// Token: 0x040036E5 RID: 14053
		private readonly FlowDocument _document;

		// Token: 0x040036E6 RID: 14054
		private readonly FlowDocumentPaginator.CustomDispatcherObject _dispatcherObject;

		// Token: 0x040036E7 RID: 14055
		private readonly BreakRecordTable _brt;

		// Token: 0x040036E8 RID: 14056
		private Size _pageSize;

		// Token: 0x040036E9 RID: 14057
		private bool _backgroundPagination;

		// Token: 0x040036EA RID: 14058
		private const int _paginationTimeout = 30;

		// Token: 0x040036EB RID: 14059
		private static Size _defaultPageSize = new Size(816.0, 1056.0);

		// Token: 0x040036EC RID: 14060
		private List<FlowDocumentPaginator.AsyncRequest> _asyncRequests = new List<FlowDocumentPaginator.AsyncRequest>(0);

		// Token: 0x040036ED RID: 14061
		private DispatcherOperation _backgroundPaginationOperation;

		// Token: 0x02000B37 RID: 2871
		private abstract class AsyncRequest
		{
			// Token: 0x06008D6A RID: 36202 RVA: 0x0025956D File Offset: 0x0025776D
			internal AsyncRequest(object userState, FlowDocumentPaginator paginator)
			{
				this.UserState = userState;
				this.Paginator = paginator;
			}

			// Token: 0x06008D6B RID: 36203
			internal abstract void Cancel();

			// Token: 0x06008D6C RID: 36204
			internal abstract bool Process();

			// Token: 0x04004AA4 RID: 19108
			internal readonly object UserState;

			// Token: 0x04004AA5 RID: 19109
			protected readonly FlowDocumentPaginator Paginator;
		}

		// Token: 0x02000B38 RID: 2872
		private class GetPageAsyncRequest : FlowDocumentPaginator.AsyncRequest
		{
			// Token: 0x06008D6D RID: 36205 RVA: 0x00259583 File Offset: 0x00257783
			internal GetPageAsyncRequest(int pageNumber, object userState, FlowDocumentPaginator paginator) : base(userState, paginator)
			{
				this.PageNumber = pageNumber;
			}

			// Token: 0x06008D6E RID: 36206 RVA: 0x00259594 File Offset: 0x00257794
			internal override void Cancel()
			{
				this.Paginator.OnGetPageCompleted(new GetPageCompletedEventArgs(null, this.PageNumber, null, true, this.UserState));
			}

			// Token: 0x06008D6F RID: 36207 RVA: 0x002595B8 File Offset: 0x002577B8
			internal override bool Process()
			{
				if (!this.Paginator._brt.HasPageBreakRecord(this.PageNumber))
				{
					return false;
				}
				DocumentPage page = this.Paginator.FormatPage(this.PageNumber);
				this.Paginator.OnGetPageCompleted(new GetPageCompletedEventArgs(page, this.PageNumber, null, false, this.UserState));
				return true;
			}

			// Token: 0x04004AA6 RID: 19110
			internal readonly int PageNumber;
		}

		// Token: 0x02000B39 RID: 2873
		private class GetPageNumberAsyncRequest : FlowDocumentPaginator.AsyncRequest
		{
			// Token: 0x06008D70 RID: 36208 RVA: 0x00259611 File Offset: 0x00257811
			internal GetPageNumberAsyncRequest(TextPointer textPointer, object userState, FlowDocumentPaginator paginator) : base(userState, paginator)
			{
				this.TextPointer = textPointer;
			}

			// Token: 0x06008D71 RID: 36209 RVA: 0x00259622 File Offset: 0x00257822
			internal override void Cancel()
			{
				this.Paginator.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(this.TextPointer, -1, null, true, this.UserState));
			}

			// Token: 0x06008D72 RID: 36210 RVA: 0x00259644 File Offset: 0x00257844
			internal override bool Process()
			{
				int pageNumber = 0;
				if (!this.Paginator._brt.GetPageNumberForContentPosition(this.TextPointer, ref pageNumber))
				{
					return false;
				}
				this.Paginator.OnGetPageNumberCompleted(new GetPageNumberCompletedEventArgs(this.TextPointer, pageNumber, null, false, this.UserState));
				return true;
			}

			// Token: 0x04004AA7 RID: 19111
			internal readonly TextPointer TextPointer;
		}

		// Token: 0x02000B3A RID: 2874
		private class CustomDispatcherObject : DispatcherObject
		{
		}
	}
}
