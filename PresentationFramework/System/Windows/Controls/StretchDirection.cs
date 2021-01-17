using System;

namespace System.Windows.Controls
{
	/// <summary>Describes how scaling applies to content and restricts scaling to named axis types. </summary>
	// Token: 0x02000558 RID: 1368
	public enum StretchDirection
	{
		/// <summary>The content scales upward only when it is smaller than the parent. If the content is larger, no scaling downward is performed. </summary>
		// Token: 0x04002F1E RID: 12062
		UpOnly,
		/// <summary>The content scales downward only when it is larger than the parent. If the content is smaller, no scaling upward is performed. </summary>
		// Token: 0x04002F1F RID: 12063
		DownOnly,
		/// <summary>The content stretches to fit the parent according to the <see cref="T:System.Windows.Media.Stretch" /> mode. </summary>
		// Token: 0x04002F20 RID: 12064
		Both
	}
}
