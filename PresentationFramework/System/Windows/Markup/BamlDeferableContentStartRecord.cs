using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001FA RID: 506
	internal class BamlDeferableContentStartRecord : BamlRecord
	{
		// Token: 0x06002004 RID: 8196 RVA: 0x00095A95 File Offset: 0x00093C95
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ContentSize = bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x00095AA3 File Offset: 0x00093CA3
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			this._contentSizePosition = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Write(this.ContentSize);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x00095AC0 File Offset: 0x00093CC0
		internal void UpdateContentSize(int contentSize, BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			int num2 = (int)(this._contentSizePosition - num);
			bamlBinaryWriter.Seek(num2, SeekOrigin.Current);
			bamlBinaryWriter.Write(contentSize);
			bamlBinaryWriter.Seek((int)(-4L - (long)num2), SeekOrigin.Current);
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00095B00 File Offset: 0x00093D00
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDeferableContentStartRecord bamlDeferableContentStartRecord = (BamlDeferableContentStartRecord)record;
			bamlDeferableContentStartRecord._contentSize = this._contentSize;
			bamlDeferableContentStartRecord._contentSizePosition = this._contentSizePosition;
			bamlDeferableContentStartRecord._valuesBuffer = this._valuesBuffer;
		}

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x00095B3F File Offset: 0x00093D3F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DeferableContentStart;
			}
		}

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x00095B43 File Offset: 0x00093D43
		// (set) Token: 0x0600200A RID: 8202 RVA: 0x00095B4B File Offset: 0x00093D4B
		internal int ContentSize
		{
			get
			{
				return this._contentSize;
			}
			set
			{
				this._contentSize = value;
			}
		}

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x0600200B RID: 8203 RVA: 0x00094BDC File Offset: 0x00092DDC
		// (set) Token: 0x0600200C RID: 8204 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
			set
			{
			}
		}

		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x0600200D RID: 8205 RVA: 0x00095B54 File Offset: 0x00093D54
		// (set) Token: 0x0600200E RID: 8206 RVA: 0x00095B5C File Offset: 0x00093D5C
		internal byte[] ValuesBuffer
		{
			get
			{
				return this._valuesBuffer;
			}
			set
			{
				this._valuesBuffer = value;
			}
		}

		// Token: 0x0400153C RID: 5436
		private const long ContentSizeSize = 4L;

		// Token: 0x0400153D RID: 5437
		private int _contentSize = -1;

		// Token: 0x0400153E RID: 5438
		private long _contentSizePosition = -1L;

		// Token: 0x0400153F RID: 5439
		private byte[] _valuesBuffer;
	}
}
