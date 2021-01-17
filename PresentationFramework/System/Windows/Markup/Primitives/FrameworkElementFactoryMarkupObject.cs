using System;
using System.Collections.Generic;
using System.ComponentModel;
using MS.Utility;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000287 RID: 647
	internal class FrameworkElementFactoryMarkupObject : MarkupObject
	{
		// Token: 0x0600247D RID: 9341 RVA: 0x000B0EF0 File Offset: 0x000AF0F0
		internal FrameworkElementFactoryMarkupObject(FrameworkElementFactory factory, XamlDesignerSerializationManager manager)
		{
			this._factory = factory;
			this._manager = manager;
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000B0F06 File Offset: 0x000AF106
		public override void AssignRootContext(IValueSerializerContext context)
		{
			this._context = context;
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x0600247F RID: 9343 RVA: 0x000AFC31 File Offset: 0x000ADE31
		public override AttributeCollection Attributes
		{
			get
			{
				return TypeDescriptor.GetAttributes(this.ObjectType);
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x000B0F0F File Offset: 0x000AF10F
		public override Type ObjectType
		{
			get
			{
				if (this._factory.Type != null)
				{
					return this._factory.Type;
				}
				return typeof(string);
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06002481 RID: 9345 RVA: 0x000B0F3A File Offset: 0x000AF13A
		public override object Instance
		{
			get
			{
				return this._factory;
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x000B0F42 File Offset: 0x000AF142
		internal override IEnumerable<MarkupProperty> GetProperties(bool mapToConstructorArgs)
		{
			if (this._factory.Type == null)
			{
				if (this._factory.Text != null)
				{
					yield return new FrameworkElementFactoryStringContent(this._factory, this);
				}
			}
			else
			{
				FrugalStructList<PropertyValue> propertyValues = this._factory.PropertyValues;
				int num;
				for (int i = 0; i < propertyValues.Count; i = num + 1)
				{
					if (propertyValues[i].Property != XmlAttributeProperties.XmlnsDictionaryProperty)
					{
						yield return new FrameworkElementFactoryProperty(propertyValues[i], this);
					}
					num = i;
				}
				ElementMarkupObject elementMarkupObject = new ElementMarkupObject(this._factory, this.Manager);
				foreach (MarkupProperty markupProperty in elementMarkupObject.Properties)
				{
					if (markupProperty.Name == "Triggers" && markupProperty.Name == "Storyboard")
					{
						yield return markupProperty;
					}
				}
				IEnumerator<MarkupProperty> enumerator = null;
				if (this._factory.FirstChild != null)
				{
					if (this._factory.FirstChild.Type == null)
					{
						yield return new FrameworkElementFactoryStringContent(this._factory.FirstChild, this);
					}
					else
					{
						yield return new FrameworkElementFactoryContent(this._factory, this);
					}
				}
				propertyValues = default(FrugalStructList<PropertyValue>);
			}
			yield break;
			yield break;
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002483 RID: 9347 RVA: 0x000B0F52 File Offset: 0x000AF152
		internal IValueSerializerContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002484 RID: 9348 RVA: 0x000B0F5A File Offset: 0x000AF15A
		internal XamlDesignerSerializationManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x04001B36 RID: 6966
		private FrameworkElementFactory _factory;

		// Token: 0x04001B37 RID: 6967
		private IValueSerializerContext _context;

		// Token: 0x04001B38 RID: 6968
		private XamlDesignerSerializationManager _manager;
	}
}
