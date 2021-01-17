using System;
using System.Collections;
using System.IO;
using System.Threading;

namespace System.Windows.Markup
{
	// Token: 0x02000268 RID: 616
	internal class ReadWriteStreamManager
	{
		// Token: 0x0600233B RID: 9019 RVA: 0x000ACDA0 File Offset: 0x000AAFA0
		internal ReadWriteStreamManager()
		{
			this.ReaderFirstBufferPosition = 0L;
			this.WriterFirstBufferPosition = 0L;
			this.ReaderBufferArrayList = new ArrayList();
			this.WriterBufferArrayList = new ArrayList();
			this._writerStream = new WriterStream(this);
			this._readerStream = new ReaderStream(this);
			this._bufferLock = new ReaderWriterLock();
		}

		// Token: 0x0600233C RID: 9020 RVA: 0x000ACDFC File Offset: 0x000AAFFC
		internal void Write(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			int num;
			int num2;
			byte[] bufferFromFilePosition = this.GetBufferFromFilePosition(this.WritePosition, false, out num, out num2);
			int num3 = this.BufferSize - num;
			int num4;
			int num5;
			if (count > num3)
			{
				num4 = num3;
				num5 = count - num3;
			}
			else
			{
				num5 = 0;
				num4 = count;
			}
			for (int i = 0; i < num4; i++)
			{
				bufferFromFilePosition[num++] = buffer[offset++];
			}
			this.WritePosition += (long)num4;
			if (this.WritePosition > this.WriteLength)
			{
				this.WriteLength = this.WritePosition;
			}
			if (num5 > 0)
			{
				this.Write(buffer, offset, num5);
			}
		}

		// Token: 0x0600233D RID: 9021 RVA: 0x000ACE9C File Offset: 0x000AB09C
		internal long WriterSeek(long offset, SeekOrigin loc)
		{
			switch (loc)
			{
			case SeekOrigin.Begin:
				this.WritePosition = (long)((int)offset);
				break;
			case SeekOrigin.Current:
				this.WritePosition = (long)((int)(this.WritePosition + offset));
				break;
			case SeekOrigin.End:
				throw new NotSupportedException(SR.Get("ParserWriterNoSeekEnd"));
			default:
				throw new ArgumentException(SR.Get("ParserWriterUnknownOrigin"));
			}
			if (this.WritePosition > this.WriteLength || this.WritePosition < this.ReadLength)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			return this.WritePosition;
		}

