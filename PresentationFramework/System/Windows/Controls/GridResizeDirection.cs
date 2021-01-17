using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies whether a <see cref="T:System.Windows.Controls.GridSplitter" /> control redistributes space between rows or between columns.</summary>
	// Token: 0x020004DE RID: 1246
	public enum GridResizeDirection
	{
		/// <summary>Space is redistributed based on the values of the <see cref="P:System.Windows.FrameworkElement.HorizontalAlignment" />, <see cref="P:System.Windows.FrameworkElement.VerticalAlignment" />, <see cref="P:System.Windows.FrameworkElement.ActualWidth" />, and <see cref="P:System.Windows.FrameworkElement.ActualHeight" /> properties of the <see cref="T:System.Windows.Controls.GridSplitter" />. </summary>
		// Token: 0x04002B8C RID: 11148
		Auto,
		/// <summary>Space is redistributed between columns.</summary>
		// Token: 0x04002B8D RID: 11149
		Columns,
		/// <summary>Space is redistributed between rows.</summary>
		// Token: 0x04002B8E RID: 11150
		Rows
	}
}
