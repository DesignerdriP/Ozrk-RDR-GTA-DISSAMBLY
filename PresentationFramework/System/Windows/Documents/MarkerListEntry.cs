using System;

namespace System.Windows.Documents
{
	// Token: 0x020003C2 RID: 962
	internal class MarkerListEntry
	{
		// Token: 0x060033CF RID: 13263 RVA: 0x000E6F07 File Offset: 0x000E5107
		internal MarkerListEntry()
		{
			this._marker = MarkerStyle.MarkerBullet;
			this._nILS = -1L;
			this._nStartIndexOverride = -1L;
			this._nStartIndexDefault = -1L;
			this._nVirtualListLevel = -1L;
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x060033D0 RID: 13264 RVA: 0x000E6F37 File Offset: 0x000E5137
		// (set) Token: 0x060033D1 RID: 13265 RVA: 0x000E6F3F File Offset: 0x000E513F
		internal MarkerStyle Marker
		{
			get
			{
				return this._marker;
			}
			set
			{
				this._marker = value;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x060033D2 RID: 13266 RVA: 0x000E6F48 File Offset: 0x000E5148
		// (set) Token: 0x060033D3 RID: 13267 RVA: 0x000E6F50 File Offset: 0x000E5150
		internal long StartIndexOverride
		{
			get
			{
				return this._nStartIndexOverride;
			}
			set
			{
				this._nStartIndexOverride = value;
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x060033D4 RID: 13268 RVA: 0x000E6F59 File Offset: 0x000E5159
		// (set) Token: 0x060033D5 RID: 13269 RVA: 0x000E6F61 File Offset: 0x000E5161
		internal long StartIndexDefault
		{
			get
			{
				return this._nStartIndexDefault;
			}
			set
			{
				this._nStartIndexDefault = value;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x060033D6 RID: 13270 RVA: 0x000E6F6A File Offset: 0x000E516A
		// (set) Token: 0x060033D7 RID: 13271 RVA: 0x000E6F72 File Offset: 0x000E5172
		internal long VirtualListLevel
		{
			get
			{
				return this._nVirtualListLevel;
			}
			set
			{
				this._nVirtualListLevel = value;
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x060033D8 RID: 13272 RVA: 0x000E6F7B File Offset: 0x000E517B
		internal long StartIndexToUse
		{
			get
			{
				if (this._nStartIndexOverride <= 0L)
				{
					return this._nStartIndexDefault;
				}
				return this._nStartIndexOverride;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x060033D9 RID: 13273 RVA: 0x000E6F94 File Offset: 0x000E5194
		// (set) Token: 0x060033DA RID: 13274 RVA: 0x000E6F9C File Offset: 0x000E519C
		internal long ILS
		{
			get
			{
				return this._nILS;
			}
			set
			{
				this._nILS = value;
			}
		}

		// Token: 0x040024AC RID: 9388
		private MarkerStyle _marker;

		// Token: 0x040024AD RID: 9389
		private long _nStartIndexOverride;

		// Token: 0x040024AE RID: 9390
		private long _nStartIndexDefault;

		// Token: 0x040024AF RID: 9391
		private long _nVirtualListLevel;

		// Token: 0x040024B0 RID: 9392
		private long _nILS;
	}
}
