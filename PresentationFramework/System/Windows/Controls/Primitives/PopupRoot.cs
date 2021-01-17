using System;
using System.Windows.Automation.Peers;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MS.Internal.KnownBoxes;

namespace System.Windows.Controls.Primitives
{
	// Token: 0x0200059E RID: 1438
	internal sealed class PopupRoot : FrameworkElement
	{
		// Token: 0x06005F27 RID: 24359 RVA: 0x001AAE90 File Offset: 0x001A9090
		static PopupRoot()
		{
			UIElement.SnapsToDevicePixelsProperty.OverrideMetadata(typeof(PopupRoot), new FrameworkPropertyMetadata(BooleanBoxes.TrueBox));
		}

		// Token: 0x06005F28 RID: 24360 RVA: 0x001AAEB0 File Offset: 0x001A90B0
		internal PopupRoot()
		{
			this.Initialize();
		}

		// Token: 0x06005F29 RID: 24361 RVA: 0x001AAEC0 File Offset: 0x001A90C0
		private void Initialize()
		{
			this._transformDecorator = new Decorator();
			base.AddVisualChild(this._transformDecorator);
			this._transformDecorator.ClipToBounds = true;
			this._adornerDecorator = new NonLogicalAdornerDecorator();
			this._transformDecorator.Child = this._adornerDecorator;
		}

		// Token: 0x170016F3 RID: 5875
		// (get) Token: 0x06005F2A RID: 24362 RVA: 0x00016748 File Offset: 0x00014948
		protected override int VisualChildrenCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06005F2B RID: 24363 RVA: 0x001AAF0C File Offset: 0x001A910C
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._transformDecorator;
		}

		// Token: 0x06005F2C RID: 24364 RVA: 0x001AAF32 File Offset: 0x001A9132
		protected override AutomationPeer OnCreateAutomationPeer()
		{
			return new PopupRootAutomationPeer(this);
		}

		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x06005F2D RID: 24365 RVA: 0x001AAF3A File Offset: 0x001A913A
		// (set) Token: 0x06005F2E RID: 24366 RVA: 0x001AAF47 File Offset: 0x001A9147
		internal UIElement Child
		{
			get
			{
				return this._adornerDecorator.Child;
			}
			set
			{
				this._adornerDecorator.Child = value;
			}
		}

		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x06005F2F RID: 24367 RVA: 0x001AAF58 File Offset: 0x001A9158
		internal Vector AnimationOffset
		{
			get
			{
				TranslateTransform translateTransform = this._adornerDecorator.RenderTransform as TranslateTransform;
				if (translateTransform != null)
				{
					return new Vector(translateTransform.X, translateTransform.Y);
				}
				return default(Vector);
			}
		}

		// Token: 0x170016F6 RID: 5878
		// (set) Token: 0x06005F30 RID: 24368 RVA: 0x001AAF94 File Offset: 0x001A9194
		internal Transform Transform
		{
			set
			{
				this._transformDecorator.LayoutTransform = value;
			}
		}

		// Token: 0x06005F31 RID: 24369 RVA: 0x001AAFA4 File Offset: 0x001A91A4
		protected override Size MeasureOverride(Size constraint)
		{
			Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Popup popup = base.Parent as Popup;
			try
			{
				this._transformDecorator.Measure(size);
			}
			catch (Exception savedException)
			{
				if (popup != null)
				{
					popup.SavedException = savedException;
				}
				throw;
			}
			size = this._transformDecorator.DesiredSize;
			if (popup != null)
			{
				bool flag;
				bool flag2;
				Size popupSizeRestrictions = this.GetPopupSizeRestrictions(popup, size, out flag, out flag2);
				if (flag || flag2)
				{
					if (flag == flag2)
					{
						size = this.Get2DRestrictedDesiredSize(popupSizeRestrictions);
					}
					else
					{
						Size availableSize = new Size(flag ? popupSizeRestrictions.Width : double.PositiveInfinity, flag2 ? popupSizeRestrictions.Height : double.PositiveInfinity);
						this._transformDecorator.Measure(availableSize);
						size = this._transformDecorator.DesiredSize;
						popupSizeRestrictions = this.GetPopupSizeRestrictions(popup, size, out flag, out flag2);
						if (flag || flag2)
						{
							size = this.Get2DRestrictedDesiredSize(popupSizeRestrictions);
						}
					}
				}
			}
			return size;
		}

		// Token: 0x06005F32 RID: 24370 RVA: 0x001AB0A0 File Offset: 0x001A92A0
		private Size GetPopupSizeRestrictions(Popup popup, Size desiredSize, out bool restrictWidth, out bool restrictHeight)
		{
			Size result = popup.RestrictSize(desiredSize);
			restrictWidth = (Math.Abs(result.Width - desiredSize.Width) > 0.01);
			restrictHeight = (Math.Abs(result.Height - desiredSize.Height) > 0.01);
			return result;
		}

