using System;
using System.Collections;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020003C7 RID: 967
	internal class ColorTable : ArrayList
	{
		// Token: 0x06003401 RID: 13313 RVA: 0x000E7989 File Offset: 0x000E5B89
		internal ColorTable() : base(20)
		{
			this._inProgress = false;
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x000E799A File Offset: 0x000E5B9A
		internal Color ColorAt(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return this.EntryAt(index).Color;
			}
			return Color.FromArgb(byte.MaxValue, 0, 0, 0);
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x000E79C4 File Offset: 0x000E5BC4
		internal void FinishColor()
		{
			if (this._inProgress)
			{
				this._inProgress = false;
				return;
			}
			int index = this.AddColor(Color.FromArgb(byte.MaxValue, 0, 0, 0));
			this.EntryAt(index).IsAuto = true;
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000E7A04 File Offset: 0x000E5C04
		internal int AddColor(Color color)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.ColorAt(i) == color)
				{
					return i;
				}
			}
			this.Add(new ColorTableEntry
			{
				Color = color
			});
			return this.Count - 1;
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x000E7A50 File Offset: 0x000E5C50
		internal ColorTableEntry EntryAt(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return (ColorTableEntry)this[index];
			}
			return null;
		}

		// Token: 0x17000D5B RID: 3419
		// (set) Token: 0x06003406 RID: 13318 RVA: 0x000E7A70 File Offset: 0x000E5C70
		internal byte NewRed
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Red = value;
				}
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (set) Token: 0x06003407 RID: 13319 RVA: 0x000E7A90 File Offset: 0x000E5C90
		internal byte NewGreen
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Green = value;
				}
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (set) Token: 0x06003408 RID: 13320 RVA: 0x000E7AB0 File Offset: 0x000E5CB0
		internal byte NewBlue
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Blue = value;
				}
			}
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x000E7AD0 File Offset: 0x000E5CD0
		private ColorTableEntry GetInProgressEntry()
		{
			if (this._inProgress)
			{
				return this.EntryAt(this.Count - 1);
			}
			this._inProgress = true;
			ColorTableEntry colorTableEntry = new ColorTableEntry();
			this.Add(colorTableEntry);
			return colorTableEntry;
		}

		// Token: 0x040024BA RID: 9402
		private bool _inProgress;
	}
}
