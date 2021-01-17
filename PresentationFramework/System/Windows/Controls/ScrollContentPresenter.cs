using System;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal;
using MS.Utility;

namespace System.Windows.Controls
{
	/// <summary>Displays the content of a <see cref="T:System.Windows.Controls.ScrollViewer" /> control.</summary>
	// Token: 0x0200051A RID: 1306
	public sealed class ScrollContentPresenter : ContentPresenter, IScrollInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> class. </summary>
		// Token: 0x06005433 RID: 21555 RVA: 0x001751F8 File Offset: 0x001733F8
		public ScrollContentPresenter()
		{
			this._adornerLayer = new AdornerLayer();
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content upward by one line.</summary>
		// Token: 0x06005434 RID: 21556 RVA: 0x0017520B File Offset: 0x0017340B
		public void LineUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - 16.0);
			}
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content downward by one line.</summary>
		// Token: 0x06005435 RID: 21557 RVA: 0x0017522B File Offset: 0x0017342B
		public void LineDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + 16.0);
			}
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content to the left by a predetermined amount.</summary>
		// Token: 0x06005436 RID: 21558 RVA: 0x0017524B File Offset: 0x0017344B
		public void LineLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - 16.0);
			}
		}

		/// <summary>Scrolls the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> content to the right by a predetermined amount.</summary>
		// Token: 0x06005437 RID: 21559 RVA: 0x0017526B File Offset: 0x0017346B
		public void LineRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + 16.0);
			}
		}

		/// <summary>Scrolls up within content by one page.</summary>
		// Token: 0x06005438 RID: 21560 RVA: 0x0017528B File Offset: 0x0017348B
		public void PageUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - this.ViewportHeight);
			}
		}

		/// <summary>Scrolls down within content by one page.</summary>
		// Token: 0x06005439 RID: 21561 RVA: 0x001752A8 File Offset: 0x001734A8
		public void PageDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + this.ViewportHeight);
			}
		}

		/// <summary>Scrolls left within content by one page.</summary>
		// Token: 0x0600543A RID: 21562 RVA: 0x001752C5 File Offset: 0x001734C5
		public void PageLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - this.ViewportWidth);
			}
		}

		/// <summary>Scrolls right within content by one page.</summary>
		// Token: 0x0600543B RID: 21563 RVA: 0x001752E2 File Offset: 0x001734E2
		public void PageRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + this.ViewportWidth);
			}
		}

		/// <summary>Scrolls up within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x0600543C RID: 21564 RVA: 0x001752FF File Offset: 0x001734FF
		public void MouseWheelUp()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset - 48.0);
			}
		}

		/// <summary>Scrolls down within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x0600543D RID: 21565 RVA: 0x0017531F File Offset: 0x0017351F
		public void MouseWheelDown()
		{
			if (this.IsScrollClient)
			{
				this.SetVerticalOffset(this.VerticalOffset + 48.0);
			}
		}

		/// <summary>Scrolls left within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x0600543E RID: 21566 RVA: 0x0017533F File Offset: 0x0017353F
		public void MouseWheelLeft()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset - 48.0);
			}
		}

		/// <summary>Scrolls right within content after a user clicks the wheel button on a mouse.</summary>
		// Token: 0x0600543F RID: 21567 RVA: 0x0017535F File Offset: 0x0017355F
		public void MouseWheelRight()
		{
			if (this.IsScrollClient)
			{
				this.SetHorizontalOffset(this.HorizontalOffset + 48.0);
			}
		}

		/// <summary>Sets the amount of horizontal offset.</summary>
		/// <param name="offset">The degree to which content is horizontally offset from the containing viewport.</param>
		// Token: 0x06005440 RID: 21568 RVA: 0x00175380 File Offset: 0x00173580
		public void SetHorizontalOffset(double offset)
		{
			if (this.IsScrollClient)
			{
				double num = ScrollContentPresenter.ValidateInputOffset(offset, "HorizontalOffset");
				if (!DoubleUtil.AreClose(this.EnsureScrollData()._offset.X, num))
				{
					this._scrollData._offset.X = num;
					base.InvalidateArrange();
				}
			}
		}

		/// <summary>Sets the amount of vertical offset.</summary>
		/// <param name="offset">The degree to which content is vertically offset from the containing viewport.</param>
		// Token: 0x06005441 RID: 21569 RVA: 0x001753D0 File Offset: 0x001735D0
		public void SetVerticalOffset(double offset)
		{
			if (this.IsScrollClient)
			{
				double num = ScrollContentPresenter.ValidateInputOffset(offset, "VerticalOffset");
				if (!DoubleUtil.AreClose(this.EnsureScrollData()._offset.Y, num))
				{
					this._scrollData._offset.Y = num;
					base.InvalidateArrange();
				}
			}
		}

		/// <summary>Forces content to scroll until the coordinate space of a <see cref="T:System.Windows.Media.Visual" /> object is visible. </summary>
		/// <param name="visual">The <see cref="T:System.Windows.Media.Visual" /> that becomes visible.</param>
		/// <param name="rectangle">The bounding rectangle that identifies the coordinate space to make visible.</param>
		/// <returns>A <see cref="T:System.Windows.Rect" /> that represents the visible region.</returns>
		// Token: 0x06005442 RID: 21570 RVA: 0x00175420 File Offset: 0x00173620
		public Rect MakeVisible(Visual visual, Rect rectangle)
		{
			return this.MakeVisible(visual, rectangle, true);
		}

		/// <summary>Gets the <see cref="T:System.Windows.Documents.AdornerLayer" /> on which adorners are rendered.</summary>
		/// <returns>The <see cref="T:System.Windows.Documents.AdornerLayer" /> on which adorners are rendered.</returns>
		// Token: 0x17001476 RID: 5238
		// (get) Token: 0x06005443 RID: 21571 RVA: 0x0017542B File Offset: 0x0017362B
		public AdornerLayer AdornerLayer
		{
			get
			{
				return this._adornerLayer;
			}
		}

		/// <summary>Indicates whether the content, if it supports <see cref="T:System.Windows.Controls.Primitives.IScrollInfo" />, should be allowed to control scrolling.   </summary>
		/// <returns>
		///     <see langword="true" /> if the content is allowed to scroll; otherwise, <see langword="false" />. A <see langword="false" /> value indicates that the <see cref="T:System.Windows.Controls.ScrollContentPresenter" /> acts as the scrolling client. This property has no default value.</returns>
		// Token: 0x17001477 RID: 5239
		// (get) Token: 0x06005444 RID: 21572 RVA: 0x00175433 File Offset: 0x00173633
		// (set) Token: 0x06005445 RID: 21573 RVA: 0x00175445 File Offset: 0x00173645
		public bool CanContentScroll
		{
			get
			{
				return (bool)base.GetValue(ScrollContentPresenter.CanContentScrollProperty);
			}
			set
			{
				base.SetValue(ScrollContentPresenter.CanContentScrollProperty, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether scrolling on the horizontal axis is possible.</summary>
		/// <returns>
		///     <see langword="true" /> if scrolling is possible; otherwise, <see langword="false" />. This property has no default value.</returns>
		// Token: 0x17001478 RID: 5240
		// (get) Token: 0x06005446 RID: 21574 RVA: 0x00175453 File Offset: 0x00173653
		// (set) Token: 0x06005447 RID: 21575 RVA: 0x0017546A File Offset: 0x0017366A
		public bool CanHorizontallyScroll
		{
			get
			{
				return this.IsScrollClient && this.EnsureScrollData()._canHorizontallyScroll;
			}
			set
			{
				if (this.IsScrollClient && this.EnsureScrollData()._canHorizontallyScroll != value)
				{
					this._scrollData._canHorizontallyScroll = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether scrolling on the vertical axis is possible.</summary>
		/// <returns>
		///     <see langword="true" /> if scrolling is possible; otherwise, <see langword="false" />. This property has no default value.</returns>
		// Token: 0x17001479 RID: 5241
		// (get) Token: 0x06005448 RID: 21576 RVA: 0x00175494 File Offset: 0x00173694
		// (set) Token: 0x06005449 RID: 21577 RVA: 0x001754AB File Offset: 0x001736AB
		public bool CanVerticallyScroll
		{
			get
			{
				return this.IsScrollClient && this.EnsureScrollData()._canVerticallyScroll;
			}
			set
			{
				if (this.IsScrollClient && this.EnsureScrollData()._canVerticallyScroll != value)
				{
					this._scrollData._canVerticallyScroll = value;
					base.InvalidateMeasure();
				}
			}
		}

		/// <summary>Gets the horizontal size of the extent.</summary>
		/// <returns>The horizontal size of the extent. This property has no default value.</returns>
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x0600544A RID: 21578 RVA: 0x001754D5 File Offset: 0x001736D5
		public double ExtentWidth
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._extent.Width;
			}
		}

		/// <summary>Gets the vertical size of the extent.</summary>
		/// <returns>The vertical size of the extent. This property has no default value.</returns>
		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x0600544B RID: 21579 RVA: 0x001754F9 File Offset: 0x001736F9
		public double ExtentHeight
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._extent.Height;
			}
		}

		/// <summary>Gets the horizontal size of the viewport for this content.</summary>
		/// <returns>The horizontal size of the viewport for this content. This property has no default value.</returns>
		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x0600544C RID: 21580 RVA: 0x0017551D File Offset: 0x0017371D
		public double ViewportWidth
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._viewport.Width;
			}
		}

		/// <summary>Gets the vertical size of the viewport for this content.</summary>
		/// <returns>The vertical size of the viewport for this content. This property has no default value.</returns>
		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x0600544D RID: 21581 RVA: 0x00175541 File Offset: 0x00173741
		public double ViewportHeight
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._viewport.Height;
			}
		}

		/// <summary>Gets the horizontal offset of the scrolled content.</summary>
		/// <returns>The horizontal offset. This property has no default value.</returns>
		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x0600544E RID: 21582 RVA: 0x00175565 File Offset: 0x00173765
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double HorizontalOffset
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._computedOffset.X;
			}
		}

		/// <summary>Gets the vertical offset of the scrolled content.</summary>
		/// <returns>The vertical offset of the scrolled content. Valid values are between zero and the <see cref="P:System.Windows.Controls.ScrollContentPresenter.ExtentHeight" /> minus the <see cref="P:System.Windows.Controls.ScrollContentPresenter.ViewportHeight" />. This property has no default value.</returns>
		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x0600544F RID: 21583 RVA: 0x00175589 File Offset: 0x00173789
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double VerticalOffset
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return 0.0;
				}
				return this.EnsureScrollData()._computedOffset.Y;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Windows.Controls.ScrollViewer" /> element that controls scrolling behavior.</summary>
		/// <returns>The <see cref="T:System.Windows.Controls.ScrollViewer" /> element that controls scrolling behavior. This property has no default value.</returns>
		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x06005450 RID: 21584 RVA: 0x001755AD File Offset: 0x001737AD
		// (set) Token: 0x06005451 RID: 21585 RVA: 0x001755C4 File Offset: 0x001737C4
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ScrollViewer ScrollOwner
		{
			get
			{
				if (!this.IsScrollClient)
				{
					return null;
				}
				return this._scrollData._scrollOwner;
			}
			set
			{
				if (this.IsScrollClient)
				{
					this._scrollData._scrollOwner = value;
				}
			}
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x06005452 RID: 21586 RVA: 0x001755DA File Offset: 0x001737DA
		protected override int VisualChildrenCount
		{
			get
			{
				if (base.TemplateChild != null)
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x06005453 RID: 21587 RVA: 0x001755E8 File Offset: 0x001737E8
		protected override Visual GetVisualChild(int index)
		{
			if (base.TemplateChild == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0)
			{
				return base.TemplateChild;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._adornerLayer;
		}

		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x06005454 RID: 21588 RVA: 0x00175649 File Offset: 0x00173849
		// (set) Token: 0x06005455 RID: 21589 RVA: 0x00175654 File Offset: 0x00173854
		internal override UIElement TemplateChild
		{
			get
			{
				return base.TemplateChild;
			}
			set
			{
				UIElement templateChild = base.TemplateChild;
				if (value != templateChild)
				{
					if (templateChild != null && value == null)
					{
						base.RemoveVisualChild(this._adornerLayer);
					}
					base.TemplateChild = value;
					if (templateChild == null && value != null)
					{
						base.AddVisualChild(this._adornerLayer);
					}
				}
			}
		}

		// Token: 0x06005456 RID: 21590 RVA: 0x00175698 File Offset: 0x00173898
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = default(Size);
			bool flag = this.IsScrollClient && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:MeasureOverride");
			}
			try
			{
				int visualChildrenCount = this.VisualChildrenCount;
				if (visualChildrenCount > 0)
				{
					this._adornerLayer.Measure(constraint);
					if (!this.IsScrollClient)
					{
						size = base.MeasureOverride(constraint);
					}
					else
					{
						Size constraint2 = constraint;
						if (this._scrollData._canHorizontallyScroll)
						{
							constraint2.Width = double.PositiveInfinity;
						}
						if (this._scrollData._canVerticallyScroll)
						{
							constraint2.Height = double.PositiveInfinity;
						}
						size = base.MeasureOverride(constraint2);
					}
				}
				if (this.IsScrollClient)
				{
					this.VerifyScrollData(constraint, size);
				}
				size.Width = Math.Min(constraint.Width, size.Width);
				size.Height = Math.Min(constraint.Height, size.Height);
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:MeasureOverride");
				}
			}
			return size;
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x001757B4 File Offset: 0x001739B4
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			bool flag = this.IsScrollClient && EventTrace.IsEnabled(EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info);
			if (flag)
			{
				EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringBegin, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:ArrangeOverride");
			}
			try
			{
				int visualChildrenCount = this.VisualChildrenCount;
				if (this.IsScrollClient)
				{
					this.VerifyScrollData(arrangeSize, this._scrollData._extent);
				}
				if (visualChildrenCount > 0)
				{
					this._adornerLayer.Arrange(new Rect(arrangeSize));
					UIElement uielement = this.GetVisualChild(0) as UIElement;
					if (uielement != null)
					{
						Rect finalRect = new Rect(uielement.DesiredSize);
						if (this.IsScrollClient)
						{
							finalRect.X = -this.HorizontalOffset;
							finalRect.Y = -this.VerticalOffset;
						}
						finalRect.Width = Math.Max(finalRect.Width, arrangeSize.Width);
						finalRect.Height = Math.Max(finalRect.Height, arrangeSize.Height);
						uielement.Arrange(finalRect);
					}
				}
			}
			finally
			{
				if (flag)
				{
					EventTrace.EventProvider.TraceEvent(EventTrace.Event.WClientStringEnd, EventTrace.Keyword.KeywordGeneral, EventTrace.Level.Info, "SCROLLCONTENTPRESENTER:ArrangeOverride");
				}
			}
			return arrangeSize;
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x001758CC File Offset: 0x00173ACC
		protected override Geometry GetLayoutClip(Size layoutSlotSize)
		{
			return new RectangleGeometry(new Rect(base.RenderSize));
		}

		/// <summary>Invoked when an internal process or application calls <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />, which is used to build the visual tree of the current template.</summary>
		// Token: 0x06005459 RID: 21593 RVA: 0x001758DE File Offset: 0x00173ADE
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.HookupScrollingComponents();
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x001758EC File Offset: 0x00173AEC
		internal Rect MakeVisible(Visual visual, Rect rectangle, bool throwOnError)
		{
			if (rectangle.IsEmpty || visual == null || visual == this || !base.IsAncestorOf(visual))
			{
				return Rect.Empty;
			}
			GeneralTransform generalTransform = visual.TransformToAncestor(this);
			rectangle = generalTransform.TransformBounds(rectangle);
			if (!this.IsScrollClient || (!throwOnError && rectangle.IsEmpty))
			{
				return rectangle;
			}
			Rect rect = new Rect(this.HorizontalOffset, this.VerticalOffset, this.ViewportWidth, this.ViewportHeight);
			rectangle.X += rect.X;
			rectangle.Y += rect.Y;
			double num = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect.Left, rect.Right, rectangle.Left, rectangle.Right);
			double num2 = ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(rect.Top, rect.Bottom, rectangle.Top, rectangle.Bottom);
			this.SetHorizontalOffset(num);
			this.SetVerticalOffset(num2);
			rect.X = num;
			rect.Y = num2;
			rectangle.Intersect(rect);
			if (throwOnError)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			else if (!rectangle.IsEmpty)
			{
				rectangle.X -= rect.X;
				rectangle.Y -= rect.Y;
			}
			return rectangle;
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x00175A58 File Offset: 0x00173C58
		internal static double ComputeScrollOffsetWithMinimalScroll(double topView, double bottomView, double topChild, double bottomChild)
		{
			bool flag = false;
			bool flag2 = false;
			return ScrollContentPresenter.ComputeScrollOffsetWithMinimalScroll(topView, bottomView, topChild, bottomChild, ref flag, ref flag2);
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x00175A78 File Offset: 0x00173C78
		internal static double ComputeScrollOffsetWithMinimalScroll(double topView, double bottomView, double topChild, double bottomChild, ref bool alignTop, ref bool alignBottom)
		{
			bool flag = DoubleUtil.LessThan(topChild, topView) && DoubleUtil.LessThan(bottomChild, bottomView);
			bool flag2 = DoubleUtil.GreaterThan(bottomChild, bottomView) && DoubleUtil.GreaterThan(topChild, topView);
			bool flag3 = bottomChild - topChild > bottomView - topView;
			if (((flag && !flag3) || (flag2 && flag3)) | alignTop)
			{
				alignTop = true;
				return topChild;
			}
			if ((flag || flag2) | alignBottom)
			{
				alignBottom = true;
				return bottomChild - (bottomView - topView);
			}
			return topView;
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x00175AE2 File Offset: 0x00173CE2
		internal static double ValidateInputOffset(double offset, string parameterName)
		{
			if (DoubleUtil.IsNaN(offset))
			{
				throw new ArgumentOutOfRangeException(parameterName, SR.Get("ScrollViewer_CannotBeNaN", new object[]
				{
					parameterName
				}));
			}
			return Math.Max(0.0, offset);
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x00175B16 File Offset: 0x00173D16
		private ScrollContentPresenter.ScrollData EnsureScrollData()
		{
			if (this._scrollData == null)
			{
				this._scrollData = new ScrollContentPresenter.ScrollData();
			}
			return this._scrollData;
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x00175B34 File Offset: 0x00173D34
		internal void HookupScrollingComponents()
		{
			ScrollViewer scrollViewer = base.TemplatedParent as ScrollViewer;
			if (scrollViewer != null)
			{
				IScrollInfo scrollInfo = null;
				if (this.CanContentScroll)
				{
					scrollInfo = (base.Content as IScrollInfo);
					if (scrollInfo == null)
					{
						Visual visual = base.Content as Visual;
						if (visual != null)
						{
							ItemsPresenter itemsPresenter = visual as ItemsPresenter;
							if (itemsPresenter == null)
							{
								FrameworkElement frameworkElement = scrollViewer.TemplatedParent as FrameworkElement;
								if (frameworkElement != null)
								{
									itemsPresenter = (frameworkElement.GetTemplateChild("ItemsPresenter") as ItemsPresenter);
								}
							}
							if (itemsPresenter != null)
							{
								itemsPresenter.ApplyTemplate();
								int childrenCount = VisualTreeHelper.GetChildrenCount(itemsPresenter);
								if (childrenCount > 0)
								{
									scrollInfo = (VisualTreeHelper.GetChild(itemsPresenter, 0) as IScrollInfo);
								}
							}
						}
					}
				}
				if (scrollInfo == null)
				{
					scrollInfo = this;
					this.EnsureScrollData();
				}
				if (scrollInfo != this._scrollInfo && this._scrollInfo != null)
				{
					if (this.IsScrollClient)
					{
						this._scrollData = null;
					}
					else
					{
						this._scrollInfo.ScrollOwner = null;
					}
				}
				if (scrollInfo != null)
				{
					this._scrollInfo = scrollInfo;
					scrollInfo.ScrollOwner = scrollViewer;
					scrollViewer.ScrollInfo = scrollInfo;
					return;
				}
			}
			else if (this._scrollInfo != null)
			{
				if (this._scrollInfo.ScrollOwner != null)
				{
					this._scrollInfo.ScrollOwner.ScrollInfo = null;
				}
				this._scrollInfo.ScrollOwner = null;
				this._scrollInfo = null;
				this._scrollData = null;
			}
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x00175C60 File Offset: 0x00173E60
		private void VerifyScrollData(Size viewport, Size extent)
		{
			bool flag = true;
			if (double.IsInfinity(viewport.Width))
			{
				viewport.Width = extent.Width;
			}
			if (double.IsInfinity(viewport.Height))
			{
				viewport.Height = extent.Height;
			}
			flag &= DoubleUtil.AreClose(viewport, this._scrollData._viewport);
			flag &= DoubleUtil.AreClose(extent, this._scrollData._extent);
			this._scrollData._viewport = viewport;
			this._scrollData._extent = extent;
			if (!(flag & this.CoerceOffsets()))
			{
				this.ScrollOwner.InvalidateScrollInfo();
			}
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x00175CFE File Offset: 0x00173EFE
		internal static double CoerceOffset(double offset, double extent, double viewport)
		{
			if (offset > extent - viewport)
			{
				offset = extent - viewport;
			}
			if (offset < 0.0)
			{
				offset = 0.0;
			}
			return offset;
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x00175D24 File Offset: 0x00173F24
		private bool CoerceOffsets()
		{
			Vector vector = new Vector(ScrollContentPresenter.CoerceOffset(this._scrollData._offset.X, this._scrollData._extent.Width, this._scrollData._viewport.Width), ScrollContentPresenter.CoerceOffset(this._scrollData._offset.Y, this._scrollData._extent.Height, this._scrollData._viewport.Height));
			bool result = DoubleUtil.AreClose(this._scrollData._computedOffset, vector);
			this._scrollData._computedOffset = vector;
			return result;
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x00175DC4 File Offset: 0x00173FC4
		private static void OnCanContentScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ScrollContentPresenter scrollContentPresenter = (ScrollContentPresenter)d;
			if (scrollContentPresenter._scrollInfo == null)
			{
				return;
			}
			scrollContentPresenter.HookupScrollingComponents();
			scrollContentPresenter.InvalidateMeasure();
		}

		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x06005464 RID: 21604 RVA: 0x00175DED File Offset: 0x00173FED
		private bool IsScrollClient
		{
			get
			{
				return this._scrollInfo == this;
			}
		}

		// Token: 0x17001484 RID: 5252
		// (get) Token: 0x06005465 RID: 21605 RVA: 0x00095684 File Offset: 0x00093884
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 42;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ScrollContentPresenter.CanContentScroll" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ScrollContentPresenter.CanContentScroll" /> dependency property.</returns>
		// Token: 0x04002D8C RID: 11660
		public static readonly DependencyProperty CanContentScrollProperty = ScrollViewer.CanContentScrollProperty.AddOwner(typeof(ScrollContentPresenter), new FrameworkPropertyMetadata(new PropertyChangedCallback(ScrollContentPresenter.OnCanContentScrollChanged)));

		// Token: 0x04002D8D RID: 11661
		private IScrollInfo _scrollInfo;

		// Token: 0x04002D8E RID: 11662
		private ScrollContentPresenter.ScrollData _scrollData;

		// Token: 0x04002D8F RID: 11663
		private readonly AdornerLayer _adornerLayer;

		// Token: 0x020009AE RID: 2478
		private class ScrollData
		{
			// Token: 0x04004517 RID: 17687
			internal ScrollViewer _scrollOwner;

			// Token: 0x04004518 RID: 17688
			internal bool _canHorizontallyScroll;

			// Token: 0x04004519 RID: 17689
			internal bool _canVerticallyScroll;

			// Token: 0x0400451A RID: 17690
			internal Vector _offset;

			// Token: 0x0400451B RID: 17691
			internal Vector _computedOffset;

			// Token: 0x0400451C RID: 17692
			internal Size _viewport;

			// Token: 0x0400451D RID: 17693
			internal Size _extent;
		}
	}
}
