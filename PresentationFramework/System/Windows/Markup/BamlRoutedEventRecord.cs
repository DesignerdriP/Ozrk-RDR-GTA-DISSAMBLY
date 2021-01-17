using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001F5 RID: 501
	internal class BamlRoutedEventRecord : BamlStringValueRecord
	{
		// Token: 0x06001FD9 RID: 8153 RVA: 0x000956F7 File Offset: 0x000938F7
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			base.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00095711 File Offset: 0x00093911
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(base.Value);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0009572C File Offset: 0x0009392C
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlRoutedEventRecord bamlRoutedEventRecord = (BamlRoutedEventRecord)record;
			bamlRoutedEventRecord._attributeId = this._attributeId;
		}

		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06001FDC RID: 8156 RVA: 0x00095753 File Offset: 0x00093953
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.RoutedEvent;
			}
		}

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x00095757 File Offset: 0x00093957
		// (set) Token: 0x06001FDE RID: 8158 RVA: 0x0009575F File Offset: 0x0009395F
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

		// Token: 0x04001534 RID: 5428
		private short _attributeId = -1;
	}
}
