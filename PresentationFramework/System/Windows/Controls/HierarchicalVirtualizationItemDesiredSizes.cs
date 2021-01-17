using System;

namespace System.Windows.Controls
{
	/// <summary>Represents the desired size of the control's items, in device-independent units (1/96th inch per unit) and in logical units.</summary>
	// Token: 0x02000519 RID: 1305
	public struct HierarchicalVirtualizationItemDesiredSizes
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> class.</summary>
		/// <param name="logicalSize">The size of the control's child items, in logical units.</param>
		/// <param name="logicalSizeInViewport">The size of the control's child items that are in the viewport, in logical units.</param>
		/// <param name="logicalSizeBeforeViewport">The size of the control's child items that are in the cache before the viewport, in logical units.</param>
		/// <param name="logicalSizeAfterViewport">The size of the control's child items that are in the cache after the viewport, in logical units.</param>
		/// <param name="pixelSize">The size of the control's child items, in device-independent units (1/96th inch per unit).</param>
		/// <param name="pixelSizeInViewport">The size of the control's child items that are in viewport, in device-independent units (1/96th inch per unit).</param>
		/// <param name="pixelSizeBeforeViewport">The size of the control's child items that are in the cache before the viewport, in device-independent units (1/96th inch per unit).</param>
		/// <param name="pixelSizeAfterViewport">The size of the control's child items that are in the cache after the viewport, in device-independent units (1/96th inch per unit).</param>
		// Token: 0x06005425 RID: 21541 RVA: 0x00174F35 File Offset: 0x00173135
		public HierarchicalVirtualizationItemDesiredSizes(Size logicalSize, Size logicalSizeInViewport, Size logicalSizeBeforeViewport, Size logicalSizeAfterViewport, Size pixelSize, Size pixelSizeInViewport, Size pixelSizeBeforeViewport, Size pixelSizeAfterViewport)
		{
			this._logicalSize = logicalSize;
			this._logicalSizeInViewport = logicalSizeInViewport;
			this._logicalSizeBeforeViewport = logicalSizeBeforeViewport;
			this._logicalSizeAfterViewport = logicalSizeAfterViewport;
			this._pixelSize = pixelSize;
			this._pixelSizeInViewport = pixelSizeInViewport;
			this._pixelSizeBeforeViewport = pixelSizeBeforeViewport;
			this._pixelSizeAfterViewport = pixelSizeAfterViewport;
		}

		/// <summary>Gets the size of the control's child items, in logical units.</summary>
		/// <returns>The size of the control's child items, in logical units.</returns>
		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x06005426 RID: 21542 RVA: 0x00174F74 File Offset: 0x00173174
		public Size LogicalSize
		{
			get
			{
				return this._logicalSize;
			}
		}

		/// <summary>Gets the control's child items that are in the viewport, in logical units.</summary>
		/// <returns>The control's child items that are in the viewport, in logical units.</returns>
		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x06005427 RID: 21543 RVA: 0x00174F7C File Offset: 0x0017317C
		public Size LogicalSizeInViewport
		{
			get
			{
				return this._logicalSizeInViewport;
			}
		}

		/// <summary>Gets the control's child items that are in the cache before the viewport, in logical units.</summary>
		/// <returns>The control's child items that are in the cache before the viewport, in logical units.</returns>
		// Token: 0x17001470 RID: 5232
		// (get) Token: 0x06005428 RID: 21544 RVA: 0x00174F84 File Offset: 0x00173184
		public Size LogicalSizeBeforeViewport
		{
			get
			{
				return this._logicalSizeBeforeViewport;
			}
		}

		/// <summary>Gets the size of the control's child items that are in the cache after the viewport, in logical units.</summary>
		/// <returns>The control's child items that are in the cache after the viewport, in logical units.</returns>
		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x06005429 RID: 21545 RVA: 0x00174F8C File Offset: 0x0017318C
		public Size LogicalSizeAfterViewport
		{
			get
			{
				return this._logicalSizeAfterViewport;
			}
		}

		/// <summary>Gets the size of the control's child items, in device-independent units (1/96th inch per unit).</summary>
		/// <returns>The size of the control's child items, in device-independent units (1/96th inch per unit).</returns>
		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x0600542A RID: 21546 RVA: 0x00174F94 File Offset: 0x00173194
		public Size PixelSize
		{
			get
			{
				return this._pixelSize;
			}
		}

		/// <summary>Gets the size of the control's child items that are in the viewport, in device-independent units (1/96th inch per unit).</summary>
		/// <returns>The size of the control's child items that are in the viewport, in device-independent units (1/96th inch per unit).</returns>
		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x0600542B RID: 21547 RVA: 0x00174F9C File Offset: 0x0017319C
		public Size PixelSizeInViewport
		{
			get
			{
				return this._pixelSizeInViewport;
			}
		}

		/// <summary>Gets the size of the control's child items that are in the cache before the viewport, in device-independent units (1/96th inch per unit).</summary>
		/// <returns>The size of the control's child items that are in the cache before the viewport, in device-independent units (1/96th inch per unit).</returns>
		// Token: 0x17001474 RID: 5236
		// (get) Token: 0x0600542C RID: 21548 RVA: 0x00174FA4 File Offset: 0x001731A4
		public Size PixelSizeBeforeViewport
		{
			get
			{
				return this._pixelSizeBeforeViewport;
			}
		}

