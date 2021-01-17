using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006CC RID: 1740
	internal interface IDocumentScrollInfo : IScrollInfo
	{
		// Token: 0x0600705C RID: 28764
		void MakePageVisible(int pageNumber);

		// Token: 0x0600705D RID: 28765
		void MakeSelectionVisible();

		// Token: 0x0600705E RID: 28766
		Rect MakeVisible(object o, Rect r, int pageNumber);

		// Token: 0x0600705F RID: 28767
		void ScrollToNextRow();

		// Token: 0x06007060 RID: 28768
		void ScrollToPreviousRow();

		// Token: 0x06007061 RID: 28769
		void ScrollToHome();

		// Token: 0x06007062 RID: 28770
		void ScrollToEnd();

		// Token: 0x06007063 RID: 28771
		void SetScale(double scale);

		// Token: 0x06007064 RID: 28772
		void SetColumns(int columns);

		// Token: 0x06007065 RID: 28773
		void FitColumns(int columns);

		// Token: 0x06007066 RID: 28774
		void FitToPageWidth();

		// Token: 0x06007067 RID: 28775
		void FitToPageHeight();

		// Token: 0x06007068 RID: 28776
		void ViewThumbnails();

		// Token: 0x17001AB0 RID: 6832
		// (get) Token: 0x06007069 RID: 28777
		// (set) Token: 0x0600706A RID: 28778
		DynamicDocumentPaginator Content { get; set; }

		// Token: 0x17001AB1 RID: 6833
		// (get) Token: 0x0600706B RID: 28779
		int PageCount { get; }

		// Token: 0x17001AB2 RID: 6834
		// (get) Token: 0x0600706C RID: 28780
		int FirstVisiblePageNumber { get; }

		// Token: 0x17001AB3 RID: 6835
		// (get) Token: 0x0600706D RID: 28781
		double Scale { get; }

		// Token: 0x17001AB4 RID: 6836
		// (get) Token: 0x0600706E RID: 28782
		int MaxPagesAcross { get; }

		// Token: 0x17001AB5 RID: 6837
		// (get) Token: 0x0600706F RID: 28783
		// (set) Token: 0x06007070 RID: 28784
		double VerticalPageSpacing { get; set; }

		// Token: 0x17001AB6 RID: 6838
		// (get) Token: 0x06007071 RID: 28785
		// (set) Token: 0x06007072 RID: 28786
		double HorizontalPageSpacing { get; set; }

		// Token: 0x17001AB7 RID: 6839
		// (get) Token: 0x06007073 RID: 28787
		// (set) Token: 0x06007074 RID: 28788
		bool ShowPageBorders { get; set; }

		// Token: 0x17001AB8 RID: 6840
		// (get) Token: 0x06007075 RID: 28789
		// (set) Token: 0x06007076 RID: 28790
		bool LockViewModes { get; set; }

		// Token: 0x17001AB9 RID: 6841
		// (get) Token: 0x06007077 RID: 28791
		ITextView TextView { get; }

		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x06007078 RID: 28792
		ITextContainer TextContainer { get; }

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x06007079 RID: 28793
		ReadOnlyCollection<DocumentPageView> PageViews { get; }

		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x0600707A RID: 28794
		// (set) Token: 0x0600707B RID: 28795
		DocumentViewer DocumentViewerOwner { get; set; }
	}
}
