using System;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Windows;
using MS.Internal.Interop;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200066D RID: 1645
	internal class EncryptedPackageFilter : IFilter
	{
		// Token: 0x06006CC3 RID: 27843 RVA: 0x001F4BD9 File Offset: 0x001F2DD9
		internal EncryptedPackageFilter(EncryptedPackageEnvelope encryptedPackage)
		{
			if (encryptedPackage == null)
			{
				throw new ArgumentNullException("encryptedPackage");
			}
			this._filter = new IndexingFilterMarshaler(new CorePropertiesFilter(encryptedPackage.PackageProperties));
		}

		// Token: 0x06006CC4 RID: 27844 RVA: 0x001F4C05 File Offset: 0x001F2E05
		public IFILTER_FLAGS Init([In] IFILTER_INIT grfFlags, [In] uint cAttributes, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] [In] FULLPROPSPEC[] aAttributes)
		{
			return this._filter.Init(grfFlags, cAttributes, aAttributes);
		}

		// Token: 0x06006CC5 RID: 27845 RVA: 0x001F4C15 File Offset: 0x001F2E15
		public STAT_CHUNK GetChunk()
		{
			return this._filter.GetChunk();
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x001F4C22 File Offset: 0x001F2E22
		public void GetText(ref uint bufCharacterCount, IntPtr pBuffer)
		{
			throw new COMException(SR.Get("FilterGetTextNotSupported"), -2147215611);
		}

		// Token: 0x06006CC7 RID: 27847 RVA: 0x001F4C38 File Offset: 0x001F2E38
		public IntPtr GetValue()
		{
			return this._filter.GetValue();
		}

		// Token: 0x06006CC8 RID: 27848 RVA: 0x001F207F File Offset: 0x001F027F
		public IntPtr BindRegion([In] FILTERREGION origPos, [In] ref Guid riid)
		{
			throw new NotImplementedException(SR.Get("FilterBindRegionNotImplemented"));
		}

		// Token: 0x0400356C RID: 13676
		private IFilter _filter;
	}
}
