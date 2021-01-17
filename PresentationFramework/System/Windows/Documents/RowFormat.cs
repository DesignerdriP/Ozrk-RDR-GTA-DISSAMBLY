using System;
using System.Collections;

namespace System.Windows.Documents
{
	// Token: 0x020003C1 RID: 961
	internal class RowFormat
	{
		// Token: 0x060033BB RID: 13243 RVA: 0x000E6B34 File Offset: 0x000E4D34
		internal RowFormat()
		{
			this._rowCellFormat = new CellFormat();
			this._widthA = new CellWidth();
			this._widthB = new CellWidth();
			this._widthRow = new CellWidth();
			this._cellFormats = new ArrayList();
			this._dir = DirState.DirLTR;
			this._nTrgaph = -1L;
			this._nTrleft = 0L;
		}

		// Token: 0x060033BC RID: 13244 RVA: 0x000E6B98 File Offset: 0x000E4D98
		internal RowFormat(RowFormat ri)
		{
			this._rowCellFormat = new CellFormat(ri.RowCellFormat);
			this._cellFormats = new ArrayList();
			this._widthA = new CellWidth(ri.WidthA);
			this._widthB = new CellWidth(ri.WidthB);
			this._widthRow = new CellWidth(ri.WidthRow);
			this._nTrgaph = ri.Trgaph;
			this._dir = ri.Dir;
			this._nTrleft = ri._nTrleft;
			for (int i = 0; i < ri.CellCount; i++)
			{
				this._cellFormats.Add(new CellFormat(ri.NthCellFormat(i)));
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x060033BD RID: 13245 RVA: 0x000E6C47 File Offset: 0x000E4E47
		internal CellFormat RowCellFormat
		{
			get
			{
				return this._rowCellFormat;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x060033BE RID: 13246 RVA: 0x000E6C4F File Offset: 0x000E4E4F
		internal int CellCount
		{
			get
			{
				return this._cellFormats.Count;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x060033BF RID: 13247 RVA: 0x000E6C5C File Offset: 0x000E4E5C
		internal CellFormat TopCellFormat
		{
			get
			{
				if (this.CellCount <= 0)
				{
					return null;
				}
				return this.NthCellFormat(this.CellCount - 1);
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x060033C0 RID: 13248 RVA: 0x000E6C77 File Offset: 0x000E4E77
		internal CellWidth WidthA
		{
			get
			{
				return this._widthA;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x060033C1 RID: 13249 RVA: 0x000E6C7F File Offset: 0x000E4E7F
		internal CellWidth WidthB
		{
			get
			{
				return this._widthB;
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x060033C2 RID: 13250 RVA: 0x000E6C87 File Offset: 0x000E4E87
		internal CellWidth WidthRow
		{
			get
			{
				return this._widthRow;
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000E6C8F File Offset: 0x000E4E8F
		// (set) Token: 0x060033C4 RID: 13252 RVA: 0x000E6C97 File Offset: 0x000E4E97
		internal long Trgaph
		{
			get
			{
				return this._nTrgaph;
			}
			set
			{
				this._nTrgaph = value;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x060033C5 RID: 13253 RVA: 0x000E6CA0 File Offset: 0x000E4EA0
		// (set) Token: 0x060033C6 RID: 13254 RVA: 0x000E6CA8 File Offset: 0x000E4EA8
		internal long Trleft
		{
			get
			{
				return this._nTrleft;
			}
			set
			{
				this._nTrleft = value;
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x000E6CB1 File Offset: 0x000E4EB1
		// (set) Token: 0x060033C8 RID: 13256 RVA: 0x000E6CB9 File Offset: 0x000E4EB9
		internal DirState Dir
		{
			get
			{
				return this._dir;
			}
			set
			{
				this._dir = value;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x060033C9 RID: 13257 RVA: 0x000E6CC4 File Offset: 0x000E4EC4
		internal bool IsVMerge
		{
			get
			{
				for (int i = 0; i < this.CellCount; i++)
				{
					if (this.NthCellFormat(i).IsVMerge)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x000E6CF3 File Offset: 0x000E4EF3
		internal CellFormat NthCellFormat(int n)
		{
			if (n < 0 || n >= this.CellCount)
			{
				return this.RowCellFormat;
			}
			return (CellFormat)this._cellFormats[n];
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000E6D1A File Offset: 0x000E4F1A
		internal CellFormat NextCellFormat()
		{
			this._cellFormats.Add(new CellFormat(this.RowCellFormat));
			return this.TopCellFormat;
		}

		// Token: 0x060033CC RID: 13260 RVA: 0x000E6D39 File Offset: 0x000E4F39
		internal CellFormat CurrentCellFormat()
		{
			if (this.CellCount == 0 || !this.TopCellFormat.IsPending)
			{
				return this.NextCellFormat();
			}
			return this.TopCellFormat;
		}

		// Token: 0x060033CD RID: 13261 RVA: 0x000E6D60 File Offset: 0x000E4F60
		internal void CanonicalizeWidthsFromRTF()
		{
			if (this.CellCount == 0)
			{
				return;
			}
			CellFormat cellFormat = null;
			long num = this.Trleft;
			for (int i = 0; i < this.CellCount; i++)
			{
				CellFormat cellFormat2 = this.NthCellFormat(i);
				if (!cellFormat2.IsHMerge)
				{
					if (cellFormat2.IsHMergeFirst)
					{
						for (int j = i + 1; j < this.CellCount; j++)
						{
							CellFormat cellFormat3 = this.NthCellFormat(j);
							if (!cellFormat3.IsHMerge)
							{
								break;
							}
							cellFormat2.CellX = cellFormat3.CellX;
						}
					}
					if (cellFormat2.Width.Value == 0L && cellFormat2.IsCellXSet)
					{
						cellFormat2.Width.Type = WidthType.WidthTwips;
						cellFormat2.Width.Value = ((cellFormat == null) ? (cellFormat2.CellX - this.Trleft) : (cellFormat2.CellX - cellFormat.CellX));
					}
					else if (cellFormat2.Width.Value > 0L && !cellFormat2.IsCellXSet)
					{
						num += cellFormat2.Width.Value;
						cellFormat2.CellX = num;
					}
					cellFormat = cellFormat2;
				}
			}
			num = this.NthCellFormat(0).CellX;
			for (int k = 1; k < this.CellCount; k++)
			{
				CellFormat cellFormat4 = this.NthCellFormat(k);
				if (cellFormat4.CellX < num)
				{
					cellFormat4.CellX = num + 1L;
				}
				num = cellFormat4.CellX;
			}
		}

		// Token: 0x060033CE RID: 13262 RVA: 0x000E6EAC File Offset: 0x000E50AC
		internal void CanonicalizeWidthsFromXaml()
		{
			long num = this.Trleft;
			for (int i = 0; i < this.CellCount; i++)
			{
				CellFormat cellFormat = this.NthCellFormat(i);
				if (cellFormat.Width.Type == WidthType.WidthTwips)
				{
					num += cellFormat.Width.Value;
				}
				else
				{
					num += 1440L;
				}
				cellFormat.CellX = num;
			}
		}

		// Token: 0x040024A4 RID: 9380
		private CellFormat _rowCellFormat;

		// Token: 0x040024A5 RID: 9381
		private CellWidth _widthA;

		// Token: 0x040024A6 RID: 9382
		private CellWidth _widthB;

		// Token: 0x040024A7 RID: 9383
		private CellWidth _widthRow;

		// Token: 0x040024A8 RID: 9384
		private ArrayList _cellFormats;

		// Token: 0x040024A9 RID: 9385
		private long _nTrgaph;

		// Token: 0x040024AA RID: 9386
		private long _nTrleft;

		// Token: 0x040024AB RID: 9387
		private DirState _dir;
	}
}
