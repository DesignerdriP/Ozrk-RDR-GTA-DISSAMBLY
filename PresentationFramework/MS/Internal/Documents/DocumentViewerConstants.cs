using System;

namespace MS.Internal.Documents
{
	// Token: 0x020006C2 RID: 1730
	internal static class DocumentViewerConstants
	{
		// Token: 0x17001A8C RID: 6796
		// (get) Token: 0x06006FCA RID: 28618 RVA: 0x00201F82 File Offset: 0x00200182
		public static double MinimumZoom
		{
			get
			{
				return 5.0;
			}
		}

		// Token: 0x17001A8D RID: 6797
		// (get) Token: 0x06006FCB RID: 28619 RVA: 0x00201F8D File Offset: 0x0020018D
		public static double MaximumZoom
		{
			get
			{
				return 5000.0;
			}
		}

		// Token: 0x17001A8E RID: 6798
		// (get) Token: 0x06006FCC RID: 28620 RVA: 0x00201F98 File Offset: 0x00200198
		public static double MinimumScale
		{
			get
			{
				return 0.05;
			}
		}

		// Token: 0x17001A8F RID: 6799
		// (get) Token: 0x06006FCD RID: 28621 RVA: 0x00201FA3 File Offset: 0x002001A3
		public static double MinimumThumbnailsScale
		{
			get
			{
				return 0.125;
			}
		}

		// Token: 0x17001A90 RID: 6800
		// (get) Token: 0x06006FCE RID: 28622 RVA: 0x00201FAE File Offset: 0x002001AE
		public static double MaximumScale
		{
			get
			{
				return 50.0;
			}
		}

		// Token: 0x17001A91 RID: 6801
		// (get) Token: 0x06006FCF RID: 28623 RVA: 0x000969C4 File Offset: 0x00094BC4
		public static int MaximumMaxPagesAcross
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x040036D0 RID: 14032
		private const double _minimumZoom = 5.0;

		// Token: 0x040036D1 RID: 14033
		private const double _minimumThumbnailsZoom = 12.5;

		// Token: 0x040036D2 RID: 14034
		private const double _maximumZoom = 5000.0;

		// Token: 0x040036D3 RID: 14035
		private const int _maximumMaxPagesAcross = 32;
	}
}
