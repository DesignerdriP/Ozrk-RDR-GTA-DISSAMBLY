﻿using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001FD RID: 509
	internal class BamlOptimizedStaticResourceRecord : BamlRecord, IOptimizedMarkupExtension
	{
		// Token: 0x06002014 RID: 8212 RVA: 0x00095B8C File Offset: 0x00093D8C
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			byte b = bamlBinaryReader.ReadByte();
			this.ValueId = bamlBinaryReader.ReadInt16();
			this.IsValueTypeExtension = ((b & BamlOptimizedStaticResourceRecord.TypeExtensionValueMask) > 0);
			this.IsValueStaticExtension = ((b & BamlOptimizedStaticResourceRecord.StaticExtensionValueMask) > 0);
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x00095BCC File Offset: 0x00093DCC
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			byte b = 0;
			if (this.IsValueTypeExtension)
			{
				b |= BamlOptimizedStaticResourceRecord.TypeExtensionValueMask;
			}
			else if (this.IsValueStaticExtension)
			{
				b |= BamlOptimizedStaticResourceRecord.StaticExtensionValueMask;
			}
			bamlBinaryWriter.Write(b);
			bamlBinaryWriter.Write(this.ValueId);
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x00095C14 File Offset: 0x00093E14
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlOptimizedStaticResourceRecord bamlOptimizedStaticResourceRecord = (BamlOptimizedStaticResourceRecord)record;
			bamlOptimizedStaticResourceRecord._valueId = this._valueId;
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002017 RID: 8215 RVA: 0x00095C3B File Offset: 0x00093E3B
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.OptimizedStaticResource;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x00095C3F File Offset: 0x00093E3F
		public short ExtensionTypeId
		{
			get
			{
				return 603;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x00095C46 File Offset: 0x00093E46
		// (set) Token: 0x0600201A RID: 8218 RVA: 0x00095C4E File Offset: 0x00093E4E
		public short ValueId
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

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600201B RID: 8219 RVA: 0x0007FFD2 File Offset: 0x0007E1D2
		// (set) Token: 0x0600201C RID: 8220 RVA: 0x00002137 File Offset: 0x00000337
		internal override int RecordSize
		{
			get
			{
				return 3;
			}
			set
			{
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x00095C57 File Offset: 0x00093E57
		// (set) Token: 0x0600201E RID: 8222 RVA: 0x00095C6F File Offset: 0x00093E6F
		public bool IsValueTypeExtension
		{
			get
			{
				return this._flags[BamlOptimizedStaticResourceRecord._isValueTypeExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlOptimizedStaticResourceRecord._isValueTypeExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600201F RID: 8223 RVA: 0x00095C88 File Offset: 0x00093E88
		// (set) Token: 0x06002020 RID: 8224 RVA: 0x00095CA0 File Offset: 0x00093EA0
		public bool IsValueStaticExtension
		{
			get
			{
				return this._flags[BamlOptimizedStaticResourceRecord._isValueStaticExtensionSection] == 1;
			}
			set
			{
				this._flags[BamlOptimizedStaticResourceRecord._isValueStaticExtensionSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06002021 RID: 8225 RVA: 0x00095CB9 File Offset: 0x00093EB9
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlOptimizedStaticResourceRecord._isValueStaticExtensionSection;
			}
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x00095CC0 File Offset: 0x00093EC0
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} extn(StaticResourceExtension) valueId({1})", new object[]
			{
				this.RecordType,
				this._valueId
			});
		}

		// Token: 0x04001540 RID: 5440
		private short _valueId;

		// Token: 0x04001541 RID: 5441
		private static readonly byte TypeExtensionValueMask = 1;

		// Token: 0x04001542 RID: 5442
		private static readonly byte StaticExtensionValueMask = 2;

		// Token: 0x04001543 RID: 5443
		private static BitVector32.Section _isValueTypeExtensionSection = BitVector32.CreateSection(1, BamlRecord.LastFlagsSection);

		// Token: 0x04001544 RID: 5444
		private static BitVector32.Section _isValueStaticExtensionSection = BitVector32.CreateSection(1, BamlOptimizedStaticResourceRecord._isValueTypeExtensionSection);
	}
}
