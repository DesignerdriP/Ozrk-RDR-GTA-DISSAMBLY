using System;
using System.Windows.Controls;

namespace MS.Internal.Controls
{
	// Token: 0x02000761 RID: 1889
	internal class HeaderedContentModelTreeEnumerator : ModelTreeEnumerator
	{
		// Token: 0x06007839 RID: 30777 RVA: 0x00223FBA File Offset: 0x002221BA
		internal HeaderedContentModelTreeEnumerator(HeaderedContentControl headeredContentControl, object content, object header) : base(header)
		{
			this._owner = headeredContentControl;
			this._content = content;
		}

		// Token: 0x17001C7F RID: 7295
		// (get) Token: 0x0600783A RID: 30778 RVA: 0x00223FD1 File Offset: 0x002221D1
		protected override object Current
		{
			get
			{
				if (base.Index == 1 && this._content != null)
				{
					return this._content;
				}
				return base.Current;
			}
		}

		// Token: 0x0600783B RID: 30779 RVA: 0x00223FF4 File Offset: 0x002221F4
		protected override bool MoveNext()
		{
			if (this._content != null)
			{
				if (base.Index == 0)
				{
					int index = base.Index;
					base.Index = index + 1;
					base.VerifyUnchanged();
					return true;
				}
				if (base.Index == 1)
				{
					int index = base.Index;
					base.Index = index + 1;
					return false;
				}
			}
			return base.MoveNext();
		}

		// Token: 0x17001C80 RID: 7296
		// (get) Token: 0x0600783C RID: 30780 RVA: 0x0022404C File Offset: 0x0022224C
		protected override bool IsUnchanged
		{
			get
			{
				object content = base.Content;
				return content == this._owner.Header && this._content == this._owner.Content;
			}
		}

		// Token: 0x040038EA RID: 14570
		private HeaderedContentControl _owner;

		// Token: 0x040038EB RID: 14571
		private object _content;
	}
}
