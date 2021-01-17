﻿using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Describes the placement of where a <see cref="T:System.Windows.Controls.Primitives.Popup" /> control appears on the screen.</summary>
	// Token: 0x0200059A RID: 1434
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public enum PlacementMode
	{
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control relative to the upper-left corner of the screen and at an offset that is defined by the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> and <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> property values. If the screen edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control then repositions itself to align with the edge.</summary>
		// Token: 0x0400305E RID: 12382
		Absolute,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control relative to the upper-left corner of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> and at an offset that is defined by the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> and <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> property values. If the screen edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself to align with the screen edge.</summary>
		// Token: 0x0400305F RID: 12383
		Relative,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control where the control  aligns its upper edge with the lower edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> and aligns its left edge with the left edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the lower screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself so that its lower edge aligns with the upper edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the upper screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control then repositions itself so that its upper edge aligns with the upper screen-edge.</summary>
		// Token: 0x04003060 RID: 12384
		Bottom,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control where it is centered over the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If a screen edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself to align with the screen edge. </summary>
		// Token: 0x04003061 RID: 12385
		Center,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control that aligns its left edge with the right edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> and aligns its upper edge with the upper edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the right screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself so that its left edge aligns with the left edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the left screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself so that its left edge aligns with the left screen-edge. If the upper or lower screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control then repositions itself to align with the obscuring screen edge.</summary>
		// Token: 0x04003062 RID: 12386
		Right,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control relative to the upper-left corner of the screen and at an offset that is defined by the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> and <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> property values. If the screen edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control extends in the opposite direction from the axis defined by the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> or <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" />=.</summary>
		// Token: 0x04003063 RID: 12387
		AbsolutePoint,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control relative to the upper-left corner of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> and at an offset that is defined by the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> and <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> property values. If a screen edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the <see cref="T:System.Windows.Controls.Primitives.Popup" /> extends in the opposite direction from the direction from the axis defined by the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> or <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" />. If the opposite screen edge also obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control then aligns with this screen edge.</summary>
		// Token: 0x04003064 RID: 12388
		RelativePoint,
		/// <summary>A postion of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control that aligns its upper edge with the lower edge of the bounding box of the mouse and aligns its left edge with the left edge of the bounding box of the mouse. If the lower screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, it repositions itself to align with the upper edge of the bounding box of the mouse. If the upper screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself to align with the upper screen-edge. </summary>
		// Token: 0x04003065 RID: 12389
		Mouse,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control relative to the tip of the mouse cursor and at an offset that is defined by the <see cref="P:System.Windows.Controls.Primitives.Popup.HorizontalOffset" /> and <see cref="P:System.Windows.Controls.Primitives.Popup.VerticalOffset" /> property values. If a horizontal or vertical screen edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, it opens in the opposite direction from the obscuring edge. If the opposite screen edge also obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, it then aligns with the obscuring screen edge.</summary>
		// Token: 0x04003066 RID: 12390
		MousePoint,
		/// <summary>A <see cref="T:System.Windows.Controls.Primitives.Popup" /> control that aligns its right edge with the left edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> and aligns its upper edge with the upper edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the left screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the <see cref="T:System.Windows.Controls.Primitives.Popup" /> repositions itself so that its left edge aligns with the right edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the right screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the right edge of the control aligns with the right screen-edge. If the upper or lower screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself to align with the obscuring screen edge.</summary>
		// Token: 0x04003067 RID: 12391
		Left,
		/// <summary>A position of the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control that aligns its lower edge with the upper edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" /> and aligns its left edge with the left edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the upper screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the control repositions itself so that its upper edge aligns with the lower edge of the <see cref="P:System.Windows.Controls.Primitives.Popup.PlacementTarget" />. If the lower screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, the lower edge of the control aligns with the lower screen-edge. If the left or right screen-edge obscures the <see cref="T:System.Windows.Controls.Primitives.Popup" />, it then repositions itself to align with the obscuring screen.</summary>
		// Token: 0x04003068 RID: 12392
		Top,
		/// <summary>A position and repositioning behavior for the <see cref="T:System.Windows.Controls.Primitives.Popup" /> control that is defined by the  <see cref="T:System.Windows.Controls.Primitives.CustomPopupPlacementCallback" /> delegate specified by the <see cref="P:System.Windows.Controls.Primitives.Popup.CustomPopupPlacementCallback" /> property.</summary>
		// Token: 0x04003069 RID: 12393
		Custom
	}
}