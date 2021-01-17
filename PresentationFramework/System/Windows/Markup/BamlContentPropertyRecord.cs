using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200020D RID: 525
	internal class BamlContentPropertyRecord : BamlRecord
	{
		// Token: 0x060020BC RID: 8380 RVA: 0x00096A35 File Offset: 0x00094C35
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00096A43 File Offset: 0x00094C43
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00096A54 File Offset: 0x00094C54
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlContentPropertyRecord bamlContentPropertyRecord = (BamlContentPropertyRecord)record;
			bamlContentPropertyRecord._attributeId = this._attributeId;
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x00096A7B File Offset: 0x00094C7B
		// (set) Token: 0x060020C0 RID: 8384 RVA: 0x00096A83 File Offset: 0x00094C83
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

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060020C1 RID: 8385 RVA: 0x00096A8C File Offset: 0x00094C8C
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ContentProperty;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool HasSerializer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04001569 RID: 5481
		private short _attributeId = -1;
	}
}