		// Token: 0x06005F33 RID: 24371 RVA: 0x001AB0FC File Offset: 0x001A92FC
		private Size Get2DRestrictedDesiredSize(Size restrictedSize)
		{
			this._transformDecorator.Measure(restrictedSize);
			Size desiredSize = this._transformDecorator.DesiredSize;
			return new Size(Math.Min(restrictedSize.Width, desiredSize.Width), Math.Min(restrictedSize.Height, desiredSize.Height));
		}

		// Token: 0x06005F34 RID: 24372 RVA: 0x001AB14C File Offset: 0x001A934C
		protected override Size ArrangeOverride(Size arrangeSize)
		{
			this._transformDecorator.Arrange(new Rect(arrangeSize));
			return arrangeSize;
		}

		// Token: 0x06005F35 RID: 24373 RVA: 0x001AB160 File Offset: 0x001A9360
		internal void SetupLayoutBindings(Popup popup)
		{
			Binding binding = new Binding("Width");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.WidthProperty, binding);
			binding = new Binding("Height");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.HeightProperty, binding);
			binding = new Binding("MinWidth");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MinWidthProperty, binding);
			binding = new Binding("MinHeight");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MinHeightProperty, binding);
			binding = new Binding("MaxWidth");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MaxWidthProperty, binding);
			binding = new Binding("MaxHeight");
			binding.Mode = BindingMode.OneWay;
			binding.Source = popup;
			this._adornerDecorator.SetBinding(FrameworkElement.MaxHeightProperty, binding);
		}

		// Token: 0x06005F36 RID: 24374 RVA: 0x001AB270 File Offset: 0x001A9470
		internal void SetupFadeAnimation(Duration duration, bool visible)
		{
			DoubleAnimation animation = new DoubleAnimation(visible ? 0.0 : 1.0, visible ? 1.0 : 0.0, duration, FillBehavior.HoldEnd);
			base.BeginAnimation(UIElement.OpacityProperty, animation);
		}

		// Token: 0x06005F37 RID: 24375 RVA: 0x001AB2C0 File Offset: 0x001A94C0
		internal void SetupTranslateAnimations(PopupAnimation animationType, Duration duration, bool animateFromRight, bool animateFromBottom)
		{
			UIElement child = this.Child;
			if (child == null)
			{
				return;
			}
			TranslateTransform translateTransform = this._adornerDecorator.RenderTransform as TranslateTransform;
			if (translateTransform == null)
			{
				translateTransform = new TranslateTransform();
				this._adornerDecorator.RenderTransform = translateTransform;
			}
			if (animationType == PopupAnimation.Scroll)
			{
				FlowDirection flowDirection = (FlowDirection)child.GetValue(FrameworkElement.FlowDirectionProperty);
				FlowDirection flowDirection2 = base.FlowDirection;
				if (flowDirection != flowDirection2)
				{
					animateFromRight = !animateFromRight;
				}
				double width = this._adornerDecorator.RenderSize.Width;
				DoubleAnimation animation = new DoubleAnimation(animateFromRight ? width : (-width), 0.0, duration, FillBehavior.Stop);
				translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
			}
			double height = this._adornerDecorator.RenderSize.Height;
			DoubleAnimation animation2 = new DoubleAnimation(animateFromBottom ? height : (-height), 0.0, duration, FillBehavior.Stop);
			translateTransform.BeginAnimation(TranslateTransform.YProperty, animation2);
		}

		// Token: 0x06005F38 RID: 24376 RVA: 0x001AB3A4 File Offset: 0x001A95A4
		internal void StopAnimations()
		{
			base.BeginAnimation(UIElement.OpacityProperty, null);
			TranslateTransform translateTransform = this._adornerDecorator.RenderTransform as TranslateTransform;
			if (translateTransform != null)
			{
				translateTransform.BeginAnimation(TranslateTransform.XProperty, null);
				translateTransform.BeginAnimation(TranslateTransform.YProperty, null);
			}
		}

		// Token: 0x06005F39 RID: 24377 RVA: 0x001AB3EC File Offset: 0x001A95EC
		internal override bool IgnoreModelParentBuildRoute(RoutedEventArgs e)
		{
			if (e is QueryCursorEventArgs)
			{
				return true;
			}
			FrameworkElement frameworkElement = this.Child as FrameworkElement;
			if (frameworkElement != null)
			{
				return frameworkElement.IgnoreModelParentBuildRoute(e);
			}
			return base.IgnoreModelParentBuildRoute(e);
		}

		// Token: 0x04003092 RID: 12434
		private Decorator _transformDecorator;

		// Token: 0x04003093 RID: 12435
		private AdornerDecorator _adornerDecorator;
	}
}
