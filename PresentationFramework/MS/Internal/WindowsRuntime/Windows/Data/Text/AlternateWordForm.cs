using System;

namespace MS.Internal.WindowsRuntime.Windows.Data.Text
{
	// Token: 0x020007F2 RID: 2034
	internal class AlternateWordForm
	{
		// Token: 0x06007D4F RID: 32079 RVA: 0x00233608 File Offset: 0x00231808
		static AlternateWordForm()
		{
			try
			{
				AlternateWordForm.s_WinRTType = Type.GetType(AlternateWordForm.s_TypeName);
			}
			catch
			{
				AlternateWordForm.s_WinRTType = null;
			}
		}

		// Token: 0x06007D50 RID: 32080 RVA: 0x00233650 File Offset: 0x00231850
		public AlternateWordForm(object alternateWordForm)
		{
			if (AlternateWordForm.s_WinRTType == null)
			{
				throw new PlatformNotSupportedException();
			}
			if (alternateWordForm.GetType() != AlternateWordForm.s_WinRTType)
			{
				throw new ArgumentException();
			}
			this._alternateWordForm = alternateWordForm;
		}

		// Token: 0x17001D19 RID: 7449
		// (get) Token: 0x06007D51 RID: 32081 RVA: 0x0023368A File Offset: 0x0023188A
		public string AlternateText
		{
			get
			{
				if (this._alternateText == null)
				{
					this._alternateText = this._alternateWordForm.ReflectionGetProperty("AlternateText");
				}
				return this._alternateText;
			}
		}

		// Token: 0x17001D1A RID: 7450
		// (get) Token: 0x06007D52 RID: 32082 RVA: 0x002336B0 File Offset: 0x002318B0
		public AlternateNormalizationFormat NormalizationFormat
		{
			get
			{
				if (this._alternateNormalizationFormat == null)
				{
					this._alternateNormalizationFormat = new AlternateNormalizationFormat?(this._alternateWordForm.ReflectionGetProperty("NormalizationFormat"));
				}
				return this._alternateNormalizationFormat.Value;
			}
		}

		// Token: 0x17001D1B RID: 7451
		// (get) Token: 0x06007D53 RID: 32083 RVA: 0x002336E5 File Offset: 0x002318E5
		public TextSegment SourceTextSegment
		{
			get
			{
				if (this._sourceTextSegment == null)
				{
					this._sourceTextSegment = new TextSegment(this._alternateWordForm.ReflectionGetProperty("SourceTextSegment"));
				}
				return this._sourceTextSegment;
			}
		}

		// Token: 0x17001D1C RID: 7452
		// (get) Token: 0x06007D54 RID: 32084 RVA: 0x00233710 File Offset: 0x00231910
		public static Type WinRTType
		{
			get
			{
				return AlternateWordForm.s_WinRTType;
			}
		}

		// Token: 0x04003AF4 RID: 15092
		private static Type s_WinRTType = null;

		// Token: 0x04003AF5 RID: 15093
		private static string s_TypeName = "Windows.Data.Text.AlternateWordForm, Windows, ContentType=WindowsRuntime";

		// Token: 0x04003AF6 RID: 15094
		private object _alternateWordForm;

		// Token: 0x04003AF7 RID: 15095
		private TextSegment _sourceTextSegment;

		// Token: 0x04003AF8 RID: 15096
		private AlternateNormalizationFormat? _alternateNormalizationFormat;

		// Token: 0x04003AF9 RID: 15097
		private string _alternateText;
	}
}
