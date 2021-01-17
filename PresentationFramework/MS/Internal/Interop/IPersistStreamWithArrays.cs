using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.Interop
{
	// Token: 0x0200067E RID: 1662
	[ComVisible(true)]
	[Guid("00000109-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IPersistStreamWithArrays
	{
		// Token: 0x06006CF9 RID: 27897
		void GetClassID(out Guid pClassID);

		// Token: 0x06006CFA RID: 27898
		[PreserveSig]
		int IsDirty();

		// Token: 0x06006CFB RID: 27899
		void Load(IStream pstm);

		// Token: 0x06006CFC RID: 27900
		void Save(IStream pstm, [MarshalAs(UnmanagedType.Bool)] bool fRemember);

		// Token: 0x06006CFD RID: 27901
		void GetSizeMax(out long pcbSize);
	}
}
