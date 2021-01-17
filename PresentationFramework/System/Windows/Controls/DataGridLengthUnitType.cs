﻿using System;

namespace System.Windows.Controls
{
	/// <summary>Defines constants that specify how elements in a <see cref="T:System.Windows.Controls.DataGrid" /> are sized.</summary>
	// Token: 0x020004B1 RID: 1201
	public enum DataGridLengthUnitType
	{
		/// <summary>The size is based on the contents of both the cells and the column header.</summary>
		// Token: 0x040029CD RID: 10701
		Auto,
		/// <summary>The size is a fixed value expressed in pixels.</summary>
		// Token: 0x040029CE RID: 10702
		Pixel,
		/// <summary>The size is based on the contents of the cells.</summary>
		// Token: 0x040029CF RID: 10703
		SizeToCells,
		/// <summary>The size is based on the contents of the column header.</summary>
		// Token: 0x040029D0 RID: 10704
		SizeToHeader,
		/// <summary>The size is a weighted proportion of available space.</summary>
		// Token: 0x040029D1 RID: 10705
		Star
	}
}
