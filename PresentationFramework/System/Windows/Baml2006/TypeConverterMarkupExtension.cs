using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Markup;

namespace System.Windows.Baml2006
{
	// Token: 0x0200016C RID: 364
	internal class TypeConverterMarkupExtension : MarkupExtension
	{
		// Token: 0x06001092 RID: 4242 RVA: 0x00041D74 File Offset: 0x0003FF74
		public TypeConverterMarkupExtension(TypeConverter converter, object value)
		{
			this._converter = converter;
			this._value = value;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00041D8A File Offset: 0x0003FF8A
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this._converter.ConvertFrom(new TypeConverterMarkupExtension.TypeConverterContext(serviceProvider), CultureInfo.InvariantCulture, this._value);
		}

		// Token: 0x04001237 RID: 4663
		private TypeConverter _converter;

		// Token: 0x04001238 RID: 4664
		private object _value;

		// Token: 0x0200084D RID: 2125
		private class TypeConverterContext : ITypeDescriptorContext, IServiceProvider
		{
			// Token: 0x06007F3A RID: 32570 RVA: 0x0024112B File Offset: 0x0023F32B
			public TypeConverterContext(IServiceProvider serviceProvider)
			{
				this._serviceProvider = serviceProvider;
			}

			// Token: 0x06007F3B RID: 32571 RVA: 0x0024113A File Offset: 0x0023F33A
			object IServiceProvider.GetService(Type serviceType)
			{
				return this._serviceProvider.GetService(serviceType);
			}

			// Token: 0x06007F3C RID: 32572 RVA: 0x00002137 File Offset: 0x00000337
			void ITypeDescriptorContext.OnComponentChanged()
			{
			}

			// Token: 0x06007F3D RID: 32573 RVA: 0x0000B02A File Offset: 0x0000922A
			bool ITypeDescriptorContext.OnComponentChanging()
			{
				return false;
			}

			// Token: 0x17001D91 RID: 7569
			// (get) Token: 0x06007F3E RID: 32574 RVA: 0x0000C238 File Offset: 0x0000A438
			IContainer ITypeDescriptorContext.Container
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001D92 RID: 7570
			// (get) Token: 0x06007F3F RID: 32575 RVA: 0x0000C238 File Offset: 0x0000A438
			object ITypeDescriptorContext.Instance
			{
				get
				{
					return null;
				}
			}

			// Token: 0x17001D93 RID: 7571
			// (get) Token: 0x06007F40 RID: 32576 RVA: 0x0000C238 File Offset: 0x0000A438
			PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor
			{
				get
				{
					return null;
				}
			}

			// Token: 0x04003CFE RID: 15614
			private IServiceProvider _serviceProvider;
		}
	}
}
