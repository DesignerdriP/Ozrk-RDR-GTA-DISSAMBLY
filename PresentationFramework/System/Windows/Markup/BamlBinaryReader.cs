using System;
using System.IO;
using System.Text;

namespace System.Windows.Markup
{
	// Token: 0x020001C8 RID: 456
	internal class BamlBinaryReader : BinaryReader
	{
		// Token: 0x06001D24 RID: 7460 RVA: 0x00087E77 File Offset: 0x00086077
		public BamlBinaryReader(Stream stream, Encoding code) : base(stream, code)
		{
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x0003C94D File Offset: 0x0003AB4D
		public new int Read7BitEncodedInt()
		{
			return base.Read7BitEncodedInt();
		}
	}
}
