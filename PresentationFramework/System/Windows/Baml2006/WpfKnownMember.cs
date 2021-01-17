using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x0200016E RID: 366
	internal class WpfKnownMember : WpfXamlMember
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x0006969B File Offset: 0x0006789B
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x000696A9 File Offset: 0x000678A9
		private bool Frozen
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 1);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 1, value);
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x000696B8 File Offset: 0x000678B8
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x000696C6 File Offset: 0x000678C6
		private bool ReadOnly
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 4);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 4, value);
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x000696DB File Offset: 0x000678DB
		// (set) Token: 0x06001551 RID: 5457 RVA: 0x000696E9 File Offset: 0x000678E9
		public bool HasSpecialTypeConverter
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 2);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 2, value);
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001552 RID: 5458 RVA: 0x000696FE File Offset: 0x000678FE
		// (set) Token: 0x06001553 RID: 5459 RVA: 0x0006970C File Offset: 0x0006790C
		public bool Ambient
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 8);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 8, value);
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x00069721 File Offset: 0x00067921
		// (set) Token: 0x06001555 RID: 5461 RVA: 0x00069730 File Offset: 0x00067930
		public bool IsReadPrivate
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 16);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 16, value);
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x00069746 File Offset: 0x00067946
		// (set) Token: 0x06001557 RID: 5463 RVA: 0x00069755 File Offset: 0x00067955
		public bool IsWritePrivate
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 32);
			}
			set
			{
				this.CheckFrozen();
				WpfXamlType.SetFlag(ref this._bitField, 32, value);
			}
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0006976B File Offset: 0x0006796B
		public WpfKnownMember(XamlSchemaContext schema, XamlType declaringType, string name, DependencyProperty dProperty, bool isReadOnly, bool isAttachable) : base(dProperty, isAttachable)
		{
			base.DependencyProperty = dProperty;
			this.ReadOnly = isReadOnly;
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00069787 File Offset: 0x00067987
		public WpfKnownMember(XamlSchemaContext schema, XamlType declaringType, string name, Type type, bool isReadOnly, bool isAttachable) : base(name, declaringType, isAttachable)
		{
			this._type = type;
			this.ReadOnly = isReadOnly;
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool LookupIsUnknown()
		{
			return false;
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x000697A3 File Offset: 0x000679A3
		public void Freeze()
		{
			this.Frozen = true;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x000697AC File Offset: 0x000679AC
		private void CheckFrozen()
		{
			if (this.Frozen)
			{
				throw new InvalidOperationException("Can't Assign to Known Member attributes");
			}
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x000697C1 File Offset: 0x000679C1
		protected override XamlMemberInvoker LookupInvoker()
		{
			return new WpfKnownMemberInvoker(this);
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x0600155E RID: 5470 RVA: 0x000697C9 File Offset: 0x000679C9
		// (set) Token: 0x0600155F RID: 5471 RVA: 0x000697D1 File Offset: 0x000679D1
		public Action<object, object> SetDelegate
		{
			get
			{
				return this._setDelegate;
			}
			set
			{
				this.CheckFrozen();
				this._setDelegate = value;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x000697E0 File Offset: 0x000679E0
		// (set) Token: 0x06001561 RID: 5473 RVA: 0x000697E8 File Offset: 0x000679E8
		public Func<object, object> GetDelegate
		{
			get
			{
				return this._getDelegate;
			}
			set
			{
				this.CheckFrozen();
				this._getDelegate = value;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001562 RID: 5474 RVA: 0x000697F7 File Offset: 0x000679F7
		// (set) Token: 0x06001563 RID: 5475 RVA: 0x000697FF File Offset: 0x000679FF
		public Type TypeConverterType
		{
			get
			{
				return this._typeConverterType;
			}
			set
			{
				this.CheckFrozen();
				this._typeConverterType = value;
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00069810 File Offset: 0x00067A10
		protected override XamlValueConverter<TypeConverter> LookupTypeConverter()
		{
			WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
			if (this.HasSpecialTypeConverter)
			{
				return bamlSharedSchemaContext.GetXamlType(this._typeConverterType).TypeConverter;
			}
			if (this._typeConverterType != null)
			{
				return bamlSharedSchemaContext.GetTypeConverter(this._typeConverterType);
			}
			return null;
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001565 RID: 5477 RVA: 0x00069859 File Offset: 0x00067A59
		// (set) Token: 0x06001566 RID: 5478 RVA: 0x00069861 File Offset: 0x00067A61
		public Type DeferringLoaderType
		{
			get
			{
				return this._deferringLoader;
			}
			set
			{
				this.CheckFrozen();
				this._deferringLoader = value;
			}
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x00069870 File Offset: 0x00067A70
		protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
		{
			if (this._deferringLoader != null)
			{
				WpfSharedBamlSchemaContext bamlSharedSchemaContext = System.Windows.Markup.XamlReader.BamlSharedSchemaContext;
				return bamlSharedSchemaContext.GetDeferringLoader(this._deferringLoader);
			}
			return null;
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x0006989F File Offset: 0x00067A9F
		protected override bool LookupIsReadOnly()
		{
			return this.ReadOnly;
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x000698A7 File Offset: 0x00067AA7
		protected override XamlType LookupType()
		{
			if (base.DependencyProperty != null)
			{
				return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(base.DependencyProperty.PropertyType);
			}
			return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(this._type);
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x000698D7 File Offset: 0x00067AD7
		protected override MemberInfo LookupUnderlyingMember()
		{
			return base.LookupUnderlyingMember();
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x000698DF File Offset: 0x00067ADF
		protected override bool LookupIsAmbient()
		{
			return this.Ambient;
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x000698E7 File Offset: 0x00067AE7
		protected override bool LookupIsWritePublic()
		{
			return !this.IsWritePrivate;
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x000698F2 File Offset: 0x00067AF2
		protected override bool LookupIsReadPublic()
		{
			return !this.IsReadPrivate;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x0001B7E3 File Offset: 0x000199E3
		protected override WpfXamlMember GetAsContentProperty()
		{
			return this;
		}

		// Token: 0x0400124C RID: 4684
		private Action<object, object> _setDelegate;

		// Token: 0x0400124D RID: 4685
		private Func<object, object> _getDelegate;

		// Token: 0x0400124E RID: 4686
		private Type _deferringLoader;

		// Token: 0x0400124F RID: 4687
		private Type _typeConverterType;

		// Token: 0x04001250 RID: 4688
		private Type _type;

		// Token: 0x04001251 RID: 4689
		private byte _bitField;

		// Token: 0x0200084F RID: 2127
		[Flags]
		private enum BoolMemberBits
		{
			// Token: 0x04004054 RID: 16468
			Frozen = 1,
			// Token: 0x04004055 RID: 16469
			HasSpecialTypeConverter = 2,
			// Token: 0x04004056 RID: 16470
			ReadOnly = 4,
			// Token: 0x04004057 RID: 16471
			Ambient = 8,
			// Token: 0x04004058 RID: 16472
			ReadPrivate = 16,
			// Token: 0x04004059 RID: 16473
			WritePrivate = 32
		}
	}
}
