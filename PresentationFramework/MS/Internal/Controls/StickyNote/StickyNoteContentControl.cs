using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x0200076A RID: 1898
	internal abstract class StickyNoteContentControl
	{
		// Token: 0x06007868 RID: 30824 RVA: 0x00224F4B File Offset: 0x0022314B
		protected StickyNoteContentControl(FrameworkElement innerControl)
		{
			this.SetInnerControl(innerControl);
		}

		// Token: 0x06007869 RID: 30825 RVA: 0x0000326D File Offset: 0x0000146D
		private StickyNoteContentControl()
		{
		}

		// Token: 0x0600786A RID: 30826
		public abstract void Save(XmlNode node);

		// Token: 0x0600786B RID: 30827
		public abstract void Load(XmlNode node);

		// Token: 0x0600786C RID: 30828
		public abstract void Clear();

		// Token: 0x17001C88 RID: 7304
		// (get) Token: 0x0600786D RID: 30829
		public abstract bool IsEmpty { get; }

		// Token: 0x17001C89 RID: 7305
		// (get) Token: 0x0600786E RID: 30830
		public abstract StickyNoteType Type { get; }

		// Token: 0x17001C8A RID: 7306
		// (get) Token: 0x0600786F RID: 30831 RVA: 0x00224F5A File Offset: 0x0022315A
		public FrameworkElement InnerControl
		{
			get
			{
				return this._innerControl;
			}
		}

		// Token: 0x06007870 RID: 30832 RVA: 0x00224F62 File Offset: 0x00223162
		protected void SetInnerControl(FrameworkElement innerControl)
		{
			this._innerControl = innerControl;
		}

		// Token: 0x04003917 RID: 14615
		protected FrameworkElement _innerControl;

		// Token: 0x04003918 RID: 14616
		protected const long MaxBufferSize = 1610612733L;
	}
}
