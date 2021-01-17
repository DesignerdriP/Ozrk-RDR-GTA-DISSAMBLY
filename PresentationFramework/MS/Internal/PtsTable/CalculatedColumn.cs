using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsTable
{
	// Token: 0x0200060A RID: 1546
	internal struct CalculatedColumn
	{
		// Token: 0x0600670B RID: 26379 RVA: 0x001CD5CC File Offset: 0x001CB7CC
		internal void ValidateAuto(double durMinWidth, double durMaxWidth)
		{
			this._durMinWidth = durMinWidth;
			this._durMaxWidth = durMaxWidth;
			this.SetFlags(true, CalculatedColumn.Flags.ValidAutofit);
		}

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x0600670C RID: 26380 RVA: 0x001CD5E4 File Offset: 0x001CB7E4
		internal int PtsWidthChanged
		{
			get
			{
				return PTS.FromBoolean(!this.CheckFlags(CalculatedColumn.Flags.ValidWidth));
			}
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x0600670D RID: 26381 RVA: 0x001CD5F5 File Offset: 0x001CB7F5
		internal double DurMinWidth
		{
			get
			{
				return this._durMinWidth;
			}
		}

		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x0600670E RID: 26382 RVA: 0x001CD5FD File Offset: 0x001CB7FD
		internal double DurMaxWidth
		{
			get
			{
				return this._durMaxWidth;
			}
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x0600670F RID: 26383 RVA: 0x001CD605 File Offset: 0x001CB805
		// (set) Token: 0x06006710 RID: 26384 RVA: 0x001CD60D File Offset: 0x001CB80D
		internal GridLength UserWidth
		{
			get
			{
				return this._userWidth;
			}
			set
			{
				if (this._userWidth != value)
				{
					this.SetFlags(false, CalculatedColumn.Flags.ValidAutofit);
				}
				this._userWidth = value;
			}
		}

		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x06006711 RID: 26385 RVA: 0x001CD62C File Offset: 0x001CB82C
		// (set) Token: 0x06006712 RID: 26386 RVA: 0x001CD634 File Offset: 0x001CB834
		internal double DurWidth
		{
			get
			{
				return this._durWidth;
			}
			set
			{
				if (!DoubleUtil.AreClose(this._durWidth, value))
				{
					this.SetFlags(false, CalculatedColumn.Flags.ValidWidth);
				}
				this._durWidth = value;
			}
		}

		// Token: 0x170018E4 RID: 6372
		// (get) Token: 0x06006713 RID: 26387 RVA: 0x001CD653 File Offset: 0x001CB853
		// (set) Token: 0x06006714 RID: 26388 RVA: 0x001CD65B File Offset: 0x001CB85B
		internal double UrOffset
		{
			get
			{
				return this._urOffset;
			}
			set
			{
				this._urOffset = value;
			}
		}

		// Token: 0x06006715 RID: 26389 RVA: 0x001CD664 File Offset: 0x001CB864
		private void SetFlags(bool value, CalculatedColumn.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x06006716 RID: 26390 RVA: 0x001CD682 File Offset: 0x001CB882
		private bool CheckFlags(CalculatedColumn.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x04003343 RID: 13123
		private GridLength _userWidth;

		// Token: 0x04003344 RID: 13124
		private double _durWidth;

		// Token: 0x04003345 RID: 13125
		private double _durMinWidth;

		// Token: 0x04003346 RID: 13126
		private double _durMaxWidth;

		// Token: 0x04003347 RID: 13127
		private double _urOffset;

		// Token: 0x04003348 RID: 13128
		private CalculatedColumn.Flags _flags;

		// Token: 0x02000A1A RID: 2586
		[Flags]
		private enum Flags
		{
			// Token: 0x040046F0 RID: 18160
			ValidWidth = 1,
			// Token: 0x040046F1 RID: 18161
			ValidAutofit = 2
		}
	}
}
