using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Defines custom placement parameters for a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control.</summary>
	// Token: 0x0200057A RID: 1402
	public struct CustomPopupPlacement
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure.</summary>
		/// <param name="point">The <see cref="T:System.Windows.Point" /> that is relative to the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> where the upper-left corner of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is placed.</param>
		/// <param name="primaryAxis">The <see cref="T:System.Windows.Controls.Primitives.PopupPrimaryAxis" /> along which a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control moves when it is obstructed by a screen edge.</param>
		// Token: 0x06005C81 RID: 23681 RVA: 0x001A08A4 File Offset: 0x0019EAA4
		public CustomPopupPlacement(Point point, PopupPrimaryAxis primaryAxis)
		{
			this._point = point;
			this._primaryAxis = primaryAxis;
		}

		/// <summary>Gets or sets the point that is relative to the target object where the upper-left corner of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control is placedl. </summary>
		/// <returns>A <see cref="T:System.Windows.Point" /> that is used to position a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control. The default value is (0,0).</returns>
		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x06005C82 RID: 23682 RVA: 0x001A08B4 File Offset: 0x0019EAB4
		// (set) Token: 0x06005C83 RID: 23683 RVA: 0x001A08BC File Offset: 0x0019EABC
		public Point Point
		{
			get
			{
				return this._point;
			}
			set
			{
				this._point = value;
			}
		}

		/// <summary>Gets or sets the direction in which to move a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control when the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is obscured by screen boundaries.</summary>
		/// <returns>The direction in which to move a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control when the <see cref="T:System.Windows.Controls.Primitives.Popup" /> is obscured by screen boundaries.</returns>
		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x06005C84 RID: 23684 RVA: 0x001A08C5 File Offset: 0x0019EAC5
		// (set) Token: 0x06005C85 RID: 23685 RVA: 0x001A08CD File Offset: 0x0019EACD
		public PopupPrimaryAxis PrimaryAxis
		{
			get
			{
				return this._primaryAxis;
			}
			set
			{
				this._primaryAxis = value;
			}
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structures to determine whether they are equal.</summary>
		/// <param name="placement1">The first <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <param name="placement2">The second <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the structures have the same values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005C86 RID: 23686 RVA: 0x001A08D6 File Offset: 0x0019EAD6
		public static bool operator ==(CustomPopupPlacement placement1, CustomPopupPlacement placement2)
		{
			return placement1.Equals(placement2);
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structures to determine whether they are not equal. </summary>
		/// <param name="placement1">The first <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <param name="placement2">The second <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the structures do not have the same values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005C87 RID: 23687 RVA: 0x001A08EB File Offset: 0x0019EAEB
		public static bool operator !=(CustomPopupPlacement placement1, CustomPopupPlacement placement2)
		{
			return !placement1.Equals(placement2);
		}

		/// <summary>Compares this structure with another <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to determine whether they are equal.</summary>
		/// <param name="o">The <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacement" /> structure to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the structures have the same values; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005C88 RID: 23688 RVA: 0x001A0904 File Offset: 0x0019EB04
		public override bool Equals(object o)
		{
			if (o is CustomPopupPlacement)
			{
				CustomPopupPlacement customPopupPlacement = (CustomPopupPlacement)o;
				return customPopupPlacement._primaryAxis == this._primaryAxis && customPopupPlacement._point == this._point;
			}
			return false;
		}

		/// <summary>Gets the hash code for this structure. </summary>
		/// <returns>The hash code for this structure.</returns>
		// Token: 0x06005C89 RID: 23689 RVA: 0x001A0943 File Offset: 0x0019EB43
		public override int GetHashCode()
		{
			return this._primaryAxis.GetHashCode() ^ this._point.GetHashCode();
		}

		// Token: 0x04002FD2 RID: 12242
		private Point _point;

		// Token: 0x04002FD3 RID: 12243
		private PopupPrimaryAxis _primaryAxis;
	}
}
