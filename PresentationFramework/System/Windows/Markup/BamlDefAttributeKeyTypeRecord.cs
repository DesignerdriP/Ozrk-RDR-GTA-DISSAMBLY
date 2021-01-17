﻿using System;
using System.Collections.Specialized;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001DF RID: 479
	internal class BamlDefAttributeKeyTypeRecord : BamlElementStartRecord, IBamlDictionaryKey
	{
		// Token: 0x06001F1D RID: 7965 RVA: 0x00094508 File Offset: 0x00092708
		internal BamlDefAttributeKeyTypeRecord()
		{
			base.Pin();
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x0009451E File Offset: 0x0009271E
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.LoadRecordData(bamlBinaryReader);
			this._valuePosition = bamlBinaryReader.ReadInt32();
			((IBamlDictionaryKey)this).Shared = bamlBinaryReader.ReadBoolean();
			((IBamlDictionaryKey)this).SharedSet = bamlBinaryReader.ReadBoolean();
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x0009454B File Offset: 0x0009274B
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			base.WriteRecordData(bamlBinaryWriter);
			this._valuePositionPosition = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			bamlBinaryWriter.Write(this._valuePosition);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).Shared);
			bamlBinaryWriter.Write(((IBamlDictionaryKey)this).SharedSet);
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x00094588 File Offset: 0x00092788
		void IBamlDictionaryKey.UpdateValuePosition(int newPosition, BinaryWriter bamlBinaryWriter)
		{
			long num = bamlBinaryWriter.Seek(0, SeekOrigin.Current);
			int num2 = (int)(this._valuePositionPosition - num);
			bamlBinaryWriter.Seek(num2, SeekOrigin.Current);
			bamlBinaryWriter.Write(newPosition);
			bamlBinaryWriter.Seek(-4 - num2, SeekOrigin.Current);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000945C4 File Offset: 0x000927C4
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlDefAttributeKeyTypeRecord bamlDefAttributeKeyTypeRecord = (BamlDefAttributeKeyTypeRecord)record;
			bamlDefAttributeKeyTypeRecord._valuePosition = this._valuePosition;
			bamlDefAttributeKeyTypeRecord._valuePositionPosition = this._valuePositionPosition;
			bamlDefAttributeKeyTypeRecord._keyObject = this._keyObject;
			bamlDefAttributeKeyTypeRecord._staticResourceValues = this._staticResourceValues;
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06001F22 RID: 7970 RVA: 0x0009460F File Offset: 0x0009280F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.DefAttributeKeyType;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x00094613 File Offset: 0x00092813
		// (set) Token: 0x06001F24 RID: 7972 RVA: 0x0009461B File Offset: 0x0009281B
		int IBamlDictionaryKey.ValuePosition
		{
			get
			{
				return this._valuePosition;
			}
			set
			{
				this._valuePosition = value;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06001F25 RID: 7973 RVA: 0x00094624 File Offset: 0x00092824
		// (set) Token: 0x06001F26 RID: 7974 RVA: 0x0009462C File Offset: 0x0009282C
		object IBamlDictionaryKey.KeyObject
		{
			get
			{
				return this._keyObject;
			}
			set
			{
				this._keyObject = value;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x00094635 File Offset: 0x00092835
		// (set) Token: 0x06001F28 RID: 7976 RVA: 0x0009463D File Offset: 0x0009283D
		long IBamlDictionaryKey.ValuePositionPosition
		{
			get
			{
				return this._valuePositionPosition;
			}
			set
			{
				this._valuePositionPosition = value;
			}
		}

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06001F29 RID: 7977 RVA: 0x00094646 File Offset: 0x00092846
		// (set) Token: 0x06001F2A RID: 7978 RVA: 0x0009465E File Offset: 0x0009285E
		bool IBamlDictionaryKey.Shared
		{
			get
			{
				return this._flags[BamlDefAttributeKeyTypeRecord._sharedSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyTypeRecord._sharedSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x00094677 File Offset: 0x00092877
		// (set) Token: 0x06001F2C RID: 7980 RVA: 0x0009468F File Offset: 0x0009288F
		bool IBamlDictionaryKey.SharedSet
		{
			get
			{
				return this._flags[BamlDefAttributeKeyTypeRecord._sharedSetSection] == 1;
			}
			set
			{
				this._flags[BamlDefAttributeKeyTypeRecord._sharedSetSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x06001F2D RID: 7981 RVA: 0x000946A8 File Offset: 0x000928A8
		// (set) Token: 0x06001F2E RID: 7982 RVA: 0x000946B0 File Offset: 0x000928B0
		object[] IBamlDictionaryKey.StaticResourceValues
		{
			get
			{
				return this._staticResourceValues;
			}
			set
			{
				this._staticResourceValues = value;
			}
		}

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x06001F2F RID: 7983 RVA: 0x000946B9 File Offset: 0x000928B9
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlDefAttributeKeyTypeRecord._sharedSetSection;
			}
		}

		// Token: 0x04001505 RID: 5381
		private static BitVector32.Section _sharedSection = BitVector32.CreateSection(1, BamlElementStartRecord.LastFlagsSection);

		// Token: 0x04001506 RID: 5382
		private static BitVector32.Section _sharedSetSection = BitVector32.CreateSection(1, BamlDefAttributeKeyTypeRecord._sharedSection);

		// Token: 0x04001507 RID: 5383
		internal const int ValuePositionSize = 4;

		// Token: 0x04001508 RID: 5384
		private int _valuePosition;

		// Token: 0x04001509 RID: 5385
		private long _valuePositionPosition = -1L;

		// Token: 0x0400150A RID: 5386
		private object _keyObject;

		// Token: 0x0400150B RID: 5387
		private object[] _staticResourceValues;
	}
}
