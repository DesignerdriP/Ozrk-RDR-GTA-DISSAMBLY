using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies how <see cref="T:System.Windows.Controls.ScrollViewer" /> reacts to touch manipulation.  </summary>
	// Token: 0x0200050E RID: 1294
	public enum PanningMode
	{
		/// <summary>The <see cref="T:System.Windows.Controls.ScrollViewer" /> does not respond to touch input.</summary>
		// Token: 0x04002CE2 RID: 11490
		None,
		/// <summary>The <see cref="T:System.Windows.Controls.ScrollViewer" /> scrolls horizontally only.</summary>
		// Token: 0x04002CE3 RID: 11491
		HorizontalOnly,
		/// <summary>The <see cref="T:System.Windows.Controls.ScrollViewer" /> scrolls vertically only.</summary>
		// Token: 0x04002CE4 RID: 11492
		VerticalOnly,
		/// <summary>The <see cref="T:System.Windows.Controls.ScrollViewer" /> scrolls horizontally and vertically.</summary>
		// Token: 0x04002CE5 RID: 11493
		Both,
		/// <summary>The <see cref="T:System.Windows.Controls.ScrollViewer" /> scrolls when the user moves a finger horizontally first.  If the user moves the vertically first, the movement is treated as mouse events.  After the <see cref="T:System.Windows.Controls.ScrollViewer" /> begins to scroll, it will scroll horizontally and vertically.</summary>
		// Token: 0x04002CE6 RID: 11494
		HorizontalFirst,
		/// <summary>The <see cref="T:System.Windows.Controls.ScrollViewer" /> scrolls when the user moves a finger vertically first.  If the user moves the horizontally first, the movement is treated as mouse events.  After the <see cref="T:System.Windows.Controls.ScrollViewer" /> begins to scroll, it will scroll horizontally and vertically.</summary>
		// Token: 0x04002CE7 RID: 11495
		VerticalFirst
	}
}
