using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006D4 RID: 1748
	internal interface IUndoUnit
	{
		// Token: 0x060070F5 RID: 28917
		void Do();

		// Token: 0x060070F6 RID: 28918
		bool Merge(IUndoUnit unit);
	}
}
