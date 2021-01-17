using System;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000664 RID: 1636
	internal class ManagedFullPropSpec
	{
		// Token: 0x170019E5 RID: 6629
		// (get) Token: 0x06006C63 RID: 27747 RVA: 0x001F38AB File Offset: 0x001F1AAB
		internal Guid Guid
		{
			get
			{
				return this._guid;
			}
		}

		// Token: 0x170019E6 RID: 6630
		// (get) Token: 0x06006C64 RID: 27748 RVA: 0x001F38B3 File Offset: 0x001F1AB3
		internal ManagedPropSpec Property
		{
			get
			{
				return this._property;
			}
		}

		// Token: 0x06006C65 RID: 27749 RVA: 0x001F38BB File Offset: 0x001F1ABB
		internal ManagedFullPropSpec(Guid guid, uint propId)
		{
			this._guid = guid;
			this._property = new ManagedPropSpec(propId);
		}

		// Token: 0x06006C66 RID: 27750 RVA: 0x001F38D6 File Offset: 0x001F1AD6
		internal ManagedFullPropSpec(FULLPROPSPEC nativePropSpec)
		{
			this._guid = nativePropSpec.guid;
			this._property = new ManagedPropSpec(nativePropSpec.property);
		}

		// Token: 0x0400353B RID: 13627
		private Guid _guid;

		// Token: 0x0400353C RID: 13628
		private ManagedPropSpec _property;
	}
}
