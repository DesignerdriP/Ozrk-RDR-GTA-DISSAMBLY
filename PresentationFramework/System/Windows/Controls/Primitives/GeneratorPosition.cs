﻿using System;
using System.Windows.Markup;

namespace System.Windows.Controls.Primitives
{
	/// <summary>
	///     <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /> is used to describe the position of an item that is managed by <see cref="T:System.Windows.Controls.ItemContainerGenerator" />.</summary>
	// Token: 0x0200058F RID: 1423
	public struct GeneratorPosition
	{
		/// <summary>Gets or sets the <see cref="T:System.Int32" /> index that is relative to the generated (realized) items.</summary>
		/// <returns>An <see cref="T:System.Int32" /> index that is relative to the generated (realized) items.</returns>
		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x06005E29 RID: 24105 RVA: 0x001A72F9 File Offset: 0x001A54F9
		// (set) Token: 0x06005E2A RID: 24106 RVA: 0x001A7301 File Offset: 0x001A5501
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Int32" /> offset that is relative to the ungenerated (unrealized) items near the indexed item.</summary>
		/// <returns>An <see cref="T:System.Int32" /> offset that is relative to the ungenerated (unrealized) items near the indexed item.</returns>
		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x06005E2B RID: 24107 RVA: 0x001A730A File Offset: 0x001A550A
		// (set) Token: 0x06005E2C RID: 24108 RVA: 0x001A7312 File Offset: 0x001A5512
		public int Offset
		{
			get
			{
				return this._offset;
			}
			set
			{
				this._offset = value;
			}
		}

		/// <summary>Initializes a new instance of <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /> with the specified index and offset.</summary>
		/// <param name="index">An <see cref="T:System.Int32" /> index that is relative to the generated (realized) items. -1 is a special value that refers to a fictitious item at the beginning or the end of the items list.</param>
		/// <param name="offset">An <see cref="T:System.Int32" /> offset that is relative to the ungenerated (unrealized) items near the indexed item. An offset of 0 refers to the indexed element itself, an offset 1 refers to the next ungenerated (unrealized) item, and an offset of -1 refers to the previous item.</param>
		// Token: 0x06005E2D RID: 24109 RVA: 0x001A731B File Offset: 0x001A551B
		public GeneratorPosition(int index, int offset)
		{
			this._index = index;
			this._offset = offset;
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" />.</summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" />.</returns>
		// Token: 0x06005E2E RID: 24110 RVA: 0x001A732B File Offset: 0x001A552B
		public override int GetHashCode()
		{
			return this._index.GetHashCode() + this._offset.GetHashCode();
		}

		/// <summary>Returns a string representation of this instance of <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" />.</summary>
		/// <returns>A string representation of this instance of <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /></returns>
		// Token: 0x06005E2F RID: 24111 RVA: 0x001A7344 File Offset: 0x001A5544
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"GeneratorPosition (",
				this._index.ToString(TypeConverterHelper.InvariantEnglishUS),
				",",
				this._offset.ToString(TypeConverterHelper.InvariantEnglishUS),
				")"
			});
		}

		/// <summary>Compares the specified instance and the current instance of <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /> for value equality.</summary>
		/// <param name="o">The <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /> instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="o" /> and this instance of <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /> have the same values.</returns>
		// Token: 0x06005E30 RID: 24112 RVA: 0x001A739C File Offset: 0x001A559C
		public override bool Equals(object o)
		{
			if (o is GeneratorPosition)
			{
				GeneratorPosition generatorPosition = (GeneratorPosition)o;
				return this._index == generatorPosition._index && this._offset == generatorPosition._offset;
			}
			return false;
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /> objects for value equality.</summary>
		/// <param name="gp1">The first instance to compare.</param>
		/// <param name="gp2">The second instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two objects are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005E31 RID: 24113 RVA: 0x001A73D8 File Offset: 0x001A55D8
		public static bool operator ==(GeneratorPosition gp1, GeneratorPosition gp2)
		{
			return gp1._index == gp2._index && gp1._offset == gp2._offset;
		}

		/// <summary>Compares two <see cref="T:System.Windows.Controls.Primitives.GeneratorPosition" /> objects for value inequality.</summary>
		/// <param name="gp1">The first instance to compare.</param>
		/// <param name="gp2">The second instance to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the values are not equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06005E32 RID: 24114 RVA: 0x001A73F8 File Offset: 0x001A55F8
		public static bool operator !=(GeneratorPosition gp1, GeneratorPosition gp2)
		{
			return !(gp1 == gp2);
		}

		// Token: 0x04003046 RID: 12358
		private int _index;

		// Token: 0x04003047 RID: 12359
		private int _offset;
	}
}
