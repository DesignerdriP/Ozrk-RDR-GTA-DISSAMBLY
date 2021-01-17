using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000289 RID: 649
	internal class FrameworkElementFactoryContent : ElementPropertyBase
	{
		// Token: 0x0600248F RID: 9359 RVA: 0x000B10E6 File Offset: 0x000AF2E6
		internal FrameworkElementFactoryContent(FrameworkElementFactory factory, FrameworkElementFactoryMarkupObject item) : base(item.Manager)
		{
			this._item = item;
			this._factory = factory;
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06002490 RID: 9360 RVA: 0x000B1102 File Offset: 0x000AF302
		public override string Name
		{
			get
			{
				return "Content";
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsContent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x00016748 File Offset: 0x00014948
		public override bool IsComposite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x000B110C File Offset: 0x000AF30C
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				for (FrameworkElementFactory child = this._factory.FirstChild; child != null; child = child.NextSibling)
				{
					yield return new FrameworkElementFactoryMarkupObject(child, base.Manager);
				}
				yield break;
			}
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000B1129 File Offset: 0x000AF329
		protected override IValueSerializerContext GetItemContext()
		{
			return this._item.Context;
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x000B1136 File Offset: 0x000AF336
		protected override Type GetObjectType()
		{
			return this._item.ObjectType;
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x000B1143 File Offset: 0x000AF343
		public override AttributeCollection Attributes
		{
			get
			{
				return new AttributeCollection(new Attribute[0]);
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06002497 RID: 9367 RVA: 0x000B1150 File Offset: 0x000AF350
		public override Type PropertyType
		{
			get
			{
				return typeof(IEnumerable);
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06002498 RID: 9368 RVA: 0x000B115C File Offset: 0x000AF35C
		public override object Value
		{
			get
			{
				return this._factory;
			}
		}

		// Token: 0x04001B3D RID: 6973
		private FrameworkElementFactoryMarkupObject _item;

		// Token: 0x04001B3E RID: 6974
		private FrameworkElementFactory _factory;
	}
}
