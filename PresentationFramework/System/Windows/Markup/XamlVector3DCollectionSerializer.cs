using System;
using System.IO;
using System.Windows.Media.Media3D;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000235 RID: 565
	internal class XamlVector3DCollectionSerializer : XamlSerializer
	{
		// Token: 0x06002277 RID: 8823 RVA: 0x000AB5AD File Offset: 0x000A97AD
		internal XamlVector3DCollectionSerializer()
		{
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000AB5E4 File Offset: 0x000A97E4
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializeVector3D(writer, stringValue);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000AB5ED File Offset: 0x000A97ED
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Vector3DCollection.DeserializeFrom(reader);
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000AB5F5 File Offset: 0x000A97F5
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Vector3DCollection.DeserializeFrom(reader);
		}
	}
}
