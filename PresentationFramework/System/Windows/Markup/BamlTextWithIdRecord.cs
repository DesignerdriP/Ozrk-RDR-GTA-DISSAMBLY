using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000201 RID: 513
	internal class BamlTextWithIdRecord : BamlTextRecord
	{
		// Token: 0x0600203B RID: 8251 RVA: 0x00095E8F File Offset: 0x0009408F
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.ValueId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x00095E9D File Offset: 0x0009409D
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.ValueId);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x00095EAC File Offset: 0x000940AC
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTextWithIdRecord bamlTextWithIdRecord = (BamlTextWithIdRecord)record;
			bamlTextWithIdRecord._valueId = this._valueId;
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x00095ED3 File Offset: 0x000940D3
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TextWithId;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x00095ED7 File Offset: 0x000940D7
		// (set) Token: 0x06002040 RID: 8256 RVA: 0x00095EDF File Offset: 0x000940DF
		internal short ValueId
		{
			get
			{
				return this._valueId;
			}
			set
			{
				this._valueId = value;
			}
		}

		// Token: 0x04001547 RID: 5447
		private short _valueId;
	}
}
