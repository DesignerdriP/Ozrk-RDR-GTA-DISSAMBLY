using System;

namespace MS.Internal.Data
{
	// Token: 0x0200073F RID: 1855
	internal struct RBFinger<T>
	{
		// Token: 0x17001C2D RID: 7213
		// (get) Token: 0x06007665 RID: 30309 RVA: 0x0021D554 File Offset: 0x0021B754
		// (set) Token: 0x06007666 RID: 30310 RVA: 0x0021D55C File Offset: 0x0021B75C
		public RBNode<T> Node { get; set; }

		// Token: 0x17001C2E RID: 7214
		// (get) Token: 0x06007667 RID: 30311 RVA: 0x0021D565 File Offset: 0x0021B765
		// (set) Token: 0x06007668 RID: 30312 RVA: 0x0021D56D File Offset: 0x0021B76D
		public int Offset { get; set; }

		// Token: 0x17001C2F RID: 7215
		// (get) Token: 0x06007669 RID: 30313 RVA: 0x0021D576 File Offset: 0x0021B776
		// (set) Token: 0x0600766A RID: 30314 RVA: 0x0021D57E File Offset: 0x0021B77E
		public int Index { get; set; }

		// Token: 0x17001C30 RID: 7216
		// (get) Token: 0x0600766B RID: 30315 RVA: 0x0021D587 File Offset: 0x0021B787
		// (set) Token: 0x0600766C RID: 30316 RVA: 0x0021D58F File Offset: 0x0021B78F
		public bool Found { get; set; }

		// Token: 0x17001C31 RID: 7217
		// (get) Token: 0x0600766D RID: 30317 RVA: 0x0021D598 File Offset: 0x0021B798
		public T Item
		{
			get
			{
				return this.Node.GetItemAt(this.Offset);
			}
		}

		// Token: 0x0600766E RID: 30318 RVA: 0x0021D5AB File Offset: 0x0021B7AB
		public void SetItem(T x)
		{
			this.Node.SetItemAt(this.Offset, x);
		}

		// Token: 0x17001C32 RID: 7218
		// (get) Token: 0x0600766F RID: 30319 RVA: 0x0021D5C0 File Offset: 0x0021B7C0
		public bool IsValid
		{
			get
			{
				return this.Node != null && this.Node.HasData;
			}
		}

		// Token: 0x06007670 RID: 30320 RVA: 0x0021D5D7 File Offset: 0x0021B7D7
		public static RBFinger<T>operator +(RBFinger<T> finger, int delta)
		{
			if (delta >= 0)
			{
				while (delta > 0)
				{
					if (!finger.IsValid)
					{
						break;
					}
					finger = ++finger;
					delta--;
				}
			}
			else
			{
				while (delta < 0 && finger.IsValid)
				{
					finger = --finger;
					delta++;
				}
			}
			return finger;
		}

		// Token: 0x06007671 RID: 30321 RVA: 0x0021D616 File Offset: 0x0021B816
		public static RBFinger<T>operator -(RBFinger<T> finger, int delta)
		{
			return finger + -delta;
		}

		// Token: 0x06007672 RID: 30322 RVA: 0x0021D620 File Offset: 0x0021B820
		public static int operator -(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index - f2.Index;
		}

		// Token: 0x06007673 RID: 30323 RVA: 0x0021D634 File Offset: 0x0021B834
		public static RBFinger<T>operator ++(RBFinger<T> finger)
		{
			finger.Offset++;
			finger.Index++;
			if (finger.Offset == finger.Node.Size)
			{
				finger.Node = finger.Node.GetSuccessor();
				finger.Offset = 0;
			}
			return finger;
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x0021D690 File Offset: 0x0021B890
		public static RBFinger<T>operator --(RBFinger<T> finger)
		{
			finger.Offset--;
			finger.Index--;
			if (finger.Offset < 0)
			{
				finger.Node = finger.Node.GetPredecessor();
				if (finger.Node != null)
				{
					finger.Offset = finger.Node.Size - 1;
				}
			}
			return finger;
		}

		// Token: 0x06007675 RID: 30325 RVA: 0x0021D6F7 File Offset: 0x0021B8F7
		public static bool operator <(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index < f2.Index;
		}

		// Token: 0x06007676 RID: 30326 RVA: 0x0021D709 File Offset: 0x0021B909
		public static bool operator >(RBFinger<T> f1, RBFinger<T> f2)
		{
			return f1.Index > f2.Index;
		}
	}
}
