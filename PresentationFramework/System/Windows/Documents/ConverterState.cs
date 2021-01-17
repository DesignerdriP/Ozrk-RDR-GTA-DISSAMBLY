using System;

namespace System.Windows.Documents
{
	// Token: 0x020003D2 RID: 978
	internal class ConverterState
	{
		// Token: 0x060034B2 RID: 13490 RVA: 0x000EAD54 File Offset: 0x000E8F54
		internal ConverterState()
		{
			this._rtfFormatStack = new RtfFormatStack();
			this._documentNodeArray = new DocumentNodeArray();
			this._documentNodeArray.IsMain = true;
			this._fontTable = new FontTable();
			this._colorTable = new ColorTable();
			this._listTable = new ListTable();
			this._listOverrideTable = new ListOverrideTable();
			this._defaultFont = -1L;
			this._defaultLang = -1L;
			this._defaultLangFE = -1L;
			this._bMarkerWhiteSpace = false;
			this._bMarkerPresent = false;
			this._border = null;
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x000EADE2 File Offset: 0x000E8FE2
		internal FormatState PreviousTopFormatState(int fromTop)
		{
			return this._rtfFormatStack.PrevTop(fromTop);
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x060034B4 RID: 13492 RVA: 0x000EADF0 File Offset: 0x000E8FF0
		internal RtfFormatStack RtfFormatStack
		{
			get
			{
				return this._rtfFormatStack;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x060034B5 RID: 13493 RVA: 0x000EADF8 File Offset: 0x000E8FF8
		internal FontTable FontTable
		{
			get
			{
				return this._fontTable;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x060034B6 RID: 13494 RVA: 0x000EAE00 File Offset: 0x000E9000
		internal ColorTable ColorTable
		{
			get
			{
				return this._colorTable;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x060034B7 RID: 13495 RVA: 0x000EAE08 File Offset: 0x000E9008
		internal ListTable ListTable
		{
			get
			{
				return this._listTable;
			}
		}

		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x060034B8 RID: 13496 RVA: 0x000EAE10 File Offset: 0x000E9010
		internal ListOverrideTable ListOverrideTable
		{
			get
			{
				return this._listOverrideTable;
			}
		}

		// Token: 0x17000D97 RID: 3479
		// (get) Token: 0x060034B9 RID: 13497 RVA: 0x000EAE18 File Offset: 0x000E9018
		internal DocumentNodeArray DocumentNodeArray
		{
			get
			{
				return this._documentNodeArray;
			}
		}

		// Token: 0x17000D98 RID: 3480
		// (get) Token: 0x060034BA RID: 13498 RVA: 0x000EAE20 File Offset: 0x000E9020
		internal FormatState TopFormatState
		{
			get
			{
				return this._rtfFormatStack.Top();
			}
		}

		// Token: 0x17000D99 RID: 3481
		// (get) Token: 0x060034BB RID: 13499 RVA: 0x000EAE2D File Offset: 0x000E902D
		// (set) Token: 0x060034BC RID: 13500 RVA: 0x000EAE35 File Offset: 0x000E9035
		internal int CodePage
		{
			get
			{
				return this._codePage;
			}
			set
			{
				this._codePage = value;
			}
		}

		// Token: 0x17000D9A RID: 3482
		// (get) Token: 0x060034BD RID: 13501 RVA: 0x000EAE3E File Offset: 0x000E903E
		// (set) Token: 0x060034BE RID: 13502 RVA: 0x000EAE46 File Offset: 0x000E9046
		internal long DefaultFont
		{
			get
			{
				return this._defaultFont;
			}
			set
			{
				this._defaultFont = value;
			}
		}

		// Token: 0x17000D9B RID: 3483
		// (get) Token: 0x060034BF RID: 13503 RVA: 0x000EAE4F File Offset: 0x000E904F
		// (set) Token: 0x060034C0 RID: 13504 RVA: 0x000EAE57 File Offset: 0x000E9057
		internal long DefaultLang
		{
			get
			{
				return this._defaultLang;
			}
			set
			{
				this._defaultLang = value;
			}
		}

		// Token: 0x17000D9C RID: 3484
		// (get) Token: 0x060034C1 RID: 13505 RVA: 0x000EAE60 File Offset: 0x000E9060
		// (set) Token: 0x060034C2 RID: 13506 RVA: 0x000EAE68 File Offset: 0x000E9068
		internal long DefaultLangFE
		{
			get
			{
				return this._defaultLangFE;
			}
			set
			{
				this._defaultLangFE = value;
			}
		}

		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x060034C3 RID: 13507 RVA: 0x000EAE71 File Offset: 0x000E9071
		// (set) Token: 0x060034C4 RID: 13508 RVA: 0x000EAE79 File Offset: 0x000E9079
		internal bool IsMarkerWhiteSpace
		{
			get
			{
				return this._bMarkerWhiteSpace;
			}
			set
			{
				this._bMarkerWhiteSpace = value;
			}
		}

		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x060034C5 RID: 13509 RVA: 0x000EAE82 File Offset: 0x000E9082
		// (set) Token: 0x060034C6 RID: 13510 RVA: 0x000EAE8A File Offset: 0x000E908A
		internal bool IsMarkerPresent
		{
			get
			{
				return this._bMarkerPresent;
			}
			set
			{
				this._bMarkerPresent = value;
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x060034C7 RID: 13511 RVA: 0x000EAE93 File Offset: 0x000E9093
		// (set) Token: 0x060034C8 RID: 13512 RVA: 0x000EAE9B File Offset: 0x000E909B
		internal BorderFormat CurrentBorder
		{
			get
			{
				return this._border;
			}
			set
			{
				this._border = value;
			}
		}

		// Token: 0x040024DF RID: 9439
		private RtfFormatStack _rtfFormatStack;

		// Token: 0x040024E0 RID: 9440
		private DocumentNodeArray _documentNodeArray;

		// Token: 0x040024E1 RID: 9441
		private FontTable _fontTable;

		// Token: 0x040024E2 RID: 9442
		private ColorTable _colorTable;

		// Token: 0x040024E3 RID: 9443
		private ListTable _listTable;

		// Token: 0x040024E4 RID: 9444
		private ListOverrideTable _listOverrideTable;

		// Token: 0x040024E5 RID: 9445
		private long _defaultFont;

		// Token: 0x040024E6 RID: 9446
		private long _defaultLang;

		// Token: 0x040024E7 RID: 9447
		private long _defaultLangFE;

		// Token: 0x040024E8 RID: 9448
		private int _codePage;

		// Token: 0x040024E9 RID: 9449
		private bool _bMarkerWhiteSpace;

		// Token: 0x040024EA RID: 9450
		private bool _bMarkerPresent;

		// Token: 0x040024EB RID: 9451
		private BorderFormat _border;
	}
}