		/// <summary>Gets the size of the control's child items that are in the cache after the viewport, in device-independent units (1/96th inch per unit).</summary>
		/// <returns>The size of the control's child items that are in the cache after the viewport, in device-independent units (1/96th inch per unit).</returns>
		// Token: 0x17001475 RID: 5237
		// (get) Token: 0x0600542D RID: 21549 RVA: 0x00174FAC File Offset: 0x001731AC
		public Size PixelSizeAfterViewport
		{
			get
			{
				return this._pixelSizeAfterViewport;
			}
		}

		/// <summary>Returns a value that indicates whether the specified object <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> are equal.</summary>
		/// <param name="itemDesiredSizes1">The first object to compare.</param>
		/// <param name="itemDesiredSizes2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600542E RID: 21550 RVA: 0x00174FB4 File Offset: 0x001731B4
		public static bool operator ==(HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes1, HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes2)
		{
			return itemDesiredSizes1.LogicalSize == itemDesiredSizes2.LogicalSize && itemDesiredSizes1.LogicalSizeInViewport == itemDesiredSizes2.LogicalSizeInViewport && itemDesiredSizes1.LogicalSizeBeforeViewport == itemDesiredSizes2.LogicalSizeBeforeViewport && itemDesiredSizes1.LogicalSizeAfterViewport == itemDesiredSizes2.LogicalSizeAfterViewport && itemDesiredSizes1.PixelSize == itemDesiredSizes2.PixelSize && itemDesiredSizes1.PixelSizeInViewport == itemDesiredSizes2.PixelSizeInViewport && itemDesiredSizes1.PixelSizeBeforeViewport == itemDesiredSizes2.PixelSizeBeforeViewport && itemDesiredSizes1.PixelSizeAfterViewport == itemDesiredSizes2.PixelSizeAfterViewport;
		}

		/// <summary>Returns a value that indicates whether the specified object <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> are unequal.</summary>
		/// <param name="itemDesiredSizes1">The first object to compare.</param>
		/// <param name="itemDesiredSizes2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> are unequal; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600542F RID: 21551 RVA: 0x0017506C File Offset: 0x0017326C
		public static bool operator !=(HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes1, HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes2)
		{
			return itemDesiredSizes1.LogicalSize != itemDesiredSizes2.LogicalSize || itemDesiredSizes1.LogicalSizeInViewport != itemDesiredSizes2.LogicalSizeInViewport || itemDesiredSizes1.LogicalSizeBeforeViewport != itemDesiredSizes2.LogicalSizeBeforeViewport || itemDesiredSizes1.LogicalSizeAfterViewport != itemDesiredSizes2.LogicalSizeAfterViewport || itemDesiredSizes1.PixelSize != itemDesiredSizes2.PixelSize || itemDesiredSizes1.PixelSizeInViewport != itemDesiredSizes2.PixelSizeInViewport || itemDesiredSizes1.PixelSizeBeforeViewport != itemDesiredSizes2.PixelSizeBeforeViewport || itemDesiredSizes1.PixelSizeAfterViewport != itemDesiredSizes2.PixelSizeAfterViewport;
		}

		/// <summary>Returns a value that indicates whether the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> object.</summary>
		/// <param name="oCompare">The object to compare to this object.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005430 RID: 21552 RVA: 0x00175124 File Offset: 0x00173324
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationItemDesiredSizes)
			{
				HierarchicalVirtualizationItemDesiredSizes itemDesiredSizes = (HierarchicalVirtualizationItemDesiredSizes)oCompare;
				return this == itemDesiredSizes;
			}
			return false;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> object.</summary>
		/// <param name="comparisonItemSizes">The object to compare to this object.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005431 RID: 21553 RVA: 0x0017514E File Offset: 0x0017334E
		public bool Equals(HierarchicalVirtualizationItemDesiredSizes comparisonItemSizes)
		{
			return this == comparisonItemSizes;
		}

		/// <summary>Gets a hash code for this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> object.</summary>
		/// <returns>A hash code for this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationItemDesiredSizes" /> object.</returns>
		// Token: 0x06005432 RID: 21554 RVA: 0x0017515C File Offset: 0x0017335C
		public override int GetHashCode()
		{
			return this._logicalSize.GetHashCode() ^ this._logicalSizeInViewport.GetHashCode() ^ this._logicalSizeBeforeViewport.GetHashCode() ^ this._logicalSizeAfterViewport.GetHashCode() ^ this._pixelSize.GetHashCode() ^ this._pixelSizeInViewport.GetHashCode() ^ this._pixelSizeBeforeViewport.GetHashCode() ^ this._pixelSizeAfterViewport.GetHashCode();
		}

		// Token: 0x04002D84 RID: 11652
		private Size _logicalSize;

		// Token: 0x04002D85 RID: 11653
		private Size _logicalSizeInViewport;

		// Token: 0x04002D86 RID: 11654
		private Size _logicalSizeBeforeViewport;

		// Token: 0x04002D87 RID: 11655
		private Size _logicalSizeAfterViewport;

		// Token: 0x04002D88 RID: 11656
		private Size _pixelSize;

		// Token: 0x04002D89 RID: 11657
		private Size _pixelSizeInViewport;

		// Token: 0x04002D8A RID: 11658
		private Size _pixelSizeBeforeViewport;

		// Token: 0x04002D8B RID: 11659
		private Size _pixelSizeAfterViewport;
	}
}
