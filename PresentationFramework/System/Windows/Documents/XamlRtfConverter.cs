using System;

namespace System.Windows.Documents
{
	// Token: 0x02000434 RID: 1076
	internal class XamlRtfConverter
	{
		// Token: 0x06003F35 RID: 16181 RVA: 0x0000326D File Offset: 0x0000146D
		internal XamlRtfConverter()
		{
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x00121030 File Offset: 0x0011F230
		internal string ConvertXamlToRtf(string xamlContent)
		{
			if (xamlContent == null)
			{
				throw new ArgumentNullException("xamlContent");
			}
			string result = string.Empty;
			if (xamlContent != string.Empty)
			{
				XamlToRtfWriter xamlToRtfWriter = new XamlToRtfWriter(xamlContent);
				if (this.WpfPayload != null)
				{
					xamlToRtfWriter.WpfPayload = this.WpfPayload;
				}
				xamlToRtfWriter.Process();
				result = xamlToRtfWriter.Output;
			}
			return result;
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00121088 File Offset: 0x0011F288
		internal string ConvertRtfToXaml(string rtfContent)
		{
			if (rtfContent == null)
			{
				throw new ArgumentNullException("rtfContent");
			}
			string result = string.Empty;
			if (rtfContent != string.Empty)
			{
				RtfToXamlReader rtfToXamlReader = new RtfToXamlReader(rtfContent);
				rtfToXamlReader.ForceParagraph = this.ForceParagraph;
				if (this.WpfPayload != null)
				{
					rtfToXamlReader.WpfPayload = this.WpfPayload;
				}
				rtfToXamlReader.Process();
				result = rtfToXamlReader.Output;
			}
			return result;
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06003F38 RID: 16184 RVA: 0x001210EC File Offset: 0x0011F2EC
		// (set) Token: 0x06003F39 RID: 16185 RVA: 0x001210F4 File Offset: 0x0011F2F4
		internal bool ForceParagraph
		{
			get
			{
				return this._forceParagraph;
			}
			set
			{
				this._forceParagraph = value;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06003F3A RID: 16186 RVA: 0x001210FD File Offset: 0x0011F2FD
		// (set) Token: 0x06003F3B RID: 16187 RVA: 0x00121105 File Offset: 0x0011F305
		internal WpfPayload WpfPayload
		{
			get
			{
				return this._wpfPayload;
			}
			set
			{
				this._wpfPayload = value;
			}
		}

		// Token: 0x04002710 RID: 10000
		internal const int RtfCodePage = 1252;

		// Token: 0x04002711 RID: 10001
		private bool _forceParagraph;

		// Token: 0x04002712 RID: 10002
		private WpfPayload _wpfPayload;
	}
}
