using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001F6 RID: 502
	internal class BamlLiteralContentRecord : BamlStringValueRecord
	{
		// Token: 0x06001FE0 RID: 8160 RVA: 0x00095778 File Offset: 0x00093978
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.Value = bamlBinaryReader.ReadString();
			int num = bamlBinaryReader.ReadInt32();
			int num2 = bamlBinaryReader.ReadInt32();
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0009579F File Offset: 0x0009399F
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.Value);
			bamlBinaryWriter.Write(0);
			bamlBinaryWriter.Write(0);
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06001FE2 RID: 8162 RVA: 0x000957BB File Offset: 0x000939BB
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.LiteralContent;
			}
		}
	}
}
