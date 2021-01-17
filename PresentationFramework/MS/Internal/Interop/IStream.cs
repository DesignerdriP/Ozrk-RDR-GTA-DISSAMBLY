using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace MS.Internal.Interop
{
	// Token: 0x0200067D RID: 1661
	[ComVisible(true)]
	[Guid("0000000C-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IStream
	{
		// Token: 0x06006CEE RID: 27886
		void Read(IntPtr bufferBase, int sizeInBytes, IntPtr refToNumBytesRead);

		// Token: 0x06006CEF RID: 27887
		void Write(IntPtr bufferBase, int sizeInBytes, IntPtr refToNumBytesWritten);

		// Token: 0x06006CF0 RID: 27888
		void Seek(long offset, int origin, IntPtr refToNewOffsetNullAllowed);

		// Token: 0x06006CF1 RID: 27889
		void SetSize(long newSize);

		// Token: 0x06006CF2 RID: 27890
		void CopyTo(IStream targetStream, long bytesToCopy, IntPtr refToNumBytesRead, IntPtr refToNumBytesWritten);

		// Token: 0x06006CF3 RID: 27891
		void Commit(int commitFlags);

		// Token: 0x06006CF4 RID: 27892
		void Revert();

		// Token: 0x06006CF5 RID: 27893
		void LockRegion(long offset, long sizeInBytes, int lockType);

		// Token: 0x06006CF6 RID: 27894
		void UnlockRegion(long offset, long sizeInBytes, int lockType);

		// Token: 0x06006CF7 RID: 27895
		void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG statStructure, int statFlag);

		// Token: 0x06006CF8 RID: 27896
		void Clone(out IStream newStream);
	}
}
