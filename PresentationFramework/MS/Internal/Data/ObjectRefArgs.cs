using System;

namespace MS.Internal.Data
{
	// Token: 0x02000732 RID: 1842
	internal class ObjectRefArgs
	{
		// Token: 0x17001C16 RID: 7190
		// (get) Token: 0x060075EB RID: 30187 RVA: 0x0021A37E File Offset: 0x0021857E
		// (set) Token: 0x060075EC RID: 30188 RVA: 0x0021A386 File Offset: 0x00218586
		internal bool IsTracing { get; set; }

		// Token: 0x17001C17 RID: 7191
		// (get) Token: 0x060075ED RID: 30189 RVA: 0x0021A38F File Offset: 0x0021858F
		// (set) Token: 0x060075EE RID: 30190 RVA: 0x0021A397 File Offset: 0x00218597
		internal bool ResolveNamesInTemplate { get; set; }

		// Token: 0x17001C18 RID: 7192
		// (get) Token: 0x060075EF RID: 30191 RVA: 0x0021A3A0 File Offset: 0x002185A0
		// (set) Token: 0x060075F0 RID: 30192 RVA: 0x0021A3A8 File Offset: 0x002185A8
		internal bool NameResolvedInOuterScope { get; set; }
	}
}
