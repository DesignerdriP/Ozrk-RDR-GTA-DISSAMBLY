using System;

namespace System.Windows.Markup
{
	// Token: 0x02000267 RID: 615
	internal static class XamlSerializerUtil
	{
		// Token: 0x0600233A RID: 9018 RVA: 0x000ACD54 File Offset: 0x000AAF54
		internal static void ThrowIfNonWhiteSpaceInAddText(string s, object parent)
		{
			if (s != null)
			{
				for (int i = 0; i < s.Length; i++)
				{
					if (!char.IsWhiteSpace(s[i]))
					{
						throw new ArgumentException(SR.Get("NonWhiteSpaceInAddText", new object[]
						{
							s
						}));
					}
				}
			}
		}
	}
}
