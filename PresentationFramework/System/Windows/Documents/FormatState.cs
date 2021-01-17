using System;

namespace System.Windows.Documents
{
	// Token: 0x020003BA RID: 954
	internal class FormatState
	{
		// Token: 0x060032DA RID: 13018 RVA: 0x000E5169 File Offset: 0x000E3369
		internal FormatState()
		{
			this._dest = RtfDestination.DestNormal;
			this._stateSkip = 1;
			this.SetCharDefaults();
			this.SetParaDefaults();
			this.SetRowDefaults();
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x000E5194 File Offset: 0x000E3394
		internal FormatState(FormatState formatState)
		{
			this.Bold = formatState.Bold;
			this.Italic = formatState.Italic;
			this.Engrave = formatState.Engrave;
			this.Shadow = formatState.Shadow;
			this.SCaps = formatState.SCaps;
			this.Outline = formatState.Outline;
			this.Super = formatState.Super;
			this.Sub = formatState.Sub;
			this.SuperOffset = formatState.SuperOffset;
			this.FontSize = formatState.FontSize;
			this.Font = formatState.Font;
			this.CodePage = formatState.CodePage;
			this.CF = formatState.CF;
			this.CB = formatState.CB;
			this.DirChar = formatState.DirChar;
			this.UL = formatState.UL;
			this.Strike = formatState.Strike;
			this.Expand = formatState.Expand;
			this.Lang = formatState.Lang;
			this.LangFE = formatState.LangFE;
			this.LangCur = formatState.LangCur;
			this.FontSlot = formatState.FontSlot;
			this.SB = formatState.SB;
			this.SA = formatState.SA;
			this.FI = formatState.FI;
			this.RI = formatState.RI;
			this.LI = formatState.LI;
			this.SL = formatState.SL;
			this.SLMult = formatState.SLMult;
			this.HAlign = formatState.HAlign;
			this.ILVL = formatState.ILVL;
			this.ITAP = formatState.ITAP;
			this.ILS = formatState.ILS;
			this.DirPara = formatState.DirPara;
			this.CFPara = formatState.CFPara;
			this.CBPara = formatState.CBPara;
			this.ParaShading = formatState.ParaShading;
			this.Marker = formatState.Marker;
			this.IsContinue = formatState.IsContinue;
			this.StartIndex = formatState.StartIndex;
			this.StartIndexDefault = formatState.StartIndexDefault;
			this.IsInTable = formatState.IsInTable;
			this._pb = (formatState.HasParaBorder ? new ParaBorder(formatState.ParaBorder) : null);
			this.RowFormat = formatState._rowFormat;
			this.RtfDestination = formatState.RtfDestination;
			this.IsHidden = formatState.IsHidden;
			this._stateSkip = formatState.UnicodeSkip;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x000E53EC File Offset: 0x000E35EC
		internal void SetCharDefaults()
		{
			this._fBold = false;
			this._fItalic = false;
			this._fEngrave = false;
			this._fShadow = false;
			this._fScaps = false;
			this._fOutline = false;
			this._fSub = false;
			this._fSuper = false;
			this._superOffset = 0L;
			this._fs = 24L;
			this._font = -1L;
			this._codePage = -1;
			this._cf = -1L;
			this._cb = -1L;
			this._dirChar = DirState.DirLTR;
			this._ul = ULState.ULNone;
			this._strike = StrikeState.StrikeNone;
			this._expand = 0L;
			this._fHidden = false;
			this._lang = -1L;
			this._langFE = -1L;
			this._langCur = -1L;
			this._fontSlot = FontSlot.LOCH;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x000E54A4 File Offset: 0x000E36A4
		internal void SetParaDefaults()
		{
			this._sb = 0L;
			this._sa = 0L;
			this._fi = 0L;
			this._ri = 0L;
			this._li = 0L;
			this._align = HAlign.AlignDefault;
			this._ilvl = 0L;
			this._pnlvl = 0L;
			this._itap = 0L;
			this._ils = -1L;
			this._dirPara = DirState.DirLTR;
			this._cbPara = -1L;
			this._nParaShading = -1L;
			this._cfPara = -1L;
			this._marker = MarkerStyle.MarkerNone;
			this._fContinue = false;
			this._nStartIndex = -1L;
			this._nStartIndexDefault = -1L;
			this._sl = 0L;
			this._slMult = false;
			this._pb = null;
			this._fInTable = false;
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x000E555A File Offset: 0x000E375A
		internal void SetRowDefaults()
		{
			this.RowFormat = null;
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x000E5564 File Offset: 0x000E3764
		internal bool IsEqual(FormatState formatState)
		{
			return this.Bold == formatState.Bold && this.Italic == formatState.Italic && this.Engrave == formatState.Engrave && this.Shadow == formatState.Shadow && this.SCaps == formatState.SCaps && this.Outline == formatState.Outline && this.Super == formatState.Super && this.Sub == formatState.Sub && this.SuperOffset == formatState.SuperOffset && this.FontSize == formatState.FontSize && this.Font == formatState.Font && this.CodePage == formatState.CodePage && this.CF == formatState.CF && this.CB == formatState.CB && this.DirChar == formatState.DirChar && this.UL == formatState.UL && this.Strike == formatState.Strike && this.Expand == formatState.Expand && this.Lang == formatState.Lang && this.LangFE == formatState.LangFE && this.LangCur == formatState.LangCur && this.FontSlot == formatState.FontSlot && this.SB == formatState.SB && this.SA == formatState.SA && this.FI == formatState.FI && this.RI == formatState.RI && this.LI == formatState.LI && this.HAlign == formatState.HAlign && this.ILVL == formatState.ILVL && this.ITAP == formatState.ITAP && this.ILS == formatState.ILS && this.DirPara == formatState.DirPara && this.CFPara == formatState.CFPara && this.CBPara == formatState.CBPara && this.ParaShading == formatState.ParaShading && this.Marker == formatState.Marker && this.IsContinue == formatState.IsContinue && this.StartIndex == formatState.StartIndex && this.StartIndexDefault == formatState.StartIndexDefault && this.SL == formatState.SL && this.SLMult == formatState.SLMult && this.IsInTable == formatState.IsInTable && this.RtfDestination == formatState.RtfDestination && this.IsHidden == formatState.IsHidden && this.UnicodeSkip == formatState.UnicodeSkip;
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x060032E0 RID: 13024 RVA: 0x000E5852 File Offset: 0x000E3A52
		internal static FormatState EmptyFormatState
		{
			get
			{
				if (FormatState._fsEmptyState == null)
				{
					FormatState._fsEmptyState = new FormatState();
					FormatState._fsEmptyState.FontSize = -1L;
				}
				return FormatState._fsEmptyState;
			}
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000E5876 File Offset: 0x000E3A76
		internal string GetBorderAttributeString(ConverterState converterState)
		{
			if (this.HasParaBorder)
			{
				return this.ParaBorder.GetBorderAttributeString(converterState);
			}
			return string.Empty;
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x060032E2 RID: 13026 RVA: 0x000E5892 File Offset: 0x000E3A92
		// (set) Token: 0x060032E3 RID: 13027 RVA: 0x000E589A File Offset: 0x000E3A9A
		internal RtfDestination RtfDestination
		{
			get
			{
				return this._dest;
			}
			set
			{
				this._dest = value;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x060032E4 RID: 13028 RVA: 0x000E58A3 File Offset: 0x000E3AA3
		// (set) Token: 0x060032E5 RID: 13029 RVA: 0x000E58AB File Offset: 0x000E3AAB
		internal bool IsHidden
		{
			get
			{
				return this._fHidden;
			}
			set
			{
				this._fHidden = value;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x060032E6 RID: 13030 RVA: 0x000E58B4 File Offset: 0x000E3AB4
		internal bool IsContentDestination
		{
			get
			{
				return this._dest == RtfDestination.DestNormal || this._dest == RtfDestination.DestFieldResult || this._dest == RtfDestination.DestShapeResult || this._dest == RtfDestination.DestShape || this._dest == RtfDestination.DestListText;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x060032E7 RID: 13031 RVA: 0x000E58E8 File Offset: 0x000E3AE8
		// (set) Token: 0x060032E8 RID: 13032 RVA: 0x000E58F0 File Offset: 0x000E3AF0
		internal bool Bold
		{
			get
			{
				return this._fBold;
			}
			set
			{
				this._fBold = value;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x060032E9 RID: 13033 RVA: 0x000E58F9 File Offset: 0x000E3AF9
		// (set) Token: 0x060032EA RID: 13034 RVA: 0x000E5901 File Offset: 0x000E3B01
		internal bool Italic
		{
			get
			{
				return this._fItalic;
			}
			set
			{
				this._fItalic = value;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x060032EB RID: 13035 RVA: 0x000E590A File Offset: 0x000E3B0A
		// (set) Token: 0x060032EC RID: 13036 RVA: 0x000E5912 File Offset: 0x000E3B12
		internal bool Engrave
		{
			get
			{
				return this._fEngrave;
			}
			set
			{
				this._fEngrave = value;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x060032ED RID: 13037 RVA: 0x000E591B File Offset: 0x000E3B1B
		// (set) Token: 0x060032EE RID: 13038 RVA: 0x000E5923 File Offset: 0x000E3B23
		internal bool Shadow
		{
			get
			{
				return this._fShadow;
			}
			set
			{
				this._fShadow = value;
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x060032EF RID: 13039 RVA: 0x000E592C File Offset: 0x000E3B2C
		// (set) Token: 0x060032F0 RID: 13040 RVA: 0x000E5934 File Offset: 0x000E3B34
		internal bool SCaps
		{
			get
			{
				return this._fScaps;
			}
			set
			{
				this._fScaps = value;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x060032F1 RID: 13041 RVA: 0x000E593D File Offset: 0x000E3B3D
		// (set) Token: 0x060032F2 RID: 13042 RVA: 0x000E5945 File Offset: 0x000E3B45
		internal bool Outline
		{
			get
			{
				return this._fOutline;
			}
			set
			{
				this._fOutline = value;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x060032F3 RID: 13043 RVA: 0x000E594E File Offset: 0x000E3B4E
		// (set) Token: 0x060032F4 RID: 13044 RVA: 0x000E5956 File Offset: 0x000E3B56
		internal bool Sub
		{
			get
			{
				return this._fSub;
			}
			set
			{
				this._fSub = value;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x060032F5 RID: 13045 RVA: 0x000E595F File Offset: 0x000E3B5F
		// (set) Token: 0x060032F6 RID: 13046 RVA: 0x000E5967 File Offset: 0x000E3B67
		internal bool Super
		{
			get
			{
				return this._fSuper;
			}
			set
			{
				this._fSuper = value;
			}
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x000E5970 File Offset: 0x000E3B70
		// (set) Token: 0x060032F8 RID: 13048 RVA: 0x000E5978 File Offset: 0x000E3B78
		internal long SuperOffset
		{
			get
			{
				return this._superOffset;
			}
			set
			{
				this._superOffset = value;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x060032F9 RID: 13049 RVA: 0x000E5981 File Offset: 0x000E3B81
		// (set) Token: 0x060032FA RID: 13050 RVA: 0x000E5989 File Offset: 0x000E3B89
		internal long FontSize
		{
			get
			{
				return this._fs;
			}
			set
			{
				this._fs = value;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x060032FB RID: 13051 RVA: 0x000E5992 File Offset: 0x000E3B92
		// (set) Token: 0x060032FC RID: 13052 RVA: 0x000E599A File Offset: 0x000E3B9A
		internal long Font
		{
			get
			{
				return this._font;
			}
			set
			{
				this._font = value;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x000E59A3 File Offset: 0x000E3BA3
		// (set) Token: 0x060032FE RID: 13054 RVA: 0x000E59AB File Offset: 0x000E3BAB
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

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x000E59B4 File Offset: 0x000E3BB4
		// (set) Token: 0x06003300 RID: 13056 RVA: 0x000E59BC File Offset: 0x000E3BBC
		internal long CF
		{
			get
			{
				return this._cf;
			}
			set
			{
				this._cf = value;
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06003301 RID: 13057 RVA: 0x000E59C5 File Offset: 0x000E3BC5
		// (set) Token: 0x06003302 RID: 13058 RVA: 0x000E59CD File Offset: 0x000E3BCD
		internal long CB
		{
			get
			{
				return this._cb;
			}
			set
			{
				this._cb = value;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x000E59D6 File Offset: 0x000E3BD6
		// (set) Token: 0x06003304 RID: 13060 RVA: 0x000E59DE File Offset: 0x000E3BDE
		internal DirState DirChar
		{
			get
			{
				return this._dirChar;
			}
			set
			{
				this._dirChar = value;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x000E59E7 File Offset: 0x000E3BE7
		// (set) Token: 0x06003306 RID: 13062 RVA: 0x000E59EF File Offset: 0x000E3BEF
		internal ULState UL
		{
			get
			{
				return this._ul;
			}
			set
			{
				this._ul = value;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06003307 RID: 13063 RVA: 0x000E59F8 File Offset: 0x000E3BF8
		// (set) Token: 0x06003308 RID: 13064 RVA: 0x000E5A00 File Offset: 0x000E3C00
		internal StrikeState Strike
		{
			get
			{
				return this._strike;
			}
			set
			{
				this._strike = value;
			}
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x000E5A09 File Offset: 0x000E3C09
		// (set) Token: 0x0600330A RID: 13066 RVA: 0x000E5A11 File Offset: 0x000E3C11
		internal long Expand
		{
			get
			{
				return this._expand;
			}
			set
			{
				this._expand = value;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x0600330B RID: 13067 RVA: 0x000E5A1A File Offset: 0x000E3C1A
		// (set) Token: 0x0600330C RID: 13068 RVA: 0x000E5A22 File Offset: 0x000E3C22
		internal long Lang
		{
			get
			{
				return this._lang;
			}
			set
			{
				this._lang = value;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x0600330D RID: 13069 RVA: 0x000E5A2B File Offset: 0x000E3C2B
		// (set) Token: 0x0600330E RID: 13070 RVA: 0x000E5A33 File Offset: 0x000E3C33
		internal long LangFE
		{
			get
			{
				return this._langFE;
			}
			set
			{
				this._langFE = value;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x0600330F RID: 13071 RVA: 0x000E5A3C File Offset: 0x000E3C3C
		// (set) Token: 0x06003310 RID: 13072 RVA: 0x000E5A44 File Offset: 0x000E3C44
		internal long LangCur
		{
			get
			{
				return this._langCur;
			}
			set
			{
				this._langCur = value;
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06003311 RID: 13073 RVA: 0x000E5A4D File Offset: 0x000E3C4D
		// (set) Token: 0x06003312 RID: 13074 RVA: 0x000E5A55 File Offset: 0x000E3C55
		internal FontSlot FontSlot
		{
			get
			{
				return this._fontSlot;
			}
			set
			{
				this._fontSlot = value;
			}
		}

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06003313 RID: 13075 RVA: 0x000E5A5E File Offset: 0x000E3C5E
		// (set) Token: 0x06003314 RID: 13076 RVA: 0x000E5A66 File Offset: 0x000E3C66
		internal long SB
		{
			get
			{
				return this._sb;
			}
			set
			{
				this._sb = value;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06003315 RID: 13077 RVA: 0x000E5A6F File Offset: 0x000E3C6F
		// (set) Token: 0x06003316 RID: 13078 RVA: 0x000E5A77 File Offset: 0x000E3C77
		internal long SA
		{
			get
			{
				return this._sa;
			}
			set
			{
				this._sa = value;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x06003317 RID: 13079 RVA: 0x000E5A80 File Offset: 0x000E3C80
		// (set) Token: 0x06003318 RID: 13080 RVA: 0x000E5A88 File Offset: 0x000E3C88
		internal long FI
		{
			get
			{
				return this._fi;
			}
			set
			{
				this._fi = value;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06003319 RID: 13081 RVA: 0x000E5A91 File Offset: 0x000E3C91
		// (set) Token: 0x0600331A RID: 13082 RVA: 0x000E5A99 File Offset: 0x000E3C99
		internal long RI
		{
			get
			{
				return this._ri;
			}
			set
			{
				this._ri = value;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x0600331B RID: 13083 RVA: 0x000E5AA2 File Offset: 0x000E3CA2
		// (set) Token: 0x0600331C RID: 13084 RVA: 0x000E5AAA File Offset: 0x000E3CAA
		internal long LI
		{
			get
			{
				return this._li;
			}
			set
			{
				this._li = value;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x0600331D RID: 13085 RVA: 0x000E5AB3 File Offset: 0x000E3CB3
		// (set) Token: 0x0600331E RID: 13086 RVA: 0x000E5ABB File Offset: 0x000E3CBB
		internal HAlign HAlign
		{
			get
			{
				return this._align;
			}
			set
			{
				this._align = value;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x0600331F RID: 13087 RVA: 0x000E5AC4 File Offset: 0x000E3CC4
		// (set) Token: 0x06003320 RID: 13088 RVA: 0x000E5ACC File Offset: 0x000E3CCC
		internal long ILVL
		{
			get
			{
				return this._ilvl;
			}
			set
			{
				if (value >= 0L && value <= 32L)
				{
					this._ilvl = value;
				}
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06003321 RID: 13089 RVA: 0x000E5AE0 File Offset: 0x000E3CE0
		// (set) Token: 0x06003322 RID: 13090 RVA: 0x000E5AE8 File Offset: 0x000E3CE8
		internal long PNLVL
		{
			get
			{
				return this._pnlvl;
			}
			set
			{
				this._pnlvl = value;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06003323 RID: 13091 RVA: 0x000E5AF1 File Offset: 0x000E3CF1
		// (set) Token: 0x06003324 RID: 13092 RVA: 0x000E5AF9 File Offset: 0x000E3CF9
		internal long ITAP
		{
			get
			{
				return this._itap;
			}
			set
			{
				if (value >= 0L && value <= 32L)
				{
					this._itap = value;
				}
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06003325 RID: 13093 RVA: 0x000E5B0D File Offset: 0x000E3D0D
		// (set) Token: 0x06003326 RID: 13094 RVA: 0x000E5B15 File Offset: 0x000E3D15
		internal long ILS
		{
			get
			{
				return this._ils;
			}
			set
			{
				this._ils = value;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x06003327 RID: 13095 RVA: 0x000E5B1E File Offset: 0x000E3D1E
		// (set) Token: 0x06003328 RID: 13096 RVA: 0x000E5B26 File Offset: 0x000E3D26
		internal DirState DirPara
		{
			get
			{
				return this._dirPara;
			}
			set
			{
				this._dirPara = value;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x06003329 RID: 13097 RVA: 0x000E5B2F File Offset: 0x000E3D2F
		// (set) Token: 0x0600332A RID: 13098 RVA: 0x000E5B37 File Offset: 0x000E3D37
		internal long CFPara
		{
			get
			{
				return this._cfPara;
			}
			set
			{
				this._cfPara = value;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x0600332B RID: 13099 RVA: 0x000E5B40 File Offset: 0x000E3D40
		// (set) Token: 0x0600332C RID: 13100 RVA: 0x000E5B48 File Offset: 0x000E3D48
		internal long CBPara
		{
			get
			{
				return this._cbPara;
			}
			set
			{
				this._cbPara = value;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x0600332D RID: 13101 RVA: 0x000E5B51 File Offset: 0x000E3D51
		// (set) Token: 0x0600332E RID: 13102 RVA: 0x000E5B59 File Offset: 0x000E3D59
		internal long ParaShading
		{
			get
			{
				return this._nParaShading;
			}
			set
			{
				this._nParaShading = Validators.MakeValidShading(value);
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x0600332F RID: 13103 RVA: 0x000E5B67 File Offset: 0x000E3D67
		// (set) Token: 0x06003330 RID: 13104 RVA: 0x000E5B6F File Offset: 0x000E3D6F
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

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06003331 RID: 13105 RVA: 0x000E5B78 File Offset: 0x000E3D78
		// (set) Token: 0x06003332 RID: 13106 RVA: 0x000E5B80 File Offset: 0x000E3D80
		internal bool IsContinue
		{
			get
			{
				return this._fContinue;
			}
			set
			{
				this._fContinue = value;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06003333 RID: 13107 RVA: 0x000E5B89 File Offset: 0x000E3D89
		// (set) Token: 0x06003334 RID: 13108 RVA: 0x000E5B91 File Offset: 0x000E3D91
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

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06003335 RID: 13109 RVA: 0x000E5B9A File Offset: 0x000E3D9A
		// (set) Token: 0x06003336 RID: 13110 RVA: 0x000E5BA2 File Offset: 0x000E3DA2
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

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06003337 RID: 13111 RVA: 0x000E5BAB File Offset: 0x000E3DAB
		// (set) Token: 0x06003338 RID: 13112 RVA: 0x000E5BB3 File Offset: 0x000E3DB3
		internal long SL
		{
			get
			{
				return this._sl;
			}
			set
			{
				this._sl = value;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06003339 RID: 13113 RVA: 0x000E5BBC File Offset: 0x000E3DBC
		// (set) Token: 0x0600333A RID: 13114 RVA: 0x000E5BC4 File Offset: 0x000E3DC4
		internal bool SLMult
		{
			get
			{
				return this._slMult;
			}
			set
			{
				this._slMult = value;
			}
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x0600333B RID: 13115 RVA: 0x000E5BCD File Offset: 0x000E3DCD
		// (set) Token: 0x0600333C RID: 13116 RVA: 0x000E5BD5 File Offset: 0x000E3DD5
		internal bool IsInTable
		{
			get
			{
				return this._fInTable;
			}
			set
			{
				this._fInTable = value;
			}
		}

		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x000E5BDE File Offset: 0x000E3DDE
		internal long TableLevel
		{
			get
			{
				if (!this._fInTable && this._itap <= 0L)
				{
					return 0L;
				}
				if (this._itap <= 0L)
				{
					return 1L;
				}
				return this._itap;
			}
		}

		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x000E5C08 File Offset: 0x000E3E08
		internal long ListLevel
		{
			get
			{
				if (this._ils >= 0L || this._ilvl > 0L)
				{
					if (this._ilvl <= 0L)
					{
						return 1L;
					}
					return this._ilvl + 1L;
				}
				else
				{
					if (this.PNLVL > 0L)
					{
						return this.PNLVL;
					}
					if (this._marker != MarkerStyle.MarkerNone)
					{
						return 1L;
					}
					return 0L;
				}
			}
		}

		// Token: 0x17000D01 RID: 3329
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x000E5C5F File Offset: 0x000E3E5F
		// (set) Token: 0x06003340 RID: 13120 RVA: 0x000E5C67 File Offset: 0x000E3E67
		internal int UnicodeSkip
		{
			get
			{
				return this._stateSkip;
			}
			set
			{
				if (value >= 0 && value < 65535)
				{
					this._stateSkip = value;
				}
			}
		}

		// Token: 0x17000D02 RID: 3330
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x000E5C7C File Offset: 0x000E3E7C
		// (set) Token: 0x06003342 RID: 13122 RVA: 0x000E5C97 File Offset: 0x000E3E97
		internal RowFormat RowFormat
		{
			get
			{
				if (this._rowFormat == null)
				{
					this._rowFormat = new RowFormat();
				}
				return this._rowFormat;
			}
			set
			{
				this._rowFormat = value;
			}
		}

		// Token: 0x17000D03 RID: 3331
		// (get) Token: 0x06003343 RID: 13123 RVA: 0x000E5CA0 File Offset: 0x000E3EA0
		internal bool HasRowFormat
		{
			get
			{
				return this._rowFormat != null;
			}
		}

		// Token: 0x17000D04 RID: 3332
		// (get) Token: 0x06003344 RID: 13124 RVA: 0x000E5CAB File Offset: 0x000E3EAB
		internal ParaBorder ParaBorder
		{
			get
			{
				if (this._pb == null)
				{
					this._pb = new ParaBorder();
				}
				return this._pb;
			}
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x06003345 RID: 13125 RVA: 0x000E5CC6 File Offset: 0x000E3EC6
		internal bool HasParaBorder
		{
			get
			{
				return this._pb != null && !this._pb.IsNone;
			}
		}

		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06003346 RID: 13126 RVA: 0x000E5CE0 File Offset: 0x000E3EE0
		// (set) Token: 0x06003347 RID: 13127 RVA: 0x000E5CE8 File Offset: 0x000E3EE8
		internal RtfImageFormat ImageFormat
		{
			get
			{
				return this._imageFormat;
			}
			set
			{
				this._imageFormat = value;
			}
		}

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x000E5CF1 File Offset: 0x000E3EF1
		// (set) Token: 0x06003349 RID: 13129 RVA: 0x000E5CF9 File Offset: 0x000E3EF9
		internal string ImageSource
		{
			get
			{
				return this._imageSource;
			}
			set
			{
				this._imageSource = value;
			}
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x0600334A RID: 13130 RVA: 0x000E5D02 File Offset: 0x000E3F02
		// (set) Token: 0x0600334B RID: 13131 RVA: 0x000E5D0A File Offset: 0x000E3F0A
		internal double ImageWidth
		{
			get
			{
				return this._imageWidth;
			}
			set
			{
				this._imageWidth = value;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x0600334C RID: 13132 RVA: 0x000E5D13 File Offset: 0x000E3F13
		// (set) Token: 0x0600334D RID: 13133 RVA: 0x000E5D1B File Offset: 0x000E3F1B
		internal double ImageHeight
		{
			get
			{
				return this._imageHeight;
			}
			set
			{
				this._imageHeight = value;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x0600334E RID: 13134 RVA: 0x000E5D24 File Offset: 0x000E3F24
		// (set) Token: 0x0600334F RID: 13135 RVA: 0x000E5D2C File Offset: 0x000E3F2C
		internal double ImageBaselineOffset
		{
			get
			{
				return this._imageBaselineOffset;
			}
			set
			{
				this._imageBaselineOffset = value;
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06003350 RID: 13136 RVA: 0x000E5D35 File Offset: 0x000E3F35
		// (set) Token: 0x06003351 RID: 13137 RVA: 0x000E5D3D File Offset: 0x000E3F3D
		internal bool IncludeImageBaselineOffset
		{
			get
			{
				return this._isIncludeImageBaselineOffset;
			}
			set
			{
				this._isIncludeImageBaselineOffset = value;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06003352 RID: 13138 RVA: 0x000E5D46 File Offset: 0x000E3F46
		// (set) Token: 0x06003353 RID: 13139 RVA: 0x000E5D4E File Offset: 0x000E3F4E
		internal double ImageScaleWidth
		{
			get
			{
				return this._imageScaleWidth;
			}
			set
			{
				this._imageScaleWidth = value;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06003354 RID: 13140 RVA: 0x000E5D57 File Offset: 0x000E3F57
		// (set) Token: 0x06003355 RID: 13141 RVA: 0x000E5D5F File Offset: 0x000E3F5F
		internal double ImageScaleHeight
		{
			get
			{
				return this._imageScaleHeight;
			}
			set
			{
				this._imageScaleHeight = value;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06003356 RID: 13142 RVA: 0x000E5D68 File Offset: 0x000E3F68
		// (set) Token: 0x06003357 RID: 13143 RVA: 0x000E5D70 File Offset: 0x000E3F70
		internal bool IsImageDataBinary
		{
			get
			{
				return this._isImageDataBinary;
			}
			set
			{
				this._isImageDataBinary = value;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003358 RID: 13144 RVA: 0x000E5D79 File Offset: 0x000E3F79
		// (set) Token: 0x06003359 RID: 13145 RVA: 0x000E5D81 File Offset: 0x000E3F81
		internal string ImageStretch
		{
			get
			{
				return this._imageStretch;
			}
			set
			{
				this._imageStretch = value;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x0600335A RID: 13146 RVA: 0x000E5D8A File Offset: 0x000E3F8A
		// (set) Token: 0x0600335B RID: 13147 RVA: 0x000E5D92 File Offset: 0x000E3F92
		internal string ImageStretchDirection
		{
			get
			{
				return this._imageStretchDirection;
			}
			set
			{
				this._imageStretchDirection = value;
			}
		}

		// Token: 0x04002439 RID: 9273
		private RtfDestination _dest;

		// Token: 0x0400243A RID: 9274
		private bool _fBold;

		// Token: 0x0400243B RID: 9275
		private bool _fItalic;

		// Token: 0x0400243C RID: 9276
		private bool _fSuper;

		// Token: 0x0400243D RID: 9277
		private bool _fSub;

		// Token: 0x0400243E RID: 9278
		private bool _fOutline;

		// Token: 0x0400243F RID: 9279
		private bool _fEngrave;

		// Token: 0x04002440 RID: 9280
		private bool _fShadow;

		// Token: 0x04002441 RID: 9281
		private bool _fScaps;

		// Token: 0x04002442 RID: 9282
		private long _fs;

		// Token: 0x04002443 RID: 9283
		private long _font;

		// Token: 0x04002444 RID: 9284
		private int _codePage;

		// Token: 0x04002445 RID: 9285
		private long _superOffset;

		// Token: 0x04002446 RID: 9286
		private long _cf;

		// Token: 0x04002447 RID: 9287
		private long _cb;

		// Token: 0x04002448 RID: 9288
		private DirState _dirChar;

		// Token: 0x04002449 RID: 9289
		private ULState _ul;

		// Token: 0x0400244A RID: 9290
		private StrikeState _strike;

		// Token: 0x0400244B RID: 9291
		private long _expand;

		// Token: 0x0400244C RID: 9292
		private long _lang;

		// Token: 0x0400244D RID: 9293
		private long _langFE;

		// Token: 0x0400244E RID: 9294
		private long _langCur;

		// Token: 0x0400244F RID: 9295
		private FontSlot _fontSlot;

		// Token: 0x04002450 RID: 9296
		private long _sa;

		// Token: 0x04002451 RID: 9297
		private long _sb;

		// Token: 0x04002452 RID: 9298
		private long _li;

		// Token: 0x04002453 RID: 9299
		private long _ri;

		// Token: 0x04002454 RID: 9300
		private long _fi;

		// Token: 0x04002455 RID: 9301
		private HAlign _align;

		// Token: 0x04002456 RID: 9302
		private long _ils;

		// Token: 0x04002457 RID: 9303
		private long _ilvl;

		// Token: 0x04002458 RID: 9304
		private long _pnlvl;

		// Token: 0x04002459 RID: 9305
		private long _itap;

		// Token: 0x0400245A RID: 9306
		private DirState _dirPara;

		// Token: 0x0400245B RID: 9307
		private long _cfPara;

		// Token: 0x0400245C RID: 9308
		private long _cbPara;

		// Token: 0x0400245D RID: 9309
		private long _nParaShading;

		// Token: 0x0400245E RID: 9310
		private MarkerStyle _marker;

		// Token: 0x0400245F RID: 9311
		private bool _fContinue;

		// Token: 0x04002460 RID: 9312
		private long _nStartIndex;

		// Token: 0x04002461 RID: 9313
		private long _nStartIndexDefault;

		// Token: 0x04002462 RID: 9314
		private long _sl;

		// Token: 0x04002463 RID: 9315
		private bool _slMult;

		// Token: 0x04002464 RID: 9316
		private ParaBorder _pb;

		// Token: 0x04002465 RID: 9317
		private bool _fInTable;

		// Token: 0x04002466 RID: 9318
		private bool _fHidden;

		// Token: 0x04002467 RID: 9319
		private int _stateSkip;

		// Token: 0x04002468 RID: 9320
		private RowFormat _rowFormat;

		// Token: 0x04002469 RID: 9321
		private static FormatState _fsEmptyState;

		// Token: 0x0400246A RID: 9322
		private RtfImageFormat _imageFormat;

		// Token: 0x0400246B RID: 9323
		private string _imageSource;

		// Token: 0x0400246C RID: 9324
		private double _imageWidth;

		// Token: 0x0400246D RID: 9325
		private double _imageHeight;

		// Token: 0x0400246E RID: 9326
		private double _imageBaselineOffset;

		// Token: 0x0400246F RID: 9327
		private bool _isIncludeImageBaselineOffset;

		// Token: 0x04002470 RID: 9328
		private double _imageScaleWidth;

		// Token: 0x04002471 RID: 9329
		private double _imageScaleHeight;

		// Token: 0x04002472 RID: 9330
		private bool _isImageDataBinary;

		// Token: 0x04002473 RID: 9331
		private string _imageStretch;

		// Token: 0x04002474 RID: 9332
		private string _imageStretchDirection;

		// Token: 0x04002475 RID: 9333
		private const int MAX_LIST_DEPTH = 32;

		// Token: 0x04002476 RID: 9334
		private const int MAX_TABLE_DEPTH = 32;
	}
}
