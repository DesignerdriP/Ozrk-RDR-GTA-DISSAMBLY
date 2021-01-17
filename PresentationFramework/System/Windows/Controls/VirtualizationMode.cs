using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the method the <see cref="T:System.Windows.Controls.VirtualizingStackPanel" /> uses to manage virtualizing its child items.</summary>
	// Token: 0x0200055B RID: 1371
	public enum VirtualizationMode
	{
		/// <summary>Create and discard the item containers.</summary>
		// Token: 0x04002F29 RID: 12073
		Standard,
		/// <summary>Reuse the item containers.</summary>
		// Token: 0x04002F2A RID: 12074
		Recycling
	}
}
