using System;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006D2 RID: 1746
	internal interface IIndexedChild<TParent> where TParent : TextElement
	{
		// Token: 0x060070E2 RID: 28898
		void OnEnterParentTree();

		// Token: 0x060070E3 RID: 28899
		void OnExitParentTree();

		// Token: 0x060070E4 RID: 28900
		void OnAfterExitParentTree(TParent parent);

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x060070E5 RID: 28901
		// (set) Token: 0x060070E6 RID: 28902
		int Index { get; set; }
	}
}
