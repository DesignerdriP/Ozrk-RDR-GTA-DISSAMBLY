using System;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000760 RID: 1888
	internal class ContentModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x06007837 RID: 30775 RVA: 0x00223F95 File Offset: 0x00222195
		internal ContentModelTreeEnumerator(ContentControl contentControl, object content) : base(content)
		{
			this._owner = contentControl;
		}

		// Token: 0x17001C7E RID: 7294
		// (get) Token: 0x06007838 RID: 30776 RVA: 0x00223FA5 File Offset: 0x002221A5
		protected override bool IsUnchanged
		{
			get
			{
				return base.Content == this._owner.Content;
			}
		}

		// Token: 0x040038E9 RID: 14569
		private ContentControl _owner;
	}
}