		// Token: 0x0600233E RID: 9022 RVA: 0x000ACF28 File Offset: 0x000AB128
		internal void UpdateReaderLength(long position)
		{
			if (this.ReadLength > position)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this.ReadLength = position;
			if (this.ReadLength > this.WriteLength)
			{
				throw new ArgumentOutOfRangeException("position");
			}
			this.CheckIfCanRemoveFromArrayList(position, this.WriterBufferArrayList, ref this._writerFirstBufferPosition);
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x00002137 File Offset: 0x00000337
		internal void WriterClose()
		{
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000ACF7C File Offset: 0x000AB17C
		internal int Read(byte[] buffer, int offset, int count)
		{
			if ((long)count + this.ReadPosition > this.ReadLength)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			int num;
			int num2;
			byte[] bufferFromFilePosition = this.GetBufferFromFilePosition(this.ReadPosition, true, out num, out num2);
			int num3 = this.BufferSize - num;
			int num4;
			int num5;
			if (count > num3)
			{
				num4 = num3;
				num5 = count - num3;
			}
			else
			{
				num5 = 0;
				num4 = count;
			}
			for (int i = 0; i < num4; i++)
			{
				buffer[offset++] = bufferFromFilePosition[num++];
			}
			this.ReadPosition += (long)num4;
			if (num5 > 0)
			{
				this.Read(buffer, offset, num5);
			}
			return count;
		}

		// Token: 0x06002341 RID: 9025 RVA: 0x000AD01C File Offset: 0x000AB21C
		internal int ReadByte()
		{
			byte[] array = new byte[1];
			this.Read(array, 0, 1);
			return (int)array[0];
		}

		// Token: 0x06002342 RID: 9026 RVA: 0x000AD040 File Offset: 0x000AB240
		internal long ReaderSeek(long offset, SeekOrigin loc)
		{
			switch (loc)
			{
			case SeekOrigin.Begin:
				this.ReadPosition = (long)((int)offset);
				break;
			case SeekOrigin.Current:
				this.ReadPosition = (long)((int)(this.ReadPosition + offset));
				break;
			case SeekOrigin.End:
				throw new NotSupportedException(SR.Get("ParserWriterNoSeekEnd"));
			default:
				throw new ArgumentException(SR.Get("ParserWriterUnknownOrigin"));
			}
			if (this.ReadPosition < this.ReaderFirstBufferPosition || this.ReadPosition >= this.ReadLength)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			return this.ReadPosition;
		}

		// Token: 0x06002343 RID: 9027 RVA: 0x000AD0CB File Offset: 0x000AB2CB
		internal void ReaderDoneWithFileUpToPosition(long position)
		{
			this.CheckIfCanRemoveFromArrayList(position, this.ReaderBufferArrayList, ref this._readerFirstBufferPosition);
		}

		// Token: 0x06002344 RID: 9028 RVA: 0x000AD0E0 File Offset: 0x000AB2E0
		private byte[] GetBufferFromFilePosition(long position, bool reader, out int bufferOffset, out int bufferIndex)
		{
			this._bufferLock.AcquireWriterLock(-1);
			ArrayList arrayList;
			long num;
			if (reader)
			{
				arrayList = this.ReaderBufferArrayList;
				num = this.ReaderFirstBufferPosition;
			}
			else
			{
				arrayList = this.WriterBufferArrayList;
				num = this.WriterFirstBufferPosition;
			}
			bufferIndex = (int)((position - num) / (long)this.BufferSize);
			bufferOffset = (int)(position - num - (long)(bufferIndex * this.BufferSize));
			byte[] array;
			if (arrayList.Count <= bufferIndex)
			{
				array = new byte[this.BufferSize];
				this.ReaderBufferArrayList.Add(array);
				this.WriterBufferArrayList.Add(array);
			}
			else
			{
				array = (arrayList[bufferIndex] as byte[]);
			}
			this._bufferLock.ReleaseWriterLock();
			return array;
		}

		// Token: 0x06002345 RID: 9029 RVA: 0x000AD18C File Offset: 0x000AB38C
		private void CheckIfCanRemoveFromArrayList(long position, ArrayList arrayList, ref long firstBufferPosition)
		{
			int num = (int)((position - firstBufferPosition) / (long)this.BufferSize);
			if (num > 0)
			{
				int num2 = num;
				this._bufferLock.AcquireWriterLock(-1);
				firstBufferPosition += (long)(num2 * this.BufferSize);
				arrayList.RemoveRange(0, num);
				this._bufferLock.ReleaseWriterLock();
			}
		}

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x000AD1DA File Offset: 0x000AB3DA
		internal WriterStream WriterStream
		{
			get
			{
				return this._writerStream;
			}
		}

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002347 RID: 9031 RVA: 0x000AD1E2 File Offset: 0x000AB3E2
		internal ReaderStream ReaderStream
		{
			get
			{
				return this._readerStream;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002348 RID: 9032 RVA: 0x000AD1EA File Offset: 0x000AB3EA
		// (set) Token: 0x06002349 RID: 9033 RVA: 0x000AD1F2 File Offset: 0x000AB3F2
		internal long ReadPosition
		{
			get
			{
				return this._readPosition;
			}
			set
			{
				this._readPosition = value;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x0600234A RID: 9034 RVA: 0x000AD1FB File Offset: 0x000AB3FB
		// (set) Token: 0x0600234B RID: 9035 RVA: 0x000AD203 File Offset: 0x000AB403
		internal long ReadLength
		{
			get
			{
				return this._readLength;
			}
			set
			{
				this._readLength = value;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x0600234C RID: 9036 RVA: 0x000AD20C File Offset: 0x000AB40C
		// (set) Token: 0x0600234D RID: 9037 RVA: 0x000AD214 File Offset: 0x000AB414
		internal long WritePosition
		{
			get
			{
				return this._writePosition;
			}
			set
			{
				this._writePosition = value;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x0600234E RID: 9038 RVA: 0x000AD21D File Offset: 0x000AB41D
		// (set) Token: 0x0600234F RID: 9039 RVA: 0x000AD225 File Offset: 0x000AB425
		internal long WriteLength
		{
			get
			{
				return this._writeLength;
			}
			set
			{
				this._writeLength = value;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002350 RID: 9040 RVA: 0x000AD22E File Offset: 0x000AB42E
		private int BufferSize
		{
			get
			{
				return 4096;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002351 RID: 9041 RVA: 0x000AD235 File Offset: 0x000AB435
		// (set) Token: 0x06002352 RID: 9042 RVA: 0x000AD23D File Offset: 0x000AB43D
		private long ReaderFirstBufferPosition
		{
			get
			{
				return this._readerFirstBufferPosition;
			}
			set
			{
				this._readerFirstBufferPosition = value;
			}
		}

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002353 RID: 9043 RVA: 0x000AD246 File Offset: 0x000AB446
		// (set) Token: 0x06002354 RID: 9044 RVA: 0x000AD24E File Offset: 0x000AB44E
		private long WriterFirstBufferPosition
		{
			get
			{
				return this._writerFirstBufferPosition;
			}
			set
			{
				this._writerFirstBufferPosition = value;
			}
		}

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002355 RID: 9045 RVA: 0x000AD257 File Offset: 0x000AB457
		// (set) Token: 0x06002356 RID: 9046 RVA: 0x000AD25F File Offset: 0x000AB45F
		private ArrayList ReaderBufferArrayList
		{
			get
			{
				return this._readerBufferArrayList;
			}
			set
			{
				this._readerBufferArrayList = value;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x000AD268 File Offset: 0x000AB468
		// (set) Token: 0x06002358 RID: 9048 RVA: 0x000AD270 File Offset: 0x000AB470
		private ArrayList WriterBufferArrayList
		{
			get
			{
				return this._writerBufferArrayList;
			}
			set
			{
				this._writerBufferArrayList = value;
			}
		}

		// Token: 0x04001A7A RID: 6778
		private long _readPosition;

		// Token: 0x04001A7B RID: 6779
		private long _readLength;

		// Token: 0x04001A7C RID: 6780
		private long _writePosition;

		// Token: 0x04001A7D RID: 6781
		private long _writeLength;

		// Token: 0x04001A7E RID: 6782
		private ReaderWriterLock _bufferLock;

		// Token: 0x04001A7F RID: 6783
		private WriterStream _writerStream;

		// Token: 0x04001A80 RID: 6784
		private ReaderStream _readerStream;

		// Token: 0x04001A81 RID: 6785
		private long _readerFirstBufferPosition;

		// Token: 0x04001A82 RID: 6786
		private long _writerFirstBufferPosition;

		// Token: 0x04001A83 RID: 6787
		private ArrayList _readerBufferArrayList;

		// Token: 0x04001A84 RID: 6788
		private ArrayList _writerBufferArrayList;

		// Token: 0x04001A85 RID: 6789
		private const int _bufferSize = 4096;
	}
}
