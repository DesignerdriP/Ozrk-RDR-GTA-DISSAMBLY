using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007DE RID: 2014
	internal sealed class AnnotationAdorner : Adorner
	{
		// Token: 0x06007C7E RID: 31870 RVA: 0x002302A8 File Offset: 0x0022E4A8
		public AnnotationAdorner(IAnnotationComponent component, UIElement annotatedElement) : base(annotatedElement)
		{
			if (component is UIElement)
			{
				this._annotationComponent = component;
				base.AddVisualChild((UIElement)this._annotationComponent);
				return;
			}
			throw new ArgumentException(SR.Get("AnnotationAdorner_NotUIElement"), "component");
		}

		// Token: 0x06007C7F RID: 31871 RVA: 0x002302E8 File Offset: 0x0022E4E8
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (!(this._annotationComponent is UIElement))
			{
				return null;
			}
			transform = base.GetDesiredTransform(transform);
			GeneralTransform desiredTransform = this._annotationComponent.GetDesiredTransform(transform);
			if (this._annotationComponent.AnnotatedElement == null)
			{
				return null;
			}
			if (desiredTransform == null)
			{
				this._annotatedElement = this._annotationComponent.AnnotatedElement;
				this._annotatedElement.LayoutUpdated += this.OnLayoutUpdated;
				return transform;
			}
			return desiredTransform;
		}

		// Token: 0x06007C80 RID: 31872 RVA: 0x00230357 File Offset: 0x0022E557
		protected override Visual GetVisualChild(int index)
		{
			if (index != 0 || this._annotationComponent == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return (UIElement)this._annotationComponent;
		}

		// Token: 0x17001CEE RID: 7406
		// (get) Token: 0x06007C81 RID: 31873 RVA: 0x0023038A File Offset: 0x0022E58A
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._annotationComponent == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06007C82 RID: 31874 RVA: 0x00230398 File Offset: 0x0022E598
		protected override Size MeasureOverride(Size availableSize)
		{
			Size availableSize2 = new Size(double.PositiveInfinity, double.PositiveInfinity);
			Invariant.Assert(this._annotationComponent != null, "AnnotationAdorner should only have one child - the annotation component.");
			((UIElement)this._annotationComponent).Measure(availableSize2);
			return new Size(0.0, 0.0);
		}

		// Token: 0x06007C83 RID: 31875 RVA: 0x002303F9 File Offset: 0x0022E5F9
		protected override Size ArrangeOverride(Size finalSize)
		{
			Invariant.Assert(this._annotationComponent != null, "AnnotationAdorner should only have one child - the annotation component.");
			((UIElement)this._annotationComponent).Arrange(new Rect(((UIElement)this._annotationComponent).DesiredSize));
			return finalSize;
		}

		// Token: 0x06007C84 RID: 31876 RVA: 0x00230434 File Offset: 0x0022E634
		internal void RemoveChildren()
		{
			base.RemoveVisualChild((UIElement)this._annotationComponent);
			this._annotationComponent = null;
		}

		// Token: 0x06007C85 RID: 31877 RVA: 0x00230450 File Offset: 0x0022E650
		internal void InvalidateTransform()
		{
			AdornerLayer adornerLayer = (AdornerLayer)VisualTreeHelper.GetParent(this);
			base.InvalidateMeasure();
			adornerLayer.InvalidateVisual();
		}

		// Token: 0x17001CEF RID: 7407
		// (get) Token: 0x06007C86 RID: 31878 RVA: 0x00230475 File Offset: 0x0022E675
		internal IAnnotationComponent AnnotationComponent
		{
			get
			{
				return this._annotationComponent;
			}
		}

		// Token: 0x06007C87 RID: 31879 RVA: 0x00230480 File Offset: 0x0022E680
		private void OnLayoutUpdated(object sender, EventArgs args)
		{
			this._annotatedElement.LayoutUpdated -= this.OnLayoutUpdated;
			this._annotatedElement = null;
			if (this._annotationComponent.AttachedAnnotations.Count > 0)
			{
				this._annotationComponent.PresentationContext.Host.InvalidateMeasure();
				base.InvalidateMeasure();
			}
		}

		// Token: 0x04003A66 RID: 14950
		private IAnnotationComponent _annotationComponent;

		// Token: 0x04003A67 RID: 14951
		private UIElement _annotatedElement;
	}
}
