using System;

namespace System.Windows.Controls
{
	/// <summary> Defines the different roles that a <see cref="T:System.Windows.Controls.MenuItem" /> can have. </summary>
	// Token: 0x02000502 RID: 1282
	public enum MenuItemRole
	{
		/// <summary> Top-level menu item that can invoke commands. </summary>
		// Token: 0x04002C81 RID: 11393
		TopLevelItem,
		/// <summary> Header for top-level menus. </summary>
		// Token: 0x04002C82 RID: 11394
		TopLevelHeader,
		/// <summary> Menu item in a submenu that can invoke commands. </summary>
		// Token: 0x04002C83 RID: 11395
		SubmenuItem,
		/// <summary> Header for a submenu. </summary>
		// Token: 0x04002C84 RID: 11396
		SubmenuHeader
	}
}
