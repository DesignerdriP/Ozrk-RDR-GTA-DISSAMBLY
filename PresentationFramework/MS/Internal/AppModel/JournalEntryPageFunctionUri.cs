using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078E RID: 1934
	[Serializable]
	internal class JournalEntryPageFunctionUri : JournalEntryPageFunctionSaver, ISerializable
	{
		// Token: 0x06007975 RID: 31093 RVA: 0x00227415 File Offset: 0x00225615
		internal JournalEntryPageFunctionUri(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction, Uri markupUri) : base(jeGroupState, pageFunction)
		{
			this._markupUri = markupUri;
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x00227426 File Offset: 0x00225626
		protected JournalEntryPageFunctionUri(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._markupUri = (Uri)info.GetValue("_markupUri", typeof(Uri));
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x00227450 File Offset: 0x00225650
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_markupUri", this._markupUri);
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x0022746C File Offset: 0x0022566C
		internal override PageFunctionBase ResumePageFunction()
		{
			PageFunctionBase pageFunctionBase = Application.LoadComponent(this._markupUri, true) as PageFunctionBase;
			this.RestoreState(pageFunctionBase);
			return pageFunctionBase;
		}

		// Token: 0x04003984 RID: 14724
		private Uri _markupUri;
	}
}
