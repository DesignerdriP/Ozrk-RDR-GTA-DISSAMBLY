using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200020A RID: 522
	internal class BamlTypeInfoWithSerializerRecord : BamlTypeInfoRecord
	{
		// Token: 0x06002080 RID: 8320 RVA: 0x00096433 File Offset: 0x00094633
		internal BamlTypeInfoWithSerializerRecord()
		{
			base.Pin();
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00096441 File Offset: 0x00094641
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.SerializerTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00096456 File Offset: 0x00094656
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.SerializerTypeId);
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x0009646C File Offset: 0x0009466C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTypeInfoWithSerializerRecord bamlTypeInfoWithSerializerRecord = (BamlTypeInfoWithSerializerRecord)record;
			bamlTypeInfoWithSerializerRecord._serializerTypeId = this._serializerTypeId;
			bamlTypeInfoWithSerializerRecord._serializerType = this._serializerType;
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002084 RID: 8324 RVA: 0x0009649F File Offset: 0x0009469F
		// (set) Token: 0x06002085 RID: 8325 RVA: 0x000964A7 File Offset: 0x000946A7
		internal short SerializerTypeId
		{
			get
			{
				return this._serializerTypeId;
			}
			set
			{
				this._serializerTypeId = value;
			}
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002086 RID: 8326 RVA: 0x000964B0 File Offset: 0x000946B0
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TypeSerializerInfo;
			}
		}

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x000964B4 File Offset: 0x000946B4
		// (set) Token: 0x06002088 RID: 8328 RVA: 0x000964BC File Offset: 0x000946BC
		internal Type SerializerType
		{
			get
			{
				return this._serializerType;
			}
			set
			{
				this._serializerType = value;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002089 RID: 8329 RVA: 0x00016748 File Offset: 0x00014948
		internal override bool HasSerializer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001557 RID: 5463
		private short _serializerTypeId;

		// Token: 0x04001558 RID: 5464
		private Type _serializerType;
	}
}
