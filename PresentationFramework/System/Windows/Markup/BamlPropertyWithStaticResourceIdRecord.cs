using System;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001FF RID: 511
	internal class BamlPropertyWithStaticResourceIdRecord : BamlStaticResourceIdRecord
	{
		// Token: 0x0600202F RID: 8239 RVA: 0x00095DBE File Offset: 0x00093FBE
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			base.StaticResourceId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x00095DD8 File Offset: 0x00093FD8
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(base.StaticResourceId);
		}

		// Token: 0x06002031 RID: 8241 RVA: 0x00095DF4 File Offset: 0x00093FF4
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyWithStaticResourceIdRecord bamlPropertyWithStaticResourceIdRecord = (BamlPropertyWithStaticResourceIdRecord)record;
			bamlPropertyWithStaticResourceIdRecord._attributeId = this._attributeId;
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002032 RID: 8242 RVA: 0x00095E1B File Offset: 0x0009401B
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyWithStaticResourceId;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x00094BDC File Offset: 0x00092DDC
		// (set) Token: 0x06002034 RID: 8244 RVA: 0x00002137 File Offset: 0x00000337
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

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002035 RID: 8245 RVA: 0x00095E1F File Offset: 0x0009401F
		// (set) Token: 0x06002036 RID: 8246 RVA: 0x00095E27 File Offset: 0x00094027
		internal short AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x00095E30 File Offset: 0x00094030
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} attr({1}) staticResourceId({2})", new object[]
			{
				this.RecordType,
				this.AttributeId,
				base.StaticResourceId
			});
		}

		// Token: 0x04001546 RID: 5446
		private short _attributeId = -1;
	}
}
