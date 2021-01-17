using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x0200067C RID: 1660
	[ComVisible(true)]
	[Guid("00000109-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPersistStream
	{
		// Token: 0x06006CE9 RID: 27881
		void GetClassID(out Guid pClassID);

		// Token: 0x06006CEA RID: 27882
		[PreserveSig]
		int IsDirty();

		// Token: 0x06006CEB RID: 27883
		void Load(IStream pstm);

		// Token: 0x06006CEC RID: 27884
		void Save(IStream pstm, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06006CED RID: 27885
		void GetSizeMax(out long pcbSize);
	}
}
