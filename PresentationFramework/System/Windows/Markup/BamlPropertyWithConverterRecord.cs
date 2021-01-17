using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E6 RID: 486
	internal class BamlPropertyWithConverterRecord : BamlPropertyRecord
	{
		// Token: 0x06001F79 RID: 8057 RVA: 0x00094C58 File Offset: 0x00092E58
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this.ConverterTypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00094C6D File Offset: 0x00092E6D
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			bamlBinaryWriter.Write(this.ConverterTypeId);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00094C84 File Offset: 0x00092E84
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyWithConverterRecord bamlPropertyWithConverterRecord = (BamlPropertyWithConverterRecord)record;
			bamlPropertyWithConverterRecord._converterTypeId = this._converterTypeId;
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x00094CAB File Offset: 0x00092EAB
		// (set) Token: 0x06001F7D RID: 8061 RVA: 0x00094CB3 File Offset: 0x00092EB3
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

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06001F7E RID: 8062 RVA: 0x00094CBC File Offset: 0x00092EBC
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyWithConverter;
			}
		}

		// Token: 0x0400151C RID: 5404
		private short _converterTypeId;
	}
}
