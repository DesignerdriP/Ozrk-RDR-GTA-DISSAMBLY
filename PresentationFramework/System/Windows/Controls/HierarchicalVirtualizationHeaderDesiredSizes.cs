using System;

namespace System.Windows.Controls
{
	/// <summary>Represents the desired size of the control's header, in pixels and in logical units. This structure is used by the <see cref="T:System.Windows.Controls.Primitives.IHierarchicalVirtualizationAndScrollInfo" /> interface.</summary>
	// Token: 0x02000518 RID: 1304
	public struct HierarchicalVirtualizationHeaderDesiredSizes
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> class.</summary>
		/// <param name="logicalSize">The size of the header, in logical units.</param>
		/// <param name="pixelSize">The size of the header, in device-independent units (1/96th inch per unit).</param>
		// Token: 0x0600541D RID: 21533 RVA: 0x00174E60 File Offset: 0x00173060
		public HierarchicalVirtualizationHeaderDesiredSizes(Size logicalSize, Size pixelSize)
		{
			this._logicalSize = logicalSize;
			this._pixelSize = pixelSize;
		}

		/// <summary>Gets the size of the header, in logical units.</summary>
		/// <returns>The size of the header, in logical units.</returns>
		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x0600541E RID: 21534 RVA: 0x00174E70 File Offset: 0x00173070
		public Size LogicalSize
		{
			get
			{
				return this._logicalSize;
			}
		}

		/// <summary>Gets the size of the header, in device-independent units (1/96th inch per unit).</summary>
		/// <returns>The size of the header, in device-independent units (1/96th inch per unit).</returns>
		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x0600541F RID: 21535 RVA: 0x00174E78 File Offset: 0x00173078
		public Size PixelSize
		{
			get
			{
				return this._pixelSize;
			}
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> objects are equal.</summary>
		/// <param name="headerDesiredSizes1">The first object to compare.</param>
		/// <param name="headerDesiredSizes2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005420 RID: 21536 RVA: 0x00174E80 File Offset: 0x00173080
		public static bool operator ==(HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes1, HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes2)
		{
			return headerDesiredSizes1.LogicalSize == headerDesiredSizes2.LogicalSize && headerDesiredSizes1.PixelSize == headerDesiredSizes2.PixelSize;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> objects are unequal.</summary>
		/// <param name="headerDesiredSizes1">The first object to compare.</param>
		/// <param name="headerDesiredSizes2">The second object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified objects are unequal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005421 RID: 21537 RVA: 0x00174EAC File Offset: 0x001730AC
		public static bool operator !=(HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes1, HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes2)
		{
			return headerDesiredSizes1.LogicalSize != headerDesiredSizes2.LogicalSize || headerDesiredSizes1.PixelSize != headerDesiredSizes2.PixelSize;
		}

		/// <summary>Returns a value that indicates whether the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> object.</summary>
		/// <param name="oCompare">The object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005422 RID: 21538 RVA: 0x00174ED8 File Offset: 0x001730D8
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationHeaderDesiredSizes)
			{
				HierarchicalVirtualizationHeaderDesiredSizes headerDesiredSizes = (HierarchicalVirtualizationHeaderDesiredSizes)oCompare;
				return this == headerDesiredSizes;
			}
			return false;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" /> object.</summary>
		/// <param name="comparisonHeaderSizes">The object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005423 RID: 21539 RVA: 0x00174F02 File Offset: 0x00173102
		public bool Equals(HierarchicalVirtualizationHeaderDesiredSizes comparisonHeaderSizes)
		{
			return this == comparisonHeaderSizes;
		}

		/// <summary>Gets a hash code for the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" />.</summary>
		/// <returns>A hash code for the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationHeaderDesiredSizes" />.</returns>
		// Token: 0x06005424 RID: 21540 RVA: 0x00174F10 File Offset: 0x00173110
		public override int GetHashCode()
		{
			return this._logicalSize.GetHashCode() ^ this._pixelSize.GetHashCode();
		}

		// Token: 0x04002D82 RID: 11650
		private Size _logicalSize;

		// Token: 0x04002D83 RID: 11651
		private Size _pixelSize;
	}
}
