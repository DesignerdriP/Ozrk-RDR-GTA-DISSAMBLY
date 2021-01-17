using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x02000683 RID: 1667
	internal class DrawingAttributesDefaultValueFactory : DefaultValueFactory
	{
		// Token: 0x06006D15 RID: 27925 RVA: 0x001F57F2 File Offset: 0x001F39F2
		internal DrawingAttributesDefaultValueFactory()
		{
		}

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x06006D16 RID: 27926 RVA: 0x001F57FA File Offset: 0x001F39FA
		internal override object DefaultValue
		{
			get
			{
				return new DrawingAttributes();
			}
		}

		// Token: 0x06006D17 RID: 27927 RVA: 0x001F5804 File Offset: 0x001F3A04
		internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
		{
			DrawingAttributes drawingAttributes = new DrawingAttributes();
			DrawingAttributesDefaultValueFactory.DrawingAttributesDefaultPromoter @object = new DrawingAttributesDefaultValueFactory.DrawingAttributesDefaultPromoter((InkCanvas)owner);
			drawingAttributes.AttributeChanged += @object.OnDrawingAttributesChanged;
			drawingAttributes.PropertyDataChanged += @object.OnDrawingAttributesChanged;
			return drawingAttributes;
		}

		// Token: 0x02000B20 RID: 2848
		private class DrawingAttributesDefaultPromoter
		{
			// Token: 0x06008D2D RID: 36141 RVA: 0x00258C47 File Offset: 0x00256E47
			internal DrawingAttributesDefaultPromoter(InkCanvas owner)
			{
				this._owner = owner;
			}

			// Token: 0x06008D2E RID: 36142 RVA: 0x00258C58 File Offset: 0x00256E58
			internal void OnDrawingAttributesChanged(object sender, PropertyDataChangedEventArgs e)
			{
				DrawingAttributes drawingAttributes = (DrawingAttributes)sender;
				drawingAttributes.AttributeChanged -= this.OnDrawingAttributesChanged;
				drawingAttributes.PropertyDataChanged -= this.OnDrawingAttributesChanged;
				if (this._owner.ReadLocalValue(InkCanvas.DefaultDrawingAttributesProperty) == DependencyProperty.UnsetValue)
				{
					this._owner.SetValue(InkCanvas.DefaultDrawingAttributesProperty, drawingAttributes);
				}
				PropertyMetadata metadata = InkCanvas.DefaultDrawingAttributesProperty.GetMetadata(this._owner.DependencyObjectType);
				metadata.ClearCachedDefaultValue(this._owner, InkCanvas.DefaultDrawingAttributesProperty);
			}

			// Token: 0x04004A55 RID: 19029
			private readonly InkCanvas _owner;
		}
	}
}
