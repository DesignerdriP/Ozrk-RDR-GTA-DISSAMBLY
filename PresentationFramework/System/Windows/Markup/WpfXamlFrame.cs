using System;
using System.Xaml;
using MS.Internal.Xaml.Context;

namespace System.Windows.Markup
{
	// Token: 0x0200026F RID: 623
	internal class WpfXamlFrame : XamlFrame
	{
		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060023AE RID: 9134 RVA: 0x000AE70C File Offset: 0x000AC90C
		// (set) Token: 0x060023AF RID: 9135 RVA: 0x000AE714 File Offset: 0x000AC914
		public bool FreezeFreezable { get; set; }

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060023B0 RID: 9136 RVA: 0x000AE71D File Offset: 0x000AC91D
		// (set) Token: 0x060023B1 RID: 9137 RVA: 0x000AE725 File Offset: 0x000AC925
		public XamlMember Property { get; set; }

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060023B2 RID: 9138 RVA: 0x000AE72E File Offset: 0x000AC92E
		// (set) Token: 0x060023B3 RID: 9139 RVA: 0x000AE736 File Offset: 0x000AC936
		public XamlType Type { get; set; }

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060023B4 RID: 9140 RVA: 0x000AE73F File Offset: 0x000AC93F
		// (set) Token: 0x060023B5 RID: 9141 RVA: 0x000AE747 File Offset: 0x000AC947
		public object Instance { get; set; }

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060023B6 RID: 9142 RVA: 0x000AE750 File Offset: 0x000AC950
		// (set) Token: 0x060023B7 RID: 9143 RVA: 0x000AE758 File Offset: 0x000AC958
		public XmlnsDictionary XmlnsDictionary { get; set; }

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060023B8 RID: 9144 RVA: 0x000AE761 File Offset: 0x000AC961
		// (set) Token: 0x060023B9 RID: 9145 RVA: 0x000AE769 File Offset: 0x000AC969
		public bool? XmlSpace { get; set; }

		// Token: 0x060023BA RID: 9146 RVA: 0x000AE774 File Offset: 0x000AC974
		public override void Reset()
		{
			this.Type = null;
			this.Property = null;
			this.Instance = null;
			this.XmlnsDictionary = null;
			this.XmlSpace = null;
			if (this.FreezeFreezable)
			{
				this.FreezeFreezable = false;
			}
		}
	}
}
