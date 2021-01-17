using System;
using System.Windows;
using System.Windows.Documents;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000612 RID: 1554
	internal class CellInfo
	{
		// Token: 0x06006777 RID: 26487 RVA: 0x001CEE94 File Offset: 0x001CD094
		internal CellInfo(TableParaClient tpc, CellParaClient cpc)
		{
			this._rectTable = new Rect(TextDpi.FromTextDpi(tpc.Rect.u), TextDpi.FromTextDpi(tpc.Rect.v), TextDpi.FromTextDpi(tpc.Rect.du), TextDpi.FromTextDpi(tpc.Rect.dv));
			this._rectCell = new Rect(TextDpi.FromTextDpi(cpc.Rect.u), TextDpi.FromTextDpi(cpc.Rect.v), TextDpi.FromTextDpi(cpc.Rect.du), TextDpi.FromTextDpi(cpc.Rect.dv));
			this._autofitWidth = tpc.AutofitWidth;
			this._columnWidths = new double[tpc.CalculatedColumns.Length];
			for (int i = 0; i < tpc.CalculatedColumns.Length; i++)
			{
				this._columnWidths[i] = tpc.CalculatedColumns[i].DurWidth;
			}
			this._cell = cpc.Cell;
		}

		// Token: 0x06006778 RID: 26488 RVA: 0x001CEF94 File Offset: 0x001CD194
		internal void Adjust(Point ptAdjust)
		{
			this._rectTable.X = this._rectTable.X + ptAdjust.X;
			this._rectTable.Y = this._rectTable.Y + ptAdjust.Y;
			this._rectCell.X = this._rectCell.X + ptAdjust.X;
			this._rectCell.Y = this._rectCell.Y + ptAdjust.Y;
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x06006779 RID: 26489 RVA: 0x001CF005 File Offset: 0x001CD205
		internal TableCell Cell
		{
			get
			{
				return this._cell;
			}
		}

		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600677A RID: 26490 RVA: 0x001CF00D File Offset: 0x001CD20D
		internal double[] TableColumnWidths
		{
			get
			{
				return this._columnWidths;
			}
		}

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x0600677B RID: 26491 RVA: 0x001CF015 File Offset: 0x001CD215
		internal double TableAutofitWidth
		{
			get
			{
				return this._autofitWidth;
			}
		}

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600677C RID: 26492 RVA: 0x001CF01D File Offset: 0x001CD21D
		internal Rect TableArea
		{
			get
			{
				return this._rectTable;
			}
		}

		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x0600677D RID: 26493 RVA: 0x001CF025 File Offset: 0x001CD225
		internal Rect CellArea
		{
			get
			{
				return this._rectCell;
			}
		}

		// Token: 0x0400336E RID: 13166
		private Rect _rectCell;

		// Token: 0x0400336F RID: 13167
		private Rect _rectTable;

		// Token: 0x04003370 RID: 13168
		private TableCell _cell;

		// Token: 0x04003371 RID: 13169
		private double[] _columnWidths;

		// Token: 0x04003372 RID: 13170
		private double _autofitWidth;
	}
}
