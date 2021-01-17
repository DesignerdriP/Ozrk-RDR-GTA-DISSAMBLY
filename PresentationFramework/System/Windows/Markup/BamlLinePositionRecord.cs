using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200020F RID: 527
	internal class BamlLinePositionRecord : BamlRecord
	{
		// Token: 0x060020CF RID: 8399 RVA: 0x00096B7C File Offset: 0x00094D7C
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.LinePosition = (uint)bamlBinaryReader.ReadInt32();
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00096B8A File Offset: 0x00094D8A
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.LinePosition);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x00096B98 File Offset: 0x00094D98
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlLinePositionRecord bamlLinePositionRecord = (BamlLinePositionRecord)record;
			bamlLinePositionRecord._linePosition = this._linePosition;
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060020D2 RID: 8402 RVA: 0x00096BBF File Offset: 0x00094DBF
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.LinePosition;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060020D3 RID: 8403 RVA: 0x00096BC3 File Offset: 0x00094DC3
		// (set) Token: 0x060020D4 RID: 8404 RVA: 0x00096BCB File Offset: 0x00094DCB
		internal uint LinePosition
		{
			get
			{
				return this._linePosition;
			}
			set
			{
				this._linePosition = value;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060020D5 RID: 8405 RVA: 0x00094BDC File Offset: 0x00092DDC
		internal override int RecordSize
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00096BD4 File Offset: 0x00094DD4
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} LinePos={1}", new object[]
			{
				this.RecordType,
				this.LinePosition
			});
		}

		// Token: 0x0400156C RID: 5484
		private uint _linePosition;
	}
}
