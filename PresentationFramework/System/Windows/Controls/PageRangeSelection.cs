using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies whether all the pages or only a limited range will be processed by an operation, usually printing.</summary>
	// Token: 0x0200050B RID: 1291
	public enum PageRangeSelection
	{
		/// <summary>All pages.</summary>
		// Token: 0x04002CD2 RID: 11474
		AllPages,
		/// <summary>A user-specified range of pages.</summary>
		// Token: 0x04002CD3 RID: 11475
		UserPages,
		/// <summary>The current page.</summary>
		// Token: 0x04002CD4 RID: 11476
		CurrentPage,
		/// <summary>The selected pages.</summary>
		// Token: 0x04002CD5 RID: 11477
		SelectedPages
	}
}
