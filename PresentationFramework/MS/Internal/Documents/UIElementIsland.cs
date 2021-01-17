using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006F8 RID: 1784
	internal class UIElementIsland : ContainerVisual, IContentHost, IDisposable
	{
		// Token: 0x06007301 RID: 29441 RVA: 0x0020FE28 File Offset: 0x0020E028
		internal UIElementIsland(UIElement child)
		{
			base.SetFlags(true, VisualFlags.IsLayoutIslandRoot);
			this._child = child;
			if (this._child != null)
			{
				Visual visual = VisualTreeHelper.GetParent(this._child) as Visual;
				if (visual != null)
				{
					Invariant.Assert(visual is UIElementIsland, "Parent should always be a UIElementIsland.");
					((UIElementIsland)visual).Dispose();
				}
				base.Children.Add(this._child);
			}
		}

		// Token: 0x06007302 RID: 29442 RVA: 0x0020FE9C File Offset: 0x0020E09C
		internal Size DoLayout(Size availableSize, bool horizontalAutoSize, bool verticalAutoSize)
		{
			Size size = default(Size);
			if (this._child != null)
			{
				if (this._child is FrameworkElement && ((FrameworkElement)this._child).Parent != null)
				{
					base.SetValue(FrameworkElement.FlowDirectionProperty, ((FrameworkElement)this._child).Parent.GetValue(FrameworkElement.FlowDirectionProperty));
				}
				try
				{
					this._layoutInProgress = true;
					this._child.Measure(availableSize);
					size.Width = (horizontalAutoSize ? this._child.DesiredSize.Width : availableSize.Width);
					size.Height = (verticalAutoSize ? this._child.DesiredSize.Height : availableSize.Height);
					this._child.Arrange(new Rect(size));
				}
				finally
				{
					this._layoutInProgress = false;
				}
			}
			return size;
		}

		// Token: 0x17001B48 RID: 6984
		// (get) Token: 0x06007303 RID: 29443 RVA: 0x0020FF8C File Offset: 0x0020E18C
		internal UIElement Root
		{
			get
			{
				return this._child;
			}
		}

		// Token: 0x14000154 RID: 340
		// (add) Token: 0x06007304 RID: 29444 RVA: 0x0020FF94 File Offset: 0x0020E194
		// (remove) Token: 0x06007305 RID: 29445 RVA: 0x0020FFCC File Offset: 0x0020E1CC
		internal event DesiredSizeChangedEventHandler DesiredSizeChanged;

		// Token: 0x06007306 RID: 29446 RVA: 0x00210001 File Offset: 0x0020E201
		public void Dispose()
		{
			if (this._child != null)
			{
				base.Children.Clear();
				this._child = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06007307 RID: 29447 RVA: 0x0000C238 File Offset: 0x0000A438
		IInputElement IContentHost.InputHitTest(Point point)
		{
			return null;
		}

		// Token: 0x06007308 RID: 29448 RVA: 0x00210023 File Offset: 0x0020E223
		ReadOnlyCollection<Rect> IContentHost.GetRectangles(ContentElement child)
		{
			return new ReadOnlyCollection<Rect>(new List<Rect>());
		}

		// Token: 0x17001B49 RID: 6985
		// (get) Token: 0x06007309 RID: 29449 RVA: 0x00210030 File Offset: 0x0020E230
		IEnumerator<IInputElement> IContentHost.HostedElements
		{
			get
			{
				List<IInputElement> list = new List<IInputElement>();
				if (this._child != null)
				{
					list.Add(this._child);
				}
				return list.GetEnumerator();
			}
		}

		// Token: 0x0600730A RID: 29450 RVA: 0x00210062 File Offset: 0x0020E262
		void IContentHost.OnChildDesiredSizeChanged(UIElement child)
		{
			Invariant.Assert(child == this._child);
			if (!this._layoutInProgress && this.DesiredSizeChanged != null)
			{
				this.DesiredSizeChanged(this, new DesiredSizeChangedEventArgs(child));
			}
		}

		// Token: 0x0400376D RID: 14189
		private UIElement _child;

		// Token: 0x0400376E RID: 14190
		private bool _layoutInProgress;
	}
}
