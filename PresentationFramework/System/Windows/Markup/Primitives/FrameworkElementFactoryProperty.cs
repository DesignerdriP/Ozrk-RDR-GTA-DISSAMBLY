using System;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000288 RID: 648
	internal class FrameworkElementFactoryProperty : ElementPropertyBase
	{
		// Token: 0x06002485 RID: 9349 RVA: 0x000B0F62 File Offset: 0x000AF162
		public FrameworkElementFactoryProperty(PropertyValue propertyValue, FrameworkElementFactoryMarkupObject item) : base(item.Manager)
		{
			this._propertyValue = propertyValue;
			this._item = item;
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06002486 RID: 9350 RVA: 0x000B0F80 File Offset: 0x000AF180
		public override PropertyDescriptor PropertyDescriptor
		{
			get
			{
				if (!this._descriptorCalculated)
				{
					this._descriptorCalculated = true;
					if (DependencyProperty.FromName(this._propertyValue.Property.Name, this._item.ObjectType) == this._propertyValue.Property)
					{
						this._descriptor = DependencyPropertyDescriptor.FromProperty(this._propertyValue.Property, this._item.ObjectType);
					}
				}
				return this._descriptor;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06002487 RID: 9351 RVA: 0x000B0FF0 File Offset: 0x000AF1F0
		public override bool IsAttached
		{
			get
			{
				DependencyPropertyDescriptor dependencyPropertyDescriptor = this.PropertyDescriptor as DependencyPropertyDescriptor;
				return dependencyPropertyDescriptor != null && dependencyPropertyDescriptor.IsAttached;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06002488 RID: 9352 RVA: 0x000B1014 File Offset: 0x000AF214
		public override AttributeCollection Attributes
		{
			get
			{
				if (this._descriptor != null)
				{
					return this._descriptor.Attributes;
				}
				PropertyDescriptor propertyDescriptor = DependencyPropertyDescriptor.FromProperty(this._propertyValue.Property, this._item.ObjectType);
				return propertyDescriptor.Attributes;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06002489 RID: 9353 RVA: 0x000B1057 File Offset: 0x000AF257
		public override string Name
		{
			get
			{
				return this._propertyValue.Property.Name;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x0600248A RID: 9354 RVA: 0x000B1069 File Offset: 0x000AF269
		public override Type PropertyType
		{
			get
			{
				return this._propertyValue.Property.PropertyType;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x0600248B RID: 9355 RVA: 0x000B107B File Offset: 0x000AF27B
		public override DependencyProperty DependencyProperty
		{
			get
			{
				return this._propertyValue.Property;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x0600248C RID: 9356 RVA: 0x000B1088 File Offset: 0x000AF288
		public override object Value
		{
			get
			{
				PropertyValueType valueType = this._propertyValue.ValueType;
				if (valueType == PropertyValueType.Set || valueType == PropertyValueType.TemplateBinding)
				{
					return this._propertyValue.Value;
				}
				if (valueType != PropertyValueType.Resource)
				{
					return null;
				}
				return new DynamicResourceExtension(this._propertyValue.Value);
			}
		}

		// Token: 0x0600248D RID: 9357 RVA: 0x000B10CC File Offset: 0x000AF2CC
		protected override IValueSerializerContext GetItemContext()
		{
			return this._item.Context;
		}

		// Token: 0x0600248E RID: 9358 RVA: 0x000B10D9 File Offset: 0x000AF2D9
		protected override Type GetObjectType()
		{
			return this._item.ObjectType;
		}

		// Token: 0x04001B39 RID: 6969
		private PropertyValue _propertyValue;

		// Token: 0x04001B3A RID: 6970
		private FrameworkElementFactoryMarkupObject _item;

		// Token: 0x04001B3B RID: 6971
		private bool _descriptorCalculated;

		// Token: 0x04001B3C RID: 6972
		private PropertyDescriptor _descriptor;
	}
}
