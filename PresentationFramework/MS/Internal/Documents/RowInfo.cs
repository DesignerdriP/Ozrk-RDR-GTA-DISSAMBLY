using System;
using System.Windows;

namespace MS.Internal.Documents
{
	// Token: 0x020006EA RID: 1770
	internal class RowInfo
	{
		// Token: 0x060071E3 RID: 29155 RVA: 0x00209247 File Offset: 0x00207447
		public RowInfo()
		{
			this._rowSize = new Size(0.0, 0.0);
		}

		// Token: 0x060071E4 RID: 29156 RVA: 0x0020926C File Offset: 0x0020746C
		public void AddPage(Size pageSize)
		{
			this._pageCount++;
			this._rowSize.Width = this._rowSize.Width + pageSize.Width;
			this._rowSize.Height = Math.Max(pageSize.Height, this._rowSize.Height);
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x002092C2 File Offset: 0x002074C2
		public void ClearPages()
		{
			this._pageCount = 0;
			this._rowSize.Width = 0.0;
			this._rowSize.Height = 0.0;
		}

		// Token: 0x17001B18 RID: 6936
		// (get) Token: 0x060071E6 RID: 29158 RVA: 0x002092F3 File Offset: 0x002074F3
		public Size RowSize
		{
			get
			{
				return this._rowSize;
			}
		}

		// Token: 0x17001B19 RID: 6937
		// (get) Token: 0x060071E7 RID: 29159 RVA: 0x002092FB File Offset: 0x002074FB
		// (set) Token: 0x060071E8 RID: 29160 RVA: 0x00209303 File Offset: 0x00207503
		public double VerticalOffset
		{
			get
			{
				return this._verticalOffset;
			}
			set
			{
				this._verticalOffset = value;
			}
		}

		// Token: 0x17001B1A RID: 6938
		// (get) Token: 0x060071E9 RID: 29161 RVA: 0x0020930C File Offset: 0x0020750C
		// (set) Token: 0x060071EA RID: 29162 RVA: 0x00209314 File Offset: 0x00207514
		public int FirstPage
		{
			get
			{
				return this._firstPage;
			}
			set
			{
				this._firstPage = value;
			}
		}

		// Token: 0x17001B1B RID: 6939
		// (get) Token: 0x060071EB RID: 29163 RVA: 0x0020931D File Offset: 0x0020751D
		public int PageCount
		{
			get
			{
				return this._pageCount;
			}
		}

		// Token: 0x04003749 RID: 14153
		private Size _rowSize;

		// Token: 0x0400374A RID: 14154
		private double _verticalOffset;

		// Token: 0x0400374B RID: 14155
		private int _firstPage;

		// Token: 0x0400374C RID: 14156
		private int _pageCount;
	}
}
