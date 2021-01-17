using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000619 RID: 1561
	internal class AttachedObject : EmbeddedObject
	{
		// Token: 0x060067BE RID: 26558 RVA: 0x001D150D File Offset: 0x001CF70D
		internal AttachedObject(int dcp, BaseParagraph para) : base(dcp)
		{
			this.Para = para;
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x001D151D File Offset: 0x001CF71D
		internal override void Dispose()
		{
			this.Para.Dispose();
			this.Para = null;
			base.Dispose();
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x001D1538 File Offset: 0x001CF738
		internal override void Update(EmbeddedObject newObject)
		{
			AttachedObject attachedObject = newObject as AttachedObject;
			ErrorHandler.Assert(attachedObject != null, ErrorHandler.EmbeddedObjectTypeMismatch);
			ErrorHandler.Assert(attachedObject.Element.Equals(this.Element), ErrorHandler.EmbeddedObjectOwnerMismatch);
			this.Dcp = attachedObject.Dcp;
			this.Para.SetUpdateInfo(PTS.FSKCHANGE.fskchInside, false);
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x060067C1 RID: 26561 RVA: 0x001D158E File Offset: 0x001CF78E
		internal override DependencyObject Element
		{
			get
			{
				return this.Para.Element;
			}
		}

		// Token: 0x04003386 RID: 13190
		internal BaseParagraph Para;
	}
}
