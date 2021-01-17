using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001EE RID: 494
	internal class BamlConstructorParameterTypeRecord : BamlRecord
	{
		// Token: 0x06001FC4 RID: 8132 RVA: 0x0009568C File Offset: 0x0009388C
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x0009569A File Offset: 0x0009389A
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000956A8 File Offset: 0x000938A8
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlConstructorParameterTypeRecord bamlConstructorParameterTypeRecord = (BamlConstructorParameterTypeRecord)record;
			bamlConstructorParameterTypeRecord._typeId = this._typeId;
		}

		// Token: 0x1700078F RID: 1935
		// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x000956CF File Offset: 0x000938CF
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.ConstructorParameterType;
			}
		}

		// Token: 0x17000790 RID: 1936
		// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x000956D3 File Offset: 0x000938D3
		// (set) Token: 0x06001FC9 RID: 8137 RVA: 0x000956DB File Offset: 0x000938DB
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

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x00094B24 File Offset: 0x00092D24
		// (set) Token: 0x06001FCB RID: 8139 RVA: 0x00002137 File Offset: 0x00000337
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

		// Token: 0x04001533 RID: 5427
		private short _typeId;
	}
}
