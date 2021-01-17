using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Windows;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000667 RID: 1639
	internal class ManagedIStream : IStream
	{
		// Token: 0x06006C78 RID: 27768 RVA: 0x001F39D3 File Offset: 0x001F1BD3
		internal ManagedIStream(Stream ioStream)
		{
			if (ioStream == null)
			{
				throw new ArgumentNullException("ioStream");
			}
			this._ioStream = ioStream;
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x001F39F0 File Offset: 0x001F1BF0
		[SecurityCritical]
		void IStream.Read(byte[] buffer, int bufferSize, IntPtr bytesReadPtr)
		{
			int val = this._ioStream.Read(buffer, 0, bufferSize);
			if (bytesReadPtr != IntPtr.Zero)
			{
				Marshal.WriteInt32(bytesReadPtr, val);
			}
		}

		// Token: 0x06006C7A RID: 27770 RVA: 0x001F3A20 File Offset: 0x001F1C20
		[SecurityCritical]
		void IStream.Seek(long offset, int origin, IntPtr newPositionPtr)
		{
			SeekOrigin origin2;
			switch (origin)
			{
			case 0:
				origin2 = SeekOrigin.Begin;
				break;
			case 1:
				origin2 = SeekOrigin.Current;
				break;
			case 2:
				origin2 = SeekOrigin.End;
				break;
			default:
				throw new ArgumentOutOfRangeException("origin");
			}
			long val = this._ioStream.Seek(offset, origin2);
			if (newPositionPtr != IntPtr.Zero)
			{
				Marshal.WriteInt64(newPositionPtr, val);
			}
		}

		// Token: 0x06006C7B RID: 27771 RVA: 0x001F3A7A File Offset: 0x001F1C7A
		void IStream.SetSize(long libNewSize)
		{
			this._ioStream.SetLength(libNewSize);
		}

		// Token: 0x06006C7C RID: 27772 RVA: 0x001F3A88 File Offset: 0x001F1C88
		void IStream.Stat(out System.Runtime.InteropServices.ComTypes.STATSTG streamStats, int grfStatFlag)
		{
			streamStats = default(System.Runtime.InteropServices.ComTypes.STATSTG);
			streamStats.type = 2;
			streamStats.cbSize = this._ioStream.Length;
			streamStats.grfMode = 0;
			if (this._ioStream.CanRead && this._ioStream.CanWrite)
			{
				streamStats.grfMode |= 2;
				return;
			}
			if (this._ioStream.CanRead)
			{
				streamStats.grfMode |= 0;
				return;
			}
			if (this._ioStream.CanWrite)
			{
				streamStats.grfMode |= 1;
				return;
			}
			throw new IOException(SR.Get("StreamObjectDisposed"));
		}

		// Token: 0x06006C7D RID: 27773 RVA: 0x001F3B22 File Offset: 0x001F1D22
		[SecurityCritical]
		void IStream.Write(byte[] buffer, int bufferSize, IntPtr bytesWrittenPtr)
		{
			this._ioStream.Write(buffer, 0, bufferSize);
			if (bytesWrittenPtr != IntPtr.Zero)
			{
				Marshal.WriteInt32(bytesWrittenPtr, bufferSize);
			}
		}

		// Token: 0x06006C7E RID: 27774 RVA: 0x001F3B46 File Offset: 0x001F1D46
		void IStream.Clone(out IStream streamCopy)
		{
			streamCopy = null;
			throw new NotSupportedException();
		}

		// Token: 0x06006C7F RID: 27775 RVA: 0x00041C10 File Offset: 0x0003FE10
		void IStream.CopyTo(IStream targetStream, long bufferSize, IntPtr buffer, IntPtr bytesWrittenPtr)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C80 RID: 27776 RVA: 0x00041C10 File Offset: 0x0003FE10
		void IStream.Commit(int flags)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x00041C10 File Offset: 0x0003FE10
		void IStream.LockRegion(long offset, long byteCount, int lockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C82 RID: 27778 RVA: 0x00041C10 File Offset: 0x0003FE10
		void IStream.Revert()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06006C83 RID: 27779 RVA: 0x00041C10 File Offset: 0x0003FE10
		void IStream.UnlockRegion(long offset, long byteCount, int lockType)
		{
			throw new NotSupportedException();
		}

		// Token: 0x04003545 RID: 13637
		private Stream _ioStream;
	}
}
