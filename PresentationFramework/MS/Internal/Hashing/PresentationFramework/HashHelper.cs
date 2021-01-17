using System;
using System.Windows;
using System.Windows.Markup.Localizer;
using MS.Internal.Hashing.PresentationCore;

namespace MS.Internal.Hashing.PresentationFramework
{
	// Token: 0x020007F8 RID: 2040
	internal static class HashHelper
	{
		// Token: 0x06007D78 RID: 32120 RVA: 0x00233F7C File Offset: 0x0023217C
		static HashHelper()
		{
			HashHelper.Initialize();
			Type[] types = new Type[]
			{
				typeof(BamlLocalizableResource),
				typeof(ComponentResourceKey)
			};
			BaseHashHelper.RegisterTypes(typeof(HashHelper).Assembly, types);
			HashHelper.Initialize();
		}

		// Token: 0x06007D79 RID: 32121 RVA: 0x00233FC9 File Offset: 0x002321C9
		internal static bool HasReliableHashCode(object item)
		{
			return BaseHashHelper.HasReliableHashCode(item);
		}

		// Token: 0x06007D7A RID: 32122 RVA: 0x00002137 File Offset: 0x00000337
		internal static void Initialize()
		{
		}
	}
}
