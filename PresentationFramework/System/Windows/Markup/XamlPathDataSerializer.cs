using System;
using System.IO;
using MS.Internal;

namespace System.Windows.Markup
{
	// Token: 0x02000238 RID: 568
	internal class XamlPathDataSerializer : XamlSerializer
	{
		// Token: 0x06002285 RID: 8837 RVA: 0x000AB845 File Offset: 0x000A9A45
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			Parsers.PathMinilanguageToBinary(writer, stringValue);
			return true;
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000AB84F File Offset: 0x000A9A4F
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Parsers.DeserializeStreamGeometry(reader);
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000AB857 File Offset: 0x000A9A57
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Parsers.DeserializeStreamGeometry(reader);
		}
	}
}
