using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MS.Utility;

namespace MS.Internal.Documents
{
	// Token: 0x020006BC RID: 1724
	internal class DocumentGrid : FrameworkElement, IDocumentScrollInfo, IScrollInfo
	{
		// Token: 0x06006F09 RID: 28425 RVA: 0x001FE58F File Offset: 0x001FC78F
		static DocumentGrid()
		{
			EventManager.RegisterClassHandler(typeof(DocumentGrid), FrameworkElement.RequestBringIntoViewEvent, new RequestBringIntoViewEventHandler(DocumentGrid.OnRequestBringIntoView));
			DocumentGridContextMenu.RegisterClassHandler();
		}

		// Token: 0x06006F0A RID: 28426 RVA: 0x001FE5B8 File Offset: 0x001FC7B8
		public DocumentGrid()
		{
			this.Initialize();
		}

		// Token: 0x06006F0B RID: 28427 RVA: 0x001FE618 File Offset: 0x001FC818
		internal DocumentPage GetDocumentPageFromPoint(Point point)
		{
			DocumentPageView documentPageViewFromPoint = this.GetDocumentPageViewFromPoint(point);
			if (documentPageViewFromPoint != null)
			{
				return documentPageViewFromPoint.DocumentPage;
			}
			return null;
		}

		// Token: 0x06006F0C RID: 28428 RVA: 0x001FE638 File Offset: 0x001FC838
		public void LineUp()
		{
			if (this._canVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset - 16.0);
			}
		}

