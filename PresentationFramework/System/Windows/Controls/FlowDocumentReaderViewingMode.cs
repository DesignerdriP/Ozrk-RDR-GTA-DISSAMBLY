using System;

namespace System.Windows.Controls
{
	/// <summary>Names viewing modes for the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> control.</summary>
	// Token: 0x020004D1 RID: 1233
	public enum FlowDocumentReaderViewingMode
	{
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> should show content one page at a time.</summary>
		// Token: 0x04002ADE RID: 10974
		Page,
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> should show content two pages at a time, similar to an open book.</summary>
		// Token: 0x04002ADF RID: 10975
		TwoPage,
		/// <summary>Indicates that the <see cref="T:System.Windows.Controls.FlowDocumentReader" /> should show content in continuous scrolling mode.</summary>
		// Token: 0x04002AE0 RID: 10976
		Scroll
	}
}
