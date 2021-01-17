﻿using System;

namespace MS.Internal.Interop
{
	// Token: 0x02000672 RID: 1650
	[Flags]
	internal enum STGM_FLAGS
	{
		// Token: 0x04003586 RID: 13702
		CREATE = 4096,
		// Token: 0x04003587 RID: 13703
		MODE = 4096,
		// Token: 0x04003588 RID: 13704
		READ = 0,
		// Token: 0x04003589 RID: 13705
		WRITE = 1,
		// Token: 0x0400358A RID: 13706
		READWRITE = 2,
		// Token: 0x0400358B RID: 13707
		ACCESS = 3,
		// Token: 0x0400358C RID: 13708
		SHARE_DENY_NONE = 64,
		// Token: 0x0400358D RID: 13709
		SHARE_DENY_READ = 48,
		// Token: 0x0400358E RID: 13710
		SHARE_DENY_WRITE = 32,
		// Token: 0x0400358F RID: 13711
		SHARE_EXCLUSIVE = 16,
		// Token: 0x04003590 RID: 13712
		SHARING = 112
	}
}
