using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the type of unit that is used by the <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> attached property.</summary>
	// Token: 0x02000513 RID: 1299
	public enum VirtualizationCacheLengthUnit
	{
		/// <summary>The <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> is measured in terms of device-independent units (1/96th inch per unit).</summary>
		// Token: 0x04002D1C RID: 11548
		Pixel,
		/// <summary>The <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> is measured in terms of the items that are displayed in the panel.</summary>
		// Token: 0x04002D1D RID: 11549
		Item,
		/// <summary>The <see cref="P:System.Windows.Controls.VirtualizingPanel.CacheLength" /> is measured in terms of a page, which is equal to the size of the panel's viewport.</summary>
		// Token: 0x04002D1E RID: 11550
		Page
	}
}
