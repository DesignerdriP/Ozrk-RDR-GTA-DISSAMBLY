using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace MS.Internal.Controls
{
	// Token: 0x02000763 RID: 1891
	internal sealed class TemplatedAdorner : Adorner
	{
		// Token: 0x06007842 RID: 30786 RVA: 0x00224126 File Offset: 0x00222326
		public void ClearChild()
		{
			base.RemoveVisualChild(this._child);
			this._child = null;
		}

		// Token: 0x06007843 RID: 30787 RVA: 0x0022413C File Offset: 0x0022233C
		public TemplatedAdorner(UIElement adornedElement, ControlTemplate adornerTemplate) : base(adornedElement)
		{
			this._child = new Control
			{
				DataContext = Validation.GetErrors(adornedElement),
				IsTabStop = false,
				Template = adornerTemplate
			};
			base.AddVisualChild(this._child);
		}

		// Token: 0x06007844 RID: 30788 RVA: 0x00224184 File Offset: 0x00222384
		public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
		{
			if (this.ReferenceElement == null)
			{
				return transform;
			}
			GeneralTransformGroup generalTransformGroup = new GeneralTransformGroup();
			generalTransformGroup.Children.Add(transform);
			GeneralTransform generalTransform = base.TransformToDescendant(this.ReferenceElement);
			if (generalTransform != null)
			{
				generalTransformGroup.Children.Add(generalTransform);
			}
			return generalTransformGroup;
		}

		// Token: 0x17001C83 RID: 7299
		// (get) Token: 0x06007845 RID: 30789 RVA: 0x002241CA File Offset: 0x002223CA
		// (set) Token: 0x06007846 RID: 30790 RVA: 0x002241D2 File Offset: 0x002223D2
		public FrameworkElement ReferenceElement
		{
			get
			{
				return this._referenceElement;
			}
			set
			{
				this._referenceElement = value;
			}
		}

		// Token: 0x06007847 RID: 30791 RVA: 0x002241DB File Offset: 0x002223DB
		protected override Visual GetVisualChild(int index)
		{
			if (this._child == null || index != 0)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._child;
		}

		// Token: 0x17001C84 RID: 7300
		// (get) Token: 0x06007848 RID: 30792 RVA: 0x00224209 File Offset: 0x00222409
		protected override int VisualChildrenCount
		{
			get
			{
				if (this._child == null)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06007849 RID: 30793 RVA: 0x00224218 File Offset: 0x00222418
		protected override Size MeasureOverride(Size constraint)
		{
			if (this.ReferenceElement != null && base.AdornedElement != null && base.AdornedElement.IsMeasureValid && !DoubleUtil.AreClose(this.ReferenceElement.DesiredSize, base.AdornedElement.DesiredSize))
			{
				this.ReferenceElement.InvalidateMeasure();
			}
			this._child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			return this._child.DesiredSize;
		}

		// Token: 0x0600784A RID: 30794 RVA: 0x00224298 File Offset: 0x00222498
		protected override Size ArrangeOverride(Size size)
		{
			Size size2 = base.ArrangeOverride(size);
			if (this._child != null)
			{
				this._child.Arrange(new Rect(default(Point), size2));
			}
			return size2;
		}

		// Token: 0x0600784B RID: 30795 RVA: 0x002242D0 File Offset: 0x002224D0
		internal override bool NeedsUpdate(Size oldSize)
		{
			bool result = base.NeedsUpdate(oldSize);
			Visibility visibility = base.AdornedElement.IsVisible ? Visibility.Visible : Visibility.Collapsed;
			if (visibility != base.Visibility)
			{
				base.Visibility = visibility;
				result = true;
			}
			return result;
		}

		// Token: 0x040038EE RID: 14574
		private Control _child;

		// Token: 0x040038EF RID: 14575
		private FrameworkElement _referenceElement;
	}
}
