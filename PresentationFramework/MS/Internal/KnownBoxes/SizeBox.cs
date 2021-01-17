using System;
using System.Windows;

namespace MS.Internal.KnownBoxes
{
	// Token: 0x0200065B RID: 1627
	internal class SizeBox
	{
		// Token: 0x06006C02 RID: 27650 RVA: 0x001F171D File Offset: 0x001EF91D
		internal SizeBox(double width, double height)
		{
			if (width < 0.0 || height < 0.0)
			{
				throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
			}
			this._width = width;
			this._height = height;
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x001F175B File Offset: 0x001EF95B
		internal SizeBox(Size size) : this(size.Width, size.Height)
		{
		}

		// Token: 0x170019D1 RID: 6609
		// (get) Token: 0x06006C04 RID: 27652 RVA: 0x001F1771 File Offset: 0x001EF971
		// (set) Token: 0x06006C05 RID: 27653 RVA: 0x001F1779 File Offset: 0x001EF979
		internal double Width
		{
			get
			{
				return this._width;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
				}
				this._width = value;
			}
		}

		// Token: 0x170019D2 RID: 6610
		// (get) Token: 0x06006C06 RID: 27654 RVA: 0x001F179E File Offset: 0x001EF99E
		// (set) Token: 0x06006C07 RID: 27655 RVA: 0x001F17A6 File Offset: 0x001EF9A6
		internal double Height
		{
			get
			{
				return this._height;
			}
			set
			{
				if (value < 0.0)
				{
					throw new ArgumentException(SR.Get("Rect_WidthAndHeightCannotBeNegative"));
				}
				this._height = value;
			}
		}

		// Token: 0x04003504 RID: 13572
		private double _width;

		// Token: 0x04003505 RID: 13573
		private double _height;
	}
}
