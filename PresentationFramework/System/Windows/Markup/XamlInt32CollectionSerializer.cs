using System;
using System.IO;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000237 RID: 567
	internal class XamlInt32CollectionSerializer : XamlSerializer
	{
		// Token: 0x06002280 RID: 8832 RVA: 0x000AB618 File Offset: 0x000A9818
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			Int32Collection int32Collection = Int32Collection.Parse(stringValue);
			int num = 0;
			int count = int32Collection.Count;
			bool flag = true;
			bool flag2 = true;
			for (int i = 1; i < count; i++)
			{
				int num2 = int32Collection.Internal_GetItem(i - 1);
				int num3 = int32Collection.Internal_GetItem(i);
				if (flag && num2 + 1 != num3)
				{
					flag = false;
				}
				if (num3 < 0)
				{
					flag2 = false;
				}
				if (num3 > num)
				{
					num = num3;
				}
			}
			if (flag)
			{
				writer.Write(1);
				writer.Write(count);
				writer.Write(int32Collection.Internal_GetItem(0));
			}
			else
			{
				XamlInt32CollectionSerializer.IntegerCollectionType value;
				if (flag2 && num <= 255)
				{
					value = XamlInt32CollectionSerializer.IntegerCollectionType.Byte;
				}
				else if (flag2 && num <= 65535)
				{
					value = XamlInt32CollectionSerializer.IntegerCollectionType.UShort;
				}
				else
				{
					value = XamlInt32CollectionSerializer.IntegerCollectionType.Integer;
				}
				writer.Write((byte)value);
				writer.Write(count);
				switch (value)
				{
				case XamlInt32CollectionSerializer.IntegerCollectionType.Byte:
					for (int j = 0; j < count; j++)
					{
						writer.Write((byte)int32Collection.Internal_GetItem(j));
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.UShort:
					for (int k = 0; k < count; k++)
					{
						writer.Write((ushort)int32Collection.Internal_GetItem(k));
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.Integer:
					for (int l = 0; l < count; l++)
					{
						writer.Write(int32Collection.Internal_GetItem(l));
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000AB74E File Offset: 0x000A994E
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return XamlInt32CollectionSerializer.DeserializeFrom(reader);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000AB756 File Offset: 0x000A9956
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return XamlInt32CollectionSerializer.DeserializeFrom(reader);
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000AB760 File Offset: 0x000A9960
		private static Int32Collection DeserializeFrom(BinaryReader reader)
		{
			XamlInt32CollectionSerializer.IntegerCollectionType integerCollectionType = (XamlInt32CollectionSerializer.IntegerCollectionType)reader.ReadByte();
			int num = reader.ReadInt32();
			if (num < 0)
			{
				throw new ArgumentException(SR.Get("IntegerCollectionLengthLessThanZero"));
			}
			Int32Collection int32Collection = new Int32Collection(num);
			if (integerCollectionType == XamlInt32CollectionSerializer.IntegerCollectionType.Consecutive)
			{
				int num2 = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					int32Collection.Add(num2 + i);
				}
			}
			else
			{
				switch (integerCollectionType)
				{
				case XamlInt32CollectionSerializer.IntegerCollectionType.Byte:
					for (int j = 0; j < num; j++)
					{
						int32Collection.Add((int)reader.ReadByte());
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.UShort:
					for (int k = 0; k < num; k++)
					{
						int32Collection.Add((int)reader.ReadUInt16());
					}
					break;
				case XamlInt32CollectionSerializer.IntegerCollectionType.Integer:
					for (int l = 0; l < num; l++)
					{
						int value = reader.ReadInt32();
						int32Collection.Add(value);
					}
					break;
				default:
					throw new ArgumentException(SR.Get("UnknownIndexType"));
				}
			}
			return int32Collection;
		}

		// Token: 0x0200089A RID: 2202
		internal enum IntegerCollectionType : byte
		{
			// Token: 0x0400419D RID: 16797
			Unknown,
			// Token: 0x0400419E RID: 16798
			Consecutive,
			// Token: 0x0400419F RID: 16799
			Byte,
			// Token: 0x040041A0 RID: 16800
			UShort,
			// Token: 0x040041A1 RID: 16801
			Integer
		}
	}
}
