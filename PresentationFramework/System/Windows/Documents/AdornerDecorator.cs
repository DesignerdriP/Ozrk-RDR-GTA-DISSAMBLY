using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace System.Windows.Documents
{
	/// <summary>Provides an <see cref="T:System.Windows.Documents.AdornerLayer" /> for the child elements in the visual tree. </summary>
	/// <exception cref="T:System.ArgumentException">An attempt is made to add more than a single child to the <see cref="T:System.Windows.Documents.AdornerDecorator" />.</exception>
	// Token: 0x0200032A RID: 810
	public class AdornerDecorator : Decorator
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Documents.AdornerDecorator" /> class.</summary>
		// Token: 0x06002AA0 RID: 10912 RVA: 0x000C2BF5 File Offset: 0x000C0DF5
		public AdornerDecorator()
		{
			this._adornerLayer = new AdornerLayer();
		}

		/// <summary>Gets the <see cref="T:System.Windows.Documents.AdornerLayer" /> associated with this <see cref="T:System.Windows.Documents.AdornerDecorator" />.</summary>
		/// <returns>The <see cref="T:System.Windows.Documents.AdornerLayer" /> associated with this adorner decorator. </returns>
		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000C2C08 File Offset: 0x000C0E08
		public AdornerLayer AdornerLayer
		{
			get
			{
				return this._adornerLayer;
			}
		}

		/// <summary>Measures the size required for child elements and determines a size for the <see cref="T:System.Windows.Documents.AdornerDecorator" />.</summary>
		/// <param name="constraint">A size to constrain the <see cref="T:System.Windows.Documents.AdornerDecorator" /> to.</param>
		/// <returns>A <see cref="T:System.Windows.Size" /> object representing the amount of layout space needed by the <see cref="T:System.Windows.Documents.AdornerDecorator" />.</returns>
		// Token: 0x06002AA2 RID: 10914 RVA: 0x000C2C10 File Offset: 0x000C0E10
		protected override Size MeasureOverride(Size constraint)
		{
			Size result = base.MeasureOverride(constraint);
			if (VisualTreeHelper.GetParent(this._adornerLayer) != null)
			{
				this._adornerLayer.Measure(constraint);
			}
			return result;
		}

		/// <summary>Positions child elements and determines a size for the <see cref="T:System.Windows.Documents.AdornerDecorator" />.</summary>
		/// <param name="finalSize">The size reserved for this element by its parent.</param>
		/// <returns>The actual size needed by the element.  This return value is typically the same as the value passed to finalSize.</returns>
		// Token: 0x06002AA3 RID: 10915 RVA: 0x000C2C40 File Offset: 0x000C0E40
		protected override Size ArrangeOverride(Size finalSize)
		{
			Size result = base.ArrangeOverride(finalSize);
			if (VisualTreeHelper.GetParent(this._adornerLayer) != null)
			{
				this._adornerLayer.Arrange(new Rect(finalSize));
			}
			return result;
		}

		/// <summary>Gets or sets the single child of an <see cref="T:System.Windows.Documents.AdornerDecorator" />.</summary>
		/// <returns>The single child of an <see cref="T:System.Windows.Documents.AdornerDecorator" />. This property has no default value.</returns>
		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x000C2C74 File Offset: 0x000C0E74
		// (set) Token: 0x06002AA5 RID: 10917 RVA: 0x000C2C7C File Offset: 0x000C0E7C
		public override UIElement Child
		{
			get
			{
				return base.Child;
			}
			set
			{
				Visual child = base.Child;
				if (child == value)
				{
					return;
				}
				if (value == null)
				{
					base.Child = null;
					base.RemoveVisualChild(this._adornerLayer);
					return;
				}
				base.Child = value;
				if (child == null)
				{
					base.AddVisualChild(this._adornerLayer);
				}
			}
		}

		/// <summary>Gets the number of child <see cref="T:System.Windows.Media.Visual" /> objects in this instance of <see cref="T:System.Windows.Documents.AdornerDecorator" />.</summary>
		/// <returns>Either returns 2 (one for the <see cref="T:System.Windows.Documents.AdornerLayer" /> and one for the <see cref="P:System.Windows.Documents.AdornerDecorator.Child" />) or the property returns 0 if the <see cref="T:System.Windows.Documents.AdornerDecorator" /> has no child.</returns>
		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06002AA6 RID: 10918 RVA: 0x000C2CC2 File Offset: 0x000C0EC2
		protected override int VisualChildrenCount
		{
			get
			{
				if (base.Child != null)
				{
					return 2;
				}
				return 0;
			}
		}

		/// <summary>Gets a <see cref="T:System.Windows.Media.Visual" /> child at the specified <paramref name="index" /> position.</summary>
		/// <param name="index">The index position of the wanted <see cref="T:System.Windows.Media.Visual" /> child.</param>
		/// <returns>A <see cref="T:System.Windows.Media.Visual" /> child of the parent <see cref="T:System.Windows.Controls.Viewbox" /> element.</returns>
		// Token: 0x06002AA7 RID: 10919 RVA: 0x000C2CD0 File Offset: 0x000C0ED0
		protected override Visual GetVisualChild(int index)
		{
			if (base.Child == null)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			if (index == 0)
			{
				return base.Child;
			}
			if (index != 1)
			{
				throw new ArgumentOutOfRangeException("index", index, SR.Get("Visual_ArgumentOutOfRange"));
			}
			return this._adornerLayer;
		}

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x00094EB6 File Offset: 0x000930B6
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x04001C4A RID: 7242
		private readonly AdornerLayer _adornerLayer;
	}
}
