using System;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000175 RID: 373
	internal class WpfXamlMember : XamlMember, IProvideValueTarget
	{
		// Token: 0x060015AE RID: 5550 RVA: 0x0006A44D File Offset: 0x0006864D
		public WpfXamlMember(DependencyProperty dp, bool isAttachable) : base(dp.Name, System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(dp.OwnerType), isAttachable)
		{
			this.DependencyProperty = dp;
			this._useV3Rules = true;
			this._isBamlMember = true;
			this._underlyingMemberIsKnown = false;
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0006A488 File Offset: 0x00068688
		public WpfXamlMember(RoutedEvent re, bool isAttachable) : base(re.Name, System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(re.OwnerType), isAttachable)
		{
			this.RoutedEvent = re;
			this._useV3Rules = true;
			this._isBamlMember = true;
			this._underlyingMemberIsKnown = false;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0006A4C3 File Offset: 0x000686C3
		public WpfXamlMember(DependencyProperty dp, MethodInfo getter, MethodInfo setter, XamlSchemaContext schemaContext, bool useV3Rules) : base(dp.Name, getter, setter, schemaContext)
		{
			this.DependencyProperty = dp;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0006A4EB File Offset: 0x000686EB
		public WpfXamlMember(DependencyProperty dp, PropertyInfo property, XamlSchemaContext schemaContext, bool useV3Rules) : base(property, schemaContext)
		{
			this.DependencyProperty = dp;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0006A50B File Offset: 0x0006870B
		public WpfXamlMember(RoutedEvent re, MethodInfo setter, XamlSchemaContext schemaContext, bool useV3Rules) : base(re.Name, setter, schemaContext)
		{
			this.RoutedEvent = re;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0006A531 File Offset: 0x00068731
		public WpfXamlMember(RoutedEvent re, EventInfo eventInfo, XamlSchemaContext schemaContext, bool useV3Rules) : base(eventInfo, schemaContext)
		{
			this.RoutedEvent = re;
			this._useV3Rules = useV3Rules;
			this._underlyingMemberIsKnown = true;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0006A551 File Offset: 0x00068751
		protected WpfXamlMember(string name, XamlType declaringType, bool isAttachable) : base(name, declaringType, isAttachable)
		{
			this._useV3Rules = true;
			this._isBamlMember = true;
			this._underlyingMemberIsKnown = false;
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060015B5 RID: 5557 RVA: 0x0006A571 File Offset: 0x00068771
		// (set) Token: 0x060015B6 RID: 5558 RVA: 0x0006A579 File Offset: 0x00068779
		public DependencyProperty DependencyProperty { get; set; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x0006A582 File Offset: 0x00068782
		// (set) Token: 0x060015B8 RID: 5560 RVA: 0x0006A58A File Offset: 0x0006878A
		public RoutedEvent RoutedEvent { get; set; }

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060015B9 RID: 5561 RVA: 0x0006A593 File Offset: 0x00068793
		// (set) Token: 0x060015BA RID: 5562 RVA: 0x0006A5A1 File Offset: 0x000687A1
		internal bool ApplyGetterFallback
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 8);
			}
			private set
			{
				WpfXamlType.SetFlag(ref this._bitField, 8, value);
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060015BB RID: 5563 RVA: 0x0006A5B0 File Offset: 0x000687B0
		internal WpfXamlMember AsContentProperty
		{
			get
			{
				if (this._asContentProperty == null)
				{
					this._asContentProperty = this.GetAsContentProperty();
				}
				return this._asContentProperty;
			}
		}

		// Token: 0x060015BC RID: 5564 RVA: 0x0006A5D4 File Offset: 0x000687D4
		protected virtual WpfXamlMember GetAsContentProperty()
		{
			if (this.DependencyProperty == null)
			{
				return this;
			}
			WpfXamlMember wpfXamlMember;
			if (this._underlyingMemberIsKnown)
			{
				PropertyInfo propertyInfo = base.UnderlyingMember as PropertyInfo;
				if (propertyInfo == null)
				{
					return this;
				}
				wpfXamlMember = new WpfXamlMember(this.DependencyProperty, propertyInfo, base.DeclaringType.SchemaContext, this._useV3Rules);
			}
			else
			{
				wpfXamlMember = new WpfXamlMember(this.DependencyProperty, false);
			}
			wpfXamlMember.ApplyGetterFallback = true;
			return wpfXamlMember;
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0006A644 File Offset: 0x00068844
		protected override XamlType LookupType()
		{
			if (this.DependencyProperty != null)
			{
				if (this._isBamlMember)
				{
					return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(this.DependencyProperty.PropertyType);
				}
				return System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(this.DependencyProperty.PropertyType);
			}
			else
			{
				if (this.RoutedEvent == null)
				{
					return base.LookupType();
				}
				if (this._isBamlMember)
				{
					return System.Windows.Markup.XamlReader.BamlSharedSchemaContext.GetXamlType(this.RoutedEvent.HandlerType);
				}
				return System.Windows.Markup.XamlReader.GetWpfSchemaContext().GetXamlType(this.RoutedEvent.HandlerType);
			}
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0006A6D0 File Offset: 0x000688D0
		protected override MemberInfo LookupUnderlyingMember()
		{
			MemberInfo memberInfo = base.LookupUnderlyingMember();
			if (memberInfo == null && this.BaseUnderlyingMember != null)
			{
				memberInfo = this.BaseUnderlyingMember.UnderlyingMember;
			}
			this._underlyingMemberIsKnown = true;
			return memberInfo;
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0006A710 File Offset: 0x00068910
		protected override MethodInfo LookupUnderlyingSetter()
		{
			MethodInfo methodInfo = base.LookupUnderlyingSetter();
			if (methodInfo == null && this.BaseUnderlyingMember != null)
			{
				methodInfo = this.BaseUnderlyingMember.Invoker.UnderlyingSetter;
			}
			this._underlyingMemberIsKnown = true;
			return methodInfo;
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0006A754 File Offset: 0x00068954
		protected override MethodInfo LookupUnderlyingGetter()
		{
			MethodInfo methodInfo = base.LookupUnderlyingGetter();
			if (methodInfo == null && this.BaseUnderlyingMember != null)
			{
				methodInfo = this.BaseUnderlyingMember.Invoker.UnderlyingGetter;
			}
			this._underlyingMemberIsKnown = true;
			return methodInfo;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0006A798 File Offset: 0x00068998
		protected override bool LookupIsReadOnly()
		{
			if (this.DependencyProperty != null)
			{
				return this.DependencyProperty.ReadOnly;
			}
			return base.LookupIsReadOnly();
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0006A7B4 File Offset: 0x000689B4
		protected override bool LookupIsEvent()
		{
			return this.RoutedEvent != null;
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0006A7C1 File Offset: 0x000689C1
		protected override XamlMemberInvoker LookupInvoker()
		{
			return new WpfMemberInvoker(this);
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0000B02A File Offset: 0x0000922A
		protected override bool LookupIsUnknown()
		{
			return false;
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0006A7C9 File Offset: 0x000689C9
		protected override XamlValueConverter<XamlDeferringLoader> LookupDeferringLoader()
		{
			if (this._useV3Rules)
			{
				return null;
			}
			return base.LookupDeferringLoader();
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x0006A7DB File Offset: 0x000689DB
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x0006A7E9 File Offset: 0x000689E9
		private bool _useV3Rules
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

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060015C8 RID: 5576 RVA: 0x0006A7F8 File Offset: 0x000689F8
		// (set) Token: 0x060015C9 RID: 5577 RVA: 0x0006A806 File Offset: 0x00068A06
		private bool _isBamlMember
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 2);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 2, value);
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060015CA RID: 5578 RVA: 0x0006A815 File Offset: 0x00068A15
		// (set) Token: 0x060015CB RID: 5579 RVA: 0x0006A823 File Offset: 0x00068A23
		private bool _underlyingMemberIsKnown
		{
			get
			{
				return WpfXamlType.GetFlag(ref this._bitField, 4);
			}
			set
			{
				WpfXamlType.SetFlag(ref this._bitField, 4, value);
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060015CC RID: 5580 RVA: 0x00041C10 File Offset: 0x0003FE10
		object IProvideValueTarget.TargetObject
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x0006A832 File Offset: 0x00068A32
		object IProvideValueTarget.TargetProperty
		{
			get
			{
				if (this.DependencyProperty != null)
				{
					return this.DependencyProperty;
				}
				return base.UnderlyingMember;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x0006A84C File Offset: 0x00068A4C
		private XamlMember BaseUnderlyingMember
		{
			get
			{
				if (this._baseUnderlyingMember == null)
				{
					WpfXamlType wpfXamlType = base.DeclaringType as WpfXamlType;
					this._baseUnderlyingMember = wpfXamlType.FindBaseXamlMember(base.Name, base.IsAttachable);
					if (this._baseUnderlyingMember == null)
					{
						this._baseUnderlyingMember = wpfXamlType.FindBaseXamlMember(base.Name, !base.IsAttachable);
					}
				}
				return this._baseUnderlyingMember;
			}
		}

		// Token: 0x04001275 RID: 4725
		private byte _bitField;

		// Token: 0x04001276 RID: 4726
		private XamlMember _baseUnderlyingMember;

		// Token: 0x04001277 RID: 4727
		private WpfXamlMember _asContentProperty;

		// Token: 0x02000851 RID: 2129
		[Flags]
		private enum BoolMemberBits
		{
			// Token: 0x04004060 RID: 16480
			UseV3Rules = 1,
			// Token: 0x04004061 RID: 16481
			BamlMember = 2,
			// Token: 0x04004062 RID: 16482
			UnderlyingMemberIsKnown = 4,
			// Token: 0x04004063 RID: 16483
			ApplyGetterFallback = 8
		}
	}
}
