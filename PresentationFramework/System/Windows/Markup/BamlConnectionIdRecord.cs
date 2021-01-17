using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001F7 RID: 503
	internal class BamlConnectionIdRecord : BamlRecord
	{
		// Token: 0x06001FE4 RID: 8164 RVA: 0x000957BF File Offset: 0x000939BF
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ConnectionId = bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000957CD File Offset: 0x000939CD
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.ConnectionId);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000957DC File Offset: 0x000939DC
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlConnectionIdRecord bamlConnectionIdRecord = (BamlConnectionIdRecord)record;
			bamlConnectionIdRecord._connectionId = this._connectionId;
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06001FE7 RID: 8167 RVA: 0x00095803 File Offset: 0x00093A03
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ConnectionId;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06001FE8 RID: 8168 RVA: 0x00095807 File Offset: 0x00093A07
		// (set) Token: 0x06001FE9 RID: 8169 RVA: 0x0009580F File Offset: 0x00093A0F
		internal int ConnectionId
		{
			get
			{
				return this._connectionId;
			}
			set
			{
				this._connectionId = value;
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06001FEA RID: 8170 RVA: 0x00094BDC File Offset: 0x00092DDC
		// (set) Token: 0x06001FEB RID: 8171 RVA: 0x00002137 File Offset: 0x00000337
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

		// Token: 0x04001535 RID: 5429
		private int _connectionId = -1;
	}
}
