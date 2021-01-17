using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001FE RID: 510
	internal class BamlStaticResourceIdRecord : BamlRecord
	{
		// Token: 0x06002025 RID: 8229 RVA: 0x00095D21 File Offset: 0x00093F21
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.StaticResourceId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x00095D2F File Offset: 0x00093F2F
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.StaticResourceId);
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x00095D40 File Offset: 0x00093F40
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlStaticResourceIdRecord bamlStaticResourceIdRecord = (BamlStaticResourceIdRecord)record;
			bamlStaticResourceIdRecord._staticResourceId = this._staticResourceId;
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x00095D67 File Offset: 0x00093F67
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StaticResourceId;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x00094B24 File Offset: 0x00092D24
		// (set) Token: 0x0600202A RID: 8234 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 2;
			}
			set
			{
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x00095D6B File Offset: 0x00093F6B
		// (set) Token: 0x0600202C RID: 8236 RVA: 0x00095D73 File Offset: 0x00093F73
		internal short StaticResourceId
		{
			get
			{
				return this._staticResourceId;
			}
			set
			{
				this._staticResourceId = value;
			}
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x00095D7C File Offset: 0x00093F7C
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} staticResourceId({1})", new object[]
			{
				this.RecordType,
				this.StaticResourceId
			});
		}

		// Token: 0x04001545 RID: 5445
		private short _staticResourceId = -1;
	}
}
