using System;

namespace System.Windows.Documents
{
	// Token: 0x020003BF RID: 959
	internal class CellWidth
	{
		// Token: 0x0600337E RID: 13182 RVA: 0x000E6420 File Offset: 0x000E4620
		internal CellWidth()
		{
			this.Type = WidthType.WidthAuto;
			this.Value = 0L;
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x000E6437 File Offset: 0x000E4637
		internal CellWidth(CellWidth cw)
		{
			this.Type = cw.Type;
			this.Value = cw.Value;
		}

		// Token: 0x17000D21 RID: 3361
		// (get) Token: 0x06003380 RID: 13184 RVA: 0x000E6457 File Offset: 0x000E4657
		// (set) Token: 0x06003381 RID: 13185 RVA: 0x000E645F File Offset: 0x000E465F
		internal WidthType Type
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

		// Token: 0x17000D22 RID: 3362
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x000E6468 File Offset: 0x000E4668
		// (set) Token: 0x06003383 RID: 13187 RVA: 0x000E6470 File Offset: 0x000E4670
		internal long Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x000E6479 File Offset: 0x000E4679
		internal void SetDefaults()
		{
			this.Type = WidthType.WidthAuto;
			this.Value = 0L;
		}

		// Token: 0x0400248A RID: 9354
		private WidthType _type;

		// Token: 0x0400248B RID: 9355
		private long _value;
	}
}
