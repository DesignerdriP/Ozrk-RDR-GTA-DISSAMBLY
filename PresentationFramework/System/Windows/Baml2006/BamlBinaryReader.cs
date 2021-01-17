using System;
using System.IO;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015E RID: 350
	internal class BamlBinaryReader : BinaryReader
	{
		// Token: 0x06000FA3 RID: 4003 RVA: 0x0003C944 File Offset: 0x0003AB44
		public BamlBinaryReader(Stream stream) : base(stream)
		{
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0003C94D File Offset: 0x0003AB4D
		public new int Read7BitEncodedInt()
		{
			return base.Read7BitEncodedInt();
		}
	}
}
