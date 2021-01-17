using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;

namespace MS.Internal.Documents
{
	// Token: 0x020006CF RID: 1743
	internal class ReaderScrollViewer : FlowDocumentScrollViewer, IFlowDocumentViewer
	{
		// Token: 0x0600709A RID: 28826 RVA: 0x00204442 File Offset: 0x00202642
		protected override void OnPrintCompleted()
		{
			base.OnPrintCompleted();
			if (this._printCompleted != null)
			{
				this._printCompleted(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600709B RID: 28827 RVA: 0x00204463 File Offset: 0x00202663
		protected override void OnPrintCommand()
		{
			base.OnPrintCommand();
			if (this._printStarted != null && base.IsPrinting)
			{
				this._printStarted(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600709C RID: 28828 RVA: 0x0020448C File Offset: 0x0020268C
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == FlowDocumentScrollViewer.DocumentProperty)
			{
				if (this._pageNumberChanged != null)
				{
					this._pageNumberChanged(this, EventArgs.Empty);
				}
				if (this._pageCountChanged != null)
				{
					this._pageCountChanged(this, EventArgs.Empty);
				}
			}
		}

		// Token: 0x0600709D RID: 28829 RVA: 0x002044E0 File Offset: 0x002026E0
		private bool IsValidTextSelectionForDocument(ITextSelection textSelection, FlowDocument flowDocument)
		{
			return textSelection.Start != null && textSelection.Start.TextContainer == flowDocument.StructuralCache.TextContainer;
		}

		// Token: 0x0600709E RID: 28830 RVA: 0x00204508 File Offset: 0x00202708
		private object SetTextSelection(object arg)
		{
			ITextSelection textSelection = arg as ITextSelection;
			if (textSelection != null && base.Document != null && this.IsValidTextSelectionForDocument(textSelection, base.Document))
			{
				ITextSelection textSelection2 = base.Document.StructuralCache.TextContainer.TextSelection;
				if (textSelection2 != null)
				{
					textSelection2.SetCaretToPosition(textSelection.AnchorPosition, textSelection.MovingPosition.LogicalDirection, true, true);
					textSelection2.ExtendToPosition(textSelection.MovingPosition);
				}
			}
			return null;
		}

		// Token: 0x0600709F RID: 28831 RVA: 0x00204575 File Offset: 0x00202775
		void IFlowDocumentViewer.PreviousPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.PageUp();
			}
		}

		// Token: 0x060070A0 RID: 28832 RVA: 0x0020458A File Offset: 0x0020278A
		void IFlowDocumentViewer.NextPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.PageDown();
			}
		}

		// Token: 0x060070A1 RID: 28833 RVA: 0x0020459F File Offset: 0x0020279F
		void IFlowDocumentViewer.FirstPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x060070A2 RID: 28834 RVA: 0x002045B4 File Offset: 0x002027B4
		void IFlowDocumentViewer.LastPage()
		{
			if (base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToEnd();
			}
		}

		// Token: 0x060070A3 RID: 28835 RVA: 0x00154A4D File Offset: 0x00152C4D
		void IFlowDocumentViewer.Print()
		{
			this.OnPrintCommand();
		}

		// Token: 0x060070A4 RID: 28836 RVA: 0x00154A55 File Offset: 0x00152C55
		void IFlowDocumentViewer.CancelPrint()
		{
			this.OnCancelPrintCommand();
		}

		// Token: 0x060070A5 RID: 28837 RVA: 0x00002137 File Offset: 0x00000337
		void IFlowDocumentViewer.ShowFindResult(ITextRange findResult)
		{
		}

		// Token: 0x060070A6 RID: 28838 RVA: 0x002045C9 File Offset: 0x002027C9
		bool IFlowDocumentViewer.CanGoToPage(int pageNumber)
		{
			return pageNumber == 1;
		}

		// Token: 0x060070A7 RID: 28839 RVA: 0x002045CF File Offset: 0x002027CF
		void IFlowDocumentViewer.GoToPage(int pageNumber)
		{
			if (pageNumber == 1 && base.ScrollViewer != null)
			{
				base.ScrollViewer.ScrollToHome();
			}
		}

		// Token: 0x060070A8 RID: 28840 RVA: 0x002045E8 File Offset: 0x002027E8
		void IFlowDocumentViewer.SetDocument(FlowDocument document)
		{
			base.Document = document;
		}

		// Token: 0x17001AC4 RID: 6852
		// (get) Token: 0x060070A9 RID: 28841 RVA: 0x002045F1 File Offset: 0x002027F1
		// (set) Token: 0x060070AA RID: 28842 RVA: 0x002045F9 File Offset: 0x002027F9
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

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x060070AB RID: 28843 RVA: 0x00204620 File Offset: 0x00202820
		// (set) Token: 0x060070AC RID: 28844 RVA: 0x00204628 File Offset: 0x00202828
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

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x060070AD RID: 28845 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IFlowDocumentViewer.CanGoToPreviousPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x060070AE RID: 28846 RVA: 0x0000B02A File Offset: 0x0000922A
		bool IFlowDocumentViewer.CanGoToNextPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001AC8 RID: 6856
		// (get) Token: 0x060070AF RID: 28847 RVA: 0x0020464F File Offset: 0x0020284F
		int IFlowDocumentViewer.PageNumber
		{
			get
			{
				if (base.Document == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x17001AC9 RID: 6857
		// (get) Token: 0x060070B0 RID: 28848 RVA: 0x0020464F File Offset: 0x0020284F
		int IFlowDocumentViewer.PageCount
		{
			get
			{
				if (base.Document == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x14000140 RID: 320
		// (add) Token: 0x060070B1 RID: 28849 RVA: 0x0020465C File Offset: 0x0020285C
		// (remove) Token: 0x060070B2 RID: 28850 RVA: 0x00204675 File Offset: 0x00202875
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

		// Token: 0x14000141 RID: 321
		// (add) Token: 0x060070B3 RID: 28851 RVA: 0x0020468E File Offset: 0x0020288E
		// (remove) Token: 0x060070B4 RID: 28852 RVA: 0x002046A7 File Offset: 0x002028A7
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

		// Token: 0x14000142 RID: 322
		// (add) Token: 0x060070B5 RID: 28853 RVA: 0x002046C0 File Offset: 0x002028C0
		// (remove) Token: 0x060070B6 RID: 28854 RVA: 0x002046D9 File Offset: 0x002028D9
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

		// Token: 0x14000143 RID: 323
		// (add) Token: 0x060070B7 RID: 28855 RVA: 0x002046F2 File Offset: 0x002028F2
		// (remove) Token: 0x060070B8 RID: 28856 RVA: 0x0020470B File Offset: 0x0020290B
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

		// Token: 0x040036F4 RID: 14068
		private EventHandler _pageNumberChanged;

		// Token: 0x040036F5 RID: 14069
		private EventHandler _pageCountChanged;

		// Token: 0x040036F6 RID: 14070
		private EventHandler _printCompleted;

		// Token: 0x040036F7 RID: 14071
		private EventHandler _printStarted;
	}
}
