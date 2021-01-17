using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000279 RID: 633
	internal abstract class ElementPropertyBase : MarkupProperty
	{
		// Token: 0x06002417 RID: 9239 RVA: 0x000B0102 File Offset: 0x000AE302
		public ElementPropertyBase(XamlDesignerSerializationManager manager)
		{
			this._manager = manager;
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x000B0114 File Offset: 0x000AE314
		public override bool IsComposite
		{
			get
			{
				if (!this._isCompositeCalculated)
				{
					this._isCompositeCalculated = true;
					object value = this.Value;
					if (value == null)
					{
						this._isComposite = true;
					}
					else if (value is string && this.PropertyType.IsAssignableFrom(typeof(object)))
					{
						this._isComposite = false;
					}
					else if (value is MarkupExtension)
					{
						this._isComposite = true;
					}
					else
					{
						this._isComposite = !this.CanConvertToString(value);
					}
				}
				return this._isComposite;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x000B0194 File Offset: 0x000AE394
		public override string StringValue
		{
			get
			{
				if (this.IsComposite)
				{
					return string.Empty;
				}
				object value = this.Value;
				string text = value as string;
				if (text != null)
				{
					return text;
				}
				ValueSerializer valueSerializer = this.GetValueSerializer();
				if (valueSerializer == null)
				{
					return string.Empty;
				}
				return valueSerializer.ConvertToString(value, this.Context);
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000B01E0 File Offset: 0x000AE3E0
		public override IEnumerable<MarkupObject> Items
		{
			get
			{
				object value = this.Value;
				if (value != null)
				{
					if (this.PropertyDescriptor != null && (this.PropertyDescriptor.IsReadOnly || (!this.PropertyIsAttached(this.PropertyDescriptor) && this.PropertyType == value.GetType() && (typeof(IList).IsAssignableFrom(this.PropertyType) || typeof(IDictionary).IsAssignableFrom(this.PropertyType) || typeof(Freezable).IsAssignableFrom(this.PropertyType) || typeof(FrameworkElementFactory).IsAssignableFrom(this.PropertyType)) && this.HasNoSerializableProperties(value) && !this.IsEmpty(value))))
					{
						IDictionary dictionary = value as IDictionary;
						if (dictionary != null)
						{
							Type keyType = ElementPropertyBase.GetDictionaryKeyType(dictionary);
							DictionaryEntry[] array = new DictionaryEntry[dictionary.Count];
							dictionary.CopyTo(array, 0);
							Array.Sort<DictionaryEntry>(array, (DictionaryEntry one, DictionaryEntry two) => string.Compare(one.Key.ToString(), two.Key.ToString()));
							foreach (DictionaryEntry dictionaryEntry in array)
							{
								ElementMarkupObject elementMarkupObject = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), dictionaryEntry.Value, this.Context, false), this.Manager);
								elementMarkupObject.SetKey(new ElementKey(dictionaryEntry.Key, keyType, elementMarkupObject));
								yield return elementMarkupObject;
							}
							DictionaryEntry[] array2 = null;
							keyType = null;
						}
						else
						{
							IEnumerable enumerable = value as IEnumerable;
							if (enumerable != null)
							{
								foreach (object value2 in enumerable)
								{
									MarkupObject markupObject = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), value2, this.Context, false), this.Manager);
									yield return markupObject;
								}
								IEnumerator enumerator = null;
							}
							else if (this.PropertyType == typeof(FrameworkElementFactory) && value is FrameworkElementFactory)
							{
								MarkupObject markupObject2 = new FrameworkElementFactoryMarkupObject(value as FrameworkElementFactory, this.Manager);
								yield return markupObject2;
							}
							else
							{
								MarkupObject markupObject3 = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), value, this.Context, true), this.Manager);
								yield return markupObject3;
							}
						}
					}
					else
					{
						MarkupObject markupObject4 = new ElementMarkupObject(ElementProperty.CheckForMarkupExtension(typeof(object), value, this.Context, true), this.Manager);
						yield return markupObject4;
					}
				}
				else
				{
					MarkupObject markupObject5 = new ElementMarkupObject(new NullExtension(), this.Manager);
					yield return markupObject5;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600241B RID: 9243 RVA: 0x000B0200 File Offset: 0x000AE400
		private bool PropertyIsAttached(PropertyDescriptor descriptor)
		{
			DependencyPropertyDescriptor dependencyPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(this.PropertyDescriptor);
			return dependencyPropertyDescriptor != null && dependencyPropertyDescriptor.IsAttached;
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x000B0224 File Offset: 0x000AE424
		private bool HasNoSerializableProperties(object value)
		{
			if (value is FrameworkElementFactory)
			{
				return true;
			}
			ElementMarkupObject elementMarkupObject = new ElementMarkupObject(value, this.Manager);
			foreach (MarkupProperty markupProperty in elementMarkupObject.Properties)
			{
				if (!markupProperty.IsContent)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x000B0290 File Offset: 0x000AE490
		private bool IsEmpty(object value)
		{
			IEnumerable enumerable = value as IEnumerable;
			if (enumerable != null)
			{
				using (IEnumerator enumerator = enumerable.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x000B02F0 File Offset: 0x000AE4F0
		protected IValueSerializerContext Context
		{
			get
			{
				if (this._context == null)
				{
					this._context = new ElementPropertyBase.ElementPropertyContext(this, this.GetItemContext());
				}
				return this._context;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x000B0312 File Offset: 0x000AE512
		internal XamlDesignerSerializationManager Manager
		{
			get
			{
				return this._manager;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x000B031C File Offset: 0x000AE51C
		public override IEnumerable<Type> TypeReferences
		{
			get
			{
				ValueSerializer valueSerializer = this.GetValueSerializer();
				if (valueSerializer == null)
				{
					return ElementPropertyBase.EmptyTypes;
				}
				return valueSerializer.TypeReferences(this.Value, this.Context);
			}
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x000B034C File Offset: 0x000AE54C
		protected bool CanConvertToString(object value)
		{
			if (value == null)
			{
				return false;
			}
			ValueSerializer valueSerializer = this.GetValueSerializer();
			return valueSerializer != null && valueSerializer.CanConvertToString(value, this.Context);
		}

		// Token: 0x06002422 RID: 9250
		protected abstract IValueSerializerContext GetItemContext();

		// Token: 0x06002423 RID: 9251
		protected abstract Type GetObjectType();

		// Token: 0x06002424 RID: 9252 RVA: 0x000B0378 File Offset: 0x000AE578
		private ValueSerializer GetValueSerializer()
		{
			PropertyDescriptor propertyDescriptor = this.PropertyDescriptor;
			if (propertyDescriptor == null)
			{
				DependencyProperty dependencyProperty = this.DependencyProperty;
				if (dependencyProperty != null)
				{
					propertyDescriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, this.GetObjectType());
				}
			}
			if (propertyDescriptor != null)
			{
				return ValueSerializer.GetSerializerFor(propertyDescriptor, this.GetItemContext());
			}
			return ValueSerializer.GetSerializerFor(this.PropertyType, this.GetItemContext());
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000B03C8 File Offset: 0x000AE5C8
		private static Type GetDictionaryKeyType(IDictionary value)
		{
			Type type = value.GetType();
			if (ElementPropertyBase._keyTypeMap == null)
			{
				ElementPropertyBase._keyTypeMap = new Dictionary<Type, Type>();
			}
			Type typeFromHandle;
			if (!ElementPropertyBase._keyTypeMap.TryGetValue(type, out typeFromHandle))
			{
				foreach (Type type2 in type.GetInterfaces())
				{
					if (type2.IsGenericType)
					{
						Type genericTypeDefinition = type2.GetGenericTypeDefinition();
						if (genericTypeDefinition == typeof(IDictionary<, >))
						{
							return type2.GetGenericArguments()[0];
						}
					}
				}
				typeFromHandle = typeof(object);
				ElementPropertyBase._keyTypeMap[type] = typeFromHandle;
			}
			return typeFromHandle;
		}

		// Token: 0x04001B20 RID: 6944
		private static readonly List<Type> EmptyTypes = new List<Type>();

		// Token: 0x04001B21 RID: 6945
		private static Dictionary<Type, Type> _keyTypeMap;

		// Token: 0x04001B22 RID: 6946
		private bool _isComposite;

		// Token: 0x04001B23 RID: 6947
		private bool _isCompositeCalculated;

		// Token: 0x04001B24 RID: 6948
		private IValueSerializerContext _context;

		// Token: 0x04001B25 RID: 6949
		private XamlDesignerSerializationManager _manager;

		// Token: 0x020008A5 RID: 2213
		private sealed class ElementPropertyContext : ValueSerializerContextWrapper, IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x060083C4 RID: 33732 RVA: 0x0024643F File Offset: 0x0024463F
			public ElementPropertyContext(ElementPropertyBase property, IValueSerializerContext baseContext) : base(baseContext)
			{
				this._property = property;
			}

			// Token: 0x17001DDE RID: 7646
			// (get) Token: 0x060083C5 RID: 33733 RVA: 0x0024644F File Offset: 0x0024464F
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return this._property.PropertyDescriptor;
				}
			}

			// Token: 0x040041C4 RID: 16836
			private ElementPropertyBase _property;
		}
	}
}
