using System;
using System.ComponentModel;

namespace System.Windows.Markup.Primitives
{
	// Token: 0x02000282 RID: 642
	internal class ValueSerializerContextWrapper : IValueSerializerContext, ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x06002452 RID: 9298 RVA: 0x000B08C9 File Offset: 0x000AEAC9
		public ValueSerializerContextWrapper(IValueSerializerContext baseContext)
		{
			this._baseContext = baseContext;
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000B08D8 File Offset: 0x000AEAD8
		public ValueSerializer GetValueSerializerFor(PropertyDescriptor descriptor)
		{
			if (this._baseContext != null)
			{
				return this._baseContext.GetValueSerializerFor(descriptor);
			}
			return null;
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x000B08F0 File Offset: 0x000AEAF0
		public ValueSerializer GetValueSerializerFor(Type type)
		{
			if (this._baseContext != null)
			{
				return this._baseContext.GetValueSerializerFor(type);
			}
			return null;
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06002455 RID: 9301 RVA: 0x000B0908 File Offset: 0x000AEB08
		public IContainer Container
		{
			get
			{
				if (this._baseContext != null)
				{
					return this._baseContext.Container;
				}
				return null;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06002456 RID: 9302 RVA: 0x000B091F File Offset: 0x000AEB1F
		public object Instance
		{
			get
			{
				if (this._baseContext != null)
				{
					return this._baseContext.Instance;
				}
				return null;
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x000B0936 File Offset: 0x000AEB36
		public void OnComponentChanged()
		{
			if (this._baseContext != null)
			{
				this._baseContext.OnComponentChanged();
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000B094B File Offset: 0x000AEB4B
		public bool OnComponentChanging()
		{
			return this._baseContext == null || this._baseContext.OnComponentChanging();
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x000B0962 File Offset: 0x000AEB62
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				if (this._baseContext != null)
				{
					return this._baseContext.PropertyDescriptor;
				}
				return null;
			}
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000B0979 File Offset: 0x000AEB79
		public object GetService(Type serviceType)
		{
			if (this._baseContext != null)
			{
				return this._baseContext.GetService(serviceType);
			}
			return null;
		}

		// Token: 0x04001B30 RID: 6960
		private IValueSerializerContext _baseContext;
	}
}
