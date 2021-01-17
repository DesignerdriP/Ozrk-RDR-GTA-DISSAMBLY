﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;
using System.Windows.Markup;
using System.Windows.Threading;
using MS.Internal.AppModel;

namespace MS.Internal.Navigation
{
	// Token: 0x02000659 RID: 1625
	internal class BindStream : Stream, IStreamInfo
	{
		// Token: 0x06006BE2 RID: 27618 RVA: 0x001F11CC File Offset: 0x001EF3CC
		internal BindStream(Stream stream, long maxBytes, Uri uri, IContentContainer cc, Dispatcher callbackDispatcher)
		{
			this._bytesRead = 0L;
			this._maxBytes = maxBytes;
			this._lastProgressEventByte = 0L;
			this._stream = stream;
			this._uri = uri;
			this._cc = cc;
			this._callbackDispatcher = callbackDispatcher;
		}

		// Token: 0x06006BE3 RID: 27619 RVA: 0x001F120C File Offset: 0x001EF40C
		private void UpdateNavigationProgress()
		{
			for (long num = this._lastProgressEventByte + 1024L; num <= this._bytesRead; num += 1024L)
			{
				this.UpdateNavProgressHelper(num);
				this._lastProgressEventByte = num;
			}
			if (this._bytesRead == this._maxBytes && this._lastProgressEventByte < this._maxBytes)
			{
				this.UpdateNavProgressHelper(this._maxBytes);
				this._lastProgressEventByte = this._maxBytes;
			}
		}

