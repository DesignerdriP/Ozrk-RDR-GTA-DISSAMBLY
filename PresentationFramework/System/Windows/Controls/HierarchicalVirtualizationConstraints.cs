using System;

namespace System.Windows.Controls
{
	/// <summary>Specifies the sizes of a control's viewport and cache. This structure is used by the <see cref="T:System.Windows.Controls.Primitives.IHierarchicalVirtualizationAndScrollInfo" /> interface.</summary>
	// Token: 0x02000517 RID: 1303
	public struct HierarchicalVirtualizationConstraints
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> class.</summary>
		/// <param name="cacheLength">The size of the cache before and after the viewport.</param>
		/// <param name="cacheLengthUnit">The type of unit that is used by the <see cref="P:System.Windows.Controls.HierarchicalVirtualizationConstraints.CacheLength" /> property.</param>
		/// <param name="viewport">The size of the cache before and after the viewport.</param>
		// Token: 0x06005412 RID: 21522 RVA: 0x00174D2F File Offset: 0x00172F2F
		public HierarchicalVirtualizationConstraints(VirtualizationCacheLength cacheLength, VirtualizationCacheLengthUnit cacheLengthUnit, Rect viewport)
		{
			this._cacheLength = cacheLength;
			this._cacheLengthUnit = cacheLengthUnit;
			this._viewport = viewport;
			this._scrollGeneration = 0L;
		}

		/// <summary>Gets the size of the cache before and after the viewport.</summary>
		/// <returns>The size of the cache before and after the viewport.</returns>
		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x06005413 RID: 21523 RVA: 0x00174D4E File Offset: 0x00172F4E
		public VirtualizationCacheLength CacheLength
		{
			get
			{
				return this._cacheLength;
			}
		}

		/// <summary>Gets the type of unit that is used by the <see cref="P:System.Windows.Controls.HierarchicalVirtualizationConstraints.CacheLength" /> property.</summary>
		/// <returns>The type of unit that is used by the <see cref="P:System.Windows.Controls.HierarchicalVirtualizationConstraints.CacheLength" /> property.</returns>
		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x06005414 RID: 21524 RVA: 0x00174D56 File Offset: 0x00172F56
		public VirtualizationCacheLengthUnit CacheLengthUnit
		{
			get
			{
				return this._cacheLengthUnit;
			}
		}

		/// <summary>Gets the area that displays the items of the control. </summary>
		/// <returns>The area that displays the items of the control.</returns>
		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x06005415 RID: 21525 RVA: 0x00174D5E File Offset: 0x00172F5E
		public Rect Viewport
		{
			get
			{
				return this._viewport;
			}
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are equal.</summary>
		/// <param name="constraints1">The first <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <param name="constraints2">The second <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005416 RID: 21526 RVA: 0x00174D66 File Offset: 0x00172F66
		public static bool operator ==(HierarchicalVirtualizationConstraints constraints1, HierarchicalVirtualizationConstraints constraints2)
		{
			return constraints1.CacheLength == constraints2.CacheLength && constraints1.CacheLengthUnit == constraints2.CacheLengthUnit && constraints2.Viewport == constraints2.Viewport;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are unequal.</summary>
		/// <param name="constraints1">The first <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <param name="constraints2">The second <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> objects are unequal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005417 RID: 21527 RVA: 0x00174DA2 File Offset: 0x00172FA2
		public static bool operator !=(HierarchicalVirtualizationConstraints constraints1, HierarchicalVirtualizationConstraints constraints2)
		{
			return constraints1.CacheLength != constraints2.CacheLength || constraints1.CacheLengthUnit != constraints2.CacheLengthUnit || constraints1.Viewport != constraints2.Viewport;
		}

		/// <summary>Returns a value that indicates whether the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</summary>
		/// <param name="oCompare">The object to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005418 RID: 21528 RVA: 0x00174DE0 File Offset: 0x00172FE0
		public override bool Equals(object oCompare)
		{
			if (oCompare is HierarchicalVirtualizationConstraints)
			{
				HierarchicalVirtualizationConstraints constraints = (HierarchicalVirtualizationConstraints)oCompare;
				return this == constraints;
			}
			return false;
		}

		/// <summary>Returns a value that indicates whether the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</summary>
		/// <param name="comparisonConstraints">The <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" /> is equal to this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005419 RID: 21529 RVA: 0x00174E0A File Offset: 0x0017300A
		public bool Equals(HierarchicalVirtualizationConstraints comparisonConstraints)
		{
			return this == comparisonConstraints;
		}

		/// <summary>Gets a hash code for this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</summary>
		/// <returns>A hash code for this <see cref="T:System.Windows.Controls.HierarchicalVirtualizationConstraints" />.</returns>
		// Token: 0x0600541A RID: 21530 RVA: 0x00174E18 File Offset: 0x00173018
		public override int GetHashCode()
		{
			return this._cacheLength.GetHashCode() ^ this._cacheLengthUnit.GetHashCode() ^ this._viewport.GetHashCode();
		}

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x0600541B RID: 21531 RVA: 0x00174E4F File Offset: 0x0017304F
		// (set) Token: 0x0600541C RID: 21532 RVA: 0x00174E57 File Offset: 0x00173057
		internal long ScrollGeneration
		{
			get
			{
				return this._scrollGeneration;
			}
			set
			{
				this._scrollGeneration = value;
			}
		}

		// Token: 0x04002D7E RID: 11646
		private VirtualizationCacheLength _cacheLength;

		// Token: 0x04002D7F RID: 11647
		private VirtualizationCacheLengthUnit _cacheLengthUnit;

		// Token: 0x04002D80 RID: 11648
		private Rect _viewport;

		// Token: 0x04002D81 RID: 11649
		private long _scrollGeneration;
	}
}
