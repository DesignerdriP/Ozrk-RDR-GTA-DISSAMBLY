using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020006D0 RID: 1744
	internal class ReaderPageViewer : FlowDocumentPageViewer, IFlowDocumentViewer
	{
		// Token: 0x060070BA RID: 28858 RVA: 0x0020472C File Offset: 0x0020292C
		protected override void OnPrintCompleted()
		{
			base.OnPrintCompleted();
			if (this._printCompleted != null)
			{
				this._printCompleted(this, EventArgs.Empty);
			}
		}

		// Token: 0x060070BB RID: 28859 RVA: 0x0020474D File Offset: 0x0020294D
		protected override void OnPrintCommand()
		{
			base.OnPrintCommand();
			if (this._printStarted != null && base.IsPrinting)
			{
				this._printStarted(this, EventArgs.Empty);
			}
		}

		// Token: 0x060070BC RID: 28860 RVA: 0x00204778 File Offset: 0x00202978
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DocumentViewerBase.PageCountProperty || e.Property == DocumentViewerBase.MasterPageNumberProperty || e.Property == DocumentViewerBase.CanGoToPreviousPageProperty || e.Property == DocumentViewerBase.CanGoToNextPageProperty)
			{
				if (!this._raisePageNumberChanged && !this._raisePageCountChanged)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.RaisePropertyChangedAsync), null);
				}
				if (e.Property == DocumentViewerBase.PageCountProperty)
				{
					this._raisePageCountChanged = true;
					base.CoerceValue(DocumentViewerBase.CanGoToNextPageProperty);
					return;
				}
				if (e.Property == DocumentViewerBase.MasterPageNumberProperty)
				{
					this._raisePageNumberChanged = true;
					base.CoerceValue(DocumentViewerBase.CanGoToNextPageProperty);
					return;
				}
				this._raisePageNumberChanged = true;
			}
		}

		// Token: 0x060070BD RID: 28861 RVA: 0x00204838 File Offset: 0x00202A38
		private object SetTextSelection(object arg)
		{
			ITextSelection textSelection = arg as ITextSelection;
			FlowDocument flowDocument = base.Document as FlowDocument;
			if (textSelection != null && flowDocument != null && textSelection.AnchorPosition != null && textSelection.AnchorPosition.TextContainer == flowDocument.StructuralCache.TextContainer)
			{
				ITextSelection textSelection2 = flowDocument.StructuralCache.TextContainer.TextSelection;
				if (textSelection2 != null)
				{
					textSelection2.SetCaretToPosition(textSelection.AnchorPosition, textSelection.MovingPosition.LogicalDirection, true, true);
					textSelection2.ExtendToPosition(textSelection.MovingPosition);
				}
			}
			return null;
		}

		// Token: 0x060070BE RID: 28862 RVA: 0x002048B8 File Offset: 0x00202AB8
		private object RaisePropertyChangedAsync(object arg)
		{
			if (this._raisePageCountChanged)
			{
				if (this._pageCountChanged != null)
				{
					this._pageCountChanged(this, EventArgs.Empty);
				}
				this._raisePageCountChanged = false;
			}
			if (this._raisePageNumberChanged)
			{
				if (this._pageNumberChanged != null)
				{
					this._pageNumberChanged(this, EventArgs.Empty);
				}
				this._raisePageNumberChanged = false;
			}
			return null;
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x001A5AEE File Offset: 0x001A3CEE
		void IFlowDocumentViewer.PreviousPage()
		{
			this.OnPreviousPageCommand();
		}

		// Token: 0x060070C0 RID: 28864 RVA: 0x001A5AF6 File Offset: 0x001A3CF6
		void IFlowDocumentViewer.NextPage()
		{
			this.OnNextPageCommand();
		}

		// Token: 0x060070C1 RID: 28865 RVA: 0x001A5AFE File Offset: 0x001A3CFE
		void IFlowDocumentViewer.FirstPage()
		{
			this.OnFirstPageCommand();
		}

		// Token: 0x060070C2 RID: 28866 RVA: 0x001A5B06 File Offset: 0x001A3D06
		void IFlowDocumentViewer.LastPage()
		{
			this.OnLastPageCommand();
		}

		// Token: 0x060070C3 RID: 28867 RVA: 0x001A5B17 File Offset: 0x001A3D17
		void IFlowDocumentViewer.Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x060070C4 RID: 28868 RVA: 0x001A5B1F File Offset: 0x001A3D1F
		void IFlowDocumentViewer.CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x060070C5 RID: 28869 RVA: 0x00204916 File Offset: 0x00202B16
		void IFlowDocumentViewer.ShowFindResult(ITextRange findResult)
		{
			if (findResult.Start is ContentPosition)
			{
				base.BringContentPositionIntoView((ContentPosition)findResult.Start);
			}
		}

		// Token: 0x060070C6 RID: 28870 RVA: 0x00204937 File Offset: 0x00202B37
		bool IFlowDocumentViewer.CanGoToPage(int pageNumber)
		{
			return this.CanGoToPage(pageNumber);
		}

		// Token: 0x060070C7 RID: 28871 RVA: 0x001A5B0E File Offset: 0x001A3D0E
		void IFlowDocumentViewer.GoToPage(int pageNumber)
		{
			this.OnGoToPageCommand(pageNumber);
		}

		// Token: 0x060070C8 RID: 28872 RVA: 0x00204940 File Offset: 0x00202B40
		void IFlowDocumentViewer.SetDocument(FlowDocument document)
		{
			base.Document = document;
		}

		// Token: 0x17001ACA RID: 6858
		// (get) Token: 0x060070C9 RID: 28873 RVA: 0x00204949 File Offset: 0x00202B49
		// (set) Token: 0x060070CA RID: 28874 RVA: 0x00204951 File Offset: 0x00202B51
		ContentPosition IFlowDocumentViewer.ContentPosition
		{
			get
			{
				return base.ContentPosition;
			}
			set
			{
				if (value != null && base.Document != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(base.BringContentPositionIntoView), value);
				}
			}
		}

		// Token: 0x17001ACB RID: 6859
		// (get) Token: 0x060070CB RID: 28875 RVA: 0x00204978 File Offset: 0x00202B78
		// (set) Token: 0x060070CC RID: 28876 RVA: 0x00204980 File Offset: 0x00202B80
		ITextSelection IFlowDocumentViewer.TextSelection
		{
			get
			{
				return base.Selection;
			}
			set
			{
				if (value != null && base.Document != null)
				{
					base.Dispatcher.BeginInvoke(DispatcherPriority.Input, new DispatcherOperationCallback(this.SetTextSelection), value);
				}
			}
		}

		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x060070CD RID: 28877 RVA: 0x002049A7 File Offset: 0x00202BA7
		bool IFlowDocumentViewer.CanGoToPreviousPage
		{
			get
			{
				return this.CanGoToPreviousPage;
			}
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x060070CE RID: 28878 RVA: 0x002049AF File Offset: 0x00202BAF
		bool IFlowDocumentViewer.CanGoToNextPage
		{
			get
			{
				return this.CanGoToNextPage;
			}
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x060070CF RID: 28879 RVA: 0x002049B7 File Offset: 0x00202BB7
		int IFlowDocumentViewer.PageNumber
		{
			get
			{
				return this.MasterPageNumber;
			}
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x060070D0 RID: 28880 RVA: 0x002049BF File Offset: 0x00202BBF
		int IFlowDocumentViewer.PageCount
		{
			get
			{
				return base.PageCount;
			}
		}

		// Token: 0x14000144 RID: 324
		// (add) Token: 0x060070D1 RID: 28881 RVA: 0x002049C7 File Offset: 0x00202BC7
		// (remove) Token: 0x060070D2 RID: 28882 RVA: 0x002049E0 File Offset: 0x00202BE0
		event EventHandler IFlowDocumentViewer.PageNumberChanged
		{
			add
			{
				this._pageNumberChanged = (EventHandler)Delegate.Combine(this._pageNumberChanged, value);
			}
			remove
			{
				this._pageNumberChanged = (EventHandler)Delegate.Remove(this._pageNumberChanged, value);
			}
		}

		// Token: 0x14000145 RID: 325
		// (add) Token: 0x060070D3 RID: 28883 RVA: 0x002049F9 File Offset: 0x00202BF9
		// (remove) Token: 0x060070D4 RID: 28884 RVA: 0x00204A12 File Offset: 0x00202C12
		event EventHandler IFlowDocumentViewer.PageCountChanged
		{
			add
			{
				this._pageCountChanged = (EventHandler)Delegate.Combine(this._pageCountChanged, value);
			}
			remove
			{
				this._pageCountChanged = (EventHandler)Delegate.Remove(this._pageCountChanged, value);
			}
		}

		// Token: 0x14000146 RID: 326
		// (add) Token: 0x060070D5 RID: 28885 RVA: 0x00204A2B File Offset: 0x00202C2B
		// (remove) Token: 0x060070D6 RID: 28886 RVA: 0x00204A44 File Offset: 0x00202C44
		event EventHandler IFlowDocumentViewer.PrintStarted
		{
			add
			{
				this._printStarted = (EventHandler)Delegate.Combine(this._printStarted, value);
			}
			remove
			{
				this._printStarted = (EventHandler)Delegate.Remove(this._printStarted, value);
			}
		}

		// Token: 0x14000147 RID: 327
		// (add) Token: 0x060070D7 RID: 28887 RVA: 0x00204A5D File Offset: 0x00202C5D
		// (remove) Token: 0x060070D8 RID: 28888 RVA: 0x00204A76 File Offset: 0x00202C76
		event EventHandler IFlowDocumentViewer.PrintCompleted
		{
			add
			{
				this._printCompleted = (EventHandler)Delegate.Combine(this._printCompleted, value);
			}
			remove
			{
				this._printCompleted = (EventHandler)Delegate.Remove(this._printCompleted, value);
			}
		}

		// Token: 0x040036F8 RID: 14072
		private EventHandler _pageNumberChanged;

		// Token: 0x040036F9 RID: 14073
		private EventHandler _pageCountChanged;

		// Token: 0x040036FA RID: 14074
		private EventHandler _printCompleted;

		// Token: 0x040036FB RID: 14075
		private EventHandler _printStarted;

		// Token: 0x040036FC RID: 14076
		private bool _raisePageNumberChanged;

		// Token: 0x040036FD RID: 14077
		private bool _raisePageCountChanged;
	}
}
