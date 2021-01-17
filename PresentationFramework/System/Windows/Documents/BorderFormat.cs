using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x020003BC RID: 956
	internal class BorderFormat
	{
		// Token: 0x0600335D RID: 13149 RVA: 0x000E5D9B File Offset: 0x000E3F9B
		internal BorderFormat()
		{
			this.SetDefaults();
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x000E5DA9 File Offset: 0x000E3FA9
		internal BorderFormat(BorderFormat cb)
		{
			this.CF = cb.CF;
			this.Width = cb.Width;
			this.Type = cb.Type;
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x0600335F RID: 13151 RVA: 0x000E5DD5 File Offset: 0x000E3FD5
		// (set) Token: 0x06003360 RID: 13152 RVA: 0x000E5DDD File Offset: 0x000E3FDD
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

		// Token: 0x17000D12 RID: 3346
		// (get) Token: 0x06003361 RID: 13153 RVA: 0x000E5DE6 File Offset: 0x000E3FE6
		// (set) Token: 0x06003362 RID: 13154 RVA: 0x000E5DEE File Offset: 0x000E3FEE
		internal long Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = Validators.MakeValidBorderWidth(value);
			}
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06003363 RID: 13155 RVA: 0x000E5DFC File Offset: 0x000E3FFC
		internal long EffectiveWidth
		{
			get
			{
				switch (this.Type)
				{
				case BorderType.BorderNone:
					return 0L;
				case BorderType.BorderDouble:
					return this.Width * 2L;
				}
				return this.Width;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003364 RID: 13156 RVA: 0x000E5E37 File Offset: 0x000E4037
		// (set) Token: 0x06003365 RID: 13157 RVA: 0x000E5E3F File Offset: 0x000E403F
		internal BorderType Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003366 RID: 13158 RVA: 0x000E5E48 File Offset: 0x000E4048
		internal bool IsNone
		{
			get
			{
				return this.EffectiveWidth <= 0L || this.Type == BorderType.BorderNone;
			}
		}

		// Token: 0x17000D16 RID: 3350
		// (get) Token: 0x06003367 RID: 13159 RVA: 0x000E5E60 File Offset: 0x000E4060
		internal string RTFEncoding
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.IsNone)
				{
					stringBuilder.Append("\\brdrnone");
				}
				else
				{
					stringBuilder.Append("\\brdrs\\brdrw");
					stringBuilder.Append(this.EffectiveWidth.ToString(CultureInfo.InvariantCulture));
					if (this.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.CF.ToString(CultureInfo.InvariantCulture));
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000D17 RID: 3351
		// (get) Token: 0x06003368 RID: 13160 RVA: 0x000E5EE5 File Offset: 0x000E40E5
		internal static BorderFormat EmptyBorderFormat
		{
			get
			{
				if (BorderFormat._emptyBorderFormat == null)
				{
					BorderFormat._emptyBorderFormat = new BorderFormat();
				}
				return BorderFormat._emptyBorderFormat;
			}
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x000E5EFD File Offset: 0x000E40FD
		internal void SetDefaults()
		{
			this._cf = -1L;
			this._width = 0L;
			this._type = BorderType.BorderNone;
		}

		// Token: 0x0400247B RID: 9339
		private long _cf;

		// Token: 0x0400247C RID: 9340
		private long _width;

		// Token: 0x0400247D RID: 9341
		private BorderType _type;

		// Token: 0x0400247E RID: 9342
		private static BorderFormat _emptyBorderFormat;
	}
}
