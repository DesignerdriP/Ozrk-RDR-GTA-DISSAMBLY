﻿using System;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200068D RID: 1677
	internal enum CMS_ASSEMBLY_DEPLOYMENT_FLAG
	{
		// Token: 0x040021E3 RID: 8675
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_BEFORE_APPLICATION_STARTUP = 4,
		// Token: 0x040021E4 RID: 8676
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_RUN_AFTER_INSTALL = 16,
		// Token: 0x040021E5 RID: 8677
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_INSTALL = 32,
		// Token: 0x040021E6 RID: 8678
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_TRUST_URL_PARAMETERS = 64,
		// Token: 0x040021E7 RID: 8679
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_DISALLOW_URL_ACTIVATION = 128,
		// Token: 0x040021E8 RID: 8680
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_MAP_FILE_EXTENSIONS = 256,
		// Token: 0x040021E9 RID: 8681
		CMS_ASSEMBLY_DEPLOYMENT_FLAG_CREATE_DESKTOP_SHORTCUT = 512
	}
}