		// Token: 0x06006F0D RID: 28429 RVA: 0x001FE658 File Offset: 0x001FC858
		public void LineDown()
		{
			if (this._canVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset + 16.0);
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLineDown);
			}
		}

		// Token: 0x06006F0E RID: 28430 RVA: 0x001FE684 File Offset: 0x001FC884
		public void LineLeft()
		{
			if (this._canHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset - 16.0);
			}
		}

		// Token: 0x06006F0F RID: 28431 RVA: 0x001FE6A4 File Offset: 0x001FC8A4
		public void LineRight()
		{
			if (this._canHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset + 16.0);
			}
		}

		// Token: 0x06006F10 RID: 28432 RVA: 0x001FE6C4 File Offset: 0x001FC8C4
		public void PageUp()
		{
			this.SetVerticalOffsetInternal(this.VerticalOffset - this.ViewportHeight);
		}

		// Token: 0x06006F11 RID: 28433 RVA: 0x001FE6D9 File Offset: 0x001FC8D9
		public void PageDown()
		{
			this.SetVerticalOffsetInternal(this.VerticalOffset + this.ViewportHeight);
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageDown, (int)this.VerticalOffset);
		}

		// Token: 0x06006F12 RID: 28434 RVA: 0x001FE706 File Offset: 0x001FC906
		public void PageLeft()
		{
			this.SetHorizontalOffsetInternal(this.HorizontalOffset - this.ViewportWidth);
		}

		// Token: 0x06006F13 RID: 28435 RVA: 0x001FE71B File Offset: 0x001FC91B
		public void PageRight()
		{
			this.SetHorizontalOffsetInternal(this.HorizontalOffset + this.ViewportWidth);
		}

		// Token: 0x06006F14 RID: 28436 RVA: 0x001FE730 File Offset: 0x001FC930
		public void MouseWheelUp()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset - this.MouseWheelVerticalScrollAmount);
				return;
			}
			this.PageUp();
		}

		// Token: 0x06006F15 RID: 28437 RVA: 0x001FE754 File Offset: 0x001FC954
		public void MouseWheelDown()
		{
			if (this.CanMouseWheelVerticallyScroll)
			{
				this.SetVerticalOffsetInternal(this.VerticalOffset + this.MouseWheelVerticalScrollAmount);
				return;
			}
			this.PageDown();
		}

		// Token: 0x06006F16 RID: 28438 RVA: 0x001FE778 File Offset: 0x001FC978
		public void MouseWheelLeft()
		{
			if (this.CanMouseWheelHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset - this.MouseWheelHorizontalScrollAmount);
				return;
			}
			this.PageLeft();
		}

		// Token: 0x06006F17 RID: 28439 RVA: 0x001FE79C File Offset: 0x001FC99C
		public void MouseWheelRight()
		{
			if (this.CanMouseWheelHorizontallyScroll)
			{
				this.SetHorizontalOffsetInternal(this.HorizontalOffset + this.MouseWheelHorizontalScrollAmount);
				return;
			}
			this.PageRight();
		}

		// Token: 0x06006F18 RID: 28440 RVA: 0x001FE7C0 File Offset: 0x001FC9C0
		public Rect MakeVisible(Visual v, Rect r)
		{
			if (this.Content != null && v != null)
			{
				ContentPosition objectPosition = this.Content.GetObjectPosition(v);
				this.MakeContentPositionVisibleAsync(new DocumentGrid.MakeVisibleData(v, objectPosition, r));
			}
			return r;
		}

		// Token: 0x06006F19 RID: 28441 RVA: 0x001FE7F4 File Offset: 0x001FC9F4
		public Rect MakeVisible(object o, Rect r, int pageNumber)
		{
			ContentPosition objectPosition = this.Content.GetObjectPosition(o);
			this.MakeVisibleAsync(new DocumentGrid.MakeVisibleData(o as Visual, objectPosition, r), pageNumber);
			return r;
		}

		// Token: 0x06006F1A RID: 28442 RVA: 0x001FE824 File Offset: 0x001FCA24
		public void MakeSelectionVisible()
		{
			if (this.TextEditor != null && this.TextEditor.Selection != null)
			{
				ITextPointer textPointer = this.TextEditor.Selection.Start;
				textPointer = textPointer.CreatePointer(LogicalDirection.Forward);
				ContentPosition contentPosition = textPointer as ContentPosition;
				this.MakeContentPositionVisibleAsync(new DocumentGrid.MakeVisibleData(null, contentPosition, Rect.Empty));
			}
		}

		// Token: 0x06006F1B RID: 28443 RVA: 0x001FE878 File Offset: 0x001FCA78
		public void MakePageVisible(int pageNumber)
		{
			if (Math.Abs(pageNumber - this._firstVisiblePageNumber) > 1)
			{
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageJump, this._firstVisiblePageNumber, pageNumber);
			}
			if (pageNumber < 0)
			{
				this.SetVerticalOffsetInternal(0.0);
				this.SetHorizontalOffsetInternal(0.0);
				return;
			}
			if (pageNumber < this._pageCache.PageCount && this._rowCache.RowCount != 0)
			{
				RowInfo rowForPageNumber = this._rowCache.GetRowForPageNumber(pageNumber);
				this.SetVerticalOffsetInternal(rowForPageNumber.VerticalOffset);
				double horizontalOffsetForPage = this.GetHorizontalOffsetForPage(rowForPageNumber, pageNumber);
				this.SetHorizontalOffsetInternal(horizontalOffsetForPage);
				return;
			}
			if (this._pageCache.IsPaginationCompleted && this._rowCache.HasValidLayout)
			{
				this.SetVerticalOffsetInternal(this.ExtentHeight);
				this.SetHorizontalOffsetInternal(this.ExtentWidth);
				return;
			}
			this._pageJumpAfterLayout = true;
			this._pageJumpAfterLayoutPageNumber = pageNumber;
		}

		// Token: 0x06006F1C RID: 28444 RVA: 0x001FE95C File Offset: 0x001FCB5C
		public void ScrollToNextRow()
		{
			int num = this._firstVisibleRow + 1;
			if (num < this._rowCache.RowCount)
			{
				RowInfo row = this._rowCache.GetRow(num);
				this.SetVerticalOffsetInternal(row.VerticalOffset);
			}
		}

		// Token: 0x06006F1D RID: 28445 RVA: 0x001FE99C File Offset: 0x001FCB9C
		public void ScrollToPreviousRow()
		{
			int num = this._firstVisibleRow - 1;
			if (num >= 0 && num < this._rowCache.RowCount)
			{
				RowInfo row = this._rowCache.GetRow(num);
				this.SetVerticalOffsetInternal(row.VerticalOffset);
			}
		}

		// Token: 0x06006F1E RID: 28446 RVA: 0x001FE9DD File Offset: 0x001FCBDD
		public void ScrollToHome()
		{
			this.SetVerticalOffsetInternal(0.0);
		}

		// Token: 0x06006F1F RID: 28447 RVA: 0x001FE9EE File Offset: 0x001FCBEE
		public void ScrollToEnd()
		{
			this.SetVerticalOffsetInternal(this.ExtentHeight);
		}

		// Token: 0x06006F20 RID: 28448 RVA: 0x001FE9FC File Offset: 0x001FCBFC
		public void SetScale(double scale)
		{
			if (!DoubleUtil.AreClose(scale, this.Scale))
			{
				if (scale <= 0.0)
				{
					throw new ArgumentOutOfRangeException("scale");
				}
				if (!Helper.IsDoubleValid(scale))
				{
					throw new ArgumentOutOfRangeException("scale");
				}
				this.QueueSetScale(scale);
			}
		}

		// Token: 0x06006F21 RID: 28449 RVA: 0x001FEA48 File Offset: 0x001FCC48
		public void SetColumns(int columns)
		{
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(columns, DocumentGrid.ViewMode.SetColumns));
		}

		// Token: 0x06006F22 RID: 28450 RVA: 0x001FEA72 File Offset: 0x001FCC72
		public void FitColumns(int columns)
		{
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(columns, DocumentGrid.ViewMode.FitColumns));
		}

		// Token: 0x06006F23 RID: 28451 RVA: 0x001FEA9C File Offset: 0x001FCC9C
		public void FitToPageWidth()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.PageWidth));
		}

		// Token: 0x06006F24 RID: 28452 RVA: 0x001FEAB7 File Offset: 0x001FCCB7
		public void FitToPageHeight()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.PageHeight));
		}

		// Token: 0x06006F25 RID: 28453 RVA: 0x001FEAD2 File Offset: 0x001FCCD2
		public void ViewThumbnails()
		{
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXLayoutBegin);
			this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.Thumbnails));
		}

		// Token: 0x17001A5E RID: 6750
		// (get) Token: 0x06006F26 RID: 28454 RVA: 0x001FEAED File Offset: 0x001FCCED
		// (set) Token: 0x06006F27 RID: 28455 RVA: 0x001FEAF5 File Offset: 0x001FCCF5
		public bool CanHorizontallyScroll
		{
			get
			{
				return this._canHorizontallyScroll;
			}
			set
			{
				this._canHorizontallyScroll = value;
			}
		}

		// Token: 0x17001A5F RID: 6751
		// (get) Token: 0x06006F28 RID: 28456 RVA: 0x001FEAFE File Offset: 0x001FCCFE
		// (set) Token: 0x06006F29 RID: 28457 RVA: 0x001FEB06 File Offset: 0x001FCD06
		public bool CanVerticallyScroll
		{
			get
			{
				return this._canVerticallyScroll;
			}
			set
			{
				this._canVerticallyScroll = value;
			}
		}

		// Token: 0x17001A60 RID: 6752
		// (get) Token: 0x06006F2A RID: 28458 RVA: 0x001FEB0F File Offset: 0x001FCD0F
		public double ExtentWidth
		{
			get
			{
				return this._rowCache.ExtentWidth;
			}
		}

		// Token: 0x17001A61 RID: 6753
		// (get) Token: 0x06006F2B RID: 28459 RVA: 0x001FEB1C File Offset: 0x001FCD1C
		public double ExtentHeight
		{
			get
			{
				return this._rowCache.ExtentHeight;
			}
		}

		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x06006F2C RID: 28460 RVA: 0x001FEB29 File Offset: 0x001FCD29
		public double ViewportWidth
		{
			get
			{
				return this._viewportWidth;
			}
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x06006F2D RID: 28461 RVA: 0x001FEB31 File Offset: 0x001FCD31
		public double ViewportHeight
		{
			get
			{
				return this._viewportHeight;
			}
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x06006F2E RID: 28462 RVA: 0x001FEB3C File Offset: 0x001FCD3C
		public double HorizontalOffset
		{
			get
			{
				double val = Math.Min(this._horizontalOffset, this.ExtentWidth - this.ViewportWidth);
				return Math.Max(val, 0.0);
			}
		}

		// Token: 0x06006F2F RID: 28463 RVA: 0x001FEB73 File Offset: 0x001FCD73
		public void SetHorizontalOffset(double offset)
		{
			if (!DoubleUtil.AreClose(this._horizontalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (this._documentLayoutsPending == 0)
				{
					this.SetHorizontalOffsetInternal(offset);
					return;
				}
				this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(offset, DocumentGrid.ViewMode.SetHorizontalOffset));
			}
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x06006F30 RID: 28464 RVA: 0x001FEBB4 File Offset: 0x001FCDB4
		public double VerticalOffset
		{
			get
			{
				double val = Math.Min(this._verticalOffset, this.ExtentHeight - this.ViewportHeight);
				return Math.Max(val, 0.0);
			}
		}

		// Token: 0x06006F31 RID: 28465 RVA: 0x001FEBEB File Offset: 0x001FCDEB
		public void SetVerticalOffset(double offset)
		{
			if (!DoubleUtil.AreClose(this._verticalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				if (this._documentLayoutsPending == 0)
				{
					this.SetVerticalOffsetInternal(offset);
					return;
				}
				this.QueueUpdateDocumentLayout(new DocumentGrid.DocumentLayout(offset, DocumentGrid.ViewMode.SetVerticalOffset));
			}
		}

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x06006F32 RID: 28466 RVA: 0x001FEC2B File Offset: 0x001FCE2B
		// (set) Token: 0x06006F33 RID: 28467 RVA: 0x001FEC38 File Offset: 0x001FCE38
		public DynamicDocumentPaginator Content
		{
			get
			{
				return this._pageCache.Content;
			}
			set
			{
				if (value != this._pageCache.Content)
				{
					this._textContainer = null;
					if (this._pageCache.Content != null)
					{
						this._pageCache.Content.GetPageNumberCompleted -= this.OnGetPageNumberCompleted;
					}
					if (this.ScrollOwner != null)
					{
						this.ScrollOwner.ScrollChanged -= new ScrollChangedEventHandler(this.OnScrollChanged);
						this._scrollChangedEventAttached = false;
					}
					this._pageCache.Content = value;
					if (this._pageCache.Content != null)
					{
						this._pageCache.Content.GetPageNumberCompleted += this.OnGetPageNumberCompleted;
					}
					this.ResetVisualTree(false);
					this.ResetPageViewCollection();
					this._firstVisiblePageNumber = 0;
					this._lastVisiblePageNumber = 0;
					EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
					this._lastRowChangeExtentWidth = 0.0;
					this._lastRowChangeVerticalOffset = 0.0;
					if (this._documentLayout.ViewMode == DocumentGrid.ViewMode.Thumbnails)
					{
						this._documentLayout.ViewMode = DocumentGrid.ViewMode.SetColumns;
					}
					this.QueueUpdateDocumentLayout(this._documentLayout);
					base.InvalidateMeasure();
					this.InvalidateDocumentScrollInfo();
				}
			}
		}

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x06006F34 RID: 28468 RVA: 0x001FED6E File Offset: 0x001FCF6E
		public int PageCount
		{
			get
			{
				return this._pageCache.PageCount;
			}
		}

		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x06006F35 RID: 28469 RVA: 0x001FED7B File Offset: 0x001FCF7B
		public int FirstVisiblePageNumber
		{
			get
			{
				return this._firstVisiblePageNumber;
			}
		}

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x06006F36 RID: 28470 RVA: 0x001FED83 File Offset: 0x001FCF83
		public double Scale
		{
			get
			{
				return this._rowCache.Scale;
			}
		}

		// Token: 0x17001A6A RID: 6762
		// (get) Token: 0x06006F37 RID: 28471 RVA: 0x001FED90 File Offset: 0x001FCF90
		public int MaxPagesAcross
		{
			get
			{
				return this._maxPagesAcross;
			}
		}

		// Token: 0x17001A6B RID: 6763
		// (get) Token: 0x06006F38 RID: 28472 RVA: 0x001FED98 File Offset: 0x001FCF98
		// (set) Token: 0x06006F39 RID: 28473 RVA: 0x001FEDA5 File Offset: 0x001FCFA5
		public double VerticalPageSpacing
		{
			get
			{
				return this._rowCache.VerticalPageSpacing;
			}
			set
			{
				if (!Helper.IsDoubleValid(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._rowCache.VerticalPageSpacing = value;
			}
		}

		// Token: 0x17001A6C RID: 6764
		// (get) Token: 0x06006F3A RID: 28474 RVA: 0x001FEDC6 File Offset: 0x001FCFC6
		// (set) Token: 0x06006F3B RID: 28475 RVA: 0x001FEDD3 File Offset: 0x001FCFD3
		public double HorizontalPageSpacing
		{
			get
			{
				return this._rowCache.HorizontalPageSpacing;
			}
			set
			{
				if (!Helper.IsDoubleValid(value))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._rowCache.HorizontalPageSpacing = value;
			}
		}

		// Token: 0x17001A6D RID: 6765
		// (get) Token: 0x06006F3C RID: 28476 RVA: 0x001FEDF4 File Offset: 0x001FCFF4
		// (set) Token: 0x06006F3D RID: 28477 RVA: 0x001FEDFC File Offset: 0x001FCFFC
		public bool ShowPageBorders
		{
			get
			{
				return this._showPageBorders;
			}
			set
			{
				if (this._showPageBorders != value)
				{
					this._showPageBorders = value;
					int count = this._childrenCollection.Count;
					for (int i = 0; i < count; i++)
					{
						DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
						if (documentGridPage != null)
						{
							documentGridPage.ShowPageBorders = this._showPageBorders;
						}
					}
				}
			}
		}

		// Token: 0x17001A6E RID: 6766
		// (get) Token: 0x06006F3E RID: 28478 RVA: 0x001FEE52 File Offset: 0x001FD052
		// (set) Token: 0x06006F3F RID: 28479 RVA: 0x001FEE5A File Offset: 0x001FD05A
		public bool LockViewModes
		{
			get
			{
				return this._lockViewModes;
			}
			set
			{
				this._lockViewModes = value;
			}
		}

		// Token: 0x17001A6F RID: 6767
		// (get) Token: 0x06006F40 RID: 28480 RVA: 0x001FEE64 File Offset: 0x001FD064
		public ITextContainer TextContainer
		{
			get
			{
				if (this._textContainer == null && this.Content != null)
				{
					IServiceProvider serviceProvider = this.Content as IServiceProvider;
					if (serviceProvider != null)
					{
						this._textContainer = (ITextContainer)serviceProvider.GetService(typeof(ITextContainer));
					}
				}
				return this._textContainer;
			}
		}

		// Token: 0x17001A70 RID: 6768
		// (get) Token: 0x06006F41 RID: 28481 RVA: 0x001FEEB1 File Offset: 0x001FD0B1
		public ITextView TextView
		{
			get
			{
				if (this.TextEditor != null)
				{
					return this.TextEditor.TextView;
				}
				return null;
			}
		}

		// Token: 0x17001A71 RID: 6769
		// (get) Token: 0x06006F42 RID: 28482 RVA: 0x001FEEC8 File Offset: 0x001FD0C8
		public ReadOnlyCollection<DocumentPageView> PageViews
		{
			get
			{
				return this._pageViews;
			}
		}

		// Token: 0x17001A72 RID: 6770
		// (get) Token: 0x06006F43 RID: 28483 RVA: 0x001FEED0 File Offset: 0x001FD0D0
		// (set) Token: 0x06006F44 RID: 28484 RVA: 0x001FEED8 File Offset: 0x001FD0D8
		public ScrollViewer ScrollOwner
		{
			get
			{
				return this._scrollOwner;
			}
			set
			{
				this._scrollOwner = value;
				this.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x17001A73 RID: 6771
		// (get) Token: 0x06006F45 RID: 28485 RVA: 0x001FEEE7 File Offset: 0x001FD0E7
		// (set) Token: 0x06006F46 RID: 28486 RVA: 0x001FEEEF File Offset: 0x001FD0EF
		public DocumentViewer DocumentViewerOwner
		{
			get
			{
				return this._documentViewerOwner;
			}
			set
			{
				this._documentViewerOwner = value;
			}
		}

		// Token: 0x06006F47 RID: 28487 RVA: 0x001FEEF8 File Offset: 0x001FD0F8
		protected override Visual GetVisualChild(int index)
		{
			if (this._childrenCollection == null || index < 0 || index >= this._childrenCollection.Count)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._childrenCollection[index];
		}

		// Token: 0x17001A74 RID: 6772
		// (get) Token: 0x06006F48 RID: 28488 RVA: 0x001FEF46 File Offset: 0x001FD146
		protected override int VisualChildrenCount
		{
			get
			{
				return this._childrenCollection.Count;
			}
		}

		// Token: 0x06006F49 RID: 28489 RVA: 0x001FEF54 File Offset: 0x001FD154
		protected override Size MeasureOverride(Size constraint)
		{
			if (double.IsInfinity(constraint.Width) || double.IsInfinity(constraint.Height))
			{
				constraint = this._defaultConstraint;
			}
			this.RecalculateVisualPages(this.VerticalOffset, constraint);
			int count = this._childrenCollection.Count;
			for (int i = 0; i < count; i++)
			{
				if (i == 0)
				{
					Border border = this._childrenCollection[i] as Border;
					if (border == null)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeContainsNonBorderAsFirstElement"));
					}
					border.Measure(constraint);
				}
				else
				{
					DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
					if (documentGridPage == null)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeContainsNonDocumentGridPage"));
					}
					Size pageSize = this._pageCache.GetPageSize(documentGridPage.PageNumber);
					pageSize.Width *= this.Scale;
					pageSize.Height *= this.Scale;
					if (!documentGridPage.IsMeasureValid)
					{
						documentGridPage.Measure(pageSize);
						Size pageSize2 = this._pageCache.GetPageSize(documentGridPage.PageNumber);
						if (pageSize2 != Size.Empty)
						{
							pageSize2.Width *= this.Scale;
							pageSize2.Height *= this.Scale;
							if (pageSize2.Width != pageSize.Width || pageSize2.Height != pageSize.Height)
							{
								documentGridPage.Measure(pageSize2);
							}
						}
					}
				}
			}
			return constraint;
		}

		// Token: 0x06006F4A RID: 28490 RVA: 0x001FF0C8 File Offset: 0x001FD2C8
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			if (this._viewportHeight != arrangeSize.Height || this._viewportWidth != arrangeSize.Width)
			{
				this._viewportWidth = arrangeSize.Width;
				this._viewportHeight = arrangeSize.Height;
				if (this.LockViewModes && this.IsViewLoaded() && this._firstVisiblePageNumber < this._pageCache.PageCount && this._rowCache.HasValidLayout)
				{
					this.ApplyViewParameters(this._rowCache.GetRowForPageNumber(this._firstVisiblePageNumber));
					this.MeasureOverride(arrangeSize);
				}
				this.UpdateTextView();
			}
			if (this.IsViewportNonzero && this.ExecutePendingLayoutRequests())
			{
				this.MeasureOverride(arrangeSize);
			}
			if (this._previousConstraint != arrangeSize)
			{
				this._previousConstraint = arrangeSize;
				this.InvalidateDocumentScrollInfo();
			}
			if (this._childrenCollection.Count == 0)
			{
				return arrangeSize;
			}
			UIElement uielement = this._childrenCollection[0] as UIElement;
			uielement.Arrange(new Rect(new Point(0.0, 0.0), arrangeSize));
			int num = 1;
			for (int i = this._firstVisibleRow; i < this._firstVisibleRow + this._visibleRowCount; i++)
			{
				double num2;
				double y;
				this.CalculateRowOffsets(i, out num2, out y);
				RowInfo row = this._rowCache.GetRow(i);
				for (int j = row.FirstPage; j < row.FirstPage + row.PageCount; j++)
				{
					if (num > this._childrenCollection.Count - 1)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeOutOfSync"));
					}
					Size pageSize = this._pageCache.GetPageSize(j);
					pageSize.Width *= this.Scale;
					pageSize.Height *= this.Scale;
					UIElement uielement2 = this._childrenCollection[num] as UIElement;
					if (uielement2 == null)
					{
						throw new InvalidOperationException(SR.Get("DocumentGridVisualTreeContainsNonUIElement"));
					}
					Point location;
					if (this._pageCache.IsContentRightToLeft)
					{
						location = new Point(Math.Max(this.ViewportWidth, this.ExtentWidth) - (num2 + pageSize.Width), y);
					}
					else
					{
						location = new Point(num2, y);
					}
					uielement2.Arrange(new Rect(location, pageSize));
					num2 += pageSize.Width + this.HorizontalPageSpacing;
					num++;
				}
			}
			AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
			if (adornerLayer != null && adornerLayer.GetAdorners(this) != null)
			{
				adornerLayer.Update(this);
			}
			return arrangeSize;
		}

		// Token: 0x06006F4B RID: 28491 RVA: 0x001FF350 File Offset: 0x001FD550
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			bool flag = Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
			if (flag && this._rubberBandSelector == null)
			{
				IServiceProvider serviceProvider = this.Content as IServiceProvider;
				if (serviceProvider != null)
				{
					this._rubberBandSelector = (serviceProvider.GetService(typeof(RubberbandSelector)) as RubberbandSelector);
					if (this._rubberBandSelector != null)
					{
						this.DocumentViewerOwner.Focus();
						ITextRange selection = this.TextEditor.Selection;
						selection.Select(selection.Start, selection.Start);
						this.DocumentViewerOwner.IsSelectionEnabled = false;
						this._rubberBandSelector.AttachRubberbandSelector(this);
						return;
					}
				}
			}
			else if (!flag && this._rubberBandSelector != null)
			{
				if (this._rubberBandSelector != null)
				{
					this._rubberBandSelector.DetachRubberbandSelector();
					this._rubberBandSelector = null;
				}
				this.DocumentViewerOwner.IsSelectionEnabled = true;
			}
		}

		// Token: 0x06006F4C RID: 28492 RVA: 0x001FF428 File Offset: 0x001FD628
		protected internal override void OnVisualParentChanged(DependencyObject oldParent)
		{
			base.OnVisualParentChanged(oldParent);
			if (VisualTreeHelper.GetParent(this) != null)
			{
				this.ResetVisualTree(false);
			}
		}

		// Token: 0x06006F4D RID: 28493 RVA: 0x001FF440 File Offset: 0x001FD640
		private void RecalculateVisualPages(double offset, Size constraint)
		{
			if (this._rowCache.RowCount == 0)
			{
				this.ResetVisualTree(false);
				this.ResetPageViewCollection();
				this._firstVisibleRow = 0;
				this._visibleRowCount = 0;
				this._firstVisiblePageNumber = 0;
				this._lastVisiblePageNumber = 0;
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
				return;
			}
			int num = 0;
			int num2 = 0;
			this._rowCache.GetVisibleRowIndices(offset, offset + constraint.Height, out num, out num2);
			if (num2 == 0)
			{
				this.ResetVisualTree(false);
				this.ResetPageViewCollection();
				this._firstVisibleRow = 0;
				this._visibleRowCount = 0;
				this._firstVisiblePageNumber = 0;
				this._lastVisiblePageNumber = 0;
				EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
				return;
			}
			int num3 = -1;
			int num4 = -1;
			if (this._childrenCollection.Count > 1)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[1] as DocumentGridPage;
				num3 = ((documentGridPage != null) ? documentGridPage.PageNumber : -1);
				DocumentGridPage documentGridPage2 = this._childrenCollection[this._childrenCollection.Count - 1] as DocumentGridPage;
				num4 = ((documentGridPage2 != null) ? documentGridPage2.PageNumber : -1);
			}
			RowInfo row = this._rowCache.GetRow(num);
			this._firstVisiblePageNumber = row.FirstPage;
			RowInfo row2 = this._rowCache.GetRow(num + num2 - 1);
			this._lastVisiblePageNumber = row2.FirstPage + row2.PageCount - 1;
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXPageVisible, this._firstVisiblePageNumber, this._lastVisiblePageNumber);
			this._firstVisibleRow = num;
			this._visibleRowCount = num2;
			if (this._firstVisiblePageNumber != num3 || this._lastVisiblePageNumber != num4)
			{
				ArrayList arrayList = new ArrayList();
				for (int i = this._firstVisibleRow; i < this._firstVisibleRow + this._visibleRowCount; i++)
				{
					RowInfo row3 = this._rowCache.GetRow(i);
					for (int j = row3.FirstPage; j < row3.FirstPage + row3.PageCount; j++)
					{
						if (j < num3 || j > num4 || this._childrenCollection.Count <= 1)
						{
							DocumentGridPage documentGridPage3 = new DocumentGridPage(this.Content);
							documentGridPage3.ShowPageBorders = this.ShowPageBorders;
							documentGridPage3.PageNumber = j;
							documentGridPage3.PageLoaded += this.OnPageLoaded;
							arrayList.Add(documentGridPage3);
						}
						else
						{
							arrayList.Add(this._childrenCollection[1 + j - Math.Max(0, num3)]);
						}
					}
				}
				this.ResetVisualTree(true);
				Collection<DocumentPageView> collection = new Collection<DocumentPageView>();
				DocumentGrid.VisualTreeModificationState visualTreeModificationState = DocumentGrid.VisualTreeModificationState.BeforeExisting;
				int num5 = 1;
				for (int k = 0; k < arrayList.Count; k++)
				{
					Visual visual = (Visual)arrayList[k];
					switch (visualTreeModificationState)
					{
					case DocumentGrid.VisualTreeModificationState.BeforeExisting:
						if (num5 < this._childrenCollection.Count && this._childrenCollection[num5] == visual)
						{
							visualTreeModificationState = DocumentGrid.VisualTreeModificationState.DuringExisting;
						}
						else
						{
							this._childrenCollection.Insert(num5, visual);
						}
						num5++;
						break;
					case DocumentGrid.VisualTreeModificationState.DuringExisting:
						if (num5 >= this._childrenCollection.Count || this._childrenCollection[num5] != visual)
						{
							visualTreeModificationState = DocumentGrid.VisualTreeModificationState.AfterExisting;
							this._childrenCollection.Add(visual);
						}
						num5++;
						break;
					case DocumentGrid.VisualTreeModificationState.AfterExisting:
						this._childrenCollection.Add(visual);
						break;
					}
					collection.Add(((DocumentGridPage)arrayList[k]).DocumentPageView);
				}
				this._pageViews = new ReadOnlyCollection<DocumentPageView>(collection);
				this.InvalidatePageViews();
				this.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x06006F4E RID: 28494 RVA: 0x001FF7E8 File Offset: 0x001FD9E8
		private void OnPageLoaded(object sender, EventArgs args)
		{
			DocumentGridPage documentGridPage = sender as DocumentGridPage;
			Invariant.Assert(documentGridPage != null, "Invalid sender for OnPageLoaded event.");
			documentGridPage.PageLoaded -= this.OnPageLoaded;
			if (this._makeVisiblePageNeeded == documentGridPage.PageNumber)
			{
				this._makeVisiblePageNeeded = -1;
				this._makeVisibleDispatcher.Priority = DispatcherPriority.Background;
			}
			if (EventTrace.IsEnabled(EventTrace.Keyword.KeywordXPS, EventTrace.Level.Info))
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientDRXPageLoaded, EventTrace.Keyword.KeywordXPS, EventTrace.Level.Info, documentGridPage.PageNumber);
			}
		}

		// Token: 0x06006F4F RID: 28495 RVA: 0x001FF868 File Offset: 0x001FDA68
		private void CalculateRowOffsets(int row, out double xOffset, out double yOffset)
		{
			xOffset = 0.0;
			yOffset = 0.0;
			RowInfo row2 = this._rowCache.GetRow(row);
			double num = Math.Max(this.ViewportWidth, this.ExtentWidth);
			if (row == this._rowCache.RowCount - 1 && !this._pageCache.DynamicPageSizes)
			{
				xOffset = (num - this.ExtentWidth) / 2.0 + this.HorizontalPageSpacing / 2.0 - this.HorizontalOffset;
			}
			else
			{
				xOffset = (num - row2.RowSize.Width) / 2.0 + this.HorizontalPageSpacing / 2.0 - this.HorizontalOffset;
			}
			if (this.ExtentHeight > this.ViewportHeight)
			{
				yOffset = row2.VerticalOffset + this.VerticalPageSpacing / 2.0 - this.VerticalOffset;
				return;
			}
			yOffset = row2.VerticalOffset + (this.ViewportHeight - this.ExtentHeight) / 2.0 + this.VerticalPageSpacing / 2.0;
		}

		// Token: 0x06006F50 RID: 28496 RVA: 0x001FF98C File Offset: 0x001FDB8C
		private void ResetVisualTree(bool pruneOnly)
		{
			for (int i = this._childrenCollection.Count - 1; i >= 1; i--)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
				if (documentGridPage != null && (!pruneOnly || this._rowCache.RowCount == 0 || documentGridPage.PageNumber < this._firstVisiblePageNumber || documentGridPage.PageNumber > this._lastVisiblePageNumber))
				{
					this._childrenCollection.Remove(documentGridPage);
					documentGridPage.PageLoaded -= this.OnPageLoaded;
					((IDisposable)documentGridPage).Dispose();
				}
			}
			if (this._documentGridBackground == null)
			{
				this._documentGridBackground = new Border();
				this._documentGridBackground.Background = Brushes.Transparent;
				this._childrenCollection.Add(this._documentGridBackground);
			}
		}

		// Token: 0x06006F51 RID: 28497 RVA: 0x001FFA4B File Offset: 0x001FDC4B
		private void ResetPageViewCollection()
		{
			this._pageViews = null;
			this.InvalidatePageViews();
		}

		// Token: 0x06006F52 RID: 28498 RVA: 0x001FFA5C File Offset: 0x001FDC5C
		private void OnGetPageNumberCompleted(object sender, GetPageNumberCompletedEventArgs e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e.UserState is DocumentGrid.MakeVisibleData)
			{
				DocumentGrid.MakeVisibleData data = (DocumentGrid.MakeVisibleData)e.UserState;
				this.MakeVisibleAsync(data, e.PageNumber);
			}
		}

		// Token: 0x06006F53 RID: 28499 RVA: 0x001FFA9D File Offset: 0x001FDC9D
		private void MakeVisibleAsync(DocumentGrid.MakeVisibleData data, int pageNumber)
		{
			base.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DocumentGrid.BringPageIntoViewCallback(this.BringPageIntoViewDelegate), data, new object[]
			{
				pageNumber
			});
		}

		// Token: 0x06006F54 RID: 28500 RVA: 0x001FFAD0 File Offset: 0x001FDCD0
		private void BringPageIntoViewDelegate(DocumentGrid.MakeVisibleData data, int pageNumber)
		{
			if (!this._rowCache.HasValidLayout || (data.Visual is FixedPage && data.Visual.VisualContentBounds == data.Rect) || pageNumber < this._firstVisiblePageNumber || pageNumber > this._lastVisiblePageNumber)
			{
				this.MakePageVisible(pageNumber);
			}
			if (this.IsPageLoaded(pageNumber))
			{
				this.MakeVisibleImpl(data);
				return;
			}
			this._makeVisiblePageNeeded = pageNumber;
			this._makeVisibleDispatcher = base.Dispatcher.BeginInvoke(DispatcherPriority.Inactive, new DispatcherOperationCallback(delegate(object arg)
			{
				this.MakeVisibleImpl((DocumentGrid.MakeVisibleData)arg);
				return null;
			}), data);
		}

		// Token: 0x06006F55 RID: 28501 RVA: 0x001FFB68 File Offset: 0x001FDD68
		private void MakeVisibleImpl(DocumentGrid.MakeVisibleData data)
		{
			if (data.Visual != null)
			{
				if (base.IsAncestorOf(data.Visual))
				{
					GeneralTransform generalTransform = data.Visual.TransformToAncestor(this);
					Rect rect = (data.Rect != Rect.Empty) ? data.Rect : data.Visual.VisualContentBounds;
					Rect r = generalTransform.TransformBounds(rect);
					this.MakeRectVisible(r, false);
					return;
				}
			}
			else if (data.ContentPosition != null)
			{
				ITextPointer textPointer = data.ContentPosition as ITextPointer;
				if (this.TextViewContains(textPointer))
				{
					this.MakeRectVisible(this.TextView.GetRectangleFromTextPosition(textPointer), false);
					return;
				}
			}
			else
			{
				Invariant.Assert(false, "Invalid object brought into view.");
			}
		}

		// Token: 0x06006F56 RID: 28502 RVA: 0x001FFC18 File Offset: 0x001FDE18
		private void MakeRectVisible(Rect r, bool alwaysCenter)
		{
			if (r != Rect.Empty)
			{
				Rect rect = new Rect(this.HorizontalOffset + r.X, this.VerticalOffset + r.Y, r.Width, r.Height);
				Rect rect2 = new Rect(this.HorizontalOffset, this.VerticalOffset, this.ViewportWidth, this.ViewportHeight);
				if (alwaysCenter || !rect.IntersectsWith(rect2))
				{
					this.SetHorizontalOffsetInternal(rect.X - this.ViewportWidth / 2.0);
					this.SetVerticalOffsetInternal(rect.Y - this.ViewportHeight / 2.0);
				}
			}
		}

		// Token: 0x06006F57 RID: 28503 RVA: 0x001FFCD0 File Offset: 0x001FDED0
		private void MakeIPVisible(Rect r)
		{
			if (r != Rect.Empty && this.TextEditor != null)
			{
				Rect rect = new Rect(this.HorizontalOffset, this.VerticalOffset, this.ViewportWidth, this.ViewportHeight);
				if (!rect.Contains(r))
				{
					if (r.X < this.HorizontalOffset)
					{
						this.SetHorizontalOffsetInternal(this.HorizontalOffset - (this.HorizontalOffset - r.X));
					}
					else if (r.X > this.HorizontalOffset + this.ViewportWidth)
					{
						this.SetHorizontalOffsetInternal(this.HorizontalOffset + (r.X - (this.HorizontalOffset + this.ViewportWidth)));
					}
					if (r.Y < this.VerticalOffset)
					{
						this.SetVerticalOffsetInternal(this.VerticalOffset - (this.VerticalOffset - r.Y));
						return;
					}
					if (r.Y + r.Height > this.VerticalOffset + this.ViewportHeight)
					{
						this.SetVerticalOffsetInternal(this.VerticalOffset + (r.Y + r.Height - (this.VerticalOffset + this.ViewportHeight)));
					}
				}
			}
		}

		// Token: 0x06006F58 RID: 28504 RVA: 0x001FFDFB File Offset: 0x001FDFFB
		private void MakeContentPositionVisibleAsync(DocumentGrid.MakeVisibleData data)
		{
			if (data.ContentPosition != null && data.ContentPosition != ContentPosition.Missing)
			{
				this.Content.GetPageNumberAsync(data.ContentPosition, data);
			}
		}

		// Token: 0x06006F59 RID: 28505 RVA: 0x001FFE2C File Offset: 0x001FE02C
		private void QueueSetScale(double scale)
		{
			if (this._setScaleOperation != null && this._setScaleOperation.Status == DispatcherOperationStatus.Pending)
			{
				this._setScaleOperation.Abort();
			}
			this._setScaleOperation = Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.SetScaleDelegate), scale);
		}

		// Token: 0x06006F5A RID: 28506 RVA: 0x001FFE80 File Offset: 0x001FE080
		private object SetScaleDelegate(object scale)
		{
			if (!(scale is double))
			{
				return null;
			}
			double scale2 = (double)scale;
			this._documentLayout.ViewMode = DocumentGrid.ViewMode.Zoom;
			ITextPointer visibleSelection = this.GetVisibleSelection();
			if (visibleSelection != null)
			{
				int pageNumberForVisibleSelection = this.GetPageNumberForVisibleSelection(visibleSelection);
				this.UpdateLayoutScale(scale2);
				this.MakePageVisible(pageNumberForVisibleSelection);
				base.LayoutUpdated += this.OnZoomLayoutUpdated;
			}
			else
			{
				this.UpdateLayoutScale(scale2);
			}
			return null;
		}

		// Token: 0x06006F5B RID: 28507 RVA: 0x001FFEE8 File Offset: 0x001FE0E8
		private void UpdateLayoutScale(double scale)
		{
			if (!DoubleUtil.AreClose(scale, this.Scale))
			{
				double extentHeight = this.ExtentHeight;
				double extentWidth = this.ExtentWidth;
				this._rowCache.Scale = scale;
				double num = (extentHeight == 0.0) ? 1.0 : (this.ExtentHeight / extentHeight);
				double num2 = (extentWidth == 0.0) ? 1.0 : (this.ExtentWidth / extentWidth);
				this.SetVerticalOffsetInternal(this._verticalOffset * num);
				this.SetHorizontalOffsetInternal(this._horizontalOffset * num2);
				base.InvalidateMeasure();
				this.InvalidateChildMeasure();
				this.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x06006F5C RID: 28508 RVA: 0x001FFF8E File Offset: 0x001FE18E
		private void QueueUpdateDocumentLayout(DocumentGrid.DocumentLayout layout)
		{
			this._documentLayoutsPending++;
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.UpdateDocumentLayoutDelegate), layout);
		}

		// Token: 0x06006F5D RID: 28509 RVA: 0x001FFFB7 File Offset: 0x001FE1B7
		private object UpdateDocumentLayoutDelegate(object layout)
		{
			if (layout is DocumentGrid.DocumentLayout)
			{
				this.UpdateDocumentLayout((DocumentGrid.DocumentLayout)layout);
			}
			this._documentLayoutsPending--;
			return null;
		}

		// Token: 0x06006F5E RID: 28510 RVA: 0x001FFFDC File Offset: 0x001FE1DC
		private void UpdateDocumentLayout(DocumentGrid.DocumentLayout layout)
		{
			if (layout.ViewMode == DocumentGrid.ViewMode.SetHorizontalOffset)
			{
				this.SetHorizontalOffsetInternal(layout.Offset);
				return;
			}
			if (layout.ViewMode == DocumentGrid.ViewMode.SetVerticalOffset)
			{
				this.SetVerticalOffsetInternal(layout.Offset);
				return;
			}
			this._documentLayout = layout;
			this._maxPagesAcross = this._documentLayout.Columns;
			if (this.IsViewportNonzero)
			{
				if (this._documentLayout.ViewMode == DocumentGrid.ViewMode.Thumbnails)
				{
					this._maxPagesAcross = (this._documentLayout.Columns = this.CalculateThumbnailColumns());
				}
				int activeFocusPage = this.GetActiveFocusPage();
				this._rowCache.RecalcRows(activeFocusPage, this._documentLayout.Columns);
				this._isLayoutRequested = false;
				return;
			}
			this._isLayoutRequested = true;
		}

		// Token: 0x06006F5F RID: 28511 RVA: 0x0020008A File Offset: 0x001FE28A
		private bool ExecutePendingLayoutRequests()
		{
			if (this._isLayoutRequested)
			{
				this.UpdateDocumentLayout(this._documentLayout);
				return true;
			}
			return false;
		}

		// Token: 0x06006F60 RID: 28512 RVA: 0x002000A3 File Offset: 0x001FE2A3
		private void SetHorizontalOffsetInternal(double offset)
		{
			if (!DoubleUtil.AreClose(this._horizontalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this._horizontalOffset = offset;
				base.InvalidateMeasure();
				this.InvalidateDocumentScrollInfo();
				this.UpdateTextView();
			}
		}

		// Token: 0x06006F61 RID: 28513 RVA: 0x002000DF File Offset: 0x001FE2DF
		private void SetVerticalOffsetInternal(double offset)
		{
			if (!DoubleUtil.AreClose(this._verticalOffset, offset))
			{
				if (double.IsNaN(offset))
				{
					throw new ArgumentOutOfRangeException("offset");
				}
				this._verticalOffset = offset;
				base.InvalidateMeasure();
				this.InvalidateDocumentScrollInfo();
				this.UpdateTextView();
			}
		}

		// Token: 0x06006F62 RID: 28514 RVA: 0x0020011C File Offset: 0x001FE31C
		private void UpdateTextView()
		{
			MultiPageTextView multiPageTextView = this.TextView as MultiPageTextView;
			if (multiPageTextView != null)
			{
				multiPageTextView.OnPageLayoutChanged();
			}
		}

		// Token: 0x06006F63 RID: 28515 RVA: 0x00200140 File Offset: 0x001FE340
		private int CalculateThumbnailColumns()
		{
			if (!this.IsViewportNonzero)
			{
				return 1;
			}
			if (this._pageCache.PageCount == 0)
			{
				return 1;
			}
			Size pageSize = this._pageCache.GetPageSize(0);
			double num = this.ViewportWidth / this.ViewportHeight;
			int num2 = (int)Math.Floor(this.ViewportWidth / (this.CurrentMinimumScale * pageSize.Width + this.HorizontalPageSpacing));
			num2 = Math.Min(num2, this._pageCache.PageCount);
			num2 = Math.Min(num2, DocumentViewerConstants.MaximumMaxPagesAcross);
			int result = 1;
			double num3 = double.MaxValue;
			for (int i = 1; i <= num2; i++)
			{
				int num4 = (int)Math.Floor((double)(this._pageCache.PageCount / i));
				double num5 = pageSize.Width * (double)i;
				double num6 = pageSize.Height * (double)num4;
				double num7 = num5 / num6;
				double num8 = Math.Abs(num7 - num);
				if (num8 < num3)
				{
					num3 = num8;
					result = i;
				}
			}
			return result;
		}

		// Token: 0x06006F64 RID: 28516 RVA: 0x00200230 File Offset: 0x001FE430
		private void InvalidateChildMeasure()
		{
			int count = this._childrenCollection.Count;
			for (int i = 0; i < count; i++)
			{
				UIElement uielement = this._childrenCollection[i] as UIElement;
				if (uielement != null)
				{
					uielement.InvalidateMeasure();
				}
			}
		}

		// Token: 0x06006F65 RID: 28517 RVA: 0x00200270 File Offset: 0x001FE470
		private bool RowIsClean(RowInfo row)
		{
			bool result = true;
			for (int i = row.FirstPage; i < row.FirstPage + row.PageCount; i++)
			{
				if (this._pageCache.IsPageDirty(i))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06006F66 RID: 28518 RVA: 0x002002B0 File Offset: 0x001FE4B0
		private void EnsureFit(RowInfo pivotRow)
		{
			double num = this.CalculateScaleFactor(pivotRow);
			double num2 = num * this._rowCache.Scale;
			if (num2 < this.CurrentMinimumScale || num2 > DocumentViewerConstants.MaximumScale)
			{
				return;
			}
			if (!DoubleUtil.AreClose(1.0, num))
			{
				this.ApplyViewParameters(pivotRow);
				this.SetVerticalOffsetInternal(pivotRow.VerticalOffset);
			}
		}

		// Token: 0x06006F67 RID: 28519 RVA: 0x0020030C File Offset: 0x001FE50C
		private void ApplyViewParameters(RowInfo pivotRow)
		{
			if (this._pageCache.DynamicPageSizes)
			{
				this._maxPagesAcross = pivotRow.PageCount;
			}
			double num = this.CalculateScaleFactor(pivotRow);
			double num2 = num * this._rowCache.Scale;
			num2 = Math.Max(num2, this.CurrentMinimumScale);
			num2 = Math.Min(num2, DocumentViewerConstants.MaximumScale);
			this.UpdateLayoutScale(num2);
		}

		// Token: 0x06006F68 RID: 28520 RVA: 0x00200368 File Offset: 0x001FE568
		private double CalculateScaleFactor(RowInfo pivotRow)
		{
			double num;
			if (this._pageCache.DynamicPageSizes)
			{
				num = pivotRow.RowSize.Width - (double)pivotRow.PageCount * this.HorizontalPageSpacing;
			}
			else
			{
				num = this.ExtentWidth - (double)this.MaxPagesAcross * this.HorizontalPageSpacing;
			}
			double num2 = pivotRow.RowSize.Height - this.VerticalPageSpacing;
			if (num <= 0.0 || num2 <= 0.0)
			{
				return 1.0;
			}
			double num3;
			if (this._pageCache.DynamicPageSizes)
			{
				num3 = this.ViewportWidth - (double)pivotRow.PageCount * this.HorizontalPageSpacing;
			}
			else
			{
				num3 = this.ViewportWidth - (double)this.MaxPagesAcross * this.HorizontalPageSpacing;
			}
			double num4 = this.ViewportHeight - this.VerticalPageSpacing;
			if (num3 <= 0.0 || num4 <= 0.0)
			{
				return 1.0;
			}
			double result = 1.0;
			switch (this._documentLayout.ViewMode)
			{
			case DocumentGrid.ViewMode.SetColumns:
			case DocumentGrid.ViewMode.Zoom:
				break;
			case DocumentGrid.ViewMode.FitColumns:
				result = Math.Min(num3 / num, num4 / num2);
				break;
			case DocumentGrid.ViewMode.PageWidth:
				result = num3 / num;
				break;
			case DocumentGrid.ViewMode.PageHeight:
				result = num4 / num2;
				break;
			case DocumentGrid.ViewMode.Thumbnails:
			{
				double num5 = this.ExtentHeight - this.VerticalPageSpacing * (double)this._rowCache.RowCount;
				double num6 = this.ViewportHeight - this.VerticalPageSpacing * (double)this._rowCache.RowCount;
				if (num6 <= 0.0)
				{
					result = 1.0;
				}
				else
				{
					result = Math.Min(num3 / num, num6 / num5);
				}
				break;
			}
			default:
				throw new InvalidOperationException(SR.Get("DocumentGridInvalidViewMode"));
			}
			return result;
		}

		// Token: 0x06006F69 RID: 28521 RVA: 0x00200530 File Offset: 0x001FE730
		private void Initialize()
		{
			this._pageCache = new PageCache();
			this._childrenCollection = new VisualCollection(this);
			this._rowCache = new RowCache();
			this._rowCache.PageCache = this._pageCache;
			this._rowCache.RowCacheChanged += this.OnRowCacheChanged;
			this._rowCache.RowLayoutCompleted += this.OnRowLayoutCompleted;
		}

		// Token: 0x06006F6A RID: 28522 RVA: 0x0020059E File Offset: 0x001FE79E
		private void InvalidateDocumentScrollInfo()
		{
			if (this.ScrollOwner != null)
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
			if (this.DocumentViewerOwner != null)
			{
				this.DocumentViewerOwner.InvalidateDocumentScrollInfo();
			}
		}

		// Token: 0x06006F6B RID: 28523 RVA: 0x002005C6 File Offset: 0x001FE7C6
		private void InvalidatePageViews()
		{
			Invariant.Assert(this.DocumentViewerOwner != null, "DocumentViewerOwner cannot be null.");
			if (this.DocumentViewerOwner != null)
			{
				this.DocumentViewerOwner.InvalidatePageViewsInternal();
				this.DocumentViewerOwner.ApplyTemplate();
			}
			EventTrace.EasyTraceEvent(EventTrace.Keyword.KeywordXPS, EventTrace.Event.WClientDRXInvalidateView);
		}

		// Token: 0x06006F6C RID: 28524 RVA: 0x00200608 File Offset: 0x001FE808
		private ITextPointer GetVisibleSelection()
		{
			ITextPointer result = null;
			if (this.HasSelection())
			{
				ITextPointer start = this.TextEditor.Selection.Start;
				if (this.TextViewContains(start))
				{
					result = start;
				}
			}
			return result;
		}

		// Token: 0x06006F6D RID: 28525 RVA: 0x0020063C File Offset: 0x001FE83C
		private bool HasSelection()
		{
			return this.TextEditor != null && this.TextEditor.Selection != null;
		}

		// Token: 0x06006F6E RID: 28526 RVA: 0x00200658 File Offset: 0x001FE858
		private int GetPageNumberForVisibleSelection(ITextPointer selection)
		{
			Invariant.Assert(this.TextViewContains(selection));
			foreach (DocumentPageView documentPageView in this._pageViews)
			{
				DocumentPageTextView documentPageTextView = ((IServiceProvider)documentPageView).GetService(typeof(ITextView)) as DocumentPageTextView;
				if (documentPageTextView != null && documentPageTextView.IsValid && documentPageTextView.Contains(selection))
				{
					return documentPageView.PageNumber;
				}
			}
			Invariant.Assert(false, "Selection was in TextView, but not found in any visible page!");
			return 0;
		}

		// Token: 0x06006F6F RID: 28527 RVA: 0x002006EC File Offset: 0x001FE8EC
		private Point GetActiveFocusPoint()
		{
			ITextPointer visibleSelection = this.GetVisibleSelection();
			if (visibleSelection != null && visibleSelection.HasValidLayout)
			{
				Rect rectangleFromTextPosition = this.TextView.GetRectangleFromTextPosition(visibleSelection);
				if (rectangleFromTextPosition != Rect.Empty)
				{
					return new Point(rectangleFromTextPosition.Left, rectangleFromTextPosition.Top);
				}
			}
			return new Point(0.0, 0.0);
		}

		// Token: 0x06006F70 RID: 28528 RVA: 0x00200750 File Offset: 0x001FE950
		private int GetActiveFocusPage()
		{
			DocumentPageView documentPageViewFromPoint = this.GetDocumentPageViewFromPoint(this.GetActiveFocusPoint());
			if (documentPageViewFromPoint != null)
			{
				return documentPageViewFromPoint.PageNumber;
			}
			return this._firstVisiblePageNumber;
		}

		// Token: 0x06006F71 RID: 28529 RVA: 0x0020077C File Offset: 0x001FE97C
		private DocumentPageView GetDocumentPageViewFromPoint(Point point)
		{
			HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);
			for (DependencyObject dependencyObject = (hitTestResult != null) ? hitTestResult.VisualHit : null; dependencyObject != null; dependencyObject = VisualTreeHelper.GetParent(dependencyObject))
			{
				DocumentPageView documentPageView = dependencyObject as DocumentPageView;
				if (documentPageView != null)
				{
					return documentPageView;
				}
			}
			return null;
		}

		// Token: 0x06006F72 RID: 28530 RVA: 0x002007B9 File Offset: 0x001FE9B9
		private bool TextViewContains(ITextPointer tp)
		{
			return this.TextView != null && this.TextView.IsValid && this.TextView.Contains(tp);
		}

		// Token: 0x06006F73 RID: 28531 RVA: 0x002007E0 File Offset: 0x001FE9E0
		private double GetHorizontalOffsetForPage(RowInfo row, int pageNumber)
		{
			if (row == null)
			{
				throw new ArgumentNullException("row");
			}
			if (pageNumber < row.FirstPage || pageNumber > row.FirstPage + row.PageCount)
			{
				throw new ArgumentOutOfRangeException("pageNumber");
			}
			double num = this._pageCache.DynamicPageSizes ? Math.Max(0.0, (this.ExtentWidth - row.RowSize.Width) / 2.0) : 0.0;
			for (int i = row.FirstPage; i < pageNumber; i++)
			{
				num += this._pageCache.GetPageSize(i).Width * this.Scale + this.HorizontalPageSpacing;
			}
			return num;
		}

		// Token: 0x06006F74 RID: 28532 RVA: 0x0020089C File Offset: 0x001FEA9C
		private bool RowCacheChangeIsVisible(RowCacheChange change)
		{
			int firstVisibleRow = this._firstVisibleRow;
			int num = this._firstVisibleRow + this._visibleRowCount;
			int start = change.Start;
			int num2 = change.Start + change.Count;
			return (start >= firstVisibleRow && start <= num) || (num2 >= firstVisibleRow && num2 <= num) || (start < firstVisibleRow && num2 > num);
		}

		// Token: 0x06006F75 RID: 28533 RVA: 0x002008F0 File Offset: 0x001FEAF0
		private bool IsPageLoaded(int pageNumber)
		{
			DocumentGridPage documentGridPageForPageNumber = this.GetDocumentGridPageForPageNumber(pageNumber);
			return documentGridPageForPageNumber != null && documentGridPageForPageNumber.IsPageLoaded;
		}

		// Token: 0x06006F76 RID: 28534 RVA: 0x00200910 File Offset: 0x001FEB10
		private bool IsViewLoaded()
		{
			bool result = true;
			for (int i = 1; i < this._childrenCollection.Count; i++)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
				if (documentGridPage != null && !documentGridPage.IsPageLoaded)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06006F77 RID: 28535 RVA: 0x00200958 File Offset: 0x001FEB58
		private DocumentGridPage GetDocumentGridPageForPageNumber(int pageNumber)
		{
			for (int i = 1; i < this._childrenCollection.Count; i++)
			{
				DocumentGridPage documentGridPage = this._childrenCollection[i] as DocumentGridPage;
				if (documentGridPage != null && documentGridPage.PageNumber == pageNumber)
				{
					return documentGridPage;
				}
			}
			return null;
		}

		// Token: 0x06006F78 RID: 28536 RVA: 0x0020099C File Offset: 0x001FEB9C
		private static void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
		{
			DocumentGrid documentGrid = sender as DocumentGrid;
			DocumentGrid documentGrid2 = args.TargetObject as DocumentGrid;
			if (documentGrid != null && documentGrid2 != null && documentGrid == documentGrid2)
			{
				args.Handled = true;
				documentGrid2.MakeIPVisible(args.TargetRect);
				return;
			}
			args.Handled = false;
		}

		// Token: 0x06006F79 RID: 28537 RVA: 0x002009E4 File Offset: 0x001FEBE4
		private void OnScrollChanged(object sender, EventArgs args)
		{
			if (this.ScrollOwner != null)
			{
				this._scrollChangedEventAttached = false;
				this.ScrollOwner.ScrollChanged -= new ScrollChangedEventHandler(this.OnScrollChanged);
			}
			if (this._rowCache.HasValidLayout)
			{
				this.EnsureFit(this._rowCache.GetRowForPageNumber(this.FirstVisiblePageNumber));
			}
		}

		// Token: 0x06006F7A RID: 28538 RVA: 0x00200A3C File Offset: 0x001FEC3C
		private void OnZoomLayoutUpdated(object sender, EventArgs args)
		{
			base.LayoutUpdated -= this.OnZoomLayoutUpdated;
			ITextPointer visibleSelection = this.GetVisibleSelection();
			if (visibleSelection != null)
			{
				this.MakeRectVisible(this.TextView.GetRectangleFromTextPosition(visibleSelection), true);
			}
		}

		// Token: 0x06006F7B RID: 28539 RVA: 0x00200A78 File Offset: 0x001FEC78
		private void OnRowCacheChanged(object source, RowCacheChangedEventArgs args)
		{
			if (this._savedPivotRow != null && this.RowIsClean(this._savedPivotRow))
			{
				if (this._documentLayout.ViewMode != DocumentGrid.ViewMode.Zoom && this._documentLayout.ViewMode != DocumentGrid.ViewMode.SetColumns)
				{
					if (this._savedPivotRow.FirstPage < this._rowCache.RowCount)
					{
						RowInfo rowForPageNumber = this._rowCache.GetRowForPageNumber(this._savedPivotRow.FirstPage);
						if (rowForPageNumber.RowSize.Width != this._savedPivotRow.RowSize.Width || rowForPageNumber.RowSize.Height != this._savedPivotRow.RowSize.Height)
						{
							this.ApplyViewParameters(rowForPageNumber);
						}
						this._savedPivotRow = null;
					}
				}
				else
				{
					this._savedPivotRow = null;
				}
			}
			if (this._pageCache.DynamicPageSizes && this._lastRowChangeVerticalOffset != this.VerticalOffset && this._lastRowChangeExtentWidth < this.ExtentWidth)
			{
				if (this._lastRowChangeExtentWidth != 0.0)
				{
					this.SetHorizontalOffsetInternal(this.HorizontalOffset + (this.ExtentWidth - this._lastRowChangeExtentWidth) / 2.0);
				}
				this._lastRowChangeExtentWidth = this.ExtentWidth;
			}
			this._lastRowChangeVerticalOffset = this.VerticalOffset;
			for (int i = 0; i < args.Changes.Count; i++)
			{
				RowCacheChange change = args.Changes[i];
				if (this.RowCacheChangeIsVisible(change))
				{
					base.InvalidateMeasure();
					this.InvalidateChildMeasure();
				}
			}
			this.InvalidateDocumentScrollInfo();
		}

		// Token: 0x06006F7C RID: 28540 RVA: 0x00200C04 File Offset: 0x001FEE04
		private void OnRowLayoutCompleted(object source, RowLayoutCompletedEventArgs args)
		{
			if (args == null)
			{
				return;
			}
			if (args.PivotRowIndex >= this._rowCache.RowCount)
			{
				throw new ArgumentOutOfRangeException("args");
			}
			RowInfo row = this._rowCache.GetRow(args.PivotRowIndex);
			if (!this.RowIsClean(row) && this._documentLayout.ViewMode != DocumentGrid.ViewMode.Zoom)
			{
				this._savedPivotRow = row;
			}
			else
			{
				this._savedPivotRow = null;
			}
			this.ApplyViewParameters(row);
			if (!this._firstRowLayout && !this._pageJumpAfterLayout)
			{
				this.MakePageVisible(row.FirstPage);
			}
			else if (this._pageJumpAfterLayout)
			{
				this.MakePageVisible(this._pageJumpAfterLayoutPageNumber);
				this._pageJumpAfterLayout = false;
			}
			this._firstRowLayout = false;
			if (!this._scrollChangedEventAttached && this.ScrollOwner != null && this._documentLayout.ViewMode != DocumentGrid.ViewMode.Zoom && this._documentLayout.ViewMode != DocumentGrid.ViewMode.SetColumns)
			{
				this._scrollChangedEventAttached = true;
				this.ScrollOwner.ScrollChanged += new ScrollChangedEventHandler(this.OnScrollChanged);
			}
		}

		// Token: 0x17001A75 RID: 6773
		// (get) Token: 0x06006F7D RID: 28541 RVA: 0x00200CFC File Offset: 0x001FEEFC
		private bool IsViewportNonzero
		{
			get
			{
				return this.ViewportWidth != 0.0 && this.ViewportHeight != 0.0;
			}
		}

		// Token: 0x17001A76 RID: 6774
		// (get) Token: 0x06006F7E RID: 28542 RVA: 0x00200D25 File Offset: 0x001FEF25
		private TextEditor TextEditor
		{
			get
			{
				if (this.DocumentViewerOwner != null)
				{
					return this.DocumentViewerOwner.TextEditor;
				}
				return null;
			}
		}

		// Token: 0x17001A77 RID: 6775
		// (get) Token: 0x06006F7F RID: 28543 RVA: 0x00200D3C File Offset: 0x001FEF3C
		private double MouseWheelVerticalScrollAmount
		{
			get
			{
				return 16.0 * (double)SystemParameters.WheelScrollLines;
			}
		}

		// Token: 0x17001A78 RID: 6776
		// (get) Token: 0x06006F80 RID: 28544 RVA: 0x00200D4E File Offset: 0x001FEF4E
		private bool CanMouseWheelVerticallyScroll
		{
			get
			{
				return this._canVerticallyScroll && SystemParameters.WheelScrollLines > 0;
			}
		}

		// Token: 0x17001A79 RID: 6777
		// (get) Token: 0x06006F81 RID: 28545 RVA: 0x00200D3C File Offset: 0x001FEF3C
		private double MouseWheelHorizontalScrollAmount
		{
			get
			{
				return 16.0 * (double)SystemParameters.WheelScrollLines;
			}
		}

		// Token: 0x17001A7A RID: 6778
		// (get) Token: 0x06006F82 RID: 28546 RVA: 0x00200D62 File Offset: 0x001FEF62
		private bool CanMouseWheelHorizontallyScroll
		{
			get
			{
				return this._canHorizontallyScroll && SystemParameters.WheelScrollLines > 0;
			}
		}

		// Token: 0x17001A7B RID: 6779
		// (get) Token: 0x06006F83 RID: 28547 RVA: 0x00200D76 File Offset: 0x001FEF76
		private double CurrentMinimumScale
		{
			get
			{
				if (this._documentLayout.ViewMode != DocumentGrid.ViewMode.Thumbnails)
				{
					return DocumentViewerConstants.MinimumScale;
				}
				return DocumentViewerConstants.MinimumThumbnailsScale;
			}
		}

		// Token: 0x04003692 RID: 13970
		private PageCache _pageCache;

		// Token: 0x04003693 RID: 13971
		private RowCache _rowCache;

		// Token: 0x04003694 RID: 13972
		private ReadOnlyCollection<DocumentPageView> _pageViews;

		// Token: 0x04003695 RID: 13973
		private bool _canHorizontallyScroll;

		// Token: 0x04003696 RID: 13974
		private bool _canVerticallyScroll;

		// Token: 0x04003697 RID: 13975
		private double _verticalOffset;

		// Token: 0x04003698 RID: 13976
		private double _horizontalOffset;

		// Token: 0x04003699 RID: 13977
		private double _viewportHeight;

		// Token: 0x0400369A RID: 13978
		private double _viewportWidth;

		// Token: 0x0400369B RID: 13979
		private int _firstVisibleRow;

		// Token: 0x0400369C RID: 13980
		private int _visibleRowCount;

		// Token: 0x0400369D RID: 13981
		private int _firstVisiblePageNumber;

		// Token: 0x0400369E RID: 13982
		private int _lastVisiblePageNumber;

		// Token: 0x0400369F RID: 13983
		private ScrollViewer _scrollOwner;

		// Token: 0x040036A0 RID: 13984
		private DocumentViewer _documentViewerOwner;

		// Token: 0x040036A1 RID: 13985
		private bool _showPageBorders = true;

		// Token: 0x040036A2 RID: 13986
		private bool _lockViewModes;

		// Token: 0x040036A3 RID: 13987
		private int _maxPagesAcross = 1;

		// Token: 0x040036A4 RID: 13988
		private Size _previousConstraint;

		// Token: 0x040036A5 RID: 13989
		private DocumentGrid.DocumentLayout _documentLayout = new DocumentGrid.DocumentLayout(1, DocumentGrid.ViewMode.SetColumns);

		// Token: 0x040036A6 RID: 13990
		private int _documentLayoutsPending;

		// Token: 0x040036A7 RID: 13991
		private RowInfo _savedPivotRow;

		// Token: 0x040036A8 RID: 13992
		private double _lastRowChangeExtentWidth;

		// Token: 0x040036A9 RID: 13993
		private double _lastRowChangeVerticalOffset;

		// Token: 0x040036AA RID: 13994
		private ITextContainer _textContainer;

		// Token: 0x040036AB RID: 13995
		private RubberbandSelector _rubberBandSelector;

		// Token: 0x040036AC RID: 13996
		private bool _isLayoutRequested;

		// Token: 0x040036AD RID: 13997
		private bool _pageJumpAfterLayout;

		// Token: 0x040036AE RID: 13998
		private int _pageJumpAfterLayoutPageNumber;

		// Token: 0x040036AF RID: 13999
		private bool _firstRowLayout = true;

		// Token: 0x040036B0 RID: 14000
		private bool _scrollChangedEventAttached;

		// Token: 0x040036B1 RID: 14001
		private Border _documentGridBackground;

		// Token: 0x040036B2 RID: 14002
		private const int _backgroundVisualIndex = 0;

		// Token: 0x040036B3 RID: 14003
		private const int _firstPageVisualIndex = 1;

		// Token: 0x040036B4 RID: 14004
		private readonly Size _defaultConstraint = new Size(250.0, 250.0);

		// Token: 0x040036B5 RID: 14005
		private VisualCollection _childrenCollection;

		// Token: 0x040036B6 RID: 14006
		private int _makeVisiblePageNeeded = -1;

		// Token: 0x040036B7 RID: 14007
		private DispatcherOperation _makeVisibleDispatcher;

		// Token: 0x040036B8 RID: 14008
		private DispatcherOperation _setScaleOperation;

		// Token: 0x040036B9 RID: 14009
		private const double _verticalLineScrollAmount = 16.0;

		// Token: 0x040036BA RID: 14010
		private const double _horizontalLineScrollAmount = 16.0;

		// Token: 0x02000B2D RID: 2861
		// (Invoke) Token: 0x06008D51 RID: 36177
		private delegate void BringPageIntoViewCallback(DocumentGrid.MakeVisibleData data, int pageNumber);

		// Token: 0x02000B2E RID: 2862
		private enum VisualTreeModificationState
		{
			// Token: 0x04004A85 RID: 19077
			BeforeExisting,
			// Token: 0x04004A86 RID: 19078
			DuringExisting,
			// Token: 0x04004A87 RID: 19079
			AfterExisting
		}

		// Token: 0x02000B2F RID: 2863
		private enum ViewMode
		{
			// Token: 0x04004A89 RID: 19081
			SetColumns,
			// Token: 0x04004A8A RID: 19082
			FitColumns,
			// Token: 0x04004A8B RID: 19083
			PageWidth,
			// Token: 0x04004A8C RID: 19084
			PageHeight,
			// Token: 0x04004A8D RID: 19085
			Thumbnails,
			// Token: 0x04004A8E RID: 19086
			Zoom,
			// Token: 0x04004A8F RID: 19087
			SetHorizontalOffset,
			// Token: 0x04004A90 RID: 19088
			SetVerticalOffset
		}

		// Token: 0x02000B30 RID: 2864
		private class DocumentLayout
		{
			// Token: 0x06008D54 RID: 36180 RVA: 0x00259380 File Offset: 0x00257580
			public DocumentLayout(int columns, DocumentGrid.ViewMode viewMode) : this(columns, 0.0, viewMode)
			{
			}

			// Token: 0x06008D55 RID: 36181 RVA: 0x00259393 File Offset: 0x00257593
			public DocumentLayout(double offset, DocumentGrid.ViewMode viewMode) : this(1, offset, viewMode)
			{
			}

			// Token: 0x06008D56 RID: 36182 RVA: 0x0025939E File Offset: 0x0025759E
			public DocumentLayout(int columns, double offset, DocumentGrid.ViewMode viewMode)
			{
				this._columns = columns;
				this._offset = offset;
				this._viewMode = viewMode;
			}

			// Token: 0x17001F72 RID: 8050
			// (get) Token: 0x06008D58 RID: 36184 RVA: 0x002593C4 File Offset: 0x002575C4
			// (set) Token: 0x06008D57 RID: 36183 RVA: 0x002593BB File Offset: 0x002575BB
			public DocumentGrid.ViewMode ViewMode
			{
				get
				{
					return this._viewMode;
				}
				set
				{
					this._viewMode = value;
				}
			}

			// Token: 0x17001F73 RID: 8051
			// (get) Token: 0x06008D5A RID: 36186 RVA: 0x002593D5 File Offset: 0x002575D5
			// (set) Token: 0x06008D59 RID: 36185 RVA: 0x002593CC File Offset: 0x002575CC
			public int Columns
			{
				get
				{
					return this._columns;
				}
				set
				{
					this._columns = value;
				}
			}

			// Token: 0x17001F74 RID: 8052
			// (get) Token: 0x06008D5B RID: 36187 RVA: 0x002593DD File Offset: 0x002575DD
			public double Offset
			{
				get
				{
					return this._offset;
				}
			}

			// Token: 0x04004A91 RID: 19089
			private DocumentGrid.ViewMode _viewMode;

			// Token: 0x04004A92 RID: 19090
			private int _columns;

			// Token: 0x04004A93 RID: 19091
			private double _offset;
		}

		// Token: 0x02000B31 RID: 2865
		private struct MakeVisibleData
		{
			// Token: 0x06008D5C RID: 36188 RVA: 0x002593E5 File Offset: 0x002575E5
			public MakeVisibleData(Visual visual, ContentPosition contentPosition, Rect rect)
			{
				this._visual = visual;
				this._contentPosition = contentPosition;
				this._rect = rect;
			}

			// Token: 0x17001F75 RID: 8053
			// (get) Token: 0x06008D5D RID: 36189 RVA: 0x002593FC File Offset: 0x002575FC
			public Visual Visual
			{
				get
				{
					return this._visual;
				}
			}

			// Token: 0x17001F76 RID: 8054
			// (get) Token: 0x06008D5E RID: 36190 RVA: 0x00259404 File Offset: 0x00257604
			public ContentPosition ContentPosition
			{
				get
				{
					return this._contentPosition;
				}
			}

			// Token: 0x17001F77 RID: 8055
			// (get) Token: 0x06008D5F RID: 36191 RVA: 0x0025940C File Offset: 0x0025760C
			public Rect Rect
			{
				get
				{
					return this._rect;
				}
			}

			// Token: 0x04004A94 RID: 19092
			private Visual _visual;

			// Token: 0x04004A95 RID: 19093
			private ContentPosition _contentPosition;

			// Token: 0x04004A96 RID: 19094
			private Rect _rect;
		}
	}
}
