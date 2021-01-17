using System;
using System.Collections.Generic;
using System.Windows;

namespace MS.Internal.Ink
{
	// Token: 0x02000686 RID: 1670
	internal abstract class ElementsClipboardData : ClipboardData
	{
		// Token: 0x06006D5D RID: 27997 RVA: 0x001F6682 File Offset: 0x001F4882
		internal ElementsClipboardData()
		{
		}

		// Token: 0x06006D5E RID: 27998 RVA: 0x001F668A File Offset: 0x001F488A
		internal ElementsClipboardData(UIElement[] elements)
		{
			if (elements != null)
			{
				this.ElementList = new List<UIElement>(elements);
			}
		}

		// Token: 0x17001A18 RID: 6680
		// (get) Token: 0x06006D5F RID: 27999 RVA: 0x001F66A1 File Offset: 0x001F48A1
		internal List<UIElement> Elements
		{
			get
			{
				if (this.ElementList != null)
				{
					return this._elementList;
				}
				return new List<UIElement>();
			}
		}

		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x06006D60 RID: 28000 RVA: 0x001F66B7 File Offset: 0x001F48B7
		// (set) Token: 0x06006D61 RID: 28001 RVA: 0x001F66BF File Offset: 0x001F48BF
		protected List<UIElement> ElementList
		{
			get
			{
				return this._elementList;
			}
			set
			{
				this._elementList = value;
			}
		}

		// Token: 0x040035E3 RID: 13795
		private List<UIElement> _elementList;
	}
}
