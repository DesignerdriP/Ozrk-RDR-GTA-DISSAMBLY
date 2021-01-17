using System;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020006FA RID: 1786
	internal class DesiredSizeChangedEventArgs : EventArgs
	{
		// Token: 0x0600730F RID: 29455 RVA: 0x00210094 File Offset: 0x0020E294
		internal DesiredSizeChangedEventArgs(UIElement child)
		{
			this._child = child;
		}

		// Token: 0x17001B4A RID: 6986
		// (get) Token: 0x06007310 RID: 29456 RVA: 0x002100A3 File Offset: 0x0020E2A3
		internal UIElement Child
		{
			get
			{
				return this._child;
			}
		}

		// Token: 0x0400376F RID: 14191
		private readonly UIElement _child;
	}
}
