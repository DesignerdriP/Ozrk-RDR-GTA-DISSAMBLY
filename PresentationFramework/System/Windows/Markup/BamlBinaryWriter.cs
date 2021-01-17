using System;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x020001C9 RID: 457
	internal class BamlBinaryWriter : BinaryWriter
	{
		// Token: 0x06001D26 RID: 7462 RVA: 0x00087E81 File Offset: 0x00086081
		public BamlBinaryWriter(Stream stream, Encoding code) : base(stream, code)
		{
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x00087E8B File Offset: 0x0008608B
		public new void Write7BitEncodedInt(int value)
		{
			base.Write7BitEncodedInt(value);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00087E94 File Offset: 0x00086094
		public static int SizeOf7bitEncodedSize(int size)
		{
			if ((size & -128) == 0)
			{
				return 1;
			}
			if ((size & -16384) == 0)
			{
				return 2;
			}
			if ((size & -2097152) == 0)
			{
				return 3;
			}
			if ((size & -268435456) == 0)
			{
				return 4;
			}
			return 5;
		}
	}
}
