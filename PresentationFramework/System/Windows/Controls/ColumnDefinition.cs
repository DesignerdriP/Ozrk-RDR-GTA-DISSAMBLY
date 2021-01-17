using System;
using System.ComponentModel;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines column-specific properties that apply to <see cref="T:System.Windows.Controls.Grid" /> elements. </summary>
	// Token: 0x02000571 RID: 1393
	public class ColumnDefinition : DefinitionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ColumnDefinition" /> class.</summary>
		// Token: 0x06005BB4 RID: 23476 RVA: 0x0019CC77 File Offset: 0x0019AE77
		public ColumnDefinition() : base(true)
		{
		}

		/// <summary>Gets the calculated width of a <see cref="T:System.Windows.Controls.ColumnDefinition" /> element, or sets the <see cref="T:System.Windows.GridLength" /> value of a column that is defined by the <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>The <see cref="T:System.Windows.GridLength" /> that represents the width of the Column. The default value is 1.0.</returns>
		// Token: 0x17001636 RID: 5686
		// (get) Token: 0x06005BB5 RID: 23477 RVA: 0x0019CC80 File Offset: 0x0019AE80
		// (set) Token: 0x06005BB6 RID: 23478 RVA: 0x0019CC88 File Offset: 0x0019AE88
		public GridLength Width
		{
			get
			{
				return base.UserSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.WidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the minimum width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the minimum width. The default value is 0.</returns>
		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x06005BB7 RID: 23479 RVA: 0x0014FC08 File Offset: 0x0014DE08
		// (set) Token: 0x06005BB8 RID: 23480 RVA: 0x0019CC9B File Offset: 0x0019AE9B
		[TypeConverter(typeof(LengthConverter))]
		public double MinWidth
		{
			get
			{
				return base.UserMinSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MinWidthProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the maximum width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />.   </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the maximum width. The default value is <see cref="F:System.Double.PositiveInfinity" />.</returns>
		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x06005BB9 RID: 23481 RVA: 0x0014FC10 File Offset: 0x0014DE10
		// (set) Token: 0x06005BBA RID: 23482 RVA: 0x0019CCAE File Offset: 0x0019AEAE
		[TypeConverter(typeof(LengthConverter))]
		public double MaxWidth
		{
			get
			{
				return base.UserMaxSizeValueCache;
			}
			set
			{
				base.SetValue(ColumnDefinition.MaxWidthProperty, value);
			}
		}

		/// <summary>Gets a value that represents the actual calculated width of a <see cref="T:System.Windows.Controls.ColumnDefinition" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the actual calculated width in device independent pixels. The default value is 0.0.</returns>
		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x06005BBB RID: 23483 RVA: 0x0019CCC4 File Offset: 0x0019AEC4
		public double ActualWidth
		{
			get
			{
				double result = 0.0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalColumnDefinitionWidth(base.Index);
				}
				return result;
			}
		}

		/// <summary>Gets a value that represents the offset value of this <see cref="T:System.Windows.Controls.ColumnDefinition" />. </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the offset of the column. The default value is 0.0.</returns>
		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x06005BBC RID: 23484 RVA: 0x0019CCFC File Offset: 0x0019AEFC
		public double Offset
		{
			get
			{
				double result = 0.0;
				if (base.Index != 0)
				{
					result = base.FinalOffset;
				}
				return result;
			}
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.Width" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.Width" /> dependency property.</returns>
		// Token: 0x04002F90 RID: 12176
		[CommonDependencyProperty]
		public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width", typeof(GridLength), typeof(ColumnDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.MinWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.MinWidth" /> dependency property.</returns>
		// Token: 0x04002F91 RID: 12177
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register("MinWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.ColumnDefinition.MaxWidth" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.ColumnDefinition.MaxWidth" /> dependency property.</returns>
		// Token: 0x04002F92 RID: 12178
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MaxWidthProperty = DependencyProperty.Register("MaxWidth", typeof(double), typeof(ColumnDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
	}
}
