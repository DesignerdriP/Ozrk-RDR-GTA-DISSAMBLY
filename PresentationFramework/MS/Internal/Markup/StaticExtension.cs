using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace MS.Internal.Markup
{
	// Token: 0x0200065A RID: 1626
	internal class StaticExtension : StaticExtension
	{
		// Token: 0x06006BFF RID: 27647 RVA: 0x001F1592 File Offset: 0x001EF792
		public StaticExtension()
		{
		}

		// Token: 0x06006C00 RID: 27648 RVA: 0x001F159A File Offset: 0x001EF79A
		public StaticExtension(string member) : base(member)
		{
		}

		// Token: 0x06006C01 RID: 27649 RVA: 0x001F15A4 File Offset: 0x001EF7A4
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (base.Member == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionStaticMember"));
			}
			object obj;
			if (base.MemberType != null)
			{
				obj = SystemResourceKey.GetSystemResourceKey(base.MemberType.Name + "." + base.Member);
				if (obj != null)
				{
					return obj;
				}
			}
			else
			{
				obj = SystemResourceKey.GetSystemResourceKey(base.Member);
				if (obj != null)
				{
					return obj;
				}
				int num = base.Member.IndexOf('.');
				if (num < 0)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionBadStatic", new object[]
					{
						base.Member
					}));
				}
				string text = base.Member.Substring(0, num);
				if (text == string.Empty)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionBadStatic", new object[]
					{
						base.Member
					}));
				}
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}
				IXamlTypeResolver xamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
				if (xamlTypeResolver == null)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionNoContext", new object[]
					{
						base.GetType().Name,
						"IXamlTypeResolver"
					}));
				}
				base.MemberType = xamlTypeResolver.Resolve(text);
				base.Member = base.Member.Substring(num + 1, base.Member.Length - num - 1);
			}
			obj = CommandConverter.GetKnownControlCommand(base.MemberType, base.Member);
			if (obj != null)
			{
				return obj;
			}
			return base.ProvideValue(serviceProvider);
		}
	}
}
