using System;

namespace System.Windows.Documents
{
	// Token: 0x020003CF RID: 975
	internal class ColumnState
	{
		// Token: 0x06003480 RID: 13440 RVA: 0x000E97F6 File Offset: 0x000E79F6
		internal ColumnState()
		{
			this._nCellX = 0L;
			this._row = null;
			this._fFilled = false;
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003481 RID: 13441 RVA: 0x000E9814 File Offset: 0x000E7A14
		// (set) Token: 0x06003482 RID: 13442 RVA: 0x000E981C File Offset: 0x000E7A1C
		internal long CellX
		{
			get
			{
				return this._nCellX;
			}
			set
			{
				this._nCellX = value;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003483 RID: 13443 RVA: 0x000E9825 File Offset: 0x000E7A25
		// (set) Token: 0x06003484 RID: 13444 RVA: 0x000E982D File Offset: 0x000E7A2D
		internal DocumentNode Row
		{
			get
			{
				return this._row;
			}
			set
			{
				this._row = value;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x000E9836 File Offset: 0x000E7A36
		// (set) Token: 0x06003486 RID: 13446 RVA: 0x000E983E File Offset: 0x000E7A3E
		internal bool IsFilled
		{
			get
			{
				return this._fFilled;
			}
			set
			{
				this._fFilled = value;
			}
		}

		// Token: 0x040024DA RID: 9434
		private long _nCellX;

		// Token: 0x040024DB RID: 9435
		private DocumentNode _row;

		// Token: 0x040024DC RID: 9436
		private bool _fFilled;
	}
}
