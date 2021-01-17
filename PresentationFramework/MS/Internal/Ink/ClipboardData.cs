using System;
using System.Security;
using System.Windows;

namespace MS.Internal.Ink
{
	// Token: 0x02000680 RID: 1664
	internal abstract class ClipboardData
	{
		// Token: 0x06006D04 RID: 27908 RVA: 0x001F5225 File Offset: 0x001F3425
		[SecurityCritical]
		internal bool CopyToDataObject(IDataObject dataObject)
		{
			if (this.CanCopy())
			{
				this.DoCopy(dataObject);
				return true;
			}
			return false;
		}

		// Token: 0x06006D05 RID: 27909 RVA: 0x001F5239 File Offset: 0x001F3439
		internal void PasteFromDataObject(IDataObject dataObject)
		{
			if (this.CanPaste(dataObject))
			{
				this.DoPaste(dataObject);
			}
		}

		// Token: 0x06006D06 RID: 27910
		internal abstract bool CanPaste(IDataObject dataObject);

		// Token: 0x06006D07 RID: 27911
		protected abstract bool CanCopy();

		// Token: 0x06006D08 RID: 27912
		protected abstract void DoCopy(IDataObject dataObject);

		// Token: 0x06006D09 RID: 27913
		protected abstract void DoPaste(IDataObject dataObject);
	}
}
