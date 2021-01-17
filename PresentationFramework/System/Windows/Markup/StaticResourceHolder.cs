using System;

namespace System.Windows.Markup
{
	// Token: 0x020001D2 RID: 466
	internal class StaticResourceHolder : StaticResourceExtension
	{
		// Token: 0x06001E7F RID: 7807 RVA: 0x00091907 File Offset: 0x0008FB07
		internal StaticResourceHolder(object resourceKey, DeferredResourceReference prefetchedValue) : base(resourceKey)
		{
			this._prefetchedValue = prefetchedValue;
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x00091917 File Offset: 0x0008FB17
		internal override DeferredResourceReference PrefetchedValue
		{
			get
			{
				return this._prefetchedValue;
			}
		}

		// Token: 0x04001498 RID: 5272
		private DeferredResourceReference _prefetchedValue;
	}
}
