using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost;
using MS.Internal.Text;

namespace MS.Internal.Documents
{
	// Token: 0x020006CA RID: 1738
	internal class FlowDocumentView : FrameworkElement, IScrollInfo, IServiceProvider
	{
		// Token: 0x06007031 RID: 28721 RVA: 0x000D6F38 File Offset: 0x000D5138
		internal FlowDocumentView()
		{
		}

		// Token: 0x06007032 RID: 28722 RVA: 0x00203AB8 File Offset: 0x00201CB8
		protected sealed override Size MeasureOverride(Size constraint)
		{
			Size result = default(Size);
			if (this._suspendLayout)
			{
				result = base.DesiredSize;
			}
			else if (this.Document != null)
			{
				this.EnsureFormatter();
				this._formatter.Format(constraint);
				if (this._scrollData != null)
				{
					result.Width = Math.Min(constraint.Width, this._formatter.DocumentPage.Size.Width);
					result.Height = Math.Min(constraint.Height, this._formatter.DocumentPage.Size.Height);
				}
				else
				{
					result = this._formatter.DocumentPage.Size;
				}
			}
			return result;
		}

		// Token: 0x06007033 RID: 28723 RVA: 0x00203B70 File Offset: 0x00201D70
		protected sealed override Size ArrangeOverride(Size arrangeSize)
		{
			Rect empty = Rect.Empty;
			bool flag = false;
			Size size = arrangeSize;
			if (!this._suspendLayout)
			{
				TextDpi.SnapToTextDpi(ref size);
				if (this.Document != null)
				{
					this.EnsureFormatter();
					if (this._scrollData != null)
					{
						if (!DoubleUtil.AreClose(this._scrollData.Viewport, size))
						{
							this._scrollData.Viewport = size;
							flag = true;
						}
						if (!DoubleUtil.AreClose(this._scrollData.Extent, this._formatter.DocumentPage.Size))
						{
							this._scrollData.Extent = this._formatter.DocumentPage.Size;
							flag = true;
							if (Math.Abs(this._scrollData.ExtentWidth - this._scrollData.ViewportWidth) < 1.0)
							{
								this._scrollData.ExtentWidth = this._scrollData.ViewportWidth;
							}
							if (Math.Abs(this._scrollData.ExtentHeight - this._scrollData.ViewportHeight) < 1.0)
							{
								this._scrollData.ExtentHeight = this._scrollData.ViewportHeight;
							}
						}
						Vector vector = new Vector(Math.Max(0.0, Math.Min(this._scrollData.ExtentWidth - this._scrollData.ViewportWidth, this._scrollData.HorizontalOffset)), Math.Max(0.0, Math.Min(this._scrollData.ExtentHeight - this._scrollData.ViewportHeight, this._scrollData.VerticalOffset)));
						if (!DoubleUtil.AreClose(vector, this._scrollData.Offset))
						{
							this._scrollData.Offset = vector;
							flag = true;
						}
						if (flag && this._scrollData.ScrollOwner != null)
						{
							this._scrollData.ScrollOwner.InvalidateScrollInfo();
						}
						empty = new Rect(this._scrollData.HorizontalOffset, this._scrollData.VerticalOffset, size.Width, size.Height);
					}
					this._formatter.Arrange(size, empty);
					if (this._pageVisual != this._formatter.DocumentPage.Visual)
					{
						if (this._textView != null)
						{
							this._textView.OnPageConnected();
						}
						if (this._pageVisual != null)
						{
							base.RemoveVisualChild(this._pageVisual);
						}
						this._pageVisual = (PageVisual)this._formatter.DocumentPage.Visual;
						base.AddVisualChild(this._pageVisual);
					}
					if (this._scrollData != null)
					{
						this._pageVisual.Offset = new Vector(-this._scrollData.HorizontalOffset, -this._scrollData.VerticalOffset);
					}
					PtsHelper.UpdateMirroringTransform(base.FlowDirection, FlowDirection.LeftToRight, this._pageVisual, size.Width);
				}
				else
				{
					if (this._pageVisual != null)
					{
						if (this._textView != null)
						{
							this._textView.OnPageDisconnected();
						}
						base.RemoveVisualChild(this._pageVisual);
						this._pageVisual = null;
					}
					if (this._scrollData != null)
					{
						if (!DoubleUtil.AreClose(this._scrollData.Viewport, size))
						{
							this._scrollData.Viewport = size;
							flag = true;
						}
						if (!DoubleUtil.AreClose(this._scrollData.Extent, default(Size)))
						{
							this._scrollData.Extent = default(Size);
							flag = true;
						}
						if (!DoubleUtil.AreClose(this._scrollData.Offset, default(Vector)))
						{
							this._scrollData.Offset = default(Vector);
							flag = true;
						}
						if (flag && this._scrollData.ScrollOwner != null)
						{
							this._scrollData.ScrollOwner.InvalidateScrollInfo();
						}
					}
				}
			}
			return arrangeSize;
		}

