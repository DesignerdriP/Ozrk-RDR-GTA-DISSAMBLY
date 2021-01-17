using System;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000712 RID: 1810
	internal class WeakDependencySource
	{
		// Token: 0x0600749B RID: 29851 RVA: 0x00215E85 File Offset: 0x00214085
		internal WeakDependencySource(DependencyObject item, DependencyProperty dp)
		{
			this._item = BindingExpressionBase.CreateReference(item);
			this._dp = dp;
		}

		// Token: 0x0600749C RID: 29852 RVA: 0x00215EA0 File Offset: 0x002140A0
		internal WeakDependencySource(WeakReference wr, DependencyProperty dp)
		{
			this._item = wr;
			this._dp = dp;
		}

		// Token: 0x17001BBE RID: 7102
		// (get) Token: 0x0600749D RID: 29853 RVA: 0x00215EB6 File Offset: 0x002140B6
		internal DependencyObject DependencyObject
		{
			get
			{
				return (DependencyObject)BindingExpressionBase.GetReference(this._item);
			}
		}

		// Token: 0x17001BBF RID: 7103
		// (get) Token: 0x0600749E RID: 29854 RVA: 0x00215EC8 File Offset: 0x002140C8
		internal DependencyProperty DependencyProperty
		{
			get
			{
				return this._dp;
			}
		}

		// Token: 0x040037DA RID: 14298
		private object _item;

		// Token: 0x040037DB RID: 14299
		private DependencyProperty _dp;
	}
}
