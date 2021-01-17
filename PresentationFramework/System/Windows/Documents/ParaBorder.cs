using System;
using System.Globalization;
using System.Text;

namespace System.Windows.Documents
{
	// Token: 0x020003BD RID: 957
	internal class ParaBorder
	{
		// Token: 0x0600336B RID: 13163 RVA: 0x000E5F18 File Offset: 0x000E4118
		internal ParaBorder()
		{
			this.BorderLeft = new BorderFormat();
			this.BorderTop = new BorderFormat();
			this.BorderRight = new BorderFormat();
			this.BorderBottom = new BorderFormat();
			this.BorderAll = new BorderFormat();
			this.Spacing = 0L;
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x000E5F6C File Offset: 0x000E416C
		internal ParaBorder(ParaBorder pb)
		{
			this.BorderLeft = new BorderFormat(pb.BorderLeft);
			this.BorderTop = new BorderFormat(pb.BorderTop);
			this.BorderRight = new BorderFormat(pb.BorderRight);
			this.BorderBottom = new BorderFormat(pb.BorderBottom);
			this.BorderAll = new BorderFormat(pb.BorderAll);
			this.Spacing = pb.Spacing;
		}

		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x0600336D RID: 13165 RVA: 0x000E5FE0 File Offset: 0x000E41E0
		// (set) Token: 0x0600336E RID: 13166 RVA: 0x000E5FE8 File Offset: 0x000E41E8
		internal BorderFormat BorderLeft
		{
			get
			{
				return this._bfLeft;
			}
			set
			{
				this._bfLeft = value;
			}
		}

		// Token: 0x17000D19 RID: 3353
		// (get) Token: 0x0600336F RID: 13167 RVA: 0x000E5FF1 File Offset: 0x000E41F1
		// (set) Token: 0x06003370 RID: 13168 RVA: 0x000E5FF9 File Offset: 0x000E41F9
		internal BorderFormat BorderTop
		{
			get
			{
				return this._bfTop;
			}
			set
			{
				this._bfTop = value;
			}
		}

		// Token: 0x17000D1A RID: 3354
		// (get) Token: 0x06003371 RID: 13169 RVA: 0x000E6002 File Offset: 0x000E4202
		// (set) Token: 0x06003372 RID: 13170 RVA: 0x000E600A File Offset: 0x000E420A
		internal BorderFormat BorderRight
		{
			get
			{
				return this._bfRight;
			}
			set
			{
				this._bfRight = value;
			}
		}

		// Token: 0x17000D1B RID: 3355
		// (get) Token: 0x06003373 RID: 13171 RVA: 0x000E6013 File Offset: 0x000E4213
		// (set) Token: 0x06003374 RID: 13172 RVA: 0x000E601B File Offset: 0x000E421B
		internal BorderFormat BorderBottom
		{
			get
			{
				return this._bfBottom;
			}
			set
			{
				this._bfBottom = value;
			}
		}

		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06003375 RID: 13173 RVA: 0x000E6024 File Offset: 0x000E4224
		// (set) Token: 0x06003376 RID: 13174 RVA: 0x000E602C File Offset: 0x000E422C
		internal BorderFormat BorderAll
		{
			get
			{
				return this._bfAll;
			}
			set
			{
				this._bfAll = value;
			}
		}

		// Token: 0x17000D1D RID: 3357
		// (get) Token: 0x06003377 RID: 13175 RVA: 0x000E6035 File Offset: 0x000E4235
		// (set) Token: 0x06003378 RID: 13176 RVA: 0x000E603D File Offset: 0x000E423D
		internal long Spacing
		{
			get
			{
				return this._nSpacing;
			}
			set
			{
				this._nSpacing = value;
			}
		}

		// Token: 0x17000D1E RID: 3358
		// (get) Token: 0x06003379 RID: 13177 RVA: 0x000E6046 File Offset: 0x000E4246
		// (set) Token: 0x0600337A RID: 13178 RVA: 0x000E6053 File Offset: 0x000E4253
		internal long CF
		{
			get
			{
				return this.BorderLeft.CF;
			}
			set
			{
				this.BorderLeft.CF = value;
				this.BorderTop.CF = value;
				this.BorderRight.CF = value;
				this.BorderBottom.CF = value;
				this.BorderAll.CF = value;
			}
		}

		// Token: 0x17000D1F RID: 3359
		// (get) Token: 0x0600337B RID: 13179 RVA: 0x000E6094 File Offset: 0x000E4294
		internal bool IsNone
		{
			get
			{
				return this.BorderLeft.IsNone && this.BorderTop.IsNone && this.BorderRight.IsNone && this.BorderBottom.IsNone && this.BorderAll.IsNone;
			}
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x000E60E4 File Offset: 0x000E42E4
		internal string GetBorderAttributeString(ConverterState converterState)
		{
			if (this.IsNone)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" BorderThickness=\"");
			if (!this.BorderAll.IsNone)
			{
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderAll.EffectiveWidth));
			}
			else
			{
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderLeft.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderTop.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderRight.EffectiveWidth));
				stringBuilder.Append(",");
				stringBuilder.Append(Converters.TwipToPositiveVisiblePxString((double)this.BorderBottom.EffectiveWidth));
			}
			stringBuilder.Append("\"");
			ColorTableEntry colorTableEntry = null;
			if (this.CF >= 0L)
			{
				colorTableEntry = converterState.ColorTable.EntryAt((int)this.CF);
			}
			if (colorTableEntry != null)
			{
				stringBuilder.Append(" BorderBrush=\"");
				stringBuilder.Append(colorTableEntry.Color.ToString());
				stringBuilder.Append("\"");
			}
			else
			{
				stringBuilder.Append(" BorderBrush=\"#FF000000\"");
			}
			if (this.Spacing != 0L)
			{
				stringBuilder.Append(" Padding=\"");
				stringBuilder.Append(Converters.TwipToPositivePxString((double)this.Spacing));
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x0600337D RID: 13181 RVA: 0x000E6268 File Offset: 0x000E4468
		internal string RTFEncoding
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.IsNone)
				{
					stringBuilder.Append("\\brdrnil");
				}
				else
				{
					stringBuilder.Append("\\brdrl");
					stringBuilder.Append(this.BorderLeft.RTFEncoding);
					if (this.BorderLeft.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderLeft.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrt");
					stringBuilder.Append(this.BorderTop.RTFEncoding);
					if (this.BorderTop.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderTop.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrr");
					stringBuilder.Append(this.BorderRight.RTFEncoding);
					if (this.BorderRight.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderRight.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brdrb");
					stringBuilder.Append(this.BorderBottom.RTFEncoding);
					if (this.BorderBottom.CF >= 0L)
					{
						stringBuilder.Append("\\brdrcf");
						stringBuilder.Append(this.BorderBottom.CF.ToString(CultureInfo.InvariantCulture));
					}
					stringBuilder.Append("\\brsp");
					stringBuilder.Append(this.Spacing.ToString(CultureInfo.InvariantCulture));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0400247F RID: 9343
		private BorderFormat _bfLeft;

		// Token: 0x04002480 RID: 9344
		private BorderFormat _bfTop;

		// Token: 0x04002481 RID: 9345
		private BorderFormat _bfRight;

		// Token: 0x04002482 RID: 9346
		private BorderFormat _bfBottom;

		// Token: 0x04002483 RID: 9347
		private BorderFormat _bfAll;

		// Token: 0x04002484 RID: 9348
		private long _nSpacing;
	}
}
