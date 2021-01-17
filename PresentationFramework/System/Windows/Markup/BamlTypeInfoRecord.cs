using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x02000209 RID: 521
	internal class BamlTypeInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x0600206D RID: 8301 RVA: 0x000961FE File Offset: 0x000943FE
		internal BamlTypeInfoRecord()
		{
			base.Pin();
			this.TypeId = -1;
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x0009621C File Offset: 0x0009441C
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.TypeId = bamlBinaryReader.ReadInt16();
			this.AssemblyId = bamlBinaryReader.ReadInt16();
			this.TypeFullName = bamlBinaryReader.ReadString();
			this._typeInfoFlags = (BamlTypeInfoRecord.TypeInfoFlags)(this.AssemblyId >> 12);
			this._assemblyId &= 4095;
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x00096270 File Offset: 0x00094470
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.TypeId);
			bamlBinaryWriter.Write((short)((ushort)this.AssemblyId | (ushort)(this._typeInfoFlags << 12)));
			bamlBinaryWriter.Write(this.TypeFullName);
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x000962A4 File Offset: 0x000944A4
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlTypeInfoRecord bamlTypeInfoRecord = (BamlTypeInfoRecord)record;
			bamlTypeInfoRecord._typeInfoFlags = this._typeInfoFlags;
			bamlTypeInfoRecord._assemblyId = this._assemblyId;
			bamlTypeInfoRecord._typeFullName = this._typeFullName;
			bamlTypeInfoRecord._type = this._type;
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06002071 RID: 8305 RVA: 0x000962F0 File Offset: 0x000944F0
		// (set) Token: 0x06002072 RID: 8306 RVA: 0x00096327 File Offset: 0x00094527
		internal short TypeId
		{
			get
			{
				short num = (short)this._flags[BamlTypeInfoRecord._typeIdLowSection];
				return num | (short)(this._flags[BamlTypeInfoRecord._typeIdHighSection] << 8);
			}
			set
			{
				this._flags[BamlTypeInfoRecord._typeIdLowSection] = (int)(value & 255);
				this._flags[BamlTypeInfoRecord._typeIdHighSection] = (int)((short)(((int)value & 65280) >> 8));
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06002073 RID: 8307 RVA: 0x0009635B File Offset: 0x0009455B
		// (set) Token: 0x06002074 RID: 8308 RVA: 0x00096363 File Offset: 0x00094563
		internal short AssemblyId
		{
			get
			{
				return this._assemblyId;
			}
			set
			{
				if (this._assemblyId > 4095)
				{
					throw new XamlParseException(SR.Get("ParserTooManyAssemblies"));
				}
				this._assemblyId = value;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06002075 RID: 8309 RVA: 0x00096389 File Offset: 0x00094589
		// (set) Token: 0x06002076 RID: 8310 RVA: 0x00096391 File Offset: 0x00094591
		internal string TypeFullName
		{
			get
			{
				return this._typeFullName;
			}
			set
			{
				this._typeFullName = value;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x0009639A File Offset: 0x0009459A
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.TypeInfo;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x0009639E File Offset: 0x0009459E
		// (set) Token: 0x06002079 RID: 8313 RVA: 0x000963A6 File Offset: 0x000945A6
		internal Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x000963B0 File Offset: 0x000945B0
		internal string ClrNamespace
		{
			get
			{
				int num = this._typeFullName.LastIndexOf('.');
				if (num <= 0)
				{
					return string.Empty;
				}
				return this._typeFullName.Substring(0, num);
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x0600207B RID: 8315 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool HasSerializer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x0600207C RID: 8316 RVA: 0x000963E2 File Offset: 0x000945E2
		// (set) Token: 0x0600207D RID: 8317 RVA: 0x000963EF File Offset: 0x000945EF
		internal bool IsInternalType
		{
			get
			{
				return (this._typeInfoFlags & BamlTypeInfoRecord.TypeInfoFlags.Internal) == BamlTypeInfoRecord.TypeInfoFlags.Internal;
			}
			set
			{
				if (value)
				{
					this._typeInfoFlags |= BamlTypeInfoRecord.TypeInfoFlags.Internal;
				}
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x0600207E RID: 8318 RVA: 0x00096402 File Offset: 0x00094602
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlTypeInfoRecord._typeIdHighSection;
			}
		}

		// Token: 0x04001551 RID: 5457
		private static BitVector32.Section _typeIdLowSection = BitVector32.CreateSection(255, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x04001552 RID: 5458
		private static BitVector32.Section _typeIdHighSection = BitVector32.CreateSection(255, BamlTypeInfoRecord._typeIdLowSection);

		// Token: 0x04001553 RID: 5459
		private BamlTypeInfoRecord.TypeInfoFlags _typeInfoFlags;

		// Token: 0x04001554 RID: 5460
		private short _assemblyId = -1;

		// Token: 0x04001555 RID: 5461
		private string _typeFullName;

		// Token: 0x04001556 RID: 5462
		private Type _type;

		// Token: 0x0200088E RID: 2190
		[Flags]
		private enum TypeInfoFlags : byte
		{
			// Token: 0x04004173 RID: 16755
			Internal = 1,
			// Token: 0x04004174 RID: 16756
			UnusedTwo = 2,
			// Token: 0x04004175 RID: 16757
			UnusedThree = 4
		}
	}
}
