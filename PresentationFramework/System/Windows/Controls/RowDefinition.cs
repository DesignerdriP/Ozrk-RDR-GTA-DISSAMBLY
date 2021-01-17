using System;
using System.ComponentModel;
using MS.Internal.PresentationFramework;

namespace System.Windows.Controls
{
	/// <summary>Defines row-specific properties that apply to <see cref="T:System.Windows.Controls.Grid" /> elements.</summary>
	// Token: 0x02000573 RID: 1395
	public class RowDefinition : DefinitionBase
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.RowDefinition" /> class.</summary>
		// Token: 0x06005BE5 RID: 23525 RVA: 0x0019D62F File Offset: 0x0019B82F
		public RowDefinition() : base(false)
		{
		}

		/// <summary>Gets the calculated height of a <see cref="T:System.Windows.Controls.RowDefinition" /> element, or sets the <see cref="T:System.Windows.GridLength" /> value of a row that is defined by the <see cref="T:System.Windows.Controls.RowDefinition" />.   </summary>
		/// <returns>The <see cref="T:System.Windows.GridLength" /> that represents the height of the row. The default value is 1.0.</returns>
		// Token: 0x17001644 RID: 5700
		// (get) Token: 0x06005BE6 RID: 23526 RVA: 0x0019CC80 File Offset: 0x0019AE80
		// (set) Token: 0x06005BE7 RID: 23527 RVA: 0x0019D638 File Offset: 0x0019B838
		public GridLength Height
		{
			get
			{
				return base.UserSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.HeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the minimum allowable height of a <see cref="T:System.Windows.Controls.RowDefinition" />.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the minimum allowable height. The default value is 0.</returns>
		// Token: 0x17001645 RID: 5701
		// (get) Token: 0x06005BE8 RID: 23528 RVA: 0x0014FC08 File Offset: 0x0014DE08
		// (set) Token: 0x06005BE9 RID: 23529 RVA: 0x0019D64B File Offset: 0x0019B84B
		[TypeConverter(typeof(LengthConverter))]
		public double MinHeight
		{
			get
			{
				return base.UserMinSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.MinHeightProperty, value);
			}
		}

		/// <summary>Gets or sets a value that represents the maximum height of a <see cref="T:System.Windows.Controls.RowDefinition" />.  </summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the maximum height. </returns>
		// Token: 0x17001646 RID: 5702
		// (get) Token: 0x06005BEA RID: 23530 RVA: 0x0014FC10 File Offset: 0x0014DE10
		// (set) Token: 0x06005BEB RID: 23531 RVA: 0x0019D65E File Offset: 0x0019B85E
		[TypeConverter(typeof(LengthConverter))]
		public double MaxHeight
		{
			get
			{
				return base.UserMaxSizeValueCache;
			}
			set
			{
				base.SetValue(RowDefinition.MaxHeightProperty, value);
			}
		}

		/// <summary>Gets a value that represents the calculated height of the <see cref="T:System.Windows.Controls.RowDefinition" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the calculated height in device independent pixels. The default value is 0.0.</returns>
		// Token: 0x17001647 RID: 5703
		// (get) Token: 0x06005BEC RID: 23532 RVA: 0x0019D674 File Offset: 0x0019B874
		public double ActualHeight
		{
			get
			{
				double result = 0.0;
				if (base.InParentLogicalTree)
				{
					result = ((Grid)base.Parent).GetFinalRowDefinitionHeight(base.Index);
				}
				return result;
			}
		}

		/// <summary>Gets a value that represents the offset value of this <see cref="T:System.Windows.Controls.RowDefinition" />.</summary>
		/// <returns>A <see cref="T:System.Double" /> that represents the offset of the row. The default value is 0.0.</returns>
		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x06005BED RID: 23533 RVA: 0x0019D6AC File Offset: 0x0019B8AC
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

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.Height" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.Height" /> dependency property.</returns>
		// Token: 0x04002F98 RID: 12184
		[CommonDependencyProperty]
		public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height", typeof(GridLength), typeof(RowDefinition), new FrameworkPropertyMetadata(new GridLength(1.0, GridUnitType.Star), new PropertyChangedCallback(DefinitionBase.OnUserSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.MinHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.MinHeight" /> dependency property.</returns>
		// Token: 0x04002F99 RID: 12185
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MinHeightProperty = DependencyProperty.Register("MinHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(0.0, new PropertyChangedCallback(DefinitionBase.OnUserMinSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMinSizePropertyValueValid));

		/// <summary>Identifies the <see cref="P:System.Windows.Controls.RowDefinition.MaxHeight" /> dependency property. </summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Controls.RowDefinition.MaxHeight" /> dependency property.</returns>
		// Token: 0x04002F9A RID: 12186
		[CommonDependencyProperty]
		[TypeConverter("System.Windows.LengthConverter, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, Custom=null")]
		public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register("MaxHeight", typeof(double), typeof(RowDefinition), new FrameworkPropertyMetadata(double.PositiveInfinity, new PropertyChangedCallback(DefinitionBase.OnUserMaxSizePropertyChanged)), new ValidateValueCallback(DefinitionBase.IsUserMaxSizePropertyValueValid));
	}
}
