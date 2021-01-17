using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Describes the means by which the automatic <see cref="T:System.Windows.Controls.ToolTip" /> is positioned on a <see cref="T:System.Windows.Controls.Slider" /> control.</summary>
	// Token: 0x02000574 RID: 1396
	public enum AutoToolTipPlacement
	{
		/// <summary>No automatic <see cref="T:System.Windows.Controls.ToolTip" /></summary>
		// Token: 0x04002F9C RID: 12188
		None,
		/// <summary>For a horizontal <see cref="T:System.Windows.Controls.Slider" /> show the automatic <see cref="T:System.Windows.Controls.ToolTip" /> at the top edge of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />. For a vertical <see cref="T:System.Windows.Controls.Slider" /> show the automatic <see cref="T:System.Windows.Controls.ToolTip" /> at the left edge of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		// Token: 0x04002F9D RID: 12189
		TopLeft,
		/// <summary>For a horizontal <see cref="T:System.Windows.Controls.Slider" /> show the automatic <see cref="T:System.Windows.Controls.ToolTip" /> at the bottom edge of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />. For a vertical <see cref="T:System.Windows.Controls.Slider" /> show the automatic <see cref="T:System.Windows.Controls.ToolTip" /> at the right edge of the <see cref="T:System.Windows.Controls.Primitives.Thumb" />.</summary>
		// Token: 0x04002F9E RID: 12190
		BottomRight
	}
}
