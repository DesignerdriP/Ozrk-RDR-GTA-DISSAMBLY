using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using MS.Internal.KnownBoxes;

namespace MS.Internal.Documents
{
	// Token: 0x020006D1 RID: 1745
	internal class ReaderTwoPageViewer : ReaderPageViewer
	{
		// Token: 0x060070DA RID: 28890 RVA: 0x00204A97 File Offset: 0x00202C97
		protected override void OnPreviousPageCommand()
		{
			base.GoToPage(Math.Max(1, this.MasterPageNumber - 2));
		}

		// Token: 0x060070DB RID: 28891 RVA: 0x00204AAD File Offset: 0x00202CAD
		protected override void OnNextPageCommand()
		{
			base.GoToPage(Math.Min(base.PageCount, this.MasterPageNumber + 2));
		}

		// Token: 0x060070DC RID: 28892 RVA: 0x00204AC8 File Offset: 0x00202CC8
		protected override void OnLastPageCommand()
		{
			base.GoToPage(base.PageCount);
		}

		// Token: 0x060070DD RID: 28893 RVA: 0x00204AD6 File Offset: 0x00202CD6
		protected override void OnGoToPageCommand(int pageNumber)
		{
			base.OnGoToPageCommand((pageNumber - 1) / 2 * 2 + 1);
		}

		// Token: 0x060070DE RID: 28894 RVA: 0x00204AE8 File Offset: 0x00202CE8
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if (e.Property == DocumentViewerBase.MasterPageNumberProperty)
			{
				int num = (int)e.NewValue;
				num = (num - 1) / 2 * 2 + 1;
				if (num != (int)e.NewValue)
				{
					base.GoToPage(num);
				}
			}
		}

		// Token: 0x060070DF RID: 28895 RVA: 0x00204B37 File Offset: 0x00202D37
		static ReaderTwoPageViewer()
		{
			DocumentViewerBase.CanGoToNextPagePropertyKey.OverrideMetadata(typeof(ReaderTwoPageViewer), new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, null, new CoerceValueCallback(ReaderTwoPageViewer.CoerceCanGoToNextPage)));
		}

		// Token: 0x060070E0 RID: 28896 RVA: 0x00204B64 File Offset: 0x00202D64
		private static object CoerceCanGoToNextPage(DependencyObject d, object value)
		{
			Invariant.Assert(d != null && d is ReaderTwoPageViewer);
			ReaderTwoPageViewer readerTwoPageViewer = (ReaderTwoPageViewer)d;
			return readerTwoPageViewer.MasterPageNumber < readerTwoPageViewer.PageCount - 1;
		}
	}
}
