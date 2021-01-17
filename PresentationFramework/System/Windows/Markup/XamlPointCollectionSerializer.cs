using System;
using System.IO;
using System.Windows.Media;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000236 RID: 566
	internal class XamlPointCollectionSerializer : XamlSerializer
	{
		// Token: 0x0600227C RID: 8828 RVA: 0x000AB5FD File Offset: 0x000A97FD
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializePoint(writer, stringValue);
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x000AB606 File Offset: 0x000A9806
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return PointCollection.DeserializeFrom(reader);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x000AB60E File Offset: 0x000A980E
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return PointCollection.DeserializeFrom(reader);
		}
	}
}
