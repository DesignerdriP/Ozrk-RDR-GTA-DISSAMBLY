using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000666 RID: 1638
	internal interface IManagedFilter
	{
		// Token: 0x06006C74 RID: 27764
		IFILTER_FLAGS Init(IFILTER_INIT grfFlags, ManagedFullPropSpec[] aAttributes);

		// Token: 0x06006C75 RID: 27765
		ManagedChunk GetChunk();

		// Token: 0x06006C76 RID: 27766
		string GetText(int bufferCharacterCount);

		// Token: 0x06006C77 RID: 27767
		object GetValue();
	}
}
