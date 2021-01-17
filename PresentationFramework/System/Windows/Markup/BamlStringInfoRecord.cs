using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x0200020C RID: 524
	internal class BamlStringInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x060020AF RID: 8367 RVA: 0x000968D7 File Offset: 0x00094AD7
		internal BamlStringInfoRecord()
		{
			base.Pin();
			this.StringId = -1;
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000968EC File Offset: 0x00094AEC
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.StringId = bamlBinaryReader.ReadInt16();
			this.Value = bamlBinaryReader.ReadString();
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x00096906 File Offset: 0x00094B06
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.StringId);
			bamlBinaryWriter.Write(this.Value);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00096920 File Offset: 0x00094B20
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlStringInfoRecord bamlStringInfoRecord = (BamlStringInfoRecord)record;
			bamlStringInfoRecord._value = this._value;
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x060020B3 RID: 8371 RVA: 0x00096948 File Offset: 0x00094B48
		// (set) Token: 0x060020B4 RID: 8372 RVA: 0x0009697F File Offset: 0x00094B7F
		internal short StringId
		{
			get
			{
				short num = (short)this._flags[BamlStringInfoRecord._stringIdLowSection];
				return num | (short)(this._flags[BamlStringInfoRecord._stringIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlStringInfoRecord._stringIdLowSection] = (int)(value & 255);
				this._flags[BamlStringInfoRecord._stringIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060020B5 RID: 8373 RVA: 0x000969B3 File Offset: 0x00094BB3
		// (set) Token: 0x060020B6 RID: 8374 RVA: 0x000969BB File Offset: 0x00094BBB
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

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060020B7 RID: 8375 RVA: 0x000969C4 File Offset: 0x00094BC4
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.StringInfo;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060020B8 RID: 8376 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool HasSerializer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000969C8 File Offset: 0x00094BC8
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} stringId({1}='{2}'", new object[]
			{
				this.RecordType,
				this.StringId,
				this._value
			});
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x00096A04 File Offset: 0x00094C04
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlStringInfoRecord._stringIdHighSection;
			}
		}

		// Token: 0x04001566 RID: 5478
		private static BitVector32.Section _stringIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001567 RID: 5479
		private static BitVector32.Section _stringIdHighSection = BitVector32.CreateSection(255, BamlStringInfoRecord._stringIdLowSection);

		// Token: 0x04001568 RID: 5480
		private string _value;
	}
}
