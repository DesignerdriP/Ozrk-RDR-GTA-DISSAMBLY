﻿using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200026A RID: 618
	internal class ReaderStream : Stream
	{
		// Token: 0x06002369 RID: 9065 RVA: 0x000AD2DB File Offset: 0x000AB4DB
		internal ReaderStream(ReadWriteStreamManager streamManager)
		{
			this._streamManager = streamManager;
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x0600236A RID: 9066 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x0600236B RID: 9067 RVA: 0x00016748 File Offset: 0x00014948
		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x0600236C RID: 9068 RVA: 0x0000B02A File Offset: 0x0000922A
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600236D RID: 9069 RVA: 0x00002137 File Offset: 0x00000337
		public override void Close()
		{
		}

		// Token: 0x0600236E RID: 9070 RVA: 0x00002137 File Offset: 0x00000337
		public override void Flush()
		{
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x0600236F RID: 9071 RVA: 0x000AD2EA File Offset: 0x000AB4EA
		public override long Length
		{
			get
			{
				return this.StreamManager.ReadLength;
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002370 RID: 9072 RVA: 0x000AD2F7 File Offset: 0x000AB4F7
		// (set) Token: 0x06002371 RID: 9073 RVA: 0x000AD304 File Offset: 0x000AB504
		public override long Position
		{
			get
			{
				return this.StreamManager.ReadPosition;
			}
			set
			{
				this.StreamManager.ReaderSeek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x06002372 RID: 9074 RVA: 0x000AD314 File Offset: 0x000AB514
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.StreamManager.Read(buffer, offset, count);
		}

		// Token: 0x06002373 RID: 9075 RVA: 0x000AD324 File Offset: 0x000AB524
		public override int ReadByte()
		{
			return this.StreamManager.ReadByte();
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000AD331 File Offset: 0x000AB531
		public override long Seek(long offset, SeekOrigin loc)
		{
			return this.StreamManager.ReaderSeek(offset, loc);
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x00041C10 File Offset: 0x0003FE10
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002376 RID: 9078 RVA: 0x00041C10 File Offset: 0x0003FE10
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002377 RID: 9079 RVA: 0x000AD340 File Offset: 0x000AB540
		internal void ReaderDoneWithFileUpToPosition(long position)
		{
			this.StreamManager.ReaderDoneWithFileUpToPosition(position);
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002378 RID: 9080 RVA: 0x000AD34E File Offset: 0x000AB54E
		private ReadWriteStreamManager StreamManager
		{
			get
			{
				return this._streamManager;
			}
		}

		// Token: 0x04001A87 RID: 6791
		private ReadWriteStreamManager _streamManager;
	}
}
