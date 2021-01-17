using System;
using System.Collections;

namespace System.Windows.Markup
{
	// Token: 0x02000226 RID: 550
	internal class ParserStack : ArrayList
	{
		// Token: 0x06002204 RID: 8708 RVA: 0x000AA000 File Offset: 0x000A8200
		internal ParserStack()
		{
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000AA008 File Offset: 0x000A8208
		private ParserStack(ICollection collection) : base(collection)
		{
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000AA011 File Offset: 0x000A8211
		public void Push(object o)
		{
			this.Add(o);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000AA01C File Offset: 0x000A821C
		public object Pop()
		{
			object result = this[this.Count - 1];
			this.RemoveAt(this.Count - 1);
			return result;
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000AA047 File Offset: 0x000A8247
		public object Peek()
		{
			return this[this.Count - 1];
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000AA057 File Offset: 0x000A8257
		public override object Clone()
		{
			return new ParserStack(this);
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x0600220A RID: 8714 RVA: 0x000AA05F File Offset: 0x000A825F
		internal object CurrentContext
		{
			get
			{
				if (this.Count <= 0)
				{
					return null;
				}
				return this[this.Count - 1];
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x000AA07A File Offset: 0x000A827A
		internal object ParentContext
		{
			get
			{
				if (this.Count <= 1)
				{
					return null;
				}
				return this[this.Count - 2];
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x0600220C RID: 8716 RVA: 0x000AA095 File Offset: 0x000A8295
		internal object GrandParentContext
		{
			get
			{
				if (this.Count <= 2)
				{
					return null;
				}
				return this[this.Count - 3];
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x000AA0B0 File Offset: 0x000A82B0
		internal object GreatGrandParentContext
		{
			get
			{
				if (this.Count <= 3)
				{
					return null;
				}
				return this[this.Count - 4];
			}
		}
	}
}
