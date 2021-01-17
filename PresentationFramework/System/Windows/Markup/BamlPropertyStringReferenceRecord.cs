using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E4 RID: 484
	internal class BamlPropertyStringReferenceRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x06001F67 RID: 8039 RVA: 0x00094B69 File Offset: 0x00092D69
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.AttributeId = bamlBinaryReader.ReadInt16();
			this.StringId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x00094B83 File Offset: 0x00092D83
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.AttributeId);
			bamlBinaryWriter.Write(this.StringId);
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00094BA0 File Offset: 0x00092DA0
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyStringReferenceRecord bamlPropertyStringReferenceRecord = (BamlPropertyStringReferenceRecord)record;
			bamlPropertyStringReferenceRecord._stringId = this._stringId;
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x00094BC7 File Offset: 0x00092DC7
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyStringReference;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06001F6B RID: 8043 RVA: 0x00094BCB File Offset: 0x00092DCB
		// (set) Token: 0x06001F6C RID: 8044 RVA: 0x00094BD3 File Offset: 0x00092DD3
		internal short StringId
		{
			get
			{
				return this._stringId;
			}
			set
			{
				this._stringId = value;
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06001F6D RID: 8045 RVA: 0x00094BDC File Offset: 0x00092DDC
		// (set) Token: 0x06001F6E RID: 8046 RVA: 0x00002137 File Offset: 0x00000337
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

		// Token: 0x0400151A RID: 5402
		private short _stringId;
	}
}
