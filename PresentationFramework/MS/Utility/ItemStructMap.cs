using System;

namespace MS.Utility
{
	// Token: 0x020005D9 RID: 1497
	internal struct ItemStructMap<T>
	{
		// Token: 0x06006397 RID: 25495 RVA: 0x001C06D8 File Offset: 0x001BE8D8
		public int EnsureEntry(int key)
		{
			int num = this.Search(key);
			if (num < 0)
			{
				if (this.Entries == null)
				{
					this.Entries = new ItemStructMap<T>.Entry[4];
				}
				num = ~num;
				ItemStructMap<T>.Entry[] array = this.Entries;
				if (this.Count + 1 > this.Entries.Length)
				{
					array = new ItemStructMap<T>.Entry[this.Entries.Length * 2];
					Array.Copy(this.Entries, 0, array, 0, num);
				}
				Array.Copy(this.Entries, num, array, num + 1, this.Count - num);
				this.Entries = array;
				this.Entries[num] = ItemStructMap<T>.EmptyEntry;
				this.Entries[num].Key = key;
				this.Count++;
			}
			return num;
		}

		// Token: 0x06006398 RID: 25496 RVA: 0x001C0794 File Offset: 0x001BE994
		public int Search(int key)
		{
			int num = int.MaxValue;
			int num2 = 0;
			if (this.Count > 4)
			{
				int i = 0;
				int num3 = this.Count - 1;
				while (i <= num3)
				{
					num2 = (num3 + i) / 2;
					num = this.Entries[num2].Key;
					if (key == num)
					{
						return num2;
					}
					if (key < num)
					{
						num3 = num2 - 1;
					}
					else
					{
						i = num2 + 1;
					}
				}
			}
			else
			{
				for (int j = 0; j < this.Count; j++)
				{
					num2 = j;
					num = this.Entries[num2].Key;
					if (key == num)
					{
						return num2;
					}
					if (key < num)
					{
						break;
					}
				}
			}
			if (key > num)
			{
				num2++;
			}
			return ~num2;
		}

		// Token: 0x040031E7 RID: 12775
		private const int SearchTypeThreshold = 4;

		// Token: 0x040031E8 RID: 12776
		public ItemStructMap<T>.Entry[] Entries;

		// Token: 0x040031E9 RID: 12777
		public int Count;

		// Token: 0x040031EA RID: 12778
		private static ItemStructMap<T>.Entry EmptyEntry;

		// Token: 0x020009FF RID: 2559
		public struct Entry
		{
			// Token: 0x040046AA RID: 18090
			public int Key;

			// Token: 0x040046AB RID: 18091
			public T Value;
		}
	}
}
