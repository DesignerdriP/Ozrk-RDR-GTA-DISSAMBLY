using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.PtsHost;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006C7 RID: 1735
	internal class FlowDocumentFormatter : IFlowDocumentFormatter
	{
		// Token: 0x06006FFC RID: 28668 RVA: 0x002028E3 File Offset: 0x00200AE3
		internal FlowDocumentFormatter(FlowDocument document)
		{
			this._document = document;
			this._documentPage = new FlowDocumentPage(this._document.StructuralCache);
		}

		// Token: 0x06006FFD RID: 28669 RVA: 0x00202908 File Offset: 0x00200B08
		internal void Format(Size constraint)
		{
			if (this._document.StructuralCache.IsFormattingInProgress)
			{
				throw new InvalidOperationException(SR.Get("FlowDocumentFormattingReentrancy"));
			}
			if (this._document.StructuralCache.IsContentChangeInProgress)
			{
				throw new InvalidOperationException(SR.Get("TextContainerChangingReentrancyInvalid"));
			}
			if (this._document.StructuralCache.IsFormattedOnce)
			{
				if (!this._lastFormatSuccessful)
				{
					this._document.StructuralCache.InvalidateFormatCache(true);
				}
				if (!this._arrangedAfterFormat && (!this._document.StructuralCache.ForceReformat || !this._document.StructuralCache.DestroyStructure))
				{
					this._documentPage.Arrange(this._documentPage.ContentSize);
					this._documentPage.EnsureValidVisuals();
				}
			}
			this._arrangedAfterFormat = false;
			this._lastFormatSuccessful = false;
			this._isContentFormatValid = false;
			Size pageSize = this.ComputePageSize(constraint);
			Thickness pageMargin = this.ComputePageMargin();
			using (this._document.Dispatcher.DisableProcessing())
			{
				this._document.StructuralCache.IsFormattingInProgress = true;
				try
				{
					this._document.StructuralCache.BackgroundFormatInfo.ViewportHeight = constraint.Height;
					this._documentPage.FormatBottomless(pageSize, pageMargin);
				}
				finally
				{
					this._document.StructuralCache.IsFormattingInProgress = false;
				}
			}
			this._lastFormatSuccessful = true;
		}

		// Token: 0x06006FFE RID: 28670 RVA: 0x00202A88 File Offset: 0x00200C88
		internal void Arrange(Size arrangeSize, Rect viewport)
		{
			Invariant.Assert(this._document.StructuralCache.DtrList == null || this._document.StructuralCache.DtrList.Length == 0 || (this._document.StructuralCache.DtrList.Length == 1 && this._document.StructuralCache.BackgroundFormatInfo.DoesFinalDTRCoverRestOfText));
			this._documentPage.Arrange(arrangeSize);
			this._documentPage.EnsureValidVisuals();
			this._arrangedAfterFormat = true;
			if (viewport.IsEmpty)
			{
				viewport = new Rect(0.0, 0.0, arrangeSize.Width, this._document.StructuralCache.BackgroundFormatInfo.ViewportHeight);
			}
			PTS.FSRECT fsrect = new PTS.FSRECT(viewport);
			this._documentPage.UpdateViewport(ref fsrect, true);
			this._isContentFormatValid = true;
		}

		// Token: 0x17001A9B RID: 6811
		// (get) Token: 0x06006FFF RID: 28671 RVA: 0x00202B6F File Offset: 0x00200D6F
		internal FlowDocumentPage DocumentPage
		{
			get
			{
				return this._documentPage;
			}
		}

		// Token: 0x14000139 RID: 313
		// (add) Token: 0x06007000 RID: 28672 RVA: 0x00202B78 File Offset: 0x00200D78
		// (remove) Token: 0x06007001 RID: 28673 RVA: 0x00202BB0 File Offset: 0x00200DB0
		internal event EventHandler ContentInvalidated;

		// Token: 0x1400013A RID: 314
		// (add) Token: 0x06007002 RID: 28674 RVA: 0x00202BE8 File Offset: 0x00200DE8
		// (remove) Token: 0x06007003 RID: 28675 RVA: 0x00202C20 File Offset: 0x00200E20
		internal event EventHandler Suspended;

		// Token: 0x06007004 RID: 28676 RVA: 0x00202C58 File Offset: 0x00200E58
		private Size ComputePageSize(Size constraint)
		{
			Size result = new Size(this._document.PageWidth, double.PositiveInfinity);
			if (DoubleUtil.IsNaN(result.Width))
			{
				result.Width = constraint.Width;
				double maxPageWidth = this._document.MaxPageWidth;
				if (result.Width > maxPageWidth)
				{
					result.Width = maxPageWidth;
				}
				double minPageWidth = this._document.MinPageWidth;
				if (result.Width < minPageWidth)
				{
					result.Width = minPageWidth;
				}
			}
			if (double.IsPositiveInfinity(result.Width))
			{
				result.Width = 500.0;
			}
			return result;
		}

		// Token: 0x06007005 RID: 28677 RVA: 0x00202CF8 File Offset: 0x00200EF8
		private Thickness ComputePageMargin()
		{
			double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(this._document);
			Thickness pagePadding = this._document.PagePadding;
			if (DoubleUtil.IsNaN(pagePadding.Left))
			{
				pagePadding.Left = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Top))
			{
				pagePadding.Top = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Right))
			{
				pagePadding.Right = lineHeightValue;
			}
			if (DoubleUtil.IsNaN(pagePadding.Bottom))
			{
				pagePadding.Bottom = lineHeightValue;
			}
			return pagePadding;
		}

		// Token: 0x06007006 RID: 28678 RVA: 0x00202D76 File Offset: 0x00200F76
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout)
		{
			if (affectsLayout)
			{
				if (!this._arrangedAfterFormat)
				{
					this._document.StructuralCache.InvalidateFormatCache(true);
				}
				this._isContentFormatValid = false;
			}
			if (this.ContentInvalidated != null)
			{
				this.ContentInvalidated(this, EventArgs.Empty);
			}
		}

		// Token: 0x06007007 RID: 28679 RVA: 0x00202DB4 File Offset: 0x00200FB4
		void IFlowDocumentFormatter.OnContentInvalidated(bool affectsLayout, ITextPointer start, ITextPointer end)
		{
			((IFlowDocumentFormatter)this).OnContentInvalidated(affectsLayout);
		}

		// Token: 0x06007008 RID: 28680 RVA: 0x00202DBD File Offset: 0x00200FBD
		void IFlowDocumentFormatter.Suspend()
		{
			if (this.Suspended != null)
			{
				this.Suspended(this, EventArgs.Empty);
			}
		}

		// Token: 0x17001A9C RID: 6812
		// (get) Token: 0x06007009 RID: 28681 RVA: 0x00202DD8 File Offset: 0x00200FD8
		bool IFlowDocumentFormatter.IsLayoutDataValid
		{
			get
			{
				return this._documentPage != null && this._document.StructuralCache.IsFormattedOnce && !this._document.StructuralCache.ForceReformat && this._isContentFormatValid && !this._document.StructuralCache.IsContentChangeInProgress && !this._document.StructuralCache.IsFormattingInProgress;
			}
		}

		// Token: 0x040036DE RID: 14046
		private readonly FlowDocument _document;

		// Token: 0x040036DF RID: 14047
		private FlowDocumentPage _documentPage;

		// Token: 0x040036E0 RID: 14048
		private bool _arrangedAfterFormat;

		// Token: 0x040036E1 RID: 14049
		private bool _lastFormatSuccessful;

		// Token: 0x040036E2 RID: 14050
		private const double _defaultWidth = 500.0;

		// Token: 0x040036E3 RID: 14051
		private bool _isContentFormatValid;
	}
}
