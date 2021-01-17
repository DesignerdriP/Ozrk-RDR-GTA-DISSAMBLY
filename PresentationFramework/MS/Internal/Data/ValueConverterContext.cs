using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MS.Internal.Data
{
	// Token: 0x02000721 RID: 1825
	internal class ValueConverterContext : ITypeDescriptorContext, IServiceProvider, IUriContext
	{
		// Token: 0x060074F4 RID: 29940 RVA: 0x0021741F File Offset: 0x0021561F
		public virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(IUriContext))
			{
				return this;
			}
			return null;
		}

		// Token: 0x17001BCC RID: 7116
		// (get) Token: 0x060074F5 RID: 29941 RVA: 0x00217436 File Offset: 0x00215636
		// (set) Token: 0x060074F6 RID: 29942 RVA: 0x00041C10 File Offset: 0x0003FE10
		public Uri BaseUri
		{
			get
			{
				if (this._cachedBaseUri == null)
				{
					if (this._targetElement != null)
					{
						this._cachedBaseUri = BaseUriHelper.GetBaseUri(this._targetElement);
					}
					else
					{
						this._cachedBaseUri = BaseUriHelper.BaseUri;
					}
				}
				return this._cachedBaseUri;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060074F7 RID: 29943 RVA: 0x00217474 File Offset: 0x00215674
		internal void SetTargetElement(DependencyObject target)
		{
			if (target != null)
			{
				this._nestingLevel++;
			}
			else if (this._nestingLevel > 0)
			{
				this._nestingLevel--;
			}
			Invariant.Assert(this._nestingLevel <= 1, "illegal to recurse/reenter ValueConverterContext.SetTargetElement()");
			this._targetElement = target;
			this._cachedBaseUri = null;
		}

		// Token: 0x17001BCD RID: 7117
		// (get) Token: 0x060074F8 RID: 29944 RVA: 0x002174CF File Offset: 0x002156CF
		internal bool IsInUse
		{
			get
			{
				return this._nestingLevel > 0;
			}
		}

		// Token: 0x17001BCE RID: 7118
		// (get) Token: 0x060074F9 RID: 29945 RVA: 0x0000C238 File Offset: 0x0000A438
		public IContainer Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BCF RID: 7119
		// (get) Token: 0x060074FA RID: 29946 RVA: 0x0000C238 File Offset: 0x0000A438
		public object Instance
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BD0 RID: 7120
		// (get) Token: 0x060074FB RID: 29947 RVA: 0x0000C238 File Offset: 0x0000A438
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060074FC RID: 29948 RVA: 0x00002137 File Offset: 0x00000337
		public void OnComponentChanged()
		{
		}

		// Token: 0x060074FD RID: 29949 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool OnComponentChanging()
		{
			return false;
		}

		// Token: 0x0400380A RID: 14346
		private DependencyObject _targetElement;

		// Token: 0x0400380B RID: 14347
		private int _nestingLevel;

		// Token: 0x0400380C RID: 14348
		private Uri _cachedBaseUri;
	}
}
