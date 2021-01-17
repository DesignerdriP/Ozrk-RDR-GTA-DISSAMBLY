using System;

namespace System.Windows.Documents
{
	// Token: 0x020003C8 RID: 968
	internal class ListLevel
	{
		// Token: 0x0600340A RID: 13322 RVA: 0x000E7B0A File Offset: 0x000E5D0A
		internal ListLevel()
		{
			this._nStartIndex = 1L;
			this._numberType = MarkerStyle.MarkerArabic;
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x0600340B RID: 13323 RVA: 0x000E7B21 File Offset: 0x000E5D21
		// (set) Token: 0x0600340C RID: 13324 RVA: 0x000E7B29 File Offset: 0x000E5D29
		internal long StartIndex
		{
			get
			{
				return this._nStartIndex;
			}
			set
			{
				this._nStartIndex = value;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x0600340D RID: 13325 RVA: 0x000E7B32 File Offset: 0x000E5D32
		// (set) Token: 0x0600340E RID: 13326 RVA: 0x000E7B3A File Offset: 0x000E5D3A
		internal MarkerStyle Marker
		{
			get
			{
				return this._numberType;
			}
			set
			{
				this._numberType = value;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (set) Token: 0x0600340F RID: 13327 RVA: 0x000E7B43 File Offset: 0x000E5D43
		internal FormatState FormatState
		{
			set
			{
				this._formatState = value;
			}
		}

		// Token: 0x040024BB RID: 9403
		private long _nStartIndex;

		// Token: 0x040024BC RID: 9404
		private MarkerStyle _numberType;

		// Token: 0x040024BD RID: 9405
		private FormatState _formatState;
	}
}
