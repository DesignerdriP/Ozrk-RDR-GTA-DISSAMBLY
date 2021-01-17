using System;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200065E RID: 1630
	internal class ContentDescriptor
	{
		// Token: 0x06006C1D RID: 27677 RVA: 0x001F1B6A File Offset: 0x001EFD6A
		internal ContentDescriptor(bool hasIndexableContent, bool isInline, string contentProp, string titleProp)
		{
			this.HasIndexableContent = hasIndexableContent;
			this.IsInline = isInline;
			this.ContentProp = contentProp;
			this.TitleProp = titleProp;
		}

		// Token: 0x06006C1E RID: 27678 RVA: 0x001F1B8F File Offset: 0x001EFD8F
		internal ContentDescriptor(bool hasIndexableContent)
		{
			this.HasIndexableContent = hasIndexableContent;
			this.IsInline = false;
			this.ContentProp = null;
			this.TitleProp = null;
		}

		// Token: 0x170019DB RID: 6619
		// (get) Token: 0x06006C1F RID: 27679 RVA: 0x001F1BB3 File Offset: 0x001EFDB3
		// (set) Token: 0x06006C20 RID: 27680 RVA: 0x001F1BBB File Offset: 0x001EFDBB
		internal bool HasIndexableContent
		{
			get
			{
				return this._hasIndexableContent;
			}
			set
			{
				this._hasIndexableContent = value;
			}
		}

		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x06006C21 RID: 27681 RVA: 0x001F1BC4 File Offset: 0x001EFDC4
		// (set) Token: 0x06006C22 RID: 27682 RVA: 0x001F1BCC File Offset: 0x001EFDCC
		internal bool IsInline
		{
			get
			{
				return this._isInline;
			}
			set
			{
				this._isInline = value;
			}
		}

		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x06006C23 RID: 27683 RVA: 0x001F1BD5 File Offset: 0x001EFDD5
		// (set) Token: 0x06006C24 RID: 27684 RVA: 0x001F1BDD File Offset: 0x001EFDDD
		internal string ContentProp
		{
			get
			{
				return this._contentProp;
			}
			set
			{
				this._contentProp = value;
			}
		}

		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x06006C25 RID: 27685 RVA: 0x001F1BE6 File Offset: 0x001EFDE6
		// (set) Token: 0x06006C26 RID: 27686 RVA: 0x001F1BEE File Offset: 0x001EFDEE
		internal string TitleProp
		{
			get
			{
				return this._titleProp;
			}
			set
			{
				this._titleProp = value;
			}
		}

		// Token: 0x0400350F RID: 13583
		internal const string ResourceKeyName = "Dictionary";

		// Token: 0x04003510 RID: 13584
		internal const string ResourceName = "ElementTable";

		// Token: 0x04003511 RID: 13585
		private bool _hasIndexableContent;

		// Token: 0x04003512 RID: 13586
		private bool _isInline;

		// Token: 0x04003513 RID: 13587
		private string _contentProp;

		// Token: 0x04003514 RID: 13588
		private string _titleProp;
	}
}
