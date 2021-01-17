using System;
using System.Runtime.Serialization;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000788 RID: 1928
	[Serializable]
	internal class JournalEntryUri : JournalEntry, ISerializable
	{
		// Token: 0x0600794F RID: 31055 RVA: 0x00226F2F File Offset: 0x0022512F
		internal JournalEntryUri(JournalEntryGroupState jeGroupState, Uri uri) : base(jeGroupState, uri)
		{
		}

		// Token: 0x06007950 RID: 31056 RVA: 0x00226F39 File Offset: 0x00225139
		protected JournalEntryUri(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06007951 RID: 31057 RVA: 0x00226F43 File Offset: 0x00225143
		internal override void SaveState(object contentObject)
		{
			Invariant.Assert(base.Source != null, "Can't journal by Uri without a Uri.");
			base.SaveState(contentObject);
		}
	}
}
