﻿using System;
using System.Windows;

namespace MS.Internal.Globalization
{
	// Token: 0x020006B8 RID: 1720
	internal class LocalizabilityGroup
	{
		// Token: 0x06006EC9 RID: 28361 RVA: 0x001FDA04 File Offset: 0x001FBC04
		internal LocalizabilityGroup()
		{
			this.Modifiability = (Modifiability)(-1);
			this.Readability = (Readability)(-1);
			this.Category = (LocalizationCategory)(-1);
		}

		// Token: 0x06006ECA RID: 28362 RVA: 0x001FDA24 File Offset: 0x001FBC24
		internal LocalizabilityAttribute Override(LocalizabilityAttribute attribute)
		{
			Modifiability modifiability = attribute.Modifiability;
			Readability readability = attribute.Readability;
			LocalizationCategory category = attribute.Category;
			bool flag = false;
			if (this.Modifiability != (Modifiability)(-1))
			{
				modifiability = this.Modifiability;
				flag = true;
			}
			if (this.Readability != (Readability)(-1))
			{
				readability = this.Readability;
				flag = true;
			}
			if (this.Category != (LocalizationCategory)(-1))
			{
				category = this.Category;
				flag = true;
			}
			if (flag)
			{
				attribute = new LocalizabilityAttribute(category);
				attribute.Modifiability = modifiability;
				attribute.Readability = readability;
			}
			return attribute;
		}

		// Token: 0x04003681 RID: 13953
		private const int InvalidValue = -1;

		// Token: 0x04003682 RID: 13954
		internal Modifiability Modifiability;

		// Token: 0x04003683 RID: 13955
		internal Readability Readability;

		// Token: 0x04003684 RID: 13956
		internal LocalizationCategory Category;
	}
}
