using System;
using System.Windows;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000618 RID: 1560
	internal abstract class EmbeddedObject
	{
		// Token: 0x060067BA RID: 26554 RVA: 0x001D14FE File Offset: 0x001CF6FE
		protected EmbeddedObject(int dcp)
		{
			this.Dcp = dcp;
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void Dispose()
		{
		}

		// Token: 0x060067BC RID: 26556
		internal abstract void Update(EmbeddedObject newObject);

		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x060067BD RID: 26557
		internal abstract DependencyObject Element { get; }

		// Token: 0x04003385 RID: 13189
		internal int Dcp;
	}
}
