using System;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006BF RID: 1727
	internal class DocumentPageHost : FrameworkElement
	{
		// Token: 0x06006F9B RID: 28571 RVA: 0x000D6F38 File Offset: 0x000D5138
		internal DocumentPageHost()
		{
		}

		// Token: 0x06006F9C RID: 28572 RVA: 0x0020154C File Offset: 0x001FF74C
		internal static void DisconnectPageVisual(Visual pageVisual)
		{
			Visual visual = VisualTreeHelper.GetParent(pageVisual) as Visual;
			if (visual != null)
			{
				ContainerVisual containerVisual = visual as ContainerVisual;
				if (containerVisual == null)
				{
					throw new ArgumentException(SR.Get("DocumentPageView_ParentNotDocumentPageHost"), "pageVisual");
				}
				DocumentPageHost documentPageHost = VisualTreeHelper.GetParent(containerVisual) as DocumentPageHost;
				if (documentPageHost == null)
				{
					throw new ArgumentException(SR.Get("DocumentPageView_ParentNotDocumentPageHost"), "pageVisual");
				}
				documentPageHost.PageVisual = null;
			}
		}

		// Token: 0x17001A82 RID: 6786
		// (get) Token: 0x06006F9D RID: 28573 RVA: 0x002015B2 File Offset: 0x001FF7B2
		// (set) Token: 0x06006F9E RID: 28574 RVA: 0x002015BC File Offset: 0x001FF7BC
		internal Visual PageVisual
		{
			get
			{
				return this._pageVisual;
			}
			set
			{
				if (this._pageVisual != null)
				{
					ContainerVisual containerVisual = VisualTreeHelper.GetParent(this._pageVisual) as ContainerVisual;
					Invariant.Assert(containerVisual != null);
					containerVisual.Children.Clear();
					base.RemoveVisualChild(containerVisual);
				}
				this._pageVisual = value;
				if (this._pageVisual != null)
				{
					ContainerVisual containerVisual = new ContainerVisual();
					base.AddVisualChild(containerVisual);
					containerVisual.Children.Add(this._pageVisual);
					containerVisual.SetValue(FrameworkElement.FlowDirectionProperty, FlowDirection.LeftToRight);
				}
			}
		}

		// Token: 0x06006F9F RID: 28575 RVA: 0x0020163C File Offset: 0x001FF83C
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._pageVisual == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return VisualTreeHelper.GetParent(this._pageVisual) as Visual;
		}

		// Token: 0x17001A83 RID: 6787
		// (get) Token: 0x06006FA0 RID: 28576 RVA: 0x00201674 File Offset: 0x001FF874
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._pageVisual == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x040036CA RID: 14026
		internal Point CachedOffset;

		// Token: 0x040036CB RID: 14027
		private Visual _pageVisual;
	}
}
