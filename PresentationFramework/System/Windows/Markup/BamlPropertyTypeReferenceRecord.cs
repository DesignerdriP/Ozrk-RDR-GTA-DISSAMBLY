using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001E5 RID: 485
	internal class BamlPropertyTypeReferenceRecord : BamlPropertyComplexStartRecord
	{
		// Token: 0x06001F70 RID: 8048 RVA: 0x00094BE7 File Offset: 0x00092DE7
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.AttributeId = bamlBinaryReader.ReadInt16();
			this.TypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x00094C01 File Offset: 0x00092E01
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.AttributeId);
			bamlBinaryWriter.Write(this.TypeId);
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x00094C1C File Offset: 0x00092E1C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlPropertyTypeReferenceRecord bamlPropertyTypeReferenceRecord = (BamlPropertyTypeReferenceRecord)record;
			bamlPropertyTypeReferenceRecord._typeId = this._typeId;
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06001F73 RID: 8051 RVA: 0x00094C43 File Offset: 0x00092E43
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.PropertyTypeReference;
			}
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x00094C47 File Offset: 0x00092E47
		// (set) Token: 0x06001F75 RID: 8053 RVA: 0x00094C4F File Offset: 0x00092E4F
		internal short TypeId
		{
			get
			{
				return this._typeId;
			}
			set
			{
				this._typeId = value;
			}
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x00094BDC File Offset: 0x00092DDC
		// (set) Token: 0x06001F77 RID: 8055 RVA: 0x00002137 File Offset: 0x00000337
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

		// Token: 0x0400151B RID: 5403
		private short _typeId;
	}
}
