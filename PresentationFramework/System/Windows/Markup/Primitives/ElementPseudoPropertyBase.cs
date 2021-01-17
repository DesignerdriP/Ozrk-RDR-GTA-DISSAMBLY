using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x0200027D RID: 637
	internal abstract class ElementPseudoPropertyBase : ElementObjectPropertyBase
	{
		// Token: 0x0600243D RID: 9277 RVA: 0x000B0801 File Offset: 0x000AEA01
		internal ElementPseudoPropertyBase(object value, Type type, ElementMarkupObject obj) : base(obj)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x000B0818 File Offset: 0x000AEA18
		public override Type PropertyType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x0600243F RID: 9279 RVA: 0x000B0820 File Offset: 0x000AEA20
		public override object Value
		{
			get
			{
				return ElementProperty.CheckForMarkupExtension(this.PropertyType, this._value, base.Context, true);
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06002440 RID: 9280 RVA: 0x000B07B5 File Offset: 0x000AE9B5
		public override AttributeCollection Attributes
		{
			get
			{
				return AttributeCollection.Empty;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002441 RID: 9281 RVA: 0x000B083A File Offset: 0x000AEA3A
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				return new Type[0];
			}
		}

		// Token: 0x04001B2C RID: 6956
		private object _value;

		// Token: 0x04001B2D RID: 6957
		private Type _type;
	}
}
