using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001DA RID: 474
	internal class BamlXmlnsPropertyRecord : BamlVariableSizedRecord
	{
		// Token: 0x06001EEE RID: 7918 RVA: 0x00094228 File Offset: 0x00092428
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			this.Prefix = bamlBinaryReader.ReadString();
			this.XmlNamespace = bamlBinaryReader.ReadString();
			short num = bamlBinaryReader.ReadInt16();
			if (num > 0)
			{
				this.AssemblyIds = new short[(int)num];
				for (short num2 = 0; num2 < num; num2 += 1)
				{
					this.AssemblyIds[(int)num2] = bamlBinaryReader.ReadInt16();
				}
				return;
			}
			this.AssemblyIds = null;
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x00094288 File Offset: 0x00092488
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(this.Prefix);
			bamlBinaryWriter.Write(this.XmlNamespace);
			short num = 0;
			if (this.AssemblyIds != null && this.AssemblyIds.Length != 0)
			{
				num = (short)this.AssemblyIds.Length;
			}
			bamlBinaryWriter.Write(num);
			if (num > 0)
			{
				for (short num2 = 0; num2 < num; num2 += 1)
				{
					bamlBinaryWriter.Write(this.AssemblyIds[(int)num2]);
				}
			}
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x000942F0 File Offset: 0x000924F0
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlXmlnsPropertyRecord bamlXmlnsPropertyRecord = (BamlXmlnsPropertyRecord)record;
			bamlXmlnsPropertyRecord._prefix = this._prefix;
			bamlXmlnsPropertyRecord._xmlNamespace = this._xmlNamespace;
			bamlXmlnsPropertyRecord._assemblyIds = this._assemblyIds;
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x06001EF1 RID: 7921 RVA: 0x0009432F File Offset: 0x0009252F
		internal override BamlRecordType RecordType
		{
			get
			{
				return BamlRecordType.XmlnsProperty;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x00094333 File Offset: 0x00092533
		// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x0009433B File Offset: 0x0009253B
		internal string Prefix
		{
			get
			{
				return this._prefix;
			}
			set
			{
				this._prefix = value;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x06001EF4 RID: 7924 RVA: 0x00094344 File Offset: 0x00092544
		// (set) Token: 0x06001EF5 RID: 7925 RVA: 0x0009434C File Offset: 0x0009254C
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
			set
			{
				this._xmlNamespace = value;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x06001EF6 RID: 7926 RVA: 0x00094355 File Offset: 0x00092555
		// (set) Token: 0x06001EF7 RID: 7927 RVA: 0x0009435D File Offset: 0x0009255D
		internal short[] AssemblyIds
		{
			get
			{
				return this._assemblyIds;
			}
			set
			{
				this._assemblyIds = value;
			}
		}

		// Token: 0x040014FD RID: 5373
		private string _prefix;

		// Token: 0x040014FE RID: 5374
		private string _xmlNamespace;

		// Token: 0x040014FF RID: 5375
		private short[] _assemblyIds;
	}
}
