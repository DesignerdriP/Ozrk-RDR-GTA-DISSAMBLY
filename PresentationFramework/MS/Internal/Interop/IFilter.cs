using System;
using System.Runtime.InteropServices;

namespace MS.Internal.Interop
{
	// Token: 0x0200067B RID: 1659
	[ComVisible(true)]
	[Guid("89BCB740-6119-101A-BCB7-00DD010655AF")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IFilter
	{
		// Token: 0x06006CE4 RID: 27876
		IFILTER_FLAGS Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes);

		// Token: 0x06006CE5 RID: 27877
		STAT_CHUNK GetChunk();

		// Token: 0x06006CE6 RID: 27878
		void GetText([In] [Out] ref uint pcwcBuffer, [In] IntPtr pBuffer);

		// Token: 0x06006CE7 RID: 27879
		IntPtr GetValue();

		// Token: 0x06006CE8 RID: 27880
		IntPtr BindRegion([In] FILTERREGION origPos, [In] ref Guid riid);
	}
}
