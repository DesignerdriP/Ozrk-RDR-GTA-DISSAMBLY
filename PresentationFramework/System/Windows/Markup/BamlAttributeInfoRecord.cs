﻿using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace System.Windows.Markup
{
	// Token: 0x0200020B RID: 523
	internal class BamlAttributeInfoRecord : BamlVariableSizedRecord
	{
		// Token: 0x0600208A RID: 8330 RVA: 0x000964C5 File Offset: 0x000946C5
		internal BamlAttributeInfoRecord()
		{
			base.Pin();
			this.AttributeUsage = BamlAttributeUsage.Default;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x000964DA File Offset: 0x000946DA
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.AttributeId = bamlBinaryReader.ReadInt16();
			this.OwnerTypeId = bamlBinaryReader.ReadInt16();
			this.AttributeUsage = (BamlAttributeUsage)bamlBinaryReader.ReadByte();
			this.Name = bamlBinaryReader.ReadString();
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x0009650C File Offset: 0x0009470C
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.AttributeId);
			bamlBinaryWriter.Write(this.OwnerTypeId);
			bamlBinaryWriter.Write((byte)this.AttributeUsage);
			bamlBinaryWriter.Write(this.Name);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x00096540 File Offset: 0x00094740
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlAttributeInfoRecord bamlAttributeInfoRecord = (BamlAttributeInfoRecord)record;
			bamlAttributeInfoRecord._ownerId = this._ownerId;
			bamlAttributeInfoRecord._attributeId = this._attributeId;
			bamlAttributeInfoRecord._name = this._name;
			bamlAttributeInfoRecord._ownerType = this._ownerType;
			bamlAttributeInfoRecord._Event = this._Event;
			bamlAttributeInfoRecord._dp = this._dp;
			bamlAttributeInfoRecord._ei = this._ei;
			bamlAttributeInfoRecord._pi = this._pi;
			bamlAttributeInfoRecord._smi = this._smi;
			bamlAttributeInfoRecord._gmi = this._gmi;
			bamlAttributeInfoRecord._dpOrMiOrPi = this._dpOrMiOrPi;
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x0600208E RID: 8334 RVA: 0x000965DF File Offset: 0x000947DF
		// (set) Token: 0x0600208F RID: 8335 RVA: 0x000965E7 File Offset: 0x000947E7
		internal short OwnerTypeId
		{
			get
			{
				return this._ownerId;
			}
			set
			{
				this._ownerId = value;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002091 RID: 8337 RVA: 0x000965F9 File Offset: 0x000947F9
		// (set) Token: 0x06002090 RID: 8336 RVA: 0x000965F0 File Offset: 0x000947F0
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

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002092 RID: 8338 RVA: 0x00096601 File Offset: 0x00094801
		// (set) Token: 0x06002093 RID: 8339 RVA: 0x00096609 File Offset: 0x00094809
		internal string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002094 RID: 8340 RVA: 0x00096612 File Offset: 0x00094812
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.AttributeInfo;
			}
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00096618 File Offset: 0x00094818
		internal Type GetPropertyType()
		{
			DependencyProperty dp = this.DP;
			Type result;
			if (dp == null)
			{
				MethodInfo attachedPropertySetter = this.AttachedPropertySetter;
				if (attachedPropertySetter == null)
				{
					PropertyInfo propInfo = this.PropInfo;
					result = propInfo.PropertyType;
				}
				else
				{
					ParameterInfo[] parameters = attachedPropertySetter.GetParameters();
					result = parameters[1].ParameterType;
				}
			}
			else
			{
				result = dp.PropertyType;
			}
			return result;
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00096670 File Offset: 0x00094870
		internal void SetPropertyMember(object propertyMember)
		{
			if (this.PropertyMember == null)
			{
				this.PropertyMember = propertyMember;
				return;
			}
			object[] array = this.PropertyMember as object[];
			if (array == null)
			{
				array = new object[3];
				array[0] = this.PropertyMember;
				array[1] = propertyMember;
				return;
			}
			array[2] = propertyMember;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x000966B8 File Offset: 0x000948B8
		internal object GetPropertyMember(bool onlyPropInfo)
		{
			if (this.PropertyMember == null || this.PropertyMember is MemberInfo || KnownTypes.Types[136].IsAssignableFrom(this.PropertyMember.GetType()))
			{
				if (onlyPropInfo)
				{
					return this.PropInfo;
				}
				return this.PropertyMember;
			}
			else
			{
				object[] array = (object[])this.PropertyMember;
				if (!onlyPropInfo)
				{
					return array[0];
				}
				if (array[0] is PropertyInfo)
				{
					return (PropertyInfo)array[0];
				}
				if (array[1] is PropertyInfo)
				{
					return (PropertyInfo)array[1];
				}
				return array[2] as PropertyInfo;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06002098 RID: 8344 RVA: 0x0009674D File Offset: 0x0009494D
		// (set) Token: 0x06002099 RID: 8345 RVA: 0x00096755 File Offset: 0x00094955
		internal object PropertyMember
		{
			get
			{
				return this._dpOrMiOrPi;
			}
			set
			{
				this._dpOrMiOrPi = value;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x0600209A RID: 8346 RVA: 0x0009675E File Offset: 0x0009495E
		// (set) Token: 0x0600209B RID: 8347 RVA: 0x00096766 File Offset: 0x00094966
		internal Type OwnerType
		{
			get
			{
				return this._ownerType;
			}
			set
			{
				this._ownerType = value;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600209C RID: 8348 RVA: 0x0009676F File Offset: 0x0009496F
		// (set) Token: 0x0600209D RID: 8349 RVA: 0x00096777 File Offset: 0x00094977
		internal RoutedEvent Event
		{
			get
			{
				return this._Event;
			}
			set
			{
				this._Event = value;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x0600209E RID: 8350 RVA: 0x00096780 File Offset: 0x00094980
		// (set) Token: 0x0600209F RID: 8351 RVA: 0x0009679C File Offset: 0x0009499C
		internal DependencyProperty DP
		{
			get
			{
				if (this._dp != null)
				{
					return this._dp;
				}
				return this._dpOrMiOrPi as DependencyProperty;
			}
			set
			{
				this._dp = value;
				if (this._dp != null)
				{
					this._name = this._dp.Name;
				}
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x060020A0 RID: 8352 RVA: 0x000967BE File Offset: 0x000949BE
		// (set) Token: 0x060020A1 RID: 8353 RVA: 0x000967C6 File Offset: 0x000949C6
		internal MethodInfo AttachedPropertySetter
		{
			get
			{
				return this._smi;
			}
			set
			{
				this._smi = value;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060020A2 RID: 8354 RVA: 0x000967CF File Offset: 0x000949CF
		// (set) Token: 0x060020A3 RID: 8355 RVA: 0x000967D7 File Offset: 0x000949D7
		internal MethodInfo AttachedPropertyGetter
		{
			get
			{
				return this._gmi;
			}
			set
			{
				this._gmi = value;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060020A4 RID: 8356 RVA: 0x000967E0 File Offset: 0x000949E0
		// (set) Token: 0x060020A5 RID: 8357 RVA: 0x000967E8 File Offset: 0x000949E8
		internal EventInfo EventInfo
		{
			get
			{
				return this._ei;
			}
			set
			{
				this._ei = value;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x000967F1 File Offset: 0x000949F1
		// (set) Token: 0x060020A7 RID: 8359 RVA: 0x000967F9 File Offset: 0x000949F9
		internal PropertyInfo PropInfo
		{
			get
			{
				return this._pi;
			}
			set
			{
				this._pi = value;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060020A8 RID: 8360 RVA: 0x00096802 File Offset: 0x00094A02
		// (set) Token: 0x060020A9 RID: 8361 RVA: 0x0009681A File Offset: 0x00094A1A
		internal bool IsInternal
		{
			get
			{
				return this._flags[BamlAttributeInfoRecord._isInternalSection] == 1;
			}
			set
			{
				this._flags[BamlAttributeInfoRecord._isInternalSection] = (value ? 1 : 0);
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060020AA RID: 8362 RVA: 0x00096833 File Offset: 0x00094A33
		// (set) Token: 0x060020AB RID: 8363 RVA: 0x00096846 File Offset: 0x00094A46
		internal BamlAttributeUsage AttributeUsage
		{
			get
			{
				return (BamlAttributeUsage)this._flags[BamlAttributeInfoRecord._attributeUsageSection];
			}
			set
			{
				this._flags[BamlAttributeInfoRecord._attributeUsageSection] = (int)value;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x060020AC RID: 8364 RVA: 0x00096859 File Offset: 0x00094A59
		internal new static BitVector32.Section LastFlagsSection
		{
			get
			{
				return BamlAttributeInfoRecord._attributeUsageSection;
			}
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00096860 File Offset: 0x00094A60
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} owner={1} attr({2}) is '{3}'", new object[]
			{
				this.RecordType,
				BamlRecord.GetTypeName((int)this.OwnerTypeId),
				this.AttributeId,
				this._name
			});
		}

		// Token: 0x04001559 RID: 5465
		private static BitVector32.Section _isInternalSection = BitVector32.CreateSection(1, BamlVariableSizedRecord.LastFlagsSection);

		// Token: 0x0400155A RID: 5466
		private static BitVector32.Section _attributeUsageSection = BitVector32.CreateSection(3, BamlAttributeInfoRecord._isInternalSection);

		// Token: 0x0400155B RID: 5467
		private short _ownerId;

		// Token: 0x0400155C RID: 5468
		private short _attributeId;

		// Token: 0x0400155D RID: 5469
		private string _name;

		// Token: 0x0400155E RID: 5470
		private Type _ownerType;

		// Token: 0x0400155F RID: 5471
		private RoutedEvent _Event;

		// Token: 0x04001560 RID: 5472
		private DependencyProperty _dp;

		// Token: 0x04001561 RID: 5473
		private EventInfo _ei;

		// Token: 0x04001562 RID: 5474
		private PropertyInfo _pi;

		// Token: 0x04001563 RID: 5475
		private MethodInfo _smi;

		// Token: 0x04001564 RID: 5476
		private MethodInfo _gmi;

		// Token: 0x04001565 RID: 5477
		private object _dpOrMiOrPi;
	}
}
