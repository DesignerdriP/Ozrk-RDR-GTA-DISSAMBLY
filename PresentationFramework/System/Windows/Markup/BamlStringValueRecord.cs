using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001DC RID: 476
	internal abstract class BamlStringValueRecord : BamlVariableSizedRecord
	{
		// Token: 0x06001F06 RID: 7942 RVA: 0x000944B4 File Offset: 0x000926B4
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x000944C2 File Offset: 0x000926C2
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.Value);
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x000944D0 File Offset: 0x000926D0
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlStringValueRecord bamlStringValueRecord = (BamlStringValueRecord)record;
			bamlStringValueRecord._value = this._value;
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x000944F7 File Offset: 0x000926F7
		// (set) Token: 0x06001F0A RID: 7946 RVA: 0x000944FF File Offset: 0x000926FF
		internal string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x04001504 RID: 5380
		private string _value;
	}
}
