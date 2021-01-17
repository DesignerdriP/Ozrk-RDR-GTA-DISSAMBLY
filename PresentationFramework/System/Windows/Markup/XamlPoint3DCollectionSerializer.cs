using System;
using System.IO;
using System.Windows.Media.Media3D;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000234 RID: 564
	internal class XamlPoint3DCollectionSerializer : XamlSerializer
	{
		// Token: 0x06002274 RID: 8820 RVA: 0x000AB5CB File Offset: 0x000A97CB
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializePoint3D(writer, stringValue);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000AB5D4 File Offset: 0x000A97D4
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Point3DCollection.DeserializeFrom(reader);
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000AB5DC File Offset: 0x000A97DC
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Point3DCollection.DeserializeFrom(reader);
		}
	}
}
