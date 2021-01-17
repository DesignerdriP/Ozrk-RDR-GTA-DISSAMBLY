using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000202 RID: 514
	internal class BamlTextWithConverterRecord : BamlTextRecord
	{
		// Token: 0x06002042 RID: 8258 RVA: 0x00095EF0 File Offset: 0x000940F0
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.ConverterTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x00095F05 File Offset: 0x00094105
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.ConverterTypeId);
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x00095F1C File Offset: 0x0009411C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTextWithConverterRecord bamlTextWithConverterRecord = (BamlTextWithConverterRecord)record;
			bamlTextWithConverterRecord._converterTypeId = this._converterTypeId;
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x00095F43 File Offset: 0x00094143
		// (set) Token: 0x06002046 RID: 8262 RVA: 0x00095F4B File Offset: 0x0009414B
		internal short ConverterTypeId
		{
			get
			{
				return this._converterTypeId;
			}
			set
			{
				this._converterTypeId = value;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002047 RID: 8263 RVA: 0x00095F54 File Offset: 0x00094154
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TextWithConverter;
			}
		}

		// Token: 0x04001548 RID: 5448
		private short _converterTypeId;
	}
}
