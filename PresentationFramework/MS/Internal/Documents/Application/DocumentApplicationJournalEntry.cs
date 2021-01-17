using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace MS.Internal.Documents.Application
{
	// Token: 0x020006FE RID: 1790
	[Serializable]
	internal sealed class DocumentApplicationJournalEntry : CustomContentState
	{
		// Token: 0x0600733A RID: 29498 RVA: 0x00210CE8 File Offset: 0x0020EEE8
		public DocumentApplicationJournalEntry(object state, string name)
		{
			Invariant.Assert(state is DocumentApplicationState, "state should be of type DocumentApplicationState");
			this._state = state;
			this._displayName = name;
		}

		// Token: 0x0600733B RID: 29499 RVA: 0x00210D11 File Offset: 0x0020EF11
		public DocumentApplicationJournalEntry(object state) : this(state, null)
		{
		}

		// Token: 0x0600733C RID: 29500 RVA: 0x00210D1C File Offset: 0x0020EF1C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public override void Replay(NavigationService navigationService, NavigationMode mode)
		{
			ContentControl contentControl = (ContentControl)navigationService.INavigatorHost;
			contentControl.ApplyTemplate();
			DocumentApplicationDocumentViewer documentApplicationDocumentViewer = contentControl.Template.FindName("PUIDocumentApplicationDocumentViewer", contentControl) as DocumentApplicationDocumentViewer;
			if (documentApplicationDocumentViewer != null)
			{
				if (this._state is DocumentApplicationState)
				{
					documentApplicationDocumentViewer.StoredDocumentApplicationState = (DocumentApplicationState)this._state;
				}
				if (navigationService.Content != null)
				{
					IDocumentPaginatorSource documentPaginatorSource = navigationService.Content as IDocumentPaginatorSource;
					if (documentPaginatorSource != null && documentPaginatorSource.DocumentPaginator.IsPageCountValid)
					{
						documentApplicationDocumentViewer.SetUIToStoredState();
					}
				}
			}
		}

		// Token: 0x17001B59 RID: 7001
		// (get) Token: 0x0600733D RID: 29501 RVA: 0x00210D9E File Offset: 0x0020EF9E
		public override string JournalEntryName
		{
			get
			{
				return this._displayName;
			}
		}

		// Token: 0x04003788 RID: 14216
		private object _state;

		// Token: 0x04003789 RID: 14217
		private string _displayName;
	}
}
