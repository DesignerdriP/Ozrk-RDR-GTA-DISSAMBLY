using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Windows;
using MS.Internal.Interop;
using MS.Win32;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x02000668 RID: 1640
	[SecurityCritical(SecurityCriticalScope.Everything)]
	internal class UnsafeIndexingFilterStream : Stream
	{
		// Token: 0x06006C84 RID: 27780 RVA: 0x001F3B50 File Offset: 0x001F1D50
		internal UnsafeIndexingFilterStream(MS.Internal.Interop.IStream oleStream)
		{
			if (oleStream == null)
			{
				throw new ArgumentNullException("oleStream");
			}
			this._oleStream = oleStream;
			this._disposed = false;
		}

		// Token: 0x06006C85 RID: 27781 RVA: 0x001F3B74 File Offset: 0x001F1D74
		public unsafe override int Read(byte[] buffer, int offset, int count)
		{
			this.ThrowIfStreamDisposed();
			PackagingUtilities.VerifyStreamReadArgs(this, buffer, offset, count);
			if (count == 0)
			{
				return 0;
			}
			int result;
			IntPtr refToNumBytesRead = new IntPtr((void*)(&result));
			long position = this.Position;
			try
			{
				try
				{
					fixed (byte* ptr = &buffer[offset])
					{
						this._oleStream.Read(new IntPtr((void*)ptr), count, refToNumBytesRead);
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			catch (COMException innerException)
			{
				this.Position = position;
				throw new IOException("Read", innerException);
			}
			catch (IOException innerException2)
			{
				this.Position = position;
				throw new IOException("Read", innerException2);
			}
			return result;
		}

		// Token: 0x06006C86 RID: 27782 RVA: 0x001F3C20 File Offset: 0x001F1E20
		public unsafe override long Seek(long offset, SeekOrigin origin)
		{
			this.ThrowIfStreamDisposed();
			long result = 0L;
			IntPtr refToNewOffsetNullAllowed = new IntPtr((void*)(&result));
			this._oleStream.Seek(offset, (int)origin, refToNewOffsetNullAllowed);
			return result;
		}

		// Token: 0x06006C87 RID: 27783 RVA: 0x001F3C4F File Offset: 0x001F1E4F
		public override void SetLength(long newLength)
		{
			this.ThrowIfStreamDisposed();
			throw new NotSupportedException(SR.Get("StreamDoesNotSupportWrite"));
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x001F3C4F File Offset: 0x001F1E4F
		public override void Write(byte[] buf, int offset, int count)
		{
			this.ThrowIfStreamDisposed();
			throw new NotSupportedException(SR.Get("StreamDoesNotSupportWrite"));
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x001F3C66 File Offset: 0x001F1E66
		public override void Flush()
		{
			this.ThrowIfStreamDisposed();
		}

		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x06006C8A RID: 27786 RVA: 0x001F3C6E File Offset: 0x001F1E6E
		public override bool CanRead
		{
			get
			{
				return !this._disposed;
			}
		}

		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x06006C8B RID: 27787 RVA: 0x001F3C6E File Offset: 0x001F1E6E
		public override bool CanSeek
		{
			get
			{
				return !this._disposed;
			}
		}

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x06006C8C RID: 27788 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x06006C8D RID: 27789 RVA: 0x001F3C79 File Offset: 0x001F1E79
		// (set) Token: 0x06006C8E RID: 27790 RVA: 0x001F3C8A File Offset: 0x001F1E8A
		public override long Position
		{
			get
			{
				this.ThrowIfStreamDisposed();
				return this.Seek(0L, SeekOrigin.Current);
			}
			set
			{
				this.ThrowIfStreamDisposed();
				if (value < 0L)
				{
					throw new ArgumentException(SR.Get("CannotSetNegativePosition"));
				}
				this.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x06006C8F RID: 27791 RVA: 0x001F3CB0 File Offset: 0x001F1EB0
		public override long Length
		{
			get
			{
				this.ThrowIfStreamDisposed();
				System.Runtime.InteropServices.ComTypes.STATSTG statstg;
				this._oleStream.Stat(out statstg, 1);
				return statstg.cbSize;
			}
		}

		// Token: 0x06006C90 RID: 27792 RVA: 0x001F3CD8 File Offset: 0x001F1ED8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._oleStream != null)
				{
					UnsafeNativeMethods.SafeReleaseComObject(this._oleStream);
				}
			}
			finally
			{
				this._oleStream = null;
				this._disposed = true;
				base.Dispose(disposing);
			}
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x001F3D24 File Offset: 0x001F1F24
		private void ThrowIfStreamDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(null, SR.Get("StreamObjectDisposed"));
			}
		}

		// Token: 0x04003546 RID: 13638
		private MS.Internal.Interop.IStream _oleStream;

		// Token: 0x04003547 RID: 13639
		private bool _disposed;
	}
}
