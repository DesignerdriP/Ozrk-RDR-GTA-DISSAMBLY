using System;
using System.Windows;
using System.Windows.Ink;

namespace MS.Internal.Ink
{
	// Token: 0x02000692 RID: 1682
	internal class StrokeCollectionDefaultValueFactory : DefaultValueFactory
	{
		// Token: 0x06006DED RID: 28141 RVA: 0x001F57F2 File Offset: 0x001F39F2
		internal StrokeCollectionDefaultValueFactory()
		{
		}

		// Token: 0x17001A23 RID: 6691
		// (get) Token: 0x06006DEE RID: 28142 RVA: 0x001FA285 File Offset: 0x001F8485
		internal override object DefaultValue
		{
			get
			{
				return new StrokeCollection();
			}
		}

		// Token: 0x06006DEF RID: 28143 RVA: 0x001FA28C File Offset: 0x001F848C
		internal override object CreateDefaultValue(DependencyObject owner, DependencyProperty property)
		{
			StrokeCollection strokeCollection = new StrokeCollection();
			StrokeCollectionDefaultValueFactory.StrokeCollectionDefaultPromoter @object = new StrokeCollectionDefaultValueFactory.StrokeCollectionDefaultPromoter(owner, property);
			strokeCollection.StrokesChanged += @object.OnStrokeCollectionChanged<StrokeCollectionChangedEventArgs>;
			strokeCollection.PropertyDataChanged += @object.OnStrokeCollectionChanged<PropertyDataChangedEventArgs>;
			return strokeCollection;
		}

		// Token: 0x02000B25 RID: 2853
		private class StrokeCollectionDefaultPromoter
		{
			// Token: 0x06008D3C RID: 36156 RVA: 0x00258EBF File Offset: 0x002570BF
			internal StrokeCollectionDefaultPromoter(DependencyObject owner, DependencyProperty property)
			{
				this._owner = owner;
				this._dependencyProperty = property;
			}

			// Token: 0x06008D3D RID: 36157 RVA: 0x00258ED8 File Offset: 0x002570D8
			internal void OnStrokeCollectionChanged<TEventArgs>(object sender, TEventArgs e)
			{
				StrokeCollection strokeCollection = (StrokeCollection)sender;
				strokeCollection.StrokesChanged -= this.OnStrokeCollectionChanged<StrokeCollectionChangedEventArgs>;
				strokeCollection.PropertyDataChanged -= this.OnStrokeCollectionChanged<PropertyDataChangedEventArgs>;
				if (this._owner.ReadLocalValue(this._dependencyProperty) == DependencyProperty.UnsetValue)
				{
					this._owner.SetValue(this._dependencyProperty, strokeCollection);
				}
				PropertyMetadata metadata = this._dependencyProperty.GetMetadata(this._owner.DependencyObjectType);
				metadata.ClearCachedDefaultValue(this._owner, this._dependencyProperty);
			}

			// Token: 0x04004A6C RID: 19052
			private readonly DependencyObject _owner;

			// Token: 0x04004A6D RID: 19053
			private readonly DependencyProperty _dependencyProperty;
		}
	}
}
