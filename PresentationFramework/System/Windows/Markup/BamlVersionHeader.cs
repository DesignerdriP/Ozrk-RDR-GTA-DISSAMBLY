using System;
using System.Globalization;
using System.IO;
using MS.Internal.IO.Packaging.CompoundFile;

namespace System.Windows.Markup
{
	// Token: 0x02000210 RID: 528
	internal class BamlVersionHeader
	{
		// Token: 0x060020D9 RID: 8409 RVA: 0x00096C16 File Offset: 0x00094E16
		public BamlVersionHeader()
		{
			this._bamlVersion = new FormatVersion("MSBAML", BamlVersionHeader.BamlWriterVersion);
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060020DA RID: 8410 RVA: 0x00096C33 File Offset: 0x00094E33
		// (set) Token: 0x060020DB RID: 8411 RVA: 0x00096C3B File Offset: 0x00094E3B
		public FormatVersion BamlVersion
		{
			get
			{
				return this._bamlVersion;
			}
			set
			{
				this._bamlVersion = value;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x060020DC RID: 8412 RVA: 0x000961BF File Offset: 0x000943BF
		public static int BinarySerializationSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00096C44 File Offset: 0x00094E44
		internal void LoadVersion(BinaryReader bamlBinaryReader)
		{
			this.BamlVersion = FormatVersion.LoadFromStream(bamlBinaryReader.BaseStream);
			if (this.BamlVersion.ReaderVersion != BamlVersionHeader.BamlWriterVersion)
			{
				throw new InvalidOperationException(SR.Get("ParserBamlVersion", new object[]
				{
					this.BamlVersion.ReaderVersion.Major.ToString(CultureInfo.CurrentCulture) + "." + this.BamlVersion.ReaderVersion.Minor.ToString(CultureInfo.CurrentCulture),
					BamlVersionHeader.BamlWriterVersion.Major.ToString(CultureInfo.CurrentCulture) + "." + BamlVersionHeader.BamlWriterVersion.Minor.ToString(CultureInfo.CurrentCulture)
				}));
			}
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00096D14 File Offset: 0x00094F14
		internal void WriteVersion(BinaryWriter bamlBinaryWriter)
		{
			this.BamlVersion.SaveToStream(bamlBinaryWriter.BaseStream);
		}

		// Token: 0x0400156D RID: 5485
		internal static readonly VersionPair BamlWriterVersion = new VersionPair(0, 96);

		// Token: 0x0400156E RID: 5486
		private FormatVersion _bamlVersion;
	}
}
