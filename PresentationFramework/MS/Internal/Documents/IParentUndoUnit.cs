using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006D3 RID: 1747
	internal interface IParentUndoUnit : IUndoUnit
	{
		// Token: 0x060070E7 RID: 28903
		void Clear();

		// Token: 0x060070E8 RID: 28904
		void Open(IParentUndoUnit newUnit);

		// Token: 0x060070E9 RID: 28905
		void Close(UndoCloseAction closeAction);

		// Token: 0x060070EA RID: 28906
		void Close(IParentUndoUnit closingUnit, UndoCloseAction closeAction);

		// Token: 0x060070EB RID: 28907
		void Add(IUndoUnit newUnit);

		// Token: 0x060070EC RID: 28908
		void OnNextAdd();

		// Token: 0x060070ED RID: 28909
		void OnNextDiscard();

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x060070EE RID: 28910
		IUndoUnit LastUnit { get; }

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x060070EF RID: 28911
		IParentUndoUnit OpenedUnit { get; }

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x060070F0 RID: 28912
		// (set) Token: 0x060070F1 RID: 28913
		string Description { get; set; }

		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x060070F2 RID: 28914
		bool Locked { get; }

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x060070F3 RID: 28915
		// (set) Token: 0x060070F4 RID: 28916
		object Container { get; set; }
	}
}