		// Token: 0x06007034 RID: 28724 RVA: 0x00203F11 File Offset: 0x00202111
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._pageVisual;
		}

		// Token: 0x17001AA3 RID: 6819
		// (get) Token: 0x06007035 RID: 28725 RVA: 0x00203F37 File Offset: 0x00202137
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._pageVisual != null)
				{
					return 1;
				}
				return 0;
			}
		}

		// Token: 0x06007036 RID: 28726 RVA: 0x00203F44 File Offset: 0x00202144
		internal void SuspendLayout()
		{
			this._suspendLayout = true;
			if (this._pageVisual != null)
			{
				this._pageVisual.Opacity = 0.5;
			}
		}

		// Token: 0x06007037 RID: 28727 RVA: 0x00203F69 File Offset: 0x00202169
		internal void ResumeLayout()
		{
			this._suspendLayout = false;
			if (this._pageVisual != null)
			{
				this._pageVisual.Opacity = 1.0;
			}
			base.InvalidateMeasure();
		}

		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x06007038 RID: 28728 RVA: 0x00203F94 File Offset: 0x00202194
		// (set) Token: 0x06007039 RID: 28729 RVA: 0x00203F9C File Offset: 0x0020219C
		internal FlowDocument Document
		{
			get
			{
				return this._document;
			}
			set
			{
				if (this._formatter != null)
				{
					this.HandleFormatterSuspended(this._formatter, EventArgs.Empty);
				}
				this._suspendLayout = false;
				this._textView = null;
				this._document = value;
				base.InvalidateMeasure();
				base.InvalidateVisual();
			}
		}

		// Token: 0x17001AA5 RID: 6821
		// (get) Token: 0x0600703A RID: 28730 RVA: 0x00203FD8 File Offset: 0x002021D8
		internal FlowDocumentPage DocumentPage
		{
			get
			{
				if (this._document != null)
				{
					this.EnsureFormatter();
					return this._formatter.DocumentPage;
				}
				return null;
			}
		}

		// Token: 0x0600703B RID: 28731 RVA: 0x00203FF8 File Offset: 0x002021F8
		private void EnsureFormatter()
		{
			Invariant.Assert(this._document != null);
			if (this._formatter == null)
			{
				this._formatter = this._document.BottomlessFormatter;
				this._formatter.ContentInvalidated += this.HandleContentInvalidated;
				this._formatter.Suspended += this.HandleFormatterSuspended;
			}
			Invariant.Assert(this._formatter == this._document.BottomlessFormatter);
		}

		// Token: 0x0600703C RID: 28732 RVA: 0x00204072 File Offset: 0x00202272
		private void HandleContentInvalidated(object sender, EventArgs e)
		{
			Invariant.Assert(sender == this._formatter);
			base.InvalidateMeasure();
			base.InvalidateVisual();
		}

		// Token: 0x0600703D RID: 28733 RVA: 0x00204090 File Offset: 0x00202290
		private void HandleFormatterSuspended(object sender, EventArgs e)
		{
			Invariant.Assert(sender == this._formatter);
			this._formatter.ContentInvalidated -= this.HandleContentInvalidated;
			this._formatter.Suspended -= this.HandleFormatterSuspended;
			this._formatter = null;
			if (this._pageVisual != null && !this._suspendLayout)
			{
				if (this._textView != null)
				{
					this._textView.OnPageDisconnected();
				}
				base.RemoveVisualChild(this._pageVisual);
				this._pageVisual = null;
			}
		}

		// Token: 0x0600703E RID: 28734 RVA: 0x00204116 File Offset: 0x00202316
		void IScrollInfo.LineUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineUp(this);
			}
		}

		// Token: 0x0600703F RID: 28735 RVA: 0x0020412C File Offset: 0x0020232C
		void IScrollInfo.LineDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineDown(this);
			}
		}

		// Token: 0x06007040 RID: 28736 RVA: 0x00204142 File Offset: 0x00202342
		void IScrollInfo.LineLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineLeft(this);
			}
		}

		// Token: 0x06007041 RID: 28737 RVA: 0x00204158 File Offset: 0x00202358
		void IScrollInfo.LineRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.LineRight(this);
			}
		}

		// Token: 0x06007042 RID: 28738 RVA: 0x0020416E File Offset: 0x0020236E
		void IScrollInfo.PageUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageUp(this);
			}
		}

		// Token: 0x06007043 RID: 28739 RVA: 0x00204184 File Offset: 0x00202384
		void IScrollInfo.PageDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageDown(this);
			}
		}

		// Token: 0x06007044 RID: 28740 RVA: 0x0020419A File Offset: 0x0020239A
		void IScrollInfo.PageLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageLeft(this);
			}
		}

		// Token: 0x06007045 RID: 28741 RVA: 0x002041B0 File Offset: 0x002023B0
		void IScrollInfo.PageRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.PageRight(this);
			}
		}

		// Token: 0x06007046 RID: 28742 RVA: 0x002041C6 File Offset: 0x002023C6
		void IScrollInfo.MouseWheelUp()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelUp(this);
			}
		}

		// Token: 0x06007047 RID: 28743 RVA: 0x002041DC File Offset: 0x002023DC
		void IScrollInfo.MouseWheelDown()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelDown(this);
			}
		}

		// Token: 0x06007048 RID: 28744 RVA: 0x002041F2 File Offset: 0x002023F2
		void IScrollInfo.MouseWheelLeft()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelLeft(this);
			}
		}

		// Token: 0x06007049 RID: 28745 RVA: 0x00204208 File Offset: 0x00202408
		void IScrollInfo.MouseWheelRight()
		{
			if (this._scrollData != null)
			{
				this._scrollData.MouseWheelRight(this);
			}
		}

		// Token: 0x0600704A RID: 28746 RVA: 0x0020421E File Offset: 0x0020241E
		void IScrollInfo.SetHorizontalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetHorizontalOffset(this, offset);
			}
		}

		// Token: 0x0600704B RID: 28747 RVA: 0x00204235 File Offset: 0x00202435
		void IScrollInfo.SetVerticalOffset(double offset)
		{
			if (this._scrollData != null)
			{
				this._scrollData.SetVerticalOffset(this, offset);
			}
		}

		// Token: 0x0600704C RID: 28748 RVA: 0x0020424C File Offset: 0x0020244C
		Rect IScrollInfo.MakeVisible(Visual visual, Rect rectangle)
		{
			if (this._scrollData == null)
			{
				rectangle = Rect.Empty;
			}
			else
			{
				rectangle = this._scrollData.MakeVisible(this, visual, rectangle);
			}
			return rectangle;
		}

		// Token: 0x17001AA6 RID: 6822
		// (get) Token: 0x0600704D RID: 28749 RVA: 0x00204270 File Offset: 0x00202470
		// (set) Token: 0x0600704E RID: 28750 RVA: 0x00204287 File Offset: 0x00202487
		bool IScrollInfo.CanVerticallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanVerticallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanVerticallyScroll = value;
				}
			}
		}

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x0600704F RID: 28751 RVA: 0x0020429D File Offset: 0x0020249D
		// (set) Token: 0x06007050 RID: 28752 RVA: 0x002042B4 File Offset: 0x002024B4
		bool IScrollInfo.CanHorizontallyScroll
		{
			get
			{
				return this._scrollData != null && this._scrollData.CanHorizontallyScroll;
			}
			set
			{
				if (this._scrollData != null)
				{
					this._scrollData.CanHorizontallyScroll = value;
				}
			}
		}

		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x06007051 RID: 28753 RVA: 0x002042CA File Offset: 0x002024CA
		double IScrollInfo.ExtentWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ExtentWidth;
			}
		}

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x06007052 RID: 28754 RVA: 0x002042E9 File Offset: 0x002024E9
		double IScrollInfo.ExtentHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ExtentHeight;
			}
		}

		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x06007053 RID: 28755 RVA: 0x00204308 File Offset: 0x00202508
		double IScrollInfo.ViewportWidth
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportWidth;
			}
		}

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x06007054 RID: 28756 RVA: 0x00204327 File Offset: 0x00202527
		double IScrollInfo.ViewportHeight
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.ViewportHeight;
			}
		}

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x06007055 RID: 28757 RVA: 0x00204346 File Offset: 0x00202546
		double IScrollInfo.HorizontalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.HorizontalOffset;
			}
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x06007056 RID: 28758 RVA: 0x00204365 File Offset: 0x00202565
		double IScrollInfo.VerticalOffset
		{
			get
			{
				if (this._scrollData == null)
				{
					return 0.0;
				}
				return this._scrollData.VerticalOffset;
			}
		}

		// Token: 0x17001AAE RID: 6830
		// (get) Token: 0x06007057 RID: 28759 RVA: 0x00204384 File Offset: 0x00202584
		// (set) Token: 0x06007058 RID: 28760 RVA: 0x0020439B File Offset: 0x0020259B
		ScrollViewer IScrollInfo.ScrollOwner
		{
			get
			{
				if (this._scrollData == null)
				{
					return null;
				}
				return this._scrollData.ScrollOwner;
			}
			set
			{
				if (this._scrollData == null)
				{
					this._scrollData = new ScrollData();
				}
				this._scrollData.SetScrollOwner(this, value);
			}
		}

		// Token: 0x06007059 RID: 28761 RVA: 0x002043C0 File Offset: 0x002025C0
		object IServiceProvider.GetService(Type serviceType)
		{
			object result = null;
			if (serviceType == typeof(ITextView))
			{
				if (this._textView == null && this._document != null)
				{
					this._textView = new DocumentPageTextView(this, this._document.StructuralCache.TextContainer);
				}
				result = this._textView;
			}
			else if (serviceType == typeof(ITextContainer) && this.Document != null)
			{
				result = this.Document.StructuralCache.TextContainer;
			}
			return result;
		}

		// Token: 0x040036EE RID: 14062
		private FlowDocument _document;

		// Token: 0x040036EF RID: 14063
		private PageVisual _pageVisual;

		// Token: 0x040036F0 RID: 14064
		private FlowDocumentFormatter _formatter;

		// Token: 0x040036F1 RID: 14065
		private ScrollData _scrollData;

		// Token: 0x040036F2 RID: 14066
		private DocumentPageTextView _textView;

		// Token: 0x040036F3 RID: 14067
		private bool _suspendLayout;
	}
}