		// Token: 0x06006BE4 RID: 27620 RVA: 0x001F1280 File Offset: 0x001EF480
		private void UpdateNavProgressHelper(long numBytes)
		{
			if (this._callbackDispatcher != null && !this._callbackDispatcher.CheckAccess())
			{
				this._callbackDispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this._cc.OnNavigationProgress(this._uri, numBytes, this._maxBytes);
					return null;
				}), null);
				return;
			}
			this._cc.OnNavigationProgress(this._uri, numBytes, this._maxBytes);
		}

		// Token: 0x170019CA RID: 6602
		// (get) Token: 0x06006BE5 RID: 27621 RVA: 0x001F12EF File Offset: 0x001EF4EF
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x170019CB RID: 6603
		// (get) Token: 0x06006BE6 RID: 27622 RVA: 0x001F12FC File Offset: 0x001EF4FC
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x170019CC RID: 6604
		// (get) Token: 0x06006BE7 RID: 27623 RVA: 0x001F1309 File Offset: 0x001EF509
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x170019CD RID: 6605
		// (get) Token: 0x06006BE8 RID: 27624 RVA: 0x001F1316 File Offset: 0x001EF516
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x170019CE RID: 6606
		// (get) Token: 0x06006BE9 RID: 27625 RVA: 0x001F1323 File Offset: 0x001EF523
		// (set) Token: 0x06006BEA RID: 27626 RVA: 0x001F1330 File Offset: 0x001EF530
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

		// Token: 0x06006BEB RID: 27627 RVA: 0x001F133E File Offset: 0x001EF53E
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginRead(buffer, offset, count, callback, state);
		}

		// Token: 0x06006BEC RID: 27628 RVA: 0x001F1352 File Offset: 0x001EF552
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return this._stream.BeginWrite(buffer, offset, count, callback, state);
		}

		// Token: 0x06006BED RID: 27629 RVA: 0x001F1368 File Offset: 0x001EF568
		public override void Close()
		{
			this._stream.Close();
			if (this._callbackDispatcher != null && !this._callbackDispatcher.CheckAccess())
			{
				this._callbackDispatcher.BeginInvoke(DispatcherPriority.Send, new DispatcherOperationCallback(delegate(object unused)
				{
					this._cc.OnStreamClosed(this._uri);
					return null;
				}), null);
				return;
			}
			this._cc.OnStreamClosed(this._uri);
		}

		// Token: 0x06006BEE RID: 27630 RVA: 0x001F13C2 File Offset: 0x001EF5C2
		[SecurityCritical]
		public override ObjRef CreateObjRef(Type requestedType)
		{
			return this._stream.CreateObjRef(requestedType);
		}

		// Token: 0x06006BEF RID: 27631 RVA: 0x001F13D0 File Offset: 0x001EF5D0
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this._stream.EndRead(asyncResult);
		}

		// Token: 0x06006BF0 RID: 27632 RVA: 0x001F13DE File Offset: 0x001EF5DE
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this._stream.EndWrite(asyncResult);
		}

		// Token: 0x06006BF1 RID: 27633 RVA: 0x001F13EC File Offset: 0x001EF5EC
		public override bool Equals(object obj)
		{
			return this._stream.Equals(obj);
		}

		// Token: 0x06006BF2 RID: 27634 RVA: 0x001F13FA File Offset: 0x001EF5FA
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x001F1407 File Offset: 0x001EF607
		public override int GetHashCode()
		{
			return this._stream.GetHashCode();
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x001F1414 File Offset: 0x001EF614
		[SecurityCritical]
		public override object InitializeLifetimeService()
		{
			return this._stream.InitializeLifetimeService();
		}

		// Token: 0x06006BF5 RID: 27637 RVA: 0x001F1424 File Offset: 0x001EF624
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = this._stream.Read(buffer, offset, count);
			this._bytesRead += (long)num;
			this._maxBytes = ((this._bytesRead > this._maxBytes) ? this._bytesRead : this._maxBytes);
			if (this._lastProgressEventByte + 1024L <= this._bytesRead || num == 0)
			{
				this.UpdateNavigationProgress();
			}
			return num;
		}

		// Token: 0x06006BF6 RID: 27638 RVA: 0x001F1490 File Offset: 0x001EF690
		public override int ReadByte()
		{
			int num = this._stream.ReadByte();
			if (num != -1)
			{
				this._bytesRead += 1L;
				this._maxBytes = ((this._bytesRead > this._maxBytes) ? this._bytesRead : this._maxBytes);
			}
			if (this._lastProgressEventByte + 1024L <= this._bytesRead || num == -1)
			{
				this.UpdateNavigationProgress();
			}
			return num;
		}

		// Token: 0x06006BF7 RID: 27639 RVA: 0x001F14FE File Offset: 0x001EF6FE
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this._stream.Seek(offset, origin);
		}

		// Token: 0x06006BF8 RID: 27640 RVA: 0x001F150D File Offset: 0x001EF70D
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
		}

		// Token: 0x06006BF9 RID: 27641 RVA: 0x001F151B File Offset: 0x001EF71B
		public override string ToString()
		{
			return this._stream.ToString();
		}

		// Token: 0x06006BFA RID: 27642 RVA: 0x001F1528 File Offset: 0x001EF728
		public override void Write(byte[] buffer, int offset, int count)
		{
			this._stream.Write(buffer, offset, count);
		}

		// Token: 0x06006BFB RID: 27643 RVA: 0x001F1538 File Offset: 0x001EF738
		public override void WriteByte(byte value)
		{
			this._stream.WriteByte(value);
		}

		// Token: 0x170019CF RID: 6607
		// (get) Token: 0x06006BFC RID: 27644 RVA: 0x001F1546 File Offset: 0x001EF746
		public Stream Stream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x170019D0 RID: 6608
		// (get) Token: 0x06006BFD RID: 27645 RVA: 0x001F1550 File Offset: 0x001EF750
		Assembly IStreamInfo.Assembly
		{
			get
			{
				Assembly result = null;
				if (this._stream != null)
				{
					IStreamInfo streamInfo = this._stream as IStreamInfo;
					if (streamInfo != null)
					{
						result = streamInfo.Assembly;
					}
				}
				return result;
			}
		}

		// Token: 0x040034FC RID: 13564
		private long _bytesRead;

		// Token: 0x040034FD RID: 13565
		private long _maxBytes;

		// Token: 0x040034FE RID: 13566
		private long _lastProgressEventByte;

		// Token: 0x040034FF RID: 13567
		private Stream _stream;

		// Token: 0x04003500 RID: 13568
		private Uri _uri;

		// Token: 0x04003501 RID: 13569
		private IContentContainer _cc;

		// Token: 0x04003502 RID: 13570
		private Dispatcher _callbackDispatcher;

		// Token: 0x04003503 RID: 13571
		private const long _bytesInterval = 1024L;
	}
}
