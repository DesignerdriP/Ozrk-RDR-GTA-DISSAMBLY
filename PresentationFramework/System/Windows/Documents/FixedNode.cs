using System;

namespace System.Windows.Documents
{
	// Token: 0x02000354 RID: 852
	internal struct FixedNode : IComparable
	{
		// Token: 0x06002D4D RID: 11597 RVA: 0x000CC6BF File Offset: 0x000CA8BF
		internal static FixedNode Create(int pageIndex, int childLevels, int level1Index, int level2Index, int[] childPath)
		{
			if (childLevels == 1)
			{
				return new FixedNode(pageIndex, level1Index);
			}
			if (childLevels != 2)
			{
				return FixedNode.Create(pageIndex, childPath);
			}
			return new FixedNode(pageIndex, level1Index, level2Index);
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000CC6E4 File Offset: 0x000CA8E4
		internal static FixedNode Create(int pageIndex, int[] childPath)
		{
			int[] array = new int[childPath.Length + 1];
			array[0] = pageIndex;
			childPath.CopyTo(array, 1);
			return new FixedNode(array);
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000CC70E File Offset: 0x000CA90E
		private FixedNode(int page, int level1Index)
		{
			this._path = new int[2];
			this._path[0] = page;
			this._path[1] = level1Index;
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000CC72E File Offset: 0x000CA92E
		private FixedNode(int page, int level1Index, int level2Index)
		{
			this._path = new int[3];
			this._path[0] = page;
			this._path[1] = level1Index;
			this._path[2] = level2Index;
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000CC757 File Offset: 0x000CA957
		private FixedNode(int[] path)
		{
			this._path = path;
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000CC760 File Offset: 0x000CA960
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (o.GetType() != typeof(FixedNode))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FixedNode)
				}), "o");
			}
			FixedNode fixedNode = (FixedNode)o;
			return this.CompareTo(fixedNode);
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000CC7D4 File Offset: 0x000CA9D4
		public int CompareTo(FixedNode fixedNode)
		{
			int num = this.Page.CompareTo(fixedNode.Page);
			if (num == 0)
			{
				int num2 = 1;
				while (num2 <= this.ChildLevels && num2 <= fixedNode.ChildLevels)
				{
					int num3 = this[num2];
					int num4 = fixedNode[num2];
					if (num3 != num4)
					{
						return num3.CompareTo(num4);
					}
					num2++;
				}
			}
			return num;
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000CC83C File Offset: 0x000CAA3C
		internal int ComparetoIndex(int[] childPath)
		{
			int num = 0;
			while (num < childPath.Length && num < this._path.Length - 1)
			{
				if (childPath[num] != this._path[num + 1])
				{
					return childPath[num].CompareTo(this._path[num + 1]);
				}
				num++;
			}
			return 0;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000CC88B File Offset: 0x000CAA8B
		public static bool operator <(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) < 0;
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000CC898 File Offset: 0x000CAA98
		public static bool operator <=(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) <= 0;
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000CC8A8 File Offset: 0x000CAAA8
		public static bool operator >(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) > 0;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000CC8B5 File Offset: 0x000CAAB5
		public static bool operator >=(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) >= 0;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000CC8C5 File Offset: 0x000CAAC5
		public override bool Equals(object o)
		{
			return o is FixedNode && this.Equals((FixedNode)o);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000CC8DD File Offset: 0x000CAADD
		public bool Equals(FixedNode fixedp)
		{
			return this.CompareTo(fixedp) == 0;
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000CC8E9 File Offset: 0x000CAAE9
		public static bool operator ==(FixedNode fp1, FixedNode fp2)
		{
			return fp1.Equals(fp2);
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000CC8F3 File Offset: 0x000CAAF3
		public static bool operator !=(FixedNode fp1, FixedNode fp2)
		{
			return !fp1.Equals(fp2);
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000CC900 File Offset: 0x000CAB00
		public override int GetHashCode()
		{
			int num = 0;
			foreach (int num2 in this._path)
			{
				num = 43 * num + num2;
			}
			return num;
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06002D5E RID: 11614 RVA: 0x000CC930 File Offset: 0x000CAB30
		internal int Page
		{
			get
			{
				return this._path[0];
			}
		}

		// Token: 0x17000B4C RID: 2892
		internal int this[int level]
		{
			get
			{
				return this._path[level];
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06002D60 RID: 11616 RVA: 0x000CC944 File Offset: 0x000CAB44
		internal int ChildLevels
		{
			get
			{
				return this._path.Length - 1;
			}
		}

		// Token: 0x04001DA5 RID: 7589
		private readonly int[] _path;
	}
}
