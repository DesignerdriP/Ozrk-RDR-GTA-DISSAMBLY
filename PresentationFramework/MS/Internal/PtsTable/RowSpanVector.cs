using System;
using System.Windows.Documents;

namespace MS.Internal.PtsTable
{
	// Token: 0x0200060B RID: 1547
	internal sealed class RowSpanVector
	{
		// Token: 0x06006717 RID: 26391 RVA: 0x001CD690 File Offset: 0x001CB890
		internal RowSpanVector()
		{
			this._entries = new RowSpanVector.Entry[8];
			this._entries[0].Cell = null;
			this._entries[0].Start = 1073741823;
			this._entries[0].Range = 1073741823;
			this._entries[0].Ttl = int.MaxValue;
			this._size = 1;
		}

		// Token: 0x06006718 RID: 26392 RVA: 0x001CD70C File Offset: 0x001CB90C
		internal void Register(TableCell cell)
		{
			int columnIndex = cell.ColumnIndex;
			if (this._size == this._entries.Length)
			{
				this.InflateCapacity();
			}
			for (int i = this._size - 1; i >= this._index; i--)
			{
				this._entries[i + 1] = this._entries[i];
			}
			this._entries[this._index].Cell = cell;
			this._entries[this._index].Start = columnIndex;
			this._entries[this._index].Range = cell.ColumnSpan;
			this._entries[this._index].Ttl = cell.RowSpan - 1;
			this._size++;
			this._index++;
		}

		// Token: 0x06006719 RID: 26393 RVA: 0x001CD7EC File Offset: 0x001CB9EC
		internal void GetFirstAvailableRange(out int firstAvailableIndex, out int firstOccupiedIndex)
		{
			this._index = 0;
			firstAvailableIndex = 0;
			firstOccupiedIndex = this._entries[this._index].Start;
		}

		// Token: 0x0600671A RID: 26394 RVA: 0x001CD810 File Offset: 0x001CBA10
		internal void GetNextAvailableRange(out int firstAvailableIndex, out int firstOccupiedIndex)
		{
			firstAvailableIndex = this._entries[this._index].Start + this._entries[this._index].Range;
			RowSpanVector.Entry[] entries = this._entries;
			int index = this._index;
			entries[index].Ttl = entries[index].Ttl - 1;
			this._index++;
			firstOccupiedIndex = this._entries[this._index].Start;
		}

		// Token: 0x0600671B RID: 26395 RVA: 0x001CD890 File Offset: 0x001CBA90
		internal void GetSpanCells(out TableCell[] cells, out bool isLastRowOfAnySpan)
		{
			cells = RowSpanVector.s_noCells;
			isLastRowOfAnySpan = false;
			while (this._index < this._size)
			{
				RowSpanVector.Entry[] entries = this._entries;
				int index = this._index;
				entries[index].Ttl = entries[index].Ttl - 1;
				this._index++;
			}
			if (this._size > 1)
			{
				cells = new TableCell[this._size - 1];
				int num = 0;
				int num2 = 0;
				do
				{
					cells[num] = this._entries[num].Cell;
					if (this._entries[num].Ttl > 0)
					{
						if (num != num2)
						{
							this._entries[num2] = this._entries[num];
						}
						num2++;
					}
					num++;
				}
				while (num < this._size - 1);
				if (num != num2)
				{
					this._entries[num2] = this._entries[num];
					isLastRowOfAnySpan = true;
				}
				this._size = num2 + 1;
			}
		}

		// Token: 0x0600671C RID: 26396 RVA: 0x001CD97F File Offset: 0x001CBB7F
		internal bool Empty()
		{
			return this._size == 1;
		}

		// Token: 0x0600671D RID: 26397 RVA: 0x001CD98C File Offset: 0x001CBB8C
		private void InflateCapacity()
		{
			RowSpanVector.Entry[] array = new RowSpanVector.Entry[this._entries.Length * 2];
			Array.Copy(this._entries, array, this._entries.Length);
			this._entries = array;
		}

		// Token: 0x04003349 RID: 13129
		private RowSpanVector.Entry[] _entries;

		// Token: 0x0400334A RID: 13130
		private int _size;

		// Token: 0x0400334B RID: 13131
		private int _index;

		// Token: 0x0400334C RID: 13132
		private const int c_defaultCapacity = 8;

		// Token: 0x0400334D RID: 13133
		private static TableCell[] s_noCells = new TableCell[0];

		// Token: 0x02000A1B RID: 2587
		private struct Entry
		{
			// Token: 0x040046F2 RID: 18162
			internal TableCell Cell;

			// Token: 0x040046F3 RID: 18163
			internal int Start;

			// Token: 0x040046F4 RID: 18164
			internal int Range;

			// Token: 0x040046F5 RID: 18165
			internal int Ttl;
		}
	}
}
