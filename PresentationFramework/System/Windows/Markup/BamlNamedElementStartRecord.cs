using System;
using System.IO;

namespace System.Windows.Markup
{
	// Token: 0x020001F9 RID: 505
	internal class BamlNamedElementStartRecord : BamlElementStartRecord
	{
		// Token: 0x06001FFC RID: 8188 RVA: 0x000959F9 File Offset: 0x00093BF9
		internal override void LoadRecordData(BinaryReader bamlBinaryReader)
		{
			base.TypeId = bamlBinaryReader.ReadInt16();
			this.RuntimeName = bamlBinaryReader.ReadString();
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x00095A13 File Offset: 0x00093C13
		internal override void WriteRecordData(BinaryWriter bamlBinaryWriter)
		{
			bamlBinaryWriter.Write(base.TypeId);
			if (this.RuntimeName != null)
			{
				bamlBinaryWriter.Write(this.RuntimeName);
			}
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x00095A38 File Offset: 0x00093C38
		internal override void Copy(BamlRecord record)
		{
			base.Copy(record);
			BamlNamedElementStartRecord bamlNamedElementStartRecord = (BamlNamedElementStartRecord)record;
			bamlNamedElementStartRecord._isTemplateChild = this._isTemplateChild;
			bamlNamedElementStartRecord._runtimeName = this._runtimeName;
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06001FFF RID: 8191 RVA: 0x00095A6B File Offset: 0x00093C6B
		// (set) Token: 0x06002000 RID: 8192 RVA: 0x00095A73 File Offset: 0x00093C73
		internal string RuntimeName
		{
			get
			{
				return this._runtimeName;
			}
			set
			{
				this._runtimeName = value;
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002001 RID: 8193 RVA: 0x00095A7C File Offset: 0x00093C7C
		// (set) Token: 0x06002002 RID: 8194 RVA: 0x00095A84 File Offset: 0x00093C84
		internal bool IsTemplateChild
		{
			get
			{
				return this._isTemplateChild;
			}
			set
			{
				this._isTemplateChild = value;
			}
		}

		// Token: 0x0400153A RID: 5434
		private bool _isTemplateChild;

		// Token: 0x0400153B RID: 5435
		private string _runtimeName;
	}
}
