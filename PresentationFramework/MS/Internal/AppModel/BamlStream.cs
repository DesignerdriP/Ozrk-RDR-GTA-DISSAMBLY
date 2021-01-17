using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Windows.Markup;

namespace MS.Internal.AppModel
{
	// Token: 0x02000770 RID: 1904
	internal class BamlStream : Stream, IStreamInfo
	{
		// Token: 0x060078B9 RID: 30905 RVA: 0x002264EE File Offset: 0x002246EE
		[SecurityCritical]
		internal BamlStream(Stream stream, Assembly assembly)
		{
			this._assembly.Value = assembly;
			this._stream = stream;
		}

		// Token: 0x17001C93 RID: 7315
		// (get) Token: 0x060078BA RID: 30906 RVA: 0x00226509 File Offset: 0x00224709
		Assembly IStreamInfo.Assembly
		{
			get
			{
				return this._assembly.Value;
			}
		}

		// Token: 0x17001C94 RID: 7316
		// (get) Token: 0x060078BB RID: 30907 RVA: 0x00226516 File Offset: 0x00224716
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17001C95 RID: 7317
		// (get) Token: 0x060078BC RID: 30908 RVA: 0x00226523 File Offset: 0x00224723
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17001C96 RID: 7318
		// (get) Token: 0x060078BD RID: 30909 RVA: 0x00226530 File Offset: 0x00224730
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17001C97 RID: 7319
		// (get) Token: 0x060078BE RID: 30910 RVA: 0x0022653D File Offset: 0x0022473D
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x17001C98 RID: 7320
		// (get) Token: 0x060078BF RID: 30911 RVA: 0x0022654A File Offset: 0x0022474A
		// (set) Token: 0x060078C0 RID: 30912 RVA: 0x00226557 File Offset: 0x00224757
		public override long Position
		{
			get
			{
				return this._stream.Position;
			}
			set
			{
				this._stream.Position = value;
			}
		}

		// Token: 0x060078C1 RID: 30913 RVA: 0x00226565 File Offset: 0x00224765
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x060078C2 RID: 30914 RVA: 0x00226579 File Offset: 0x00224779
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x060078C3 RID: 30915 RVA: 0x0022658D File Offset: 0x0022478D
		public override void Close()
		{
			this._stream.Close();
		}

		// Token: 0x060078C4 RID: 30916 RVA: 0x0022659A File Offset: 0x0022479A
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._stream.EndRead(asyncResult);
		}

		// Token: 0x060078C5 RID: 30917 RVA: 0x002265A8 File Offset: 0x002247A8
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._stream.EndWrite(asyncResult);
		}

		// Token: 0x060078C6 RID: 30918 RVA: 0x002265B6 File Offset: 0x002247B6
		public override bool Equals(object obj)
		{
			return this._stream.Equals(obj);
		}

		// Token: 0x060078C7 RID: 30919 RVA: 0x002265C4 File Offset: 0x002247C4
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x060078C8 RID: 30920 RVA: 0x002265D1 File Offset: 0x002247D1
		public override int GetHashCode()
		{
			return this._stream.GetHashCode();
		}

		// Token: 0x060078C9 RID: 30921 RVA: 0x002265DE File Offset: 0x002247DE
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this._stream.Read(buffer, offset, count);
		}

		// Token: 0x060078CA RID: 30922 RVA: 0x002265EE File Offset: 0x002247EE
		public override int ReadByte()
		{
			return this._stream.ReadByte();
		}

		// Token: 0x060078CB RID: 30923 RVA: 0x002265FB File Offset: 0x002247FB
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._stream.Seek(offset, origin);
		}

		// Token: 0x060078CC RID: 30924 RVA: 0x0022660A File Offset: 0x0022480A
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x060078CD RID: 30925 RVA: 0x00226618 File Offset: 0x00224818
		public override string ToString()
		{
			return this._stream.ToString();
		}

		// Token: 0x060078CE RID: 30926 RVA: 0x00226625 File Offset: 0x00224825
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._stream.Write(buffer, offset, count);
		}

		// Token: 0x060078CF RID: 30927 RVA: 0x00226635 File Offset: 0x00224835
		public override void WriteByte(byte value)
		{
			this._stream.WriteByte(value);
		}

		// Token: 0x04003933 RID: 14643
		private SecurityCriticalDataForSet<Assembly> _assembly;

		// Token: 0x04003934 RID: 14644
		private Stream _stream;
	}
}
