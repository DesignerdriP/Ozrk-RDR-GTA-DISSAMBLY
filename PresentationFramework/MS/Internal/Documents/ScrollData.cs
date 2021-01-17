using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MS.Internal.Documents
{
	// Token: 0x020006F0 RID: 1776
	internal class ScrollData
	{
		// Token: 0x060071FB RID: 29179 RVA: 0x00209379 File Offset: 0x00207579
		internal void LineUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - 16.0);
		}

		// Token: 0x060071FC RID: 29180 RVA: 0x00209397 File Offset: 0x00207597
		internal void LineDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + 16.0);
		}

		// Token: 0x060071FD RID: 29181 RVA: 0x002093B5 File Offset: 0x002075B5
		internal void LineLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - 16.0);
		}

		// Token: 0x060071FE RID: 29182 RVA: 0x002093D3 File Offset: 0x002075D3
		internal void LineRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + 16.0);
		}

		// Token: 0x060071FF RID: 29183 RVA: 0x002093F1 File Offset: 0x002075F1
		internal void PageUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - this._viewport.Height);
		}

		// Token: 0x06007200 RID: 29184 RVA: 0x00209411 File Offset: 0x00207611
		internal void PageDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + this._viewport.Height);
		}

		// Token: 0x06007201 RID: 29185 RVA: 0x00209431 File Offset: 0x00207631
		internal void PageLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - this._viewport.Width);
		}

		// Token: 0x06007202 RID: 29186 RVA: 0x00209451 File Offset: 0x00207651
		internal void PageRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + this._viewport.Width);
		}

		// Token: 0x06007203 RID: 29187 RVA: 0x00209471 File Offset: 0x00207671
		internal void MouseWheelUp(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y - 48.0);
		}

		// Token: 0x06007204 RID: 29188 RVA: 0x0020948F File Offset: 0x0020768F
		internal void MouseWheelDown(UIElement owner)
		{
			this.SetVerticalOffset(owner, this._offset.Y + 48.0);
		}

		// Token: 0x06007205 RID: 29189 RVA: 0x002094AD File Offset: 0x002076AD
		internal void MouseWheelLeft(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X - 48.0);
		}

		// Token: 0x06007206 RID: 29190 RVA: 0x002094CB File Offset: 0x002076CB
		internal void MouseWheelRight(UIElement owner)
		{
			this.SetHorizontalOffset(owner, this._offset.X + 48.0);
		}

		// Token: 0x06007207 RID: 29191 RVA: 0x002094EC File Offset: 0x002076EC
		internal void SetHorizontalOffset(UIElement owner, double offset)
		{
			if (!this.CanHorizontallyScroll)
			{
				return;
			}
			offset = Math.Max(0.0, Math.Min(this._extent.Width - this._viewport.Width, offset));
			if (!DoubleUtil.AreClose(offset, this._offset.X))
			{
				this._offset.X = offset;
				owner.InvalidateArrange();
				if (this._scrollOwner != null)
				{
					this._scrollOwner.InvalidateScrollInfo();
				}
			}
		}

		// Token: 0x06007208 RID: 29192 RVA: 0x00209568 File Offset: 0x00207768
		internal void SetVerticalOffset(UIElement owner, double offset)
		{
			if (!this.CanVerticallyScroll)
			{
				return;
			}
			offset = Math.Max(0.0, Math.Min(this._extent.Height - this._viewport.Height, offset));
			if (!DoubleUtil.AreClose(offset, this._offset.Y))
			{
				this._offset.Y = offset;
				owner.InvalidateArrange();
				if (this._scrollOwner != null)
				{
					this._scrollOwner.InvalidateScrollInfo();
				}
			}
		}

		// Token: 0x06007209 RID: 29193 RVA: 0x002095E4 File Offset: 0x002077E4
		internal Rect MakeVisible(UIElement owner, Visual visual, Rect rectangle)
		{
			if (rectangle.IsEmpty || visual == null || (visual != owner && !owner.IsAncestorOf(visual)))
			{
				return Rect.Empty;
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(owner);
			rectangle = generalTransform.TransformBounds(rectangle);
			Rect rect = new Rect(this._offset.X, this._offset.Y, this._viewport.Width, this._viewport.Height);
			rectangle.X += rect.X;
			rectangle.Y += rect.Y;
			double num = this.ComputeScrollOffset(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
			double num2 = this.ComputeScrollOffset(rect.Top, rect.Bottom, rectangle.Top, rectangle.Bottom);
			this.SetHorizontalOffset(owner, num);
			this.SetVerticalOffset(owner, num2);
			if (this.CanHorizontallyScroll)
			{
				rect.X = num;
			}
			else
			{
				rectangle.X = rect.X;
			}
			if (this.CanVerticallyScroll)
			{
				rect.Y = num2;
			}
			else
			{
				rectangle.Y = rect.Y;
			}
			rectangle.Intersect(rect);
			if (!rectangle.IsEmpty)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			return rectangle;
		}

		// Token: 0x0600720A RID: 29194 RVA: 0x00209750 File Offset: 0x00207950
		internal void SetScrollOwner(UIElement owner, ScrollViewer value)
		{
			if (value != this._scrollOwner)
			{
				this._disableHorizonalScroll = false;
				this._disableVerticalScroll = false;
				this._offset = default(Vector);
				this._viewport = default(Size);
				this._extent = default(Size);
				this._scrollOwner = value;
				owner.InvalidateArrange();
			}
		}

		// Token: 0x17001B20 RID: 6944
		// (get) Token: 0x0600720B RID: 29195 RVA: 0x002097A5 File Offset: 0x002079A5
		// (set) Token: 0x0600720C RID: 29196 RVA: 0x002097B0 File Offset: 0x002079B0
		internal bool CanVerticallyScroll
		{
			get
			{
				return !this._disableVerticalScroll;
			}
			set
			{
				this._disableVerticalScroll = !value;
			}
		}

		// Token: 0x17001B21 RID: 6945
		// (get) Token: 0x0600720D RID: 29197 RVA: 0x002097BC File Offset: 0x002079BC
		// (set) Token: 0x0600720E RID: 29198 RVA: 0x002097C7 File Offset: 0x002079C7
		internal bool CanHorizontallyScroll
		{
			get
			{
				return !this._disableHorizonalScroll;
			}
			set
			{
				this._disableHorizonalScroll = !value;
			}
		}

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x0600720F RID: 29199 RVA: 0x002097D3 File Offset: 0x002079D3
		// (set) Token: 0x06007210 RID: 29200 RVA: 0x002097E0 File Offset: 0x002079E0
		internal double ExtentWidth
		{
			get
			{
				return this._extent.Width;
			}
			set
			{
				this._extent.Width = value;
			}
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x06007211 RID: 29201 RVA: 0x002097EE File Offset: 0x002079EE
		// (set) Token: 0x06007212 RID: 29202 RVA: 0x002097FB File Offset: 0x002079FB
		internal double ExtentHeight
		{
			get
			{
				return this._extent.Height;
			}
			set
			{
				this._extent.Height = value;
			}
		}

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x06007213 RID: 29203 RVA: 0x00209809 File Offset: 0x00207A09
		internal double ViewportWidth
		{
			get
			{
				return this._viewport.Width;
			}
		}

		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x06007214 RID: 29204 RVA: 0x00209816 File Offset: 0x00207A16
		internal double ViewportHeight
		{
			get
			{
				return this._viewport.Height;
			}
		}

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x06007215 RID: 29205 RVA: 0x00209823 File Offset: 0x00207A23
		internal double HorizontalOffset
		{
			get
			{
				return this._offset.X;
			}
		}

		// Token: 0x17001B27 RID: 6951
		// (get) Token: 0x06007216 RID: 29206 RVA: 0x00209830 File Offset: 0x00207A30
		internal double VerticalOffset
		{
			get
			{
				return this._offset.Y;
			}
		}

		// Token: 0x17001B28 RID: 6952
		// (get) Token: 0x06007217 RID: 29207 RVA: 0x0020983D File Offset: 0x00207A3D
		internal ScrollViewer ScrollOwner
		{
			get
			{
				return this._scrollOwner;
			}
		}

		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x06007218 RID: 29208 RVA: 0x00209845 File Offset: 0x00207A45
		// (set) Token: 0x06007219 RID: 29209 RVA: 0x0020984D File Offset: 0x00207A4D
		internal Vector Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		// Token: 0x17001B2A RID: 6954
		// (get) Token: 0x0600721A RID: 29210 RVA: 0x00209856 File Offset: 0x00207A56
		// (set) Token: 0x0600721B RID: 29211 RVA: 0x0020985E File Offset: 0x00207A5E
		internal Size Extent
		{
			get
			{
				return this._extent;
			}
			set
			{
				this._extent = value;
			}
		}

		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x0600721C RID: 29212 RVA: 0x00209867 File Offset: 0x00207A67
		// (set) Token: 0x0600721D RID: 29213 RVA: 0x0020986F File Offset: 0x00207A6F
		internal Size Viewport
		{
			get
			{
				return this._viewport;
			}
			set
			{
				this._viewport = value;
			}
		}

		// Token: 0x0600721E RID: 29214 RVA: 0x00209878 File Offset: 0x00207A78
		private double ComputeScrollOffset(double topView, double bottomView, double topChild, double bottomChild)
		{
			bool flag = DoubleUtil.GreaterThanOrClose(topChild, topView) && DoubleUtil.LessThan(topChild, bottomView);
			bool flag2 = DoubleUtil.LessThanOrClose(bottomChild, bottomView) && DoubleUtil.GreaterThan(bottomChild, topView);
			double result;
			if (flag && flag2)
			{
				result = topView;
			}
			else
			{
				result = topChild;
			}
			return result;
		}

		// Token: 0x04003751 RID: 14161
		private bool _disableHorizonalScroll;

		// Token: 0x04003752 RID: 14162
		private bool _disableVerticalScroll;

		// Token: 0x04003753 RID: 14163
		private Vector _offset;

		// Token: 0x04003754 RID: 14164
		private Size _viewport;

		// Token: 0x04003755 RID: 14165
		private Size _extent;

		// Token: 0x04003756 RID: 14166
		private ScrollViewer _scrollOwner;
	}
}
